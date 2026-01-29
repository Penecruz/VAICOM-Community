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
            { "Report Speed", "Jester.ReportSpeed" },
            { "Toggle Radar", "Jester.ToggleRadar" },
            { "Set Chaff Mode", "Jester.SetChaffMode" },
            { "Lantirn Designate", "Jester.Lantirn.Designate" },
            { "Lantirn Undesignate", "Jester.Lantirn.Undesignate" },
            { "Go Radar Silent", "Jester.Radar.GoSilent" },
            { "Go Radar Active", "Jester.Radar.GoActive" },
            { "Set Manual Frequency", "Jester.Radio.SetManualFrequency" },
            { "Select Comm Channel", "Jester.Radio.SelectCommChannel" },
            { "Select Aux Channel", "Jester.Radio.SelectAuxChannel" },
            { "Tune ATC", "Jester.Radio.TuneATC" },
            { "Tune Assets", "Jester.Radio.TuneAssets" },
            { "Select Mode", "Jester.Radio.SelectMode" },
            { "Radar Active", "Jester.Radar.Operation.Active" },
            { "Radar Standby", "Jester.Radar.Operation.Standby" },
            { "Auto Focus On", "Jester.Radar.AutoFocus.On" },
            { "Auto Focus Off", "Jester.Radar.AutoFocus.Off" },
            { "IFF Both", "Jester.Radar.IFF.Both" },
            { "IFF APX76", "Jester.Radar.IFF.APX76" },
            { "Enter Boresight", "Jester.Radar.Boresight.EnterBST" },
            { "Boresight Wide", "Jester.Radar.Boresight.Wide" },
            { "Boresight Nose", "Jester.Radar.Boresight.Nose" },
            { "Boresight Forward", "Jester.Radar.Boresight.Forward" },
            { "Boresight Aft", "Jester.Radar.Boresight.Aft" },
            { "Boresight Tail", "Jester.Radar.Boresight.Tail" },
            { "Radar Scan Center", "Jester.Radar.ScanElevation.Center" },
            { "Radar Scan Low", "Jester.Radar.ScanElevation.Low" },
            { "Radar Scan Mid Low", "Jester.Radar.ScanElevation.MidLow" },
            { "Radar Scan Far Low", "Jester.Radar.ScanElevation.FarLow" },
            { "Radar Scan Close High", "Jester.Radar.ScanElevation.CloseHigh" },
            { "Radar Scan Mid High", "Jester.Radar.ScanElevation.MidHigh" },
            { "Radar Scan Far High", "Jester.Radar.ScanElevation.FarHigh" },
            { "Radar Scan 25 Wide", "Jester.Radar.ScanType.25Wide" },
            { "Radar Scan 25 Narrow", "Jester.Radar.ScanType.25Narrow" },
            { "Radar Scan 50 Wide", "Jester.Radar.ScanType.50Wide" },
            { "Radar Scan 50 Narrow", "Jester.Radar.ScanType.50Narrow" },
            { "Focus Target", "Jester.Radar.FocusTarget" },
            { "Lock Target", "Jester.Radar.LockTarget" },
            { "TV Weapons", "Jester.A2G.TVWeapons" },
            { "TV Pave Spike", "Jester.A2G.TVPaveSpike" },
            { "Go To Resume", "Jester.Navigation.GoToResume" },
            { "Hold Current Turn Point", "Jester.Navigation.Holding.CurrentTurnPoint" },
            { "Divert Lat Long", "Jester.Navigation.Divert.LatLong" },
            { "TACAN Off", "Jester.Navigation.TACAN.SelectMode.Off" },
            { "TACAN Recieve", "Jester.Navigation.TACAN.SelectMode.R" },
            { "TACAN Transmit and Recieve", "Jester.Navigation.TACAN.SelectMode.TR" },
            { "TACAN Air refuel", "Jester.Navigation.TACAN.SelectMode.AAR" },
            { "TACAN Air to Air", "Jester.Navigation.TACAN.SelectMode.AATR" },
            { "Chaff Mode Off", "Jester.Systems.ChaffMode.Off" },
            { "Chaff Mode Single", "Jester.Systems.ChaffMode.Single" },
            { "Chaff Mode Multiple", "Jester.Systems.ChaffMode.Multiple" },
            { "Chaff Mode Program", "Jester.Systems.ChaffMode.Program" },
            { "Flare Mode Off", "Jester.Systems.FlareMode.Off" },
            { "Flare Mode Single", "Jester.Systems.FlareMode.Single" },
            { "Flare Mode Program", "Jester.Systems.FlareMode.Program" },
            { "Jammer To Standby", "Jester.Systems.Jammer.Standby" },
            { "Jammer To Transmit", "Jester.Systems.Jammer.Transmit" },
            { "AVTR Record", "Jester.Systems.AVTR.Record" },
            { "AVTR Standby", "Jester.Systems.AVTR.Standby" },
            { "AVTR Off", "Jester.Systems.AVTR.Off" },
            { "Crew Auto", "Jester.Crew.Presence.Auto" },
            { "Crew Force", "Jester.Crew.Presence.Force" },
            { "Crew Disable", "Jester.Crew.Presence.Disable" },
            { "Talk", "Jester.Crew.Talking.Talk" },
            { "Silence", "Jester.Crew.Talking.Silence" },
            { "Eject WSO", "Jester.Crew.Ejection.WSO" },
            { "Eject Both", "Jester.Crew.Ejection.Both" },
            { "Countermeasures Mine", "Jester.Crew.Countermeasures.Manual" },
            { "Countermeasures Yours", "Jester.Crew.Countermeasures.Jester" },
        };
    }
}