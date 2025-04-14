using System;
using System.Collections.Generic;

namespace VAICOM
{
    namespace Database
    {

        public static partial class Aliases
        {

            public static Dictionary<string, string> appendiceswpn = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {

                { "Missile",                "withmissile"              },
                { "Missiles",               "withmissile"              },
                { "Maverick",               "withmissile"              },
                { "Mavericks",              "withmissile"              },
                { "Pickle",                 "withunguided"             },
                { "Pickles",                "withunguided"             },
                { "Bomb",                   "withunguided"             },
                { "Bombs",                  "withunguided"             },
                { "Stores",                 "withunguided"             },
                { "Guided",                 "withguided"               },
                { "GBU",                    "withguided"               },
                { "GBUs",                   "withguided"               },
                { "Paveway",                "withguided"               },
                { "Paveways",               "withguided"               },
                { "Rocket",                 "withrocket"               },
                { "Rockets",                "withrocket"               },
                { "Dart",                   "withrocket"               },
                { "Darts",                  "withrocket"               },
                { "Marker",                 "withmarker"               },
                { "Markers",                "withmarker"               },
                { "Paint",                  "withmarker"               },
                { "Smoke",                  "withmarker"               },
                { "Gun",                    "withgun"                  },
                { "Guns",                   "withgun"                  },
                { "Strafe",                 "withgun"                  },
                { "Bullet",                 "withgun"                  },
                { "Bullets",                "withgun"                  },

            };

            public static Dictionary<string, string> appendicesdir = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {

                { "In from the North",         "fromthenorth"                },
                { "In from the NorthEast",     "fromthenortheast"            },
                { "In from the East",          "fromtheeast"                 },
                { "In from the SouthEast",     "fromthesoutheast"            },
                { "In from the South",         "fromthesouth"                },
                { "In from the SouthWest",     "fromthesouthwest"            },
                { "In from the West",          "fromthewest"                 },
                { "In from the NorthWest",     "fromthenorthwest"            },

            };



        }
    }

}
