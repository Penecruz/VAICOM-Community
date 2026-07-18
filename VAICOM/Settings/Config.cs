using System.Windows;
using VAICOM.Languages;

namespace VAICOM
{
    namespace Settings
    {


        public partial class Config
        {

            public language CurrentLanguage;

            // ptt page
            public bool ForceMultiHotkey;
            public bool ForceSingleHotkey;
            public string SingleHotkey;
            public bool OperateDial;
            public int? SelectorMode;
            public bool UseSRSmapping;
            public bool PermaTXInfo;
            public bool WagsFundraiserDismissed;
            public bool MouseExternalTX;
            public bool ReleaseHot;

            // preferences page

            // left column
            public bool ImportOtherMenu;
            public bool MP_AOCS;

            public bool HideMenus;
            public bool EnforceATCProtocol;
            public int EnforcedATCProtocol;
            public bool EnforceCallsigns;
            public int CallsignsLanguage;
            public bool UseInstantSelect;
            public bool AllowRadioTuning;
            public bool ForceLanguage;
            public int ForcedLanguage;
            public bool CarrierComms;

            // right column
            public bool UIaddhints;
            public bool Engagecuerequired;
            public bool AllowAppendices;
            public bool AllowOptions;
            public bool AllowAddCommands;
            public bool DisableSquelch;
            public bool UIsounds;
            public bool DisablePlayerVoice;
            public bool HideOnScreenText;

            // ext page

            // RIO

            public bool RIO_Enabled;
            public bool RIO_MiniWheel_Enabled;
            public bool RIO_Messages;
            public bool RIO_Hints_Only;
            public bool ICShotmic;
            public bool ICShotmic_useswitch;
            public bool HideF4EDialog;

            // World

            public bool Redirect_World_Speech;
            public int AudioDeviceNumber;
            public float TTSVolume;

            public int PanSetting_TX1;
            public int PanSetting_TX2;
            public int PanSetting_TX3;
            public int PanSetting_TX4;
            public int PanSetting_TX5;
            public int PanSetting_TX6;
            public int PanSetting_AOCS;
            public int ChatterPanSetting;

            // carrier

            public bool CarrierSuppressAuto;

            // kneeboard

            public double KneeboardOpacity;
            public bool KneeboardlinkPTT;
            public bool Kneeboard_Enabled;
            public bool OpenKneeboard_Out;
            public bool OpenKneeboard_AutoBrowse;
            public int OpenKneeboard_Out_Port;

            // multiplayer page

            public bool MP_UseTXLink;
            public bool MP_TXLink_MPOnly;
            public bool MP_UsePluginWithMultiplayer;
            public bool MP_UseVoiceChatIntegration;
            public bool MP_UseSRSIntegration;
            public bool MP_VCHotMic;
            public bool MP_SRSHotMic;
            public bool MP_WarnHumans;
            public bool MP_MixedToHumans;
            public bool MP_MixedToAI;
            public bool MP_MixedSwitchUsingRecipient;
            public bool MP_ShowOnScreenMessages;
            public int MP_VoIPSwitching;
            public bool MP_VoIPSilent;
            public bool MP_VoIPParallel;
            public bool MP_VoIPUseSwitch;
            public bool MP_VoIPAutoSwitch;
            public bool MP_DelayTransmit;
            public bool MP_IgnoreSelect;
            public bool MP_AllowSwitchCommand;

            // wingman

            public bool Wingman_Emulate;
            public int Wingman_Liveliness;

            //AOCS
            public bool AutoBrief;
            public bool BriefConcise;
            public bool DeepInterrogate;

            // Chatter
            public bool Chatter_Enabled;
            public bool ChatterAutostart;
            public bool ChatterSilentOffline;
            public string ChatterFolder;
            public float ChatterVolume;

            // config page

            public bool Debugmode;
            public bool UseRemoteIP;
            public bool Checkfornewatcs;
            public bool UseCustomFolders;
            public string DCSfoldername1;
            public string DCSfoldername2;
            public bool CustomFoldersOBFlag;
            public bool AutoImportModules;
            public bool ManualInstallServerFiles;
            public bool DisableAutomaticUpdates;
            public bool AutomaticUpdateFinished;

            public int Custom_Path_Setting1;
            public int Custom_Path_Setting2;
            public bool CustomFoldersFixReg;

            // editor page
            public bool UseNewRecognitionModel;

            // about page

            public bool BetaTester;

            // reset options

            public bool Resetimported;
            public bool Resetdb;
            public bool Resetoptions;
            public bool Resetprofile;
            public bool Resettheme;
            public bool Resetlua;

            // deep config

            public bool UseUnloadOnExit;
            public Point Windowrestorelocation;
            public bool Runningforthefirsttime;
            public string ClientSendIP;
            public int ClientSendPort;
            public string ClientReceiveIP;
            public int ClientReceivePort;
            public bool Editorrequiresreload;
            public bool Editorunsavedchanges;
            public bool ForceAliasFinished;
            public int ClientReceivePortMessages;

            public string SRS_ClientSendIP;
            public int SRS_ClientSendPort;
            public string SRS_ClientReceiveIP;
            public int SRS_ClientReceivePort;

            public bool RequireFrequency281000 { get; set; } // New property
        }

        public static class Configs
        {

            public static Config Defaultconfig = new Config
            {

                CurrentLanguage = language.ENG,

                //PTT page

                ForceSingleHotkey = false,
                ForceMultiHotkey = false,
                SingleHotkey = "TX1",
                OperateDial = false,
                SelectorMode = null,
                UseSRSmapping = false,
                PermaTXInfo = false,
                WagsFundraiserDismissed = false,
                MouseExternalTX = false,
                ReleaseHot = false,

                // preferences page

                //L
                ImportOtherMenu = false,


                HideMenus = false,
                EnforceATCProtocol = true,
                EnforcedATCProtocol = 0,
                EnforceCallsigns = false,
                CallsignsLanguage = 0,

                UseInstantSelect = false,
                AllowRadioTuning = false,
                ForceLanguage = false,
                ForcedLanguage = 0,
                CarrierComms = false,

                //R
                UIaddhints = true,
                Engagecuerequired = true,
                AllowAppendices = false,
                AllowOptions = true,
                AllowAddCommands = false,
                DisableSquelch = false,
                UIsounds = true,
                DisablePlayerVoice = false,
                HideOnScreenText = false,

                // EX page 

                // RIO

                RIO_Messages = true,
                RIO_Enabled = true,
                RIO_MiniWheel_Enabled = true,
                RIO_Hints_Only = false,
                ICShotmic = false,
                ICShotmic_useswitch = false,
                HideF4EDialog = false,

                // World

                Redirect_World_Speech = false,

                PanSetting_TX1 = 0,
                PanSetting_TX2 = 0,
                PanSetting_TX3 = 0,
                PanSetting_TX4 = 0,
                PanSetting_TX5 = 0,
                PanSetting_TX6 = 0,
                PanSetting_AOCS = 0,

                AudioDeviceNumber = 0,
                TTSVolume = 0.5f,

                // Wingman

                Wingman_Emulate = false, // disabled for now
                Wingman_Liveliness = 5,

                // AOCS
                AutoBrief = false,
                BriefConcise = false,
                DeepInterrogate = true,

                // Chatter
                Chatter_Enabled = true,
                ChatterAutostart = false,
                ChatterSilentOffline = false,
                ChatterFolder = "(AUTO)",
                ChatterVolume = 0.4f,
                ChatterPanSetting = 0,

                // carrier

                CarrierSuppressAuto = false,

                // kneeboard

                KneeboardOpacity = 128,
                Kneeboard_Enabled = true,
                OpenKneeboard_Out = false,
                OpenKneeboard_AutoBrowse = false,
                OpenKneeboard_Out_Port = 7779,

                // multiplayer page

                MP_UseTXLink = false,
                MP_TXLink_MPOnly = true,
                MP_UsePluginWithMultiplayer = false,
                MP_UseVoiceChatIntegration = false,
                MP_UseSRSIntegration = false,
                MP_VCHotMic = false,
                MP_SRSHotMic = false,
                MP_WarnHumans = false,
                MP_MixedToHumans = false,
                MP_MixedToAI = false,
                MP_MixedSwitchUsingRecipient = false,
                MP_ShowOnScreenMessages = false,
                MP_AOCS = true,
                MP_VoIPSwitching = 0,
                MP_VoIPParallel = true,
                MP_VoIPSilent = false,
                MP_VoIPUseSwitch = false,
                MP_VoIPAutoSwitch = false,
                MP_DelayTransmit = false,
                MP_IgnoreSelect = true,
                MP_AllowSwitchCommand = true,

                // config page

                Debugmode = false,
                UseRemoteIP = false,
                Checkfornewatcs = false,
                UseCustomFolders = false,
                DCSfoldername1 = "",
                DCSfoldername2 = "",
                CustomFoldersOBFlag = false,
                AutoImportModules = false,
                ManualInstallServerFiles = false,
                DisableAutomaticUpdates = false,

                Custom_Path_Setting1 = 0,
                Custom_Path_Setting2 = 0,
                CustomFoldersFixReg = false,

                // editor page
                UseNewRecognitionModel = false,

                // Reset page

                Resetimported = false,
                Resetdb = false,
                Resetoptions = false,
                Resetprofile = false,
                Resettheme = false,
                Resetlua = false,

                // deep config stuff

                AutomaticUpdateFinished = false,
                BetaTester = true,
                Windowrestorelocation = new Point(20, 20),
                Runningforthefirsttime = false,
                ClientSendIP = "127.0.0.1",
                ClientSendPort = 33491,
                ClientReceiveIP = "*",
                ClientReceivePort = 33492,
                Editorunsavedchanges = false,
                Editorrequiresreload = false,
                ForceAliasFinished = false,
                ClientReceivePortMessages = 44111,

                SRS_ClientSendIP = "127.0.0.1",
                SRS_ClientSendPort = 33501,
                SRS_ClientReceiveIP = "*",
                SRS_ClientReceivePort = 33502,

                //Chatter
                RequireFrequency281000 = false, // Default value
            };

        }
    }
}
