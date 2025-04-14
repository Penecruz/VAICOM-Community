using System;
using System.Collections.Generic;

namespace VAICOM
{
    namespace Database
    {

        public static partial class Commands
        {

            public static Dictionary<string, Command> Table = new Dictionary<string, Command>(StringComparer.OrdinalIgnoreCase)
            {

            // device control section 00000 //was this removed to limit scope creep???
            { "iCommandNull" ,          new Command { uniqueid = 00000, category = CommandCategories.cockpit, dcsid = "iCommandNull" } },
            //{ "gearup" ,                new Command { uniqueid = 00000, category = cmdcat.cockpit, eventnumber = 0068, name = "iCommandGearUp" ,   displayname=Labels.cockpitcontrol["gearup"],   hasparameter =  true, on = true} },
            //{ "geardown" ,              new Command { uniqueid = 00000, category = cmdcat.cockpit, eventnumber = 0068, name = "iCommandGearDown" , displayname=Labels.cockpitcontrol["geardown"], hasparameter =  true, on = false} },
            { "iCommandMaximum" ,       new Command { uniqueid = 09999, category = CommandCategories.cockpit, dcsid = "iCommandMaximum" } },


            // aicomms 10000
            { "wMsgNull" ,              new Command { uniqueid = 10000, category = CommandCategories.aicomms, eventnumber = 4000, dcsid = "wMsgNull",                        } },
            { "wMsgLeaderNull" ,        new Command { uniqueid = 10001, category = CommandCategories.aicomms, dcsid = "wMsgLeaderNull",                  } },

            // to Flight 11000
            { "wMsgLeaderToWingmenNull",new Command { uniqueid = 11000, category = CommandCategories.aicommsflight, dcsid = "wMsgLeaderToWingmenNull",         } },

            //engage
            { "mytarget" ,              new Command { uniqueid = 11001, category = CommandCategories.aicommsflightengage, dcsid = "wMsgLeaderEngageMyTarget",        displayname = Labels.aicommands["mytarget"] } },
            { "bandit" ,                new Command { uniqueid = 11002, category = CommandCategories.aicommsflightengage, dcsid = "wMsgLeaderEngageBandits",         displayname = Labels.aicommands["bandit"] } },
            { "groundtarget" ,          new Command { uniqueid = 11003, category = CommandCategories.aicommsflightengage, dcsid = "wMsgLeaderEngageGroundTargets",   displayname = Labels.aicommands["groundtarget"] } },
            { "armor" ,                 new Command { uniqueid = 11004, category = CommandCategories.aicommsflightengage, dcsid = "wMsgLeaderEngageArmor",           displayname = Labels.aicommands["armor"] } },
            { "artillery" ,             new Command { uniqueid = 11005, category = CommandCategories.aicommsflightengage, dcsid = "wMsgLeaderEngageArtillery",       displayname = Labels.aicommands["artillery"] } },
            { "airdefense" ,            new Command { uniqueid = 11006, category = CommandCategories.aicommsflightengage, dcsid = "wMsgLeaderEngageAirDefenses",     displayname = Labels.aicommands["airdefense"] } },
            { "utility" ,               new Command { uniqueid = 11007, category = CommandCategories.aicommsflightengage, dcsid = "wMsgLeaderEngageUtilityVehicles", displayname = Labels.aicommands["utility"] } },
            { "infantry" ,              new Command { uniqueid = 11008, category = CommandCategories.aicommsflightengage, dcsid = "wMsgLeaderEngageInfantry",        displayname = Labels.aicommands["infantry"] } },
            { "ship" ,                  new Command { uniqueid = 11009, category = CommandCategories.aicommsflightengage, dcsid = "wMsgLeaderEngageNavalTargets",    displayname = Labels.aicommands["ship"] } },
            { "dlinktarget" ,           new Command { uniqueid = 11010, category = CommandCategories.aicommsflightengage, dcsid = "wMsgLeaderEngageDlinkTarget",     displayname = Labels.aicommands["dlinktarget"] } },
            { "dlinktargets" ,          new Command { uniqueid = 11011, category = CommandCategories.aicommsflightengage, dcsid = "wMsgLeaderEngageDlinkTargets",    displayname = Labels.aicommands["dlinktargets"] } },
            { "dlinktargetbytype" ,     new Command { uniqueid = 11012, category = CommandCategories.aicommsflightengage, dcsid = "wMsgLeaderEngageDlinkTargetByType",   displayname = Labels.aicommands["dlinktargetbytype"] } },
            { "dlinktargetsbytype" ,    new Command { uniqueid = 11013, category = CommandCategories.aicommsflightengage, dcsid = "wMsgLeaderEngageDlinkTargetsByType",  displayname = Labels.aicommands["dlinktargetsbytype"] } },
            { "completeandrejoin" ,     new Command { uniqueid = 11014, category = CommandCategories.aicommsflightengage, dcsid = "wMsgLeaderFulfilTheTaskAndJoinUp",        displayname = Labels.aicommands["completeandrejoin"] } },
            { "completeandrtb" ,        new Command { uniqueid = 11015, category = CommandCategories.aicommsflightengage, dcsid = "wMsgLeaderFulfilTheTaskAndReturnToBase",  displayname = Labels.aicommands["completeandrtb"] } },
            { "raytarget" ,             new Command { uniqueid = 11016, category = CommandCategories.aicommsflightengage, dcsid = "wMsgLeaderRayTarget",             displayname = Labels.aicommands["raytarget"], blockedforFree = true} },
            { "myenemy" ,               new Command { uniqueid = 11017, category = CommandCategories.aicommsflightengage, dcsid = "wMsgLeaderMyEnemyAttack",         displayname = Labels.aicommands["myenemy"] } },
            { "coverme" ,               new Command { uniqueid = 11018, category = CommandCategories.aicommsflightengage, dcsid = "wMsgLeaderCoverMe",               displayname = Labels.aicommands["coverme"] } },

            // maneuver
            { "flightcheckin" ,         new Command { uniqueid = 11019, category = CommandCategories.aicommsflightmaneuver, dcsid = "wMsgLeaderFlightCheckIn",         displayname = Labels.aicommands["flightcheckin"], blockedforFree = true} },
            { "pincerright" ,           new Command { uniqueid = 11020, category = CommandCategories.aicommsflightmaneuver, dcsid = "wMsgLeaderPincerRight",           displayname = Labels.aicommands["pincerright"] } },
            { "pincerleft" ,            new Command { uniqueid = 11021, category = CommandCategories.aicommsflightmaneuver, dcsid = "wMsgLeaderPincerLeft",            displayname = Labels.aicommands["pincerleft"] } },
            { "pincerhigh" ,            new Command { uniqueid = 11022, category = CommandCategories.aicommsflightmaneuver, dcsid = "wMsgLeaderPincerHigh",            displayname = Labels.aicommands["pincerhigh"] } },
            { "pincerlow" ,             new Command { uniqueid = 11023, category = CommandCategories.aicommsflightmaneuver, dcsid = "wMsgLeaderPincerLow",             displayname = Labels.aicommands["pincerlow"] } },
            { "breakright" ,            new Command { uniqueid = 11024, category = CommandCategories.aicommsflightmaneuver, dcsid = "wMsgLeaderBreakRight",            displayname = Labels.aicommands["breakright"] } },
            { "breakleft" ,             new Command { uniqueid = 11025, category = CommandCategories.aicommsflightmaneuver, dcsid = "wMsgLeaderBreakLeft",             displayname = Labels.aicommands["breakleft"] } },
            { "breakhigh" ,             new Command { uniqueid = 11026, category = CommandCategories.aicommsflightmaneuver, dcsid = "wMsgLeaderBreakHigh",             displayname = Labels.aicommands["breakhigh"] } },
            { "breaklow" ,              new Command { uniqueid = 11027, category = CommandCategories.aicommsflightmaneuver, dcsid = "wMsgLeaderBreakLow",              displayname = Labels.aicommands["breaklow"] } },
            { "clearright" ,            new Command { uniqueid = 11028, category = CommandCategories.aicommsflightmaneuver, dcsid = "wMsgLeaderClearRight",            displayname = Labels.aicommands["clearright"] } },
            { "clearleft" ,             new Command { uniqueid = 11029, category = CommandCategories.aicommsflightmaneuver, dcsid = "wMsgLeaderClearLeft",             displayname = Labels.aicommands["clearleft"] } },
            { "pump" ,                  new Command { uniqueid = 11030, category = CommandCategories.aicommsflightmaneuver, dcsid = "wMsgLeaderPump",                  displayname = Labels.aicommands["pump"] } },
            { "anchorhere" ,            new Command { uniqueid = 11031, category = CommandCategories.aicommsflightmaneuver, dcsid = "wMsgLeaderAnchorHere",            displayname = Labels.aicommands["anchorhere"] } },
            { "anchoratsteerpoint" ,    new Command { uniqueid = 11032, category = CommandCategories.aicommsflightmaneuver, dcsid = "wMsgLeaderFlyAndOrbitAtSteerPoint", displayname = Labels.aicommands["anchoratsteerpoint"], blockedforFC = true } },
            { "anchoratspi" ,           new Command { uniqueid = 11033, category = CommandCategories.aicommsflightmaneuver, dcsid = "wMsgLeaderFlyAndOrbitAtSPI",      displayname = Labels.aicommands["anchoratspi"], blockedforFC = true } },
            { "anchoratpoint" ,         new Command { uniqueid = 11034, category = CommandCategories.aicommsflightmaneuver, dcsid = "wMsgLeaderFlyAndOrbitAtPoint",    displayname = Labels.aicommands["anchoratpoint"] } },
            { "returntobase" ,          new Command { uniqueid = 11035, category = CommandCategories.aicommsflightmaneuver, dcsid = "wMsgLeaderReturnToBase",          displayname = Labels.aicommands["returntobase"] } },
            { "flytotanker" ,           new Command { uniqueid = 11036, category = CommandCategories.aicommsflightmaneuver, dcsid = "wMsgLeaderGoRefueling",           displayname = Labels.aicommands["flytotanker"] } },
            { "joinup" ,                new Command { uniqueid = 11037, category = CommandCategories.aicommsflightmaneuver, dcsid = "wMsgLeaderJoinUp",                displayname = Labels.aicommands["joinup"] } },
            { "flyroute" ,              new Command { uniqueid = 11038, category = CommandCategories.aicommsflightmaneuver, dcsid = "wMsgLeaderFlyRoute",              displayname = Labels.aicommands["flyroute"] } },
            { "makerecon1" ,            new Command { uniqueid = 11039, category = CommandCategories.aicommsflightmaneuver, dcsid = "wMsgLeaderMakeRecon",             displayname = Labels.aicommands["makerecon1"], hasparameter = true, parameters = 1000, blockedforFree = true  } },
            { "makerecon2" ,            new Command { uniqueid = 11040, category = CommandCategories.aicommsflightmaneuver, dcsid = "wMsgLeaderMakeRecon",             displayname = Labels.aicommands["makerecon2"], hasparameter = true, parameters = 2000, blockedforFree = true } },
            { "makerecon3" ,            new Command { uniqueid = 11041, category = CommandCategories.aicommsflightmaneuver, dcsid = "wMsgLeaderMakeRecon",             displayname = Labels.aicommands["makerecon3"], hasparameter = true, parameters = 3000, blockedforFree = true } },
            { "makerecon5" ,            new Command { uniqueid = 11042, category = CommandCategories.aicommsflightmaneuver, dcsid = "wMsgLeaderMakeRecon",             displayname = Labels.aicommands["makerecon5"], hasparameter = true, parameters = 5000, blockedforFree = true  } },
            { "makerecon8" ,            new Command { uniqueid = 11043, category = CommandCategories.aicommsflightmaneuver, dcsid = "wMsgLeaderMakeRecon",             displayname = Labels.aicommands["makerecon8"], hasparameter = true, parameters = 8000, blockedforFree = true } },
            { "makerecon10" ,           new Command { uniqueid = 11044, category = CommandCategories.aicommsflightmaneuver, dcsid = "wMsgLeaderMakeRecon",             displayname = Labels.aicommands["makerecon10"],hasparameter = true, parameters = 10000,blockedforFree = true } },
            { "makereconatpoint" ,      new Command { uniqueid = 11045, category = CommandCategories.aicommsflightmaneuver, dcsid = "wMsgLeaderMakeReconAtPoint",      displayname = Labels.aicommands["makereconatpoint"],blockedforFree = true  } },

            //tactical
            { "radaron" ,               new Command { uniqueid = 11046, category = CommandCategories.aicommsflighttactical, dcsid = "wMsgLeaderRadarOn",               displayname = Labels.aicommands["radaron"] } },
            { "radaroff" ,              new Command { uniqueid = 11047, category = CommandCategories.aicommsflighttactical, dcsid = "wMsgLeaderRadarOff",              displayname = Labels.aicommands["radaroff"] } },
            { "ecmon" ,                 new Command { uniqueid = 11048, category = CommandCategories.aicommsflighttactical, dcsid = "wMsgLeaderDisturberOn",           displayname = Labels.aicommands["ecmon"] } },
            { "ecmoff" ,                new Command { uniqueid = 11049, category = CommandCategories.aicommsflighttactical, dcsid = "wMsgLeaderDisturberOff",          displayname = Labels.aicommands["ecmoff"] } },
            { "smokeon" ,               new Command { uniqueid = 11050, category = CommandCategories.aicommsflighttactical, dcsid = "wMsgLeaderSmokeOn",               displayname = Labels.aicommands["smokeon"] } },
            { "smokeoff" ,              new Command { uniqueid = 11051, category = CommandCategories.aicommsflighttactical, dcsid = "wMsgLeaderSmokeOff",              displayname = Labels.aicommands["smokeoff"] } },
            { "jettisonweapons" ,       new Command { uniqueid = 11052, category = CommandCategories.aicommsflighttactical, dcsid = "wMsgLeaderJettisonWeapons",       displayname = Labels.aicommands["jettisonweapons"] } },
            { "fencein" ,               new Command { uniqueid = 11053, category = CommandCategories.aicommsflighttactical, dcsid = "wMsgLeaderFenceIn",               displayname = Labels.aicommands["fencein"], blockedforFree = true } },
            { "fenceout" ,              new Command { uniqueid = 11054, category = CommandCategories.aicommsflighttactical, dcsid = "wMsgLeaderFenceOut",              displayname = Labels.aicommands["fenceout"], blockedforFree = true } },
            { "out" ,                   new Command { uniqueid = 11055, category = CommandCategories.aicommsflighttactical, dcsid = "wMsgLeaderOut",                   displayname = Labels.aicommands["out"], blockedforFree = true } },

            //formation
            { "golineabreast" ,         new Command { uniqueid = 11056, category = CommandCategories.aicommsflightformation, dcsid = "wMsgLeaderLineAbreast",           displayname = Labels.aicommands["golineabreast"] } },
            { "gotrail" ,               new Command { uniqueid = 11057, category = CommandCategories.aicommsflightformation, dcsid = "wMsgLeaderGoTrail",               displayname = Labels.aicommands["gotrail"] } },
            { "gowedge" ,               new Command { uniqueid = 11058, category = CommandCategories.aicommsflightformation, dcsid = "wMsgLeaderWedge",                 displayname = Labels.aicommands["gowedge"] } },
            { "goechelonright" ,        new Command { uniqueid = 11059, category = CommandCategories.aicommsflightformation, dcsid = "wMsgLeaderEchelonRight",          displayname = Labels.aicommands["goechelonright"] } },
            { "goechelonleft" ,         new Command { uniqueid = 11060, category = CommandCategories.aicommsflightformation, dcsid = "wMsgLeaderEchelonLeft",           displayname = Labels.aicommands["goechelonleft"] } },
            { "gofingerfour" ,          new Command { uniqueid = 11061, category = CommandCategories.aicommsflightformation, dcsid = "wMsgLeaderFingerFour",            displayname = Labels.aicommands["gofingerfour"] } },
            { "gospreadfour" ,          new Command { uniqueid = 11062, category = CommandCategories.aicommsflightformation, dcsid = "wMsgLeaderSpreadFour",            displayname = Labels.aicommands["gospreadfour"] } },
            { "closeformation" ,        new Command { uniqueid = 11063, category = CommandCategories.aicommsflightformation, dcsid = "wMsgLeaderCloseFormation",        displayname = Labels.aicommands["closeformation"] } },
            { "openformation" ,         new Command { uniqueid = 11064, category = CommandCategories.aicommsflightformation, dcsid = "wMsgLeaderOpenFormation",         displayname = Labels.aicommands["openformation"] } },
            { "closegroupformation" ,   new Command { uniqueid = 11065, category = CommandCategories.aicommsflightformation, dcsid = "wMsgLeaderCloseGroupFormation",   displayname = Labels.aicommands["closegroupformation"] , blockedforFree = true} },
            { "helogoheavy" ,           new Command { uniqueid = 11066, category = CommandCategories.aicommsflightformation, dcsid = "wMsgLeaderHelGoHeavy",            displayname = Labels.aicommands["helogoheavy"] } },
            { "helogoechelon" ,         new Command { uniqueid = 11067, category = CommandCategories.aicommsflightformation, dcsid = "wMsgLeaderHelGoEchelon",          displayname = Labels.aicommands["helogoechelon"] } },
            { "helogospread" ,          new Command { uniqueid = 11068, category = CommandCategories.aicommsflightformation, dcsid = "wMsgLeaderHelGoSpread",           displayname = Labels.aicommands["helogospread"] } },
            { "helogotrail" ,           new Command { uniqueid = 11069, category = CommandCategories.aicommsflightformation, dcsid = "wMsgLeaderHelGoTrail",            displayname = Labels.aicommands["helogotrail"] } },
            { "helogooverwatch" ,       new Command { uniqueid = 11070, category = CommandCategories.aicommsflightformation, dcsid = "wMsgLeaderHelGoOverwatch",        displayname = Labels.aicommands["helogooverwatch"] } },
            { "helogoleft" ,            new Command { uniqueid = 11071, category = CommandCategories.aicommsflightformation, dcsid = "wMsgLeaderHelGoLeft",             displayname = Labels.aicommands["helogoleft"] } },
            { "helogoright" ,           new Command { uniqueid = 11072, category = CommandCategories.aicommsflightformation, dcsid = "wMsgLeaderHelGoRight",            displayname = Labels.aicommands["helogoright"] } },
            { "helogotight" ,           new Command { uniqueid = 11073, category = CommandCategories.aicommsflightformation, dcsid = "wMsgLeaderHelGoTight",            displayname = Labels.aicommands["helogotight"], blockedforFree = true } },
            { "helogocruise" ,          new Command { uniqueid = 11074, category = CommandCategories.aicommsflightformation, dcsid = "wMsgLeaderHelGoCruise",           displayname = Labels.aicommands["helogocruise"], blockedforFree = true } },
            { "helogocombat" ,          new Command { uniqueid = 11075, category = CommandCategories.aicommsflightformation, dcsid = "wMsgLeaderHelGoCombat",           displayname = Labels.aicommands["helogocombat"], blockedforFree = true } },

            //tac turns
            { "30leftgo" ,              new Command { uniqueid = 11080, category = CommandCategories.aicommsflightformation, dcsid = "wMsgLeaderTacTurnLeft30",         displayname = Labels.aicommands["30leftgo"] } },
            { "30rightgo" ,             new Command { uniqueid = 11081, category = CommandCategories.aicommsflightformation, dcsid = "wMsgLeaderTacTurnRight30",        displayname = Labels.aicommands["30rightgo"] } },
            { "45leftgo" ,              new Command { uniqueid = 11082, category = CommandCategories.aicommsflightformation, dcsid = "wMsgLeaderTacTurnLeft45",         displayname = Labels.aicommands["45leftgo"] } },
            { "45rightgo" ,             new Command { uniqueid = 11083, category = CommandCategories.aicommsflightformation, dcsid = "wMsgLeaderTacTurnRight45",        displayname = Labels.aicommands["45rightgo"] } },
            { "60leftgo" ,              new Command { uniqueid = 11084, category = CommandCategories.aicommsflightformation, dcsid = "wMsgLeaderTacTurnLeft60",         displayname = Labels.aicommands["60leftgo"] } },
            { "60rightgo" ,             new Command { uniqueid = 11085, category = CommandCategories.aicommsflightformation, dcsid = "wMsgLeaderTacTurnRight60",        displayname = Labels.aicommands["60rightgo"] } },
            { "90leftgo" ,              new Command { uniqueid = 11086, category = CommandCategories.aicommsflightformation, dcsid = "wMsgLeaderTacTurnLeft90",         displayname = Labels.aicommands["90leftgo"] } },
            { "90rightgo" ,             new Command { uniqueid = 11087, category = CommandCategories.aicommsflightformation, dcsid = "wMsgLeaderTacTurnRight90",        displayname = Labels.aicommands["90rightgo"] } },
            { "turnaboutleftgo" ,       new Command { uniqueid = 11088, category = CommandCategories.aicommsflightformation, dcsid = "wMsgLeaderTacTurnLeft180",        displayname = Labels.aicommands["turnaboutleftgo"] } },
            { "turnaboutrightgo" ,      new Command { uniqueid = 11089, category = CommandCategories.aicommsflightformation, dcsid = "wMsgLeaderTacTurnRight180",       displayname = Labels.aicommands["turnaboutrightgo"] } },
            { "rotatego" ,              new Command { uniqueid = 11090, category = CommandCategories.aicommsflightformation, dcsid = "wMsgLeaderTacTurnRotate180",      displayname = Labels.aicommands["rotatego"] } },
            { "shacklego" ,             new Command { uniqueid = 11091, category = CommandCategories.aicommsflightformation, dcsid = "wMsgLeaderTacTurnShackle",        displayname = Labels.aicommands["shacklego"] } },
            { "HeadingN" ,              new Command { uniqueid = 11092, category = CommandCategories.aicommsflightformation, dcsid = "wMsgLeaderHeading000",            displayname = Labels.aicommands["HeadingN"] } },
            { "HeadingNE" ,             new Command { uniqueid = 11093, category = CommandCategories.aicommsflightformation, dcsid = "wMsgLeaderHeading045",            displayname = Labels.aicommands["HeadingNE"] } },
            { "HeadingE" ,              new Command { uniqueid = 11094, category = CommandCategories.aicommsflightformation, dcsid = "wMsgLeaderHeading090",            displayname = Labels.aicommands["HeadingE"] } },
            { "HeadingSE" ,             new Command { uniqueid = 11095, category = CommandCategories.aicommsflightformation, dcsid = "wMsgLeaderHeading135",            displayname = Labels.aicommands["HeadingSE"] } },
            { "HeadingS" ,              new Command { uniqueid = 11096, category = CommandCategories.aicommsflightformation, dcsid = "wMsgLeaderHeading180",            displayname = Labels.aicommands["HeadingS"] } },
            { "HeadingSW" ,             new Command { uniqueid = 11097, category = CommandCategories.aicommsflightformation, dcsid = "wMsgLeaderHeading225",            displayname = Labels.aicommands["HeadingSW"] } },
            { "HeadingW" ,              new Command { uniqueid = 11098, category = CommandCategories.aicommsflightformation, dcsid = "wMsgLeaderHeading270",            displayname = Labels.aicommands["HeadingW"] } },
            { "HeadingNW" ,             new Command { uniqueid = 11099, category = CommandCategories.aicommsflightformation, dcsid = "wMsgLeaderHeading315",            displayname = Labels.aicommands["HeadingNW"] } },
            { "Widen" ,                 new Command { uniqueid = 11100, category = CommandCategories.aicommsflightformation, dcsid = "wMsgLeaderWiden",                 displayname = Labels.aicommands["Widen"] } },
            { "CloseUp" ,               new Command { uniqueid = 11101, category = CommandCategories.aicommsflightformation, dcsid = "wMsgLeaderCloseUp",               displayname = Labels.aicommands["CloseUp"] } },

            {"wMsgLeaderToWingmenMaximum",new Command{uniqueid= 11130,  category = CommandCategories.aicommsflight, dcsid = "wMsgLeaderToWingmenMaximum",  } },

            // to JTAC 14000
            { "wMsgLeaderToFACNull",new Command { uniqueid = 14000, category = CommandCategories.aicommsjtac, dcsid = "wMsgLeaderToFACNull",          } },
            { "checkin" ,           new Command { uniqueid = 14001, category = CommandCategories.aicommsjtac, dcsid = "wMsgLeaderCheckIn",           displayname = Labels.aicommands["checkin"], blockedforFCnonPro = true} },
            { "checkin05" ,         new Command { uniqueid = 14002, category = CommandCategories.aicommsjtac, dcsid = "wMsgLeaderCheckIn",         displayname = Labels.aicommands["checkin05"], hasparameter = true, parameters = 0300 , blockedforFCnonPro = true } },
            { "checkin10" ,         new Command { uniqueid = 14003, category = CommandCategories.aicommsjtac, dcsid = "wMsgLeaderCheckIn",         displayname = Labels.aicommands["checkin10"], hasparameter = true, parameters = 0600 , blockedforFCnonPro = true } },
            { "checkin15" ,         new Command { uniqueid = 14004, category = CommandCategories.aicommsjtac, dcsid = "wMsgLeaderCheckIn",         displayname = Labels.aicommands["checkin15"], hasparameter = true, parameters = 0900 , blockedforFCnonPro = true } },
            { "checkin20" ,         new Command { uniqueid = 14005, category = CommandCategories.aicommsjtac, dcsid = "wMsgLeaderCheckIn",         displayname = Labels.aicommands["checkin20"], hasparameter = true, parameters = 1200 , blockedforFCnonPro = true } },
            { "checkin25" ,         new Command { uniqueid = 14006, category = CommandCategories.aicommsjtac, dcsid = "wMsgLeaderCheckIn",         displayname = Labels.aicommands["checkin25"], hasparameter = true, parameters = 1500 , blockedforFCnonPro = true } },
            { "checkin30" ,         new Command { uniqueid = 14007, category = CommandCategories.aicommsjtac, dcsid = "wMsgLeaderCheckIn",         displayname = Labels.aicommands["checkin30"], hasparameter = true, parameters = 1800 , blockedforFCnonPro = true } },
            { "checkin35" ,         new Command { uniqueid = 14008, category = CommandCategories.aicommsjtac, dcsid = "wMsgLeaderCheckIn",         displayname = Labels.aicommands["checkin35"], hasparameter = true, parameters = 2100 , blockedforFCnonPro = true } },
            { "checkin40" ,         new Command { uniqueid = 14009, category = CommandCategories.aicommsjtac, dcsid = "wMsgLeaderCheckIn",         displayname = Labels.aicommands["checkin40"], hasparameter = true, parameters = 2400 , blockedforFCnonPro = true } },
            { "checkin45" ,         new Command { uniqueid = 14010, category = CommandCategories.aicommsjtac, dcsid = "wMsgLeaderCheckIn",         displayname = Labels.aicommands["checkin45"], hasparameter = true, parameters = 2700 , blockedforFCnonPro = true } },
            { "checkin50" ,         new Command { uniqueid = 14011, category = CommandCategories.aicommsjtac, dcsid = "wMsgLeaderCheckIn",         displayname = Labels.aicommands["checkin50"], hasparameter = true, parameters = 3000 , blockedforFCnonPro = true } },
            { "checkin55" ,         new Command { uniqueid = 14012, category = CommandCategories.aicommsjtac, dcsid = "wMsgLeaderCheckIn",         displayname = Labels.aicommands["checkin55"], hasparameter = true, parameters = 3300 , blockedforFCnonPro = true } },
            { "checkin60" ,         new Command { uniqueid = 14013, category = CommandCategories.aicommsjtac, dcsid = "wMsgLeaderCheckIn",         displayname = Labels.aicommands["checkin60"], hasparameter = true, parameters = 3600 , blockedforFCnonPro = true } },
            { "checkout" ,          new Command { uniqueid = 14014, category = CommandCategories.aicommsjtac, dcsid = "wMsgLeaderCheckOut",          displayname = Labels.aicommands["checkout"], blockedforFCnonPro = true} },
            { "readytocopy" ,       new Command { uniqueid = 14015, category = CommandCategories.aicommsjtac, dcsid = "wMsgLeaderReadyToCopy",       displayname = Labels.aicommands["readytocopy"], blockedforFCnonPro = true} },
            { "readyforremarks" ,   new Command { uniqueid = 14016, category = CommandCategories.aicommsjtac, dcsid = "wMsgLeaderReadyToCopyRemarks",displayname = Labels.aicommands["readyforremarks"], blockedforFCnonPro = true} },
            { "ninelinereadback" ,  new Command { uniqueid = 14017, category = CommandCategories.aicommsjtac, dcsid = "wMsgLeader9LineReadback",     displayname = Labels.aicommands["ninelinereadback"], hasparameter = true, readback = true, blockedforFCnonPro = true} },
            { "requesttasking" ,    new Command { uniqueid = 14018, category = CommandCategories.aicommsjtac, dcsid = "wMsgLeaderRequestTasking",    displayname = Labels.aicommands["requesttasking"], blockedforFCnonPro = true} },
            { "requestbda" ,        new Command { uniqueid = 14019, category = CommandCategories.aicommsjtac, dcsid = "wMsgLeaderRequestBDA",        displayname = Labels.aicommands["requestbda"], blockedforFCnonPro = true} },
            { "requesttargetdescr", new Command { uniqueid = 14020, category = CommandCategories.aicommsjtac, dcsid = "wMsgLeaderRequestTargetDescription", displayname = Labels.aicommands["requesttargetdescr"], blockedforFCnonPro = true} },
            { "unabletocomply",     new Command { uniqueid = 14021, category = CommandCategories.aicommsjtac, dcsid = "wMsgLeaderUnableToComply",    displayname = Labels.aicommands["unabletocomply"], blockedforFCnonPro = true} },
            //{ "repeat",             new Command { uniqueid = 14022, category = CommandCategories.aicommsjtac, eventnumber = 4000, name = "wMsgLeaderFACRepeat",         displayname = Labels.aicommands["repeat"], blockedforFCnonPro = true} }, // 4117 disabled, has bug
            
            { "ipinbound",          new Command { uniqueid = 14023, category = CommandCategories.aicommsjtac, dcsid = "wMsgLeader_IP_INBOUND",       displayname = Labels.aicommands["ipinbound"], blockedforFCnonPro = true} },
            { "oneminute",          new Command { uniqueid = 14024, category = CommandCategories.aicommsjtac, dcsid = "wMsgLeader_ONE_MINUTE",       displayname = Labels.aicommands["oneminute"], blockedforFCnonPro = true} },
            { "in",                 new Command { uniqueid = 14025, category = CommandCategories.aicommsjtac, dcsid = "wMsgLeader_IN",               displayname = Labels.aicommands["in"], blockedforFCnonPro = true} },
            { "off",                new Command { uniqueid = 14026, category = CommandCategories.aicommsjtac, dcsid = "wMsgLeader_OFF",              displayname = Labels.aicommands["off"], blockedforFCnonPro = true} },
            { "attackcomplete",     new Command { uniqueid = 14027, category = CommandCategories.aicommsjtac, dcsid = "wMsgLeader_ATTACK_COMPLETE",  displayname = Labels.aicommands["attackcomplete"], blockedforFCnonPro = true} },
            { "advisereadyforbda",  new Command { uniqueid = 14028, category = CommandCategories.aicommsjtac, dcsid = "wMsgLeaderAdviseWhenReadyToCopyBDA", displayname = Labels.aicommands["advisereadyforbda"], blockedforFCnonPro = true} },
            { "contact",            new Command { uniqueid = 14029, category = CommandCategories.aicommsjtac, dcsid = "wMsgLeaderContact",           displayname = Labels.aicommands["contact"], hasparameter = true, parameters = 0, blockedforFCnonPro = true} }, // changed!
            { "nojoy",              new Command { uniqueid = 14030, category = CommandCategories.aicommsjtac, dcsid = "wMsgLeader_NO_JOY",           displayname = Labels.aicommands["nojoy"], blockedforFCnonPro = true} },
            { "contactthemark",     new Command { uniqueid = 14031, category = CommandCategories.aicommsjtac, dcsid = "wMsgLeader_CONTACT_the_mark", displayname = Labels.aicommands["contactthemark"], blockedforFCnonPro = true} },
            { "sparkle",            new Command { uniqueid = 14032, category = CommandCategories.aicommsjtac, dcsid = "wMsgLeader_SPARKLE",          displayname = Labels.aicommands["sparkle"], blockedforFCnonPro = true} },
            { "snake",              new Command { uniqueid = 14033, category = CommandCategories.aicommsjtac, dcsid = "wMsgLeader_SNAKE",            displayname = Labels.aicommands["snake"], blockedforFCnonPro = true} },
            { "pulse",              new Command { uniqueid = 14034, category = CommandCategories.aicommsjtac, dcsid = "wMsgLeader_PULSE",            displayname = Labels.aicommands["pulse"], blockedforFCnonPro = true} },
            { "steady",             new Command { uniqueid = 14035, category = CommandCategories.aicommsjtac, dcsid = "wMsgLeader_STEADY",           displayname = Labels.aicommands["steady"], blockedforFCnonPro = true} },
            { "rope",               new Command { uniqueid = 14036, category = CommandCategories.aicommsjtac, dcsid = "wMsgLeader_ROPE",             displayname = Labels.aicommands["rope"], blockedforFCnonPro = true} },
            { "contactsparkle",     new Command { uniqueid = 14037, category = CommandCategories.aicommsjtac, dcsid = "wMsgLeader_CONTACT_SPARKLE",  displayname = Labels.aicommands["contactsparkle"], blockedforFCnonPro = true} },
            { "stop",               new Command { uniqueid = 14038, category = CommandCategories.aicommsjtac, dcsid = "wMsgLeader_STOP",             displayname = Labels.aicommands["stop"], blockedforFCnonPro = true} },
            { "tenseconds",         new Command { uniqueid = 14039, category = CommandCategories.aicommsjtac, dcsid = "wMsgLeader_TEN_SECONDS",      displayname = Labels.aicommands["tenseconds"], blockedforFCnonPro = true} },
            { "laseron",            new Command { uniqueid = 14040, category = CommandCategories.aicommsjtac, dcsid = "wMsgLeader_LASER_ON",         displayname = Labels.aicommands["laseron"], blockedforFCnonPro = true} },
            { "shift",              new Command { uniqueid = 14041, category = CommandCategories.aicommsjtac, dcsid = "wMsgLeader_SHIFT",            displayname = Labels.aicommands["shift"], blockedforFCnonPro = true} },
            { "spot",               new Command { uniqueid = 14042, category = CommandCategories.aicommsjtac, dcsid = "wMsgLeader_SPOT",             displayname = Labels.aicommands["spot"], blockedforFCnonPro = true} },
            { "terminate",          new Command { uniqueid = 14043, category = CommandCategories.aicommsjtac, dcsid = "wMsgLeader_TERMINATE",        displayname = Labels.aicommands["terminate"], blockedforFCnonPro = true} },
            { "gunsgunsguns",       new Command { uniqueid = 14044, category = CommandCategories.aicommsjtac, dcsid = "wMsgLeaderGuns",              displayname = Labels.aicommands["gunsgunsguns"], blockedforFCnonPro = true, blockedforFree = true} },
            { "bombsaway",          new Command { uniqueid = 14045, category = CommandCategories.aicommsjtac, dcsid = "wMsgLeaderBombsAway",         displayname = Labels.aicommands["bombsaway"], blockedforFCnonPro = true, blockedforFree = true} },
            { "rifles",             new Command { uniqueid = 14046, category = CommandCategories.aicommsjtac, dcsid = "wMsgLeaderRIFLE",             displayname = Labels.aicommands["rifles"], blockedforFCnonPro = true, blockedforFree = true} },
            { "rockets",            new Command { uniqueid = 14047, category = CommandCategories.aicommsjtac, dcsid = "wMsgLeaderRockets",           displayname = Labels.aicommands["rockets"], blockedforFCnonPro = true, blockedforFree = true} },
            { "bda",                new Command { uniqueid = 14048, category = CommandCategories.aicommsjtac, dcsid = "wMsgLeaderBDA",               displayname = Labels.aicommands["bda"], blockedforFCnonPro = true, blockedforFree = true} },
            { "inflightrep",        new Command { uniqueid = 14049, category = CommandCategories.aicommsjtac, dcsid = "wMsgLeaderINFLIGHTREP",       displayname = Labels.aicommands["inflightrep"], blockedforFCnonPro = true, blockedforFree = true} },
            { "wMsgLeaderToFACMaximum",new Command{uniqueid= 14050, category = CommandCategories.aicommsjtac, dcsid = "wMsgLeaderToFACMaximum",  } },

            // to ATC 15000
            { "wMsgLeaderToATCNull" ,   new Command { uniqueid = 15000, category = CommandCategories.aicommsatc, dcsid = "wMsgLeaderToATCNull",              } },
            { "requestenginesstart" ,   new Command { uniqueid = 15001, category = CommandCategories.aicommsatc, dcsid = "wMsgLeaderRequestEnginesLaunch",  displayname = Labels.aicommands["requestenginesstart"] } },
            { "requesthover" ,          new Command { uniqueid = 15002, category = CommandCategories.aicommsatc, dcsid = "wMsgLeaderRequestControlHover",   displayname = Labels.aicommands["requesthover"] } },
            { "requesttaxitorunway" ,   new Command { uniqueid = 15003, category = CommandCategories.aicommsatc, dcsid = "wMsgLeaderRequestTaxiToRunway",   displayname = Labels.aicommands["requesttaxitorunway"] } },
            { "requesttakeoff" ,        new Command { uniqueid = 15004, category = CommandCategories.aicommsatc, dcsid = "wMsgLeaderRequestTakeOff",        displayname = Labels.aicommands["requesttakeoff"] } },
            { "aborttakeoff" ,          new Command { uniqueid = 15005, category = CommandCategories.aicommsatc, dcsid = "wMsgLeaderAbortTakeoff",          displayname = Labels.aicommands["aborttakeoff"] } },
            { "requestazimuth" ,        new Command { uniqueid = 15006, category = CommandCategories.aicommsatc, dcsid = "wMsgLeaderRequestAzimuth",        displayname = Labels.aicommands["requestazimuth"] } },
            { "inbound" ,               new Command { uniqueid = 15007, category = CommandCategories.aicommsatc, dcsid = "wMsgLeaderInbound",               displayname = Labels.aicommands["inbound"], hasparameter = true, point = new Servers.Server.Vector() } },
            { "abortinbound" ,          new Command { uniqueid = 15008, category = CommandCategories.aicommsatc, dcsid = "wMsgLeaderAbortInbound",          displayname = Labels.aicommands["abortinbound"] } },
            { "requestlanding" ,        new Command { uniqueid = 15009, category = CommandCategories.aicommsatc, dcsid = "wMsgLeaderRequestLanding",        displayname = Labels.aicommands["requestlanding"] } },
            { "reqtaxifortakeoff" ,     new Command { uniqueid = 15010, category = CommandCategories.aicommsatc, dcsid = "wMsgLeaderRequestTaxiForTakeoff", displayname = Labels.aicommands["reqtaxifortakeoff"], blockedforFree = true } },
            { "reqtaxitoparking" ,      new Command { uniqueid = 15011, category = CommandCategories.aicommsatc, dcsid = "wMsgLeaderRequestTaxiToParking",  displayname = Labels.aicommands["reqtaxitoparking"], blockedforFree = true } },
            { "towerreqtakeoff" ,       new Command { uniqueid = 15012, category = CommandCategories.aicommsatc, dcsid = "wMsgLeaderTowerRequestTakeOff",   displayname = Labels.aicommands["towerreqtakeoff"], blockedforFree = true } },
            { "inboundstraight" ,       new Command { uniqueid = 15013, category = CommandCategories.aicommsatc, dcsid = "wMsgLeaderInboundStraight",       displayname = Labels.aicommands["inboundstraight"], blockedforFree = true, requiresrealatc = true  } },
            { "approachoverhead" ,      new Command { uniqueid = 15014, category = CommandCategories.aicommsatc, dcsid = "wMsgLeaderApproachOverhead",      displayname = Labels.aicommands["approachoverhead"], blockedforFree = true, requiresrealatc = true  } },
            { "approachstraight" ,      new Command { uniqueid = 15015, category = CommandCategories.aicommsatc, dcsid = "wMsgLeaderApproachStraight",      displayname = Labels.aicommands["approachstraight"], blockedforFree = true, requiresrealatc = true  } },
            { "approachinstrument" ,    new Command { uniqueid = 15016, category = CommandCategories.aicommsatc, dcsid = "wMsgLeaderApproachInstrument",    displayname = Labels.aicommands["approachinstrument"], blockedforFree = true, requiresrealatc = true  } },

            // insert carrier ATC commands
            // { "seeyouat10" ,    new Command { uniqueid = 15017, category = CommandCategories.aicommsatc, eventnumber = 4091, name = "wMsgLeaderApproachInstrument",    displayname = Labels.aicommands["approachinstrument"], blockedforFree = true, requirescarrier = true } },

            { "wMsgLeaderToATCMaximum", new Command { uniqueid = 15017, category = CommandCategories.aicommsatc, dcsid = "wMsgLeaderToATCMaximum",          } },

            // to AWACS 16000
            { "wMsgLeaderToAWACSNull" , new Command { uniqueid = 16000, category = CommandCategories.aicommsawacs, dcsid = "wMsgLeaderToAWACSNull",           } },
            { "vectortobullseye" ,      new Command { uniqueid = 16001, category = CommandCategories.aicommsawacs, dcsid = "wMsgLeaderVectorToBullseye",      displayname = Labels.aicommands["vectortobullseye"] } },
            { "vectortobandit" ,        new Command { uniqueid = 16002, category = CommandCategories.aicommsawacs, dcsid = "wMsgLeaderVectorToNearestBandit", displayname = Labels.aicommands["vectortobandit"] } },
            { "vectortobase" ,          new Command { uniqueid = 16003, category = CommandCategories.aicommsawacs, dcsid = "wMsgLeaderVectorToHomeplate",     displayname = Labels.aicommands["vectortobase"] } },
            { "vectortotanker" ,        new Command { uniqueid = 16004, category = CommandCategories.aicommsawacs, dcsid = "wMsgLeaderVectorToTanker",        displayname = Labels.aicommands["vectortotanker"] } },
            { "declare" ,               new Command { uniqueid = 16005, category = CommandCategories.aicommsawacs, dcsid = "wMsgLeaderDeclare",               displayname = Labels.aicommands["declare"] } },
            { "requestpicture" ,        new Command { uniqueid = 16006, category = CommandCategories.aicommsawacs, dcsid = "wMsgLeaderPicture",               displayname = Labels.aicommands["requestpicture"] } },
            { "wMsgLeaderToAWACSMaximum",new Command{ uniqueid = 16007, category = CommandCategories.aicommsawacs, dcsid = "wMsgLeaderToAWACSMaximum",        } },

            // to Tanker 17000
            { "wMsgLeaderToTankerNull", new Command { uniqueid = 17000, category = CommandCategories.aicommstanker, dcsid = "wMsgLeaderToTankerNull",          } },
            { "intenttorefuel" ,        new Command { uniqueid = 17001, category = CommandCategories.aicommstanker, dcsid = "wMsgLeaderIntentToRefuel",        displayname = Labels.aicommands["intenttorefuel"] } },
            { "abortrefuel" ,           new Command { uniqueid = 17002, category = CommandCategories.aicommstanker, dcsid = "wMsgLeaderAbortRefueling",        displayname = Labels.aicommands["abortrefuel"] } },
            { "readyprecontact" ,       new Command { uniqueid = 17003, category = CommandCategories.aicommstanker, dcsid = "wMsgLeaderReadyPreContact",       displayname = Labels.aicommands["readyprecontact"] } },
            { "stoprefueling" ,         new Command { uniqueid = 17004, category = CommandCategories.aicommstanker, dcsid = "wMsgLeaderStopRefueling",         displayname = Labels.aicommands["stoprefueling"] } },
            { "wMsgLeaderToTankerMaximum",new Command{uniqueid = 17005, category = CommandCategories.aicommstanker, dcsid = "wMsgLeaderToTankerMaximum",       } },

            // to Crew 18000
            { "wMsgLeaderToGroundCrewNull" ,    new Command { uniqueid = 18000, category = CommandCategories.aicommscrew, dcsid = "wMsgLeaderToGroundCrewNull",  } },
            //{ "dospecial" ,                     new Command { uniqueid = 18001, category = cmdcat.aicommscrew, eventnumber = 4147, name = "wMsgLeaderSpecialCommand",    displayname = Labels.aicommands["dospecial"] } },
            { "stowboardingladder" ,            new Command { uniqueid = 18002, category = CommandCategories.aicommscrew, dcsid = "wMsgLeaderSpecialCommand",displayname = Labels.aicommands["stowboardingladder"], hasparameter = true, type = 7  } },
            { "runinertialstarter" ,            new Command { uniqueid = 18003, category = CommandCategories.aicommscrew, dcsid = "wMsgLeaderSpecialCommand",displayname = Labels.aicommands["runinertialstarter"], hasparameter = true, type = 9 } },
            { "requesthmd" ,                    new Command { uniqueid = 18004, category = CommandCategories.aicommscrew, dcsid = "wMsgLeaderSpecialCommand",displayname = Labels.aicommands["requesthmd"], hasparameter = true, type = 4, device = 0 } },
            { "requestnvg" ,                    new Command { uniqueid = 18005, category = CommandCategories.aicommscrew, dcsid = "wMsgLeaderSpecialCommand",displayname = Labels.aicommands["requestnvg"], hasparameter = true, type = 4, device = 1 } },
            { "loadwater" ,                     new Command { uniqueid = 18006, category = CommandCategories.aicommscrew, dcsid = "wMsgLeaderSpecialCommand",displayname = Labels.aicommands["loadwater"], hasparameter = true, volume = 1.0, device = 28 } },
            // type = 4 : COMMAND_CHANGE_COCKPIT_EQUIPMENT
            // type = 4, device = 0 --> jhmcs
            // type = 4, device = 1 --> nvg
            // type = 4, device = 2 --> gun sight
            // type = 4, device = 3 --> bomb sight
            // type = 4, device = 4 --> flare pistol , param action = 0 or 1
            // type = 4, device = 5 --> blind screen
            { "turboon" ,                       new Command { uniqueid = 18007, category = CommandCategories.aicommscrew, dcsid = "wMsgLeaderSpecialCommand",           displayname = Labels.aicommands["turboon"], hasparameter = true, type = 5, power_source = 0   } },
            { "turbooff" ,                      new Command { uniqueid = 18008, category = CommandCategories.aicommscrew, dcsid = "wMsgLeaderSpecialCommand",          displayname = Labels.aicommands["turbooff"], hasparameter = true, type = 5, power_source = 1 } },
            { "eppuon" ,                        new Command { uniqueid = 18009, category = CommandCategories.aicommscrew, dcsid = "wMsgLeaderSpecialCommand",            displayname = Labels.aicommands["eppuon"], hasparameter = true, parametername = "EPPU", value = true  } },
            { "eppuoff" ,                       new Command { uniqueid = 18010, category = CommandCategories.aicommscrew, dcsid = "wMsgLeaderSpecialCommand",           displayname = Labels.aicommands["eppuoff"], hasparameter = true, parametername = "EPPU", value = false } },
            { "requestrefueling" ,              new Command { uniqueid = 18011, category = CommandCategories.aicommscrew, dcsid = "wMsgLeaderRequestRefueling",      displayname = Labels.aicommands["requestrefueling"], hasparameter = true, volume = 1.0, blockedforFree = true } },
            { "requestcannonreload" ,           new Command { uniqueid = 18012, category = CommandCategories.aicommscrew, dcsid = "wMsgLeaderRequestCannonReloading",displayname = Labels.aicommands["requestcannonreload"], blockedforFree = true } },
            { "requestrearming" ,               new Command { uniqueid = 18013, category = CommandCategories.aicommscrew, dcsid = "wMsgLeaderRequestRearming",       displayname = Labels.aicommands["requestrearming"] } },
            { "groundpoweron" ,                 new Command { uniqueid = 18014, category = CommandCategories.aicommscrew, dcsid = "wMsgLeaderGroundToggleElecPower",        displayname = Labels.aicommands["groundpoweron"], hasparameter = true, on = true} },
            { "groundpoweroff" ,                new Command { uniqueid = 18015, category = CommandCategories.aicommscrew, dcsid = "wMsgLeaderGroundToggleElecPower",        displayname = Labels.aicommands["groundpoweroff"], hasparameter = true, on = false} },
            { "wheelchocksplace" ,              new Command { uniqueid = 18016, category = CommandCategories.aicommscrew, dcsid = "wMsgLeaderGroundToggleWheelChocks",      displayname = Labels.aicommands["wheelchocksplace"], hasparameter = true, on = true, blockedforFree = true} },
            { "wheelchocksremove" ,             new Command { uniqueid = 18017, category = CommandCategories.aicommscrew, dcsid = "wMsgLeaderGroundToggleWheelChocks",     displayname = Labels.aicommands["wheelchocksremove"], hasparameter = true, on = false, blockedforFree = true} },
            { "Seq_J_CANOPY_OPEN" ,             new Command { uniqueid = 18018, category = CommandCategories.aicommscrew, dcsid = "wMsgLeaderGroundToggleCanopy",            displayname = Labels.aicommands["Seq_J_CANOPY_OPEN"], hasparameter = true, close = true, blockedforFree = true} }, // on = true
            { "Seq_J_CANOPY_CLOSE" ,            new Command { uniqueid = 18019, category = CommandCategories.aicommscrew, dcsid = "wMsgLeaderGroundToggleCanopy",           displayname = Labels.aicommands["Seq_J_CANOPY_CLOSE"], hasparameter = true, close = false,  blockedforFree = true} }, // on = false
            { "airsupplyconnect" ,              new Command { uniqueid = 18020, category = CommandCategories.aicommscrew, dcsid = "wMsgLeaderGroundToggleAir",      displayname = Labels.aicommands["airsupplyconnect"], hasparameter = true, on = true, blockedforFree = true} },
            { "airsupplydisconnect" ,           new Command { uniqueid = 18021, category = CommandCategories.aicommscrew, dcsid = "wMsgLeaderGroundToggleAir",    displayname = Labels.aicommands["airsupplydisconnect"], hasparameter = true, on = false, blockedforFree = true} },
            { "applyair" ,                      new Command { uniqueid = 18022, category = CommandCategories.aicommscrew, dcsid = "wMsgLeaderGroundApplyAir",        displayname = Labels.aicommands["applyair"], hasparameter = true, on = true, blockedforFree = true } },
            { "requestrepair" ,                 new Command { uniqueid = 18023, category = CommandCategories.aicommscrew, dcsid = "wMsgLeaderGroundRepair",          displayname = Labels.aicommands["requestrepair"] } },

            // new for Supercarrier
            { "crewsalute" ,                    new Command { uniqueid = 18024, category = CommandCategories.aicommscrew, dcsid = "wMsgLeaderGroundGestureSalut",    displayname = Labels.aicommands["crewsalute"], hasparameter = true, on = true, blockedforFree = true, requiresrealatc = true    } }, // type is to be found out
            { "crewrequestcatlaunch" ,          new Command { uniqueid = 18025, category = CommandCategories.aicommscrew, dcsid = "wMsgLeaderGroundRequestLaunch",   displayname = Labels.aicommands["crewrequestcatlaunch"], hasparameter = true, on = true, blockedforFree = true, requiresrealatc = true    } }, // type is to be found out

            { "wMsgLeaderToGroundCrewMaximum" , new Command { uniqueid = 18030, category = CommandCategories.aicommscrew, dcsid = "wMsgLeaderToGroundCrewMaximum",   } },
            //service max
            { "wMsgLeaderMaximum" ,             new Command { uniqueid = 18035, category = CommandCategories.aicomms,     dcsid = "wMsgLeaderMaximum",               } },

            // navy carrier comms
            { "wMsgLeaderToNavyATCNull" ,       new Command { uniqueid = 18036, category = CommandCategories.aicommscarrier, dcsid = "wMsgLeaderToNavyATCNull",         } },
            { "wMsgLeaderInboundCarrier" ,      new Command { uniqueid = 18037, category = CommandCategories.aicommscarrier, dcsid = "wMsgLeaderInboundCarrier",        displayname = Labels.aicommands["wMsgLeaderInboundCarrier"], blockedforFree = true, requiresrealatc = true        } },
            { "wMsgLeaderConfirm" ,             new Command { uniqueid = 18038, category = CommandCategories.aicommscarrier, dcsid = "wMsgLeaderConfirm",               displayname = Labels.aicommands["wMsgLeaderConfirm"], blockedforFree = true, requiresrealatc = true        } },
            { "wMsgLeaderConfirmRemainingFuel", new Command { uniqueid = 18039, category = CommandCategories.aicommscarrier, dcsid = "wMsgLeaderConfirmRemainingFuel",  displayname = Labels.aicommands["wMsgLeaderConfirmRemainingFuel"], blockedforFree = true, requiresrealatc = true        } },
            { "wMsgLeaderInboundMarshallRespond",new Command { uniqueid = 18040, category = CommandCategories.aicommscarrier, dcsid = "wMsgLeaderInboundMarshallRespond",displayname = Labels.aicommands["wMsgLeaderInboundMarshallRespond"],blockedforFree = true, requiresrealatc = true        } },
            { "wMsgLeaderEsteblished",          new Command { uniqueid = 18041, category = CommandCategories.aicommscarrier, dcsid = "wMsgLeaderEstablished",           displayname = Labels.aicommands["wMsgLeaderEsteblished"],blockedforFree = true, requiresrealatc = true        } },
            { "wMsgLeaderCommencing",           new Command { uniqueid = 18042, category = CommandCategories.aicommscarrier, dcsid = "wMsgLeaderCommencing",            displayname = Labels.aicommands["wMsgLeaderCommencing"],blockedforFree = true, requiresrealatc = true        } },
            { "wMsgLeaderCheckingIn",           new Command { uniqueid = 18043, category = CommandCategories.aicommscarrier, dcsid = "wMsgLeaderCheckingIn",            displayname = Labels.aicommands["wMsgLeaderCheckingIn"],blockedforFree = true, requiresrealatc = true        } },
            { "wMsgLeaderPlatform",             new Command { uniqueid = 18044, category = CommandCategories.aicommscarrier, dcsid = "wMsgLeaderPlatform",              displayname = Labels.aicommands["wMsgLeaderPlatform"],blockedforFree = true, requiresrealatc = true        } },
            { "wMsgLeaderSayNeedle",            new Command { uniqueid = 18045, category = CommandCategories.aicommscarrier, dcsid = "wMsgLeaderSayNeedle",             displayname = Labels.aicommands["wMsgLeaderSayNeedle"],blockedforFree = true, requiresrealatc = true        } },
            { "wMsgLeaderSeeYouAtTen",          new Command { uniqueid = 18046, category = CommandCategories.aicommscarrier, dcsid = "wMsgLeaderSeeYouAtTen",           displayname = Labels.aicommands["wMsgLeaderSeeYouAtTen"],blockedforFree = true, requiresrealatc = true        } },
            { "wMsgLeaderHornetBall",           new Command { uniqueid = 18047, category = CommandCategories.aicommscarrier, dcsid = "wMsgLeaderHornetBall",            displayname = Labels.aicommands["wMsgLeaderHornetBall"],blockedforFree = true, requiresrealatc = true        } },
            { "wMsgLeaderCLARA",                new Command { uniqueid = 18048, category = CommandCategories.aicommscarrier, dcsid = "wMsgLeaderCLARA",                 displayname = Labels.aicommands["wMsgLeaderCLARA"],blockedforFree = true, requiresrealatc = true        } },
            { "wMsgLeaderBall",                 new Command { uniqueid = 18049, category = CommandCategories.aicommscarrier, dcsid = "wMsgLeaderBall",                  displayname = Labels.aicommands["wMsgLeaderBall"],blockedforFree = true, requiresrealatc = true        } },
            { "wMsgLeaderTowerOverhead",        new Command { uniqueid = 18050, category = CommandCategories.aicommscarrier, dcsid = "wMsgLeaderTowerOverhead",         displayname = Labels.aicommands["wMsgLeaderTowerOverhead"],blockedforFree = true, requiresrealatc = true        } },
            { "wMsgLeaderFlightKissOff",        new Command { uniqueid = 18051, category = CommandCategories.aicommscarrier, dcsid = "wMsgLeaderFlightKissOff",         displayname = Labels.aicommands["wMsgLeaderFlightKissOff"],blockedforFree = true, requiresrealatc = true        } },
            { "wMsgLeaderAirborn",              new Command { uniqueid = 18052, category = CommandCategories.aicommscarrier, dcsid = "wMsgLeaderAirborn",               displayname = Labels.aicommands["wMsgLeaderAirborn"],blockedforFree = true, requiresrealatc = true        } },
            { "wMsgLeaderPassing2_5Kilo",       new Command { uniqueid = 18053, category = CommandCategories.aicommscarrier, dcsid = "wMsgLeaderPassing2_5Kilo",        displayname = Labels.aicommands["wMsgLeaderPassing2_5Kilo"],blockedforFree = true, requiresrealatc = true        } },

            { "wMsgLeaderToNavyATCMaximum" ,    new Command { uniqueid = 18060, category = CommandCategories.aicommscarrier, dcsid = "wMsgLeaderToNavyATCMaximum",      } },
            // end carrier comms

            /// ----------------------------------------------------------------------
            /// non-player command ranges
            
            /// from wingmen
            { "wMsgWingmenNull" ,       new Command { dcsid = "wMsgWingmenNull"     } },
            { "wMsgWingmenMaximum" ,    new Command { dcsid = "wMsgWingmenMaximum"  } },        
            /// allied flight
            { "wMsgFlightNull" ,        new Command { dcsid = "wMsgFlightNull"      } },
            { "wMsgFlightMaximum" ,     new Command { dcsid = "wMsgFlightMaximum"   } },
            /// ATC
            { "wMsgATCNull" ,           new Command { dcsid = "wMsgATCNull"         } },
            { "wMsgATCMaximum" ,        new Command { dcsid = "wMsgATCMaximum"      } },
            /// Carrier ATC - Departure
            { "wMsgATCNAVYDepartureNull" ,      new Command {  dcsid = "wMsgATCNAVYDepartureNull"         } },
            { "wMsgATCNAVYDepartureMaximum" ,   new Command {  dcsid = "wMsgATCNAVYDepartureMaximum"      } },
            /// Carrier ATC - Marshal
            { "wMsgATCNAVYMarshalNull" ,      new Command {  dcsid = "wMsgATCNAVYMarshalNull"         } },
            { "wMsgATCNAVYMarshalMaximum" ,   new Command {  dcsid = "wMsgATCNAVYMarshalMaximum"      } },
            /// Carrier ATC - Approach Tower
            { "wMsgATCNAVYApproachTowerNull" ,      new Command {  dcsid = "wMsgATCNAVYApproachTowerNull"         } },
            { "wMsgATCNAVYApproachTowerMaximum" ,   new Command {  dcsid = "wMsgATCNAVYApproachTowerMaximum"      } },
            /// Carrier ATC - LSO
            { "wMsgATCNAVYLSONull" ,      new Command {  dcsid = "wMsgATCNAVYLSONull"         } },
            { "wMsgATCNAVYLSOMaximum" ,   new Command {  dcsid = "wMsgATCNAVYLSOMaximum"      } },
            /// AWACS
            { "wMsgAWACSNull" ,      new Command {  dcsid = "wMsgAWACSNull"         } },
            { "wMsgAWACSMaximum" ,   new Command {  dcsid = "wMsgAWACSMaximum"      } },
            /// Tanker
            { "wMsgTankerNull" ,      new Command {  dcsid = "wMsgTankerNull"         } },
            { "wMsgTankerMaximum" ,   new Command {  dcsid = "wMsgTankerMaximum"      } },
            /// FAC
            { "wMsgFACNull" ,      new Command {  dcsid = "wMsgFACNull"         } },
            { "wMsgFACMaximum" ,   new Command {  dcsid = "wMsgFACMaximum"      } },
            /// CCC
            { "wMsgCCCNull" ,      new Command {  dcsid = "wMsgCCCNull"         } }, //
            { "wMsgCCCMaximum" ,   new Command {  dcsid = "wMsgCCCMaximum"      } }, //
            /// Ground Crew
            { "wMsgGroundCrewNull" ,      new Command {  dcsid = "wMsgGroundCrewNull"         } },
            { "wMsgGroundCrewMaximum" ,   new Command {  dcsid = "wMsgGroundCrewMaximum"      } }, 
            /// Betty
            { "wMsgBettyNull" ,      new Command {  dcsid = "wMsgBettyNull"         } },
            { "wMsgBettyMaximum" ,   new Command {  dcsid = "wMsgBettyMaximum"      } },
            /// ALMAZ_
            { "wMsgALMAZ_Null" ,    new Command {  dcsid = "wMsgALMAZ_Null"         } },
            { "wMsgALMAZ_Maximum" , new Command {  dcsid = "wMsgALMAZ_Maximum"      } },
            /// RI65_
            { "wMsgRI65_Null" ,    new Command {  dcsid = "wMsgRI65_Null"         } },
            { "wMsgRI65_Maximum" , new Command {  dcsid = "wMsgRI65_Maximum"      } },
            /// External Cargo
            { "wMsgExternalCargo_Null" ,    new Command {  dcsid = "wMsgExternalCargo_Null"         } },
            { "wMsgExternalCargo_Maximum" , new Command {  dcsid = "wMsgExternalCargo_Maximum"      } },

            /// (Mi8 checklist)

            /// A10_VMU
            { "wMsgA10_VMU_Null" ,    new Command {  dcsid = "wMsgA10_VMU_Null"         } },
            { "wMsgA10_VMU_Maximum" , new Command {  dcsid = "wMsgA10_VMU_Maximum"      } },
            //..



            ///
            /// ----------------------------------------------------------------------

            { "wMsgMaximum" ,                   new Command { uniqueid = 19999, category = CommandCategories.aicomms, dcsid = "wMsgMaximum",                     } },
            // end of regular commands

            // added stuff (all 4000 events): ----------------------------------------
            //

            // to Aux (F10 menu) 20000
            { "wMsgLeaderToAuxNull" ,           new Command { uniqueid = 20000, category = CommandCategories.auxmenu, eventnumber = 4000, dcsid = "wMsgLeaderToAuxNull",             } },
            // AI_ATC endpoints start
            //{ "Action Request Engine Start" ,   new Command { uniqueid = 20001, category = CommandCategories.auxmenu, eventnumber = 4000, dcsid = "ActionRequestEngineStart", displayname = Labels.aicommands["Request Engine Start"] } },
            //{ "Action Request Taxi Clearance" , new Command { uniqueid = 20002, category = CommandCategories.auxmenu, eventnumber = 4000, dcsid = "ActionRequestTaxiClearance", displayname = Labels.aicommands["Request Taxi Clearance"] } },
            // AI_ATC endpoints finish
            { "wMsgLeaderToAuxMaximum" ,        new Command { uniqueid = 20999, category = CommandCategories.auxmenu, eventnumber = 4000, dcsid = "wMsgLeaderToAuxMaximum",          } },

            // to Cargo 21000
            { "wMsgLeaderToCargoNull",          new Command { uniqueid = 21000, category = CommandCategories.cargocontrol, eventnumber = 4000, dcsid = "wMsgLeaderToCargoNull",           } },
            { "wMsgLeaderToCargoMaximum",       new Command { uniqueid = 21499, category = CommandCategories.cargocontrol, eventnumber = 4000, dcsid = "wMsgLeaderToCargoMaximum",        } },
        
            // to Cargo 21500
            { "wMsgLeaderToDescentNull",        new Command { uniqueid = 21500, category = CommandCategories.cargocontrol, eventnumber = 4000, dcsid = "wMsgLeaderToDescentNull",           } },
            { "wMsgLeaderToDescentMaximum",     new Command { uniqueid = 21999, category = CommandCategories.cargocontrol, eventnumber = 4000, dcsid = "wMsgLeaderToDescentMaximum",        } },

            // replies 22000
            { "wMsgReplyNull" ,                 new Command { uniqueid = 22000, category = CommandCategories.reply, eventnumber = 4000, dcsid = "wMsgReplyNull",    } },
            { "roger" ,                         new Command { uniqueid = 22001, category = CommandCategories.reply, eventnumber = 4000, dcsid = "wMsgReplyRoger",   displayname = Labels.aicommands["roger"] } },
            { "copy" ,                          new Command { uniqueid = 22002, category = CommandCategories.reply, eventnumber = 4000, dcsid = "wMsgReplyCopy",    displayname = Labels.aicommands["copy"] } },
            { "affirm" ,                        new Command { uniqueid = 22003, category = CommandCategories.reply, eventnumber = 4000, dcsid = "wMsgReplyAffirm",  displayname = Labels.aicommands["affirm"] } },
            { "wilco" ,                         new Command { uniqueid = 22004, category = CommandCategories.reply, eventnumber = 4000, dcsid = "wMsgReplyWilco",   displayname = Labels.aicommands["wilco"] } },
            { "negative" ,                      new Command { uniqueid = 22005, category = CommandCategories.reply, eventnumber = 4000, dcsid = "wMsgReplyNegative",displayname = Labels.aicommands["negative"] } },

            // repeat
            
            { "wMsgReplyMaximum",               new Command { uniqueid = 22499, category = CommandCategories.reply, eventnumber = 4000, dcsid = "wMsgReplyMaximum", } },

            // menu items 22500
            { "wMsgMenuNull" ,                 new Command { uniqueid = 22500, category = CommandCategories.menu, eventnumber = 4000, dcsid = "wMsgMenuNull",    } },
            { "menu01" ,                       new Command { uniqueid = 22501, category = CommandCategories.menu, eventnumber = 4000, dcsid = "wMsgMenu01",   displayname = Labels.aicommands["menu01"] } },
            { "menu02" ,                       new Command { uniqueid = 22502, category = CommandCategories.menu, eventnumber = 4000, dcsid = "wMsgMenu02",   displayname = Labels.aicommands["menu02"] } },
            { "menu03" ,                       new Command { uniqueid = 22503, category = CommandCategories.menu, eventnumber = 4000, dcsid = "wMsgMenu03",   displayname = Labels.aicommands["menu03"] } },
            { "menu04" ,                       new Command { uniqueid = 22504, category = CommandCategories.menu, eventnumber = 4000, dcsid = "wMsgMenu04",   displayname = Labels.aicommands["menu04"] } },
            { "menu05" ,                       new Command { uniqueid = 22505, category = CommandCategories.menu, eventnumber = 4000, dcsid = "wMsgMenu05",   displayname = Labels.aicommands["menu05"] } },
            { "menu06" ,                       new Command { uniqueid = 22506, category = CommandCategories.menu, eventnumber = 4000, dcsid = "wMsgMenu06",   displayname = Labels.aicommands["menu06"] } },
            { "menu07" ,                       new Command { uniqueid = 22507, category = CommandCategories.menu, eventnumber = 4000, dcsid = "wMsgMenu07",   displayname = Labels.aicommands["menu07"] } },
            { "menu08" ,                       new Command { uniqueid = 22508, category = CommandCategories.menu, eventnumber = 4000, dcsid = "wMsgMenu08",   displayname = Labels.aicommands["menu08"] } },
            { "menu09" ,                       new Command { uniqueid = 22509, category = CommandCategories.menu, eventnumber = 4000, dcsid = "wMsgMenu09",   displayname = Labels.aicommands["menu09"] } },
            { "menu10" ,                       new Command { uniqueid = 22510, category = CommandCategories.menu, eventnumber = 4000, dcsid = "wMsgMenu10",   displayname = Labels.aicommands["menu10"] } },
            { "menu11" ,                       new Command { uniqueid = 22511, category = CommandCategories.menu, eventnumber = 4000, dcsid = "wMsgMenu11",   displayname = Labels.aicommands["menu11"] } },
            { "menu12" ,                       new Command { uniqueid = 22512, category = CommandCategories.menu, eventnumber = 4000, dcsid = "wMsgMenu12",   displayname = Labels.aicommands["menu12"] } },
            { "wMsgMenuMaximum",               new Command { uniqueid = 22999, category = CommandCategories.menu, eventnumber = 4000, dcsid = "wMsgMenuMaximum", } },

            // special commands 23000
            { "wMsgSpecialCmndsNull",          new Command { uniqueid = 23000, category = CommandCategories.special, eventnumber = 4000, dcsid = "wMsgSpecialCmndsNull",   } },
            { "select",                        new Command { uniqueid = 23001, category = CommandCategories.special, eventnumber = 4000, dcsid = "wMsgSelectRecipient",    displayname = Labels.aicommands["select"] } },
            { "options",                       new Command { uniqueid = 23002, category = CommandCategories.special, eventnumber = 4000, dcsid = "wMsgShowOptions",        displayname = Labels.aicommands["options"] } },
            { "switch",                        new Command { uniqueid = 23003, category = CommandCategories.special, eventnumber = 4000, dcsid = "wMsgSwitchListening",    displayname = Labels.aicommands["switch"] } },
            { "wMsgShowKneeboardTab",          new Command { uniqueid = 23004, category = CommandCategories.special, eventnumber = 4000, dcsid = "wMsgShowKneeboardTab",   displayname = Labels.aicommands["wMsgShowKneeboardTab"],blockedforFree = true } },
            //{ "wMsgClearKneeboardTab",         new Command { uniqueid = 23005, category = CommandCategories.special, eventnumber = 4000, name = "wMsgClearKneeboardTab",  displayname = Labels.aicommands["wMsgClearKneeboardTab"],blockedforFree = true } },
            { "repeat",                        new Command { uniqueid = 23006, category = CommandCategories.special, eventnumber = 4000, dcsid = "wMsgReplySayAgain",displayname = Labels.aicommands["repeat"], blockedforFCnonPro = true} }, //
            { "wMsgSpecialCmndsMaximum",       new Command { uniqueid = 23099, category = CommandCategories.special, eventnumber = 4000, dcsid = "wMsgSpecialCmndsMaximum" } },

            // AOCS commands 23100
            { "wMsgAOCSCmndsNull" ,            new Command { uniqueid = 23100, category = CommandCategories.special, eventnumber = 4000, dcsid = "wMsgAOCSCmndsNull",   } },
            { "state" ,                        new Command { uniqueid = 23101, category = CommandCategories.special, eventnumber = 4000, dcsid = "wMsgProvideState",       displayname = Labels.aicommands["state"] } },
            { "readbriefing",                  new Command { uniqueid = 23102, category = CommandCategories.aicommsaocs, eventnumber = 4000, dcsid = "wMsgReadBriefing",       displayname = Labels.aicommands["readbriefing"] } },
            { "wMsgAOCSCmndsMaximum" ,         new Command { uniqueid = 23198, category = CommandCategories.special, eventnumber = 4000, dcsid = "wMsgSAOCSCmndsMaximum" } },

            // RIO commands 23200-23799
            { "wMsgRIOCmndsNull" ,             new Command { uniqueid = 23199, category = CommandCategories.RIO,    eventnumber = 4000,  dcsid = "wMsgRIOCmndsNull",   } },
            // extension pack 
            { "wMsgRIOCmndsMaximum" ,          new Command { uniqueid = 23799, category = CommandCategories.RIO,    eventnumber = 4000,  dcsid = "wMsgRIOCmndsMaximum" } },
            { "wMsgAIPilotCmndsNull" ,         new Command { uniqueid = 23799, category = CommandCategories.AI_pilot,eventnumber = 4000,  dcsid = "wMsgAIPilotCmndsNull",   } },
            // extension pack 
            { "wMsgAIPilotCmndsMaximum" ,      new Command { uniqueid = 23999, category = CommandCategories.AI_pilot, eventnumber = 4000,dcsid = "wMsgAIPilotCmndsMaximum" } },


            // Kneeboard 24000
            { "wMsgKneeboardCmndsNull" ,             new Command { uniqueid = 24000, category = CommandCategories.kneeboard,    eventnumber = 4000,  dcsid = "wMsgKneeboardCmndsNull" } },
            { "wMsgKneeboardDictateStart" ,          new Command { uniqueid = 24001, category = CommandCategories.kneeboard,    eventnumber = 4000,  dcsid = "wMsgKneeboardDictateStart",blockedforFree = true   } },
            { "wMsgKneeboardDictateStop" ,           new Command { uniqueid = 24002, category = CommandCategories.kneeboard,    eventnumber = 4000,  dcsid = "wMsgKneeboardDictateStop",blockedforFree = true   } },
            { "wMsgKneeboardCorrection" ,            new Command { uniqueid = 24003, category = CommandCategories.kneeboard,    eventnumber = 4000,  dcsid = "wMsgKneeboardCorrection",blockedforFree = true   } },
            { "wMsgKneeboardClearNotes" ,            new Command { uniqueid = 24004, category = CommandCategories.kneeboard,    eventnumber = 4000,  dcsid = "wMsgKneeboardClearNotes",blockedforFree = true   } },
            { "wMsgKneeboardShowNotes" ,             new Command { uniqueid = 24005, category = CommandCategories.kneeboard,    eventnumber = 4000,  dcsid = "wMsgKneeboardShowNotes",blockedforFree = true   } },
            { "wMsgKneeboardShowLog" ,               new Command { uniqueid = 24006, category = CommandCategories.kneeboard,    eventnumber = 4000,  dcsid = "wMsgKneeboardShowLog",blockedforFree = true   } },
            { "wMsgKneeboardCmndsMaximum" ,          new Command { uniqueid = 24100, category = CommandCategories.kneeboard,    eventnumber = 4000,  dcsid = "wMsgKneeboardCmndsMaximum" } },


            // Moose Ops 
            { "wMsgLeaderToMooseCmndsNull" ,       new Command { uniqueid = 25000, category = CommandCategories.Moose,  eventnumber = 4000, dcsid = "wMsgLeaderToMooseCmndsNull" } },
            //{ "Radio Check Marshal" ,              new Command { uniqueid = 25001, category = CommandCategories.Moose,  eventnumber = 4000, dcsid = "wMsgLeaderToMooseRadioChkMarshal",displayname = Labels.aicommands["Radio Check Marshal"],blockedforFree = true   } },
            //{ "Radio Check LSO" ,                  new Command { uniqueid = 25002, category = CommandCategories.Moose,  eventnumber = 4000, dcsid = "wMsgLeaderToMooseRadioChkLSO",displayname = Labels.aicommands["Radio Check LSO"],blockedforFree = true   } },
            //{ "Request Commence" ,                 new Command { uniqueid = 25003, category = CommandCategories.Moose,  eventnumber = 4000, dcsid = "wMsgLeaderToMooseRqstCommence",displayname = Labels.aicommands["Request Commence"],blockedforFree = true   } },
            //{ "Emergency Landing" ,                new Command { uniqueid = 25003, category = CommandCategories.Moose,  eventnumber = 4000, dcsid = "wMsgLeaderToMooseEmerLanding",displayname = Labels.aicommands["Emergency Landing"],blockedforFree = true   } },
            { "wMsgLeaderToMooseCmndsMaximum" ,    new Command { uniqueid = 25100, category = CommandCategories.Moose,  eventnumber = 4000, dcsid = "wMsgLeaderToMooseCmndsMaximum" } },
            
            // ----------------------------------------------------------------------

            };

        }
    }
}
