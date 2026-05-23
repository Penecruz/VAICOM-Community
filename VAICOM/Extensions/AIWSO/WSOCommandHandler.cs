using System;
using System.Collections.Generic;
using VAICOM.Extensions.Kneeboard;
using VAICOM.PushToTalk;
using VAICOM.Static;
using VAICOM.WSO;

namespace VAICOM
{
    namespace Extensions
    {
        namespace AIWSO
        {
            public static class WSOCommandHandler
            {
                public static void NavigationResumeWaypoint()
                {
                    // Although the issued action will be resume_flightplan_X we look up the cache entry using
                    // divert_tgt1_lat_lon. This is due to when adding new points to the flight plan they are not
                    // automatically given a cache key of resume_flightplan_X so will not be found in the cache.
                    // They are however given a cache key of divert_tgt1_lat_lon.
                    List<string> waypointCacheKeys = new List<string> { "divert_tgt1_lat_lon" };

                    string waypointCommandKey = "";
                    int waypointIndex = GetLastSegmentIndex(4);
                    if (!GetNumberFromSegment(waypointIndex, out string flightPlanWaypoint))
                    {
                        Log.Write($"Invalid waypoint for go to flightplan and waypoint", Colors.Warning);
                        return;
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

                    CommandCompleted("Resume At Waypoint", new List<string> { $"Flight Plan {waypointFlightPlan}", $"Waypoint {flightPlanWaypoint}" });
                    
                    if (WSOActionCache.TryGetByActionAndIndex(waypointCacheKey, out string resolvedWaypoint)
                            && !string.IsNullOrWhiteSpace(resolvedWaypoint))
                    {
                        // Append the waypoint number, after a semi-colon; to the lat long when sending the command as this is required
                        HbSendProxyCommand.SendWsoCommand(State.WsoWheelClient, waypointCommandKey, $"{resolvedWaypoint};{flightPlanWaypoint}");
                    }
                    else
                    {
                        Log.Write($"Waypoint not found for flight plan {waypointFlightPlan} waypoint {flightPlanWaypoint}", Colors.Warning);
                        UI.Playsound.Recipientna();
                    }
                }

                public static void NavigationHoldTurnPoint()
                {
                    string deactivate = GetSegment(1);
                    // We work backwards as the command may have been edited with additional segments added.
                    int holdpointIndex = GetLastSegmentIndex(5);
                    if (!GetNumberFromSegment(holdpointIndex, out string flightPlanHoldpoint))
                    {
                        Log.Write($"Invalid waypoint for holding to flightplan and waypoint", Colors.Warning);
                        return;
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

                    CommandCompleted("Hold At Waypoint", new List<string> { $"Flight Plan {holdpointFlightPlan}", $"Waypoint {flightPlanHoldpoint}" });
                    
                    if (WSOActionCache.TryGetByActionAndIndex(holdpointCacheKey, out string resolvedHoldpoint)
                            && !string.IsNullOrWhiteSpace(resolvedHoldpoint))
                    {
                        HbSendProxyCommand.SendWsoCommand(State.WsoWheelClient, holdpointCommandKey, resolvedHoldpoint);
                    }
                    else
                    {
                        Log.Write($"Holdpoint not found for flight plan {holdpointFlightPlan} waypoint {flightPlanHoldpoint}", Colors.Warning);
                        UI.Playsound.Recipientna();
                    }
                }

                public static void NavigationDivertToWaypoint()
                {
                    List<string> divertCacheKeys = new List<string> { "divert_tgt1_lat_lon" };

                    string divertCommandKey = "wMsgWSO_Navigation_Divert_LatLong";

                    // We work backwards as the command may have been edited with additional segments added.
                    int divertWaypointIndex = GetLastSegmentIndex(4);
                    if (!GetNumberFromSegment(divertWaypointIndex, out string divertWaypoint))
                    {
                        Log.Write($"Invalid waypoint for divert to flightplan and waypoint", Colors.Warning);
                        return;
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

                    CommandCompleted("Divert To Waypoint", new List<string> { $"Flight Plan {divertFlightPlan}", $"Waypoint {divertWaypoint}" });

                    if (WSOActionCache.TryGetByActionAndIndex(divertCacheKey, out string resolvedLatLong)
                            && !string.IsNullOrWhiteSpace(resolvedLatLong))
                    {
                        HbSendProxyCommand.SendWsoCommand(State.WsoWheelClient, divertCommandKey, resolvedLatLong);
                    }
                    else
                    {
                        Log.Write($"Waypoint not found for flight plan {divertFlightPlan} waypoint {divertWaypoint}", Colors.Warning);
                        UI.Playsound.Recipientna();
                    }
                }

                public static void NavigationDesignateWaypoint()
                {
                    string designateWaypointCommandKey = "wMsgWSO_Navigation_Designate_Waypoint";

                    int designationTypeIndex = GetLastSegmentIndex(6);
                    string designationType = GetWaypointDesignationType(GetSegment(designationTypeIndex));
                    if (string.IsNullOrEmpty(designationType))
                    {
                        Log.Write($"Unknown waypoint designation type for '{designationType}'", Colors.Warning);
                        UI.Playsound.Error();
                        return;
                    }
                    if (!GetNumberFromSegment(designationTypeIndex - 2, out string designatationWaypoint))
                    {
                        Log.Write($"Invalid waypoint for designating flightplan and waypoint", Colors.Warning);
                        return;
                    }

                    string designationFlightPlanValue = GetSegment(designationTypeIndex - 4);
                    int designationFlightPlan;
                    if (!String.IsNullOrEmpty(designationFlightPlanValue) && IsSecondaryFlightPlan(designationFlightPlanValue))
                    {
                        designationFlightPlan = 2;
                    }
                    else
                    {
                        designationFlightPlan = 1;
                    }
                    string designationWaypointValue = $"{designationFlightPlan};{designatationWaypoint};{designationType}";
                    
                    CommandCompleted("Designate Waypoint", new List<string> { $"Flight Plan {designationFlightPlan}", $"Waypoint {designatationWaypoint}", designationType });

                    HbSendProxyCommand.SendWsoCommand(State.WsoWheelClient, designateWaypointCommandKey, designationWaypointValue);
                }

                public static void NavigationTuneTACANChannel()
                {
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
                    
                    CommandCompleted($"Tune TACAN Channel", new List<string> { $"{digit1}{digit2}{digit3}{tacanBand.ToUpper()}" });

                    HbSendProxyCommand.SendWsoCommand(State.WsoWheelClient, "wMsgWSO_Navigation_TACAN_SelectChannel", $"{digit1}{digit2}{digit3}{tacanBand}");
                }

                public static void NavigationTuneTACANStation()
                {
                    int stationIndex = GetLastSegmentIndex(4);
                    string alpha1 = GetSegment(stationIndex - 2).Substring(0, 1);
                    string alpha2 = GetSegment(stationIndex - 1).Substring(0, 1);
                    string alpha3 = GetSegment(stationIndex).Substring(0, 1);
                    string tacanStation = $"{alpha1}{alpha2}{alpha3}";
                    string tacanStationUpper = tacanStation.ToUpper();

                    CommandCompleted("Tune TACAN Station", new List<string> { tacanStationUpper });
                    
                    if (WSOActionCache.TryGetByActionAndName("nav_tacan_tr", tacanStation, out string resolvedTacanStation)
                            && !string.IsNullOrWhiteSpace(resolvedTacanStation))
                    {
                        HbSendProxyCommand.SendWsoCommand(State.WsoWheelClient, "wMsgWSO_Navigation_TACAN_TuneStation", resolvedTacanStation);
                    }
                    else
                    {
                        Log.Write($"TACAN station '{tacanStationUpper}' not found", Colors.Warning);
                        UI.Playsound.Recipientna();
                    }
                }

                public static void RadioSetCommChannel()
                {
                    string commsChannel = GetNumberFromCommand();
                    CommandCompleted("Set Comm Channel", new List<string> { commsChannel });
                    
                    HbSendProxyCommand.SendWsoCommand(State.WsoWheelClient, "wMsgWSO_Radio_SelectCommChannel", commsChannel);
                }

                public static void RadioSetAuxChannel()
                {
                    string auxChannel = GetNumberFromCommand();
                    CommandCompleted("Set AUX Channel", new List<string> { auxChannel });
                    
                    HbSendProxyCommand.SendWsoCommand(State.WsoWheelClient, "wMsgWSO_Radio_SelectAuxChannel", auxChannel);
                }

                public static void RadioTuneFrequency()
                {
                    string radioFrequency = GetNumberFromCommand();
                    string fullRadioFrequency = radioFrequency.PadRight(6, '0');
                    CommandCompleted("Tune Radio Freqency", new List<string> { fullRadioFrequency });
                    
                    HbSendProxyCommand.SendWsoCommand(State.WsoWheelClient, "wMsgWSO_Radio_SetManualFrequency", fullRadioFrequency);
                }

                public static void PaveSpikeSetLaserCode()
                {
                    string laserCode = GetNumberFromCommand();
                    CommandCompleted("Pave Spike Laser Code", new List<string> { laserCode });
                        
                    HbSendProxyCommand.SendWsoCommand(State.WsoWheelClient, "wMsgWSO_A2G_PaveSpike_LaserCode", laserCode);
                }

                public static bool IsWSO()
                {
                    if (!State.currentmodule.Id.Equals("F-4E-45MC", StringComparison.OrdinalIgnoreCase))
                    {
                        Log.Write("WSO commands are only available for the F-4E-45MC module.", Colors.Warning);
                        UI.Playsound.Sorry();
                        return false;
                    }

                    return true;
                }

                private static void CommandCompleted(string message, List<string> args)
                {
                    UI.Playsound.Commandcomplete();

                    string messageArgs = "[ " + string.Join(" ] [ ", args) + " ]";

                    if ((State.currentmodule.Singlehotkey & !State.activeconfig.ForceMultiHotkey) || (!State.currentmodule.Singlehotkey & State.activeconfig.ForceSingleHotkey)) // for single mode
                    {
                        Log.Write(State.currentTXnode.name + " | " + PTT.RadioDevices.SEL.name + ": [ F-4E AI WSO ], [ " + message + " ] " + messageArgs, Colors.Message);
                    }
                    else
                    {
                        Log.Write(State.currentTXnode.name + " | " + State.currentTXnode.radios[0].name + ": [ F-4E AI WSO ], [ " + message + " ] " + messageArgs , Colors.Message);
                    }

                    try
                    {
                        string wsoLog = "WSO | " + message;
                        OpenKneeboardBridge.UpdateLog("AI CREW", wsoLog);
                        OpenKneeboardBridge.SetLastAiCrewCommand(wsoLog);
                    }
                    catch
                    {
                    }
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
}
