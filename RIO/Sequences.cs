using System.Collections.Generic;

namespace VAICOM.Extensions.RIO
{

    public static partial class DeviceActionsLibrary
    {

        public partial class Sequences // strings using base commands
        {
            public class Macro
            {
                // examples
                public static List<DeviceAction> Seq_J_CANOPY_OPEN = new List<DeviceAction>() { RIO.Atom_J_CANOPY_OPEN }; //.... can use multiple actions in Macro:
                public static List<DeviceAction> Seq_J_CANOPY_CLOSE = new List<DeviceAction>() { RIO.Atom_J_CANOPY_CLOSE }; // , RIO.EjectionSeatSafeArmedHandleIn,.. : 

                // block: menu + radar 000-099
                public static List<DeviceAction> Seq_J_MENU_TOGGLE = new List<DeviceAction>() { RIO.Atom_J_MENU_TOGGLE };
                public static List<DeviceAction> Seq_J_MENU_OPTION_1 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_1 };
                public static List<DeviceAction> Seq_J_MENU_OPTION_2 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2 };
                public static List<DeviceAction> Seq_J_MENU_OPTION_3 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_3 };
                public static List<DeviceAction> Seq_J_MENU_OPTION_4 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4 };
                public static List<DeviceAction> Seq_J_MENU_OPTION_5 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5 };
                public static List<DeviceAction> Seq_J_MENU_OPTION_6 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_6 };
                public static List<DeviceAction> Seq_J_MENU_OPTION_7 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_7 };
                public static List<DeviceAction> Seq_J_MENU_OPTION_8 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_8 };

                public static List<DeviceAction> Seq_J_MENU_DIR_D = new List<DeviceAction>() { RIO.Atom_J_MENU_DIR_D }; // not used
                public static List<DeviceAction> Seq_J_MENU_DIR_DL = new List<DeviceAction>() { RIO.Atom_J_MENU_DIR_DL };
                public static List<DeviceAction> Seq_J_MENU_DIR_DR = new List<DeviceAction>() { RIO.Atom_J_MENU_DIR_DR };
                public static List<DeviceAction> Seq_J_MENU_DIR_L = new List<DeviceAction>() { RIO.Atom_J_MENU_DIR_L };
                public static List<DeviceAction> Seq_J_MENU_DIR_R = new List<DeviceAction>() { RIO.Atom_J_MENU_DIR_R };
                public static List<DeviceAction> Seq_J_MENU_DIR_U = new List<DeviceAction>() { RIO.Atom_J_MENU_DIR_U };
                public static List<DeviceAction> Seq_J_MENU_DIR_UL = new List<DeviceAction>() { RIO.Atom_J_MENU_DIR_UL };
                public static List<DeviceAction> Seq_J_MENU_DIR_UR = new List<DeviceAction>() { RIO.Atom_J_MENU_DIR_UR };
                public static List<DeviceAction> Seq_J_MENU_OPEN = new List<DeviceAction>() { RIO.Atom_J_MENU_OPEN };
                public static List<DeviceAction> Seq_J_MENU_CLOSE = new List<DeviceAction>() { RIO.Atom_J_MENU_CLOSE };
                public static List<DeviceAction> Seq_J_MENU_CONTEXT = new List<DeviceAction>() { RIO.Atom_J_MENU_CLOSE, RIO.Atom_J_MENU_OPEN };

                public static List<DeviceAction> Seq_J_MENU_MAIN = new List<DeviceAction>() { RIO.Atom_J_MENU_CLOSE, RIO.Atom_J_MENU_OPEN, RIO.Atom_J_MENU_OPEN }; // start closing immediately { RIO.Atom_J_MENU_CLOSE, RIO.Atom_J_MENU_OPEN, RIO.Atom_J_MENU_OPEN };  // CLOSED AT END FOR TEST!!
                public static List<DeviceAction> Seq_J_MENU_CTXT_CLOSE = new List<DeviceAction>() { RIO.Atom_J_MENU_CLOSE, RIO.Atom_J_MENU_CLOSE };
                public static List<DeviceAction> Seq_J_MENU_MAIN_CLOSE = new List<DeviceAction>() { RIO.Atom_J_MENU_CLOSE, RIO.Atom_J_MENU_CLOSE, RIO.Atom_J_MENU_CLOSE, RIO.Atom_J_MENU_CLOSE };

                public static List<DeviceAction> Seq_JESTER_LANTIRN_inhibit_auto_designate = new List<DeviceAction>() { RIO.Atom_J_MENU_CLOSE, RIO.Atom_J_MENU_OPEN, RIO.Atom_J_MENU_OPTION_7, RIO.Atom_J_MENU_OPTION_5 }; //RIO.Atom_JESTER_LANTIRN_inhibit_auto_designate     };
                public static List<DeviceAction> Seq_JESTER_LANTIRN_track_target_id = new List<DeviceAction>() { RIO.Atom_JESTER_LANTIRN_track_target_id }; // not used
                public static List<DeviceAction> Seq_JESTER_LANTIRN_track_zone_id = new List<DeviceAction>() { RIO.Atom_JESTER_LANTIRN_track_zone_id }; // not used
                public static List<DeviceAction> Seq_JESTER_LANTIRN_designate = new List<DeviceAction>() { RIO.Atom_J_MENU_CLOSE, RIO.Atom_J_MENU_OPEN, RIO.Atom_J_MENU_OPTION_7, RIO.Atom_J_MENU_OPTION_3 }; // RIO.Atom_JESTER_LANTIRN_designate     };
                //public static List<DeviceAction> Seq_KNEEBOARD_Laser100                     = new List<DeviceAction>() { RIO.Atom_KNEEBOARD_Laser100     };
                //public static List<DeviceAction> Seq_KNEEBOARD_Laser10                      = new List<DeviceAction>() { RIO.Atom_KNEEBOARD_Laser10     };
                //public static List<DeviceAction> Seq_KNEEBOARD_Laser1                       = new List<DeviceAction>() { RIO.Atom_KNEEBOARD_Laser1     };
                // end of menu control

                // spare block
                public static List<DeviceAction> Seq_LANTIRN_ToggleWHOTBHOT = new List<DeviceAction>() { RIO.Atom_J_MENU_CLOSE, RIO.Atom_J_MENU_OPEN, RIO.Atom_J_MENU_OPTION_7, RIO.Atom_J_MENU_OPTION_6 };
                public static List<DeviceAction> Seq_LANTIRN_LaserLatched = new List<DeviceAction>() { RIO.Atom_J_MENU_CLOSE, RIO.Atom_J_MENU_OPEN, RIO.Atom_J_MENU_OPTION_7, RIO.Atom_J_MENU_OPTION_1 }; // RIO.Atom_LANTIRN_LaserLatched          };
                public static List<DeviceAction> Seq_LANTIRN_Laser_ARM = new List<DeviceAction>() { RIO.Atom_LANTIRN_Laser_ARM };
                public static List<DeviceAction> Seq_LANTIRN_Laser_ARM_Toggle = new List<DeviceAction>() { RIO.Atom_LANTIRN_Laser_ARM_Toggle };
                public static List<DeviceAction> Seq_LANTIRN_Undesignate = new List<DeviceAction>() { RIO.Atom_J_MENU_CLOSE, RIO.Atom_J_MENU_OPEN, RIO.Atom_J_MENU_OPTION_7, RIO.Atom_J_MENU_OPTION_4 };
                public static List<DeviceAction> Seq_LANTIRN_QHUD_QADL = new List<DeviceAction>() { RIO.Atom_J_MENU_CLOSE, RIO.Atom_J_MENU_OPEN, RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_1 };
                public static List<DeviceAction> Seq_LANTIRN_QSNO = new List<DeviceAction>() { RIO.Atom_J_MENU_CLOSE, RIO.Atom_J_MENU_OPEN, RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_2 };
                public static List<DeviceAction> Seq_LANTIRN_QDES = new List<DeviceAction>() { RIO.Atom_J_MENU_CLOSE, RIO.Atom_J_MENU_OPEN, RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_4 };
                public static List<DeviceAction> Seq_LANTIRN_QWP_Minus = new List<DeviceAction>() { RIO.Atom_J_MENU_CLOSE, RIO.Atom_J_MENU_OPEN, RIO.Atom_J_MENU_OPTION_6 }; // RIO.Atom_LANTIRN_QWP_Minus        };
                public static List<DeviceAction> Seq_LANTIRN_QWP_Plus = new List<DeviceAction>() { RIO.Atom_J_MENU_CLOSE, RIO.Atom_J_MENU_OPEN, RIO.Atom_J_MENU_OPTION_5 }; // RIO.Atom_LANTIRN_QWP_Plus        };

                // block: radar (DONE)
                public static List<DeviceAction> Seq_J_RDR_GO_SILENT = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_7 }; // <<< now 7! (was 5)
                public static List<DeviceAction> Seq_J_RDR_SPOT = new List<DeviceAction>() { RIO.Atom_J_RDR_SPOT }; // not used
                public static List<DeviceAction> Seq_J_RDR_BREAK_LOCK = new List<DeviceAction>() { RIO.Atom_J_RDR_BREAK_LOCK }; // when state is STT locked 
                public static List<DeviceAction> Seq_J_RDR_TO_PSTT = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_1 }; // when state is STT locked 
                public static List<DeviceAction> Seq_J_RDR_SCAN_ELEV = new List<DeviceAction>() { }; // not endpoint
                public static List<DeviceAction> Seq_J_RDR_SCAN_AZ = new List<DeviceAction>() { }; // not endpoint 
                public static List<DeviceAction> Seq_J_RDR_SCAN_DIST = new List<DeviceAction>() { }; // not endpoint
                public static List<DeviceAction> Seq_J_RDR_TOGGLE_STT = new List<DeviceAction>() { RIO.Atom_J_RDR_TOGGLE_STT };
                public static List<DeviceAction> Seq_J_RDR_VSL_HIGH = new List<DeviceAction>() { RIO.Atom_J_RDR_VSL_HIGH };
                public static List<DeviceAction> Seq_J_RDR_VSL_LOW = new List<DeviceAction>() { RIO.Atom_J_RDR_VSL_LOW };
                public static List<DeviceAction> Seq_J_RDR_STT_TGT_AHEAD = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_1 };
                public static List<DeviceAction> Seq_J_RDR_STT_ENMY_TGT_AHEAD = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_2 };
                public static List<DeviceAction> Seq_J_RDR_STT_FRNDLY_TGT_AHEAD = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_3 };
                public static List<DeviceAction> Seq_J_RDR_STT_CHOOSE_SPECIFIC_TGT = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_4 };
                public static List<DeviceAction> Seq_J_RDR_STT_FIRST_TWS_TGT = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_5 };
                public static List<DeviceAction> Seq_J_RDR_STT_TWS_TGT_NUM = new List<DeviceAction>() { };
                public static List<DeviceAction> Seq_J_RDR_BVR = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_2 }; // not endpoint (root, disable cmd)
                public static List<DeviceAction> Seq_J_RDR_GO_ACTIVE = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_6 }; // <<< NOW 6! (was 4)
                public static List<DeviceAction> Seq_J_RDR_STT_LOCK = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_1 }; // not endpoint
                public static List<DeviceAction> Seq_J_RDR_AUTO = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_1 }; ////elevation auto
                public static List<DeviceAction> Seq_J_RDR_RNG_100 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_4 }; // 
                public static List<DeviceAction> Seq_J_RDR_RNG_200 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_5 };
                public static List<DeviceAction> Seq_J_RDR_RNG_400 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_6 }; // 
                public static List<DeviceAction> Seq_J_RDR_POS = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_3, RIO.Atom_J_MENU_OPTION_1 }; // AZ AUTO option
                public static List<DeviceAction> Seq_J_RDR_POS_CTR = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_3, RIO.Atom_J_MENU_OPTION_4 }; // POS = Azimuth
                public static List<DeviceAction> Seq_J_RDR_POS_CTR_L = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_3, RIO.Atom_J_MENU_OPTION_3 };
                public static List<DeviceAction> Seq_J_RDR_POS_CTR_R = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_3, RIO.Atom_J_MENU_OPTION_5 };
                public static List<DeviceAction> Seq_J_RDR_POS_L = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_3, RIO.Atom_J_MENU_OPTION_2 };
                public static List<DeviceAction> Seq_J_RDR_POS_R = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_3, RIO.Atom_J_MENU_OPTION_6 };
                public static List<DeviceAction> Seq_J_RDR_POS_HI = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_6 }; // ELEV
                public static List<DeviceAction> Seq_J_RDR_POS_LO = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_2 }; // 
                public static List<DeviceAction> Seq_J_RDR_POS_MID = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_4 };//
                public static List<DeviceAction> Seq_J_RDR_POS_MID_HI = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_5 };//
                public static List<DeviceAction> Seq_J_RDR_POS_MID_LO = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_3 };//
                public static List<DeviceAction> Seq_J_RDR_STT_TWS_TGT_1 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_1 };
                public static List<DeviceAction> Seq_J_RDR_STT_TWS_TGT_2 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_2 };
                public static List<DeviceAction> Seq_J_RDR_STT_TWS_TGT_3 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_3 };
                public static List<DeviceAction> Seq_J_RDR_STT_TWS_TGT_4 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_4 };
                public static List<DeviceAction> Seq_J_RDR_STT_TWS_TGT_5 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_5 };
                public static List<DeviceAction> Seq_J_RDR_STT_TWS_TGT_6 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_6 };
                public static List<DeviceAction> Seq_J_RDR_STT_TWS_TGT_7 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_7 };
                public static List<DeviceAction> Seq_J_RDR_STT_TWS_TGT_8 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_8 };
                public static List<DeviceAction> Seq_J_RDR_RNG_25 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_2 };
                public static List<DeviceAction> Seq_J_RDR_RNG_50 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_3 };
                public static List<DeviceAction> Seq_J_RDR_RNG_AUTO = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_1 };


                public static List<DeviceAction> Seq_J_RDR_MODE_AUTO = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_1 }; // 
                public static List<DeviceAction> Seq_J_RDR_MODE_TWS = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_2 }; // 
                public static List<DeviceAction> Seq_J_RDR_MODE_TWS_MAN = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_3 }; // NEW! TWS MANUAL
                public static List<DeviceAction> Seq_J_RDR_MODE_RWS = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_4 }; // NOW 4
                public static List<DeviceAction> Seq_J_RDR_MODE_SIZE_M = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_1 }; // NEW! TGTSIZE NORMAL
                public static List<DeviceAction> Seq_J_RDR_MODE_SIZE_L = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_2 }; // NEW! TGTSIZE LARGE
                public static List<DeviceAction> Seq_J_RDR_MODE_SIZE_S = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_3 }; // NEW! TGTSIZE SMAALL


                public static List<DeviceAction> Seq_J_RDR_MODE = new List<DeviceAction>() { };// show hint
                // end of radar

                // spare block
                public static List<DeviceAction> Seq_J_RDR_STAB = new List<DeviceAction>() { }; // show hint
                public static List<DeviceAction> Seq_J_RDR_STAB_15 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_1 };
                public static List<DeviceAction> Seq_J_RDR_STAB_30 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_2 };
                public static List<DeviceAction> Seq_J_RDR_STAB_60 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_3 };
                public static List<DeviceAction> Seq_J_RDR_STAB_120 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_4 };
                public static List<DeviceAction> Seq_J_RDR_STAB_INDEF = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_5 };
                public static List<DeviceAction> Seq_J_RDR_STAB_GROUND = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_3 };

                public static List<DeviceAction> Seq_LANTIRN_Head_Eyeball = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_1 };
                public static List<DeviceAction> Seq_LANTIRN_Head_Head = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_2 };
                //public static List<DeviceAction> Seq_PlaceHolder098 = new List<DeviceAction>() { RIO.Atom_Placeholder098 };
                //public static List<DeviceAction> Seq_PlaceHolder099 = new List<DeviceAction>() { RIO.Atom_Placeholder099 };

                // block: weapons 100-199 (DONE check: jettison) 

                public static List<DeviceAction> Seq_J_WPN_AG_SORDN = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_1 };
                public static List<DeviceAction> Seq_J_WPN_AG_SORDN_WPN_1 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_1 }; // all 1 dafault
                public static List<DeviceAction> Seq_J_WPN_AG_SORDN_WPN_2 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_1 };
                public static List<DeviceAction> Seq_J_WPN_AG_SORDN_WPN_3 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_1 };
                public static List<DeviceAction> Seq_J_WPN_AG_SORDN_WPN_4 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_1 };
                public static List<DeviceAction> Seq_J_WPN_AG_SORDN_WPN_5 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_1 };
                public static List<DeviceAction> Seq_J_WPN_AG_SORDN_WPN_6 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_1 };
                public static List<DeviceAction> Seq_J_WPN_AG_SORDN_WPN_7 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_1 };
                public static List<DeviceAction> Seq_J_WPN_AG_SORDN_WPN_8 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_1 };
                public static List<DeviceAction> Seq_J_WPN_AG_SPOT = new List<DeviceAction>() { RIO.Atom_J_WPN_AG_SPOT };
                public static List<DeviceAction> Seq_J_WPN_AG_SET_COMP_TGT = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_1 }; // was 2
                public static List<DeviceAction> Seq_J_WPN_AG_SET_PAIRS = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_3 };
                public static List<DeviceAction> Seq_J_WPN_AG_SETFUSE = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_4 };
                public static List<DeviceAction> Seq_J_WPN_AG_SETFUSE_NOSETAIL = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_1 };
                public static List<DeviceAction> Seq_J_WPN_AG_SETFUSE_NOSE = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_2 };
                public static List<DeviceAction> Seq_J_WPN_AG_SETFUSE_SAFE = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_3 };
                public static List<DeviceAction> Seq_J_WPN_AG_RIP_QTY = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_1 };
                public static List<DeviceAction> Seq_J_WPN_AG_RIP_QTY_STEP = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_1 };
                public static List<DeviceAction> Seq_J_WPN_AG_RIP_QTY_2 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_2 }; //2
                public static List<DeviceAction> Seq_J_WPN_AG_RIP_QTY_5 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_3 }; //3
                public static List<DeviceAction> Seq_J_WPN_AG_RIP_QTY_10 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_4 }; //4
                public static List<DeviceAction> Seq_J_WPN_AG_RIP_QTY_20 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_5 }; //6
                public static List<DeviceAction> Seq_J_WPN_AG_RIP_QTY_30 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_6 }; //8 (7=16,8=28)
                public static List<DeviceAction> Seq_J_WPN_AG_RIP_TIME = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_2 };
                public static List<DeviceAction> Seq_J_WPN_AG_RIP_TIME_STEP = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_1 };
                public static List<DeviceAction> Seq_J_WPN_AG_RIP_TIME_10 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_2 };
                public static List<DeviceAction> Seq_J_WPN_AG_RIP_TIME_20 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_3 };
                public static List<DeviceAction> Seq_J_WPN_AG_RIP_TIME_50 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_4 };
                public static List<DeviceAction> Seq_J_WPN_AG_RIP_TIME_100 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_5 };
                public static List<DeviceAction> Seq_J_WPN_AG_RIP_TIME_200 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_6 };
                public static List<DeviceAction> Seq_J_WPN_AG_RIP_TIME_500 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_7 };
                public static List<DeviceAction> Seq_J_WPN_AG_RIP_TIME_990 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_8 };
                public static List<DeviceAction> Seq_J_WPN_AG_RIP_DIST = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_3 };
                public static List<DeviceAction> Seq_J_WPN_AG_RIP_DIST_STEP = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_3, RIO.Atom_J_MENU_OPTION_1 }; // actually not in dist menu
                public static List<DeviceAction> Seq_J_WPN_AG = new List<DeviceAction>() { RIO.Atom_J_WPN_AG };
                public static List<DeviceAction> Seq_J_WPN_AA = new List<DeviceAction>() { RIO.Atom_J_WPN_AA };
                public static List<DeviceAction> Seq_J_WPN_AG_RIP = new List<DeviceAction>() { RIO.Atom_J_WPN_AG_RIP };
                public static List<DeviceAction> Seq_J_WPN_AG_JETT = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_6 }; // jetisson selected
                public static List<DeviceAction> Seq_J_WPN_AG_RIP_DIST_5 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_3, RIO.Atom_J_MENU_OPTION_1 };
                public static List<DeviceAction> Seq_J_WPN_AG_RIP_DIST_10 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_3, RIO.Atom_J_MENU_OPTION_2 };
                public static List<DeviceAction> Seq_J_WPN_AG_RIP_DIST_25 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_3, RIO.Atom_J_MENU_OPTION_3 };
                public static List<DeviceAction> Seq_J_WPN_AG_RIP_DIST_50 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_3, RIO.Atom_J_MENU_OPTION_4 };
                public static List<DeviceAction> Seq_J_WPN_AG_RIP_DIST_100 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_3, RIO.Atom_J_MENU_OPTION_5 };
                public static List<DeviceAction> Seq_J_WPN_AG_RIP_DIST_200 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_3, RIO.Atom_J_MENU_OPTION_6 };
                public static List<DeviceAction> Seq_J_WPN_AG_RIP_DIST_400 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_3, RIO.Atom_J_MENU_OPTION_7 };
                public static List<DeviceAction> Seq_J_WPN_AG_UTIL_LANTIRN = new List<DeviceAction>() { new DeviceAction() { device = Devices.TID, command = 3498, value = 1 }, new DeviceAction() { device = Devices.TID, command = 3498, value = 0 } }; //new DeviceAction() { device = Devices.TID, command = 3498, value = 0}, 
                public static List<DeviceAction> Seq_J_WPN_AG_SORDN_WPN_9 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_1 };
                public static List<DeviceAction> Seq_J_WPN_AG_SORDN_WPN_10 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_1 };
                public static List<DeviceAction> Seq_J_WPN_AG_SET_SNGL = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_3 };
                public static List<DeviceAction> Seq_J_WPN_AG_SET_COMP_PILOT = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_2 }; // same
                public static List<DeviceAction> Seq_J_WPN_AG_DROP_TANKS = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_3, RIO.Atom_J_MENU_OPTION_1 };

                public static List<DeviceAction> Seq_J_WPN_AG_STN = new List<DeviceAction>() { }; // not endpoint
                public static List<DeviceAction> Seq_J_WPN_AG_STN_18 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_7, RIO.Atom_J_MENU_OPTION_1 };
                public static List<DeviceAction> Seq_J_WPN_AG_STN_27 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_7, RIO.Atom_J_MENU_OPTION_2 };
                public static List<DeviceAction> Seq_J_WPN_AG_STN_3456 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_7, RIO.Atom_J_MENU_OPTION_3 };
                public static List<DeviceAction> Seq_J_WPN_AG_STN_36 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_7, RIO.Atom_J_MENU_OPTION_4 };
                public static List<DeviceAction> Seq_J_WPN_AG_STN_45 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_7, RIO.Atom_J_MENU_OPTION_5 };

                public static List<DeviceAction> Seq_PlaceHolder155 = new List<DeviceAction>() { RIO.Atom_J_RAD_DL_MAJ1_0 };
                public static List<DeviceAction> Seq_PlaceHolder156 = new List<DeviceAction>() { RIO.Atom_J_RAD_DL_MAJ1_1 };
                public static List<DeviceAction> Seq_PlaceHolder157 = new List<DeviceAction>() { RIO.Atom_J_RAD_DL_MAJ1_2 };
                public static List<DeviceAction> Seq_PlaceHolder158 = new List<DeviceAction>() { RIO.Atom_J_RAD_DL_MAJ1_3 };
                public static List<DeviceAction> Seq_PlaceHolder159 = new List<DeviceAction>() { RIO.Atom_J_RAD_DL_MAJ1_4 };
                public static List<DeviceAction> Seq_PlaceHolder160 = new List<DeviceAction>() { RIO.Atom_J_RAD_DL_MAJ1_5 };
                public static List<DeviceAction> Seq_PlaceHolder161 = new List<DeviceAction>() { RIO.Atom_J_RAD_DL_MAJ1_6 };
                public static List<DeviceAction> Seq_PlaceHolder162 = new List<DeviceAction>() { RIO.Atom_J_RAD_DL_MAJ1_7 };
                public static List<DeviceAction> Seq_PlaceHolder163 = new List<DeviceAction>() { RIO.Atom_J_RAD_DL_MAJ1_8 };
                public static List<DeviceAction> Seq_PlaceHolder164 = new List<DeviceAction>() { RIO.Atom_J_RAD_DL_MAJ1_9 };
                public static List<DeviceAction> Seq_PlaceHolder165 = new List<DeviceAction>() { RIO.Atom_J_RAD_DL_MAJ2_0 };
                public static List<DeviceAction> Seq_PlaceHolder166 = new List<DeviceAction>() { RIO.Atom_J_RAD_DL_MAJ2_1 };
                public static List<DeviceAction> Seq_PlaceHolder167 = new List<DeviceAction>() { RIO.Atom_J_RAD_DL_MAJ2_2 };
                public static List<DeviceAction> Seq_PlaceHolder168 = new List<DeviceAction>() { RIO.Atom_J_RAD_DL_MAJ2_3 };
                public static List<DeviceAction> Seq_PlaceHolder169 = new List<DeviceAction>() { RIO.Atom_J_RAD_DL_MAJ2_4 };
                public static List<DeviceAction> Seq_PlaceHolder170 = new List<DeviceAction>() { RIO.Atom_J_RAD_DL_MAJ2_5 };
                public static List<DeviceAction> Seq_PlaceHolder171 = new List<DeviceAction>() { RIO.Atom_J_RAD_DL_MAJ2_6 };
                public static List<DeviceAction> Seq_PlaceHolder172 = new List<DeviceAction>() { RIO.Atom_J_RAD_DL_MAJ2_7 };
                public static List<DeviceAction> Seq_PlaceHolder173 = new List<DeviceAction>() { RIO.Atom_J_RAD_DL_MAJ2_8 };
                public static List<DeviceAction> Seq_PlaceHolder174 = new List<DeviceAction>() { RIO.Atom_J_RAD_DL_MAJ2_9 };
                public static List<DeviceAction> Seq_PlaceHolder175 = new List<DeviceAction>() { RIO.Atom_J_RAD_DL_MIN_0 };
                public static List<DeviceAction> Seq_PlaceHolder176 = new List<DeviceAction>() { RIO.Atom_J_RAD_DL_MIN_1 };
                public static List<DeviceAction> Seq_PlaceHolder177 = new List<DeviceAction>() { RIO.Atom_J_RAD_DL_MIN_2 };
                public static List<DeviceAction> Seq_PlaceHolder178 = new List<DeviceAction>() { RIO.Atom_J_RAD_DL_MIN_3 };
                public static List<DeviceAction> Seq_PlaceHolder179 = new List<DeviceAction>() { RIO.Atom_J_RAD_DL_MIN_4 };
                public static List<DeviceAction> Seq_PlaceHolder180 = new List<DeviceAction>() { RIO.Atom_J_RAD_DL_MIN_5 };
                public static List<DeviceAction> Seq_PlaceHolder181 = new List<DeviceAction>() { RIO.Atom_J_RAD_DL_MIN_6 };
                public static List<DeviceAction> Seq_PlaceHolder182 = new List<DeviceAction>() { RIO.Atom_J_RAD_DL_MIN_7 };
                public static List<DeviceAction> Seq_PlaceHolder183 = new List<DeviceAction>() { RIO.Atom_J_RAD_DL_MIN_8 };
                public static List<DeviceAction> Seq_PlaceHolder184 = new List<DeviceAction>() { RIO.Atom_J_RAD_DL_MIN_9 };
                public static List<DeviceAction> Seq_PlaceHolder185 = new List<DeviceAction>() { RIO.Atom_Placeholder185 };
                public static List<DeviceAction> Seq_PlaceHolder186 = new List<DeviceAction>() { RIO.Atom_Placeholder186 };
                public static List<DeviceAction> Seq_PlaceHolder187 = new List<DeviceAction>() { RIO.Atom_Placeholder187 };
                public static List<DeviceAction> Seq_J_WPN_AG_RIP_QTY_16 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_7 };
                public static List<DeviceAction> Seq_J_WPN_AG_RIP_QTY_28 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_8 };
                // end of weapons

                // spare block
                public static List<DeviceAction> Seq_J_RAD_TCN_TAC_STEN = new List<DeviceAction>() { RIO.Atom_J_RAD_TCN_TAC_STEN };
                public static List<DeviceAction> Seq_J_RAD_TCN_TAC_ARCO = new List<DeviceAction>() { RIO.Atom_J_RAD_TCN_TAC_ARCO };
                public static List<DeviceAction> Seq_J_RAD_TCN_TAC_SHEL = new List<DeviceAction>() { RIO.Atom_J_RAD_TCN_TAC_SHEL };
                public static List<DeviceAction> Seq_J_RAD_TCN_TAC_TEXA = new List<DeviceAction>() { RIO.Atom_J_RAD_TCN_TAC_TEXA };

                public static List<DeviceAction> Seq_J_RAD_TCN_TAC_WASH = new List<DeviceAction>() { RIO.Atom_J_RAD_TCN_TAC_WASH };
                public static List<DeviceAction> Seq_J_RAD_TCN_TAC_ROOS = new List<DeviceAction>() { RIO.Atom_J_RAD_TCN_TAC_ROOS };
                public static List<DeviceAction> Seq_J_RAD_TCN_TAC_LINC = new List<DeviceAction>() { RIO.Atom_J_RAD_TCN_TAC_LINC };
                public static List<DeviceAction> Seq_J_RAD_TCN_TAC_TRUM = new List<DeviceAction>() { RIO.Atom_J_RAD_TCN_TAC_TRUM };
                public static List<DeviceAction> Seq_J_RAD_TCN_TAC_FORE = new List<DeviceAction>() { RIO.Atom_J_RAD_TCN_TAC_FORE };
                //public static List<DeviceAction> Seq_PlaceHolder198 = new List<DeviceAction>() { RIO.Atom_Placeholder198 };
                //public static List<DeviceAction> Seq_PlaceHolder199 = new List<DeviceAction>() { RIO.Atom_Placeholder199 };

                // block: radio 200-299
                public static List<DeviceAction> Seq_J_RAD_159 = new List<DeviceAction>() { RIO.Atom_J_RAD_159 };
                public static List<DeviceAction> Seq_J_RAD_159_USE_GUARD = new List<DeviceAction>() { RIO.Atom_J_RAD_159_USE_GUARD };
                public static List<DeviceAction> Seq_J_RAD_159_USE_MANUAL = new List<DeviceAction>() { RIO.Atom_J_RAD_159_USE_MANUAL };
                public static List<DeviceAction> Seq_J_RAD_159_USE_CHAN = new List<DeviceAction>() { RIO.Atom_J_RAD_159_USE_CHAN };
                public static List<DeviceAction> Seq_J_RAD_159_USE_CHAN_1 = new List<DeviceAction>() { RIO.Atom_J_RAD_159_USE_CHAN_1 };
                public static List<DeviceAction> Seq_J_RAD_159_USE_CHAN_2 = new List<DeviceAction>() { RIO.Atom_J_RAD_159_USE_CHAN_2 };
                public static List<DeviceAction> Seq_J_RAD_159_USE_CHAN_3 = new List<DeviceAction>() { RIO.Atom_J_RAD_159_USE_CHAN_3 };
                public static List<DeviceAction> Seq_J_RAD_159_USE_CHAN_4 = new List<DeviceAction>() { RIO.Atom_J_RAD_159_USE_CHAN_4 };
                public static List<DeviceAction> Seq_J_RAD_159_USE_CHAN_5 = new List<DeviceAction>() { RIO.Atom_J_RAD_159_USE_CHAN_5 };
                public static List<DeviceAction> Seq_J_RAD_159_USE_CHAN_6 = new List<DeviceAction>() { RIO.Atom_J_RAD_159_USE_CHAN_6 };
                public static List<DeviceAction> Seq_J_RAD_159_USE_CHAN_7 = new List<DeviceAction>() { RIO.Atom_J_RAD_159_USE_CHAN_7 };
                public static List<DeviceAction> Seq_J_RAD_159_USE_CHAN_8 = new List<DeviceAction>() { RIO.Atom_J_RAD_159_USE_CHAN_8 };
                public static List<DeviceAction> Seq_J_RAD_159_TUNE_MAN = new List<DeviceAction>() { RIO.Atom_J_RAD_159_TUNE_MAN };
                public static List<DeviceAction> Seq_J_RAD_159_SELECT_CHAN = new List<DeviceAction>() { RIO.Atom_J_RAD_159_SELECT_CHAN };
                public static List<DeviceAction> Seq_J_RAD_159_SELECT_MODE = new List<DeviceAction>() { RIO.Atom_J_RAD_159_SELECT_MODE };
                public static List<DeviceAction> Seq_J_RAD_159_TUNE_ATC = new List<DeviceAction>() { RIO.Atom_J_RAD_159_TUNE_ATC };
                public static List<DeviceAction> Seq_J_RAD_159_TUNE_TAC = new List<DeviceAction>() { RIO.Atom_J_RAD_159_TUNE_TAC };
                public static List<DeviceAction> Seq_J_RAD_182 = new List<DeviceAction>() { RIO.Atom_J_RAD_182 };
                public static List<DeviceAction> Seq_J_RAD_182_USE_GUARD = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_1 };
                public static List<DeviceAction> Seq_J_RAD_182_USE_MANUAL = new List<DeviceAction>() { RIO.Atom_J_RAD_182_USE_MANUAL };
                public static List<DeviceAction> Seq_J_RAD_182_USE_CHAN = new List<DeviceAction>() { RIO.Atom_J_RAD_182_USE_CHAN };
                public static List<DeviceAction> Seq_J_RAD_182_USE_CHAN_1 = new List<DeviceAction>() { RIO.Atom_J_RAD_182_USE_CHAN_1 };
                public static List<DeviceAction> Seq_J_RAD_182_USE_CHAN_2 = new List<DeviceAction>() { RIO.Atom_J_RAD_182_USE_CHAN_2 };
                public static List<DeviceAction> Seq_J_RAD_182_USE_CHAN_3 = new List<DeviceAction>() { RIO.Atom_J_RAD_182_USE_CHAN_3 };
                public static List<DeviceAction> Seq_J_RAD_182_USE_CHAN_4 = new List<DeviceAction>() { RIO.Atom_J_RAD_182_USE_CHAN_4 };
                public static List<DeviceAction> Seq_J_RAD_182_USE_CHAN_5 = new List<DeviceAction>() { RIO.Atom_J_RAD_182_USE_CHAN_5 };
                public static List<DeviceAction> Seq_J_RAD_182_USE_CHAN_6 = new List<DeviceAction>() { RIO.Atom_J_RAD_182_USE_CHAN_6 };
                public static List<DeviceAction> Seq_J_RAD_182_USE_CHAN_7 = new List<DeviceAction>() { RIO.Atom_J_RAD_182_USE_CHAN_7 };
                public static List<DeviceAction> Seq_J_RAD_182_USE_CHAN_8 = new List<DeviceAction>() { RIO.Atom_J_RAD_182_USE_CHAN_8 };
                public static List<DeviceAction> Seq_J_RAD_182_TUNE_MAN = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_4 };
                public static List<DeviceAction> Seq_J_RAD_182_SELECT_CHAN = new List<DeviceAction>() { RIO.Atom_J_RAD_182_SELECT_CHAN };
                public static List<DeviceAction> Seq_J_RAD_182_SELECT_MODE = new List<DeviceAction>() { RIO.Atom_J_RAD_182_SELECT_MODE };
                public static List<DeviceAction> Seq_J_RAD_182_TUNE_ATC = new List<DeviceAction>() { RIO.Atom_J_RAD_182_TUNE_ATC };
                public static List<DeviceAction> Seq_J_RAD_182_TUNE_TAC = new List<DeviceAction>() { RIO.Atom_J_RAD_182_TUNE_TAC };

                public static List<DeviceAction> Seq_J_RAD_DL = new List<DeviceAction>() { }; // rootnot endpoint (disable this command)
                public static List<DeviceAction> Seq_J_RAD_DL_SET_MODE = new List<DeviceAction>() { }; // not endpoint
                public static List<DeviceAction> Seq_J_RAD_DL_SET_FREQ_PRST = new List<DeviceAction>() { }; // not endpoint
                public static List<DeviceAction> Seq_J_RAD_DL_SET_HOST = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_7, RIO.Atom_J_MENU_OPTION_3 };
                public static List<DeviceAction> Seq_J_RAD_DL_TACT = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_7, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_1 };
                public static List<DeviceAction> Seq_J_RAD_DL_OFF = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_7, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_3 };
                public static List<DeviceAction> Seq_J_RAD_DL_FGHT = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_7, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_2 };

                public static List<DeviceAction> Seq_J_RAD_DL_SET_FREQ_1 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_7, RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_1 };
                public static List<DeviceAction> Seq_J_RAD_DL_SET_FREQ_2 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_7, RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_2 };
                public static List<DeviceAction> Seq_J_RAD_DL_SET_FREQ_3 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_7, RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_3 };
                public static List<DeviceAction> Seq_J_RAD_DL_SET_FREQ_4 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_7, RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_4 };
                public static List<DeviceAction> Seq_J_RAD_DL_SET_FREQ_5 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_7, RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_5 };
                public static List<DeviceAction> Seq_J_RAD_DL_SET_FREQ_6 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_7, RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_6 };
                public static List<DeviceAction> Seq_J_RAD_DL_SET_FREQ_7 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_7, RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_7 };
                public static List<DeviceAction> Seq_J_RAD_DL_SET_FREQ_8 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_7, RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_8 };

                public static List<DeviceAction> Seq_J_RAD_TCN = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_5 };
                public static List<DeviceAction> Seq_J_RAD_TCN_MODE = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_1 }; // show hint
                public static List<DeviceAction> Seq_J_RAD_TCN_MODE_OFF = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_1 };
                public static List<DeviceAction> Seq_J_RAD_TCN_MODE_REC = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_2 };
                public static List<DeviceAction> Seq_J_RAD_TCN_MODE_TR = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_3 };
                public static List<DeviceAction> Seq_J_RAD_TCN_MODE_AA = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_4 };
                public static List<DeviceAction> Seq_J_RAD_TCN_MODE_BCN = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_5 };

                public static List<DeviceAction> Seq_J_RAD_TCN_SEL_CHAN = new List<DeviceAction>() { RIO.Atom_J_RAD_TCN_SEL_CHAN };
                public static List<DeviceAction> Seq_J_RAD_TCN_SEL_GND_STN = new List<DeviceAction>() { RIO.Atom_J_RAD_TCN_SEL_GND_STN };
                public static List<DeviceAction> Seq_J_RAD_TCN_TUNE_TAC = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_4 };

                public static List<DeviceAction> Seq_J_RAD_TCN_T_CS_TSK = new List<DeviceAction>() { RIO.Atom_J_RAD_TCN_X, RIO.Atom_J_RAD_TCN_MAJ_03, RIO.Atom_J_RAD_TCN_MIN_1, };
                public static List<DeviceAction> Seq_J_RAD_TCN_T_CS_KBL = new List<DeviceAction>() { RIO.Atom_J_RAD_TCN_X, RIO.Atom_J_RAD_TCN_MAJ_06, RIO.Atom_J_RAD_TCN_MIN_7, };
                public static List<DeviceAction> Seq_J_RAD_TCN_T_CS_BTM = new List<DeviceAction>() { RIO.Atom_J_RAD_TCN_X, RIO.Atom_J_RAD_TCN_MAJ_01, RIO.Atom_J_RAD_TCN_MIN_6, };
                public static List<DeviceAction> Seq_J_RAD_TCN_T_CS_KTS = new List<DeviceAction>() { RIO.Atom_J_RAD_TCN_X, RIO.Atom_J_RAD_TCN_MAJ_04, RIO.Atom_J_RAD_TCN_MIN_4, };
                public static List<DeviceAction> Seq_J_RAD_TCN_T_CS_GTB = new List<DeviceAction>() { RIO.Atom_J_RAD_TCN_X, RIO.Atom_J_RAD_TCN_MAJ_02, RIO.Atom_J_RAD_TCN_MIN_5, };
                public static List<DeviceAction> Seq_J_RAD_TCN_T_CS_VAS = new List<DeviceAction>() { RIO.Atom_J_RAD_TCN_X, RIO.Atom_J_RAD_TCN_MAJ_02, RIO.Atom_J_RAD_TCN_MIN_2, };
                public static List<DeviceAction> Seq_J_RAD_TCN_T_NV_LSV = new List<DeviceAction>() { RIO.Atom_J_RAD_TCN_X, RIO.Atom_J_RAD_TCN_MAJ_01, RIO.Atom_J_RAD_TCN_MIN_2, };
                public static List<DeviceAction> Seq_J_RAD_TCN_T_NV_LAS = new List<DeviceAction>() { RIO.Atom_J_RAD_TCN_X, RIO.Atom_J_RAD_TCN_MAJ_11, RIO.Atom_J_RAD_TCN_MIN_6, };
                public static List<DeviceAction> Seq_J_RAD_TCN_T_NV_BLD = new List<DeviceAction>() { RIO.Atom_J_RAD_TCN_X, RIO.Atom_J_RAD_TCN_MAJ_11, RIO.Atom_J_RAD_TCN_MIN_4, };
                public static List<DeviceAction> Seq_J_RAD_TCN_T_NV_INS = new List<DeviceAction>() { RIO.Atom_J_RAD_TCN_X, RIO.Atom_J_RAD_TCN_MAJ_08, RIO.Atom_J_RAD_TCN_MIN_7, };
                public static List<DeviceAction> Seq_J_RAD_TCN_T_NV_MMM = new List<DeviceAction>() { RIO.Atom_J_RAD_TCN_X, RIO.Atom_J_RAD_TCN_MAJ_09, RIO.Atom_J_RAD_TCN_MIN_0, };
                public static List<DeviceAction> Seq_J_RAD_TCN_T_NV_GFS = new List<DeviceAction>() { RIO.Atom_J_RAD_TCN_X, RIO.Atom_J_RAD_TCN_MAJ_09, RIO.Atom_J_RAD_TCN_MIN_1, };
                public static List<DeviceAction> Seq_J_RAD_TCN_T_NV_GRL = new List<DeviceAction>() { RIO.Atom_J_RAD_TCN_X, RIO.Atom_J_RAD_TCN_MAJ_01, RIO.Atom_J_RAD_TCN_MIN_8, };
                public static List<DeviceAction> Seq_J_RAD_TCN_T_NV_PGS = new List<DeviceAction>() { RIO.Atom_J_RAD_TCN_X, RIO.Atom_J_RAD_TCN_MAJ_05, RIO.Atom_J_RAD_TCN_MIN_7, };
                public static List<DeviceAction> Seq_J_RAD_TCN_T_NV_BTY = new List<DeviceAction>() { RIO.Atom_J_RAD_TCN_X, RIO.Atom_J_RAD_TCN_MAJ_09, RIO.Atom_J_RAD_TCN_MIN_4, };
                public static List<DeviceAction> Seq_J_RAD_TCN_T_NV_EER = new List<DeviceAction>() { RIO.Atom_J_RAD_TCN_X, RIO.Atom_J_RAD_TCN_MAJ_09, RIO.Atom_J_RAD_TCN_MIN_9, };
                public static List<DeviceAction> Seq_J_RAD_TCN_T_NV_DAG = new List<DeviceAction>() { RIO.Atom_J_RAD_TCN_X, RIO.Atom_J_RAD_TCN_MAJ_07, RIO.Atom_J_RAD_TCN_MIN_9, };
                public static List<DeviceAction> Seq_J_RAD_TCN_T_NV_HEC = new List<DeviceAction>() { RIO.Atom_J_RAD_TCN_X, RIO.Atom_J_RAD_TCN_MAJ_07, RIO.Atom_J_RAD_TCN_MIN_4, };
                public static List<DeviceAction> Seq_J_RAD_TCN_T_NV_OAL = new List<DeviceAction>() { RIO.Atom_J_RAD_TCN_X, RIO.Atom_J_RAD_TCN_MAJ_12, RIO.Atom_J_RAD_TCN_MIN_4, };
                public static List<DeviceAction> Seq_J_RAD_TCN_T_NV_BIH = new List<DeviceAction>() { RIO.Atom_J_RAD_TCN_X, RIO.Atom_J_RAD_TCN_MAJ_03, RIO.Atom_J_RAD_TCN_MIN_3, };
                public static List<DeviceAction> Seq_J_RAD_TCN_T_NV_MVA = new List<DeviceAction>() { RIO.Atom_J_RAD_TCN_X, RIO.Atom_J_RAD_TCN_MAJ_09, RIO.Atom_J_RAD_TCN_MIN_8, };
                public static List<DeviceAction> Seq_J_RAD_TCN_T_PG_KCK = new List<DeviceAction>() { RIO.Atom_J_RAD_TCN_X, RIO.Atom_J_RAD_TCN_MAJ_08, RIO.Atom_J_RAD_TCN_MIN_9, };
                public static List<DeviceAction> Seq_J_RAD_TCN_T_PG_KSB = new List<DeviceAction>() { RIO.Atom_J_RAD_TCN_X, RIO.Atom_J_RAD_TCN_MAJ_08, RIO.Atom_J_RAD_TCN_MIN_4, };
                public static List<DeviceAction> Seq_J_RAD_TCN_T_PG_HDR = new List<DeviceAction>() { RIO.Atom_J_RAD_TCN_X, RIO.Atom_J_RAD_TCN_MAJ_04, RIO.Atom_J_RAD_TCN_MIN_7, };
                public static List<DeviceAction> Seq_J_RAD_TCN_T_PG_MA = new List<DeviceAction>() { RIO.Atom_J_RAD_TCN_X, RIO.Atom_J_RAD_TCN_MAJ_09, RIO.Atom_J_RAD_TCN_MIN_6, };
                public static List<DeviceAction> Seq_J_RAD_TCN_T_PG_SYZI = new List<DeviceAction>() { RIO.Atom_J_RAD_TCN_X, RIO.Atom_J_RAD_TCN_MAJ_09, RIO.Atom_J_RAD_TCN_MIN_4, };
                public static List<DeviceAction> Seq_J_RAD_TCN_T_STN = new List<DeviceAction>() { RIO.Atom_J_RAD_TCN_X, RIO.Atom_J_RAD_TCN_MAJ_07, RIO.Atom_J_RAD_TCN_MIN_4, };

                public static List<DeviceAction> Seq_J_RAD_182_MODE_TR = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_2 };
                public static List<DeviceAction> Seq_J_RAD_182_MODE_TRG = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_3 };
                public static List<DeviceAction> Seq_J_RAD_182_MODE_DF = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_4 };
                public static List<DeviceAction> Seq_J_RAD_182_MODE_TEST = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_5 };
                // end of radio

                // spare block
                public static List<DeviceAction> Seq_J_RAD_182_MODE_AM = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_6 };
                public static List<DeviceAction> Seq_J_RAD_182_MODE_FM = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_6 };
                public static List<DeviceAction> Seq_J_RAD_182_MODE = new List<DeviceAction>() { }; // hint
                public static List<DeviceAction> Seq_J_RAD_182_MODE_OFF = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_1 };

                public static List<DeviceAction> Seq_J_RAD_DL_HOST_STEN = new List<DeviceAction>() { RIO.Atom_J_RAD_DL_HOST_STEN };
                public static List<DeviceAction> Seq_J_RAD_DL_HOST_DARK = new List<DeviceAction>() { RIO.Atom_J_RAD_DL_HOST_DARK };
                public static List<DeviceAction> Seq_J_RAD_DL_HOST_FOCS = new List<DeviceAction>() { RIO.Atom_J_RAD_DL_HOST_FOCS };
                public static List<DeviceAction> Seq_J_RAD_DL_HOST_MAGC = new List<DeviceAction>() { RIO.Atom_J_RAD_DL_HOST_MAGC };
                public static List<DeviceAction> Seq_J_RAD_DL_HOST_OVRL = new List<DeviceAction>() { RIO.Atom_J_RAD_DL_HOST_OVRL };
                public static List<DeviceAction> Seq_J_RAD_DL_HOST_WIZR = new List<DeviceAction>() { RIO.Atom_J_RAD_DL_HOST_WIZR };

                // block: utility 300-399
                public static List<DeviceAction> Seq_J_UTIL_NAV = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5 };
                public static List<DeviceAction> Seq_J_UTIL_NAV_SEL_DEST_SPT = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_1 }; // not used
                public static List<DeviceAction> Seq_J_UTIL_NAV_REST_MSN_SPT = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_2 }; // base for others
                public static List<DeviceAction> Seq_J_UTIL_NAV_MAN_ENT_SPT = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_8 }; // for hostile zone
                public static List<DeviceAction> Seq_J_UTIL_NAV_MAP_SPT = new List<DeviceAction>() { }; // not endpoint
                public static List<DeviceAction> Seq_J_UTIL_NAV_MAP_SPT_1 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_1 };
                public static List<DeviceAction> Seq_J_UTIL_NAV_MAP_SPT_2 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_2 };
                public static List<DeviceAction> Seq_J_UTIL_NAV_MAP_SPT_3 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_3 };
                public static List<DeviceAction> Seq_J_UTIL_NAV_MAP_SPT_4 = new List<DeviceAction>() { RIO.Atom_J_UTIL_NAV_MAP_SPT_4 }; // not used 4-8
                public static List<DeviceAction> Seq_J_UTIL_NAV_MAP_SPT_5 = new List<DeviceAction>() { RIO.Atom_J_UTIL_NAV_MAP_SPT_5 };//
                public static List<DeviceAction> Seq_J_UTIL_NAV_MAP_SPT_6 = new List<DeviceAction>() { RIO.Atom_J_UTIL_NAV_MAP_SPT_6 };//
                public static List<DeviceAction> Seq_J_UTIL_NAV_MAP_FIX_PNT = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_4 };
                public static List<DeviceAction> Seq_J_UTIL_NAV_MAP_INIT_PNT = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_5 };
                public static List<DeviceAction> Seq_J_UTIL_NAV_SURF_TGT = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_6 };
                public static List<DeviceAction> Seq_J_UTIL_NAV_HOME_BASE = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_7 };

                public static List<DeviceAction> Seq_J_UTIL_NAV_REST_MORE = new List<DeviceAction>() { };  // not endpoint
                public static List<DeviceAction> Seq_J_UTIL_NAV_REST_MSN_SPT_1 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_1 };
                public static List<DeviceAction> Seq_J_UTIL_NAV_REST_MSN_SPT_2 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2 };
                public static List<DeviceAction> Seq_J_UTIL_NAV_REST_MSN_SPT_3 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_3 };
                public static List<DeviceAction> Seq_J_UTIL_NAV_REST_INIT_PT_1 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5 };
                public static List<DeviceAction> Seq_J_UTIL_NAV_REST_INIT_PT_2 = new List<DeviceAction>() { RIO.Atom_J_UTIL_NAV_REST_INIT_PT_2 }; //
                public static List<DeviceAction> Seq_J_UTIL_NAV_REST_FIX_PT = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4 };
                public static List<DeviceAction> Seq_J_UTIL_NAV_REST_MN_FIX_PT = new List<DeviceAction>() { RIO.Atom_J_UTIL_NAV_REST_MN_FIX_PT }; //
                public static List<DeviceAction> Seq_J_UTIL_NAV_REST_STGT_1 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_6 };
                public static List<DeviceAction> Seq_J_UTIL_NAV_REST_HOME = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_7 };
                public static List<DeviceAction> Seq_J_UTIL_NAV_DEF_PNT = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_1 }; // defense point
                public static List<DeviceAction> Seq_J_UTIL_NAV_HSTZONE = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2 }; // hostile zone

                public static List<DeviceAction> Seq_J_UTIL_CONTR = new List<DeviceAction>() { }; // not endpoint
                public static List<DeviceAction> Seq_J_UTIL_CONTR_NO_TALK = new List<DeviceAction>() { RIO.Atom_J_UTIL_CONTR_NO_TALK };
                public static List<DeviceAction> Seq_J_UTIL_CONTR_TALK = new List<DeviceAction>() { RIO.Atom_J_UTIL_CONTR_TALK };
                public static List<DeviceAction> Seq_J_UTIL_CONTR_EJECT_BTH = new List<DeviceAction>() { RIO.Atom_J_UTIL_CONTR_EJECT_BTH };
                public static List<DeviceAction> Seq_J_UTIL_CONTR_EJECT_SNG = new List<DeviceAction>() { RIO.Atom_J_UTIL_CONTR_EJECT_SNG };
                public static List<DeviceAction> Seq_J_UTIL_CONTR_ACTIVE = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_1 }; // new
                public static List<DeviceAction> Seq_J_UTIL_CONTR_INACTIVE = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_8, RIO.Atom_J_MENU_OPTION_1 }; // new
                public static List<DeviceAction> Seq_J_UTIL_CONTR_CALL = new List<DeviceAction>() { RIO.Atom_J_UTIL_CONTR_CALL };
                public static List<DeviceAction> Seq_J_UTIL_CONTR_NO_CALL = new List<DeviceAction>() { RIO.Atom_J_UTIL_CONTR_NO_CALL };

                public static List<DeviceAction> Seq_J_RESET = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_RESET, RIO.Atom_J_MENU_OPEN, RIO.Atom_J_MENU_CLOSE };

                public static List<DeviceAction> Seq_JESTER_Steerpoint_SP1 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_3, RIO.Atom_J_MENU_OPTION_1 }; //{ RIO.Atom_JESTER_Steerpoint_SP1 };
                public static List<DeviceAction> Seq_JESTER_Steerpoint_SP2 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_3, RIO.Atom_J_MENU_OPTION_2 }; //{ RIO.Atom_JESTER_Steerpoint_SP2 };
                public static List<DeviceAction> Seq_JESTER_Steerpoint_SP3 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_3, RIO.Atom_J_MENU_OPTION_3 }; //{ RIO.Atom_JESTER_Steerpoint_SP3 };
                public static List<DeviceAction> Seq_JESTER_Steerpoint_FP = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_3, RIO.Atom_J_MENU_OPTION_4 }; //{ RIO.Atom_JESTER_Steerpoint_FP };
                public static List<DeviceAction> Seq_JESTER_Steerpoint_IP = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_3, RIO.Atom_J_MENU_OPTION_5 }; //{ RIO.Atom_JESTER_Steerpoint_IP };
                public static List<DeviceAction> Seq_JESTER_Steerpoint_ST = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_3, RIO.Atom_J_MENU_OPTION_6 }; //{ RIO.Atom_JESTER_Steerpoint_ST };
                public static List<DeviceAction> Seq_JESTER_Steerpoint_HB = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_3, RIO.Atom_J_MENU_OPTION_7 }; //{ RIO.Atom_JESTER_Steerpoint_HB };
                public static List<DeviceAction> Seq_JESTER_Steerpoint_MAN = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_3, RIO.Atom_J_MENU_OPTION_8 }; //{ RIO.Atom_JESTER_Steerpoint_MAN };
                public static List<DeviceAction> Seq_LANTIRN_GPSZero = new List<DeviceAction>() { RIO.Atom_LANTIRN_GPSZero };
                public static List<DeviceAction> Seq_LANTIRN_ToggleFOV = new List<DeviceAction>() { RIO.Atom_LANTIRN_ToggleFOV };

                public static List<DeviceAction> Seq_J_INPUT_NUM_1 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_1 };
                public static List<DeviceAction> Seq_J_INPUT_NUM_2 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2 };
                public static List<DeviceAction> Seq_J_INPUT_NUM_3 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_3 };
                public static List<DeviceAction> Seq_J_INPUT_NUM_4 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4 };
                public static List<DeviceAction> Seq_J_INPUT_NUM_5 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5 };
                public static List<DeviceAction> Seq_J_INPUT_NUM_6 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_6 }; // 342
                public static List<DeviceAction> Seq_J_INPUT_NUM_7 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_7 };
                public static List<DeviceAction> Seq_J_INPUT_NUM_8 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_8, RIO.Atom_J_MENU_OPTION_8 };
                public static List<DeviceAction> Seq_J_INPUT_NUM_9 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_8, RIO.Atom_J_MENU_OPTION_7 };
                public static List<DeviceAction> Seq_J_INPUT_NUM_0 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_8, RIO.Atom_J_MENU_OPTION_6 }; // normal 0
                public static List<DeviceAction> Seq_J_INPUT_NUM_0_ = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_8 }; // first digit only (radio)
                public static List<DeviceAction> Seq_J_INPUT_NUM_00 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_1 };
                public static List<DeviceAction> Seq_J_INPUT_NUM_25 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2 };
                public static List<DeviceAction> Seq_J_INPUT_NUM_50 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_3 };
                public static List<DeviceAction> Seq_J_INPUT_NUM_75 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4 };

                public static List<DeviceAction> Seq_J_UTIL_NAV_MAP_MARKER = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_4 };
                public static List<DeviceAction> Seq_J_UTIL_NAV_GRD_ENABLE = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_1 };
                public static List<DeviceAction> Seq_J_UTIL_NAV_GRD_DSABLE = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_2 };
                public static List<DeviceAction> Seq_J_UTIL_NAV_GRD_CENTER = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_8 };
                public static List<DeviceAction> Seq_J_UTIL_NAV_GRD_REL_180 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_1 };
                public static List<DeviceAction> Seq_J_UTIL_NAV_GRD_REL_u30 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_2 };
                public static List<DeviceAction> Seq_J_UTIL_NAV_GRD_REL_u90 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_3 };
                public static List<DeviceAction> Seq_J_UTIL_NAV_GRD_REL_u120 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_4 };
                public static List<DeviceAction> Seq_J_UTIL_NAV_GRD_REL_d120 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_6 };
                public static List<DeviceAction> Seq_J_UTIL_NAV_GRD_REL_d90 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_7 };
                public static List<DeviceAction> Seq_J_UTIL_NAV_GRD_REL_d30 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_8 };
                public static List<DeviceAction> Seq_J_UTIL_NAV_GRD_ABS_0 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_1 };
                public static List<DeviceAction> Seq_J_UTIL_NAV_GRD_ABS_45 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_2 };
                public static List<DeviceAction> Seq_J_UTIL_NAV_GRD_ABS_90 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_3 };
                public static List<DeviceAction> Seq_J_UTIL_NAV_GRD_ABS_135 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_4 };
                public static List<DeviceAction> Seq_J_UTIL_NAV_GRD_ABS_180 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_5 };
                public static List<DeviceAction> Seq_J_UTIL_NAV_GRD_ABS_225 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_6 };
                public static List<DeviceAction> Seq_J_UTIL_NAV_GRD_ABS_270 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_7 };
                public static List<DeviceAction> Seq_J_UTIL_NAV_GRD_ABS_315 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_8 };
                public static List<DeviceAction> Seq_J_UTIL_NAV_GRD_COV_30 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_1 };
                public static List<DeviceAction> Seq_J_UTIL_NAV_GRD_COV_60 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_2 };
                public static List<DeviceAction> Seq_J_UTIL_NAV_GRD_COV_90 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_3 };
                public static List<DeviceAction> Seq_J_UTIL_NAV_GRD_COV_120 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_4 };
                public static List<DeviceAction> Seq_J_UTIL_NAV_GRD_COV_180 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_5 };
                public static List<DeviceAction> Seq_J_UTIL_NAV_GRD_1SCTR = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_7, RIO.Atom_J_MENU_OPTION_1 };
                public static List<DeviceAction> Seq_J_UTIL_NAV_GRD_2SCTR = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_7, RIO.Atom_J_MENU_OPTION_2 };
                public static List<DeviceAction> Seq_J_UTIL_NAV_GRD_3SCTR = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_7, RIO.Atom_J_MENU_OPTION_3 };
                public static List<DeviceAction> Seq_J_UTIL_NAV_GRD_4SCTR = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_7, RIO.Atom_J_MENU_OPTION_4 };
                public static List<DeviceAction> Seq_J_UTIL_NAV_GRD_5SCTR = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_7, RIO.Atom_J_MENU_OPTION_5 };
                public static List<DeviceAction> Seq_J_UTIL_NAV_GRD_6SCTR = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_7, RIO.Atom_J_MENU_OPTION_6 };
                public static List<DeviceAction> Seq_J_UTIL_NAV_GRD_MARKER = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_3 };

                //Supercarriers
                public static List<DeviceAction> Seq_J_RAD_DL_HOST_WASH = new List<DeviceAction>() { RIO.Atom_J_RAD_DL_HOST_WASH };
                public static List<DeviceAction> Seq_J_RAD_DL_HOST_ROOS = new List<DeviceAction>() { RIO.Atom_J_RAD_DL_HOST_ROOS };
                public static List<DeviceAction> Seq_J_RAD_DL_HOST_LINC = new List<DeviceAction>() { RIO.Atom_J_RAD_DL_HOST_LINC };
                public static List<DeviceAction> Seq_J_RAD_DL_HOST_TRUM = new List<DeviceAction>() { RIO.Atom_J_RAD_DL_HOST_TRUM };
                public static List<DeviceAction> Seq_J_RAD_DL_HOST_TICO = new List<DeviceAction>() { RIO.Atom_J_RAD_DL_HOST_TICO };
                public static List<DeviceAction> Seq_J_RAD_DL_HOST_FORE = new List<DeviceAction>() { RIO.Atom_J_RAD_DL_HOST_FORE };
                //public static List<DeviceAction> Seq_PlaceHolder388 = new List<DeviceAction>() { RIO.Atom_Placeholder388 };
                //public static List<DeviceAction> Seq_PlaceHolder389 = new List<DeviceAction>() { RIO.Atom_Placeholder389 };
                // end of utility

                // spare block
                public static List<DeviceAction> Seq_J_WLKMN_PLAY = new List<DeviceAction>() { RIO.Atom_J_WLKMN_PLAY };
                public static List<DeviceAction> Seq_J_WLKMN_STOP = new List<DeviceAction>() { RIO.Atom_J_WLKMN_STOP };
                public static List<DeviceAction> Seq_J_WLKMN_NEXT = new List<DeviceAction>() { RIO.Atom_J_WLKMN_NEXT };
                public static List<DeviceAction> Seq_J_WLKMN_PREV = new List<DeviceAction>() { RIO.Atom_J_WLKMN_PREV };
                public static List<DeviceAction> Seq_J_RDR_ASP_BEAM = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_1 };
                public static List<DeviceAction> Seq_J_RDR_ASP_NOSE = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_2 };
                public static List<DeviceAction> Seq_J_RDR_ASP_TAIL = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_3 };
                //public static List<DeviceAction> Seq_PlaceHolder397 = new List<DeviceAction>() { RIO.Atom_Placeholder397 };
                //public static List<DeviceAction> Seq_PlaceHolder398 = new List<DeviceAction>() { RIO.Atom_Placeholder398 };
                //public static List<DeviceAction> Seq_PlaceHolder399 = new List<DeviceAction>() { RIO.Atom_Placeholder399 };

                // block: defensive 400-499
                public static List<DeviceAction> Seq_J_DEF_CMS_MOD = new List<DeviceAction>() { };
                public static List<DeviceAction> Seq_J_DEF_CMS_MOD_OFF = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_3 };
                public static List<DeviceAction> Seq_J_DEF_CMS_MOD_MAN = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_2 };
                public static List<DeviceAction> Seq_J_DEF_CMS_MOD_AUTO = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_1 };

                public static List<DeviceAction> Seq_J_DEF_FLR_MOD = new List<DeviceAction>() { };
                public static List<DeviceAction> Seq_J_DEF_FLR_MOD_PILOT = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_1 };
                public static List<DeviceAction> Seq_J_DEF_FLR_MOD_NORM = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_2 };
                public static List<DeviceAction> Seq_J_DEF_FLR_MOD_MULTI = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_3 };

                // Chaff program
                public static List<DeviceAction> Seq_J_DEF_CHF_PGM = new List<DeviceAction>() { };
                public static List<DeviceAction> Seq_J_DEF_CHF_PGM_RR_12 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_3, RIO.Atom_J_MENU_OPTION_1 };
                public static List<DeviceAction> Seq_J_DEF_CHF_PGM_RR_46 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_3, RIO.Atom_J_MENU_OPTION_2 };
                public static List<DeviceAction> Seq_J_DEF_CHF_PGM_RR_86 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_3, RIO.Atom_J_MENU_OPTION_3 };
                public static List<DeviceAction> Seq_J_DEF_CHF_PGM_20_44 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_3, RIO.Atom_J_MENU_OPTION_4 };
                public static List<DeviceAction> Seq_J_DEF_CHF_PGM_20_84 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_3, RIO.Atom_J_MENU_OPTION_5 };
                public static List<DeviceAction> Seq_J_DEF_CHF_PGM_40_44 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_3, RIO.Atom_J_MENU_OPTION_6 };
                public static List<DeviceAction> Seq_J_DEF_CHF_PGM_40_84 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_3, RIO.Atom_J_MENU_OPTION_7 };
                public static List<DeviceAction> Seq_J_DEF_CHF_PGM_R1_12 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_3, RIO.Atom_J_MENU_OPTION_8 };

                // RWR display
                public static List<DeviceAction> Seq_J_DEF_RWR_DSP_TYP = new List<DeviceAction>() { };
                public static List<DeviceAction> Seq_J_DEF_RWR_NORM = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_1 };
                public static List<DeviceAction> Seq_J_DEF_RWR_AIRB = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_2 };
                public static List<DeviceAction> Seq_J_DEF_RWR_AAA = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_3 };
                public static List<DeviceAction> Seq_J_DEF_RWR_UNK = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_4 };
                public static List<DeviceAction> Seq_J_DEF_RWR_FRND = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_5 };

                // Jammer ON/OFF (toggle)
                public static List<DeviceAction> Seq_J_DEF_JMR_ON = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_7 };
                public static List<DeviceAction> Seq_J_DEF_JMR_SBY = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_7 };

                // Dispense Order
                public static List<DeviceAction> Seq_J_DEF_CMS_CTL_ORD = new List<DeviceAction>() { };
                public static List<DeviceAction> Seq_J_DEF_CMS_CHF_PGM = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_1 };
                public static List<DeviceAction> Seq_J_DEF_CMS_CHF_SNGL = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_2 };
                public static List<DeviceAction> Seq_J_DEF_CMS_CHF_TGHT = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_3 };
                public static List<DeviceAction> Seq_J_DEF_CMS_FLR_PGM = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_4 };
                public static List<DeviceAction> Seq_J_DEF_CMS_FLR_SNGL = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_5 };
                public static List<DeviceAction> Seq_J_DEF_CMS_FLR_TGHT = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_6 };

                // Flares program
                public static List<DeviceAction> Seq_J_DEF_FLR_PGM = new List<DeviceAction>() { };
                public static List<DeviceAction> Seq_J_DEF_FLR_PGM_1 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_1 };
                public static List<DeviceAction> Seq_J_DEF_FLR_PGM_2 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_2 };
                public static List<DeviceAction> Seq_J_DEF_FLR_PGM_3 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_3 };
                public static List<DeviceAction> Seq_J_DEF_FLR_PGM_4 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_4 };
                public static List<DeviceAction> Seq_J_DEF_FLR_PGM_5 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_5 };
                public static List<DeviceAction> Seq_J_DEF_FLR_PGM_6 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_6 };
                public static List<DeviceAction> Seq_J_DEF_FLR_PGM_7 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_7 };
                public static List<DeviceAction> Seq_J_DEF_FLR_PGM_8 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_8 };



                public static List<DeviceAction> Seq_LANTIRN_Srch_Any = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_1 };
                public static List<DeviceAction> Seq_LANTIRN_Srch_Any_Active = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_2 };
                public static List<DeviceAction> Seq_LANTIRN_Srch_Air = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_3 };
                public static List<DeviceAction> Seq_LANTIRN_Srch_Air_Active = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_4 };
                public static List<DeviceAction> Seq_LANTIRN_Srch_SAM_Active = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_5 };
                public static List<DeviceAction> Seq_LANTIRN_Srch_Armor_Active = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_6 };
                public static List<DeviceAction> Seq_LANTIRN_Srch_Vehicle = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_7 };
                public static List<DeviceAction> Seq_LANTIRN_Ships_Active = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_8 };

                public static List<DeviceAction> Seq_Atom_Laser_500 = new List<DeviceAction>() { RIO.Atom_J_MENU_CLOSE, RIO.Atom_J_MENU_OPEN, RIO.Atom_J_MENU_OPTION_7, RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_5 };
                public static List<DeviceAction> Seq_Atom_Laser_600 = new List<DeviceAction>() { RIO.Atom_J_MENU_CLOSE, RIO.Atom_J_MENU_OPEN, RIO.Atom_J_MENU_OPTION_7, RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_6 };
                public static List<DeviceAction> Seq_Atom_Laser_700 = new List<DeviceAction>() { RIO.Atom_J_MENU_CLOSE, RIO.Atom_J_MENU_OPEN, RIO.Atom_J_MENU_OPTION_7, RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_7 };

                public static List<DeviceAction> Seq_Atom_Laser_010 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_1 };
                public static List<DeviceAction> Seq_Atom_Laser_020 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2 };
                public static List<DeviceAction> Seq_Atom_Laser_030 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_3 };
                public static List<DeviceAction> Seq_Atom_Laser_040 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4 };
                public static List<DeviceAction> Seq_Atom_Laser_050 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5 };
                public static List<DeviceAction> Seq_Atom_Laser_060 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_6 };
                public static List<DeviceAction> Seq_Atom_Laser_070 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_7 };
                public static List<DeviceAction> Seq_Atom_Laser_080 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_8 };

                public static List<DeviceAction> Seq_Atom_Laser_001 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_1 };
                public static List<DeviceAction> Seq_Atom_Laser_002 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2 };
                public static List<DeviceAction> Seq_Atom_Laser_003 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_3 };
                public static List<DeviceAction> Seq_Atom_Laser_004 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4 };
                public static List<DeviceAction> Seq_Atom_Laser_005 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5 };
                public static List<DeviceAction> Seq_Atom_Laser_006 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_6 };
                public static List<DeviceAction> Seq_Atom_Laser_007 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_7 };
                public static List<DeviceAction> Seq_Atom_Laser_008 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_8 };

                //public static List<DeviceAction> Seq_PlaceHolder450 = new List<DeviceAction>() { RIO.Atom_Placeholder450 };
                //public static List<DeviceAction> Seq_PlaceHolder451 = new List<DeviceAction>() { RIO.Atom_Placeholder451 };
                //public static List<DeviceAction> Seq_PlaceHolder452 = new List<DeviceAction>() { RIO.Atom_Placeholder452 };
                //public static List<DeviceAction> Seq_PlaceHolder453 = new List<DeviceAction>() { RIO.Atom_Placeholder453 };
                //public static List<DeviceAction> Seq_PlaceHolder454 = new List<DeviceAction>() { RIO.Atom_Placeholder454 };
                //public static List<DeviceAction> Seq_PlaceHolder455 = new List<DeviceAction>() { RIO.Atom_Placeholder455 };
                //public static List<DeviceAction> Seq_PlaceHolder456 = new List<DeviceAction>() { RIO.Atom_Placeholder456 };
                //public static List<DeviceAction> Seq_PlaceHolder457 = new List<DeviceAction>() { RIO.Atom_Placeholder457 };
                //public static List<DeviceAction> Seq_PlaceHolder458 = new List<DeviceAction>() { RIO.Atom_Placeholder458 };
                //public static List<DeviceAction> Seq_PlaceHolder459 = new List<DeviceAction>() { RIO.Atom_Placeholder459 };
                //public static List<DeviceAction> Seq_PlaceHolder460 = new List<DeviceAction>() { RIO.Atom_Placeholder460 };
                //public static List<DeviceAction> Seq_PlaceHolder461 = new List<DeviceAction>() { RIO.Atom_Placeholder461 };
                //public static List<DeviceAction> Seq_PlaceHolder462 = new List<DeviceAction>() { RIO.Atom_Placeholder462 };
                //public static List<DeviceAction> Seq_PlaceHolder463 = new List<DeviceAction>() { RIO.Atom_Placeholder463 };
                //public static List<DeviceAction> Seq_PlaceHolder464 = new List<DeviceAction>() { RIO.Atom_Placeholder464 };
                //public static List<DeviceAction> Seq_PlaceHolder465 = new List<DeviceAction>() { RIO.Atom_Placeholder465 };
                //public static List<DeviceAction> Seq_PlaceHolder466 = new List<DeviceAction>() { RIO.Atom_Placeholder466 };
                //public static List<DeviceAction> Seq_PlaceHolder467 = new List<DeviceAction>() { RIO.Atom_Placeholder467 };
                //public static List<DeviceAction> Seq_PlaceHolder468 = new List<DeviceAction>() { RIO.Atom_Placeholder468 };
                //public static List<DeviceAction> Seq_PlaceHolder469 = new List<DeviceAction>() { RIO.Atom_Placeholder469 };
                //public static List<DeviceAction> Seq_PlaceHolder470 = new List<DeviceAction>() { RIO.Atom_Placeholder470 };
                //public static List<DeviceAction> Seq_PlaceHolder471 = new List<DeviceAction>() { RIO.Atom_Placeholder471 };
                //public static List<DeviceAction> Seq_PlaceHolder472 = new List<DeviceAction>() { RIO.Atom_Placeholder472 };
                //public static List<DeviceAction> Seq_PlaceHolder473 = new List<DeviceAction>() { RIO.Atom_Placeholder473 };
                //public static List<DeviceAction> Seq_PlaceHolder474 = new List<DeviceAction>() { RIO.Atom_Placeholder474 };
                //public static List<DeviceAction> Seq_PlaceHolder475 = new List<DeviceAction>() { RIO.Atom_Placeholder475 };
                //public static List<DeviceAction> Seq_PlaceHolder476 = new List<DeviceAction>() { RIO.Atom_Placeholder476 };
                //public static List<DeviceAction> Seq_PlaceHolder477 = new List<DeviceAction>() { RIO.Atom_Placeholder477 };
                //public static List<DeviceAction> Seq_PlaceHolder478 = new List<DeviceAction>() { RIO.Atom_Placeholder478 };
                //public static List<DeviceAction> Seq_PlaceHolder479 = new List<DeviceAction>() { RIO.Atom_Placeholder479 };
                //public static List<DeviceAction> Seq_PlaceHolder480 = new List<DeviceAction>() { RIO.Atom_Placeholder480 };
                //public static List<DeviceAction> Seq_PlaceHolder481 = new List<DeviceAction>() { RIO.Atom_Placeholder481 };
                //public static List<DeviceAction> Seq_PlaceHolder482 = new List<DeviceAction>() { RIO.Atom_Placeholder482 };
                //public static List<DeviceAction> Seq_PlaceHolder483 = new List<DeviceAction>() { RIO.Atom_Placeholder483 };
                //public static List<DeviceAction> Seq_PlaceHolder484 = new List<DeviceAction>() { RIO.Atom_Placeholder484 };
                //public static List<DeviceAction> Seq_PlaceHolder485 = new List<DeviceAction>() { RIO.Atom_Placeholder485 };
                //public static List<DeviceAction> Seq_PlaceHolder486 = new List<DeviceAction>() { RIO.Atom_Placeholder486 };
                //public static List<DeviceAction> Seq_PlaceHolder487 = new List<DeviceAction>() { RIO.Atom_Placeholder487 };
                //public static List<DeviceAction> Seq_PlaceHolder488 = new List<DeviceAction>() { RIO.Atom_Placeholder488 };
                //public static List<DeviceAction> Seq_PlaceHolder489 = new List<DeviceAction>() { RIO.Atom_Placeholder489 };
                // end of defensive

                // spare block
                //public static List<DeviceAction> Seq_PlaceHolder490 = new List<DeviceAction>() { RIO.Atom_Placeholder490 };
                //public static List<DeviceAction> Seq_PlaceHolder491 = new List<DeviceAction>() { RIO.Atom_Placeholder491 };
                //public static List<DeviceAction> Seq_PlaceHolder492 = new List<DeviceAction>() { RIO.Atom_Placeholder492 };
                //public static List<DeviceAction> Seq_PlaceHolder493 = new List<DeviceAction>() { RIO.Atom_Placeholder493 };
                //public static List<DeviceAction> Seq_PlaceHolder494 = new List<DeviceAction>() { RIO.Atom_Placeholder494 };
                //public static List<DeviceAction> Seq_PlaceHolder495 = new List<DeviceAction>() { RIO.Atom_Placeholder495 };
                //public static List<DeviceAction> Seq_PlaceHolder496 = new List<DeviceAction>() { RIO.Atom_Placeholder496 };
                //public static List<DeviceAction> Seq_PlaceHolder497 = new List<DeviceAction>() { RIO.Atom_Placeholder497 };
                //public static List<DeviceAction> Seq_PlaceHolder498 = new List<DeviceAction>() { RIO.Atom_Placeholder498 };
                //public static List<DeviceAction> Seq_PlaceHolder499 = new List<DeviceAction>() { RIO.Atom_Placeholder499 };

                // 500-600 RIO misc

                // startup
                public static List<DeviceAction> Seq_J_STRT_ABORT = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_3 };
                public static List<DeviceAction> Seq_J_STRT_INS_FINE = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4 };
                public static List<DeviceAction> Seq_J_STRT_INS_MIN_WPN = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5 };
                public static List<DeviceAction> Seq_J_STRT_INS_COARSE = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_6 };
                public static List<DeviceAction> Seq_J_STRT_INS_NOW = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_7 };

                public static List<DeviceAction> Seq_J_STRT_CHECK = new List<DeviceAction>() { RIO.Atom_J_STRT_CHECK };
                public static List<DeviceAction> Seq_J_STRT_LOUD_CLR = new List<DeviceAction>() { RIO.Atom_J_STRT_LOUD_CLR };
                public static List<DeviceAction> Seq_J_STRT_PAUSE = new List<DeviceAction>() { RIO.Atom_J_STRT_PAUSE };
                public static List<DeviceAction> Seq_J_STRT_STARTUP = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_3 };
                public static List<DeviceAction> Seq_J_STRT_ASSISTED = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4 };

                // block 600-700: AI pilot
                public static List<DeviceAction> Seq_I_ALT = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5 }; // to menus
                public static List<DeviceAction> Seq_I_ALT_ANG_1 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_1 };
                public static List<DeviceAction> Seq_I_ALT_ANG_5 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_2 };
                public static List<DeviceAction> Seq_I_ALT_ANG_10 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_3 };
                public static List<DeviceAction> Seq_I_ALT_ANG_15 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_4 };
                public static List<DeviceAction> Seq_I_ALT_ANG_20 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_5 };
                public static List<DeviceAction> Seq_I_ALT_ANG_25 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_6 };
                public static List<DeviceAction> Seq_I_ALT_ANG_30 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_7 };
                public static List<DeviceAction> Seq_I_ALT_ANG_35 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_5, RIO.Atom_J_MENU_OPTION_8 };
                public static List<DeviceAction> Seq_I_ALT_CHG = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4 };
                public static List<DeviceAction> Seq_I_ALT_DESC_10K = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_1 };
                public static List<DeviceAction> Seq_I_ALT_DESC_5K = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_2 };
                public static List<DeviceAction> Seq_I_ALT_DESC_1K = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_3 };
                public static List<DeviceAction> Seq_I_ALT_DESC_500 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_4 };
                public static List<DeviceAction> Seq_I_ALT_CLMB_500 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_5 };
                public static List<DeviceAction> Seq_I_ALT_CLMB_1K = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_6 };
                public static List<DeviceAction> Seq_I_ALT_CLMB_5K = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_7 };
                public static List<DeviceAction> Seq_I_ALT_CLMB_10K = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_4, RIO.Atom_J_MENU_OPTION_8 };
                public static List<DeviceAction> Seq_I_SPD_MINUS_200 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_3, RIO.Atom_J_MENU_OPTION_1 };
                public static List<DeviceAction> Seq_I_SPD_MINUS_100 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_3, RIO.Atom_J_MENU_OPTION_2 };
                public static List<DeviceAction> Seq_I_SPD_MINUS_50 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_3, RIO.Atom_J_MENU_OPTION_3 };
                public static List<DeviceAction> Seq_I_SPD_PLUS_50 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_3, RIO.Atom_J_MENU_OPTION_4 };
                public static List<DeviceAction> Seq_I_SPD_PLUS_100 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_3, RIO.Atom_J_MENU_OPTION_5 };
                public static List<DeviceAction> Seq_I_SPD_PLUS_200 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_3, RIO.Atom_J_MENU_OPTION_6 };
                public static List<DeviceAction> Seq_I_DIR_N = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_1 };
                public static List<DeviceAction> Seq_I_DIR_NE = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_2 };
                public static List<DeviceAction> Seq_I_DIR_E = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_3 };
                public static List<DeviceAction> Seq_I_DIR_SE = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_4 };
                public static List<DeviceAction> Seq_I_DIR_S = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_5 };
                public static List<DeviceAction> Seq_I_DIR_SW = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_6 };
                public static List<DeviceAction> Seq_I_DIR_W = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_7 };
                public static List<DeviceAction> Seq_I_DIR_NW = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2, RIO.Atom_J_MENU_OPTION_8 };
                public static List<DeviceAction> Seq_I_DIR = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_2 };
                public static List<DeviceAction> Seq_I_DIR_CHG_L45 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_5 };
                public static List<DeviceAction> Seq_I_DIR_CHG_L30 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_6 };
                public static List<DeviceAction> Seq_I_DIR_CHG_L10 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_7 };
                public static List<DeviceAction> Seq_I_DIR_CHG_L5 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_8 };
                public static List<DeviceAction> Seq_I_DIR_CHG_R5 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_1 };
                public static List<DeviceAction> Seq_I_DIR_CHG_R10 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_2 };
                public static List<DeviceAction> Seq_I_DIR_CHG_R30 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_3 };
                public static List<DeviceAction> Seq_I_DIR_CHG_R45 = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_4 };
                public static List<DeviceAction> Seq_I_DIR_HOLD_CRS = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_7 }; // now 7 was 8 
                public static List<DeviceAction> Seq_I_SPD = new List<DeviceAction>() { RIO.Atom_I_SPD };
                public static List<DeviceAction> Seq_I_DIR_CHG = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_1 };

                public static List<DeviceAction> Seq_I_SPT_FLYTO = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_1 }; // Orbit/fly to destination: 
                public static List<DeviceAction> Seq_I_SPT_ORBIT = new List<DeviceAction>() { RIO.Atom_J_MENU_OPTION_6, RIO.Atom_J_MENU_OPTION_3, RIO.Atom_J_MENU_OPTION_1, RIO.Atom_J_MENU_OPTION_2 }; // Orbit/orbit destination: 
                //public static List<DeviceAction> Seq_PlaceHolder646 = new List<DeviceAction>() { RIO.Atom_Placeholder646 }; // Orbit/fly to map marker add: RIO.Atom_J_MENU_OPTION_6 RIO.Atom_J_MENU_OPTION_2 + (submenu 1=bullseye,..)
                //public static List<DeviceAction> Seq_PlaceHolder647 = new List<DeviceAction>() { RIO.Atom_Placeholder647 }; // Orbit/orbit map marker add: RIO.Atom_J_MENU_OPTION_6 RIO.Atom_J_MENU_OPTION_4 + (submenu 1=bullseye,..)
                //public static List<DeviceAction> Seq_PlaceHolder648 = new List<DeviceAction>() { RIO.Atom_Placeholder648 };
                //public static List<DeviceAction> Seq_PlaceHolder649 = new List<DeviceAction>() { RIO.Atom_Placeholder649 };
                //public static List<DeviceAction> Seq_PlaceHolder650 = new List<DeviceAction>() { RIO.Atom_Placeholder650 };
                //public static List<DeviceAction> Seq_PlaceHolder651 = new List<DeviceAction>() { RIO.Atom_Placeholder651 };
                //public static List<DeviceAction> Seq_PlaceHolder652 = new List<DeviceAction>() { RIO.Atom_Placeholder652 };
                //public static List<DeviceAction> Seq_PlaceHolder653 = new List<DeviceAction>() { RIO.Atom_Placeholder653 };
                //public static List<DeviceAction> Seq_PlaceHolder654 = new List<DeviceAction>() { RIO.Atom_Placeholder654 };
                //public static List<DeviceAction> Seq_PlaceHolder655 = new List<DeviceAction>() { RIO.Atom_Placeholder655 };
                //public static List<DeviceAction> Seq_PlaceHolder656 = new List<DeviceAction>() { RIO.Atom_Placeholder656 };
                //public static List<DeviceAction> Seq_PlaceHolder657 = new List<DeviceAction>() { RIO.Atom_Placeholder657 };
                //public static List<DeviceAction> Seq_PlaceHolder658 = new List<DeviceAction>() { RIO.Atom_Placeholder658 };
                //public static List<DeviceAction> Seq_PlaceHolder659 = new List<DeviceAction>() { RIO.Atom_Placeholder659 };
                //public static List<DeviceAction> Seq_PlaceHolder660 = new List<DeviceAction>() { RIO.Atom_Placeholder660 };
                //public static List<DeviceAction> Seq_PlaceHolder661 = new List<DeviceAction>() { RIO.Atom_Placeholder661 };
                //public static List<DeviceAction> Seq_PlaceHolder662 = new List<DeviceAction>() { RIO.Atom_Placeholder662 };
                //public static List<DeviceAction> Seq_PlaceHolder663 = new List<DeviceAction>() { RIO.Atom_Placeholder663 };
                //public static List<DeviceAction> Seq_PlaceHolder664 = new List<DeviceAction>() { RIO.Atom_Placeholder664 };
                //public static List<DeviceAction> Seq_PlaceHolder665 = new List<DeviceAction>() { RIO.Atom_Placeholder665 };
                //public static List<DeviceAction> Seq_PlaceHolder666 = new List<DeviceAction>() { RIO.Atom_Placeholder666 };
                //public static List<DeviceAction> Seq_PlaceHolder667 = new List<DeviceAction>() { RIO.Atom_Placeholder667 };
                //public static List<DeviceAction> Seq_PlaceHolder668 = new List<DeviceAction>() { RIO.Atom_Placeholder668 };
                //public static List<DeviceAction> Seq_PlaceHolder669 = new List<DeviceAction>() { RIO.Atom_Placeholder669 };
                //public static List<DeviceAction> Seq_PlaceHolder670 = new List<DeviceAction>() { RIO.Atom_Placeholder670 };
                //public static List<DeviceAction> Seq_PlaceHolder671 = new List<DeviceAction>() { RIO.Atom_Placeholder671 };
                //public static List<DeviceAction> Seq_PlaceHolder672 = new List<DeviceAction>() { RIO.Atom_Placeholder672 };
                //public static List<DeviceAction> Seq_PlaceHolder673 = new List<DeviceAction>() { RIO.Atom_Placeholder673 };
                //public static List<DeviceAction> Seq_PlaceHolder674 = new List<DeviceAction>() { RIO.Atom_Placeholder674 };
                //public static List<DeviceAction> Seq_PlaceHolder675 = new List<DeviceAction>() { RIO.Atom_Placeholder675 };
                //public static List<DeviceAction> Seq_PlaceHolder676 = new List<DeviceAction>() { RIO.Atom_Placeholder676 };
                //public static List<DeviceAction> Seq_PlaceHolder677 = new List<DeviceAction>() { RIO.Atom_Placeholder677 };
                //public static List<DeviceAction> Seq_PlaceHolder678 = new List<DeviceAction>() { RIO.Atom_Placeholder678 };
                //public static List<DeviceAction> Seq_PlaceHolder679 = new List<DeviceAction>() { RIO.Atom_Placeholder679 };
                //public static List<DeviceAction> Seq_PlaceHolder680 = new List<DeviceAction>() { RIO.Atom_Placeholder680 };
                //public static List<DeviceAction> Seq_PlaceHolder681 = new List<DeviceAction>() { RIO.Atom_Placeholder681 };
                //public static List<DeviceAction> Seq_PlaceHolder682 = new List<DeviceAction>() { RIO.Atom_Placeholder682 };
                //public static List<DeviceAction> Seq_PlaceHolder683 = new List<DeviceAction>() { RIO.Atom_Placeholder683 };
                //public static List<DeviceAction> Seq_PlaceHolder684 = new List<DeviceAction>() { RIO.Atom_Placeholder684 };
                //public static List<DeviceAction> Seq_PlaceHolder685 = new List<DeviceAction>() { RIO.Atom_Placeholder685 };
                //public static List<DeviceAction> Seq_PlaceHolder686 = new List<DeviceAction>() { RIO.Atom_Placeholder686 };
                //public static List<DeviceAction> Seq_PlaceHolder687 = new List<DeviceAction>() { RIO.Atom_Placeholder687 };
                //public static List<DeviceAction> Seq_PlaceHolder688 = new List<DeviceAction>() { RIO.Atom_Placeholder688 };
                //public static List<DeviceAction> Seq_PlaceHolder689 = new List<DeviceAction>() { RIO.Atom_Placeholder689 };
                // end of AI pilot

                // spare block
                //public static List<DeviceAction> Seq_PlaceHolder690 = new List<DeviceAction>() { RIO.Atom_Placeholder690 };
                //public static List<DeviceAction> Seq_PlaceHolder691 = new List<DeviceAction>() { RIO.Atom_Placeholder691 };
                //public static List<DeviceAction> Seq_PlaceHolder692 = new List<DeviceAction>() { RIO.Atom_Placeholder692 };
                //public static List<DeviceAction> Seq_PlaceHolder693 = new List<DeviceAction>() { RIO.Atom_Placeholder693 };
                //public static List<DeviceAction> Seq_PlaceHolder694 = new List<DeviceAction>() { RIO.Atom_Placeholder694 };
                //public static List<DeviceAction> Seq_PlaceHolder695 = new List<DeviceAction>() { RIO.Atom_Placeholder695 };
                //public static List<DeviceAction> Seq_PlaceHolder696 = new List<DeviceAction>() { RIO.Atom_Placeholder696 };
                //public static List<DeviceAction> Seq_PlaceHolder697 = new List<DeviceAction>() { RIO.Atom_Placeholder697 };
                //public static List<DeviceAction> Seq_PlaceHolder698 = new List<DeviceAction>() { RIO.Atom_Placeholder698 };
                //public static List<DeviceAction> Seq_PlaceHolder699 = new List<DeviceAction>() { RIO.Atom_Placeholder699 };

            }

            public class Special
            {
            }

        }

    }

}
