using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Speech.Synthesis;
using VAICOM.Database;
using VAICOM.Extensions.Kneeboard;
using VAICOM.Extensions.WorldAudio;
using VAICOM.FileManager;
using VAICOM.Products;
using VAICOM.PushToTalk;
using VAICOM.Servers;
using VAICOM.Static;

namespace VAICOM
{

    namespace Client
    {

        public partial class DcsClient
        {

            public static void GetAssemblies(dynamic vaProxy)
            {

                try
                {
                    Framework.Assemblies.GetAssembliesInfo_Plugin();
                }
                catch
                {
                    State.dll_info_plugin = new AssemblyName();
                    State.dll_info_plugin.Version = Version.Parse("0.0.0.0");
                    State.dll_version_plugin = "(not installed)";
                    State.dll_installed_plugin = false;
                    //Log.Write("Failed to get plugin info.", Colors.Warning);
                }

                try
                {
                    Framework.Assemblies.GetAssembliesInfo_Chatter();
                }
                catch
                {
                    State.dll_info_chatter = new AssemblyName();
                    State.dll_info_chatter.Version = Version.Parse("0.0.0.0");
                    State.dll_version_chatter = "(not installed)";
                    State.dll_installed_chatter = false;
                    //Log.Write("Failed to get chatter info.", Colors.Warning);
                }

                try
                {
                    Framework.Assemblies.GetAssembliesInfo_Rio();
                }
                catch
                {
                    State.dll_info_rio = new AssemblyName();
                    State.dll_info_rio.Version = Version.Parse("0.0.0.0");
                    State.dll_version_rio = "(not installed)";
                    State.dll_installed_rio = false;
                    //Log.Write("Failed to get RIO info.", Colors.Warning);
                }

                try
                {
                    string faultfile = "VoiceAttackFault.txt";
                    string cleanpath = vaProxy.SessionState["VA_DIR"] + "\\" + faultfile;
                    if (File.Exists(cleanpath))
                    {
                        File.Delete(cleanpath);
                    }
                }
                catch
                {
                }

            }

            public static void CheckVAVersion(dynamic vaProxy)
            {
                try
                {

                    System.Version VAminversion = Version.Parse(State.vaminversion); // set minimum tested version of voice attack in state
                    System.Version VAcurrentversion = vaProxy.VAVersion;

                    if (VAcurrentversion < VAminversion)
                    {
                        Log.Write("ALERT: VoiceAttack is not up to date! Update to version " + State.vaminversion + " or higher to use with VAICOM Community Edition.", Colors.Critical);
                        UI.Playsound.Error();
                    }
                    else
                    {
                        Log.Write("VoiceAttack current version " + VAcurrentversion.ToString() + " up-to-date for use with VAICOM Community Edition.", Colors.Text);
                    }

                }
                catch (Exception e)
                {
                    Log.Write("VA version error " + e.Message, Colors.Critical);
                }

            }

            public static void ResetProcessValues(dynamic vaProxy)
            {
                State.dcsrunning = false;
                State.tempblockedcommands = false;
                State.blockedmodule = false;
                State.blockallcommands = false;
            }

            public static void ResetStateValues(dynamic vaProxy)
            {
                State.random1 = new Random();
                State.random2 = new Random();
                State.wingnextrandomtimeout = 30;
                State.KneeboardState = new KneeboardState();
                State.KneeboardCatAliasStrings = new Dictionary<string, SortedDictionary<string, List<string>>>[4]; //"AWACS", "Request", {"Vector to Base","vect to Tanker"} 
                State.KneeboardCatAliasStrings[0] = new Dictionary<string, SortedDictionary<string, List<string>>>();
                State.KneeboardCatAliasStrings[1] = new Dictionary<string, SortedDictionary<string, List<string>>>();
                State.KneeboardCatAliasStrings[2] = new Dictionary<string, SortedDictionary<string, List<string>>>();
                State.KneeboardCatAliasStrings[3] = new Dictionary<string, SortedDictionary<string, List<string>>>();

                State.wingmanspeechpath = "";
                State.previousstate = new Server.ServerState();
                State.currentstate = new Server.ServerState();
                State.menuauximported = false;
                State.currentstate.easycomms = true;
                State.currentmodule = DCSmodules.LookupTable["----"];
                State.currentrecipientclass = Recipientclasses.Undefined;
                State.oneradioactive = true;
                State.currentradiodevicename = "";
                State.lastupdaterequesttimer = 0;
                State.briefinginprogress = false;
                State.chatteroutput = new NAudio.Wave.WaveOutEvent();
                State.currentTXnode = PTT.TXNodes.TX5;//new PTT.TXNode();
                State.currentwingmanstate = Extensions.WorldAudio.Processor.wingmanstate.NEUTRAL;
                State.lastmessage = new Dictionary<Extensions.WorldAudio.Processor.commcat, Server.ServerCommsMessage>()
                    {
                        { Processor.commcat.ALL,            new Server.ServerCommsMessage()},
                        { Processor.commcat.UNKNOWN,        new Server.ServerCommsMessage()},
                        { Processor.commcat.PLAYER,         new Server.ServerCommsMessage()},
                        { Processor.commcat.WINGMAN,        new Server.ServerCommsMessage()},
                        { Processor.commcat.ATC,            new Server.ServerCommsMessage()},
                        { Processor.commcat.AWACS,          new Server.ServerCommsMessage()},
                        { Processor.commcat.TANKER,         new Server.ServerCommsMessage()},
                        { Processor.commcat.JTAC,           new Server.ServerCommsMessage()},
                        { Processor.commcat.CCC,            new Server.ServerCommsMessage()},
                        { Processor.commcat.ALLIED_FLIGHT,  new Server.ServerCommsMessage()},
                        { Processor.commcat.ExternalCargo,  new Server.ServerCommsMessage()},
                        { Processor.commcat.GROUND_CREW,    new Server.ServerCommsMessage()},
                    };
                State.Stopwatch = new System.Diagnostics.Stopwatch();
                Server.homebaselocation = new Server.Vector();

            }

            public static void ResetConfig(dynamic vaProxy)
            {
                State.activeconfig = Settings.Configs.Defaultconfig;
                Settings.ConfigFile.WriteConfigToFile(false);
                State.activeconfig = Settings.ConfigFile.ReadConfigFromFile();
            }

            public static void FixFiles(dynamic vaProxy)
            {
                FileHandler.Updater.CleanUpdateFolder();
                Settings.ConfigFile.AddNewConfigItems();
                Settings.ConfigFile.WriteConfigToFile(true);
            }
            public static void CheckUpdates(dynamic vaProxy)
            {
                if (State.activeconfig.AutomaticUpdateFinished)
                {
                    Log.Write("Plugin files were updated.", Colors.System);
                    State.activeconfig.AutomaticUpdateFinished = false;
                }
                else
                {
                    // ---- only check for new updates every <checkmax> sessions (1 will check each session)

                    Random rnd = new Random();
                    int checkmax = 1;
                    int dice = rnd.Next(1, 1 + checkmax);
                    if (dice.Equals(1))
                    {
                        FileHandler.Updater.GetPluginUpdates();
                    }
                    else
                    {
                        Log.Write("Update check skipped: " + dice, Colors.Inline);
                    }
                }

            }
            public static void LogVersionData(dynamic vaProxy)
            {
                string betastring = "";
                if (State.versionbeta)
                {
                    betastring = "(Beta)";
                }
                else
                if (State.versiondev)
                {
                    betastring = "(Dev)";
                }
                else
                {
                    betastring = "";
                }

                State.versionstring = State.dll_version_plugin + " " + betastring;

                Log.Write("Plugin version " + State.versionstring, Colors.Text);

            }

            public static void ResetPTTConfig(dynamic vaProxy)
            {
                PTT.PTT_TXAssignmentDefault();
                PTT.PTT_ApplyNewConfig();
            }

            public static void InstallLuaFiles(dynamic vaProxy)
            {

                if (!State.activeconfig.ManualInstallServerFiles)
                {
                    Log.Write("Executing automatic lua code installation.", Colors.Text);
                    FileHandler.Lua.LuaFiles_Install(false, false); // rebuilds all lua files for all DCS installs

                    try
                    {
                        // KNEEBOARD ADDITION
                        if (State.kneeboardactivated && State.activeconfig.Kneeboard_Enabled && State.installkneeboard)
                        {
                            FileHandler.Lua.LuaFiles_Install_Kneeboard(false, State.clientmode.Equals(ClientModes.Normal)); // quiet if not debug
                        }
                        else // clear kneeboard entries (restore mode)
                        {
                            FileHandler.Lua.LuaFiles_Install_Kneeboard(true, State.clientmode.Equals(ClientModes.Normal)); // quiet if not debug
                        }
                    }
                    catch (Exception e)
                    {
                        Log.Write("Problems were reported during kneeboard initialization" + e.Message, Colors.Text);
                    }
                }
                else
                {
                    Log.Write("Manual installation selected: skipping automatic files configuration", Colors.Text);
                }
            }

            public static bool MergeRIO(dynamic vaProxy)
            {
                bool RIOmerged = false;

                try
                {

                    if (State.jesteractivated)
                    {
                        int updates = Extensions.RIO.ExtImport.MergeRIO();
                        if (updates > 0)
                        {
                            RIOmerged = true;
                        }
                    }

                }
                catch
                {
                    Log.Write("WARNING: Could not load the AIRIO plugin extension.", Colors.Warning);
                    RIOmerged = false;
                }

                return RIOmerged;

            }

            public static void CreateDatabase(dynamic vaProxy)
            {

                Log.Write("Initializing database tables...", Colors.Text);

                Dcs.CreateDcsMessagesTable();
                Dcs.InitAutoGeneratedEventIDs();
                Dcs.CreateDcsInputCommandsTable();

                Log.Write("Done.", Colors.Text);

                // gets the default database
                Database.Aliases.ResetDatabase();
                Database.Labels.ResetDatabase();

                // .. writes it to files but only if not yet existing
                FileHandler.Database.WriteAllCategoriesToFile(false);
                FileHandler.Database.WriteAuxMenuItemsToFile(false);

                // now get the up-to-date database from disk
                FileHandler.Database.ReadAllCategoriesFromFile(); // also check if there are new keywords and adds them.
                FileHandler.Database.ReadAuxMenuItemsFromFile();

                // force any additional aliases;
                FileHandler.Database.ForceAddAliases();

                // add imported stuff from disk
                Server.SetImportedATCsAsRecipients();
                Server.SetImportedMenusAsCommands();

                // create the master tables
                Database.Aliases.BuildNewMasterTable();
                Database.Labels.BuildNewMasterTable();

                // add AOCS
                Database.Aliases.SetAOCSCallsign();

                // add imported modules..
                State.importeddcsmodules = new List<DCSmodule>();
                FileHandler.Database.ReadModulesFromFile();

            }

            public static void StartNetwork(dynamic vaProxy)
            {
                try
                {
                    Interfaces.Network.UDPsetup();
                    Interfaces.Network.UDPstart();
                }
                catch (Exception e)
                {
                    Log.Write(e.StackTrace, Colors.Text);
                }
            }

            public static void StartTimers(dynamic vaProxy)
            {
                try
                {
                    Interfaces.Beacon.Beacon_Initialize();
                    Interfaces.Beacon.Beacon_PlayToggle();
                }
                catch (Exception ex)
                {
                    Log.Write("Beacon not started. " + ex.Message, Colors.Inline);
                }

                try
                {
                    UI.Timers.UI_Timer_Initialize();
                    UI.Timers.UI_Timer_Playtoggle();
                }
                catch (Exception ex)
                {
                    Log.Write("UI timer not started. " + ex.Message, Colors.Inline);
                }

                try
                {
                    Extensions.SRSclient.SRS_Timer_Initialize();
                }
                catch (Exception ex)
                {
                    Log.Write("SRS Timer not initialized. " + ex.Message, Colors.Inline);
                }

                try
                {
                    Extensions.Chatter.AudioTimer.Chatter_Initialize();
                    if (State.activeconfig.ChatterAutostart)
                    { Extensions.Chatter.AudioTimer.Chatter_TimerPlayToggle(); }
                }
                catch (Exception ex)
                {
                    Log.Write("Chatter not started. " + ex.Message, Colors.Inline);
                }
            }

            public static void StartSpeechSynth(dynamic vaProxy)
            {

                try
                {
                    State.synth = new SpeechSynthesizer();
                }
                catch (Exception ex)
                {
                    Log.Write("TTS not started. " + ex.Message, Colors.Inline);
                }

            }

            public static void InitListeningState(dynamic vaProxy)
            {
                vaProxy.Command.SetSessionEnabledByCategory("Keyword Collections", !State.activeconfig.ReleaseHot);
                vaProxy.Command.SetSessionEnabledByCategory("Extension packs", !State.activeconfig.ReleaseHot);

                if (State.activeconfig.ReleaseHot && State.Proxy.GetProfileName().ToLower().Contains(State.defProfileName.ToLower()))
                {
                    vaProxy.Command.SetSessionEnabled("Chatter", true);
                }

                vaProxy.State.SetListeningEnabled(State.activeconfig.ReleaseHot);
            }

            public static void Initialize(dynamic vaProxy)
            {

                State.startup = true;

                try
                {

                    State.SetEnvironment(vaProxy);
                    ResetProcessValues(vaProxy);

                    Products.Products.CheckActiveLicenses();

                    FileHandler.Root.CheckSubFolders();
                    FileHandler.Root.ExtractCompagnionApp();
                    FileHandler.Root.ExtractNoLoadContext();

                    Log.Write("VAICOM Community Edition for DCS World.", Colors.System);
                    Log.Write("Press LCtrl+LAlt+C to open Vaicom UI", Colors.System);
                    Log.Write("Initializing..", Colors.System);

                    ResetConfig(vaProxy);

                    Log.Reset();

                    CheckVAVersion(vaProxy);
                    GetAssemblies(vaProxy);

                }
                catch
                {
                    Log.Write("Problems were reported during plugin startup.", Colors.Message);
                }

                State.startup = false;

                try
                {

                    FixFiles(vaProxy);
                    CheckUpdates(vaProxy);
                    LogVersionData(vaProxy);
                    ResetStateValues(vaProxy);
                    Processor.InitTTSPlaybackStream();
                    ResetPTTConfig(vaProxy);
                    InstallLuaFiles(vaProxy);
                    FileHandler.Root.CheckProfile(false);
                    MergeRIO(vaProxy);
                    CreateDatabase(vaProxy);
                    StartNetwork(vaProxy);
                    StartTimers(vaProxy);
                    StartSpeechSynth(vaProxy);
                    InitListeningState(vaProxy);
                    UI.Playsound.Startup();
                    SendUpdateRequest();

                    Log.Write("Startup finished.", Colors.Text);
                    Log.Write("Ready for commands.", Colors.Message);

                    if (State.clientmode.Equals(ClientModes.Debug))
                    {
                        State.realatcactivated = true;
                    }

                }
                catch (Exception e)
                {
                    Log.Write("Problems were reported during initialization." + e.StackTrace, Colors.Warning);
                }

            }

        }
    }
}
