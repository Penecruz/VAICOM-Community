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
            // Add more WSO commands as needed
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