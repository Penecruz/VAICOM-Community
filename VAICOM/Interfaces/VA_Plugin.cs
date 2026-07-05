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

                string contextinput = API.NormalizeKneeboardContext(Helpers.Common.StringNormalize(vaProxy.Context));
                bool longpress = State.Proxy.Utility.ParseTokens("{CMDLONGPRESSINVOKED}") == "1";

                if (API.IsOpenKneeboardTabActionContext(contextinput))
                {
                    API.ControlOpenKneeboardOut(vaProxy, contextinput);
                    VA_ExposeVariables(State.Proxy);
                    return;
                }

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
                            bool hadDcsOptionsOpen = State.showingoptions;
                            bool hadJesterMenuOpen = VAICOM.Extensions.RIO.helper.showingjestermenu;
                            bool _complete = DcsClient.Message.processcommand();

                            bool keepTxLinkListeningForMenuFlow = State.showingoptions || VAICOM.Extensions.RIO.helper.showingjestermenu;
                            bool menuFlowClosedThisCommand = (hadDcsOptionsOpen || hadJesterMenuOpen) && !keepTxLinkListeningForMenuFlow;
                            bool shouldAutoSuspend = menuFlowClosedThisCommand || (_complete && !keepTxLinkListeningForMenuFlow);

                            if (PTT.TXLinkApply && shouldAutoSuspend)
                            {
                                Thread.Sleep(200);
                                PTT.PTT_ForceSuspendListeningAfterCommand();
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

                    case "wso.radar.focustarget":
                        if (WSOCommandHandler.IsWSO())
                        {
                            WSOCommandHandler.RadarFocusTarget();
                        }
                        break;

                    case "wso.radar.locktarget":
                        if (WSOCommandHandler.IsWSO())
                        {
                            WSOCommandHandler.RadarLockTarget();
                        }
                        break;

                    case "wso.fuel.tanker":
                        if (WSOCommandHandler.IsWSO())
                        {
                            WSOCommandHandler.RejoinWithTanker();
                        }
                        break;

                    case "wso.cap.time":
                        if (WSOCommandHandler.IsWSO())
                        {
                            WSOCommandHandler.OnCapStationTime();
                        }
                        break;

                    case "wso.mod.jesterwheel":
                        if (WSOCommandHandler.IsWSO())
                        {
                            WSOCommandHandler.JesterWheelModProxy();
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
        }
    }
}
