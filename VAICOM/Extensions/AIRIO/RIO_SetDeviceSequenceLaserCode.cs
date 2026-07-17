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

                public static void SetRioDeviceSequence_LaserCode()
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
                            //case 0:
                            //    State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_Laser_000);
                            //    break;
                            //case 1:
                            //    State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_Laser_100);
                            //    break;
                            //case 2:
                            //    State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_Laser_200);
                            //    break;
                            //case 3:
                            //    State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_Laser_300);
                            //    break;
                            //case 4:
                            //    State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_Laser_400);
                            //    break;
                            case 5:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_Atom_Laser_500);
                                break;
                            case 6:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_Atom_Laser_600);
                                break;
                            case 7:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_Atom_Laser_700);
                                break;
                                //case 8:
                                //    State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_Laser_800);
                                //    break;
                                //case 9:
                                //    State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_Laser_900);
                                //    break;
                        }

                        int majval2;
                        Int32.TryParse(State.Proxy.Utility.ParseTokens("{CMDSEGMENT:2}"), out majval2);
                        //Log.Write("majval2 = " + majval2, Colors.Warning);
                        switch (majval2)
                        {
                            //case 0:
                            //    State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_Laser_x00);
                            //    break;
                            case 1:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_Atom_Laser_010);
                                break;
                            case 2:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_Atom_Laser_020);
                                break;
                            case 3:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_Atom_Laser_030);
                                break;
                            case 4:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_Atom_Laser_040);
                                break;
                            case 5:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_Atom_Laser_050);
                                break;
                            case 6:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_Atom_Laser_060);
                                break;
                            case 7:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_Atom_Laser_070);
                                break;
                            case 8:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_Atom_Laser_080);
                                break;
                                //case 9:
                                //    State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_Laser_090);
                                //    break;
                        }

                        int minval;
                        Int32.TryParse(State.Proxy.Utility.ParseTokens("{CMDSEGMENT:3}"), out minval);
                        //Log.Write("Segment 3 = " + minval, Colors.Warning);
                        switch (minval)
                        {
                            //case 0:
                            //    State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_Laser_xx0);
                            //    break;
                            case 1:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_Atom_Laser_001);
                                break;
                            case 2:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_Atom_Laser_002);
                                break;
                            case 3:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_Atom_Laser_003);
                                break;
                            case 4:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_Atom_Laser_004);
                                break;
                            case 5:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_Atom_Laser_005);
                                break;
                            case 6:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_Atom_Laser_006);
                                break;
                            case 7:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_Atom_Laser_007);
                                break;
                            case 8:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_Atom_Laser_008);
                                break;
                                //case 9:
                                //    State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_Laser_009);
                                //    break;
                        }

                        string message = majval1.ToString() + majval2.ToString() + minval.ToString();

                        if (State.activeconfig.RIO_Messages && !State.activeconfig.RIO_Hints_Only)
                        {
                            State.currentmessage.dspmsg = "AIRIO : " + "Laser code set to 1" + message;
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
                            int devicemaxvalue = 788;
                            if (combinedvalue > devicemaxvalue)
                            {
                                State.currentmessage.dspmsg = "AIRIO : value out of range.\n";
                                State.currentmessage.msgdur = 5;
                                State.currentmessage.extsequence = new List<Extensions.RIO.DeviceAction>(); // empty
                                //riospeech.riospeakrandom(2); //negative
                            }
                            else // not out of range
                            {
                                //riospeech.riospeakrandom(1); //ok
                            }
                        }

                        SendNewMessage();

                        // write message to log 
                        // for single:
                        if ((State.currentmodule.Singlehotkey & !State.activeconfig.ForceMultiHotkey) || (!State.currentmodule.Singlehotkey & State.activeconfig.ForceSingleHotkey)) // for single mode
                        {
                            Log.Write(State.currentTXnode.name + " | " + PTT.RadioDevices.SEL.name + ": [ " + "RIO" + " ],[ " + " ],[ " + " ] " + "Laser Code " + message + " [ " + " ] [ " + " ]", Colors.Message);
                        }
                        else // for multi:
                        {
                            Log.Write(State.currentTXnode.name + " | " + State.currentTXnode.radios[0].name + ": [ " + "RIO" + " ],[ " + " ],[ " + " ] " + "Laser Code " + message + " [ " + " ] [ " + " ]", Colors.Message);
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



