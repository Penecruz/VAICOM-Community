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

                            if (!FoundKey.Equals(null))
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
                                tacanInfo = " TACAN " + descr.tacan;
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

                            string lineitem = descr.frq + (descr.frq2 != null ? " / " + descr.frq2 : "") + " " +
                                              "[" + descr.alias + "]" + descr.istuned + " " +
                                              descr.callsign + tankerTypeInfo + " " + descr.bearing + " " +
                                               descr.range + " " + descr.alt + " " + altfreqs + tacanInfo;

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

                    Log.Write($"Sending {unitslist.Count} Flight units to kneeboard.", Colors.Text);
                }
            }

        }
    }
}
