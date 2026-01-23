using System;
using System.Collections.Generic;

namespace VAICOM.Extensions.WSO
{
    public static class WSOCommands
    {
        // WSO commands: 24000 - 24999 (reserved range for WSO commands)
        public static Dictionary<string, CommandInfo> all = new Dictionary<string, CommandInfo>(StringComparer.OrdinalIgnoreCase)
        {
            { "wMsgWSO_ReportSpeed", new CommandInfo { uniqueid = 24000, name = "wMsgWSO_ReportSpeed", displayname = "WSO: Report Speed", enabled = true } },
            { "wMsgWSO_ToggleRadar", new CommandInfo { uniqueid = 24001, name = "wMsgWSO_ToggleRadar", displayname = "WSO: Toggle Radar", enabled = true } },
            { "wMsgWSO_SetChaffMode", new CommandInfo { uniqueid = 24002, name = "wMsgWSO_SetChaffMode", displayname = "WSO: Set Chaff Mode", enabled = true } },
            { "wMsgWSO_LantirnDesignate", new CommandInfo { uniqueid = 24003, name = "wMsgWSO_LantirnDesignate", displayname = "WSO: LANTIRN Designate", enabled = true } },
            { "wMsgWSO_LantirnUndesignate", new CommandInfo { uniqueid = 24004, name = "wMsgWSO_LantirnUndesignate", displayname = "WSO: LANTIRN Undesignate", enabled = true } },
            { "wMsgWSO_RadarGoSilent", new CommandInfo { uniqueid = 24005, name = "wMsgWSO_RadarGoSilent", displayname = "WSO: Radar Go Silent", enabled = true } },
            { "wMsgWSO_RadarGoActive", new CommandInfo { uniqueid = 24006, name = "wMsgWSO_RadarGoActive", displayname = "WSO: Radar Go Active", enabled = true } },
            { "wMsgWSO_SetManualFrequency", new CommandInfo { uniqueid = 24007, name = "wMsgWSO_SetManualFrequency", displayname = "WSO: Set Manual Frequency", enabled = true } },
            { "wMsgWSO_SelectCommChannel", new CommandInfo { uniqueid = 24008, name = "wMsgWSO_SelectCommChannel", displayname = "WSO: Select Comm Channel", enabled = true } },
            { "wMsgWSO_SelectAuxChannel", new CommandInfo { uniqueid = 24009, name = "wMsgWSO_SelectAuxChannel", displayname = "WSO: Select Aux Channel", enabled = true } },
            { "wMsgWSO_TuneATC", new CommandInfo { uniqueid = 24010, name = "wMsgWSO_TuneATC", displayname = "WSO: Tune ATC", enabled = true } },
            { "wMsgWSO_TuneAssets", new CommandInfo { uniqueid = 24011, name = "wMsgWSO_TuneAssets", displayname = "WSO: Tune Assets", enabled = true } },
            { "wMsgWSO_SelectMode", new CommandInfo { uniqueid = 24012, name = "wMsgWSO_SelectMode", displayname = "WSO: Select Mode", enabled = true } },
            { "wMsgWSO_RadarOperationActive", new CommandInfo { uniqueid = 24013, name = "wMsgWSO_RadarOperationActive", displayname = "WSO: Radar Operation Active", enabled = true } },
            { "wMsgWSO_RadarOperationStandby", new CommandInfo { uniqueid = 24014, name = "wMsgWSO_RadarOperationStandby", displayname = "WSO: Radar Operation Standby", enabled = true } },
            { "wMsgWSO_RadarAutoFocusOn", new CommandInfo { uniqueid = 24015, name = "wMsgWSO_RadarAutoFocusOn", displayname = "WSO: Radar Auto Focus On", enabled = true } },
            { "wMsgWSO_RadarAutoFocusOff", new CommandInfo { uniqueid = 24016, name = "wMsgWSO_RadarAutoFocusOff", displayname = "WSO: Radar Auto Focus Off", enabled = true } },
            { "wMsgWSO_RadarIFFBoth", new CommandInfo { uniqueid = 24017, name = "wMsgWSO_RadarIFFBoth", displayname = "WSO: Radar IFF Both", enabled = true } },
            { "wMsgWSO_RadarIFFAPX76", new CommandInfo { uniqueid = 24018, name = "wMsgWSO_RadarIFFAPX76", displayname = "WSO: Radar IFF APX-76", enabled = true } },
            { "wMsgWSO_RadarEnterBoresight", new CommandInfo { uniqueid = 24019, name = "wMsgWSO_RadarEnterBoresight", displayname = "WSO: Radar Enter Boresight", enabled = true } },
            { "wMsgWSO_RadarBoresightWide", new CommandInfo { uniqueid = 24020, name = "wMsgWSO_RadarBoresightWide", displayname = "WSO: Radar Boresight Wide", enabled = true } },
            { "wMsgWSO_RadarBoresightNose", new CommandInfo { uniqueid = 24021, name = "wMsgWSO_RadarBoresightNose", displayname = "WSO: Radar Boresight Nose", enabled = true } },
            { "wMsgWSO_RadarBoresightForward", new CommandInfo { uniqueid = 24022, name = "wMsgWSO_RadarBoresightForward", displayname = "WSO: Radar Boresight Forward", enabled = true } },
            { "wMsgWSO_RadarBoresightAft", new CommandInfo { uniqueid = 24023, name = "wMsgWSO_RadarBoresightAft", displayname = "WSO: Radar Boresight Aft", enabled = true } },
            { "wMsgWSO_RadarBoresightTail", new CommandInfo { uniqueid = 24024, name = "wMsgWSO_RadarBoresightTail", displayname = "WSO: Radar Boresight Tail", enabled = true } },
            { "wMsgWSO_RadarScanElevationCenter", new CommandInfo { uniqueid = 24025, name = "wMsgWSO_RadarScanElevationCenter", displayname = "WSO: Radar Scan Elevation Center", enabled = true } },
            { "wMsgWSO_RadarScanElevationLow", new CommandInfo { uniqueid = 24026, name = "wMsgWSO_RadarScanElevationLow", displayname = "WSO: Radar Scan Elevation Low", enabled = true } },
            { "wMsgWSO_RadarScanElevationMidLow", new CommandInfo { uniqueid = 24027, name = "wMsgWSO_RadarScanElevationMidLow", displayname = "WSO: Radar Scan Elevation Mid Low", enabled = true } },
            { "wMsgWSO_RadarScanElevationFarLow", new CommandInfo { uniqueid = 24028, name = "wMsgWSO_RadarScanElevationFarLow", displayname = "WSO: Radar Scan Elevation Far Low", enabled = true } },
            { "wMsgWSO_RadarScanElevationCloseHigh", new CommandInfo { uniqueid = 24029, name = "wMsgWSO_RadarScanElevationCloseHigh", displayname = "WSO: Radar Scan Elevation Close High", enabled = true } },
            { "wMsgWSO_RadarScanElevationMidHigh", new CommandInfo { uniqueid = 24030, name = "wMsgWSO_RadarScanElevationMidHigh", displayname = "WSO: Radar Scan Elevation Mid High", enabled = true } },
            { "wMsgWSO_RadarScanElevationFarHigh", new CommandInfo { uniqueid = 24031, name = "wMsgWSO_RadarScanElevationFarHigh", displayname = "WSO: Radar Scan Elevation Far High", enabled = true } },
            { "wMsgWSO_RadarScanType25Wide", new CommandInfo { uniqueid = 24032, name = "wMsgWSO_RadarScanType25Wide", displayname = "WSO: Radar Scan Type 25 Wide", enabled = true } },
            { "wMsgWSO_RadarScanType25Narrow", new CommandInfo { uniqueid = 24033, name = "wMsgWSO_RadarScanType25Narrow", displayname = "WSO: Radar Scan Type 25 Narrow", enabled = true } },
            { "wMsgWSO_RadarScanType50Wide", new CommandInfo { uniqueid = 24034, name = "wMsgWSO_RadarScanType50Wide", displayname = "WSO: Radar Scan Type 50 Wide", enabled = true } },
            { "wMsgWSO_RadarScanType50Narrow", new CommandInfo { uniqueid = 24035, name = "wMsgWSO_RadarScanType50Narrow", displayname = "WSO: Radar Scan Type 50 Narrow", enabled = true } },
            { "wMsgWSO_RadarFocusTarget", new CommandInfo { uniqueid = 24036, name = "wMsgWSO_RadarFocusTarget", displayname = "WSO: Radar Focus Target", enabled = true } },
            { "wMsgWSO_RadarLockTarget", new CommandInfo { uniqueid = 24037, name = "wMsgWSO_RadarLockTarget", displayname = "WSO: Radar Lock Target", enabled = true } },
            { "wMsgWSO_A2GTVWeapons", new CommandInfo { uniqueid = 24038, name = "wMsgWSO_A2GTVWeapons", displayname = "WSO: A2G TV Weapons", enabled = true } },
            { "wMsgWSO_A2GTVPaveSpike", new CommandInfo { uniqueid = 24039, name = "wMsgWSO_A2GTVPaveSpike", displayname = "WSO: A2G TV Pave Spike", enabled = true } },
            { "wMsgWSO_NavigationGoToResume", new CommandInfo { uniqueid = 24040, name = "wMsgWSO_NavigationGoToResume", displayname = "WSO: Navigation Go To Resume", enabled = true } },
            { "wMsgWSO_NavigationHoldingCurrentTurnPoint", new CommandInfo { uniqueid = 24041, name = "wMsgWSO_NavigationHoldingCurrentTurnPoint", displayname = "WSO: Navigation Holding Current Turn Point", enabled = true } },
            { "wMsgWSO_NavigationDivertLatLong", new CommandInfo { uniqueid = 24042, name = "wMsgWSO_NavigationDivertLatLong", displayname = "WSO: Navigation Divert Lat Long", enabled = true } },
            { "wMsgWSO_NavigationTACANSelectModeOff", new CommandInfo { uniqueid = 24043, name = "wMsgWSO_NavigationTACANSelectModeOff", displayname = "WSO: Navigation TACAN Select Mode Off", enabled = true } },
            { "wMsgWSO_NavigationTACANSelectModeR", new CommandInfo { uniqueid = 24044, name = "wMsgWSO_NavigationTACANSelectModeR", displayname = "WSO: Navigation TACAN Select Mode R", enabled = true } },
            { "wMsgWSO_NavigationTACANSelectModeTR", new CommandInfo { uniqueid = 24045, name = "wMsgWSO_NavigationTACANSelectModeTR", displayname = "WSO: Navigation TACAN Select Mode TR", enabled = true } },
            { "wMsgWSO_NavigationTACANSelectModeAAR", new CommandInfo { uniqueid = 24046, name = "wMsgWSO_NavigationTACANSelectModeAAR", displayname = "WSO: Navigation TACAN Select Mode AAR", enabled = true } },
            { "wMsgWSO_NavigationTACANSelectModeAATR", new CommandInfo { uniqueid = 24047, name = "wMsgWSO_NavigationTACANSelectModeAATR", displayname = "WSO: Navigation TACAN Select Mode AATR", enabled = true } },
            { "wMsgWSO_SystemsChaffModeOff", new CommandInfo { uniqueid = 24048, name = "wMsgWSO_SystemsChaffModeOff", displayname = "WSO: Systems Chaff Mode Off", enabled = true } },
            { "wMsgWSO_SystemsChaffModeSingle", new CommandInfo { uniqueid = 24049, name = "wMsgWSO_SystemsChaffModeSingle", displayname = "WSO: Systems Chaff Mode Single", enabled = true } },
            { "wMsgWSO_SystemsChaffModeMultiple", new CommandInfo { uniqueid = 24050, name = "wMsgWSO_SystemsChaffModeMultiple", displayname = "WSO: Systems Chaff Mode Multiple", enabled = true } },
            { "wMsgWSO_SystemsChaffModeProgram", new CommandInfo { uniqueid = 24051, name = "wMsgWSO_SystemsChaffModeProgram", displayname = "WSO: Systems Chaff Mode Program", enabled = true } },
            { "wMsgWSO_SystemsFlareModeOff", new CommandInfo { uniqueid = 24052, name = "wMsgWSO_SystemsFlareModeOff", displayname = "WSO: Systems Flare Mode Off", enabled = true } },
            { "wMsgWSO_SystemsFlareModeSingle", new CommandInfo { uniqueid = 24053, name = "wMsgWSO_SystemsFlareModeSingle", displayname = "WSO: Systems Flare Mode Single", enabled = true } },
            { "wMsgWSO_SystemsFlareModeProgram", new CommandInfo { uniqueid = 24054, name = "wMsgWSO_SystemsFlareModeProgram", displayname = "WSO: Systems Flare Mode Program", enabled = true } },
            { "wMsgWSO_SystemsJammerStandby", new CommandInfo { uniqueid = 24055, name = "wMsgWSO_SystemsJammerStandby", displayname = "WSO: Systems Jammer Standby", enabled = true } },
            { "wMsgWSO_SystemsJammerTransmit", new CommandInfo { uniqueid = 24056, name = "wMsgWSO_SystemsJammerTransmit", displayname = "WSO: Systems Jammer Transmit", enabled = true } },
            { "wMsgWSO_SystemsAVTRRecord", new CommandInfo { uniqueid = 24057, name = "wMsgWSO_SystemsAVTRRecord", displayname = "WSO: Systems AVTR Record", enabled = true } },
            { "wMsgWSO_SystemsAVTRStandby", new CommandInfo { uniqueid = 24058, name = "wMsgWSO_SystemsAVTRStandby", displayname = "WSO: Systems AVTR Standby", enabled = true } },
            { "wMsgWSO_SystemsAVTROff", new CommandInfo { uniqueid = 24059, name = "wMsgWSO_SystemsAVTROff", displayname = "WSO: Systems AVTR Off", enabled = true } },
            { "wMsgWSO_CrewPresenceAuto", new CommandInfo { uniqueid = 24060, name = "wMsgWSO_CrewPresenceAuto", displayname = "WSO: Crew Presence Auto", enabled = true } },
            { "wMsgWSO_CrewPresenceForce", new CommandInfo { uniqueid = 24061, name = "wMsgWSO_CrewPresenceForce", displayname = "WSO: Crew Presence Force", enabled = true } },
            { "wMsgWSO_CrewPresenceDisable", new CommandInfo { uniqueid = 24062, name = "wMsgWSO_CrewPresenceDisable", displayname = "WSO: Crew Presence Disable", enabled = true } },
            { "wMsgWSO_CrewTalkingTalk", new CommandInfo { uniqueid = 24063, name = "wMsgWSO_CrewTalkingTalk", displayname = "WSO: Crew Talking Talk", enabled = true } },
            { "wMsgWSO_CrewTalkingSilence", new CommandInfo { uniqueid = 24064, name = "wMsgWSO_CrewTalkingSilence", displayname = "WSO: Crew Talking Silence", enabled = true } },
            { "wMsgWSO_CrewEjectionWSO", new CommandInfo { uniqueid = 24065, name = "wMsgWSO_CrewEjectionWSO", displayname = "WSO: Crew Ejection WSO", enabled = true } },
            { "wMsgWSO_CrewEjectionBoth", new CommandInfo { uniqueid = 24066, name = "wMsgWSO_CrewEjectionBoth", displayname = "WSO: Crew Ejection Both", enabled = true } },
            { "wMsgWSO_CrewCountermeasuresManual", new CommandInfo { uniqueid = 24067, name = "wMsgWSO_CrewCountermeasuresManual", displayname = "WSO: Crew Countermeasures Manual", enabled = true } },
            { "wMsgWSO_CrewCountermeasuresJester", new CommandInfo { uniqueid = 24068, name = "wMsgWSO_CrewCountermeasuresJester", displayname = "WSO: Crew Countermeasures Jester", enabled = true } },
        };
    }

    public class CommandInfo
    {
        public int uniqueid { get; set; }
        public string name { get; set; }
        public string displayname { get; set; }
        public bool enabled { get; set; }
    }
}