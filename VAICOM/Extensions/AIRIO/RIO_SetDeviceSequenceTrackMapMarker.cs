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


                public static void SetRioDeviceSequence_TrackMapMarker()
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

                        // root sequence
                        State.currentmessage.extsequence.AddRange(DeviceActionsLibrary.Sequences.Macro.Seq_J_MENU_CONTEXT); // includes close first
                        State.currentmessage.extsequence.AddRange(DeviceActionsLibrary.Sequences.Macro.Seq_J_MENU_OPTION_2);
                        State.currentmessage.extsequence.AddRange(DeviceActionsLibrary.Sequences.Macro.Seq_J_MENU_OPTION_5);
                        string header = State.Proxy.Utility.ParseTokens("{CMDSEGMENT:0}"); // Track Marker
                                                                                           //Log.Write("Segment 0 = " + header, Colors.Warning);

                        int segment1;
                        Int32.TryParse(State.Proxy.Utility.ParseTokens("{CMDSEGMENT:1}"), out segment1); // Marker Number 1-10
                        int markercount = State.currentstate.riostate.markers;

                        int position = 1 + markercount - segment1; // 1+
                        //Log.Write("Position = " + position, Colors.Warning);

                        switch (position)
                        {
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
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_1);
                                break;
                            case 8:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_7);
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_2);
                                break;
                            case 9:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_7);
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_3);
                                break;
                            case 10:
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_7);
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_4);
                                break;
                            default:
                                break;

                        }


                        // always close menu wheel: add at the very end
                        State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_MENU_CLOSE);

                        string message = "LANTIRN Track Map Marker " + segment1.ToString();

                        if (State.activeconfig.RIO_Messages && !State.activeconfig.RIO_Hints_Only)
                        {
                            State.currentmessage.dspmsg = "AIRIO : " + message;
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
                        }

                        if (segment1 > markercount)
                        {
                            Log.Write("Marker out of range! There are " + markercount + " markers on the map.", Colors.Warning);
                            State.currentmessage.dspmsg = "AIRIO : Marker out of range!\nThere are " + markercount + " markers on the map.";
                            State.currentmessage.msgdur = 5;
                            State.currentmessage.extsequence = new List<Extensions.RIO.DeviceAction>(); // empty
                            riospeech.riospeakrandom(2); //negative
                        }

                        SendNewMessage();

                        // write message to log 

                        // for single:
                        if ((State.currentmodule.Singlehotkey & !State.activeconfig.ForceMultiHotkey) || (!State.currentmodule.Singlehotkey & State.activeconfig.ForceSingleHotkey)) // for single mode
                        {
                            Log.Write(State.currentTXnode.name + " | " + PTT.RadioDevices.SEL.name + ": [ " + "RIO" + " ],[ " + " ],[ " + " ] " + message + " [ " + " ] [ " + " ]", Colors.Message);
                        }
                        else // for multi:
                        {
                            Log.Write(State.currentTXnode.name + " | " + State.currentTXnode.radios[0].name + ": [ " + "RIO" + " ],[ " + " ],[ " + " ] " + message + " [ " + " ] [ " + " ]", Colors.Message);
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



