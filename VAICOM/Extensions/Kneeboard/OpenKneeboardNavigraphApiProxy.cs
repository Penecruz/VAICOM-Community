using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;


namespace VAICOM
{
    namespace Extensions
    {
        namespace Kneeboard
        {
            public static class OpenKneeboardNavigraphApiProxy
            {
                // Minimal stubbed proxy: for baseline testing we return local charts JSON via BuildEfbChartsJson fallback.
                // Real Navigraph API calls will be implemented later; these helpers centralize the bridge behavior.
                private static readonly ConcurrentDictionary<string, string> ChartImageDayUrlById = new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                private static readonly ConcurrentDictionary<string, string> ChartImageNightUrlById = new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                private static readonly ConcurrentDictionary<string, string> AirportNameByIcao = new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                private static readonly object IcaoOverridesSync = new object();
                private static readonly object TokenRefreshSync = new object();
                private static readonly object TileRequestModeSync = new object();
                private static readonly ConcurrentDictionary<string, object> TileFetchLocks = new ConcurrentDictionary<string, object>(StringComparer.Ordinal);
                private static DateTime LastPreflightRefreshUtc = DateTime.MinValue;
                private static bool? PreferTileBearerAuth = null;
                private static bool? PreferTileTmsY = null;
                private static Dictionary<string, HashSet<string>> IcaoAirportsByTheatre;
                private static Dictionary<string, Dictionary<string, string>> IcaoAirportNamesByTheatre;

                static OpenKneeboardNavigraphApiProxy()
                {
                    try
                    {
                        ServicePointManager.DefaultConnectionLimit = Math.Max(ServicePointManager.DefaultConnectionLimit, 32);
                    }
                    catch
                    {
                    }
                }

                public static string BuildChartsJsonFallback(string airport)
                {
                    try
                    {
                        string normalizedAirport = (airport ?? "").Trim().ToUpperInvariant();
                        if (string.IsNullOrWhiteSpace(normalizedAirport)) return null;

                        string authPayload;
                        if (!OpenKneeboardNavigraphEfbState.TryGetDecryptedAuthBlob(out authPayload) || string.IsNullOrWhiteSpace(authPayload))
                        {
                            return null;
                        }

                        JObject payload = JObject.Parse(authPayload);
                        string accessToken = (string)payload["access_token"] ?? "";
                        if (string.IsNullOrWhiteSpace(accessToken)) return null;

                        if (!TryEnsureFreshAccessToken())
                        {
                            return null;
                        }

                        string refreshedAuthPayload;
                        if (!OpenKneeboardNavigraphEfbState.TryGetDecryptedAuthBlob(out refreshedAuthPayload) || string.IsNullOrWhiteSpace(refreshedAuthPayload))
                        {
                            return null;
                        }

                        JObject refreshedPayload = JObject.Parse(refreshedAuthPayload);
                        accessToken = (string)refreshedPayload["access_token"] ?? accessToken;

                        using (var client = new HttpClient())
                        {
                            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                            string apiBase = null;
                            try { apiBase = VAICOM.State.activeconfig.OpenKneeboard_EfbApiBase; } catch { apiBase = null; }
                            if (string.IsNullOrWhiteSpace(apiBase)) apiBase = "https://api.navigraph.com/v1";

                            string txt = null;
                            string[] catalogUrls = new[]
                            {
                                "https://api.navigraph.com/v2/charts/" + Uri.EscapeDataString(normalizedAirport) + "?version=STD&rules=IFR",
                                apiBase.TrimEnd('/') + "/charts?airport=" + Uri.EscapeDataString(airport ?? ""),
                                "https://charts.api.navigraph.com/2/airports/" + Uri.EscapeDataString(normalizedAirport) + "/signedurls/charts.json",
                            };

                            string resolvedCatalogUrl = null;
                            foreach (var url in catalogUrls)
                            {
                                // try { Log.Write("Navigraph charts request URL: " + url, VAICOM.Static.Colors.Debug); } catch { }
                                var resp = client.GetAsync(url).GetAwaiter().GetResult();
                                if (!resp.IsSuccessStatusCode)
                                {
                                    try { Log.Write("Navigraph charts request failed: " + ((int)resp.StatusCode) + " " + resp.ReasonPhrase, VAICOM.Static.Colors.Warning); } catch { }
                                    continue;
                                }

                                txt = resp.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                                // try { Log.Write("Navigraph charts response body preview: " + BuildLogPreview(txt), VAICOM.Static.Colors.Debug); } catch { }
                                if (string.IsNullOrWhiteSpace(txt))
                                {
                                    continue;
                                }

                                bool shouldTrySignedUrl = false;
                                string signedUrl = null;
                                try
                                {
                                    JObject firstObj = JObject.Parse(txt);
                                    shouldTrySignedUrl = url.IndexOf("signedurls/charts.json", StringComparison.OrdinalIgnoreCase) >= 0;
                                    if (shouldTrySignedUrl)
                                    {
                                        signedUrl =
                                            (string)firstObj["url"] ??
                                            (string)firstObj["href"] ??
                                            (string)firstObj["data"] ??
                                            (string)firstObj["signedUrl"];
                                    }
                                }
                                catch
                                {
                                    shouldTrySignedUrl = url.IndexOf("signedurls/charts.json", StringComparison.OrdinalIgnoreCase) >= 0;
                                    if (shouldTrySignedUrl)
                                    {
                                        string maybeUrl = (txt ?? "").Trim().Trim('"');
                                        if (maybeUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || maybeUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                                        {
                                            signedUrl = maybeUrl;
                                        }
                                    }
                                }

                                if (shouldTrySignedUrl && !string.IsNullOrWhiteSpace(signedUrl))
                                {
                                    // try { Log.Write("Navigraph charts signed URL request: " + signedUrl, VAICOM.Static.Colors.Debug); } catch { }
                                    var signedResp = client.GetAsync(signedUrl).GetAwaiter().GetResult();
                                    if (!signedResp.IsSuccessStatusCode)
                                    {
                                        try { Log.Write("Navigraph charts signed URL request failed: " + ((int)signedResp.StatusCode) + " " + signedResp.ReasonPhrase, VAICOM.Static.Colors.Warning); } catch { }
                                        continue;
                                    }

                                    txt = signedResp.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                                    // try { Log.Write("Navigraph charts signed URL body preview: " + BuildLogPreview(txt), VAICOM.Static.Colors.Debug); } catch { }
                                    resolvedCatalogUrl = signedUrl;
                                }
                                else
                                {
                                    resolvedCatalogUrl = url;
                                }

                                if (!string.IsNullOrWhiteSpace(txt))
                                {
                                    break;
                                }
                            }

                            if (string.IsNullOrWhiteSpace(txt))
                            {
                                return null;
                            }

                            try
                            {
                                var parsedToken = JToken.Parse(txt);
                                JObject parsed = parsedToken as JObject;
                                JArray items = null;
                                if (parsedToken is JArray)
                                {
                                    items = (JArray)parsedToken;
                                }
                                else if (parsed != null && parsed["charts"] is JArray)
                                {
                                    items = (JArray)parsed["charts"];
                                }
                                else if (parsed != null && parsed["items"] is JArray)
                                {
                                    items = (JArray)parsed["items"];
                                }
                                else if (parsed != null && parsed["data"] is JArray)
                                {
                                    items = (JArray)parsed["data"];
                                }

                                var outObj = new JObject();
                                var outArr = new JArray();

                                if (items != null)
                                {
                                    foreach (var it in items)
                                    {
                                        try
                                        {
                                            string chartId = (string)(it["id"] ?? it["chartId"] ?? it["chart_id"] ?? it["uid"] ?? "");
                                            string title = (string)(it["title"] ?? it["name"] ?? it["label"] ?? it["procedure_identifier"] ?? chartId ?? "Untitled");
                                            string dayFile = (string)(it["image_day"] ?? it["file_day"] ?? "");
                                            string dayUrl = (string)(it["image_day_url"] ?? "");
                                            string nightFile = (string)(it["image_night"] ?? it["file_night"] ?? "");
                                            string nightUrl = (string)(it["image_night_url"] ?? "");

                                            string resolvedDayUrl = ResolveChartImageUrl(dayUrl, dayFile, normalizedAirport, resolvedCatalogUrl);
                                            string resolvedNightUrl = ResolveChartImageUrl(nightUrl, nightFile, normalizedAirport, resolvedCatalogUrl);

                                            if (!string.IsNullOrWhiteSpace(chartId) && !string.IsNullOrWhiteSpace(resolvedDayUrl))
                                            {
                                                ChartImageDayUrlById[chartId] = resolvedDayUrl;
                                            }

                                            if (!string.IsNullOrWhiteSpace(chartId) && !string.IsNullOrWhiteSpace(resolvedNightUrl))
                                            {
                                                ChartImageNightUrlById[chartId] = resolvedNightUrl;
                                            }

                                            var entry = new JObject();
                                            entry["id"] = "nav:" + chartId;
                                            entry["name"] = title;
                                            entry["title"] = title;
                                            entry["provider"] = "navigraph";
                                            entry["raw"] = it;
                                            outArr.Add(entry);
                                        }
                                        catch
                                        {
                                        }
                                    }
                                }

                                outObj["charts"] = outArr;
                                return outObj.ToString(Newtonsoft.Json.Formatting.None);
                            }
                            catch
                            {
                                return txt;
                            }
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }

                public static Dictionary<string, string> GetAirportNamesByIcao(IEnumerable<string> airportIcaos)
                {
                    var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                    try
                    {
                        if (airportIcaos == null)
                        {
                            return result;
                        }

                        var normalized = airportIcaos
                            .Where(v => !string.IsNullOrWhiteSpace(v))
                            .Select(v => v.Trim().ToUpperInvariant())
                            .Where(v => v.Length == 4)
                            .Distinct(StringComparer.OrdinalIgnoreCase)
                            .ToList();

                        if (normalized.Count == 0)
                        {
                            return result;
                        }

                        foreach (var icao in normalized)
                        {
                            string cachedName;
                            if (AirportNameByIcao.TryGetValue(icao, out cachedName) && !string.IsNullOrWhiteSpace(cachedName))
                            {
                                result[icao] = cachedName;
                            }
                        }

                        var missing = normalized.Where(v => !result.ContainsKey(v)).ToList();
                        if (missing.Count == 0)
                        {
                            return result;
                        }

                        if (!TryEnsureFreshAccessToken())
                        {
                            return result;
                        }

                        string authPayload;
                        if (!OpenKneeboardNavigraphEfbState.TryGetDecryptedAuthBlob(out authPayload) || string.IsNullOrWhiteSpace(authPayload))
                        {
                            return result;
                        }

                        JObject payload = JObject.Parse(authPayload);
                        string accessToken = (string)payload["access_token"] ?? "";
                        if (string.IsNullOrWhiteSpace(accessToken))
                        {
                            return result;
                        }

                        using (var client = new HttpClient())
                        {
                            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

                            int attempts = 0;
                            foreach (var icao in missing)
                            {
                                if (attempts >= 10)
                                {
                                    break;
                                }

                                attempts++;

                                string name = TryResolveAirportName(client, icao);
                                if (string.IsNullOrWhiteSpace(name))
                                {
                                    continue;
                                }

                                AirportNameByIcao[icao] = name;
                                result[icao] = name;
                            }
                        }
                    }
                    catch
                    {
                    }

                    return result;
                }

                private static bool TryEnsureFreshAccessToken()
                {
                    try
                    {
                        bool shouldThrottleRefresh = false;
                        lock (TokenRefreshSync)
                        {
                            shouldThrottleRefresh = (DateTime.UtcNow - LastPreflightRefreshUtc).TotalSeconds < 180;
                        }

                        if (shouldThrottleRefresh)
                        {
                            return true;
                        }

                        string authPayload;
                        if (!OpenKneeboardNavigraphEfbState.TryGetDecryptedAuthBlob(out authPayload) || string.IsNullOrWhiteSpace(authPayload))
                        {
                            return false;
                        }

                        JObject token = JObject.Parse(authPayload);
                        if (HasUsableAccessToken(token))
                        {
                            return true;
                        }

                        string msg;
                        bool refreshed = OpenKneeboardNavigraphOAuthService.TryRefreshAccessToken(out msg);
                        try { Log.Write(refreshed ? "Navigraph token refresh: received." : ("Navigraph token refresh failed: " + msg), refreshed ? VAICOM.Static.Colors.Text : VAICOM.Static.Colors.Warning); } catch { }
                        if (refreshed)
                        {
                            lock (TokenRefreshSync)
                            {
                                LastPreflightRefreshUtc = DateTime.UtcNow;
                            }
                        }
                        return refreshed;
                    }
                    catch
                    {
                        return false;
                    }
                }

                private static bool HasUsableAccessToken(JObject token)
                {
                    try
                    {
                        if (token == null)
                        {
                            return false;
                        }

                        string accessToken = (string)token["access_token"] ?? "";
                        if (string.IsNullOrWhiteSpace(accessToken))
                        {
                            return false;
                        }

                        int expiresIn = (int?)token["expires_in"] ?? 0;
                        DateTime obtainedUtc;
                        if (expiresIn > 0
                            && DateTime.TryParse((string)token["obtained_utc"], out obtainedUtc)
                            && DateTime.UtcNow < obtainedUtc.ToUniversalTime().AddSeconds(Math.Max(30, expiresIn - 60)))
                        {
                            return true;
                        }

                        long expUnix;
                        if (TryReadJwtExpUnix(accessToken, out expUnix))
                        {
                            DateTime expUtc = DateTimeOffset.FromUnixTimeSeconds(expUnix).UtcDateTime;
                            if (DateTime.UtcNow < expUtc.AddSeconds(-60))
                            {
                                return true;
                            }
                        }
                    }
                    catch
                    {
                    }

                    return false;
                }

                private static bool TryReadJwtExpUnix(string jwt, out long expUnix)
                {
                    expUnix = 0;
                    try
                    {
                        if (string.IsNullOrWhiteSpace(jwt))
                        {
                            return false;
                        }

                        var parts = jwt.Split('.');
                        if (parts.Length < 2)
                        {
                            return false;
                        }

                        string payload = parts[1]
                            .Replace('-', '+')
                            .Replace('_', '/');
                        switch (payload.Length % 4)
                        {
                            case 2: payload += "=="; break;
                            case 3: payload += "="; break;
                        }

                        string json = Encoding.UTF8.GetString(Convert.FromBase64String(payload));
                        JObject obj = JObject.Parse(json);
                        expUnix = (long?)obj["exp"] ?? 0;
                        return expUnix > 0;
                    }
                    catch
                    {
                        return false;
                    }
                }

                public static List<string> GetTheatreAirportCandidates(string theatre)
                {
                    string key = NormalizeTheatreKey(theatre);
                    if (string.IsNullOrWhiteSpace(key))
                    {
                        return new List<string>();
                    }

                    EnsureIcaoOverridesLoaded();

                    lock (IcaoOverridesSync)
                    {
                        if (IcaoAirportsByTheatre == null)
                        {
                            return new List<string>();
                        }

                        HashSet<string> scoped;
                        if (!IcaoAirportsByTheatre.TryGetValue(key, out scoped) || scoped == null)
                        {
                            return new List<string>();
                        }

                        return scoped
                            .Where(v => !string.IsNullOrWhiteSpace(v))
                            .Select(v => v.Trim().ToUpperInvariant())
                            .Where(v => v.Length == 4)
                            .Distinct(StringComparer.OrdinalIgnoreCase)
                            .OrderBy(v => v, StringComparer.OrdinalIgnoreCase)
                            .ToList();
                    }
                }

                public static Dictionary<string, string> GetTheatreAirportDisplayNames(string theatre)
                {
                    string key = NormalizeTheatreKey(theatre);
                    if (string.IsNullOrWhiteSpace(key))
                    {
                        return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                    }

                    EnsureIcaoOverridesLoaded();

                    lock (IcaoOverridesSync)
                    {
                        if (IcaoAirportNamesByTheatre == null)
                        {
                            return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                        }

                        Dictionary<string, string> scoped;
                        if (!IcaoAirportNamesByTheatre.TryGetValue(key, out scoped) || scoped == null)
                        {
                            return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                        }

                        return new Dictionary<string, string>(scoped, StringComparer.OrdinalIgnoreCase);
                    }
                }

                public static Dictionary<string, string> GetAirportDisplayNamesFromOverrides(IEnumerable<string> airportIcaos)
                {
                    var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                    try
                    {
                        if (airportIcaos == null)
                        {
                            return result;
                        }

                        EnsureIcaoOverridesLoaded();

                        var requested = airportIcaos
                            .Where(v => !string.IsNullOrWhiteSpace(v))
                            .Select(v => v.Trim().ToUpperInvariant())
                            .Where(v => v.Length == 4)
                            .Distinct(StringComparer.OrdinalIgnoreCase)
                            .ToList();

                        if (requested.Count == 0)
                        {
                            return result;
                        }

                        lock (IcaoOverridesSync)
                        {
                            if (IcaoAirportNamesByTheatre == null)
                            {
                                return result;
                            }

                            foreach (var icao in requested)
                            {
                                foreach (var section in IcaoAirportNamesByTheatre.Values)
                                {
                                    if (section == null)
                                    {
                                        continue;
                                    }

                                    string name;
                                    if (section.TryGetValue(icao, out name) && !string.IsNullOrWhiteSpace(name))
                                    {
                                        result[icao] = name;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                    }

                    return result;
                }

                private static void EnsureIcaoOverridesLoaded()
                {
                    if (IcaoAirportsByTheatre != null)
                    {
                        return;
                    }

                    lock (IcaoOverridesSync)
                    {
                        if (IcaoAirportsByTheatre != null)
                        {
                            return;
                        }

                        var result = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
                        var nameResult = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);

                        try
                        {
                            string content = VAICOM.Properties.Resources.ICAOOverrides_lua ?? "";
                            string[] lines = content.Replace("\r", "").Split('\n');
                            string section = "";

                            foreach (string rawLine in lines)
                            {
                                string line = rawLine ?? "";

                                Match sectionOpen = Regex.Match(line, @"^\s*([A-Za-z0-9_]+)\s*=\s*\{\s*$");
                                if (sectionOpen.Success)
                                {
                                    section = NormalizeTheatreKey(sectionOpen.Groups[1].Value);
                                    if (!result.ContainsKey(section))
                                    {
                                        result[section] = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                                    }
                                    if (!nameResult.ContainsKey(section))
                                    {
                                        nameResult[section] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                                    }
                                    continue;
                                }

                                if (!string.IsNullOrWhiteSpace(section) && Regex.IsMatch(line, @"^\s*\},?\s*$"))
                                {
                                    section = "";
                                    continue;
                                }

                                if (string.IsNullOrWhiteSpace(section))
                                {
                                    continue;
                                }

                                Match icaoDirect = Regex.Match(line, "\\[[\\s]*\"([^\"]+)\"[\\s]*\\][\\s]*=[\\s]*\"([A-Za-z]{4})\"");
                                if (icaoDirect.Success)
                                {
                                    string name = (icaoDirect.Groups[1].Value ?? "").Trim();
                                    string icao = (icaoDirect.Groups[2].Value ?? "").ToUpperInvariant();
                                    result[section].Add(icao);
                                    if (!string.IsNullOrWhiteSpace(name) && !nameResult[section].ContainsKey(icao))
                                    {
                                        nameResult[section][icao] = name;
                                    }
                                    continue;
                                }

                                Match icaoObject = Regex.Match(line, "\\[[\\s]*\"([^\"]+)\"[\\s]*\\][\\s]*=[\\s]*\\{[^\\}]*icao[\\s]*=[\\s]*\"([A-Za-z]{4})\"");
                                if (icaoObject.Success)
                                {
                                    string name = (icaoObject.Groups[1].Value ?? "").Trim();
                                    string icao = (icaoObject.Groups[2].Value ?? "").ToUpperInvariant();
                                    result[section].Add(icao);
                                    if (!string.IsNullOrWhiteSpace(name) && !nameResult[section].ContainsKey(icao))
                                    {
                                        nameResult[section][icao] = name;
                                    }
                                }
                            }
                        }
                        catch
                        {
                        }

                        IcaoAirportsByTheatre = result;
                        IcaoAirportNamesByTheatre = nameResult;
                    }
                }

                private static string NormalizeTheatreKey(string theatre)
                {
                    string key = Regex.Replace((theatre ?? "").Trim().ToLowerInvariant(), "[^a-z0-9]", "");
                    if (key == "marianaislands") return "marianas";
                    if (key == "sinaimap") return "sinai";
                    return key;
                }

                public static byte[] GetChartTileBytes(string chartId, int z, int x, int y)
                {
                    try
                    {
                        string cacheKey = "nav_tile_" + chartId + "_" + z + "_" + x + "_" + y;
                        byte[] cached = TryReadTileCache(cacheKey);
                        if (cached != null) return cached;

                        string authPayload;
                        if (!OpenKneeboardNavigraphEfbState.TryGetDecryptedAuthBlob(out authPayload) || string.IsNullOrWhiteSpace(authPayload))
                        {
                            return null;
                        }

                        JObject payload = JObject.Parse(authPayload);
                        string accessToken = (string)payload["access_token"] ?? "";
                        if (string.IsNullOrWhiteSpace(accessToken)) return null;

                        using (var client = new HttpClient())
                        {
                            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                            string apiBase = null;
                            try { apiBase = VAICOM.State.activeconfig.OpenKneeboard_EfbApiBase; } catch { apiBase = null; }
                            if (string.IsNullOrWhiteSpace(apiBase)) apiBase = "https://api.navigraph.com/v1";
                            var url = apiBase.TrimEnd('/') + $"/tiles/{Uri.EscapeDataString(chartId)}/{z}/{x}/{y}";
                            var resp = client.GetAsync(url).GetAwaiter().GetResult();
                            if (!resp.IsSuccessStatusCode)
                            {
                                return null;
                            }
                            var bytes = resp.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();
                            TryWriteTileCache(cacheKey, bytes);
                            return bytes;
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }

                public static byte[] GetChartImageBytes(string chartId)
                {
                    return GetChartImageBytes(chartId, false);
                }

                public static byte[] GetChartImageBytes(string chartId, bool useNight)
                {
                    try
                    {
                        string cacheKey = "nav_chartimg_" + chartId + "_" + (useNight ? "n" : "d");
                        byte[] cached = TryReadTileCache(cacheKey);
                        if (cached != null) return cached;

                        string authPayload;
                        if (!OpenKneeboardNavigraphEfbState.TryGetDecryptedAuthBlob(out authPayload) || string.IsNullOrWhiteSpace(authPayload))
                        {
                            return null;
                        }

                        JObject payload = JObject.Parse(authPayload);
                        string accessToken = (string)payload["access_token"] ?? "";
                        if (string.IsNullOrWhiteSpace(accessToken)) return null;

                        string directImageUrl = null;
                        if (useNight)
                        {
                            ChartImageNightUrlById.TryGetValue(chartId, out directImageUrl);
                        }
                        if (string.IsNullOrWhiteSpace(directImageUrl))
                        {
                            ChartImageDayUrlById.TryGetValue(chartId, out directImageUrl);
                        }

                        if (!string.IsNullOrWhiteSpace(directImageUrl))
                        {
                            using (var client = new HttpClient())
                            {
                                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                                // try { Log.Write("Navigraph chart image request URL: " + directImageUrl, VAICOM.Static.Colors.Debug); } catch { }
                                var resp = client.GetAsync(directImageUrl).GetAwaiter().GetResult();
                                if (!resp.IsSuccessStatusCode)
                                {
                                    try
                                    {
                                        string body = resp.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                                        Log.Write("Navigraph chart image request failed: " + ((int)resp.StatusCode) + " " + resp.ReasonPhrase + " body=" + BuildLogPreview(body), VAICOM.Static.Colors.Warning);
                                    }
                                    catch { }
                                }
                                if (resp.IsSuccessStatusCode)
                                {
                                    var bytes = resp.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();
                                    TryWriteTileCache(cacheKey, bytes);
                                    return bytes;
                                }
                            }
                        }
                        else
                        {
                            try { Log.Write("Navigraph chart image URL cache miss for chart id: " + chartId + " (night=" + (useNight ? "1" : "0") + ")", VAICOM.Static.Colors.Warning); } catch { }
                        }

                        using (var client = new HttpClient())
                        {
                            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                            string apiBase = null;
                            try { apiBase = VAICOM.State.activeconfig.OpenKneeboard_EfbApiBase; } catch { apiBase = null; }
                            if (string.IsNullOrWhiteSpace(apiBase)) apiBase = "https://api.navigraph.com/v1";
                            var url = apiBase.TrimEnd('/') + $"/charts/{Uri.EscapeDataString(chartId)}/image";
                            var resp = client.GetAsync(url).GetAwaiter().GetResult();
                            if (!resp.IsSuccessStatusCode) return null;
                            var bytes = resp.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();
                            TryWriteTileCache(cacheKey, bytes);
                            return bytes;
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }

                private static string GetCacheFolder()
                {
                    try
                    {
                        string profile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                        string dir = Path.Combine(profile, "VAICOM", "NavigraphCache");
                        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                        return dir;
                    }
                    catch
                    {
                        return null;
                    }
                }

                private static string HashKey(string key)
                {
                    try
                    {
                        using (var sha = SHA256.Create())
                        {
                            var bytes = Encoding.UTF8.GetBytes(key ?? "");
                            var hash = sha.ComputeHash(bytes);
                            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                        }
                    }
                    catch { return null; }
                }

                private static string BuildLogPreview(string value)
                {
                    string s = value ?? "";
                    if (s.Length <= 220)
                    {
                        return s;
                    }

                    return s.Substring(0, 220) + "...";
                }

                private static string TryResolveAirportName(HttpClient client, string icao)
                {
                    try
                    {
                        if (client == null || string.IsNullOrWhiteSpace(icao))
                        {
                            return null;
                        }

                        string apiBase = null;
                        try { apiBase = VAICOM.State.activeconfig.OpenKneeboard_EfbApiBase; } catch { apiBase = null; }

                        var urls = new List<string>();
                        urls.Add("https://api.navigraph.com/v2/airports/" + Uri.EscapeDataString(icao));

                        if (!string.IsNullOrWhiteSpace(apiBase))
                        {
                            var trimmed = apiBase.TrimEnd('/');
                            urls.Add(trimmed + "/v2/airports/" + Uri.EscapeDataString(icao));
                            urls.Add(trimmed + "/airports/" + Uri.EscapeDataString(icao));
                        }

                        foreach (var url in urls.Distinct(StringComparer.OrdinalIgnoreCase))
                        {
                            try
                            {
                                var resp = client.GetAsync(url).GetAwaiter().GetResult();
                                if (!resp.IsSuccessStatusCode)
                                {
                                    continue;
                                }

                                var txt = resp.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                                if (string.IsNullOrWhiteSpace(txt))
                                {
                                    continue;
                                }

                                var parsed = JToken.Parse(txt);
                                string name = ExtractAirportName(parsed);
                                if (!string.IsNullOrWhiteSpace(name))
                                {
                                    return name.Trim();
                                }
                            }
                            catch
                            {
                            }
                        }
                    }
                    catch
                    {
                    }

                    return null;
                }

                private static string ExtractAirportName(JToken token)
                {
                    if (token == null)
                    {
                        return null;
                    }

                    var obj = token as JObject;
                    if (obj != null)
                    {
                        string direct = (string)(obj["name"] ?? obj["airport_name"] ?? obj["display_name"] ?? obj["full_name"]);
                        if (!string.IsNullOrWhiteSpace(direct))
                        {
                            return direct;
                        }

                        var dataObj = obj["data"] as JObject;
                        if (dataObj != null)
                        {
                            string nested = (string)(dataObj["name"] ?? dataObj["airport_name"] ?? dataObj["display_name"] ?? dataObj["full_name"]);
                            if (!string.IsNullOrWhiteSpace(nested))
                            {
                                return nested;
                            }
                        }

                        var dataArray = obj["data"] as JArray;
                        if (dataArray != null)
                        {
                            foreach (var item in dataArray)
                            {
                                var itemObj = item as JObject;
                                if (itemObj == null)
                                {
                                    continue;
                                }

                                string fromItem = (string)(itemObj["name"] ?? itemObj["airport_name"] ?? itemObj["display_name"] ?? itemObj["full_name"]);
                                if (!string.IsNullOrWhiteSpace(fromItem))
                                {
                                    return fromItem;
                                }
                            }
                        }
                    }

                    return null;
                }

                private static string ResolveChartImageUrl(string explicitUrl, string fileName, string normalizedAirport, string resolvedCatalogUrl)
                {
                    if (!string.IsNullOrWhiteSpace(explicitUrl))
                    {
                        return explicitUrl;
                    }

                    if (!string.IsNullOrWhiteSpace(fileName) && !string.IsNullOrWhiteSpace(normalizedAirport))
                    {
                        // Prefer documented v2 image endpoint shape for image files.
                        // Signed charts.json URLs can be valid only for the JSON document,
                        // while chart PNG siblings may return 403 when reusing the same signature.
                        return "https://api.navigraph.com/v2/charts/"
                            + Uri.EscapeDataString(normalizedAirport)
                            + "/"
                            + Uri.EscapeDataString(fileName);
                    }

                    if (!string.IsNullOrWhiteSpace(fileName) && !string.IsNullOrWhiteSpace(resolvedCatalogUrl))
                    {
                        try
                        {
                            Uri baseUri = new Uri(resolvedCatalogUrl, UriKind.Absolute);
                            Uri imageUri = new Uri(baseUri, fileName);

                            // Some signed catalog URLs require keeping the same query signature
                            // when requesting sibling chart images.
                            string signedQuery = baseUri.Query;
                            if (!string.IsNullOrWhiteSpace(signedQuery) && string.IsNullOrWhiteSpace(imageUri.Query))
                            {
                                string withQuery = imageUri.ToString();
                                withQuery += signedQuery.StartsWith("?", StringComparison.Ordinal) ? signedQuery : ("?" + signedQuery);
                                return withQuery;
                            }

                            return imageUri.ToString();
                        }
                        catch
                        {
                        }
                    }

                    return null;
                }

                private static string NormalizeNavigraphEnrouteLayer(string layer)
                {
                    string v = (layer ?? "").Trim().ToLowerInvariant();
                    if (v == "ifr-lo" || v == "ifr.lo" || v == "ifrlow" || v == "low") return "ifr.lo";
                    if (v == "ifr-hi" || v == "ifr.hi" || v == "ifrhigh" || v == "high") return "ifr.hi";
                    return "vfr";
                }

                public static byte[] GetEnrouteVfrTileBytes(int z, int x, int y, bool nightMode, string layer)
                {
                    try
                    {
                        if (z < 0 || z > 18 || x < 0 || y < 0)
                        {
                            return null;
                        }

                        // Navigraph enroute bitmap uses XYZ addressing while MapLibre requests XYZ with TMS-compatible
                        // Y orientation differences depending on source style setup. Try both Y variants.
                        int tmsY = ((1 << z) - 1) - y;

                        string layerBase = NormalizeNavigraphEnrouteLayer(layer);
                        string layerId = layerBase + (nightMode ? ".night" : ".day");
                        string cacheKeyBase = "nav_enroute_v2_" + layerId + "_" + z + "_" + x + "_" + y;
                        byte[] cached = TryReadTileCache(cacheKeyBase + "_2x");
                        if (cached == null)
                        {
                            cached = TryReadTileCache(cacheKeyBase + "_1x");
                        }
                        if (cached != null)
                        {
                            return cached;
                        }

                        object tileLock = TileFetchLocks.GetOrAdd(cacheKeyBase, _ => new object());
                        lock (tileLock)
                        {
                            cached = TryReadTileCache(cacheKeyBase + "_2x");
                            if (cached == null)
                            {
                                cached = TryReadTileCache(cacheKeyBase + "_1x");
                            }
                            if (cached != null)
                            {
                                return cached;
                            }

                        if (!TryEnsureFreshAccessToken())
                        {
                            return null;
                        }

                        string authPayload;
                        if (!OpenKneeboardNavigraphEfbState.TryGetDecryptedAuthBlob(out authPayload) || string.IsNullOrWhiteSpace(authPayload))
                        {
                            return null;
                        }

                        JObject payload = JObject.Parse(authPayload);
                        string accessToken = (string)payload["access_token"] ?? "";
                        if (string.IsNullOrWhiteSpace(accessToken))
                        {
                            return null;
                        }

                        string policy = (string)payload["tile_cookie_policy"] ?? "";
                        string signature = (string)payload["tile_cookie_signature"] ?? "";
                        string keyPairId = (string)payload["tile_cookie_keypairid"] ?? "";

                        bool hasAllCookies = !string.IsNullOrWhiteSpace(policy)
                            && !string.IsNullOrWhiteSpace(signature)
                            && !string.IsNullOrWhiteSpace(keyPairId);

                        if (!hasAllCookies)
                        {
                            string refreshMsg;
                            bool refreshed = OpenKneeboardNavigraphOAuthService.TryRefreshAccessToken(out refreshMsg);
                            try { Log.Write(refreshed ? "Navigraph tile auth refresh: received." : ("Navigraph tile auth refresh failed: " + refreshMsg), refreshed ? VAICOM.Static.Colors.Text : VAICOM.Static.Colors.Warning); } catch { }

                            if (refreshed && OpenKneeboardNavigraphEfbState.TryGetDecryptedAuthBlob(out authPayload) && !string.IsNullOrWhiteSpace(authPayload))
                            {
                                payload = JObject.Parse(authPayload);
                                accessToken = (string)payload["access_token"] ?? accessToken;
                                policy = (string)payload["tile_cookie_policy"] ?? "";
                                signature = (string)payload["tile_cookie_signature"] ?? "";
                                keyPairId = (string)payload["tile_cookie_keypairid"] ?? "";
                                hasAllCookies = !string.IsNullOrWhiteSpace(policy)
                                    && !string.IsNullOrWhiteSpace(signature)
                                    && !string.IsNullOrWhiteSpace(keyPairId);
                            }
                        }

                        // try
                        // {
                        //     Log.Write("Navigraph tile auth cookies present: policy=" + (!string.IsNullOrWhiteSpace(policy) ? "1" : "0")
                        //         + " sig=" + (!string.IsNullOrWhiteSpace(signature) ? "1" : "0")
                        //         + " keyPair=" + (!string.IsNullOrWhiteSpace(keyPairId) ? "1" : "0"), VAICOM.Static.Colors.Debug);
                        // }
                        // catch { }

                        bool preferTmsY;
                        bool hasPreferTmsY;
                        lock (TileRequestModeSync)
                        {
                            hasPreferTmsY = PreferTileTmsY.HasValue;
                            preferTmsY = PreferTileTmsY.GetValueOrDefault(false);
                        }

                        int[] yCandidates;
                        if (tmsY >= 0 && tmsY != y)
                        {
                            yCandidates = hasPreferTmsY
                                ? (preferTmsY ? new[] { tmsY, y } : new[] { y, tmsY })
                                : new[] { y, tmsY };
                        }
                        else
                        {
                            yCandidates = new[] { y };
                        }

                        bool preferBearer;
                        bool hasPreferBearer;
                        lock (TileRequestModeSync)
                        {
                            hasPreferBearer = PreferTileBearerAuth.HasValue;
                            preferBearer = PreferTileBearerAuth.GetValueOrDefault(true);
                        }

                        bool[] authModes = hasPreferBearer
                            ? (preferBearer ? new[] { true, false } : new[] { false, true })
                            : new[] { true, false };

                        string[] scales = new[] { "@2x", "" };
                        foreach (var yCandidate in yCandidates)
                        {
                            foreach (var currentScale in scales)
                            {
                                string url = "https://enroute-bitmap.charts.api-v2.navigraph.com/styles/"
                                    + layerId + "/"
                                    + z + "/"
                                    + x + "/"
                                    + yCandidate + currentScale + ".png";

                                var cookieJar = new CookieContainer();
                                int cookieCount = 0;
                                if (hasAllCookies)
                                {
                                    cookieCount = TryAddNavigraphTileCookies(cookieJar, policy, signature, keyPairId);
                                }

                                foreach (var includeBearer in authModes)
                                {
                                    using (var handler = new HttpClientHandler { UseCookies = true, CookieContainer = cookieJar })
                                    using (var client = new HttpClient(handler))
                                    {
                                        client.Timeout = TimeSpan.FromSeconds(10);
                                        if (includeBearer)
                                        {
                                            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                                        }
                                        client.DefaultRequestHeaders.Referrer = new Uri("https://charts.navigraph.com/");
                                        client.DefaultRequestHeaders.TryAddWithoutValidation("Origin", "https://charts.navigraph.com");

                                            var resp = client.GetAsync(url).GetAwaiter().GetResult();
                                            if (!resp.IsSuccessStatusCode)
                                            {
                                                try
                                                {
                                                    Log.Write("Navigraph enroute tile request failed: " + ((int)resp.StatusCode) + " " + resp.ReasonPhrase, VAICOM.Static.Colors.Warning);

                                                    if ((int)resp.StatusCode == 403)
                                                    {
                                                        string body = resp.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                                                        Log.Write("Navigraph enroute tile 403 body preview: " + BuildLogPreview(body), VAICOM.Static.Colors.Warning);
                                                    }
                                                }
                                                catch { }
                                                continue;
                                            }

                                            var bytes = resp.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();
                                            if (bytes == null || bytes.Length == 0)
                                            {
                                                continue;
                                            }

                                            string cacheKey = cacheKeyBase + (string.IsNullOrWhiteSpace(currentScale) ? "_1x" : "_2x");
                                            TryWriteTileCache(cacheKey, bytes);
                                            return bytes;
                                        }
                                    }
                                }
                            }
                            return null;
                        }
                    }
                    catch
                    {
                        return null;
                    }
                    finally
                    {
                        try
                        {
                            string layerBase = NormalizeNavigraphEnrouteLayer(layer);
                            string layerId = layerBase + (nightMode ? ".night" : ".day");
                            string cacheKeyBase = "nav_enroute_v2_" + layerId + "_" + z + "_" + x + "_" + y;
                            object removed;
                            TileFetchLocks.TryRemove(cacheKeyBase, out removed);
                        }
                        catch
                        {
                        }
                    }
                }

                private static int TryAddNavigraphTileCookies(CookieContainer jar, string policy, string signature, string keyPairId)
                {
                    int added = 0;
                    try
                    {
                        if (jar == null)
                        {
                            return added;
                        }

                        Uri tileBase = new Uri("https://enroute-bitmap.charts.api-v2.navigraph.com/");

                        if (!string.IsNullOrWhiteSpace(policy))
                        {
                            jar.Add(tileBase, new Cookie("CloudFront-Policy", policy));
                            added++;
                        }

                        if (!string.IsNullOrWhiteSpace(signature))
                        {
                            jar.Add(tileBase, new Cookie("CloudFront-Signature", signature));
                            added++;
                        }

                        if (!string.IsNullOrWhiteSpace(keyPairId))
                        {
                            jar.Add(tileBase, new Cookie("CloudFront-Key-Pair-Id", keyPairId));
                            added++;
                        }
                    }
                    catch
                    {
                    }

                    return added;
                }

                private static byte[] TryReadTileCache(string cacheKey)
                {
                    try
                    {
                        var folder = GetCacheFolder();
                        if (string.IsNullOrWhiteSpace(folder)) return null;
                        var name = HashKey(cacheKey);
                        if (string.IsNullOrWhiteSpace(name)) return null;
                        var path = Path.Combine(folder, name + ".bin");
                        if (!File.Exists(path)) return null;
                        var info = new FileInfo(path);
                        if ((DateTime.UtcNow - info.LastWriteTimeUtc).TotalHours > 24) return null;
                        return File.ReadAllBytes(path);
                    }
                    catch { return null; }
                }

                private static void TryWriteTileCache(string cacheKey, byte[] bytes)
                {
                    try
                    {
                        if (bytes == null || bytes.Length == 0) return;
                        var folder = GetCacheFolder();
                        if (string.IsNullOrWhiteSpace(folder)) return;
                        var name = HashKey(cacheKey);
                        if (string.IsNullOrWhiteSpace(name)) return;
                        var path = Path.Combine(folder, name + ".bin");
                        File.WriteAllBytes(path, bytes);
                    }
                    catch { }
                }
            }
        }
    }
}
