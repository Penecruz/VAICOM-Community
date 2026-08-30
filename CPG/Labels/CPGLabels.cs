using System;
using System.Collections.Generic;

namespace VAICOM.Extensions.CPG
{
    public static partial class Labels
    {
        // Labels for WSO recipients
        public static Dictionary<string, string> airecipients = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "george", "AH-64D AI CP/G" }
        };

        // Labels for CPG commands (mirrored from George AI labels)
        public static Dictionary<string, string> aicommands = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "georgeshowhide",                     "George Show/Hide"      },
            { "georgeup",                           "George Up"             },
            { "georgedown",                         "George Down"           },
            { "georgeleft",                         "George Left"           },
            { "georgeright",                        "George Right"          },
            { "georgecenter",                       "George Select"         },
            { "georgecontrolrequest",               "George Request Control"},
            { "georgestoretarget",                  "George Store Target"   },
            { "georgeuplong",                       "George Up (Hold)"      },
            { "georgedownlong",                     "George Down (Hold)"    },
            { "georgeleftlong",                     "George Left (Hold)"    },
            { "georgerightlong",                    "George Right (Hold)"   },
            { "georgecenterlong",                   "George Select (Hold)"  },
            { "georgemacrophssearch",               "George PHS Search"    },
            { "georgemacrotadslos",                 "George TADS LOS"       },
            { "georgemacronextsearch",              "George Next Search" },
            { "georgemacroprevioussearch",          "George Previous Search" },
            { "georgemacronextpoint",               "George Next Point" },
            { "georgemacropreviouspoint",           "George Previous Point" },
            { "georgelasetarget",                   "George Lase Target" },
            { "georgelaseron",                      "George Laser On" },
            { "georgelaseroff",                     "George Laser Off" },
            { "georgeburstlimit",                   "George Burst Limit" },
            { "georgerocketquantity",               "George Rocket Quantity" },
            { "georgelobl",                         "George LOBL" },
            { "georgeloal",                         "George LOAL" },
            { "georgenextweapon",                   "George Next Weapon" },
            { "georgeclearedfire",                  "George Cleared Fire" },
            { "georgeweaponsfree",                  "George Weapons Free" },
            { "georgeholdfire",                     "George Hold Fire" },
            { "georgestartup",                      "George Start Up" },
            { "georgeshutdown",                     "George Shutdown" },
            { "georgeadjustaim",                    "George Adjust Aim" },
            { "georgetadssensor",                   "George TADS Sensor" },
            { "georgetadsfov",                      "George TADS FOV" },
            { "georgetargetlistfilter",             "George Target List Filter" },
            { "georgepointlistfiltermode",          "George Point List Filter Mode" },
            { "georgepointlistfilterthreat",        "George Point List Filter Threat" },
            { "georgetargetlistzoomin",             "George Target List Zoom In" },
            { "georgetargetlistzoomout",            "George Target List Zoom Out" },
            { "georgepreviuoustarget",              "George Previous Target" },
            { "georgenexttarget",                   "George Next Target" },
            { "georgeexitlist",                     "George Exit List" },
            { "georgetracktarget",                  "George Track Target" },
            { "georgetadszoomin",                   "George TADS Zoom In" },
            { "georgetadszoomout",                  "George TADS Zoom Out" },
            { "georgenextrkt",                      "George Next Rocket" },
            { "georgemsltraj",                      "George Missile Trajectory" },
            { "georgeselecttarget",                 "George Select Target" },
            { "georgepreviousitem",                 "George Previous Item" },
            { "georgenextitem",                     "George Next Item" },
            { "georgelistitemselect",               "George List Item Select" },
            { "georgeareasearch",                   "George Area Search" },
            { "georgepointsearch",                  "George Point Search" },
            { "georgelaststoredtarget",             "George Last Stored Target" },
            { "georgeareaselect",                   "George Area Select" },
            { "georgepointselect",                  "George Point Select" },
            { "georgemacroaddtwotargetstrack",      "George Add and Track Top 2 Targets" },
            { "georgemacroaddthreetargetstrack",    "George Add and Track Top 3 Targets" },
            { "georgemacroaddfourtargetstrack",     "George Add and Track Top 4 Targets" },
            { "georgelastfoundtarget",              "George Last Found Target" },
            { "georgemacrotrackengage",             "George Track and Engage" },
            { "georgemacroselectgun",               "George Select Gun" },
            { "georgemacroselectmissiles",          "George Select Missiles" },
            { "georgemacroselectrockets",           "George Select Rockets" },
            { "georgemacroselectnoweapon",          "George Select No Weapon" },
            { "georgenextmsl",                      "George Next Missile Type" },
        };
    }
}