param(
	[string]$RootPath = "WSO/DevPipe/DCS.Lua.Exporter",
	[string]$OutputPath = "WSO/DevPipe/StoreClsidLookup.fromDcsLua.json",
	[switch]$IncludeMetadata
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Normalize-Clsid {
	param([string]$Clsid)

	if ([string]::IsNullOrWhiteSpace($Clsid)) { return '' }
	$value = $Clsid.Trim()
	if ($value.StartsWith('{') -and $value.EndsWith('}') -and $value.Length -gt 2) {
		$value = $value.Substring(1, $value.Length - 2)
	}
	return $value.Trim().ToUpperInvariant()
}

function Get-ShortName {
	param([string]$Display)

	if ([string]::IsNullOrWhiteSpace($Display)) { return '' }

	$clean = ($Display -replace '\s+', ' ').Trim()
	$head = ($clean -split '\s-\s', 2)[0].Trim()
	if ([string]::IsNullOrWhiteSpace($head)) { return $clean }

	return $head
}

function Get-CategoryName {
	param([int]$Level2)

	switch ($Level2) {
		4 { return 'missile' }
		5 { return 'bomb' }
		7 { return 'rocket' }
		8 { return 'rocket' }
		15 { return 'pod' }
		43 { return 'tank' }
		default { return 'other' }
	}
}

if (-not (Test-Path -LiteralPath $RootPath)) {
	throw "Root path not found: $RootPath"
}

$launcherPath = Join-Path $RootPath "_G/launcher"
if (-not (Test-Path -LiteralPath $launcherPath)) {
	throw "Launcher folder not found: $launcherPath"
}

$files = Get-ChildItem -LiteralPath $launcherPath -Filter *.lua -File -Recurse

$rxClsid = [regex]'(?m)^\s*CLSID\s*=\s*"(?<v>[^"]+)"'
$rxDisplayA = [regex]'(?m)^\s*displayName\s*=\s*"(?<v>[^"]+)"'
$rxDisplayB = [regex]'(?m)^\s*display_name\s*=\s*"(?<v>[^"]+)"'
$rxName = [regex]'(?m)^\s*name\s*=\s*"(?<v>[^"]+)"'
$rxAttribute = [regex]'attribute\s*=\s*\{(?<v>[^}]*)\}'
$rxNumbers = [regex]'-?\d+'

$allowedLevel2 = @(4, 5, 7, 8, 15, 43)
$byKey = @{}
$scanned = 0
$accepted = 0

foreach ($file in $files) {
	$scanned++
	$text = Get-Content -LiteralPath $file.FullName -Raw

	$mClsid = $rxClsid.Match($text)
	if (-not $mClsid.Success) { continue }

	$rawClsid = $mClsid.Groups['v'].Value.Trim()
	$normalized = Normalize-Clsid $rawClsid
	if ([string]::IsNullOrWhiteSpace($normalized)) { continue }

	$display = ''
	$mDisplay = $rxDisplayA.Match($text)
	if ($mDisplay.Success) {
		$display = $mDisplay.Groups['v'].Value.Trim()
	} else {
		$mDisplay = $rxDisplayB.Match($text)
		if ($mDisplay.Success) {
			$display = $mDisplay.Groups['v'].Value.Trim()
		}
	}

	$name = ''
	$mName = $rxName.Match($text)
	if ($mName.Success) {
		$name = $mName.Groups['v'].Value.Trim()
	}

	$level1 = $null
	$level2 = $null
	$attr = $rxAttribute.Match($text)
	if ($attr.Success) {
		$nums = $rxNumbers.Matches($attr.Groups['v'].Value) | ForEach-Object { [int]$_.Value }
		if ($nums.Count -ge 2) {
			$level1 = $nums[0]
			$level2 = $nums[1]
		}
	}

	$isAllowedByAttribute = ($level1 -eq 4 -and $allowedLevel2 -contains $level2)
	$fallbackText = ("$display $name $rawClsid").ToUpperInvariant()
	$isAllowedByText = ($fallbackText -match 'BOMB|GBU|JDAM|MISSILE|AIM-|AGM-|ROCKET|HYDRA|POD|ALQ|LITENING|ATFLIR|TARGETING|FUEL\s*TANK|DROP\s*TANK|\bTANK\b|\bGAL\b|300GAL|370GAL|600GAL|610\s*GAL')

	if (-not ($isAllowedByAttribute -or $isAllowedByText)) {
		continue
	}

	$accepted++

	if (-not $byKey.ContainsKey($normalized)) {
		$byKey[$normalized] = [ordered]@{
			clsidNormalized = $normalized
			preferredName = ''
			shortName = ''
			category = 'other'
			clsidSamples = New-Object System.Collections.Generic.HashSet[string]
			aliases = New-Object System.Collections.Generic.HashSet[string]
			sourceFiles = New-Object System.Collections.Generic.HashSet[string]
			level2Samples = New-Object System.Collections.Generic.HashSet[string]
		}
	}

	$entry = $byKey[$normalized]
	$candidate = if (-not [string]::IsNullOrWhiteSpace($display)) { $display } elseif (-not [string]::IsNullOrWhiteSpace($name)) { $name } else { $rawClsid }

	if ([string]::IsNullOrWhiteSpace($entry.preferredName) -or $candidate.Length -lt $entry.preferredName.Length) {
		$entry.preferredName = $candidate
		$entry.shortName = Get-ShortName $candidate
	}

	if ($level2 -ne $null) {
		[void]$entry.level2Samples.Add([string]$level2)
		if ($entry.category -eq 'other') {
			$entry.category = Get-CategoryName -Level2 $level2
		}
	}

	[void]$entry.clsidSamples.Add($rawClsid)
	if (-not [string]::IsNullOrWhiteSpace($display)) { [void]$entry.aliases.Add($display) }
	if (-not [string]::IsNullOrWhiteSpace($name)) { [void]$entry.aliases.Add($name) }

	$relative = $file.FullName.Substring((Resolve-Path -LiteralPath $RootPath).Path.Length).TrimStart('\\')
	[void]$entry.sourceFiles.Add($relative)
}

$sortedKeys = $byKey.Keys | Sort-Object
$map = [ordered]@{}
$entries = @()

foreach ($key in $sortedKeys) {
	$e = $byKey[$key]
	$map[$key] = if ([string]::IsNullOrWhiteSpace($e.shortName)) { $key } else { $e.shortName }
	$entries += [ordered]@{
		clsidNormalized = $e.clsidNormalized
		shortName = if ([string]::IsNullOrWhiteSpace($e.shortName)) { $e.clsidNormalized } else { $e.shortName }
		preferredName = if ([string]::IsNullOrWhiteSpace($e.preferredName)) { $e.clsidNormalized } else { $e.preferredName }
		category = $e.category
		clsidSamples = @($e.clsidSamples | Sort-Object)
		aliases = @($e.aliases | Sort-Object)
		level2Samples = @($e.level2Samples | Sort-Object)
		sourceFiles = @($e.sourceFiles | Sort-Object)
	}
}

$output = if ($IncludeMetadata) {
	[ordered]@{
		generatedUtc = (Get-Date).ToUniversalTime().ToString('o')
		sourceRoot = (Resolve-Path -LiteralPath $RootPath).Path
		sourceFolder = '_G/launcher'
		includeCategories = @('bomb', 'rocket', 'missile', 'pod', 'tank')
		scannedFiles = $scanned
		acceptedRows = $accepted
		uniqueEntries = $entries.Count
		map = $map
		entries = $entries
	}
} else {
	$map
}

$outDir = Split-Path -Parent $OutputPath
if (-not [string]::IsNullOrWhiteSpace($outDir) -and -not (Test-Path -LiteralPath $outDir)) {
	New-Item -ItemType Directory -Path $outDir -Force | Out-Null
}

$output | ConvertTo-Json -Depth 8 | Set-Content -LiteralPath $OutputPath -Encoding UTF8

if ($IncludeMetadata) {
	Write-Host "Generated lookup with metadata: $OutputPath ($($entries.Count) entries, scanned $scanned files)"
} else {
	Write-Host "Generated compact lookup map: $OutputPath ($($entries.Count) entries)"
}
