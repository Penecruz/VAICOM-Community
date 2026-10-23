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

                        string lasercode = Extensions.CommandNumbers.Digits();

                        // The device's thumbwheels are fixed at 1xxx, so the leading 1 is
                        // implicit. Accept the full real-world code as well as the short form.
                        if (lasercode.Length == 4 && lasercode[0] == '1')
                        {
                            lasercode = lasercode.Substring(1);
                        }

                        if (lasercode.Length != 3)
                        {
                            ReportRioInputError("Could not read the laser code.\nSay three digits, or the full code as 1xxx.",
                                                "Laser code: expected 3 digits, got '" + lasercode + "'");
                            return;
                        }

                        // Each thumbwheel has its own range: 5-7, 1-8, 1-8. Checking only the
                        // combined value against 788 lets a code such as 699 through, which
                        // then matches no case below and silently sets a different code.
                        if (lasercode[0] < '5' || lasercode[0] > '7'
                            || lasercode[1] < '1' || lasercode[1] > '8'
                            || lasercode[2] < '1' || lasercode[2] > '8')
                        {
                            ReportRioInputError("1" + lasercode + " is not a valid laser code.\nRange is 1511 to 1788.",
                                                "Laser code 1" + lasercode + " is out of range (1511 to 1788)");
                            return;
                        }

                        string header = State.Proxy.Utility.ParseTokens("{CMDSEGMENT:0}");
                        //Log.Write("Segment 0 = " + header, Colors.Warning);

                        int majval1 = Extensions.CommandNumbers.At(lasercode, 0);
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

                        int majval2 = Extensions.CommandNumbers.At(lasercode, 1);
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

                        int minval = Extensions.CommandNumbers.At(lasercode, 2);
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



