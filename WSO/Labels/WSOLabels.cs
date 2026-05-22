using System;
using System.Collections.Generic;

namespace VAICOM.Extensions.WSO
{
    public static partial class Labels
    {
        // Labels for WSO recipients
        public static Dictionary<string, string> airecipients = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "WSO", "F-4E AI WSO" }
        };

        // Labels for WSO commands
        public static Dictionary<string, string> aicommands = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "wMsgWSO_ReportSpeed", "WSO Report Speed" },
            { "wMsgWSO_ToggleRadar", "WSO Toggle Radar" },
            { "wMsgWSO_SetChaffMode", "WSO Set Chaff Mode" },
            { "wMsgWSO_Lantirn_Designate", "WSO Designate" },
            { "wMsgWSO_Lantirn_Undesignate", "WSO Undesignate" },
            { "wMsgWSO_Radar_GoSilent", "WSO Go Silent" },
            { "wMsgWSO_Radar_GoActive", "WSO Go Active" },
            { "wMsgWSO_Radio_TuneATC", "WSO Tune Radio" },
            { "wMsgWSO_Radio_SelectMode", "WSO Select Mode" },
            { "wMsgWSO_Radar_Operation_Active", "WSO Radar Active" },
            { "wMsgWSO_Radar_Operation_Standby", "WSO Radar Standby" },
            { "wMsgWSO_Radar_AutoFocus_On", "WSO Auto Focus On" },
            { "wMsgWSO_Radar_AutoFocus_Off", "WSO Auto Focus Off" },
            { "wMsgWSO_Radar_IFF_Both", "WSO IFF Both" },
            { "wMsgWSO_Radar_IFF_APX76", "WSO IFF APX76" },
            { "wMsgWSO_Radar_Boresight_EnterBST", "WSO Enter Boresight" },
            { "wMsgWSO_Radar_Boresight_Wide", "WSO Boresight Wide" },
            { "wMsgWSO_Radar_Boresight_Nose", "WSO Boresight Nose" },
            { "wMsgWSO_Radar_Boresight_Forward", "WSO Boresight Forward" },
            { "wMsgWSO_Radar_Boresight_Aft", "WSO Boresight Aft" },
            { "wMsgWSO_Radar_Boresight_Tail", "WSO Boresight Tail" },
            { "wMsgWSO_Radar_ScanElevation_Center", "WSO Scan Center" },
            { "wMsgWSO_Radar_ScanElevation_Low", "WSO Scan Low" },
            { "wMsgWSO_Radar_ScanElevation_MidLow", "WSO Scan Mid Low" },
            { "wMsgWSO_Radar_ScanElevation_FarLow", "WSO Scan Far Low" },
            { "wMsgWSO_Radar_ScanElevation_CloseHigh", "WSO Scan Close High" },
            { "wMsgWSO_Radar_ScanElevation_MidHigh", "WSO Scan Mid High" },
            { "wMsgWSO_Radar_ScanElevation_FarHigh", "WSO Scan Far High" },
            { "wMsgWSO_Radar_ScanType_25Wide", "WSO Scan 25 Wide" },
            { "wMsgWSO_Radar_ScanType_25Narrow", "WSO Scan 25 Narrow" },
            { "wMsgWSO_Radar_ScanType_50Wide", "WSO Scan 50 Wide" },
            { "wMsgWSO_Radar_ScanType_50Narrow", "WSO Scan 50 Narrow" },
            { "wMsgWSO_Radar_FocusTarget", "WSO Focus Target" },
            { "wMsgWSO_Radar_LockTarget", "WSO Lock Target" },
            { "wMsgWSO_A2G_TVWeapons", "WSO TV Weapons" },
            { "wMsgWSO_A2G_TVPaveSpike", "WSO TV Pave Spike" },
            { "wMsgWSO_Navigation_Divert_LatLong", "WSO Divert To" },
            { "wMsgWSO_Navigation_Holding_ActivateCurrentTurnPoint", "WSO Hold Current Turn Point" },
            { "wMsgWSO_Navigation_Holding_DeactivateCurrentTurnPoint", "WSO Deactivate Current Turn Point" },
            { "wMsgWSO_Navigation_ResumeNextWaypoint", "WSO Resume Waypoint" },
            { "wMsgWSO_Navigation_TACAN_SelectMode_Off", "WSO TACAN Mode Off" },
            { "wMsgWSO_Navigation_TACAN_SelectMode_R", "WSO TACAN Mode R" },
            { "wMsgWSO_Navigation_TACAN_SelectMode_TR", "WSO TACAN Mode TR" },
            { "wMsgWSO_Navigation_TACAN_SelectMode_AAR", "WSO TACAN Mode AAR" },
            { "wMsgWSO_Navigation_TACAN_SelectMode_AATR", "WSO TACAN Mode AATR" },
            { "wMsgWSO_Navigation_TACAN_TuneAsset", "WSO Tune TACAN" },
            { "wMsgWSO_Navigation_RestartAlignment", "WSO Restart Alignment" },
            { "wMsgWSO_Systems_ChaffMode_Off", "WSO Chaff Mode Off" },
            { "wMsgWSO_Systems_ChaffMode_Single", "WSO Chaff Mode Single" },
            { "wMsgWSO_Systems_ChaffMode_Multiple", "WSO Chaff Mode Multiple" },
            { "wMsgWSO_Systems_ChaffMode_Program", "WSO Chaff Mode Program" },
            { "wMsgWSO_Systems_FlareMode_Off", "WSO Flare Mode Off" },
            { "wMsgWSO_Systems_FlareMode_Single", "WSO Flare Mode Single" },
            { "wMsgWSO_Systems_FlareMode_Program", "WSO Flare Mode Program" },
            { "wMsgWSO_Systems_Jammer_Standby", "WSO Jammer Standby" },
            { "wMsgWSO_Systems_Jammer_Transmit", "WSO Jammer Transmit" },
            { "wMsgWSO_Systems_AVTR_Record", "WSO AVTR Record" },
            { "wMsgWSO_Systems_AVTR_Standby", "WSO AVTR Standby" },
            { "wMsgWSO_Systems_AVTR_Off", "WSO AVTR Off" },
            { "wMsgWSO_Crew_Presence_Auto", "WSO Crew Auto" },
            { "wMsgWSO_Crew_Presence_Force", "WSO Crew Force" },
            { "wMsgWSO_Crew_Presence_Disable", "WSO Crew Disable" },
            { "wMsgWSO_Crew_Talking_Talk", "WSO Talk" },
            { "wMsgWSO_Crew_Talking_Silence", "WSO Silence" },
            { "wMsgWSO_Crew_Ejection_WSO", "WSO Eject WSO" },
            { "wMsgWSO_Crew_Ejection_Both", "WSO Eject Both" },
            { "wMsgWSO_Crew_Countermeasures_Manual", "WSO Countermeasures Manual" },
            { "wMsgWSO_Crew_Countermeasures_Jester", "WSO Countermeasures Jester" },
            { "wMsgWSO_Crew_StartAlignment", "WSO Start Alignment Now" },

            // Ground Crew commands
            { "wMsgWSO_Ground_WheelChocks_Place", "WSO Ground Place Chocks" },
            { "wMsgWSO_Ground_WheelChocks_Remove", "WSO Ground Remove Chocks" },
            { "wMsgWSO_Ground_Power_Connect", "WSO Ground Connect Power" },
            { "wMsgWSO_Ground_Power_Disconnect", "WSO Ground Disconnect Power" },
            { "wMsgWSO_Ground_Air_ConnectRight", "WSO Ground Connect Air Right" },
            { "wMsgWSO_Ground_Air_ConnectLeft", "WSO Ground Connect Air Left" },
            { "wMsgWSO_Ground_Air_StartAirflow", "WSO Ground Start Airflow" },
            { "wMsgWSO_Ground_Air_StopAirflow", "WSO Ground Stop Airflow" },
            { "wMsgWSO_Ground_Air_Disconnect", "WSO Ground Disconnect Air" },
            { "wMsgWSO_Ground_EngineStartCartridges_Load", "WSO Ground Load Start Cartridges" },
            { "wMsgWSO_Ground_EngineStartCartridges_Remove", "WSO Ground Remove Start Cartridges" },
            { "wMsgWSO_Ground_Ladder_Place", "WSO Ground Place Ladder" },
            { "wMsgWSO_Ground_Ladder_Remove", "WSO Ground Remove Ladder" },
            { "wMsgWSO_Ground_Steps_Extend", "WSO Ground Extend Steps" },
            { "wMsgWSO_Ground_Steps_Retract", "WSO Ground Retract Steps" },
            { "wMsgWSO_Ground_CommsCheck", "WSO Ground Comms Check" },
            { "wMsgWSO_Ground_PitotCheck", "WSO Ground Pitot Check" },
            { "wMsgWSO_Ground_SpoilerActuatorCheck", "WSO Ground Spoiler Actuator Check" },
            { "wMsgWSO_Ground_FlightControlsCheck", "WSO Ground Flight Controls Check" },
            { "wMsgWSO_Ground_AriCheck", "WSO Ground ARI Check" },
            { "wMsgWSO_Ground_StabAugCheck", "WSO Ground Stab Aug Check" },
            { "wMsgWSO_Ground_TrimCheck", "WSO Ground Trim Check" },
            { "wMsgWSO_Ground_Cancel", "WSO Ground Cancel" },

            // Jester startup INS alignment responses
            { "wMsgWSO_INSAlignment_FullAlignment", "WSO Start Full Alignment" },
            { "wMsgWSO_INSAlignment_BathAlignment", "WSO Start BATH Alignment" },
            { "wMsgWSO_INSAlignment_StoredAlignment", "WSO Start Stored Alignment" },
            { "wMsgWSO_INSAlignment_Yes", "WSO Yes Start Alignment" },
            { "wMsgWSO_INSAlignment_No", "WSO Negative On Alignment" },
            { "wMsgWSO_INSAlignment_LetYouKnow", "WSO Will Let You Know" },

            // Jester mission altitude responses
            { "wMsgWSO_Altitude_Negative", "WSO Negative Not Going Low" },
            { "wMsgWSO_Altitude_GoingBelow50", "WSO Going Below 50 Feet" },
            { "wMsgWSO_Altitude_GoingBelow100", "WSO Going Below 100 Feet" },
            { "wMsgWSO_Altitude_GoingBelow150", "WSO Going Below 150 Feet" },
            { "wMsgWSO_Altitude_GoingBelow200", "WSO Going Below 200 Feet" },

            // Jester fuel responses
            { "wMsgWSO_Fuel_FuelIsGood", "WSO Fuel Is Looking Good" },

            { "wMsgWSO_Systems_FlaresJettison", "WSO Flares Jettison" },
            { "wMsgWSO_Systems_Countermeasures_Quantity", "WSO Countermeasures Quantity" },
            { "wMsgWSO_Radar_FocusTarget_Direct", "WSO Focus Target Direct" },
            { "wMsgWSO_Radar_LockTarget_Direct", "WSO Lock Target Direct" },
            { "wMsgWSO_Radar_UnlockTarget", "WSO Unlock Target" },
            { "wMsgWSO_A2G_PaveSpike_Ready", "WSO Pave Spike Ready" },
            { "wMsgWSO_A2G_PaveSpike_Standby", "WSO Pave Spike Standby" },
            { "wMsgWSO_A2G_PaveSpike_LockUnlockTargetAhead", "WSO Pave Spike Lock Target Ahead" },

            // Context response commands
            { "wMsgWSO_Context_Short", "WSO Context Action Short" },
            { "wMsgWSO_Context_Long", "WSO Context Action Long" },
            { "wMsgWSO_Context_Double", "WSO Context Action Double" },
        };
    }
}