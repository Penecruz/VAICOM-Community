using System.Collections.Generic;
using VAICOM.Database;

namespace VAICOM
{

    namespace Static
    {

        public static class Messagetypes
        {
            public static readonly string Undefined = "sim.undefined";
            public static readonly string SettingsChange = "sim.changesettings";

            public static readonly string RequestUpdate = "mission.player.requestupdate";
            public static readonly string DeviceControl = "mission.player.devicecontrol";
            public static readonly string iCommandSequence = "mission.player.cmdsequence";
            public static readonly string ActionIndexSequence = "mission.player.actionsequence";
            public static readonly string CommsMessage = "mission.player.aicomms";
        }

        public static class ClientModes
        {
            public static readonly string Normal = "normal";
            public static readonly string Debug = "debug";
        }

        public static class Colors
        {

            // normal log
            public static readonly string Critical = "red";
            public static readonly string System = "blue";
            public static readonly string Recognition = "green";
            public static readonly string Warning = "orange";
            public static readonly string Message = "purple";

            // additional info in debug mode
            public static readonly string Alert = "yellow";
            public static readonly string Text = "black";
            public static readonly string Notification = "grey";

            // deep debug
            public static readonly string Inline = "blank";

        }

        public static class Weapons
        {
            public static int Default = 0;
            public static int Missile = 1;
            public static int Unguided = 2;
            public static int Guided = 3;
            public static int Rocket = 4;
            public static int Marker = 5;
            public static int Gun = 6;

        }

        public static class Directions
        {
            public static double Default = 0;
            public static double North = 2 * 3.1416 * 0 / 8;
            public static double NorthEast = 2 * 3.1416 * 1 / 8;
            public static double East = 2 * 3.1416 * 2 / 8;
            public static double SouthEast = 2 * 3.1416 * 3 / 8;
            public static double South = 2 * 3.1416 * 4 / 8;
            public static double SouthWest = 2 * 3.1416 * 5 / 8;
            public static double West = 2 * 3.1416 * 6 / 8;
            public static double NorthWest = 2 * 3.1416 * 7 / 8;

        }

        public static class EditorCatLabels
        {
            public static Dictionary<RecipientCategories, string> Recipients = new Dictionary<RecipientCategories, string>()
            {
                {RecipientCategories.undefined,      "Miscellaneous"                         },
                {RecipientCategories.aiunit,         "AI Recipient Units"                    },
                {RecipientCategories.aiflight,       "AI Recipient Units | Flight"           },
                {RecipientCategories.aijtac,         "AI Recipient Units | JTAC"             },
                {RecipientCategories.aiatc,          "AI Recipient Units | Airbase | Carrier"},
                {RecipientCategories.aifarp,         "AI Recipient Units | ATC (FARP)"       },
                {RecipientCategories.aiship,         "AI Recipient Units | ATC (Carrier)"    },
                {RecipientCategories.aitanker,       "AI Recipient Units | Tanker"           },
                {RecipientCategories.aiawacs,        "AI Recipient Units | AWACS"            },
                {RecipientCategories.aicrew,         "AI Recipient Units | Ground Crew"      },
                {RecipientCategories.aocs,           "Virtualized Units  | AOCS"             },
                {RecipientCategories.cockpitdevice,  "Cockpit Devices"                       },
                {RecipientCategories.auxmenu,        "Aux Menu"                              },
                {RecipientCategories.cargo,          "Cargo Menu"                            },
                {RecipientCategories.RIO,            "RIO"                                   },
                {RecipientCategories.AI_pilot,       "AI Pilot"                              },
                {RecipientCategories.kneeboard,      "Interactive Kneeboard extension"       },
                {RecipientCategories.Moose,          "Moose Script Interaction"              },
            };

            public static Dictionary<CommandCategories, string> Commands = new Dictionary<CommandCategories, string>()
            {
                {CommandCategories.undefined,              "Undefined Category"            },
                {CommandCategories.aicomms,                "AI Comms"                      },
                {CommandCategories.aicommsflight,          "AI Comms | Flight"             },
                {CommandCategories.aicommsflightengage,    "AI Comms | Flight | Engage"    },
                {CommandCategories.aicommsflightmaneuver,  "AI Comms | Flight | Maneuver"  },
                {CommandCategories.aicommsflighttactical,  "AI Comms | Flight | Tactical"  },
                {CommandCategories.aicommsflightformation, "AI Comms | Flight | Formation" },
                {CommandCategories.aicommsjtac,            "AI Comms | JTAC"               },
                {CommandCategories.aicommsatc,             "AI Comms | ATC"                },
                {CommandCategories.aicommscarrier,         "AI Comms | Carrier ATC"        },
                {CommandCategories.aicommscarrier_LSO,     "AI Comms | Carrier LSO"        },
                {CommandCategories.aicommscarrier_Marshal, "AI Comms | Carrier Marshal"    },
                {CommandCategories.aicommscarrier_Approach,"AI Comms | Carrier Approach"   },
                {CommandCategories.aicommscarrier_Departure,"AI Comms | Carrier Departure" },
                {CommandCategories.aicommsawacs,           "AI Comms | AWACS"              },
                {CommandCategories.aicommstanker,          "AI Comms | Tanker"             },
                {CommandCategories.aicommscrew,            "AI Comms | Crew"               },
                {CommandCategories.aicommsaocs,            "AI Comms | AOCS"               },
                {CommandCategories.cockpit,                "Cockpit Device Commands"       },
                {CommandCategories.reply,                  "Reply Statements"              },
                {CommandCategories.auxmenu,                "F10 Menu Commands"             },
                {CommandCategories.menu,                   "Menu Commands"                 },
                {CommandCategories.cargocontrol,           "Cargo Menu Commands"           },
                {CommandCategories.special,                "Special Commands"              },
                {CommandCategories.RIO,                    "F-14 AI RIO"                   },
                {CommandCategories.RIO_menu,               "F-14 AI RIO | Menu Control"    },
                {CommandCategories.RIO_radar,              "F-14 AI RIO | Radar"           },
                {CommandCategories.RIO_weapons,            "F-14 AI RIO | Weapons"         },
                {CommandCategories.RIO_radio,              "F-14 AI RIO | Radio"           },
                {CommandCategories.RIO_utility,            "F-14 AI RIO | Utility"         },
                {CommandCategories.RIO_defensive,          "F-14 AI RIO | Defensive"       },
                {CommandCategories.RIO_misc,               "F-14 AI RIO | Misc"            },
                {CommandCategories.AI_pilot,               "F-14 AI Pilot"                 },
                {CommandCategories.kneeboard,              "Interactive Kneeboard extension"},
                {CommandCategories.Moose,                  "Moose Script | Ops"},
            };
        }

        public static class AppData
        {
            public static string RootFolder = Products.Products.Families.Vaicom.VaicomProPlugin.rootfoldername;

            public static Dictionary<string, string> SubFolders = new Dictionary<string, string>()
            {
                { "logfiles",   "Logs"      },
                { "config",     "Config"    },
                { "database",   "Database"  },
                { "profiles",   "Profiles"  },
                { "export",     "Export"    },
                //{ "extensions", "Extensions"}, //Extensions folder no longer required
            };

        }

    }
}
