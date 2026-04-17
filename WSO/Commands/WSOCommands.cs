using System;
using System.Collections.Generic;
using VAICOM.Shared;

namespace VAICOM.Extensions.WSO
{
    public static class Commands //Adds WSO commands to the command database
    {
        // WSO commands: 24000 - 24999 (1000 slots reserved for WSO commands)
        /// <summary>
        /// Breakdown of ranges:
        /// 
        /// CommandCategories.WSO_menu:      24000 - 24099
        /// CommandCategories.WSO_radar:     24100 - 24199
        /// CommandCategories.WSO_weapons:   24200 - 24299
        /// CommandCategories.WSO_radio:     24300 - 24399
        /// CommandCategories.WSO_utility:   24400 - 24499
        /// CommandCategories.WSO_defensive: 24500 - 24599
        /// CommandCategories.WSO_misc:      24600 - 24999
        /// 
        /// </summary>
        public static Dictionary<string, CommandInfo> all = new Dictionary<string, CommandInfo>(StringComparer.OrdinalIgnoreCase)
        {            
            { "wMsgWSO_ReportSpeed", new CommandInfo { uniqueid = 24600, category = CommandCategories.WSO, name = "wMsgWSO_ReportSpeed", displayname = Labels.aicommands["wMsgWSO_ReportSpeed"], enabled = true } },
            { "wMsgWSO_ToggleRadar", new CommandInfo { uniqueid = 24100, category = CommandCategories.WSO, name = "wMsgWSO_ToggleRadar", displayname = Labels.aicommands["wMsgWSO_ToggleRadar"], enabled = true } },
            { "wMsgWSO_SetChaffMode", new CommandInfo { uniqueid = 24500, category = CommandCategories.WSO, name = "wMsgWSO_SetChaffMode", displayname = Labels.aicommands["wMsgWSO_SetChaffMode"], enabled = true } },
            { "wMsgWSO_Lantirn_Designate", new CommandInfo { uniqueid = 24004, category = CommandCategories.WSO, name = "wMsgWSO_Lantirn_Designate", displayname = Labels.aicommands["wMsgWSO_Lantirn_Designate"], enabled = true } },
            { "wMsgWSO_Lantirn_Undesignate", new CommandInfo { uniqueid = 24005, category = CommandCategories.WSO, name = "wMsgWSO_Lantirn_Undesignate", displayname = Labels.aicommands["wMsgWSO_Lantirn_Undesignate"], enabled = true } },
            { "wMsgWSO_Radar_GoSilent", new CommandInfo { uniqueid = 24006, category = CommandCategories.WSO, name = "wMsgWSO_Radar_GoSilent", displayname = Labels.aicommands["wMsgWSO_Radar_GoSilent"], enabled = true } },
            { "wMsgWSO_Radar_GoActive", new CommandInfo { uniqueid = 24007, category = CommandCategories.WSO, name = "wMsgWSO_Radar_GoActive", displayname = Labels.aicommands["wMsgWSO_Radar_GoActive"], enabled = true } },
            { "wMsgWSO_Radio_SetManualFrequency", new CommandInfo { uniqueid = 24008, category = CommandCategories.WSO, name = "wMsgWSO_Radio_SetManualFrequency", displayname = Labels.aicommands["wMsgWSO_Radio_SetManualFrequency"], enabled = true } },
            { "wMsgWSO_Radio_SelectCommChannel", new CommandInfo { uniqueid = 24009, category = CommandCategories.WSO, name = "wMsgWSO_Radio_SelectCommChannel", displayname = Labels.aicommands["wMsgWSO_Radio_SelectCommChannel"], enabled = true } },
            { "wMsgWSO_Radio_SelectAuxChannel", new CommandInfo { uniqueid = 24010, category = CommandCategories.WSO, name = "wMsgWSO_Radio_SelectAuxChannel", displayname = Labels.aicommands["wMsgWSO_Radio_SelectAuxChannel"], enabled = true } },
            { "wMsgWSO_Radio_TuneATC", new CommandInfo { uniqueid = 24300, category = CommandCategories.WSO, name = "wMsgWSO_Radio_TuneATC", displayname = Labels.aicommands["wMsgWSO_Radio_TuneATC"], enabled = true } },
            { "wMsgWSO_Radio_TuneAssets", new CommandInfo { uniqueid = 24301, category = CommandCategories.WSO, name = "wMsgWSO_Radio_TuneAssets", displayname = Labels.aicommands["wMsgWSO_Radio_TuneAssets"], enabled = true } },
            { "wMsgWSO_Radio_SelectMode", new CommandInfo { uniqueid = 24302, category = CommandCategories.WSO, name = "wMsgWSO_Radio_SelectMode", displayname = Labels.aicommands["wMsgWSO_Radio_SelectMode"], enabled = true } },
            { "wMsgWSO_Radar_Operation_Active", new CommandInfo { uniqueid = 24014, category = CommandCategories.WSO, name = "wMsgWSO_Radar_Operation_Active", displayname = Labels.aicommands["wMsgWSO_Radar_Operation_Active"], enabled = true } },
            { "wMsgWSO_Radar_Operation_Standby", new CommandInfo { uniqueid = 24015, category = CommandCategories.WSO, name = "wMsgWSO_Radar_Operation_Standby", displayname = Labels.aicommands["wMsgWSO_Radar_Operation_Standby"], enabled = true } },
            { "wMsgWSO_Radar_AutoFocus_On", new CommandInfo { uniqueid = 24101, category = CommandCategories.WSO, name = "wMsgWSO_Radar_AutoFocus_On", displayname = Labels.aicommands["wMsgWSO_Radar_AutoFocus_On"], enabled = true } },
            { "wMsgWSO_Radar_AutoFocus_Off", new CommandInfo { uniqueid = 24102, category = CommandCategories.WSO, name = "wMsgWSO_Radar_AutoFocus_Off", displayname = Labels.aicommands["wMsgWSO_Radar_AutoFocus_Off"], enabled = true } },
            { "wMsgWSO_Radar_IFF_Both", new CommandInfo { uniqueid = 24103, category = CommandCategories.WSO, name = "wMsgWSO_Radar_IFF_Both", displayname = Labels.aicommands["wMsgWSO_Radar_IFF_Both"], enabled = true } },
            { "wMsgWSO_Radar_IFF_APX76", new CommandInfo { uniqueid = 24104, category = CommandCategories.WSO, name = "wMsgWSO_Radar_IFF_APX76", displayname = Labels.aicommands["wMsgWSO_Radar_IFF_APX76"], enabled = true } },
            { "wMsgWSO_Radar_Boresight_EnterBST", new CommandInfo { uniqueid = 24105, category = CommandCategories.WSO, name = "wMsgWSO_Radar_Boresight_EnterBST", displayname = Labels.aicommands["wMsgWSO_Radar_Boresight_EnterBST"], enabled = true } },
            { "wMsgWSO_Radar_Boresight_Wide", new CommandInfo { uniqueid = 24106, category = CommandCategories.WSO, name = "wMsgWSO_Radar_Boresight_Wide", displayname = Labels.aicommands["wMsgWSO_Radar_Boresight_Wide"], enabled = true } },
            { "wMsgWSO_Radar_Boresight_Nose", new CommandInfo { uniqueid = 24107, category = CommandCategories.WSO, name = "wMsgWSO_Radar_Boresight_Nose", displayname = Labels.aicommands["wMsgWSO_Radar_Boresight_Nose"], enabled = true } },
            { "wMsgWSO_Radar_Boresight_Forward", new CommandInfo { uniqueid = 24108, category = CommandCategories.WSO, name = "wMsgWSO_Radar_Boresight_Forward", displayname = Labels.aicommands["wMsgWSO_Radar_Boresight_Forward"], enabled = true } },
            { "wMsgWSO_Radar_Boresight_Aft", new CommandInfo { uniqueid = 24109, category = CommandCategories.WSO, name = "wMsgWSO_Radar_Boresight_Aft", displayname = Labels.aicommands["wMsgWSO_Radar_Boresight_Aft"], enabled = true } },
            { "wMsgWSO_Radar_Boresight_Tail", new CommandInfo { uniqueid = 24110, category = CommandCategories.WSO, name = "wMsgWSO_Radar_Boresight_Tail", displayname = Labels.aicommands["wMsgWSO_Radar_Boresight_Tail"], enabled = true } },
            { "wMsgWSO_Radar_ScanElevation_Center", new CommandInfo { uniqueid = 24111, category = CommandCategories.WSO, name = "wMsgWSO_Radar_ScanElevation_Center", displayname = Labels.aicommands["wMsgWSO_Radar_ScanElevation_Center"], enabled = true } },
            { "wMsgWSO_Radar_ScanElevation_Low", new CommandInfo { uniqueid = 24112, category = CommandCategories.WSO, name = "wMsgWSO_Radar_ScanElevation_Low", displayname = Labels.aicommands["wMsgWSO_Radar_ScanElevation_Low"], enabled = true } },
            { "wMsgWSO_Radar_ScanElevation_MidLow", new CommandInfo { uniqueid = 24113, category = CommandCategories.WSO, name = "wMsgWSO_Radar_ScanElevation_MidLow", displayname = Labels.aicommands["wMsgWSO_Radar_ScanElevation_MidLow"], enabled = true } },
            { "wMsgWSO_Radar_ScanElevation_FarLow", new CommandInfo { uniqueid = 24114, category = CommandCategories.WSO, name = "wMsgWSO_Radar_ScanElevation_FarLow", displayname = Labels.aicommands["wMsgWSO_Radar_ScanElevation_FarLow"], enabled = true } },
            { "wMsgWSO_Radar_ScanElevation_CloseHigh", new CommandInfo { uniqueid = 24115, category = CommandCategories.WSO, name = "wMsgWSO_Radar_ScanElevation_CloseHigh", displayname = Labels.aicommands["wMsgWSO_Radar_ScanElevation_CloseHigh"], enabled = true } },
            { "wMsgWSO_Radar_ScanElevation_MidHigh", new CommandInfo { uniqueid = 24116, category = CommandCategories.WSO, name = "wMsgWSO_Radar_ScanElevation_MidHigh", displayname = Labels.aicommands["wMsgWSO_Radar_ScanElevation_MidHigh"], enabled = true } },
            { "wMsgWSO_Radar_ScanElevation_FarHigh", new CommandInfo { uniqueid = 24117, category = CommandCategories.WSO, name = "wMsgWSO_Radar_ScanElevation_FarHigh", displayname = Labels.aicommands["wMsgWSO_Radar_ScanElevation_FarHigh"], enabled = true } },
            { "wMsgWSO_Radar_ScanType_25Wide", new CommandInfo { uniqueid = 24118, category = CommandCategories.WSO, name = "wMsgWSO_Radar_ScanType_25Wide", displayname = Labels.aicommands["wMsgWSO_Radar_ScanType_25Wide"], enabled = true } },
            { "wMsgWSO_Radar_ScanType_25Narrow", new CommandInfo { uniqueid = 24119, category = CommandCategories.WSO, name = "wMsgWSO_Radar_ScanType_25Narrow", displayname = Labels.aicommands["wMsgWSO_Radar_ScanType_25Narrow"], enabled = true } },
            { "wMsgWSO_Radar_ScanType_50Wide", new CommandInfo { uniqueid = 24120, category = CommandCategories.WSO, name = "wMsgWSO_Radar_ScanType_50Wide", displayname = Labels.aicommands["wMsgWSO_Radar_ScanType_50Wide"], enabled = true } },
            { "wMsgWSO_Radar_ScanType_50Narrow", new CommandInfo { uniqueid = 24121, category = CommandCategories.WSO, name = "wMsgWSO_Radar_ScanType_50Narrow", displayname = Labels.aicommands["wMsgWSO_Radar_ScanType_50Narrow"], enabled = true } },
            { "wMsgWSO_Radar_FocusTarget", new CommandInfo { uniqueid = 24122, category = CommandCategories.WSO, name = "wMsgWSO_Radar_FocusTarget", displayname = Labels.aicommands["wMsgWSO_Radar_FocusTarget"], enabled = true } },
            { "wMsgWSO_Radar_LockTarget", new CommandInfo { uniqueid = 24123, category = CommandCategories.WSO, name = "wMsgWSO_Radar_LockTarget", displayname = Labels.aicommands["wMsgWSO_Radar_LockTarget"], enabled = true } },
            { "wMsgWSO_A2G_TVWeapons", new CommandInfo { uniqueid = 24039, category = CommandCategories.WSO, name = "wMsgWSO_A2G_TVWeapons", displayname = Labels.aicommands["wMsgWSO_A2G_TVWeapons"], enabled = true } },
            { "wMsgWSO_A2G_TVPaveSpike", new CommandInfo { uniqueid = 24040, category = CommandCategories.WSO, name = "wMsgWSO_A2G_TVPaveSpike", displayname = Labels.aicommands["wMsgWSO_A2G_TVPaveSpike"], enabled = true } },
            { "wMsgWSO_Navigation_GoToResume", new CommandInfo { uniqueid = 24041, category = CommandCategories.WSO, name = "wMsgWSO_Navigation_GoToResume", displayname = Labels.aicommands["wMsgWSO_Navigation_GoToResume"], enabled = true } },
            { "wMsgWSO_Navigation_Holding_CurrentTurnPoint", new CommandInfo { uniqueid = 24042, category = CommandCategories.WSO, name = "wMsgWSO_Navigation_Holding_CurrentTurnPoint", displayname = Labels.aicommands["wMsgWSO_Navigation_Holding_CurrentTurnPoint"], enabled = true } },
            { "wMsgWSO_Navigation_Divert_LatLong", new CommandInfo { uniqueid = 24043, category = CommandCategories.WSO, name = "wMsgWSO_Navigation_Divert_LatLong", displayname = Labels.aicommands["wMsgWSO_Navigation_Divert_LatLong"], enabled = true } },
            { "wMsgWSO_Navigation_TACAN_SelectMode_Off", new CommandInfo { uniqueid = 24044, category = CommandCategories.WSO, name = "wMsgWSO_Navigation_TACAN_SelectMode_Off", displayname = Labels.aicommands["wMsgWSO_Navigation_TACAN_SelectMode_Off"], enabled = true } },
            { "wMsgWSO_Navigation_TACAN_SelectMode_R", new CommandInfo { uniqueid = 24045, category = CommandCategories.WSO, name = "wMsgWSO_Navigation_TACAN_SelectMode_R", displayname = Labels.aicommands["wMsgWSO_Navigation_TACAN_SelectMode_R"], enabled = true } },
            { "wMsgWSO_Navigation_TACAN_SelectMode_TR", new CommandInfo { uniqueid = 24046, category = CommandCategories.WSO, name = "wMsgWSO_Navigation_TACAN_SelectMode_TR", displayname = Labels.aicommands["wMsgWSO_Navigation_TACAN_SelectMode_TR"], enabled = true } },
            { "wMsgWSO_Navigation_TACAN_SelectMode_AAR", new CommandInfo { uniqueid = 24047, category = CommandCategories.WSO, name = "wMsgWSO_Navigation_TACAN_SelectMode_AAR", displayname = Labels.aicommands["wMsgWSO_Navigation_TACAN_SelectMode_AAR"], enabled = true } },
            { "wMsgWSO_Navigation_TACAN_SelectMode_AATR", new CommandInfo { uniqueid = 24048, category = CommandCategories.WSO, name = "wMsgWSO_Navigation_TACAN_SelectMode_AATR", displayname = Labels.aicommands["wMsgWSO_Navigation_TACAN_SelectMode_AATR"], enabled = true } },
            { "wMsgWSO_Systems_ChaffMode_Off", new CommandInfo { uniqueid = 24501, category = CommandCategories.WSO, name = "wMsgWSO_Systems_ChaffMode_Off", displayname = Labels.aicommands["wMsgWSO_Systems_ChaffMode_Off"], enabled = true } },
            { "wMsgWSO_Systems_ChaffMode_Single", new CommandInfo { uniqueid = 24502, category = CommandCategories.WSO, name = "wMsgWSO_Systems_ChaffMode_Single", displayname = Labels.aicommands["wMsgWSO_Systems_ChaffMode_Single"], enabled = true } },
            { "wMsgWSO_Systems_ChaffMode_Multiple", new CommandInfo { uniqueid = 24503, category = CommandCategories.WSO, name = "wMsgWSO_Systems_ChaffMode_Multiple", displayname = Labels.aicommands["wMsgWSO_Systems_ChaffMode_Multiple"], enabled = true } },
            { "wMsgWSO_Systems_ChaffMode_Program", new CommandInfo { uniqueid = 24504, category = CommandCategories.WSO, name = "wMsgWSO_Systems_ChaffMode_Program", displayname = Labels.aicommands["wMsgWSO_Systems_ChaffMode_Program"], enabled = true } },
            { "wMsgWSO_Systems_FlareMode_Off", new CommandInfo { uniqueid = 24505, category = CommandCategories.WSO, name = "wMsgWSO_Systems_FlareMode_Off", displayname = Labels.aicommands["wMsgWSO_Systems_FlareMode_Off"], enabled = true } },
            { "wMsgWSO_Systems_FlareMode_Single", new CommandInfo { uniqueid = 24506, category = CommandCategories.WSO, name = "wMsgWSO_Systems_FlareMode_Single", displayname = Labels.aicommands["wMsgWSO_Systems_FlareMode_Single"], enabled = true } },
            { "wMsgWSO_Systems_FlareMode_Program", new CommandInfo { uniqueid = 24507, category = CommandCategories.WSO, name = "wMsgWSO_Systems_FlareMode_Program", displayname = Labels.aicommands["wMsgWSO_Systems_FlareMode_Program"], enabled = true } },
            { "wMsgWSO_Systems_Jammer_Standby", new CommandInfo { uniqueid = 24508, category = CommandCategories.WSO, name = "wMsgWSO_Systems_Jammer_Standby", displayname = Labels.aicommands["wMsgWSO_Systems_Jammer_Standby"], enabled = true } },
            { "wMsgWSO_Systems_Jammer_Transmit", new CommandInfo { uniqueid = 24509, category = CommandCategories.WSO, name = "wMsgWSO_Systems_Jammer_Transmit", displayname = Labels.aicommands["wMsgWSO_Systems_Jammer_Transmit"], enabled = true } },
            { "wMsgWSO_Systems_AVTR_Record", new CommandInfo { uniqueid = 24601, category = CommandCategories.WSO, name = "wMsgWSO_Systems_AVTR_Record", displayname = Labels.aicommands["wMsgWSO_Systems_AVTR_Record"], enabled = true } },
            { "wMsgWSO_Systems_AVTR_Standby", new CommandInfo { uniqueid = 24602, category = CommandCategories.WSO, name = "wMsgWSO_Systems_AVTR_Standby", displayname = Labels.aicommands["wMsgWSO_Systems_AVTR_Standby"], enabled = true } },
            { "wMsgWSO_Systems_AVTR_Off", new CommandInfo { uniqueid = 24603, category = CommandCategories.WSO, name = "wMsgWSO_Systems_AVTR_Off", displayname = Labels.aicommands["wMsgWSO_Systems_AVTR_Off"], enabled = true } },
            { "wMsgWSO_Crew_Presence_Auto", new CommandInfo { uniqueid = 24061, category = CommandCategories.WSO, name = "wMsgWSO_Crew_Presence_Auto", displayname = Labels.aicommands["wMsgWSO_Crew_Presence_Auto"], enabled = true } },
            { "wMsgWSO_Crew_Presence_Force", new CommandInfo { uniqueid = 24062, category = CommandCategories.WSO, name = "wMsgWSO_Crew_Presence_Force", displayname = Labels.aicommands["wMsgWSO_Crew_Presence_Force"], enabled = true } },
            { "wMsgWSO_Crew_Presence_Disable", new CommandInfo { uniqueid = 24063, category = CommandCategories.WSO, name = "wMsgWSO_Crew_Presence_Disable", displayname = Labels.aicommands["wMsgWSO_Crew_Presence_Disable"], enabled = true } },
            { "wMsgWSO_Crew_Talking_Talk", new CommandInfo { uniqueid = 24064, category = CommandCategories.WSO, name = "wMsgWSO_Crew_Talking_Talk", displayname = Labels.aicommands["wMsgWSO_Crew_Talking_Talk"], enabled = true } },
            { "wMsgWSO_Crew_Talking_Silence", new CommandInfo { uniqueid = 24065, category = CommandCategories.WSO, name = "wMsgWSO_Crew_Talking_Silence", displayname = Labels.aicommands["wMsgWSO_Crew_Talking_Silence"], enabled = true } },
            { "wMsgWSO_Crew_Ejection_WSO", new CommandInfo { uniqueid = 24066, category = CommandCategories.WSO, name = "wMsgWSO_Crew_Ejection_WSO", displayname = Labels.aicommands["wMsgWSO_Crew_Ejection_WSO"], enabled = true } },
            { "wMsgWSO_Crew_Ejection_Both", new CommandInfo { uniqueid = 24067, category = CommandCategories.WSO, name = "wMsgWSO_Crew_Ejection_Both", displayname = Labels.aicommands["wMsgWSO_Crew_Ejection_Both"], enabled = true } },
            { "wMsgWSO_Crew_Countermeasures_Manual", new CommandInfo { uniqueid = 24510, category = CommandCategories.WSO, name = "wMsgWSO_Crew_Countermeasures_Manual", displayname = Labels.aicommands["wMsgWSO_Crew_Countermeasures_Manual"], enabled = true } },
            { "wMsgWSO_Crew_Countermeasures_Jester", new CommandInfo { uniqueid = 24511, category = CommandCategories.WSO, name = "wMsgWSO_Crew_Countermeasures_Jester", displayname = Labels.aicommands["wMsgWSO_Crew_Countermeasures_Jester"], enabled = true } },

            // Ground Crew commands
            { "wMsgWSO_Ground_WheelChocks_Place", new CommandInfo { uniqueid = 24420, category = CommandCategories.WSO, name = "wMsgWSO_Ground_WheelChocks_Place", displayname = Labels.aicommands["wMsgWSO_Ground_WheelChocks_Place"], enabled = true } },
            { "wMsgWSO_Ground_WheelChocks_Remove", new CommandInfo { uniqueid = 24421, category = CommandCategories.WSO, name = "wMsgWSO_Ground_WheelChocks_Remove", displayname = Labels.aicommands["wMsgWSO_Ground_WheelChocks_Remove"], enabled = true } },
            { "wMsgWSO_Ground_Power_Connect", new CommandInfo { uniqueid = 24422, category = CommandCategories.WSO, name = "wMsgWSO_Ground_Power_Connect", displayname = Labels.aicommands["wMsgWSO_Ground_Power_Connect"], enabled = true } },
            { "wMsgWSO_Ground_Power_Disconnect", new CommandInfo { uniqueid = 24423, category = CommandCategories.WSO, name = "wMsgWSO_Ground_Power_Disconnect", displayname = Labels.aicommands["wMsgWSO_Ground_Power_Disconnect"], enabled = true } },
            { "wMsgWSO_Ground_Air_ConnectRight", new CommandInfo { uniqueid = 24424, category = CommandCategories.WSO, name = "wMsgWSO_Ground_Air_ConnectRight", displayname = Labels.aicommands["wMsgWSO_Ground_Air_ConnectRight"], enabled = true } },
            { "wMsgWSO_Ground_Air_ConnectLeft", new CommandInfo { uniqueid = 24425, category = CommandCategories.WSO, name = "wMsgWSO_Ground_Air_ConnectLeft", displayname = Labels.aicommands["wMsgWSO_Ground_Air_ConnectLeft"], enabled = true } },
            { "wMsgWSO_Ground_Air_StartAirflow", new CommandInfo { uniqueid = 24426, category = CommandCategories.WSO, name = "wMsgWSO_Ground_Air_StartAirflow", displayname = Labels.aicommands["wMsgWSO_Ground_Air_StartAirflow"], enabled = true } },
            { "wMsgWSO_Ground_Air_StopAirflow", new CommandInfo { uniqueid = 24427, category = CommandCategories.WSO, name = "wMsgWSO_Ground_Air_StopAirflow", displayname = Labels.aicommands["wMsgWSO_Ground_Air_StopAirflow"], enabled = true } },
            { "wMsgWSO_Ground_Air_Disconnect", new CommandInfo { uniqueid = 24428, category = CommandCategories.WSO, name = "wMsgWSO_Ground_Air_Disconnect", displayname = Labels.aicommands["wMsgWSO_Ground_Air_Disconnect"], enabled = true } },
            { "wMsgWSO_Ground_EngineStartCartridges_Load", new CommandInfo { uniqueid = 24429, category = CommandCategories.WSO, name = "wMsgWSO_Ground_EngineStartCartridges_Load", displayname = Labels.aicommands["wMsgWSO_Ground_EngineStartCartridges_Load"], enabled = true } },
            { "wMsgWSO_Ground_EngineStartCartridges_Remove", new CommandInfo { uniqueid = 24430, category = CommandCategories.WSO, name = "wMsgWSO_Ground_EngineStartCartridges_Remove", displayname = Labels.aicommands["wMsgWSO_Ground_EngineStartCartridges_Remove"], enabled = true } },
            { "wMsgWSO_Ground_Ladder_Place", new CommandInfo { uniqueid = 24431, category = CommandCategories.WSO, name = "wMsgWSO_Ground_Ladder_Place", displayname = Labels.aicommands["wMsgWSO_Ground_Ladder_Place"], enabled = true } },
            { "wMsgWSO_Ground_Ladder_Remove", new CommandInfo { uniqueid = 24432, category = CommandCategories.WSO, name = "wMsgWSO_Ground_Ladder_Remove", displayname = Labels.aicommands["wMsgWSO_Ground_Ladder_Remove"], enabled = true } },
            { "wMsgWSO_Ground_Steps_Extend", new CommandInfo { uniqueid = 24433, category = CommandCategories.WSO, name = "wMsgWSO_Ground_Steps_Extend", displayname = Labels.aicommands["wMsgWSO_Ground_Steps_Extend"], enabled = true } },
            { "wMsgWSO_Ground_Steps_Retract", new CommandInfo { uniqueid = 24434, category = CommandCategories.WSO, name = "wMsgWSO_Ground_Steps_Retract", displayname = Labels.aicommands["wMsgWSO_Ground_Steps_Retract"], enabled = true } },
            { "wMsgWSO_Ground_CommsCheck", new CommandInfo { uniqueid = 24448, category = CommandCategories.WSO, name = "wMsgWSO_Ground_CommsCheck", displayname = Labels.aicommands["wMsgWSO_Ground_CommsCheck"], enabled = true } },
            { "wMsgWSO_Ground_PitotCheck", new CommandInfo { uniqueid = 24449, category = CommandCategories.WSO, name = "wMsgWSO_Ground_PitotCheck", displayname = Labels.aicommands["wMsgWSO_Ground_PitotCheck"], enabled = true } },
            { "wMsgWSO_Ground_SpoilerActuatorCheck", new CommandInfo { uniqueid = 24450, category = CommandCategories.WSO, name = "wMsgWSO_Ground_SpoilerActuatorCheck", displayname = Labels.aicommands["wMsgWSO_Ground_SpoilerActuatorCheck"], enabled = true } },
            { "wMsgWSO_Ground_FlightControlsCheck", new CommandInfo { uniqueid = 24451, category = CommandCategories.WSO, name = "wMsgWSO_Ground_FlightControlsCheck", displayname = Labels.aicommands["wMsgWSO_Ground_FlightControlsCheck"], enabled = true } },
            { "wMsgWSO_Ground_AriCheck", new CommandInfo { uniqueid = 24452, category = CommandCategories.WSO, name = "wMsgWSO_Ground_AriCheck", displayname = Labels.aicommands["wMsgWSO_Ground_AriCheck"], enabled = true } },
            { "wMsgWSO_Ground_StabAugCheck", new CommandInfo { uniqueid = 24453, category = CommandCategories.WSO, name = "wMsgWSO_Ground_StabAugCheck", displayname = Labels.aicommands["wMsgWSO_Ground_StabAugCheck"], enabled = true } },
            { "wMsgWSO_Ground_TrimCheck", new CommandInfo { uniqueid = 24454, category = CommandCategories.WSO, name = "wMsgWSO_Ground_TrimCheck", displayname = Labels.aicommands["wMsgWSO_Ground_TrimCheck"], enabled = true } },

            // Phase 2 commands
            { "wMsgWSO_Systems_FlaresJettison", new CommandInfo { uniqueid = 24435, category = CommandCategories.WSO, name = "wMsgWSO_Systems_FlaresJettison", displayname = Labels.aicommands["wMsgWSO_Systems_FlaresJettison"], enabled = true } },
            { "wMsgWSO_Systems_Countermeasures_Quantity", new CommandInfo { uniqueid = 24436, category = CommandCategories.WSO, name = "wMsgWSO_Systems_Countermeasures_Quantity", displayname = Labels.aicommands["wMsgWSO_Systems_Countermeasures_Quantity"], enabled = true } },
            { "wMsgWSO_Radar_FocusTarget_Direct", new CommandInfo { uniqueid = 24437, category = CommandCategories.WSO, name = "wMsgWSO_Radar_FocusTarget_Direct", displayname = Labels.aicommands["wMsgWSO_Radar_FocusTarget_Direct"], enabled = true } },
            { "wMsgWSO_Radar_LockTarget_Direct", new CommandInfo { uniqueid = 24438, category = CommandCategories.WSO, name = "wMsgWSO_Radar_LockTarget_Direct", displayname = Labels.aicommands["wMsgWSO_Radar_LockTarget_Direct"], enabled = true } },
            { "wMsgWSO_Radar_UnlockTarget", new CommandInfo { uniqueid = 24439, category = CommandCategories.WSO, name = "wMsgWSO_Radar_UnlockTarget", displayname = Labels.aicommands["wMsgWSO_Radar_UnlockTarget"], enabled = true } },
            { "wMsgWSO_Radio_TuneATC_Direct", new CommandInfo { uniqueid = 24440, category = CommandCategories.WSO, name = "wMsgWSO_Radio_TuneATC_Direct", displayname = Labels.aicommands["wMsgWSO_Radio_TuneATC_Direct"], enabled = true } },
            { "wMsgWSO_A2G_PaveSpike_Operation", new CommandInfo { uniqueid = 24441, category = CommandCategories.WSO, name = "wMsgWSO_A2G_PaveSpike_Operation", displayname = Labels.aicommands["wMsgWSO_A2G_PaveSpike_Operation"], enabled = true } },
            { "wMsgWSO_A2G_PaveSpike_LockUnlockTargetAhead", new CommandInfo { uniqueid = 24442, category = CommandCategories.WSO, name = "wMsgWSO_A2G_PaveSpike_LockUnlockTargetAhead", displayname = Labels.aicommands["wMsgWSO_A2G_PaveSpike_LockUnlockTargetAhead"], enabled = true } },
            { "wMsgWSO_A2G_PaveSpike_LaserCode", new CommandInfo { uniqueid = 24443, category = CommandCategories.WSO, name = "wMsgWSO_A2G_PaveSpike_LaserCode", displayname = Labels.aicommands["wMsgWSO_A2G_PaveSpike_LaserCode"], enabled = true } },
            { "wMsgWSO_A2G_PaveSpike_LaserCode_Silent", new CommandInfo { uniqueid = 24444, category = CommandCategories.WSO, name = "wMsgWSO_A2G_PaveSpike_LaserCode_Silent", displayname = Labels.aicommands["wMsgWSO_A2G_PaveSpike_LaserCode_Silent"], enabled = true } },

            // Context response commands
            { "wMsgWSO_Context_Action_Short", new CommandInfo { uniqueid = 24445, category = CommandCategories.WSO, name = "wMsgWSO_Context_Action_Short", displayname = Labels.aicommands["wMsgWSO_Context_Action_Short"], enabled = true } },
            { "wMsgWSO_Context_Action_Long", new CommandInfo { uniqueid = 24446, category = CommandCategories.WSO, name = "wMsgWSO_Context_Action_Long", displayname = Labels.aicommands["wMsgWSO_Context_Action_Long"], enabled = true } },
            { "wMsgWSO_Context_Action_Double", new CommandInfo { uniqueid = 24447, category = CommandCategories.WSO, name = "wMsgWSO_Context_Action_Double", displayname = Labels.aicommands["wMsgWSO_Context_Action_Double"], enabled = true } },
            // Additional WSO commands can be added here within the reserved range (24000 - 24998)            
         };

    }    
}
