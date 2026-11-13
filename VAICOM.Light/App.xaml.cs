using System;
using System.IO;
using System.Windows;
using VAICOM.Client;
using VAICOM.Extensions.Kneeboard;
using VAICOM.FileManager;
using VAICOM.Interfaces;
using VAICOM.UI;

namespace VAICOM.Light
{
    public partial class App : Application
    {
        private string lightDataRoot;
        private const string LuaTargetSettingsFileName = "lua-targets.settings";
        private const string LuaAutoInstallMarkerFileName = "lua-auto-install-v1.done";
        private const string LuaActionLogFileName = "lua-actions.log";

        public static string LightDataRootPath { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            lightDataRoot = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "VAICOM-Light");
            LightDataRootPath = lightDataRoot;
            Directory.CreateDirectory(lightDataRoot);
            string vaAppsRoot = Path.Combine(lightDataRoot, "Apps");
            Directory.CreateDirectory(vaAppsRoot);

            string vaDir = AppDomain.CurrentDomain.BaseDirectory?.TrimEnd('\\') ?? string.Empty;
            string vaSounds = Path.Combine(vaDir, "Sounds");

            State.VA_DIR = vaDir;
            State.VA_APPS = vaAppsRoot;
            State.VA_SOUNDS = vaSounds;
            State.Proxy = new LightProxyShim(vaAppsRoot);

            if (State.activeconfig == null)
            {
                State.activeconfig = VAICOM.Settings.Configs.Defaultconfig;
            }

            DcsClient.ResetStateValues();

            State.activeconfig.OpenKneeboard_Out = true;
            State.activeconfig.OpenKneeboard_Out_Port = 7779;
            State.activeconfig.OpenKneeboard_Out_AllowLan = false;
            State.activeconfig.OpenKneeboard_AutoBrowse = false;
            State.activeconfig.OpenKneeboard_EfbEnabled = false;
            State.activeconfig.ImportOtherMenu = false;
            State.activeconfig.DisablePlayerVoice = false;
            State.activeconfig.HideOnScreenText = false;
            State.activeconfig.ForceLanguage = false;
            State.activeconfig.ForcedLanguage = 0;
            State.activeconfig.HideMenus = false;
            State.activeconfig.EnforceATCProtocol = false;
            State.activeconfig.EnforceCallsigns = false;
            State.activeconfig.CallsignsLanguage = 0;
            State.activeconfig.OperateDial = false;
            State.activeconfig.Redirect_World_Speech = false;
            State.activeconfig.CarrierSuppressAuto = false;
            State.activeconfig.OpenKneeboard_EfbOAuthScope = string.IsNullOrWhiteSpace(State.activeconfig.OpenKneeboard_EfbOAuthScope)
                ? "openid charts tiles offline_access"
                : State.activeconfig.OpenKneeboard_EfbOAuthScope;
            State.activeconfig.OpenKneeboard_EfbOAuthDeviceAuthEndpoint = string.IsNullOrWhiteSpace(State.activeconfig.OpenKneeboard_EfbOAuthDeviceAuthEndpoint)
                ? "https://identity.api.navigraph.com/connect/deviceauthorization"
                : State.activeconfig.OpenKneeboard_EfbOAuthDeviceAuthEndpoint;
            State.activeconfig.OpenKneeboard_EfbOAuthTokenEndpoint = string.IsNullOrWhiteSpace(State.activeconfig.OpenKneeboard_EfbOAuthTokenEndpoint)
                ? "https://identity.api.navigraph.com/connect/token"
                : State.activeconfig.OpenKneeboard_EfbOAuthTokenEndpoint;
            State.activeconfig.OpenKneeboard_EfbApiBase = string.IsNullOrWhiteSpace(State.activeconfig.OpenKneeboard_EfbApiBase)
                ? "https://api.navigraph.com/v1"
                : State.activeconfig.OpenKneeboard_EfbApiBase;
            State.activeconfig.Debugmode = false;

            OpenKneeboardBridge.SetRuntimeSettingsProvider(() => new OpenKneeboardBridge.OpenKneeboardBridgeRuntimeSettings
            {
                VaAppsRootPath = vaAppsRoot,
                PluginRootFolderName = "VAICOMPRO",
                OpenKneeboardOutEnabled = State.activeconfig != null && State.activeconfig.OpenKneeboard_Out,
                OpenKneeboardOutPort = State.activeconfig != null ? State.activeconfig.OpenKneeboard_Out_Port : 7779,
                OpenKneeboardOutAllowLan = State.activeconfig != null && State.activeconfig.OpenKneeboard_Out_AllowLan,
                DebugMode = State.activeconfig != null && State.activeconfig.Debugmode,
                PluginReadableVersion = "Light",
                PluginSemanticVersion = "0.1.0",
                ConfigSubFolder = "config",
                DocumentationSubFolder = "Documentation",
                ExportSubFolder = "Export",
                LogfilesSubFolder = "LogFiles",
            });

            TryAutoInstallLuaHooks();
            LightUpdateChannel.TryCheckForUpdatesAtStartup();

            try
            {
                Network.UDPsetup();
                Network.UDPstart();

                // Keep update request cadence aligned with Full startup so mission chunks are requested reliably.
                Beacon.Beacon_Initialize();
                Beacon.Beacon_PlayToggle();
                Timers.UI_Timer_Initialize();
                Timers.UI_Timer_Playtoggle();

                OpenKneeboardBridge.Initialize();
            }
            catch
            {
                // Keep UI available even when startup fails; user can retry from UI controls.
            }

            ShutdownMode = ShutdownMode.OnMainWindowClose;
            MainWindow = new MainWindow();
            MainWindow.Show();
        }

        private static string GetConfigFolderPath()
        {
            string root = string.IsNullOrWhiteSpace(LightDataRootPath)
                ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "VAICOM-Light")
                : LightDataRootPath;
            string configFolder = Path.Combine(root, "config");
            Directory.CreateDirectory(configFolder);
            return configFolder;
        }

        private static string GetLuaTargetSettingsPath()
        {
            return Path.Combine(GetConfigFolderPath(), LuaTargetSettingsFileName);
        }

        private static string GetLuaActionLogPath()
        {
            return Path.Combine(GetConfigFolderPath(), LuaActionLogFileName);
        }

        private static void AppendLuaActionLog(string line)
        {
            try
            {
                string entry = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " | " + (line ?? "") + Environment.NewLine;
                File.AppendAllText(GetLuaActionLogPath(), entry);
            }
            catch
            {
            }
        }

        public static string ReadLuaActionLog()
        {
            try
            {
                string path = GetLuaActionLogPath();
                if (!File.Exists(path))
                {
                    return "No Lua file actions logged yet.";
                }

                string[] lines = File.ReadAllLines(path);
                if (lines == null || lines.Length == 0)
                {
                    return "No Lua file actions logged yet.";
                }

                int take = Math.Min(12, lines.Length);
                int start = lines.Length - take;
                return string.Join(Environment.NewLine, lines, start, take);
            }
            catch (Exception ex)
            {
                return "Unable to read Lua action log: " + ex.Message;
            }
        }

        public static void LoadLuaTargetSettings(out bool installStable, out bool installCustom, out string customPath)
        {
            installStable = true;
            installCustom = false;
            customPath = "";

            try
            {
                string settingsPath = GetLuaTargetSettingsPath();
                if (!File.Exists(settingsPath))
                {
                    customPath = State.activeconfig == null ? "" : (State.activeconfig.DCSfoldername1 ?? "");
                    return;
                }

                foreach (string rawLine in File.ReadAllLines(settingsPath))
                {
                    string line = (rawLine ?? "").Trim();
                    if (line.Length == 0 || line.StartsWith("#", StringComparison.Ordinal))
                    {
                        continue;
                    }

                    int idx = line.IndexOf('=');
                    if (idx <= 0)
                    {
                        continue;
                    }

                    string key = line.Substring(0, idx).Trim();
                    string value = line.Substring(idx + 1).Trim();
                    if (key.Equals("installStable", StringComparison.OrdinalIgnoreCase))
                    {
                        installStable = value.Equals("1") || value.Equals("true", StringComparison.OrdinalIgnoreCase);
                    }
                    else if (key.Equals("installCustom", StringComparison.OrdinalIgnoreCase))
                    {
                        installCustom = value.Equals("1") || value.Equals("true", StringComparison.OrdinalIgnoreCase);
                    }
                    else if (key.Equals("customPath", StringComparison.OrdinalIgnoreCase))
                    {
                        customPath = value;
                    }
                }
            }
            catch
            {
            }
        }

        public static void SaveLuaTargetSettings(bool installStable, bool installCustom, string customPath)
        {
            try
            {
                string settingsPath = GetLuaTargetSettingsPath();
                string body = "installStable=" + (installStable ? "1" : "0") + Environment.NewLine
                    + "installCustom=" + (installCustom ? "1" : "0") + Environment.NewLine
                    + "customPath=" + (customPath ?? "");
                File.WriteAllText(settingsPath, body);
            }
            catch
            {
            }
        }

        public static bool ExecuteLuaInstallTargets(bool remove, bool installStable, bool installCustom, string customPath, out string status)
        {
            status = "No Lua targets selected.";

            bool customSelected = installCustom && !string.IsNullOrWhiteSpace(customPath);
            if (!installStable && !customSelected)
            {
                return false;
            }

            bool originalUseCustomFolders = State.activeconfig.UseCustomFolders;
            int originalCustomPathSetting1 = State.activeconfig.Custom_Path_Setting1;
            int originalCustomPathSetting2 = State.activeconfig.Custom_Path_Setting2;
            bool originalCustomFoldersOBFlag = State.activeconfig.CustomFoldersOBFlag;
            string originalDcsFolderName1 = State.activeconfig.DCSfoldername1;

            int appliedTargets = 0;

            try
            {
                Action<bool, string> apply = (useCustom, path) =>
                {
                    string targetLabel = useCustom ? ("Custom: " + (path ?? "")) : "Stable (detected)";
                    AppendLuaActionLog((remove ? "Remove" : "Repair") + " start -> " + targetLabel);

                    State.activeconfig.Custom_Path_Setting1 = 0;
                    State.activeconfig.Custom_Path_Setting2 = 0;
                    State.activeconfig.CustomFoldersOBFlag = false;
                    State.activeconfig.UseCustomFolders = useCustom;
                    State.activeconfig.DCSfoldername1 = useCustom ? (path ?? "") : "";

                    if (remove)
                    {
                        FileHandler.Lua.LuaFiles_Remove();
                        FileHandler.Lua.LuaFiles_Install(true, true);
                    }
                    else
                    {
                        FileHandler.Lua.LuaFiles_Install(false, true);
                    }

                    AppendLuaActionLog((remove ? "Remove" : "Repair") + " complete -> " + targetLabel);
                    appliedTargets++;
                };

                if (installStable)
                {
                    apply(false, "");
                }

                if (customSelected && Directory.Exists(customPath))
                {
                    apply(true, customPath);
                }

                if (appliedTargets == 0)
                {
                    status = installCustom
                        ? "Custom DCS path is not valid."
                        : "No Lua targets selected.";
                    AppendLuaActionLog("Lua action skipped -> " + status);
                    return false;
                }

                status = remove
                    ? "Removed Lua appends for selected targets."
                    : "Lua hooks installed/repaired for selected targets.";
                AppendLuaActionLog("Lua action success -> " + status);
                return true;
            }
            catch (Exception ex)
            {
                status = "Lua action failed: " + ex.Message;
                AppendLuaActionLog("Lua action failed -> " + ex.Message);
                return false;
            }
            finally
            {
                State.activeconfig.UseCustomFolders = originalUseCustomFolders;
                State.activeconfig.Custom_Path_Setting1 = originalCustomPathSetting1;
                State.activeconfig.Custom_Path_Setting2 = originalCustomPathSetting2;
                State.activeconfig.CustomFoldersOBFlag = originalCustomFoldersOBFlag;
                State.activeconfig.DCSfoldername1 = originalDcsFolderName1;
            }
        }

        private void TryAutoInstallLuaHooks()
        {
            try
            {
                string markerPath = Path.Combine(lightDataRoot, LuaAutoInstallMarkerFileName);
                if (File.Exists(markerPath))
                {
                    return;
                }

                LoadLuaTargetSettings(out bool installStable, out bool installCustom, out string customPath);
                bool ok = ExecuteLuaInstallTargets(false, installStable, installCustom, customPath, out string status);
                AppendLuaActionLog("Auto-install -> " + status);
                if (ok)
                {
                    File.WriteAllText(markerPath, DateTime.UtcNow.ToString("o"));
                }
            }
            catch
            {
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            try
            {
                if (State.beaconactive)
                {
                    Beacon.Beacon_PlayToggle();
                }
            }
            catch
            {
            }

            try
            {
                if (State.uitimeractive)
                {
                    Timers.UI_Timer_Playtoggle();
                }
            }
            catch
            {
            }

            try
            {
                OpenKneeboardBridge.Shutdown();
            }
            catch
            {
            }

            OpenKneeboardBridge.UseVoiceAttackRuntimeSettings();
            base.OnExit(e);
        }
    }
}
