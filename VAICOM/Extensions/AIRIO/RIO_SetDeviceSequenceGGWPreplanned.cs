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
                public static void SetRioDeviceSequence_GGW_PrePlanned()
                {
                    try
                    {
                        if (!State.dll_installed_rio || !State.activeconfig.RIO_Enabled || !State.IsAirioTomcatModule())
                        {
                            Log.Write("AIRIO commands are not available at this time.", Colors.Warning);
                            UI.Playsound.Recipientna();
                            return;
                        }

                        State.currentmessage = new CommsMessage();
                        setdefaultmessageparams();
                        State.currentmessage.type = Messagetypes.DeviceControl;
                        State.currentmessage.extsequence = new List<Extensions.RIO.DeviceAction>();

                        string segment1 = State.Proxy.Utility.ParseTokens("{CMDSEGMENT:1}");
                        string segment2 = State.Proxy.Utility.ParseTokens("{CMDSEGMENT:2}");

                        int first;
                        int second;
                        bool firstIsInt = Int32.TryParse(segment1, out first);
                        bool secondIsInt = Int32.TryParse(segment2, out second);

                        int ppIndex = 0;
                        int station = 0;
                        bool toAllStations = false;

                        if (firstIsInt && secondIsInt)
                        {
                            if (first >= 1 && first <= 8 && second >= 3 && second <= 6)
                            {
                                ppIndex = first;
                                station = second;
                            }
                            else if (second >= 1 && second <= 8 && first >= 3 && first <= 6)
                            {
                                ppIndex = second;
                                station = first;
                            }
                        }
                        else if (firstIsInt && first >= 1 && first <= 8 && segment2.Equals("all", StringComparison.OrdinalIgnoreCase))
                        {
                            ppIndex = first;
                            toAllStations = true;
                        }
                        else if (secondIsInt && second >= 1 && second <= 8 && segment1.Equals("all", StringComparison.OrdinalIgnoreCase))
                        {
                            ppIndex = second;
                            toAllStations = true;
                        }

                        if (ppIndex < 1 || ppIndex > 8 || (!toAllStations && (station < 3 || station > 6)))
                        {
                            State.currentmessage.dspmsg = "AIRIO : Invalid GGW preplanned command format.";
                            State.currentmessage.msgdur = 5;
                            State.currentmessage.extsequence = new List<Extensions.RIO.DeviceAction>();
                            UI.Playsound.Sorry();
                            return;
                        }

                        State.currentmessage.extsequence.AddRange(DeviceActionsLibrary.Sequences.Macro.Seq_J_MENU_MAIN);

                        if (toAllStations)
                        {
                            State.currentmessage.extsequence.Add(DeviceActionsLibrary.RIO.Atom_J_MENU_OPTION_4);
                            State.currentmessage.extsequence.Add(DeviceActionsLibrary.RIO.Atom_J_MENU_OPTION_6);
                            State.currentmessage.extsequence.Add(DeviceActionsLibrary.RIO.Atom_J_MENU_OPTION_3);
                        }
                        else
                        {
                            State.currentmessage.extsequence.Add(DeviceActionsLibrary.RIO.Atom_J_MENU_OPTION_4);
                            State.currentmessage.extsequence.Add(DeviceActionsLibrary.RIO.Atom_J_MENU_OPTION_6);
                            State.currentmessage.extsequence.Add(DeviceActionsLibrary.RIO.Atom_J_MENU_OPTION_2);

                            if (station == 3) { State.currentmessage.extsequence.Add(DeviceActionsLibrary.RIO.Atom_J_MENU_OPTION_1); }
                            if (station == 4) { State.currentmessage.extsequence.Add(DeviceActionsLibrary.RIO.Atom_J_MENU_OPTION_2); }
                            if (station == 5) { State.currentmessage.extsequence.Add(DeviceActionsLibrary.RIO.Atom_J_MENU_OPTION_3); }
                            if (station == 6) { State.currentmessage.extsequence.Add(DeviceActionsLibrary.RIO.Atom_J_MENU_OPTION_4); }
                        }

                        State.currentmessage.extsequence.Add(Extensions.RIO.helper.GetAtom(ppIndex));
                        State.currentmessage.extsequence.Add(DeviceActionsLibrary.RIO.Atom_J_MENU_CLOSE);

                        if (State.activeconfig.RIO_Messages && !State.activeconfig.RIO_Hints_Only)
                        {
                            if (toAllStations)
                            {
                                State.currentmessage.dspmsg = "AIRIO : Sent PP " + ppIndex + " to GGW all stations.";
                            }
                            else
                            {
                                State.currentmessage.dspmsg = "AIRIO : Sent PP " + ppIndex + " to GGW station " + station + ".";
                            }
                            State.currentmessage.msgdur = 4;
                        }

                        UI.Playsound.Commandcomplete();

                        if (!State.clientmode.Equals(ClientModes.Debug) && tables.menustate[tables.menucats.PLAYERSEAT].Equals(tables.menustates.RIO))
                        {
                            State.currentmessage.dspmsg = "AIRIO : You are in Jester's seat!\n";
                            State.currentmessage.msgdur = 5;
                            State.currentmessage.extsequence = new List<Extensions.RIO.DeviceAction>();
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }
    }
}
