using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VAICOM.Extensions.AICPG;
using VAICOM.Static;

namespace VAICOM
{
    namespace Servers
    {

        public static partial class Server
        {
            private static int lastDiagnosticsMdSrc = -1;
            private static int lastDiagnosticsMdFeat = -1;
            private static string lastMissionDrawingsMissionKey = "";
            private static string pendingMissionDrawingsMissionKey = "";
            private static int pendingMissionDrawingsMissionKeyConfirmCount = 0;
            private static readonly object missionDrawingsCacheLogSync = new object();
            private static DateTime missionDrawingsCacheSummaryLastUtc = DateTime.MinValue;
            private static int missionDrawingsMissingCount = 0;
            private static int missionDrawingsWithPayloadCount = 0;
            private static int missionDrawingsAcceptedCount = 0;
            private static int missionDrawingsIgnoredCount = 0;
            private static string missionDrawingsLastSummaryKey = "";
            private static string missionDrawingsTransferId = "";
            private static int missionDrawingsTransferPartCount = 0;
            private static DateTime missionDrawingsTransferStartedUtc = DateTime.MinValue;
            private static Dictionary<int, string> missionDrawingsTransferParts = new Dictionary<int, string>();

            private static void AppendMissionDrawingsCacheLog(string line)
            {
                try
                {
                    if (State.activeconfig == null || !State.activeconfig.Debugmode)
                    {
                        return;
                    }

                    string logsFolder = Path.Combine(State.VA_APPS, Products.Products.Families.Vaicom.VaicomProPlugin.rootfoldername, AppData.SubFolders["logfiles"]);
                    string filePath = Path.Combine(logsFolder, "VAICOMPRO.MissionDrawingsCache.log");

                    lock (missionDrawingsCacheLogSync)
                    {
                        Directory.CreateDirectory(logsFolder);
                        File.AppendAllText(filePath, DateTime.UtcNow.ToString("o") + " | " + line + Environment.NewLine);
                    }
                }
                catch
                {
                }
            }

            private static void AppendMissionDrawingsCacheSummary(string missionTitle, string theatre, string stableMissionKey, bool force)
            {
                try
                {
                    DateTime nowUtc = DateTime.UtcNow;
                    if (!force)
                    {
                        if ((nowUtc - missionDrawingsCacheSummaryLastUtc).TotalSeconds < 10)
                        {
                            return;
                        }
                    }

                    missionDrawingsCacheSummaryLastUtc = nowUtc;
                    missionDrawingsLastSummaryKey = stableMissionKey ?? "";

                    string line = "summary"
                        + "|title=" + (missionTitle ?? "")
                        + "|theatre=" + (theatre ?? "")
                        + "|stableKey=" + (stableMissionKey ?? "")
                        + "|payloadMissing=" + missionDrawingsMissingCount
                        + "|payloadPresent=" + missionDrawingsWithPayloadCount
                        + "|accepted=" + missionDrawingsAcceptedCount
                        + "|ignored=" + missionDrawingsIgnoredCount;

                    AppendMissionDrawingsCacheLog(line);
                }
                catch
                {
                }
            }

            public static void UpdateServerState(ServerMessage serverMessage) // gets all chuncks
            {
                ExtractAll(serverMessage);

                if (receivedupdatecomplete)
                {
                    if (!processingchunks)
                    {
                        ProcessServerData();
                    }
                }
            }

            private static string ResolveStableMissionDrawingsMissionKey(string missionKey, out bool missionKeyChanged)
            {
                missionKeyChanged = false;
                string normalizedMissionKey = (missionKey ?? "").Trim();

                if (string.IsNullOrWhiteSpace(normalizedMissionKey))
                {
                    pendingMissionDrawingsMissionKey = "";
                    pendingMissionDrawingsMissionKeyConfirmCount = 0;
                    return lastMissionDrawingsMissionKey;
                }

                if (string.Equals(normalizedMissionKey, lastMissionDrawingsMissionKey, StringComparison.Ordinal))
                {
                    pendingMissionDrawingsMissionKey = "";
                    pendingMissionDrawingsMissionKeyConfirmCount = 0;
                    return lastMissionDrawingsMissionKey;
                }

                if (string.Equals(normalizedMissionKey, pendingMissionDrawingsMissionKey, StringComparison.Ordinal))
                {
                    pendingMissionDrawingsMissionKeyConfirmCount++;
                }
                else
                {
                    pendingMissionDrawingsMissionKey = normalizedMissionKey;
                    pendingMissionDrawingsMissionKeyConfirmCount = 1;
                }

                if (pendingMissionDrawingsMissionKeyConfirmCount >= 3)
                {
                    lastMissionDrawingsMissionKey = normalizedMissionKey;
                    pendingMissionDrawingsMissionKey = "";
                    pendingMissionDrawingsMissionKeyConfirmCount = 0;
                    missionKeyChanged = true;
                }

                return lastMissionDrawingsMissionKey;
            }

            public static void DumpStateToLog()
            {
                Log.Reset();
                string state = JsonConvert.SerializeObject(State.currentstate, Formatting.Indented).ToString();
                Log.Write("STATE: " + state, Colors.Critical);
            }

            public static int chunkcount = 13;

            private static bool IsValidOwnshipVector(Vector pos)
            {
                if (pos == null)
                {
                    return false;
                }

                return !(double.IsNaN(pos.x) || double.IsInfinity(pos.x)
                    || double.IsNaN(pos.y) || double.IsInfinity(pos.y)
                    || double.IsNaN(pos.z) || double.IsInfinity(pos.z));
            }

            private static string BuildMissionDrawingsMissionKey(string theatre, string missionTitle)
            {
                string normalizedTheatre = (theatre ?? "").Trim().ToUpperInvariant();
                string normalizedTitle = (missionTitle ?? "").Trim().ToUpperInvariant();

                if (string.IsNullOrWhiteSpace(normalizedTheatre)
                    || string.IsNullOrWhiteSpace(normalizedTitle))
                {
                    return "";
                }

                return normalizedTheatre + "|" + normalizedTitle;
            }

            private static bool HasMissionDrawingObjects(object missionDrawings)
            {
                if (missionDrawings == null)
                {
                    return false;
                }

                try
                {
                    JToken token = missionDrawings as JToken ?? JToken.FromObject(missionDrawings);
                    if (token == null || token.Type != JTokenType.Object)
                    {
                        return false;
                    }

                    JArray layers = token["layers"] as JArray ?? token["Layers"] as JArray;
                    if (layers == null || layers.Count == 0)
                    {
                        return false;
                    }

                    foreach (JToken layer in layers)
                    {
                        JArray objects = layer?["objects"] as JArray ?? layer?["Objects"] as JArray;
                        if (objects != null && objects.Count > 0)
                        {
                            return true;
                        }
                    }

                    return false;
                }
                catch
                {
                    return false;
                }
            }

            public static void ExtractAll(ServerMessage serverMessage)
            {
                // The final received message contains the "completed" property
                // indicating that all chunks have been sent and can now be processed.
                if (serverMessage.completed)
                {
                    receivedupdatecomplete = true;
                    processingchunks = false;
                    return;
                }

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
                    case 13:
                        ExtractChunk13(serverMessage);
                        break;
                }
            }

            private static void ResetMissionDrawingsCounters()
            {
                missionDrawingsMissingCount = 0;
                missionDrawingsWithPayloadCount = 0;
                missionDrawingsAcceptedCount = 0;
                missionDrawingsIgnoredCount = 0;
            }

            private static void ResetMissionDrawingsTransfer()
            {
                missionDrawingsTransferId = "";
                missionDrawingsTransferPartCount = 0;
                missionDrawingsTransferStartedUtc = DateTime.MinValue;
                missionDrawingsTransferParts = new Dictionary<int, string>();
            }

            private static object TryReassembleMissionDrawings(ServerMessage serverMessage)
            {
                if (serverMessage == null)
                {
                    return null;
                }

                string transferId = (serverMessage.md_transferid ?? "").Trim();
                int partIndex = serverMessage.md_partindex ?? 0;
                int partCount = serverMessage.md_partcount ?? 0;
                string part = serverMessage.md_part ?? "";

                if (string.IsNullOrWhiteSpace(transferId)
                    || partIndex <= 0
                    || partCount <= 0
                    || string.IsNullOrEmpty(part))
                {
                    return null;
                }

                DateTime nowUtc = DateTime.UtcNow;
                bool isExpired = missionDrawingsTransferStartedUtc != DateTime.MinValue
                    && (nowUtc - missionDrawingsTransferStartedUtc).TotalSeconds > 20;

                if (isExpired
                    || !string.Equals(missionDrawingsTransferId, transferId, StringComparison.Ordinal)
                    || missionDrawingsTransferPartCount != partCount)
                {
                    ResetMissionDrawingsTransfer();
                    missionDrawingsTransferId = transferId;
                    missionDrawingsTransferPartCount = partCount;
                    missionDrawingsTransferStartedUtc = nowUtc;
                }

                missionDrawingsTransferParts[partIndex] = part;

                if (missionDrawingsTransferParts.Count < missionDrawingsTransferPartCount)
                {
                    return null;
                }

                StringBuilder writer = new StringBuilder();
                for (int i = 1; i <= missionDrawingsTransferPartCount; i++)
                {
                    string piece;
                    if (!missionDrawingsTransferParts.TryGetValue(i, out piece))
                    {
                        return null;
                    }

                    writer.Append(piece);
                }

                string combined = writer.ToString();
                if (string.IsNullOrWhiteSpace(combined))
                {
                    return null;
                }

                try
                {
                    JToken token = JToken.Parse(combined);
                    ResetMissionDrawingsTransfer();
                    return token;
                }
                catch
                {
                    ResetMissionDrawingsTransfer();
                    return null;
                }
            }

            private static void IngestMissionDrawingsPayload(ServerMessage serverMessage, string stableMissionKey, string sourceLabel)
            {
                if (serverMessage != null && serverMessage.missiondrawings != null)
                {
                    bool incomingHasObjects = HasMissionDrawingObjects(serverMessage.missiondrawings);
                    missionDrawingsWithPayloadCount++;

                    if (incomingHasObjects)
                    {
                        State.currentstate.missiondrawings = serverMessage.missiondrawings;
                        Log.Write($"Mission drawings snapshot received in {sourceLabel}.", Colors.Text);
                        missionDrawingsAcceptedCount++;
                    }
                    else
                    {
                        missionDrawingsIgnoredCount++;
                    }
                }
                else
                {
                    missionDrawingsMissingCount++;
                }

                bool summaryKeyChanged = !string.Equals(missionDrawingsLastSummaryKey ?? "", stableMissionKey ?? "", StringComparison.Ordinal);
                AppendMissionDrawingsCacheSummary(State.currentstate.missiontitle, State.currentstate.theatre, stableMissionKey, summaryKeyChanged);
            }

            public static void ExtractChunk1(ServerMessage serverMessage)
            {
                processingchunks = true;

                State.previousstate = State.currentstate;
                State.currentstate = new ServerState();

                // Keep the last known ownship/camera position alive while new chunks stream in.
                if (State.previousstate != null)
                {
                    if (State.previousstate.missiondrawings != null)
                    {
                        State.currentstate.missiondrawings = State.previousstate.missiondrawings;
                    }

                    if (IsValidOwnshipVector(State.previousstate.bpos))
                    {
                        State.currentstate.bpos = State.previousstate.bpos;
                    }

                    if (State.previousstate.cpos != null)
                    {
                        State.currentstate.cpos = State.previousstate.cpos;
                    }
                }

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

                    string missionKey = BuildMissionDrawingsMissionKey(
                        State.currentstate.theatre,
                        State.currentstate.missiontitle);

                    bool stableMissionKeyChanged;
                    string stableMissionKey = ResolveStableMissionDrawingsMissionKey(missionKey, out stableMissionKeyChanged);

                    if (stableMissionKeyChanged
                        && !string.IsNullOrWhiteSpace(stableMissionKey))
                    {
                        Log.Write("Mission change detected; keeping cached mission drawings until replacement snapshot arrives.", Colors.Text);
                        ResetMissionDrawingsCounters();
                        AppendMissionDrawingsCacheLog("mission-key-changed|key=" + stableMissionKey + "|action=keep-cache");
                        AppendMissionDrawingsCacheSummary(State.currentstate.missiontitle, State.currentstate.theatre, stableMissionKey, true);
                    }
                    if (serverMessage.fuel_unit_mass_max.HasValue && !double.IsNaN(serverMessage.fuel_unit_mass_max.Value) && serverMessage.fuel_unit_mass_max.Value > 0)
                    {
                        State.currentstate.fuel_unit_mass_max = serverMessage.fuel_unit_mass_max.Value;
                    }
                }
                catch (Exception e)
                {
                    Log.Write("ERROR 3/" + chunkcount + " :" + e.StackTrace, Colors.Inline);
                }
                receivedupdatecomplete = false;
            }

            public static void ExtractChunk13(ServerMessage serverMessage)
            {
                processingchunks = true;
                try
                {
                    string missionKey = BuildMissionDrawingsMissionKey(
                        State.currentstate.theatre,
                        State.currentstate.missiontitle);

                    bool stableMissionKeyChanged;
                    string stableMissionKey = ResolveStableMissionDrawingsMissionKey(missionKey, out stableMissionKeyChanged);

                    if (stableMissionKeyChanged
                        && !string.IsNullOrWhiteSpace(stableMissionKey))
                    {
                        Log.Write("Mission change detected; keeping cached mission drawings until replacement snapshot arrives.", Colors.Text);
                        ResetMissionDrawingsCounters();
                        ResetMissionDrawingsTransfer();
                        AppendMissionDrawingsCacheLog("mission-key-changed|key=" + stableMissionKey + "|action=keep-cache");
                        AppendMissionDrawingsCacheSummary(State.currentstate.missiontitle, State.currentstate.theatre, stableMissionKey, true);
                    }

                    object payload = null;
                    if (serverMessage.missiondrawings != null)
                    {
                        payload = serverMessage.missiondrawings;
                    }
                    else
                    {
                        payload = TryReassembleMissionDrawings(serverMessage);
                    }

                    if (payload != null)
                    {
                        serverMessage.missiondrawings = payload;
                    }

                    IngestMissionDrawingsPayload(serverMessage, stableMissionKey, "chunk 13");
                }
                catch (Exception e)
                {
                    Log.Write("ERROR 13/" + chunkcount + " :" + e.StackTrace, Colors.Inline);
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

                    // Only replace ownship position when the incoming chunk contains valid numbers.
                    if (IsValidOwnshipVector(serverMessage.bpos))
                    {
                        State.currentstate.bpos = serverMessage.bpos;
                    }

                    if (serverMessage.cpos != null)
                    {
                        State.currentstate.cpos = serverMessage.cpos;
                    }

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
                receivedupdatecomplete = false;
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
                    LogMissionDrawingsDiagnostics(State.currentstate.diagnostics);
                }
                catch (Exception e)
                {
                    Log.Write("ERROR 12/" + chunkcount + " :" + e.StackTrace, Colors.Inline);
                }
                receivedupdatecomplete = false;
            }

            private static void LogMissionDrawingsDiagnostics(object diagnostics)
            {
                try
                {
                    if (diagnostics == null)
                    {
                        return;
                    }

                    JToken token = diagnostics as JToken;
                    if (token == null)
                    {
                        token = JToken.FromObject(diagnostics);
                    }

                    if (token == null || token.Type != JTokenType.Object)
                    {
                        return;
                    }

                    int mdSrc = token.Value<int?>("mdSrc") ?? -1;
                    int mdFeat = token.Value<int?>("mdFeat") ?? -1;
                    int mdTxCount = token.Value<int?>("mdTxCount") ?? -1;
                    bool? mdTxLocked = token.Value<bool?>("mdTxLocked");
                    string mdTxReason = token.Value<string>("mdTxReason") ?? "";
                    int mdTxNextRetryIn = token.Value<int?>("mdTxNextRetryIn") ?? -1;
                    int mdTxChunk13Bytes = token.Value<int?>("mdTxChunk13Bytes") ?? -1;
                    bool? mdTxChunk13HasTable = token.Value<bool?>("mdTxChunk13HasTable");
                    bool? mdTxChunk13HasKey = token.Value<bool?>("mdTxChunk13HasKey");
                    bool? mdTxChunk13SentOk = token.Value<bool?>("mdTxChunk13SentOk");
                    if (mdSrc < 0 && mdFeat < 0)
                    {
                        return;
                    }

                    if (mdSrc != lastDiagnosticsMdSrc || mdFeat != lastDiagnosticsMdFeat)
                    {
                        lastDiagnosticsMdSrc = mdSrc;
                        lastDiagnosticsMdFeat = mdFeat;
                        string txSuffix = "";
                        if (mdTxCount >= 0 || mdTxLocked.HasValue || !string.IsNullOrWhiteSpace(mdTxReason) || mdTxNextRetryIn >= 0
                            || mdTxChunk13Bytes >= 0 || mdTxChunk13HasTable.HasValue || mdTxChunk13HasKey.HasValue || mdTxChunk13SentOk.HasValue)
                        {
                            txSuffix = $", txCount={Math.Max(0, mdTxCount)}, txLocked={(mdTxLocked.HasValue ? mdTxLocked.Value.ToString().ToLowerInvariant() : "n/a")}, txReason={mdTxReason}, txNextRetryIn={mdTxNextRetryIn}, tx13Bytes={Math.Max(0, mdTxChunk13Bytes)}, tx13HasTable={(mdTxChunk13HasTable.HasValue ? mdTxChunk13HasTable.Value.ToString().ToLowerInvariant() : "n/a")}, tx13HasKey={(mdTxChunk13HasKey.HasValue ? mdTxChunk13HasKey.Value.ToString().ToLowerInvariant() : "n/a")}, tx13SentOk={(mdTxChunk13SentOk.HasValue ? mdTxChunk13SentOk.Value.ToString().ToLowerInvariant() : "n/a")}";
                        }
                        Log.Write($"Mission drawings diagnostics | mdSrc={mdSrc}, mdFeat={mdFeat}{txSuffix}", Colors.Text);
                    }
                }
                catch
                {
                }
            }

            public static void LogFlightUnits(ServerMessage serverMessage)
            {
                Log.Write($"Flight units in serverMessage: {serverMessage.availablerecipients["Flight"]?.Count ?? 0}", Colors.Text);
                Log.Write($"Flight units in State: {State.currentstate.availablerecipients["Flight"].Count}", Colors.Text);
            }
        }
    }
}
