using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using VAICOM.Servers;
using VAICOM.Static;

namespace VAICOM
{
    namespace Extensions
    {
        namespace Kneeboard
        {

            // core class for kneeboard messages

            public class KneeboardMessage
            {
                public int eventid;
                public bool? dictmode;
                public double opacity;
                public bool? autoswitch;
                public bool? switchpage;

                // payload:

                public KneeboardServerData serverdata;
                public KneeboardUnitsData unitsdata;
                public KneeboardUnitsDetails unitsdetails;
                public LogData logdata;
                public AliasData aliasdata;

                public KneeboardMessage()
                {
                    eventid = 4000; // default
                    opacity = State.activeconfig.KneeboardOpacity;
                    dictmode = State.Proxy.Dictation.IsOn();
                    autoswitch = State.activeconfig.KneeboardlinkPTT;
                    switchpage = false; // only when explicitly set true 
                }
            }

            public class AliasData
            {
                public string category;
                public SortedDictionary<string, List<string>> content;
                public int chunk;

                public AliasData(string cat, SortedDictionary<string, List<string>> cont)
                {
                    category = cat;
                    content = cont;
                    chunk = 0;
                }
            }

            public class LogData
            {
                public string category;
                public string content;
                public int timer;

                public LogData(string cat, string cont)
                {
                    category = cat;
                    content = cont;
                }
            }

            public class KneeboardServerData
            {
                public string theater;
                public string dcsversion;
                public string aircraft;
                public int flightsize;
                public string playerusername;
                public string playercallsign;
                public string coalition;
                public string sortie;
                public string task;
                public string country;
                public string missiontitle;
                public string missionbriefing;
                public string missiondetails;
                public bool multiplayer;

                public KneeboardServerData()
                {
                    theater = State.currentstate.theatre;
                    dcsversion = State.currentstate.dcsversion;
                    aircraft = State.currentstate.id;
                    flightsize = State.currentstate.availablerecipients["Flight"].Count;
                    playerusername = State.currentstate.playerusername;
                    playercallsign = State.currentstate.playercallsign;
                    coalition = State.currentstate.playercoalition.ToUpper();
                    Int32.TryParse(State.currentstate.sortie, out int daytimeinsecs);
                    int hr = (daytimeinsecs / 3600);
                    int min = (daytimeinsecs - hr * 3600) / 60;
                    sortie = "TO " + KneeboardHelper.theatercode(State.currentstate.theatre) + " MST " + hr.ToString().PadLeft(2, '0') + ":" + min.ToString().PadRight(2, '0') + " " + KneeboardHelper.theatertimezonestring(State.currentstate.theatre);
                    task = State.currentstate.task;
                    country = State.currentstate.country;
                    missiontitle = State.currentstate.missiontitle;
                    missionbriefing = State.currentstate.missionbriefing;
                    missiondetails = State.currentstate.missiondetails;
                    multiplayer = State.currentstate.multiplayer;
                }

            }

            public class KneeboardUnitSummary
            {
                public string cat;
                public string code;
                public string callsign;
                public string alias;
                public string range;
                public string bearing;
                public string alt;
                public string frq; // Primary frequency
                public string frq2; // Secondary frequency
                public string tacan;
                public string istuned;
                public string humanname;
                public List<string> altfreq;

            }

            public class KneeboardUnitsDetails
            {
                public string category;
                public List<string> unitsummary;
                public int timer;

                public KneeboardUnitsDetails(string recipientcat, List<string> contents, bool AOCS)
                {
                    string cat;

                    if (recipientcat.Equals("AOCS"))
                    {
                        cat = "Aux";
                    }
                    else
                    {
                        cat = recipientcat;
                    }

                    category = cat.ToUpper();
                    unitsummary = contents;
                }
            }

            public class KneeboardUnitsData
            {
                private static readonly Dictionary<string, int> LastLoggedUnitsCountByCategory = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

                private static string WrapForKneeboard(string text, int maxLineLength)
                {
                    if (string.IsNullOrWhiteSpace(text) || maxLineLength < 8)
                    {
                        return text;
                    }

                    var words = text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (words.Length == 0)
                    {
                        return text;
                    }

                    var lines = new List<string>();
                    var current = "";

                    foreach (var word in words)
                    {
                        var candidate = string.IsNullOrEmpty(current) ? word : current + " " + word;
                        if (candidate.Length <= maxLineLength)
                        {
                            current = candidate;
                            continue;
                        }

                        if (!string.IsNullOrEmpty(current))
                        {
                            lines.Add(current);
                        }

                        current = word;
                    }

                    if (!string.IsNullOrEmpty(current))
                    {
                        lines.Add(current);
                    }

                    return string.Join("\n", lines);
                }

                private static string ResolveTankerAircraftType(Server.DcsUnit unit)
                {
                    if (unit == null)
                    {
                        return "";
                    }

                    string source = ((unit.fullname ?? "") + " " + (unit.callsign ?? "")).ToUpperInvariant();
                    if (string.IsNullOrWhiteSpace(source))
                    {
                        return "";
                    }

                    if (source.Contains("KC-135MPRS") || source.Contains("KC135MPRS")) return "KC-135MPRS";
                    if (source.Contains("KC-135") || source.Contains("KC135")) return "KC-135";
                    if (source.Contains("KC-130") || source.Contains("KC130")) return "KC-130";
                    if (source.Contains("IL-78") || source.Contains("IL78")) return "IL-78";
                    if (source.Contains("S-3B") || source.Contains("S3B")) return "S-3B";

                    Match kcMatch = Regex.Match(source, @"\b(KC)[-\s]?(\d{2,3}[A-Z]*)\b");
                    if (kcMatch.Success)
                    {
                        return kcMatch.Groups[1].Value + "-" + kcMatch.Groups[2].Value;
                    }

                    Match ilMatch = Regex.Match(source, @"\b(IL)[-\s]?(\d{2,3}[A-Z]*)\b");
                    if (ilMatch.Success)
                    {
                        return ilMatch.Groups[1].Value + "-" + ilMatch.Groups[2].Value;
                    }

                    return "";
                }

                private static bool IsLikelyAwacsUnit(Server.DcsUnit unit)
                {
                    if (unit == null)
                    {
                        return false;
                    }

                    string callsign = (unit.callsign ?? "").Trim();
                    bool isKnownAwacsCallsign = callsign.IndexOf("Darkstar", StringComparison.OrdinalIgnoreCase) >= 0
                        || callsign.IndexOf("Focus", StringComparison.OrdinalIgnoreCase) >= 0
                        || callsign.IndexOf("Magic", StringComparison.OrdinalIgnoreCase) >= 0
                        || callsign.IndexOf("Overlord", StringComparison.OrdinalIgnoreCase) >= 0
                        || callsign.IndexOf("Wizard", StringComparison.OrdinalIgnoreCase) >= 0;

                    string typeSource = ((unit.typename ?? "") + " " + (unit.fullname ?? "")).ToUpperInvariant();
                    bool isAwacsType = typeSource.Contains("HAWKEYE")
                        || typeSource.Contains("SENTRY")
                        || typeSource.Contains("WEDGETAIL")
                        || Regex.IsMatch(typeSource, @"(^|[^A-Z0-9])E[-\s]?2[A-Z]?([^A-Z0-9]|$)")
                        || Regex.IsMatch(typeSource, @"(^|[^A-Z0-9])E[-\s]?3[A-Z]?([^A-Z0-9]|$)")
                        || Regex.IsMatch(typeSource, @"(^|[^A-Z0-9])E[-\s]?7[A-Z]?([^A-Z0-9]|$)")
                        || Regex.IsMatch(typeSource, @"(^|[^A-Z0-9])A[-\s]?50([^A-Z0-9]|$)")
                        || Regex.IsMatch(typeSource, @"(^|[^A-Z0-9])KJ[-\s]?2000([^A-Z0-9]|$)")
                        || Regex.IsMatch(typeSource, @"(^|[^A-Z0-9])KJ[-\s]?500([^A-Z0-9]|$)");

                    return isKnownAwacsCallsign || isAwacsType;
                }

                private static bool IsActiveThreatUnit(Server.DcsUnit unit)
                {
                    if (unit == null)
                    {
                        return false;
                    }

                    if (unit.pos == null)
                    {
                        return false;
                    }

                    string status = (unit.status ?? string.Empty).Trim();
                    if (status.Length == 0)
                    {
                        return true;
                    }

                    string statusUpper = status.ToUpperInvariant();
                    if (statusUpper.Contains("RADARACTIVE:FALSE"))
                    {
                        return false;
                    }

                    string normalized = statusUpper;
                    if (normalized.Contains("DEAD")
                        || normalized.Contains("DESTROYED")
                        || normalized.Contains("INACTIVE")
                        || normalized.Contains("DISABLED"))
                    {
                        return false;
                    }

                    return true;
                }

                private static bool IsOppositionCoalitionThreat(Server.DcsUnit unit)
                {
                    if (unit == null)
                    {
                        return false;
                    }

                    string unitCoalition = (unit.coalition ?? string.Empty).Trim().ToUpperInvariant();
                    string playerCoalition = (State.currentstate?.playercoalition ?? string.Empty).Trim().ToUpperInvariant();
                    if (string.IsNullOrWhiteSpace(unitCoalition) || string.IsNullOrWhiteSpace(playerCoalition))
                    {
                        return true;
                    }

                    return !unitCoalition.Equals(playerCoalition, StringComparison.OrdinalIgnoreCase)
                        && !unitCoalition.Equals("ALLIES", StringComparison.OrdinalIgnoreCase)
                        && !unitCoalition.Equals("FRIENDLY", StringComparison.OrdinalIgnoreCase);
                }

                private static bool IsAirborneThreatUnit(Server.DcsUnit unit)
                {
                    if (unit == null)
                    {
                        return false;
                    }

                    string section = GetOppositionThreatSection(unit);
                    if (!section.Equals("AIR", StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }

                    string status = (unit.status ?? string.Empty).ToUpperInvariant();
                    if (!status.Contains("AIRBORNE:TRUE") && !status.Contains("AIRBORNE:FALSE"))
                    {
                        // If no airborne signal is present for AIR threats,
                        // treat as not airborne to avoid showing taxiing/ground aircraft.
                        return false;
                    }
                    if (status.Contains("AIRBORNE:FALSE"))
                    {
                        return false;
                    }

                    return true;
                }

                private static string GetOppositionThreatSection(Server.DcsUnit unit)
                {
                    if (unit == null)
                    {
                        return string.Empty;
                    }

                    string typeSource = string.Join(" ", new[]
                    {
                        unit.typename ?? string.Empty,
                        unit.fullname ?? string.Empty,
                        unit.descr ?? string.Empty,
                        unit.callsign ?? string.Empty,
                    }).ToUpperInvariant();

                    if (string.IsNullOrWhiteSpace(typeSource))
                    {
                        return string.Empty;
                    }

                    string tokenizedTypeSource = " " + Regex.Replace(typeSource, @"[^A-Z0-9]+", " ").Trim() + " ";

                    // Exclude non-threat static infrastructure (airfields/helipads/FARPs).
                    if (typeSource.Contains("FARP")
                        || typeSource.Contains("HELIPAD")
                        || typeSource.Contains("HELI PAD")
                        || typeSource.Contains("HELIPORT")
                        || typeSource.Contains("AIRBASE")
                        || typeSource.Contains("AIRFIELD")
                        || typeSource.Contains("AIRDROME")
                        || typeSource.Contains("RUNWAY")
                        || typeSource.Contains("FOB")
                        || typeSource.Contains("CAMP")
                        || Regex.IsMatch(typeSource, @"\bHS\d{1,3}\b"))
                    {
                        return string.Empty;
                    }

                    // Exclude SAM launchers; include radar emitters only.
                    bool isLauncher = typeSource.Contains("LAUNCHER")
                        || Regex.IsMatch(typeSource, @"(^|[^A-Z0-9])TEL([^A-Z0-9]|$)")
                        || Regex.IsMatch(typeSource, @"(^|[^A-Z0-9])TELAR([^A-Z0-9]|$)")
                        || Regex.IsMatch(typeSource, @"(^|[^A-Z0-9])LN([^A-Z0-9]|$)");

                    if (isLauncher)
                    {
                        return string.Empty;
                    }

                    bool isEwr = typeSource.Contains("EWR") || typeSource.Contains("EARLY WARNING");
                    bool hasSamToken = tokenizedTypeSource.Contains(" SAM ")
                        || typeSource.Contains("SAM ")
                        || typeSource.Contains(" SAM")
                        || Regex.IsMatch(typeSource, @"(^|[^A-Z0-9])SA[\-_ ]?\d{1,3}([^A-Z0-9]|$)");
                    bool hasSrStrToken = tokenizedTypeSource.Contains(" SR ") || tokenizedTypeSource.Contains(" STR ");
                    bool hasRadarToken = tokenizedTypeSource.Contains(" RADAR ");
                    bool isSamRadarSimpleTest = hasSamToken && (hasRadarToken || hasSrStrToken);
                    bool isKnownSamRadarType = typeSource.Contains("KUB 1S91") // SA-6 straight flush
                        || typeSource.Contains("40B6M TR") // S-300PS/S-300PMU1/S-300PMU2 Flap Lid
                        || typeSource.Contains("40B6MD SR") // S-300PS/S-300PMU1/S-300PMU2 Clam Shell
                        || typeSource.Contains("64H6E SR") // S-300PMU1/S-300PMU2 Big Bird
                        || typeSource.Contains("9S18M1") // SA-11 Snow Drift
                        || typeSource.Contains("DOG EAR RADAR") // Dog Ear SA-8/SA-9/SA-13
                        || typeSource.Contains("HAWK TR") // Hawk
                        || typeSource.Contains("HAWK SR") // Hawk
                        || typeSource.Contains("HAWK CWAR") // Hawk
                        || typeSource.Contains("PATRIOT STR") // Patriot
                        || typeSource.Contains("P-19 S-125 SR") // SA-3/SA-2
                        || typeSource.Contains("P 19 S 125 SR") // SA-3/SA-2
                        || typeSource.Contains("ROLAND RADAR") // Roland
                        || typeSource.Contains("SNR S-125 TR") // SA-3/SA-2
                        || typeSource.Contains("SNR S 125 TR") // SA-3/SA-2
                        || typeSource.Contains("HQ-7 STR") // HQ-7 STR SP
                        || typeSource.Contains("HQ 7 STR") // HQ-7 STR SP
                        || typeSource.Contains("MPQ64F1") // NASAMS SR 
                        || typeSource.Contains("NASAMS RADAR") // NASAMS 
                        || typeSource.Contains("P14_SR") // SA-5 Tall King
                        || typeSource.Contains("SA-5 SR") // SA-5 Tall King
                        || typeSource.Contains("RLS_19J6") // SA-5 Tin Shield
                        || typeSource.Contains("TIN SHIELD"); // SA-5 Tin Shield
                    bool hasSrStrMarker = Regex.IsMatch(typeSource, @"(^|[^A-Z0-9])(SR|STR)([^A-Z0-9]|$)")
                        || Regex.IsMatch(typeSource, @"\b[A-Z0-9]+(?:[\s_\-\./]+[A-Z0-9]+)*[\s_\-\./]+(SR|STR)\b")
                        || Regex.IsMatch(typeSource, @"\b(SR|STR)[\s_\-\./]+[A-Z0-9]+(?:[\s_\-\./]+[A-Z0-9]+)*\b")
                        || Regex.IsMatch(typeSource, @"(^|[^A-Z0-9])(SR|STR)[A-Z0-9_\-]+([^A-Z0-9]|$)")
                        || Regex.IsMatch(typeSource, @"(^|[^A-Z0-9])[A-Z0-9_\-]+(SR|STR)([^A-Z0-9]|$)");

                    bool isSamSearchRadar = typeSource.Contains("SEARCH RADAR")
                        || isSamRadarSimpleTest
                        || hasSrStrToken
                        || (hasSamToken && hasSrStrToken)
                        || isKnownSamRadarType
                        || hasSrStrMarker;
                        
                        

                    if (isEwr)
                    {
                        return "EWR";
                    }

                    if (isSamSearchRadar)
                    {
                        return "SAM";
                    }

                    // Keep aircraft contacts (fixed-wing + rotary) from opposition.
                    bool isAircraft = Regex.IsMatch(typeSource,
                        @"(^|[^A-Z0-9])(F|A|B|C|E|J|MIG|SU|TU|IL|AN|KC|MQ|RQ|P|FW|BF|UH|AH|CH|KA|MI|OH|SH|SA)[\-_ ]?\d{1,3}[A-Z0-9]{0,6}([^A-Z0-9]|$)")
                        || typeSource.Contains("SPITFIRE")
                        || typeSource.Contains("MIRAGE")
                        || typeSource.Contains("RAFALE")
                        || typeSource.Contains("TORNADO")
                        || typeSource.Contains("GRIPEN")
                        || typeSource.Contains("FIGHTER")
                        || typeSource.Contains("BOMBER")
                        || typeSource.Contains("ATTACK")
                        || typeSource.Contains("AIRCRAFT")
                        || typeSource.Contains("HELI")
                        || typeSource.Contains("HELICOPTER");

                    if (isAircraft)
                    {
                        return "AIR";
                    }

                    return string.Empty;
                }

                private static bool IsOppositionAircraftThreat(Server.DcsUnit unit)
                {
                    return !string.IsNullOrWhiteSpace(GetOppositionThreatSection(unit));
                }

                private static string ExtractIcaoFromMetar(string metar)
                {
                    if (string.IsNullOrWhiteSpace(metar))
                    {
                        return null;
                    }

                    Match m = Regex.Match(metar, @"\bMETAR\s+([A-Z]{4})\b", RegexOptions.IgnoreCase);
                    if (m.Success)
                    {
                        return m.Groups[1].Value.ToUpperInvariant();
                    }

                    return null;
                }

                private static string ResolveAtcDisplayCallsign(Server.DcsUnit unit)
                {
                    if (unit == null || State.currentstate == null || State.currentstate.atcmetars == null)
                    {
                        return unit != null ? unit.callsign : string.Empty;
                    }

                    string[] keys =
                    {
                        unit.callsign,
                        unit.fullname,
                        unit.callsign != null ? unit.callsign.ToUpperInvariant() : null,
                        unit.fullname != null ? unit.fullname.ToUpperInvariant() : null,
                    };

                    foreach (string key in keys)
                    {
                        if (string.IsNullOrWhiteSpace(key))
                        {
                            continue;
                        }

                        if (State.currentstate.atcmetars.TryGetValue(key, out string metar))
                        {
                            string icao = ExtractIcaoFromMetar(metar);
                            if (!string.IsNullOrWhiteSpace(icao))
                            {
                                return icao;
                            }
                        }
                    }

                    return unit.callsign;
                }

                private static string FormatDisplayCallsign(string callsign)
                {
                    string value = (callsign ?? "").Trim();
                    if (value.Length == 0)
                    {
                        return value;
                    }

                    value = Regex.Replace(value, @"([A-Za-z])(\d)", "$1 $2");
                    return Regex.Replace(value, @"\s{2,}", " ");
                }

                private static string UppercaseOutsideQuotes(string text)
                {
                    if (string.IsNullOrWhiteSpace(text))
                    {
                        return text;
                    }

                    var result = new System.Text.StringBuilder(text.Length);
                    bool inQuotes = false;
                    for (int i = 0; i < text.Length; i++)
                    {
                        char c = text[i];
                        if (c == '"')
                        {
                            inQuotes = !inQuotes;
                            result.Append(c);
                            continue;
                        }

                        result.Append(inQuotes ? c : char.ToUpperInvariant(c));
                    }

                    return result.ToString();
                }

                private static string GetOppositionTypeKey(Server.DcsUnit unit)
                {
                    return ((unit?.typename ?? unit?.fullname ?? "unknown").Trim()).ToUpperInvariant();
                }

                private static double GetSurfaceDistanceMeters(Server.DcsUnit a, Server.DcsUnit b)
                {
                    if (a?.pos == null || b?.pos == null)
                    {
                        return double.MaxValue;
                    }

                    double dx = a.pos.x - b.pos.x;
                    double dz = a.pos.z - b.pos.z;
                    return Math.Sqrt((dx * dx) + (dz * dz));
                }

                public string category;
                public List<string> unitslist;
                public int timer;

                public KneeboardUnitsData(string recipientcat, bool AOCS)
                {
                    category = recipientcat.ToUpper();

                    string cat;

                    if (recipientcat.Equals("AOCS"))
                    {
                        cat = "Aux";
                    }
                    else
                    {
                        cat = recipientcat;
                    }

                    unitslist = new List<string>();
                    List<KneeboardUnitSummary> units = new List<KneeboardUnitSummary>();

                    if (cat.Equals("AWACS", StringComparison.OrdinalIgnoreCase))
                    {
                        List<Server.DcsUnit> awacsUnits = new List<Server.DcsUnit>();
                        if (State.currentstate.availablerecipients.ContainsKey("AWACS")
                            && State.currentstate.availablerecipients["AWACS"] != null)
                        {
                            awacsUnits = State.currentstate.availablerecipients["AWACS"]
                                .Where(IsLikelyAwacsUnit)
                                .OrderBy(u => u.range)
                                .ToList();
                        }

                        bool awacsAvailable = awacsUnits.Count > 0;

                        unitslist.Add("AWACS:");

                        if (awacsAvailable)
                        {
                            foreach (Server.DcsUnit awacs in awacsUnits)
                            {
                                string awacsLine = awacs.getfreqstr() + " " + FormatDisplayCallsign(awacs.callsign) + " " + awacs.getbearingstr() + "/" + awacs.getrangestr() + "/" + awacs.getaltstr();
                                if (!string.IsNullOrWhiteSpace(awacs.tacan))
                                {
                                    awacsLine += " " + awacs.tacan;
                                }
                                unitslist.Add(awacsLine);
                            }
                        }
                        else
                        {
                            unitslist.Add("No AWACS available.");
                        }

                        unitslist.Add("");
                        unitslist.Add("Datalink Threats:");

                        if (!awacsAvailable)
                        {
                            unitslist.Add("No Datalink threats are displayed.");
                            return;
                        }

                        List<Server.DcsUnit> opposition = new List<Server.DcsUnit>();
                        if (State.currentstate.availablerecipients.ContainsKey("Opposition")
                            && State.currentstate.availablerecipients["Opposition"] != null)
                        {
                            opposition = State.currentstate.availablerecipients["Opposition"];
                        }

                        const double contactMergeDistanceMeters = 5d * 1852d;
                        string[] threatSectionOrder = { "AIR", "SAM", "EWR" };
                        Dictionary<string, List<List<Server.DcsUnit>>> groupedThreatsBySection = new Dictionary<string, List<List<Server.DcsUnit>>>(StringComparer.OrdinalIgnoreCase)
                        {
                            ["AIR"] = new List<List<Server.DcsUnit>>(),
                            ["SAM"] = new List<List<Server.DcsUnit>>(),
                            ["EWR"] = new List<List<Server.DcsUnit>>(),
                        };

                        foreach (Server.DcsUnit threat in opposition
                            .Where(IsActiveThreatUnit)
                            .Where(IsAirborneThreatUnit)
                            .Where(IsOppositionCoalitionThreat)
                            .Where(IsOppositionAircraftThreat)
                            .Where(u => !State.currentstate.multiplayer || !u.ishuman)
                            .OrderBy(u => u.range))
                        {
                            string section = GetOppositionThreatSection(threat);
                            if (string.IsNullOrWhiteSpace(section) || !groupedThreatsBySection.ContainsKey(section))
                            {
                                continue;
                            }

                            List<List<Server.DcsUnit>> sectionGroups = groupedThreatsBySection[section];
                            string typeKey = GetOppositionTypeKey(threat);
                            List<Server.DcsUnit> targetGroup = sectionGroups.FirstOrDefault(g =>
                                g.Count > 0
                                && GetOppositionTypeKey(g[0]).Equals(typeKey, StringComparison.OrdinalIgnoreCase)
                                && GetSurfaceDistanceMeters(g[0], threat) <= contactMergeDistanceMeters);

                            if (targetGroup == null)
                            {
                                sectionGroups.Add(new List<Server.DcsUnit> { threat });
                            }
                            else
                            {
                                targetGroup.Add(threat);
                            }
                        }

                        bool anyThreats = false;
                        foreach (string section in threatSectionOrder)
                        {
                            List<List<Server.DcsUnit>> sectionGroups = groupedThreatsBySection[section];
                            if (sectionGroups.Count == 0)
                            {
                                continue;
                            }

                            anyThreats = true;
                            unitslist.Add(section + ":");

                            foreach (List<Server.DcsUnit> group in sectionGroups.OrderBy(g => g.Min(u => u.range)))
                            {
                                Server.DcsUnit threat = group.OrderBy(u => u.range).First();
                                string fullName = !string.IsNullOrWhiteSpace(threat.fullname) ? threat.fullname : (threat.typename ?? "unknown");
                                fullName = UppercaseOutsideQuotes(fullName);
                                string line = fullName + " " + threat.getbearingstr() + "/" + threat.getrangestr() + "/" + threat.getaltstr();
                                if (group.Count > 1)
                                {
                                    line += " " + group.Count + " Contacts";
                                }
                                unitslist.Add(line);
                            }

                            unitslist.Add("");
                        }

                        if (!anyThreats)
                        {
                            unitslist.Add("No Datalink threats are displayed.");
                        }

                        return;
                    }

                    //Log.Write($"Processing {State.currentstate.availablerecipients[cat].Count} {cat} units for kneeboard.", Colors.Text);
                    //Log.Write($"Current Theater: {State.currentstate.theatre}", Colors.Text);

                    foreach (Server.DcsUnit unit in State.currentstate.availablerecipients[cat])
                    {
                        try
                        {
                            string altfreqs = "";

                            KneeboardUnitSummary descr = new KneeboardUnitSummary();

                            descr.cat = cat;
                            descr.code = "XXX";
                            descr.callsign = unit.callsign;
                            descr.tacan = unit.tacan;

                            string searchcallsign = Regex.Replace(unit.callsign.Replace("-", ""), "[0-9]", "");

                            var FoundKey = Database.Aliases.airecipients.FirstOrDefault(x =>
                                (x.Value.Equals(searchcallsign) ||
                                 x.Value.Equals(searchcallsign.ToLower()) ||
                                 x.Value.Equals(unit.fullname) ||
                                 x.Value.Equals(unit.fullname.ToLower()))).Key;

                            if (FoundKey != null)
                            {
                                descr.alias = FoundKey;
                            }
                            else
                            {
                                string substract = Regex.Replace(unit.callsign.Replace("-", ""), "[0-9]", "");
                                descr.alias = substract.Equals("") ? unit.callsign : substract;
                            }

                            descr.istuned = unit.isunittuned();

                            if (!State.currentmodule.Theme.Equals("WWII"))
                            {
                                // Prioritize UHF, then VHF_HI, then VHF_LOW, then HF
                                string primaryFreq = null;
                                string secondaryFreq = null;

                                // Combine main frequency and alternate frequencies into a single list
                                List<string> allFrequencies = new List<string>();
                                if (!string.IsNullOrEmpty(unit.freq))
                                {
                                    allFrequencies.Add(unit.freq);
                                }
                                if (unit.altfreq != null && unit.altfreq.Any())
                                {
                                    allFrequencies.AddRange(unit.altfreq);
                                }

                                // Parse frequencies as numerical values and classify them
                                var uhfFreqs = allFrequencies.Where(freq => double.TryParse(freq, out double f) && f >= 225000000 && f <= 399950000).ToList();
                                var vhfHiFreqs = allFrequencies.Where(freq => double.TryParse(freq, out double f) && f >= 118000000 && f <= 137000000).ToList();
                                var vhfLowFreqs = allFrequencies.Where(freq => double.TryParse(freq, out double f) && f >= 30000000 && f <= 75000000).ToList();
                                var hfFreqs = allFrequencies.Where(freq => double.TryParse(freq, out double f) && f >= 3000000 && f <= 30000000).ToList();

                                // Select the top two frequencies based on priority
                                if (uhfFreqs.Any())
                                {
                                    primaryFreq = uhfFreqs.First();
                                    if (vhfHiFreqs.Any())
                                    {
                                        secondaryFreq = vhfHiFreqs.First();
                                    }
                                    else if (uhfFreqs.Count > 1)
                                    {
                                        secondaryFreq = uhfFreqs.Skip(1).FirstOrDefault();
                                    }
                                }
                                else if (vhfHiFreqs.Any())
                                {
                                    primaryFreq = vhfHiFreqs.First();
                                    if (vhfHiFreqs.Count > 1)
                                    {
                                        secondaryFreq = vhfHiFreqs.Skip(1).FirstOrDefault();
                                    }
                                    else if (vhfLowFreqs.Any())
                                    {
                                        secondaryFreq = vhfLowFreqs.First();
                                    }
                                }
                                else if (vhfLowFreqs.Any())
                                {
                                    primaryFreq = vhfLowFreqs.First();
                                    if (vhfLowFreqs.Count > 1)
                                    {
                                        secondaryFreq = vhfLowFreqs.Skip(1).FirstOrDefault();
                                    }
                                    else if (hfFreqs.Any())
                                    {
                                        secondaryFreq = hfFreqs.First();
                                    }
                                }
                                else if (hfFreqs.Any())
                                {
                                    primaryFreq = hfFreqs.First();
                                    if (hfFreqs.Count > 1)
                                    {
                                        secondaryFreq = hfFreqs.Skip(1).FirstOrDefault();
                                    }
                                }

                                // Normalize and assign frequencies
                                descr.frq = primaryFreq != null ? Helpers.Common.NormalizeFreqString(primaryFreq) : unit.getfreqstr();
                                descr.frq2 = secondaryFreq != null ? Helpers.Common.NormalizeFreqString(secondaryFreq) : null;

                                // Log frequencies for debugging
                                //Log.Write($"Primary Frequency: {descr.frq}", Colors.Text);
                                //Log.Write($"Secondary Frequency: {descr.frq2}", Colors.Text);
                            }
                            else
                            {
                                // For WWII theme, use the main frequency
                                descr.frq = unit.getfreqstr();
                                descr.frq2 = null;
                            }

                            descr.bearing = unit.getbearingstr();
                            descr.range = unit.getrangestr();
                            descr.alt = unit.getaltstr();

                            if (AOCS)
                            {
                                descr.humanname = unit.gethumanname();
                                descr.altfreq = unit.altfreq;

                                foreach (string alt in descr.altfreq)
                                {
                                    altfreqs += Helpers.Common.NormalizeFreqString(alt) + " ";
                                }
                            }

                            units.Add(descr);

                            string tacanInfo = "";
                            if (!string.IsNullOrWhiteSpace(descr.tacan)
                                && (cat.Equals("Tanker", StringComparison.OrdinalIgnoreCase)
                                || cat.Equals("AWACS", StringComparison.OrdinalIgnoreCase)
                                || cat.Equals("Flight", StringComparison.OrdinalIgnoreCase)))
                            {
                                tacanInfo = " " + descr.tacan;
                            }

                            string tankerTypeInfo = "";
                            if (cat.Equals("Tanker", StringComparison.OrdinalIgnoreCase))
                            {
                                string tankerType = ResolveTankerAircraftType(unit);
                                if (!string.IsNullOrWhiteSpace(tankerType))
                                {
                                    tankerTypeInfo = " " + tankerType;
                                }
                            }

                            bool isTanker = cat.Equals("Tanker", StringComparison.OrdinalIgnoreCase);
                            string callsignDisplay = cat.Equals("ATC", StringComparison.OrdinalIgnoreCase)
                                ? ResolveAtcDisplayCallsign(unit)
                                : descr.callsign;

                            callsignDisplay = FormatDisplayCallsign(callsignDisplay);

                            string prefix = isTanker
                                ? (descr.istuned ?? "")
                                : ("[" + descr.alias + "]" + descr.istuned + " ");

                            string lineitem = descr.frq + (descr.frq2 != null ? " / " + descr.frq2 : "") + " " +
                                              prefix + callsignDisplay + tankerTypeInfo + " " + descr.bearing + "/" +
                                              descr.range + "/" + descr.alt + " " + altfreqs + tacanInfo;

                            if (cat.Equals("ATC", StringComparison.OrdinalIgnoreCase)
                                && !string.IsNullOrWhiteSpace(State.currentstate.metar)
                                && unitslist.Count == 0)
                            {
                                unitslist.Add("METAR");
                                unitslist.Add(WrapForKneeboard(State.currentstate.metar, 48));
                                unitslist.Add("");
                            }

                            unitslist.Add(lineitem);
                        }
                        catch (Exception x)
                        {
                            Log.Write(x.Message, Colors.Inline);
                        }
                    }

                    string logCategory = string.IsNullOrWhiteSpace(cat) ? "Unknown" : cat;

                    int currentCount = unitslist.Count;
                    int lastLoggedCount;
                    bool hadLastCount = LastLoggedUnitsCountByCategory.TryGetValue(logCategory, out lastLoggedCount);
                    if (!hadLastCount || lastLoggedCount != currentCount)
                    {
                        Log.Write($"Sending {currentCount} {logCategory} units to kneeboard.", Colors.Text);
                        LastLoggedUnitsCountByCategory[logCategory] = currentCount;
                    }
                }
            }

        }
    }
}
