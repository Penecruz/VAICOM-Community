$ErrorActionPreference = 'Stop'

$root = Split-Path -Parent $PSScriptRoot
$root = Split-Path -Parent $root

$commandsPath = Join-Path $root 'RIO\Commands.cs'
$aliasesPath = Join-Path $root 'RIO\Aliases.cs'
$mapPath = Join-Path $root 'RIO\Commands Map.cs'
$seqPath = Join-Path $root 'RIO\Sequences.cs'
$outPath = Join-Path $root 'RIO\WIP\Jester-Command-Mapping-AB-vs-BU.full.md'

$commandsText = Get-Content -Raw $commandsPath
$aliasesText = Get-Content -Raw $aliasesPath
$mapText = Get-Content -Raw $mapPath
$seqText = Get-Content -Raw $seqPath

$cmdMatches = [regex]::Matches($commandsText, '\{\s*"(?<cmd>wMsg(?:J|LANTIRN|JESTER)_[^"]+)"\s*,\s*new\s+CommandInfo\s*\{\s*uniqueid\s*=\s*(?<id>\d+)')
$cmdToId = @{}
foreach ($m in $cmdMatches) {
	$cmd = $m.Groups['cmd'].Value
	if (-not $cmdToId.ContainsKey($cmd)) {
		$cmdToId[$cmd] = $m.Groups['id'].Value
	}
}

$cmdToAliases = @{}
$aliasMatches = [regex]::Matches($aliasesText, '\{\s*"(?<phrase>[^"]+)"\s*,\s*"(?<cmd>wMsg(?:J|LANTIRN|JESTER)_[^"]+)"\s*\}')
foreach ($m in $aliasMatches) {
	$cmd = $m.Groups['cmd'].Value
	$phrase = $m.Groups['phrase'].Value
	if (-not $cmdToAliases.ContainsKey($cmd)) {
		$cmdToAliases[$cmd] = New-Object System.Collections.Generic.List[string]
	}
	if (-not $cmdToAliases[$cmd].Contains($phrase)) {
		[void]$cmdToAliases[$cmd].Add($phrase)
	}
}

$cmdToMapExpr = @{}
$mapMatches = [regex]::Matches($mapText, '\{\s*"(?<cmd>wMsg(?:J|LANTIRN|JESTER)_[^"]+)"\s*,\s*new\s+List<List<DeviceAction>>\s*\{\s*(?<expr>[^}]*)\}\s*\}\s*,?')
foreach ($m in $mapMatches) {
	$cmdToMapExpr[$m.Groups['cmd'].Value] = (($m.Groups['expr'].Value -replace '\s+', ' ').Trim())
}

$seqToBody = @{}
$seqMatches = [regex]::Matches($seqText, 'public\s+static\s+List<DeviceAction>\s+(?<seq>Seq_[A-Za-z0-9_]+)\s*=\s*new\s+List<DeviceAction>\(\)\s*\{\s*(?<body>[^}]*)\}\s*;')
foreach ($m in $seqMatches) {
	$seqToBody[$m.Groups['seq'].Value] = (($m.Groups['body'].Value -replace '\s+', ' ').Trim())
}

function Get-SequenceNamesFromMapExpr {
	param([string]$expr)
	if ([string]::IsNullOrWhiteSpace($expr)) { return @() }
	$matches = [regex]::Matches($expr, 'Macro\.(Seq_[A-Za-z0-9_]+)')
	$result = @()
	foreach ($m in $matches) { $result += $m.Groups[1].Value }
	return $result
}

function Get-DerivedMenuPath {
	param([string[]]$seqNames)
	$options = New-Object System.Collections.Generic.List[string]
	foreach ($seqName in $seqNames) {
		if (-not $seqToBody.ContainsKey($seqName)) { continue }
		$body = $seqToBody[$seqName]
		$optMatches = [regex]::Matches($body, 'Atom_J_MENU_OPTION_(\d+)')
		foreach ($om in $optMatches) {
			[void]$options.Add($om.Groups[1].Value)
		}
	}
	if ($options.Count -eq 0) { return '' }
	return ($options -join '>')
}

function Get-MapType {
	param(
		[string[]]$seqNames,
		[string]$expr
	)
	if ([string]::IsNullOrWhiteSpace($expr)) { return 'Unmapped' }
	if ($seqNames.Count -eq 0) { return 'Unknown' }

	$hasMenuOptions = $false
	$hasNonMenuAtoms = $false

	foreach ($seqName in $seqNames) {
		if (-not $seqToBody.ContainsKey($seqName)) { continue }
		$body = $seqToBody[$seqName]
		if ($body -match 'Atom_J_MENU_OPTION_') { $hasMenuOptions = $true }
		if ($body -match 'Atom_(?!J_MENU_OPTION_|J_MENU_MAIN|J_MENU_OPEN|J_MENU_CLOSE)') { $hasNonMenuAtoms = $true }
	}

	if ($hasMenuOptions -and -not $hasNonMenuAtoms) { return 'Macro' }
	if ($hasNonMenuAtoms -and -not $hasMenuOptions) { return 'Direct/Atomic' }
	if ($hasNonMenuAtoms -and $hasMenuOptions) { return 'Hybrid' }
	return 'Macro/Control'
}

$orderedCommands = $cmdToId.Keys | Sort-Object

$sb = New-Object System.Text.StringBuilder
[void]$sb.AppendLine('# Jester Command Mapping (A/B vs B(U))')
[void]$sb.AppendLine()
[void]$sb.AppendLine('Auto-generated inventory from current code (RIO/Commands.cs, RIO/Aliases.cs, RIO/Commands Map.cs, RIO/Sequences.cs).')
[void]$sb.AppendLine()
[void]$sb.AppendLine('## Full Command Inventory')
[void]$sb.AppendLine()
[void]$sb.AppendLine('| Command | Unique ID | Alias Phrases | Type | Menu Path (derived) | Map Expr | A/B Mapping | B(U) Mapping | Status | Notes |')
[void]$sb.AppendLine('|---|---:|---|---|---|---|---|---|---|---|')

foreach ($cmd in $orderedCommands) {
	$id = $cmdToId[$cmd]
	$aliases = ''
	if ($cmdToAliases.ContainsKey($cmd)) {
		$aliases = ($cmdToAliases[$cmd] -join '; ')
	}
	$expr = ''
	if ($cmdToMapExpr.ContainsKey($cmd)) {
		$expr = $cmdToMapExpr[$cmd]
	}

	$seqNames = Get-SequenceNamesFromMapExpr -expr $expr
	$menuPath = Get-DerivedMenuPath -seqNames $seqNames
	$type = Get-MapType -seqNames $seqNames -expr $expr

	$aliases = $aliases -replace '\|', '/'
	$expr = $expr -replace '\|', '/'
	$menuPath = $menuPath -replace '\|', '/'

	[void]$sb.AppendLine("| ``$cmd`` | $id | $aliases | $type | $menuPath | $expr | TBD | TBD | Needs test |  |")
}

[void]$sb.AppendLine()
[void]$sb.AppendLine('## Suggested Test Workflow')
[void]$sb.AppendLine('1. Test by context block (Radar, Weapons, Utility, Navigation, etc.).')
[void]$sb.AppendLine('2. Fill A/B and B(U) observed behavior/path for each command.')
[void]$sb.AppendLine('3. For mismatches, patch RIO/Sequences.cs macros first; use direct mappings only when semantics are confirmed.')

Set-Content -Path $outPath -Value $sb.ToString() -Encoding UTF8
Write-Output ("Generated file: " + $outPath)
Write-Output ("Rows: " + $orderedCommands.Count)

