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

                public static void SetRioDeviceSequence_Datalink_Tuning()
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
                        //Log.Write("Segment 0 = " + header, Colors.Warning);

                        int majval1;
                        Int32.TryParse(State.Proxy.Utility.ParseTokens("{CMDSEGMENT:1}"), out majval1);
                        //Log.Write("majval1 = " + majval1, Colors.Warning);
                        switch (majval1)
                        {
                            case 0:
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_DL_MAJ1_0);
                                break;
                            case 1:
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_DL_MAJ1_1);
                                break;
                            case 2:
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_DL_MAJ1_2);
                                break;
                            case 3:
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_DL_MAJ1_3);
                                break;
                            case 4:
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_DL_MAJ1_4);
                                break;
                            case 5:
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_DL_MAJ1_5);
                                break;
                            case 6:
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_DL_MAJ1_6);
                                break;
                            case 7:
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_DL_MAJ1_7);
                                break;
                            case 8:
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_DL_MAJ1_8);
                                break;
                            case 9:
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_DL_MAJ1_9);
                                break;
                        }

                        int majval2;
                        Int32.TryParse(State.Proxy.Utility.ParseTokens("{CMDSEGMENT:2}"), out majval2);
                        //Log.Write("majval2 = " + majval2, Colors.Warning);
                        switch (majval2)
                        {
                            case 0:
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_DL_MAJ2_0);
                                break;
                            case 1:
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_DL_MAJ2_1);
                                break;
                            case 2:
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_DL_MAJ2_2);
                                break;
                            case 3:
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_DL_MAJ2_3);
                                break;
                            case 4:
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_DL_MAJ2_4);
                                break;
                            case 5:
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_DL_MAJ2_5);
                                break;
                            case 6:
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_DL_MAJ2_6);
                                break;
                            case 7:
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_DL_MAJ2_7);
                                break;
                            case 8:
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_DL_MAJ2_8);
                                break;
                            case 9:
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_DL_MAJ2_9);
                                break;
                        }

                        int minval;
                        Int32.TryParse(State.Proxy.Utility.ParseTokens("{CMDSEGMENT:4}"), out minval);
                        //Log.Write("Segment 3 = " + minval, Colors.Warning);
                        switch (minval)
                        {
                            case 0:
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_DL_MIN_0);
                                break;
                            case 1:
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_DL_MIN_1);
                                break;
                            case 2:
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_DL_MIN_2);
                                break;
                            case 3:
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_DL_MIN_3);
                                break;
                            case 4:
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_DL_MIN_4);
                                break;
                            case 5:
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_DL_MIN_5);
                                break;
                            case 6:
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_DL_MIN_6);
                                break;
                            case 7:
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_DL_MIN_7);
                                break;
                            case 8:
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_DL_MIN_8);
                                break;
                            case 9:
                                State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_RAD_DL_MIN_9);
                                break;
                        }

                        string message = "3" + majval1.ToString() + majval2.ToString() + "." + minval.ToString() + "0 Mhz";

                        if (State.activeconfig.RIO_Messages && !State.activeconfig.RIO_Hints_Only)
                        {
                            State.currentmessage.dspmsg = "AIRIO : " + "Datalink Tune " + message;
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
                            int combinedvalue = ((100 * majval1) + (10 * majval2) + minval);
                            int devicemaxvalue = 999;
                            if (combinedvalue > devicemaxvalue)
                            {
                                State.currentmessage.dspmsg = "AIRIO : Datalink command out of range.\n";
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
                            Log.Write(State.currentTXnode.name + " | " + PTT.RadioDevices.SEL.name + ": [ " + "RIO" + " ],[ " + " ],[ " + " ] " + "Datalink Tune " + message + " [ " + " ] [ " + " ]", Colors.Message);
                        }
                        else // for multi:
                        {
                            Log.Write(State.currentTXnode.name + " | " + State.currentTXnode.radios[0].name + ": [ " + "RIO" + " ],[ " + " ],[ " + " ] " + "Datalink Tune " + message + " [ " + " ] [ " + " ]", Colors.Message);
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



