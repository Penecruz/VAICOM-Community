using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using VAICOM.Client;
using VAICOM.Extensions.Kneeboard;
using Forms = System.Windows.Forms;

namespace VAICOM.Light
{
    public partial class MainWindow : Window
    {
        private readonly Forms.NotifyIcon trayIcon;
        private readonly DispatcherTimer statusTimer;
        private bool exitRequested;
        private bool bridgeInitialized;
        private DateTime lastDcsUpdateRequestUtc = DateTime.MinValue;

        public MainWindow()
        {
            InitializeComponent();

            RefreshConfigStatusLog();

            trayIcon = new Forms.NotifyIcon
            {
                Text = "VAICOM Light",
                Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetExecutingAssembly().Location) ?? SystemIcons.Application,
                Visible = true,
                ContextMenuStrip = BuildTrayMenu(),
            };

            trayIcon.DoubleClick += (s, e) => RestoreFromTray();
            StateChanged += OnStateChanged;
            Closing += OnClosing;

            statusTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1),
            };
            statusTimer.Tick += (s, e) => RefreshStatus();
            statusTimer.Start();

            bridgeInitialized = OpenKneeboardBridge.IsHostRunning;
            LoadVkbSettingsToUi();
            LoadLuaTargetSettingsToUi();
            RefreshStatus();
            SetActionStatus(bridgeInitialized
                ? "Host is running. If the VAICOM tab is missing in OpenKneeboard, restart OpenKneeboard once to rescan plugins."
                : "Ready. Click Start Host to initialize OpenKneeboard bridge.");
        }

        private Forms.ContextMenuStrip BuildTrayMenu()
        {
            var menu = new Forms.ContextMenuStrip();

            var openItem = new Forms.ToolStripMenuItem("Open VAICOM Light");
            openItem.Click += (s, e) => RestoreFromTray();
            menu.Items.Add(openItem);

            var exitItem = new Forms.ToolStripMenuItem("Exit");
            exitItem.Click += (s, e) =>
            {
                exitRequested = true;
                Close();
            };
            menu.Items.Add(exitItem);

            return menu;
        }

        private void OnStateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                Hide();
            }
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            exitRequested = true;
            statusTimer.Stop();
            trayIcon.Visible = false;
            trayIcon.Dispose();
        }

        private void RestoreFromTray()
        {
            Show();
            WindowState = WindowState.Normal;
            Activate();
        }

        private void RefreshStatus()
        {
            HostStatusText.Text = OpenKneeboardBridge.IsHostRunning ? "Running" : "Stopped";
            ConnectionStatusText.Text = OpenKneeboardBridge.HasActiveConnection ? "Active" : "Idle";
            bridgeInitialized = bridgeInitialized || OpenKneeboardBridge.IsHostRunning;
            EnsureDcsUpdateRequest();
        }

        private void SetActionStatus(string message)
        {
            ActionStatusText.Text = string.IsNullOrWhiteSpace(message) ? "Ready." : message;
        }

        private void EnsureDcsUpdateRequest(bool force = false)
        {
            try
            {
                if (!OpenKneeboardBridge.IsHostRunning)
                {
                    return;
                }

                DateTime nowUtc = DateTime.UtcNow;
                if (!force && (nowUtc - lastDcsUpdateRequestUtc).TotalSeconds < 5)
                {
                    return;
                }

                DcsClient.SendUpdateRequest();
                lastDcsUpdateRequestUtc = nowUtc;
            }
            catch
            {
            }
        }

        private void LoadVkbSettingsToUi()
        {
            if (State.activeconfig == null)
            {
                return;
            }

            OpenKneeboardOutCheckBox.IsChecked = State.activeconfig.OpenKneeboard_Out;
            BroadcastPortTextBox.Text = State.activeconfig.OpenKneeboard_Out_Port.ToString(CultureInfo.InvariantCulture);
            AllowLanAccessCheckBox.IsChecked = State.activeconfig.OpenKneeboard_Out_AllowLan;
            AutoBrowseCheckBox.IsChecked = State.activeconfig.OpenKneeboard_AutoBrowse;
            EnableEfbTabCheckBox.IsChecked = State.activeconfig.OpenKneeboard_EfbEnabled;
        }

        private static void PersistConfig()
        {
            try
            {
                Settings.ConfigFile.WriteConfigToFile(true);
            }
            catch
            {
            }
        }

        private void LoadLuaTargetSettingsToUi()
        {
            App.LoadLuaTargetSettings(out bool installStable, out bool installCustom, out string customPath);
            InstallStableCheckBox.IsChecked = installStable;
            InstallCustomCheckBox.IsChecked = installCustom;
            CustomDcsPathTextBox.Text = customPath ?? "";
        }

        private void SaveLuaTargetSettingsFromUi()
        {
            bool installStable = InstallStableCheckBox.IsChecked == true;
            bool installCustom = InstallCustomCheckBox.IsChecked == true;
            string customPath = (CustomDcsPathTextBox.Text ?? "").Trim();
            App.SaveLuaTargetSettings(installStable, installCustom, customPath);
        }

        private string GetBroadcastEndpointSummary()
        {
            if (State.activeconfig == null)
            {
                return "127.0.0.1:7779";
            }

            int port = State.activeconfig.OpenKneeboard_Out_Port > 0 ? State.activeconfig.OpenKneeboard_Out_Port : 7779;
            if (!State.activeconfig.OpenKneeboard_Out_AllowLan)
            {
                return "127.0.0.1:" + port.ToString(CultureInfo.InvariantCulture);
            }

            string ip = "0.0.0.0";
            try
            {
                foreach (IPAddress address in Dns.GetHostAddresses(Dns.GetHostName()))
                {
                    if (address.AddressFamily != AddressFamily.InterNetwork || IPAddress.IsLoopback(address))
                    {
                        continue;
                    }

                    ip = address.ToString();
                    break;
                }
            }
            catch
            {
            }

            return ip + ":" + port.ToString(CultureInfo.InvariantCulture);
        }

        private void StartHost_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (State.activeconfig != null)
                {
                    State.activeconfig.OpenKneeboard_Out = true;
                }

                OpenKneeboardOutCheckBox.IsChecked = true;
                ApplyEnabledSetting(true);

                RefreshStatus();
                SetActionStatus("Host start requested. If OpenKneeboard is already open and tab is missing, restart OpenKneeboard to rescan plugins.");
            }
            catch (Exception ex)
            {
                SetActionStatus("Host start failed: " + ex.Message);
            }
        }

        private void StopHost_Click(object sender, RoutedEventArgs e)
        {
            if (State.activeconfig != null)
            {
                State.activeconfig.OpenKneeboard_Out = false;
            }

            OpenKneeboardOutCheckBox.IsChecked = false;
            ApplyEnabledSetting(false);
            RefreshStatus();
            SetActionStatus("Host stop requested.");
        }

        private void RestartHost_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (bridgeInitialized)
                {
                    OpenKneeboardBridge.Shutdown();
                }

                OpenKneeboardBridge.Initialize();
                bridgeInitialized = true;
                RefreshStatus();
                SetActionStatus("Host restarted.");
            }
            catch (Exception ex)
            {
                SetActionStatus("Host restart failed: " + ex.Message);
            }
        }

        private void ApplyEnabledSetting(bool enabled)
        {
            if (State.activeconfig != null)
            {
                State.activeconfig.OpenKneeboard_Out = enabled;
            }

            if (enabled)
            {
                if (!bridgeInitialized)
                {
                    OpenKneeboardBridge.Initialize();
                    bridgeInitialized = true;
                }
                else
                {
                    OpenKneeboardBridge.SetEnabled(true);
                }
            }
            else
            {
                if (!bridgeInitialized)
                {
                    bridgeInitialized = OpenKneeboardBridge.IsHostRunning;
                }

                if (bridgeInitialized)
                {
                    OpenKneeboardBridge.SetEnabled(false);
                }
            }

            PersistConfig();
        }

        private void OpenKneeboardOutCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                ApplyEnabledSetting(true);
                EnsureDcsUpdateRequest(true);
                RefreshStatus();
                SetActionStatus("VKB Out enabled. Broadcasting on " + GetBroadcastEndpointSummary() + ".");
            }
            catch (Exception ex)
            {
                SetActionStatus("Enable failed: " + ex.Message);
            }
        }

        private void OpenKneeboardOutCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                ApplyEnabledSetting(false);
                RefreshStatus();
                SetActionStatus("VKB Out disabled.");
            }
            catch (Exception ex)
            {
                SetActionStatus("Disable failed: " + ex.Message);
            }
        }

        private void BroadcastPortTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (State.activeconfig == null)
            {
                return;
            }

            if (!int.TryParse(BroadcastPortTextBox.Text, NumberStyles.None, CultureInfo.InvariantCulture, out int port) || port < 1 || port > 65535)
            {
                BroadcastPortTextBox.Text = State.activeconfig.OpenKneeboard_Out_Port.ToString(CultureInfo.InvariantCulture);
                SetActionStatus("Invalid VKB Broadcast Port. Enter a value from 1 to 65535.");
                return;
            }

            if (State.activeconfig.OpenKneeboard_Out_Port == port)
            {
                return;
            }

            State.activeconfig.OpenKneeboard_Out_Port = port;
            PersistConfig();

            if (State.activeconfig.OpenKneeboard_Out)
            {
                OpenKneeboardBridge.SetEnabled(true);
            }

            RefreshStatus();
            SetActionStatus("VKB Broadcast Port set to " + port.ToString(CultureInfo.InvariantCulture) + ".");
        }

        private void AllowLanAccessCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (State.activeconfig == null)
            {
                return;
            }

            State.activeconfig.OpenKneeboard_Out_AllowLan = true;
            PersistConfig();
            if (State.activeconfig.OpenKneeboard_Out)
            {
                OpenKneeboardBridge.SetEnabled(true);
            }

            RefreshStatus();
            SetActionStatus("Allow LAN Access enabled. Broadcasting on " + GetBroadcastEndpointSummary() + ".");
        }

        private void AllowLanAccessCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (State.activeconfig == null)
            {
                return;
            }

            State.activeconfig.OpenKneeboard_Out_AllowLan = false;
            PersistConfig();
            if (State.activeconfig.OpenKneeboard_Out)
            {
                OpenKneeboardBridge.SetEnabled(true);
            }

            RefreshStatus();
            SetActionStatus("Allow LAN Access disabled.");
        }

        private void AutoBrowseCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (State.activeconfig == null)
            {
                return;
            }

            State.activeconfig.OpenKneeboard_AutoBrowse = true;
            PersistConfig();
            SetActionStatus("Auto Browse enabled.");
        }

        private void AutoBrowseCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (State.activeconfig == null)
            {
                return;
            }

            State.activeconfig.OpenKneeboard_AutoBrowse = false;
            PersistConfig();
            SetActionStatus("Auto Browse disabled.");
        }

        private void EnableEfbTabCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (State.activeconfig == null)
            {
                return;
            }

            State.activeconfig.OpenKneeboard_EfbEnabled = true;
            PersistConfig();
            OpenKneeboardBridge.UpdateServerData();
            OpenKneeboardBridge.UpdateStatus("Navigraph EFB tab enabled.", "sent");
            SetActionStatus("Enable EFB Tab enabled.");
        }

        private void EnableEfbTabCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (State.activeconfig == null)
            {
                return;
            }

            State.activeconfig.OpenKneeboard_EfbEnabled = false;
            PersistConfig();
            OpenKneeboardBridge.UpdateServerData();
            OpenKneeboardBridge.UpdateStatus("Navigraph EFB tab disabled.", "warning");
            SetActionStatus("Enable EFB Tab disabled.");
        }

        private async void NavigraphConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SetActionStatus("Starting Navigraph authentication...");
                var result = await OpenKneeboardNavigraphOAuthService.ConnectWithDeviceFlowAsync(CancellationToken.None).ConfigureAwait(true);
                PersistConfig();
                OpenKneeboardBridge.UpdateServerData();

                if (result.Success)
                {
                    OpenKneeboardBridge.UpdateStatus("Navigraph EFB connected.", "sent");
                    SetActionStatus("Navigraph connected.");
                }
                else
                {
                    string message = string.IsNullOrWhiteSpace(result.Message)
                        ? "Navigraph authentication did not complete."
                        : result.Message;
                    OpenKneeboardBridge.UpdateStatus(message, "warning");
                    SetActionStatus(message);
                }
            }
            catch (Exception ex)
            {
                OpenKneeboardBridge.UpdateStatus("Navigraph authentication failed.", "error");
                SetActionStatus("Navigraph connect failed: " + ex.Message);
            }
        }

        private void NavigraphLogout_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenKneeboardNavigraphEfbState.ClearEncryptedAuthBlob();
                PersistConfig();
                OpenKneeboardBridge.UpdateServerData();
                OpenKneeboardBridge.UpdateStatus("Navigraph EFB auth cleared.", "warning");
                SetActionStatus("Navigraph logged out.");
            }
            catch (Exception ex)
            {
                SetActionStatus("Navigraph logout failed: " + ex.Message);
            }
        }

        private void LuaTargetSettings_Changed(object sender, RoutedEventArgs e)
        {
            SaveLuaTargetSettingsFromUi();
        }

        private void CustomDcsPathTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            SaveLuaTargetSettingsFromUi();
        }

        private void BrowseCustomDcsPath_Click(object sender, RoutedEventArgs e)
        {
            using (var pathDialog = new Forms.FolderBrowserDialog())
            {
                pathDialog.Description = "Select DCS World program folder";
                pathDialog.ShowNewFolderButton = false;
                if (pathDialog.ShowDialog() == Forms.DialogResult.OK)
                {
                    CustomDcsPathTextBox.Text = pathDialog.SelectedPath;
                    SaveLuaTargetSettingsFromUi();
                }
            }
        }

        private void RepairLuaHooks_Click(object sender, RoutedEventArgs e)
        {
            SaveLuaTargetSettingsFromUi();

            bool installStable = InstallStableCheckBox.IsChecked == true;
            bool installCustom = InstallCustomCheckBox.IsChecked == true;
            string customPath = (CustomDcsPathTextBox.Text ?? "").Trim();

            bool ok = App.ExecuteLuaInstallTargets(false, installStable, installCustom, customPath, out string status);
            RefreshConfigStatusLog();
            SetActionStatus(ok ? status : "Lua repair failed: " + status);
        }

        private void RemoveLuaHooks_Click(object sender, RoutedEventArgs e)
        {
            SaveLuaTargetSettingsFromUi();

            bool installStable = InstallStableCheckBox.IsChecked == true;
            bool installCustom = InstallCustomCheckBox.IsChecked == true;
            string customPath = (CustomDcsPathTextBox.Text ?? "").Trim();

            bool ok = App.ExecuteLuaInstallTargets(true, installStable, installCustom, customPath, out string status);
            RefreshConfigStatusLog();
            SetActionStatus(ok ? status : "Lua removal failed: " + status);
        }

        private void RefreshConfigStatusLog()
        {
            UpdateFeedText.Text = App.ReadLuaActionLog();
        }

        private void OpenHelpDoc_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string helpPath = OpenKneeboardBridge.GetOfflineHelpDocBrowserPath();
                if (string.IsNullOrWhiteSpace(helpPath))
                {
                    SetActionStatus("Help document path is unavailable.");
                    return;
                }

                Process.Start(helpPath);
                SetActionStatus("Opened OKB help document.");
            }
            catch (Exception ex)
            {
                SetActionStatus("Failed to open help document: " + ex.Message);
            }
        }

        private void RefreshStatus_Click(object sender, RoutedEventArgs e)
        {
            RefreshStatus();
            SetActionStatus("Status refreshed.");
        }
    }
}
