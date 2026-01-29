using System;
using System.Collections.Generic;
using VAICOM.Shared;

namespace VAICOM.Extensions.WSO
{
    public class AuxData
    {
        // Placeholder for auxiliary data related to WSO
    }

    public class RecipientInfo
    {
        public int uniqueid;
        public string name;
        public string displayname;
        public bool requiresWSO;
        public bool enabled;
        public bool blockedforFree;
        public bool requiresJester;

        public RecipientInfo()
        {
            requiresWSO = true;
            enabled = false;
            blockedforFree = true;
        }
    }

    public class CommandInfo : BaseCommandInfo
    {
        public bool requiresWSO;
        public bool requiresJester;
        internal object category;

        public CommandInfo()
        {
            eventnumber = 5000; // Default event number for WSO commands
            requiresWSO = true;
        }
    }

    public static partial class Recipients
    {
        public static Dictionary<string, RecipientInfo> aicomms = new Dictionary<string, RecipientInfo>(StringComparer.OrdinalIgnoreCase)
        {
            { "WSO", new RecipientInfo { uniqueid = 19501, name = "wAIUnitFlightCrewMembersWSO", displayname = Labels.airecipients["WSO"], requiresWSO = true, enabled = true } },
        };
    }
}