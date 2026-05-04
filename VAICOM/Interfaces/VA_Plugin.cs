using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using VAICOM.Client;
using VAICOM.Extensions.WorldAudio;
using VAICOM.FileManager;
using VAICOM.PushToTalk;
using VAICOM.Static;
using VAICOM.WSO;

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
                        if (!IsWSO()) break;

                        // Although the issued action will be resume_flightplan_X we look up the cache entry using
                        // divert_tgt1_lat_lon. This is due to when adding new points to the flight plan they are not
                        // automatically given a cache key of resume_flightplan_X so will not be found in the cache.
                        // They are however given a cache key of divert_tgt1_lat_lon.
                        List<string> waypointCacheKeys = new List<string> { "divert_tgt1_lat_lon" };

                        string waypointCommandKey = "";
                        int waypointIndex = GetLastSegmentIndex(4);
                        if (!GetNumberFromSegment(waypointIndex, out string flightPlanWaypoint))
                        {
                            vaProxy.WriteToLog($"Invalid waypoint for go to flightplan and waypoint", Colors.Warning);
                            break;
                        }

                        string waypointFlightPlan = State.Proxy.Command.Segment(waypointIndex - 2);
                        // Check if a flight plan was included in the command, and whether or not it was flight plan 2.
                        if (!String.IsNullOrEmpty(waypointFlightPlan) && IsSecondaryFlightPlan(waypointFlightPlan))
                        {
                            waypointCommandKey = "wMsgWSO_Navigation_ResumeFlightPlan2Waypoint";
                            waypointCacheKeys.Add("fp2");
                        }
                        else
                        {
                            waypointCommandKey = "wMsgWSO_Navigation_ResumeFlightPlan1Waypoint";
                            waypointCacheKeys.Add("fp1"); ;
                        }
                        waypointCacheKeys.Add(flightPlanWaypoint);

                        string waypointCacheKey = String.Join("|", waypointCacheKeys);
                        if (State.WsoNavCacheByActionAndIndex.TryGetValue(waypointCacheKey, out string resolvedWaypoint)
                                && !string.IsNullOrWhiteSpace(resolvedWaypoint))
                        {
                            // Append the waypoint number, after a semi-colon; to the lat long when sending the command as this is required
                            HbSendProxyCommand.SendWsoCommand(State.WebSocketClient, waypointCommandKey, $"{resolvedWaypoint};{flightPlanWaypoint}");
                        }
                        else
                        {
                            vaProxy.WriteToLog($"Waypoint not found for flight plan {waypointFlightPlan} waypoint {flightPlanWaypoint}", Colors.Warning);
                        }
                        break;

                    case "wso.navigation.holdpoint":
                        if (!IsWSO()) break;

                        string deactivate = GetSegment(1);
                        // We work backwards as the command may have been edited with additional segments added.
                        int holdpointIndex = GetLastSegmentIndex(5);
                        if (!GetNumberFromSegment(holdpointIndex, out string flightPlanHoldpoint))
                        {
                            vaProxy.WriteToLog($"Invalid waypoint for holding to flightplan and waypoint", Colors.Warning);
                            break;
                        }

                        string holdpointFlightPlan = GetSegment(holdpointIndex - 2);
                        List<string> holdpointCacheKeys = new List<string>();
                        string holdpointCommandKey = "";

                        // Check if a flight plan was included in the command, and whether or not it was flight plan 2
                        if (IsSecondaryFlightPlan(holdpointFlightPlan))
                        {
                            // Check if we are activating or deactivating the hold point.
                            if (String.IsNullOrEmpty(deactivate))
                            {
                                holdpointCommandKey = "wMsgWSO_Navigation_Holding_ActivateFlightPlan2TurnPoint";
                            }
                            else
                            {
                                holdpointCommandKey = "wMsgWSO_Navigation_Holding_DeactivateFlightPlan2TurnPoint";
                            }
                            // There is no cache key for deactivate so we lookup based on the hold_flightplan_x key
                            holdpointCacheKeys.Add("hold_flightplan_2");
                            holdpointCacheKeys.Add("fp2");
                        }
                        else
                        {
                            if (String.IsNullOrEmpty(deactivate))
                            {
                                holdpointCommandKey = "wMsgWSO_Navigation_Holding_ActivateFlightPlan1TurnPoint";
                            }
                            else
                            {
                                holdpointCommandKey = "wMsgWSO_Navigation_Holding_DeactivateFlightPlan1TurnPoint";
                            }
                            holdpointCacheKeys.Add("hold_flightplan_1");
                            holdpointCacheKeys.Add("fp1");
                        }
                        holdpointCacheKeys.Add(flightPlanHoldpoint);

                        string holdpointCacheKey = String.Join("|", holdpointCacheKeys);
                        if (State.WsoNavCacheByActionAndIndex.TryGetValue(holdpointCacheKey, out string resolvedHoldpoint)
                                && !string.IsNullOrWhiteSpace(resolvedHoldpoint))
                        {
                            HbSendProxyCommand.SendWsoCommand(State.WebSocketClient, holdpointCommandKey, resolvedHoldpoint);
                        }
                        else
                        {
                            vaProxy.WriteToLog($"Holdpoint not found for flight plan {holdpointFlightPlan} waypoint {flightPlanHoldpoint}", Colors.Warning);
                        }
                        break;

                    case "wso.navigation.divert":
                        if (!IsWSO()) break;

                        List<string> divertCacheKeys = new List<string> { "divert_tgt1_lat_lon" };

                        string divertCommandKey = "wMsgWSO_Navigation_Divert_LatLong";

                        // We work backwards as the command may have been edited with additional segments added.
                        int divertWaypointIndex = GetLastSegmentIndex(4);
                        if (!GetNumberFromSegment(divertWaypointIndex, out string divertWaypoint))
                        {
                            vaProxy.WriteToLog($"Invalid waypoint for divert to flightplan and waypoint", Colors.Warning);
                            break;
                        }
                        string divertFlightPlan = GetSegment(divertWaypointIndex - 2);

                        if (!String.IsNullOrEmpty(divertFlightPlan) && IsSecondaryFlightPlan(divertFlightPlan))
                        {
                            divertCacheKeys.Add("fp2");
                        }
                        else
                        {
                            divertCacheKeys.Add("fp1");
                        }
                        divertCacheKeys.Add(divertWaypoint);

                        string divertCacheKey = String.Join("|", divertCacheKeys);
                        if (State.WsoNavCacheByActionAndIndex.TryGetValue(divertCacheKey, out string resolvedLatLong)
                                && !string.IsNullOrWhiteSpace(resolvedLatLong))
                        {
                            HbSendProxyCommand.SendWsoCommand(State.WebSocketClient, divertCommandKey, resolvedLatLong);
                        }
                        else
                        {
                            vaProxy.WriteToLog($"Divert lat/long not found for flight plan {divertFlightPlan} waypoint {divertWaypoint}", Colors.Warning);
                        }
                        break;

                    case "wso.navigation.tacan.channel":
                        if (!IsWSO()) break;

                        // We work backwards as the command may have been edited with additional segments added.
                        int bandIndex = GetLastSegmentIndex(5);
                        string digit1 = GetSegment(bandIndex - 3);
                        string digit2 = GetSegment(bandIndex - 2);
                        string digit3 = GetSegment(bandIndex - 1);
                        // Use the first character for the band so we can support the NATO alphabet.
                        string tacanBand = GetSegment(bandIndex).Substring(0, 1);

                        // If the command uses zero at the beginning then is in text and the TXTUM
                        // function does not convert it so we need to do manually.
                        if (digit1.Equals("zero"))
                        {
                            digit1 = "0";
                        }
                        HbSendProxyCommand.SendWsoCommand(State.WebSocketClient, "wMsgWSO_Navigation_TACAN_SelectChannel", $"{digit1}{digit2}{digit3}{tacanBand}");
                        break;

                    case "wso.navigation.tacan.station":
                        if (!IsWSO()) break;

                        int stationIndex = GetLastSegmentIndex(4);
                        string alpha1 = GetSegment(stationIndex - 2).Substring(0, 1);
                        string alpha2 = GetSegment(stationIndex - 1).Substring(0, 1);
                        string alpha3 = GetSegment(stationIndex).Substring(0, 1);

                        string tacanCacheKey = $"nav_tacan_tr|{alpha1}{alpha2}{alpha3}";
                        if (State.WsoNavCacheByActionAndName.TryGetValue(tacanCacheKey, out string resolvedTacanStation)
                                && !string.IsNullOrWhiteSpace(resolvedTacanStation))
                        {
                            HbSendProxyCommand.SendWsoCommand(State.WebSocketClient, "wMsgWSO_Navigation_TACAN_TuneStation", resolvedTacanStation);
                        }
                        else
                        {
                            vaProxy.WriteToLog($"TACAN station '{alpha1}{alpha2}{alpha3}' not found", Colors.Warning);
                        }
                        break;

                    case "wso.radio.setchn":
                        if (!IsWSO()) break;

                        HbSendProxyCommand.SendWsoCommand(State.WebSocketClient, "wMsgWSO_Radio_SelectCommChannel", GetNumberFromCommand());
                        break;

                    case "wso.radio.setauxchn":
                        if (!IsWSO()) break;

                        HbSendProxyCommand.SendWsoCommand(State.WebSocketClient, "wMsgWSO_Radio_SelectAuxChannel", GetNumberFromCommand());
                        break;

                    case "wso.radio.tunefreq":
                        if (!IsWSO()) break;

                        string radioFrequency = GetNumberFromCommand();
                        string fullRadioFrequency = radioFrequency.PadRight(6, '0');
                        HbSendProxyCommand.SendWsoCommand(State.WebSocketClient, "wMsgWSO_Radio_SetManualFrequency", fullRadioFrequency);
                        break;

                    case "wso.pavespike.laser":
                        string laserCode = GetNumberFromCommand();
                        // If a code was provided then use it, otherwise this was a command to silence it
                        if (String.IsNullOrEmpty(laserCode))
                        {
                            HbSendProxyCommand.SendWsoCommand(State.WebSocketClient, "wMsgWSO_A2G_PaveSpike_LaserCode_Silent");
                        }
                        else
                        {
                            HbSendProxyCommand.SendWsoCommand(State.WebSocketClient, "wMsgWSO_A2G_PaveSpike_LaserCode", laserCode);
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
        }
    }
}
