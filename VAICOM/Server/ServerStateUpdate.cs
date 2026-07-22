using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using VAICOM.Static;

namespace VAICOM
{
    namespace Servers
    {

        public static partial class Server
        {

            public static void UpdateServerState(ServerMessage serverMessage) // gets all chuncks
            {
                ExtractAll(serverMessage);

                // Some MP environments may not deliver all legacy chunks (11/12).
                // If core state is present by chunk 10, allow processing to continue.
                if (!receivedupdatecomplete
                    && serverMessage != null
                    && serverMessage.cid >= 10
                    && State.currentstate != null
                    && !string.IsNullOrEmpty(State.currentstate.id)
                    && !State.currentstate.id.Equals("----"))
                {
                    receivedupdatecomplete = true;
                    processingchunks = false;
                }

                if (receivedupdatecomplete)
                {
                    if (!processingchunks)
                    {
                        ProcessServerData();
                    }
                }

            }

            public static void DumpStateToLog()
            {
                Log.Reset();
                string state = JsonConvert.SerializeObject(State.currentstate, Formatting.Indented).ToString();
                Log.Write("STATE: " + state, Colors.Critical);
            }

            public static int chunkcount = 12;

            public static void ExtractAll(ServerMessage serverMessage)
            {
                switch (serverMessage.cid)
                {
                    case 1:
                        ExtractChunk1(serverMessage);
                        break;
                    case 2:
                        ExtractChunk2(serverMessage);
                        break;
                    case 3:
                        ExtractChunk3(serverMessage);
                        break;
                    case 4:
                        ExtractChunk4(serverMessage);
                        break;
                    case 5:
                        ExtractChunk5(serverMessage);
                        break;
                    case 6:
                        ExtractChunk6(serverMessage);
                        break;
                    case 7:
                        ExtractChunk7(serverMessage);
                        break;
                    case 8:
                        ExtractChunk8(serverMessage);
                        break;
                    case 9:
                        ExtractChunk9(serverMessage);
                        break;
                    case 10:
                        ExtractChunk10(serverMessage);
                        break;
                    case 11:
                        ExtractChunk11(serverMessage);
                        break;
                    case 12:
                        ExtractChunk12(serverMessage);
                        break;
                }
            }

            public static void ExtractChunk1(ServerMessage serverMessage)
            {

                processingchunks = true;

                State.previousstate = State.currentstate;
                State.currentstate = new ServerState();

                try
                {
                    State.currentstate.client = serverMessage.client;
                    State.currentstate.clientversion = serverMessage.clientversion;
                    State.currentstate.mode = serverMessage.mode;
                    State.currentstate.type = serverMessage.type;
                    State.currentstate.dcsversion = serverMessage.dcsversion.Length > 5 ? serverMessage.dcsversion.Substring(0, 5) : serverMessage.dcsversion;
                    State.currentstate.root = serverMessage.root;
                    State.currentstate.multiplayer = serverMessage.multiplayer;
                    State.currentstate.vrmode = serverMessage.vrmode;
                    State.currentstate.easycomms = serverMessage.easycomms;
                    State.currentstate.pausebasestate = serverMessage.pausebasestate;
                    State.currentstate.theatre = serverMessage.theatre;
                    State.currentstate.sortie = serverMessage.sortie;
                    State.currentstate.task = serverMessage.task;
                    State.currentstate.country = serverMessage.country;
                    State.currentstate.options = serverMessage.options;
                }
                catch (Exception e)
                {
                    Log.Write("ERROR 1/" + chunkcount + " :" + e.StackTrace, Colors.Inline);

                }
                receivedupdatecomplete = false;
            }
            public static void ExtractChunk2(ServerMessage serverMessage)
            {
                processingchunks = true;
                try
                {
                    State.currentstate.timer = serverMessage.timer;
                    State.currentstate.tod = serverMessage.tod;
                    State.currentstate.id = serverMessage.id;
                    State.currentstate.playerusername = serverMessage.playerusername;
                    State.currentstate.playercallsign = serverMessage.playercallsign;
                    State.currentstate.playercoalition = serverMessage.playercoalition;
                    State.currentstate.playerunitid = serverMessage.playerunitid;
                    State.currentstate.playerunitcat = serverMessage.playerunitcat;
                    State.currentstate.airborne = serverMessage.airborne;
                    State.currentstate.intercom = serverMessage.intercom;
                    State.currentstate.fsmstate = serverMessage.fsmstate;
                    State.currentstate.selectedradio = serverMessage.selectedradio;
                    State.currentstate.radios = serverMessage.radios;

                    if (!State.currentstate.airborne)
                    {
                        if (State.AH64GeorgeSelectedWeapon != State.AH64GeorgeWeaponMode.NoWeapon)
                        {
                            State.AH64GeorgeSelectedWeapon = State.AH64GeorgeWeaponMode.NoWeapon;
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Write("ERROR 2/" + chunkcount + " :" + e.StackTrace, Colors.Inline);
                }
                receivedupdatecomplete = false;
            }

            public static void ExtractChunk3(ServerMessage serverMessage)
            {

                processingchunks = true;
                try
                {
                    State.currentstate.missiontitle = serverMessage.missiontitle;
                    State.currentstate.missionbriefing = serverMessage.missionbriefing;
                    State.currentstate.missiondetails = serverMessage.missiondetails;
                }
                catch (Exception e)
                {
                    Log.Write("ERROR 3/" + chunkcount + " :" + e.StackTrace, Colors.Inline);
                }
                receivedupdatecomplete = false;
            }

            public static bool CheckSuperCarrier(string checkstr)
            {
                return checkstr.ToLower().Contains("kuznetsov") || checkstr.ToLower().Contains("stennis") || checkstr.ToLower().Contains("roosevelt") || checkstr.ToLower().Contains("lincoln") || checkstr.ToLower().Contains("washington") || checkstr.ToLower().Contains("truman") || checkstr.ToLower().Contains("forrestal");
            }

            public static void ExtractChunk4(ServerMessage serverMessage)
            {

                processingchunks = true;
                try
                {
                    List<string> cats = new List<string>() { "Player", "Flight", "JTAC", "AWACS", "Tanker", "Opposition", "Crew", "Aux", "Cargo" };
                    foreach (string catstr in cats)
                    {
                        try
                        {
                            foreach (DcsUnit a in serverMessage.availablerecipients[catstr])
                            {
                                a.reccat = catstr;
                                if (catstr.Equals("Opposition", StringComparison.OrdinalIgnoreCase))
                                {
                                    a.descr = "OPPOSITION";
                                }
                                else
                                {
                                    a.descr = catdescriptions[catstr];
                                }
                                State.currentstate.availablerecipients[catstr].Add(a);
                            }
                        }
                        catch
                        {
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Write("ERROR 4/" + chunkcount + " :" + e.StackTrace, Colors.Inline);
                }
                receivedupdatecomplete = false;
            }
            public static void ExtractChunk5(ServerMessage serverMessage)
            {
                processingchunks = true;
                try
                {
                    string cat = "ATC";
                    foreach (DcsUnit a in serverMessage.availablerecipients[cat])
                    {
                        a.reccat = cat;
                        a.descr = catdescriptions[cat];
                        if (CheckSuperCarrier(a.callsign + a.fullname))
                        {
                            a.descr = catdescriptions["Carrier"];
                        }
                        State.currentstate.availablerecipients[cat].Add(a);
                    }
                }
                catch (Exception e)
                {
                    Log.Write("ERROR 5/" + chunkcount + " :" + e.StackTrace, Colors.Inline);
                }
                receivedupdatecomplete = false;
            }
            public static void ExtractChunk6(ServerMessage serverMessage)
            {

                processingchunks = true;
                try
                {
                    string cat = "ATC";
                    foreach (DcsUnit a in serverMessage.availablerecipients[cat])
                    {
                        a.reccat = cat;
                        a.descr = catdescriptions[cat];
                        if (CheckSuperCarrier(a.callsign + a.fullname))
                        {
                            a.descr = catdescriptions["Carrier"];
                        }
                        State.currentstate.availablerecipients[cat].Add(a);
                    }
                }
                catch (Exception e)
                {
                    Log.Write("ERROR 6/" + chunkcount + " :" + e.StackTrace, Colors.Inline);
                }
                receivedupdatecomplete = false;
            }

            public static void ExtractChunk7(ServerMessage serverMessage)
            {
                processingchunks = true;
                try
                {
                    string cat = "Allies";
                    if (serverMessage.availablerecipients == null
                        || !serverMessage.availablerecipients.ContainsKey(cat)
                        || serverMessage.availablerecipients[cat] == null
                        || State.currentstate.availablerecipients == null
                        || !State.currentstate.availablerecipients.ContainsKey(cat)
                        || !State.currentstate.availablerecipients.ContainsKey("Player"))
                    {
                        receivedupdatecomplete = false;
                        return;
                    }

                    int playerId = (State.currentstate.availablerecipients["Player"] != null
                        && State.currentstate.availablerecipients["Player"].Count > 0
                        && State.currentstate.availablerecipients["Player"][0] != null)
                        ? State.currentstate.availablerecipients["Player"][0].id_
                        : -1;

                    foreach (DcsUnit a in serverMessage.availablerecipients[cat])
                    {
                        if (a == null)
                        {
                            continue;
                        }

                        if (playerId < 0 || !a.id_.Equals(playerId))
                        {
                            a.reccat = cat;
                            a.descr = catdescriptions[cat];
                            State.currentstate.availablerecipients[cat].Add(a);
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Write("ERROR 7/" + chunkcount + " :" + e.Data, Colors.Inline);
                }
                receivedupdatecomplete = false;
            }
            public static void ExtractChunk8(ServerMessage serverMessage)
            {
                processingchunks = true;
                try
                {
                    string cat = "Allies";
                    if (serverMessage.availablerecipients == null
                        || !serverMessage.availablerecipients.ContainsKey(cat)
                        || serverMessage.availablerecipients[cat] == null
                        || State.currentstate.availablerecipients == null
                        || !State.currentstate.availablerecipients.ContainsKey(cat)
                        || !State.currentstate.availablerecipients.ContainsKey("Player"))
                    {
                        receivedupdatecomplete = false;
                        return;
                    }

                    int playerId = (State.currentstate.availablerecipients["Player"] != null
                        && State.currentstate.availablerecipients["Player"].Count > 0
                        && State.currentstate.availablerecipients["Player"][0] != null)
                        ? State.currentstate.availablerecipients["Player"][0].id_
                        : -1;

                    foreach (DcsUnit a in serverMessage.availablerecipients[cat])
                    {
                        if (a == null)
                        {
                            continue;
                        }

                        if (playerId < 0 || !a.id_.Equals(playerId))
                        {
                            a.reccat = cat;
                            a.descr = catdescriptions[cat];
                            State.currentstate.availablerecipients[cat].Add(a);
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Write("ERROR 8/" + chunkcount + " :" + e.StackTrace, Colors.Inline);
                }
                receivedupdatecomplete = false;
            }
            public static void ExtractChunk9(ServerMessage serverMessage)
            {
                processingchunks = true;
                try
                {
                    if (serverMessage.menuaux != null)
                    {
                        State.currentstate.menuaux = serverMessage.menuaux;
                        State.currentstate.menucargo = serverMessage.menucargo;
                    }
                }
                catch (Exception e)
                {
                    Log.Write("ERROR 9/" + chunkcount + " :" + e.StackTrace, Colors.Inline);
                }
                receivedupdatecomplete = false;
            }
            public static void ExtractChunk10(ServerMessage serverMessage)
            {
                processingchunks = true;
                try
                {
                    State.currentstate.riostate = serverMessage.riostate;
                    State.currentstate.bpos = serverMessage.bpos;
                    State.currentstate.cpos = serverMessage.cpos;
                    State.currentstate.viewexternal = !State.currentstate.cpos.type.Equals(0);
                    State.currentstate.soundsallowexternal = State.currentstate.options.sound.headphones_on_external_views;

                }
                catch (Exception e)
                {
                    Log.Write("ERROR 10/" + chunkcount + " :" + e.StackTrace + " " + e.Message, Colors.Inline);
                }
                receivedupdatecomplete = false;
            }
            public static void ExtractChunk11(ServerMessage serverMessage)
            {
                processingchunks = true;
                try
                {
                    State.currentstate.payload = serverMessage.payload;
                }
                catch (Exception e)
                {
                    Log.Write("ERROR 11/" + chunkcount + " :" + e.StackTrace, Colors.Inline);
                }
                receivedupdatecomplete = true;
                processingchunks = false;

            }

            public static void ExtractChunk12(ServerMessage serverMessage)
            {
                processingchunks = true;
                try
                {
                    State.currentstate.metar = serverMessage.metar;
                    State.currentstate.atcmetars = serverMessage.atcmetars ?? new Dictionary<string, string>();
                    State.currentstate.atcicaotypes = serverMessage.atcicaotypes ?? new Dictionary<string, string>();
                    State.currentstate.diagnostics = serverMessage.diagnostics;
                }
                catch (Exception e)
                {
                    Log.Write("ERROR 12/" + chunkcount + " :" + e.StackTrace, Colors.Inline);
                }
                receivedupdatecomplete = true;
                processingchunks = false;
            }

            public static void LogFlightUnits(ServerMessage serverMessage)
            {
                Log.Write($"Flight units in serverMessage: {serverMessage.availablerecipients["Flight"]?.Count ?? 0}", Colors.Text);
                Log.Write($"Flight units in State: {State.currentstate.availablerecipients["Flight"].Count}", Colors.Text);
            }
        }
    }
}
