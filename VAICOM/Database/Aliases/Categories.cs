using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using VAICOM.Static;

namespace VAICOM
{
    namespace Database
    {
        public static partial class Aliases
        {
            public static Dictionary<string, string> importedmenus = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            public static Dictionary<string, string> importedatcs = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            public static Dictionary<string, string> cockpitcontrol = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            public static Dictionary<string, string> simcontrol = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            public static Dictionary<string, Dictionary<string, string>> categories;

            public static void ResetDatabase() // resets the full database encrypted files list
            {
                categories = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase)
                {
                    { "aiappendiceswpn",    Aliases.appendiceswpn       },
                    { "aiappendicesdir",    Aliases.appendicesdir       },
                    { "aicues",             Aliases.aicues              },
                    { "aicommands",         Aliases.aicommands          },
                    { "airecipients",       Aliases.airecipients        },
                    { "cockpitcontrol",     Aliases.cockpitcontrol      },
                    { "importedatcs",       Aliases.importedatcs        },
                    { "importedmenus",      Aliases.importedmenus       },
                    { "playercallsigns",    Aliases.playercallsigns     },
                    { "simcontrol",         Aliases.simcontrol          },                    
                };
            }

            public static void ResetImported() // resets only the imported F10 menu Items and Imported Theater ATC encrypted database files
            {
                categories = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase)
                {
                    { "importedatcs",       Aliases.importedatcs        },
                    { "importedmenus",      Aliases.importedmenus       },
                };
            }

            public static Dictionary<string, Dictionary<string, string>> reference = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase)
            {
                { "aiappendiceswpn",    Aliases.appendiceswpn       },
                { "aiappendicesdir",    Aliases.appendicesdir       },
                { "aicues",             Aliases.aicues              },
                { "aicommands",         Aliases.aicommands          },
                { "airecipients",       Aliases.airecipients        },
                { "cockpitcontrol",     Aliases.cockpitcontrol      },
                { "importedatcs",       Aliases.importedatcs        },
                { "importedmenus",      Aliases.importedmenus       },
                { "playercallsigns",    Aliases.playercallsigns     },
                { "simcontrol",         Aliases.simcontrol          },                
            };

            public static Dictionary<string, string> displaynames = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "aiappendiceswpn",    "(Engage) Appendix Weapon"              },
                { "aiappendicesdir",    "(Engage) Appendix Direction"           },
                { "aicues",             "Cue words"                             },
                { "aicommands",         "AI Unit Commands"                      },
                { "airecipients",       "AI Unit Recipients"                    },
                { "cockpitcontrol",     "Cockpit Control"                       },
                { "importedatcs",       "Imported ATC names"                    },
                { "importedmenus",      "Imported F10 menu commands"            },
                { "playercallsigns",    "Flight Callsigns"                      },
                { "simcontrol",         "Sim Control"                           },                
            };

            public static Dictionary<string, string> scrambleddbfilenames = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "config",             "7h0jdfau663v0o4r"            },
                { "importedmodules",    "34vd6n3m8ghv6mgi"            },
                { "menuitems",          "c5j73h7n3cm83401"            },
                { "aiappendiceswpn",    "4435353353he2las"            },
                { "aiappendicesdir",    "f83f9f2023t0qq53"            },
                { "aicues",             "436363636ao3ndw1"            },
                { "aicommands",         "2478wu7y93oh5fm5"            },
                { "airecipients",       "du8d33j3d21536yj"            },
                { "cockpitcontrol",     "5387383838bcx9a7"            },
                { "importedatcs",       "wdjdw7w8s42ha90f"            },
                { "importedmenus",      "vie728f3j839jxcn"            },
                { "playercallsigns",    "4289248wui8cb8w5"            },
                { "simcontrol",         "accu4264e0a7anv2"            },                
            };

            public static Dictionary<string, Dictionary<string, string>> inputscancats = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase)
            {

                { "recipient",          Aliases.airecipients        },
                { "importedatcs",       Aliases.importedatcs        },
                { "importedmenus",      Aliases.importedmenus       },
                { "sender",             Aliases.playercallsigns     },
                { "cue",                Aliases.aicues              },
                { "command",            Aliases.aicommands          },
                { "apxwpn",             Aliases.appendiceswpn       },
                { "apxdir",             Aliases.appendicesdir       },
                { "wsocmdrecipient",    Aliases.airecipients        },
            };

            public static void UpdateScanCats()
            {
                inputscancats = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase)
                {
                    { "recipient",          Aliases.airecipients        },
                    { "importedatcs",       Aliases.importedatcs        },
                    { "importedmenus",      Aliases.importedmenus       },
                    { "sender",             Aliases.playercallsigns     },
                    { "cue",                Aliases.aicues              },
                    { "command",            Aliases.aicommands          },
                    { "apxwpn",             Aliases.appendiceswpn       },
                    { "apxdir",             Aliases.appendicesdir       },
                    { "wsocmdrecipient",    Aliases.airecipients        },
                };
            }

            public static SortedDictionary<string, string> master = new SortedDictionary<string, string>(new Database.CollectionData.StringFilter());   // (StringComparer.OrdinalIgnoreCase);

            public static void BuildNewMasterTable()
            {
                try
                {
                    ResetDatabase();
                    master.Clear();
                    int count = 0;
                    Log.Write("Building master keywords table...", Colors.Text);
                    foreach (KeyValuePair<string, Dictionary<string, string>> subset in categories)
                    {
                        Dictionary<string, string> set = subset.Value;
                        foreach (KeyValuePair<string, string> element in set)
                        {
                            string entry = element.Key;

                            if (!State.activeconfig.UseNewRecognitionModel)
                            {
                                entry = entry.Replace("*", "");
                            }

                            if (!master.ContainsKey(entry))
                            {
                                master.Add(entry, element.Value);
                                count = count + 1;
                            }
                            else
                            {
                                //Log.Write("Database build: double entry " + element.Key, colors.Text);
                            }
                        }
                    }
                    State.aliascount = count;
                    Log.Write("Success.", Colors.Text);
                }
                catch
                {
                }

                try
                {
                    BuildKneeboardAliasList();
                }
                catch
                {
                }

            }


            public static void BuildKneeboardAliasList()
            {
                try
                {
                    Log.Write("Building kneeboard tables...", Colors.Text);

                    int count = 0;

                    State.KneeboardCatAliasStrings[0] = new Dictionary<string, SortedDictionary<string, List<string>>>(); // cat/"Request"/{"vector to base","vect to tanker'}
                    State.KneeboardCatAliasStrings[1] = new Dictionary<string, SortedDictionary<string, List<string>>>();

                    List<string> commandscomplete = new List<string>();

                    Dictionary<string, int> currentlines = new Dictionary<string, int>();

                    foreach (KeyValuePair<string, string> element in Aliases.aicommands) // go through all command aliases 
                    {

                        string commandname = element.Value;

                        if (!commandscomplete.Contains(commandname)) // no alias for command yet
                        {

                            count += 1;

                            string alias = element.Key.Replace("*", "");

                            //--------------------------------------------
                            //first correct the alias if needed

                            for (int i = 60; i >= 0; i -= 5) // for JTAC cheeck in
                            {
                                alias = alias.Replace(i.ToString() + " minutes", "XX minutes");
                            }

                            // for kick out xx miles
                            alias = alias.Replace("1 mile", "XX miles");
                            alias = alias.Replace("2 miles", "XX miles");
                            alias = alias.Replace("3 miles", "XX miles");
                            alias = alias.Replace("5 miles", "XX miles");
                            alias = alias.Replace("8 miles", "XX miles");
                            alias = alias.Replace("10 miles", "XX miles");

                            if (alias.Contains("Ball"))
                            {
                                alias = "XX Ball";
                            }

                            alias = alias.Replace("from the NorthEast", "from the XX");
                            alias = alias.Replace("from the SouthEast", "from the XX");
                            alias = alias.Replace("from the SouthWest", "from the XX");
                            alias = alias.Replace("from the NorthWest", "from the XX");
                            alias = alias.Replace("from the North", "from the XX");
                            alias = alias.Replace("from the East", "from the XX");
                            alias = alias.Replace("from the South", "from the XX");
                            alias = alias.Replace("from the West", "from the XX");

                            alias = alias.Replace("D-link Targets", "D-link Target");

                            //--------------------------------------------
                            // split alias string
                            string aliasfirstword = "";

                            if (alias.ToLower().StartsWith("go pincer") || alias.ToLower().StartsWith("go echelon") || alias.ToLower().StartsWith("go helo") || alias.ToLower().StartsWith("tacan mode") || alias.ToLower().StartsWith("tacan tune") || alias.ToLower().StartsWith("set ripple") || alias.ToLower().StartsWith("turn left") || alias.ToLower().StartsWith("turn right") || alias.ToLower().StartsWith("track single") || alias.ToLower().StartsWith("flares mode") || alias.ToLower().StartsWith("flares program") || alias.ToLower().StartsWith("attack mode") || alias.ToLower().StartsWith("chaff program") || alias.ToLower().StartsWith("dispense order") || alias.ToLower().StartsWith("link") || alias.ToLower().StartsWith("dispense") || alias.ToLower().StartsWith("radar mode") || alias.ToLower().StartsWith("track single"))
                            {
                                aliasfirstword = alias.Split(' ')[0].TrimEnd() + " " + alias.Split(' ')[1].TrimEnd();
                            }
                            else
                            {
                                if (!alias.StartsWith("Request"))
                                {
                                    aliasfirstword = alias.Split(' ')[0].TrimEnd();
                                }
                                else
                                {
                                    aliasfirstword = alias;
                                }
                            }

                            string aliasresidue = alias.Replace(aliasfirstword, "").TrimStart();

                            //--------------------------------------------
                            // add string to tables

                            if (Commands.Table.ContainsKey(commandname))
                            {

                                string category = Commands.Table[commandname].RecipientClass().Name;

                                if (!currentlines.ContainsKey(category))
                                {
                                    currentlines.Add(category, 1);
                                }

                                int chunk = 0;
                                if (currentlines[category] > 24)
                                {
                                    chunk = 1;
                                }
                                if (currentlines[category] > 48)
                                {
                                    chunk = 2;
                                }


                                if (!State.KneeboardCatAliasStrings[chunk].ContainsKey(category)) // add category if needed
                                {
                                    State.KneeboardCatAliasStrings[chunk][category] = new SortedDictionary<string, List<string>>();
                                }

                                if (!State.KneeboardCatAliasStrings[chunk][category].ContainsKey(aliasfirstword)) // add first word entry if needed
                                {
                                    State.KneeboardCatAliasStrings[chunk][category].Add(aliasfirstword, new List<string>());
                                    currentlines[category] += 1;
                                }

                                if (!State.KneeboardCatAliasStrings[chunk][category][aliasfirstword].Contains(aliasresidue)) //add residue to first word
                                {
                                    int stringlength = 0;
                                    foreach (string str in State.KneeboardCatAliasStrings[chunk][category][aliasfirstword])
                                    {
                                        stringlength += 3 + str.Length;
                                    }
                                    int maxlength = 25;
                                    if (category.Equals("RIO"))
                                    {
                                        maxlength = 45;
                                    }

                                    //if not too many.. category.Equals("RIO") &&
                                    if (stringlength > maxlength) // for test
                                    {
                                        if (!State.KneeboardCatAliasStrings[chunk][category][aliasfirstword].Contains(".."))
                                        {
                                            State.KneeboardCatAliasStrings[chunk][category][aliasfirstword].Add("..");
                                        }
                                    }
                                    else
                                    {
                                        State.KneeboardCatAliasStrings[chunk][category][aliasfirstword].Add(aliasresidue);
                                        State.KneeboardCatAliasStrings[chunk][category][aliasfirstword].Sort();
                                    }
                                }

                                commandscomplete.Add(commandname); // no new aliases will be added for this command ie max 1
                            }

                        }
                    }

                    Log.Write("Success.", Colors.Text);
                }
                catch (Exception a)
                {
                    Log.Write(a.Message + "/n" + a.StackTrace, Colors.Text);
                }

            }

            // checks profile string against database
            public static int ValidateProfileString(dynamic vaProxy, bool silent)
            {
                int counter = 0;
                try
                {
                    foreach (KeyValuePair<string, string> entry in master)
                    {
                        if (!vaProxy.Command.Exists(Helpers.Common.TrimmedString(entry.Key)))
                        {
                            try
                            {
                                if (!silent)
                                {
                                    Log.Write("Alias absent from profile: " + entry.Key + " for " + Labels.master[entry.Value], Colors.Text);
                                }
                            }
                            catch
                            {
                            }
                            counter = counter + 1;
                        }
                    }
                }
                catch
                {
                }
                return counter;
            }

            // VSPX: checks profile string against database
            public static int ValidateProfileStringNew(dynamic vaProxy, bool silent)
            {
                int counter = 0;
                try
                {
                    foreach (KeyValuePair<string, string> entry in master)
                    {
                        if (!vaProxy.Command.Exists(Helpers.Common.TrimmedString(entry.Key)))
                        {
                            try
                            {
                                if (!silent)
                                {
                                    Log.Write("Alias absent from profile: " + entry.Key + " for " + Labels.master[entry.Value], Colors.Text);
                                }
                            }
                            catch
                            {
                            }
                            counter = counter + 1;
                        }
                    }
                }
                catch
                {
                }
                return counter;
            }



            // creates csv file to reflect database
            public static string CreateCSVFile()
            {
                int counter = 0;
                string outputstring = "";
                outputstring = outputstring + "#" + ";" + "voice command phrase" + ";" + "category" + ";" + "command segment" + "\n";
                try
                {
                    foreach (KeyValuePair<string, Dictionary<string, string>> set in categories)
                    {
                        Dictionary<string, string> thistable = Labels.categories[set.Key];
                        outputstring = outputstring + "" + ";" + Labels.categorylabels[set.Key] + ";" + "" + "\n";
                        foreach (KeyValuePair<string, string> entry in Aliases.categories[set.Key])
                        {
                            counter = counter + 1;
                            string subcatcatstr = "";
                            if (set.Key.Equals("aicommands") & Commands.Table.ContainsKey(entry.Value))
                            {
                                subcatcatstr = Commands.Table[entry.Value].RecipientClass().Name.ToString();
                                if (Commands.Table[entry.Value].dcsid.StartsWith("wMsgGeorge", StringComparison.OrdinalIgnoreCase))
                                {
                                    subcatcatstr = "CPG";
                                }
                            }
                            if (set.Key.Equals("airecipients") & Recipients.Table.ContainsKey(entry.Value)) { subcatcatstr = Recipients.Table[entry.Value].RecipientClass().Name.ToString(); }
                            try
                            {
                                string keystr = entry.Key;
                                if (!State.activeconfig.UseNewRecognitionModel)
                                {
                                    keystr = keystr.Replace("*", "");
                                }
                                outputstring = outputstring + counter.ToString() + ";" + keystr + ";" + subcatcatstr + ";" + thistable[entry.Value] + "\n";
                            }
                            catch
                            {

                            }
                        }
                    }
                    Log.Write("Created aliases database file for CSV export, keyword count = " + counter.ToString(), Colors.Text);
                }
                catch (Exception e)
                {
                    //Log.Write("Export error " + e.Message + e.StackTrace +e.InnerException, Colors.Text);
                }
                return outputstring;
            }

            // creates html file to reflect database, grouped by action inside each category
            public static string CreateHTMLFile()
            {
                var html = new StringBuilder();
                string logoDataUri = "";
                try
                {
                    byte[] logoBytes = Properties.Resources.icon;
                    if (logoBytes != null && logoBytes.Length > 0)
                    {
                        logoDataUri = "data:image/png;base64," + Convert.ToBase64String(logoBytes);
                    }
                }
                catch
                {
                }

                Func<CommandCategories, string> getCommandSection = category =>
                {
                    if (category == CommandCategories.AH64D_GeorgeAI) { return "AI Crew | AH-64D Apache"; }
                    if (category == CommandCategories.AI_pilot || category.ToString().StartsWith("RIO", StringComparison.OrdinalIgnoreCase)) { return "AI Crew | F-14 Tomcat"; }
                    if (category == CommandCategories.WSO || category.ToString().StartsWith("WSO", StringComparison.OrdinalIgnoreCase)) { return "AI Crew | F-4E Phantom II"; }

                    if (category == CommandCategories.aicommsflight
                        || category == CommandCategories.aicommsflightengage
                        || category == CommandCategories.aicommsflightmaneuver
                        || category == CommandCategories.aicommsflighttactical
                        || category == CommandCategories.aicommsflightformation)
                    {
                        return "Flight";
                    }

                    if (category == CommandCategories.aicommsjtac) { return "JTAC"; }
                    if (category == CommandCategories.aicommsatc
                        || category == CommandCategories.aicommscarrier
                        || category == CommandCategories.aicommscarrier_LSO
                        || category == CommandCategories.aicommscarrier_Marshal
                        || category == CommandCategories.aicommscarrier_Approach
                        || category == CommandCategories.aicommscarrier_Departure)
                    {
                        return "ATC";
                    }

                    if (category == CommandCategories.aicommsawacs) { return "AWACS"; }
                    if (category == CommandCategories.aicommstanker) { return "Tanker"; }
                    if (category == CommandCategories.aicommscrew) { return "Crew"; }
                    if (category == CommandCategories.aicommsaocs) { return "AOCS"; }
                    if (category == CommandCategories.kneeboard) { return "Kneeboard"; }
                    if (category == CommandCategories.cockpit) { return "Cockpit"; }
                    if (category == CommandCategories.reply) { return "Reply"; }
                    if (category == CommandCategories.menu || category == CommandCategories.auxmenu) { return "Menu"; }
                    if (category == CommandCategories.cargocontrol) { return "Cargo"; }
                    if (category == CommandCategories.special) { return "Special"; }

                    return "Other";
                };

                Func<string, IEnumerable<string>, string> inferDynamicGroupDetail = (actionKey, aliases) =>
                {
                    string haystack = ((actionKey ?? "") + " " + string.Join(" ", aliases ?? Enumerable.Empty<string>())).ToLowerInvariant();

                    if (haystack.Contains("airboss") || haystack.Contains("mother") || haystack.Contains("lso") || haystack.Contains("marshal") || haystack.Contains("skipper"))
                    {
                        return "Community | Moose Airboss";
                    }

                    if (haystack.Contains("chalice") || haystack.Contains("bogey dope") || haystack.Contains("awacs"))
                    {
                        return "Community | Moose AWACS";
                    }

                    if (haystack.Contains("range") || haystack.Contains("strafe") || haystack.Contains("smoke") || haystack.Contains("illuminate"))
                    {
                        return "Community | Moose Range";
                    }

                    if (haystack.Contains("talon bravo") || haystack.Contains("talon alpha") || haystack.Contains("authenticates good") || haystack.Contains("authenticates bad"))
                    {
                        return "Campaign | Pacific Sentry 26 | M03 Vote for Pedro";
                    }

                    if (haystack.Contains("i will sink ship") || haystack.Contains("you sink ship") || haystack.Contains("explode ship"))
                    {
                        return "Campaign | Pacific Sentry 26 | M05 Hog Litening Viper Thunder";
                    }

                    if (haystack.Contains("i will hit fast boat") || haystack.Contains("you hit fast boat"))
                    {
                        return "Campaign | Pacific Sentry 26 | M06 Contested Waters";
                    }

                    if (haystack.Contains("recommend weather cancel") || haystack.Contains("recommend low war") || haystack.Contains("recommend high war") || haystack.Contains("bone one") || haystack.Contains("devil dog") || haystack.Contains("dozer hit trucks") || haystack.Contains("i will hit trucks"))
                    {
                        return "Campaign | Pacific Sentry 26 | M07 The Show Must Go On";
                    }

                    if (haystack.Contains("fast boat moving south") || haystack.Contains("both boats moving north"))
                    {
                        return "Campaign | Pacific Sentry 26 | M08 Sails Calls, Wrong Number";
                    }

                    if (haystack.Contains("give tower excuse") || haystack.Contains("tell tower the truth"))
                    {
                        return "Campaign | Pacific Sentry 26 | M09 Hot Start, Hotter DZ";
                    }

                    if (haystack.Contains("return south") || haystack.Contains("stay here"))
                    {
                        return "Campaign | Pacific Sentry 26 | M10 Snake Eaters in Paradise";
                    }

                    if (haystack.Contains("wingloon") || haystack.Contains("lost caravan"))
                    {
                        return "Campaign | Pacific Sentry 26 | M11 Check Out before Checking Out";
                    }

                    if (haystack.Contains("i can find parking") || haystack.Contains("request assistance"))
                    {
                        return "Campaign | Pacific Sentry 25 | M01 Fly to Gyam by Viper";
                    }

                    if (haystack.Contains("cruise missiles") || haystack.Contains("captured bogey") || haystack.Contains("venom"))
                    {
                        return "Campaign | Pacific Sentry 25 | M02 A Red Horizon";
                    }

                    if (haystack.Contains("send it") || haystack.Contains("say again nine line") || haystack.Contains("sorry no gas") || haystack.Contains("sure thing"))
                    {
                        return "Campaign | Pacific Sentry 25 | M03 A Red Horizon";
                    }

                    if (haystack.Contains("review mission") || haystack.Contains("skip it"))
                    {
                        return "Campaign | Pacific Sentry 25 | M04 Sprung A Leak";
                    }

                    if (haystack.Contains("tiny go to alpha") || haystack.Contains("tiny rejoin"))
                    {
                        return "Campaign | Pacific Sentry 25 | M10 Laying It All Out There";
                    }

                    if (haystack.Contains("incentive ride"))
                    {
                        return "Campaign | Pacific Sentry 25 | M11 Work Hard, Play Hard";
                    }

                    if (haystack.Contains("ifr") || haystack.Contains("ils") || haystack.Contains("localizer") || haystack.Contains("tacan") || haystack.Contains("gca") || haystack.Contains("par"))
                    {
                        return "Community | AI-ATC | IFR Sub Menu";
                    }

                    if (haystack.Contains("vfr") || haystack.Contains("gass_peak") || haystack.Contains("dry_lake") || haystack.Contains("red_horse") || haystack.Contains("sunrise"))
                    {
                        return "Community | AI-ATC | VFR Sub Menu";
                    }

                    if (haystack.Contains("squawk"))
                    {
                        return "Community | AI-ATC | Clearance Readback";
                    }

                    if (haystack.Contains("engine start") || haystack.Contains("taxi") || haystack.Contains("clear of runway"))
                    {
                        return "Community | AI-ATC | Ground";
                    }

                    if (haystack.Contains("cross") && (haystack.Contains("runway") || haystack.Contains("clearance")))
                    {
                        return "Community | AI-ATC | Runway Crossing";
                    }

                    if (haystack.Contains("unrestricted climb") || haystack.Contains("full stop") || haystack.Contains("touch and go") || haystack.Contains("low approach") || haystack.Contains("request takeoff") || haystack.Contains("ready") || haystack.Contains("tower") || haystack.Contains("landing"))
                    {
                        return "Community | AI-ATC | Tower";
                    }

                    if (haystack.Contains("departure"))
                    {
                        return "Community | AI-ATC | Departure";
                    }

                    if (haystack.Contains("approach") || haystack.Contains("report established") || haystack.Contains("radar"))
                    {
                        return "Community | AI-ATC | Approach";
                    }

                    if (haystack.Contains("vector"))
                    {
                        return "Community | AI-ATC | NAV Assist";
                    }

                    if (haystack.Contains("hammerhead") || haystack.Contains("blackjack") || haystack.Contains("sally") || haystack.Contains("lee"))
                    {
                        return "Community | AI-ATC | Range Control";
                    }

                    if (haystack.Contains("atc") || haystack.Contains("airfield") || haystack.Contains("tower") || haystack.Contains("takeoff") || haystack.Contains("landing"))
                    {
                        return "Community | AI-ATC";
                    }

                    return "Community | Imported Dynamic";
                };

                Func<string, string> getAtcTheater = recipientName =>
                {
                    string name = recipientName ?? "";
                    if (name.IndexOf("Carriers", StringComparison.OrdinalIgnoreCase) >= 0) return "Carrier";
                    if (name.IndexOf("Caucasus", StringComparison.OrdinalIgnoreCase) >= 0) return "Caucasus";
                    if (name.IndexOf("PersianGulf", StringComparison.OrdinalIgnoreCase) >= 0) return "Persian Gulf";
                    if (name.IndexOf("MarianasWWII", StringComparison.OrdinalIgnoreCase) >= 0) return "Marianas WWII";
                    if (name.IndexOf("Marianas", StringComparison.OrdinalIgnoreCase) >= 0) return "Marianas";
                    if (name.IndexOf("Nevada", StringComparison.OrdinalIgnoreCase) >= 0) return "Nevada";
                    if (name.IndexOf("Normandy", StringComparison.OrdinalIgnoreCase) >= 0) return "Normandy";
                    if (name.IndexOf("Syria", StringComparison.OrdinalIgnoreCase) >= 0) return "Syria";
                    if (name.IndexOf("Sinai", StringComparison.OrdinalIgnoreCase) >= 0) return "Sinai";
                    if (name.IndexOf("Channel", StringComparison.OrdinalIgnoreCase) >= 0) return "Channel";
                    if (name.IndexOf("SouthAtlantic", StringComparison.OrdinalIgnoreCase) >= 0) return "South Atlantic";
                    if (name.IndexOf("Kola", StringComparison.OrdinalIgnoreCase) >= 0) return "Kola";
                    if (name.IndexOf("Afghanistan", StringComparison.OrdinalIgnoreCase) >= 0) return "Afghanistan";
                    if (name.IndexOf("Iraq", StringComparison.OrdinalIgnoreCase) >= 0) return "Iraq";
                    return "General";
                };

                Func<Recipient, string> getRecipientSection = recipient =>
                {
                    string className = recipient.RecipientClass().Name;
                    if (className.Equals("ATC", StringComparison.OrdinalIgnoreCase))
                    {
                        return "ATC | " + getAtcTheater(recipient.name);
                    }

                    return className;
                };

                Func<string, string> slugify = value =>
                {
                    var slug = new StringBuilder();
                    bool previousDash = false;
                    foreach (char c in (value ?? "").ToLowerInvariant())
                    {
                        if (char.IsLetterOrDigit(c))
                        {
                            slug.Append(c);
                            previousDash = false;
                        }
                        else if (!previousDash)
                        {
                            slug.Append('-');
                            previousDash = true;
                        }
                    }

                    return slug.ToString().Trim('-');
                };

                Func<string, int> getGroupDetailOrder = groupDetail =>
                {
                    string detail = (groupDetail ?? "").ToLowerInvariant();

                    if (detail.Contains("crew control") || detail.Contains("crew")) return 0;
                    if (detail.Contains("weapons") || detail.Contains("weapon")) return 1;
                    if (detail.Contains("radar")) return 2;
                    if (detail.Contains("radio")) return 3;
                    if (detail.Contains("navigation")) return 4;
                    if (detail.Contains("defensive") || detail.Contains("countermeasure")) return 5;
                    if (detail.Contains("ground crew")) return 6;
                    if (detail.Contains("engage")) return 7;
                    if (detail.Contains("maneuver")) return 8;
                    if (detail.Contains("tactical")) return 9;
                    if (detail.Contains("formation")) return 10;
                    if (detail.Contains("menu control") || detail.Contains("menu")) return 11;
                    if (detail.Contains("utility")) return 12;
                    if (detail.Contains("misc")) return 13;

                    return 14;
                };

                html.Append("<!DOCTYPE html><html><head><meta charset=\"utf-8\" />");
                html.Append("<title>VAICOM Keywords</title>");
                html.Append("<style>");
                html.Append("html{margin:0 !important;padding:0 !important;background:#ffffff;} body{font-family:Segoe UI,Arial,sans-serif;margin:0 auto !important;max-width:190mm !important;width:100% !important;box-sizing:border-box !important;padding:0 10px 10px 10px !important;color:#1f2937;background:#ffffff;}");
                html.Append(".header{display:flex;justify-content:space-between;align-items:flex-start;gap:10px;margin-bottom:4px;} .header h1{margin:0;} .logo{width:34px;height:34px;object-fit:contain;}");
                html.Append("h1{font-size:22px;margin:0 0 4px 0;}h2{font-size:18px;margin:24px 0 8px 0;}");
                html.Append("p.meta{margin:0 0 14px 0;color:#6b7280;font-size:12px;}");
                html.Append("table{display:table !important;width:100% !important;max-width:100% !important;border-collapse:collapse !important;margin-bottom:14px !important;table-layout:fixed !important;}");
                html.Append("th,td{border:1px solid #d1d5db !important;padding:3px 5px !important;vertical-align:top !important;text-align:left !important;font-size:13px !important;}");
                html.Append("th{background:#f3f4f6;font-weight:600;}tr:nth-child(even){background:#fafafa;}");
                html.Append("td.aliases,td.group,td.action{white-space:normal !important;word-break:break-word !important;overflow-wrap:anywhere !important;}");
                html.Append("h3{font-size:15px;margin:12px 0 6px 0;color:#111827;}");
                html.Append(".toc{background:#f9fafb;border:1px solid #e5e7eb;padding:10px 12px;margin:8px 0 16px 0;} .toc h2{margin:0 0 6px 0;font-size:16px;} .toc ul{margin:0;padding-left:18px;} .toc li{margin:3px 0;} .toc a{text-decoration:none;color:#1d4ed8;} .toc a:hover{text-decoration:underline;}");
                html.Append(".okb-nav{display:none;margin:0 0 8px 0;gap:8px;position:sticky;top:0;z-index:20;background:#ffffff;padding:4px 0;} .okb-nav button{font-family:Segoe UI,Arial,sans-serif;font-size:12px;padding:4px 10px;border:1px solid #9ca3af;background:#f3f4f6;color:#111827;cursor:pointer;} .okb-nav button:hover{background:#e5e7eb;} .okb-nav button:disabled{opacity:.5;cursor:default;} body.okb-mode .okb-nav{display:flex;}");
                html.Append("td.group .group-detail{color:#4b5563;}");
                html.Append("td.aliases .alias-item{display:block;margin:0 0 2px 0;}");
                html.Append("@media screen{html,body{margin-top:0 !important;padding-top:0 !important;} body{max-width:190mm !important;margin:0 auto !important;} .header{margin-bottom:6px;} p.meta{margin:0 0 10px 0;} .toc{margin:6px 0 12px 0;} h2{margin:18px 0 6px 0;} h3{margin:10px 0 5px 0;} table{margin-bottom:10px !important;}}");
                html.Append("@media print{body{max-width:190mm !important;width:auto !important;padding:0 !important;margin:8mm auto !important;color:#000 !important;} .okb-nav{display:none !important;} .toc{display:none !important;} h2,h3{page-break-after:avoid;} table{page-break-inside:auto;} tr{page-break-inside:avoid;page-break-after:auto;} a{color:#000 !important;text-decoration:none !important;}}");
                html.Append("</style></head><body>");

                html.Append("<div class=\"header\"><h1>VAICOM KEYWORDS</h1>");
                if (!string.IsNullOrWhiteSpace(logoDataUri))
                {
                    html.Append("<img class=\"logo\" alt=\"VAICOM\" src=\"");
                    html.Append(logoDataUri);
                    html.Append("\" />");
                }
                html.Append("</div>");
                html.Append("<p class=\"meta\">Generated: ");
                html.Append(WebUtility.HtmlEncode(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                html.Append("</p>");
                html.Append("<div class=\"okb-nav\"><button type=\"button\" id=\"okb-back\">◀ Back</button><button type=\"button\" id=\"okb-forward\">Forward ▶</button></div>");

                var content = new StringBuilder();
                var index = new StringBuilder();
                int maxActionLen = "Action".Length;
                int maxGroupLen = "Group".Length;
                int maxAliasLen = "Aliases".Length;
                var usedIds = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                Func<string, string> createId = text =>
                {
                    string id = slugify(text);
                    if (string.IsNullOrWhiteSpace(id))
                    {
                        id = "section";
                    }

                    if (!usedIds.ContainsKey(id))
                    {
                        usedIds[id] = 0;
                        return id;
                    }

                    usedIds[id] = usedIds[id] + 1;
                    return id + "-" + usedIds[id].ToString();
                };

                index.Append("<div class=\"toc\"><h2>Index</h2><ul>");

                try
                {
                    var orderedSets = categories
                        .Where(set => Labels.categorylabels.ContainsKey(set.Key) && Labels.categories.ContainsKey(set.Key))
                        .OrderBy(set =>
                        {
                            if (set.Key.Equals("aicommands", StringComparison.OrdinalIgnoreCase)) return 0;
                            if (set.Key.Equals("aiappendiceswpn", StringComparison.OrdinalIgnoreCase)) return 1;
                            if (set.Key.Equals("aiappendicesdir", StringComparison.OrdinalIgnoreCase)) return 2;
                            if (set.Key.Equals("aicues", StringComparison.OrdinalIgnoreCase)) return 3;
                            return 10;
                        })
                        .ThenBy(set => Labels.categorylabels[set.Key], StringComparer.OrdinalIgnoreCase)
                        .ToList();

                    foreach (KeyValuePair<string, Dictionary<string, string>> set in orderedSets)
                    {
                        Dictionary<string, string> labelTable = Labels.categories[set.Key];
                        var groupedEntries = set.Value
                            .GroupBy(entry => entry.Value, StringComparer.OrdinalIgnoreCase)
                            .OrderBy(group => group.Key, StringComparer.OrdinalIgnoreCase)
                            .ToList();

                        if (groupedEntries.Count == 0)
                        {
                            continue;
                        }

                        string categoryLabel = Labels.categorylabels[set.Key];
                        string categoryId = createId("cat-" + categoryLabel);
                        content.Append("<h2 id=\"");
                        content.Append(WebUtility.HtmlEncode(categoryId));
                        content.Append("\">");
                        content.Append(WebUtility.HtmlEncode(categoryLabel));
                        content.Append("</h2>");
                        index.Append("<li><a href=\"#");
                        index.Append(WebUtility.HtmlEncode(categoryId));
                        index.Append("\">");
                        index.Append(WebUtility.HtmlEncode(categoryLabel));
                        index.Append("</a>");
                        var rows = new List<Tuple<string, string, string, List<string>>>();

                        foreach (var actionGroup in groupedEntries)
                        {
                            string actionKey = actionGroup.Key;
                            string actionName = labelTable.ContainsKey(actionKey) ? labelTable[actionKey] : actionKey;
                            string groupName = "";
                            string sectionHeading = "";

                            List<string> aliases = actionGroup
                                .Select(alias => alias.Key)
                                .Where(alias => !string.IsNullOrWhiteSpace(alias))
                                .OrderBy(alias => alias, StringComparer.OrdinalIgnoreCase)
                                .ToList();

                            if (set.Key.Equals("aicommands", StringComparison.OrdinalIgnoreCase) && Commands.Table.ContainsKey(actionKey))
                            {
                                CommandCategories commandCategory = Commands.Table[actionKey].category;
                                if (EditorCatLabels.Commands.ContainsKey(commandCategory))
                                {
                                    groupName = EditorCatLabels.Commands[commandCategory];
                                }
                                else
                                {
                                    groupName = commandCategory.ToString();
                                }

                                sectionHeading = getCommandSection(commandCategory);
                                if ((commandCategory == CommandCategories.auxmenu || commandCategory == CommandCategories.menu) && actionKey.StartsWith("Action ", StringComparison.OrdinalIgnoreCase))
                                {
                                    sectionHeading = "Dynamic Commands";
                                    groupName = inferDynamicGroupDetail(actionKey, aliases);
                                }
                            }
                            else if (set.Key.Equals("aicommands", StringComparison.OrdinalIgnoreCase) && actionKey.StartsWith("Action ", StringComparison.OrdinalIgnoreCase))
                            {
                                sectionHeading = "Dynamic Commands";
                                groupName = inferDynamicGroupDetail(actionKey, aliases);
                            }
                            else if (set.Key.Equals("airecipients", StringComparison.OrdinalIgnoreCase) && Recipients.Table.ContainsKey(actionKey))
                            {
                                groupName = Recipients.Table[actionKey].RecipientClass().Name.ToString();
                                sectionHeading = getRecipientSection(Recipients.Table[actionKey]);
                            }
                            else if (set.Key.Equals("importedmenus", StringComparison.OrdinalIgnoreCase))
                            {
                                sectionHeading = "Dynamic Commands";
                                groupName = inferDynamicGroupDetail(actionKey + " " + actionName, aliases);
                            }
                            else if (set.Key.Equals("importedatcs", StringComparison.OrdinalIgnoreCase))
                            {
                                sectionHeading = "Dynamic Commands";
                                groupName = "Community | AI-ATC";
                            }

                            rows.Add(Tuple.Create(sectionHeading, actionName, groupName, aliases));
                        }

                        var sectionGroups = rows
                            .GroupBy(row => row.Item1, StringComparer.OrdinalIgnoreCase)
                            .OrderBy(group =>
                            {
                                if (group.Key.Equals("Flight", StringComparison.OrdinalIgnoreCase)) return 0;
                                if (group.Key.Equals("Crew", StringComparison.OrdinalIgnoreCase)) return 1;
                                if (group.Key.Equals("ATC", StringComparison.OrdinalIgnoreCase)) return 2;
                                if (group.Key.StartsWith("ATC | ", StringComparison.OrdinalIgnoreCase)) return 2;
                                if (group.Key.Equals("AWACS", StringComparison.OrdinalIgnoreCase)) return 3;
                                if (group.Key.Equals("Tanker", StringComparison.OrdinalIgnoreCase)) return 4;
                                if (group.Key.Equals("JTAC", StringComparison.OrdinalIgnoreCase)) return 5;
                                if (group.Key.Equals("AOCS", StringComparison.OrdinalIgnoreCase)) return 6;
                                if (group.Key.Equals("AI Crew | AH-64D Apache", StringComparison.OrdinalIgnoreCase)) return 7;
                                if (group.Key.Equals("AI Crew | F-14 Tomcat", StringComparison.OrdinalIgnoreCase)) return 8;
                                if (group.Key.Equals("AI Crew | F-4E Phantom II", StringComparison.OrdinalIgnoreCase)) return 9;
                                if (group.Key.Equals("Dynamic Commands", StringComparison.OrdinalIgnoreCase)) return 10;
                                if (group.Key.Equals("Menu", StringComparison.OrdinalIgnoreCase)) return 11;
                                if (group.Key.Equals("Reply", StringComparison.OrdinalIgnoreCase)) return 12;
                                if (group.Key.Equals("Special", StringComparison.OrdinalIgnoreCase)) return 13;
                                if (group.Key.Equals("Cargo", StringComparison.OrdinalIgnoreCase)) return 14;
                                if (group.Key.Equals("Kneeboard", StringComparison.OrdinalIgnoreCase)) return 15;
                                if (group.Key.Equals("Cockpit", StringComparison.OrdinalIgnoreCase)) return 16;
                                if (group.Key.Equals("Other", StringComparison.OrdinalIgnoreCase)) return 17;
                                return 18;
                            })
                            .ThenBy(group => group.Key)
                            .ToList();

                        bool hasSectionLinks = sectionGroups.Any(group => !string.IsNullOrWhiteSpace(group.Key));
                        if (hasSectionLinks)
                        {
                            index.Append("<ul>");
                        }

                        foreach (var section in sectionGroups)
                        {
                            string sectionId = "";
                            if (!string.IsNullOrEmpty(section.Key))
                            {
                                sectionId = createId(categoryLabel + "-" + section.Key);
                                content.Append("<h3 id=\"");
                                content.Append(WebUtility.HtmlEncode(sectionId));
                                content.Append("\">");
                                content.Append(WebUtility.HtmlEncode(section.Key));
                                content.Append("</h3>");

                                if (hasSectionLinks)
                                {
                                    index.Append("<li><a href=\"#");
                                    index.Append(WebUtility.HtmlEncode(sectionId));
                                    index.Append("\">");
                                    index.Append(WebUtility.HtmlEncode(section.Key));
                                    index.Append("</a></li>");
                                }
                            }

                            content.Append("<table><colgroup><col style=\"width:__COL_ACTION_PCT__% !important;\" /><col style=\"width:__COL_GROUP_PCT__% !important;\" /><col style=\"width:__COL_ALIAS_PCT__% !important;\" /></colgroup>");
                            content.Append("<thead><tr><th>Action</th><th>Group</th><th>Aliases</th></tr></thead><tbody>");

                            var orderedRows = section
                                .OrderBy(row => getGroupDetailOrder(row.Item3))
                                .ThenBy(row => row.Item3, StringComparer.OrdinalIgnoreCase)
                                .ThenBy(row => row.Item2, StringComparer.OrdinalIgnoreCase)
                                .ToList();

                            foreach (var row in orderedRows)
                            {
                                if (!string.IsNullOrWhiteSpace(row.Item2)) { maxActionLen = Math.Max(maxActionLen, row.Item2.Length); }
                                if (!string.IsNullOrWhiteSpace(row.Item3)) { maxGroupLen = Math.Max(maxGroupLen, row.Item3.Length); }
                                if (row.Item4 != null && row.Item4.Count > 0)
                                {
                                    int localMaxAliasLen = row.Item4.Max(alias => alias == null ? 0 : alias.Length);
                                    maxAliasLen = Math.Max(maxAliasLen, localMaxAliasLen);
                                }

                                content.Append("<tr><td class=\"action\">");
                                content.Append(WebUtility.HtmlEncode(row.Item2));
                                content.Append("</td><td class=\"group\"><div class=\"group-detail\">");
                                content.Append(WebUtility.HtmlEncode(row.Item3));
                                content.Append("</div></td><td class=\"aliases\">");
                                foreach (string alias in row.Item4)
                                {
                                    content.Append("<span class=\"alias-item\">");
                                    content.Append(WebUtility.HtmlEncode(alias));
                                    content.Append("</span>");
                                }
                                content.Append("</td></tr>");
                            }

                            content.Append("</tbody></table>");
                        }

                        if (hasSectionLinks)
                        {
                            index.Append("</ul>");
                        }

                        index.Append("</li>");
                    }
                }
                catch
                {
                }

                index.Append("</ul></div>");
                html.Append(index.ToString());

                int actionWidthPx = Math.Min(360, Math.Max(120, (maxActionLen * 8) + 10));
                int groupWidthPx = Math.Min(380, Math.Max(140, (maxGroupLen * 8) + 10));
                int aliasWidthPx = Math.Min(520, Math.Max(180, (maxAliasLen * 8) + 10));

                int totalWidthPx = actionWidthPx + groupWidthPx + aliasWidthPx;
                string actionPct = ((actionWidthPx * 100.0) / totalWidthPx).ToString("0.##", CultureInfo.InvariantCulture);
                string groupPct = ((groupWidthPx * 100.0) / totalWidthPx).ToString("0.##", CultureInfo.InvariantCulture);
                string aliasPct = ((aliasWidthPx * 100.0) / totalWidthPx).ToString("0.##", CultureInfo.InvariantCulture);

                string contentHtml = content.ToString()
                    .Replace("__COL_ACTION_PCT__", actionPct)
                    .Replace("__COL_GROUP_PCT__", groupPct)
                    .Replace("__COL_ALIAS_PCT__", aliasPct);

                html.Append(contentHtml);

                html.Append("<script>(function(){try{var p=new URLSearchParams(window.location.search||'');if(p.get('okb')==='1'){document.body.className=(document.body.className?document.body.className+' ':'')+'okb-mode';var b=document.getElementById('okb-back');var f=document.getElementById('okb-forward');var stack=[];var idx=-1;var moving=false;function now(){return window.location.hash||'#';}function update(){if(b){b.disabled=idx<=0;}if(f){f.disabled=idx<0||idx>=stack.length-1;}}function push(h){if(idx>=0&&stack[idx]===h){update();return;}stack=stack.slice(0,idx+1);stack.push(h);idx=stack.length-1;update();}function jump(i){if(i<0||i>=stack.length||i===idx){update();return;}idx=i;moving=true;var h=stack[idx];if(h==='#'||h===''){window.scrollTo(0,0);}else{var el=document.getElementById(h.substring(1));if(el){el.scrollIntoView();}window.location.hash=h;}setTimeout(function(){moving=false;},0);update();}window.addEventListener('hashchange',function(){if(!moving){push(now());}});push(now());if(b){b.addEventListener('click',function(){if(idx>0){jump(idx-1);}else if(window.history.length>1){window.history.back();}});}if(f){f.addEventListener('click',function(){if(idx<stack.length-1){jump(idx+1);}else{window.history.forward();}});}}}catch(e){}})();</script>");
                html.Append("</body></html>");
                return html.ToString();
            }

            public static void SetAOCSCallsign()
            {
                try
                {
                    foreach (KeyValuePair<string, string> entry in Aliases.master)
                    {
                        if (entry.Value.Equals("aocs"))
                        {
                            State.aocscallsign = entry.Key;
                        }
                    }
                }
                catch
                {
                    State.aocscallsign = "Crystal Palace";
                }
            }

        }
    }
}