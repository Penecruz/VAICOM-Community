using System;
using System.Windows;
using System.Windows.Controls;
using VAICOM.FileManager;
using VAICOM.Static;

namespace VAICOM
{
    namespace UI
    {

        public partial class ConfigWindow : Window
        {
            // ------------  EXTENSIONS PAGE -----------------------------

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
                string caption = "AIRIO setting changed";
                string message = "AIRIO setting changed:\nRestart VoiceAttack and DCS.";
                MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Information);
            }

            // Left column:        

            private void SetConfigEnableRIO_Enable(object sender, RoutedEventArgs e) { if (State.allowairioswitching) { if (!State.activeconfig.RIO_Enabled) { } State.activeconfig.RIO_Enabled = true; FileHandler.Lua.LuaFiles_Install(false, true); } else { RIO_Enable_Box.IsEnabled = false; } } // State.activeconfig.RIO_Enabled = false; 
            private void SetConfigDisableRIO_Enable(object sender, RoutedEventArgs e) { if (State.allowairioswitching) { if (State.activeconfig.RIO_Enabled) { } State.activeconfig.RIO_Enabled = false; FileHandler.Lua.LuaFiles_Install(false, true); } else { RIO_Enable_Box.IsEnabled = false; } } // State.activeconfig.RIO_Enabled = false;

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
                RIO_Enable_Box.IsChecked = State.activeconfig.RIO_Enabled;
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
                if (State.AIRIOactive)
                {
                    State.Proxy.Command.SetSessionEnabledByCategory("Keyword Collections", true);
                    State.Proxy.Command.SetSessionEnabledByCategory("Extension packs", true);
                    State.Proxy.State.SetListeningEnabled(true);
                }
            }
            private void SetConfigDisableRIO_ICShotmic(object sender, RoutedEventArgs e)
            {
                State.activeconfig.ICShotmic = false;
                if (State.AIRIOactive)
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
                RIO_ICShotmic.IsChecked = State.activeconfig.ICShotmic;
            }

            private void SetConfigEnableRIO_ICShotmic_useswitch(object sender, RoutedEventArgs e)
            {
                State.activeconfig.ICShotmic_useswitch = true;
                if (State.currentstate.riostate != null && State.currentstate.riostate.ics)
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
