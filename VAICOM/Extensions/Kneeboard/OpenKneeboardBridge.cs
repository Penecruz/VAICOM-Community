using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Windows;
using System.Globalization;
using VAICOM.Client;
using VAICOM.Static;

namespace VAICOM
{
    namespace Extensions
    {
        namespace Kneeboard
        {
            public static class OpenKneeboardBridge
            {
                private static readonly object Sync = new object();
                private static readonly object RawServerLogSync = new object();
                private static readonly object StoreLookupSync = new object();
                private static readonly object FastOwnshipSync = new object();
                private static readonly SemaphoreSlim RequestDispatchLimiter = new SemaphoreSlim(12, 12);
                private static OpenKneeboardSnapshot snapshot = new OpenKneeboardSnapshot();
                private static string lastAiCrewCommand = "";
                private static bool captureRawServerMessages;
                private static readonly string[] DtcFileExtensions = new[] { ".dtc", ".json" };
                private static readonly string[] EfbChartFileExtensions = new[] { ".svg", ".png", ".jpg", ".jpeg", ".gif", ".bmp", ".webp" };
                private const string RouteSelectionPrefix = "RTE::";
                private const string StoreLookupJsonPlaceholder = "__VAICOM_STORE_LOOKUP_JSON__";
                private static string storeLookupResolvedPath = "";
                private static DateTime storeLookupLastWriteUtc = DateTime.MinValue;
                private static string storeLookupMapJson = "{}";
                private static string currentSelectedTab = "";
                private static FastOwnshipState fastOwnship = new FastOwnshipState();

                private sealed class FastOwnshipState
                {
                    public bool HasPosition;
                    public double X;
                    public double Y;
                    public double AltFeet;
                    public double HeadingDeg;
                    public bool HasHeading;
                    public DateTime UpdatedUtc;
                }

                public static void UpdateFastOwnship(double x, double y, double z, double? headingDeg)
                {
                    if (double.IsNaN(x) || double.IsInfinity(x)
                        || double.IsNaN(y) || double.IsInfinity(y)
                        || double.IsNaN(z) || double.IsInfinity(z))
                    {
                        return;
                    }

                    lock (FastOwnshipSync)
                    {
                        fastOwnship.HasPosition = true;
                        fastOwnship.X = x;
                        fastOwnship.Y = z;
                        fastOwnship.AltFeet = y * 3.28084;
                        if (headingDeg.HasValue && !double.IsNaN(headingDeg.Value) && !double.IsInfinity(headingDeg.Value))
                        {
                            fastOwnship.HasHeading = true;
                            double heading = headingDeg.Value;
                            if (Math.Abs(heading) <= (Math.PI * 2.0 + 0.001))
                            {
                                heading = heading * 180.0 / Math.PI;
                            }
                            heading = heading % 360.0;
                            if (heading < 0)
                            {
                                heading += 360.0;
                            }
                            fastOwnship.HeadingDeg = heading;
                        }
                        fastOwnship.UpdatedUtc = DateTime.UtcNow;
                    }
                }

                private static bool TryGetFastOwnship(out FastOwnshipState value)
                {
                    lock (FastOwnshipSync)
                    {
                        value = new FastOwnshipState
                        {
                            HasPosition = fastOwnship.HasPosition,
                            X = fastOwnship.X,
                            Y = fastOwnship.Y,
                            AltFeet = fastOwnship.AltFeet,
                            HasHeading = fastOwnship.HasHeading,
                            HeadingDeg = fastOwnship.HeadingDeg,
                            UpdatedUtc = fastOwnship.UpdatedUtc,
                        };
                    }

                    if (!value.HasPosition)
                    {
                        return false;
                    }

                    return (DateTime.UtcNow - value.UpdatedUtc) <= TimeSpan.FromSeconds(2);
                }

                private static string IndexHtml;

                private static HttpListener listener;
                private static Thread listenerThread;
                private static bool isRunning;
                private static long lastClientRequestUtcTicks;
                private static long fastOwnshipLastLogUtcTicks;
                private static readonly Guid SavedGamesFolderId = new Guid("4C5C32FF-BB9D-43B0-B5B4-2D72E54EAAA4");

                [DllImport("shell32.dll")]
                private static extern int SHGetKnownFolderPath([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags, IntPtr hToken, out IntPtr ppszPath);

                private const int DefaultOpenKneeboardOutPort = 7779;
                private const string OpenKneeboardPluginsRegistryKey = @"SOFTWARE\Fred Emmott\OpenKneeboard\Plugins\v1";
                private const string OpenKneeboardPluginId = "VAICOM-Community";
                private const string OpenKneeboardPluginTabId = OpenKneeboardPluginId + ";okb-out";
                private const string OpenKneeboardKeywordsPluginId = "github.com/Penecruz/VAICOM-Community/keywords";
                private const string OpenKneeboardKeywordsPluginTabId = OpenKneeboardKeywordsPluginId + ";keywords";

                public static void Initialize()
                {
                    ResetRawServerMessagesLog();
                    ResetSnapshot();
                    RefreshDtcFilesSnapshot();
                    SetPluginRegistration(State.activeconfig != null && State.activeconfig.OpenKneeboard_Out);
                    LoadIndexHtmlPage();
                    StartWebHost();
                }
                
                private static void LoadIndexHtmlPage()
                {
                    IndexHtml = Properties.Resources.OKB_Out_Index_HTML;
                }

                public static void SetEnabled(bool enabled)
                {
                    if (enabled)
                    {
                        StopWebHost();
                    }

                    SetPluginRegistration(enabled);
                    SetKeywordsPluginRegistration();

                    if (enabled)
                    {
                        StartWebHost();
                    }
                    else
                    {
                        StopWebHost();
                    }
                }

                public static bool IsHostRunning
                {
                    get { return isRunning; }
                }

                public static bool HasActiveConnection
                {
                    get
                    {
                        if (!isRunning)
                        {
                            return false;
                        }

                        long ticks = Interlocked.Read(ref lastClientRequestUtcTicks);
                        if (ticks <= 0)
                        {
                            return false;
                        }

                        DateTime lastRequestUtc = new DateTime(ticks, DateTimeKind.Utc);
                        return (DateTime.UtcNow - lastRequestUtc) <= TimeSpan.FromSeconds(3);
                    }
                }

                private static void SetKeywordsPluginRegistration()
                {
                    try
                    {
                        string manifestPath = GetKeywordsPluginManifestPath();
                        if (string.IsNullOrWhiteSpace(manifestPath))
                        {
                            return;
                        }

                        WriteKeywordsPluginManifest(manifestPath, "");

                        using (RegistryKey key = Registry.CurrentUser.CreateSubKey(OpenKneeboardPluginsRegistryKey))
                        {
                            if (key == null)
                            {
                                return;
                            }

                            key.SetValue(manifestPath, 1, RegistryValueKind.DWord);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Write("OpenKneeboard keywords plugin registration failed: " + ex.Message, Colors.Warning);
                    }
                }

                private static int GetOpenKneeboardOutPort()
                {
                    int configuredPort = DefaultOpenKneeboardOutPort;

                    try
                    {
                        if (State.activeconfig != null)
                        {
                            configuredPort = State.activeconfig.OpenKneeboard_Out_Port;
                        }
                    }
                    catch
                    {
                    }

                    if (configuredPort <= 0 || configuredPort > 65535)
                    {
                        configuredPort = DefaultOpenKneeboardOutPort;
                    }

                    return configuredPort;
                }

                private static string GetPrefix()
                {
                    return "http://127.0.0.1:" + GetOpenKneeboardOutPort() + "/okb/";
                }

                public static void RegisterKeywordsHtmlPlugin(string keywordsHtmlPath)
                {
                    try
                    {
                        string manifestPath = GetKeywordsPluginManifestPath();
                        if (string.IsNullOrWhiteSpace(manifestPath))
                        {
                            return;
                        }

                        WriteKeywordsPluginManifest(manifestPath, keywordsHtmlPath);

                        using (RegistryKey key = Registry.CurrentUser.CreateSubKey(OpenKneeboardPluginsRegistryKey))
                        {
                            if (key == null)
                            {
                                return;
                            }

                            key.SetValue(manifestPath, 1, RegistryValueKind.DWord);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Write("OpenKneeboard keywords plugin registration failed: " + ex.Message, Colors.Warning);
                    }
                }

                public static void UpdateStatus(string text, string level)
                {
                    if (string.IsNullOrWhiteSpace(text))
                    {
                        return;
                    }

                    lock (Sync)
                    {
                        snapshot.Status = new OpenKneeboardStatusSnapshot
                        {
                            Text = text,
                            Level = string.IsNullOrWhiteSpace(level) ? "" : level,
                            UpdatedUtc = DateTime.UtcNow,
                        };
                        snapshot.UpdatedUtc = DateTime.UtcNow;
                    }
                }

                public static void Shutdown()
                {
                    StopWebHost();
                }

                private static void SetPluginRegistration(bool enabled)
                {
                    string manifestPath = GetPluginManifestPath();
                    if (string.IsNullOrWhiteSpace(manifestPath))
                    {
                        return;
                    }

                    if (enabled)
                    {
                        WritePluginManifest(manifestPath);
                    }

                    try
                    {
                        using (RegistryKey key = Registry.CurrentUser.CreateSubKey(OpenKneeboardPluginsRegistryKey))
                        {
                            if (key == null)
                            {
                                return;
                            }

                            if (enabled)
                            {
                                DeactivateAllOpenKneeboardOutRegistrations(key);
                                CleanupStaleOpenKneeboardOutManifestFiles(manifestPath);

                                key.SetValue(manifestPath, 1, RegistryValueKind.DWord);
                            }
                            else
                            {
                                DeactivateAllOpenKneeboardOutRegistrations(key);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Write("OpenKneeboard plugin registration failed: " + ex.Message, Colors.Warning);
                    }
                }

                private static void DeactivateAllOpenKneeboardOutRegistrations(RegistryKey key)
                {
                    try
                    {
                        foreach (string valueName in key.GetValueNames())
                        {
                            if (string.IsNullOrWhiteSpace(valueName))
                            {
                                continue;
                            }

                            string fileName = Path.GetFileName(valueName);
                            if (string.IsNullOrWhiteSpace(fileName))
                            {
                                continue;
                            }

                            if (!fileName.StartsWith("OpenKneeboard", StringComparison.OrdinalIgnoreCase))
                            {
                                continue;
                            }

                            if (fileName.IndexOf("OpenKneeboard.Keywords", StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                continue;
                            }

                            key.SetValue(valueName, 0, RegistryValueKind.DWord);
                        }
                    }
                    catch
                    {
                    }
                }

                private static void CleanupStaleOpenKneeboardOutManifestFiles(string activeManifestPath)
                {
                    try
                    {
                        string activeFullPath = Path.GetFullPath(activeManifestPath);
                        string manifestFolder = Path.GetDirectoryName(activeFullPath);
                        if (string.IsNullOrWhiteSpace(manifestFolder) || !Directory.Exists(manifestFolder))
                        {
                            return;
                        }

                        string[] staleManifests = Directory.GetFiles(manifestFolder, "OpenKneeboard*.v1.json");
                        foreach (string staleManifest in staleManifests)
                        {
                            string staleManifestFileName = Path.GetFileName(staleManifest);
                            if (string.IsNullOrWhiteSpace(staleManifestFileName))
                            {
                                continue;
                            }

                            if (staleManifestFileName.IndexOf("OpenKneeboard.Keywords", StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                continue;
                            }

                            string staleFullPath = Path.GetFullPath(staleManifest);
                            if (string.Equals(staleFullPath, activeFullPath, StringComparison.OrdinalIgnoreCase))
                            {
                                continue;
                            }

                            try
                            {
                                File.Delete(staleManifest);
                            }
                            catch
                            {
                            }
                        }
                    }
                    catch
                    {
                    }
                }

                private static string GetPluginManifestPath()
                {
                    try
                    {
                        string appsRoot = string.IsNullOrWhiteSpace(State.VA_APPS)
                            ? (State.Proxy == null ? "" : Convert.ToString(State.Proxy.SessionState["VA_APPS"]))
                            : State.VA_APPS;

                        if (string.IsNullOrWhiteSpace(appsRoot))
                        {
                            return "";
                        }

                        string pluginRoot = Path.Combine(appsRoot, Products.Products.Families.Vaicom.VaicomProPlugin.rootfoldername);
                        string configFolder = AppData.SubFolders.ContainsKey("config")
                            ? AppData.SubFolders["config"]
                            : "config";
                        string outputFolder = Path.Combine(pluginRoot, configFolder);

                        return Path.Combine(outputFolder, "OpenKneeboard." + GetOpenKneeboardOutPort() + ".v1.json");
                    }
                    catch
                    {
                        return "";
                    }
                }

                private static string GetKeywordsPluginManifestPath()
                {
                    try
                    {
                        string appsRoot = string.IsNullOrWhiteSpace(State.VA_APPS)
                            ? (State.Proxy == null ? "" : Convert.ToString(State.Proxy.SessionState["VA_APPS"]))
                            : State.VA_APPS;

                        if (string.IsNullOrWhiteSpace(appsRoot))
                        {
                            return "";
                        }

                        string pluginRoot = Path.Combine(appsRoot, Products.Products.Families.Vaicom.VaicomProPlugin.rootfoldername);
                        string configFolder = AppData.SubFolders.ContainsKey("config")
                            ? AppData.SubFolders["config"]
                            : "config";
                        string outputFolder = Path.Combine(pluginRoot, configFolder);

                        return Path.Combine(outputFolder, "OpenKneeboard.Keywords.v1.json");
                    }
                    catch
                    {
                        return "";
                    }
                }

                private static void WritePluginManifest(string manifestPath)
                {
                    try
                    {
                        string folder = Path.GetDirectoryName(manifestPath);
                        if (!string.IsNullOrWhiteSpace(folder))
                        {
                            Directory.CreateDirectory(folder);
                        }

                        string readableVersion = string.IsNullOrWhiteSpace(State.versionstring)
                            ? State.pluginversionnumber
                            : State.versionstring;
                        string semanticVersion = string.IsNullOrWhiteSpace(State.pluginversionnumber)
                            ? "3.1.0"
                            : State.pluginversionnumber;

                        var manifest = new
                        {
                            ID = OpenKneeboardPluginId,
                            Metadata = new
                            {
                                PluginName = "VAICOM OpenKneeboard Out",
                                PluginReadableVersion = readableVersion,
                                PluginSemanticVersion = semanticVersion,
                                OKBMinimumVersion = "1.9",
                                Author = "VAICOM Community",
                                Website = "https://github.com/Penecruz/VAICOM-Community",
                            },
                            TabTypes = new[]
                            {
                                new
                                {
                                    ID = OpenKneeboardPluginTabId,
                                    Name = "VAICOM OpenKneeboard Out",
                                    CustomActions = new[]
                                    {
                                        new
                                        {
                                            ID = OpenKneeboardPluginTabId + ";tab-prev",
                                            Name = "Tab Previous",
                                        },
                                        new
                                        {
                                            ID = OpenKneeboardPluginTabId + ";tab-next",
                                            Name = "Tab Next",
                                        },
                                        new
                                        {
                                            ID = OpenKneeboardPluginTabId + ";tab-select",
                                            Name = "Tab Select (via ExtraData)",
                                        },
                                        new
                                        {
                                            ID = OpenKneeboardPluginTabId + ";tab-log",
                                            Name = "Tab LOG",
                                        },
                                        new
                                        {
                                            ID = OpenKneeboardPluginTabId + ";tab-atc",
                                            Name = "Tab WX/ATC",
                                        },
                                        new
                                        {
                                            ID = OpenKneeboardPluginTabId + ";tab-awacs",
                                            Name = "Tab AWACS",
                                        },
                                        new
                                        {
                                            ID = OpenKneeboardPluginTabId + ";tab-jtac",
                                            Name = "Tab JTAC",
                                        },
                                        new
                                        {
                                            ID = OpenKneeboardPluginTabId + ";tab-tanker",
                                            Name = "Tab TANKER",
                                        },
                                        new
                                        {
                                            ID = OpenKneeboardPluginTabId + ";tab-aocs",
                                            Name = "Tab AOCS",
                                        },
                                        new
                                        {
                                            ID = OpenKneeboardPluginTabId + ";tab-flight",
                                            Name = "Tab FLIGHT",
                                        },
                                        new
                                        {
                                            ID = OpenKneeboardPluginTabId + ";tab-ai-crew",
                                            Name = "Tab AI CREW",
                                        },
                                        new
                                        {
                                            ID = OpenKneeboardPluginTabId + ";tab-gnd-crew",
                                            Name = "Tab GND CREW",
                                        },
                                        new
                                        {
                                            ID = OpenKneeboardPluginTabId + ";tab-notes",
                                            Name = "Tab NOTES",
                                        },
                                        new
                                        {
                                            ID = OpenKneeboardPluginTabId + ";tab-efb",
                                            Name = "Tab EFB",
                                        },
                                    },
                                    Implementation = "WebBrowser",
                                    ImplementationArgs = new
                                    {
                                        URI = GetPrefix(),
                                        InitialSize = new
                                        {
                                            Width = 1050,
                                            Height = 1480,
                                        },
                                    },
                                },
                            },
                        };

                        string json = JsonConvert.SerializeObject(manifest, Formatting.Indented);
                        File.WriteAllText(manifestPath, json, Encoding.UTF8);
                    }
                    catch (Exception ex)
                    {
                        Log.Write("OpenKneeboard plugin manifest update failed: " + ex.Message, Colors.Warning);
                    }
                }

                private static void WriteKeywordsPluginManifest(string manifestPath, string keywordsHtmlPath)
                {
                    try
                    {
                        string folder = Path.GetDirectoryName(manifestPath);
                        if (!string.IsNullOrWhiteSpace(folder))
                        {
                            Directory.CreateDirectory(folder);
                        }

                        string readableVersion = string.IsNullOrWhiteSpace(State.versionstring)
                            ? State.pluginversionnumber
                            : State.versionstring;
                        string semanticVersion = string.IsNullOrWhiteSpace(State.pluginversionnumber)
                            ? "3.1.0"
                            : State.pluginversionnumber;

                        string targetHtmlPath = keywordsHtmlPath;
                        if (string.IsNullOrWhiteSpace(targetHtmlPath))
                        {
                            string appsRoot = string.IsNullOrWhiteSpace(State.VA_APPS)
                                ? (State.Proxy == null ? "" : Convert.ToString(State.Proxy.SessionState["VA_APPS"]))
                                : State.VA_APPS;
                            if (!string.IsNullOrWhiteSpace(appsRoot))
                            {
                                targetHtmlPath = Path.Combine(appsRoot, Products.Products.Families.Vaicom.VaicomProPlugin.rootfoldername, AppData.SubFolders["export"], "keywords.html");
                            }
                        }

                        string uri = "";
                        if (!string.IsNullOrWhiteSpace(targetHtmlPath))
                        {
                            uri = new Uri(targetHtmlPath).AbsoluteUri;
                            uri = uri + (uri.IndexOf("?", StringComparison.Ordinal) >= 0 ? "&" : "?") + "okb=1";
                        }

                        var manifest = new
                        {
                            ID = OpenKneeboardKeywordsPluginId,
                            Metadata = new
                            {
                                PluginName = "VAICOM Keywords",
                                PluginReadableVersion = readableVersion,
                                PluginSemanticVersion = semanticVersion,
                                OKBMinimumVersion = "1.9",
                                Author = "VAICOM Community",
                                Website = "https://github.com/Penecruz/VAICOM-Community",
                            },
                            TabTypes = new[]
                            {
                                new
                                {
                                    ID = OpenKneeboardKeywordsPluginTabId,
                                    Name = "VAICOM Keywords",
                                    Implementation = "WebBrowser",
                                    ImplementationArgs = new
                                    {
                                        URI = uri,
                                        InitialSize = new
                                        {
                                            Width = 1050,
                                            Height = 1480,
                                        },
                                    },
                                },
                            },
                        };

                        string json = JsonConvert.SerializeObject(manifest, Formatting.Indented);
                        File.WriteAllText(manifestPath, json, Encoding.UTF8);
                    }
                    catch (Exception ex)
                    {
                        Log.Write("OpenKneeboard keywords plugin manifest update failed: " + ex.Message, Colors.Warning);
                    }
                }

                public static void ResetSnapshot()
                {
                    lock (Sync)
                    {
                        snapshot = new OpenKneeboardSnapshot();
                    }

                    RefreshDtcFilesSnapshot();
                }

                public static void UpdateActiveCategory(string category)
                {
                    if (string.IsNullOrWhiteSpace(category))
                    {
                        return;
                    }

                    string sourceCategory = category.Trim();
                    string sendCategory = ResolveOpenKneeboardCategory(sourceCategory);

                    lock (Sync)
                    {
                        snapshot.ActiveCategory = sendCategory.ToUpperInvariant();
                        snapshot.UpdatedUtc = DateTime.UtcNow;
                    }

                    RefreshOpenKneeboardCategoryData(sourceCategory, sendCategory);
                }

                private static string ResolveOpenKneeboardCategory(string category)
                {
                    string cat = (category ?? string.Empty).Trim();
                    if (string.IsNullOrWhiteSpace(cat)) return "LOG";

                    if (cat.Equals("Crew", StringComparison.OrdinalIgnoreCase))
                    {
                        return "REF";
                    }

                    if (cat.Equals("FLIGHT", StringComparison.OrdinalIgnoreCase) || cat.Equals("Allies", StringComparison.OrdinalIgnoreCase))
                    {
                        return "Flight";
                    }

                    if (cat.IndexOf("ATC", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        return "ATC";
                    }

                    if (cat.IndexOf("AWACS", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        return "AWACS";
                    }

                    if (cat.IndexOf("TANK", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        return "Tanker";
                    }

                    if (cat.IndexOf("JTAC", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        return "JTAC";
                    }

                    return cat;
                }

                private static void RefreshOpenKneeboardCategoryData(string sourceCategory, string sendCategory)
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(sendCategory)) return;

                        if (!sendCategory.Equals("NOTES", StringComparison.OrdinalIgnoreCase)
                            && !sendCategory.Equals("LOG", StringComparison.OrdinalIgnoreCase))
                        {
                            if (!sendCategory.Equals("REF", StringComparison.OrdinalIgnoreCase))
                            {
                                RefreshUnitsForCategory(sendCategory);
                            }
                            else if (sourceCategory.Equals("Crew", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    KneeboardUnitsData crewUnits = new KneeboardUnitsData("Crew", false);
                                    UpdateUnits("CREW", crewUnits.unitslist);
                                }
                                catch
                                {
                                }
                            }

                            if (State.KneeboardCatAliasStrings != null)
                            {
                                for (int i = 0; i < State.KneeboardCatAliasStrings.Length; i++)
                                {
                                    Dictionary<string, SortedDictionary<string, List<string>>> chunk = State.KneeboardCatAliasStrings[i];
                                    SortedDictionary<string, List<string>> aliasStrings = new SortedDictionary<string, List<string>>();

                                    if (chunk != null)
                                    {
                                        SortedDictionary<string, List<string>> fromSource;
                                        SortedDictionary<string, List<string>> fromSend;
                                        if (TryGetAliasChunkCategory(chunk, sourceCategory, out fromSource))
                                        {
                                            aliasStrings = fromSource;
                                        }
                                        else if (TryGetAliasChunkCategory(chunk, sendCategory, out fromSend))
                                        {
                                            aliasStrings = fromSend;
                                        }
                                    }

                                    UpdateAliasChunk(sendCategory, i, aliasStrings);
                                }
                            }
                        }
                    }
                    catch
                    {
                    }
                }

                private static bool TryGetAliasChunkCategory(Dictionary<string, SortedDictionary<string, List<string>>> chunk, string category, out SortedDictionary<string, List<string>> aliasStrings)
                {
                    aliasStrings = null;
                    if (chunk == null || string.IsNullOrWhiteSpace(category)) return false;

                    if (chunk.TryGetValue(category, out aliasStrings))
                    {
                        return aliasStrings != null;
                    }

                    foreach (KeyValuePair<string, SortedDictionary<string, List<string>>> entry in chunk)
                    {
                        if (entry.Key.Equals(category, StringComparison.OrdinalIgnoreCase))
                        {
                            aliasStrings = entry.Value;
                            return aliasStrings != null;
                        }
                    }

                    return false;
                }

                public static void UpdateLog(string category, string content)
                {
                    if (string.IsNullOrWhiteSpace(category))
                    {
                        return;
                    }

                    lock (Sync)
                    {
                        string key = category.ToUpperInvariant();
                        string nextEntry = NormalizeLogEntry(content);

                        if (string.IsNullOrWhiteSpace(nextEntry))
                        {
                            return;
                        }

                        List<string> entries = new List<string>();
                        if (snapshot.Logs.TryGetValue(key, out string existing) && !string.IsNullOrWhiteSpace(existing))
                        {
                            entries.AddRange(existing
                                .Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(e => e.Trim())
                                .Where(e => !string.IsNullOrWhiteSpace(e)));
                        }

                        entries.Add(nextEntry);
                        if (entries.Count > 4)
                        {
                            if (!key.Equals("NOTES", StringComparison.OrdinalIgnoreCase))
                            {
                                entries = entries.Skip(entries.Count - 4).ToList();
                            }
                        }

                        snapshot.Logs[key] = string.Join("\n", entries);
                        snapshot.UpdatedUtc = DateTime.UtcNow;
                    }
                }

                public static void SetLastAiCrewCommand(string commandText)
                {
                    lock (Sync)
                    {
                        lastAiCrewCommand = NormalizeLogEntry(commandText);
                        snapshot.AiCrewPhase = InferF4EAiCrewPhase(lastAiCrewCommand, snapshot.AiCrewPhase);
                        snapshot.UpdatedUtc = DateTime.UtcNow;
                    }
                }

                private static string InferF4EAiCrewPhase(string commandText, string currentPhase)
                {
                    try
                    {
                        string moduleId = State.currentmodule == null ? "" : (State.currentmodule.Id ?? "");
                        if (!moduleId.Equals("F-4E-45MC", StringComparison.OrdinalIgnoreCase))
                        {
                            return "Unknown";
                        }

                        string text = (commandText ?? "").ToLowerInvariant();
                        if (string.IsNullOrWhiteSpace(text))
                        {
                            return string.IsNullOrWhiteSpace(currentPhase) ? "Unknown" : currentPhase;
                        }

                        bool hasAny(params string[] terms)
                        {
                            return terms.Any(t => text.IndexOf(t, StringComparison.Ordinal) >= 0);
                        }

                        if (hasAny("divert", "fuel"))
                        {
                            return "Divert/Low Fuel";
                        }

                        if (hasAny("below 50", "below 100", "below 150", "below 200", "approach", "landing"))
                        {
                            return "Approach/Landing";
                        }

                        if (hasAny(
                            "lock target",
                            "focus target",
                            "unlock target",
                            "pave spike",
                            "tv weapons",
                            "countermeasures",
                            "chaff mode",
                            "flare mode",
                            "jammer",
                            "flares jettison",
                            "context select",
                            "context hold",
                            "context double"))
                        {
                            return "Fence/Target";
                        }

                        if (hasAny(
                            "tacan",
                            "waypoint",
                            "flight plan",
                            "resume",
                            "hold",
                            "tune radio",
                            "select mode",
                            "auto focus",
                            "radar",
                            "iff",
                            "boresight",
                            "scan"))
                        {
                            return "Enroute";
                        }

                        bool groundOperation = hasAny(
                            "ground ",
                            "chocks",
                            "power",
                            "air connect",
                            "air disconnect",
                            "air on",
                            "air off",
                            "ladder",
                            "steps",
                            "pitot check",
                            "spoilers check",
                            "flight controls check",
                            "trim check",
                            "start alignment",
                            "alignment");

                        if (groundOperation)
                        {
                            if (string.Equals(currentPhase, "Enroute", StringComparison.OrdinalIgnoreCase)
                                || string.Equals(currentPhase, "Fence/Target", StringComparison.OrdinalIgnoreCase)
                                || string.Equals(currentPhase, "Approach/Landing", StringComparison.OrdinalIgnoreCase)
                                || string.Equals(currentPhase, "Divert/Low Fuel", StringComparison.OrdinalIgnoreCase))
                            {
                                return "Taxi In/Shutdown";
                            }

                            return "Startup and Taxi";
                        }

                        return string.IsNullOrWhiteSpace(currentPhase) ? "Unknown" : currentPhase;
                    }
                    catch
                    {
                        return string.IsNullOrWhiteSpace(currentPhase) ? "Unknown" : currentPhase;
                    }
                }

                public static string BuildAiCrewResponseEntry(string role, string response)
                {
                    lock (Sync)
                    {
                        string responseText = NormalizeLogEntry(response);
                        if (string.IsNullOrWhiteSpace(responseText))
                        {
                            return "";
                        }

                        string commandText = NormalizeLogEntry(lastAiCrewCommand);
                        if (!string.IsNullOrWhiteSpace(commandText))
                        {
                            return commandText + " -> " + responseText;
                        }

                        string roleText = string.IsNullOrWhiteSpace(role) ? "AI CREW" : role;
                        return roleText + " | " + responseText;
                    }
                }

                private static string NormalizeLogEntry(string content)
                {
                    if (string.IsNullOrWhiteSpace(content))
                    {
                        return "";
                    }

                    return content
                        .Replace("\r", " ")
                        .Replace("\n", " ")
                        .Trim();
                }

                public static void UpdateUnits(string category, List<string> units)
                {
                    if (string.IsNullOrWhiteSpace(category))
                    {
                        return;
                    }

                    lock (Sync)
                    {
                        snapshot.Units[category.ToUpperInvariant()] = units == null ? new List<string>() : new List<string>(units);
                        snapshot.UpdatedUtc = DateTime.UtcNow;
                    }
                }

                public static void UpdateUnitsDetails(string category, List<string> details)
                {
                    if (string.IsNullOrWhiteSpace(category))
                    {
                        return;
                    }

                    lock (Sync)
                    {
                        snapshot.UnitDetails[category.ToUpperInvariant()] = details == null ? new List<string>() : new List<string>(details);
                        snapshot.UpdatedUtc = DateTime.UtcNow;
                    }
                }

                public static void UpdateAliasChunk(string category, int chunk, SortedDictionary<string, List<string>> aliasStrings)
                {
                    if (string.IsNullOrWhiteSpace(category))
                    {
                        return;
                    }

                    lock (Sync)
                    {
                        Dictionary<string, Dictionary<string, List<string>>> chunkMap = chunk == 0 ? snapshot.AliasesChunk0 : snapshot.AliasesChunk1;
                        chunkMap[category.ToUpperInvariant()] = CloneAliasDictionary(aliasStrings);
                        snapshot.UpdatedUtc = DateTime.UtcNow;
                    }
                }

                public static void UpdateNotesBuffer(string notes)
                {
                    lock (Sync)
                    {
                        snapshot.NotesBuffer = notes ?? "";
                        snapshot.UpdatedUtc = DateTime.UtcNow;
                    }
                }

                public static void UpdateServerData()
                {
                    lock (Sync)
                    {
                        snapshot.Server = BuildServerSnapshot();
                        snapshot.AiCrewKeywords = BuildAiCrewKeywords();
                        snapshot.AiCrewKeywordSections = BuildAiCrewKeywordSections();
                        snapshot.UpdatedUtc = DateTime.UtcNow;
                    }
                }

                public static void ForceRefreshOutDashboardData()
                {
                    UpdateServerData();
                    RefreshUnitsForCategory("ATC");
                    RefreshUnitsForCategory("AWACS");
                    RefreshUnitsForCategory("Tanker");
                }

                public static void ForceRefreshFriendlyAssetsData()
                {
                    UpdateServerData();
                }

                private static void RefreshUnitsForCategory(string category)
                {
                    if (string.IsNullOrWhiteSpace(category))
                    {
                        return;
                    }

                    try
                    {
                        KneeboardUnitsData units = new KneeboardUnitsData(category, false);
                        UpdateUnits(category, units.unitslist);
                    }
                    catch
                    {
                    }
                }

                private static readonly string[] F14AiCrewKeywordSectionOrder = new[]
                {
                    "Startup and Shutdown",
                    "Radio",
                    "Radar",
                    "Utility/Navigation",
                    "Weapons",
                    "LANTIRN",
                    "Defensive/Countermeasures",
                    "Datalink",
                    "TACAN",
                    "Walkman",
                    "Crew Contract",
                    "Miscellaneous",
                    "Supercarriers",
                    "AI Pilot",
                };

                private static readonly HashSet<string> F14SupercarrierCommandIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                {
                    "wMsgJ_RAD_DL_HOST_WASH",
                    "wMsgJ_RAD_DL_HOST_ROOS",
                    "wMsgJ_RAD_DL_HOST_LINC",
                    "wMsgJ_RAD_DL_HOST_TRUM",
                    "wMsgJ_RAD_DL_HOST_TICO",
                    "wMsgJ_RAD_DL_HOST_FORE",
                    "wMsgJ_RAD_DL_HOST_BURK",
                    "wMsgJ_RAD_TCN_TAC_WASH",
                    "wMsgJ_RAD_TCN_TAC_ROOS",
                    "wMsgJ_RAD_TCN_TAC_LINC",
                    "wMsgJ_RAD_TCN_TAC_TRUM",
                    "wMsgJ_RAD_TCN_TAC_FORE",
                };

                private static Dictionary<string, List<string>> BuildAiCrewKeywordSections()
                {
                    try
                    {
                        string moduleId = State.currentmodule?.Id ?? string.Empty;
                        if (moduleId.StartsWith("F-14", StringComparison.OrdinalIgnoreCase)
                            || moduleId.StartsWith("F14", StringComparison.OrdinalIgnoreCase)
                            || moduleId.IndexOf("F-14", StringComparison.OrdinalIgnoreCase) >= 0
                            || moduleId.IndexOf("F14", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            return BuildF14AiCrewKeywordSections();
                        }
                    }
                    catch
                    {
                    }

                    return new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
                }

                private static Dictionary<string, List<string>> BuildF14AiCrewKeywordSections()
                {
                    Dictionary<string, SortedSet<string>> buckets = new Dictionary<string, SortedSet<string>>(StringComparer.OrdinalIgnoreCase);
                    foreach (string section in F14AiCrewKeywordSectionOrder)
                    {
                        buckets[section] = new SortedSet<string>(StringComparer.OrdinalIgnoreCase);
                    }

                    foreach (KeyValuePair<string, string> alias in Extensions.RIO.Aliases.aicommands)
                    {
                        string phrase = (alias.Key ?? string.Empty).Trim();
                        string commandId = alias.Value ?? string.Empty;
                        if (string.IsNullOrWhiteSpace(phrase) || string.IsNullOrWhiteSpace(commandId))
                        {
                            continue;
                        }

                        string section = ClassifyF14AiCrewKeywordSection(commandId);
                        if (string.IsNullOrWhiteSpace(section))
                        {
                            continue;
                        }

                        if (buckets.TryGetValue(section, out SortedSet<string> set))
                        {
                            set.Add(phrase);
                        }
                    }

                    foreach (KeyValuePair<string, IEnumerable<string>> extra in GetF14ManualAiCrewKeywordSections())
                    {
                        if (string.IsNullOrWhiteSpace(extra.Key) || extra.Value == null)
                        {
                            continue;
                        }

                        if (!buckets.TryGetValue(extra.Key, out SortedSet<string> set))
                        {
                            continue;
                        }

                        foreach (string keyword in extra.Value)
                        {
                            if (!string.IsNullOrWhiteSpace(keyword))
                            {
                                set.Add(keyword.Trim());
                            }
                        }
                    }

                    Dictionary<string, List<string>> sections = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
                    foreach (string section in F14AiCrewKeywordSectionOrder)
                    {
                        if (!buckets.TryGetValue(section, out SortedSet<string> set) || set.Count == 0)
                        {
                            continue;
                        }

                        sections[section] = set.ToList();
                    }

                    return sections;
                }

                private static Dictionary<string, IEnumerable<string>> GetF14ManualAiCrewKeywordSections()
                {
                    return new Dictionary<string, IEnumerable<string>>(StringComparer.OrdinalIgnoreCase)
                    {
                        {
                            "Radio",
                            new[]
                            {
                                "Radio Frequency [0..3] [0..9] [0..9] [Point; Decimal] [0..9] [0; 2 5; 5 0; 7 5]"
                            }
                        },
                        {
                            "Datalink",
                            new[]
                            {
                                "Link Tune [0..9] [0..9] [Point; Decimal] [0..9]"
                            }
                        },
                        {
                            "TACAN",
                            new[]
                            {
                                "TACAN Tune [X-Ray; Yankee] [0..1] [0..9] [0..9]"
                            }
                        },
                        {
                            "Radar",
                            new[]
                            {
                                "Scan Range [0..30,5] [Angels; on] [the deck; 1..4,5]"
                            }
                        },
                        {
                            "LANTIRN",
                            new[]
                            {
                                "Track Marker [0..10]",
                                "Laser Code [5..7] [1..8] [1..8]"
                            }
                        },
                        {
                            "Weapons",
                            new[]
                            {
                                "Pre Planned [1..8] To Station [3..6]"
                            }
                        },
                        {
                            "Utility/Navigation",
                            new[]
                            {
                                "Map Marker [0..10] to [Waypoint 1; Waypoint 2; Waypoint 3; Steerpoint]",
                                "Map Marker [0..10] to Grid",
                                "[Fly; Orbit] Marker [0..10]",
                                "Direct [Waypoint; Steerpoint] [1..50]"
                            }
                        }
                    };
                }

                private static string ClassifyF14AiCrewKeywordSection(string commandId)
                {
                    if (string.IsNullOrWhiteSpace(commandId))
                    {
                        return null;
                    }

                    if (commandId.StartsWith("wMsgJ_RDR_", StringComparison.OrdinalIgnoreCase))
                    {
                        return "Radar";
                    }

                    if (commandId.StartsWith("wMsgJ_LAN_", StringComparison.OrdinalIgnoreCase)
                        || commandId.StartsWith("wMsgLANTIRN_", StringComparison.OrdinalIgnoreCase))
                    {
                        return "LANTIRN";
                    }

                    if (commandId.StartsWith("wMsgJ_WPN_", StringComparison.OrdinalIgnoreCase))
                    {
                        return "Weapons";
                    }

                    if (commandId.StartsWith("wMsgJ_RAD_182_", StringComparison.OrdinalIgnoreCase))
                    {
                        return "Radio";
                    }

                    if (F14SupercarrierCommandIds.Contains(commandId))
                    {
                        return "Supercarriers";
                    }

                    if (commandId.StartsWith("wMsgJ_RAD_DL_", StringComparison.OrdinalIgnoreCase))
                    {
                        return "Datalink";
                    }

                    if (commandId.StartsWith("wMsgJ_RAD_TCN_", StringComparison.OrdinalIgnoreCase))
                    {
                        return "TACAN";
                    }

                    if (commandId.StartsWith("wMsgJ_UTIL_NAV_", StringComparison.OrdinalIgnoreCase))
                    {
                        return "Utility/Navigation";
                    }

                    if (commandId.StartsWith("wMsgJ_WLKMN_", StringComparison.OrdinalIgnoreCase))
                    {
                        return "Walkman";
                    }

                    if (commandId.StartsWith("wMsgJ_UTIL_CONTR_", StringComparison.OrdinalIgnoreCase)
                        || commandId.Equals("wMsgJ_RESET", StringComparison.OrdinalIgnoreCase))
                    {
                        return "Crew Contract";
                    }

                    if (commandId.StartsWith("wMsgJ_DEF_", StringComparison.OrdinalIgnoreCase))
                    {
                        return "Defensive/Countermeasures";
                    }

                    if (commandId.StartsWith("wMsgJ_STRT_", StringComparison.OrdinalIgnoreCase))
                    {
                        return "Startup and Shutdown";
                    }

                    if (commandId.StartsWith("wMsgJ_SDWN_", StringComparison.OrdinalIgnoreCase))
                    {
                        return "Startup and Shutdown";
                    }

                    if (commandId.StartsWith("wMsgJ_CANOPY_", StringComparison.OrdinalIgnoreCase)
                        || commandId.StartsWith("wMsgJ_MENU_", StringComparison.OrdinalIgnoreCase)
                        || commandId.StartsWith("wMsgJ_UTIL_", StringComparison.OrdinalIgnoreCase))
                    {
                        return "Miscellaneous";
                    }

                    if (commandId.StartsWith("wMsgI_", StringComparison.OrdinalIgnoreCase))
                    {
                        return "AI Pilot";
                    }

                    return null;
                }

                private static List<string> BuildAiCrewKeywords()
                {
                    try
                    {
                        string moduleId = State.currentmodule?.Id ?? string.Empty;
                        HashSet<string> keywords = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                        if (moduleId.StartsWith("F-14", StringComparison.OrdinalIgnoreCase)
                            || moduleId.StartsWith("F14", StringComparison.OrdinalIgnoreCase)
                            || moduleId.IndexOf("F-14", StringComparison.OrdinalIgnoreCase) >= 0
                            || moduleId.IndexOf("F14", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            Dictionary<string, List<string>> sections = BuildF14AiCrewKeywordSections();
                            foreach (string sectionName in F14AiCrewKeywordSectionOrder)
                            {
                                if (!sections.TryGetValue(sectionName, out List<string> sectionKeywords))
                                {
                                    continue;
                                }

                                foreach (string key in sectionKeywords)
                                {
                                    keywords.Add(key);
                                }
                            }
                        }
                        else if (moduleId.Equals("F-4E-45MC", StringComparison.OrdinalIgnoreCase))
                        {
                            foreach (KeyValuePair<string, string> alias in Database.Aliases.aicommands)
                            {
                                if (alias.Value != null && alias.Value.StartsWith("wMsgWSO_", StringComparison.OrdinalIgnoreCase))
                                {
                                    keywords.Add(alias.Key);
                                }
                            }

                            foreach (string key in GetF4EManualAiCrewKeywords())
                            {
                                keywords.Add(key);
                            }
                        }
                        else if (moduleId.StartsWith("AH-64D", StringComparison.OrdinalIgnoreCase))
                        {
                            foreach (string key in Database.Aliases.aicommands.Keys)
                            {
                                if (key.StartsWith("George ", StringComparison.OrdinalIgnoreCase))
                                {
                                    keywords.Add(key);
                                }
                            }
                        }

                        return keywords
                            .Where(k => !string.IsNullOrWhiteSpace(k))
                            .OrderBy(k => k, StringComparer.OrdinalIgnoreCase)
                            .ToList();
                    }
                    catch
                    {
                        return new List<string>();
                    }
                }

                private static IEnumerable<string> GetF4EManualAiCrewKeywords()
                {
                    return new[]
                    {
                        "Hold at [Flight plan 1; Primary flight plan; Flight plan 2; Secondary flight plan] waypoint [1..9]",
                        "Go to [Flight Plan; Flight plan 1; Primary flight plan; Flight plan 2; Secondary flight plan] [at] waypoint [1..9]",
                        "Resume [Flight Plan; Flight plan 1; Primary flight plan; Flight plan 2; Secondary flight plan] [at] waypoint [1..9]",
                        "Resume At Waypoint [1..9]",
                        "Hold at current Waypoint",
                        "Designate [Flight plan 1; Primary flight plan; Flight plan 2; Secondary flight plan] waypoint [1..9] as [Turn point; Nav fix; Navigation fix; Target; CAP; I P; Inbound point; Fence in; Fence out; Alternate; Homebase]",
                        "Divert to <Airfield;Asset>",
                        "Tune Radio [to] <Airfield;Asset>",
                        "Tune/Set Radio [Frequency] [2..3] [0..9] [0..9] decimal [0..9] [0;25;50;75]",
                        "Push/Select [Comm;Radio] [Button;Channel] [1..18]",
                        "Push/Select [Aux] [Button;Channel] [1..20]",
                        "Tune TACAN <asset name>",
                        "Tune TACAN station [Alpha-Zulu] [Alpha-Zulu] [Alpha-Zulu]",
                        "Set/Select TACAN [channel] [zero;0;1] [0..9] [0..9] [X-ray;Yankee]",
                        "Radar Focus Target [1..20]",
                        "Radar Lock Target [1..20]",
                        "On Station For [15; 30; 45; 60] Minutes"
                    };
                }

                private static void StartWebHost()
                {
                    if (isRunning || !State.activeconfig.OpenKneeboard_Out)
                    {
                        return;
                    }

                    try
                    {
                        string prefix = GetPrefix();
                        listener = new HttpListener();
                        listener.Prefixes.Add(prefix);
                        listener.Start();

                        Interlocked.Exchange(ref lastClientRequestUtcTicks, 0);
                        isRunning = true;
                        listenerThread = new Thread(ListenLoop) { IsBackground = true, Name = "OpenKneeboardWebHost" };
                        listenerThread.Start();

                        try
                        {
                            State.configurationwindow?.Dispatcher.BeginInvoke((Action)delegate
                            {
                                State.configurationwindow.ChangeOKHostbug();
                            });
                        }
                        catch
                        {
                        }

                        Log.Write("OpenKneeboard dashboard host started at " + prefix, Colors.Text);
                    }
                    catch (Exception ex)
                    {
                        isRunning = false;
                        Log.Write("OpenKneeboard dashboard host failed to start: " + ex.Message, Colors.Warning);
                    }
                }

                private static void StopWebHost()
                {
                    if (!isRunning)
                    {
                        return;
                    }

                    try
                    {
                        isRunning = false;
                        listener?.Stop();
                        listener?.Close();
                        listener = null;

                        try
                        {
                            State.configurationwindow?.Dispatcher.BeginInvoke((Action)delegate
                            {
                                State.configurationwindow.ChangeOKHostbug();
                            });
                        }
                        catch
                        {
                        }
                    }
                    catch
                    {
                    }
                }

                private static void ListenLoop()
                {
                    while (isRunning && listener != null)
                    {
                        try
                        {
                            HttpListenerContext context = listener.GetContext();
                            Task.Run(async () =>
                            {
                                await RequestDispatchLimiter.WaitAsync().ConfigureAwait(false);
                                try
                                {
                                    HandleContext(context);
                                }
                                catch (Exception ex)
                                {
                                    try
                                    {
                                        string msg = ex == null ? "" : (ex.Message ?? "");
                                        if (msg.IndexOf("The specified network name is no longer available", StringComparison.OrdinalIgnoreCase) < 0)
                                        {
                                            Log.Write("OpenKneeboard dashboard request worker error: " + msg, Colors.Warning);
                                        }
                                    }
                                    catch
                                    {
                                    }

                                    try
                                    {
                                        context.Response.StatusCode = 500;
                                        context.Response.Close();
                                    }
                                    catch
                                    {
                                    }
                                }
                                finally
                                {
                                    RequestDispatchLimiter.Release();
                                }
                            });
                        }
                        catch (HttpListenerException)
                        {
                            break;
                        }
                        catch (ObjectDisposedException)
                        {
                            break;
                        }
                        catch (Exception ex)
                        {
                            Log.Write("OpenKneeboard dashboard request error: " + ex.Message, Colors.Warning);
                        }
                    }
                }

                private static void HandleContext(HttpListenerContext context)
                {
                    Interlocked.Exchange(ref lastClientRequestUtcTicks, DateTime.UtcNow.Ticks);
                    string path = context.Request.Url.AbsolutePath.ToLowerInvariant();

                    if (path == "/okb")
                    {
                        context.Response.StatusCode = 302;
                        context.Response.RedirectLocation = "/okb/";
                        context.Response.Close();
                        return;
                    }

                    if (path == "/okb/" || path == "/okb/index.html")
                    {
                        WriteText(context.Response, GetIndexHtmlWithStoreLookup(), "text/html; charset=utf-8");
                        return;
                    }

                    if (path == "/okb/state" || path == "/okb/index.json")
                    {
                        // Update the selected category data and associated units if the tab has been changed to display a different category.
                        string selectedTab = context.Request.QueryString["selectedTab"] ?? "";
                        if (!currentSelectedTab.Equals(selectedTab, StringComparison.OrdinalIgnoreCase))
                        {
                            currentSelectedTab = selectedTab;
                            // If we have just switched to the flight plan tab then do an
                            // immediate server update request so we have the latest data available.
                            if (IsFlightPlanTabSelected())
                            {
                                DcsClient.SendUpdateRequest();
                            }
                            else
                            {
                                UpdateActiveCategory(currentSelectedTab);
                            }
                        }

                        WriteJson(context.Response, BuildSnapshotJson());
                        return;
                    }

                    if (path == "/okb/clock")
                    {
                        WriteJson(context.Response, BuildClockJson());
                        return;
                    }

                    if (path == "/okb/sa/ownship")
                    {
                        WriteJson(context.Response, BuildOwnshipJson());
                        return;
                    }

                    if (path == "/okb/dev/servermessages")
                    {
                        bool enable = string.Equals(context.Request.QueryString["enabled"], "1", StringComparison.OrdinalIgnoreCase)
                            || string.Equals(context.Request.QueryString["enabled"], "true", StringComparison.OrdinalIgnoreCase);

                        SetRawServerCaptureEnabled(enable);
                        WriteJson(context.Response, "{\"ok\":true}");
                        return;
                    }

                    if (path == "/okb/dev/refresh")
                    {
                        ForceRefreshOutDashboardData();
                        WriteJson(context.Response, "{\"ok\":true}");
                        return;
                    }

                    if (path == "/okb/dtc/list")
                    {
                        RefreshDtcFilesSnapshot();
                        WriteJson(context.Response, BuildSnapshotJson());
                        return;
                    }

                    if (path == "/okb/dtc/select")
                    {
                        string file = context.Request.QueryString["file"] ?? "";
                        bool ok = SelectDtcFileSnapshot(file);
                        if (!ok)
                        {
                            context.Response.StatusCode = 400;
                        }

                        WriteJson(context.Response, BuildSnapshotJson());
                        return;
                    }

                    if (path == "/okb/logo.png")
                    {
                        byte[] logo = GetLogoBytes();
                        if (logo != null && logo.Length > 0)
                        {
                            WriteBinary(context.Response, logo, "image/png");
                        }
                        else
                        {
                            context.Response.StatusCode = 404;
                            context.Response.Close();
                        }
                        return;
                    }

                    if (path == "/okb/efb/charts")
                    {
                        string airport = context.Request.QueryString["airport"] ?? "";
                        // If navigraph is enabled and auth present, try navigraph charts first
                        bool useNavigraph = false;
                        try
                        {
                            string authPayload;
                            useNavigraph = OpenKneeboardNavigraphEfbState.TryGetDecryptedAuthBlob(out authPayload) && !string.IsNullOrWhiteSpace(authPayload);
                        }
                        catch { useNavigraph = false; }

                        string normalizedAirport = (airport ?? "").Trim().ToUpperInvariant();
                        bool forceNavigraphOnlyForAirport = useNavigraph && string.Equals(normalizedAirport, "PGUA", StringComparison.Ordinal);

                        if (useNavigraph)
                        {
                            // Try fetching navigraph chart catalog via API helper; fall back to local charts on error
                            try
                            {
                                var navJson = OpenKneeboardNavigraphApiProxy.BuildChartsJsonFallback(airport);
                                if (!string.IsNullOrWhiteSpace(navJson))
                                {
                                    WriteJson(context.Response, navJson);
                                    return;
                                }
                            }
                            catch
                            {
                                // fall through to local
                            }

                            if (forceNavigraphOnlyForAirport)
                            {
                                try { Log.Write("Navigraph charts lookup for PGUA returned no data; local Saved Games fallback is disabled while Navigraph auth is active.", VAICOM.Static.Colors.Warning); } catch { }
                                WriteJson(context.Response, "{\"charts\":[]}");
                                return;
                            }
                        }

                        WriteJson(context.Response, BuildEfbChartsJson(airport));
                        return;
                    }

                    if (path == "/okb/efb/airports")
                    {
                        WriteJson(context.Response, BuildEfbAirportsJson());
                        return;
                    }

                    if (path == "/okb/efb/chart")
                    {
                        string chartId = context.Request.QueryString["id"] ?? "";
                        // If navigraph auth is present and chartId indicates navigraph provider, proxy via Navigraph API
                        bool navAuth = false;
                        try { string auth; navAuth = OpenKneeboardNavigraphEfbState.TryGetDecryptedAuthBlob(out auth) && !string.IsNullOrWhiteSpace(auth); } catch { navAuth = false; }
                        if (navAuth && chartId.StartsWith("nav:", StringComparison.OrdinalIgnoreCase))
                        {
                            try
                            {
                                var parts = chartId.Substring(4).Split(':'); // nav:chartId:tilez:tilex:tiley or nav:chartId
                                if (parts.Length >= 1)
                                {
                                    string navChartId = parts[0];
                                    byte[] tileBytes = null;
                                    string contentType = "image/png";
                                    if (parts.Length == 4)
                                    {
                                        int z = int.Parse(parts[1]);
                                        int x = int.Parse(parts[2]);
                                        int y = int.Parse(parts[3]);
                                        tileBytes = OpenKneeboardNavigraphApiProxy.GetChartTileBytes(navChartId, z, x, y);
                                    }
                                    else
                                    {
                                        bool useNight = false;
                                        string mode = context.Request.QueryString["mode"] ?? "";
                                        if (string.Equals(mode, "night", StringComparison.OrdinalIgnoreCase))
                                        {
                                            useNight = true;
                                        }
                                        tileBytes = OpenKneeboardNavigraphApiProxy.GetChartImageBytes(navChartId, useNight);
                                    }

                                    if (tileBytes != null && tileBytes.Length > 0)
                                    {
                                        WriteBinary(context.Response, tileBytes, contentType);
                                        return;
                                    }
                                }
                            }
                            catch
                            {
                                // fallback to local chart
                            }
                        }

                        string contentTypeLocal;
                        byte[] chartBytesLocal = ReadEfbChartBytes(chartId, out contentTypeLocal);
                        if (chartBytesLocal == null || chartBytesLocal.Length == 0)
                        {
                            context.Response.StatusCode = 404;
                            context.Response.Close();
                            return;
                        }
                        WriteBinary(context.Response, chartBytesLocal, contentTypeLocal);
                        return;
                    }

                    if (path == "/okb/efb/navtile")
                    {
                        int z = 0;
                        int x = 0;
                        int y = 0;
                        bool okZ = int.TryParse(context.Request.QueryString["z"] ?? "", out z);
                        bool okX = int.TryParse(context.Request.QueryString["x"] ?? "", out x);
                        bool okY = int.TryParse(context.Request.QueryString["y"] ?? "", out y);
                        string mode = (context.Request.QueryString["mode"] ?? "").Trim();
                        string layer = (context.Request.QueryString["layer"] ?? "").Trim();
                        bool useNight = string.Equals(mode, "night", StringComparison.OrdinalIgnoreCase);

                        if (!okZ || !okX || !okY || z < 0 || x < 0 || y < 0)
                        {
                            context.Response.StatusCode = 400;
                            WriteJson(context.Response, "{\"error\":\"invalid_tile_coordinates\"}");
                            return;
                        }

                        byte[] bytes = null;
                        try
                        {
                            bytes = OpenKneeboardNavigraphApiProxy.GetEnrouteVfrTileBytes(z, x, y, useNight, layer);
                        }
                        catch
                        {
                            bytes = null;
                        }

                        if (bytes == null || bytes.Length == 0)
                        {
                            context.Response.StatusCode = 404;
                            context.Response.Close();
                            return;
                        }

                        WriteBinary(context.Response, bytes, "image/png");
                        return;
                    }

                    if (path == "/okb/helpdoc")
                    {
                        string tab = context.Request.QueryString["tab"] ?? "";
                        string payload = BuildHelpDocTabHtml(tab);
                        if (string.Equals(payload, "<p>Help document not found.</p>", StringComparison.Ordinal))
                        {
                            string resolved = ResolveHelpDocPath();
                            if (!string.IsNullOrWhiteSpace(resolved))
                            {
                                payload = "<p>Help document not found at startup location.</p><p>Resolved candidate: " + WebUtility.HtmlEncode(resolved) + "</p>";
                            }
                        }
                        WriteText(context.Response, payload, "text/html; charset=utf-8");
                        return;
                    }

                    if (path == "/okb/helpimg")
                    {
                        string file = context.Request.QueryString["name"] ?? "";
                        string contentType;
                        byte[] imageBytes = ReadHelpDocImageBytes(file, out contentType);
                        if (imageBytes == null || imageBytes.Length == 0)
                        {
                            context.Response.StatusCode = 404;
                            context.Response.Close();
                            return;
                        }

                        WriteBinary(context.Response, imageBytes, contentType);
                        return;
                    }

                    context.Response.StatusCode = 404;
                    context.Response.Close();
                }

                public static bool IsFlightPlanTabSelected()
                {
                    return !string.IsNullOrEmpty(currentSelectedTab) && currentSelectedTab.Equals("DTC");
                }

                public static bool IsFlightPlanTabActive
                {
                    get
                    {
                        return HasActiveConnection && IsFlightPlanTabSelected();
                    }
                }

                private static string GetIndexHtmlWithStoreLookup()
                {
                    try
                    {
                        return IndexHtml.Replace(StoreLookupJsonPlaceholder, GetStoreLookupMapJson());
                    }
                    catch
                    {
                        return IndexHtml.Replace(StoreLookupJsonPlaceholder, "{}");
                    }
                }

                private static string GetStoreLookupMapJson()
                {
                    lock (StoreLookupSync)
                    {
                        try
                        {
                            string path = ResolveStoreLookupPath();
                            string raw = "";

                            if (!string.IsNullOrWhiteSpace(path) && File.Exists(path))
                            {
                                DateTime lastWrite = File.GetLastWriteTimeUtc(path);
                                bool refresh = !string.Equals(path, storeLookupResolvedPath, StringComparison.OrdinalIgnoreCase)
                                    || lastWrite != storeLookupLastWriteUtc;

                                if (!refresh)
                                {
                                    return storeLookupMapJson;
                                }

                                raw = File.ReadAllText(path);
                                storeLookupResolvedPath = path;
                                storeLookupLastWriteUtc = lastWrite;
                            }
                            else
                            {
                                if (!TryReadEmbeddedStoreLookupJson(out raw))
                                {
                                    return storeLookupMapJson;
                                }

                                storeLookupResolvedPath = "<embedded>";
                                storeLookupLastWriteUtc = DateTime.MinValue;
                            }

                            JObject parsed = JObject.Parse(raw);

                            JToken mapToken = null;
                            JProperty mapProperty = parsed.Properties()
                                .FirstOrDefault(p => string.Equals(p.Name, "map", StringComparison.OrdinalIgnoreCase));

                            if (mapProperty != null && mapProperty.Value != null && mapProperty.Value.Type == JTokenType.Object)
                            {
                                mapToken = mapProperty.Value;
                            }
                            else
                            {
                                mapToken = parsed;
                            }

                            storeLookupMapJson = mapToken.ToString(Formatting.None);

                        }
                        catch
                        {
                            if (string.IsNullOrWhiteSpace(storeLookupMapJson))
                            {
                                storeLookupMapJson = "{}";
                            }
                        }

                        return string.IsNullOrWhiteSpace(storeLookupMapJson) ? "{}" : storeLookupMapJson;
                    }
                }

                private static string ResolveStoreLookupPath()
                {
                    try
                    {
                        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                        if (string.IsNullOrWhiteSpace(baseDir))
                        {
                            return "";
                        }

                        string[] candidates = new[]
                        {
                            Path.Combine(baseDir, "Helpers", "StoreClsidLookup.json"),
                            Path.Combine(baseDir, "StoreClsidLookup.json")
                        };

                        foreach (string candidate in candidates)
                        {
                            if (!string.IsNullOrWhiteSpace(candidate) && File.Exists(candidate))
                            {
                                return candidate;
                            }
                        }
                    }
                    catch
                    {
                    }

                    return "";
                }

                private static bool TryReadEmbeddedStoreLookupJson(out string rawJson)
                {
                    rawJson = "";

                    try
                    {
                        Assembly asm = typeof(OpenKneeboardBridge).Assembly;
                        string resourceName = asm
                            .GetManifestResourceNames()
                            .FirstOrDefault(n => n.EndsWith("Helpers.StoreClsidLookup.json", StringComparison.OrdinalIgnoreCase));

                        if (string.IsNullOrWhiteSpace(resourceName))
                        {
                            return false;
                        }

                        using (Stream stream = asm.GetManifestResourceStream(resourceName))
                        {
                            if (stream == null)
                            {
                                return false;
                            }

                            using (StreamReader reader = new StreamReader(stream))
                            {
                                rawJson = reader.ReadToEnd();
                            }
                        }

                        return !string.IsNullOrWhiteSpace(rawJson);
                    }
                    catch
                    {
                        return false;
                    }
                }

                private static string BuildHelpDocTabHtml(string tab)
                {
                    try
                    {
                        EnsureHelpDocInVaAppsDocumentation();
                        string path = ResolveHelpDocPath();
                        if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
                        {
                            return "<p>Help document not found.</p>";
                        }

                        string html = File.ReadAllText(path);
                        if (string.IsNullOrWhiteSpace(html))
                        {
                            return "<p>Help document is empty.</p>";
                        }

                        string globalSection = ExtractSectionById(html, "global");
                        string tabSection = ExtractSectionById(html, NormalizeHelpDocSectionId(tab));

                        if (string.IsNullOrWhiteSpace(globalSection) && string.IsNullOrWhiteSpace(tabSection))
                        {
                            return "<p>No help content found for this tab.</p>";
                        }

                        string scopedStyle = "<style>"
                            + ".okbHelpDocScope{font-family:'Segoe UI',Tahoma,Arial,sans-serif;color:#14263a !important;line-height:1.45;background:transparent;}"
                            + ".okbHelpDocScope *{writing-mode:horizontal-tb !important;text-orientation:mixed !important;transform:none !important;}"
                            + ".okbHelpDocScope,.okbHelpDocScope p,.okbHelpDocScope li,.okbHelpDocScope span,.okbHelpDocScope div{color:#14263a !important;}"
                            + ".okbHelpDocScope h2{margin:0 0 10px 0;font-size:22px;border-bottom:1px solid #c7d3df;padding-bottom:4px;color:#0f2438 !important;}"
                            + ".okbHelpDocScope h3{margin:12px 0 8px 0;font-size:18px;color:#16324b !important;}"
                            + ".okbHelpDocScope h4,.okbHelpDocScope h5,.okbHelpDocScope h6{margin:10px 0 6px 0;color:#1b3953 !important;}"
                            + ".okbHelpDocScope p{margin:8px 0;}"
                            + ".okbHelpDocScope ul,.okbHelpDocScope ol{margin:6px 0 10px 24px;padding:0;text-align:left !important;list-style-position:outside;}"
                            + ".okbHelpDocScope li{margin:4px 0;}"
                            + ".okbHelpDocScope strong,.okbHelpDocScope b{color:#0f2a43 !important;font-weight:700;}"
                            + ".okbHelpDocScope em,.okbHelpDocScope i{color:#1a3a56 !important;}"
                            + ".okbHelpDocScope code{background:#eaf1f8;border:1px solid #d5e2ef;border-radius:4px;padding:1px 5px;}"
                            + ".okbHelpDocScope pre{background:#eef4fb;border:1px solid #d5e2ef;border-radius:8px;padding:8px;overflow:auto;color:#11283f !important;}"
                            + ".okbHelpDocScope .tab{background:#ffffff;border:1px solid #c7d3df;border-radius:10px;padding:12px;margin-bottom:12px;display:block;}"
                            + ".okbHelpDocScope .controls{display:block !important;border-left:4px solid #9fb8d4;padding-left:10px;margin-top:10px;text-align:left !important;background:rgba(245,249,255,0.62);border-radius:0 6px 6px 0;}"
                            + ".okbHelpDocScope .controls > h3{display:block !important;margin:0 0 6px 0 !important;text-align:left !important;clear:both;}"
                            + ".okbHelpDocScope .controls > ul,.okbHelpDocScope .controls > ol{display:block !important;margin:0 0 12px 28px !important;padding:0 !important;text-align:left !important;clear:both;}"
                            + ".okbHelpDocScope a{color:#1a4b84;text-decoration:underline;font-weight:600;}"
                            + ".okbHelpDocScope table{width:100%;border-collapse:collapse;margin:10px 0;color:#112a42 !important;}"
                            + ".okbHelpDocScope th,.okbHelpDocScope td{border:1px solid #c7d3df;padding:6px 8px;text-align:left;vertical-align:top;}"
                            + ".okbHelpDocScope th{background:#edf3fa;color:#0f2a43 !important;}"
                            + ".okbHelpDocScope hr{border:none;border-top:1px solid #c7d3df;margin:14px 0;}"
                            + ".okbHelpDocScope .helpImage{display:block;max-width:92%;height:auto;margin:12px auto;border:1px solid #c7d3df;border-radius:8px;background:#fff;}"
                            + ".okbHelpDocScope .helpImageCaption{margin:4px auto 12px auto;text-align:center;font-size:14px;color:#3c5166;}"
                            + "body.night-mode .okbHelpDocScope{color:#dfe8f2;}"
                            + "body.night-mode .okbHelpDocScope,body.night-mode .okbHelpDocScope p,body.night-mode .okbHelpDocScope li,body.night-mode .okbHelpDocScope span,body.night-mode .okbHelpDocScope div{color:#dfe8f2 !important;}"
                            + "body.night-mode .okbHelpDocScope .tab{background:rgba(45,57,72,0.62);border-color:#5f7184;}"
                            + "body.night-mode .okbHelpDocScope h2{border-bottom-color:#5f7184;color:#eef5ff !important;}"
                            + "body.night-mode .okbHelpDocScope h3{color:#d7e8ff !important;}"
                            + "body.night-mode .okbHelpDocScope h4,body.night-mode .okbHelpDocScope h5,body.night-mode .okbHelpDocScope h6{color:#cfe1f7 !important;}"
                            + "body.night-mode .okbHelpDocScope strong,body.night-mode .okbHelpDocScope b{color:#f0f7ff !important;}"
                            + "body.night-mode .okbHelpDocScope em,body.night-mode .okbHelpDocScope i{color:#d2e7ff !important;}"
                            + "body.night-mode .okbHelpDocScope code{background:rgba(52,67,84,0.9);border-color:#5f7184;color:#e7eef8;}"
                            + "body.night-mode .okbHelpDocScope pre{background:rgba(41,54,69,0.9);border-color:#5f7184;color:#e7eef8 !important;}"
                            + "body.night-mode .okbHelpDocScope .controls{background:rgba(49,64,81,0.65);border-left-color:#7fa6ce;}"
                            + "body.night-mode .okbHelpDocScope a{color:#9bc8ff;}"
                            + "body.night-mode .okbHelpDocScope table,body.night-mode .okbHelpDocScope th,body.night-mode .okbHelpDocScope td{border-color:#5f7184;color:#e2ebf6 !important;}"
                            + "body.night-mode .okbHelpDocScope th{background:rgba(57,72,90,0.92);color:#f0f7ff !important;}"
                            + "body.night-mode .okbHelpDocScope hr{border-top-color:#5f7184;}"
                            + "body.night-mode .okbHelpDocScope .helpImage{border-color:#5f7184;background:rgba(35,47,61,0.9);}"
                            + "body.night-mode .okbHelpDocScope .helpImageCaption{color:#bfd5ec;}"
                            + "</style>";

                        string merged = string.IsNullOrWhiteSpace(globalSection)
                            ? (tabSection ?? "")
                            : (string.IsNullOrWhiteSpace(tabSection) ? globalSection : (globalSection + "\n" + tabSection));

                        return scopedStyle + "<div class='okbHelpDocScope'>" + merged + "</div>";
                    }
                    catch (Exception ex)
                    {
                        return "<p>Failed to load help content: " + WebUtility.HtmlEncode(ex.Message) + "</p>";
                    }
                }

                private static string ResolveHelpDocPath()
                {
                    try
                    {
                        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                        string asmDir = "";
                        try
                        {
                            string asmPath = typeof(OpenKneeboardBridge).Assembly.Location;
                            asmDir = string.IsNullOrWhiteSpace(asmPath) ? "" : Path.GetDirectoryName(asmPath);
                        }
                        catch
                        {
                        }

                        string vaAppsDocPath = GetVaAppsHelpDocPath();

                        string[] candidates = new[]
                        {
                            vaAppsDocPath,
                            Path.Combine(baseDir, "Extensions", "Kneeboard", "OKBHelpDoc.html"),
                            Path.Combine(baseDir, "OKBHelpDoc.html"),
                            Path.Combine(asmDir ?? "", "Extensions", "Kneeboard", "OKBHelpDoc.html"),
                            Path.Combine(asmDir ?? "", "OKBHelpDoc.html")
                        };

                        foreach (string candidate in candidates)
                        {
                            if (!string.IsNullOrWhiteSpace(candidate) && File.Exists(candidate))
                            {
                                return candidate;
                            }
                        }
                    }
                    catch
                    {
                    }

                    return "";
                }

                public static string GetOfflineHelpDocPath()
                {
                    try
                    {
                        EnsureHelpDocInVaAppsDocumentation();
                        string path = ResolveHelpDocPath();
                        if (!string.IsNullOrWhiteSpace(path) && File.Exists(path))
                        {
                            return path;
                        }
                    }
                    catch
                    {
                    }

                    return "";
                }

                public static string GetOfflineHelpDocBrowserPath()
                {
                    try
                    {
                        EnsureHelpDocInVaAppsDocumentation();

                        string vaAppsPath = GetVaAppsHelpDocPath();
                        if (!string.IsNullOrWhiteSpace(vaAppsPath) && File.Exists(vaAppsPath))
                        {
                            return CreateOfflineBrowserHelpDoc(vaAppsPath);
                        }

                        string resolved = ResolveHelpDocPath();
                        if (!string.IsNullOrWhiteSpace(resolved) && File.Exists(resolved))
                        {
                            return CreateOfflineBrowserHelpDoc(resolved);
                        }
                    }
                    catch
                    {
                    }

                    return "";
                }

                private static string CreateOfflineBrowserHelpDoc(string sourcePath)
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(sourcePath) || !File.Exists(sourcePath))
                        {
                            return "";
                        }

                        string sourceDir = Path.GetDirectoryName(sourcePath) ?? "";
                        if (string.IsNullOrWhiteSpace(sourceDir))
                        {
                            return sourcePath;
                        }

                        string imageDir = Path.Combine(sourceDir, "Images");
                        if (Directory.Exists(imageDir))
                        {
                            EnsureBundledHelpImagesInVaAppsDocumentation(imageDir);
                        }

                        string html = File.ReadAllText(sourcePath);
                        if (string.IsNullOrWhiteSpace(html))
                        {
                            return sourcePath;
                        }

                        string offlineHtml = html.Replace("/okb/helpimg?name=", "Images/");
                        string offlinePath = Path.Combine(sourceDir, "OKBHelpDoc.offline.html");
                        File.WriteAllText(offlinePath, offlineHtml, new UTF8Encoding(false));
                        return offlinePath;
                    }
                    catch
                    {
                        return sourcePath;
                    }
                }

                private static string GetVaAppsHelpDocPath()
                {
                    try
                    {
                        string appsRoot = string.IsNullOrWhiteSpace(State.VA_APPS)
                            ? (State.Proxy == null ? "" : Convert.ToString(State.Proxy.SessionState["VA_APPS"]))
                            : State.VA_APPS;

                        if (string.IsNullOrWhiteSpace(appsRoot))
                        {
                            try
                            {
                                string roaming = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                                if (!string.IsNullOrWhiteSpace(roaming))
                                {
                                    appsRoot = Path.Combine(roaming, "VoiceAttack", "Apps");
                                }
                            }
                            catch
                            {
                            }
                        }

                        if (string.IsNullOrWhiteSpace(appsRoot))
                        {
                            return "";
                        }

                        string docFolder = AppData.SubFolders.ContainsKey("documentation")
                            ? AppData.SubFolders["documentation"]
                            : "Documentation";

                        string pluginRoot = Path.Combine(appsRoot, Products.Products.Families.Vaicom.VaicomProPlugin.rootfoldername);
                        return Path.Combine(pluginRoot, docFolder, "OKBHelpDoc.html");
                    }
                    catch
                    {
                        return "";
                    }
                }

                private static string GetVaAppsHelpDocImageFolder()
                {
                    try
                    {
                        string helpDocPath = GetVaAppsHelpDocPath();
                        if (string.IsNullOrWhiteSpace(helpDocPath))
                        {
                            return "";
                        }

                        string docDir = Path.GetDirectoryName(helpDocPath);
                        if (string.IsNullOrWhiteSpace(docDir))
                        {
                            return "";
                        }

                        return Path.Combine(docDir, "Images");
                    }
                    catch
                    {
                        return "";
                    }
                }

                private static void EnsureHelpDocInVaAppsDocumentation()
                {
                    try
                    {
                        string targetPath = GetVaAppsHelpDocPath();
                        if (string.IsNullOrWhiteSpace(targetPath))
                        {
                            return;
                        }

                        string targetDir = Path.GetDirectoryName(targetPath);
                        if (string.IsNullOrWhiteSpace(targetDir))
                        {
                            return;
                        }

                        string imageDir = GetVaAppsHelpDocImageFolder();
                        if (!string.IsNullOrWhiteSpace(imageDir))
                        {
                            Directory.CreateDirectory(imageDir);
                            EnsureBundledHelpImagesInVaAppsDocumentation(imageDir);
                        }

                        string sourceHtml = GetBundledHelpDocHtml();
                        if (string.IsNullOrWhiteSpace(sourceHtml))
                        {
                            return;
                        }

                        Directory.CreateDirectory(targetDir);

                        if (!File.Exists(targetPath))
                        {
                            File.WriteAllText(targetPath, sourceHtml, new UTF8Encoding(false));
                            return;
                        }

                        string existing = "";
                        try
                        {
                            existing = File.ReadAllText(targetPath);
                        }
                        catch
                        {
                        }

                        if (!string.Equals(existing ?? "", sourceHtml, StringComparison.Ordinal))
                        {
                            File.WriteAllText(targetPath, sourceHtml, new UTF8Encoding(false));
                        }
                    }
                    catch
                    {
                    }
                }

                private static void EnsureBundledHelpImagesInVaAppsDocumentation(string imageRoot)
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(imageRoot))
                        {
                            return;
                        }

                        Assembly asm = typeof(OpenKneeboardBridge).Assembly;
                        string[] resources = asm.GetManifestResourceNames()
                            .Where(n => n.StartsWith("OKBHelpImages/", StringComparison.OrdinalIgnoreCase)
                                || n.StartsWith("OKBHelpImages\\", StringComparison.OrdinalIgnoreCase)
                                || n.IndexOf("OKBHelpImages/", StringComparison.OrdinalIgnoreCase) >= 0
                                || n.IndexOf("OKBHelpImages\\", StringComparison.OrdinalIgnoreCase) >= 0)
                            .ToArray();

                        foreach (string resourceName in resources)
                        {
                            string relative = GetHelpImageRelativePathFromResource(resourceName);
                            if (string.IsNullOrWhiteSpace(relative))
                            {
                                continue;
                            }

                            string targetPath = Path.GetFullPath(Path.Combine(imageRoot, relative));
                            string fullRoot = Path.GetFullPath(imageRoot);
                            if (!targetPath.StartsWith(fullRoot, StringComparison.OrdinalIgnoreCase))
                            {
                                continue;
                            }

                            string targetDir = Path.GetDirectoryName(targetPath);
                            if (!string.IsNullOrWhiteSpace(targetDir))
                            {
                                Directory.CreateDirectory(targetDir);
                            }

                            using (Stream stream = asm.GetManifestResourceStream(resourceName))
                            {
                                if (stream == null) continue;

                                using (MemoryStream ms = new MemoryStream())
                                {
                                    stream.CopyTo(ms);
                                    byte[] payload = ms.ToArray();

                                    bool shouldWrite = true;
                                    if (File.Exists(targetPath))
                                    {
                                        try
                                        {
                                            byte[] existing = File.ReadAllBytes(targetPath);
                                            shouldWrite = !existing.SequenceEqual(payload);
                                        }
                                        catch
                                        {
                                            shouldWrite = true;
                                        }
                                    }

                                    if (shouldWrite)
                                    {
                                        File.WriteAllBytes(targetPath, payload);
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                    }
                }

                private static string GetHelpImageRelativePathFromResource(string resourceName)
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(resourceName))
                        {
                            return "";
                        }

                        string token = "OKBHelpImages";
                        int idxSlash = resourceName.IndexOf(token + "/", StringComparison.OrdinalIgnoreCase);
                        int idxBackslash = resourceName.IndexOf(token + "\\", StringComparison.OrdinalIgnoreCase);
                        int idx = idxSlash >= 0 ? idxSlash : idxBackslash;
                        if (idx < 0)
                        {
                            return "";
                        }

                        string rel = resourceName.Substring(idx + token.Length + 1);
                        rel = rel.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
                        rel = rel.Trim();
                        if (string.IsNullOrWhiteSpace(rel) || rel.Contains(".."))
                        {
                            return "";
                        }

                        return rel;
                    }
                    catch
                    {
                        return "";
                    }
                }

                private static string GetBundledHelpDocHtml()
                {
                    try
                    {
                        Assembly asm = typeof(OpenKneeboardBridge).Assembly;
                        string resourceName = asm
                            .GetManifestResourceNames()
                            .FirstOrDefault(n => n.EndsWith("Extensions.Kneeboard.OKBHelpDoc.html", StringComparison.OrdinalIgnoreCase)
                                || n.EndsWith("OKBHelpDoc.html", StringComparison.OrdinalIgnoreCase));

                        if (!string.IsNullOrWhiteSpace(resourceName))
                        {
                            using (Stream stream = asm.GetManifestResourceStream(resourceName))
                            {
                                if (stream != null)
                                {
                                    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8, true))
                                    {
                                        string embedded = reader.ReadToEnd();
                                        if (!string.IsNullOrWhiteSpace(embedded))
                                        {
                                            return embedded;
                                        }
                                    }
                                }
                            }
                        }

                        string path = ResolveHelpDocPath();
                        if (!string.IsNullOrWhiteSpace(path) && File.Exists(path))
                        {
                            return File.ReadAllText(path);
                        }
                    }
                    catch
                    {
                    }

                    return "";
                }

                private static string NormalizeHelpDocSectionId(string tab)
                {
                    string token = (tab ?? "").Trim().ToUpperInvariant();
                    switch (token)
                    {
                        case "DTC":
                        case "FLT PLN":
                        case "FLTPLN":
                            return "fltpln";
                        case "ATC":
                        case "WX/ATC":
                            return "atc";
                        case "AWACS":
                            return "awacs";
                        case "JTAC":
                            return "jtac";
                        case "TANKER":
                            return "tanker";
                        case "AOCS":
                            return "aocs";
                        case "FLIGHT":
                            return "flight";
                        case "AI CREW":
                        case "AICREW":
                            return "aicrew";
                        case "GND CREW":
                        case "GNDCREW":
                            return "gndcrew";
                        case "NOTES":
                            return "notes";
                        case "EFB":
                            return "efb";
                        case "LOG":
                        default:
                            return "log";
                    }
                }

                private static string ExtractSectionById(string html, string sectionId)
                {
                    if (string.IsNullOrWhiteSpace(html) || string.IsNullOrWhiteSpace(sectionId))
                    {
                        return "";
                    }

                    string pattern = "<section\\b[^>]*\\bid\\s*=\\s*['\\\"]" + Regex.Escape(sectionId) + "['\\\"][^>]*>.*?</section>";
                    Match match = Regex.Match(html, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    return match.Success ? match.Value : "";
                }

                private static byte[] ReadHelpDocImageBytes(string fileName, out string contentType)
                {
                    contentType = "application/octet-stream";

                    try
                    {
                        string normalized = (fileName ?? "").Trim();
                        if (string.IsNullOrWhiteSpace(normalized))
                        {
                            return null;
                        }

                        normalized = normalized.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
                        if (normalized.Contains(".."))
                        {
                            return null;
                        }

                        string ext = (Path.GetExtension(normalized) ?? "").ToLowerInvariant();
                        switch (ext)
                        {
                            case ".png": contentType = "image/png"; break;
                            case ".jpg":
                            case ".jpeg": contentType = "image/jpeg"; break;
                            case ".gif": contentType = "image/gif"; break;
                            case ".bmp": contentType = "image/bmp"; break;
                            case ".webp": contentType = "image/webp"; break;
                            case ".svg": contentType = "image/svg+xml"; break;
                            default: return null;
                        }

                        string baseFolder = GetVaAppsHelpDocImageFolder();
                        if (string.IsNullOrWhiteSpace(baseFolder) || !Directory.Exists(baseFolder))
                        {
                            return null;
                        }

                        string fullPath = Path.GetFullPath(Path.Combine(baseFolder, normalized));
                        string fullBase = Path.GetFullPath(baseFolder);
                        if (!fullPath.StartsWith(fullBase, StringComparison.OrdinalIgnoreCase))
                        {
                            return null;
                        }

                        if (!File.Exists(fullPath))
                        {
                            return null;
                        }

                        return File.ReadAllBytes(fullPath);
                    }
                    catch
                    {
                        return null;
                    }
                }

                private static string BuildSnapshotJson()
                {
                    OpenKneeboardSnapshot responseModel;

                    lock (Sync)
                    {
                        responseModel = snapshot.Clone();
                    }

                    try
                    {
                        responseModel.Efb = OpenKneeboardNavigraphEfbState.BuildSnapshot(responseModel.Server);
                    }
                    catch
                    {
                    }

                    try
                    {
                        return JsonConvert.SerializeObject(responseModel, Formatting.Indented);
                    }
                    catch
                    {
                        var fallback = new
                        {
                            ActiveCategory = responseModel.ActiveCategory,
                            NotesBuffer = responseModel.NotesBuffer,
                            AiCrewPhase = responseModel.AiCrewPhase,
                            UpdatedUtc = responseModel.UpdatedUtc,
                            Server = new
                            {
                                Theater = responseModel.Server?.Theater ?? "",
                                DcsLocation = responseModel.Server?.DcsLocation ?? "",
                                Aircraft = responseModel.Server?.Aircraft ?? "",
                                PlayerUsername = responseModel.Server?.PlayerUsername ?? "",
                                PlayerCallsign = responseModel.Server?.PlayerCallsign ?? "",
                                MissionTitle = responseModel.Server?.MissionTitle ?? "",
                                MissionBriefing = responseModel.Server?.MissionBriefing ?? "",
                                MissionDetails = responseModel.Server?.MissionDetails ?? "",
                                MissionTimeSeconds = responseModel.Server?.MissionTimeSeconds ?? 0,
                                PlayerPosX = responseModel.Server?.PlayerPosX ?? 0,
                                PlayerPosY = responseModel.Server?.PlayerPosY ?? 0,
                                PlayerAltFeet = responseModel.Server?.PlayerAltFeet ?? 0,
                                Multiplayer = responseModel.Server?.Multiplayer ?? false,
                                DebugMode = responseModel.Server?.DebugMode ?? false,
                                AtcMetars = responseModel.Server?.AtcMetars ?? new Dictionary<string, string>(),
                                FlightMembers = responseModel.Server?.FlightMembers ?? new List<OpenKneeboardFlightMember>(),
                                Diagnostics = (object)null,
                            },
                            Status = responseModel.Status,
                            AiCrewKeywords = responseModel.AiCrewKeywords,
                            AiCrewKeywordSections = responseModel.AiCrewKeywordSections,
                            RawServerMessages = responseModel.RawServerMessages,
                            Logs = responseModel.Logs,
                            Units = responseModel.Units,
                            UnitDetails = responseModel.UnitDetails,
                            AliasesChunk0 = responseModel.AliasesChunk0,
                            AliasesChunk1 = responseModel.AliasesChunk1,
                            Efb = responseModel.Efb,
                            DtcFiles = responseModel.DtcFiles,
                            DtcSelectedFile = responseModel.DtcSelectedFile,
                            DtcJson = responseModel.DtcJson,
                            DtcSourceType = responseModel.DtcSourceType,
                        };

                        return JsonConvert.SerializeObject(fallback, Formatting.Indented);
                    }
                }

                private static string BuildClockJson()
                {
                    double missionTimeSeconds = 0;
                    string missionIdentity = "";
                    DateTime sourceUtc = DateTime.UtcNow;

                    try
                    {
                        if (State.currentstate != null)
                        {
                            double missionStartSeconds = 0;
                            double missionElapsedSeconds = State.currentstate.timer;
                            bool hasMissionStart = double.TryParse(State.currentstate.sortie ?? "", out missionStartSeconds);
                            if (hasMissionStart && missionElapsedSeconds >= 0)
                            {
                                missionTimeSeconds = missionStartSeconds + missionElapsedSeconds;
                            }
                            else
                            {
                                missionTimeSeconds = State.currentstate.tod;
                            }

                            missionIdentity = string.Join("|", new[]
                            {
                                State.currentstate.theatre ?? "",
                                State.currentstate.missiontitle ?? "",
                                State.currentstate.id ?? "",
                                State.currentstate.playercallsign ?? "",
                                State.currentstate.multiplayer ? "1" : "0"
                            });
                        }
                    }
                    catch
                    {
                    }

                    var payload = new
                    {
                        MissionTimeSeconds = missionTimeSeconds,
                        SourceUtc = sourceUtc,
                        MissionIdentity = missionIdentity,
                    };

                    return JsonConvert.SerializeObject(payload, Formatting.None);
                }

                private static string BuildOwnshipJson()
                {
                    try
                    {
                        string theater = "";
                        double posX = 0;
                        double posY = 0;
                        double altFeet = 0;
                        double headingDeg = 0;
                        bool hasHeading = false;
                        double missionTimeSeconds = 0;
                        bool hasPosition = false;

                        FastOwnshipState ownshipFast;
                        if (TryGetFastOwnship(out ownshipFast))
                        {
                            posX = ownshipFast.X;
                            posY = ownshipFast.Y;
                            altFeet = ownshipFast.AltFeet;
                            hasPosition = true;
                            hasHeading = ownshipFast.HasHeading;
                            headingDeg = ownshipFast.HeadingDeg;
                        }

                        if (State.currentstate != null)
                        {
                            theater = State.currentstate.theatre ?? "";

                            if (!hasPosition && State.currentstate.bpos != null)
                            {
                                posX = State.currentstate.bpos.x;
                                posY = State.currentstate.bpos.z;
                                altFeet = State.currentstate.bpos.y * 3.28084;
                                hasPosition = true;
                            }

                            double missionStartSeconds;
                            double missionElapsedSeconds = State.currentstate.timer;
                            bool hasMissionStart = double.TryParse(State.currentstate.sortie ?? "", out missionStartSeconds);
                            missionTimeSeconds = hasMissionStart && missionElapsedSeconds >= 0
                                ? (missionStartSeconds + missionElapsedSeconds)
                                : State.currentstate.tod;
                        }

                        var payload = new
                        {
                            updatedUtc = DateTime.UtcNow,
                            theater = theater,
                            hasPosition = hasPosition,
                            posX = posX,
                            posY = posY,
                            altFeet = altFeet,
                            hasHeading = hasHeading,
                            headingDeg = headingDeg,
                            missionTimeSeconds = missionTimeSeconds,
                        };

                        return JsonConvert.SerializeObject(payload, Formatting.None);
                    }
                    catch
                    {
                        return "{\"updatedUtc\":\"\",\"theater\":\"\",\"hasPosition\":false,\"posX\":0,\"posY\":0,\"altFeet\":0,\"missionTimeSeconds\":0}";
                    }
                }

                private static string BuildEfbAirportsJson()
                {
                    List<string> airports = new List<string>();
                    List<string> localAirports = new List<string>();
                    Dictionary<string, string> airportNames = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                    bool useNavigraph = false;
                    string chartsRoot = GetEfbChartsRootPath();

                    try
                    {
                        string authPayload;
                        useNavigraph = OpenKneeboardNavigraphEfbState.TryGetDecryptedAuthBlob(out authPayload) && !string.IsNullOrWhiteSpace(authPayload);
                    }
                    catch
                    {
                        useNavigraph = false;
                    }

                    if (useNavigraph)
                    {
                        try
                        {
                            string theatre = (State.currentstate == null ? "" : State.currentstate.theatre) ?? "";
                            airports.AddRange(OpenKneeboardNavigraphApiProxy.GetTheatreAirportCandidates(theatre));
                            airportNames = OpenKneeboardNavigraphApiProxy.GetTheatreAirportDisplayNames(theatre);
                        }
                        catch
                        {
                            airportNames = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                        }
                    }

                    try
                    {
                        if (Directory.Exists(chartsRoot))
                        {
                            foreach (string dir in Directory.GetDirectories(chartsRoot, "*", SearchOption.TopDirectoryOnly))
                            {
                                string airport = (Path.GetFileName(dir) ?? "").Trim().ToUpperInvariant();
                                if (string.IsNullOrWhiteSpace(airport))
                                {
                                    continue;
                                }

                                bool hasChartFiles = false;
                                try
                                {
                                    hasChartFiles = Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories)
                                        .Any(path => IsSupportedEfbChartExtension(Path.GetExtension(path)));
                                }
                                catch
                                {
                                }

                                if (hasChartFiles)
                                {
                                    localAirports.Add(airport);
                                }
                            }
                        }
                    }
                    catch
                    {
                    }

                    airports.AddRange(localAirports);

                    airports = airports
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .OrderBy(a => a, StringComparer.OrdinalIgnoreCase)
                        .ToList();

                    if (useNavigraph && airports.Count > 0)
                    {
                        try
                        {
                            Dictionary<string, string> overrideNames = OpenKneeboardNavigraphApiProxy.GetAirportDisplayNamesFromOverrides(airports);
                            foreach (var kv in overrideNames)
                            {
                                if (string.IsNullOrWhiteSpace(kv.Key) || string.IsNullOrWhiteSpace(kv.Value))
                                {
                                    continue;
                                }
                                if (!airportNames.ContainsKey(kv.Key) || string.IsNullOrWhiteSpace(airportNames[kv.Key]))
                                {
                                    airportNames[kv.Key] = kv.Value;
                                }
                            }

                            Dictionary<string, string> resolvedNames = OpenKneeboardNavigraphApiProxy.GetAirportNamesByIcao(airports);
                            foreach (var kv in resolvedNames)
                            {
                                if (string.IsNullOrWhiteSpace(kv.Key) || string.IsNullOrWhiteSpace(kv.Value))
                                {
                                    continue;
                                }
                                airportNames[kv.Key] = kv.Value;
                            }
                        }
                        catch
                        {
                        }
                    }

                    var airportEntries = airports
                        .Select(a => new
                        {
                            icao = a,
                            name = airportNames.ContainsKey(a) ? (airportNames[a] ?? "") : "",
                        })
                        .ToList();

                    var payload = new
                    {
                        root = chartsRoot,
                        airports = airports,
                        airportEntries = airportEntries,
                    };

                    return JsonConvert.SerializeObject(payload, Formatting.None);
                }

                private static string BuildEfbChartsJson(string airport)
                {
                    string normalizedAirport = string.IsNullOrWhiteSpace(airport)
                        ? "UNSET"
                        : airport.Trim().ToUpperInvariant();

                    string chartsRoot = GetEfbChartsRootPath();
                    string airportFolder = Path.Combine(chartsRoot, normalizedAirport);
                    List<object> charts = new List<object>();

                    try
                    {
                        if (Directory.Exists(airportFolder))
                        {
                            string[] files = Directory.GetFiles(airportFolder, "*.*", SearchOption.AllDirectories)
                                .Where(path => IsSupportedEfbChartExtension(Path.GetExtension(path)))
                                .OrderBy(path => path, StringComparer.OrdinalIgnoreCase)
                                .ToArray();

                            foreach (string file in files)
                            {
                                string chartId = BuildEfbChartId(normalizedAirport, airportFolder, file);
                                if (string.IsNullOrWhiteSpace(chartId))
                                {
                                    continue;
                                }

                                string displayName = BuildEfbChartDisplayName(airportFolder, file);
                                charts.Add(new
                                {
                                    id = chartId,
                                    name = displayName,
                                    type = "AIRPORT",
                                });
                            }
                        }
                    }
                    catch
                    {
                    }

                    var payload = new
                    {
                        airport = normalizedAirport,
                        charts = charts
                    };

                    return JsonConvert.SerializeObject(payload, Formatting.None);
                }

                private static string GetEfbChartsRootPath()
                {
                    try
                    {
                        foreach (string savedGames in GetSavedGamesRoots())
                        {
                            if (string.IsNullOrWhiteSpace(savedGames) || !Directory.Exists(savedGames))
                            {
                                continue;
                            }

                            try
                            {
                                foreach (string dcsRoot in Directory.EnumerateDirectories(savedGames, "DCS*", SearchOption.TopDirectoryOnly))
                                {
                                    string chartsPath = Path.Combine(dcsRoot, "Kneeboard", "Vaicom Charts");
                                    if (Directory.Exists(chartsPath))
                                    {
                                        return chartsPath;
                                    }
                                }
                            }
                            catch
                            {
                            }

                            string fallbackChartsPath = Path.Combine(savedGames, "DCS", "Kneeboard", "Vaicom Charts");
                            if (Directory.Exists(fallbackChartsPath))
                            {
                                return fallbackChartsPath;
                            }
                        }

                        string firstSavedGames = GetSavedGamesRoots().FirstOrDefault(p => !string.IsNullOrWhiteSpace(p));
                        if (!string.IsNullOrWhiteSpace(firstSavedGames))
                        {
                            return Path.Combine(firstSavedGames, "DCS", "Kneeboard", "Vaicom Charts");
                        }
                    }
                    catch
                    {
                    }

                    string profile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                    return Path.Combine(profile, "Saved Games", "DCS", "Kneeboard", "Vaicom Charts");
                }

                private static bool IsSupportedEfbChartExtension(string extension)
                {
                    string ext = string.IsNullOrWhiteSpace(extension) ? "" : extension.Trim();
                    return EfbChartFileExtensions.Any(supported => string.Equals(supported, ext, StringComparison.OrdinalIgnoreCase));
                }

                private static string BuildEfbChartId(string airport, string airportFolder, string fullFilePath)
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(fullFilePath))
                        {
                            return "";
                        }

                        string relative = fullFilePath;
                        if (!string.IsNullOrWhiteSpace(airportFolder)
                            && fullFilePath.StartsWith(airportFolder, StringComparison.OrdinalIgnoreCase))
                        {
                            relative = fullFilePath.Substring(airportFolder.Length)
                                .TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                        }

                        if (string.IsNullOrWhiteSpace(relative))
                        {
                            return "";
                        }

                        string normalizedRelative = relative
                            .Replace(Path.DirectorySeparatorChar, '/')
                            .Replace(Path.AltDirectorySeparatorChar, '/');

                        return string.Concat(airport, "/", normalizedRelative);
                    }
                    catch
                    {
                        return "";
                    }
                }

                private static string BuildEfbChartDisplayName(string airportFolder, string fullFilePath)
                {
                    try
                    {
                        string relative = fullFilePath;
                        if (!string.IsNullOrWhiteSpace(airportFolder) && !string.IsNullOrWhiteSpace(fullFilePath)
                            && fullFilePath.StartsWith(airportFolder, StringComparison.OrdinalIgnoreCase))
                        {
                            relative = fullFilePath.Substring(airportFolder.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                        }

                        string noExt = Path.ChangeExtension(relative, null) ?? relative;
                        return noExt.Replace(Path.DirectorySeparatorChar, '/').Replace(Path.AltDirectorySeparatorChar, '/');
                    }
                    catch
                    {
                        return Path.GetFileNameWithoutExtension(fullFilePath) ?? "Chart";
                    }
                }

                private static byte[] ReadEfbChartBytes(string chartId, out string contentType)
                {
                    contentType = "application/octet-stream";

                    if (string.IsNullOrWhiteSpace(chartId))
                    {
                        return null;
                    }

                    string[] parts = chartId.Split(new[] { '/' }, 2, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length != 2)
                    {
                        return null;
                    }

                    string airport = (parts[0] ?? "").Trim().ToUpperInvariant();
                    string fileName = (parts[1] ?? "").Trim();
                    if (string.IsNullOrWhiteSpace(airport) || string.IsNullOrWhiteSpace(fileName))
                    {
                        return null;
                    }

                    string chartsRoot = GetEfbChartsRootPath();
                    string airportFolder = Path.Combine(chartsRoot, airport);
                    string fullPath;

                    try
                    {
                        fullPath = Path.GetFullPath(Path.Combine(airportFolder, fileName));
                    }
                    catch
                    {
                        return null;
                    }

                    string allowedRoot;
                    try
                    {
                        allowedRoot = Path.GetFullPath(airportFolder).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                            + Path.DirectorySeparatorChar;
                    }
                    catch
                    {
                        return null;
                    }

                    if (!fullPath.StartsWith(allowedRoot, StringComparison.OrdinalIgnoreCase))
                    {
                        return null;
                    }

                    string extension = Path.GetExtension(fullPath);
                    if (!IsSupportedEfbChartExtension(extension) || !File.Exists(fullPath))
                    {
                        return null;
                    }

                    contentType = GetEfbChartContentType(extension);
                    return File.ReadAllBytes(fullPath);
                }

                private static string GetEfbChartContentType(string extension)
                {
                    string ext = string.IsNullOrWhiteSpace(extension) ? "" : extension.Trim().ToLowerInvariant();
                    switch (ext)
                    {
                        case ".svg":
                            return "image/svg+xml";
                        case ".png":
                            return "image/png";
                        case ".jpg":
                        case ".jpeg":
                            return "image/jpeg";
                        case ".gif":
                            return "image/gif";
                        case ".bmp":
                            return "image/bmp";
                        case ".webp":
                            return "image/webp";
                        default:
                            return "application/octet-stream";
                    }
                }

                private static void WriteJson(HttpListenerResponse response, string payload)
                {
                    WriteText(response, payload, "application/json; charset=utf-8");
                }

                private static void WriteText(HttpListenerResponse response, string payload, string contentType)
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(payload ?? "");
                    response.ContentType = contentType;
                    response.ContentEncoding = Encoding.UTF8;
                    response.ContentLength64 = bytes.Length;
                    response.OutputStream.Write(bytes, 0, bytes.Length);
                    response.OutputStream.Close();
                }

                private static void WriteBinary(HttpListenerResponse response, byte[] payload, string contentType)
                {
                    byte[] bytes = payload ?? Array.Empty<byte>();
                    response.ContentType = contentType;
                    response.ContentLength64 = bytes.Length;
                    response.OutputStream.Write(bytes, 0, bytes.Length);
                    response.OutputStream.Close();
                }

                private static byte[] GetLogoBytes()
                {
                    try
                    {
                        Uri uri = new Uri("pack://application:,,,/VAICOMPRO;component/Resources/Images/VAICOMPRO logo icon 5 128.png", UriKind.Absolute);
                        var resourceInfo = Application.GetResourceStream(uri);
                        if (resourceInfo != null && resourceInfo.Stream != null)
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                resourceInfo.Stream.CopyTo(ms);
                                return ms.ToArray();
                            }
                        }
                    }
                    catch
                    {
                    }

                    return null;
                }

                public static void SetRawServerCaptureEnabled(bool enabled)
                {
                    bool allow = enabled && State.activeconfig != null && State.activeconfig.Debugmode;

                    lock (Sync)
                    {
                        captureRawServerMessages = allow;
                        if (!allow)
                        {
                            snapshot.RawServerMessages.Clear();
                        }
                    }
                }

                public static void AppendRawServerMessage(string rawMessage)
                {
                    if (string.IsNullOrWhiteSpace(rawMessage))
                    {
                        return;
                    }

                    bool doCapture;
                    lock (Sync)
                    {
                        doCapture = captureRawServerMessages && State.activeconfig != null && State.activeconfig.Debugmode;
                        if (!doCapture)
                        {
                            return;
                        }

                        string entry = DateTime.UtcNow.ToString("o") + " | " + rawMessage;
                        snapshot.RawServerMessages.Add(entry);
                        if (snapshot.RawServerMessages.Count > 200)
                        {
                            snapshot.RawServerMessages = snapshot.RawServerMessages.Skip(snapshot.RawServerMessages.Count - 200).ToList();
                        }
                        snapshot.UpdatedUtc = DateTime.UtcNow;
                    }

                    try
                    {
                        string logsFolder = Path.Combine(State.VA_APPS, Products.Products.Families.Vaicom.VaicomProPlugin.rootfoldername, AppData.SubFolders["logfiles"]);
                        string filePath = Path.Combine(logsFolder, "VAICOMPRO.ServerMessages.log");

                        lock (RawServerLogSync)
                        {
                            Directory.CreateDirectory(logsFolder);
                            File.AppendAllText(filePath, DateTime.UtcNow.ToString("o") + " | " + rawMessage + Environment.NewLine);
                        }
                    }
                    catch
                    {
                    }
                }

                private static void ResetRawServerMessagesLog()
                {
                    try
                    {
                        string logsFolder = Path.Combine(State.VA_APPS, Products.Products.Families.Vaicom.VaicomProPlugin.rootfoldername, AppData.SubFolders["logfiles"]);
                        string filePath = Path.Combine(logsFolder, "VAICOMPRO.ServerMessages.log");

                        lock (RawServerLogSync)
                        {
                            if (File.Exists(filePath))
                            {
                                File.Delete(filePath);
                            }
                        }
                    }
                    catch
                    {
                    }
                }

                private static Dictionary<string, List<string>> CloneAliasDictionary(SortedDictionary<string, List<string>> source)
                {
                    Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();

                    if (source == null)
                    {
                        return result;
                    }

                    foreach (KeyValuePair<string, List<string>> pair in source)
                    {
                        result[pair.Key] = pair.Value == null ? new List<string>() : new List<string>(pair.Value);
                    }

                    return result;
                }

                private static void RefreshDtcFilesSnapshot()
                {
                    try
                    {
                        List<string> files = GetAvailableDtcFiles();

                        lock (Sync)
                        {
                            snapshot.DtcFiles = files;

                            if (!string.IsNullOrWhiteSpace(snapshot.DtcSelectedFile))
                            {
                                string selected = FindMatchingFlightPlanSelection(snapshot.DtcSelectedFile, files);
                                if (string.IsNullOrWhiteSpace(selected))
                                {
                                    snapshot.DtcSelectedFile = "";
                                    snapshot.DtcJson = "";
                                }
                                else
                                {
                                    snapshot.DtcSelectedFile = selected;
                                    if (TryReadValidatedFlightPlan(selected, out string json, out string sourceType))
                                    {
                                        snapshot.DtcJson = json;
                                        snapshot.DtcSourceType = sourceType;
                                    }
                                }
                            }

                            snapshot.UpdatedUtc = DateTime.UtcNow;
                        }
                    }
                    catch
                    {
                    }
                }

                private static bool SelectDtcFileSnapshot(string file)
                {
                    lock (Sync)
                    {
                        if (string.IsNullOrWhiteSpace(file))
                        {
                            snapshot.DtcSelectedFile = "";
                            snapshot.DtcJson = "";
                            snapshot.DtcSourceType = "";
                            snapshot.UpdatedUtc = DateTime.UtcNow;
                            return true;
                        }

                        string selected = FindMatchingFlightPlanSelection(file, snapshot.DtcFiles ?? new List<string>());

                        if (string.IsNullOrWhiteSpace(selected))
                        {
                            return false;
                        }

                        if (!TryReadValidatedFlightPlan(selected, out string json, out string sourceType))
                        {
                            return false;
                        }

                        snapshot.DtcSelectedFile = selected;
                        snapshot.DtcJson = json;
                        snapshot.DtcSourceType = sourceType;
                        snapshot.UpdatedUtc = DateTime.UtcNow;
                        return true;
                    }
                }

                private static string FindMatchingFlightPlanSelection(string requested, List<string> candidates)
                {
                    try
                    {
                        List<string> list = candidates ?? new List<string>();
                        string req = requested ?? "";

                        string direct = list.FirstOrDefault(f => string.Equals(f, req, StringComparison.OrdinalIgnoreCase));
                        if (!string.IsNullOrWhiteSpace(direct))
                        {
                            return direct;
                        }

                        string reqPath;
                        string reqRoute;
                        if (TryParseRouteSelectionToken(req, out reqPath, out reqRoute))
                        {
                            foreach (string c in list)
                            {
                                string cPath;
                                string cRoute;
                                if (!TryParseRouteSelectionToken(c, out cPath, out cRoute))
                                {
                                    continue;
                                }

                                if (string.Equals(reqRoute, cRoute, StringComparison.OrdinalIgnoreCase)
                                    && string.Equals(Path.GetFullPath(reqPath ?? ""), Path.GetFullPath(cPath ?? ""), StringComparison.OrdinalIgnoreCase))
                                {
                                    return c;
                                }
                            }
                        }

                        if (req.IndexOf(RouteSelectionPrefix, StringComparison.Ordinal) < 0)
                        {
                            string reqFull = Path.GetFullPath(req);
                            string pathMatch = list.FirstOrDefault(c =>
                            {
                                try { return string.Equals(Path.GetFullPath(c ?? ""), reqFull, StringComparison.OrdinalIgnoreCase); }
                                catch { return false; }
                            });
                            if (!string.IsNullOrWhiteSpace(pathMatch))
                            {
                                return pathMatch;
                            }
                        }
                    }
                    catch
                    {
                    }

                    return "";
                }

                private static List<string> GetAvailableDtcFiles()
                {
                    List<string> files = new List<string>();

                    foreach (string dtcDir in GetSavedGamesDtcFolders())
                    {
                        try
                        {
                            if (!Directory.Exists(dtcDir))
                            {
                                continue;
                            }

                            foreach (string candidate in Directory.EnumerateFiles(dtcDir, "*.*", SearchOption.AllDirectories))
                            {
                                if (!IsDtcExtension(candidate))
                                {
                                    continue;
                                }

                                if (!IsPathUnderDirectory(candidate, dtcDir))
                                {
                                    continue;
                                }

                                if (!TryReadValidatedFlightPlan(candidate, out _, out _))
                                {
                                    continue;
                                }

                                files.Add(Path.GetFullPath(candidate));
                                if (files.Count >= 250)
                                {
                                    break;
                                }
                            }
                        }
                        catch
                        {
                        }

                        if (files.Count >= 250)
                        {
                            break;
                        }
                    }

                    foreach (string missionsDir in GetSavedGamesMissionFolders())
                    {
                        try
                        {
                            if (!Directory.Exists(missionsDir))
                            {
                                continue;
                            }

                            foreach (string routeCandidate in Directory.EnumerateFiles(missionsDir, "*.rte", SearchOption.AllDirectories))
                            {
                                if (!IsPathUnderDirectory(routeCandidate, missionsDir))
                                {
                                    continue;
                                }

                                if (!TryReadValidatedFlightPlan(routeCandidate, out _, out _))
                                {
                                    continue;
                                }

                                files.Add(Path.GetFullPath(routeCandidate));
                                if (files.Count >= 250)
                                {
                                    break;
                                }
                            }
                        }
                        catch
                        {
                        }

                        if (files.Count >= 250)
                        {
                            break;
                        }
                    }

                    foreach (string routeToolFile in GetRouteToolPresetFiles())
                    {
                        try
                        {
                            if (string.IsNullOrWhiteSpace(routeToolFile) || !File.Exists(routeToolFile))
                            {
                                continue;
                            }

                            List<string> routeNames = ExtractRouteNamesFromLua(routeToolFile);
                            if (routeNames.Count == 0)
                            {
                                continue;
                            }

                            foreach (string routeName in routeNames)
                            {
                                string token = RouteSelectionPrefix + Uri.EscapeDataString(routeName) + "::" + Path.GetFullPath(routeToolFile);
                                files.Add(token);
                                if (files.Count >= 250)
                                {
                                    break;
                                }
                            }

                            if (files.Count >= 250)
                            {
                                break;
                            }
                        }
                        catch
                        {
                        }

                        if (files.Count >= 250)
                        {
                            break;
                        }
                    }

                    return files
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .OrderByDescending(p =>
                        {
                            try
                            {
                                string routePath;
                                string routeName;
                                if (TryParseRouteSelectionToken(p, out routePath, out routeName))
                                {
                                    return File.GetLastWriteTimeUtc(routePath);
                                }

                                return File.GetLastWriteTimeUtc(p);
                            }
                            catch { return DateTime.MinValue; }
                        })
                        .ThenBy(p => p, StringComparer.OrdinalIgnoreCase)
                        .ToList();
                }

                private static List<string> GetSavedGamesDtcFolders()
                {
                    List<string> roots = new List<string>();

                    try
                    {
                        foreach (string savedGames in GetSavedGamesRoots())
                        {
                            if (!Directory.Exists(savedGames))
                            {
                                continue;
                            }

                            foreach (string dcsRoot in Directory.EnumerateDirectories(savedGames, "DCS*", SearchOption.TopDirectoryOnly))
                            {
                                try
                                {
                                    string name = Path.GetFileName(dcsRoot);
                                    if (string.IsNullOrWhiteSpace(name) || !name.StartsWith("DCS", StringComparison.OrdinalIgnoreCase))
                                    {
                                        continue;
                                    }

                                    roots.Add(Path.Combine(dcsRoot, "DTC"));
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                    catch
                    {
                    }

                    return roots
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .ToList();
                }

                private static List<string> GetRouteToolPresetFiles()
                {
                    List<string> files = new List<string>();

                    try
                    {
                        foreach (string savedGames in GetSavedGamesRoots())
                        {
                            if (!Directory.Exists(savedGames))
                            {
                                continue;
                            }

                            foreach (string dcsRoot in Directory.EnumerateDirectories(savedGames, "DCS*", SearchOption.TopDirectoryOnly))
                            {
                                try
                                {
                                    string name = Path.GetFileName(dcsRoot);
                                    if (string.IsNullOrWhiteSpace(name) || !name.StartsWith("DCS", StringComparison.OrdinalIgnoreCase))
                                    {
                                        continue;
                                    }

                                    string routeDir = Path.Combine(dcsRoot, "Config", "RouteToolPresets");
                                    if (!Directory.Exists(routeDir))
                                    {
                                        continue;
                                    }

                                    string activeTheater = string.IsNullOrWhiteSpace(State.currentstate == null ? "" : State.currentstate.theatre)
                                        ? ""
                                        : State.currentstate.theatre.Trim();

                                    if (!string.IsNullOrWhiteSpace(activeTheater))
                                    {
                                        string activeFile = Path.Combine(routeDir, activeTheater + ".lua");
                                        if (File.Exists(activeFile))
                                        {
                                            files.Add(Path.GetFullPath(activeFile));
                                            continue;
                                        }
                                    }

                                    foreach (string fallback in Directory.EnumerateFiles(routeDir, "*.lua", SearchOption.TopDirectoryOnly))
                                    {
                                        files.Add(Path.GetFullPath(fallback));
                                    }
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                    catch
                    {
                    }

                    return files
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .OrderBy(p => p, StringComparer.OrdinalIgnoreCase)
                        .ToList();
                }

                private static List<string> GetSavedGamesMissionFolders()
                {
                    List<string> roots = new List<string>();

                    try
                    {
                        foreach (string savedGames in GetSavedGamesRoots())
                        {
                            if (!Directory.Exists(savedGames))
                            {
                                continue;
                            }

                            foreach (string dcsRoot in Directory.EnumerateDirectories(savedGames, "DCS*", SearchOption.TopDirectoryOnly))
                            {
                                try
                                {
                                    string name = Path.GetFileName(dcsRoot);
                                    if (string.IsNullOrWhiteSpace(name) || !name.StartsWith("DCS", StringComparison.OrdinalIgnoreCase))
                                    {
                                        continue;
                                    }

                                    roots.Add(Path.Combine(dcsRoot, "Missions"));
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                    catch
                    {
                    }

                    return roots
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .ToList();
                }

                private static List<string> GetSavedGamesRoots()
                {
                    List<string> roots = new List<string>();

                    try
                    {
                        string knownFolderPath = GetKnownFolderPath(SavedGamesFolderId);
                        if (!string.IsNullOrWhiteSpace(knownFolderPath))
                        {
                            roots.Add(Path.GetFullPath(knownFolderPath));
                        }
                    }
                    catch
                    {
                    }

                    try
                    {
                        string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                        if (!string.IsNullOrWhiteSpace(userProfile))
                        {
                            roots.Add(Path.GetFullPath(Path.Combine(userProfile, "Saved Games")));
                        }
                    }
                    catch
                    {
                    }

                    return roots
                        .Where(p => !string.IsNullOrWhiteSpace(p))
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .ToList();
                }

                private static string GetKnownFolderPath(Guid folderId)
                {
                    IntPtr outPath = IntPtr.Zero;

                    try
                    {
                        int hr = SHGetKnownFolderPath(folderId, 0, IntPtr.Zero, out outPath);
                        if (hr != 0 || outPath == IntPtr.Zero)
                        {
                            return "";
                        }

                        return Marshal.PtrToStringUni(outPath) ?? "";
                    }
                    catch
                    {
                        return "";
                    }
                    finally
                    {
                        if (outPath != IntPtr.Zero)
                        {
                            Marshal.FreeCoTaskMem(outPath);
                        }
                    }
                }

                private static bool IsDtcExtension(string path)
                {
                    try
                    {
                        string ext = Path.GetExtension(path);
                        return DtcFileExtensions.Any(x => string.Equals(x, ext, StringComparison.OrdinalIgnoreCase));
                    }
                    catch
                    {
                        return false;
                    }
                }

                private static bool IsPathUnderDirectory(string filePath, string rootDirectory)
                {
                    try
                    {
                        string fullFile = Path.GetFullPath(filePath ?? "");
                        string fullRoot = Path.GetFullPath(rootDirectory ?? "").TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                            + Path.DirectorySeparatorChar;

                        return fullFile.StartsWith(fullRoot, StringComparison.OrdinalIgnoreCase);
                    }
                    catch
                    {
                        return false;
                    }
                }

                private static bool TryReadValidatedFlightPlan(string filePath, out string json, out string sourceType)
                {
                    json = "";
                    sourceType = "";

                    try
                    {
                        string routeSelectionPath;
                        string routeSelectionName;
                        if (TryParseRouteSelectionToken(filePath, out routeSelectionPath, out routeSelectionName))
                        {
                            return TryReadRouteSelection(routeSelectionPath, routeSelectionName, out json, out sourceType);
                        }

                        if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
                        {
                            return false;
                        }

                        string raw = File.ReadAllText(filePath);
                        if (string.IsNullOrWhiteSpace(raw))
                        {
                            return false;
                        }

                        string ext = Path.GetExtension(filePath ?? "");
                        if (string.Equals(ext, ".rte", StringComparison.OrdinalIgnoreCase)
                            || string.Equals(ext, ".lua", StringComparison.OrdinalIgnoreCase))
                        {
                            if (!TryConvertRouteToolLuaToJson(raw, out string rteJson))
                            {
                                return false;
                            }

                            JObject rteParsed = JObject.Parse(rteJson);
                            json = rteParsed.ToString(Formatting.None);
                            sourceType = "RTE";
                            return true;
                        }

                        JObject parsed = JObject.Parse(raw);
                        if (parsed == null)
                        {
                            return false;
                        }

                        JToken dataToken;
                        if (parsed.TryGetValue("data", StringComparison.OrdinalIgnoreCase, out dataToken))
                        {
                            if (dataToken == null || dataToken.Type != JTokenType.Object)
                            {
                                return false;
                            }
                        }

                        json = parsed.ToString(Formatting.None);
                        sourceType = "DTC";
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }

                private static bool TryReadRouteSelection(string luaFilePath, string routeName, out string json, out string sourceType)
                {
                    json = "";
                    sourceType = "";

                    try
                    {
                        if (string.IsNullOrWhiteSpace(luaFilePath) || !File.Exists(luaFilePath))
                        {
                            return false;
                        }

                        string raw = File.ReadAllText(luaFilePath);
                        if (!TryConvertRouteToolLuaToJson(raw, out string rteJson))
                        {
                            return false;
                        }

                        JObject parsed = JObject.Parse(rteJson);
                        JObject data = parsed["data"] as JObject;
                        if (data == null)
                        {
                            return false;
                        }

                        JToken selectedRoute;
                        if (!data.TryGetValue(routeName ?? "", StringComparison.OrdinalIgnoreCase, out selectedRoute))
                        {
                            return false;
                        }

                        JObject wrapped = new JObject();
                        JObject routeContainer = new JObject();
                        routeContainer[routeName] = selectedRoute;
                        wrapped["data"] = routeContainer;

                        json = wrapped.ToString(Formatting.None);
                        sourceType = "RTE";
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }

                private static bool TryParseRouteSelectionToken(string token, out string luaFilePath, out string routeName)
                {
                    luaFilePath = "";
                    routeName = "";

                    try
                    {
                        string text = token ?? "";
                        if (!text.StartsWith(RouteSelectionPrefix, StringComparison.Ordinal))
                        {
                            return false;
                        }

                        int sep = text.IndexOf("::", RouteSelectionPrefix.Length, StringComparison.Ordinal);
                        if (sep < 0)
                        {
                            return false;
                        }

                        string encodedRoute = text.Substring(RouteSelectionPrefix.Length, sep - RouteSelectionPrefix.Length);
                        routeName = Uri.UnescapeDataString(encodedRoute ?? "");
                        luaFilePath = text.Substring(sep + 2);

                        return !string.IsNullOrWhiteSpace(routeName) && !string.IsNullOrWhiteSpace(luaFilePath);
                    }
                    catch
                    {
                        return false;
                    }
                }

                private static List<string> ExtractRouteNamesFromLua(string luaFilePath)
                {
                    List<string> routes = new List<string>();

                    try
                    {
                        if (string.IsNullOrWhiteSpace(luaFilePath) || !File.Exists(luaFilePath))
                        {
                            return routes;
                        }

                        string raw = File.ReadAllText(luaFilePath);
                        if (!TryConvertRouteToolLuaToJson(raw, out string rteJson))
                        {
                            return routes;
                        }

                        JObject parsed = JObject.Parse(rteJson);
                        JObject data = parsed["data"] as JObject;
                        if (data == null)
                        {
                            return routes;
                        }

                        routes = data.Properties()
                            .Select(p => p.Name)
                            .Where(n => !string.IsNullOrWhiteSpace(n))
                            .Distinct(StringComparer.OrdinalIgnoreCase)
                            .OrderBy(n => n, StringComparer.OrdinalIgnoreCase)
                            .ToList();
                    }
                    catch
                    {
                    }

                    return routes;
                }

                private static bool TryConvertRouteToolLuaToJson(string luaText, out string json)
                {
                    json = "";

                    try
                    {
                        if (string.IsNullOrWhiteSpace(luaText))
                        {
                            return false;
                        }

                        string s = luaText;
                        s = Regex.Replace(s, @"--.*?$", "", RegexOptions.Multiline);
                        s = Regex.Replace(s, @"\bend of\b[\s\S]*$", "", RegexOptions.IgnoreCase);
                        s = Regex.Replace(s, @"\bpresets\s*=", "", RegexOptions.IgnoreCase);
                        s = s.Replace("\r", " ").Replace("\n", " ");
                        s = Regex.Replace(s, @"\s+", " ");

                        s = Regex.Replace(s, @"\[(\d+)\]\s*=", "\"$1\":");
                        s = Regex.Replace(s, @"\[\s*""((?:\\.|[^""\\])*)""\s*\]\s*=", "\"$1\":");
                        s = Regex.Replace(s, @"\b([A-Za-z_][A-Za-z0-9_]*)\s*=", "\"$1\":");

                        s = s.Replace("'", "\"");
                        s = Regex.Replace(s, @",\s*([}\]])", "$1");
                        s = s.Trim();

                        if (!s.StartsWith("{", StringComparison.Ordinal))
                        {
                            return false;
                        }

                        JToken parsed = JToken.Parse(s);
                        JObject wrapped = new JObject
                        {
                            ["data"] = parsed
                        };

                        json = wrapped.ToString(Formatting.None);
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }

                private static OpenKneeboardServerSnapshot BuildServerSnapshot()
                {
                    OpenKneeboardServerSnapshot server = new OpenKneeboardServerSnapshot();

                    try
                    {
                        if (State.currentstate != null)
                        {
                            server.Theater = State.currentstate.theatre;
                            server.DcsLocation = !string.IsNullOrWhiteSpace(State.currentstate.dcsversion)
                                ? State.currentstate.dcsversion
                                : (!string.IsNullOrWhiteSpace(State.currentstate.clientversion)
                                    ? State.currentstate.clientversion
                                    : State.currentstate.root);
                            server.Aircraft = State.currentstate.id;
                            server.PlayerUsername = State.currentstate.playerusername;
                            server.PlayerCallsign = State.currentstate.playercallsign;
                            server.MissionTitle = State.currentstate.missiontitle;
                            server.MissionBriefing = State.currentstate.missionbriefing;
                            server.MissionDetails = State.currentstate.missiondetails;
                            double missionStartSeconds = 0;
                            double missionElapsedSeconds = State.currentstate.timer;
                            bool hasMissionStart = double.TryParse(State.currentstate.sortie ?? "", out missionStartSeconds);
                            if (hasMissionStart && missionElapsedSeconds >= 0)
                            {
                                server.MissionTimeSeconds = missionStartSeconds + missionElapsedSeconds;
                            }
                            else
                            {
                                server.MissionTimeSeconds = State.currentstate.tod;
                            }
                            server.PlayerPosX = State.currentstate.bpos == null ? 0 : State.currentstate.bpos.x;
                            server.PlayerPosY = State.currentstate.bpos == null ? 0 : State.currentstate.bpos.z;
                            server.PlayerAltFeet = State.currentstate.bpos == null ? 0 : (State.currentstate.bpos.y * 3.28084);
                            server.Multiplayer = State.currentstate.multiplayer;
                            server.DebugMode = State.activeconfig.Debugmode;
                            server.Payload = State.currentstate.payload;
                            server.Radios = State.currentstate.radios == null
                                ? new List<Servers.Server.RadioDevice>()
                                : new List<Servers.Server.RadioDevice>(State.currentstate.radios);
                            server.AtcMetars = State.currentstate.atcmetars == null
                                ? new Dictionary<string, string>()
                                : new Dictionary<string, string>(State.currentstate.atcmetars);
                            server.AtcIcaoTypes = State.currentstate.atcicaotypes == null
                                ? new Dictionary<string, string>()
                                : new Dictionary<string, string>(State.currentstate.atcicaotypes);
                            server.Diagnostics = State.currentstate.diagnostics;
                            server.FlightMembers = BuildFlightMemberSnapshot();
                            server.FriendlyAssets = BuildFriendlyAssetsSnapshot();
                            server.MapMarkers = BuildMapMarkerSnapshot();
                        }
                    }
                    catch
                    {
                    }

                    return server;
                }

                private static List<OpenKneeboardFlightMember> BuildFlightMemberSnapshot()
                {
                    List<OpenKneeboardFlightMember> members = new List<OpenKneeboardFlightMember>();

                    try
                    {
                        if (State.currentstate == null)
                        {
                            return members;
                        }

                        string playerCallsign = string.IsNullOrWhiteSpace(State.currentstate.playercallsign) ? "" : State.currentstate.playercallsign.Trim();
                        string playerName = string.IsNullOrWhiteSpace(State.currentstate.playerusername) ? "" : State.currentstate.playerusername.Trim();
                        int playerJet = ParseJetPositionFromCallsign(playerCallsign);

                        if (!string.IsNullOrWhiteSpace(playerCallsign) || !string.IsNullOrWhiteSpace(playerName))
                        {
                            members.Add(new OpenKneeboardFlightMember
                            {
                                Callsign = playerCallsign,
                                Pilot = playerName,
                                Jet = playerJet > 0 ? playerJet : 1,
                            });
                        }

                        List<Servers.Server.DcsUnit> flightUnits = null;
                        if (State.currentstate.availablerecipients != null)
                        {
                            State.currentstate.availablerecipients.TryGetValue("Flight", out flightUnits);
                        }

                        if (flightUnits != null)
                        {
                            foreach (Servers.Server.DcsUnit unit in flightUnits
                                .Where(u => u != null)
                                .OrderBy(u => u.range)
                                .ThenBy(u => u.callsign, StringComparer.OrdinalIgnoreCase))
                            {
                                string callsign = string.IsNullOrWhiteSpace(unit.callsign)
                                    ? (unit.fullname ?? "")
                                    : unit.callsign.Trim();

                                string pilot = "";
                                if (unit.ishuman)
                                {
                                    pilot = string.IsNullOrWhiteSpace(unit.playerid) ? "" : unit.playerid.Trim();
                                }

                                int jet = ParseJetPositionFromCallsign(callsign);
                                UpsertFlightMember(members, callsign, pilot, jet);
                            }
                        }

                        FillMissingFlightJets(members);

                        members = members
                            .Where(m => !string.IsNullOrWhiteSpace(m.Callsign) || !string.IsNullOrWhiteSpace(m.Pilot))
                            .OrderBy(m => m.Jet <= 0 ? 99 : m.Jet)
                            .ThenBy(m => m.Callsign, StringComparer.OrdinalIgnoreCase)
                            .Take(8)
                            .ToList();
                    }
                    catch
                    {
                    }

                    return members;
                }

                private static void UpsertFlightMember(List<OpenKneeboardFlightMember> members, string callsign, string pilot, int jet)
                {
                    if (members == null)
                    {
                        return;
                    }

                    string safeCallsign = string.IsNullOrWhiteSpace(callsign) ? "" : callsign.Trim();
                    string safePilot = string.IsNullOrWhiteSpace(pilot) ? "" : pilot.Trim();
                    if (string.IsNullOrWhiteSpace(safeCallsign) && string.IsNullOrWhiteSpace(safePilot))
                    {
                        return;
                    }

                    OpenKneeboardFlightMember existing = null;
                    if (!string.IsNullOrWhiteSpace(safeCallsign))
                    {
                        existing = members.FirstOrDefault(m => string.Equals(m.Callsign, safeCallsign, StringComparison.OrdinalIgnoreCase));
                    }

                    if (existing == null)
                    {
                        existing = new OpenKneeboardFlightMember
                        {
                            Callsign = safeCallsign,
                            Pilot = safePilot,
                            Jet = jet > 0 ? jet : 0,
                        };
                        members.Add(existing);
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(existing.Pilot) && !string.IsNullOrWhiteSpace(safePilot))
                    {
                        existing.Pilot = safePilot;
                    }

                    if (string.IsNullOrWhiteSpace(existing.Callsign) && !string.IsNullOrWhiteSpace(safeCallsign))
                    {
                        existing.Callsign = safeCallsign;
                    }

                    if ((existing.Jet <= 0) && jet > 0)
                    {
                        existing.Jet = jet;
                    }
                }

                private static int ParseJetPositionFromCallsign(string callsign)
                {
                    try
                    {
                        string text = (callsign ?? "").Trim();
                        if (text.Length == 0)
                        {
                            return 0;
                        }

                        Match pair = Regex.Match(text, @"(\d{2})\s*$");
                        if (pair.Success)
                        {
                            string value = pair.Groups[1].Value;
                            if (value.Length >= 2)
                            {
                                int jet;
                                if (int.TryParse(value.Substring(value.Length - 1, 1), out jet) && jet > 0 && jet <= 9)
                                {
                                    return jet;
                                }
                            }
                        }

                        Match single = Regex.Match(text, @"(\d)\s*$");
                        if (single.Success)
                        {
                            int jet;
                            if (int.TryParse(single.Groups[1].Value, out jet) && jet > 0 && jet <= 9)
                            {
                                return jet;
                            }
                        }
                    }
                    catch
                    {
                    }

                    return 0;
                }

                private static void FillMissingFlightJets(List<OpenKneeboardFlightMember> members)
                {
                    if (members == null || members.Count == 0)
                    {
                        return;
                    }

                    HashSet<int> used = new HashSet<int>();
                    foreach (OpenKneeboardFlightMember member in members)
                    {
                        if (member != null && member.Jet > 0)
                        {
                            used.Add(member.Jet);
                        }
                    }

                    int fallback = 1;
                    foreach (OpenKneeboardFlightMember member in members)
                    {
                        if (member == null || member.Jet > 0)
                        {
                            continue;
                        }

                        while (used.Contains(fallback))
                        {
                            fallback++;
                        }

                        member.Jet = fallback;
                        used.Add(fallback);
                    }
                }

                private static bool IsLikelyAwacsUnit(Servers.Server.DcsUnit unit)
                {
                    if (unit == null)
                    {
                        return false;
                    }

                    string callsign = (unit.callsign ?? "").Trim();
                    bool isKnownAwacsCallsign = callsign.IndexOf("Darkstar", StringComparison.OrdinalIgnoreCase) >= 0
                        || callsign.IndexOf("Focus", StringComparison.OrdinalIgnoreCase) >= 0
                        || callsign.IndexOf("Magic", StringComparison.OrdinalIgnoreCase) >= 0
                        || callsign.IndexOf("Overlord", StringComparison.OrdinalIgnoreCase) >= 0
                        || callsign.IndexOf("Wizard", StringComparison.OrdinalIgnoreCase) >= 0;

                    string typeSource = ((unit.typename ?? "") + " " + (unit.fullname ?? "")).ToUpperInvariant();
                    bool isAwacsType = typeSource.Contains("HAWKEYE")
                        || typeSource.Contains("SENTRY")
                        || typeSource.Contains("WEDGETAIL")
                        || Regex.IsMatch(typeSource, @"(^|[^A-Z0-9])E[-\s]?2[A-Z]?([^A-Z0-9]|$)")
                        || Regex.IsMatch(typeSource, @"(^|[^A-Z0-9])E[-\s]?3[A-Z]?([^A-Z0-9]|$)")
                        || Regex.IsMatch(typeSource, @"(^|[^A-Z0-9])E[-\s]?7[A-Z]?([^A-Z0-9]|$)")
                        || Regex.IsMatch(typeSource, @"(^|[^A-Z0-9])A[-\s]?50([^A-Z0-9]|$)")
                        || Regex.IsMatch(typeSource, @"(^|[^A-Z0-9])KJ[-\s]?2000([^A-Z0-9]|$)")
                        || Regex.IsMatch(typeSource, @"(^|[^A-Z0-9])KJ[-\s]?500([^A-Z0-9]|$)");

                    return isKnownAwacsCallsign || isAwacsType;
                }

                private static string ResolveAtcIcaoType(string callsign, string atcName)
                {
                    try
                    {
                        if (State.currentstate == null || State.currentstate.atcicaotypes == null)
                        {
                            return string.Empty;
                        }

                        string keyCallsign = NormalizeAtcLookupKey(callsign);
                        string keyName = NormalizeAtcLookupKey(atcName);

                        string type;
                        if (!string.IsNullOrWhiteSpace(keyCallsign)
                            && State.currentstate.atcicaotypes.TryGetValue(keyCallsign, out type))
                        {
                            return NormalizeAtcIcaoTypeValue(type);
                        }

                        if (!string.IsNullOrWhiteSpace(keyName)
                            && State.currentstate.atcicaotypes.TryGetValue(keyName, out type))
                        {
                            return NormalizeAtcIcaoTypeValue(type);
                        }

                        foreach (KeyValuePair<string, string> pair in State.currentstate.atcicaotypes)
                        {
                            string normalizedKey = NormalizeAtcLookupKey(pair.Key);
                            if (!string.IsNullOrWhiteSpace(keyCallsign)
                                && string.Equals(normalizedKey, keyCallsign, StringComparison.Ordinal))
                            {
                                return NormalizeAtcIcaoTypeValue(pair.Value);
                            }

                            if (!string.IsNullOrWhiteSpace(keyName)
                                && string.Equals(normalizedKey, keyName, StringComparison.Ordinal))
                            {
                                return NormalizeAtcIcaoTypeValue(pair.Value);
                            }
                        }
                    }
                    catch
                    {
                    }

                    return string.Empty;
                }

                private static string NormalizeAtcLookupKey(string value)
                {
                    string s = (value ?? string.Empty).Trim().ToUpperInvariant();
                    if (string.IsNullOrWhiteSpace(s))
                    {
                        return string.Empty;
                    }

                    s = Regex.Replace(s, @"[_\-/\.,\(\)]", " ");
                    s = Regex.Replace(s, @"\s+", " ").Trim();
                    return s;
                }

                private static string NormalizeAtcIcaoTypeValue(string value)
                {
                    string t = (value ?? string.Empty).Trim().ToUpperInvariant();
                    return (t == "MIL" || t == "CIV" || t == "JOINT") ? t : string.Empty;
                }

                private static List<OpenKneeboardMapMarker> BuildMapMarkerSnapshot()
                {
                    List<OpenKneeboardMapMarker> markers = new List<OpenKneeboardMapMarker>();

                    try
                    {
                        if (State.currentstate == null
                            || State.currentstate.riostate == null
                            || State.currentstate.riostate.markerdetails == null)
                        {
                            return markers;
                        }

                        foreach (Servers.Server.MapMarkerState marker in State.currentstate.riostate.markerdetails)
                        {
                            if (marker == null)
                            {
                                continue;
                            }

                            if (double.IsNaN(marker.x)
                                || double.IsInfinity(marker.x)
                                || double.IsNaN(marker.z)
                                || double.IsInfinity(marker.z))
                            {
                                continue;
                            }

                            markers.Add(new OpenKneeboardMapMarker
                            {
                                Id = marker.id,
                                Text = marker.text ?? string.Empty,
                                Author = marker.author ?? string.Empty,
                                Coalition = marker.coalition,
                                X = marker.x,
                                Y = marker.y,
                                Z = marker.z,
                            });

                            if (markers.Count >= 64)
                            {
                                break;
                            }
                        }
                    }
                    catch
                    {
                    }

                    return markers;
                }

                private static List<OpenKneeboardFriendlyAsset> BuildFriendlyAssetsSnapshot()
                {
                    List<OpenKneeboardFriendlyAsset> assets = new List<OpenKneeboardFriendlyAsset>();

                    try
                    {
                        if (State.currentstate == null || State.currentstate.availablerecipients == null)
                        {
                            return assets;
                        }

                        string playerCoalition = (State.currentstate.playercoalition ?? string.Empty).Trim();
                        string[] categories = new[] { "Player", "Flight", "Tanker", "AWACS", "JTAC", "ATC", "Allies" };
                        HashSet<string> seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                        if (State.currentstate.bpos != null)
                        {
                            double playerNorth = State.currentstate.bpos.x;
                            double playerEast = State.currentstate.bpos.z;
                            if (!double.IsNaN(playerNorth) && !double.IsInfinity(playerNorth) && !double.IsNaN(playerEast) && !double.IsInfinity(playerEast))
                            {
                                string playerCallsign = string.IsNullOrWhiteSpace(State.currentstate.playercallsign) ? "PLAYER" : State.currentstate.playercallsign;
                                string playerName = string.IsNullOrWhiteSpace(State.currentstate.id) ? "PLAYER" : State.currentstate.id;
                                string playerKey = string.Format("{0}|{1}|{2}|{3}", playerCallsign, playerName, Math.Round(playerNorth), Math.Round(playerEast));
                                seen.Add(playerKey);
                                assets.Add(new OpenKneeboardFriendlyAsset
                                {
                                    Callsign = playerCallsign,
                                    Name = playerName,
                                    Category = "PLAYER",
                                    TypeName = string.IsNullOrWhiteSpace(State.currentstate.id) ? "" : State.currentstate.id,
                                    Frequency = "",
                                    Tacan = "",
                                    MpClientCallsign = string.IsNullOrWhiteSpace(State.currentstate.playerusername) ? "" : State.currentstate.playerusername,
                                    RawLine = string.Empty,
                                    X = playerNorth,
                                    Y = playerEast,
                                    AltFeet = State.currentstate.bpos.y * 3.28084,
                                });
                            }
                        }

                        foreach (string category in categories)
                        {
                            List<Servers.Server.DcsUnit> units;
                            if (!State.currentstate.availablerecipients.TryGetValue(category, out units) || units == null)
                            {
                                continue;
                            }

                            bool categoryIsAtc = category.Equals("ATC", StringComparison.OrdinalIgnoreCase);

                            foreach (Servers.Server.DcsUnit unit in units)
                            {
                                if (unit == null || unit.pos == null)
                                {
                                    continue;
                                }

                                if (!string.IsNullOrWhiteSpace(playerCoalition))
                                {
                                    string unitCoalition = (unit.coalition ?? string.Empty).Trim();
                                    if (!string.IsNullOrWhiteSpace(unitCoalition)
                                        && !categoryIsAtc
                                        && !playerCoalition.Equals(unitCoalition, StringComparison.OrdinalIgnoreCase))
                                    {
                                        continue;
                                    }
                                }

                                double x = unit.pos.x;
                                double y = unit.pos.z;
                                if (double.IsNaN(x) || double.IsInfinity(x) || double.IsNaN(y) || double.IsInfinity(y))
                                {
                                    continue;
                                }

                                string callsign = string.IsNullOrWhiteSpace(unit.callsign) ? unit.fullname : unit.callsign;
                                string name = string.IsNullOrWhiteSpace(unit.fullname) ? unit.callsign : unit.fullname;
                                string dedupeKey = string.Format("{0}|{1}|{2}|{3}", callsign ?? "", name ?? "", Math.Round(x), Math.Round(y));
                                if (seen.Contains(dedupeKey))
                                {
                                    continue;
                                }
                                seen.Add(dedupeKey);

                                string normalizedCategory = category.ToUpperInvariant();
                                if (normalizedCategory.Equals("AWACS", StringComparison.OrdinalIgnoreCase)
                                    && !IsLikelyAwacsUnit(unit))
                                {
                                    normalizedCategory = "FLIGHT";
                                }

                                assets.Add(new OpenKneeboardFriendlyAsset
                                {
                                    Callsign = callsign ?? "",
                                    Name = name ?? "",
                                    Category = normalizedCategory,
                                    TypeName = string.IsNullOrWhiteSpace(unit.typename) ? "" : unit.typename,
                                    Frequency = string.IsNullOrWhiteSpace(unit.freq) ? "" : unit.freq,
                                    IcaoType = ResolveAtcIcaoType(callsign, name),
                                    AltFrequencies = unit.altfreq == null
                                        ? new List<string>()
                                        : unit.altfreq.Where(f => !string.IsNullOrWhiteSpace(f)).ToList(),
                                    Tacan = string.IsNullOrWhiteSpace(unit.tacan) ? "" : unit.tacan,
                                    MpClientCallsign = unit.ishuman && !string.IsNullOrWhiteSpace(unit.playerid) ? unit.playerid : "",
                                    RawLine = string.Empty,
                                    X = x,
                                    Y = y,
                                    AltFeet = unit.pos.y * 3.28084,
                                });

                                if (assets.Count >= 64)
                                {
                                    return assets;
                                }
                            }
                        }
                    }
                    catch
                    {
                    }

                    return assets;
                }
            }

            public class OpenKneeboardSnapshot
            {
                public string ActiveCategory { get; set; } = "LOG";
                public string NotesBuffer { get; set; } = "";
                public string AiCrewPhase { get; set; } = "Unknown";
                public DateTime UpdatedUtc { get; set; } = DateTime.UtcNow;
                public OpenKneeboardServerSnapshot Server { get; set; } = new OpenKneeboardServerSnapshot();
                public OpenKneeboardStatusSnapshot Status { get; set; } = new OpenKneeboardStatusSnapshot();
                public List<string> AiCrewKeywords { get; set; } = new List<string>();
                public Dictionary<string, List<string>> AiCrewKeywordSections { get; set; } = new Dictionary<string, List<string>>();
                public List<string> RawServerMessages { get; set; } = new List<string>();
                public Dictionary<string, string> Logs { get; set; } = new Dictionary<string, string>();
                public Dictionary<string, List<string>> Units { get; set; } = new Dictionary<string, List<string>>();
                public Dictionary<string, List<string>> UnitDetails { get; set; } = new Dictionary<string, List<string>>();
                public Dictionary<string, Dictionary<string, List<string>>> AliasesChunk0 { get; set; } = new Dictionary<string, Dictionary<string, List<string>>>();
                public Dictionary<string, Dictionary<string, List<string>>> AliasesChunk1 { get; set; } = new Dictionary<string, Dictionary<string, List<string>>>();
                public OpenKneeboardNavigraphEfbState.OpenKneeboardEfbSnapshot Efb { get; set; } = new OpenKneeboardNavigraphEfbState.OpenKneeboardEfbSnapshot();
                public List<string> DtcFiles { get; set; } = new List<string>();
                public string DtcSelectedFile { get; set; } = "";
                public string DtcJson { get; set; } = "";
                public string DtcSourceType { get; set; } = "";

                public OpenKneeboardSnapshot Clone()
                {
                    OpenKneeboardSnapshot clone = new OpenKneeboardSnapshot
                    {
                        ActiveCategory = ActiveCategory,
                        NotesBuffer = NotesBuffer,
                        AiCrewPhase = AiCrewPhase,
                        UpdatedUtc = UpdatedUtc,
                        Server = Server == null ? new OpenKneeboardServerSnapshot() : Server.Clone(),
                        Status = Status == null ? new OpenKneeboardStatusSnapshot() : Status.Clone(),
                        AiCrewKeywords = new List<string>(AiCrewKeywords ?? new List<string>()),
                        AiCrewKeywordSections = CloneListMap(AiCrewKeywordSections),
                        RawServerMessages = new List<string>(RawServerMessages ?? new List<string>()),
                        Logs = new Dictionary<string, string>(Logs),
                        Units = CloneListMap(Units),
                        UnitDetails = CloneListMap(UnitDetails),
                        AliasesChunk0 = CloneAliasMap(AliasesChunk0),
                        AliasesChunk1 = CloneAliasMap(AliasesChunk1),
                        Efb = Efb == null ? new OpenKneeboardNavigraphEfbState.OpenKneeboardEfbSnapshot() : Efb.Clone(),
                        DtcFiles = new List<string>(DtcFiles ?? new List<string>()),
                        DtcSelectedFile = DtcSelectedFile,
                        DtcJson = DtcJson,
                        DtcSourceType = DtcSourceType,
                    };

                    return clone;
                }

                private static Dictionary<string, List<string>> CloneListMap(Dictionary<string, List<string>> source)
                {
                    Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();

                    foreach (KeyValuePair<string, List<string>> pair in source)
                    {
                        result[pair.Key] = pair.Value == null ? new List<string>() : new List<string>(pair.Value);
                    }

                    return result;
                }

                private static Dictionary<string, Dictionary<string, List<string>>> CloneAliasMap(Dictionary<string, Dictionary<string, List<string>>> source)
                {
                    Dictionary<string, Dictionary<string, List<string>>> result = new Dictionary<string, Dictionary<string, List<string>>>();

                    foreach (KeyValuePair<string, Dictionary<string, List<string>>> categoryPair in source)
                    {
                        Dictionary<string, List<string>> categoryValues = new Dictionary<string, List<string>>();
                        foreach (KeyValuePair<string, List<string>> aliasPair in categoryPair.Value)
                        {
                            categoryValues[aliasPair.Key] = aliasPair.Value == null ? new List<string>() : new List<string>(aliasPair.Value);
                        }

                        result[categoryPair.Key] = categoryValues;
                    }

                    return result;
                }
            }

            public class OpenKneeboardStatusSnapshot
            {
                public string Text { get; set; } = "";
                public string Level { get; set; } = "";
                public DateTime UpdatedUtc { get; set; } = DateTime.UtcNow;

                public OpenKneeboardStatusSnapshot Clone()
                {
                    return new OpenKneeboardStatusSnapshot
                    {
                        Text = Text,
                        Level = Level,
                        UpdatedUtc = UpdatedUtc,
                    };
                }
            }

            public class OpenKneeboardServerSnapshot
            {
                public string Theater { get; set; } = "";
                public string DcsLocation { get; set; } = "";
                public string Aircraft { get; set; } = "";
                public string PlayerUsername { get; set; } = "";
                public string PlayerCallsign { get; set; } = "";
                public string MissionTitle { get; set; } = "";
                public string MissionBriefing { get; set; } = "";
                public string MissionDetails { get; set; } = "";
                public double MissionTimeSeconds { get; set; }
                public double PlayerPosX { get; set; }
                public double PlayerPosY { get; set; }
                public double PlayerAltFeet { get; set; }
                public bool Multiplayer { get; set; }
                public bool DebugMode { get; set; }
                public object Payload { get; set; } = null;
                public List<Servers.Server.RadioDevice> Radios { get; set; } = new List<Servers.Server.RadioDevice>();
                public Dictionary<string, string> AtcMetars { get; set; } = new Dictionary<string, string>();
                public Dictionary<string, string> AtcIcaoTypes { get; set; } = new Dictionary<string, string>();
                public List<OpenKneeboardFlightMember> FlightMembers { get; set; } = new List<OpenKneeboardFlightMember>();
                public List<OpenKneeboardFriendlyAsset> FriendlyAssets { get; set; } = new List<OpenKneeboardFriendlyAsset>();
                public object Diagnostics { get; set; } = null;
                public List<OpenKneeboardMapMarker> MapMarkers { get; set; } = new List<OpenKneeboardMapMarker>();

                public OpenKneeboardServerSnapshot Clone()
                {
                    return new OpenKneeboardServerSnapshot
                    {
                        Theater = Theater,
                        DcsLocation = DcsLocation,
                        Aircraft = Aircraft,
                        PlayerUsername = PlayerUsername,
                        PlayerCallsign = PlayerCallsign,
                        MissionTitle = MissionTitle,
                        MissionBriefing = MissionBriefing,
                        MissionDetails = MissionDetails,
                        MissionTimeSeconds = MissionTimeSeconds,
                        PlayerPosX = PlayerPosX,
                        PlayerPosY = PlayerPosY,
                        PlayerAltFeet = PlayerAltFeet,
                        Multiplayer = Multiplayer,
                        DebugMode = DebugMode,
                        Payload = Payload,
                        Radios = Radios == null ? new List<Servers.Server.RadioDevice>() : new List<Servers.Server.RadioDevice>(Radios),
                        AtcMetars = new Dictionary<string, string>(AtcMetars ?? new Dictionary<string, string>()),
                        AtcIcaoTypes = new Dictionary<string, string>(AtcIcaoTypes ?? new Dictionary<string, string>()),
                        FlightMembers = FlightMembers == null
                            ? new List<OpenKneeboardFlightMember>()
                            : FlightMembers.ConvertAll(member => member == null ? null : member.Clone()).FindAll(member => member != null),
                        FriendlyAssets = FriendlyAssets == null
                            ? new List<OpenKneeboardFriendlyAsset>()
                            : FriendlyAssets.ConvertAll(functionAsset => functionAsset == null ? null : functionAsset.Clone()).FindAll(functionAsset => functionAsset != null),
                        MapMarkers = MapMarkers == null
                            ? new List<OpenKneeboardMapMarker>()
                            : MapMarkers.ConvertAll(marker => marker == null ? null : marker.Clone()).FindAll(marker => marker != null),
                        Diagnostics = Diagnostics,
                    };
                }
            }

            public class OpenKneeboardMapMarker
            {
                public int Id { get; set; }
                public string Text { get; set; } = "";
                public string Author { get; set; } = "";
                public int Coalition { get; set; }
                public double X { get; set; }
                public double Y { get; set; }
                public double Z { get; set; }

                public OpenKneeboardMapMarker Clone()
                {
                    return new OpenKneeboardMapMarker
                    {
                        Id = Id,
                        Text = Text,
                        Author = Author,
                        Coalition = Coalition,
                        X = X,
                        Y = Y,
                        Z = Z,
                    };
                }
            }

            public class OpenKneeboardFlightMember
            {
                public string Pilot { get; set; } = "";
                public string Callsign { get; set; } = "";
                public int Jet { get; set; }

                public OpenKneeboardFlightMember Clone()
                {
                    return new OpenKneeboardFlightMember
                    {
                        Pilot = Pilot,
                        Callsign = Callsign,
                        Jet = Jet,
                    };
                }
            }

            public class OpenKneeboardFriendlyAsset
            {
                public string Callsign { get; set; } = "";
                public string Name { get; set; } = "";
                public string Category { get; set; } = "";
                public string TypeName { get; set; } = "";
                public string Frequency { get; set; } = "";
                public string IcaoType { get; set; } = "";
                public List<string> AltFrequencies { get; set; } = new List<string>();
                public string Tacan { get; set; } = "";
                public string MpClientCallsign { get; set; } = "";
                public string RawLine { get; set; } = "";
                public double X { get; set; }
                public double Y { get; set; }
            public double AltFeet { get; set; }

                public OpenKneeboardFriendlyAsset Clone()
                {
                    return new OpenKneeboardFriendlyAsset
                    {
                        Callsign = Callsign,
                        Name = Name,
                        Category = Category,
                        TypeName = TypeName,
                        Frequency = Frequency,
                        IcaoType = IcaoType,
                        AltFrequencies = AltFrequencies == null ? new List<string>() : new List<string>(AltFrequencies),
                        Tacan = Tacan,
                        MpClientCallsign = MpClientCallsign,
                        RawLine = RawLine,
                        X = X,
                        Y = Y,
                        AltFeet = AltFeet,
                    };
                }
            }
        }
    }
}
