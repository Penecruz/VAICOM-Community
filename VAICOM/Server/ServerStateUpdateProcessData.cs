using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using VAICOM.Extensions.AOCS;
using VAICOM.Extensions.RIO;
using VAICOM.PushToTalk;
using VAICOM.Static;



namespace VAICOM
{
    namespace Servers
    {

        public static partial class Server
        {

            public static bool InvalidUnitForTuning(Server.DcsUnit unit)
            {
                bool invalid = false;

                bool ticonderoga = unit.fullname.ToLower().Contains("ticonderoga") || unit.callsign.ToLower().Contains("ticonderoga");
                bool tarawa = unit.fullname.ToLower().Contains("tarawa") || unit.callsign.ToLower().Contains("tarawa");
                bool burke = unit.fullname.ToLower().Contains("burke") || unit.callsign.ToLower().Contains("burke");
                bool perry = unit.fullname.ToLower().Contains("perry") || unit.callsign.ToLower().Contains("perry");

                bool fixedwing = State.currentstate.playerunitcat.ToLower().Equals("planes") & !State.currentstate.id.Equals("AV8BNA");

                invalid = fixedwing && (ticonderoga || tarawa || burke || perry);

                return invalid;
            }


            public static void FixBadNamingAndRemove()
            {
                try
                {
                    foreach (DcsUnit unit in State.currentstate.availablerecipients["ATC"])
                    {
                        if (unit.callsign.Equals("unknown"))
                        {
                            unit.callsign = unit.fullname;
                        }
                    }
                }
                catch
                {
                }

                //remove
                try
                {
                    List<DcsUnit> ATCs = new List<DcsUnit>();
                    ATCs = State.currentstate.availablerecipients["ATC"];

                    foreach (DcsUnit unit in ATCs.ToArray())
                    {
                        if (InvalidUnitForTuning(unit))
                        {
                            unit.allowtuning = false;
                            State.currentstate.availablerecipients["ATC"].Remove(unit);
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Write("Could not remove unit " + e.Message, Colors.Inline);
                }

            }

            public static void CreateListTACAN()
            {
                try
                {
                    List<DcsUnit> TACANunits = new List<DcsUnit>();

                    foreach (DcsUnit unit in State.currentstate.availablerecipients["Tanker"])
                    {
                        if (unit.callsign.Contains("Arco") || unit.fullname.Contains("Arco"))
                        {
                            TACANunits.Add(unit);
                        }
                        if (unit.callsign.Contains("Shell") || unit.fullname.Contains("Shell"))
                        {
                            TACANunits.Add(unit);
                        }
                        if (unit.callsign.Contains("Texaco") || unit.fullname.Contains("Texaco"))
                        {
                            TACANunits.Add(unit);
                        }
                        if (unit.callsign.Contains("Navy One") || unit.fullname.Contains("Navy One")) //Add New S3 Tanker Callsigns
                        {
                            TACANunits.Add(unit);
                        }
                        if (unit.callsign.Contains("Bloodhound") || unit.fullname.Contains("Bloodhound"))
                        {
                            TACANunits.Add(unit);
                        }
                        if (unit.callsign.Contains("Mauler") || unit.fullname.Contains("Mauler"))
                        {
                            TACANunits.Add(unit);
                        }
                    }
                    foreach (DcsUnit unit in State.currentstate.availablerecipients["ATC"])
                    {

                        if (CheckSuperCarrier(unit.callsign + unit.fullname) && !(unit.callsign + unit.fullname).ToLower().Contains("kuznetsov") && !(unit.callsign + unit.fullname).ToLower().Contains("vinson"))
                        {
                            TACANunits.Add(unit);
                        }

                        if (unit.callsign.Contains("Tarawa") || unit.fullname.Contains("Tarawa"))
                        {
                            TACANunits.Add(unit);
                        }

                    }
                    State.currentstate.TACANunits = TACANunits.OrderBy(o => o.range).ToList();
                    helper.getTACANstate();
                }
                catch
                {
                    Log.Write("Could not update TACAN state", Colors.Inline);
                }
            }

            public static void CreateListDL()
            {
                try
                {
                    List<DcsUnit> awacsUnits = new List<DcsUnit>();
                    List<DcsUnit> supercarrierUnits = new List<DcsUnit>();
                    List<DcsUnit> escortShipUnits = new List<DcsUnit>();

                    Func<DcsUnit, bool> isAwacsLikeUnit = unit =>
                    {
                        if (unit == null)
                        {
                            return false;
                        }

                        string unitName = (unit.callsign ?? string.Empty) + " " + (unit.fullname ?? string.Empty);
                        string typeName = unit.typename ?? string.Empty;

                        bool isKnownAwacsCallsign = unitName.IndexOf("Darkstar", StringComparison.OrdinalIgnoreCase) >= 0
                            || unitName.IndexOf("Focus", StringComparison.OrdinalIgnoreCase) >= 0
                            || unitName.IndexOf("Magic", StringComparison.OrdinalIgnoreCase) >= 0
                            || unitName.IndexOf("Overlord", StringComparison.OrdinalIgnoreCase) >= 0
                            || unitName.IndexOf("Wizard", StringComparison.OrdinalIgnoreCase) >= 0;

                        bool isHawkeyeType = typeName.IndexOf("E-2D", StringComparison.OrdinalIgnoreCase) >= 0
                            || typeName.IndexOf("E-2C", StringComparison.OrdinalIgnoreCase) >= 0
                            || typeName.IndexOf("Hawkeye", StringComparison.OrdinalIgnoreCase) >= 0;

                        bool isSentryType = typeName.IndexOf("E-3A", StringComparison.OrdinalIgnoreCase) >= 0
                            || typeName.IndexOf("E-3", StringComparison.OrdinalIgnoreCase) >= 0
                            || typeName.IndexOf("Sentry", StringComparison.OrdinalIgnoreCase) >= 0;

                        bool isWedgetailType = typeName.IndexOf("E-7A", StringComparison.OrdinalIgnoreCase) >= 0
                            || typeName.IndexOf("E-7", StringComparison.OrdinalIgnoreCase) >= 0
                            || typeName.IndexOf("Wedgetail", StringComparison.OrdinalIgnoreCase) >= 0;

                        return isKnownAwacsCallsign || isHawkeyeType || isSentryType || isWedgetailType;
                    };

                    Action<DcsUnit> addUniqueAwacs = unit =>
                    {
                        if (unit == null)
                        {
                            return;
                        }

                        bool exists = awacsUnits.Any(u => u.id_ == unit.id_);
                        if (!exists)
                        {
                            awacsUnits.Add(unit);
                        }
                    };

                    foreach (DcsUnit unit in State.currentstate.availablerecipients["AWACS"])
                    {
                        if (isAwacsLikeUnit(unit))
                        {
                            addUniqueAwacs(unit);
                        }
                    }

                    foreach (KeyValuePair<string, List<DcsUnit>> recipientCategory in State.currentstate.availablerecipients)
                    {
                        if (recipientCategory.Key.Equals("AWACS", StringComparison.OrdinalIgnoreCase)
                            || recipientCategory.Key.Equals("Opposition", StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }

                        foreach (DcsUnit unit in recipientCategory.Value)
                        {
                            if (isAwacsLikeUnit(unit))
                            {
                                addUniqueAwacs(unit);
                            }
                        }
                    }

                    foreach (DcsUnit unit in State.currentstate.availablerecipients["ATC"])
                    {
                        string unitName = (unit.callsign ?? string.Empty) + " " + (unit.fullname ?? string.Empty);
                        string lowerName = unitName.ToLower();

                        if (CheckSuperCarrier(unitName) && !lowerName.Contains("kuznetsov") && !lowerName.Contains("vinson"))
                        {
                            supercarrierUnits.Add(unit);
                        }
                        else if (lowerName.Contains("ticonderoga") || lowerName.Contains("arleigh burke"))
                        {
                            escortShipUnits.Add(unit);
                        }

                    }

                    State.currentstate.DLunits = awacsUnits.OrderBy(o => o.range)
                        .Concat(supercarrierUnits.OrderBy(o => o.range))
                        .Concat(escortShipUnits.OrderBy(o => o.range))
                        .ToList();

                    helper.getDLstate();
                }
                catch
                {
                    Log.Write("Could not update DL state", Colors.Inline);
                }
            }

            public static void GetAuxMenu()
            {
                if (!State.activeconfig.ImportOtherMenu)
                {
                    return;
                }

                if (State.currentstate.menuaux == null)
                {
                    Log.Write("No F10 menu data available to process. Ensure the module is connected and try again.", Colors.Text);
                    return;
                }

                try
                {
                    Log.Write("Processing F10 menu items...", Colors.Text);
                    ImportAuxMenu(); // Always process the menu
                }
                catch (Exception ex)
                {
                    Log.Write($"There was a problem importing F10 menu items: {ex.Message}", Colors.Text);
                }
            }

            public static void GUI_Update()
            {
                try
                {
                    if (State.configwindowopen && (State.configurationwindow != null))
                    {
                        State.configurationwindow.Dispatcher.BeginInvoke((MethodInvoker)delegate
                        {
                            State.configurationwindow.UpdateAllbugs();
                        });
                    }
                }
                catch
                {
                }
            }

            public static bool tunedforAOCS;


            public static void ProcessServerData()
            {
                State.deepdebugmode = State.clientmode.Equals(ClientModes.Debug) || State.currentstate.playerusername.Equals(State.debuguser);

                if (State.currentstate.playerusername.Equals(State.debuguser))
                {
                    State.clientmode = ClientModes.Debug;
                }
                else
                {
                    State.clientmode = ClientModes.Normal;
                }

                if (State.configwindowopen && (State.configurationwindow != null))
                {
                    State.configurationwindow.Dispatcher.BeginInvoke((MethodInvoker)delegate
                    {
                        State.configurationwindow.DebugViewState();
                    });
                }

                State.dcsrunning = true;

                State.oneradioactive = AtLeastOneRadioCount();
                State.beaconlocked = State.oneradioactive;

                // Validate module first
                ValidateDcsModule(true); // true = silent
                if (!State.moduleConnected)
                {
                    Log.Write("Module validation failed. Unable to connect to module.", Colors.Warning);
                    return; // Exit early if module validation fails
                }

                ForceAH64GeorgeNoWeaponWhenOnGround();

                // PTT configuration and activate AIRIO if conditions are met
                PTT.PTT_ApplyNewConfig();
                State.AIRIOactive = State.dll_installed_rio && 
                                    State.activeconfig.RIO_Enabled && 
                                    State.IsAirioTomcatModule();

                if (DetectNewMission())
                {
                    Log.Write("New mission detected. Initializing...", Colors.Text);
                    InitNewMission();
                }

                AOCSProvider.AddAOCSUnit();

                if (State.AIRIOactive)
                {
                    try
                    {
                        Client.DcsClient.UpdateRIOState();
                        CreateListTACAN();
                        CreateListDL();
                    }
                    catch (Exception ex)
                    {
                        Log.Write($"Failed to update AIRIO state: {ex.Message}", Colors.Warning);
                    }
                }

                if (!State.AIRIOactive
                    && State.currentmodule != null
                    && (State.currentmodule.Id.Equals("F-4E-45MC", StringComparison.OrdinalIgnoreCase)
                        || State.currentmodule.Id.Equals("AH-64D", StringComparison.OrdinalIgnoreCase))
                    && State.activeconfig.ICShotmic_useswitch)
                {
                    bool hotmic = State.IsF4EIntercomSelected();
                    bool previous = State.activeconfig.ICShotmic;
                    State.activeconfig.ICShotmic = hotmic;

                    if (previous != hotmic)
                    {
                        double pilotIcsArg = State.currentstate.riostate != null ? State.currentstate.riostate.f4ePilotIcs : 0;
                        Log.Write($"{State.currentmodule.Id} ICS switch: hotmic={hotmic}, releaseHot={State.activeconfig.ReleaseHot}, intercomDevice={State.currentstate.intercom}, pilotIcsArg={pilotIcsArg:0.000}", Colors.Inline);
                        PushToTalk.PTT.PTT_Manage_Listen_VA(State.activeconfig.ReleaseHot || hotmic);
                        PushToTalk.PTT.PTT_Manage_Listen_VAICOM(!State.activeconfig.ReleaseHot || hotmic);
                    }

                    if (State.configwindowopen
                        && (State.configurationwindow != null))
                    {
                        State.configurationwindow.Dispatcher.BeginInvoke((MethodInvoker)delegate
                        {
                            State.configurationwindow.CheckBoxHotMic();
                            State.configurationwindow.Dictate_set_relhot_Light(State.activeconfig.ICShotmic);
                        });
                    }
                }

                FixBadNamingAndRemove();

                Client.DcsClient.Assign_Tuned_Units_to_Radios();

                State.Stopwatch.Restart();

                if (State.transmitting)
                {
                    List<Server.DcsUnit> tunedunits = Client.DcsClient.GetTunedUnitsForTX(State.currentTXnode, false);
                    Client.DcsClient.ShowTunedUnitsForTX(State.currentTXnode);
                    tunedforAOCS = State.currentstate.easycomms || State.currentmodule.IsFC || tunedunits.Contains(State.currentstate.availablerecipients["Aux"].Find(Client.DcsClient.Message.IsAOCS));
                    PTT.PTT_GUI_Update(State.currentTXnode, true);
                    PTT.PTT_Manage_Listen_States_OnUpdate(State.currentTXnode);
                }
                else
                {
                    PTT.PTT_Manage_Listen_States_OnPressRelease(false, false);
                }

                // Process F10 menu after module validation
                EnsureModuleConnectedAndProcessF10Menu();

                VAICOM.Interfaces.VA_Plugin.VA_ExposeVariables(State.Proxy);

                State.Stopwatch.Stop();
            }

            private static void ForceAH64GeorgeNoWeaponWhenOnGround()
            {
                try
                {
                    string moduleId = State.currentmodule != null ? (State.currentmodule.Id ?? string.Empty) : string.Empty;
                    string stateId = State.currentstate != null ? (State.currentstate.id ?? string.Empty) : string.Empty;
                    bool isAH64 = moduleId.IndexOf("AH-64D", StringComparison.OrdinalIgnoreCase) >= 0
                                  || stateId.IndexOf("AH-64D", StringComparison.OrdinalIgnoreCase) >= 0;

                    if (!isAH64 || State.currentstate == null || State.currentstate.airborne)
                    {
                        if (isAH64)
                        {
                            State.AH64GeorgeWowFromServerState = false;
                        }
                        return;
                    }

                    State.AH64GeorgeWowFromServerState = true;

                    if (State.AH64GeorgeSelectedWeapon != State.AH64GeorgeWeaponMode.NoWeapon)
                    {
                        var previous = State.AH64GeorgeSelectedWeapon;
                        State.AH64GeorgeSelectedWeapon = State.AH64GeorgeWeaponMode.NoWeapon;
                        Log.Write("AH-64D ground sync: forced local George state " + previous + " -> NoWeapon (WOW).", Colors.Warning);
                    }
                }
                catch (Exception ex)
                {
                    Log.Write("AH-64D ground sync check failed: " + ex.Message, Colors.Warning);
                }
            }

            private static void EnsureModuleConnectedAndProcessF10Menu()
            {
                if (State.moduleConnected)
                {
                    GetAuxMenu();
                }
                else
                {
                    Log.Write("Module not connected yet. Retrying F10 menu processing in 5 seconds...", Colors.Warning);
                    System.Threading.Tasks.Task.Delay(5000).ContinueWith(_ =>
                    {
                        ValidateDcsModule(true); // Revalidate module connection
                        State.moduleConnected = true; // Update connection state
                        EnsureModuleConnectedAndProcessF10Menu(); // Retry F10 menu processing
                    });
                }
            }

        }
    }
}

