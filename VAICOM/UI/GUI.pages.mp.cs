using System;
using System.Windows;
using VAICOM.PushToTalk;


namespace VAICOM
{
    namespace UI
    {

        public partial class ConfigWindow : Window
        {
            // ------------  MULTIPLAYER PAGE -----------------------------

            // use with multiplayer
            private void SetConfigEnableMP_UsePluginWithMultiplayer(object sender, RoutedEventArgs e) { State.activeconfig.MP_UsePluginWithMultiplayer = true; }
            private void SetConfigDisableMP_UsePluginWithMultiplayer(object sender, RoutedEventArgs e) { State.activeconfig.MP_UsePluginWithMultiplayer = false; }
            private void SetCurrentValueMP_UsePluginWithMultiplayer(object sender, EventArgs e) { MP_UsePluginWithMultiplayer.IsEnabled = true; MP_UsePluginWithMultiplayer.IsChecked = State.activeconfig.MP_UsePluginWithMultiplayer; }

            // MP_ShowOnScreenMessages;
            private void SetConfigEnableMP_ShowOnScreenMessages(object sender, RoutedEventArgs e) { State.activeconfig.MP_ShowOnScreenMessages = true; }
            private void SetConfigDisableMP_ShowOnScreenMessages(object sender, RoutedEventArgs e) { State.activeconfig.MP_ShowOnScreenMessages = false; }
            private void SetCurrentValueMP_ShowOnScreenMessages(object sender, EventArgs e) { MP_ShowOnScreenMessages.IsEnabled = true; MP_ShowOnScreenMessages.IsChecked = State.activeconfig.MP_ShowOnScreenMessages; }

            // MP_AOCS
            private void SetConfigEnableMP_AOCS(object sender, RoutedEventArgs e) { State.activeconfig.MP_AOCS = true; }
            private void SetConfigDisableMP_AOCS(object sender, RoutedEventArgs e) { State.activeconfig.MP_AOCS = false; }
            private void SetCurrentValueMP_AOCS(object sender, EventArgs e) { MP_AOCS.IsEnabled = true; MP_AOCS.IsChecked = State.activeconfig.MP_AOCS; } //

            // use VC integration
            private void SetConfigEnableMP_UseVoiceChatIntegration(object sender, RoutedEventArgs e) { State.activeconfig.MP_UseVoiceChatIntegration = true; }
            private void SetConfigDisableMP_UseVoiceChatIntegration(object sender, RoutedEventArgs e) { State.activeconfig.MP_UseVoiceChatIntegration = false; }
            private void SetCurrentValueMP_UseVoiceChatIntegration(object sender, EventArgs e) { MP_UseVoiceChatIntegration.IsEnabled = true; MP_UseVoiceChatIntegration.IsChecked = State.activeconfig.MP_UseVoiceChatIntegration; }

            // VC Hot mic
            private void SetConfigEnableMP_VCHotMic(object sender, RoutedEventArgs e) { State.activeconfig.MP_VCHotMic = true; PTT.PTT_Manage_Listen_VC(true); }
            private void SetConfigDisableMP_VCHotMic(object sender, RoutedEventArgs e) { State.activeconfig.MP_VCHotMic = false; PTT.PTT_Manage_Listen_VC(false); }
            private void SetCurrentValueMP_VCHotMic(object sender, EventArgs e) { MP_VCHotMic.IsEnabled = true; MP_VCHotMic.IsChecked = State.activeconfig.MP_VCHotMic; }

            // use SRS integration
            private void SetConfigEnableMP_UseSRSIntegration(object sender, RoutedEventArgs e) { State.activeconfig.MP_UseSRSIntegration = true; }
            private void SetConfigDisableMP_UseSRSIntegration(object sender, RoutedEventArgs e) { State.activeconfig.MP_UseSRSIntegration = false; }
            private void SetCurrentValueMP_UseSRSIntegration(object sender, EventArgs e) { MP_UseSRSIntegration.IsEnabled = true; MP_UseSRSIntegration.IsChecked = State.activeconfig.MP_UseSRSIntegration; }

            // use SRS mappping
            private void SetConfigEnableSRSMapping(object sender, RoutedEventArgs e) { State.activeconfig.UseSRSmapping = true; PTT.PTT_ApplyNewConfig(); updatepttinfo(); ChangeSRSbug(); Switch_SRS(); }
            private void SetConfigDisableSRSMapping(object sender, RoutedEventArgs e) { State.activeconfig.UseSRSmapping = false; PTT.PTT_ApplyNewConfig(); updatepttinfo(); ChangeSRSbug(); Switch_SRS(); }
            private void SetCurrentValueSRSMapping(object sender, EventArgs e) { UseSRSMapping.IsEnabled = true; UseSRSMapping.IsChecked = State.activeconfig.UseSRSmapping; PTT.PTT_ApplyNewConfig(); updatepttinfo(); ChangeSRSbug(); }

            // SRS ----
            private void SetConfigEnableMP_SRSHotMic(object sender, RoutedEventArgs e) { State.activeconfig.MP_SRSHotMic = true; PTT.PTT_Manage_Listen_SRS(true); }
            private void SetConfigDisableMP_SRSHotMic(object sender, RoutedEventArgs e) { State.activeconfig.MP_SRSHotMic = false; PTT.PTT_Manage_Listen_SRS(false); }
            private void SetCurrentValueMP_SRSHotMic(object sender, EventArgs e) { MP_SRSHotMic.IsEnabled = true; MP_SRSHotMic.IsChecked = State.activeconfig.MP_SRSHotMic; }

            // AutoSwitch
            private void SetConfigEnableMP_AutoSwitch(object sender, RoutedEventArgs e) { State.activeconfig.MP_VoIPAutoSwitch = true; }
            private void SetConfigDisableMP_AutoSwitch(object sender, RoutedEventArgs e) { State.activeconfig.MP_VoIPAutoSwitch = false; }
            private void SetCurrentValueMP_AutoSwitch(object sender, EventArgs e) { MP_AutoSwitch.IsEnabled = State.activeconfig.MP_VoIPUseSwitch; MP_AutoSwitch.IsChecked = State.activeconfig.MP_VoIPAutoSwitch; }

            // use TX link method
            private void SetConfigEnableMP_UseTXLink(object sender, RoutedEventArgs e) { State.activeconfig.MP_UseTXLink = true; }
            private void SetConfigDisableMP_UseTXLink(object sender, RoutedEventArgs e) { State.activeconfig.MP_UseTXLink = false; }
            private void SetCurrentValueMP_UseTXLink(object sender, EventArgs e) { MP_UseTXLink.IsEnabled = State.activeconfig.MP_VoIPUseSwitch; MP_UseTXLink.IsChecked = State.activeconfig.MP_UseTXLink; }

            // TX link MP only
            private void SetConfigEnableMP_TXLink_MPOnly(object sender, RoutedEventArgs e) { State.activeconfig.MP_TXLink_MPOnly = true; }
            private void SetConfigDisableMP_TXLink_MPOnly(object sender, RoutedEventArgs e) { State.activeconfig.MP_TXLink_MPOnly = false; }
            private void SetCurrentValueMP_TXLink_MPOnly(object sender, EventArgs e) { MP_TXLink_MPOnly.IsEnabled = true; MP_TXLink_MPOnly.IsChecked = State.activeconfig.MP_TXLink_MPOnly; }

            // Sound notify
            private void SetConfigEnableMP_WarnHumans(object sender, RoutedEventArgs e) { State.activeconfig.MP_WarnHumans = true; }
            private void SetConfigDisableMP_WarnHumans(object sender, RoutedEventArgs e) { State.activeconfig.MP_WarnHumans = false; }
            private void SetCurrentValueMP_WarnHumans(object sender, EventArgs e) { MP_WarnHumans.IsEnabled = true; MP_WarnHumans.IsChecked = State.activeconfig.MP_WarnHumans; }

            // delay transmit
            private void SetConfigEnableMP_DelayTransmit(object sender, RoutedEventArgs e) { State.activeconfig.MP_DelayTransmit = true; }
            private void SetConfigDisableMP_DelayTransmit(object sender, RoutedEventArgs e) { State.activeconfig.MP_DelayTransmit = false; }
            private void SetCurrentValueMP_DelayTransmit(object sender, EventArgs e) { MP_DelayTransmit.IsEnabled = State.activeconfig.MP_VoIPUseSwitch || State.activeconfig.MP_VoIPParallel; MP_DelayTransmit.IsChecked = State.activeconfig.MP_DelayTransmit; }

            // ignore select
            private void SetConfigEnableMP_IgnoreSelect(object sender, RoutedEventArgs e) { State.activeconfig.MP_IgnoreSelect = true; }
            private void SetConfigDisableMP_IgnoreSelect(object sender, RoutedEventArgs e) { State.activeconfig.MP_IgnoreSelect = false; }
            private void SetCurrentValueMP_IgnoreSelect(object sender, EventArgs e) { MP_IgnoreSelect.IsEnabled = State.activeconfig.MP_VoIPUseSwitch; MP_IgnoreSelect.IsChecked = State.activeconfig.MP_IgnoreSelect; }

            // allow switch command
            private void SetConfigEnableMP_AllowSwitchCommand(object sender, RoutedEventArgs e) { State.activeconfig.MP_AllowSwitchCommand = true; }
            private void SetConfigDisableMP_AllowSwitchCommand(object sender, RoutedEventArgs e) { State.activeconfig.MP_AllowSwitchCommand = false; }
            private void SetCurrentValueMP_AllowSwitchCommand(object sender, EventArgs e) { MP_AllowSwitchCommand.IsEnabled = State.activeconfig.MP_VoIPUseSwitch; MP_AllowSwitchCommand.IsChecked = State.activeconfig.MP_AllowSwitchCommand; }

            private void SetCurrentValueVoIPSwitching(object sender, EventArgs e) { VoIPSwitching.IsEnabled = true; VoIPSwitching.Value = State.activeconfig.MP_VoIPSwitching; }
            private void VoIPSwitching_Change(object sender, RoutedPropertyChangedEventArgs<double> e)
            {

                if (!State.activeconfig.MP_VoIPSwitching.Equals((int)e.NewValue))
                {
                    UI.Playsound.Soft_Click();
                    State.activeconfig.MP_VoIPSwitching = (int)VoIPSwitching.Value;
                    SetNotify();

                    switch (State.activeconfig.MP_VoIPSwitching)
                    {

                        case 0: //parallel
                            State.activeconfig.MP_VoIPParallel = true;
                            State.activeconfig.MP_UseTXLink = false;
                            State.activeconfig.MP_VoIPUseSwitch = false;
                            break;

                        case 1: // TX Link
                            State.activeconfig.MP_VoIPParallel = false;
                            State.activeconfig.MP_UseTXLink = true;
                            State.activeconfig.MP_VoIPUseSwitch = false;
                            break;

                        case 2: // use switching
                            State.activeconfig.MP_VoIPParallel = false;
                            State.activeconfig.MP_UseTXLink = false;
                            State.activeconfig.MP_VoIPUseSwitch = true;
                            break;

                        default: //0
                            State.activeconfig.MP_VoIPParallel = true;
                            State.activeconfig.MP_UseTXLink = false;
                            State.activeconfig.MP_VoIPUseSwitch = false;
                            break;

                    }

                    Settings.ConfigFile.WriteConfigToFile(true);

                }

            }

            public void SetNotify()
            {
                int slider = State.activeconfig.MP_VoIPSwitching;

                MP_AutoSwitch.IsEnabled = slider.Equals(2);
                MP_WarnHumans.IsEnabled = true;
                MP_DelayTransmit.IsEnabled = slider.Equals(2) || slider.Equals(0); // Enable for Dynamic Switching and Broadcast Parallel
                MP_IgnoreSelect.IsEnabled = slider.Equals(2);
                MP_AllowSwitchCommand.IsEnabled = slider.Equals(2);
                MP_UseTXLink.IsEnabled = slider.Equals(2);
            }


            // ------------------------------------------------------------
        }
    }
}
