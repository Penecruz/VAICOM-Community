using System;
using System.Collections.Generic;

namespace VAICOM.Extensions.RIO
{

    public class AuxData
    {

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

    public class CommandInfo
    {
        public int uniqueid;
        public string name;
        public string displayname;
        public int eventnumber;
        public bool enabled;

        public CommandInfo()
        {
            eventnumber = 4000;
            enabled = false;
        }
    }

    // get added to recipients.all
    public static partial class Recipients
    {
        public static Dictionary<string, RecipientInfo> aicomms = new Dictionary<string, RecipientInfo>(StringComparer.OrdinalIgnoreCase)
        {
            { "RIO",    new RecipientInfo { uniqueid = 19301, name = "wAIUnitFlightCrewMembersRIO",     displayname = Labels.airecipients["RIO"], enabled = true } },
            { "Iceman", new RecipientInfo { uniqueid = 19302, name = "wAIUnitFlightCrewMembersIceman",  displayname = Labels.airecipients["Iceman"], enabled = true } },
        };
    }





}
