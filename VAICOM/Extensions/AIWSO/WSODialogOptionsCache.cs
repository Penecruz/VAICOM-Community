using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using VAICOM.Database;
using VAICOM.Static;

namespace VAICOM
{
    namespace Extensions
    {
        namespace AIWSO
        {
            public static class WSODialogOptionsCache
            {
                private static Dictionary<string, string> ActionToOptionCache = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                private static Dictionary<string, string> OptionToActionCache = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                private static int cacheCount = 0;

                private static readonly object CacheLock = new object();
                
                public static bool TryHandleDialogCacheMessage(string receivedMessage)
                {
                    if (string.IsNullOrWhiteSpace(receivedMessage) || !receivedMessage.TrimStart().StartsWith("{"))
                    {
                        return false;
                    }

                    try
                    {
                        JObject message = JObject.Parse(receivedMessage);
                        JArray dialogs = message["dialog_queue"] as JArray;

                        // Dialog has likely been closed
                        if (dialogs.Count == 0)
                        {
                            return false;
                        }

                        if (State.activeconfig.Debugmode)
                        {
                            WriteDialogOptionsToFile(receivedMessage);
                        }

                        // An update has been made to the menu items. Rebuild the cache so that
                        // it is in a consistent state as entries may have been removed.
                        lock (CacheLock)
                        {
                            Log.Write($"WSO dialog options have been updated so rebuilding caches", Colors.Text);
                            ActionToOptionCache = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                            OptionToActionCache = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                            cacheCount = 0;
                        }

                        foreach (JObject dialog in dialogs)
                        {
                            CacheDialogOptions(dialog);
                        }

                        if (cacheCount > 0)
                        {
                            Log.Write($"WSO dialog cache updated ({cacheCount} entries).", Colors.Text);
                            return true;
                        }

                        return false;
                    }
                    catch
                    {
                        return false;
                    }
                }

                private static void CacheDialogOptions(JObject dialog)
                {
                    JArray options = dialog["options"] as JArray;
                    
                    // Recursively look through dialogs if any options
                    // do not have an "action" but have "more" instead.
                    foreach (JObject optionsObject in options)
                    {
                        if (optionsObject["action"] != null)
                        {
                            if (TryCacheEntry(optionsObject))
                            {
                                cacheCount++;
                            }
                        }
                        else if (optionsObject["more"] != null)
                        {
                            CacheDialogOptions(optionsObject["more"] as JObject);
                        }
                    }
                }

                public static bool TryGetActionByOption(string option, out string action)
                {
                    lock (CacheLock)
                    {
                        if (OptionToActionCache.TryGetValue(option, out action))
                        {
                            return true;
                        }
                    }

                    return false;
                }

                public static bool TryGetOptionByAction(string action, out string option)
                {
                    lock (CacheLock)
                    {
                        if (ActionToOptionCache.TryGetValue(action, out option))
                        {
                            return true;
                        }
                    }

                    return false;
                }

                public static bool TryGetActionByRecipient(string recipient, out string action)
                {
                    lock (CacheLock)
                    {
                        if (OptionToActionCache.TryGetValue(recipient, out action))
                        {
                            return true;
                        }
                    }

                    return false;
                }

                private static bool TryCacheEntry(JObject message, bool logSingleEntry = true)
                {
                    string action = message["action"]?.ToString();
                    string option = message["option"]?.ToString();

                    if (string.IsNullOrWhiteSpace(action) || string.IsNullOrWhiteSpace(option))
                    {
                        return false;
                    }

                    if (action.StartsWith("divert_airfield_", StringComparison.OrdinalIgnoreCase))
                    {
                        lock (CacheLock)
                        {
                            if (TryGetRecipientByOption(option.ToLowerInvariant(), out string recipient))
                            {
                                OptionToActionCache[$"{recipient.ToLowerInvariant()}"] = action;
                                ActionToOptionCache[action] = $"{recipient.ToLowerInvariant()}";
                            }
                            else
                            {
                                OptionToActionCache[option] = action;
                                ActionToOptionCache[action] = option;
                            }
                        }
                    }
                    else
                    {
                        lock (CacheLock)
                        {
                            OptionToActionCache[option] = action;
                            ActionToOptionCache[action] = option;
                        }
                    }

                    if (logSingleEntry)
                    {
                         Log.Write($"WSO dialog cache entry: option={option}, action={action}", Colors.Text);
                    }

                    return true;
                }

                public static object GetDialogOptionsCacheLock()
                {
                    return CacheLock;
                }

                private static bool TryGetRecipientByOption(string option, out string recipient)
                {
                    recipient = "";

                    // Search the recipients for one which matches the option name. This allows us to use
                    // aliases in commands and still map them back to the actual cache entry.
                    // Search through the standard set of AI recipients
                    foreach (KeyValuePair<string, string> set in Aliases.airecipients)
                    {
                        // Check for recipients value, or if there are other aliased values.
                        if (option.StartsWith(set.Value, StringComparison.OrdinalIgnoreCase) || option.StartsWith(set.Key, StringComparison.OrdinalIgnoreCase))
                        {
                            recipient = set.Value;
                            return true;
                        }
                    }
                    // Search through any imported ATCs
                    foreach (KeyValuePair<string, string> set in Aliases.importedatcs)
                    {
                        // Check for recipients value, or if there are other aliased values.
                        if (option.StartsWith(set.Value, StringComparison.OrdinalIgnoreCase) || option.StartsWith(set.Key, StringComparison.OrdinalIgnoreCase))
                        {
                            recipient = set.Value;
                            return true;
                        }
                    }

                    return false;
                }

                private static void WriteDialogOptionsToFile(string rawMessage)
                {
                    try
                    {
                        string logsFolder = State.VA_APPS + "\\" + Products.Products.Families.Vaicom.VaicomProPlugin.rootfoldername + "\\" + AppData.SubFolders["logfiles"];
                        Directory.CreateDirectory(logsFolder);

                        string rawFile = Path.Combine(logsFolder, "WSO_DIALOG_CACHE_RAW.json");
                        File.WriteAllText(rawFile, rawMessage ?? "");
                    }
                    catch
                    {
                    }
                }
            }
        }
    }
}