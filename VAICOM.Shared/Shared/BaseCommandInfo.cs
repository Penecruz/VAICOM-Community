using System;

namespace VAICOM.Shared
{
    public class BaseCommandInfo
    {
        public int uniqueid;
        public string name;
        public string displayname;
        public int eventnumber;
        public bool enabled;
        public bool blockedforFree;

        public BaseCommandInfo()
        {
            eventnumber = 4000; // Default event number
            enabled = false;
            blockedforFree = true;
        }
    }
}