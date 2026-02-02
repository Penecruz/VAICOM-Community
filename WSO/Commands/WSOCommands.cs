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
            { "Jester.ReportSpeed", new CommandInfo { uniqueid = 24600, category = CommandCategories.WSO, name = "Jester.ReportSpeed", displayname = "Jester.Report.Speed", enabled = true } },
            { "Jester.ToggleRadar", new CommandInfo { uniqueid = 24100, category = CommandCategories.WSO, name = "Jester.ToggleRadar", displayname = "Jester.Toggle.Radar", enabled = true } },
            { "Jester.SetChaffMode", new CommandInfo { uniqueid = 24500, category = CommandCategories.WSO, name = "Jester.SetChaffMode", displayname = "Jester.Set.Chaff.Mode", enabled = true } },
            { "Jester.Lantirn.Designate", new CommandInfo { uniqueid = 24004, category = CommandCategories.WSO, name = "Jester.Lantirn.Designate", displayname = "Jester.Lantirn.Designate", enabled = true } },
            { "Jester.Lantirn.Undesignate", new CommandInfo { uniqueid = 24005, category = CommandCategories.WSO, name = "Jester.Lantirn.Undesignate", displayname = "Jester.Lantirn.Undesignate", enabled = true } },
            { "Jester.Radar.GoSilent", new CommandInfo { uniqueid = 24006, category = CommandCategories.WSO, name = "Jester.Radar.GoSilent", displayname = "Jester.Radar.GoSilent", enabled = true } },
            { "Jester.Radar.GoActive", new CommandInfo { uniqueid = 24007, category = CommandCategories.WSO, name = "Jester.Radar.GoActive", displayname = "Jester.Radar.GoActive", enabled = true } },
            { "Jester.Radio.SetManualFrequency", new CommandInfo { uniqueid = 24008, category = CommandCategories.WSO, name = "Jester.Radio.SetManualFrequency", displayname = "Jester.Radio.SetManualFrequency", enabled = true } },
            { "Jester.Radio.SelectCommChannel", new CommandInfo { uniqueid = 24009, category = CommandCategories.WSO, name = "Jester.Radio.SelectCommChannel", displayname = "Jester.Radio.SelectCommChannel", enabled = true } },
            { "Jester.Radio.SelectAuxChannel", new CommandInfo { uniqueid = 24010, category = CommandCategories.WSO, name = "Jester.Radio.SelectAuxChannel", displayname = "Jester.Radio.SelectAuxChannel", enabled = true } },
            { "Jester.Radio.TuneATC", new CommandInfo { uniqueid = 24300, category = CommandCategories.WSO, name = "Jester.Radio.TuneATC", displayname = "Jester.Radio.TuneATC", enabled = true } },
            { "Jester.Radio.TuneAssets", new CommandInfo { uniqueid = 24301, category = CommandCategories.WSO, name = "Jester.Radio.TuneAssets", displayname = "Jester.Radio.TuneAssets", enabled = true } },
            { "Jester.Radio.SelectMode", new CommandInfo { uniqueid = 24302, category = CommandCategories.WSO, name = "Jester.Radio.SelectMode", displayname = "Jester.Radio.SelectMode", enabled = true } },
            { "Jester.Radar.Operation.Active", new CommandInfo { uniqueid = 24014, category = CommandCategories.WSO, name = "Jester.Radar.Operation.Active", displayname = "Jester.Radar.Operation.Active", enabled = true } },
            { "Jester.Radar.Operation.Standby", new CommandInfo { uniqueid = 24015, category = CommandCategories.WSO, name = "Jester.Radar.Operation.Standby", displayname = "Jester.Radar.Operation.Standby", enabled = true } },
            { "Jester.Radar.AutoFocus.On", new CommandInfo { uniqueid = 24101, category = CommandCategories.WSO, name = "Jester.Radar.AutoFocus.On", displayname = "Jester.Radar.AutoFocus.On", enabled = true } },
            { "Jester.Radar.AutoFocus.Off", new CommandInfo { uniqueid = 24102, category = CommandCategories.WSO, name = "Jester.Radar.AutoFocus.Off", displayname = "Jester.Radar.AutoFocus.Off", enabled = true } },
            { "Jester.Radar.IFF.Both", new CommandInfo { uniqueid = 24103, category = CommandCategories.WSO, name = "Jester.Radar.IFF.Both", displayname = "Jester.Radar.IFF.Both", enabled = true } },
            { "Jester.Radar.IFF.APX76", new CommandInfo { uniqueid = 24104, category = CommandCategories.WSO, name = "Jester.Radar.IFF.APX76", displayname = "Jester.Radar.IFF.APX76", enabled = true } },
            { "Jester.Radar.Boresight.EnterBST", new CommandInfo { uniqueid = 24105, category = CommandCategories.WSO, name = "Jester.Radar.Boresight.EnterBST", displayname = "Jester.Radar.Boresight.EnterBST", enabled = true } },
            { "Jester.Radar.Boresight.Wide", new CommandInfo { uniqueid = 24106, category = CommandCategories.WSO, name = "Jester.Radar.Boresight.Wide", displayname = "Jester.Radar.Boresight.Wide", enabled = true } },
            { "Jester.Radar.Boresight.Nose", new CommandInfo { uniqueid = 24107, category = CommandCategories.WSO, name = "Jester.Radar.Boresight.Nose", displayname = "Jester.Radar.Boresight.Nose", enabled = true } },
            { "Jester.Radar.Boresight.Forward", new CommandInfo { uniqueid = 24108, category = CommandCategories.WSO, name = "Jester.Radar.Boresight.Forward", displayname = "Jester.Radar.Boresight.Forward", enabled = true } },
            { "Jester.Radar.Boresight.Aft", new CommandInfo { uniqueid = 24109, category = CommandCategories.WSO, name = "Jester.Radar.Boresight.Aft", displayname = "Jester.Radar.Boresight.Aft", enabled = true } },
            { "Jester.Radar.Boresight.Tail", new CommandInfo { uniqueid = 24110, category = CommandCategories.WSO, name = "Jester.Radar.Boresight.Tail", displayname = "Jester.Radar.Boresight.Tail", enabled = true } },
            { "Jester.Radar.ScanElevation.Center", new CommandInfo { uniqueid = 24111, category = CommandCategories.WSO, name = "Jester.Radar.ScanElevation.Center", displayname = "Jester.Radar.ScanElevation.Center", enabled = true } },
            { "Jester.Radar.ScanElevation.Low", new CommandInfo { uniqueid = 24112, category = CommandCategories.WSO, name = "Jester.Radar.ScanElevation.Low", displayname = "Jester.Radar.ScanElevation.Low", enabled = true } },
            { "Jester.Radar.ScanElevation.MidLow", new CommandInfo { uniqueid = 24113, category = CommandCategories.WSO, name = "Jester.Radar.ScanElevation.MidLow", displayname = "Jester.Radar.ScanElevation.MidLow", enabled = true } },
            { "Jester.Radar.ScanElevation.FarLow", new CommandInfo { uniqueid = 24114, category = CommandCategories.WSO, name = "Jester.Radar.ScanElevation.FarLow", displayname = "Jester.Radar.ScanElevation.FarLow", enabled = true } },
            { "Jester.Radar.ScanElevation.CloseHigh", new CommandInfo { uniqueid = 24115, category = CommandCategories.WSO, name = "Jester.Radar.ScanElevation.CloseHigh", displayname = "Jester.Radar.ScanElevation.CloseHigh", enabled = true } },
            { "Jester.Radar.ScanElevation.MidHigh", new CommandInfo { uniqueid = 24116, category = CommandCategories.WSO, name = "Jester.Radar.ScanElevation.MidHigh", displayname = "Jester.Radar.ScanElevation.MidHigh", enabled = true } },
            { "Jester.Radar.ScanElevation.FarHigh", new CommandInfo { uniqueid = 24117, category = CommandCategories.WSO, name = "Jester.Radar.ScanElevation.FarHigh", displayname = "Jester.Radar.ScanElevation.FarHigh", enabled = true } },
            { "Jester.Radar.ScanType.25Wide", new CommandInfo { uniqueid = 24118, category = CommandCategories.WSO, name = "Jester.Radar.ScanType.25Wide", displayname = "Jester.Radar.ScanType.25Wide", enabled = true } },
            { "Jester.Radar.ScanType.25Narrow", new CommandInfo { uniqueid = 24119, category = CommandCategories.WSO, name = "Jester.Radar.ScanType.25Narrow", displayname = "Jester.Radar.ScanType.25Narrow", enabled = true } },
            { "Jester.Radar.ScanType.50Wide", new CommandInfo { uniqueid = 24120, category = CommandCategories.WSO, name = "Jester.Radar.ScanType.50Wide", displayname = "Jester.Radar.ScanType.50Wide", enabled = true } },
            { "Jester.Radar.ScanType.50Narrow", new CommandInfo { uniqueid = 24121, category = CommandCategories.WSO, name = "Jester.Radar.ScanType.50Narrow", displayname = "Jester.Radar.ScanType.50Narrow", enabled = true } },
            { "Jester.Radar.FocusTarget", new CommandInfo { uniqueid = 24122, category = CommandCategories.WSO, name = "Jester.Radar.FocusTarget", displayname = "Jester.Radar.FocusTarget", enabled = true } },
            { "Jester.Radar.LockTarget", new CommandInfo { uniqueid = 24123, category = CommandCategories.WSO, name = "Jester.Radar.LockTarget", displayname = "Jester.Radar.LockTarget", enabled = true } },
            { "Jester.A2G.TVWeapons", new CommandInfo { uniqueid = 24039, category = CommandCategories.WSO, name = "Jester.A2G.TVWeapons", displayname = "Jester.A2G.TVWeapons", enabled = true } },
            { "Jester.A2G.TVPaveSpike", new CommandInfo { uniqueid = 24040, category = CommandCategories.WSO, name = "Jester.A2G.TVPaveSpike", displayname = "Jester.A2G.TVPaveSpike", enabled = true } },
            { "Jester.Navigation.GoToResume", new CommandInfo { uniqueid = 24041, category = CommandCategories.WSO, name = "Jester.Navigation.GoToResume", displayname = "Jester.Navigation.GoToResume", enabled = true } },
            { "Jester.Navigation.Holding.CurrentTurnPoint", new CommandInfo { uniqueid = 24042, category = CommandCategories.WSO, name = "Jester.Navigation.Holding.CurrentTurnPoint", displayname = "Jester.Navigation.Holding.CurrentTurnPoint", enabled = true } },
            { "Jester.Navigation.Divert.LatLong", new CommandInfo { uniqueid = 24043, category = CommandCategories.WSO, name = "Jester.Navigation.Divert.LatLong", displayname = "Jester.Navigation.Divert.LatLong", enabled = true } },
            { "Jester.Navigation.TACAN.SelectMode.Off", new CommandInfo { uniqueid = 24044, category = CommandCategories.WSO, name = "Jester.Navigation.TACAN.SelectMode.Off", displayname = "Jester.Navigation.TACAN.SelectMode.Off", enabled = true } },
            { "Jester.Navigation.TACAN.SelectMode.R", new CommandInfo { uniqueid = 24045, category = CommandCategories.WSO, name = "Jester.Navigation.TACAN.SelectMode.R", displayname = "Jester.Navigation.TACAN.SelectMode.R", enabled = true } },
            { "Jester.Navigation.TACAN.SelectMode.TR", new CommandInfo { uniqueid = 24046, category = CommandCategories.WSO, name = "Jester.Navigation.TACAN.SelectMode.TR", displayname = "Jester.Navigation.TACAN.SelectMode.TR", enabled = true } },
            { "Jester.Navigation.TACAN.SelectMode.AAR", new CommandInfo { uniqueid = 24047, category = CommandCategories.WSO, name = "Jester.Navigation.TACAN.SelectMode.AAR", displayname = "Jester.Navigation.TACAN.SelectMode.AAR", enabled = true } },
            { "Jester.Navigation.TACAN.SelectMode.AATR", new CommandInfo { uniqueid = 24048, category = CommandCategories.WSO, name = "Jester.Navigation.TACAN.SelectMode.AATR", displayname = "Jester.Navigation.TACAN.SelectMode.AATR", enabled = true } },
            { "Jester.Systems.ChaffMode.Off", new CommandInfo { uniqueid = 24501, category = CommandCategories.WSO, name = "Jester.Systems.ChaffMode.Off", displayname = "Jester.Systems.ChaffMode.Off", enabled = true } },
            { "Jester.Systems.ChaffMode.Single", new CommandInfo { uniqueid = 24502, category = CommandCategories.WSO, name = "Jester.Systems.ChaffMode.Single", displayname = "Jester.Systems.ChaffMode.Single", enabled = true } },
            { "Jester.Systems.ChaffMode.Multiple", new CommandInfo { uniqueid = 24503, category = CommandCategories.WSO, name = "Jester.Systems.ChaffMode.Multiple", displayname = "Jester.Systems.ChaffMode.Multiple", enabled = true } },
            { "Jester.Systems.ChaffMode.Program", new CommandInfo { uniqueid = 24504, category = CommandCategories.WSO, name = "Jester.Systems.ChaffMode.Program", displayname = "Jester.Systems.ChaffMode.Program", enabled = true } },
            { "Jester.Systems.FlareMode.Off", new CommandInfo { uniqueid = 24505, category = CommandCategories.WSO, name = "Jester.Systems.FlareMode.Off", displayname = "Jester.Systems.FlareMode.Off", enabled = true } },
            { "Jester.Systems.FlareMode.Single", new CommandInfo { uniqueid = 24506, category = CommandCategories.WSO, name = "Jester.Systems.FlareMode.Single", displayname = "Jester.Systems.FlareMode.Single", enabled = true } },
            { "Jester.Systems.FlareMode.Program", new CommandInfo { uniqueid = 24507, category = CommandCategories.WSO, name = "Jester.Systems.FlareMode.Program", displayname = "Jester.Systems.FlareMode.Program", enabled = true } },
            { "Jester.Systems.Jammer.Standby", new CommandInfo { uniqueid = 24508, category = CommandCategories.WSO, name = "Jester.Systems.Jammer.Standby", displayname = "Jester.Systems.Jammer.Standby", enabled = true } },
            { "Jester.Systems.Jammer.Transmit", new CommandInfo { uniqueid = 24509, category = CommandCategories.WSO, name = "Jester.Systems.Jammer.Transmit", displayname = "Jester.Systems.Jammer.Transmit", enabled = true } },
            { "Jester.Systems.AVTR.Record", new CommandInfo { uniqueid = 24601, category = CommandCategories.WSO, name = "Jester.Systems.AVTR.Record", displayname = "Jester.Systems.AVTR.Record", enabled = true } },
            { "Jester.Systems.AVTR.Standby", new CommandInfo { uniqueid = 24602, category = CommandCategories.WSO, name = "Jester.Systems.AVTR.Standby", displayname = "Jester.Systems.AVTR.Standby", enabled = true } },
            { "Jester.Systems.AVTR.Off", new CommandInfo { uniqueid = 24603, category = CommandCategories.WSO, name = "Jester.Systems.AVTR.Off", displayname = "Jester.Systems.AVTR.Off", enabled = true } },
            { "Jester.Crew.Presence.Auto", new CommandInfo { uniqueid = 24061, category = CommandCategories.WSO, name = "Jester.Crew.Presence.Auto", displayname = "Jester.Crew.Presence.Auto", enabled = true } },
            { "Jester.Crew.Presence.Force", new CommandInfo { uniqueid = 24062, category = CommandCategories.WSO, name = "Jester.Crew.Presence.Force", displayname = "Jester.Crew.Presence.Force", enabled = true } },
            { "Jester.Crew.Presence.Disable", new CommandInfo { uniqueid = 24063, category = CommandCategories.WSO, name = "Jester.Crew.Presence.Disable", displayname = "Jester.Crew.Presence.Disable", enabled = true } },
            { "Jester.Crew.Talking.Talk", new CommandInfo { uniqueid = 24064, category = CommandCategories.WSO, name = "Jester.Crew.Talking.Talk", displayname = "Jester.Crew.Talking.Talk", enabled = true } },
            { "Jester.Crew.Talking.Silence", new CommandInfo { uniqueid = 24065, category = CommandCategories.WSO, name = "Jester.Crew.Talking.Silence", displayname = "Jester.Crew.Talking.Silence", enabled = true } },
            { "Jester.Crew.Ejection.WSO", new CommandInfo { uniqueid = 24066, category = CommandCategories.WSO, name = "Jester.Crew.Ejection.WSO", displayname = "Jester.Crew.Ejection.WSO", enabled = true } },
            { "Jester.Crew.Ejection.Both", new CommandInfo { uniqueid = 24067, category = CommandCategories.WSO, name = "Jester.Crew.Ejection.Both", displayname = "Jester.Crew.Ejection.Both", enabled = true } },
            { "Jester.Crew.Countermeasures.Manual", new CommandInfo { uniqueid = 24510, category = CommandCategories.WSO, name = "Jester.Crew.Countermeasures.Manual", displayname = "Jester.Crew.Countermeasures.Manual", enabled = true } },
            { "Jester.Crew.Countermeasures.Jester", new CommandInfo { uniqueid = 24511, category = CommandCategories.WSO, name = "Jester.Crew.Countermeasures.Jester", displayname = "Jester.Crew.Countermeasures.Jester", enabled = true } },
            // Additional WSO commands can be added here within the reserved range (24000 - 24998)            
         };

    }    
}
