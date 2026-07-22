using System;
using System.Collections.Generic;
using VAICOM.Database;
using VAICOM.Extensions.Kneeboard;
using VAICOM.Extensions.RIO;
using VAICOM.PushToTalk;
using VAICOM.Static;
using DatabaseCommands = VAICOM.Database.Commands;

namespace VAICOM
{
    namespace Client
    {
        public partial class DcsClient
        {
            public static partial class Message
            {
                private static readonly Dictionary<string, string> georgeoptionhints = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    { "wMsgGeorgeNextWeapon", "\"Next Weapon\"\nCycles through currently available George weapons." },
                    { "wMsgGeorgeMacroSelectGun", "\"Select Gun\"\nSelects Gun when available." },
                    { "wMsgGeorgeMacroSelectMissiles", "\"Select Missiles\"\nSelects Missiles when available." },
                    { "wMsgGeorgeMacroSelectRockets", "\"Select Rockets\"\nSelects Rockets when available." },
                    { "wMsgGeorgeMacroSelectNoWeapon", "\"Select No Weapon\"\nSelects No Weapon (de-WAS)." },
                    { "wMsgGeorgeMacroAddTwoTargetsTrack", "\"Add Two Targets and Track\"\nAdds top 2 targets from list and starts track." },
                    { "wMsgGeorgeMacroAddThreeTargetsTrack", "\"Add Three Targets and Track\"\nAdds top 3 targets from list and starts track." },
                    { "wMsgGeorgeMacroAddFourTargetsTrack", "\"Add Four Targets and Track\"\nAdds top 4 targets from list and starts track." },
                    { "wMsgGeorgeMacroTrackEngage", "\"Track and Engage\"\nTracks current target and sends engage sequence." }
                };

                public static bool IsAOCS(Servers.Server.DcsUnit unit)
                {
                    return unit.id_.Equals(123456789);
                }

                public static void settunenum()
                {
                    State.currentmessage.tunenum = (State.currentstate.easycomms && State.currentcommand.isState()) || 
                        (State.currentrecipientclass.Name.Equals("AOCS") && 
                        (State.currentstate.easycomms || (State.currentcommand.isSelect() && State.activeconfig.AllowRadioTuning)));

                    if ((bool)State.currentmessage.tunenum)
                    {
                        try
                        {
                            State.currentmessage.type = Messagetypes.SettingsChange;

                            State.currentmessage.tunemod = State.currentstate.availablerecipients["Aux"].Find(IsAOCS).mod.Equals("FM") ? 1 : 0;
                            State.currentmessage.tunefrq.Add(State.currentstate.availablerecipients["Aux"].Find(IsAOCS).freq);
                            State.currentmessage.tunefrq.AddRange(State.currentstate.availablerecipients["Aux"].Find(IsAOCS).altfreq);

                            Log.Write("Tune numeric: " + State.currentmessage.tunemod + " " + State.currentmessage.tunefrq[0], Colors.Inline);
                        }
                        catch (Exception e)
                        {
                            Log.Write("Tune numeric: " + e.StackTrace, Colors.Inline);
                        }
                    }
                    else
                    {
                        State.currentmessage.tunenum = null;
                    }
                }

                public static int GetSendDeviceId()
                {
                    try
                    {
                        int returndeviceid = 255;
                        State.currentradiodevicename = "VAICOM PRO";

                        // single hotkey (native or forced)
                        if ((State.currentmodule.Singlehotkey & !State.activeconfig.ForceMultiHotkey) || 
                            (!State.currentmodule.Singlehotkey & State.activeconfig.ForceSingleHotkey))
                        {
                            if (State.currentTXnode != null && State.currentTXnode.Equals(PTT.TXNodes.TX5))
                            {
                                returndeviceid = PTT.RadioDevices.INT.deviceid;
                                State.currentradiodevicename = PTT.RadioDevices.INT.name;
                            }
                            else
                            {
                                returndeviceid = PTT.RadioDevices.SEL.deviceid; // = 0 or deviceid from currently selected radio
                                State.currentradiodevicename = PTT.RadioDevices.SEL.name;
                            }
                        }
                        else // multi hotkey (native or forced)
                        {
                            returndeviceid = State.currentTXnode.radios[0].deviceid;
                            State.currentradiodevicename = State.currentTXnode.radios[0].name;
                        }

                        return returndeviceid;
                    }
                    catch
                    {
                        return 255;
                    }
                }

                public static bool ProcessIfRIO()
                {
                    if (State.currentcommand.isRIO())
                    {
                        if (State.dll_installed_rio && State.activeconfig.RIO_Enabled)
                        {
                            constructRIO();
                            return true;
                        }
                        else
                        {
                            Log.Write("AIRIO extension is not enabled.", Colors.Warning);
                            return false;
                        }
                    }
                    else // command not rio
                    {
                        if (State.IsCrewHotMicActiveOnIntercomTX() && 
                            !(State.activeconfig.MP_VoIPUseSwitch && State.activeconfig.MP_DelayTransmit))
                        {
                            // hotmic is active, RIO not called
                            if (!State.valistening) // hotmic was used
                            {
                                bool isGeorgeCommand = State.currentcommand != null
                                    && !string.IsNullOrEmpty(State.currentcommand.dcsid)
                                    && State.currentcommand.dcsid.StartsWith("wMsgGeorge", StringComparison.OrdinalIgnoreCase);

                                if (!State.currentrecipientclass.Equals(Recipientclasses.Crew)
                                    && !State.currentcommand.isMenu()
                                    && !State.currentcommand.isOptions()
                                    && !isGeorgeCommand)
                                {
                                    Log.Write("ICS HOT MIC: Use Push-To-Talk TX nodes to transmit radio messages.", Colors.Warning);
                                    if (State.activeconfig.UIaddhints)
                                    {
                                        UI.Playsound.Error();
                                    }
                                    return false;
                                }
                            }
                        }

                        return true;
                    }
                }
                // George AI commands are only available in AH-64D, Device Command Macros for message construction.
                public static bool ProcessIfGeorge()
                {
                    if (!State.currentcommand.dcsid.StartsWith("wMsgGeorge", StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }

                    if (State.currentmodule == null || !State.currentmodule.Id.Equals("AH-64D", StringComparison.OrdinalIgnoreCase))
                    {
                        Log.Write("George AI commands are only available in AH-64D.", Colors.Warning);
                        return false;
                    }

                    constructGeorge();
                    return true;
                }

                public static void constructGeorge()
                {
                    State.currentmessage.type = Messagetypes.DeviceControl;
                    State.currentmessage.extsequence = new List<Extensions.RIO.DeviceAction>();

                    bool hadValidWeaponState = State.AH64GeorgeWeaponStateValid;
                    bool previousGunAvailable = State.AH64GeorgeGunAvailable;
                    bool previousMissilesAvailable = State.AH64GeorgeMissilesAvailable;
                    bool previousRocketsAvailable = State.AH64GeorgeRocketsAvailable;

                    RefreshGeorgeWeaponAvailabilityFromPayloadProbe();
                    ForceGeorgeNoWeaponOnDepletedSelection(hadValidWeaponState, previousGunAvailable, previousMissilesAvailable, previousRocketsAvailable);

                    if (State.currentstate != null && !State.currentstate.airborne && State.AH64GeorgeSelectedWeapon != State.AH64GeorgeWeaponMode.NoWeapon)
                    {
                        ForceGeorgeNoWeaponLocalSync("weight-on-wheels", false);
                    }

                    if (State.activeconfig.RIO_Messages)
                    {
                        if (!State.activeconfig.RIO_Hints_Only)
                        {
                            State.currentmessage.dspmsg = "VAICOM: GEORGE | " + Database.Labels.aicommands[State.currentkey["command"]];
                            State.currentmessage.msgdur = 3;
                        }

                        string contextualHint;
                        if (georgeoptionhints.TryGetValue(State.currentkey["command"], out contextualHint))
                        {
                            State.currentmessage.dspmsg = "GEORGE command use:\n" + contextualHint;
                            State.currentmessage.msgdur = 5;
                        }
                    }

                    try
                    {
                        string georgeLog = State.currentmessage.dspmsg;
                        if (string.IsNullOrWhiteSpace(georgeLog))
                        {
                            georgeLog = "GEORGE | " + Database.Labels.aicommands[State.currentkey["command"]];
                        }
                        OpenKneeboardBridge.UpdateLog("AI CREW", georgeLog);
                    }
                    catch
                    {
                    }

                    switch (State.currentcommand.dcsid)
                    {
                        //Show/Hide George Overlay
                        case "wMsgGeorgeShowHide":
                            AddGeorgeButton(3002);
                            break;
                        //Up Short Presses
                        case "wMsgGeorgeUp":
                        case "wMsgGeorgePreviuousTarget":
                        case "wMsgGeorgePreviousItem":
                            AddGeorgeButton(3003);
                            break;
                        //Down Short Presses
                        case "wMsgGeorgeDown":
                        case "wMsgGeorgeNextTarget":
                        case "wMsgGeorgeNextItem":
                            AddGeorgeButton(3004);
                            break;
                        //Left Short Presses
                        case "wMsgGeorgeLeft":
                        case "wMsgGeorgeNextWeapon":
                        case "wMsgGeorgeExitList":
                            if (State.currentcommand.dcsid.Equals("wMsgGeorgeNextWeapon", StringComparison.OrdinalIgnoreCase) && !CanChangeGeorgeWeaponSelection())
                            {
                                break;
                            }

                            AddGeorgeButton(3005);
                            if (State.currentcommand.dcsid.Equals("wMsgGeorgeNextWeapon", StringComparison.OrdinalIgnoreCase))
                            {
                                State.AH64GeorgeSelectedWeapon = GetNextGeorgeWeapon(State.AH64GeorgeSelectedWeapon);
                            }
                            break;
                        //Right Short Presses
                        case "wMsgGeorgeRight":
                        case "wMsgGeorgeTrackTarget":                            
                        case "wMsgGeorgeLaseTarget":
                        case "wMsgGeorgeLaserOn":
                        case "wMsgGeorgeLaserOff":
                        case "wMsgGeorgeBurstLimit":
                        case "wMsgGeorgeRocketQuantity":
                        case "wMsgGeorgeLOBL":
                        case "wMsgGeorgeLOAL":
                        case "wMsgGeorgeListItemSelect":
                            AddGeorgeButton(3006);
                            break;
                        //Multifunction Short Presses
                        case "wMsgGeorgeCenter":                        
                        case "wMsgGeorgeClearedFire":
                        case "wMsgGeorgeTadsFov":
                        case "wMsgGeorgeSelectTarget":
                        case "wMsgGeorgelastStoredTarget":
                        case "wMsgGeorgePointSearch":
                            AddGeorgeButton(3008);
                            break;
                        //Request Control when in the CPG seat
                        case "wMsgGeorgeControlRequest":
                            AddGeorgeAction(3001, 1.0);
                            break;
                        //Store Target 
                        case "wMsgGeorgeStoreTarget":
                            AddGeorgeAction(3009, 1.0);
                            break;
                        //Up Long Presses                         
                        case "wMsgGeorgeUpLong":
                        case "wMsgGeorgeWeaponsFree":
                        case "wMsgGeorgeHoldFire":
                        case "wMsgGeorgeTadsZoomIn":
                        case "wMsgGeorgeTargetListZoomIn":
                            AddGeorgeLongButton(3003);
                            break;
                        //Down Long Presses
                        case "wMsgGeorgeDownLong":
                        case "wMsgGeorgeTadsZoomOut":                            
                        case "wMsgGeorgeTargetListZoomOut":
                        case "wMsgGeorgeLastFoundTarget":
                            AddGeorgeLongButton(3004);
                            break;
                        //Left Long Presses
                        case "wMsgGeorgeLeftLong":                                            
                        case "wMsgGeorgeTargetListFilter":
                        case "wMsgGeorgePointListFilterMode":
                        case "wMsgGeorgeNextRkt":
                        case "wMsgGeorgeNextMSL":
                        case "wMsgGeorgeAreaSelect":
                            AddGeorgeLongButton(3005);
                            break;
                        //Right Long Presses
                        case "wMsgGeorgeRightLong":                            
                        case "wMsgGeorgePointListFilterThreat":
                        case "wMsgGeorgeMslTraj":
                        case "wMsgGeorgePointSelect":
                            AddGeorgeLongButton(3006);
                            break;
                        //Multifunction Long Presses
                        case "wMsgGeorgeCenterLong":                                                     
                        case "wMsgGeorgeStartUp":
                        case "wMsgGeorgeShutdown":
                        case "wMsgGeorgeAdjustAim":
                        case "wMsgGeorgeTadsSensor":
                        case "wMsgGeorgeAreaSearch":
                            AddGeorgeLongButton(3008);
                            break;
                        
                        //Search Tasks
                        //Direct Searches
                        case "wMsgGeorgeMacroPHSsearch":                            
                            AddGeorgeButton(3003);
                            break;
                        case "wMsgGeorgeMacroTADSLOS":                            
                            AddGeorgeLongButton(3004);
                            break;
                        //Area Search Macros PHS, FWD, PFZ and hide overlay
                        case "wMsgGeorgeMacroNextSearch":
                            AddGeorgeLongButton(3005, 120);
                            AddGeorgeButton(3004, 80);
                            AddGeorgeButton(3006, 80);
                            AddGeorgeButton(3002);
                            break;
                        case "wMsgGeorgeMacroPreviousSearch":
                            AddGeorgeLongButton(3005, 200);
                            AddGeorgeButton(3003, 150);
                            AddGeorgeButton(3006, 80);
                            AddGeorgeButton(3002);
                            break;
                        //Point Search Macros and hide overlay
                        case "wMsgGeorgeMacroNextPoint":
                            AddGeorgeLongButton(3006, 200);
                            AddGeorgeButton(3004, 150);
                            AddGeorgeButton(3006, 80);
                            AddGeorgeButton(3002);
                            break;
                        case "wMsgGeorgeMacroPreviousPoint":
                            AddGeorgeLongButton(3006, 200);
                            AddGeorgeButton(3003, 150);
                            AddGeorgeButton(3006, 80);
                            AddGeorgeButton(3002);
                            break;
                         //Target List and Track macros
                         case "wMsgGeorgeMacroAddTwoTargetsTrack": //Add and Track Top 2 targets in list
                            AddGeorgeButton(3008, 100);
                            AddGeorgeButton(3004, 100);
                            AddGeorgeButton(3008, 100);
                            AddGeorgeButton(3006);
                            break;
                         case "wMsgGeorgeMacroAddThreeTargetsTrack": //Add and Track Top 3 targets in list
                            AddGeorgeButton(3008, 100);
                            AddGeorgeButton(3004, 100);
                            AddGeorgeButton(3008, 100);
                            AddGeorgeButton(3004, 100);
                            AddGeorgeButton(3008, 100);
                            AddGeorgeButton(3006);
                            break;
                         case "wMsgGeorgeMacroAddFourTargetsTrack": //Add and Track Top 4 targets in list
                            AddGeorgeButton(3008, 100);
                            AddGeorgeButton(3004, 100);
                            AddGeorgeButton(3008, 100);
                            AddGeorgeButton(3004, 100);
                            AddGeorgeButton(3008, 100);
                            AddGeorgeButton(3004, 100);
                            AddGeorgeButton(3008, 100);
                            AddGeorgeButton(3006);
                            break;
                        case "wMsgGeorgeMacroTrackEngage": //Tracks current target and give engage command if ROE is Weapons Hold
                            AddGeorgeButton(3006, 100);                            
                            AddGeorgeButton(3008, 100);                            
                            break;

                        case "wMsgGeorgeMacroSelectGun":
                            SelectGeorgeWeapon(State.AH64GeorgeWeaponMode.Gun);
                            break;
                        case "wMsgGeorgeMacroSelectMissiles":
                            SelectGeorgeWeapon(State.AH64GeorgeWeaponMode.Missiles);
                            break;
                        case "wMsgGeorgeMacroSelectRockets":
                            SelectGeorgeWeapon(State.AH64GeorgeWeaponMode.Rockets);
                            break;
                        case "wMsgGeorgeMacroSelectNoWeapon":
                            SelectGeorgeWeapon(State.AH64GeorgeWeaponMode.NoWeapon);
                            break;


                    }
                }

                private static void RefreshGeorgeWeaponAvailabilityFromPayloadProbe()
                {
                    try
                    {
                        var payload = State.currentstate != null ? State.currentstate.payload : null;
                        if (payload == null)
                        {
                            return;
                        }

                        State.AH64GeorgeGunAvailable = payload.Cannon != null && payload.Cannon.shells > 0;

                        bool missilesAvailable = false;
                        bool rocketsAvailable = false;

                        if (payload.Stations != null)
                        {
                            foreach (var station in payload.Stations)
                            {
                                if (station == null || string.IsNullOrEmpty(station.CLSID))
                                {
                                    continue;
                                }

                                string clsid = station.CLSID.ToUpperInvariant();
                                bool hasCount = station.count > 0;

                                if (IsGeorgeMissileStation(clsid))
                                {
                                    if (hasCount)
                                    {
                                        missilesAvailable = true;
                                    }
                                }

                                if (IsGeorgeRocketStation(clsid))
                                {
                                    if (hasCount)
                                    {
                                        rocketsAvailable = true;
                                    }
                                }
                            }
                        }

                        State.AH64GeorgeMissilesAvailable = missilesAvailable;
                        State.AH64GeorgeRocketsAvailable = rocketsAvailable;
                        State.AH64GeorgeWeaponStateValid = true;
                    }
                    catch
                    {
                    }
                }

                private static bool IsGeorgeMissileStation(string clsid)
                {
                    if (string.IsNullOrEmpty(clsid))
                    {
                        return false;
                    }

                    if (clsid.Contains("EMPTY") || clsid.Contains("INERT") || clsid.Contains("DUMMY"))
                    {
                        return false;
                    }

                    return clsid.Contains("AGM_114")
                        || clsid.Contains("HELLFIRE")
                        || clsid.Contains("88D18A5E-99C8-4B04-B40B-1C02F2018B6E")
                        || clsid.Contains("M299")
                        || clsid.Contains("M310");
                }

                private static bool IsGeorgeRocketStation(string clsid)
                {
                    return clsid.Contains("HYDRA")
                        || clsid.Contains("M261")
                        || clsid.Contains("M260")
                        || clsid.Contains("FFAR")
                        || clsid.Contains("APKWS")
                        || clsid.Contains("M151")
                        || clsid.Contains("M229");
                }

                private static void ForceGeorgeNoWeaponOnDepletedSelection(bool hadValidWeaponState, bool previousGunAvailable, bool previousMissilesAvailable, bool previousRocketsAvailable)
                {
                    if (!State.AH64GeorgeWeaponStateValid)
                    {
                        return;
                    }

                    var selected = State.AH64GeorgeSelectedWeapon;
                    if (selected == State.AH64GeorgeWeaponMode.Unknown || selected == State.AH64GeorgeWeaponMode.NoWeapon)
                    {
                        return;
                    }

                    if (GeorgeWeaponAvailable(selected))
                    {
                        return;
                    }

                    if (!hadValidWeaponState || !WeaponAvailableFromSnapshot(selected, previousGunAvailable, previousMissilesAvailable, previousRocketsAvailable))
                    {
                        return;
                    }

                    if (selected == State.AH64GeorgeWeaponMode.Missiles && HasGeorgeEmptyM299Stations())
                    {
                        return;
                    }

                    AddGeorgeAction(3005, 1.0, 2000);
                    AddGeorgeAction(3005, 0.0, 80);
                    State.AH64GeorgeSelectedWeapon = State.AH64GeorgeWeaponMode.NoWeapon;
                    if (State.activeconfig.RIO_Messages)
                    {
                        State.currentmessage.dspmsg = "GEORGE ammo sync:\nSelected weapon depleted. Forcing No WPN (de-WAS).";
                        State.currentmessage.msgdur = 5;
                    }
                    Log.Write("AH-64D George ammo sync: " + selected + " depleted, forcing No WPN with 2s delay.", Colors.Warning);
                }

                private static bool HasGeorgeEmptyM299Stations()
                {
                    var payload = State.currentstate != null ? State.currentstate.payload : null;
                    if (payload == null || payload.Stations == null)
                    {
                        return false;
                    }

                    foreach (var station in payload.Stations)
                    {
                        if (station == null || string.IsNullOrEmpty(station.CLSID))
                        {
                            continue;
                        }

                        string clsid = station.CLSID.ToUpperInvariant();
                        if (clsid.Contains("M299_EMPTY"))
                        {
                            return true;
                        }
                    }

                    return false;
                }

                private static bool WeaponAvailableFromSnapshot(State.AH64GeorgeWeaponMode mode, bool gunAvailable, bool missilesAvailable, bool rocketsAvailable)
                {
                    switch (mode)
                    {
                        case State.AH64GeorgeWeaponMode.Gun:
                            return gunAvailable;
                        case State.AH64GeorgeWeaponMode.Missiles:
                            return missilesAvailable;
                        case State.AH64GeorgeWeaponMode.Rockets:
                            return rocketsAvailable;
                        case State.AH64GeorgeWeaponMode.NoWeapon:
                        case State.AH64GeorgeWeaponMode.Unknown:
                        default:
                            return true;
                    }
                }

                private static void SelectGeorgeWeapon(State.AH64GeorgeWeaponMode target)
                {
                    if (!CanChangeGeorgeWeaponSelection())
                    {
                        return;
                    }

                    if (!GeorgeWeaponAvailable(target))
                    {
                        Log.Write("Requested George weapon is not available with current payload.", Colors.Inline);
                        return;
                    }

                    var current = State.AH64GeorgeSelectedWeapon;
                    if (current == State.AH64GeorgeWeaponMode.Unknown)
                    {
                        current = State.AH64GeorgeWeaponMode.NoWeapon;
                    }

                    int steps = GetGeorgeCycleSteps(current, target);
                    for (int i = 0; i < steps; i++)
                    {
                        AddGeorgeButton(3005, 80);
                    }

                    State.AH64GeorgeSelectedWeapon = target;
                }

                private static bool CanChangeGeorgeWeaponSelection()
                {
                    if (State.currentstate != null && !State.currentstate.airborne)
                    {
                        bool changed = ForceGeorgeNoWeaponLocalSync("selection blocked by weight-on-wheels", true);
                        if (State.activeconfig.RIO_Messages)
                        {
                            if (changed)
                            {
                                State.currentmessage.dspmsg = "GEORGE:\nWeapon selection synced to No WPN with Weight on Wheels.";
                            }
                            else
                            {
                                State.currentmessage.dspmsg = "GEORGE:\nWeapon selection not available with Weight on Wheels.";
                            }
                            State.currentmessage.msgdur = 4;
                        }
                        Log.Write("George weapon selection is not available with Weight on Wheels. Command Sent " + changed + "; selected=" + State.AH64GeorgeSelectedWeapon + ".", Colors.Recognition);
                        return false;
                    }

                    return true;
                }

                private static bool ForceGeorgeNoWeaponLocalSync(string reason, bool logWhenAlreadyNoWeapon)
                {
                    var previous = State.AH64GeorgeSelectedWeapon;
                    if (previous != State.AH64GeorgeWeaponMode.NoWeapon)
                    {
                        State.AH64GeorgeSelectedWeapon = State.AH64GeorgeWeaponMode.NoWeapon;
                        return true;
                    }

                    return false;
                }

                private static int GetGeorgeCycleSteps(State.AH64GeorgeWeaponMode from, State.AH64GeorgeWeaponMode to)
                {
                    if (from == to)
                    {
                        return 0;
                    }

                    var order = GetGeorgeCycleOrder();
                    int fromIndex = order.IndexOf(from);
                    int toIndex = order.IndexOf(to);

                    if (fromIndex < 0)
                    {
                        fromIndex = 0;
                    }

                    if (toIndex < 0)
                    {
                        return 0;
                    }

                    if (toIndex >= fromIndex)
                    {
                        return toIndex - fromIndex;
                    }

                    return (order.Count - fromIndex) + toIndex;
                }

                private static State.AH64GeorgeWeaponMode GetNextGeorgeWeapon(State.AH64GeorgeWeaponMode current)
                {
                    var order = GetGeorgeCycleOrder();
                    int idx = order.IndexOf(current);
                    if (idx < 0)
                    {
                        return order[0];
                    }

                    return order[(idx + 1) % order.Count];
                }

                private static List<State.AH64GeorgeWeaponMode> GetGeorgeCycleOrder()
                {
                    var order = new List<State.AH64GeorgeWeaponMode>
                    {
                        State.AH64GeorgeWeaponMode.NoWeapon
                    };

                    if (State.AH64GeorgeGunAvailable)
                    {
                        order.Add(State.AH64GeorgeWeaponMode.Gun);
                    }

                    if (State.AH64GeorgeMissilesAvailable)
                    {
                        order.Add(State.AH64GeorgeWeaponMode.Missiles);
                    }

                    if (State.AH64GeorgeRocketsAvailable)
                    {
                        order.Add(State.AH64GeorgeWeaponMode.Rockets);
                    }

                    return order;
                }

                private static bool GeorgeWeaponAvailable(State.AH64GeorgeWeaponMode mode)
                {
                    if (!State.AH64GeorgeWeaponStateValid)
                    {
                        return true;
                    }

                    switch (mode)
                    {
                        case State.AH64GeorgeWeaponMode.NoWeapon:
                            return true;
                        case State.AH64GeorgeWeaponMode.Gun:
                            return State.AH64GeorgeGunAvailable;
                        case State.AH64GeorgeWeaponMode.Missiles:
                            return State.AH64GeorgeMissilesAvailable;
                        case State.AH64GeorgeWeaponMode.Rockets:
                            return State.AH64GeorgeRocketsAvailable;
                        default:
                            return false;
                    }
                }

                public static void AddGeorgeLongButton(int command)
                {
                    AddGeorgeAction(command, 1.0, 1200);
                    AddGeorgeAction(command, 0.0);
                }

                public static void AddGeorgeLongButton(int command, int postDelayMs)
                {
                    AddGeorgeAction(command, 1.0, 1200);
                    AddGeorgeAction(command, 0.0, postDelayMs);
                }

                public static void AddGeorgeButton(int command)
                {
                    AddGeorgeAction(command, 1.0);
                    AddGeorgeAction(command, 0.0);
                }

                public static void AddGeorgeButton(int command, int postDelayMs)
                {
                    AddGeorgeAction(command, 1.0);
                    AddGeorgeAction(command, 0.0, postDelayMs);
                }

                public static void AddGeorgeAction(int command, double value, int delayMs = 0)
                {
                    State.currentmessage.extsequence.Add(new Extensions.RIO.DeviceAction
                    {
                        device = 87, // George AI
                        command = command,
                        value = value,
                        delayMs = delayMs
                    });
                }

                public static bool ProcessIfWSO()
                {
                    if (State.currentcommand.isWSO())
                    {
                        if (State.wsoactivated && State.IsF4E)
                        {
                            return ConstructWSOMessage();
                        }
                        else
                        {
                            Log.Write("WSO extension is not active or Module is not F-4E.", Colors.Warning);
                            return false;
                        }
                    }
                    else
                    {
                        return true; // Not a WSO command, proceed with other processing
                    }
                }

                public static bool ConstructWSOMessage()
                {
                    State.currentmessage.type = Messagetypes.DeviceControl;

                    string commandKey = State.currentkey.ContainsKey("command") ? State.currentkey["command"] : State.currentcommand?.dcsid;

                    if (!string.IsNullOrEmpty(commandKey) && DatabaseCommands.Table.TryGetValue(commandKey, out var command) &&
                        (command.category == CommandCategories.WSO || (command.uniqueid >= 24000 && command.uniqueid <= 24999)))
                    {
                        // Handle WSO-specific parameters
                        State.currentmessage.parameters = new List<object>
                        {
                            command.dcsid,
                            command.displayname,
                            command.uniqueid
                        };

                        Log.Write($"Constructed WSO message for command: {command.dcsid}", Colors.Text);

                        try
                        {
                            string wsoLog = "WSO | " + command.displayname;
                            OpenKneeboardBridge.UpdateLog("AI CREW", wsoLog);
                            OpenKneeboardBridge.SetLastAiCrewCommand(wsoLog);
                        }
                        catch
                        {
                        }
                    }
                    else
                    {
                        Log.Write($"Failed to construct WSO message. Command not found: {commandKey ?? State.currentcommand?.dcsid}", Colors.Warning);
                        return false;
                    }

                    return true;
                }

                public static void F14Salute()
                {
                    State.currentmessage.type = Messagetypes.DeviceControl;

                    State.currentmessage.extsequence.Add(new Extensions.RIO.DeviceAction());

                    State.currentmessage.extsequence[0].device = 18; // gearhook now 18
                    State.currentmessage.extsequence[0].command = 3023; //CATAPULT_Salute
                    State.currentmessage.extsequence[0].value = 1.0; // press
                }

                public static void constructRIO()
                {
                    State.currentmessage.type = Messagetypes.DeviceControl;

                    if (State.activeconfig.RIO_Messages)
                    {
                        try
                        {
                            if (!State.activeconfig.RIO_Hints_Only)
                            {
                                State.currentmessage.dspmsg = "VAICOM PRO: RIO | " + Database.Labels.aicommands[State.currentkey["command"]];
                            }

                            if (Extensions.RIO.menuhelper.optionhints.ContainsKey(State.currentkey["command"]))
                            {
                                State.currentmessage.dspmsg = "AIRIO command use:\n" + Extensions.RIO.menuhelper.optionhints[State.currentkey["command"]];
                                State.currentmessage.msgdur = 5;

                                if (State.currentkey["command"].Equals("wMsgJ_WPN_AG_SORDN"))
                                {
                                    State.currentmessage.dspmsg = State.currentmessage.dspmsg + "\nCurrent ordnance loadout:";
                                    helper.getAGweaponsstate();
                                    foreach (KeyValuePair<string, Extensions.RIO.DeviceAction> entry in helper.AGweaponsstate)
                                    {
                                        try
                                        {
                                            if (!entry.Value.Equals(DeviceActionsLibrary.RIO.Atom_J_VOID))
                                            {
                                                State.currentmessage.dspmsg = State.currentmessage.dspmsg + " " + entry.Key;
                                            }
                                        }
                                        catch
                                        {
                                        }
                                    }
                                    State.currentmessage.dspmsg = State.currentmessage.dspmsg + ".";
                                }

                                if (State.currentkey["command"].Equals("wMsgJ_RAD_DL_SET_HOST"))
                                {
                                    State.currentmessage.dspmsg = State.currentmessage.dspmsg + "\nCurrently available hosts:";
                                    helper.getDLstate();
                                    foreach (KeyValuePair<string, Extensions.RIO.DeviceAction> entry in helper.DLstate)
                                    {
                                        try
                                        {
                                            if (!entry.Value.Equals(DeviceActionsLibrary.RIO.Atom_J_VOID))
                                            {
                                                State.currentmessage.dspmsg = State.currentmessage.dspmsg + " " + entry.Key;
                                            }
                                        }
                                        catch
                                        {
                                        }

                                    }
                                    State.currentmessage.dspmsg = State.currentmessage.dspmsg + ".";
                                }

                                if (State.currentkey["command"].Equals("wMsgJ_RAD_TCN_TUNE_TAC"))
                                {
                                    State.currentmessage.dspmsg = State.currentmessage.dspmsg + "\nCurrently available TAC units:";
                                    helper.getTACANstate();
                                    foreach (KeyValuePair<string, Extensions.RIO.DeviceAction> entry in helper.TACANstate)
                                    {
                                        try
                                        {
                                            if (!entry.Value.Equals(DeviceActionsLibrary.RIO.Atom_J_VOID))
                                            {
                                                State.currentmessage.dspmsg = State.currentmessage.dspmsg + " " + entry.Key;
                                            }
                                        }
                                        catch
                                        {
                                        }
                                    }
                                    State.currentmessage.dspmsg = State.currentmessage.dspmsg + ".";
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Log.Write("Construct RIO Message:" + e.Message + e.StackTrace, Colors.Warning);

                        }

                    }

                    Message.SetRioDeviceSequence();

                    if (!State.activeconfig.RIO_Messages)
                    {
                        State.currentmessage.dspmsg = null;
                    }

                    try
                    {
                        string rioLog = State.currentmessage.dspmsg;
                        if (string.IsNullOrWhiteSpace(rioLog))
                        {
                            rioLog = "RIO | " + Database.Labels.aicommands[State.currentkey["command"]];
                        }
                        OpenKneeboardBridge.UpdateLog("AI CREW", rioLog);
                        OpenKneeboardBridge.SetLastAiCrewCommand(rioLog);
                    }
                    catch
                    {
                    }
                }

                public static void ConstructMessage()
                {
                    try
                    {
                        Log.Write("Constructing message... ", Colors.Text);

                        State.currentmessage = new CommsMessage();

                        State.currentmessage.debug = State.activeconfig.Debugmode;
                        State.currentmessage.client = State.client;
                        State.currentmessage.mode = State.clientmode;
                        State.currentmessage.tgtdevid = Message.GetSendDeviceId();
                        State.currentmessage.tgtdevname = State.currentradiodevicename;
                        
                        if (State.currentcommand.isWSO())
                        {
                            // Handle WSO-specific message construction
                            ConstructWSOMessage();
                        }
                        else
                        {
                            // Handle other message types
                            State.currentmessage.type = State.currentcommand.ApplicationType();
                            State.currentmessage.command = State.currentcommand.eventnumber != 0
                                ? State.currentcommand.eventnumber
                                : State.currentcommand.geteventnumber();
                            State.currentmessage.dcsid = !string.IsNullOrEmpty(State.currentcommand.dcsid)
                                ? State.currentcommand.dcsid
                                : "wMsgNull";
                        }

                        State.currentmessage.insert = State.currentcommand.RequiresInsert();
                        State.currentmessage.reccat = State.currentrecipientclass.Name;
                        State.currentmessage.selectrecipient = State.currentmessageunit.fullname;
                        State.currentmessage.selectunit = State.currentmessageunit.id_;
                        State.currentmessage.havedial = State.currentmodule.Havedial;
                        State.currentmessage.fc3 = State.currentmodule.IsFC;
                        State.currentmessage.forcetune = !State.currentstate.easycomms && State.activeconfig.AllowRadioTuning && State.currentcommand.isSelect();

                        // Set numeric tuning if required
                        settunenum();

                        // Inserts, parameters, appendices
                        if (State.currentcommand.RequiresFlightNumInsert())
                        {
                            (State.currentmessage.parameters as List<object>)?.Add(State.currentflightrecipientnumber);
                        }
                        if (State.currentcommand.hasparameter) { SetParameters(); }
                        if (State.currentcommand.hasAppendix() & (State.have["apxwpn"] || State.have["apxdir"])) { SetAppendices(); }

                        // Settings
                        State.currentmessage.redirect_world_speech = State.activeconfig.Redirect_World_Speech;
                        State.currentmessage.disableplayervoice = State.activeconfig.DisablePlayerVoice;
                        State.currentmessage.hideonscreentext = State.activeconfig.HideOnScreenText;
                        State.currentmessage.forcelanguage = State.activeconfig.ForceLanguage;
                        State.currentmessage.forcedlanguage = Languages.localization.languages[State.activeconfig.ForcedLanguage];
                        State.currentmessage.menuinvisible = State.activeconfig.HideMenus;
                        State.currentmessage.forcenatoprotocol = State.activeconfig.EnforceATCProtocol && (State.activeconfig.EnforcedATCProtocol == 0);
                        State.currentmessage.forcecallsigns = State.activeconfig.EnforceCallsigns;
                        State.currentmessage.forcedcallsigns = Languages.localization.languages[State.activeconfig.CallsignsLanguage];
                        State.currentmessage.operatedial = State.activeconfig.OperateDial;
                        State.currentmessage.importmenus = State.activeconfig.ImportOtherMenu;

                        State.currentmessage.AIRIO = State.IsAirioTomcatModule();
                        State.currentmessage.carriersuppressauto = State.activeconfig.CarrierSuppressAuto;

                        // SPECIAL: CONSTRUCT MESSAGE FOR GEORGE
                        if (!ProcessIfGeorge())
                        {
                            return;
                        }

                        // SPECIAL: CONSTRUCT MESSAGE FOR AIRIO
                        if (!ProcessIfRIO())
                        {
                            return;
                        }

                        // EXCEPTION: F14 Cat Launch
                        if (State.IsAirioTomcatModule() && State.currentcommand.dcsid.Equals("wMsgLeaderGroundGestureSalut"))
                        {
                            F14Salute();
                        }

                        // Handle WSO-specific commands
                        if (!ProcessIfWSO())
                        {
                            Log.Write("WSO command processing failed.", Colors.Warning);
                            return; // Exit the method
                        }

                        // OPTIONS
                        if (State.currentcommand.isOptions())
                        {
                            Log.Write("Identified as options command", Colors.Inline);
                            if (State.currentkey["recipient"].Equals("RIO") || State.currentkey["recipient"].Equals("Iceman"))
                            {
                                State.showingoptions = false;
                                // Force a fresh wheel-open request even if local state flag got stale.
                                VAICOM.Extensions.RIO.helper.showingjestermenu = false;
                                VAICOM.Extensions.RIO.helper.ShowWheel(true);
                            }
                            else
                            {
                                VAICOM.Extensions.RIO.helper.showingjestermenu = false;
                                State.currentmessage.type = Messagetypes.iCommandSequence;
                                Message.setoptionscmdsequence();
                                State.showingoptions = true;
                            }
                        }

                        // MENU CONTROL
                        if (State.currentcommand.isMenu())
                        {
                            Log.Write("Identified as menu command unique id " + State.currentcommand.uniqueid, Colors.Inline);

                            bool useJesterMenu = Extensions.RIO.helper.showingjestermenu && !State.showingoptions;

                            if (useJesterMenu)
                            {
                                try
                                {
                                    Log.Write("Setting RIO sequence", Colors.Inline);

                                    State.currentmessage.type = Messagetypes.DeviceControl;
                                    State.currentmessage.extsequence = new List<Extensions.RIO.DeviceAction>();

                                    int command = 3725;
                                    int value = -1;
                                    bool jesterCloseSelection = false;

                                    switch (State.currentcommand.uniqueid)
                                    {
                                        case 22501: command = 3551; break; // Vertical up
                                        case 22502: command = 3552; break; // Diagonal 45 up
                                        case 22503: command = 3553; break; // Horizontal
                                        case 22504: command = 3554; break; // Diagonal 135
                                        case 22505: command = 3555; break; // Vertical down
                                        case 22506: command = 3556; break; // Diagonal 45 down
                                        case 22507: command = 3557; break; // Horizontal
                                        case 22508: command = 3558; break; // Diagonal 135
                                        case 22509:
                                        case 22510:
                                        case 22511:
                                        case 22512:
                                            command = 3725;
                                            value = 1;
                                            jesterCloseSelection = true;
                                            break;
                                        default: break;
                                    }

                                    List<Extensions.RIO.DeviceAction> menu = new List<Extensions.RIO.DeviceAction>
                                    {
                                        new Extensions.RIO.DeviceAction
                                        {
                                            device = 62, // JesterAI
                                            command = command,
                                            value = value,
                                        }
                                    };

                                    State.currentmessage.extsequence.AddRange(menu);

                                    if (jesterCloseSelection)
                                    {
                                        State.showingoptions = false;
                                        Extensions.RIO.helper.showingjestermenu = false;
                                    }
                                }
                                catch (Exception e)
                                {
                                    Log.Write("Error setting device sequence: " + e.StackTrace, Colors.Inline);
                                }
                            }
                            else
                            {
                                State.currentmessage.type = Messagetypes.iCommandSequence;
                                Message.SetMenuCmdSequence();
                                State.showingoptions = true;
                            }
                        }

                        // For imported F10 menu commands: change type and add action sequence..
                        if ((State.currentrecipientclass.Equals(Recipientclasses.Aux) && !State.currentrecipientclass.Name.Equals("AOCS")) & !State.currentcommand.isOptions())
                        {
                            State.currentmessage.type = Messagetypes.ActionIndexSequence;
                            Message.SetMenuItemAction();
                        }

                        Log.Write("Message construction completed.", Colors.Text);
                    }
                    catch (Exception e)
                    {
                        Log.Write("Message construction error: " + e.Message + "\n" + e.StackTrace, Colors.Warning);
                    }
                }
            }
        }
    }
}