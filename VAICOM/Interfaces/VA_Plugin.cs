using System;
using System.Threading;
using VAICOM.Client;
using VAICOM.Extensions.AIWSO;
using VAICOM.Extensions.WorldAudio;
using VAICOM.FileManager;
using VAICOM.PushToTalk;
using VAICOM.Static;

namespace VAICOM
{
    namespace Interfaces
    {

        //25

        public class VA_Plugin
        {

            public static string VA_DisplayName()
            {
                return "VAICOM Community Edition for DCS World";
            }

            public static string VA_DisplayInfo()
            {
                return "VAICOM Community Edition 3.0 for DCS World";
            }

            public static Guid VA_Id()
            {
                return new Guid("{5B433065-DEC8-4852-8912-2FF6EDF9807F}");
            }

            public static void VA_StopCommand()
            {
            }

            public static void VA_Init1(dynamic vaProxy)
            {
                State.Proxy = vaProxy;
                AppDomain.CurrentDomain.AssemblyResolve += Framework.Assemblies.AssemblyResolve;
                State.initialized = false;
                DcsClient.Initialize(vaProxy);
                State.initialized = true;
                VA_ExposeVariables(State.Proxy);
            }

            public static void VA_Invoke1(dynamic vaProxy)
            {
                State.Proxy = vaProxy;

                if (State.activeconfig.Runningforthefirsttime || (State.initialized == false))
                {
                    return;
                }

                string contextinput = Helpers.Common.StringNormalize(vaProxy.Context);
                bool longpress = State.Proxy.Utility.ParseTokens("{CMDLONGPRESSINVOKED}") == "1";

                switch (contextinput)
                {

                    // Push-To-Talk handlers

                    case "ptt.hotkey.tx1.press":
                        PTT.PTT_Handler(vaProxy, PTT.TXNodes.TX1, true, longpress);
                        break;

                    case "ptt.hotkey.tx1.release":
                        PTT.PTT_Handler(vaProxy, PTT.TXNodes.TX1, false, longpress);
                        break;

                    case "ptt.hotkey.tx2.press":
                        PTT.PTT_Handler(vaProxy, PTT.TXNodes.TX2, true, longpress);
                        break;

                    case "ptt.hotkey.tx2.release":
                        PTT.PTT_Handler(vaProxy, PTT.TXNodes.TX2, false, longpress);
                        break;

                    case "ptt.hotkey.tx3.press":
                        PTT.PTT_Handler(vaProxy, PTT.TXNodes.TX3, true, longpress);
                        break;

                    case "ptt.hotkey.tx3.release":
                        PTT.PTT_Handler(vaProxy, PTT.TXNodes.TX3, false, longpress);
                        break;

                    case "ptt.hotkey.tx4.press":
                        PTT.PTT_Handler(vaProxy, PTT.TXNodes.TX4, true, longpress);
                        break;

                    case "ptt.hotkey.tx4.release":
                        PTT.PTT_Handler(vaProxy, PTT.TXNodes.TX4, false, longpress);
                        break;

                    case "ptt.hotkey.tx5.press":
                        PTT.PTT_Handler(vaProxy, PTT.TXNodes.TX5, true, longpress);
                        break;

                    case "ptt.hotkey.tx5.release":
                        PTT.PTT_Handler(vaProxy, PTT.TXNodes.TX5, false, longpress);
                        break;

                    case "ptt.hotkey.tx6.press":
                        PTT.PTT_Handler(vaProxy, PTT.TXNodes.TX6, true, longpress);
                        break;

                    case "ptt.hotkey.tx6.release":
                        PTT.PTT_Handler(vaProxy, PTT.TXNodes.TX6, false, longpress);
                        break;

                    // comms keyword handler

                    case "alias.aicomms":
                        try
                        {
                            bool _complete = DcsClient.Message.processcommand();
                            if (PTT.TXLinkApply && _complete)
                            {
                                Thread.Sleep(200);
                                PTT.PTT_Handler(State.Proxy, State.currentTXnode, false, false);
                            }
                        }
                        catch (Exception)
                        {
                            State.processlocked = false;
                            vaProxy.WriteToLog("There was an error processing this voice command.", Colors.Text);
                        }
                        break;

                    // Config window 

                    case "config":
                        UI.Initialize.OpenConfiguration(vaProxy, false);
                        break;

                    case "config.resetwindow":
                        UI.Initialize.OpenConfiguration(vaProxy, true);
                        break;

                    // Chatter on/off

                    case "chatter":
                        if (State.activeconfig.Chatter_Enabled)
                        {
                            UI.Playsound.Commandcomplete();
                            Extensions.Chatter.AudioTimer.Chatter_TimerPlayToggle();
                        }
                        else
                        {
                            UI.Playsound.Sorry();
                            vaProxy.WriteToLog("Please enable Chatter in the Vaicom UI extension tab to use Chatter functions.", Colors.Warning);
                        }
                        break;

                    // AIRIO profile cmds

                    case "airio.showwheel":
                        if (State.dll_installed_rio)
                        {
                            Extensions.RIO.helper.ShowWheel(true);
                        }
                        else
                        {
                            UI.Playsound.Sorry();
                            vaProxy.WriteToLog("This command requires AIRIO extension.", Colors.Warning);
                        }
                        break;

                    case "airio.dev.radio.tune":
                        if (State.dll_installed_rio)
                        {
                            Client.DcsClient.Message.SetRioDeviceSequence_Radio_Tuning();
                        }
                        else
                        {
                            UI.Playsound.Sorry();
                            vaProxy.WriteToLog("This command requires AIRIO extension.", Colors.Warning);
                        }
                        break;

                    case "airio.dev.tacan.tune":
                        if (State.dll_installed_rio)
                        {
                            Client.DcsClient.Message.SetRioDeviceSequence_TACAN_Tuning();
                        }
                        else
                        {
                            UI.Playsound.Sorry();
                            vaProxy.WriteToLog("This command requires AIRIO extension.", Colors.Warning);
                        }
                        break;

                    case "airio.dev.dl.tune":
                        if (State.dll_installed_rio)
                        {
                            Client.DcsClient.Message.SetRioDeviceSequence_Datalink_Tuning();
                        }
                        else
                        {
                            UI.Playsound.Sorry();
                            vaProxy.WriteToLog("This command requires AIRIO extension.", Colors.Warning);
                        }
                        break;

                    case "airio.dev.radar.sector":
                        if (State.dll_installed_rio)
                        {
                            Client.DcsClient.Message.SetRioDeviceSequence_Radar_Sector();
                        }
                        else
                        {
                            UI.Playsound.Sorry();
                            vaProxy.WriteToLog("This command requires AIRIO extension.", Colors.Warning);
                        }
                        break;

                    case "airio.map.markers":
                        if (State.dll_installed_rio)
                        {
                            Client.DcsClient.Message.SetRioDeviceSequence_Map_Steerpoints();
                        }
                        else
                        {
                            UI.Playsound.Sorry();
                            vaProxy.WriteToLog("This command requires AIRIO extension.", Colors.Warning);
                        }
                        break;

                    case "airio.map.markers.track":
                        if (State.dll_installed_rio)
                        {
                            Client.DcsClient.Message.SetRioDeviceSequence_TrackMapMarker();
                        }
                        else
                        {
                            UI.Playsound.Sorry();
                            vaProxy.WriteToLog("This command requires AIRIO extension.", Colors.Warning);
                        }
                        break;

                    case "airio.map.markers.navigate":
                        if (State.dll_installed_rio)
                        {
                            Client.DcsClient.Message.SetRioDeviceSequence_FlyMapMarker();
                        }
                        else
                        {
                            UI.Playsound.Sorry();
                            vaProxy.WriteToLog("This command requires AIRIO extension.", Colors.Warning);
                        }
                        break;

                    case "airio.map.navgrid":
                        if (State.dll_installed_rio)
                        {
                            Client.DcsClient.Message.SetRioDeviceSequence_GridMapMarker();
                        }
                        else
                        {
                            UI.Playsound.Sorry();
                            vaProxy.WriteToLog("This command requires AIRIO extension.", Colors.Warning);
                        }
                        break;

                    case "airio.dev.laser.code":
                        if (State.dll_installed_rio)
                        {
                            Client.DcsClient.Message.SetRioDeviceSequence_LaserCode();
                        }
                        else
                        {
                            UI.Playsound.Sorry();
                            vaProxy.WriteToLog("This command requires AIRIO extension.", Colors.Warning);
                        }
                        break;

                    // Manual tune radios
                    case "dev.radio.setchn":
                        Client.DcsClient.Message.RadioControl_TuneChan();
                        break;

                    case "dev.radio.setfrq":
                        Client.DcsClient.Message.RadioControl_TuneFreq();
                        break;

                    // API calls PTT

                    case "ptt.mode.release":
                        API.PTT_Release_Hot(vaProxy);
                        break;

                    case "ptt.mode.page.up":
                        API.PTT_Mode_Page_Up(vaProxy);
                        break;

                    case "ptt.mode.page.dn":
                        API.PTT_Mode_Page_Dn(vaProxy);
                        break;

                    case "chatter.vol.up":
                        API.Chatter_Vol_Up(vaProxy);
                        break;

                    case "chatter.vol.dn":
                        API.Chatter_Vol_Dn(vaProxy);
                        break;

                    case "ptt.mode.dial":
                        API.Operate_Dial(vaProxy);
                        break;

                    case "ptt.mode.map.srs":
                        API.SRS_Mapping(vaProxy);
                        break;

                    case "ptt.mode.prv":
                        API.PTT_Mode_Prv(vaProxy);
                        break;

                    case "ptt.mode.nxt":
                        API.PTT_Mode_Nxt(vaProxy);
                        break;

                    case "ptt.mode.inv":
                        API.PTT_Mode_Inv(vaProxy);
                        break;

                    case "ptt.mode.norm":
                        API.PTT_Mode_Norm(vaProxy);
                        break;

                    case "ptt.mode.multi":
                        API.PTT_Mode_Multi(vaProxy);
                        break;

                    case "ptt.mode.sngl":
                        API.PTT_Mode_Single(vaProxy);
                        break;

                    // --------------------------------------------------------------------------
                    // kneeboard control

                    case "kneeboard.tab.log":
                        API.ControlKneeboard(vaProxy, contextinput);
                        break;

                    case "kneeboard.tab.awacs":
                        API.ControlKneeboard(vaProxy, contextinput);
                        break;

                    case "kneeboard.tab.jtac":
                        API.ControlKneeboard(vaProxy, contextinput);
                        break;

                    case "kneeboard.tab.atc":
                        API.ControlKneeboard(vaProxy, contextinput);
                        break;

                    case "kneeboard.tab.tanker":
                        API.ControlKneeboard(vaProxy, contextinput);
                        break;

                    case "kneeboard.tab.aocs":
                        API.ControlKneeboard(vaProxy, contextinput);
                        break;

                    case "kneeboard.tab.flight":
                        API.ControlKneeboard(vaProxy, contextinput);
                        break;

                    case "kneeboard.tab.notes":
                        API.ControlKneeboard(vaProxy, contextinput);
                        break;

                    case "kneeboard.tab.ref":
                        API.ControlKneeboard(vaProxy, contextinput);
                        break;

                    case "kneeboard.tab.all":
                        API.ControlKneeboard(vaProxy, contextinput);
                        break;

                    case "kneeboard.tab.prv":
                        API.ControlKneeboard(vaProxy, contextinput);
                        break;

                    case "kneeboard.tab.nxt":
                        API.ControlKneeboard(vaProxy, contextinput);
                        break;

                    case "kneeboard.opac.up":
                        API.ControlKneeboard(vaProxy, contextinput);
                        break;

                    case "kneeboard.opac.dn":
                        API.ControlKneeboard(vaProxy, contextinput);
                        break;

                    // F-4E WSO commands
                    case "wso.navigation.waypoint":
                        if (WSOCommandHandler.IsWSO())
                        {
                            WSOCommandHandler.NavigationResumeWaypoint();
                        }
                        break;

                    case "wso.navigation.holdpoint":
                        if (WSOCommandHandler.IsWSO())
                        {
                            WSOCommandHandler.NavigationHoldTurnPoint();
                        }
                        break;

                    case "wso.navigation.divert":
                        if (WSOCommandHandler.IsWSO())
                        {
                            WSOCommandHandler.NavigationDivertToWaypoint();
                        }
                        break;

                    case "wso.navigation.designate.waypoint":
                        if (WSOCommandHandler.IsWSO())
                        {
                            WSOCommandHandler.NavigationDesignateWaypoint();
                        }
                        break;

                    case "wso.navigation.tacan.channel":
                        if (WSOCommandHandler.IsWSO())
                        {
                            WSOCommandHandler.NavigationTuneTACANChannel();
                        }
                        break;

                    case "wso.navigation.tacan.station":
                        if (WSOCommandHandler.IsWSO())
                        {
                            WSOCommandHandler.NavigationTuneTACANStation();
                        }
                        break;

                    case "wso.radio.setchn":
                        if (WSOCommandHandler.IsWSO())
                        {
                            WSOCommandHandler.RadioSetCommChannel();
                        }
                        break;

                    case "wso.radio.setauxchn":
                        if (WSOCommandHandler.IsWSO())
                        {
                            WSOCommandHandler.RadioSetAuxChannel();
                        }
                        break;

                    case "wso.radio.tunefreq":
                        if (WSOCommandHandler.IsWSO())
                        {
                            WSOCommandHandler.RadioTuneFrequency();
                        }
                        break;

                    case "wso.pavespike.laser":
                        if (WSOCommandHandler.IsWSO())
                        {
                            WSOCommandHandler.PaveSpikeSetLaserCode();
                        }
                        break;

                    case "test":
                        API.API_Test(vaProxy);
                        break;

                    default:
                        vaProxy.WriteToLog("VAICOM dll undefined function call: context not set", Colors.Critical);
                        UI.Playsound.Error();
                        break;
                }

                VA_ExposeVariables(State.Proxy);
            }

            public static void VA_Exit1(dynamic vaProxy)
            {
                State.exitapp = true;

                try
                {
                    State.activeconfig.RIO_Enabled = false;

                    if (!State.datawasreset)
                    {
                        FileHandler.Lua.LuaFiles_Install(false, true); // resets F14 lua to normal (quiet)
                    }

                    FileHandler.Lua.LuaFiles_Install_Kneeboard(true, true);

                    UI.Timers.UI_Timer_Stop();
                    Beacon.Beacon_TimerStop();
                    Extensions.Chatter.AudioTimer.Chatter_TimerStop();
                    Processor.CloseTTSPlaybackStream();
                    Extensions.Kneeboard.OpenKneeboardBridge.Shutdown();

                    AppDomain.CurrentDomain.AssemblyResolve -= Framework.Assemblies.AssemblyResolve;
                    State.Proxy = null;
                }
                catch
                {
                }
            }

            public static void VA_ExposeVariables(dynamic vaProxy)
            {
                try
                {
                    vaProxy.SetText("vaicompro.serverdata.currentserver.dcsversion", State.currentstate.dcsversion);
                    vaProxy.SetText("vaicompro.serverdata.currentserver.theater", State.currentstate.theatre);

                    vaProxy.SetBoolean("vaicompro.serverdata.currentserver.multiplayer", State.currentstate.multiplayer);
                    vaProxy.SetBoolean("vaicompro.serverdata.currentserver.easycomms", State.currentstate.easycomms);
                    vaProxy.SetBoolean("vaicompro.serverdata.currentserver.vrmode", State.currentstate.vrmode);

                    vaProxy.SetText("vaicompro.playerdata.currentmodule.name", State.currentmodule.Name);
                    vaProxy.SetText("vaicompro.playerdata.currentmodule.cat", State.currentstate.playerunitcat);

                    vaProxy.SetBoolean("vaicompro.serverdata.currentserver.mission", State.currentstate.missiontitle.Substring(0, 100).Length > 100 ? State.currentstate.missiontitle.Substring(0, 100) : State.currentstate.missiontitle);

                }
                catch
                {
                }
            }

            private static bool IsWSO()
            {
                if (!State.currentmodule.Id.Equals("F-4E-45MC", StringComparison.OrdinalIgnoreCase))
                {
                    State.Proxy.WriteToLog("WSO commands are only available for the F-4E-45MC module.", Colors.Warning);
                    return false;
                }

                return true;
            }

            private static string GetSegment(int index)
            {
                return State.Proxy.Command.Segment(index);
            }

            private static int GetLastSegmentIndex(int start)
            {
                // Walk forwards till we hit "Not set" which is the VoiceAttack value
                // if there are no further segments in the configured command.
                int end = start;
                while (!string.Equals(GetSegment(end), "Not set"))
                {
                    end++;
                }

                return end - 1;
            }

            private static string GetNumberFromCommand()
            {
                return State.Proxy.Utility.ParseTokens("{TXTNUM:\"{CMD}\"}");
            }

            private static bool GetNumberFromSegment(int index, out string number)
            {
                string segment = GetSegment(index);
                if (segment.Equals("zero"))
                {
                    number = "0";
                    return true;
                }
               
                number = State.Proxy.Utility.ParseTokens("{TXTNUM:\"" + segment + "\"}");
                if (string.IsNullOrEmpty(number))
                {
                    return false;
                }

                return true;
            }

            private static bool IsSecondaryFlightPlan(string flightPlan)
            {
                return flightPlan.Contains("2") || flightPlan.Contains("second");
            }

            private static string GetWaypointDesignationType(string designation)
            {
                string designationType = "";

                switch (designation)
                {
                    case "turn point":
                        designationType = "DEFAULT";
                        break;
                    case "nav fix":
                    case "navigation fix":
                        designationType = "VIP";
                        break;
                    case "cap":
                    case "c a p":
                    case "combat air patrol":
                        designationType = "CAP";
                        break;
                    case "ip":
                    case "i p":
                    case "inbound point":
                        designationType = "IP";
                        break;
                    case "tgt":
                    case "t g t":
                    case "target":
                        designationType = "TARGET";
                        break;
                    case "fence in":
                        designationType = "FENCE_IN";
                        break;
                    case "fence out":
                        designationType = "FENCE_OUT";
                        break;
                    case "alternate":
                    case "alternate airfield":
                    case "alternative":
                        designationType = "ALTERNATE";
                        break;
                    case "homebase":
                        designationType = "HOMEBASE";
                        break;
                    default:
                        designationType = "DEFAULT";
                        break;
                }

                return designationType;
            }
        }
    }
}
