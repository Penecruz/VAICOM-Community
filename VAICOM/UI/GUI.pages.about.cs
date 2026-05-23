using System;
using System.Windows;
using System.Windows.Input;


namespace VAICOM
{
    namespace UI
    {

        public partial class ConfigWindow : Window
        {

            // ------------  LICENSE PAGE ---------------------------------


            private void setproductname(object sender, EventArgs e)
            {
                showproductname();
            }

            public void showproductname()
            {
                productname.Text = "Community Edition";

            }

            public void Alternateupdatebug()
            {
                if (!State.updateavailable_plugin)
                {
                    tagline.Visibility = Visibility.Hidden; // visible
                }
                else
                {
                    tagline.Visibility = Visibility.Hidden;
                }
            }

            public void resetenabledfeatures()
            {
                // preferences

                // left column
                ImportOtherMenu.IsEnabled = true;
                MP_AOCS.IsEnabled = true;
                UseSRSMapping.IsEnabled = true;
                DisableMenus.IsEnabled = true;
                EnforceATCProtocol.IsEnabled = true;
                EnforceCallsigns.IsEnabled = true;
                UseInstantSelect.IsEnabled = true;
                AllowRadioTuning.IsEnabled = true;
                ForceLanguage.IsEnabled = true;
                ForcedLanguage.IsEnabled = true;
                EnforceCallsigns.IsEnabled = true;
                CallsignsLanguage.IsEnabled = true;
                EnforceATCProtocol.IsEnabled = true;
                ATCProtocol.IsEnabled = true;
                UseVoiceAccessPriority.IsEnabled = false; // Temporarily disable the checkbox until Voice Access is fully supported.

                // right column
                UIaddhints.IsEnabled = true;
                RequireCues.IsEnabled = true;
                AllowAppendices.IsEnabled = true;
                AllowOptions.IsEnabled = true;
                AllowAddCommands.IsEnabled = true;
                DisableSquelch.IsEnabled = true;
                UIsounds.IsEnabled = true;
                DisablePlayerVoice.IsEnabled = true;


                // MP page
                MP_UsePluginWithMultiplayer.IsEnabled = true;
                MP_ShowOnScreenMessages.IsEnabled = true;
                MP_AOCS.IsEnabled = true;

                VoIPSwitching.IsEnabled = true;
                MP_AutoSwitch.IsEnabled = true;
                MP_WarnHumans.IsEnabled = true;
                MP_DelayTransmit.IsEnabled = true;
                MP_IgnoreSelect.IsEnabled = true;


                MP_UseVoiceChatIntegration.IsEnabled = true;
                MP_VCHotMic.IsEnabled = true;

                MP_UseSRSIntegration.IsEnabled = true;
                UseSRSMapping.IsEnabled = true;

                // EX extensions page

                RIO_Hints.IsEnabled = true;
                if (State.dll_installed_rio)
                {
                    RIO_Hints.Visibility = Visibility.Visible;
                }
                else
                {
                    RIO_Hints.Visibility = Visibility.Hidden;
                }

                RIO_Enable_Box.IsEnabled = true;
                if (State.dll_installed_rio)
                {
                    RIO_Enable_Box.Visibility = Visibility.Visible;
                }
                else
                {
                    RIO_Enable_Box.Visibility = Visibility.Hidden;
                }

                RIO_Hints_Only.IsEnabled = true;
                if (State.dll_installed_rio)
                {
                    RIO_Hints_Only.Visibility = Visibility.Visible;
                }
                else
                {
                    RIO_Hints_Only.Visibility = Visibility.Hidden;
                }

                RIO_ICShotmic.IsEnabled = true;
                if (State.dll_installed_rio)
                {
                    RIO_ICShotmic.Visibility = Visibility.Visible;
                }
                else
                {
                    RIO_ICShotmic.Visibility = Visibility.Hidden;
                }
                RIO_ICShotmic_useswitch.IsEnabled = false;
                if (State.dll_installed_rio)
                {
                    RIO_ICShotmic_useswitch.Visibility = Visibility.Hidden;
                }
                else
                {
                    RIO_ICShotmic_useswitch.Visibility = Visibility.Hidden;
                }

                autobrief.IsEnabled = true;
                briefconcise.IsEnabled = true;
                deepinterrogate.IsEnabled = true;
                ChatterAutostart.IsEnabled = true;
                ChatterSilentOffline.IsEnabled = true;
                ChatterTheme.IsEnabled = true;

                CarrierComms.IsEnabled = true;
                CarrierComms.IsChecked = true;

                //audio page

                Pan_AOCS.IsEnabled = true;
                Chatter_Pan.IsEnabled = true;
                World_Sw_Up.IsEnabled = true;
                World_Sw_Dn.IsEnabled = true;
                World_Init_Button.IsEnabled = true;


                // config page
                RunInDebugMode.IsEnabled = true;
                AutoImportATC.IsEnabled = true;
                UseCustomFolders.IsEnabled = true;
                SetFolders.IsEnabled = true;
                AutoImportModules.IsEnabled = true;
                ManualManageServer.IsEnabled = true;
                ExportFiles.IsEnabled = true;

                // PTT page
                ForceMultiHotkey.IsEnabled = true;
                ForceSingleHotkey.IsEnabled = true;
                HotkeySelection.IsEnabled = true;
                OperateDial.IsEnabled = true;
                Volume_Knob.IsEnabled = true;
                Button_Top_TX1.IsEnabled = true;
                Button_Top_TX2.IsEnabled = true;
                Button_Top_TX3.IsEnabled = true;
                Button_Top_TX4.IsEnabled = true;
                Button_Top_TX5.IsEnabled = true;
                Button_Top_TX6.IsEnabled = true;

                // keywords editor
                AliasCycle.IsEnabled = true;
                comboBoxAlias.IsEnabled = true;
                comboBoxAlias.IsEditable = true;
                Microphone.IsEnabled = true;
                newalias.IsEnabled = true;
                deletealias.IsEnabled = true;
                Revert_Button.IsEnabled = true;
                Apply_Button.IsEnabled = true;
                Restore_Button.IsEnabled = true;
                ExportCSVbutton.IsEnabled = true;
                KeywordsExport.IsEnabled = true;
                Cancel.IsEnabled = true;
                ImportCSVbutton_Add.IsEnabled = true;
               
            }

            private void ShowLicenses(object sender, MouseButtonEventArgs e)
            {

                string caption = "VAICOM PRO Community Edition";
                string message = "THE SOFTWARE IS PROVIDED \"AS IS\", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.";

                MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
