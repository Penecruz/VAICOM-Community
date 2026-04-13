using System;
using System.Collections.Generic;

namespace VAICOM.Extensions.WSO
{
    public static class Aliases
    {
        // Aliases for recipients (get added to recipient aliases)
        public static Dictionary<string, string> airecipients = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "WSO", "WSO" },
            { "Wizzo", "WSO" },
            { "Boots", "WSO" },
        };

        // Aliases for WSO commands
        public static Dictionary<string, string> aicommands = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "Report Speed", "wMsgWSO_ReportSpeed" },
            { "Toggle Radar", "wMsgWSO_ToggleRadar" },
            { "Set Chaff Mode", "wMsgWSO_SetChaffMode" },
            { "Lantirn Designate", "wMsgWSO_Lantirn_Designate" },
            { "Lantirn Undesignate", "wMsgWSO_Lantirn_Undesignate" },
            { "Go Radar Silent", "wMsgWSO_Radar_GoSilent" },
            { "Go Radar Active", "wMsgWSO_Radar_GoActive" },
            { "Set Manual Frequency", "wMsgWSO_Radio_SetManualFrequency" },
            { "Select Comm Channel", "wMsgWSO_Radio_SelectCommChannel" },
            { "Select Aux Channel", "wMsgWSO_Radio_SelectAuxChannel" },
            { "Tune ATC", "wMsgWSO_Radio_TuneATC" },
            { "Tune Assets", "wMsgWSO_Radio_TuneAssets" },
            { "Select Mode", "wMsgWSO_Radio_SelectMode" },
            { "Radar Active", "wMsgWSO_Radar_Operation_Active" },
            { "Radar Standby", "wMsgWSO_Radar_Operation_Standby" },
            { "Auto Focus On", "wMsgWSO_Radar_AutoFocus_On" },
            { "Auto Focus Off", "wMsgWSO_Radar_AutoFocus_Off" },
            { "IFF Both", "wMsgWSO_Radar_IFF_Both" },
            { "IFF APX76", "wMsgWSO_Radar_IFF_APX76" },
            { "Enter Boresight", "wMsgWSO_Radar_Boresight_EnterBST" },
            { "Boresight Wide", "wMsgWSO_Radar_Boresight_Wide" },
            { "Boresight Nose", "wMsgWSO_Radar_Boresight_Nose" },
            { "Boresight Forward", "wMsgWSO_Radar_Boresight_Forward" },
            { "Boresight Aft", "wMsgWSO_Radar_Boresight_Aft" },
            { "Boresight Tail", "wMsgWSO_Radar_Boresight_Tail" },
            { "Radar Scan Center", "wMsgWSO_Radar_ScanElevation_Center" },
            { "Radar Scan Low", "wMsgWSO_Radar_ScanElevation_Low" },
            { "Radar Scan Mid Low", "wMsgWSO_Radar_ScanElevation_MidLow" },
            { "Radar Scan Far Low", "wMsgWSO_Radar_ScanElevation_FarLow" },
            { "Radar Scan Close High", "wMsgWSO_Radar_ScanElevation_CloseHigh" },
            { "Radar Scan Mid High", "wMsgWSO_Radar_ScanElevation_MidHigh" },
            { "Radar Scan Far High", "wMsgWSO_Radar_ScanElevation_FarHigh" },
            { "Radar Scan 25 Wide", "wMsgWSO_Radar_ScanType_25Wide" },
            { "Radar Scan 25 Narrow", "wMsgWSO_Radar_ScanType_25Narrow" },
            { "Radar Scan 50 Wide", "wMsgWSO_Radar_ScanType_50Wide" },
            { "Radar Scan 50 Narrow", "wMsgWSO_Radar_ScanType_50Narrow" },
            { "Focus Target", "wMsgWSO_Radar_FocusTarget" },
            { "Lock Target", "wMsgWSO_Radar_LockTarget" },
            { "TV Weapons", "wMsgWSO_A2G_TVWeapons" },
            { "TV Pave Spike", "wMsgWSO_A2G_TVPaveSpike" },
            { "Go To Resume", "wMsgWSO_Navigation_GoToResume" },
            { "Hold Current Turn Point", "wMsgWSO_Navigation_Holding_CurrentTurnPoint" },
            { "Divert Lat Long", "wMsgWSO_Navigation_Divert_LatLong" },
            { "TACAN Off", "wMsgWSO_Navigation_TACAN_SelectMode_Off" },
            { "TACAN Recieve", "wMsgWSO_Navigation_TACAN_SelectMode_R" },
            { "TACAN Transmit and Recieve", "wMsgWSO_Navigation_TACAN_SelectMode_TR" },
            { "TACAN Air refuel", "wMsgWSO_Navigation_TACAN_SelectMode_AAR" },
            { "TACAN Air to Air", "wMsgWSO_Navigation_TACAN_SelectMode_AATR" },
            { "Chaff Mode Off", "wMsgWSO_Systems_ChaffMode_Off" },
            { "Chaff Mode Single", "wMsgWSO_Systems_ChaffMode_Single" },
            { "Chaff Mode Multiple", "wMsgWSO_Systems_ChaffMode_Multiple" },
            { "Chaff Mode Program", "wMsgWSO_Systems_ChaffMode_Program" },
            { "Flare Mode Off", "wMsgWSO_Systems_FlareMode_Off" },
            { "Flare Mode Single", "wMsgWSO_Systems_FlareMode_Single" },
            { "Flare Mode Program", "wMsgWSO_Systems_FlareMode_Program" },
            { "Jammer To Standby", "wMsgWSO_Systems_Jammer_Standby" },
            { "Jammer To Transmit", "wMsgWSO_Systems_Jammer_Transmit" },
            { "AVTR Record", "wMsgWSO_Systems_AVTR_Record" },
            { "AVTR Standby", "wMsgWSO_Systems_AVTR_Standby" },
            { "AVTR Off", "wMsgWSO_Systems_AVTR_Off" },
            { "Crew Auto", "wMsgWSO_Crew_Presence_Auto" },
            { "Crew Force", "wMsgWSO_Crew_Presence_Force" },
            { "Crew Disable", "wMsgWSO_Crew_Presence_Disable" },
            { "Talk", "wMsgWSO_Crew_Talking_Talk" },
            { "Silence", "wMsgWSO_Crew_Talking_Silence" },
            { "Eject WSO", "wMsgWSO_Crew_Ejection_WSO" },
            { "Eject Both", "wMsgWSO_Crew_Ejection_Both" },
            { "Countermeasures Mine", "wMsgWSO_Crew_Countermeasures_Manual" },
            { "Countermeasures Yours", "wMsgWSO_Crew_Countermeasures_Jester" },
        };
    }
}