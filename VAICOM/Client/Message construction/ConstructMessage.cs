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
                            returndeviceid = PTT.RadioDevices.SEL.deviceid; // = 0 or deviceid from currently selected radio
                            State.currentradiodevicename = PTT.RadioDevices.SEL.name;
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