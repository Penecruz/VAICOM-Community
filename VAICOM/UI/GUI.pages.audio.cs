using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using VAICOM.Extensions.WorldAudio;

namespace VAICOM
{
    namespace UI
    {

        public partial class ConfigWindow : Window
        {
            // ------------  AUDIO PAGE -----------------------------

            // World

            private void SetConfigEnableWorldSpeech(object sender, RoutedEventArgs e) { State.activeconfig.Redirect_World_Speech = true; }
            private void SetConfigDisableWorldSpeech(object sender, RoutedEventArgs e) { State.activeconfig.Redirect_World_Speech = false; }
            private void SetCurrentValueWorldSpeech(object sender, EventArgs e) { Redirect_World_Speech.IsEnabled = true; Redirect_World_Speech.IsChecked = State.activeconfig.Redirect_World_Speech; }

            // wingman

            private void SetConfigEnableWingman(object sender, RoutedEventArgs e) { State.activeconfig.Wingman_Emulate = true; }
            private void SetConfigDisableWingman(object sender, RoutedEventArgs e) { State.activeconfig.Wingman_Emulate = false; }
            private void SetCurrentValueWingman(object sender, EventArgs e) { Wingman_Enable.IsEnabled = State.dll_installed_wingman; Wingman_Enable.IsChecked = (State.activeconfig.Wingman_Emulate && State.dll_installed_wingman); }

            // no longer used
            private void Liveliness_Init(object sender, EventArgs e)
            {
                Wingman_Liveliness.IsEnabled = State.dll_installed_wingman;
                Wingman_Liveliness.Value = State.activeconfig.Wingman_Liveliness;
            }
            private void Liveliness_Change(object sender, RoutedPropertyChangedEventArgs<double> e)
            {
                if (State.activeconfig.Wingman_Liveliness != (int)e.NewValue)
                {
                    State.activeconfig.Wingman_Liveliness = (int)Wingman_Liveliness.Value;
                    Settings.ConfigFile.WriteConfigToFile(true);
                }
            }

            private void Set_Volume_init_World(object sender, EventArgs e)
            {
                SetWorldVolumeKnob();
            }

            public void SetWorldVolumeKnob()
            {
                Volume_World.IsEnabled = false;
                Double vol = State.activeconfig.TTSVolume;
                RotateTransform RotateTransform = new RotateTransform();
                RotateTransform.Angle = -150 + 300 * vol;
                Volume_World.RenderTransform = RotateTransform;
            }
            private void Set_Volume_Start_World(object sender, MouseButtonEventArgs e)
            {
                Mouse.Capture(Volume_World);

            }
            private void Set_Volume_Stop_World(object sender, MouseButtonEventArgs e)
            {
                Mouse.Capture(null);
            }
            private void Set_Volume_Change_World(object sender, MouseEventArgs e)
            {
                if (Mouse.Captured == Volume_World)
                {

                    Point currentLocation = Mouse.GetPosition(this);
                    Point knobCenter = new Point(Volume_World.Margin.Left + (Volume_World.ActualWidth / 2), Volume_World.Margin.Top + (Volume_World.ActualHeight / 2));

                    double delta = knobCenter.Y - currentLocation.Y;

                    if (delta > 150)
                    {
                        delta = 150;
                    }

                    if (delta < -150)
                    {
                        delta = -150;
                    }

                    Volume_World.RenderTransform = new RotateTransform() { Angle = delta };

                    State.activeconfig.TTSVolume = (float)(0.5 + (delta / 300));
                    Settings.ConfigFile.WriteConfigToFile(true);
                }
            }

            // for audio device
            private void AudioDeviceInitialValue(object sender, EventArgs e)
            {
                SetAudioDeviceSelector();
            }

            public void SetAudioDeviceSelector()
            {
                AudioDevice_Selector.IsEnabled = State.activeconfig.Redirect_World_Speech;
                string displaytext = "---";
                try
                {
                    if (State.activeconfig.Redirect_World_Speech)
                    {
                        AudioDevice_Selector.SelectedValue = State.activeconfig.AudioDeviceNumber;
                    }
                    else
                    {
                        displaytext = "(OFF)";
                        AudioDevice_Selector.Text = displaytext;
                    }
                }
                catch (Exception e)
                {
                    Log.Write(e.Message + e.StackTrace, Static.Colors.Inline);
                }

            }
            private void UpdateAudioDevice(object sender, SelectionChangedEventArgs e)
            {
                UpdateAudioDevice();
            }

            public void UpdateAudioDevice()
            {
                try
                {
                    if (!State.activeconfig.Redirect_World_Speech)
                    {
                        return;
                    }

                    int currentdevicenumber = State.activeconfig.AudioDeviceNumber;
                    int newvalue = AudioDevice_Selector.SelectedIndex - 1;

                    if (newvalue.Equals(-2)) //-2
                    {
                        // redirect is switched off
                    }
                    else
                    {
                        if (!newvalue.Equals(currentdevicenumber))
                        {
                            State.activeconfig.AudioDeviceNumber = newvalue;
                            Settings.ConfigFile.WriteConfigToFile(true);
                            Processor.WorldAudioRedirect();
                        }

                    }
                }
                catch
                {
                }
            }

            // for chatter (independent)
            private void Pan_Setting_Init(object sender, EventArgs e)
            {
                Chatter_Pan.IsEnabled = true;
                Chatter_Pan.Value = State.activeconfig.ChatterPanSetting;
            }
            private void Pan_Setting_Change(object sender, RoutedPropertyChangedEventArgs<double> e)
            {
                if (!State.activeconfig.ChatterPanSetting.Equals((int)e.NewValue))
                {
                    UI.Playsound.Soft_Click();
                    State.activeconfig.ChatterPanSetting = (int)Chatter_Pan.Value;
                    Settings.ConfigFile.WriteConfigToFile(true);
                }
            }

            private void Pan_TX1_Setting_Init(object sender, EventArgs e)
            {
                Pan_TX1.IsEnabled = false;
                Pan_TX1.Value = 0;
            }
            private void Pan_TX1_Setting_Change(object sender, RoutedPropertyChangedEventArgs<double> e)
            {
                if (!State.activeconfig.PanSetting_TX1.Equals((int)e.NewValue))
                {
                    UI.Playsound.Soft_Click();
                    State.activeconfig.PanSetting_TX1 = (int)Pan_TX1.Value;
                    Settings.ConfigFile.WriteConfigToFile(true);
                }
            }
            private void Pan_TX2_Setting_Init(object sender, EventArgs e)
            {
                Pan_TX2.IsEnabled = false;
                Pan_TX2.Value = 0;
            }
            private void Pan_TX2_Setting_Change(object sender, RoutedPropertyChangedEventArgs<double> e)
            {
                if (!State.activeconfig.PanSetting_TX2.Equals((int)e.NewValue))
                {
                    UI.Playsound.Soft_Click();
                    State.activeconfig.PanSetting_TX2 = (int)Pan_TX2.Value;
                    Settings.ConfigFile.WriteConfigToFile(true);
                }
            }
            private void Pan_TX3_Setting_Init(object sender, EventArgs e)
            {
                Pan_TX3.IsEnabled = false;
                Pan_TX3.Value = 0;
            }
            private void Pan_TX3_Setting_Change(object sender, RoutedPropertyChangedEventArgs<double> e)
            {
                if (!State.activeconfig.PanSetting_TX3.Equals((int)e.NewValue))
                {
                    UI.Playsound.Soft_Click();
                    State.activeconfig.PanSetting_TX3 = (int)Pan_TX3.Value;
                    Settings.ConfigFile.WriteConfigToFile(true);
                }
            }
            private void Pan_TX4_Setting_Init(object sender, EventArgs e)
            {
                Pan_TX4.IsEnabled = false;
                Pan_TX4.Value = 0;
            }
            private void Pan_TX4_Setting_Change(object sender, RoutedPropertyChangedEventArgs<double> e)
            {
                if (!State.activeconfig.PanSetting_TX4.Equals((int)e.NewValue))
                {
                    UI.Playsound.Soft_Click();
                    State.activeconfig.PanSetting_TX4 = (int)Pan_TX4.Value;
                    Settings.ConfigFile.WriteConfigToFile(true);
                }
            }
            private void Pan_TX5_Setting_Init(object sender, EventArgs e)
            {
                Pan_TX5.IsEnabled = false;
                Pan_TX5.Value = 0;
            }
            private void Pan_TX5_Setting_Change(object sender, RoutedPropertyChangedEventArgs<double> e)
            {
                if (!State.activeconfig.PanSetting_TX5.Equals((int)e.NewValue))
                {
                    UI.Playsound.Soft_Click();
                    State.activeconfig.PanSetting_TX5 = (int)Pan_TX5.Value;
                    Settings.ConfigFile.WriteConfigToFile(true);
                }
            }
            private void Pan_TX6_Setting_Init(object sender, EventArgs e)
            {
                Pan_TX6.IsEnabled = false;
                Pan_TX6.Value = 0;
            }
            private void Pan_TX6_Setting_Change(object sender, RoutedPropertyChangedEventArgs<double> e)
            {
                if (!State.activeconfig.PanSetting_TX6.Equals((int)e.NewValue))
                {
                    UI.Playsound.Soft_Click();
                    State.activeconfig.PanSetting_TX6 = (int)Pan_TX6.Value;
                    Settings.ConfigFile.WriteConfigToFile(true);
                }
            }

            private void Pan_AOCS_Setting_Init(object sender, EventArgs e)
            {
                Pan_AOCS.IsEnabled = true;
                Pan_AOCS.Value = State.activeconfig.PanSetting_AOCS;
            }
            private void Pan_AOCS_Setting_Change(object sender, RoutedPropertyChangedEventArgs<double> e)
            {
                if (!State.activeconfig.PanSetting_AOCS.Equals((int)e.NewValue))
                {
                    UI.Playsound.Soft_Click();
                    State.activeconfig.PanSetting_AOCS = (int)Pan_AOCS.Value;
                    Settings.ConfigFile.WriteConfigToFile(true);
                }
            }

            private void Init_Button_init(object sender, EventArgs e)
            {
                World_Init_Button.IsEnabled = State.activeconfig.Redirect_World_Speech;
            }
            private void WorldReset(object sender, MouseButtonEventArgs e) // press init button
            {
                if (State.activeconfig.Redirect_World_Speech)
                {
                    Processor.WorldAudioRedirect();
                    Playsound.TestNoise();
                    Playsound.Commandcomplete();
                }
                else
                {
                }
            }

            private void World_SetMode_Up(object sender, EventArgs e)
            {
                World_SetMode_Up();
            }
            private void World_SetMode_Dn(object sender, EventArgs e)
            {
                World_SetMode_Dn();
            }

            public void World_SetMode_Up()
            {
                if (State.activeconfig.Redirect_World_Speech)
                {
                    World_Sw_Up.Visibility = Visibility.Visible;
                }
                else
                {
                    World_Sw_Up.Visibility = Visibility.Hidden;
                }

                World_Sw_Up.IsEnabled = true;
            }
            public void World_SetMode_Dn()
            {

                if (State.activeconfig.Redirect_World_Speech)
                {
                    World_Sw_Dn.Visibility = Visibility.Hidden;
                }
                else
                {
                    World_Sw_Dn.Visibility = Visibility.Visible;
                }

                World_Sw_Dn.IsEnabled = true;

            }

            public void SetPanSwitches()
            {
                Pan_TX1.IsEnabled = false;
                Pan_TX2.IsEnabled = false;
                Pan_TX3.IsEnabled = false;
                Pan_TX4.IsEnabled = false;
                Pan_TX5.IsEnabled = false;
                Pan_TX6.IsEnabled = false;
            }

            private void World_Sw_to_Down(object sender, MouseButtonEventArgs e)
            {
                World_Sw_to_Down();
            }
            public void World_Sw_to_Down()
            {
                World_Sw_Up.Visibility = Visibility.Hidden;
                World_Sw_Dn.Visibility = Visibility.Visible;
                world_light_on.Visibility = Visibility.Hidden;
                world_light_off.Visibility = Visibility.Visible;
                UI.Playsound.Soft_Switch();
                State.activeconfig.Redirect_World_Speech = false;
                Settings.ConfigFile.WriteConfigToFile(true);
                World_Init_Button.IsEnabled = State.activeconfig.Redirect_World_Speech;

                SetAudioDeviceSelector();
                SetWorldVolumeKnob();
                SetPanSwitches();
            }
            private void World_Sw_to_Up(object sender, MouseButtonEventArgs e)
            {
                World_Sw_to_Up();
            }
            public void World_Sw_to_Up()
            {
                World_Sw_Up.Visibility = Visibility.Visible;
                World_Sw_Dn.Visibility = Visibility.Hidden;
                world_light_on.Visibility = Visibility.Visible;
                world_light_off.Visibility = Visibility.Hidden;
                UI.Playsound.Soft_Switch();
                State.activeconfig.Redirect_World_Speech = true;
                Settings.ConfigFile.WriteConfigToFile(true);
                World_Init_Button.IsEnabled = State.activeconfig.Redirect_World_Speech;

                SetAudioDeviceSelector();
                SetWorldVolumeKnob();
                SetPanSwitches();
            }

            private void World_Update_Light_On(object sender, EventArgs e)
            {
                if (State.activeconfig.Redirect_World_Speech)
                {
                    world_light_on.Visibility = Visibility.Visible;
                }
                else
                {
                    world_light_on.Visibility = Visibility.Hidden;
                }

            }
            private void World_Update_Light_Off(object sender, EventArgs e)
            {
                if (State.activeconfig.Redirect_World_Speech)
                {
                    world_light_off.Visibility = Visibility.Hidden;
                }
                else
                {
                    world_light_off.Visibility = Visibility.Visible;
                }
            }

            public void AlternateWorldLight()
            {
                if (State.activeconfig.Redirect_World_Speech)
                {
                    if (State.currentaudiodevicevalid)
                    {
                        world_light_on.Visibility = Visibility.Visible;
                        world_light_off.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        if (world_light_on.Visibility == Visibility.Hidden)
                        {
                            world_light_on.Visibility = Visibility.Visible;
                            world_light_off.Visibility = Visibility.Hidden;
                        }
                        else
                        {
                            world_light_on.Visibility = Visibility.Hidden;
                            world_light_off.Visibility = Visibility.Visible;
                        }
                    }
                }
                else
                {
                    world_light_on.Visibility = Visibility.Hidden;
                    world_light_off.Visibility = Visibility.Visible;
                }
            }

            // ------------------------------------------------------------
        }
    }
}



