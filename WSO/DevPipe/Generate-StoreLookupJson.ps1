param(
	[Parameter(Mandatory = $true)]
	[string]$InputPath,

	[Parameter(Mandatory = $true)]
	[string]$OutputPath,

	[switch]$FilterNonAircraft
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
	param([string]$Name)

	if ([string]::IsNullOrWhiteSpace($Name)) { return '' }

	$clean = ($Name -replace '\s+', ' ').Trim()
	$head = ($clean -split '\s-\s', 2)[0].Trim()

	if ([string]::IsNullOrWhiteSpace($head)) { return $clean }
	return $head
}

function Is-LikelyNonAircraft {
	param(
		[string]$Name,
		[string]$Clsid,
		[string]$WeightToken
	)

	$u = ("$Name $Clsid").ToUpperInvariant()

	$hardDeny = @(
		'PATRIOT',
		'CHAPARRAL',
		'ROLAND',
		'TOMAHAWK',
		'SA-8',
		'SA-9',
		'SA-10',
		'SA-11',
		'SA-13',
		'SA-15',
		'SA-18',
		'SA-19',
		'SS-N'
	)

	foreach ($token in $hardDeny) {
		if ($u.Contains($token)) { return $true }
	}

	if ($WeightToken -eq 'None' -and ($u.Contains('M901') -or $u.Contains('MIM_') -or $u.Contains('9M3'))) {
		return $true
	}

	return $false
}

if (-not (Test-Path -LiteralPath $InputPath)) {
	throw "Input file not found: $InputPath"
}

$lines = Get-Content -LiteralPath $InputPath
$rx = [regex]'\"clsid\"\s*:\s*\"(?<clsid>[^\"]+)\"\s*,\s*\"name\"\s*:\s*\"(?<name>[^\"]+)\"\s*,\s*\"weight\"\s*:\s*(?<weight>[^}\r\n]+)'

$parsed = 0
$byKey = @{}

foreach ($line in $lines) {
	$m = $rx.Match($line)
	if (-not $m.Success) { continue }

	$rawClsid = $m.Groups['clsid'].Value.Trim()
	$name = $m.Groups['name'].Value.Trim()
	$weightToken = $m.Groups['weight'].Value.Trim().Trim(',')

	$normalized = Normalize-Clsid $rawClsid
	if ([string]::IsNullOrWhiteSpace($normalized)) { continue }

	if ($FilterNonAircraft -and (Is-LikelyNonAircraft -Name $name -Clsid $rawClsid -WeightToken $weightToken)) {
		continue
	}

	$parsed++

	if (-not $byKey.ContainsKey($normalized)) {
		$byKey[$normalized] = [ordered]@{
			clsidNormalized = $normalized
			preferredName = $name
			shortName = Get-ShortName $name
			clsidSamples = New-Object System.Collections.Generic.HashSet[string]
			aliases = New-Object System.Collections.Generic.HashSet[string]
			weightSamples = New-Object System.Collections.Generic.HashSet[string]
		}
	}

	$entry = $byKey[$normalized]
	[void]$entry.clsidSamples.Add($rawClsid)
	[void]$entry.aliases.Add($name)
	[void]$entry.weightSamples.Add($weightToken)

	if ($name.Length -lt $entry.preferredName.Length) {
		$entry.preferredName = $name
		$entry.shortName = Get-ShortName $name
	}
}

$sortedKeys = $byKey.Keys | Sort-Object

$map = [ordered]@{}
$entries = @()

foreach ($key in $sortedKeys) {
	$e = $byKey[$key]
	$map[$key] = $e.shortName

	$entries += [ordered]@{
		clsidNormalized = $e.clsidNormalized
		shortName = $e.shortName
		preferredName = $e.preferredName
		clsidSamples = @($e.clsidSamples | Sort-Object)
		aliases = @($e.aliases | Sort-Object)
		weightSamples = @($e.weightSamples | Sort-Object)
	}
}

$output = [ordered]@{
	generatedUtc = (Get-Date).ToUniversalTime().ToString('o')
	sourceFile = (Resolve-Path -LiteralPath $InputPath).Path
	filterNonAircraft = [bool]$FilterNonAircraft
	uniqueEntries = $entries.Count
	parsedRows = $parsed
	map = $map
	entries = $entries
}

$dir = Split-Path -Parent $OutputPath
if (-not [string]::IsNullOrWhiteSpace($dir) -and -not (Test-Path -LiteralPath $dir)) {
	New-Item -ItemType Directory -Path $dir -Force | Out-Null
}

$output | ConvertTo-Json -Depth 8 | Set-Content -LiteralPath $OutputPath -Encoding UTF8
Write-Host "Generated lookup: $OutputPath ($($entries.Count) entries)"