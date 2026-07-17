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

                public static void SetRioDeviceSequence_Radio_Tuning()
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

                        // start menu sequence

                        State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_MENU_MAIN); // includes close first
                        State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_RAD_182_TUNE_MAN);

                        string header = State.Proxy.Utility.ParseTokens("{CMDSEGMENT:0}");

                        // MAJ 1
                        int majval1;
                        Int32.TryParse(State.Proxy.Utility.ParseTokens("{CMDSEGMENT:1}"), out majval1);

                        switch (majval1)
                        {
                            case 0:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_0_); // underscore for first only
                                break;
                            case 1:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_1);
                                break;
                            case 2:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_2);
                                break;
                            case 3:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_3);
                                break;
                        }

                        // MAJ 2
                        int majval2;
                        Int32.TryParse(State.Proxy.Utility.ParseTokens("{CMDSEGMENT:2}"), out majval2);

                        switch (majval2)
                        {
                            case 0:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_0);
                                break;
                            case 1:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_1);
                                break;
                            case 2:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_2);
                                break;
                            case 3:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_3);
                                break;
                            case 4:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_4);
                                break;
                            case 5:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_5);
                                break;
                            case 6:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_6);
                                break;
                            case 7:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_7);
                                break;
                            case 8:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_8);
                                break;
                            case 9:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_9);
                                break;
                        }

                        // MAJ 3
                        int majval3;
                        Int32.TryParse(State.Proxy.Utility.ParseTokens("{CMDSEGMENT:3}"), out majval3);

                        switch (majval3)
                        {
                            case 0:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_0);
                                break;
                            case 1:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_1);
                                break;
                            case 2:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_2);
                                break;
                            case 3:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_3);
                                break;
                            case 4:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_4);
                                break;
                            case 5:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_5);
                                break;
                            case 6:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_6);
                                break;
                            case 7:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_7);
                                break;
                            case 8:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_8);
                                break;
                            case 9:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_9);
                                break;
                        }

                        // MIN 1
                        int minval1;
                        Int32.TryParse(State.Proxy.Utility.ParseTokens("{CMDSEGMENT:5}"), out minval1);

                        switch (minval1)
                        {
                            case 0:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_0);
                                break;
                            case 1:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_1);
                                break;
                            case 2:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_2);
                                break;
                            case 3:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_3);
                                break;
                            case 4:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_4);
                                break;
                            case 5:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_5);
                                break;
                            case 6:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_6);
                                break;
                            case 7:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_7);
                                break;
                            case 8:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_8);
                                break;
                            case 9:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_9);
                                break;

                        }

                        // MIN 2
                        int minval2 = 0;
                        string valstr = State.Proxy.Utility.ParseTokens("{CMDSEGMENT:6}");
                        switch (valstr.ToLower())
                        {
                            //0
                            case "0":
                                minval2 = 0;
                                break;
                            case "zero":
                                minval2 = 0;
                                break;
                            case "cero":
                                minval2 = 0;
                                break;
                            case "null":
                                minval2 = 0;
                                break;

                            //25
                            case "2 5":
                                minval2 = 25;
                                break;
                            case "two five":
                                minval2 = 25;
                                break;
                            case "dos cinco":
                                minval2 = 25;
                                break;
                            case "deaux cinq":
                                minval2 = 25;
                                break;
                            case "zwei funf":
                                minval2 = 25;
                                break;
                            case "zwo funf":
                                minval2 = 25;
                                break;

                            //50
                            case "5 0":
                                minval2 = 50;
                                break;
                            case "five zero":
                                minval2 = 50;
                                break;
                            case "cinco cero":
                                minval2 = 50;
                                break;
                            case "cinq zero":
                                minval2 = 50;
                                break;
                            case "funf null":
                                minval2 = 50;
                                break;

                            // 75
                            case "7 5":
                                minval2 = 75;
                                break;
                            case "seven five":
                                minval2 = 75;
                                break;
                            case "siete cinco":
                                minval2 = 75;
                                break;
                            case "sept cinq":
                                minval2 = 75;
                                break;
                            case "sieben funf":
                                minval2 = 75;
                                break;

                            default:
                                minval2 = 0;
                                break;
                        }

                        switch (minval2)
                        {
                            case 0:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_00);
                                break;
                            case 25:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_25);
                                break;
                            case 50:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_50);
                                break;
                            case 75:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_75);
                                break;
                        }


                        // always close menu wheel: add at the very end
                        State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_MENU_CLOSE);

                        string message = majval1.ToString() + majval2.ToString() + majval3.ToString() + "." + minval1.ToString() + minval2.ToString() + " MHz";

                        if (State.activeconfig.RIO_Messages && !State.activeconfig.RIO_Hints_Only)
                        {
                            State.currentmessage.dspmsg = "AIRIO : " + "AN/ARC-182 Tune " + message;
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
                            int combinedvalue = ((100000 * majval1) + (10000 * majval2) + (1000 * majval3) + (100 * minval1) + minval2);
                            int deviceminvalue = 30000;
                            int devicemaxvalue = 399975;
                            if ((combinedvalue > devicemaxvalue) || (combinedvalue < deviceminvalue))
                            {
                                State.currentmessage.dspmsg = "AIRIO : Radio command out of range.\n";
                                State.currentmessage.msgdur = 5;
                                State.currentmessage.extsequence = new List<Extensions.RIO.DeviceAction>(); // empty
                                riospeech.riospeakrandom(2); //negative
                            }
                            else // not out of range
                            {
                                //riospeech.riospeakrandom(1); //not needed (menu)
                            }
                        }

                        SendNewMessage();

                        // write message to log 

                        // for single:
                        if ((State.currentmodule.Singlehotkey & !State.activeconfig.ForceMultiHotkey) || (!State.currentmodule.Singlehotkey & State.activeconfig.ForceSingleHotkey)) // for single mode
                        {
                            Log.Write(State.currentTXnode.name + " | " + PTT.RadioDevices.SEL.name + ": [ " + "RIO" + " ],[ " + " ],[ " + " ] " + "AN/ARC-182 Tune " + message + " [ " + " ] [ " + " ]", Colors.Message);
                        }
                        else // for multi:
                        {
                            Log.Write(State.currentTXnode.name + " | " + State.currentTXnode.radios[0].name + ": [ " + "RIO" + " ],[ " + " ],[ " + " ] " + "AN/ARC-182 Tune " + message + " [ " + " ] [ " + " ]", Colors.Message);
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



