using System;
using System.Windows;
using VAICOM.Client;
using VAICOM.FileManager;


namespace VAICOM
{
    namespace UI
    {

        public partial class ConfigWindow : Window
        {
            // ------------  PREFERENCES PAGE -----------------------------

            // Left column:

            // ImportF10Menu
            private void SetConfigEnableImportOtherMenu(object sender, RoutedEventArgs e) { State.activeconfig.ImportOtherMenu = true; }
            private void SetConfigDisableImportOtherMenu(object sender, RoutedEventArgs e) { State.activeconfig.ImportOtherMenu = false; }
            private void SetCurrentValueImportOtherMenu(object sender, EventArgs e) { ImportOtherMenu.IsEnabled = true; ImportOtherMenu.IsChecked = State.activeconfig.ImportOtherMenu; }


            // disable menus
            private void SetConfigHideMenus(object sender, RoutedEventArgs e) { State.activeconfig.HideMenus = true; }
            private void SetConfigShowMenus(object sender, RoutedEventArgs e) { State.activeconfig.HideMenus = false; }
            private void SetCurrentValueHideMenus(object sender, EventArgs e) { DisableMenus.IsEnabled = true; DisableMenus.IsChecked = State.activeconfig.HideMenus; }
            // force protocol (ATC names)
            private void EnforceProtocolNATOOn(object sender, RoutedEventArgs e) { State.activeconfig.EnforceATCProtocol = true; }
            private void EnforceProtocolNATOOff(object sender, RoutedEventArgs e) { State.activeconfig.EnforceATCProtocol = false; }
            private void SetCurrentValueProtocolNATO(object sender, EventArgs e) { EnforceATCProtocol.IsEnabled = true; EnforceATCProtocol.IsChecked = State.activeconfig.EnforceATCProtocol; }
            // Force Protocol (Slider)
            private void SetCurrentValueATCProtocol(object sender, EventArgs e) { ATCProtocol.IsEnabled = true; ATCProtocol.Value = State.activeconfig.EnforcedATCProtocol; }
            private void ATCProtocol_Change(object sender, RoutedPropertyChangedEventArgs<double> e)
            {
                if (!State.activeconfig.EnforcedATCProtocol.Equals((int)e.NewValue))
                {
                    UI.Playsound.Soft_Click();
                    State.activeconfig.EnforcedATCProtocol = (int)ATCProtocol.Value;
                    Settings.ConfigFile.WriteConfigToFile(true);
                    DcsClient.SendSettingsChange();
                }
            }
            private void Setlabellangvisible(object sender, EventArgs e) { labellang.Visibility = Visibility.Visible; }
            // force callsigns
            private void EnforceCallsignsOn(object sender, RoutedEventArgs e) { State.activeconfig.EnforceCallsigns = true; }
            private void EnforceCallsignsOff(object sender, RoutedEventArgs e) { State.activeconfig.EnforceCallsigns = false; }
            private void SetCurrentValueEnforceCallsigns(object sender, EventArgs e) { EnforceCallsigns.IsEnabled = true; EnforceCallsigns.IsChecked = State.activeconfig.EnforceCallsigns; }
            // Callsigns Language (Slider)
            private void SetCurrentValueCallsignsLanguage(object sender, EventArgs e) { CallsignsLanguage.IsEnabled = true; CallsignsLanguage.Value = State.activeconfig.CallsignsLanguage; }
            private void CallsignsLanguage_Change(object sender, RoutedPropertyChangedEventArgs<double> e)
            {
                if (!State.activeconfig.CallsignsLanguage.Equals((int)e.NewValue))
                {
                    UI.Playsound.Soft_Click();
                    State.activeconfig.CallsignsLanguage = (int)CallsignsLanguage.Value;
                    Settings.ConfigFile.WriteConfigToFile(true);
                    DcsClient.SendSettingsChange();
                }
            }

            // VSPX recognition model
            private void SetConfigEnableNewRecognitionModel(object sender, RoutedEventArgs e) { State.activeconfig.UseNewRecognitionModel = true; ReloadDB(); }
            private void SetConfigDisableNewRecognitionModel(object sender, RoutedEventArgs e) { State.activeconfig.UseNewRecognitionModel = false; ReloadDB(); }
            private void SetCurrentValueNewRecognitionModel(object sender, EventArgs e) { NewRecognitionModel.IsEnabled = true; NewRecognitionModel.IsChecked = State.activeconfig.UseNewRecognitionModel; }
            private void ChangedRecognitionModel(object sender, RoutedEventArgs e)
            {
                State.activeconfig.Editorunsavedchanges = true;
                Settings.ConfigFile.WriteConfigToFile(true);
                Toggle_VSPX_Light();
                Reflectunsavedchanges();
                WarnChangedRecognitionModel();
            }
            public void WarnChangedRecognitionModel()
            {
                string caption = "Changed Processing Model";
                string message = "A different speech processing model was selected.\n\nIMPORTANT: YOU MUST UPDATE YOUR VOICEATTACK PROFILE AND VOICEATTACK SETTING.\n\n1) On the Editor tab, press the FINISH button and follow the steps to update your VoiceAttack profile.\n2) In VoiceAttack options, adjust the Unrecognized Speech Delay value.\n\nUnrecognized Speech Delay\n\nVSPX ON : set value between 500 and 1000\nVSPX OFF: set value to 0\n\nFor details about the different recognition models refer to the User Manual.";
                MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Information);
            }

            // Force Language
            private void ForceLanguageOn(object sender, RoutedEventArgs e) { State.activeconfig.ForceLanguage = true; }
            private void ForceLanguageOff(object sender, RoutedEventArgs e) { State.activeconfig.ForceLanguage = false; }
            private void SetCurrentValueForceLanguage(object sender, EventArgs e) { ForceLanguage.IsEnabled = true; ForceLanguage.IsChecked = State.activeconfig.ForceLanguage; }
            // Forced Language (Slider)
            private void SetCurrentValueForcedLanguage(object sender, EventArgs e) { ForcedLanguage.IsEnabled = true; ForcedLanguage.Value = State.activeconfig.ForcedLanguage; }
            private void ForcedLanguage_Change(object sender, RoutedPropertyChangedEventArgs<double> e)
            {
                if (!State.activeconfig.ForcedLanguage.Equals((int)e.NewValue))
                {
                    UI.Playsound.Soft_Click();
                    State.activeconfig.ForcedLanguage = (int)ForcedLanguage.Value;
                    Settings.ConfigFile.WriteConfigToFile(true);
                    DcsClient.SendSettingsChange();
                }
            }
            // use Instant Select
            private void UseInstantSelectOn(object sender, RoutedEventArgs e) { State.activeconfig.UseInstantSelect = true; ChangeIselbug(); }
            private void UseInstantSelectOff(object sender, RoutedEventArgs e) { State.activeconfig.UseInstantSelect = false; ChangeIselbug(); }
            private void SetCurrentValueUseInstantSelect(object sender, EventArgs e) { UseInstantSelect.IsEnabled = true; UseInstantSelect.IsChecked = State.activeconfig.UseInstantSelect; ChangeIselbug(); }
            // Allow Radio Tuning
            private void AllowRadioTuningOn(object sender, RoutedEventArgs e) { State.activeconfig.AllowRadioTuning = true; }
            private void AllowRadioTuningOff(object sender, RoutedEventArgs e) { State.activeconfig.AllowRadioTuning = false; }
            private void SetCurrentValueAllowRadioTuning(object sender, EventArgs e) { AllowRadioTuning.IsEnabled = true; AllowRadioTuning.IsChecked = State.activeconfig.AllowRadioTuning; }


            // Right column:

            // use hints
            private void UIaddhintsOn(object sender, RoutedEventArgs e) { State.activeconfig.UIaddhints = true; }
            private void UIaddhintsOff(object sender, RoutedEventArgs e) { State.activeconfig.UIaddhints = false; }
            private void SetCurrentValueUIaddhints(object sender, EventArgs e) { UIaddhints.IsEnabled = true; UIaddhints.IsChecked = State.activeconfig.UIaddhints; }
            // engage cue required
            private void EngageCueRequiredOn(object sender, RoutedEventArgs e) { State.activeconfig.Engagecuerequired = true; }
            private void EngageCueRequiredOff(object sender, RoutedEventArgs e) { State.activeconfig.Engagecuerequired = false; }
            private void SetCurrentValueEngageCueRequired(object sender, EventArgs e) { RequireCues.IsEnabled = true; RequireCues.IsChecked = State.activeconfig.Engagecuerequired; }
            // allow appendices
            private void AllowAppendicesOn(object sender, RoutedEventArgs e) { State.activeconfig.AllowAppendices = true; }
            private void AllowAppendicesOff(object sender, RoutedEventArgs e) { State.activeconfig.AllowAppendices = false; }
            private void SetCurrentValueAllowAppendices(object sender, EventArgs e) { AllowAppendices.IsEnabled = true; AllowAppendices.IsChecked = State.activeconfig.AllowAppendices; }
            // allow Options command
            private void AllowOptionsOn(object sender, RoutedEventArgs e) { State.activeconfig.AllowOptions = true; }
            private void AllowOptionsOff(object sender, RoutedEventArgs e) { State.activeconfig.AllowOptions = false; }
            private void SetCurrentValueAllowOptions(object sender, EventArgs e) { AllowOptions.IsEnabled = true; AllowOptions.IsChecked = State.activeconfig.AllowOptions; }
            // allow additional commands
            private void AllowAddCommandsOn(object sender, RoutedEventArgs e) { State.activeconfig.AllowAddCommands = true; }
            private void AllowAddCommandsOff(object sender, RoutedEventArgs e) { State.activeconfig.AllowAddCommands = false; }
            private void SetCurrentValueAllowAddCommands(object sender, EventArgs e) { AllowAddCommands.IsEnabled = true; AllowAddCommands.IsChecked = State.activeconfig.AllowAddCommands; }
            // squelch
            private void DisableSquelchOn(object sender, RoutedEventArgs e) { State.activeconfig.DisableSquelch = true; }
            private void DisableSquelchOff(object sender, RoutedEventArgs e) { State.activeconfig.DisableSquelch = false; }
            private void SetCurrentValueDisableSquelch(object sender, EventArgs e) { DisableSquelch.IsEnabled = true; DisableSquelch.IsChecked = State.activeconfig.DisableSquelch; }
            // UI sounds
            private void UIsoundsOn(object sender, RoutedEventArgs e) { State.activeconfig.UIsounds = true; }
            private void UIsoundsOff(object sender, RoutedEventArgs e) { State.activeconfig.UIsounds = false; }
            private void SetCurrentValueUIsounds(object sender, EventArgs e) { UIsounds.IsEnabled = true; UIsounds.IsChecked = State.activeconfig.UIsounds; }
            // disable player voice
            private void SetConfigEnablePlayerVoice(object sender, RoutedEventArgs e) { State.activeconfig.DisablePlayerVoice = false; }
            private void SetConfigDisablePlayerVoice(object sender, RoutedEventArgs e) { State.activeconfig.DisablePlayerVoice = true; }
            private void SetCurrentValueDisablePlayerVoice(object sender, EventArgs e) { DisablePlayerVoice.IsEnabled = true; DisablePlayerVoice.IsChecked = State.activeconfig.DisablePlayerVoice; }
            // hide on screen text
            private void SetConfigEnableHideOnScreenText(object sender, RoutedEventArgs e) { State.activeconfig.HideOnScreenText = true; FileHandler.Lua.LuaFiles["2.8 gameMessages.lua"].reset = !State.activeconfig.HideOnScreenText; FileHandler.Lua.LuaFiles_Install(false, true); }
            private void SetConfigDisableHideOnScreenText(object sender, RoutedEventArgs e) { State.activeconfig.HideOnScreenText = false; FileHandler.Lua.LuaFiles["2.8 gameMessages.lua"].reset = !State.activeconfig.HideOnScreenText; FileHandler.Lua.LuaFiles_Install(false, true); }
            private void SetCurrentValueHideOnScreenText(object sender, EventArgs e) { HideOnScreenText.IsEnabled = true; HideOnScreenText.IsChecked = State.activeconfig.HideOnScreenText; }

            // Enable Voice Access Priority
            private void EnableVoiceAccessPriority(object sender, RoutedEventArgs e)
            {
                State.UseVoiceAccessPriority = true;
                Settings.ConfigFile.WriteConfigToFile(true); // Save the setting
            }

            // Disable Voice Access Priority
            private void DisableVoiceAccessPriority(object sender, RoutedEventArgs e)
            {
                State.UseVoiceAccessPriority = false;
                Settings.ConfigFile.WriteConfigToFile(true); // Save the setting
            }

            // Set the current value of the checkbox
            private void SetCurrentValueVoiceAccessPriority(object sender, EventArgs e)
            {
                UseVoiceAccessPriority.IsEnabled = false; // Temporarily disable until fully implemented
                UseVoiceAccessPriority.IsChecked = State.UseVoiceAccessPriority; // Reflect the current state
            }  

            // ------------------------------------------------------------

        }
    }
}
