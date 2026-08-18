using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VAICOM.FileManager;
using VAICOM.Static;

namespace VAICOM
{
    namespace UI
    {

        public partial class ConfigWindow : Window
        {
            // ------------  EXTENSIONS PAGE -----------------------------

            private static readonly Regex OpenKneeboardOutPortDigitsOnlyRegex = new Regex("^[0-9]+$");

            // Pene edits Kneeboard Activate/deactivate Box

            public void KNEE_restart_popup()
            {
                KNEE_Enable_Box.IsEnabled = false;
                string caption = "KNEEBOARD setting changed";
                string message = "KNEEBOARD setting changed:\nRestart VoiceAttack and DCS.";
                MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            private void SetConfigEnableKNEEOn(object sender, RoutedEventArgs e) { State.activeconfig.Kneeboard_Enabled = true; }
            private void SetConfigDisableKNEEOff(object sender, RoutedEventArgs e) { State.activeconfig.Kneeboard_Enabled = false; }
            private void SetCurrentValueKNEE(object sender, EventArgs e)
            {
                KNEE_Enable_Box.IsEnabled = true;
                KNEE_Enable_Box.IsChecked = State.activeconfig.Kneeboard_Enabled;
            }
            public void UpdateKneeboard()
            {
                Extensions.Kneeboard.KneeboardUpdater.SendHeartBeatCycle();
            }

            private bool kneeboard_init = false;
            private bool okbEfbRefreshLoopEnabled = false;

            private void SetCurrentValueKneeboardOpacity(object sender, EventArgs e)
            {
                KneeboardOpacity.IsEnabled = true;
                KneeboardOpacity.Value = State.activeconfig.KneeboardOpacity;
                kneeboard_init = true;
            }
            private void KneeboardOpacity_Change(object sender, RoutedPropertyChangedEventArgs<double> e)
            {
                if (kneeboard_init && !State.activeconfig.KneeboardOpacity.Equals(e.NewValue))
                {
                    State.activeconfig.KneeboardOpacity = e.NewValue;
                    Settings.ConfigFile.WriteConfigToFile(true);
                    UpdateKneeboard();
                }
            }

            public void AIRIO_restart_popup()
            {
                RIO_Enable_Box.IsEnabled = false;
                string caption = "Jester Mini Wheel setting changed";
                string message = "Jester Mini Wheel setting changed:\nRestart VoiceAttack and DCS.";
                MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Information);
            }

            // Left column:        

            private void SetConfigEnableRIO_Enable(object sender, RoutedEventArgs e) { if (State.allowairioswitching) { State.activeconfig.RIO_Enabled = true; State.activeconfig.RIO_MiniWheel_Enabled = true; FileHandler.Lua.LuaFiles_Install(false, true); } else { RIO_Enable_Box.IsEnabled = false; } }
            private void SetConfigDisableRIO_Enable(object sender, RoutedEventArgs e) { if (State.allowairioswitching) { State.activeconfig.RIO_Enabled = true; State.activeconfig.RIO_MiniWheel_Enabled = false; FileHandler.Lua.LuaFiles_Install(false, true); } else { RIO_Enable_Box.IsEnabled = false; } }

            private void SetCurrentValueRIO_Enable(object sender, EventArgs e)
            {
                if (State.dll_installed_rio)
                {
                    RIO_Enable_Box.Visibility = Visibility.Visible;
                }
                else
                {
                    RIO_Enable_Box.Visibility = Visibility.Hidden;
                }
                RIO_Enable_Box.IsEnabled = true;
                State.activeconfig.RIO_Enabled = true;
                RIO_Enable_Box.IsChecked = State.activeconfig.RIO_MiniWheel_Enabled;
            }

            private void SetConfigEnableRIO_Disable_Rose(object sender, RoutedEventArgs e) { State.activeconfig.RIO_Messages = true; }
            private void SetConfigDisableRIO_Disable_Rose(object sender, RoutedEventArgs e) { State.activeconfig.RIO_Messages = false; }
            private void SetCurrentValueRIO_Disable_Rose(object sender, EventArgs e)
            {
                if (State.dll_installed_rio)
                {
                    RIO_Hints.Visibility = Visibility.Visible;
                }
                else
                {
                    RIO_Hints.Visibility = Visibility.Hidden;
                }
                RIO_Hints.IsEnabled = true;
                RIO_Hints.IsChecked = State.activeconfig.RIO_Messages;
            }

            private void SetConfigEnableRIO_Hints_Only(object sender, RoutedEventArgs e) { State.activeconfig.RIO_Hints_Only = true; }
            private void SetConfigDisableRIO_Hints_Only(object sender, RoutedEventArgs e) { State.activeconfig.RIO_Hints_Only = false; }
            private void SetCurrentValueRIO_Hints_Only(object sender, EventArgs e)
            {
                if (State.dll_installed_rio)
                {
                    RIO_Hints_Only.Visibility = Visibility.Visible;
                }
                else
                {
                    RIO_Hints_Only.Visibility = Visibility.Hidden;
                }
                RIO_Hints_Only.IsEnabled = true;
                RIO_Hints_Only.IsChecked = State.activeconfig.RIO_Hints_Only;
            }

            private void SetConfigEnableRIO_ICShotmic(object sender, RoutedEventArgs e)
            {
                State.activeconfig.ICShotmic = true;
                if (State.IsCrewHotMicModuleActive())
                {
                    State.Proxy.Command.SetSessionEnabledByCategory("Keyword Collections", true);
                    State.Proxy.Command.SetSessionEnabledByCategory("Extension packs", true);
                    State.Proxy.State.SetListeningEnabled(true);
                }
            }
            private void SetConfigDisableRIO_ICShotmic(object sender, RoutedEventArgs e)
            {
                State.activeconfig.ICShotmic = false;
                if (State.IsCrewHotMicModuleActive())
                {
                    if (State.activeconfig.ReleaseHot)
                    {
                        State.Proxy.Command.SetSessionEnabledByCategory("Keyword Collections", false);
                        State.Proxy.Command.SetSessionEnabledByCategory("Extension packs", false);
                        if (State.Proxy.GetProfileName().ToLower().Contains(State.defProfileName.ToLower()))
                        {
                            State.Proxy.Command.SetSessionEnabled("Chatter", true);
                        }
                        State.Proxy.State.SetListeningEnabled(true);
                    }
                    else
                    {
                        State.Proxy.Command.SetSessionEnabledByCategory("Keyword Collections", true);
                        State.Proxy.Command.SetSessionEnabledByCategory("Extension packs", true);
                        State.Proxy.State.SetListeningEnabled(false);
                    }
                }
            }
            private void SetCurrentValueRIO_ICShotmic(object sender, EventArgs e)
            {
                RIO_ICShotmic.IsChecked = State.activeconfig.ICShotmic;
            }

            public void CheckBoxHotMic()
            {
                if (RIO_ICShotmic != null)
                {
                    RIO_ICShotmic.IsChecked = State.activeconfig.ICShotmic;
                }
            }

            private void SetConfigEnableRIO_ICShotmic_useswitch(object sender, RoutedEventArgs e)
            {
                State.activeconfig.ICShotmic_useswitch = true;

                if (State.currentstate != null && State.IsF4EIntercomSelected())
                {
                    State.activeconfig.ICShotmic = true;
                    CheckBoxHotMic();
                }
            }

            private void SetConfigDisableRIO_ICShotmic_useswitch(object sender, RoutedEventArgs e)
            {
                State.activeconfig.ICShotmic_useswitch = false;
                State.activeconfig.ICShotmic = false;
                CheckBoxHotMic();
            }

            private void SetCurrentValueRIO_ICShotmic_useswitch(object sender, EventArgs e)
            {
                if (State.dll_installed_rio)
                {
                    RIO_ICShotmic_useswitch.Visibility = Visibility.Visible;
                }
                else
                {
                    RIO_ICShotmic_useswitch.Visibility = Visibility.Hidden;
                }
                RIO_ICShotmic_useswitch.IsEnabled = true;
                RIO_ICShotmic_useswitch.IsChecked = State.activeconfig.ICShotmic_useswitch;
            }

            private void SetConfigEnableHideF4EDialog(object sender, RoutedEventArgs e)
            {
                State.activeconfig.HideF4EDialog = true;
                FileHandler.Lua.LuaFiles["2.9 WSO Renderer.js"].source = Properties.Resources.Append_F4E_Jester_Renderer;
                FileHandler.Lua.LuaFiles["2.9 WSO Renderer.js"].source_legacy = Properties.Resources.Append_F4E_Jester_Renderer;
                FileHandler.Lua.LuaFiles["2.9 WSO Renderer.js"].stringsource = Properties.Resources.Append_F4E_Jester_Renderer;
                FileHandler.Lua.LuaFiles_Install(false, true);
            }

            private void SetConfigDisableHideF4EDialog(object sender, RoutedEventArgs e)
            {
                State.activeconfig.HideF4EDialog = false;
                FileHandler.Lua.LuaFiles["2.9 WSO Renderer.js"].source = "";
                FileHandler.Lua.LuaFiles["2.9 WSO Renderer.js"].source_legacy = "";
                FileHandler.Lua.LuaFiles["2.9 WSO Renderer.js"].stringsource = "";
                FileHandler.Lua.LuaFiles_Install(false, true);
            }

            private void SetCurrentValueHideF4EDialog(object sender, EventArgs e)
            {
                HideF4EDialog.IsEnabled = true;
                HideF4EDialog.IsChecked = State.activeconfig.HideF4EDialog;
            }

            private void InitRIODllWarning(object sender, EventArgs e)
            {
                if (!State.dll_installed_rio)
                {
                    RIO_Dll_missing.Visibility = Visibility.Visible;
                }
                else
                {
                    RIO_Dll_missing.Visibility = Visibility.Hidden;
                }
            }
            private void Init_RIO_Dll_missing_Pic(object sender, EventArgs e)
            {
                if (!State.dll_installed_rio)
                {
                    RIO_Dll_missing_Pic.Visibility = Visibility.Visible;
                }
                else
                {
                    RIO_Dll_missing_Pic.Visibility = Visibility.Hidden;
                }

            }

            private void CarrierCommsOn(object sender, RoutedEventArgs e) { CarrierComms.IsChecked = true; }
            private void CarrierCommsOff(object sender, RoutedEventArgs e) { CarrierComms.IsChecked = true; }
            private void SetCurrentValueCarrierComms(object sender, EventArgs e)
            {
                CarrierComms.IsEnabled = true;
                CarrierComms.IsChecked = true;
            }

            private void CarrierSuppressAutoOn(object sender, RoutedEventArgs e) { State.activeconfig.CarrierSuppressAuto = true; }
            private void CarrierSuppressAutoOff(object sender, RoutedEventArgs e) { State.activeconfig.CarrierSuppressAuto = false; }
            private void SetCurrentValueCarrierSuppressAuto(object sender, EventArgs e)
            {
                CarrierSuppressAuto.IsEnabled = true;
                CarrierSuppressAuto.IsChecked = State.activeconfig.CarrierSuppressAuto;
            }

            // Right column:

            // auto brief
            private void AutoBriefOn(object sender, RoutedEventArgs e) { State.activeconfig.AutoBrief = true; }
            private void AutoBriefOff(object sender, RoutedEventArgs e) { State.activeconfig.AutoBrief = false; }
            private void SetCurrentValueAutoBrief(object sender, EventArgs e) { autobrief.IsEnabled = true; autobrief.IsChecked = State.activeconfig.AutoBrief; }

            // concise
            private void BriefConciseOn(object sender, RoutedEventArgs e) { State.activeconfig.BriefConcise = true; }
            private void BriefConciseOff(object sender, RoutedEventArgs e) { State.activeconfig.BriefConcise = false; }
            private void SetCurrentValueBriefConcise(object sender, EventArgs e) { briefconcise.IsEnabled = true; briefconcise.IsChecked = State.activeconfig.BriefConcise; }

            // deepinterrogate
            private void DeepInterrogateOn(object sender, RoutedEventArgs e) { State.activeconfig.DeepInterrogate = true; }
            private void DeepInterrogateOff(object sender, RoutedEventArgs e) { State.activeconfig.DeepInterrogate = false; }
            private void SetCurrentValueDeepInterrogate(object sender, EventArgs e) { deepinterrogate.IsEnabled = true; deepinterrogate.IsChecked = State.activeconfig.DeepInterrogate; }

            // chatter

            private void ChatterSilentOfflineOn(object sender, RoutedEventArgs e)
            {
                State.activeconfig.ChatterSilentOffline = true;
                State.activeconfig.RequireFrequency281000 = true; // Enable frequency requirement
                Log.Write("Radio Power and frequency 281.000 requirement enabled for Chatter.", Colors.Text);
            }

            private void ChatterSilentOfflineOff(object sender, RoutedEventArgs e)
            {
                State.activeconfig.ChatterSilentOffline = false;
                State.activeconfig.RequireFrequency281000 = false; // Disable frequency requirement
                Log.Write("Radio Power and frequency 281.000 requirement disabled for Chatter.", Colors.Text);
            }

            private void SetCurrentValueChatterSilentOffline(object sender, EventArgs e)
            {
                ChatterSilentOffline.IsEnabled = State.dll_installed_chatter;
                ChatterSilentOffline.IsChecked = State.activeconfig.ChatterSilentOffline;
            }

            private void KneeboardlinkPTTOn(object sender, RoutedEventArgs e) { State.activeconfig.KneeboardlinkPTT = true; }
            private void KneeboardlinkPTTOff(object sender, RoutedEventArgs e) { State.activeconfig.KneeboardlinkPTT = false; }
            private void SetCurrentValueKneeboardlinkPTT(object sender, EventArgs e)
            {
                KneeboardlinkPTT.IsEnabled = true;
                KneeboardlinkPTT.IsChecked = State.activeconfig.KneeboardlinkPTT;
            }

            private void OpenKneeboardOutOn(object sender, RoutedEventArgs e)
            {
                State.activeconfig.OpenKneeboard_Out = true;
                Extensions.Kneeboard.OpenKneeboardBridge.SetEnabled(true);
                ChangeOKHostbug();
            }

            private void OpenKneeboardOutOff(object sender, RoutedEventArgs e)
            {
                State.activeconfig.OpenKneeboard_Out = false;
                Extensions.Kneeboard.OpenKneeboardBridge.SetEnabled(false);
                ChangeOKHostbug();
            }

            private void SetCurrentValueOpenKneeboardOut(object sender, EventArgs e)
            {
                CheckBox checkbox = sender as CheckBox;
                if (checkbox != null)
                {
                    checkbox.IsEnabled = true;
                    checkbox.IsChecked = State.activeconfig.OpenKneeboard_Out;
                }
            }

            private void OpenKneeboardAutoBrowseOn(object sender, RoutedEventArgs e) { State.activeconfig.OpenKneeboard_AutoBrowse = true; }
            private void OpenKneeboardAutoBrowseOff(object sender, RoutedEventArgs e) { State.activeconfig.OpenKneeboard_AutoBrowse = false; }
            private void SetCurrentValueOpenKneeboardAutoBrowse(object sender, EventArgs e)
            {
                CheckBox checkbox = sender as CheckBox;
                if (checkbox != null)
                {
                    checkbox.IsEnabled = true;
                    checkbox.IsChecked = State.activeconfig.OpenKneeboard_AutoBrowse;
                }
            }

            private void OpenKneeboardEfbEnableOn(object sender, RoutedEventArgs e)
            {
                State.activeconfig.OpenKneeboard_EfbEnabled = true;
                Extensions.Kneeboard.OpenKneeboardBridge.UpdateServerData();
                Extensions.Kneeboard.OpenKneeboardBridge.UpdateStatus("Navigraph EFB tab enabled.", "sent");
            }

            private void OpenKneeboardEfbEnableOff(object sender, RoutedEventArgs e)
            {
                State.activeconfig.OpenKneeboard_EfbEnabled = false;
                Extensions.Kneeboard.OpenKneeboardBridge.UpdateServerData();
                Extensions.Kneeboard.OpenKneeboardBridge.UpdateStatus("Navigraph EFB tab disabled.", "warning");
            }
            private void SetCurrentValueOpenKneeboardEfbEnable(object sender, EventArgs e)
            {
                CheckBox checkbox = sender as CheckBox;
                if (checkbox != null)
                {
                    checkbox.IsEnabled = true;
                    checkbox.IsChecked = State.activeconfig.OpenKneeboard_EfbEnabled;
                }
            }

            private void OpenKneeboardEfbDevBypassOn(object sender, RoutedEventArgs e) { State.activeconfig.OpenKneeboard_EfbDevBypass = true; }
            private void OpenKneeboardEfbDevBypassOff(object sender, RoutedEventArgs e) { State.activeconfig.OpenKneeboard_EfbDevBypass = false; }
            private void SetCurrentValueOpenKneeboardEfbDevBypass(object sender, EventArgs e)
            {
                CheckBox checkbox = sender as CheckBox;
                if (checkbox != null)
                {
                    checkbox.IsEnabled = true;
                    checkbox.IsChecked = State.activeconfig.OpenKneeboard_EfbDevBypass;
                }
            }

            private async void OpenKneeboardEfbSeedAuthClick(object sender, RoutedEventArgs e)
            {
                if (OpenKneeboardEfbSeedAuth != null)
                {
                    OpenKneeboardEfbSeedAuth.IsEnabled = false;
                }

                try
                {
                    if (Extensions.Kneeboard.OpenKneeboardNavigraphEfbState.HasStoredAuth())
                    {
                        Log.Write("Navigraph auth already present. Connect skipped.", Colors.Text);
                        Extensions.Kneeboard.OpenKneeboardBridge.UpdateStatus("Navigraph EFB auth already present.", "sent");
                        Extensions.Kneeboard.OpenKneeboardBridge.UpdateServerData();
                        return;
                    }

                    Log.Write("Starting Navigraph device authentication...", Colors.Text);
                    Extensions.Kneeboard.OpenKneeboardBridge.UpdateStatus("Starting Navigraph authentication...", "warning");

                    var result = await Extensions.Kneeboard.OpenKneeboardNavigraphOAuthService
                        .ConnectWithDeviceFlowAsync(CancellationToken.None)
                        .ConfigureAwait(true);

                    Settings.ConfigFile.WriteConfigToFile(true);
                    Extensions.Kneeboard.OpenKneeboardBridge.UpdateServerData();

                    if (result.Success)
                    {
                        Log.Write("Navigraph authentication completed.", Colors.Text);
                        Extensions.Kneeboard.OpenKneeboardBridge.UpdateStatus("Navigraph EFB connected.", "sent");
                    }
                    else
                    {
                        string message = string.IsNullOrWhiteSpace(result.Message)
                            ? "Navigraph authentication did not complete."
                            : result.Message;
                        if (message.IndexOf("client id", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            message += " Set OpenKneeboard_EfbOAuthClientId in VAICOM config JSON.";
                        }
                        Log.Write(message, Colors.Warning);
                        Extensions.Kneeboard.OpenKneeboardBridge.UpdateStatus(message, "warning");
                    }
                }
                catch (Exception ex)
                {
                    Log.Write("Navigraph authentication failed: " + ex.Message, Colors.Warning);
                    Extensions.Kneeboard.OpenKneeboardBridge.UpdateStatus("Navigraph authentication failed.", "error");
                }
                finally
                {
                    if (OpenKneeboardEfbSeedAuth != null)
                    {
                        OpenKneeboardEfbSeedAuth.IsEnabled = true;
                    }
                }
            }

            private void OpenKneeboardEfbClearAuthClick(object sender, RoutedEventArgs e)
            {
                Extensions.Kneeboard.OpenKneeboardNavigraphEfbState.ClearEncryptedAuthBlob();
                Settings.ConfigFile.WriteConfigToFile(true);
                Extensions.Kneeboard.OpenKneeboardBridge.UpdateServerData();
                Log.Write("OKB EFB auth blob cleared.", Colors.Text);
            }

            private void OpenKneeboardEfbLogAuthClick(object sender, RoutedEventArgs e)
            {
                Extensions.Kneeboard.OpenKneeboardNavigraphEfbState.LogDecryptedAuthForDiagnostics();
            }

            private void OpenKneeboardEfbMockTokenClick(object sender, RoutedEventArgs e)
            {
                State.activeconfig.OpenKneeboard_EfbEnabled = true;
                State.activeconfig.OpenKneeboard_EfbDevBypass = true;
                Extensions.Kneeboard.OpenKneeboardNavigraphEfbState.SetMockTokenForTesting(false);
                Settings.ConfigFile.WriteConfigToFile(true);
                Extensions.Kneeboard.OpenKneeboardBridge.UpdateServerData();
                bool hasStoredAuth = Extensions.Kneeboard.OpenKneeboardNavigraphEfbState.HasStoredAuth();
                Log.Write("OKB EFB mock token generated (valid). AuthReadable=" + hasStoredAuth, hasStoredAuth ? Colors.Text : Colors.Warning);
            }

            private void OpenKneeboardEfbMockExpiredClick(object sender, RoutedEventArgs e)
            {
                State.activeconfig.OpenKneeboard_EfbEnabled = true;
                State.activeconfig.OpenKneeboard_EfbDevBypass = true;
                Extensions.Kneeboard.OpenKneeboardNavigraphEfbState.SetMockTokenForTesting(true);
                Settings.ConfigFile.WriteConfigToFile(true);
                Extensions.Kneeboard.OpenKneeboardBridge.UpdateServerData();
                bool hasStoredAuth = Extensions.Kneeboard.OpenKneeboardNavigraphEfbState.HasStoredAuth();
                Log.Write("OKB EFB mock token generated (expired). AuthReadable=" + hasStoredAuth, Colors.Warning);
            }

            private void OpenKneeboardEfbExpireTokenClick(object sender, RoutedEventArgs e)
            {
                bool success = Extensions.Kneeboard.OpenKneeboardNavigraphEfbState.ExpireCurrentTokenForTesting();
                Settings.ConfigFile.WriteConfigToFile(true);
                Extensions.Kneeboard.OpenKneeboardBridge.UpdateServerData();
                if (success)
                {
                    Log.Write("OKB EFB token force-expired for testing.", Colors.Warning);
                }
                else
                {
                    Log.Write("OKB EFB token expire test failed: no parseable token.", Colors.Warning);
                }
            }

            private void OpenKneeboardEfbRefreshOkClick(object sender, RoutedEventArgs e)
            {
                Extensions.Kneeboard.OpenKneeboardNavigraphOAuthService.SetMockRefreshMode("success");
                var result = Extensions.Kneeboard.OpenKneeboardNavigraphOAuthService.RunMockRefreshOnce();
                HandleMockRefreshResult(result, "Refresh OK");
            }

            private void OpenKneeboardEfbRefreshFailClick(object sender, RoutedEventArgs e)
            {
                Extensions.Kneeboard.OpenKneeboardNavigraphOAuthService.SetMockRefreshMode("fail");
                var result = Extensions.Kneeboard.OpenKneeboardNavigraphOAuthService.RunMockRefreshOnce();
                HandleMockRefreshResult(result, "Refresh Fail");
            }

            private void OpenKneeboardEfbRefreshExpiredClick(object sender, RoutedEventArgs e)
            {
                Extensions.Kneeboard.OpenKneeboardNavigraphOAuthService.SetMockRefreshMode("expired");
                var result = Extensions.Kneeboard.OpenKneeboardNavigraphOAuthService.RunMockRefreshOnce();
                HandleMockRefreshResult(result, "Refresh Expired");
            }

            private void OpenKneeboardEfbSchedulerToggleClick(object sender, RoutedEventArgs e)
            {
                okbEfbRefreshLoopEnabled = !okbEfbRefreshLoopEnabled;

                if (okbEfbRefreshLoopEnabled)
                {
                    Extensions.Kneeboard.OpenKneeboardNavigraphOAuthService.SetMockRefreshMode("success");
                    Extensions.Kneeboard.OpenKneeboardNavigraphOAuthService.StartMockRefreshScheduler(15);
                    if (OpenKneeboardEfbSchedulerToggle != null)
                    {
                        OpenKneeboardEfbSchedulerToggle.Content = "Refresh Loop OFF";
                    }
                    Log.Write("OKB EFB mock refresh loop started (15s).", Colors.Text);
                    return;
                }

                Extensions.Kneeboard.OpenKneeboardNavigraphOAuthService.StopMockRefreshScheduler();
                if (OpenKneeboardEfbSchedulerToggle != null)
                {
                    OpenKneeboardEfbSchedulerToggle.Content = "Refresh Loop ON";
                }
                Log.Write("OKB EFB mock refresh loop stopped.", Colors.Text);
            }

            private static void HandleMockRefreshResult(Extensions.Kneeboard.OpenKneeboardNavigraphOAuthService.ConnectResult result, string actionLabel)
            {
                Settings.ConfigFile.WriteConfigToFile(true);
                Extensions.Kneeboard.OpenKneeboardBridge.UpdateServerData();

                if (result != null && result.Success)
                {
                    Log.Write("OKB EFB " + actionLabel + ": " + result.Message, Colors.Text);
                    Extensions.Kneeboard.OpenKneeboardBridge.UpdateStatus("EFB " + actionLabel + " complete.", "sent");
                    return;
                }

                string message = result == null ? "Unknown mock refresh result." : result.Message;
                Log.Write("OKB EFB " + actionLabel + ": " + message, Colors.Warning);
                Extensions.Kneeboard.OpenKneeboardBridge.UpdateStatus("EFB " + actionLabel + " warning.", "warning");
            }

            private void OpenKneeboardEfbClientIdInit(object sender, EventArgs e) { SetTextBoxValue(sender, State.activeconfig.OpenKneeboard_EfbOAuthClientId); }
            private void OpenKneeboardEfbScopeInit(object sender, EventArgs e) { SetTextBoxValue(sender, State.activeconfig.OpenKneeboard_EfbOAuthScope); }
            private void OpenKneeboardEfbDeviceEndpointInit(object sender, EventArgs e) { SetTextBoxValue(sender, State.activeconfig.OpenKneeboard_EfbOAuthDeviceAuthEndpoint); }
            private void OpenKneeboardEfbTokenEndpointInit(object sender, EventArgs e) { SetTextBoxValue(sender, State.activeconfig.OpenKneeboard_EfbOAuthTokenEndpoint); }

            private void OpenKneeboardEfbClientIdLostFocus(object sender, RoutedEventArgs e)
            {
                string next = ReadTextBoxValue(sender);
                if (State.activeconfig.OpenKneeboard_EfbOAuthClientId == next) return;
                State.activeconfig.OpenKneeboard_EfbOAuthClientId = next;
                Settings.ConfigFile.WriteConfigToFile(true);
            }

            private void OpenKneeboardEfbScopeLostFocus(object sender, RoutedEventArgs e)
            {
                string next = ReadTextBoxValue(sender);
                if (State.activeconfig.OpenKneeboard_EfbOAuthScope == next) return;
                State.activeconfig.OpenKneeboard_EfbOAuthScope = next;
                Settings.ConfigFile.WriteConfigToFile(true);
            }

            private void OpenKneeboardEfbDeviceEndpointLostFocus(object sender, RoutedEventArgs e)
            {
                string next = ReadTextBoxValue(sender);
                if (State.activeconfig.OpenKneeboard_EfbOAuthDeviceAuthEndpoint == next) return;
                State.activeconfig.OpenKneeboard_EfbOAuthDeviceAuthEndpoint = next;
                Settings.ConfigFile.WriteConfigToFile(true);
            }

            private void OpenKneeboardEfbTokenEndpointLostFocus(object sender, RoutedEventArgs e)
            {
                string next = ReadTextBoxValue(sender);
                if (State.activeconfig.OpenKneeboard_EfbOAuthTokenEndpoint == next) return;
                State.activeconfig.OpenKneeboard_EfbOAuthTokenEndpoint = next;
                Settings.ConfigFile.WriteConfigToFile(true);
            }

            private static void SetTextBoxValue(object sender, string value)
            {
                TextBox textBox = sender as TextBox;
                if (textBox == null)
                {
                    return;
                }

                textBox.IsEnabled = true;
                textBox.Text = value ?? "";
            }

            private static string ReadTextBoxValue(object sender)
            {
                TextBox textBox = sender as TextBox;
                if (textBox == null)
                {
                    return "";
                }

                return (textBox.Text ?? "").Trim();
            }

            private void OpenKneeboardOutPortInit(object sender, EventArgs e)
            {
                TextBox textBox = sender as TextBox;
                if (textBox == null)
                {
                    return;
                }

                int configuredPort = State.activeconfig.OpenKneeboard_Out_Port;
                if (configuredPort <= 0 || configuredPort > 65535)
                {
                    configuredPort = 7779;
                    State.activeconfig.OpenKneeboard_Out_Port = configuredPort;
                }

                textBox.Text = configuredPort.ToString();
            }

            private void OpenKneeboardOutPortPreviewTextInput(object sender, TextCompositionEventArgs e)
            {
                e.Handled = !OpenKneeboardOutPortDigitsOnlyRegex.IsMatch(e.Text);
            }

            private void OpenKneeboardOutPortPasting(object sender, DataObjectPastingEventArgs e)
            {
                if (!e.DataObject.GetDataPresent(typeof(string)))
                {
                    e.CancelCommand();
                    return;
                }

                string pastedText = (string)e.DataObject.GetData(typeof(string));
                if (string.IsNullOrWhiteSpace(pastedText) || !OpenKneeboardOutPortDigitsOnlyRegex.IsMatch(pastedText))
                {
                    e.CancelCommand();
                }
            }

            private void OpenKneeboardOutPortLostFocus(object sender, RoutedEventArgs e)
            {
                TextBox textBox = sender as TextBox;
                if (textBox == null)
                {
                    return;
                }

                int parsedPort;
                if (!int.TryParse(textBox.Text, out parsedPort) || parsedPort <= 0 || parsedPort > 65535)
                {
                    textBox.Text = State.activeconfig.OpenKneeboard_Out_Port.ToString();
                    return;
                }

                if (State.activeconfig.OpenKneeboard_Out_Port != parsedPort)
                {
                    State.activeconfig.OpenKneeboard_Out_Port = parsedPort;
                    Settings.ConfigFile.WriteConfigToFile(true);

                    if (State.activeconfig.OpenKneeboard_Out)
                    {
                        Extensions.Kneeboard.OpenKneeboardBridge.SetEnabled(false);
                        Extensions.Kneeboard.OpenKneeboardBridge.SetEnabled(true);
                    }
                }

                textBox.Text = State.activeconfig.OpenKneeboard_Out_Port.ToString();
            }


            private void ChatterAutostartOn(object sender, RoutedEventArgs e) { State.activeconfig.ChatterAutostart = true; }
            private void ChatterAutostartOff(object sender, RoutedEventArgs e) { State.activeconfig.ChatterAutostart = false; }
            private void SetCurrentValueChatterAutostart(object sender, EventArgs e)
            {
                ChatterAutostart.IsEnabled = State.dll_installed_chatter;
                ChatterAutostart.IsChecked = State.activeconfig.ChatterAutostart;
            }

            private void ChatterFolderInitialValue(object sender, EventArgs e)
            {
                ChatterTheme.IsEnabled = State.dll_installed_chatter;
                string displaytext = "---";
                try
                {
                    displaytext = State.activeconfig.ChatterFolder;
                }
                catch
                {
                }
                ChatterTheme.Text = displaytext;
            }
            private void UpdateChatterFolder(object sender, SelectionChangedEventArgs e)
            {
                try
                {
                    bool chatterchanged;
                    string currenttheme = State.activeconfig.ChatterFolder;

                    State.activeconfig.ChatterFolder = ChatterTheme.SelectedValue.ToString();
                    Settings.ConfigFile.WriteConfigToFile(true);

                    chatterchanged = !(State.activeconfig.ChatterFolder.Equals(currenttheme));

                    if (chatterchanged)
                    {
                        Log.Write("Theme changed: restarting chatter.", Static.Colors.Text);
                        Extensions.Chatter.AudioTimer.Chatter_TimerPlayToggle();
                        Extensions.Chatter.AudioTimer.Chatter_Initialize();
                        Extensions.Chatter.AudioTimer.Chatter_TimerPlayToggle();
                    }
                }
                catch
                {
                }
            }
            // Pene edits Chatter Activate/deactivate box
            private void SetConfigEnableCHATOn(object sender, RoutedEventArgs e) { State.activeconfig.Chatter_Enabled = true; }
            private void SetConfigDisableCHATOff(object sender, RoutedEventArgs e) { State.activeconfig.Chatter_Enabled = false; }
            private void SetCurrentValueCHAT(object sender, EventArgs e)
            {
                CHAT_Enable_Box.IsEnabled = true;
                CHAT_Enable_Box.IsChecked = State.activeconfig.Chatter_Enabled;
            }

            // ------------------------------------------------------------
        }
    }
}
