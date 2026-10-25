param(
	[string]$Ip = "127.0.0.1",
	[int]$Port = 8090,
	[string]$OkbUiRoot = "../../../VAICOM/Extensions/Kneeboard/OKB",
	[string]$MockRoot = "../mock-api/okb"
)

$listener = New-Object System.Net.HttpListener
$listenHost = if ($Ip -eq "0.0.0.0" -or $Ip -eq "*" -or $Ip -eq "+") { "+" } else { $Ip }
$prefix = "http://$listenHost`:$Port/"
$listener.Prefixes.Add($prefix)

$repoRoot = [System.IO.Path]::GetFullPath((Join-Path $PSScriptRoot "../../.."))
$okbUiPath = [System.IO.Path]::GetFullPath((Join-Path $PSScriptRoot $OkbUiRoot))
$mockRootPath = [System.IO.Path]::GetFullPath((Join-Path $PSScriptRoot $MockRoot))
$logoPath = Join-Path $repoRoot "VAICOM/Resources/Images/VAICOMPRO logo icon 5 128.png"
$historyTracksPath = Join-Path $mockRootPath "historytracks.json"
$rteLuaPath = Join-Path $mockRootPath "MarianaIslands.lua"
$baseMissionSeconds = 45296
$missionStartUtc = [DateTime]::UtcNow
$routeLibrary = @()
$selectedRouteToken = ""

if (-not (Test-Path -Path $okbUiPath)) {
	Write-Error "OKB UI path not found: $okbUiPath"
	return
}

if (-not (Test-Path -Path $mockRootPath)) {
	Write-Error "Mock data path not found: $mockRootPath"
	return
}

try {
	$listener.Start()
}
catch {
	Write-Error "Failed to start HttpListener on $prefix. $($_.Exception.Message)"
	if ($listenHost -eq "+") {
		Write-Host "Try this once in an elevated PowerShell:"
		Write-Host "netsh http add urlacl url=http://+:$Port/ user=$env:USERNAME"
	}
	return
}

$displayUrl = if ($listenHost -eq "+") { "http://<pc-lan-ip>:$Port/" } else { "http://$Ip`:$Port/" }

Write-Host "Mock endpoint running on $displayUrl"
Write-Host "Listener prefix: $prefix"
Write-Host "OKB UI root: $okbUiPath"
Write-Host "Mock data root: $mockRootPath"
Write-Host "Supported routes: /okb/, /okb/state, /okb/index.json, /okb/clock, /okb/sa/ownship, /okb/fltpln/avbus, /okb/helpdoc"
Write-Host "Press Ctrl+C to stop"

function Add-CorsHeaders {
	param($response)

	$response.AddHeader("Access-Control-Allow-Origin", "*")
	$response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT, OPTIONS")
	$response.AddHeader("Access-Control-Allow-Headers", "Content-Type")
}

function Write-JsonResponse {
	param($response, [string]$json)

	$bytes = [System.Text.Encoding]::UTF8.GetBytes($json)
	$response.StatusCode = 200
	$response.ContentType = "application/json; charset=utf-8"
	Add-CorsHeaders -response $response
	$response.ContentLength64 = $bytes.Length
	if ($script:CurrentIsHeadRequest) {
		return
	}
	$response.OutputStream.Write($bytes, 0, $bytes.Length)
}

function Write-TextResponse {
	param($response, [string]$text, [string]$contentType = "text/plain; charset=utf-8", [int]$statusCode = 200)

	$bytes = [System.Text.Encoding]::UTF8.GetBytes($text)
	$response.StatusCode = $statusCode
	$response.ContentType = $contentType
	Add-CorsHeaders -response $response
	$response.ContentLength64 = $bytes.Length
	if ($script:CurrentIsHeadRequest) {
		return
	}
	$response.OutputStream.Write($bytes, 0, $bytes.Length)
}

function Resolve-MimeType {
	param([string]$path)

	$ext = [System.IO.Path]::GetExtension($path).ToLowerInvariant()
	switch ($ext) {
		".html" { return "text/html; charset=utf-8" }
		".css" { return "text/css; charset=utf-8" }
		".js" { return "application/javascript; charset=utf-8" }
		".json" { return "application/json; charset=utf-8" }
		".png" { return "image/png" }
		default { return "application/octet-stream" }
	}
}

function Write-FileResponse {
	param($response, [string]$path)

	if (-not (Test-Path -Path $path)) {
		Write-TextResponse -response $response -text "Not Found" -statusCode 404
		return
	}

	$bytes = [System.IO.File]::ReadAllBytes($path)
	$response.StatusCode = 200
	$response.ContentType = Resolve-MimeType -path $path
	Add-CorsHeaders -response $response
	$response.ContentLength64 = $bytes.Length
	if ($script:CurrentIsHeadRequest) {
		return
	}
	$response.OutputStream.Write($bytes, 0, $bytes.Length)
}

function Read-MockJson {
	param([string]$name)

	$path = Join-Path $mockRootPath $name
	if (-not (Test-Path -Path $path)) {
		return "{}"
	}

	return Get-Content -Raw -Path $path
}

function Get-MissionTimeSeconds {
	$elapsed = [DateTime]::UtcNow - $missionStartUtc
	return [math]::Round($baseMissionSeconds + $elapsed.TotalSeconds, 3)
}

function Get-LuaNumber {
	param([string]$luaBody, [string]$key, [double]$fallback)
	$mm = [regex]::Match($luaBody, '\["' + [regex]::Escape($key) + '"\]\s*=\s*(?<v>-?\d+(?:\.\d+)?)')
	if ($mm.Success) {
		try { return [double]::Parse($mm.Groups['v'].Value, [System.Globalization.CultureInfo]::InvariantCulture) } catch { }
	}
	return $fallback
}

function Get-LuaText {
	param([string]$luaBody, [string]$key, [string]$fallback)
	$mm = [regex]::Match($luaBody, '\["' + [regex]::Escape($key) + '"\]\s*=\s*"(?<v>[^"]*)"')
	if ($mm.Success) { return [string]$mm.Groups['v'].Value }
	return $fallback
}

function Parse-LuaPresetWaypoints {
	param([string]$presetBody)

	$rows = @()
	$wpMatches = [regex]::Matches($presetBody, '(?s)\[(?<idx>\d+)\]\s*=\s*\{(?<body>.*?)\}\s*,\s*--\s*end\s*of\s*\[\d+\]')
	foreach ($m in $wpMatches) {
		$wpBody = $m.Groups['body'].Value

		$idx = [int]$m.Groups['idx'].Value
		$x = Get-LuaNumber -luaBody $wpBody -key "x" -fallback 0
		$y = Get-LuaNumber -luaBody $wpBody -key "y" -fallback 0
		$altMeters = Get-LuaNumber -luaBody $wpBody -key "alt" -fallback 0
		$eta = Get-LuaNumber -luaBody $wpBody -key "ETA" -fallback 0
		$wpName = Get-LuaText -luaBody $wpBody -key "name" -fallback ""
		$wpType = Get-LuaText -luaBody $wpBody -key "type" -fallback "Turning Point"

		if ([string]::IsNullOrWhiteSpace($wpName)) {
			$wpName = "WP$idx"
		}

		$speed = Get-LuaNumber -luaBody $wpBody -key "speed" -fallback 0
		$altType = Get-LuaText -luaBody $wpBody -key "alt_type" -fallback "BARO"

		$rows += [pscustomobject]@{
			alt = [math]::Round($altMeters, 3)
			type = $wpType
			ETA = [math]::Round($eta, 3)
			y = [math]::Round($y, 3)
			x = [math]::Round($x, 3)
			name = $wpName
			action = $wpType
			alt_type = $altType
			speed = [math]::Round($speed, 3)
			speed_locked = $false
		}
	}
	return $rows
}

function Load-RouteLibrary {
	if (-not (Test-Path -Path $rteLuaPath)) {
		return @()
	}

	$content = Get-Content -Raw -Path $rteLuaPath
	$presetMatches = [regex]::Matches($content, '(?s)\["(?<name>[^"]+)"\]\s*=\s*\{(?<body>.*?)\}\s*,\s*--\s*end\s*of\s*\["[^\"]+"\]')
	$list = @()
	foreach ($pm in $presetMatches) {
		$presetName = [string]$pm.Groups['name'].Value
		$presetBody = [string]$pm.Groups['body'].Value
		$waypoints = Parse-LuaPresetWaypoints -presetBody $presetBody
		if ($waypoints.Count -eq 0) { continue }

		$encodedRouteName = [System.Uri]::EscapeDataString($presetName)
		$sourceFile = "MarianaIslands.lua"
		$token = "RTE::" + $encodedRouteName + "::" + $sourceFile
		$list += [pscustomobject]@{
			token = $token
			legacyToken = ("RTE::" + $presetName)
			name = $presetName
			sourceFile = $sourceFile
			waypoints = $waypoints
		}
	}
	return $list
}

function Get-RouteByToken {
	param([string]$token)
	if ([string]::IsNullOrWhiteSpace($token)) { return $null }

	$selection = [string]$token

	# 1) Exact token match (RTE::<encodedRoute>::<filePath>) or legacy token match (RTE::<routeName>)
	foreach ($r in $routeLibrary) {
		if ([string]::Equals([string]$r.token, $selection, [System.StringComparison]::OrdinalIgnoreCase)) {
			return $r
		}
		if ($r.PSObject.Properties.Name -contains 'legacyToken' -and [string]::Equals([string]$r.legacyToken, $selection, [System.StringComparison]::OrdinalIgnoreCase)) {
			return $r
		}
	}

	# 2) Parse route name from token and match by route name
	$prefix = "RTE::"
	if ($selection.StartsWith($prefix, [System.StringComparison]::OrdinalIgnoreCase)) {
		$routePart = ""
		$rest = $selection.Substring($prefix.Length)
		$sep = $rest.IndexOf("::")
		if ($sep -ge 0) {
			$routePart = $rest.Substring(0, $sep)
		}
		else {
			$routePart = $rest
		}

		$routeName = $routePart
		try { $routeName = [System.Uri]::UnescapeDataString($routePart) } catch { }

		foreach ($r in $routeLibrary) {
			if ([string]::Equals([string]$r.name, [string]$routeName, [System.StringComparison]::OrdinalIgnoreCase)) {
				return $r
			}
		}
	}

	return $null
}

function Build-RouteDtcJson {
	param($route)

	if ($null -eq $route) { return $null }

	$routeName = [string]$route.name
	if ([string]::IsNullOrWhiteSpace($routeName)) {
		$routeName = "Route"
	}

	$routeObj = [ordered]@{}
	$idx = 1
	foreach ($wp in @($route.waypoints)) {
		$routeObj[[string]$idx] = $wp
		$idx++
	}

	# RouteTool payload shape expected by OKB:
	# {
	#   "<Route Name>": {
	#     "1": { ...wp... },
	#     "2": { ...wp... }
	#   }
	# }
	$root = [ordered]@{}
	$root[$routeName] = $routeObj
	return $root
}

$routeLibrary = Load-RouteLibrary
Write-Host ("Loaded mock RTE routes: " + $routeLibrary.Count)
if ($routeLibrary.Count -gt 0) {
	Write-Host ("First route token: " + [string]$routeLibrary[0].token)
}

function Build-StateJson {
	param([string]$selectedTab, [string]$selectedFile)

	$raw = Read-MockJson -name "state.json"
	$obj = $raw | ConvertFrom-Json

	$tabToken = ""
	if ($null -ne $selectedTab) {
		$tabToken = [string]$selectedTab
	}

	if (-not [string]::IsNullOrWhiteSpace($tabToken)) {
		$obj.ActiveCategory = $tabToken.ToUpperInvariant()
	}

	$tokens = @()
	foreach ($r in $routeLibrary) {
		$tokens += [string]$r.token
	}
	if ($tokens.Count -gt 0) {
		$obj.DtcFiles = @($tokens)
	}

	if ($routeLibrary.Count -gt 0) {
		$obj.DtcSourceType = "MOCK_RTE"
		if ($null -ne $obj.Server) {
			$obj.Server.Theater = "MarianaIslands"
		}
	}

	$effectiveSelected = ""
	if (-not [string]::IsNullOrWhiteSpace($selectedFile)) {
		$effectiveSelected = [string]$selectedFile
	}
	elseif (-not [string]::IsNullOrWhiteSpace($script:selectedRouteToken)) {
		$effectiveSelected = [string]$script:selectedRouteToken
	}
	elseif ($obj.PSObject.Properties.Name -contains 'DtcSelectedFile' -and -not [string]::IsNullOrWhiteSpace([string]$obj.DtcSelectedFile)) {
		$effectiveSelected = [string]$obj.DtcSelectedFile
	}
	elseif ($tokens.Count -gt 0) {
		$effectiveSelected = [string]$tokens[0]
	}

	$route = Get-RouteByToken -token $effectiveSelected
	if ($null -eq $route -and $tokens.Count -gt 0) {
		$effectiveSelected = [string]$tokens[0]
		$route = Get-RouteByToken -token $effectiveSelected
	}

	if (-not [string]::IsNullOrWhiteSpace($effectiveSelected)) {
		$obj.DtcSelectedFile = $effectiveSelected
		$script:selectedRouteToken = $effectiveSelected
	}

	if ($null -ne $route) {
		$routePayload = Build-RouteDtcJson -route $route
		$obj.DtcJson = ($routePayload | ConvertTo-Json -Depth 40)
		$obj.DtcSourceType = "RTE"
		$selectedSourceFile = "MarianaIslands.lua"
		if ($route.PSObject.Properties.Name -contains 'sourceFile' -and -not [string]::IsNullOrWhiteSpace([string]$route.sourceFile)) {
			$selectedSourceFile = [string]$route.sourceFile
		}
		if ($obj.PSObject.Properties.Name -contains 'DtcSourceFile') {
			$obj.DtcSourceFile = $selectedSourceFile
		}
		else {
			$obj | Add-Member -MemberType NoteProperty -Name DtcSourceFile -Value $selectedSourceFile -Force
		}
	}

	$missionSeconds = Get-MissionTimeSeconds
	$obj.UpdatedUtc = [DateTime]::UtcNow.ToString("o")
	if ($null -ne $obj.Server) {
		$obj.Server.MissionTimeSeconds = $missionSeconds
	}

	# Normalize DtcJson to string payload because UI uses JSON.parse(String(data.DtcJson)).
	if ($obj.PSObject.Properties.Name -contains 'DtcJson' -and $null -ne $obj.DtcJson -and -not ($obj.DtcJson -is [string])) {
		$obj.DtcJson = ($obj.DtcJson | ConvertTo-Json -Depth 40)
	}

	$json = $obj | ConvertTo-Json -Depth 40
	if ([string]::IsNullOrWhiteSpace($json)) {
		return "{}"
	}
	return $json
}

function Build-ClockJson {
	$raw = Read-MockJson -name "clock.json"
	$obj = $raw | ConvertFrom-Json
	$obj.MissionTimeSeconds = Get-MissionTimeSeconds
	$obj.SourceUtc = [DateTime]::UtcNow.ToString("o")
	return $obj | ConvertTo-Json -Depth 20
}

function Build-OwnshipJson {
	$raw = Read-MockJson -name "ownship.json"
	$obj = $raw | ConvertFrom-Json
	$obj.updatedUtc = [DateTime]::UtcNow.ToString("o")
	$obj.missionTimeSeconds = Get-MissionTimeSeconds
	return $obj | ConvertTo-Json -Depth 20
}

function Build-AvBusJson {
	$raw = Read-MockJson -name "avbus.json"
	$obj = $raw | ConvertFrom-Json
	$obj.updatedUtc = [DateTime]::UtcNow.ToString("o")
	return $obj | ConvertTo-Json -Depth 20
}

while ($listener.IsListening) {
	$context = $listener.GetContext()
	$request = $context.Request
	$response = $context.Response
	$path = $request.Url.AbsolutePath.ToLowerInvariant()
	$script:CurrentIsHeadRequest = ($request.HttpMethod -eq "HEAD")

	try {
		if ($request.HttpMethod -eq "OPTIONS") {
			Add-CorsHeaders -response $response
			$response.StatusCode = 200
			continue
		}

		if ($path -eq "/" -or $path -eq "/okb") {
			$response.StatusCode = 302
			$response.RedirectLocation = "/okb/"
			continue
		}

		if ($path -eq "/okb/" -or $path -eq "/okb/index.html") {
			Write-FileResponse -response $response -path (Join-Path $okbUiPath "OKB.html")
			continue
		}

		if ($path -eq "/okb/logo.png") {
			Write-FileResponse -response $response -path $logoPath
			continue
		}

		if ($path -eq "/okb/helpdoc") {
			Write-FileResponse -response $response -path (Join-Path $okbUiPath "docs/OKBHelpDoc.html")
			continue
		}

		if ($path.StartsWith("/okb/css/")) {
			$rel = $path.Substring("/okb/".Length).Replace('/', [System.IO.Path]::DirectorySeparatorChar)
			Write-FileResponse -response $response -path (Join-Path $okbUiPath $rel)
			continue
		}

		if ($path.StartsWith("/okb/js/")) {
			$rel = $path.Substring("/okb/".Length).Replace('/', [System.IO.Path]::DirectorySeparatorChar)
			Write-FileResponse -response $response -path (Join-Path $okbUiPath $rel)
			continue
		}

		if ($path -eq "/okb/assets/okb-samthreatranges.json" -or $path -eq "/okb/okb-samthreatranges.json") {
			Write-FileResponse -response $response -path (Join-Path $okbUiPath "assets/OKB-SamThreatRanges.json")
			continue
		}

		if ($path -eq "/okb/state" -or $path -eq "/okb/index.json") {
			$selectedTab = $request.QueryString["selectedTab"]
			Write-JsonResponse -response $response -json (Build-StateJson -selectedTab $selectedTab -selectedFile "")
			continue
		}

		if ($path -eq "/okb/clock") {
			Write-JsonResponse -response $response -json (Build-ClockJson)
			continue
		}

		if ($path -eq "/okb/sa/ownship") {
			Write-JsonResponse -response $response -json (Build-OwnshipJson)
			continue
		}

		if ($path -eq "/okb/fltpln/avbus") {
			Write-JsonResponse -response $response -json (Build-AvBusJson)
			continue
		}

		if ($path -eq "/okb/dtc/list") {
			Write-JsonResponse -response $response -json (Build-StateJson -selectedTab "" -selectedFile "")
			continue
		}

		if ($path -eq "/okb/dtc/select") {
			$file = ""
			if ($null -ne $request.QueryString["file"]) {
				$file = [string]$request.QueryString["file"]
			}
			Write-JsonResponse -response $response -json (Build-StateJson -selectedTab "" -selectedFile $file)
			continue
		}

		if ($path -eq "/okb/efb/airports") {
			Write-JsonResponse -response $response -json (Read-MockJson -name "airports.json")
			continue
		}

		if ($path -eq "/okb/efb/charts") {
			Write-JsonResponse -response $response -json (Read-MockJson -name "charts.json")
			continue
		}

		if ($path -eq "/okb/efb/historytracks") {
			$method = "GET"
			if ($null -ne $request.HttpMethod -and -not [string]::IsNullOrWhiteSpace([string]$request.HttpMethod)) {
				$method = ([string]$request.HttpMethod).ToUpperInvariant()
			}
			if ($method -eq "GET") {
				Write-JsonResponse -response $response -json (Read-MockJson -name "historytracks.json")
				continue
			}

			if ($method -eq "POST" -or $method -eq "PUT") {
				$reader = New-Object System.IO.StreamReader($request.InputStream, $request.ContentEncoding)
				$body = $reader.ReadToEnd()
				if ([string]::IsNullOrWhiteSpace($body)) {
					$body = "{}"
				}
				Set-Content -Path $historyTracksPath -Value $body -Encoding UTF8
				Write-JsonResponse -response $response -json "{\"ok\":true}"
				continue
			}

			Write-TextResponse -response $response -text "{\"error\":\"method_not_allowed\"}" -contentType "application/json; charset=utf-8" -statusCode 405
			continue
		}

		if ($path -eq "/okb/dev/servermessages" -or $path -eq "/okb/dev/refresh") {
			Write-JsonResponse -response $response -json "{\"ok\":true}"
			continue
		}

		if ($path -eq "/api/dashboard" -or $path -eq "/dashboard") {
			Write-JsonResponse -response $response -json (Build-StateJson -selectedTab "" -selectedFile "")
			continue
		}

		Write-TextResponse -response $response -text "Not Found" -statusCode 404
	}
	catch {
		Write-TextResponse -response $response -text "Server Error: $($_.Exception.Message)" -statusCode 500
	}
	finally {
		$response.OutputStream.Close()
	}
}
