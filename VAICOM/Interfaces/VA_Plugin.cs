using System;
using System.Threading;
using VAICOM.Client;
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
                                PTT.PTT_Handler(State.Proxy, State.currentTXnode, true, false);
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
                        UI.Initialize.OpenConfiguration(vaProxy, false); //Pene Broken in VA 1.7.55 WIP
                        break;

                    case "config.resetwindow":
                        UI.Initialize.OpenConfiguration(vaProxy, true);
                        break;

                    // Chatter on/off

                    case "chatter":
                        if (State.chatterthemesactivated && State.activeconfig.Chatter_Enabled) // Pene Playing
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
                        if (State.jesteractivated && State.dll_installed_rio)
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
                        if (State.jesteractivated && State.dll_installed_rio)
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
                        if (State.jesteractivated && State.dll_installed_rio)
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
                        if (State.jesteractivated && State.dll_installed_rio)
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
                        if (State.jesteractivated && State.dll_installed_rio)
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
                        if (State.jesteractivated && State.dll_installed_rio)
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
                        if (State.jesteractivated && State.dll_installed_rio)
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
                        if (State.jesteractivated && State.dll_installed_rio)
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
                        if (State.jesteractivated && State.dll_installed_rio)
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
                        if (State.jesteractivated && State.dll_installed_rio)
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
                        if (State.PRO)
                        {
                            Client.DcsClient.Message.RadioControl_TuneChan();
                        }
                        else
                        {
                            vaProxy.WriteToLog("This command requires PRO license.", Colors.Critical);
                        }
                        break;

                    case "dev.radio.setfrq":
                        if (State.PRO)
                        {
                            Client.DcsClient.Message.RadioControl_TuneFreq();
                        }
                        else
                        {
                            vaProxy.WriteToLog("This command requires PRO license.", Colors.Critical);
                        }
                        break;

                    // API calls PTT

                    case "ptt.mode.release":
                        if (State.PRO)
                        {
                            API.PTT_Release_Hot(vaProxy);
                        }
                        break;

                    case "ptt.mode.page.up":
                        if (State.PRO)
                        {
                            API.PTT_Mode_Page_Up(vaProxy);
                        }
                        break;

                    case "ptt.mode.page.dn":
                        if (State.PRO)
                        {
                            API.PTT_Mode_Page_Dn(vaProxy);
                        }
                        break;

                    case "chatter.vol.up":
                        if (State.PRO)
                        {
                            API.Chatter_Vol_Up(vaProxy);
                        }
                        break;

                    case "chatter.vol.dn":
                        if (State.PRO)
                        {
                            API.Chatter_Vol_Dn(vaProxy);
                        }
                        break;

                    case "ptt.mode.dial":
                        if (State.PRO)
                        {
                            API.Operate_Dial(vaProxy);
                        }
                        break;

                    case "ptt.mode.map.srs":
                        if (State.PRO)
                        {
                            API.SRS_Mapping(vaProxy);
                        }
                        break;

                    case "ptt.mode.prv":
                        if (State.PRO)
                        {
                            API.PTT_Mode_Prv(vaProxy);
                        }
                        break;

                    case "ptt.mode.nxt":
                        if (State.PRO)
                        {
                            API.PTT_Mode_Nxt(vaProxy);
                        }
                        break;

                    case "ptt.mode.inv":
                        if (State.PRO)
                        {
                            API.PTT_Mode_Inv(vaProxy);
                        }
                        break;

                    case "ptt.mode.norm":
                        if (State.PRO)
                        {
                            API.PTT_Mode_Norm(vaProxy);
                        }
                        break;

                    case "ptt.mode.multi":
                        if (State.PRO)
                        {
                            API.PTT_Mode_Multi(vaProxy);
                        }
                        break;

                    case "ptt.mode.sngl":
                        if (State.PRO)
                        {
                            API.PTT_Mode_Single(vaProxy);
                        }
                        break;

                    // --------------------------------------------------------------------------
                    // kneeboard control

                    case "kneeboard.tab.log":
                        if (State.PRO)
                        {
                            API.ControlKneeboard(vaProxy, contextinput);
                        }
                        break;

                    case "kneeboard.tab.awacs":
                        if (State.PRO)
                        {
                            API.ControlKneeboard(vaProxy, contextinput);
                        }
                        break;

                    case "kneeboard.tab.jtac":
                        if (State.PRO)
                        {
                            API.ControlKneeboard(vaProxy, contextinput);
                        }
                        break;

                    case "kneeboard.tab.atc":
                        if (State.PRO)
                        {
                            API.ControlKneeboard(vaProxy, contextinput);
                        }
                        break;

                    case "kneeboard.tab.tanker":
                        if (State.PRO)
                        {
                            API.ControlKneeboard(vaProxy, contextinput);
                        }
                        break;

                    case "kneeboard.tab.aocs":
                        if (State.PRO)
                        {
                            API.ControlKneeboard(vaProxy, contextinput);
                        }
                        break;

                    case "kneeboard.tab.flight":
                        if (State.PRO)
                        {
                            API.ControlKneeboard(vaProxy, contextinput);
                        }
                        break;

                    case "kneeboard.tab.notes":
                        if (State.PRO)
                        {
                            API.ControlKneeboard(vaProxy, contextinput);
                        }
                        break;

                    case "kneeboard.tab.ref":
                        if (State.PRO)
                        {
                            API.ControlKneeboard(vaProxy, contextinput);
                        }
                        break;

                    case "kneeboard.tab.all":
                        if (State.PRO)
                        {
                            API.ControlKneeboard(vaProxy, contextinput);
                        }
                        break;

                    case "kneeboard.tab.prv":
                        if (State.PRO)
                        {
                            API.ControlKneeboard(vaProxy, contextinput);
                        }
                        break;

                    case "kneeboard.tab.nxt":
                        if (State.PRO)
                        {
                            API.ControlKneeboard(vaProxy, contextinput);
                        }
                        break;

                    case "kneeboard.opac.up":
                        if (State.PRO)
                        {
                            API.ControlKneeboard(vaProxy, contextinput);
                        }
                        break;

                    case "kneeboard.opac.dn":
                        if (State.PRO)
                        {
                            API.ControlKneeboard(vaProxy, contextinput);
                        }
                        break;

                    // --------------------------------------------------------------------------

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

                    AppDomain.CurrentDomain.AssemblyResolve -= Framework.Assemblies.AssemblyResolve;
                    State.Proxy = null;
                }
                catch
                {
                }
            }

            public static void VA_ExposeVariables(dynamic vaProxy)
            {
                if (State.PRO)
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
}
