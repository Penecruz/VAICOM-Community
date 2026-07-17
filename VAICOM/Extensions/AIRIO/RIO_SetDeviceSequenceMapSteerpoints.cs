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

                public static string FilterTranslate(string input)
                {
                    string output = input;

                    if (input.Contains("1"))
                    {
                        return "waypoint 1";
                    }
                    if (input.Contains("2"))
                    {
                        return "waypoint 2";
                    }
                    if (input.Contains("3"))
                    {
                        return "waypoint 3";
                    }
                    if (input.Contains("fix"))
                    {
                        return "fixed point";
                    }
                    if (input.Contains("init"))
                    {
                        return "initial point";
                    }
                    if (input.Contains("targ"))
                    {
                        return "surface target";
                    }
                    if (input.Contains("bas"))
                    {
                        return "home base";
                    }
                    if (input.Contains("def"))
                    {
                        return "defense point";
                    }
                    if (input.Contains("zon"))
                    {
                        return "hostile zone";
                    }

                    return output;
                }


                public static void SetRioDeviceSequence_Map_Steerpoints()
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
                        //State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_MENU_CLOSE);
                        State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_MENU_MAIN); // includes close first
                        State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_UTIL_NAV_MAP_MARKER);

                        string header = State.Proxy.Utility.ParseTokens("{CMDSEGMENT:0}");
                        //Log.Write("Segment 0 = " + header, Colors.Warning);


                        // MAJ 3
                        string segment3 = State.Proxy.Utility.ParseTokens("{CMDSEGMENT:3}");
                        //Log.Write("segment 3 = " + segment3, Colors.Warning);

                        string filterseg3 = FilterTranslate(segment3.ToLower());

                        switch (filterseg3)
                        {

                            case "waypoint 1": // translations?
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_1);
                                break;
                            case "waypoint 2":
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_2);
                                break;
                            case "waypoint 3":
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_3);
                                break;
                            case "fixed point":
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_4);
                                break;
                            case "initial point":
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_5);
                                break;
                            case "surface target":
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_6);
                                break;
                            case "home base":
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_7);
                                break;
                            case "defense point":
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_0_);
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_1);
                                break;
                            case "hostile zone":
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_0_);
                                State.currentmessage.extsequence.AddRange(VAICOM.Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_INPUT_NUM_2);
                                break;
                        }


                        // MAJ 1 --- do as last step
                        int segment1;
                        Int32.TryParse(State.Proxy.Utility.ParseTokens("{CMDSEGMENT:1}"), out segment1);
                        int markercount = State.currentstate.riostate.markers;

                        //Log.Write("Map Marker = " + segment1 + "\\" + markercount, Colors.Warning);

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

                        // MAJ 2
                        string segment2 = State.Proxy.Utility.ParseTokens("{CMDSEGMENT:2}");
                        //Log.Write("majval2 = " + majval2, Colors.Warning);


                        // always close menu wheel: add at the very end
                        State.currentmessage.extsequence.Add(VAICOM.Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_MENU_CLOSE);

                        string message = "RIO | Map marker" + " " + segment1.ToString() + " " + "to" + " " + segment3;

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
                        if (PTT.IsPTTModeSingle()) // for single mode
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



