namespace VAICOM.Extensions.RIO
{

    public class DeviceAction
    {
        public int device;
        public int command;
        public double value;
        public int delayMs;
    }

    public static partial class DeviceActionsLibrary
    {

        public static class Devices
        {
            public static int ICS = 2;
            public static int ARC159 = 3;
            public static int ARC182 = 4;
            public static int COUNTERMEASURES = 5; //hardcoded for the ground crew menu options (countermeasures loadout)
            public static int COCKPITMECHANICS = 12; // now 12
            public static int LANTIRN = 7; // <-- now 7
            public static int RADAR = 39;
            public static int TID = 43;
            public static int TACAN = 47;
            public static int DATALINK = 52;
            public static int DECM = 53;
            public static int WALKMAN = 56; // 
            public static int KNEEBOARD = 61; //
            public static int RIO = 62; // JesterAI from devices.lua
        }

        public static class Aux
        {
        }

    }

}
