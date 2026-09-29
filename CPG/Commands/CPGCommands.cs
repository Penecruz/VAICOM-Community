using System;
using System.Collections.Generic;
using VAICOM.Shared;

namespace VAICOM.Extensions.CPG
{
    public static class Commands
    {
        public static Dictionary<string, CommandInfo> all = new Dictionary<string, CommandInfo>(StringComparer.OrdinalIgnoreCase)
        {
            /// <summary>
            /// GeorgeAI commands: 25200 - 25999 (800 slots reserved for George commands)
            /// 
            /// Breakdown of ranges:
            /// 
            /// CommandCategories.AH64D_George:                 25201 - 25217
            /// CommandCategories.AH64D_George_roe:             25218 - 25219
            /// CommandCategories.AH64D_George_CPG:             25220 - 25499
            /// CommandCategories.AH64D_George_CPG_startup:     25220 - 25229
            /// CommandCategories.AH64D_George_CPG_search:      25300 - 25349
            /// CommandCategories.AH64D_George_CPG_sensors:     25350 - 25399
            /// CommandCategories.AH64D_George_CPG_targets:     25400 - 25449
            /// CommandCategories.AH64D_George_CPG_weapons:     25450 - 25499
            /// CommandCategories.AH64D_George_PLT_ground:      25500 - 25549
            /// CommandCategories.AH64D_George_PLT_flight:      25550 - 25599
            /// CommandCategories.AH64D_George_PLT_combat:      25600 - 25649
            /// CommandCategories.AH64D_George_PLT_defensive:   25650 - 25699
            /// CommandCategories.AH64D_George_PLT_hover:       25700 - 25749
            /// CommandCategories.AH64D_George_PLT:             25750 - 25999
            /// 
            /// </summary>

            // George CPG/PLT shared commands: 25201-25220
            { "georgeshowhide",               new CommandInfo { uniqueid = 25201, category = CommandCategories.AH64D_George, eventnumber = 4000, name = "wMsgGeorgeShowHide", displayname = Labels.aicommands["georgeshowhide"], enabled = true } },
            { "georgeup",                     new CommandInfo { uniqueid = 25202, category = CommandCategories.AH64D_George, eventnumber = 4000, name = "wMsgGeorgeUp", displayname = Labels.aicommands["georgeup"], enabled = true } },
            { "georgedown",                   new CommandInfo { uniqueid = 25203, category = CommandCategories.AH64D_George, eventnumber = 4000, name = "wMsgGeorgeDown", displayname = Labels.aicommands["georgedown"], enabled = true } },
            { "georgeleft",                   new CommandInfo { uniqueid = 25204, category = CommandCategories.AH64D_George, eventnumber = 4000, name = "wMsgGeorgeLeft", displayname = Labels.aicommands["georgeleft"], enabled = true } },
            { "georgeright",                  new CommandInfo { uniqueid = 25205, category = CommandCategories.AH64D_George, eventnumber = 4000, name = "wMsgGeorgeRight", displayname = Labels.aicommands["georgeright"], enabled = true } },
            { "georgecenter",                 new CommandInfo { uniqueid = 25206, category = CommandCategories.AH64D_George, eventnumber = 4000, name = "wMsgGeorgeCenter", displayname = Labels.aicommands["georgecenter"], enabled = true } },
            { "georgecontrolrequest",         new CommandInfo { uniqueid = 25207, category = CommandCategories.AH64D_George, eventnumber = 4000, name = "wMsgGeorgeControlRequest", displayname = Labels.aicommands["georgecontrolrequest"], enabled = true } },
            { "georgeuplong",                 new CommandInfo { uniqueid = 25208, category = CommandCategories.AH64D_George, eventnumber = 4000, name = "wMsgGeorgeUpLong", displayname = Labels.aicommands["georgeuplong"], enabled = true } },
            { "georgedownlong",               new CommandInfo { uniqueid = 25209, category = CommandCategories.AH64D_George, eventnumber = 4000, name = "wMsgGeorgeDownLong", displayname = Labels.aicommands["georgedownlong"], enabled = true } },
            { "georgeleftlong",               new CommandInfo { uniqueid = 25210, category = CommandCategories.AH64D_George, eventnumber = 4000, name = "wMsgGeorgeLeftLong", displayname = Labels.aicommands["georgeleftlong"], enabled = true } },
            { "georgerightlong",              new CommandInfo { uniqueid = 25211, category = CommandCategories.AH64D_George, eventnumber = 4000, name = "wMsgGeorgeRightLong", displayname = Labels.aicommands["georgerightlong"], enabled = true } },
            { "georgecenterlong",             new CommandInfo { uniqueid = 25212, category = CommandCategories.AH64D_George, eventnumber = 4000, name = "wMsgGeorgeCenterLong", displayname = Labels.aicommands["georgecenterlong"], enabled = true } },
            
            // Rules of engagement
            { "georgeweaponsfree",            new CommandInfo { uniqueid = 25218, category = CommandCategories.AH64D_George, eventnumber = 4000, name = "wMsgGeorgeWeaponsFree", displayname = Labels.aicommands["georgeweaponsfree"], enabled = true } },
            { "georgeholdfire",               new CommandInfo { uniqueid = 25219, category = CommandCategories.AH64D_George, eventnumber = 4000, name = "wMsgGeorgeHoldFire", displayname = Labels.aicommands["georgeholdfire"], enabled = true } },
            
            // George CPG commands - 25220-25499
            // Startup and shutdown
            { "georgestartup",                new CommandInfo { uniqueid = 25220, category = CommandCategories.AH64D_George_CPG, eventnumber = 4000, name = "wMsgGeorgeStartUp", displayname = Labels.aicommands["georgestartup"], enabled = true } },
            { "georgeshutdown",               new CommandInfo { uniqueid = 25221, category = CommandCategories.AH64D_George_CPG, eventnumber = 4000, name = "wMsgGeorgeShutdown", displayname = Labels.aicommands["georgeshutdown"], enabled = true } },
            
            // Search
            { "georgeareasearch",             new CommandInfo { uniqueid = 25300, category = CommandCategories.AH64D_George_CPG, eventnumber = 4000, name = "wMsgGeorgeAreaSearch", displayname = Labels.aicommands["georgeareasearch"], enabled = true } },
            { "georgepointsearch",            new CommandInfo { uniqueid = 25301, category = CommandCategories.AH64D_George_CPG, eventnumber = 4000, name = "wMsgGeorgePointSearch", displayname = Labels.aicommands["georgepointsearch"], enabled = true } },
            { "georgelaststoredtarget",       new CommandInfo { uniqueid = 25302, category = CommandCategories.AH64D_George_CPG, eventnumber = 4000, name = "wMsgGeorgelastStoredTarget", displayname = Labels.aicommands["georgelaststoredtarget"], enabled = true } },
            { "georgeareaselect",             new CommandInfo { uniqueid = 25303, category = CommandCategories.AH64D_George_CPG, eventnumber = 4000, name = "wMsgGeorgeAreaSelect", displayname = Labels.aicommands["georgeareaselect"], enabled = true } },
            { "georgepointselect",            new CommandInfo { uniqueid = 25304, category = CommandCategories.AH64D_George_CPG, eventnumber = 4000, name = "wMsgGeorgePointSelect", displayname = Labels.aicommands["georgepointselect"], enabled = true } },
            { "georgemacrophssearch",         new CommandInfo { uniqueid = 25305, category = CommandCategories.AH64D_George_CPG, eventnumber = 4000, name = "wMsgGeorgeMacroPHSsearch", displayname = Labels.aicommands["georgemacrophssearch"], enabled = true } },
            { "georgemacronextsearch",        new CommandInfo { uniqueid = 25306, category = CommandCategories.AH64D_George_CPG, eventnumber = 4000, name = "wMsgGeorgeMacroNextSearch", displayname = Labels.aicommands["georgemacronextsearch"], enabled = true } },
            { "georgemacroprevioussearch",    new CommandInfo { uniqueid = 25307, category = CommandCategories.AH64D_George_CPG, eventnumber = 4000, name = "wMsgGeorgeMacroPreviousSearch", displayname = Labels.aicommands["georgemacroprevioussearch"], enabled = true } },
            { "georgemacronextpoint",         new CommandInfo { uniqueid = 25308, category = CommandCategories.AH64D_George_CPG, eventnumber = 4000, name = "wMsgGeorgeMacroNextPoint", displayname = Labels.aicommands["georgemacronextpoint"], enabled = true } },
            { "georgemacropreviouspoint",     new CommandInfo { uniqueid = 25309, category = CommandCategories.AH64D_George_CPG, eventnumber = 4000, name = "wMsgGeorgeMacroPreviousPoint", displayname = Labels.aicommands["georgemacropreviouspoint"], enabled = true } },
            { "georgemacrotadslos",           new CommandInfo { uniqueid = 25310, category = CommandCategories.AH64D_George_CPG, eventnumber = 4000, name = "wMsgGeorgeMacroTADSLOS", displayname = Labels.aicommands["georgemacrotadslos"], enabled = true } },
            { "georgepreviousitem",           new CommandInfo { uniqueid = 25311, category = CommandCategories.AH64D_George_CPG, eventnumber = 4000, name = "wMsgGeorgePreviousItem", displayname = Labels.aicommands["georgepreviousitem"], enabled = true } },
            { "georgenextitem",               new CommandInfo { uniqueid = 25312, category = CommandCategories.AH64D_George_CPG, eventnumber = 4000, name = "wMsgGeorgeNextItem", displayname = Labels.aicommands["georgenextitem"], enabled = true } },
            { "georgelistitemselect",         new CommandInfo { uniqueid = 25313, category = CommandCategories.AH64D_George_CPG, eventnumber = 4000, name = "wMsgGeorgeListItemSelect", displayname = Labels.aicommands["georgelistitemselect"], enabled = true } },
            { "georgetargetlistfilter",       new CommandInfo { uniqueid = 25314, category = CommandCategories.AH64D_George_CPG, eventnumber = 4000, name = "wMsgGeorgeTargetListFilter", displayname = Labels.aicommands["georgetargetlistfilter"], enabled = true } },
            { "georgepointlistfiltermode",    new CommandInfo { uniqueid = 25315, category = CommandCategories.AH64D_George_CPG, eventnumber = 4000, name = "wMsgGeorgePointListFilterMode", displayname = Labels.aicommands["georgepointlistfiltermode"], enabled = true } },
            { "georgepointlistfilterthreat",  new CommandInfo { uniqueid = 25316, category = CommandCategories.AH64D_George_CPG, eventnumber = 4000, name = "wMsgGeorgePointListFilterThreat", displayname = Labels.aicommands["georgepointlistfilterthreat"], enabled = true } },
            
            // Sensors
            { "georgetadssensor",             new CommandInfo { uniqueid = 25350, category = CommandCategories.AH64D_George_CPG, eventnumber = 4000, name = "wMsgGeorgeTadsSensor", displayname = Labels.aicommands["georgetadssensor"], enabled = true } },
            { "georgetadsfov",                new CommandInfo { uniqueid = 25351, category = CommandCategories.AH64D_George_CPG, eventnumber = 4000, name = "wMsgGeorgeTadsFov", displayname = Labels.aicommands["georgetadsfov"], enabled = true } },
            { "georgetadszoomin",             new CommandInfo { uniqueid = 25352, category = CommandCategories.AH64D_George_CPG, eventnumber = 4000, name = "wMsgGeorgeTadsZoomIn", displayname = Labels.aicommands["georgetadszoomin"], enabled = true } },
            { "georgetadszoomout",            new CommandInfo { uniqueid = 25353, category = CommandCategories.AH64D_George_CPG, eventnumber = 4000, name = "wMsgGeorgeTadsZoomOut", displayname = Labels.aicommands["georgetadszoomout"], enabled = true } },

            // Targets
            { "georgestoretarget",            new CommandInfo { uniqueid = 25400, category = CommandCategories.AH64D_George_CPG, eventnumber = 4000, name = "wMsgGeorgeStoreTarget", displayname = Labels.aicommands["georgestoretarget"], enabled = true } },
            { "georgelasetarget",             new CommandInfo { uniqueid = 25401, category = CommandCategories.AH64D_George_CPG, eventnumber = 4000, name = "wMsgGeorgeLaseTarget", displayname = Labels.aicommands["georgelasetarget"], enabled = true } },
            { "georgelaseron",                new CommandInfo { uniqueid = 25402, category = CommandCategories.AH64D_George_CPG, eventnumber = 4000, name = "wMsgGeorgeLaserOn", displayname = Labels.aicommands["georgelaseron"], enabled = true } },
            { "georgelaseroff",               new CommandInfo { uniqueid = 25403, category = CommandCategories.AH64D_George_CPG, eventnumber = 4000, name = "wMsgGeorgeLaserOff", displayname = Labels.aicommands["georgelaseroff"], enabled = true } },
            { "georgemacroaddtwotargetstrack",   new CommandInfo { uniqueid = 25404, category = CommandCategories.AH64D_George_CPG, eventnumber = 4000, name = "wMsgGeorgeMacroAddTwoTargetsTrack", displayname = Labels.aicommands["georgemacroaddtwotargetstrack"], enabled = true } },
            { "georgemacroaddthreetargetstrack", new CommandInfo { uniqueid = 25405, category = CommandCategories.AH64D_George_CPG, eventnumber = 4000, name = "wMsgGeorgeMacroAddThreeTargetsTrack", displayname = Labels.aicommands["georgemacroaddthreetargetstrack"], enabled = true } },
            { "georgemacroaddfourtargetstrack",  new CommandInfo { uniqueid = 25406, category = CommandCategories.AH64D_George_CPG, eventnumber = 4000, name = "wMsgGeorgeMacroAddFourTargetsTrack", displayname = Labels.aicommands["georgemacroaddfourtargetstrack"], enabled = true } },
            { "georgelastfoundtarget",        new CommandInfo { uniqueid = 25407, category = CommandCategories.AH64D_George_CPG, eventnumber = 4000, name = "wMsgGeorgeLastFoundTarget", displayname = Labels.aicommands["georgelastfoundtarget"], enabled = true } },
            { "georgemacrotrackengage",       new CommandInfo { uniqueid = 25408, category = CommandCategories.AH64D_George_CPG, eventnumber = 4000, name = "wMsgGeorgeMacroTrackEngage", displayname = Labels.aicommands["georgemacrotrackengage"], enabled = true } },
            { "georgeadjustaim",              new CommandInfo { uniqueid = 25409, category = CommandCategories.AH64D_George_CPG, eventnumber = 4000, name = "wMsgGeorgeAdjustAim", displayname = Labels.aicommands["georgeadjustaim"], enabled = true } },
            { "georgetargetlistzoomin",       new CommandInfo { uniqueid = 25410, category = CommandCategories.AH64D_George_CPG, eventnumber = 4000, name = "wMsgGeorgeTargetListZoomIn", displayname = Labels.aicommands["georgetargetlistzoomin"], enabled = true } },
            { "georgetargetlistzoomout",      new CommandInfo { uniqueid = 25411, category = CommandCategories.AH64D_George_CPG, eventnumber = 4000, name = "wMsgGeorgeTargetListZoomOut", displayname = Labels.aicommands["georgetargetlistzoomout"], enabled = true } },
            { "georgenexttarget",             new CommandInfo { uniqueid = 25412, category = CommandCategories.AH64D_George_CPG, eventnumber = 4000, name = "wMsgGeorgeNextTarget", displayname = Labels.aicommands["georgenexttarget"], enabled = true } },
            { "georgepreviuoustarget",        new CommandInfo { uniqueid = 25413, category = CommandCategories.AH64D_George_CPG, eventnumber = 4000, name = "wMsgGeorgePreviuousTarget", displayname = Labels.aicommands["georgepreviuoustarget"], enabled = true } },
            { "georgeexitlist",               new CommandInfo { uniqueid = 25414, category = CommandCategories.AH64D_George_CPG, eventnumber = 4000, name = "wMsgGeorgeExitList", displayname = Labels.aicommands["georgeexitlist"], enabled = true } },
            { "georgetracktarget",            new CommandInfo { uniqueid = 25415, category = CommandCategories.AH64D_George_CPG, eventnumber = 4000, name = "wMsgGeorgeTrackTarget", displayname = Labels.aicommands["georgetracktarget"], enabled = true } },
            { "georgeselecttarget",           new CommandInfo { uniqueid = 25416, category = CommandCategories.AH64D_George_CPG, eventnumber = 4000, name = "wMsgGeorgeSelectTarget", displayname = Labels.aicommands["georgeselecttarget"], enabled = true } },
            { "georgeclearedfire",            new CommandInfo { uniqueid = 25417, category = CommandCategories.AH64D_George_CPG, eventnumber = 4000, name = "wMsgGeorgeClearedFire", displayname = Labels.aicommands["georgeclearedfire"], enabled = true } },
            
            // Weapons
            { "georgeburstlimit",             new CommandInfo { uniqueid = 25450, category = CommandCategories.AH64D_George_CPG, eventnumber = 4000, name = "wMsgGeorgeBurstLimit", displayname = Labels.aicommands["georgeburstlimit"], enabled = true } },
            { "georgerocketquantity",         new CommandInfo { uniqueid = 25451, category = CommandCategories.AH64D_George_CPG, eventnumber = 4000, name = "wMsgGeorgeRocketQuantity", displayname = Labels.aicommands["georgerocketquantity"], enabled = true } },
            { "georgelobl",                   new CommandInfo { uniqueid = 25452, category = CommandCategories.AH64D_George_CPG, eventnumber = 4000, name = "wMsgGeorgeLOBL", displayname = Labels.aicommands["georgelobl"], enabled = true } },
            { "georgeloal",                   new CommandInfo { uniqueid = 25453, category = CommandCategories.AH64D_George_CPG, eventnumber = 4000, name = "wMsgGeorgeLOAL", displayname = Labels.aicommands["georgeloal"], enabled = true } },
            { "georgenextweapon",             new CommandInfo { uniqueid = 25454, category = CommandCategories.AH64D_George_CPG, eventnumber = 4000, name = "wMsgGeorgeNextWeapon", displayname = Labels.aicommands["georgenextweapon"], enabled = true } },
            { "georgemacroselectgun",         new CommandInfo { uniqueid = 25455, category = CommandCategories.AH64D_George_CPG, eventnumber = 4000, name = "wMsgGeorgeMacroSelectGun", displayname = Labels.aicommands["georgemacroselectgun"], enabled = true } },
            { "georgemacroselectmissiles",    new CommandInfo { uniqueid = 25456, category = CommandCategories.AH64D_George_CPG, eventnumber = 4000, name = "wMsgGeorgeMacroSelectMissiles", displayname = Labels.aicommands["georgemacroselectmissiles"], enabled = true } },
            { "georgemacroselectrockets",     new CommandInfo { uniqueid = 25457, category = CommandCategories.AH64D_George_CPG, eventnumber = 4000, name = "wMsgGeorgeMacroSelectRockets", displayname = Labels.aicommands["georgemacroselectrockets"], enabled = true } },
            { "georgemacroselectnoweapon",    new CommandInfo { uniqueid = 25458, category = CommandCategories.AH64D_George_CPG, eventnumber = 4000, name = "wMsgGeorgeMacroSelectNoWeapon", displayname = Labels.aicommands["georgemacroselectnoweapon"], enabled = true } },
            { "georgenextmsl",                new CommandInfo { uniqueid = 25459, category = CommandCategories.AH64D_George_CPG, eventnumber = 4000, name = "wMsgGeorgeNextMSL", displayname = Labels.aicommands["georgenextmsl"], enabled = true } },
            { "georgemsltraj",                new CommandInfo { uniqueid = 25460, category = CommandCategories.AH64D_George_CPG, eventnumber = 4000, name = "wMsgGeorgeMslTraj", displayname = Labels.aicommands["georgemsltraj"], enabled = true } },
            { "georgenextrkt",                new CommandInfo { uniqueid = 25461, category = CommandCategories.AH64D_George_CPG, eventnumber = 4000, name = "wMsgGeorgeNextRkt", displayname = Labels.aicommands["georgenextrkt"], enabled = true } },
        
            // George PLT commands
            // Ground (GND)
            { "georgeapustart",               new CommandInfo { uniqueid = 25500, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeAPUStart", displayname = Labels.aicommands["georgeapustart"], enabled = true } },
            { "georgeapustop",                new CommandInfo { uniqueid = 25501, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeAPUStop", displayname = Labels.aicommands["georgeapustop"], enabled = true } },
            { "georgeapuonly",                new CommandInfo { uniqueid = 25502, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeAPUOnly", displayname = Labels.aicommands["georgeapuonly"], enabled = true } },
            { "georgestartupenginesidle",     new CommandInfo { uniqueid = 25503, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeStartUpEnginesIdle", displayname = Labels.aicommands["georgestartupenginesidle"], enabled = true } },
            { "georgestartupenginesfly",      new CommandInfo { uniqueid = 25504, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeStartUpEnginesFly", displayname = Labels.aicommands["georgestartupenginesfly"], enabled = true } },
            { "georgestartupfull",            new CommandInfo { uniqueid = 25505, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeStartUpFull", displayname = Labels.aicommands["georgestartupfull"], enabled = true } },
            { "georgeshutdownfull",           new CommandInfo { uniqueid = 25506, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeShutdownFull", displayname = Labels.aicommands["georgeshutdownfull"], enabled = true } },
            { "georgeshutdownengines",        new CommandInfo { uniqueid = 25507, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeShutdownEngines", displayname = Labels.aicommands["georgeshutdownengines"], enabled = true } },
            { "georgecmwson",                 new CommandInfo { uniqueid = 25508, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeCMWSOn", displayname = Labels.aicommands["georgecmwson"], enabled = true } },
            { "georgecmwsoff",                new CommandInfo { uniqueid = 25509, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeCMWSOff", displayname = Labels.aicommands["georgecmwsoff"], enabled = true } },
            { "georgetakeoff",                new CommandInfo { uniqueid = 25510, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeTakeOff", displayname = Labels.aicommands["georgetakeoff"], enabled = true } },
            
            // Flight (FLT)
            { "georgespeedup",                new CommandInfo { uniqueid = 25550, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeSpeedUp", displayname = Labels.aicommands["georgespeedup"], enabled = true } },
            { "georgeslowdown",               new CommandInfo { uniqueid = 25551, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeSlowDown", displayname = Labels.aicommands["georgeslowdown"], enabled = true } },
            { "georgeincreasealtitude",       new CommandInfo { uniqueid = 25552, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeIncreaseAltitude", displayname = Labels.aicommands["georgeincreasealtitude"], enabled = true } },
            { "georgedecreasealtitude",       new CommandInfo { uniqueid = 25553, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeDecreaseAltitude", displayname = Labels.aicommands["georgedecreasealtitude"], enabled = true } },
            { "georgefollowwaypoints",        new CommandInfo { uniqueid = 25554, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeFollowWaypoints", displayname = Labels.aicommands["georgefollowwaypoints"], enabled = true } },
            { "georgecomeleft",               new CommandInfo { uniqueid = 25555, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeComeLeft", displayname = Labels.aicommands["georgecomeleft"], enabled = true } },
            { "georgecomeright",              new CommandInfo { uniqueid = 25556, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeComeRight", displayname = Labels.aicommands["georgecomeright"], enabled = true } },
            { "georgesetairspeedref",         new CommandInfo { uniqueid = 25557, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeSetAirSpeedRef", displayname = Labels.aicommands["georgesetairspeedref"], enabled = true } },
            { "georgesetgroundspeedref",      new CommandInfo { uniqueid = 25558, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeSetGroundSpeedRef", displayname = Labels.aicommands["georgesetgroundspeedref"], enabled = true } },
            { "georgesetradaraltitude",       new CommandInfo { uniqueid = 25559, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeSetRadarAltitude", displayname = Labels.aicommands["georgesetradaraltitude"], enabled = true } },
            { "georgesetbarometricaltitude",  new CommandInfo { uniqueid = 25560, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeSetBarometricAltitude", displayname = Labels.aicommands["georgesetbarometricaltitude"], enabled = true } },
            
            // Combat (CMBT)
            { "georgebreakleft",              new CommandInfo { uniqueid = 25600, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeBreakLeft", displayname = Labels.aicommands["georgebreakleft"], enabled = true } },
            { "georgebreakright",             new CommandInfo { uniqueid = 25601, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeBreakRight", displayname = Labels.aicommands["georgebreakright"], enabled = true } },
            { "georgeorbitoverhead",          new CommandInfo { uniqueid = 25602, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeOrbitOverhead", displayname = Labels.aicommands["georgeorbitoverhead"], enabled = true } },
            { "georgebreakoneeighty",         new CommandInfo { uniqueid = 25603, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeBreakOneEighty", displayname = Labels.aicommands["georgebreakoneeighty"], enabled = true } },
            { "georgeheadtolocation",         new CommandInfo { uniqueid = 25604, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeHeadToLocation", displayname = Labels.aicommands["georgeheadtolocation"], enabled = true } },
            { "georgealigntotads",            new CommandInfo { uniqueid = 25605, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeAlignToTADS", displayname = Labels.aicommands["georgealigntotads"], enabled = true } },
            { "georgealigntonts",             new CommandInfo { uniqueid = 25606, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeAlignToNTS", displayname = Labels.aicommands["georgealigntonts"], enabled = true } },
            { "georgeturntoghs",              new CommandInfo { uniqueid = 25607, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeTurnToGHS", displayname = Labels.aicommands["georgeturntoghs"], enabled = true } },
            { "georgeholdposition",           new CommandInfo { uniqueid = 25608, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeHoldPosition", displayname = Labels.aicommands["georgeholdposition"], enabled = true } },
            { "georgeaddbattleposition",      new CommandInfo { uniqueid = 25609, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeAddBattlePosition", displayname = Labels.aicommands["georgeaddbattleposition"], enabled = true } },
            { "georgedeletebattleposition",   new CommandInfo { uniqueid = 25610, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeDeleteBattlePosition", displayname = Labels.aicommands["georgedeletebattleposition"], enabled = true } },
            { "georgereturntobattleposition", new CommandInfo { uniqueid = 25611, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeReturnToBattlePosition", displayname = Labels.aicommands["georgereturntobattleposition"], enabled = true } },
            { "georgemaskposition",           new CommandInfo { uniqueid = 25612, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeMaskPosition", displayname = Labels.aicommands["georgemaskposition"], enabled = true } },
            
            // Defensive (DEFN)
            { "georgereturnfire",             new CommandInfo { uniqueid = 25650, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeReturnFire", displayname = Labels.aicommands["georgereturnfire"], enabled = true } },
            { "georgeevadeoff",               new CommandInfo { uniqueid = 25651, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeEvadeOff", displayname = Labels.aicommands["georgeevadeoff"], enabled = true } },
            { "georgeevadelevel",             new CommandInfo { uniqueid = 25652, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeEvadeLevel", displayname = Labels.aicommands["georgeevadelevel"], enabled = true } },
            { "georgeevadevertical",          new CommandInfo { uniqueid = 25653, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeEvadeVertical", displayname = Labels.aicommands["georgeevadevertical"], enabled = true } },
            { "georgeevademask",              new CommandInfo { uniqueid = 25654, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeEvadeMask", displayname = Labels.aicommands["georgeevademask"], enabled = true } },
            { "georgethreatwarningson",       new CommandInfo { uniqueid = 25655, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeThreatWarningsOn", displayname = Labels.aicommands["georgethreatwarningson"], enabled = true } },
            { "georgethreatwarningsoff",      new CommandInfo { uniqueid = 25656, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeThreatWarningsOff", displayname = Labels.aicommands["georgethreatwarningsoff"], enabled = true } },
            { "georgecmwsarm",                new CommandInfo { uniqueid = 25657, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeCMWSArm", displayname = Labels.aicommands["georgecmwsarm"], enabled = true } },
            { "georgecmwssafe",               new CommandInfo { uniqueid = 25658, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeCMWSSafe", displayname = Labels.aicommands["georgecmwssafe"], enabled = true } },
            { "georgecmwsauto",               new CommandInfo { uniqueid = 25659, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeCMWSAuto", displayname = Labels.aicommands["georgecmwsauto"], enabled = true } },
            { "georgecmwsbypass",             new CommandInfo { uniqueid = 25660, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeCMWSBypass", displayname = Labels.aicommands["georgecmwsbypass"], enabled = true } },
            { "georgecmdispensenone",         new CommandInfo { uniqueid = 25661, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeCMDispenseNone", displayname = Labels.aicommands["georgecmdispensenone"], enabled = true } },
            { "georgecmdispensechaff",        new CommandInfo { uniqueid = 25662, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeCMDispenseChaff", displayname = Labels.aicommands["georgecmdispensechaff"], enabled = true } },
            { "georgecmdispenseflares",       new CommandInfo { uniqueid = 25663, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeCMDispenseFlares", displayname = Labels.aicommands["georgecmdispenseflares"], enabled = true } },
            { "georgecmdispensechaffandflares", new CommandInfo { uniqueid = 25664, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeCMDispenseChaffAndFlares", displayname = Labels.aicommands["georgecmdispensechaffandflares"], enabled = true } },
            { "georgeextlightsoff",           new CommandInfo { uniqueid = 25665, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeExtLightsOff", displayname = Labels.aicommands["georgeextlightsoff"], enabled = true } },
            { "georgeextlightsday",           new CommandInfo { uniqueid = 25666, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeExtLightsDay", displayname = Labels.aicommands["georgeextlightsday"], enabled = true } },
            { "georgeextlightsnightbright",   new CommandInfo { uniqueid = 25667, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeExtLightsNightBright", displayname = Labels.aicommands["georgeextlightsnightbright"], enabled = true } },
            { "georgeextlightsnightdim",      new CommandInfo { uniqueid = 25668, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeExtLightsNightDim", displayname = Labels.aicommands["georgeextlightsnightdim"], enabled = true } },
            { "georgeextlightsformation",     new CommandInfo { uniqueid = 25669, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeExtLightsFormation", displayname = Labels.aicommands["georgeextlightsformation"], enabled = true } },
            
            // Hover-Bobup (H-B)
            { "georgehoverforward",           new CommandInfo { uniqueid = 25700, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeHoverForward", displayname = Labels.aicommands["georgehoverforward"], enabled = true } },
            { "georgehoverback",              new CommandInfo { uniqueid = 25701, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeHoverBack", displayname = Labels.aicommands["georgehoverback"], enabled = true } },
            { "georgehoverleft",              new CommandInfo { uniqueid = 25702, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeHoverLeft", displayname = Labels.aicommands["georgehoverleft"], enabled = true } },
            { "georgehoverright",             new CommandInfo { uniqueid = 25703, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeHoverRight", displayname = Labels.aicommands["georgehoverright"], enabled = true } },
            { "georgehoveruptenfeet",         new CommandInfo { uniqueid = 25704, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeHoverUpTenFeet", displayname = Labels.aicommands["georgehoveruptenfeet"], enabled = true } },
            { "georgehoverdowntenfeet",       new CommandInfo { uniqueid = 25705, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeHoverDownTenFeet", displayname = Labels.aicommands["georgehoverdowntenfeet"], enabled = true } },

            // Menu modes
            { "georgemenucombatmode",         new CommandInfo { uniqueid = 25750, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeMenuCombatMode", displayname = Labels.aicommands["georgemenucombatmode"], enabled = true } },
            { "georgemenudefensemode",        new CommandInfo { uniqueid = 25751, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeMenuDefenseMode", displayname = Labels.aicommands["georgemenudefensemode"], enabled = true } },
            { "georgemenuflightmode",         new CommandInfo { uniqueid = 25752, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeMenuFlightMode", displayname = Labels.aicommands["georgemenuflightmode"], enabled = true } },
            { "georgemenugroundmode",         new CommandInfo { uniqueid = 25753, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeMenuGroundMode", displayname = Labels.aicommands["georgemenugroundmode"], enabled = true } },
            { "georgemenuhovermode",          new CommandInfo { uniqueid = 25754, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeMenuHoverMode", displayname = Labels.aicommands["georgemenuhovermode"], enabled = true } },
            { "georgemenunextmode",           new CommandInfo { uniqueid = 25755, category = CommandCategories.AH64D_George_PLT, eventnumber = 4000, name = "wMsgGeorgeMenuNextMode", displayname = Labels.aicommands["georgemenunextmode"], enabled = true } },
        };
    }
}