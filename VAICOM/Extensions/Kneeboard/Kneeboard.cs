using System;
using System.Collections.Generic;
using VAICOM.Client;
using VAICOM.Servers;
using VAICOM.Static;

namespace VAICOM
{
    namespace Extensions
    {
        namespace Kneeboard
        {

            public static class KneeboardUpdater
            {
                private static DateTime NextFriendlyAssetsRefreshUtc = DateTime.MinValue;

                // updates kneeboard for incoming messsages
                public static void UpdateFromReceivedMessage(Server.ServerCommsMessage message)
                {
                    // relays messages from AWACS, etc
                    try
                    {
                        KneeboardMessage msg = new KneeboardMessage();
                        msg.eventid = message.eventid;
                        string sendercat = Database.Dcs.SenderCatByString(message.eventkey).ToString().ToUpper();
                        string processedMessage = KneeboardHelper.ProcessMessageByEvent(message);
                        msg.logdata = new LogData(sendercat, processedMessage);
                        Client.DcsClient.SendKneeboardMessage(msg);
                        if (!string.IsNullOrWhiteSpace(processedMessage))
                        {
                            OpenKneeboardBridge.UpdateLog(sendercat, processedMessage);
                        }

                        if (IsAiCrewResponseMessage(message))
                        {
                            string role = GetAiCrewRoleFromEventKey(message.eventkey);
                            string responseText = string.IsNullOrWhiteSpace(message.text) ? processedMessage : message.text;
                            string aiCrewEntry = OpenKneeboardBridge.BuildAiCrewResponseEntry(role, responseText);
                            if (!string.IsNullOrWhiteSpace(aiCrewEntry))
                            {
                                OpenKneeboardBridge.UpdateLog("AI CREW", aiCrewEntry);
                            }
                        }
                    }
                    catch
                    {
                    }
                }

                private static bool IsAiCrewResponseMessage(Server.ServerCommsMessage message)
                {
                    if (message == null || string.IsNullOrWhiteSpace(message.eventkey))
                    {
                        return false;
                    }

                    string key = message.eventkey;
                    return key.StartsWith("wMsgJ_", StringComparison.OrdinalIgnoreCase)
                        || key.StartsWith("wMsgI_", StringComparison.OrdinalIgnoreCase)
                        || key.StartsWith("wMsgWSO_", StringComparison.OrdinalIgnoreCase)
                        || key.StartsWith("wMsgGeorge", StringComparison.OrdinalIgnoreCase);
                }

                private static string GetAiCrewRoleFromEventKey(string eventkey)
                {
                    if (string.IsNullOrWhiteSpace(eventkey))
                    {
                        return "AI CREW";
                    }

                    if (eventkey.StartsWith("wMsgWSO_", StringComparison.OrdinalIgnoreCase))
                    {
                        return "WSO";
                    }

                    if (eventkey.StartsWith("wMsgGeorge", StringComparison.OrdinalIgnoreCase))
                    {
                        return "GEORGE";
                    }

                    if (eventkey.StartsWith("wMsgI_", StringComparison.OrdinalIgnoreCase))
                    {
                        return "ICEMAN";
                    }

                    if (eventkey.StartsWith("wMsgJ_", StringComparison.OrdinalIgnoreCase))
                    {
                        return "RIO";
                    }

                    return "AI CREW";
                }

                // used by AOCS
                public static void UpdateMessagelogForCat(string cat, string content)
                {
                    //
                    try
                    {
                        KneeboardMessage msg = new KneeboardMessage();
                        msg.logdata = new LogData(cat, content);
                        Client.DcsClient.SendKneeboardMessage(msg);
                        if (!string.IsNullOrWhiteSpace(content))
                        {
                            OpenKneeboardBridge.UpdateLog(cat, content);
                        }
                    }
                    catch
                    {
                    }
                }

                public static void UpdateUnitsDetailsForCat(string cat, List<string> contents)
                {
                    //
                    try
                    {
                        KneeboardMessage msg = new KneeboardMessage();
                        msg.unitsdetails = new KneeboardUnitsDetails(cat, contents, true);
                        Client.DcsClient.SendKneeboardMessage(msg);
                        OpenKneeboardBridge.UpdateUnitsDetails(cat, contents);
                    }
                    catch
                    {
                    }
                }


                public static void UpdateServerData()
                {
                    //
                    try
                    {
                        KneeboardMessage msg = new KneeboardMessage();
                        msg.serverdata = new KneeboardServerData();
                        Client.DcsClient.SendKneeboardMessage(msg);
                        OpenKneeboardBridge.UpdateServerData();
                    }
                    catch
                    {
                    }
                }


                public static void SwitchPage(string cat)
                {

                    for (int i = 0; i <= 1; i += 1)
                    {
                        //
                        try
                        {
                            KneeboardMessage msg = new KneeboardMessage();
                            string sendcat = cat;
                            if (State.AIRIOactive && (cat.Equals("RIO") || cat.Equals("Iceman")))
                            {
                                sendcat = "REF";
                            }

                            if (cat.Equals("Crew")) //&& !State.currentstate.airborne
                            {
                                sendcat = "REF";
                            }

                            if (cat.Equals("Allies"))
                            {
                                sendcat = "FLIGHT";
                            }

                            msg.logdata = new LogData(sendcat.ToUpper(), sendcat.ToUpper());
                            State.KneeboardState.activecat = sendcat;
                            OpenKneeboardBridge.UpdateActiveCategory(sendcat);

                            if (!sendcat.Equals("NOTES") & !sendcat.Equals("LOG"))
                            {
                                if (!sendcat.Equals("REF"))
                                {
                                    KneeboardUnitsData catunits = new KneeboardUnitsData(sendcat, false);
                                    msg.unitsdata = catunits;
                                    OpenKneeboardBridge.UpdateUnits(sendcat, catunits.unitslist);
                                }
                                else if (cat.Equals("Crew"))
                                {
                                    KneeboardUnitsData crewUnits = new KneeboardUnitsData("Crew", false);
                                    OpenKneeboardBridge.UpdateUnits("CREW", crewUnits.unitslist);
                                }

                                SortedDictionary<string, List<string>> aliasstrings = new SortedDictionary<string, List<string>>();
                                if (State.KneeboardCatAliasStrings[i].ContainsKey(cat)) // if chunk not empty
                                {
                                    aliasstrings = State.KneeboardCatAliasStrings[i][cat]; // Key = "Request", Value = "Vector to Base", "Vector to Tanker"
                                }

                                msg.aliasdata = new AliasData(sendcat.ToUpper(), aliasstrings);
                                msg.aliasdata.chunk = i;
                                OpenKneeboardBridge.UpdateAliasChunk(sendcat, i, aliasstrings);

                            }

                            if (true) //(sendcat.Equals("NOTES") || sendcat.Equals("LOG") || msg.aliasdata.content.Count > 0) // if chunk not empty
                            {
                                if (State.activeconfig.Kneeboard_Enabled)
                                {
                                    msg.switchpage = true; // usually false
                                    Client.DcsClient.SendKneeboardMessage(msg); // send chunk
                                    Log.Write("(kneeboard switch page):" + msg.logdata.category, Colors.Inline); //+ " dict keys count " + aliasstrings.Count); ; ; ; // number of dict keys
                                }
                            }

                        }
                        catch (Exception a)
                        {
                            Log.Write("error switching page for :" + cat + "\n" + a.Message, Colors.Inline);
                        }
                    }
                }


                public static void SendHeartBeatCycle()
                {
                    try
                    {
                        KneeboardMessage msg = new KneeboardMessage(); // includes dict state

                        msg.serverdata = new KneeboardServerData();
                        OpenKneeboardBridge.UpdateServerData();

                        if (State.Proxy.Dictation.IsOn()) // in dictation mode: include buffer update every 1/4 second:
                        {
                            State.uitimerinterval = 250;
                            // RELAY TO KNEEBOARD
                            string dictbuffer = State.Proxy.Utility.ParseTokens("{DICTATION:NEWLINE}");
                            if (!State.kneeboardlastdictbuffer.Equals(dictbuffer))
                            {
                                State.kneeboardlastdictbuffer = dictbuffer;
                                State.kneeboardcurrentbuffer = dictbuffer;
                                msg.logdata = new LogData("NOTES", dictbuffer);
                                OpenKneeboardBridge.UpdateNotesBuffer(dictbuffer);
                                OpenKneeboardBridge.UpdateLog("NOTES", dictbuffer);
                            }
                        }
                        else
                        {
                            State.uitimerinterval = 1000;
                        }

                        Client.DcsClient.SendKneeboardMessage(msg);

                        DateTime nowUtc = DateTime.UtcNow;
                        if (nowUtc >= NextFriendlyAssetsRefreshUtc)
                        {
                            if (!State.currentstate.id.Equals("UH-1H"))
                            {
                                DcsClient.SendUpdateRequest();
                            }
                            OpenKneeboardBridge.ForceRefreshFriendlyAssetsData();
                            NextFriendlyAssetsRefreshUtc = nowUtc.AddSeconds(5);
                        }

                    }
                    catch
                    {
                    }
                }

                public static void SendDeviceCommand(int dev, int cmd, double val)
                {
                    try
                    {
                        // generic device action

                        State.currentmessage = new DcsClient.Message.CommsMessage();
                        State.currentmessage.type = Messagetypes.DeviceControl;

                        DcsClient.DeviceAction action = new DcsClient.DeviceAction();

                        action.device = dev;
                        action.command = cmd;
                        action.value = val;

                        State.currentmessage.devsequence.Add(action);

                        Client.DcsClient.SendClientMessage();

                    }
                    catch (Exception a)
                    {
                        Log.Write("SendDeviceCommand: " + a.StackTrace, Colors.Text);
                    }
                }


                public static void RefreshCurrentPage()
                {
                    try
                    {
                        KneeboardMessage msg = new KneeboardMessage();
                        msg.logdata = new LogData(State.KneeboardState.activecat, State.kneeboardcurrentbuffer);
                        Client.DcsClient.SendKneeboardMessage(msg);
                        OpenKneeboardBridge.UpdateLog(State.KneeboardState.activecat, State.kneeboardcurrentbuffer);
                    }
                    catch
                    {
                    }
                }

            }

            public class KneeboardState
            {
                public string activecat;

                public KneeboardState()
                {
                    activecat = "LOG";
                }

            }
        }
    }
}
