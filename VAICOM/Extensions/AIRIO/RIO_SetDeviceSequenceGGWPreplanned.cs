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
                private static bool TryParseSmallNumberToken(string token, out int number)
                {
                    number = 0;

                    if (string.IsNullOrWhiteSpace(token))
                    {
                        return false;
                    }

                    string value = token.Trim();
                    if (Int32.TryParse(value, out number))
                    {
                        return true;
                    }

                    switch (value.ToLowerInvariant())
                    {
                        case "one": number = 1; return true;
                        case "two": number = 2; return true;
                        case "three": number = 3; return true;
                        case "four": number = 4; return true;
                        case "five": number = 5; return true;
                        case "six": number = 6; return true;
                        case "seven": number = 7; return true;
                        case "eight": number = 8; return true;
                    }

                    string digitsOnly = string.Empty;
                    for (int i = 0; i < value.Length; i++)
                    {
                        char ch = value[i];
                        if (ch >= '0' && ch <= '9')
                        {
                            digitsOnly += ch;
                        }
                    }

                    if (digitsOnly.Length > 0 && Int32.TryParse(digitsOnly, out number))
                    {
                        return true;
                    }

                    number = 0;
                    return false;
                }

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

                        // Step 1: gather spoken segments and normalize into tokens we can parse reliably.
                        List<string> tokens = new List<string>();
                        for (int i = 1; i <= 5; i++)
                        {
                            string segment = State.Proxy.Utility.ParseTokens("{CMDSEGMENT:" + i.ToString() + "}");
                            if (!string.IsNullOrWhiteSpace(segment))
                            {
                                tokens.Add(segment.Trim());
                            }
                        }

                        List<int> parsedNumbers = new List<int>();
                        bool hasAllKeyword = false;
                        for (int i = 0; i < tokens.Count; i++)
                        {
                            string token = tokens[i];
                            if (token.Equals("all", StringComparison.OrdinalIgnoreCase))
                            {
                                hasAllKeyword = true;
                            }

                            int parsed;
                            if (TryParseSmallNumberToken(token, out parsed))
                            {
                                parsedNumbers.Add(parsed);
                            }
                        }

                        int ppIndex = 0;
                        int station = 0;
                        bool toAllStations = false;
                        string tokenDump = string.Join(",", tokens.ToArray());

                        // Step 2: resolve command form.
                        if (hasAllKeyword && parsedNumbers.Count >= 1)
                        {
                            int pp = parsedNumbers[0];
                            if (pp >= 1 && pp <= 8)
                            {
                                ppIndex = pp;
                                toAllStations = true;
                            }
                        }
                        else if (parsedNumbers.Count >= 2)
                        {
                            int first = parsedNumbers[0];
                            int second = parsedNumbers[1];

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
                        // Step 3: validate resolved values.
                        if (ppIndex < 1 || ppIndex > 8 || (!toAllStations && (station < 3 || station > 6)))
                        {
                            Log.Write(
                                "AIRIO GGW preplanned parse failed | tokens=" + tokenDump
                                + " | numbers=" + string.Join(",", parsedNumbers.ToArray())
                                + " | hasAll=" + hasAllKeyword.ToString()
                                + " | resolvedPP=" + ppIndex.ToString()
                                + " | resolvedStation=" + station.ToString(),
                                Colors.Warning);

                            State.currentmessage.dspmsg = "AIRIO : Invalid GGW preplanned command format.";
                            State.currentmessage.msgdur = 5;
                            State.currentmessage.extsequence = new List<Extensions.RIO.DeviceAction>();
                            riospeech.riospeakrandom(2); // negative
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

                        if (toAllStations)
                        {
                            Log.Write("AIRIO GGW preplanned queued | pp=" + ppIndex.ToString() + " | target=all stations", Colors.Message);
                        }
                        else
                        {
                            Log.Write("AIRIO GGW preplanned queued | pp=" + ppIndex.ToString() + " | target=station " + station.ToString(), Colors.Message);
                        }

                        UI.Playsound.Commandcomplete();
                        riospeech.riospeakrandom(1); // positive

                        if (!State.clientmode.Equals(ClientModes.Debug) && tables.menustate[tables.menucats.PLAYERSEAT].Equals(tables.menustates.RIO))
                        {
                            Log.Write("AIRIO GGW preplanned blocked: player is in Jester seat.", Colors.Warning);
                            State.currentmessage.dspmsg = "AIRIO : You are in Jester's seat!\n";
                            State.currentmessage.msgdur = 5;
                            State.currentmessage.extsequence = new List<Extensions.RIO.DeviceAction>();
                            riospeech.riospeakrandom(2); // negative
                        }

                        // Dispatch the constructed GGW preplanned macro sequence.
                        SendNewMessage();
                    }
                    catch
                    {
                    }
                }
            }
        }
    }
}
