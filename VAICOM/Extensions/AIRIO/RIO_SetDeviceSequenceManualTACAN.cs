using System;
using System.Collections.Generic;
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

                public static void setdefaultmessageparams()
                {


                    State.currentmessage.debug = State.activeconfig.Debugmode;
                    State.currentmessage.client = State.client;
                    State.currentmessage.mode = State.clientmode;

                    try
                    {
                        State.currentmessage.type = State.currentcommand.ApplicationType();
                        State.currentmessage.tgtdevid = Message.GetSendDeviceId();
                        State.currentmessage.tgtdevname = State.currentradiodevicename;
                        State.currentmessage.command = State.currentcommand.eventnumber != 0 ? State.currentcommand.eventnumber : State.currentcommand.geteventnumber();
                        State.currentmessage.reccat = State.currentrecipientclass.Name;
                        State.currentmessage.insert = State.currentcommand.RequiresInsert();
                        State.currentmessage.selectrecipient = State.currentmessageunit.fullname;
                        State.currentmessage.selectunit = State.currentmessageunit.id_;
                        State.currentmessage.havedial = State.currentmodule.Havedial;
                        State.currentmessage.forcetune = !State.currentstate.easycomms && State.activeconfig.AllowRadioTuning && State.currentcommand.isSelect();

                        // set preferences..
                        State.currentmessage.redirect_world_speech = State.activeconfig.Redirect_World_Speech;
                        State.currentmessage.disableplayervoice = State.activeconfig.DisablePlayerVoice;
                        State.currentmessage.menuinvisible = State.activeconfig.HideMenus;
                        State.currentmessage.forcenatoprotocol = State.activeconfig.EnforceATCProtocol;
                        State.currentmessage.forcecallsigns = State.activeconfig.EnforceCallsigns;
                        State.currentmessage.operatedial = State.activeconfig.OperateDial;
                    }
                    catch
                    {
                    }

                    State.currentmessage.AIRIO = State.IsAirioTomcatModule();

                }

                public static void SetRioDeviceSequence_TACAN_Tuning()
                {
                    try
                    {

                        // exit if AIRIO not valid
                        if (!State.dll_installed_rio || !State.activeconfig.RIO_Enabled || !State.IsAirioTomcatModule())
                        {
                            Log.Write("AIRIO commands are not available at this time.", Colors.Warning);
                            UI.Playsound.Recipientna();
                            return;
                        }

                        // else continue
                        State.currentmessage = new CommsMessage();
                        setdefaultmessageparams();
                        State.currentmessage.type = Messagetypes.DeviceControl;
                        State.currentmessage.extsequence = new List<Extensions.RIO.DeviceAction>();

                        string header = State.Proxy.Utility.ParseTokens("{CMDSEGMENT:0}");

                        // band {CMDSEGMENT:1}
                        string band = State.Proxy.Utility.ParseTokens("{CMDSEGMENT:1}");

                        switch (band)
                        {
                            case "x-ray":
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_TCN_X);
                                break;
                            case "yankee":
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_TCN_Y);
                                break;
                        }

                        // major {CMDSEGMENT:2}

                        int majval1;
                        Int32.TryParse(State.Proxy.Utility.ParseTokens("{CMDSEGMENT:2}"), out majval1);
                        int majval2;
                        Int32.TryParse(State.Proxy.Utility.ParseTokens("{CMDSEGMENT:3}"), out majval2);

                        int majval = (10 * majval1) + majval2;

                        switch (majval)
                        {
                            case 0:
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_TCN_MAJ_00);
                                break;
                            case 1:
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_TCN_MAJ_01);
                                break;
                            case 2:
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_TCN_MAJ_02);
                                break;
                            case 3:
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_TCN_MAJ_03);
                                break;
                            case 4:
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_TCN_MAJ_04);
                                break;
                            case 5:
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_TCN_MAJ_05);
                                break;
                            case 6:
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_TCN_MAJ_06);
                                break;
                            case 7:
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_TCN_MAJ_07);
                                break;
                            case 8:
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_TCN_MAJ_08);
                                break;
                            case 9:
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_TCN_MAJ_09);
                                break;
                            case 10:
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_TCN_MAJ_10);
                                break;
                            case 11:
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_TCN_MAJ_11);
                                break;
                            case 12:
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_TCN_MAJ_12);
                                break;
                        }

                        // minor {CMDSEGMENT:3}
                        int minval;
                        Int32.TryParse(State.Proxy.Utility.ParseTokens("{CMDSEGMENT:4}"), out minval);

                        switch (minval)
                        {
                            case 0:
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_TCN_MIN_0);
                                break;
                            case 1:
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_TCN_MIN_1);
                                break;
                            case 2:
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_TCN_MIN_2);
                                break;
                            case 3:
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_TCN_MIN_3);
                                break;
                            case 4:
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_TCN_MIN_4);
                                break;
                            case 5:
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_TCN_MIN_5);
                                break;
                            case 6:
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_TCN_MIN_6);
                                break;
                            case 7:
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_TCN_MIN_7);
                                break;
                            case 8:
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_TCN_MIN_8);
                                break;
                            case 9:
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_TCN_MIN_9);
                                break;
                        }


                        string message = majval.ToString() + minval.ToString() + band.ToCharArray()[0].ToString().ToUpper();

                        if (State.activeconfig.RIO_Messages && !State.activeconfig.RIO_Hints_Only)
                        {
                            State.currentmessage.dspmsg = "AIRIO : " + "TACAN Tune " + message;
                            State.currentmessage.msgdur = 5;
                        }

                        UI.Playsound.Commandcomplete();

                        if (!State.clientmode.Equals(ClientModes.Debug) && tables.menustate[tables.menucats.PLAYERSEAT].Equals(tables.menustates.RIO))
                        {
                            State.currentmessage.dspmsg = "AIRIO : You are in Jester's seat!\n";
                            State.currentmessage.msgdur = 5;
                            State.currentmessage.extsequence = new List<Extensions.RIO.DeviceAction>(); // empty
                        }
                        else // ok, in pilot seat
                        {
                            int combinedvalue = ((10 * majval) + minval);
                            int devicemaxvalue = 129;
                            if (combinedvalue > devicemaxvalue)
                            {
                                State.currentmessage.dspmsg = "AIRIO : TACAN command out of range.\n";
                                State.currentmessage.msgdur = 5;
                                State.currentmessage.extsequence = new List<Extensions.RIO.DeviceAction>(); // empty
                                riospeech.riospeakrandom(2); //negative
                            }
                            else // not out of range
                            {
                                riospeech.riospeakrandom(1); //ok
                            }
                        }

                        SendNewMessage();

                        // write message to log 
                        // for single:
                        if ((State.currentmodule.Singlehotkey & !State.activeconfig.ForceMultiHotkey) || (!State.currentmodule.Singlehotkey & State.activeconfig.ForceSingleHotkey)) // for single mode
                        {
                            Log.Write(State.currentTXnode.name + " | " + PTT.RadioDevices.SEL.name + ": [ " + "RIO" + " ],[ " + " ],[ " + " ] " + "TACAN Tune " + message + " [ " + " ] [ " + " ]", Colors.Message);
                        }
                        else // for multi:
                        {
                            Log.Write(State.currentTXnode.name + " | " + State.currentTXnode.radios[0].name + ": [ " + "RIO" + " ],[ " + " ],[ " + " ] " + "TACAN Tune " + message + " [ " + " ] [ " + " ]", Colors.Message);
                        }

                        // for hotmic:
                        if (State.activeconfig.ICShotmic) //  
                        {
                            if (!State.valistening)
                            {
                                DcsClient.SendUpdateRequest();
                                State.MessageReset();
                                State.processlocked = false;
                            }
                        }

                        State.MessageReset();

                    }
                    catch (Exception e)
                    {
                        Log.Write("Error setting RIO command sequence: " + e.StackTrace + e.InnerException, Colors.Inline);
                    }
                }

            }
        }
    }
}



