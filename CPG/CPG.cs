using System;
using System.Collections.Generic;
using VAICOM.Shared;

namespace VAICOM.Extensions.CPG
{
    public class AuxData
    {
        // Placeholder for auxiliary data related to CPG
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
            { "george", new RecipientInfo { uniqueid = 19601, name = "wAIUnitFlightCrewMembersGeorgeCPG", displayname = Labels.airecipients["george"], enabled = true } },
        };
    }
}