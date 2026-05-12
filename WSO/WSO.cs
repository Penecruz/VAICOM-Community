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
        public bool enabled;

        public RecipientInfo()
        {
            enabled = false;
        }
    }

    public class CommandInfo : BaseCommandInfo
    {
        internal object category;

        public CommandInfo()
        {
            eventnumber = 4000; // Default event number for WSO commands
        }
    }

    public static partial class Recipients
    {
        public static Dictionary<string, RecipientInfo> aicomms = new Dictionary<string, RecipientInfo>(StringComparer.OrdinalIgnoreCase)
        {
            { "WSO", new RecipientInfo { uniqueid = 19501, name = "wAIUnitFlightCrewMembersWSO", displayname = Labels.airecipients["WSO"], enabled = true } },
        };
    }
}