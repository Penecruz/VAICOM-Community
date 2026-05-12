namespace VAICOM.Extensions.RIO
{

    public static partial class DeviceActionsLibrary
    {
        // base command pool : atomary building blocks
        public static class RIO
        {
            public static int start = 3000;

            public static DeviceAction Atom_J_VOID = new DeviceAction();


            // test
            public static DeviceAction Atom_J_CANOPY_OPEN = new DeviceAction() { device = Devices.COCKPITMECHANICS, command = 3182, value = 1 }; //CanopySwitchOpen
            public static DeviceAction Atom_J_CANOPY_CLOSE = new DeviceAction() { device = Devices.COCKPITMECHANICS, command = 3182, value = -1 };

            //... unitary actions start at 3000,..
            // blocks: menu / radar / weapons / radio / utility / defensive

            // block: menu control
            public static DeviceAction Atom_J_MENU_TOGGLE = new DeviceAction() { device = Devices.RIO, command = 3550, value = 1 };
            public static DeviceAction Atom_J_MENU_OPTION_1 = new DeviceAction() { device = Devices.RIO, command = 3551, value = 1 };
            public static DeviceAction Atom_J_MENU_OPTION_2 = new DeviceAction() { device = Devices.RIO, command = 3552, value = 1 };
            public static DeviceAction Atom_J_MENU_OPTION_3 = new DeviceAction() { device = Devices.RIO, command = 3553, value = 1 };
            public static DeviceAction Atom_J_MENU_OPTION_4 = new DeviceAction() { device = Devices.RIO, command = 3554, value = 1 };
            public static DeviceAction Atom_J_MENU_OPTION_5 = new DeviceAction() { device = Devices.RIO, command = 3555, value = 1 };
            public static DeviceAction Atom_J_MENU_OPTION_6 = new DeviceAction() { device = Devices.RIO, command = 3556, value = 1 };
            public static DeviceAction Atom_J_MENU_OPTION_7 = new DeviceAction() { device = Devices.RIO, command = 3557, value = 1 };
            public static DeviceAction Atom_J_MENU_OPTION_8 = new DeviceAction() { device = Devices.RIO, command = 3558, value = 1 };
            public static DeviceAction Atom_J_MENU_DIR_D = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_MENU_DIR_DL = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_MENU_DIR_DR = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_MENU_DIR_L = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_MENU_DIR_R = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_MENU_DIR_U = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_MENU_DIR_UL = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_MENU_DIR_UR = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_MENU_OPEN = new DeviceAction() { device = Devices.RIO, command = 3550, value = -1 }; // 3550
            public static DeviceAction Atom_J_MENU_CLOSE = new DeviceAction() { device = Devices.RIO, command = 3725, value = 1 }; // 3725
            public static DeviceAction Atom_J_MENU_CONTEXT = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_MENU_MAIN = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_MENU_CTXT_CLOSE = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_MENU_MAIN_CLOSE = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };

            public static DeviceAction Atom_JESTER_LANTIRN_inhibit_auto_designate = new DeviceAction() { device = Devices.RIO, command = 3761, value = 1 }; // JESTER_LANTIRN_inhibit_auto_designate
            public static DeviceAction Atom_JESTER_LANTIRN_track_target_id = new DeviceAction() { device = Devices.RIO, command = 3762, value = -1 }; // JESTER_LANTIRN_track_target_id 
            public static DeviceAction Atom_JESTER_LANTIRN_track_zone_id = new DeviceAction() { device = Devices.RIO, command = 3763, value = 0.5 }; // JESTER_LANTIRN_track_zone_id
            public static DeviceAction Atom_JESTER_LANTIRN_designate = new DeviceAction() { device = Devices.LANTIRN, command = 3764, value = 1 }; // JESTER_LANTIRN_designate
            //public static DeviceAction Atom_KNEEBOARD_Laser100                      = new DeviceAction() { device = Devices.KNEEBOARD, command = 3593, value = 1 }; // KNEEBOARD_Laser100
            //public static DeviceAction Atom_KNEEBOARD_Laser10                       = new DeviceAction() { device = Devices.KNEEBOARD, command = 3594, value = 1 }; // KNEEBOARD_Laser10
            //public static DeviceAction Atom_KNEEBOARD_Laser1                        = new DeviceAction() { device = Devices.KNEEBOARD, command = 3595, value = 1 }; // KNEEBOARD_Laser1
            // end of menu control

            // spare block (not used)
            public static DeviceAction Atom_LANTIRN_ToggleWHOTBHOT = new DeviceAction() { device = Devices.LANTIRN, command = 3513, value = 1 }; // LANTIRN_ToggleWHOTBHOT
            public static DeviceAction Atom_LANTIRN_LaserLatched = new DeviceAction() { device = Devices.LANTIRN, command = 3503, value = 1 }; // LANTIRN_LaserLatched
            public static DeviceAction Atom_LANTIRN_Laser_ARM = new DeviceAction() { device = Devices.LANTIRN, command = 3516, value = 1 }; // LANTIRN_Laser_ARM
            public static DeviceAction Atom_LANTIRN_Laser_ARM_Toggle = new DeviceAction() { device = Devices.LANTIRN, command = 3517, value = -1 }; // LANTIRN_Laser_ARM_Toggle
            public static DeviceAction Atom_LANTIRN_Undesignate = new DeviceAction() { device = Devices.LANTIRN, command = 3504, value = 1 }; // LANTIRN_Undesignate
            public static DeviceAction Atom_LANTIRN_QHUD_QADL = new DeviceAction() { device = Devices.LANTIRN, command = 3507, value = 1 }; // LANTIRN_QHUD_QADL
            public static DeviceAction Atom_LANTIRN_QSNO = new DeviceAction() { device = Devices.LANTIRN, command = 3509, value = 1 }; // LANTIRN_QSNO
            public static DeviceAction Atom_LANTIRN_QDES = new DeviceAction() { device = Devices.LANTIRN, command = 3508, value = 1 }; // LANTIRN_QDES
            public static DeviceAction Atom_LANTIRN_QWP_Minus = new DeviceAction() { device = Devices.LANTIRN, command = 3510, value = 1 }; // LANTIRN_QWP_Minus
            public static DeviceAction Atom_LANTIRN_QWP_Plus = new DeviceAction() { device = Devices.LANTIRN, command = 3511, value = 1 }; // LANTIRN_QWP_Plus

            // block: radar (DONE!)
            public static DeviceAction Atom_J_RDR_GO_SILENT = new DeviceAction() { device = Devices.RIO, command = 3564, value = 1 }; // JESTER_Quiet
            public static DeviceAction Atom_J_RDR_SPOT = new DeviceAction() { device = Devices.RIO, command = 3563, value = 1 }; // JESTER_Spot
            public static DeviceAction Atom_J_RDR_BREAK_LOCK = new DeviceAction() { device = Devices.RIO, command = 3577, value = 1 }; // JESTER_BreakLock
            public static DeviceAction Atom_J_RDR_TO_PSTT = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RDR_SCAN_ELEV = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RDR_SCAN_AZ = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RDR_SCAN_DIST = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RDR_TOGGLE_STT = new DeviceAction() { device = Devices.RIO, command = 3576, value = 1 }; //JESTER_TogglePDorPStt
            public static DeviceAction Atom_J_RDR_VSL_HIGH = new DeviceAction() { device = Devices.RIO, command = 3574, value = 1 }; //JESTER_VSL_High
            public static DeviceAction Atom_J_RDR_VSL_LOW = new DeviceAction() { device = Devices.RIO, command = 3575, value = 1 }; //JESTER_VSL_Low
            public static DeviceAction Atom_J_RDR_STT_TGT_AHEAD = new DeviceAction() { device = Devices.RIO, command = 3584, value = 1 }; //JESTER_Lock_Ahead
            public static DeviceAction Atom_J_RDR_STT_ENMY_TGT_AHEAD = new DeviceAction() { device = Devices.RIO, command = 3585, value = 1 }; //JESTER_Lock_Ahead_Enemy
            public static DeviceAction Atom_J_RDR_STT_FRNDLY_TGT_AHEAD = new DeviceAction() { device = Devices.RIO, command = 3586, value = 1 }; //JESTER_Lock_Ahead_Friendly
            public static DeviceAction Atom_J_RDR_STT_CHOOSE_SPECIFIC_TGT = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RDR_STT_FIRST_TWS_TGT = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RDR_STT_TWS_TGT_NUM = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RDR_BVR = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RDR_GO_ACTIVE = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RDR_STT_LOCK = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RDR_AUTO = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RDR_RNG_100 = new DeviceAction() { device = Devices.RIO, command = 3581, value = 1 }; //JESTER_Range_100
            public static DeviceAction Atom_J_RDR_RNG_200 = new DeviceAction() { device = Devices.RIO, command = 3582, value = 1 }; //JESTER_Range_200
            public static DeviceAction Atom_J_RDR_RNG_400 = new DeviceAction() { device = Devices.RIO, command = 3583, value = 1 }; //JESTER_Range_400
            public static DeviceAction Atom_J_RDR_POS = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RDR_POS_CTR = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RDR_POS_CTR_L = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RDR_POS_CTR_R = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RDR_POS_L = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RDR_POS_R = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RDR_POS_HI = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RDR_POS_LO = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RDR_POS_MID = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RDR_POS_MID_HI = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RDR_POS_MID_LO = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RDR_STT_TWS_TGT_1 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RDR_STT_TWS_TGT_2 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RDR_STT_TWS_TGT_3 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RDR_STT_TWS_TGT_4 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RDR_STT_TWS_TGT_5 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RDR_STT_TWS_TGT_6 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RDR_STT_TWS_TGT_7 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RDR_STT_TWS_TGT_8 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RDR_RNG_25 = new DeviceAction() { device = Devices.RIO, command = 3579, value = 1 }; //JESTER_Range_25
            public static DeviceAction Atom_J_RDR_RNG_50 = new DeviceAction() { device = Devices.RIO, command = 3580, value = 1 }; //JESTER_Range_50
            public static DeviceAction Atom_J_RDR_RNG_AUTO = new DeviceAction() { device = Devices.RIO, command = 3578, value = 1 }; //JESTER_Range_Auto


            public static DeviceAction Atom_J_RDR_MODE_AUTO = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RDR_MODE_TWS = new DeviceAction() { device = Devices.RIO, command = 3589, value = 1 }; // JESTER_Radar_TWS
            public static DeviceAction Atom_J_RDR_MODE_RWS = new DeviceAction() { device = Devices.RIO, command = 3590, value = 1 }; //JESTER_Radar_RWS
            public static DeviceAction Atom_J_RDR_MODE = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            // end of radar

            // spare block (not used)
            public static DeviceAction Atom_J_RDR_STAB = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RDR_STAB_15 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RDR_STAB_30 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RDR_STAB_60 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RDR_STAB_120 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RDR_STAB_INDEF = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RDR_STAB_GROUND = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };

            public static DeviceAction Atom_LANTIRN_Head_Eyeball = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_LANTIRN_Head_Head = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder098 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder099 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };

            // block: weapons (DONE!)

            public static DeviceAction Atom_J_WPN_AG_SORDN = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AG_SORDN_WPN_1 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AG_SORDN_WPN_2 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AG_SORDN_WPN_3 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AG_SORDN_WPN_4 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AG_SORDN_WPN_5 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AG_SORDN_WPN_6 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AG_SORDN_WPN_7 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AG_SORDN_WPN_8 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AG_SPOT = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AG_SET_COMP_TGT = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AG_SET_PAIRS = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AG_SETFUSE = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AG_SETFUSE_NOSETAIL = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AG_SETFUSE_NOSE = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AG_SETFUSE_SAFE = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AG_RIP_QTY = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AG_RIP_QTY_STEP = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AG_RIP_QTY_2 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AG_RIP_QTY_5 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AG_RIP_QTY_10 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AG_RIP_QTY_20 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AG_RIP_QTY_30 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AG_RIP_TIME = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AG_RIP_TIME_STEP = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AG_RIP_TIME_10 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AG_RIP_TIME_20 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AG_RIP_TIME_50 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AG_RIP_TIME_100 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AG_RIP_TIME_200 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AG_RIP_TIME_500 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AG_RIP_TIME_990 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AG_RIP_DIST = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AG_RIP_DIST_STEP = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AG = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AA = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AG_RIP = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AG_JETT = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AG_RIP_DIST_5 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AG_RIP_DIST_10 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AG_RIP_DIST_25 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AG_RIP_DIST_50 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AG_RIP_DIST_100 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AG_RIP_DIST_200 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AG_RIP_DIST_400 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AG_UTIL_LANTIRN = new DeviceAction() { device = Devices.TID, command = 3498, value = 1 }; //LANTIRN_Vidoe toggle
            public static DeviceAction Atom_J_WPN_AG_SORDN_WPN_9 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AG_SET_SNGL = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AG_SET_COMP_PILOT = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AG_DROP_TANKS = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };

            public static DeviceAction Atom_J_WPN_AG_STN = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AG_STN_18 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AG_STN_27 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AG_STN_3456 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AG_STN_36 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AG_STN_45 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };

            public static DeviceAction Atom_J_RAD_DL_MAJ1_0 = new DeviceAction() { device = Devices.DATALINK, command = 3599, value = 0.0 };
            public static DeviceAction Atom_J_RAD_DL_MAJ1_1 = new DeviceAction() { device = Devices.DATALINK, command = 3599, value = 0.1 };
            public static DeviceAction Atom_J_RAD_DL_MAJ1_2 = new DeviceAction() { device = Devices.DATALINK, command = 3599, value = 0.2 };
            public static DeviceAction Atom_J_RAD_DL_MAJ1_3 = new DeviceAction() { device = Devices.DATALINK, command = 3599, value = 0.3 };
            public static DeviceAction Atom_J_RAD_DL_MAJ1_4 = new DeviceAction() { device = Devices.DATALINK, command = 3599, value = 0.5 };
            public static DeviceAction Atom_J_RAD_DL_MAJ1_5 = new DeviceAction() { device = Devices.DATALINK, command = 3599, value = 0.6 };
            public static DeviceAction Atom_J_RAD_DL_MAJ1_6 = new DeviceAction() { device = Devices.DATALINK, command = 3599, value = 0.7 };
            public static DeviceAction Atom_J_RAD_DL_MAJ1_7 = new DeviceAction() { device = Devices.DATALINK, command = 3599, value = 0.8 };
            public static DeviceAction Atom_J_RAD_DL_MAJ1_8 = new DeviceAction() { device = Devices.DATALINK, command = 3599, value = 0.9 };
            public static DeviceAction Atom_J_RAD_DL_MAJ1_9 = new DeviceAction() { device = Devices.DATALINK, command = 3599, value = 1.0 };
            public static DeviceAction Atom_J_RAD_DL_MAJ2_0 = new DeviceAction() { device = Devices.DATALINK, command = 3600, value = 0.0 };
            public static DeviceAction Atom_J_RAD_DL_MAJ2_1 = new DeviceAction() { device = Devices.DATALINK, command = 3600, value = 0.1 };
            public static DeviceAction Atom_J_RAD_DL_MAJ2_2 = new DeviceAction() { device = Devices.DATALINK, command = 3600, value = 0.2 };
            public static DeviceAction Atom_J_RAD_DL_MAJ2_3 = new DeviceAction() { device = Devices.DATALINK, command = 3600, value = 0.3 };
            public static DeviceAction Atom_J_RAD_DL_MAJ2_4 = new DeviceAction() { device = Devices.DATALINK, command = 3600, value = 0.5 };
            public static DeviceAction Atom_J_RAD_DL_MAJ2_5 = new DeviceAction() { device = Devices.DATALINK, command = 3600, value = 0.6 };
            public static DeviceAction Atom_J_RAD_DL_MAJ2_6 = new DeviceAction() { device = Devices.DATALINK, command = 3600, value = 0.7 };
            public static DeviceAction Atom_J_RAD_DL_MAJ2_7 = new DeviceAction() { device = Devices.DATALINK, command = 3600, value = 0.8 };
            public static DeviceAction Atom_J_RAD_DL_MAJ2_8 = new DeviceAction() { device = Devices.DATALINK, command = 3600, value = 0.9 };
            public static DeviceAction Atom_J_RAD_DL_MAJ2_9 = new DeviceAction() { device = Devices.DATALINK, command = 3600, value = 1.0 };
            public static DeviceAction Atom_J_RAD_DL_MIN_0 = new DeviceAction() { device = Devices.DATALINK, command = 3601, value = 0.0 };
            public static DeviceAction Atom_J_RAD_DL_MIN_1 = new DeviceAction() { device = Devices.DATALINK, command = 3601, value = 0.1 };
            public static DeviceAction Atom_J_RAD_DL_MIN_2 = new DeviceAction() { device = Devices.DATALINK, command = 3601, value = 0.2 };
            public static DeviceAction Atom_J_RAD_DL_MIN_3 = new DeviceAction() { device = Devices.DATALINK, command = 3601, value = 0.3 };
            public static DeviceAction Atom_J_RAD_DL_MIN_4 = new DeviceAction() { device = Devices.DATALINK, command = 3601, value = 0.5 };
            public static DeviceAction Atom_J_RAD_DL_MIN_5 = new DeviceAction() { device = Devices.DATALINK, command = 3601, value = 0.6 };
            public static DeviceAction Atom_J_RAD_DL_MIN_6 = new DeviceAction() { device = Devices.DATALINK, command = 3601, value = 0.7 };
            public static DeviceAction Atom_J_RAD_DL_MIN_7 = new DeviceAction() { device = Devices.DATALINK, command = 3601, value = 0.8 };
            public static DeviceAction Atom_J_RAD_DL_MIN_8 = new DeviceAction() { device = Devices.DATALINK, command = 3601, value = 0.9 };
            public static DeviceAction Atom_J_RAD_DL_MIN_9 = new DeviceAction() { device = Devices.DATALINK, command = 3601, value = 1.0 };
            public static DeviceAction Atom_Placeholder185 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_Placeholder186 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_Placeholder187 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AG_RIP_QTY_16 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_WPN_AG_RIP_QTY_28 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            // end of weapons

            // spare block
            public static DeviceAction Atom_J_RAD_TCN_TAC_STEN = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_TCN_TAC_ARCO = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_TCN_TAC_SHEL = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_TCN_TAC_TEXA = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_TCN_TAC_WASH = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_TCN_TAC_ROOS = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_TCN_TAC_LINC = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_TCN_TAC_TRUM = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_TCN_TAC_FORE = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder198 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder199 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };

            // block: radio
            public static DeviceAction Atom_J_RAD_159 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_159_USE_GUARD = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_159_USE_MANUAL = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_159_USE_CHAN = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_159_USE_CHAN_1 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_159_USE_CHAN_2 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_159_USE_CHAN_3 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_159_USE_CHAN_4 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_159_USE_CHAN_5 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_159_USE_CHAN_6 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_159_USE_CHAN_7 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_159_USE_CHAN_8 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_159_TUNE_MAN = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_159_SELECT_CHAN = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_159_SELECT_MODE = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_159_TUNE_ATC = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_159_TUNE_TAC = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_182 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_182_USE_GUARD = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_182_USE_MANUAL = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_182_USE_CHAN = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_182_USE_CHAN_1 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_182_USE_CHAN_2 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_182_USE_CHAN_3 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_182_USE_CHAN_4 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_182_USE_CHAN_5 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_182_USE_CHAN_6 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_182_USE_CHAN_7 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_182_USE_CHAN_8 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_182_TUNE_MAN = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_182_SELECT_CHAN = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_182_SELECT_MODE = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_182_TUNE_ATC = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_182_TUNE_TAC = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };

            public static DeviceAction Atom_J_RAD_DL = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_DL_SET_MODE = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_DL_SET_FREQ_PRST = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_DL_SET_HOST = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_DL_TACT = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_DL_OFF = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_DL_FGHT = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };

            public static DeviceAction Atom_J_RAD_DL_SET_FREQ_1 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_DL_SET_FREQ_2 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_DL_SET_FREQ_3 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_DL_SET_FREQ_4 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_DL_SET_FREQ_5 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_DL_SET_FREQ_6 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_DL_SET_FREQ_7 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_DL_SET_FREQ_8 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };

            public static DeviceAction Atom_J_RAD_TCN = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_TCN_MODE = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_TCN_MODE_OFF = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_TCN_MODE_REC = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_TCN_MODE_TR = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_TCN_MODE_AA = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_TCN_MODE_BCN = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };

            public static DeviceAction Atom_J_RAD_TCN_SEL_CHAN = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_TCN_SEL_GND_STN = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_TCN_TUNE_TAC = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };

            public static DeviceAction Atom_J_RAD_TCN_MAJ_00 = new DeviceAction() { device = Devices.TACAN, command = 3342, value = 0.0 };
            public static DeviceAction Atom_J_RAD_TCN_MAJ_01 = new DeviceAction() { device = Devices.TACAN, command = 3342, value = 0.1 };
            public static DeviceAction Atom_J_RAD_TCN_MAJ_02 = new DeviceAction() { device = Devices.TACAN, command = 3342, value = 0.2 };
            public static DeviceAction Atom_J_RAD_TCN_MAJ_03 = new DeviceAction() { device = Devices.TACAN, command = 3342, value = 0.25 };
            public static DeviceAction Atom_J_RAD_TCN_MAJ_04 = new DeviceAction() { device = Devices.TACAN, command = 3342, value = 0.3 };
            public static DeviceAction Atom_J_RAD_TCN_MAJ_05 = new DeviceAction() { device = Devices.TACAN, command = 3342, value = 0.4 };
            public static DeviceAction Atom_J_RAD_TCN_MAJ_06 = new DeviceAction() { device = Devices.TACAN, command = 3342, value = 0.5 };
            public static DeviceAction Atom_J_RAD_TCN_MAJ_07 = new DeviceAction() { device = Devices.TACAN, command = 3342, value = 0.6 };
            public static DeviceAction Atom_J_RAD_TCN_MAJ_08 = new DeviceAction() { device = Devices.TACAN, command = 3342, value = 0.7 };
            public static DeviceAction Atom_J_RAD_TCN_MAJ_09 = new DeviceAction() { device = Devices.TACAN, command = 3342, value = 0.75 };
            public static DeviceAction Atom_J_RAD_TCN_MAJ_10 = new DeviceAction() { device = Devices.TACAN, command = 3342, value = 0.8 };
            public static DeviceAction Atom_J_RAD_TCN_MAJ_11 = new DeviceAction() { device = Devices.TACAN, command = 3342, value = 0.9 };
            public static DeviceAction Atom_J_RAD_TCN_MAJ_12 = new DeviceAction() { device = Devices.TACAN, command = 3342, value = 1.0 };
            public static DeviceAction Atom_J_RAD_TCN_MIN_0 = new DeviceAction() { device = Devices.TACAN, command = 3344, value = 0.0 };
            public static DeviceAction Atom_J_RAD_TCN_MIN_1 = new DeviceAction() { device = Devices.TACAN, command = 3344, value = 0.1 };
            public static DeviceAction Atom_J_RAD_TCN_MIN_2 = new DeviceAction() { device = Devices.TACAN, command = 3344, value = 0.2 };
            public static DeviceAction Atom_J_RAD_TCN_MIN_3 = new DeviceAction() { device = Devices.TACAN, command = 3344, value = 0.3 };
            public static DeviceAction Atom_J_RAD_TCN_MIN_4 = new DeviceAction() { device = Devices.TACAN, command = 3344, value = 0.5 };
            public static DeviceAction Atom_J_RAD_TCN_MIN_5 = new DeviceAction() { device = Devices.TACAN, command = 3344, value = 0.6 };
            public static DeviceAction Atom_J_RAD_TCN_MIN_6 = new DeviceAction() { device = Devices.TACAN, command = 3344, value = 0.7 };
            public static DeviceAction Atom_J_RAD_TCN_MIN_7 = new DeviceAction() { device = Devices.TACAN, command = 3344, value = 0.8 };
            public static DeviceAction Atom_J_RAD_TCN_MIN_8 = new DeviceAction() { device = Devices.TACAN, command = 3344, value = 0.9 };
            public static DeviceAction Atom_J_RAD_TCN_MIN_9 = new DeviceAction() { device = Devices.TACAN, command = 3344, value = 1.0 };
            public static DeviceAction Atom_J_RAD_TCN_X = new DeviceAction() { device = Devices.TACAN, command = 3348, value = 0 };
            public static DeviceAction Atom_J_RAD_TCN_Y = new DeviceAction() { device = Devices.TACAN, command = 3348, value = 1 };
            public static DeviceAction Atom_J_RAD_TCN_RESET = new DeviceAction() { device = Devices.TACAN, command = start + 0, value = 0 };

            public static DeviceAction Atom_J_RAD_182_UNKNWN = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_182_MODE_TR = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_182_MODE_TRG = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_182_MODE_DF = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_182_MODE_TEST = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            // end of radios

            // spare block
            public static DeviceAction Atom_J_RAD_182_MODE_AM = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_182_MODE_FM = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_182_MODE = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_182_MODE_OFF = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };

            public static DeviceAction Atom_J_RAD_DL_HOST_STEN = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_DL_HOST_DARK = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_DL_HOST_FOCS = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_DL_HOST_MAGC = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_DL_HOST_OVRL = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_DL_HOST_WIZR = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };

            // block: utility /NAV 
            public static DeviceAction Atom_J_UTIL_NAV = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 }; //na
            public static DeviceAction Atom_J_UTIL_NAV_SEL_DEST_SPT = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 }; //na
            public static DeviceAction Atom_J_UTIL_NAV_REST_MSN_SPT = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 }; //na
            public static DeviceAction Atom_J_UTIL_NAV_MAN_ENT_SPT = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 }; //na
            public static DeviceAction Atom_J_UTIL_NAV_MAP_SPT = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_UTIL_NAV_MAP_SPT_1 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_UTIL_NAV_MAP_SPT_2 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_UTIL_NAV_MAP_SPT_3 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_UTIL_NAV_MAP_SPT_4 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 }; // not used
            public static DeviceAction Atom_J_UTIL_NAV_MAP_SPT_5 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 }; //na
            public static DeviceAction Atom_J_UTIL_NAV_MAP_SPT_6 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 }; //na
            public static DeviceAction Atom_J_UTIL_NAV_DEF_PNT = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_UTIL_NAV_HSTZONE = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_UTIL_NAV_MAP_FIX_PNT = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_UTIL_NAV_MAP_INIT_PNT = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_UTIL_NAV_SURF_TGT = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_UTIL_NAV_HOME_BASE = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_UTIL_NAV_REST_MORE = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 }; // not used
            public static DeviceAction Atom_J_UTIL_NAV_REST_MSN_SPT_1 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_UTIL_NAV_REST_MSN_SPT_2 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_UTIL_NAV_REST_MSN_SPT_3 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_UTIL_NAV_REST_INIT_PT_1 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_UTIL_NAV_REST_INIT_PT_2 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 }; // na
            public static DeviceAction Atom_J_UTIL_NAV_REST_FIX_PT = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_UTIL_NAV_REST_MN_FIX_PT = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 }; //na
            public static DeviceAction Atom_J_UTIL_NAV_REST_STGT_1 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_UTIL_NAV_REST_HOME = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_UTIL_CONTR = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 }; // agree

            public static DeviceAction Atom_J_UTIL_CONTR_NO_TALK = new DeviceAction() { device = Devices.RIO, command = 3564, value = -1 }; //JESTER_Quiet
            public static DeviceAction Atom_J_UTIL_CONTR_TALK = new DeviceAction() { device = Devices.RIO, command = 3564, value = 1 }; //JESTER_Quiet
            public static DeviceAction Atom_J_UTIL_CONTR_EJECT_BTH = new DeviceAction() { device = Devices.RIO, command = 3565, value = -1 }; //JESTER_EjectConfig
            public static DeviceAction Atom_J_UTIL_CONTR_EJECT_SNG = new DeviceAction() { device = Devices.RIO, command = 3565, value = 1 }; // JESTER_EjectConfig
            public static DeviceAction Atom_J_UTIL_CONTR_CALL = new DeviceAction() { device = Devices.RIO, command = 3698, value = 1 }; //JESTER_EnableLandingCallouts
            public static DeviceAction Atom_J_UTIL_CONTR_NO_CALL = new DeviceAction() { device = Devices.RIO, command = 3698, value = -1 }; //JESTER_EnableLandingCallouts
            public static DeviceAction Atom_J_UTIL_CONTR_ACTIVE = new DeviceAction() { device = Devices.RIO, command = 3671, value = 1 }; //JESTER_Pause
            public static DeviceAction Atom_J_UTIL_CONTR_INACTIVE = new DeviceAction() { device = Devices.RIO, command = 3671, value = -1 }; //JESTER_Pause

            public static DeviceAction Atom_J_RESET = new DeviceAction() { device = Devices.RIO, command = 3725, value = 1 }; // 3551, 1 //closes menu

            public static DeviceAction Atom_J_RAD_DL_HOST_WASH = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_DL_HOST_ROOS = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_DL_HOST_LINC = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_DL_HOST_TRUM = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_DL_HOST_TICO = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_DL_HOST_FORE = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RAD_DL_HOST_BURK = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_JESTER_Steerpoint_SP1 = new DeviceAction() { device = Devices.RIO, command = 3566, value = 1 }; // LANTIRN Q JESTER_Steerpoint_SP1
            public static DeviceAction Atom_JESTER_Steerpoint_SP2 = new DeviceAction() { device = Devices.RIO, command = 3567, value = 1 }; // LANTIRN Q JESTER_Steerpoint_SP2
            public static DeviceAction Atom_JESTER_Steerpoint_SP3 = new DeviceAction() { device = Devices.RIO, command = 3568, value = 1 }; // LANTIRN Q JESTER_Steerpoint_SP3
            public static DeviceAction Atom_JESTER_Steerpoint_FP = new DeviceAction() { device = Devices.RIO, command = 3569, value = 1 }; // LANTIRN Q JESTER_Steerpoint_FP
            public static DeviceAction Atom_JESTER_Steerpoint_IP = new DeviceAction() { device = Devices.RIO, command = 3570, value = 1 }; // LANTIRN Q JESTER_Steerpoint_IP
            public static DeviceAction Atom_JESTER_Steerpoint_ST = new DeviceAction() { device = Devices.RIO, command = 3571, value = 1 }; // LANTIRN Q JESTER_Steerpoint_ST
            public static DeviceAction Atom_JESTER_Steerpoint_HB = new DeviceAction() { device = Devices.RIO, command = 3572, value = 1 }; // LANTIRN Q JESTER_Steerpoint_HB
            public static DeviceAction Atom_JESTER_Steerpoint_MAN = new DeviceAction() { device = Devices.RIO, command = 3573, value = 1 }; // LANTIRN Q JESTER_Steerpoint_MAN
            public static DeviceAction Atom_LANTIRN_GPSZero = new DeviceAction() { device = Devices.LANTIRN, command = 3683, value = 1 }; // LANTIRN Q LANTIRN_GPSZero = reset??
            public static DeviceAction Atom_LANTIRN_ToggleFOV = new DeviceAction() { device = Devices.LANTIRN, command = 3512, value = 1 }; // LANTIRN_ToggleFOV

            public static DeviceAction Atom_J_UTIL_NAV_MAP_MARKER = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };

            public static DeviceAction Atom_J_UTIL_NAV_GRD_ENABLE = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_UTIL_NAV_GRD_DSABLE = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_UTIL_NAV_GRD_CENTER = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_UTIL_NAV_GRD_REL_180 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_UTIL_NAV_GRD_REL_u30 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_UTIL_NAV_GRD_REL_u90 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_UTIL_NAV_GRD_REL_u120 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_UTIL_NAV_GRD_REL_d120 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_UTIL_NAV_GRD_REL_d90 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_UTIL_NAV_GRD_REL_d30 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_UTIL_NAV_GRD_ABS_0 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_UTIL_NAV_GRD_ABS_45 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_UTIL_NAV_GRD_ABS_90 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_UTIL_NAV_GRD_ABS_135 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_UTIL_NAV_GRD_ABS_180 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_UTIL_NAV_GRD_ABS_225 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_UTIL_NAV_GRD_ABS_270 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_UTIL_NAV_GRD_ABS_315 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_UTIL_NAV_GRD_COV_30 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_UTIL_NAV_GRD_COV_60 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_UTIL_NAV_GRD_COV_90 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_UTIL_NAV_GRD_COV_120 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_UTIL_NAV_GRD_COV_180 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_UTIL_NAV_GRD_1SCTR = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_UTIL_NAV_GRD_2SCTR = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_UTIL_NAV_GRD_3SCTR = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_UTIL_NAV_GRD_4SCTR = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_UTIL_NAV_GRD_5SCTR = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_UTIL_NAV_GRD_6SCTR = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_UTIL_NAV_GRD_MARKER = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder383 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder384 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder385 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder386 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder387 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder388 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder389 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            // end of utility

            // spare block : WALKMAN! (DONE)
            public static DeviceAction Atom_J_WLKMN_PLAY = new DeviceAction() { device = Devices.WALKMAN, command = 3070, value = 1 };
            public static DeviceAction Atom_J_WLKMN_STOP = new DeviceAction() { device = Devices.WALKMAN, command = 3071, value = 1 };
            public static DeviceAction Atom_J_WLKMN_NEXT = new DeviceAction() { device = Devices.WALKMAN, command = 3072, value = 1 };
            public static DeviceAction Atom_J_WLKMN_PREV = new DeviceAction() { device = Devices.WALKMAN, command = 3073, value = 1 };

            // INS
            public static DeviceAction Atom_J_RDR_ASP_BEAM = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RDR_ASP_NOSE = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_RDR_ASP_TAIL = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder397 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder398 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder399 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };

            // block: defensive
            public static DeviceAction Atom_J_DEF_CMS_MOD = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_DEF_CMS_MOD_OFF = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_DEF_CMS_MOD_MAN = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_DEF_CMS_MOD_AUTO = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };

            public static DeviceAction Atom_J_DEF_FLR_MOD = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_DEF_FLR_MOD_PILOT = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_DEF_FLR_MOD_NORM = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_DEF_FLR_MOD_MULTI = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };

            public static DeviceAction Atom_J_DEF_CHF_PGM = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_DEF_CHF_PGM_RR_12 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_DEF_CHF_PGM_RR_46 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_DEF_CHF_PGM_RR_86 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_DEF_CHF_PGM_20_44 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_DEF_CHF_PGM_20_84 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_DEF_CHF_PGM_40_44 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_DEF_CHF_PGM_40_84 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_DEF_CHF_PGM_R1_12 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };

            public static DeviceAction Atom_J_DEF_RWR_DSP_TYP = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_DEF_RWR_AIRB = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_DEF_RWR_NORM = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_DEF_RWR_AAA = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_DEF_RWR_UNK = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_DEF_RWR_FRND = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };

            public static DeviceAction Atom_J_DEF_JMR_ON = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_DEF_JMR_SBY = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };

            public static DeviceAction Atom_J_DEF_CMS_CTL_ORD = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_DEF_CMS_CHF_PGM = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_DEF_CMS_CHF_SNGL = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_DEF_CMS_CHF_TGHT = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_DEF_CMS_FLR_PGM = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_DEF_CMS_FLR_SNGL = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_DEF_CMS_FLR_TGHT = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };

            public static DeviceAction Atom_J_DEF_FLR_PGM = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_DEF_FLR_PGM_1 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_DEF_FLR_PGM_2 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_DEF_FLR_PGM_3 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_DEF_FLR_PGM_4 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_DEF_FLR_PGM_5 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_DEF_FLR_PGM_6 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_DEF_FLR_PGM_7 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_DEF_FLR_PGM_8 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };


            public static DeviceAction Atom_LANTIRN_Srch_Any = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_LANTIRN_Srch_Any_Active = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_LANTIRN_Srch_Air = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_LANTIRN_Srch_Air_Active = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_LANTIRN_Srch_SAM_Active = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_LANTIRN_Srch_Armor_Active = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_LANTIRN_Srch_Vehicle = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_LANTIRN_Ships_Active = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };

            //public static DeviceAction Atom_Laser_000 = new DeviceAction() { device = Devices.KNEEBOARD, command = 3593, value = 0.0 };
            //public static DeviceAction Atom_Laser_100 = new DeviceAction() { device = Devices.KNEEBOARD, command = 3593, value = 0.1 };
            //public static DeviceAction Atom_Laser_200 = new DeviceAction() { device = Devices.KNEEBOARD, command = 3593, value = 0.2 };
            //public static DeviceAction Atom_Laser_300 = new DeviceAction() { device = Devices.KNEEBOARD, command = 3593, value = 0.3 };
            //public static DeviceAction Atom_Laser_400 = new DeviceAction() { device = Devices.KNEEBOARD, command = 3593, value = 0.4 };
            //public static DeviceAction Atom_Laser_500 = new DeviceAction() { device = Devices.KNEEBOARD, command = 3593, value = 0.5 };
            //public static DeviceAction Atom_Laser_600 = new DeviceAction() { device = Devices.KNEEBOARD, command = 3593, value = 0.6 };
            //public static DeviceAction Atom_Laser_700 = new DeviceAction() { device = Devices.KNEEBOARD, command = 3593, value = 0.7 };
            //public static DeviceAction Atom_Laser_800 = new DeviceAction() { device = Devices.KNEEBOARD, command = 3593, value = 0.8 };
            //public static DeviceAction Atom_Laser_900 = new DeviceAction() { device = Devices.KNEEBOARD, command = 3593, value = 0.9 };

            //public static DeviceAction Atom_Laser_x00 = new DeviceAction() { device = Devices.KNEEBOARD, command = 3594, value = 0.0 };
            //public static DeviceAction Atom_Laser_010 = new DeviceAction() { device = Devices.KNEEBOARD, command = 3594, value = 0.1 };
            //public static DeviceAction Atom_Laser_020 = new DeviceAction() { device = Devices.KNEEBOARD, command = 3594, value = 0.2 };
            //public static DeviceAction Atom_Laser_030 = new DeviceAction() { device = Devices.KNEEBOARD, command = 3594, value = 0.3 };
            //public static DeviceAction Atom_Laser_040 = new DeviceAction() { device = Devices.KNEEBOARD, command = 3594, value = 0.4 };
            //public static DeviceAction Atom_Laser_050 = new DeviceAction() { device = Devices.KNEEBOARD, command = 3594, value = 0.5 };
            //public static DeviceAction Atom_Laser_060 = new DeviceAction() { device = Devices.KNEEBOARD, command = 3594, value = 0.6 };
            //public static DeviceAction Atom_Laser_070 = new DeviceAction() { device = Devices.KNEEBOARD, command = 3594, value = 0.7 };
            //public static DeviceAction Atom_Laser_080 = new DeviceAction() { device = Devices.KNEEBOARD, command = 3594, value = 0.8 };
            //public static DeviceAction Atom_Laser_090 = new DeviceAction() { device = Devices.KNEEBOARD, command = 3594, value = 0.9 };

            //public static DeviceAction Atom_Laser_xx0 = new DeviceAction() { device = Devices.KNEEBOARD, command = 3595, value = 0.0 };
            //public static DeviceAction Atom_Laser_001 = new DeviceAction() { device = Devices.KNEEBOARD, command = 3595, value = 0.1 };
            //public static DeviceAction Atom_Laser_002 = new DeviceAction() { device = Devices.KNEEBOARD, command = 3595, value = 0.2 };
            //public static DeviceAction Atom_Laser_003 = new DeviceAction() { device = Devices.KNEEBOARD, command = 3595, value = 0.3 };
            //public static DeviceAction Atom_Laser_004 = new DeviceAction() { device = Devices.KNEEBOARD, command = 3595, value = 0.4 };
            //public static DeviceAction Atom_Laser_005 = new DeviceAction() { device = Devices.KNEEBOARD, command = 3595, value = 0.5 };
            //public static DeviceAction Atom_Laser_006 = new DeviceAction() { device = Devices.KNEEBOARD, command = 3595, value = 0.6 };
            //public static DeviceAction Atom_Laser_007 = new DeviceAction() { device = Devices.KNEEBOARD, command = 3595, value = 0.7 };
            //public static DeviceAction Atom_Laser_008 = new DeviceAction() { device = Devices.KNEEBOARD, command = 3595, value = 0.8 };
            //public static DeviceAction Atom_Laser_009 = new DeviceAction() { device = Devices.KNEEBOARD, command = 3595, value = 0.9 };

            //public static DeviceAction Atom_Placeholder450 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder451 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder452 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder453 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder454 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder455 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder456 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder457 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder458 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder459 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder460 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder461 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder462 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder463 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder464 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder465 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder466 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder467 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder468 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder469 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder470 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder471 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder472 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder473 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder474 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder475 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder476 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder477 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder478 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder479 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder480 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder481 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder482 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder483 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder484 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder485 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder486 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder487 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder488 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder489 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            // end of defensive

            // spare block
            //public static DeviceAction Atom_Placeholder490 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder491 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder492 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder493 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder494 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder495 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder496 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder497 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder498 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder499 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };

            // 500-600 SPARE: RIO misc

            // Startup
            public static DeviceAction Atom_J_STRT_ABORT = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_STRT_INS_FINE = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_STRT_INS_MIN_WPN = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_STRT_INS_COARSE = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_STRT_INS_NOW = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };

            public static DeviceAction Atom_J_STRT_CHECK = new DeviceAction() { device = Devices.RIO, command = 3669, value = 1 }; // ob:+13
            public static DeviceAction Atom_J_STRT_LOUD_CLR = new DeviceAction() { device = Devices.RIO, command = 3670, value = 1 }; // ob:+13
            public static DeviceAction Atom_J_STRT_PAUSE = new DeviceAction() { device = Devices.RIO, command = 3671, value = 1 }; // ob:+13
            public static DeviceAction Atom_J_STRT_STARTUP = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_J_STRT_ASSISTED = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };


            // block 600-700: AI pilot (iceman)

            public static DeviceAction Atom_I_VOID = new DeviceAction();

            public static DeviceAction Atom_I_ALT = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_I_ALT_ANG_1 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_I_ALT_ANG_5 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_I_ALT_ANG_10 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_I_ALT_ANG_15 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_I_ALT_ANG_20 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_I_ALT_ANG_25 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_I_ALT_ANG_30 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_I_ALT_ANG_35 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_I_ALT_CHG = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_I_ALT_DESC_10K = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_I_ALT_DESC_5K = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_I_ALT_DESC_1K = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_I_ALT_DESC_500 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_I_ALT_CLMB_500 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_I_ALT_CLMB_1K = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_I_ALT_CLMB_5K = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_I_ALT_CLMB_10K = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_I_SPD_MINUS_200 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_I_SPD_MINUS_100 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_I_SPD_MINUS_50 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_I_SPD_PLUS_50 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_I_SPD_PLUS_100 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_I_SPD_PLUS_200 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_I_DIR_N = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_I_DIR_NE = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_I_DIR_E = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_I_DIR_SE = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_I_DIR_S = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_I_DIR_SW = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_I_DIR_W = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_I_DIR_NW = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_I_DIR = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_I_DIR_CHG_L45 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_I_DIR_CHG_L30 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_I_DIR_CHG_L10 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_I_DIR_CHG_L5 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_I_DIR_CHG_R5 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_I_DIR_CHG_R10 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_I_DIR_CHG_R30 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_I_DIR_CHG_R45 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_I_DIR_HOLD_CRS = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_I_SPD = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            public static DeviceAction Atom_I_DIR_CHG = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };

            //public static DeviceAction Atom_I_SPT_FLYTO = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_I_SPT_ORBIT = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder646 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder647 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder648 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder649 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder650 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder651 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder652 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder653 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder654 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder655 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder656 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder657 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder658 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder659 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder660 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder661 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder662 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder663 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder664 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder665 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder666 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder667 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder668 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder669 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder670 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder671 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder672 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder673 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder674 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder675 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder676 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder677 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder678 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder679 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder680 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder681 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder682 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder683 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder684 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder685 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder686 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder687 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder688 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder689 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            // end of AI pilot

            // spare block
            //public static DeviceAction Atom_Placeholder690 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder691 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder692 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder693 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder694 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder695 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder696 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder697 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder698 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };
            //public static DeviceAction Atom_Placeholder699 = new DeviceAction() { device = Devices.RIO, command = start + 0, value = 0 };

        }

    }

}
