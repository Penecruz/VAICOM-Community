using System;
using System.Collections.Generic;
using VAICOM.Database;
using VAICOM.Extensions.AICPG;
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
                                if (!State.currentrecipientclass.Equals(Recipientclasses.Crew)
                                    && !State.currentcommand.isMenu()
                                    && !State.currentcommand.isOptions()
                                    && !State.currentcommand.isGeorge())
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
                    if (!State.currentcommand.isGeorge())
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

                    // Check that we are issuing commands from the correct seat for the George command category.
                    bool isPilotSeat = Helpers.Common.IsAH64PilotSeatActive();
                    if (State.currentcommand.category.Equals(CommandCategories.AH64D_George_PLT) && isPilotSeat)
                    {
                        State.currentmessage.dspmsg = "VAICOM | GEORGE: You are in the pilot seat!\n";
                        State.currentmessage.msgdur = 5;
                        return;
                    }
                    else if (State.currentcommand.category.Equals(CommandCategories.AH64D_George_CPG) && !isPilotSeat)
                    {
                        State.currentmessage.dspmsg = "VAICOM | GEORGE: You are in the CP/G seat!\n";
                        State.currentmessage.msgdur = 5;
                        return;
                    }

                    AH64GeorgeState.UpdateWeaponState();

                    CPGCommandHandler.ProcessCommand(State.currentcommand.dcsid);

                    if (State.activeconfig.RIO_Messages)
                    {
                        if (!State.activeconfig.RIO_Hints_Only)
                        {
                            State.currentmessage.dspmsg = "VAICOM: GEORGE | " + Database.Labels.aicommands[State.currentkey["command"]];
                            State.currentmessage.msgdur = 3;
                        }

                        if (georgeoptionhints.TryGetValue(State.currentkey["command"], out string contextualHint))
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
                        OpenKneeboardBridge.SetLastAiCrewCommand(georgeLog);
                    }
                    catch
                    {
                    }
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

                            string contextualHint;
                            if (Extensions.RIO.menuhelper.TryGetOptionHint(State.currentkey["command"], out contextualHint))
                            {
                                State.currentmessage.dspmsg = "AIRIO command use:\n" + contextualHint;
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