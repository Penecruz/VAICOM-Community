using Newtonsoft.Json;
using System;
using System.IO;
using VAICOM.Database;
using VAICOM.Languages;
using VAICOM.Static;


namespace VAICOM
{
    namespace Settings
    {

        public partial class ConfigFile
        {

            public static void AddNewConfigItems()
            {

                try
                {
                    //added 2.9.2.3-->
                    if (State.activeconfig.Kneeboard_Enabled.Equals(null))
                    {
                        State.activeconfig.Kneeboard_Enabled = true;
                    }
                    
                    if (State.activeconfig.Chatter_Enabled.Equals(null))
                    {
                        State.activeconfig.Chatter_Enabled = true;
                    }

                    if (State.activeconfig.HideF4EDialog.Equals(null))
                    {
                        State.activeconfig.HideF4EDialog = false;
                    }

                    if (!ConfigContainsProperty("RIO_MiniWheel_Enabled"))
                    {
                        State.activeconfig.RIO_MiniWheel_Enabled = true;
                    }

                    State.activeconfig.RIO_Enabled = true;

                    // added 2.5.25
                    if (State.activeconfig.MP_UseTXLink.Equals(null))
                    {
                        State.activeconfig.MP_UseTXLink = false;
                    }

                    if (State.activeconfig.MP_TXLink_MPOnly.Equals(null))
                    {
                        State.activeconfig.MP_TXLink_MPOnly = true;
                    }

                    // added 2.5.23
                    if (State.activeconfig.KneeboardOpacity.Equals(null) || State.activeconfig.KneeboardOpacity.Equals(0))
                    {
                        State.activeconfig.KneeboardOpacity = 0.8;
                    }
                    if (State.activeconfig.KneeboardlinkPTT.Equals(null))
                    {
                        State.activeconfig.KneeboardlinkPTT = false;
                    }
                    if (State.activeconfig.OpenKneeboard_Out.Equals(null))
                    {
                        State.activeconfig.OpenKneeboard_Out = false;
                    }
                    if (!ConfigContainsProperty("OpenKneeboard_AutoBrowse"))
                    {
                        State.activeconfig.OpenKneeboard_AutoBrowse = State.activeconfig.KneeboardlinkPTT;
                    }
                    if (State.activeconfig.OpenKneeboard_Out_Port <= 0 || State.activeconfig.OpenKneeboard_Out_Port > 65535)
                    {
                        State.activeconfig.OpenKneeboard_Out_Port = 7779;
                    }
                    if (!ConfigContainsProperty("OpenKneeboard_EfbEnabled"))
                    {
                        State.activeconfig.OpenKneeboard_EfbEnabled = false;
                    }
                    if (!ConfigContainsProperty("OpenKneeboard_EfbAuthBlob"))
                    {
                        State.activeconfig.OpenKneeboard_EfbAuthBlob = "";
                    }
                    if (!ConfigContainsProperty("OpenKneeboard_EfbDevBypass"))
                    {
                        State.activeconfig.OpenKneeboard_EfbDevBypass = false;
                    }
                    if (!ConfigContainsProperty("OpenKneeboard_EfbOAuthClientId"))
                    {
                        State.activeconfig.OpenKneeboard_EfbOAuthClientId = "";
                    }
                    if (!ConfigContainsProperty("OpenKneeboard_EfbOAuthClientSecret"))
                    {
                        State.activeconfig.OpenKneeboard_EfbOAuthClientSecret = "";
                    }
                    if (!ConfigContainsProperty("OpenKneeboard_EfbOAuthScope"))
                    {
                        State.activeconfig.OpenKneeboard_EfbOAuthScope = "openid charts tiles offline_access";
                    }
                    if (!ConfigContainsProperty("OpenKneeboard_EfbOAuthDeviceAuthEndpoint"))
                    {
                        State.activeconfig.OpenKneeboard_EfbOAuthDeviceAuthEndpoint = "https://identity.api.navigraph.com/connect/deviceauthorization";
                    }
                    if (!ConfigContainsProperty("OpenKneeboard_EfbOAuthTokenEndpoint"))
                    {
                        State.activeconfig.OpenKneeboard_EfbOAuthTokenEndpoint = "https://identity.api.navigraph.com/connect/token";
                    }

                    // added 2.5.19
                    if (State.activeconfig.Custom_Path_Setting1.Equals(null))
                    {
                        State.activeconfig.Custom_Path_Setting1 = 0;
                    }
                    if (State.activeconfig.Custom_Path_Setting2.Equals(null))
                    {
                        State.activeconfig.Custom_Path_Setting2 = 0;
                    }

                    if (State.activeconfig.CustomFoldersFixReg.Equals(null))
                    {
                        State.activeconfig.CustomFoldersFixReg = false;
                    }

                    // added 2.5.17
                    if (State.activeconfig.CarrierSuppressAuto.Equals(null))
                    {
                        State.activeconfig.CarrierSuppressAuto = false;
                    }

                    // added 2.5.17
                    if (State.activeconfig.MP_VoIPSwitching.Equals(null))
                    {
                        State.activeconfig.MP_VoIPSwitching = 0;
                    }
                    if (State.activeconfig.MP_VoIPParallel.Equals(null))
                    {
                        State.activeconfig.MP_VoIPParallel = true;
                    }
                    if (State.activeconfig.MP_VoIPUseSwitch.Equals(null))
                    {
                        State.activeconfig.MP_VoIPUseSwitch = false;
                    }
                    if (State.activeconfig.MP_VoIPAutoSwitch.Equals(null))
                    {
                        State.activeconfig.MP_VoIPAutoSwitch = false;
                    }
                    if (State.activeconfig.MP_DelayTransmit.Equals(null))
                    {
                        State.activeconfig.MP_DelayTransmit = false;
                    }
                    if (State.activeconfig.MP_IgnoreSelect.Equals(null))
                    {
                        State.activeconfig.MP_IgnoreSelect = true;
                    }




                    // added 2.5.16
                    if (State.activeconfig.UseNewRecognitionModel.Equals(null))
                    {
                        State.activeconfig.UseNewRecognitionModel = false;
                    }
                    if (State.activeconfig.MP_AOCS.Equals(null))
                    {
                        State.activeconfig.MP_AOCS = true;
                    }
                    if (State.activeconfig.SRS_ClientSendIP.Equals(""))
                    {
                        State.activeconfig.SRS_ClientSendIP = "127.0.0.1";
                    }
                    if (State.activeconfig.SRS_ClientSendPort.Equals(null))
                    {
                        State.activeconfig.SRS_ClientSendPort = 33501;
                    }
                    if (State.activeconfig.SRS_ClientReceiveIP.Equals(null))
                    {
                        State.activeconfig.SRS_ClientReceiveIP = "*";
                    }
                    if (State.activeconfig.SRS_ClientReceivePort.Equals(null))
                    {
                        State.activeconfig.SRS_ClientReceivePort = 33502;
                    }
                    if (State.activeconfig.MP_UsePluginWithMultiplayer.Equals(null))
                    {
                        State.activeconfig.MP_UsePluginWithMultiplayer = false;
                    }
                    if (State.activeconfig.MP_UseSRSIntegration.Equals(null))
                    {
                        State.activeconfig.MP_UseSRSIntegration = false;
                    }
                    if (State.activeconfig.MP_VCHotMic.Equals(null))
                    {
                        State.activeconfig.MP_VCHotMic = false;
                    }
                    if (State.activeconfig.MP_SRSHotMic.Equals(null))
                    {
                        State.activeconfig.MP_SRSHotMic = false;
                    }
                    if (State.activeconfig.MP_WarnHumans.Equals(null))
                    {
                        State.activeconfig.MP_WarnHumans = true;
                    }
                    if (State.activeconfig.MP_MixedToAI.Equals(null))
                    {
                        State.activeconfig.MP_MixedToAI = false;
                    }
                    if (State.activeconfig.MP_MixedSwitchUsingRecipient.Equals(null))
                    {
                        State.activeconfig.MP_MixedSwitchUsingRecipient = false;
                    }
                    if (State.activeconfig.MP_ShowOnScreenMessages.Equals(null))
                    {
                        State.activeconfig.MP_ShowOnScreenMessages = false;
                    }

                    // added 2.5.14
                    if (State.activeconfig.ReleaseHot.Equals(null))
                    {
                        State.activeconfig.ReleaseHot = false;
                    }

                    // added 2.5.13
                    if (State.activeconfig.ForceLanguage.Equals(null))
                    {
                        State.activeconfig.ForceLanguage = false;
                    }
                    if (State.activeconfig.ForcedLanguage.Equals(null))
                    {
                        State.activeconfig.ForcedLanguage = 0;
                    }
                    if (State.activeconfig.EnforceCallsigns.Equals(null))
                    {
                        State.activeconfig.EnforceCallsigns = false;
                    }
                    if (State.activeconfig.CallsignsLanguage.Equals(null))
                    {
                        State.activeconfig.CallsignsLanguage = 0;
                    }
                    if (State.activeconfig.EnforceATCProtocol.Equals(null))
                    {
                        State.activeconfig.EnforceATCProtocol = true;
                    }
                    if (State.activeconfig.EnforcedATCProtocol.Equals(null))
                    {
                        State.activeconfig.EnforcedATCProtocol = 0;
                    }
                    if (State.activeconfig.CarrierComms.Equals(null))
                    {
                        State.activeconfig.CarrierComms = false;
                    }

                    // added 2.5.9.1
                    if (State.activeconfig.CurrentLanguage.Equals(null))
                    {
                        State.activeconfig.CurrentLanguage = language.ENG;
                    }

                    //new default settings in 2.5.7 / 2.5.8.1 / 2.5.9:
                    // aocs

                    if (State.activeconfig.AutoBrief.Equals(null))
                    {
                        State.activeconfig.AutoBrief = false;
                    }
                    if (State.activeconfig.BriefConcise.Equals(null))
                    {
                        State.activeconfig.BriefConcise = false;
                    }
                    if (State.activeconfig.DeepInterrogate.Equals(null))
                    {
                        State.activeconfig.DeepInterrogate = true;
                    }

                    //wingman

                    if (State.activeconfig.Wingman_Emulate.Equals(null))
                    {
                        State.activeconfig.Wingman_Emulate = false;
                    }
                    if (State.activeconfig.Wingman_Liveliness.Equals(null))
                    {
                        State.activeconfig.Wingman_Liveliness = 5;
                    }

                    // world

                    if (State.activeconfig.Redirect_World_Speech.Equals(null))
                    {
                        State.activeconfig.Redirect_World_Speech = false;
                    }
                    if (State.activeconfig.AudioDeviceNumber.Equals(null))
                    {
                        State.activeconfig.AudioDeviceNumber = 0;
                    }
                    if (State.activeconfig.TTSVolume.Equals(1))
                    {
                        State.activeconfig.TTSVolume = 0.5f;
                    }
                    if (State.activeconfig.ClientReceivePortMessages.Equals(null))
                    {
                        State.activeconfig.ClientReceivePortMessages = 44111;
                    }

                    if (State.activeconfig.PanSetting_TX1.Equals(null))
                    {
                        State.activeconfig.PanSetting_TX1 = 0;
                    }
                    if (State.activeconfig.PanSetting_TX2.Equals(null))
                    {
                        State.activeconfig.PanSetting_TX2 = 0;
                    }
                    if (State.activeconfig.PanSetting_TX3.Equals(null))
                    {
                        State.activeconfig.PanSetting_TX3 = 0;
                    }
                    if (State.activeconfig.PanSetting_TX4.Equals(null))
                    {
                        State.activeconfig.PanSetting_TX4 = 0;
                    }
                    if (State.activeconfig.PanSetting_TX5.Equals(null))
                    {
                        State.activeconfig.PanSetting_TX5 = 0;
                    }
                    if (State.activeconfig.PanSetting_TX6.Equals(null))
                    {
                        State.activeconfig.PanSetting_TX6 = 0;
                    }
                    if (State.activeconfig.PanSetting_AOCS.Equals(null))
                    {
                        State.activeconfig.PanSetting_AOCS = 0;
                    }
                    if (State.activeconfig.ChatterPanSetting.Equals(null))
                    {
                        State.activeconfig.ChatterPanSetting = 0;
                    }



                    if (State.activeconfig.ChatterVolume.Equals(1))
                    {
                        State.activeconfig.ChatterVolume = 0.4f;
                    }

                }
                catch
                {
                }

            }

            // restores / writes config file
            public static void WriteConfigToFile(bool overwrite)
            {
                string filename;
                if (State.databaseencrypted)
                { filename = Aliases.scrambleddbfilenames["config"]; }
                else
                { filename = "config" + ".json"; }

                Settings.Config WriteObject = new Settings.Config();
                try
                {
                    string path = State.VA_APPS + "\\" + AppData.RootFolder + "\\" + AppData.SubFolders["config"] + "\\" + filename;
                    WriteObject = State.activeconfig;
                    string jsonstring;
                    if (State.databaseencrypted)
                    {
                        jsonstring = Helpers.Crypto.Encrypt(JsonConvert.SerializeObject(WriteObject, Formatting.Indented));
                    }
                    else
                    {
                        jsonstring = JsonConvert.SerializeObject(WriteObject, Formatting.Indented);
                    }
                    if (overwrite || !File.Exists(path))
                    {
                        File.WriteAllText(path, jsonstring);
                    }
                }
                catch
                {
                }
            }

            // reads config file
            public static Settings.Config ReadConfigFromFile()
            {
                string filename;
                if (State.databaseencrypted)
                { filename = Aliases.scrambleddbfilenames["config"]; }
                else
                { filename = "config" + ".json"; }

                Settings.Config ReturnObject = new Settings.Config();
                try
                {
                    string path = State.VA_APPS + "\\" + AppData.RootFolder + "\\" + AppData.SubFolders["config"] + "\\" + filename;
                    if (State.databaseencrypted)
                    {
                        ReturnObject = JsonConvert.DeserializeObject<Settings.Config>(Helpers.Crypto.Decrypt(File.ReadAllText(path)));
                    }
                    else
                    {
                        ReturnObject = JsonConvert.DeserializeObject<Settings.Config>(File.ReadAllText(path));
                    }
                    Log.Write("Loaded config from file.", Colors.Text);
                }
                catch
                {
                }

                return (ReturnObject);
            }

            private static bool ConfigContainsProperty(string propertyName)
            {
                try
                {
                    string filename;
                    if (State.databaseencrypted)
                    { filename = Aliases.scrambleddbfilenames["config"]; }
                    else
                    { filename = "config" + ".json"; }

                    string path = State.VA_APPS + "\\" + AppData.RootFolder + "\\" + AppData.SubFolders["config"] + "\\" + filename;
                    if (!File.Exists(path))
                    {
                        return false;
                    }

                    string content = File.ReadAllText(path);
                    if (State.databaseencrypted)
                    {
                        content = Helpers.Crypto.Decrypt(content);
                    }

                    return content.IndexOf("\"" + propertyName + "\"", StringComparison.OrdinalIgnoreCase) >= 0;
                }
                catch
                {
                    return false;
                }
            }

        }
    }
}
