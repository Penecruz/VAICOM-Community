using System;
using System.Collections.Generic;
using VAICOM.Database;
using VAICOM.Extensions.RIO;
using VAICOM.PushToTalk;
using VAICOM.Static;


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

                    State.currentmessage.tunenum = (State.currentstate.easycomms && State.currentcommand.isState()) || (State.currentrecipientclass.Name.Equals("AOCS") && (State.currentstate.easycomms || (State.currentcommand.isSelect() && State.activeconfig.AllowRadioTuning)));

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
                        if ((State.currentmodule.Singlehotkey & !State.activeconfig.ForceMultiHotkey) || (!State.currentmodule.Singlehotkey & State.activeconfig.ForceSingleHotkey))
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
                        if (State.jesteractivated && State.dll_installed_rio && State.activeconfig.RIO_Enabled)
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
                        if (State.AIRIOactive && State.activeconfig.ICShotmic && !(State.activeconfig.MP_VoIPUseSwitch && State.activeconfig.MP_DelayTransmit))
                        {
                            // hotmic is active, RIO not called

                            if (!State.valistening) //hotmic was used
                            {
                                if (!State.currentrecipientclass.Equals(Recipientclasses.Crew))
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
                    return clsid.Contains("AGM_114")
                        || clsid.Contains("HELLFIRE")
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
                            State.currentmessage.dspmsg = "GEORGE command blocked:\nWeapon selection is unavailable with Weight on Wheels.";
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
                }

                public static bool ConstructMessage()
                {

                    try
                    {
                        Log.Write("Constructing message... ", Colors.Text);

                        State.currentmessage = new CommsMessage();

                        State.currentmessage.debug = State.activeconfig.Debugmode;
                        State.currentmessage.client = State.currentlicense;
                        State.currentmessage.mode = State.clientmode;
                        State.currentmessage.tgtdevid = Message.GetSendDeviceId();
                        State.currentmessage.tgtdevname = State.currentradiodevicename;
                        State.currentmessage.type = State.currentcommand.ApplicationType();
                        State.currentmessage.command = State.currentcommand.eventnumber != 0 ? State.currentcommand.eventnumber : State.currentcommand.geteventnumber();
                        State.currentmessage.dcsid = State.currentcommand.dcsid != "" ? State.currentcommand.dcsid : "wMsgNull";
                        State.currentmessage.insert = State.currentcommand.RequiresInsert();
                        State.currentmessage.reccat = State.currentrecipientclass.Name;
                        State.currentmessage.selectrecipient = State.currentmessageunit.fullname;
                        State.currentmessage.selectunit = State.currentmessageunit.id_;
                        State.currentmessage.havedial = State.currentmodule.Havedial;
                        State.currentmessage.fc3 = State.currentmodule.IsFC;
                        State.currentmessage.forcetune = !State.currentstate.easycomms && State.activeconfig.AllowRadioTuning && State.currentcommand.isSelect();

                        // set numeric tuning if required..
                        settunenum();

                        // inserts, parameters, appendices..
                        // Flight and Wingmen: Cast parameters to List<object> before calling Add
                        if (State.currentcommand.RequiresFlightNumInsert())
                        {
                            (State.currentmessage.parameters as List<object>)?.Add(State.currentflightrecipientnumber);
                        }
                        if (State.currentcommand.hasparameter) { SetParameters(); }
                        if (State.currentcommand.hasAppendix() & (State.have["apxwpn"] || State.have["apxdir"])) { SetAppendices(); }

                        // settings..
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

                        State.currentmessage.AIRIO = State.currentmodule.Equals(Products.DCSmodules.LookupTable[State.riomod]);
                        State.currentmessage.carriersuppressauto = State.activeconfig.CarrierSuppressAuto;

                        // SPECIAL: CONSTRUCT MESSAGE FOR GEORGE
                        if (!ProcessIfGeorge())
                        {
                            return false;
                        }

                        // SPECIAL: CONSTRUCT MESSAGE FOR AIRIO
                        if (!ProcessIfRIO())
                        {
                            return false; // tried AIRIO cmd but not licensed
                        }

                        // ------------------------------------------------------------------------

                        // EXCEPTION: F14 CATLAUNCH
                        // for 14 cat launch
                        if (State.currentmodule.Equals(Products.DCSmodules.LookupTable["F-14AB"]) && State.currentcommand.dcsid.Equals("wMsgLeaderGroundGestureSalut"))
                        {
                            F14Salute();
                        }

                        // OPTIONS
                        // for options/menu commands: change type and add keysequence..
                        bool airioflightcrew = State.currentkey["recipient"].Equals("RIO") || State.currentkey["recipient"].Equals("Iceman");
                        if (State.currentcommand.isOptions())
                        {
                            Log.Write("Identified as options command", Colors.Inline);
                            if (airioflightcrew)
                            {
                                VAICOM.Extensions.RIO.helper.ShowWheel(true);
                            }
                            else
                            {

                                State.currentmessage.type = Messagetypes.iCommandSequence;
                                Message.setoptionscmdsequence();
                                State.showingoptions = true;
                            }
                        }

                        // MENU CONTROL
                        // for options/menu commands: change type and add keysequence..
                        if (State.currentcommand.isMenu())
                        {
                            Log.Write("Identified as menu command unique id " + State.currentcommand.uniqueid, Colors.Inline);

                            if (Extensions.RIO.helper.showingjestermenu)
                            {
                                try
                                {

                                    Log.Write("Setting rio sequence", Colors.Inline);

                                    // then rio sequence
                                    //State.currentmessage = new DcsClient.Message.CommsMessage();
                                    State.currentmessage.type = Messagetypes.DeviceControl;
                                    State.currentmessage.extsequence = new List<Extensions.RIO.DeviceAction>();

                                    int command = 3550;
                                    int value = -1;

                                    switch (State.currentcommand.uniqueid)
                                    {
                                        case 22501: //menu1
                                            command = 3551; // vertical up
                                            break;
                                        case 22502: //menu2
                                            command = 3552; //diag45 up
                                            break;
                                        case 22503: //menu3
                                            command = 3553; //h0riz                    
                                            break;
                                        case 22504: //menu4
                                            command = 3554; //diag135                           
                                            break;
                                        case 22505: //menu5
                                            command = 3555; // vertical down                                    
                                            break;
                                        case 22506: //menu5
                                            command = 3556; //diag45  down                               
                                            break;
                                        case 22507: //menu7
                                            command = 3557; //h0riz                               
                                            break;
                                        case 22508: //menu8
                                            command = 3558; //diag135                                   
                                            break;
                                        default:
                                            break;
                                    }


                                    List<Extensions.RIO.DeviceAction> menu = new List<Extensions.RIO.DeviceAction>();
                                    Extensions.RIO.DeviceAction menuaction = new Extensions.RIO.DeviceAction()
                                    {
                                        device = 62, // JesterAI
                                        command = command,
                                        value = value,
                                    };
                                    menu.Add(menuaction);

                                    State.currentmessage.extsequence.AddRange(menu);

                                }
                                catch (Exception e)
                                {
                                    Log.Write("Error setting dev sequence: " + e.StackTrace, Colors.Inline);
                                }

                            }
                            else // not jester menu, i..e normal comms menu
                            {
                                State.currentmessage.type = Messagetypes.iCommandSequence;
                                Message.SetMenuCmdSequence();
                                State.showingoptions = true;
                            }
                        }
                        // for imported F10 menu commands: change type and add action sequence..
                        if ((State.currentrecipientclass.Equals(Recipientclasses.Aux) && !State.currentrecipientclass.Name.Equals("AOCS")) & !State.currentcommand.isOptions())
                        {
                            State.currentmessage.type = Messagetypes.ActionIndexSequence;
                            Message.SetMenuItemAction();
                        }
                        // ------------------------------------------------------------------------

                        Log.Write("Done. ", Colors.Text);
                        return true;
                    }
                    catch (Exception e)
                    {
                        Log.Write("message construction error:" + "\n" + e.StackTrace + "\n" + e.InnerException, Colors.Inline);
                        return false;
                    }
                }
            }
        }
    }
}