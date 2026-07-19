using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Reflection;
using System.Resources;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Threading;
using VAICOM.Client;
using VAICOM.Database;
using VAICOM.Extensions.Kneeboard;
using VAICOM.Extensions.WorldAudio;
using VAICOM.Products;
using VAICOM.Servers;
using VAICOM.Static;

namespace VAICOM
{

    public static class State

    {

        // ------------------------------------------------------------------------------------------------------------
        // general


        public static bool versionbeta = false; //set if Beta version
        public static bool versiondev = false; //set if Dev version
        public static bool usenewselectmethod = false;
        public static string debuguser = "VAICOM_Tester";
        public static string client = "VAICOM";
        public static string clientmode = ClientModes.Normal; //set to Normal for release, Debug for development

        public static string versionstring = "";
        public static string pluginversionnumber = "3.1.5"; // used by Theme (Special page)
        public static string vaminversion = "1.16";
        public static string defProfileName = "VAICOM for DCS World";
        // Add a new property to control Voice Access priority
        public static bool UseVoiceAccessPriority { get; set; } = false; // Default to unchecked

        public static bool deepdebugmode = false; //set to deepdebug mode
        public static bool databaseencrypted = true; //set to true if database is encrypted, false if not
        public static bool luahardreset = true;
        public static bool exitapp = false;
        public static bool datawasreset = false;

        // ------------------------------------------------------------------------------------------------------------
        // assembly info

        public static AssemblyName dll_info_plugin = new AssemblyName() { Version = Version.Parse("0.0.0.0") };
        public static AssemblyName dll_info_chatter = new AssemblyName() { Version = Version.Parse("0.0.0.0") };
        public static AssemblyName dll_info_rio = new AssemblyName() { Version = Version.Parse("0.0.0.0") };
        public static AssemblyName dll_info_wingman = new AssemblyName() { Version = Version.Parse("0.0.0.0") };

        public static string dll_version_plugin = "";
        public static string dll_version_chatter = "";
        public static string dll_version_rio = "";
        public static string dll_version_wingman = "";

        public static bool dll_installed_plugin = true;
        public static bool dll_installed_chatter = false;
        public static bool dll_installed_rio = false;
        public static bool dll_installed_wingman = false;

        public static bool updateavailable_plugin = false;
        public static bool updateavailable_chatter = false;
        public static bool updateavailable_rio = false;
        public static bool updateavailable_wingman = false;

        // ------------------------------------------------------------------------------------------------------------
        // AIRIO

        public static string riomod = "F-14AB";

        public static bool IsAirioTomcatModule()
        {
            if (currentmodule != null)
            {
                DCSmodule f14ab;
                if (Products.DCSmodules.LookupTable.TryGetValue("F-14AB", out f14ab) && currentmodule.Equals(f14ab))
                {
                    return true;
                }

                DCSmodule f14bu;
                if (Products.DCSmodules.LookupTable.TryGetValue("F-14BU", out f14bu) && currentmodule.Equals(f14bu))
                {
                    return true;
                }
            }

            if (currentstate == null || string.IsNullOrWhiteSpace(currentstate.id))
            {
                return false;
            }

            return currentstate.id.Equals("F-14AB", StringComparison.OrdinalIgnoreCase)
                || currentstate.id.Equals("F-14BU", StringComparison.OrdinalIgnoreCase);
        }

        // ------------------------------------------------------------------------------------------------------------
        // VA system environment

        public static dynamic Proxy;
        public static string VA_DIR;
        public static string VA_APPS;
        public static string VA_SOUNDS;
        public static void SetEnvironment(dynamic vaProxy)
        {
            VA_DIR = vaProxy.SessionState["VA_DIR"];
            VA_APPS = vaProxy.SessionState["VA_APPS"];
            VA_SOUNDS = vaProxy.SessionState["VA_SOUNDS"];
        }

        // ------------------------------------------------------------------------------------------------------------      
        // network UDP stuff

        public static Socket SendSocket;
        public static UdpClient SendingUdpClient;
        public static IPEndPoint SendIpEndPoint;
        public static AsyncCallback ReturnCall;
        public static UdpClient ReceivingUdpClient;
        public static IPEndPoint ReceiveIpEndPoint;

        // WebSocket Server
        public static WebSocket WsoWheelClient;
        public static WebSocket WsoDialogClient;

        // for world Messages receive

        public static UdpClient ReceivingUdpClientMessages;
        public static IPEndPoint ReceiveIpEndPointMessages;
        public static AsyncCallback ReturnCallMessages;

        // for Tray messages receive

        public static Socket TraySocket;
        public static UdpClient ReceivingTrayMessages;
        public static IPEndPoint ReceiveIpTrayMessages;
        public static AsyncCallback TrayMessages;
        public static UdpClient SendingTrayMessages;
        public static IPEndPoint SendingIpTrayMessages;

        // for sending/receive with SRS

        public static Socket SRS_SendSocket;
        public static UdpClient SRS_SendingUdpClient;
        public static IPEndPoint SRS_SendIpEndPoint;
        public static AsyncCallback SRS_ReturnCall;
        public static UdpClient SRS_ReceivingUdpClient;
        public static IPEndPoint SRS_ReceiveIpEndPoint;

        // for sending to kneeboard

        public static Socket Kneeboard_SendSocket;
        public static IPEndPoint Kneeboard_SendIpEndPoint;
        public static KneeboardState KneeboardState;
        public static Dictionary<string, SortedDictionary<string, List<string>>>[] KneeboardCatAliasStrings;

        // ------------------------------------------------------------------------------------------------------------
        // call handling control flags

        public static string currentfullsentence;
        public static bool blockallcommands;
        public static bool showingoptions;
        public static bool processlocked;
        public static bool startup;
        public static bool initialized;
        public static bool valistening;
        public static bool transmitting;
        public static bool oneradioactive;
        public static bool vaicompttused;
        public static bool haveinputscomplete;
        public static bool briefinginprogress;
        public static bool genericstaterequest;
        public static bool calledisclass;
        public static bool dospeech;
        public static Stopwatch Stopwatch;

        // ------------------------------------------------------------------------------------------------------------
        // config / config window

        public static bool allowairioswitching = true;

        public static bool trainerrunning;
        public static Choices trainerchoices;
        public static UI.ConfigWindow configurationwindow;
        public static bool configwindowopen;
        public static Thread configwindowthread;
        public static Thread listenthread;
        public static Settings.Config activeconfig;

        public static string editorcurrentalias;
        public static string editorcurrentkeyword;
        public static string editorcurrenttext;
        public static string editorcurrentsourcetable;
        public static int editorcurrentindex;
        public static int editorselectedalias;
        public static List<string> editorcurrentaliases;

        // ------------------------------------------------------------------------------------------------------------
        // chatter playback parameters

        public static List<string> chatterthemes;
        public static ResourceSet chatterresources;
        public static bool chatterinitalized;
        public static List<string> chattersoundfiles;
        public static int chatterintervalmin;
        public static int chatterintervalmax;
        public static bool chatteractive;

        // ------------------------------------------------------------------------------------------------------------
        // text-to-speech

        public static bool dcsinstalled;
        public static string dcspath_release = "";
        public static string dcspath_openbeta = "";
        public static string dcspath_steam = "";
        public static SpeechSynthesizer synth;
        public static WaveOut ttsoutput;
        public static WaveOutEvent chatteroutput;
        public static MixingSampleProvider ttsmixer;
        public static string aocscallsign = "Crystal Palace"; // default

        // -----------------------------------------------------------------------------------------------------------
        // beacon /message / audio parameters

        public static bool beaconinitalized;
        public static int beaconinterval;
        public static bool beaconactive;
        public static bool beaconlocked;
        public static int lastupdaterequesttimer;
        public static int lasthearbeatsent = 0;
        public static int lastupdatewingmantimer;
        public static int elapsedsincelastpttrelease;
        public static bool resetvaicomlisteningexecute;
        public static int wingnextrandomtimeout;
        public static List<string> wingmansoundfiles;
        public static ResourceSet wingmanresources;
        public static ResourceManager wingmanmanager;
        public static double currentmessageduration;
        public static bool currentmessagescripted;
        public static bool currentaudiodevicevalid;
        public static int lastreceivedmessagetimer;

        public static int kneeboardmessagetimer = 0;
        public static bool havekneeboardupdate = false;
        public static Server.ServerCommsMessage receivedmessageforkneeboard = new Server.ServerCommsMessage();
        public static string wingmanspeechpath;

        public static Extensions.WorldAudio.Processor.wingmanstate currentwingmanstate;
        public static Dictionary<Extensions.WorldAudio.Processor.commcat, Server.ServerCommsMessage> lastmessage;
        public static PushToTalk.PTT.TXNode receivedtx;
        public static List<Processor.AudioDeviceObject> audiodeviceobjects = new List<Processor.AudioDeviceObject>();

        // -----------------------------------------------------------------------------------------------------------
        // randomizers

        public static Random random1;
        public static Random random2;

        // -----------------------------------------------------------------------------------------------------------
        // UI timer parameters

        public static bool uitimerinitalized;
        public static int uitimerinterval;
        public static bool uitimeractive;

        // -----------------------------------------------------------------------------------------------------------
        // preferences & settings

        public static bool doretune;
        public static bool instantselectmode;
        public static bool apxwpnallowed = false;
        public static bool apxdirallowed = false;

        // -----------------------------------------------------------------------------------------------------------
        // keyword database

        public static List<DCSmodule> importeddcsmodules;
        public static int aliascount;
        public static int labelcount;
        public static bool menuauximported;
        public static string menuauxname;
        public static string menuauxserver;

        // -----------------------------------------------------------------------------------------------------------
        // debugging

        public static string logfile;

        // -----------------------------------------------------------------------------------------------------------
        // server state handling

        public static bool dcsrunning;
        public static bool AIRIOactive;

        public static bool IsCrewHotMicModuleActive()
        {
            if (AIRIOactive)
            {
                return true;
            }

            return currentmodule != null
                && (currentmodule.Id.Equals("F-4E-45MC", StringComparison.OrdinalIgnoreCase)
                    || currentmodule.Id.Equals("AH-64D", StringComparison.OrdinalIgnoreCase));
        }

        public static bool IsF4EIntercomSelected()
        {
            if (currentstate == null)
            {
                return false;
            }

            if (currentstate.riostate != null)
            {
                return currentstate.riostate.ics;
            }

            return false;
        }

        public static bool IsCrewHotMicActive()
        {
            if (activeconfig == null || !IsCrewHotMicModuleActive())
            {
                return false;
            }

            if (currentmodule != null
                && currentmodule.Id.Equals("F-4E-45MC", StringComparison.OrdinalIgnoreCase)
                && activeconfig.ICShotmic_useswitch)
            {
                return IsF4EIntercomSelected();
            }

            if (currentmodule != null
                && currentmodule.Id.Equals("AH-64D", StringComparison.OrdinalIgnoreCase)
                && activeconfig.ICShotmic_useswitch)
            {
                return activeconfig.ICShotmic;
            }

            return activeconfig.ICShotmic;
        }

        public static bool IsCrewHotMicActiveOnIntercomTX()
        {
            return IsCrewHotMicActive()
                && currentTXnode != null
                && currentTXnode.Equals(PushToTalk.PTT.TXNodes.TX5);
        }

        public static bool IntercomHotMicLatched;

        public static Server.ServerState previousstate;
        public static Server.ServerState currentstate;
        public static int radiocount;
        public static string messagelog;
        public static string lastmessagelog;
        public static string nineline;
        public static string kneeboardcurrentbuffer = "";
        public static string kneeboardlastdictbuffer = "";
        public static bool moduleDetected;
        public static bool moduleConnected = false; // Tracks if the module is connected

        public enum AH64GeorgeWeaponMode
        {
            Unknown,
            NoWeapon,
            Gun,
            Missiles,
            Rockets
        }

        public static bool AH64GeorgeGunAvailable;
        public static bool AH64GeorgeRocketsAvailable;
        public static bool AH64GeorgeMissilesAvailable;
        public static bool AH64GeorgeWeaponStateValid;
        public static bool AH64GeorgeWowFromExport;
        public static bool AH64GeorgeWowFromServerState;
        public static AH64GeorgeWeaponMode AH64GeorgeSelectedWeapon = AH64GeorgeWeaponMode.Unknown;
        // -----------------------------------------------------------------------------------------------------------
        // call flow management tables

        public static Dictionary<string, string> currentkey = new Dictionary<string, string>()
        {
            {"recipient",""         },
            {"importedatcs",""      },
            {"importedmenus",""     },
            {"sender",""            },
            {"cue",""               },
            {"command",""           },
            {"apxwpn",""            },
            {"apxdir",""            },
            {"wsocmdrecipient",""   },
        };

        public static Dictionary<string, string> usedalias = new Dictionary<string, string>()
        {
            {"recipient",""         },
            {"importedatcs",""      },
            {"importedmenus",""     },
            {"sender",""            },
            {"cue",""               },
            {"command",""           },
            {"apxwpn",""            },
            {"apxdir",""            },
            {"wsocmdrecipient",""   },
        };

        public static Dictionary<string, bool> have = new Dictionary<string, bool>()
        {
            {"recipient",       false       },
            {"importedatcs",    false       },
            {"importedmenus",   false       },
            {"sender",          false       },
            {"cue",             false       },
            {"command",         false       },
            {"apxwpn",          false       },
            {"apxdir",          false       },
            {"wsocmdrecipient", false       },
        };

        public static void MessageReset()
        {

            have["recipient"] = false;
            have["importedatcs"] = false;
            have["importedmenus"] = false;
            have["sender"] = false;
            have["cue"] = false;
            have["command"] = false;
            have["apxwpn"] = false;
            have["apxdir"] = false;
            have["wsocmdrecipient"] = false;
            haveinputscomplete = false;

            currentkey["recipient"] = "";
            currentkey["importedatcs"] = "";
            currentkey["importedmenus"] = "";
            currentkey["sender"] = "";
            currentkey["cue"] = "";
            currentkey["command"] = "";
            currentkey["apxwpn"] = "";
            currentkey["apxdir"] = "";
            currentkey["wsocmdrecipient"] = "";

            currentmessage = new DcsClient.Message.CommsMessage();
            currentrecipient = new Recipient();
            currentcommand = new Command();
            currentrecipientclass = Recipientclasses.Undefined;
            currentmessageunit = new Server.DcsUnit();

            WSOState["currentCommand"] = null;
            WSOState["currentRecipient"] = null;
            WSOState["currentCategory"] = null;
            currentWSOCommandRecipient = null;
        }

        // -----------------------------------------------------------------------------------------------------------
        // new sendmessage construction

        public static DcsClient.Message.CommsMessage currentmessage;
        public static PushToTalk.PTT.TXNode currentTXnode;
        public static PushToTalk.PTT.TXNode activenode;
        public static Recipient currentrecipient;
        public static Recipientclass currentrecipientclass;
        public static Recipientclass previousrecipientclass;
        public static Command currentcommand;
        public static Server.DcsUnit currentmessageunit;
        public static Server.DcsUnit previousmessageunit;
        public static int currentflightrecipientnumber;
        public static DCSmodule currentmodule;
        public static string currentradiodevicename;

        // -----------------------------------------------------------------------------------------------------------
        // select units

        public static Dictionary<string, Server.DcsUnit> SelectedUnit = new Dictionary<string, Server.DcsUnit>()
        {

            { "Player"  , new Server.DcsUnit() },
            { "Flight"  , new Server.DcsUnit() },
            { "JTAC"    , new Server.DcsUnit() },
            { "ATC"     , new Server.DcsUnit() },
            { "AWACS"   , new Server.DcsUnit() },
            { "Tanker"  , new Server.DcsUnit() },
            { "Crew"    , new Server.DcsUnit() },
            { "Aux"     , new Server.DcsUnit() },
            { "Cargo"   , new Server.DcsUnit() },
            { "Allies"  , new Server.DcsUnit() },

        };

        public static void ResetSelectedUnits()
        {
            try
            {

                Log.Write("Resetting selected units.", Colors.Text);

                SelectedUnit["Player"] = new Server.DcsUnit();
                SelectedUnit["Flight"] = new Server.DcsUnit();
                SelectedUnit["JTAC"] = new Server.DcsUnit();
                SelectedUnit["ATC"] = new Server.DcsUnit();
                SelectedUnit["AWACS"] = new Server.DcsUnit();
                SelectedUnit["Tanker"] = new Server.DcsUnit();
                SelectedUnit["Crew"] = new Server.DcsUnit();
                SelectedUnit["Aux"] = new Server.DcsUnit();
                SelectedUnit["Cargo"] = new Server.DcsUnit();
                SelectedUnit["Allies"] = new Server.DcsUnit();

                // populate with nearest units

                foreach (KeyValuePair<string, List<Server.DcsUnit>> category in State.currentstate.availablerecipients)
                {
                    if (category.Key.Equals("Flight"))
                    {
                        if (category.Value.Count > 1)
                        { SelectedUnit[category.Key] = category.Value[1]; }
                        else
                        {
                            if (category.Value.Count > 0)
                            {
                                SelectedUnit[category.Key] = category.Value[0];
                            }
                        }
                    }
                    else
                    {
                        if (category.Value.Count > 0)
                        { SelectedUnit[category.Key] = category.Value[0]; }
                    }
                }
            }
            catch
            {
            }
        }

        // WSO extension state tracking
        // Add a flag to track if WSO is active
        public static bool WSOActive { get; set; } = false;

        // Add a flag to track if WSO commands are enabled
        public static bool WSOCommandsEnabled { get; set; } = true;

        // Add a dictionary to store WSO-related state
        public static Dictionary<string, object> WSOState = new Dictionary<string, object>();

        // Recipient for WSO commands, e.g. diverting to airfields.
        public static Recipient currentWSOCommandRecipient = null;

        // Add a method to initialize WSO state
        public static void InitializeWSOState()
        {
            WSOActive = true;
            WSOCommandsEnabled = true;

            currentWSOCommandRecipient = null;

            // Initialize WSOState with default values
            WSOState["currentCommand"] = null;
            WSOState["currentRecipient"] = null;
            WSOState["currentCategory"] = null;
        }

        public static bool wsoactivated = true; // Set to true if WSO extension is to be enabled

        public static bool IsF4E
        {
            get
            {
                return currentmodule != null && currentmodule.Id.Equals("F-4E-45MC", StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}
