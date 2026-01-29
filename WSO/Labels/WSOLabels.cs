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
            { "Jester.ReportSpeed", "WSO Report Speed" },
            { "Jester.ToggleRadar", "WSO Toggle Radar" },
            { "Jester.SetChaffMode", "WSO Set Chaff Mode" },
            { "Jester.Lantirn.Designate", "WSO Designate" },
            { "Jester.Lantirn.Undesignate", "WSO Undesignate" },
            { "Jester.Radar.GoSilent", "WSO Go Silent" },
            { "Jester.Radar.GoActive", "WSO Go Active" },
            { "Jester.Radio.SetManualFrequency", "WSO Set Manual Frequency" },
            { "Jester.Radio.SelectCommChannel", "WSO Select Comm Channel" },
            { "Jester.Radio.SelectAuxChannel", "WSO Select Aux Channel" },
            { "Jester.Radio.TuneATC", "WSO Tune ATC" },
            { "Jester.Radio.TuneAssets", "WSO Tune Assets" },
            { "Jester.Radio.SelectMode", "WSO Select Mode" },
            { "Jester.Radar.Operation.Active", "WSO Radar Active" },
            { "Jester.Radar.Operation.Standby", "WSO Radar Standby" },
            { "Jester.Radar.AutoFocus.On", "WSO Auto Focus On" },
            { "Jester.Radar.AutoFocus.Off", "WSO Auto Focus Off" },
            { "Jester.Radar.IFF.Both", "WSO IFF Both" },
            { "Jester.Radar.IFF.APX76", "WSO IFF APX76" },
            { "Jester.Radar.Boresight.EnterBST", "WSO Enter Boresight" },
            { "Jester.Radar.Boresight.Wide", "WSO Boresight Wide" },
            { "Jester.Radar.Boresight.Nose", "WSO Boresight Nose" },
            { "Jester.Radar.Boresight.Forward", "WSO Boresight Forward" },
            { "Jester.Radar.Boresight.Aft", "WSO Boresight Aft" },
            { "Jester.Radar.Boresight.Tail", "WSO Boresight Tail" },
            { "Jester.Radar.ScanElevation.Center", "WSO Scan Center" },
            { "Jester.Radar.ScanElevation.Low", "WSO Scan Low" },
            { "Jester.Radar.ScanElevation.MidLow", "WSO Scan Mid Low" },
            { "Jester.Radar.ScanElevation.FarLow", "WSO Scan Far Low" },
            { "Jester.Radar.ScanElevation.CloseHigh", "WSO Scan Close High" },
            { "Jester.Radar.ScanElevation.MidHigh", "WSO Scan Mid High" },
            { "Jester.Radar.ScanElevation.FarHigh", "WSO Scan Far High" },
            { "Jester.Radar.ScanType.25Wide", "WSO Scan 25 Wide" },
            { "Jester.Radar.ScanType.25Narrow", "WSO Scan 25 Narrow" },
            { "Jester.Radar.ScanType.50Wide", "WSO Scan 50 Wide" },
            { "Jester.Radar.ScanType.50Narrow", "WSO Scan 50 Narrow" },
            { "Jester.Radar.FocusTarget", "WSO Focus Target" },
            { "Jester.Radar.LockTarget", "WSO Lock Target" },
            { "Jester.A2G.TVWeapons", "WSO TV Weapons" },
            { "Jester.A2G.TVPaveSpike", "WSO TV Pave Spike" },
            { "Jester.Navigation.GoToResume", "WSO Go To Resume" },
            { "Jester.Navigation.Holding.CurrentTurnPoint", "WSO Hold Current Turn Point" },
            { "Jester.Navigation.Divert.LatLong", "WSO Divert Lat Long" },
            { "Jester.Navigation.TACAN.SelectMode.Off", "WSO TACAN Mode Off" },
            { "Jester.Navigation.TACAN.SelectMode.R", "WSO TACAN Mode R" },
            { "Jester.Navigation.TACAN.SelectMode.TR", "WSO TACAN Mode TR" },
            { "Jester.Navigation.TACAN.SelectMode.AAR", "WSO TACAN Mode AAR" },
            { "Jester.Navigation.TACAN.SelectMode.AATR", "WSO TACAN Mode AATR" },
            { "Jester.Systems.ChaffMode.Off", "WSO Chaff Mode Off" },
            { "Jester.Systems.ChaffMode.Single", "WSO Chaff Mode Single" },
            { "Jester.Systems.ChaffMode.Multiple", "WSO Chaff Mode Multiple" },
            { "Jester.Systems.ChaffMode.Program", "WSO Chaff Mode Program" },
            { "Jester.Systems.FlareMode.Off", "WSO Flare Mode Off" },
            { "Jester.Systems.FlareMode.Single", "WSO Flare Mode Single" },
            { "Jester.Systems.FlareMode.Program", "WSO Flare Mode Program" },
            { "Jester.Systems.Jammer.Standby", "WSO Jammer Standby" },
            { "Jester.Systems.Jammer.Transmit", "WSO Jammer Transmit" },
            { "Jester.Systems.AVTR.Record", "WSO AVTR Record" },
            { "Jester.Systems.AVTR.Standby", "WSO AVTR Standby" },
            { "Jester.Systems.AVTR.Off", "WSO AVTR Off" },
            { "Jester.Crew.Presence.Auto", "WSO Crew Auto" },
            { "Jester.Crew.Presence.Force", "WSO Crew Force" },
            { "Jester.Crew.Presence.Disable", "WSO Crew Disable" },
            { "Jester.Crew.Talking.Talk", "WSO Talk" },
            { "Jester.Crew.Talking.Silence", "WSO Silence" },
            { "Jester.Crew.Ejection.WSO", "WSO Eject WSO" },
            { "Jester.Crew.Ejection.Both", "WSO Eject Both" },
            { "Jester.Crew.Countermeasures.Manual", "WSO Countermeasures Manual" },
            { "Jester.Crew.Countermeasures.Jester", "WSO Countermeasures Jester" },
        };
    }
}