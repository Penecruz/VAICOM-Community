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
        /// CommandCategories.WSO_navigation:       24000 - 24099
        /// CommandCategories.WSO_radio:            24100 - 24199
        /// CommandCategories.WSO_radar:            24200 - 24299
        /// CommandCategories.WSO_weapons:          24300 - 24399
        /// CommandCategories.WSO_defensive:        24400 - 24499
        /// CommandCategories.WSO_crew:             24500 - 24599
        /// CommandCategories.WSO_groundcrew:       24600 - 24699
        /// CommandCategories.WSO_misc:             24700 - 24999
        /// 
        /// </summary>
        public static Dictionary<string, CommandInfo> all = new Dictionary<string, CommandInfo>(StringComparer.OrdinalIgnoreCase)
        {
            // Navigation
            { "wMsgWSO_Navigation_Divert_LatLong", new CommandInfo { uniqueid = 24000, category = CommandCategories.WSO, name = "wMsgWSO_Navigation_Divert_LatLong", displayname = Labels.aicommands["wMsgWSO_Navigation_Divert_LatLong"], enabled = true } },
            { "wMsgWSO_Navigation_Holding_ActivateCurrentTurnPoint", new CommandInfo { uniqueid = 24002, category = CommandCategories.WSO, name = "wMsgWSO_Navigation_Holding_ActivateCurrentTurnPoint", displayname = Labels.aicommands["wMsgWSO_Navigation_Holding_ActivateCurrentTurnPoint"], enabled = true } },
            { "wMsgWSO_Navigation_Holding_DeactivateCurrentTurnPoint", new CommandInfo { uniqueid = 24003, category = CommandCategories.WSO, name = "wMsgWSO_Navigation_Holding_DeactivateCurrentTurnPoint", displayname = Labels.aicommands["wMsgWSO_Navigation_Holding_DeactivateCurrentTurnPoint"], enabled = true } },
            { "wMsgWSO_Navigation_TACAN_SelectMode_Off", new CommandInfo { uniqueid = 24004, category = CommandCategories.WSO, name = "wMsgWSO_Navigation_TACAN_SelectMode_Off", displayname = Labels.aicommands["wMsgWSO_Navigation_TACAN_SelectMode_Off"], enabled = true } },
            { "wMsgWSO_Navigation_TACAN_SelectMode_R", new CommandInfo { uniqueid = 24005, category = CommandCategories.WSO, name = "wMsgWSO_Navigation_TACAN_SelectMode_R", displayname = Labels.aicommands["wMsgWSO_Navigation_TACAN_SelectMode_R"], enabled = true } },
            { "wMsgWSO_Navigation_TACAN_SelectMode_TR", new CommandInfo { uniqueid = 24006, category = CommandCategories.WSO, name = "wMsgWSO_Navigation_TACAN_SelectMode_TR", displayname = Labels.aicommands["wMsgWSO_Navigation_TACAN_SelectMode_TR"], enabled = true } },
            { "wMsgWSO_Navigation_TACAN_SelectMode_AAR", new CommandInfo { uniqueid = 24007, category = CommandCategories.WSO, name = "wMsgWSO_Navigation_TACAN_SelectMode_AAR", displayname = Labels.aicommands["wMsgWSO_Navigation_TACAN_SelectMode_AAR"], enabled = true } },
            { "wMsgWSO_Navigation_TACAN_SelectMode_AATR", new CommandInfo { uniqueid = 24008, category = CommandCategories.WSO, name = "wMsgWSO_Navigation_TACAN_SelectMode_AATR", displayname = Labels.aicommands["wMsgWSO_Navigation_TACAN_SelectMode_AATR"], enabled = true } },
            { "wMsgWSO_Navigation_TACAN_TuneAsset", new CommandInfo { uniqueid = 24009, category = CommandCategories.WSO, name = "wMsgWSO_Navigation_TACAN_TuneAsset", displayname = Labels.aicommands["wMsgWSO_Navigation_TACAN_TuneAsset"], enabled = true } },
            { "wMsgWSO_Navigation_ResumeNextWaypoint", new CommandInfo { uniqueid = 24010, category = CommandCategories.WSO, name = "wMsgWSO_Navigation_ResumeNextWaypoint", displayname = Labels.aicommands["wMsgWSO_Navigation_ResumeNextWaypoint"], enabled = true } },
            { "wMsgWSO_Navigation_RestartAlignment", new CommandInfo { uniqueid = 24011, category = CommandCategories.WSO, name = "wMsgWSO_Navigation_RestartAlignment", displayname = Labels.aicommands["wMsgWSO_Navigation_RestartAlignment"], enabled = true } },
            
            // Radio
            { "wMsgWSO_Radio_TuneATC", new CommandInfo { uniqueid = 24100, category = CommandCategories.WSO, name = "wMsgWSO_Radio_TuneATC", displayname = Labels.aicommands["wMsgWSO_Radio_TuneATC"], enabled = true } },
            { "wMsgWSO_Radio_SelectMode", new CommandInfo { uniqueid = 24101, category = CommandCategories.WSO, name = "wMsgWSO_Radio_SelectMode", displayname = Labels.aicommands["wMsgWSO_Radio_SelectMode"], enabled = true } },
            
            // Radar
            { "wMsgWSO_ToggleRadar", new CommandInfo { uniqueid = 24200, category = CommandCategories.WSO, name = "wMsgWSO_ToggleRadar", displayname = Labels.aicommands["wMsgWSO_ToggleRadar"], enabled = true } },
            { "wMsgWSO_Radar_GoSilent", new CommandInfo { uniqueid = 24201, category = CommandCategories.WSO, name = "wMsgWSO_Radar_GoSilent", displayname = Labels.aicommands["wMsgWSO_Radar_GoSilent"], enabled = true } },
            { "wMsgWSO_Radar_GoActive", new CommandInfo { uniqueid = 24202, category = CommandCategories.WSO, name = "wMsgWSO_Radar_GoActive", displayname = Labels.aicommands["wMsgWSO_Radar_GoActive"], enabled = true } },
            { "wMsgWSO_Radar_Operation_Active", new CommandInfo { uniqueid = 24203, category = CommandCategories.WSO, name = "wMsgWSO_Radar_Operation_Active", displayname = Labels.aicommands["wMsgWSO_Radar_Operation_Active"], enabled = true } },
            { "wMsgWSO_Radar_Operation_Standby", new CommandInfo { uniqueid = 24204, category = CommandCategories.WSO, name = "wMsgWSO_Radar_Operation_Standby", displayname = Labels.aicommands["wMsgWSO_Radar_Operation_Standby"], enabled = true } },
            { "wMsgWSO_Radar_AutoFocus_On", new CommandInfo { uniqueid = 24205, category = CommandCategories.WSO, name = "wMsgWSO_Radar_AutoFocus_On", displayname = Labels.aicommands["wMsgWSO_Radar_AutoFocus_On"], enabled = true } },
            { "wMsgWSO_Radar_AutoFocus_Off", new CommandInfo { uniqueid = 24206, category = CommandCategories.WSO, name = "wMsgWSO_Radar_AutoFocus_Off", displayname = Labels.aicommands["wMsgWSO_Radar_AutoFocus_Off"], enabled = true } },
            { "wMsgWSO_Radar_IFF_Both", new CommandInfo { uniqueid = 24207, category = CommandCategories.WSO, name = "wMsgWSO_Radar_IFF_Both", displayname = Labels.aicommands["wMsgWSO_Radar_IFF_Both"], enabled = true } },
            { "wMsgWSO_Radar_IFF_APX76", new CommandInfo { uniqueid = 24208, category = CommandCategories.WSO, name = "wMsgWSO_Radar_IFF_APX76", displayname = Labels.aicommands["wMsgWSO_Radar_IFF_APX76"], enabled = true } },
            { "wMsgWSO_Radar_Boresight_EnterBST", new CommandInfo { uniqueid = 24209, category = CommandCategories.WSO, name = "wMsgWSO_Radar_Boresight_EnterBST", displayname = Labels.aicommands["wMsgWSO_Radar_Boresight_EnterBST"], enabled = true } },
            { "wMsgWSO_Radar_Boresight_Wide", new CommandInfo { uniqueid = 24210, category = CommandCategories.WSO, name = "wMsgWSO_Radar_Boresight_Wide", displayname = Labels.aicommands["wMsgWSO_Radar_Boresight_Wide"], enabled = true } },
            { "wMsgWSO_Radar_Boresight_Nose", new CommandInfo { uniqueid = 24211, category = CommandCategories.WSO, name = "wMsgWSO_Radar_Boresight_Nose", displayname = Labels.aicommands["wMsgWSO_Radar_Boresight_Nose"], enabled = true } },
            { "wMsgWSO_Radar_Boresight_Forward", new CommandInfo { uniqueid = 24212, category = CommandCategories.WSO, name = "wMsgWSO_Radar_Boresight_Forward", displayname = Labels.aicommands["wMsgWSO_Radar_Boresight_Forward"], enabled = true } },
            { "wMsgWSO_Radar_Boresight_Aft", new CommandInfo { uniqueid = 24213, category = CommandCategories.WSO, name = "wMsgWSO_Radar_Boresight_Aft", displayname = Labels.aicommands["wMsgWSO_Radar_Boresight_Aft"], enabled = true } },
            { "wMsgWSO_Radar_Boresight_Tail", new CommandInfo { uniqueid = 24214, category = CommandCategories.WSO, name = "wMsgWSO_Radar_Boresight_Tail", displayname = Labels.aicommands["wMsgWSO_Radar_Boresight_Tail"], enabled = true } },
            { "wMsgWSO_Radar_ScanElevation_Center", new CommandInfo { uniqueid = 24215, category = CommandCategories.WSO, name = "wMsgWSO_Radar_ScanElevation_Center", displayname = Labels.aicommands["wMsgWSO_Radar_ScanElevation_Center"], enabled = true } },
            { "wMsgWSO_Radar_ScanElevation_Low", new CommandInfo { uniqueid = 24216, category = CommandCategories.WSO, name = "wMsgWSO_Radar_ScanElevation_Low", displayname = Labels.aicommands["wMsgWSO_Radar_ScanElevation_Low"], enabled = true } },
            { "wMsgWSO_Radar_ScanElevation_MidLow", new CommandInfo { uniqueid = 24217, category = CommandCategories.WSO, name = "wMsgWSO_Radar_ScanElevation_MidLow", displayname = Labels.aicommands["wMsgWSO_Radar_ScanElevation_MidLow"], enabled = true } },
            { "wMsgWSO_Radar_ScanElevation_FarLow", new CommandInfo { uniqueid = 24218, category = CommandCategories.WSO, name = "wMsgWSO_Radar_ScanElevation_FarLow", displayname = Labels.aicommands["wMsgWSO_Radar_ScanElevation_FarLow"], enabled = true } },
            { "wMsgWSO_Radar_ScanElevation_CloseHigh", new CommandInfo { uniqueid = 24219, category = CommandCategories.WSO, name = "wMsgWSO_Radar_ScanElevation_CloseHigh", displayname = Labels.aicommands["wMsgWSO_Radar_ScanElevation_CloseHigh"], enabled = true } },
            { "wMsgWSO_Radar_ScanElevation_MidHigh", new CommandInfo { uniqueid = 24220, category = CommandCategories.WSO, name = "wMsgWSO_Radar_ScanElevation_MidHigh", displayname = Labels.aicommands["wMsgWSO_Radar_ScanElevation_MidHigh"], enabled = true } },
            { "wMsgWSO_Radar_ScanElevation_FarHigh", new CommandInfo { uniqueid = 24221, category = CommandCategories.WSO, name = "wMsgWSO_Radar_ScanElevation_FarHigh", displayname = Labels.aicommands["wMsgWSO_Radar_ScanElevation_FarHigh"], enabled = true } },
            { "wMsgWSO_Radar_ScanType_25Wide", new CommandInfo { uniqueid = 24222, category = CommandCategories.WSO, name = "wMsgWSO_Radar_ScanType_25Wide", displayname = Labels.aicommands["wMsgWSO_Radar_ScanType_25Wide"], enabled = true } },
            { "wMsgWSO_Radar_ScanType_25Narrow", new CommandInfo { uniqueid = 24223, category = CommandCategories.WSO, name = "wMsgWSO_Radar_ScanType_25Narrow", displayname = Labels.aicommands["wMsgWSO_Radar_ScanType_25Narrow"], enabled = true } },
            { "wMsgWSO_Radar_ScanType_50Wide", new CommandInfo { uniqueid = 24224, category = CommandCategories.WSO, name = "wMsgWSO_Radar_ScanType_50Wide", displayname = Labels.aicommands["wMsgWSO_Radar_ScanType_50Wide"], enabled = true } },
            { "wMsgWSO_Radar_ScanType_50Narrow", new CommandInfo { uniqueid = 24225, category = CommandCategories.WSO, name = "wMsgWSO_Radar_ScanType_50Narrow", displayname = Labels.aicommands["wMsgWSO_Radar_ScanType_50Narrow"], enabled = true } },
            { "wMsgWSO_Radar_UnlockTarget", new CommandInfo { uniqueid = 24228, category = CommandCategories.WSO, name = "wMsgWSO_Radar_UnlockTarget", displayname = Labels.aicommands["wMsgWSO_Radar_UnlockTarget"], enabled = true } },
            { "wMsgWSO_Radar_FocusTarget_Direct", new CommandInfo { uniqueid = 24229, category = CommandCategories.WSO, name = "wMsgWSO_Radar_FocusTarget_Direct", displayname = Labels.aicommands["wMsgWSO_Radar_FocusTarget_Direct"], enabled = true } },
            { "wMsgWSO_Radar_LockTarget_Direct", new CommandInfo { uniqueid = 24230, category = CommandCategories.WSO, name = "wMsgWSO_Radar_LockTarget_Direct", displayname = Labels.aicommands["wMsgWSO_Radar_LockTarget_Direct"], enabled = true } },

            // Weapons
            { "wMsgWSO_A2G_TVWeapons", new CommandInfo { uniqueid = 24300, category = CommandCategories.WSO, name = "wMsgWSO_A2G_TVWeapons", displayname = Labels.aicommands["wMsgWSO_A2G_TVWeapons"], enabled = true } },
            { "wMsgWSO_A2G_TVPaveSpike", new CommandInfo { uniqueid = 24301, category = CommandCategories.WSO, name = "wMsgWSO_A2G_TVPaveSpike", displayname = Labels.aicommands["wMsgWSO_A2G_TVPaveSpike"], enabled = true } },
            { "wMsgWSO_A2G_PaveSpike_Ready", new CommandInfo { uniqueid = 24302, category = CommandCategories.WSO, name = "wMsgWSO_A2G_PaveSpike_Ready", displayname = Labels.aicommands["wMsgWSO_A2G_PaveSpike_Ready"], enabled = true } },
            { "wMsgWSO_A2G_PaveSpike_Standby", new CommandInfo { uniqueid = 24303, category = CommandCategories.WSO, name = "wMsgWSO_A2G_PaveSpike_Standby", displayname = Labels.aicommands["wMsgWSO_A2G_PaveSpike_Standby"], enabled = true } },
            { "wMsgWSO_Lantirn_Designate", new CommandInfo { uniqueid = 24305, category = CommandCategories.WSO, name = "wMsgWSO_Lantirn_Designate", displayname = Labels.aicommands["wMsgWSO_Lantirn_Designate"], enabled = true } },
            { "wMsgWSO_Lantirn_Undesignate", new CommandInfo { uniqueid = 24306, category = CommandCategories.WSO, name = "wMsgWSO_Lantirn_Undesignate", displayname = Labels.aicommands["wMsgWSO_Lantirn_Undesignate"], enabled = true } },
            
            // Defensive
            { "wMsgWSO_SetChaffMode", new CommandInfo { uniqueid = 24400, category = CommandCategories.WSO, name = "wMsgWSO_SetChaffMode", displayname = Labels.aicommands["wMsgWSO_SetChaffMode"], enabled = true } },
            { "wMsgWSO_Systems_ChaffMode_Off", new CommandInfo { uniqueid = 24401, category = CommandCategories.WSO, name = "wMsgWSO_Systems_ChaffMode_Off", displayname = Labels.aicommands["wMsgWSO_Systems_ChaffMode_Off"], enabled = true } },
            { "wMsgWSO_Systems_ChaffMode_Single", new CommandInfo { uniqueid = 24402, category = CommandCategories.WSO, name = "wMsgWSO_Systems_ChaffMode_Single", displayname = Labels.aicommands["wMsgWSO_Systems_ChaffMode_Single"], enabled = true } },
            { "wMsgWSO_Systems_ChaffMode_Multiple", new CommandInfo { uniqueid = 24403, category = CommandCategories.WSO, name = "wMsgWSO_Systems_ChaffMode_Multiple", displayname = Labels.aicommands["wMsgWSO_Systems_ChaffMode_Multiple"], enabled = true } },
            { "wMsgWSO_Systems_ChaffMode_Program", new CommandInfo { uniqueid = 24404, category = CommandCategories.WSO, name = "wMsgWSO_Systems_ChaffMode_Program", displayname = Labels.aicommands["wMsgWSO_Systems_ChaffMode_Program"], enabled = true } },
            { "wMsgWSO_Systems_FlareMode_Off", new CommandInfo { uniqueid = 24405, category = CommandCategories.WSO, name = "wMsgWSO_Systems_FlareMode_Off", displayname = Labels.aicommands["wMsgWSO_Systems_FlareMode_Off"], enabled = true } },
            { "wMsgWSO_Systems_FlareMode_Single", new CommandInfo { uniqueid = 24406, category = CommandCategories.WSO, name = "wMsgWSO_Systems_FlareMode_Single", displayname = Labels.aicommands["wMsgWSO_Systems_FlareMode_Single"], enabled = true } },
            { "wMsgWSO_Systems_FlareMode_Program", new CommandInfo { uniqueid = 24407, category = CommandCategories.WSO, name = "wMsgWSO_Systems_FlareMode_Program", displayname = Labels.aicommands["wMsgWSO_Systems_FlareMode_Program"], enabled = true } },
            { "wMsgWSO_Systems_Jammer_Standby", new CommandInfo { uniqueid = 24408, category = CommandCategories.WSO, name = "wMsgWSO_Systems_Jammer_Standby", displayname = Labels.aicommands["wMsgWSO_Systems_Jammer_Standby"], enabled = true } },
            { "wMsgWSO_Systems_Jammer_Transmit", new CommandInfo { uniqueid = 24409, category = CommandCategories.WSO, name = "wMsgWSO_Systems_Jammer_Transmit", displayname = Labels.aicommands["wMsgWSO_Systems_Jammer_Transmit"], enabled = true } },
            { "wMsgWSO_Systems_FlaresJettison", new CommandInfo { uniqueid = 24410, category = CommandCategories.WSO, name = "wMsgWSO_Systems_FlaresJettison", displayname = Labels.aicommands["wMsgWSO_Systems_FlaresJettison"], enabled = true } },
            { "wMsgWSO_Systems_Countermeasures_Quantity", new CommandInfo { uniqueid = 24411, category = CommandCategories.WSO, name = "wMsgWSO_Systems_Countermeasures_Quantity", displayname = Labels.aicommands["wMsgWSO_Systems_Countermeasures_Quantity"], enabled = true } },
            
            // Crew
            { "wMsgWSO_ReportSpeed", new CommandInfo { uniqueid = 24500, category = CommandCategories.WSO, name = "wMsgWSO_ReportSpeed", displayname = Labels.aicommands["wMsgWSO_ReportSpeed"], enabled = true } },
            { "wMsgWSO_Crew_Presence_Auto", new CommandInfo { uniqueid = 24501, category = CommandCategories.WSO, name = "wMsgWSO_Crew_Presence_Auto", displayname = Labels.aicommands["wMsgWSO_Crew_Presence_Auto"], enabled = true } },
            { "wMsgWSO_Crew_Presence_Force", new CommandInfo { uniqueid = 24502, category = CommandCategories.WSO, name = "wMsgWSO_Crew_Presence_Force", displayname = Labels.aicommands["wMsgWSO_Crew_Presence_Force"], enabled = true } },
            { "wMsgWSO_Crew_Presence_Disable", new CommandInfo { uniqueid = 24503, category = CommandCategories.WSO, name = "wMsgWSO_Crew_Presence_Disable", displayname = Labels.aicommands["wMsgWSO_Crew_Presence_Disable"], enabled = true } },
            { "wMsgWSO_Crew_Talking_Talk", new CommandInfo { uniqueid = 24504, category = CommandCategories.WSO, name = "wMsgWSO_Crew_Talking_Talk", displayname = Labels.aicommands["wMsgWSO_Crew_Talking_Talk"], enabled = true } },
            { "wMsgWSO_Crew_Talking_Silence", new CommandInfo { uniqueid = 24505, category = CommandCategories.WSO, name = "wMsgWSO_Crew_Talking_Silence", displayname = Labels.aicommands["wMsgWSO_Crew_Talking_Silence"], enabled = true } },
            { "wMsgWSO_Crew_Ejection_WSO", new CommandInfo { uniqueid = 24506, category = CommandCategories.WSO, name = "wMsgWSO_Crew_Ejection_WSO", displayname = Labels.aicommands["wMsgWSO_Crew_Ejection_WSO"], enabled = true } },
            { "wMsgWSO_Crew_Ejection_Both", new CommandInfo { uniqueid = 24507, category = CommandCategories.WSO, name = "wMsgWSO_Crew_Ejection_Both", displayname = Labels.aicommands["wMsgWSO_Crew_Ejection_Both"], enabled = true } },
            { "wMsgWSO_Crew_Countermeasures_Manual", new CommandInfo { uniqueid = 24508, category = CommandCategories.WSO, name = "wMsgWSO_Crew_Countermeasures_Manual", displayname = Labels.aicommands["wMsgWSO_Crew_Countermeasures_Manual"], enabled = true } },
            { "wMsgWSO_Crew_Countermeasures_Jester", new CommandInfo { uniqueid = 24509, category = CommandCategories.WSO, name = "wMsgWSO_Crew_Countermeasures_Jester", displayname = Labels.aicommands["wMsgWSO_Crew_Countermeasures_Jester"], enabled = true } },
            { "wMsgWSO_Crew_StartAlignment", new CommandInfo { uniqueid = 24510, category = CommandCategories.WSO, name = "wMsgWSO_Crew_StartAlignment", displayname = Labels.aicommands["wMsgWSO_Crew_StartAlignment"], enabled = true } },
            
            // Ground Crew commands
            { "wMsgWSO_Ground_WheelChocks_Place", new CommandInfo { uniqueid = 24600, category = CommandCategories.WSO, name = "wMsgWSO_Ground_WheelChocks_Place", displayname = Labels.aicommands["wMsgWSO_Ground_WheelChocks_Place"], enabled = true } },
            { "wMsgWSO_Ground_WheelChocks_Remove", new CommandInfo { uniqueid = 24601, category = CommandCategories.WSO, name = "wMsgWSO_Ground_WheelChocks_Remove", displayname = Labels.aicommands["wMsgWSO_Ground_WheelChocks_Remove"], enabled = true } },
            { "wMsgWSO_Ground_Power_Connect", new CommandInfo { uniqueid = 24602, category = CommandCategories.WSO, name = "wMsgWSO_Ground_Power_Connect", displayname = Labels.aicommands["wMsgWSO_Ground_Power_Connect"], enabled = true } },
            { "wMsgWSO_Ground_Power_Disconnect", new CommandInfo { uniqueid = 24603, category = CommandCategories.WSO, name = "wMsgWSO_Ground_Power_Disconnect", displayname = Labels.aicommands["wMsgWSO_Ground_Power_Disconnect"], enabled = true } },
            { "wMsgWSO_Ground_Air_ConnectRight", new CommandInfo { uniqueid = 24604, category = CommandCategories.WSO, name = "wMsgWSO_Ground_Air_ConnectRight", displayname = Labels.aicommands["wMsgWSO_Ground_Air_ConnectRight"], enabled = true } },
            { "wMsgWSO_Ground_Air_ConnectLeft", new CommandInfo { uniqueid = 24605, category = CommandCategories.WSO, name = "wMsgWSO_Ground_Air_ConnectLeft", displayname = Labels.aicommands["wMsgWSO_Ground_Air_ConnectLeft"], enabled = true } },
            { "wMsgWSO_Ground_Air_StartAirflow", new CommandInfo { uniqueid = 24606, category = CommandCategories.WSO, name = "wMsgWSO_Ground_Air_StartAirflow", displayname = Labels.aicommands["wMsgWSO_Ground_Air_StartAirflow"], enabled = true } },
            { "wMsgWSO_Ground_Air_StopAirflow", new CommandInfo { uniqueid = 24607, category = CommandCategories.WSO, name = "wMsgWSO_Ground_Air_StopAirflow", displayname = Labels.aicommands["wMsgWSO_Ground_Air_StopAirflow"], enabled = true } },
            { "wMsgWSO_Ground_Air_Disconnect", new CommandInfo { uniqueid = 24608, category = CommandCategories.WSO, name = "wMsgWSO_Ground_Air_Disconnect", displayname = Labels.aicommands["wMsgWSO_Ground_Air_Disconnect"], enabled = true } },
            { "wMsgWSO_Ground_EngineStartCartridges_Load", new CommandInfo { uniqueid = 24609, category = CommandCategories.WSO, name = "wMsgWSO_Ground_EngineStartCartridges_Load", displayname = Labels.aicommands["wMsgWSO_Ground_EngineStartCartridges_Load"], enabled = true } },
            { "wMsgWSO_Ground_EngineStartCartridges_Remove", new CommandInfo { uniqueid = 24610, category = CommandCategories.WSO, name = "wMsgWSO_Ground_EngineStartCartridges_Remove", displayname = Labels.aicommands["wMsgWSO_Ground_EngineStartCartridges_Remove"], enabled = true } },
            { "wMsgWSO_Ground_Ladder_Place", new CommandInfo { uniqueid = 24611, category = CommandCategories.WSO, name = "wMsgWSO_Ground_Ladder_Place", displayname = Labels.aicommands["wMsgWSO_Ground_Ladder_Place"], enabled = true } },
            { "wMsgWSO_Ground_Ladder_Remove", new CommandInfo { uniqueid = 24612, category = CommandCategories.WSO, name = "wMsgWSO_Ground_Ladder_Remove", displayname = Labels.aicommands["wMsgWSO_Ground_Ladder_Remove"], enabled = true } },
            { "wMsgWSO_Ground_Steps_Extend", new CommandInfo { uniqueid = 24613, category = CommandCategories.WSO, name = "wMsgWSO_Ground_Steps_Extend", displayname = Labels.aicommands["wMsgWSO_Ground_Steps_Extend"], enabled = true } },
            { "wMsgWSO_Ground_Steps_Retract", new CommandInfo { uniqueid = 24614, category = CommandCategories.WSO, name = "wMsgWSO_Ground_Steps_Retract", displayname = Labels.aicommands["wMsgWSO_Ground_Steps_Retract"], enabled = true } },
            { "wMsgWSO_Ground_CommsCheck", new CommandInfo { uniqueid = 24615, category = CommandCategories.WSO, name = "wMsgWSO_Ground_CommsCheck", displayname = Labels.aicommands["wMsgWSO_Ground_CommsCheck"], enabled = true } },
            { "wMsgWSO_Ground_PitotCheck", new CommandInfo { uniqueid = 24616, category = CommandCategories.WSO, name = "wMsgWSO_Ground_PitotCheck", displayname = Labels.aicommands["wMsgWSO_Ground_PitotCheck"], enabled = true } },
            { "wMsgWSO_Ground_SpoilerActuatorCheck", new CommandInfo { uniqueid = 24617, category = CommandCategories.WSO, name = "wMsgWSO_Ground_SpoilerActuatorCheck", displayname = Labels.aicommands["wMsgWSO_Ground_SpoilerActuatorCheck"], enabled = true } },
            { "wMsgWSO_Ground_FlightControlsCheck", new CommandInfo { uniqueid = 24618, category = CommandCategories.WSO, name = "wMsgWSO_Ground_FlightControlsCheck", displayname = Labels.aicommands["wMsgWSO_Ground_FlightControlsCheck"], enabled = true } },
            { "wMsgWSO_Ground_AriCheck", new CommandInfo { uniqueid = 24619, category = CommandCategories.WSO, name = "wMsgWSO_Ground_AriCheck", displayname = Labels.aicommands["wMsgWSO_Ground_AriCheck"], enabled = true } },
            { "wMsgWSO_Ground_StabAugCheck", new CommandInfo { uniqueid = 24620, category = CommandCategories.WSO, name = "wMsgWSO_Ground_StabAugCheck", displayname = Labels.aicommands["wMsgWSO_Ground_StabAugCheck"], enabled = true } },
            { "wMsgWSO_Ground_TrimCheck", new CommandInfo { uniqueid = 24621, category = CommandCategories.WSO, name = "wMsgWSO_Ground_TrimCheck", displayname = Labels.aicommands["wMsgWSO_Ground_TrimCheck"], enabled = true } },
            { "wMsgWSO_Ground_Cancel", new CommandInfo { uniqueid = 24622, category = CommandCategories.WSO, name = "wMsgWSO_Ground_Cancel", displayname = Labels.aicommands["wMsgWSO_Ground_Cancel"], enabled = true } },

            // Miscellaneous
            { "wMsgWSO_Systems_AVTR_Record", new CommandInfo { uniqueid = 24700, category = CommandCategories.WSO, name = "wMsgWSO_Systems_AVTR_Record", displayname = Labels.aicommands["wMsgWSO_Systems_AVTR_Record"], enabled = true } },
            { "wMsgWSO_Systems_AVTR_Standby", new CommandInfo { uniqueid = 24701, category = CommandCategories.WSO, name = "wMsgWSO_Systems_AVTR_Standby", displayname = Labels.aicommands["wMsgWSO_Systems_AVTR_Standby"], enabled = true } },
            { "wMsgWSO_Systems_AVTR_Off", new CommandInfo { uniqueid = 24702, category = CommandCategories.WSO, name = "wMsgWSO_Systems_AVTR_Off", displayname = Labels.aicommands["wMsgWSO_Systems_AVTR_Off"], enabled = true } },
            
            // Jester startup alignment responses
            { "wMsgWSO_INSAlignment_FullAlignment", new CommandInfo { uniqueid = 24751, category = CommandCategories.WSO, name = "wMsgWSO_INSAlignment_FullAlignment", displayname = Labels.aicommands["wMsgWSO_INSAlignment_FullAlignment"], enabled = true } },
            { "wMsgWSO_INSAlignment_BathAlignment", new CommandInfo { uniqueid = 24752, category = CommandCategories.WSO, name = "wMsgWSO_INSAlignment_BathAlignment", displayname = Labels.aicommands["wMsgWSO_INSAlignment_BathAlignment"], enabled = true } },
            { "wMsgWSO_INSAlignment_StoredAlignment", new CommandInfo { uniqueid = 24753, category = CommandCategories.WSO, name = "wMsgWSO_INSAlignment_StoredAlignment", displayname = Labels.aicommands["wMsgWSO_INSAlignment_StoredAlignment"], enabled = true } },
            { "wMsgWSO_INSAlignment_Yes", new CommandInfo { uniqueid = 24754, category = CommandCategories.WSO, name = "wMsgWSO_INSAlignment_Yes", displayname = Labels.aicommands["wMsgWSO_INSAlignment_Yes"], enabled = true } },
            { "wMsgWSO_INSAlignment_No", new CommandInfo { uniqueid = 24755, category = CommandCategories.WSO, name = "wMsgWSO_INSAlignment_No", displayname = Labels.aicommands["wMsgWSO_INSAlignment_No"], enabled = true } },
            { "wMsgWSO_INSAlignment_LetYouKnow", new CommandInfo { uniqueid = 24756, category = CommandCategories.WSO, name = "wMsgWSO_INSAlignment_LetYouKnow", displayname = Labels.aicommands["wMsgWSO_INSAlignment_LetYouKnow"], enabled = true } },
            
            // Jester mission altitude responses
            { "wMsgWSO_Altitude_Negative", new CommandInfo { uniqueid = 24760, category = CommandCategories.WSO, name = "wMsgWSO_Altitude_Negative", displayname = Labels.aicommands["wMsgWSO_Altitude_Negative"], enabled = true } },
            { "wMsgWSO_Altitude_GoingBelow50", new CommandInfo { uniqueid = 24761, category = CommandCategories.WSO, name = "wMsgWSO_Altitude_GoingBelow50", displayname = Labels.aicommands["wMsgWSO_Altitude_GoingBelow50"], enabled = true } },
            { "wMsgWSO_Altitude_GoingBelow100", new CommandInfo { uniqueid = 24762, category = CommandCategories.WSO, name = "wMsgWSO_Altitude_GoingBelow100", displayname = Labels.aicommands["wMsgWSO_Altitude_GoingBelow100"], enabled = true } },
            { "wMsgWSO_Altitude_GoingBelow150", new CommandInfo { uniqueid = 24763, category = CommandCategories.WSO, name = "wMsgWSO_Altitude_GoingBelow150", displayname = Labels.aicommands["wMsgWSO_Altitude_GoingBelow150"], enabled = true } },
            { "wMsgWSO_Altitude_GoingBelow200", new CommandInfo { uniqueid = 24764, category = CommandCategories.WSO, name = "wMsgWSO_Altitude_GoingBelow200", displayname = Labels.aicommands["wMsgWSO_Altitude_GoingBelow200"], enabled = true } },

            // Jester fuel responses
            { "wMsgWSO_Fuel_FuelIsGood", new CommandInfo { uniqueid = 24770, category = CommandCategories.WSO, name = "wMsgWSO_Fuel_FuelIsGood", displayname = Labels.aicommands["wMsgWSO_Fuel_FuelIsGood"], enabled = true } },
            { "wMsgWSO_Fuel_FuelIsLow", new CommandInfo { uniqueid = 24771, category = CommandCategories.WSO, name = "wMsgWSO_Fuel_FuelIsLow", displayname = Labels.aicommands["wMsgWSO_Fuel_FuelIsLow"], enabled = true } },
            { "wMsgWSO_Fuel_RemainOnMission", new CommandInfo { uniqueid = 24772, category = CommandCategories.WSO, name = "wMsgWSO_Fuel_RemainOnMission", displayname = Labels.aicommands["wMsgWSO_Fuel_RemainOnMission"], enabled = true } },
            { "wMsgWSO_Fuel_AirfieldOptions", new CommandInfo { uniqueid = 24773, category = CommandCategories.WSO, name = "wMsgWSO_Fuel_AirfieldOptions", displayname = Labels.aicommands["wMsgWSO_Fuel_AirfieldOptions"], enabled = true } },
            { "wMsgWSO_Fuel_TankerOptions", new CommandInfo { uniqueid = 24774, category = CommandCategories.WSO, name = "wMsgWSO_Fuel_TankerOptions", displayname = Labels.aicommands["wMsgWSO_Fuel_TankerOptions"], enabled = true } },
            { "wMsgWSO_Fuel_RefuelAtAirfield", new CommandInfo { uniqueid = 24775, category = CommandCategories.WSO, name = "wMsgWSO_Fuel_RefuelAtAirfield", displayname = Labels.aicommands["wMsgWSO_Fuel_RefuelAtAirfield"], enabled = true } },

            // Context response commands
            { "wMsgWSO_Context_Short", new CommandInfo { uniqueid = 24800, category = CommandCategories.WSO, name = "wMsgWSO_Context_Short", displayname = Labels.aicommands["wMsgWSO_Context_Short"], enabled = true } },
            { "wMsgWSO_Context_Long", new CommandInfo { uniqueid = 24801, category = CommandCategories.WSO, name = "wMsgWSO_Context_Long", displayname = Labels.aicommands["wMsgWSO_Context_Long"], enabled = true } },
            { "wMsgWSO_Context_Double", new CommandInfo { uniqueid = 24802, category = CommandCategories.WSO, name = "wMsgWSO_Context_Double", displayname = Labels.aicommands["wMsgWSO_Context_Double"], enabled = true } },
        };

        // Commands that require a mandatory recipient suffix, e.g. diverting to airfields and assets, tuning TACAN assets.
        public static List<string> recipientCommands = new List<string>
        {
            "wMsgWSO_Navigation_Divert_LatLong",
            "wMsgWSO_Navigation_TACAN_TuneAsset",
            "wMsgWSO_Radio_TuneATC",
            "wMsgWSO_Fuel_RefuelAtAirfield"
        };

    }    
}
