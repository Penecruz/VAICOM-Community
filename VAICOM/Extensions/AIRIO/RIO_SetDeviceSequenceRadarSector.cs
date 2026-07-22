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

                public static void SetRioDeviceSequence_Radar_Sector()
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

                        State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_MENU_MAIN); // includes close first
                        State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_RDR_BVR); // 2 2
                        State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_2); // submenu 2

                        string header = State.Proxy.Utility.ParseTokens("{CMDSEGMENT:0}");
                        //Log.Write("Segment 0 = " + header, Colors.Warning);

                        int scanrange;
                        Int32.TryParse(State.Proxy.Utility.ParseTokens("{CMDSEGMENT:3}"), out scanrange);
                        //Log.Write("Segment 3 = " + scanrange, Colors.Warning);
                        switch (scanrange)
                        {
                            default:
                                if (scanrange >= 0 && scanrange < 15)  // 15
                                {
                                    State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_1);
                                }
                                if (scanrange >= 15 && scanrange < 25) // 20
                                {
                                    State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_2);
                                }
                                if (scanrange >= 25 && scanrange < 35) // 30
                                {
                                    State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_3);
                                }
                                if (scanrange >= 35 && scanrange < 45) // 40
                                {
                                    State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_4);
                                }
                                if (scanrange >= 45 && scanrange < 60) // 50
                                {
                                    State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_5);
                                }
                                if (scanrange >= 60 && scanrange < 85) // 75
                                {
                                    State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_6);
                                }
                                if (scanrange >= 85 && scanrange < 125) // 100
                                {
                                    State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_7);
                                }
                                if (scanrange >= 125 && scanrange <= 150) // 150
                                {
                                    State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_8);
                                }
                                break;
                        }

                        int scanaltitude;
                        Int32.TryParse(State.Proxy.Utility.ParseTokens("{CMDSEGMENT:1}"), out scanaltitude);
                        //Log.Write("Segment 1 = " + scanaltitude, Colors.Warning);
                        switch (scanaltitude)
                        {
                            case 0:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_1);
                                break;
                            case 5:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_2);
                                break;
                            case 10:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_3);
                                break;
                            case 15:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_4);
                                break;
                            case 20:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_5);
                                break;
                            case 25:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_6);
                                break;
                            case 30:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_7);
                                break;
                            case 35:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_0_);
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_1);
                                break;
                            case 40:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_0_);
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_2);
                                break;
                            case 45:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_0_);
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_3);
                                break;
                            case 50:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_0_);
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_4);
                                break;
                            case 55:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_0_);
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_5);
                                break;
                            case 60:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_0_);
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_6);
                                break;
                            case 65:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_0_);
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_7);
                                break;
                            case 70:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_0_);
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_8);
                                break;
                        }

                        string majval2 = State.Proxy.Utility.ParseTokens("{CMDSEGMENT:2}");
                        //Log.Write("Segment 2 = " + majval2, Colors.Warning);


                        string message = scanaltitude.ToString() + ".000ft " + majval2.ToString() + " " + scanrange.ToString() + "NM";

                        if (State.activeconfig.RIO_Messages && !State.activeconfig.RIO_Hints_Only)
                        {
                            State.currentmessage.dspmsg = "AIRIO : " + "Radar Scan Sector " + message;
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
                            //riospeech.riospeakrandom(1); //ok       
                        }

                        SendNewMessage();

                        // write message to log 
                        // for single:
                        if (PTT.IsPTTModeSingle()) // for single mode
                        {
                            Log.Write(State.currentTXnode.name + " | " + PTT.RadioDevices.SEL.name + ": [ " + "RIO" + " ],[ " + " ],[ " + " ] " + "Radar Scan Sector " + message + " [ " + " ] [ " + " ]", Colors.Message);
                        }
                        else // for multi:
                        {
                            Log.Write(State.currentTXnode.name + " | " + State.currentTXnode.radios[0].name + ": [ " + "RIO" + " ],[ " + " ],[ " + " ] " + "Radar Scan Sector " + message + " [ " + " ] [ " + " ]", Colors.Message);
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



