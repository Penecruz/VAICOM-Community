using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using VAICOM.Database;
using VAICOM.Static;

namespace VAICOM
{
    namespace Extensions
    {
        namespace AIWSO
        {
            public static class WSOActionCache
            {
                private static Dictionary<string, string> CacheByActionAndIndex = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                private static Dictionary<string, string> CacheByActionAndName = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                private static readonly object CacheLock = new object();
                private static readonly object SignatureLock = new object();

                private static string LastSignature = "";

                public static bool TryHandleJesterMenuActionLine(string receivedMessage)
                {
                    if (string.IsNullOrWhiteSpace(receivedMessage))
                    {
                        return false;
                    }

                    const string prefix = "Jester 2.0 Menu:";
                    if (!receivedMessage.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    {
                        return false;
                    }

                    string payload = receivedMessage.Substring(prefix.Length).Trim();
                    Log.Write($"Received Jester 2.0 action: {payload}", Colors.Text);

                    return true;
                }

                public static bool TryHandleActionCacheMessage(string receivedMessage)
                {
                    if (string.IsNullOrWhiteSpace(receivedMessage) || !receivedMessage.TrimStart().StartsWith("{"))
                    {
                        return false;
                    }

                    try
                    {
                        JObject message = JObject.Parse(receivedMessage);
                        string messageType = message["type"]?.ToString();

                        if (string.IsNullOrWhiteSpace(messageType))
                        {
                            return false;
                        }

                        if (messageType.Equals("action_cache", StringComparison.OrdinalIgnoreCase))
                        {
                            return TryCacheEntry(message);
                        }

                        if (!messageType.Equals("action_cache_bulk", StringComparison.OrdinalIgnoreCase))
                        {
                            return false;
                        }

                        JArray items = message["items"] as JArray;
                        if (items == null || items.Count == 0)
                        {
                            return false;
                        }

                        WriteCacheRawToFile(receivedMessage);

                        if (TrySkipUnchangedCacheBulk(items))
                        {
                            return true;
                        }
                        else
                        {
                            // An update has been made to the menu items. Rebuild the cache so that
                            // it is in a consistent state as entries may have been removed.
                            lock (CacheLock)
                            {
                                Log.Write($"WSO menus have been updated so rebuilding caches", Colors.Text);
                                CacheByActionAndIndex = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                                CacheByActionAndName = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                            }
                        }

                        int cachedCount = 0;
                        foreach (JToken item in items)
                        {
                            if (item is JObject entry && TryCacheEntry(entry, false))
                            {
                                cachedCount++;
                            }
                        }

                        if (cachedCount > 0)
                        {
                            Log.Write($"WSO action cache updated ({cachedCount} entries).", Colors.Text);
                            if (State.deepdebugmode)
                            {
                                LogCacheSnapshot(50);
                            }
                            return true;
                        }

                        return false;
                    }
                    catch
                    {
                        return false;
                    }
                }

                public static bool TryGetByActionAndIndex(string action, string index, out string value)
                {
                    bool hasEntry = TryGetByActionAndIndex($"{action}|{index}", out string entry);
                    value = entry;

                    return hasEntry;
                }

                public static bool TryGetByActionAndIndex(string cacheKey, out string value)
                {
                    lock (CacheLock)
                    {
                        bool hasEntry = CacheByActionAndIndex.TryGetValue(cacheKey, out string entry);

                        if (hasEntry)
                        {
                            value = entry;
                            return true;
                        }
                    }

                    value = "";
                    return false;
                }

                public static bool TryGetByActionAndName(string action, string name, out string value)
                {
                    lock (CacheLock)
                    {
                        bool hasEntry = CacheByActionAndName.TryGetValue($"{action}|{name}", out string entry);

                        if (hasEntry)
                        {
                            value = entry;
                            return true;
                        }
                    }

                    value = "";
                    return false;
                }

                public static Dictionary<string, string> GetActionByIndexCacheEntries()
                {
                    return CacheByActionAndIndex;
                }

                public static Dictionary<string, string> GetActionByNameCacheEntries()
                {
                    return CacheByActionAndName;
                }

                private static void WriteCacheRawToFile(string rawMessage)
                {
                    try
                    {
                        string logsFolder = State.VA_APPS + "\\" + Products.Products.Families.Vaicom.VaicomProPlugin.rootfoldername + "\\" + AppData.SubFolders["logfiles"];
                        Directory.CreateDirectory(logsFolder);

                        string rawFile = Path.Combine(logsFolder, "WSO_ACTION_CACHE_RAW.json");
                        File.WriteAllText(rawFile, rawMessage ?? "");
                    }
                    catch
                    {
                    }
                }

                private static bool TrySkipUnchangedCacheBulk(JArray items)
                {
                    string currentSignature = BuildSignature(items);

                    lock (SignatureLock)
                    {
                        if (string.Equals(LastSignature, currentSignature, StringComparison.Ordinal))
                        {
                            return true;
                        }

                        LastSignature = currentSignature;
                        return false;
                    }
                }

                private static string BuildSignature(JArray items)
                {
                    List<string> normalizedEntries = new List<string>(items.Count);

                    foreach (JToken token in items)
                    {
                        if (!(token is JObject entry))
                        {
                            continue;
                        }

                        string action = entry["action"]?.ToString() ?? "";
                        string value = entry["value"]?.ToString() ?? "";
                        string name = entry["name"]?.ToString() ?? "";
                        string path = entry["path"]?.ToString() ?? "";
                        string index = entry["idx"]?.ToString() ?? "";

                        if (string.IsNullOrWhiteSpace(index))
                        {
                            index = TryExtractIndexFromValue(value);
                            if (string.IsNullOrWhiteSpace(index))
                            {
                                index = TryExtractIndexFromNameOrPath(name, path);
                            }
                        }

                        normalizedEntries.Add(string.Concat(
                            action, "|",
                            index, "|",
                            name, "|",
                            path, "|",
                            value));
                    }

                    normalizedEntries.Sort(StringComparer.Ordinal);
                    return string.Join("\n", normalizedEntries);
                }

                public static bool TryCacheEntry(JObject message, bool logSingleEntry = true)
                {
                    string action = message["action"]?.ToString();
                    string value = message["value"]?.ToString();
                    string name = message["name"]?.ToString();
                    string index = message["idx"]?.ToString();
                    string path = message["path"]?.ToString();

                    if (string.IsNullOrWhiteSpace(action) || string.IsNullOrWhiteSpace(value))
                    {
                        return false;
                    }

                    if (string.Equals(action, "radio_tune_atc") && !string.IsNullOrEmpty(name))
                    {
                        lock (CacheLock)
                        {
                            string asset = name;
                            // Remove the radio frequency from the name if present, e.g. Gelendzhik (255.000 mHz)
                            int freqIndex = name.IndexOf('(');
                            if (freqIndex > 0)
                            {
                                asset = name.Substring(0, freqIndex - 1);
                            }

                            if (TryGetRecipientByAsset(asset.ToLowerInvariant(), out string recipient))
                            {
                                CacheByActionAndName[$"{action}|{recipient.ToLowerInvariant()}"] = value;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    else if (string.Equals(action, "nav_tacan_tr"))
                    {
                        lock (CacheLock)
                        {
                            // The path for assets (Tune Assets) is for name based TACANs, e.g. Arco 1-1 KC-135
                            if (path.Contains("Tune Assets"))
                            {
                                if (TryGetRecipientByAsset(name.ToLowerInvariant(), out string recipient))
                                {
                                    CacheByActionAndName[$"{action}|{recipient.ToLowerInvariant()}"] = value;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                // Ground based TACAN stations are in the format "GTB 25X" so just get
                                // the letters for looking up the freqency.
                                string station = name.Substring(0, 3);
                                CacheByActionAndName[$"{action}|{station.ToLowerInvariant()}"] = value;
                            }
                        }
                    }
                    else if (string.Equals(action, "divert_tgt1_lat_lon") && (path.Contains("Divert To > Airfields") || path.Contains("Divert To > Assets")))
                    {
                        lock (CacheLock)
                        {
                            // Diverting to airfields, or assets such as tankers, e.g. Arco 1-1 KC-135
                            if (TryGetRecipientByAsset(name.ToLowerInvariant(), out string recipient))
                            {
                                CacheByActionAndName[$"{action}|{recipient.ToLowerInvariant()}"] = value;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(index))
                        {
                            index = TryExtractIndexFromValue(value);
                            if (string.IsNullOrWhiteSpace(index))
                            {
                                index = TryExtractIndexFromNameOrPath(name, path);
                            }
                        }

                        lock (CacheLock)
                        {
                            if (!string.IsNullOrWhiteSpace(index))
                            {
                                CacheByActionAndIndex[$"{action}|{index}"] = value;

                                string flightPlan = TryExtractFlightPlanFromPath(path);
                                if (!string.IsNullOrWhiteSpace(flightPlan))
                                {
                                    CacheByActionAndIndex[$"{action}|fp{flightPlan}|{index}"] = value;
                                }
                            }

                            if (!string.IsNullOrWhiteSpace(name))
                            {
                                CacheByActionAndName[$"{action}|{name.ToLowerInvariant()}"] = value;
                            }
                        }
                    }


                    if (logSingleEntry)
                    {
                        List<string> keyParts = new List<string>();
                        if (!string.IsNullOrWhiteSpace(index))
                        {
                            keyParts.Add($"idx={index}");
                        }
                        if (!string.IsNullOrWhiteSpace(name))
                        {
                            keyParts.Add($"name={name}");
                        }

                        string keyInfo = keyParts.Count > 0 ? string.Join(", ", keyParts) : "no key";
                        // Log.Write($"WSO action cache entry: action={action}, {keyInfo}, value={value}", Colors.Text);
                    }

                    return true;
                }

                public static object GetActionCacheLock()
                {
                    return CacheLock;
                }

                private static string TryExtractIndexFromValue(string value)
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        return "";
                    }

                    string[] tokens = value.Split(';');
                    if (tokens.Length < 3)
                    {
                        return "";
                    }

                    string candidate = tokens[tokens.Length - 1].Trim();
                    return int.TryParse(candidate, out int parsed) ? parsed.ToString() : "";
                }

                private static string TryExtractIndexFromNameOrPath(string name, string path)
                {
                    string fromName = TryExtractLeadingIndex(name);
                    if (!string.IsNullOrWhiteSpace(fromName))
                    {
                        return fromName;
                    }

                    if (string.IsNullOrWhiteSpace(path))
                    {
                        return "";
                    }

                    string[] parts = path.Split('>');
                    for (int i = parts.Length - 1; i >= 0; i--)
                    {
                        string fromPart = TryExtractLeadingIndex(parts[i]);
                        if (!string.IsNullOrWhiteSpace(fromPart))
                        {
                            return fromPart;
                        }
                    }

                    return "";
                }

                private static string TryExtractLeadingIndex(string text)
                {
                    if (string.IsNullOrWhiteSpace(text))
                    {
                        return "";
                    }

                    Match match = Regex.Match(text, @"^\s*\*?\s*(\d{1,3})\s*:");
                    if (!match.Success)
                    {
                        return "";
                    }

                    return int.TryParse(match.Groups[1].Value, out int parsed) ? parsed.ToString() : "";
                }

                private static bool TryGetRecipientByAsset(string asset, out string recipient)
                {
                    recipient = "";

                    // Search the recipients for one which matches the asset name. This allows us to use
                    // aliases in commands and still map them back to the actual cache entry.
                    // Search through the standard set of AI recipients
                    foreach (KeyValuePair<string, string> set in Aliases.airecipients)
                    {
                        // Check for recipients value, or if there are other aliased values.
                        if (asset.StartsWith(set.Value, StringComparison.OrdinalIgnoreCase) || asset.StartsWith(set.Key, StringComparison.OrdinalIgnoreCase))
                        {
                            recipient = set.Value;
                            return true;
                        }
                    }
                    // Search through any imported ATCs
                    foreach (KeyValuePair<string, string> set in Aliases.importedatcs)
                    {
                        // Check for recipients value, or if there are other aliased values.
                        if (asset.StartsWith(set.Value, StringComparison.OrdinalIgnoreCase) || asset.StartsWith(set.Key, StringComparison.OrdinalIgnoreCase))
                        {
                            recipient = set.Value;
                            return true;
                        }
                    }

                    Log.Write($"No recipient found for asset '{asset}'", Colors.Text);
                    return false;
                }

                private static string TryExtractFlightPlanFromPath(string path)
                {
                    if (string.IsNullOrWhiteSpace(path))
                    {
                        return "";
                    }

                    if (path.IndexOf("Primary Flight Plan", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        return "1";
                    }

                    if (path.IndexOf("Secondary Flight Plan", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        return "2";
                    }

                    return "";
                }

                private static void LogCacheSnapshot(int maxEntries)
                {
                    int printed = 0;

                    lock (CacheLock)
                    {
                        foreach (KeyValuePair<string, string> entry in CacheByActionAndIndex)
                        {
                            Log.Write($"WSO action cache [idx] {entry.Key} => {entry.Value}", Colors.Text);
                            printed++;
                            if (printed >= maxEntries)
                            {
                                break;
                            }
                        }

                        if (printed < maxEntries)
                        {
                            foreach (KeyValuePair<string, string> entry in CacheByActionAndName)
                            {
                                Log.Write($"WSO action cache [name] {entry.Key} => {entry.Value}", Colors.Text);
                                printed++;
                                if (printed >= maxEntries)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}