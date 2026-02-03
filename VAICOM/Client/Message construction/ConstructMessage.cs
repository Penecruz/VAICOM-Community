using System;
using System.Collections.Generic;
using VAICOM.Database;
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
                        if (State.AIRIOactive && State.activeconfig.ICShotmic && 
                            !(State.activeconfig.MP_VoIPUseSwitch && State.activeconfig.MP_DelayTransmit))
                        {
                            // hotmic is active, RIO not called
                            if (!State.valistening) // hotmic was used
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

                public static bool ProcessIfWSO()
                {
                    if (State.currentcommand.isWSO())
                    {
                        if (State.wsoactivated && State.IsF4E)
                        {
                            ConstructWSOMessage();
                            return true;
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

                public static void ConstructWSOMessage()
                {
                    State.currentmessage.type = Messagetypes.DeviceControl;

                    if (DatabaseCommands.Table.TryGetValue(State.currentcommand.name, out var command) &&
                        (command.category == CommandCategories.WSO || (command.uniqueid >= 24000 && command.uniqueid <= 24999)))
                    {
                        // Handle WSO-specific parameters
                        State.currentmessage.parameters = new List<object>
                        {
                            command.name,
                            command.displayname,
                            command.uniqueid
                        };

                        Log.Write($"Constructed WSO message for command: {command.name}", Colors.Text);
                    }
                    else
                    {
                        Log.Write($"Failed to construct WSO message. Command not found: {State.currentcommand.name}", Colors.Warning);
                    }
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

                public static void ConstructMessage()
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

                        State.currentmessage.AIRIO = State.currentmodule.Equals(Products.DCSmodules.LookupTable[State.riomod]);
                        State.currentmessage.carriersuppressauto = State.activeconfig.CarrierSuppressAuto;

                        // SPECIAL: Construct message for AIRIO
                        if (!ProcessIfRIO())
                        {
                            Log.Write("AIRIO command not processed due to licensing or configuration.", Colors.Warning);
                            return; // Exit the method
                        }

                        // EXCEPTION: F14 Cat Launch
                        if (State.currentmodule.Equals(Products.DCSmodules.LookupTable["F-14AB"]) && State.currentcommand.dcsid.Equals("wMsgLeaderGroundGestureSalut"))
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
                        if (State.currentcommand.isMenu())
                        {
                            Log.Write("Identified as menu command unique id " + State.currentcommand.uniqueid, Colors.Inline);

                            if (Extensions.RIO.helper.showingjestermenu)
                            {
                                try
                                {
                                    Log.Write("Setting RIO sequence", Colors.Inline);

                                    State.currentmessage.type = Messagetypes.DeviceControl;
                                    State.currentmessage.extsequence = new List<Extensions.RIO.DeviceAction>();

                                    int command = 3550;
                                    int value = -1;

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