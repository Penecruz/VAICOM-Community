using System;
using System.Collections.Generic;
using VAICOM.Static;


namespace VAICOM
{
    namespace Database
    {
        public static partial class Labels
        {
            public static Dictionary<string, string> importedmenus = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            public static Dictionary<string, string> importedatcs = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            public static Dictionary<string, string> cockpitcontrol = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            public static Dictionary<string, string> simcontrol = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            public static Dictionary<string, Dictionary<string, string>> categories;

            public static void ResetDatabase()
            {
                categories = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase)
                {
                    { "aiappendiceswpn",    Labels.aiappendiceswpn      },
                    { "aiappendicesdir",    Labels.aiappendicesdir      },
                    { "aicues",             Labels.aicues               },
                    { "aicommands",         Labels.aicommands           },
                    { "airecipients",       Labels.airecipients         },
                    { "cockpitcontrol",     Labels.cockpitcontrol       },
                    { "importedatcs",       Labels.importedatcs         },
                    { "importedmenus",      Labels.importedmenus        },
                    { "playercallsigns",    Labels.playercallsigns      },
                    { "simcontrol",         Labels.simcontrol           },                    
                    { "wsorecipients",      Extensions.WSO.Labels.airecipients      }, // Reference WSO recipients
                    { "wsocommands",        Extensions.WSO.Labels.aicommands  }, // Reference WSO aliases
                };

                if (State.dll_installed_rio)
                {
                    try
                    {
                        categories.Add("riorecipients", Extensions.RIO.Labels.airecipients);
                        categories.Add("riocommands", Extensions.RIO.Labels.aicommands);
                    }
                    catch
                    {
                    }
                }
            }

            public static Dictionary<string, string> categorylabels = new Dictionary<string, string>()
            {
                { "aiappendiceswpn",    "Appendices Weapon"         },
                { "aiappendicesdir",    "Appendices Direction"      },
                { "aicues",             "Cues"                      },
                { "aicommands",         "Commands"                  },
                { "airecipients",       "AI Recipients"             },
                { "cockpitcontrol",     "----"                      },
                { "importedatcs",       "Imported ATCs"             },
                { "importedmenus",      "Imported F10 menu commands"},
                { "playercallsigns",    "Player Group Callsigns"    },
                { "simcontrol",         "----"                      },

                { "riorecipients",      "RIO Recipients"            },
                { "riocommands",        "RIO Commands"              },

                { "wsorecipients",      "WSO Recipients"            }, // Add WSO recipients label
                { "wsocommands",        "WSO Commands"              }, // Add WSO commands label
            };

            public static Dictionary<string, string> master = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            public static void BuildNewMasterTable()
            {
                try
                {
                    ResetDatabase();

                    master.Clear();
                    int count = 0;

                    Log.Write("Building master labels table...", Colors.Text);

                    foreach (KeyValuePair<string, Dictionary<string, string>> subset in categories)
                    {
                        Dictionary<string, string> set = subset.Value;

                        foreach (KeyValuePair<string, string> element in set)
                        {
                            try
                            {
                                if (!master.ContainsKey(element.Key))
                                { 
                                    master.Add(element.Key, element.Value); 
                                    count = count + 1; 
                                }
                            }
                            catch
                            {
                            }
                        }
                    }

                    State.labelcount = count;

                    Log.Write("Success.", Colors.Text);
                }
                catch (Exception a)
                {
                    Log.Write(a.Message + a.StackTrace, Colors.Text);
                }
            }
        }
    }
}
