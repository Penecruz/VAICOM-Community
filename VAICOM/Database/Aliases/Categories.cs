using System;
using System.Collections.Generic;
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
                    { "apxwpn",             Aliases.appendiceswpn     },
                    { "apxdir",             Aliases.appendicesdir     },
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
                            if (set.Key.Equals("aicommands") & Commands.Table.ContainsKey(entry.Value)) { subcatcatstr = Commands.Table[entry.Value].RecipientClass().Name.ToString(); }
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