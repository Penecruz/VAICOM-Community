using System;
using System.Collections.Generic;

namespace VAICOM.Extensions.RIO
{

    // individual commands, get added to commands.all
    public static partial class Commands
    {
        // rio commands: 23200 - 23999 (some 700 slots)

        public static Dictionary<string, CommandInfo> all = new Dictionary<string, CommandInfo>(StringComparer.OrdinalIgnoreCase)
        {
            // 1-2 some test comamnds 
            { "wMsgJ_CANOPY_OPEN" ,             new CommandInfo { uniqueid = 23201, name = "wMsgJ_CANOPY_OPEN",             displayname = Labels.aicommands["wMsgJ_CANOPY_OPEN"], enabled = true                } },
            { "wMsgJ_CANOPY_CLOSE",             new CommandInfo { uniqueid = 23202, name = "wMsgJ_CANOPY_CLOSE",            displayname = Labels.aicommands["wMsgJ_CANOPY_CLOSE"], enabled = true               } },
            //...

            // menu control + radar: id 23200 - 23299
            
            // block: menu control
            { "wMsgJ_MENU_TOGGLE" ,             new CommandInfo { uniqueid = 23200, name = "wMsgJ_MENU_TOGGLE",             displayname = Labels.aicommands["wMsgJ_MENU_TOGGLE"]        } }, // block disabled
            { "wMsgJ_MENU_OPTION_1" ,           new CommandInfo { uniqueid = 23201, name = "wMsgJ_MENU_OPTION_1",           displayname = Labels.aicommands["wMsgJ_MENU_OPTION_1"]      } },
            { "wMsgJ_MENU_OPTION_2" ,           new CommandInfo { uniqueid = 23202, name = "wMsgJ_MENU_OPTION_2",           displayname = Labels.aicommands["wMsgJ_MENU_OPTION_2"]      } },
            { "wMsgJ_MENU_OPTION_3" ,           new CommandInfo { uniqueid = 23203, name = "wMsgJ_MENU_OPTION_3",           displayname = Labels.aicommands["wMsgJ_MENU_OPTION_3"]       } },
            { "wMsgJ_MENU_OPTION_4" ,           new CommandInfo { uniqueid = 23204, name = "wMsgJ_MENU_OPTION_4",           displayname = Labels.aicommands["wMsgJ_MENU_OPTION_4"]     } },
            { "wMsgJ_MENU_OPTION_5" ,           new CommandInfo { uniqueid = 23205, name = "wMsgJ_MENU_OPTION_5",           displayname = Labels.aicommands["wMsgJ_MENU_OPTION_5"]     } },
            { "wMsgJ_MENU_OPTION_6" ,           new CommandInfo { uniqueid = 23206, name = "wMsgJ_MENU_OPTION_6",           displayname = Labels.aicommands["wMsgJ_MENU_OPTION_6"]   } },
            { "wMsgJ_MENU_OPTION_7" ,           new CommandInfo { uniqueid = 23207, name = "wMsgJ_MENU_OPTION_7",           displayname = Labels.aicommands["wMsgJ_MENU_OPTION_7"]       } },
            { "wMsgJ_MENU_OPTION_8" ,           new CommandInfo { uniqueid = 23208, name = "wMsgJ_MENU_OPTION_8",           displayname = Labels.aicommands["wMsgJ_MENU_OPTION_8"]     } },
            { "wMsgJ_MENU_DIR_D" ,              new CommandInfo { uniqueid = 23209, name = "wMsgJ_MENU_DIR_D",              displayname = Labels.aicommands["wMsgJ_MENU_DIR_D"]         } },
            { "wMsgJ_MENU_DIR_DL" ,             new CommandInfo { uniqueid = 23210, name = "wMsgJ_MENU_DIR_DL",             displayname = Labels.aicommands["wMsgJ_MENU_DIR_DL"]        } },
            { "wMsgJ_MENU_DIR_DR" ,             new CommandInfo { uniqueid = 23211, name = "wMsgJ_MENU_DIR_DR",             displayname = Labels.aicommands["wMsgJ_MENU_DIR_DR"]        } },
            { "wMsgJ_MENU_DIR_L" ,              new CommandInfo { uniqueid = 23212, name = "wMsgJ_MENU_DIR_L",              displayname = Labels.aicommands["wMsgJ_MENU_DIR_L"]         } },
            { "wMsgJ_MENU_DIR_R" ,              new CommandInfo { uniqueid = 23213, name = "wMsgJ_MENU_DIR_R",              displayname = Labels.aicommands["wMsgJ_MENU_DIR_R"]         } },
            { "wMsgJ_MENU_DIR_U" ,              new CommandInfo { uniqueid = 23214, name = "wMsgJ_MENU_DIR_U",              displayname = Labels.aicommands["wMsgJ_MENU_DIR_U"]         } },
            { "wMsgJ_MENU_DIR_UL" ,             new CommandInfo { uniqueid = 23215, name = "wMsgJ_MENU_DIR_UL",             displayname = Labels.aicommands["wMsgJ_MENU_DIR_UL"]        } },
            { "wMsgJ_MENU_DIR_UR" ,             new CommandInfo { uniqueid = 23216, name = "wMsgJ_MENU_DIR_UR",             displayname = Labels.aicommands["wMsgJ_MENU_DIR_UR"]        } },
            { "wMsgJ_MENU_OPEN" ,               new CommandInfo { uniqueid = 23217, name = "wMsgJ_MENU_OPEN",               displayname = Labels.aicommands["wMsgJ_MENU_OPEN"]          } },
            { "wMsgJ_MENU_CLOSE" ,              new CommandInfo { uniqueid = 23218, name = "wMsgJ_MENU_CLOSE",              displayname = Labels.aicommands["wMsgJ_MENU_CLOSE"]       } },
            { "wMsgJ_MENU_CONTEXT" ,            new CommandInfo { uniqueid = 23219, name = "wMsgJ_MENU_CONTEXT",            displayname = Labels.aicommands["wMsgJ_MENU_CONTEXT"]       } },
            { "wMsgJ_MENU_MAIN" ,               new CommandInfo { uniqueid = 23220, name = "wMsgJ_MENU_MAIN",               displayname = Labels.aicommands["wMsgJ_MENU_MAIN"]              } },
            { "wMsgJ_MENU_CTXT_CLOSE" ,         new CommandInfo { uniqueid = 23221, name = "wMsgJ_MENU_CTXT_CLOSE",         displayname = Labels.aicommands["wMsgJ_MENU_CTXT_CLOSE"]        } },
            { "wMsgJ_MENU_MAIN_CLOSE" ,         new CommandInfo { uniqueid = 23222, name = "wMsgJ_MENU_MAIN_CLOSE",         displayname = Labels.aicommands["wMsgJ_MENU_MAIN_CLOSE"]        } },

            { "wMsgJESTER_LANTIRN_inhibit_auto_designate" ,     new CommandInfo { uniqueid = 23223, name = "wMsgJESTER_LANTIRN_inhibit_auto_designate", displayname = Labels.aicommands["wMsgJESTER_LANTIRN_inhibit_auto_designate"], enabled = true        } },//
            { "wMsgJESTER_LANTIRN_track_target_id" ,            new CommandInfo { uniqueid = 23224, name = "wMsgJESTER_LANTIRN_track_target_id", displayname = Labels.aicommands["wMsgJESTER_LANTIRN_track_target_id"], enabled = false     } }, //na
            { "wMsgJESTER_LANTIRN_track_zone_id" ,              new CommandInfo { uniqueid = 23225, name = "wMsgJESTER_LANTIRN_track_zone_id", displayname = Labels.aicommands["wMsgJESTER_LANTIRN_track_zone_id"], enabled = false        } },//na
            { "wMsgJESTER_LANTIRN_designate" ,                  new CommandInfo { uniqueid = 23226, name = "wMsgJESTER_LANTIRN_designate",            displayname = Labels.aicommands["wMsgJESTER_LANTIRN_designate"], enabled = true        } },//
            //{ "wMsgKNEEBOARD_Laser100" ,                        new CommandInfo { uniqueid = 23227, name = "wMsgKNEEBOARD_Laser100",            displayname = Labels.aicommands["wMsgKNEEBOARD_Laser100"], enabled = true        } },//
            //{ "wMsgKNEEBOARD_Laser10" ,                         new CommandInfo { uniqueid = 23228, name = "wMsgKNEEBOARD_Laser10",            displayname = Labels.aicommands["wMsgKNEEBOARD_Laser10"], enabled = true        } },//
            //{ "wMsgKNEEBOARD_Laser1" ,                          new CommandInfo { uniqueid = 23229, name = "wMsgKNEEBOARD_Laser1",            displayname = Labels.aicommands["wMsgKNEEBOARD_Laser1"], enabled = true        } },//
            // end of menu control

            // spare block
            { "wMsgLANTIRN_ToggleWHOTBHOT" ,        new CommandInfo { uniqueid = 23230, name = "wMsgLANTIRN_ToggleWHOTBHOT",          displayname = Labels.aicommands["wMsgLANTIRN_ToggleWHOTBHOT"], enabled = true          } }, //
            { "wMsgLANTIRN_LaserLatched" ,          new CommandInfo { uniqueid = 23231, name = "wMsgLANTIRN_LaserLatched",            displayname = Labels.aicommands["wMsgLANTIRN_LaserLatched"], enabled = true          } },
            { "wMsgLANTIRN_Laser_ARM" ,             new CommandInfo { uniqueid = 23232, name = "wMsgLANTIRN_Laser_ARM",               displayname = Labels.aicommands["wMsgLANTIRN_Laser_ARM"], enabled = true          } },
            { "wMsgLANTIRN_Laser_ARM_Toggle" ,      new CommandInfo { uniqueid = 23233, name = "wMsgLANTIRN_Laser_ARM_Toggle",        displayname = Labels.aicommands["wMsgLANTIRN_Laser_ARM_Toggle"], enabled = true          } },
            { "wMsgLANTIRN_Undesignate" ,           new CommandInfo { uniqueid = 23234, name = "wMsgLANTIRN_Undesignate",             displayname = Labels.aicommands["wMsgLANTIRN_Undesignate"], enabled = true          } },
            { "wMsgLANTIRN_QHUD_QADL" ,             new CommandInfo { uniqueid = 23235, name = "wMsgLANTIRN_QHUD_QADL",               displayname = Labels.aicommands["wMsgLANTIRN_QHUD_QADL"], enabled = true          } },
            { "wMsgLANTIRN_QSNO" ,                  new CommandInfo { uniqueid = 23236, name = "wMsgLANTIRN_QSNO",                    displayname = Labels.aicommands["wMsgLANTIRN_QSNO"], enabled = true          } },
            { "wMsgLANTIRN_QDES" ,                  new CommandInfo { uniqueid = 23237, name = "wMsgLANTIRN_QDES",                    displayname = Labels.aicommands["wMsgLANTIRN_QDES"], enabled = true          } },
            { "wMsgLANTIRN_QWP_Minus" ,             new CommandInfo { uniqueid = 23238, name = "wMsgLANTIRN_QWP_Minus",               displayname = Labels.aicommands["wMsgLANTIRN_QWP_Minus"], enabled = true          } },
            { "wMsgLANTIRN_QWP_Plus" ,              new CommandInfo { uniqueid = 23239, name = "wMsgLANTIRN_QWP_Plus",                displayname = Labels.aicommands["wMsgLANTIRN_QWP_Plus"], enabled = true          } },
          
            // block: radar (DONE!)
            { "wMsgJ_RDR_GO_SILENT" ,               new CommandInfo { uniqueid = 23240, name = "wMsgJ_RDR_GO_SILENT",               displayname = Labels.aicommands["wMsgJ_RDR_GO_SILENT"], enabled = true          } },
            { "wMsgJ_RDR_SPOT" ,                    new CommandInfo { uniqueid = 23241, name = "wMsgJ_RDR_SPOT",                    displayname = Labels.aicommands["wMsgJ_RDR_SPOT"]   } }, //na
            { "wMsgJ_RDR_BREAK_LOCK" ,              new CommandInfo { uniqueid = 23242, name = "wMsgJ_RDR_BREAK_LOCK",              displayname = Labels.aicommands["wMsgJ_RDR_BREAK_LOCK"], enabled = true         } },
            { "wMsgJ_RDR_TO_PSTT" ,                 new CommandInfo { uniqueid = 23243, name = "wMsgJ_RDR_TO_PSTT",                 displayname = Labels.aicommands["wMsgJ_RDR_TO_PSTT"]          } },
            { "wMsgJ_RDR_SCAN_ELEV" ,               new CommandInfo { uniqueid = 23244, name = "wMsgJ_RDR_SCAN_ELEV",               displayname = Labels.aicommands["wMsgJ_RDR_SCAN_ELEV"], enabled = true          } },
            { "wMsgJ_RDR_SCAN_AZ" ,                 new CommandInfo { uniqueid = 23245, name = "wMsgJ_RDR_SCAN_AZ",                 displayname = Labels.aicommands["wMsgJ_RDR_SCAN_AZ"], enabled = true            } },
            { "wMsgJ_RDR_SCAN_DIST" ,               new CommandInfo { uniqueid = 23246, name = "wMsgJ_RDR_SCAN_DIST",               displayname = Labels.aicommands["wMsgJ_RDR_SCAN_DIST"], enabled = true          } },
            { "wMsgJ_RDR_TOGGLE_STT" ,              new CommandInfo { uniqueid = 23247, name = "wMsgJ_RDR_TOGGLE_STT",              displayname = Labels.aicommands["wMsgJ_RDR_TOGGLE_STT"], enabled = true         } },
            { "wMsgJ_RDR_VSL_HIGH" ,                new CommandInfo { uniqueid = 23248, name = "wMsgJ_RDR_VSL_HIGH",                displayname = Labels.aicommands["wMsgJ_RDR_VSL_HIGH"], enabled = true           } },
            { "wMsgJ_RDR_VSL_LOW" ,                 new CommandInfo { uniqueid = 23249, name = "wMsgJ_RDR_VSL_LOW",                 displayname = Labels.aicommands["wMsgJ_RDR_VSL_LOW"], enabled = true            } },
            { "wMsgJ_RDR_STT_TGT_AHEAD" ,           new CommandInfo { uniqueid = 23250, name = "wMsgJ_RDR_STT_TGT_AHEAD",           displayname = Labels.aicommands["wMsgJ_RDR_STT_TGT_AHEAD"], enabled = true              } },
            { "wMsgJ_RDR_STT_ENMY_TGT_AHEAD" ,      new CommandInfo { uniqueid = 23251, name = "wMsgJ_RDR_STT_ENMY_TGT_AHEAD",      displayname = Labels.aicommands["wMsgJ_RDR_STT_ENMY_TGT_AHEAD"], enabled = true         } },
            { "wMsgJ_RDR_STT_FRNDLY_TGT_AHEAD" ,    new CommandInfo { uniqueid = 23252, name = "wMsgJ_RDR_STT_FRNDLY_TGT_AHEAD",    displayname = Labels.aicommands["wMsgJ_RDR_STT_FRNDLY_TGT_AHEAD"], enabled = true       } },
            { "wMsgJ_RDR_STT_CHOOSE_SPECIFIC_TGT",  new CommandInfo { uniqueid = 23253, name = "wMsgJ_RDR_STT_CHOOSE_SPECIFIC_TGT", displayname = Labels.aicommands["wMsgJ_RDR_STT_CHOOSE_SPECIFIC_TGT"]    } }, // not endpoint
            { "wMsgJ_RDR_STT_FIRST_TWS_TGT" ,       new CommandInfo { uniqueid = 23254, name = "wMsgJ_RDR_STT_FIRST_TWS_TGT",       displayname = Labels.aicommands["wMsgJ_RDR_STT_FIRST_TWS_TGT"], enabled = true          } },
            { "wMsgJ_RDR_STT_TWS_TGT_NUM" ,         new CommandInfo { uniqueid = 23255, name = "wMsgJ_RDR_STT_TWS_TGT_NUM",         displayname = Labels.aicommands["wMsgJ_RDR_STT_TWS_TGT_NUM"]   } }, // not endpoint, show hint
            { "wMsgJ_RDR_BVR" ,                     new CommandInfo { uniqueid = 23256, name = "wMsgJ_RDR_BVR",                     displayname = Labels.aicommands["wMsgJ_RDR_BVR"]   } }, // root,disabled
            { "wMsgJ_RDR_GO_ACTIVE" ,               new CommandInfo { uniqueid = 23257, name = "wMsgJ_RDR_GO_ACTIVE",               displayname = Labels.aicommands["wMsgJ_RDR_GO_ACTIVE"], enabled = true                  } },
            { "wMsgJ_RDR_STT_LOCK" ,                new CommandInfo { uniqueid = 23258, name = "wMsgJ_RDR_STT_LOCK",                displayname = Labels.aicommands["wMsgJ_RDR_STT_LOCK"], enabled = true                   } },
            { "wMsgJ_RDR_AUTO" ,                    new CommandInfo { uniqueid = 23259, name = "wMsgJ_RDR_AUTO",                    displayname = Labels.aicommands["wMsgJ_RDR_AUTO"], enabled = true           } },
            { "wMsgJ_RDR_RNG_100" ,                 new CommandInfo { uniqueid = 23260, name = "wMsgJ_RDR_RNG_100",                     displayname = Labels.aicommands["wMsgJ_RDR_RNG_100"], enabled = true            } },
            { "wMsgJ_RDR_RNG_200" ,                 new CommandInfo { uniqueid = 23261, name = "wMsgJ_RDR_RNG_200",                     displayname = Labels.aicommands["wMsgJ_RDR_RNG_200"], enabled = true            } },
            { "wMsgJ_RDR_RNG_400" ,                 new CommandInfo { uniqueid = 23262, name = "wMsgJ_RDR_RNG_400",                     displayname = Labels.aicommands["wMsgJ_RDR_RNG_400"], enabled = true            } },
            { "wMsgJ_RDR_POS" ,                     new CommandInfo { uniqueid = 23263, name = "wMsgJ_RDR_POS",                     displayname = Labels.aicommands["wMsgJ_RDR_POS"], enabled = true            } },
            { "wMsgJ_RDR_POS_CTR" ,                 new CommandInfo { uniqueid = 23264, name = "wMsgJ_RDR_POS_CTR",                 displayname = Labels.aicommands["wMsgJ_RDR_POS_CTR"], enabled = true        } },
            { "wMsgJ_RDR_POS_CTR_L" ,               new CommandInfo { uniqueid = 23265, name = "wMsgJ_RDR_POS_CTR_L",               displayname = Labels.aicommands["wMsgJ_RDR_POS_CTR_L"], enabled = true      } },
            { "wMsgJ_RDR_POS_CTR_R" ,               new CommandInfo { uniqueid = 23266, name = "wMsgJ_RDR_POS_CTR_R",               displayname = Labels.aicommands["wMsgJ_RDR_POS_CTR_R"], enabled = true      } },
            { "wMsgJ_RDR_POS_L" ,                   new CommandInfo { uniqueid = 23267, name = "wMsgJ_RDR_POS_L",                   displayname = Labels.aicommands["wMsgJ_RDR_POS_L"], enabled = true          } },
            { "wMsgJ_RDR_POS_R" ,                   new CommandInfo { uniqueid = 23268, name = "wMsgJ_RDR_POS_R",                   displayname = Labels.aicommands["wMsgJ_RDR_POS_R"], enabled = true          } },
            { "wMsgJ_RDR_POS_HI" ,                  new CommandInfo { uniqueid = 23269, name = "wMsgJ_RDR_POS_HI",                  displayname = Labels.aicommands["wMsgJ_RDR_POS_HI"], enabled = true         } },
            { "wMsgJ_RDR_POS_LO" ,                  new CommandInfo { uniqueid = 23270, name = "wMsgJ_RDR_POS_LO",                  displayname = Labels.aicommands["wMsgJ_RDR_POS_LO"], enabled = true         } },
            { "wMsgJ_RDR_POS_MID" ,                 new CommandInfo { uniqueid = 23271, name = "wMsgJ_RDR_POS_MID",                 displayname = Labels.aicommands["wMsgJ_RDR_POS_MID"], enabled = true        } },
            { "wMsgJ_RDR_POS_MID_HI" ,              new CommandInfo { uniqueid = 23272, name = "wMsgJ_RDR_POS_MID_HI",              displayname = Labels.aicommands["wMsgJ_RDR_POS_MID_HI"], enabled = true     } },
            { "wMsgJ_RDR_POS_MID_LO" ,              new CommandInfo { uniqueid = 23273, name = "wMsgJ_RDR_POS_MID_LO",              displayname = Labels.aicommands["wMsgJ_RDR_POS_MID_LO"], enabled = true     } },
            { "wMsgJ_RDR_STT_TWS_TGT_1" ,           new CommandInfo { uniqueid = 23274, name = "wMsgJ_RDR_STT_TWS_TGT_1",           displayname = Labels.aicommands["wMsgJ_RDR_STT_TWS_TGT_1"], enabled = true            } },
            { "wMsgJ_RDR_STT_TWS_TGT_2" ,           new CommandInfo { uniqueid = 23275, name = "wMsgJ_RDR_STT_TWS_TGT_2",           displayname = Labels.aicommands["wMsgJ_RDR_STT_TWS_TGT_2"], enabled = true            } },
            { "wMsgJ_RDR_STT_TWS_TGT_3" ,           new CommandInfo { uniqueid = 23276, name = "wMsgJ_RDR_STT_TWS_TGT_3",           displayname = Labels.aicommands["wMsgJ_RDR_STT_TWS_TGT_3"], enabled = true            } },
            { "wMsgJ_RDR_STT_TWS_TGT_4" ,           new CommandInfo { uniqueid = 23277, name = "wMsgJ_RDR_STT_TWS_TGT_4",           displayname = Labels.aicommands["wMsgJ_RDR_STT_TWS_TGT_4"], enabled = true            } },
            { "wMsgJ_RDR_STT_TWS_TGT_5" ,           new CommandInfo { uniqueid = 23278, name = "wMsgJ_RDR_STT_TWS_TGT_5",           displayname = Labels.aicommands["wMsgJ_RDR_STT_TWS_TGT_5"], enabled = true            } },
            { "wMsgJ_RDR_STT_TWS_TGT_6" ,           new CommandInfo { uniqueid = 23279, name = "wMsgJ_RDR_STT_TWS_TGT_6",           displayname = Labels.aicommands["wMsgJ_RDR_STT_TWS_TGT_6"], enabled = true            } },
            { "wMsgJ_RDR_STT_TWS_TGT_7" ,           new CommandInfo { uniqueid = 23280, name = "wMsgJ_RDR_STT_TWS_TGT_7",           displayname = Labels.aicommands["wMsgJ_RDR_STT_TWS_TGT_7"], enabled = true            } },
            { "wMsgJ_RDR_STT_TWS_TGT_8" ,           new CommandInfo { uniqueid = 23281, name = "wMsgJ_RDR_STT_TWS_TGT_8",           displayname = Labels.aicommands["wMsgJ_RDR_STT_TWS_TGT_8"], enabled = true            } },
            { "wMsgJ_RDR_RNG_25" ,                  new CommandInfo { uniqueid = 23282, name = "wMsgJ_RDR_RNG_25",                  displayname = Labels.aicommands["wMsgJ_RDR_RNG_25"], enabled = true             } },
            { "wMsgJ_RDR_RNG_50" ,                  new CommandInfo { uniqueid = 23283, name = "wMsgJ_RDR_RNG_50",                  displayname = Labels.aicommands["wMsgJ_RDR_RNG_50"], enabled = true             } },
            { "wMsgJ_RDR_RNG_AUTO" ,                new CommandInfo { uniqueid = 23284, name = "wMsgJ_RDR_RNG_AUTO",                displayname = Labels.aicommands["wMsgJ_RDR_RNG_AUTO"], enabled = true       } },


            { "wMsgJ_RDR_MODE_AUTO" ,               new CommandInfo { uniqueid = 23286, name = "wMsgJ_RDR_MODE_AUTO",           displayname = Labels.aicommands["wMsgJ_RDR_MODE_AUTO"], enabled = true          } },
            { "wMsgJ_RDR_MODE_TWS" ,                new CommandInfo { uniqueid = 23287, name = "wMsgJ_RDR_MODE_TWS",            displayname = Labels.aicommands["wMsgJ_RDR_MODE_TWS"], enabled = true           } },
            { "wMsgJ_RDR_MODE_RWS" ,                new CommandInfo { uniqueid = 23288, name = "wMsgJ_RDR_MODE_RWS",            displayname = Labels.aicommands["wMsgJ_RDR_MODE_RWS"], enabled = true           } },
            { "wMsgJ_RDR_MODE" ,                    new CommandInfo { uniqueid = 23289, name = "wMsgJ_RDR_MODE",                displayname = Labels.aicommands["wMsgJ_RDR_MODE"], enabled = true           } },
            // end of radar

            // spare block
            { "wMsgJ_RDR_STAB" ,                    new CommandInfo { uniqueid = 23290, name = "wMsgJ_RDR_STAB",            displayname = Labels.aicommands["wMsgJ_RDR_STAB"], enabled = true           } }, //hint
            { "wMsgJ_RDR_STAB_15" ,                 new CommandInfo { uniqueid = 23291, name = "wMsgJ_RDR_STAB_15",         displayname = Labels.aicommands["wMsgJ_RDR_STAB_15"], enabled = true        } },
            { "wMsgJ_RDR_STAB_30" ,                 new CommandInfo { uniqueid = 23292, name = "wMsgJ_RDR_STAB_30",         displayname = Labels.aicommands["wMsgJ_RDR_STAB_30"], enabled = true        } },
            { "wMsgJ_RDR_STAB_60" ,                 new CommandInfo { uniqueid = 23293, name = "wMsgJ_RDR_STAB_60",         displayname = Labels.aicommands["wMsgJ_RDR_STAB_60"], enabled = true        } },
            { "wMsgJ_RDR_STAB_120" ,                new CommandInfo { uniqueid = 23294, name = "wMsgJ_RDR_STAB_120",        displayname = Labels.aicommands["wMsgJ_RDR_STAB_120"], enabled = true       } },
            { "wMsgJ_RDR_STAB_INDEF" ,              new CommandInfo { uniqueid = 23295, name = "wMsgJ_RDR_STAB_INDEF",      displayname = Labels.aicommands["wMsgJ_RDR_STAB_INDEF"], enabled = true     } },
            { "wMsgJ_RDR_STAB_GROUND" ,             new CommandInfo { uniqueid = 23295, name = "wMsgJ_RDR_STAB_GROUND",     displayname = Labels.aicommands["wMsgJ_RDR_STAB_GROUND"], enabled = true     } },

            { "wMsgLANTIRN_Head_Eyeball" ,          new CommandInfo { uniqueid = 23295, name = "wMsgLANTIRN_Head_Eyeball",  displayname = Labels.aicommands["wMsgLANTIRN_Head_Eyeball"], enabled = true     } },
            { "wMsgLANTIRN_Head_Head" ,             new CommandInfo { uniqueid = 23295, name = "wMsgLANTIRN_Head_Head",     displayname = Labels.aicommands["wMsgLANTIRN_Head_Head"], enabled = true     } },
            //{ "wMsgPlaceHolder098" ,             new CommandInfo { uniqueid = 23295, name = "wMsgPlaceHolder098",       displayname = Labels.aicommands["wMsgPlaceHolder098"], enabled = true     } },
            //{ "wMsgPlaceHolder099" ,             new CommandInfo { uniqueid = 23295, name = "wMsgPlaceHolder099",       displayname = Labels.aicommands["wMsgPlaceHolder099"], enabled = true     } },
            
            //NEW:
            { "wMsgJ_RDR_MODE_TWS_MAN" ,            new CommandInfo { uniqueid = 23296, name = "wMsgJ_RDR_MODE_TWS_MAN",    displayname = Labels.aicommands["wMsgJ_RDR_MODE_TWS_MAN"], enabled = true            } },
            { "wMsgJ_RDR_MODE_SIZE_M" ,             new CommandInfo { uniqueid = 23297, name = "wMsgJ_RDR_MODE_SIZE_M",     displayname = Labels.aicommands["wMsgJ_RDR_MODE_SIZE_M"], enabled = true            } },
            { "wMsgJ_RDR_MODE_SIZE_L" ,             new CommandInfo { uniqueid = 23298, name = "wMsgJ_RDR_MODE_SIZE_L",     displayname = Labels.aicommands["wMsgJ_RDR_MODE_SIZE_L"], enabled = true            } },
            { "wMsgJ_RDR_MODE_SIZE_S" ,             new CommandInfo { uniqueid = 23299, name = "wMsgJ_RDR_MODE_SIZE_S",     displayname = Labels.aicommands["wMsgJ_RDR_MODE_SIZE_S"], enabled = true            } },

            // block: weapons id 23300-23399 (DONE!)
            { "wMsgJ_WPN_AG_SORDN" ,            new CommandInfo { uniqueid = 23300, name = "wMsgJ_WPN_AG_SORDN",            displayname = Labels.aicommands["wMsgJ_WPN_AG_SORDN"], enabled = true            } }, //hint
            { "wMsgJ_WPN_AG_SORDN_WPN_1" ,      new CommandInfo { uniqueid = 23301, name = "wMsgJ_WPN_AG_SORDN_WPN_1",      displayname = Labels.aicommands["wMsgJ_WPN_AG_SORDN_WPN_1"], enabled = true      } },
            { "wMsgJ_WPN_AG_SORDN_WPN_2" ,      new CommandInfo { uniqueid = 23302, name = "wMsgJ_WPN_AG_SORDN_WPN_2",      displayname = Labels.aicommands["wMsgJ_WPN_AG_SORDN_WPN_2"], enabled = true      } },
            { "wMsgJ_WPN_AG_SORDN_WPN_3" ,      new CommandInfo { uniqueid = 23303, name = "wMsgJ_WPN_AG_SORDN_WPN_3",      displayname = Labels.aicommands["wMsgJ_WPN_AG_SORDN_WPN_3"], enabled = true      } },
            { "wMsgJ_WPN_AG_SORDN_WPN_4" ,      new CommandInfo { uniqueid = 23304, name = "wMsgJ_WPN_AG_SORDN_WPN_4",      displayname = Labels.aicommands["wMsgJ_WPN_AG_SORDN_WPN_4"], enabled = true      } },
            { "wMsgJ_WPN_AG_SORDN_WPN_5" ,      new CommandInfo { uniqueid = 23305, name = "wMsgJ_WPN_AG_SORDN_WPN_5",      displayname = Labels.aicommands["wMsgJ_WPN_AG_SORDN_WPN_5"], enabled = true      } },
            { "wMsgJ_WPN_AG_SORDN_WPN_6" ,      new CommandInfo { uniqueid = 23306, name = "wMsgJ_WPN_AG_SORDN_WPN_6",      displayname = Labels.aicommands["wMsgJ_WPN_AG_SORDN_WPN_6"], enabled = true      } },
            { "wMsgJ_WPN_AG_SORDN_WPN_7" ,      new CommandInfo { uniqueid = 23307, name = "wMsgJ_WPN_AG_SORDN_WPN_7",      displayname = Labels.aicommands["wMsgJ_WPN_AG_SORDN_WPN_7"], enabled = true      } },
            { "wMsgJ_WPN_AG_SORDN_WPN_8" ,      new CommandInfo { uniqueid = 23308, name = "wMsgJ_WPN_AG_SORDN_WPN_8",      displayname = Labels.aicommands["wMsgJ_WPN_AG_SORDN_WPN_8"], enabled = true      } },
            { "wMsgJ_WPN_AG_SPOT" ,             new CommandInfo { uniqueid = 23309, name = "wMsgJ_WPN_AG_SPOT",             displayname = Labels.aicommands["wMsgJ_WPN_AG_SPOT"]   } }, // not used
            { "wMsgJ_WPN_AG_SET_COMP_TGT" ,     new CommandInfo { uniqueid = 23310, name = "wMsgJ_WPN_AG_SET_COMP_TGT",     displayname = Labels.aicommands["wMsgJ_WPN_AG_SET_COMP_TGT"], enabled = true     } },
            { "wMsgJ_WPN_AG_SET_PAIRS" ,        new CommandInfo { uniqueid = 23311, name = "wMsgJ_WPN_AG_SET_PAIRS",        displayname = Labels.aicommands["wMsgJ_WPN_AG_SET_PAIRS"], enabled = true        } },
            { "wMsgJ_WPN_AG_SETFUSE" ,          new CommandInfo { uniqueid = 23312, name = "wMsgJ_WPN_AG_SETFUSE",          displayname = Labels.aicommands["wMsgJ_WPN_AG_SETFUSE"], enabled = true          } },
            { "wMsgJ_WPN_AG_SETFUSE_NOSETAIL" , new CommandInfo { uniqueid = 23313, name = "wMsgJ_WPN_AG_SETFUSE_NOSETAIL", displayname = Labels.aicommands["wMsgJ_WPN_AG_SETFUSE_NOSETAIL"], enabled = true } },
            { "wMsgJ_WPN_AG_SETFUSE_NOSE" ,     new CommandInfo { uniqueid = 23314, name = "wMsgJ_WPN_AG_SETFUSE_NOSE",     displayname = Labels.aicommands["wMsgJ_WPN_AG_SETFUSE_NOSE"], enabled = true     } },
            { "wMsgJ_WPN_AG_SETFUSE_SAFE" ,     new CommandInfo { uniqueid = 23315, name = "wMsgJ_WPN_AG_SETFUSE_SAFE",     displayname = Labels.aicommands["wMsgJ_WPN_AG_SETFUSE_SAFE"], enabled = true     } },
            { "wMsgJ_WPN_AG_RIP_QTY" ,          new CommandInfo { uniqueid = 23316, name = "wMsgJ_WPN_AG_RIP_QTY",          displayname = Labels.aicommands["wMsgJ_WPN_AG_RIP_QTY"], enabled = true          } },
            { "wMsgJ_WPN_AG_RIP_QTY_STEP" ,     new CommandInfo { uniqueid = 23317, name = "wMsgJ_WPN_AG_RIP_QTY_STEP",     displayname = Labels.aicommands["wMsgJ_WPN_AG_RIP_QTY_STEP"]    } }, // was enabled
            { "wMsgJ_WPN_AG_RIP_QTY_2" ,        new CommandInfo { uniqueid = 23318, name = "wMsgJ_WPN_AG_RIP_QTY_2",        displayname = Labels.aicommands["wMsgJ_WPN_AG_RIP_QTY_2"], enabled = true        } }, //2
            { "wMsgJ_WPN_AG_RIP_QTY_5" ,        new CommandInfo { uniqueid = 23319, name = "wMsgJ_WPN_AG_RIP_QTY_5",        displayname = Labels.aicommands["wMsgJ_WPN_AG_RIP_QTY_5"], enabled = true        } }, //3
            { "wMsgJ_WPN_AG_RIP_QTY_10" ,       new CommandInfo { uniqueid = 23320, name = "wMsgJ_WPN_AG_RIP_QTY_10",       displayname = Labels.aicommands["wMsgJ_WPN_AG_RIP_QTY_10"], enabled = true       } }, //4
            { "wMsgJ_WPN_AG_RIP_QTY_20" ,       new CommandInfo { uniqueid = 23321, name = "wMsgJ_WPN_AG_RIP_QTY_20",       displayname = Labels.aicommands["wMsgJ_WPN_AG_RIP_QTY_20"], enabled = true       } }, //6
            { "wMsgJ_WPN_AG_RIP_QTY_30" ,       new CommandInfo { uniqueid = 23322, name = "wMsgJ_WPN_AG_RIP_QTY_30",       displayname = Labels.aicommands["wMsgJ_WPN_AG_RIP_QTY_30"], enabled = true       } }, //8
            { "wMsgJ_WPN_AG_RIP_TIME" ,         new CommandInfo { uniqueid = 23323, name = "wMsgJ_WPN_AG_RIP_TIME",         displayname = Labels.aicommands["wMsgJ_WPN_AG_RIP_TIME"], enabled = true         } },
            { "wMsgJ_WPN_AG_RIP_TIME_STEP" ,    new CommandInfo { uniqueid = 23324, name = "wMsgJ_WPN_AG_RIP_TIME_STEP",    displayname = Labels.aicommands["wMsgJ_WPN_AG_RIP_TIME_STEP"], enabled = true    } },
            { "wMsgJ_WPN_AG_RIP_TIME_10" ,      new CommandInfo { uniqueid = 23325, name = "wMsgJ_WPN_AG_RIP_TIME_10",      displayname = Labels.aicommands["wMsgJ_WPN_AG_RIP_TIME_10"], enabled = true      } },
            { "wMsgJ_WPN_AG_RIP_TIME_20" ,      new CommandInfo { uniqueid = 23326, name = "wMsgJ_WPN_AG_RIP_TIME_20",      displayname = Labels.aicommands["wMsgJ_WPN_AG_RIP_TIME_20"], enabled = true      } },
            { "wMsgJ_WPN_AG_RIP_TIME_50" ,      new CommandInfo { uniqueid = 23327, name = "wMsgJ_WPN_AG_RIP_TIME_50",      displayname = Labels.aicommands["wMsgJ_WPN_AG_RIP_TIME_50"], enabled = true      } },
            { "wMsgJ_WPN_AG_RIP_TIME_100" ,     new CommandInfo { uniqueid = 23328, name = "wMsgJ_WPN_AG_RIP_TIME_100",     displayname = Labels.aicommands["wMsgJ_WPN_AG_RIP_TIME_100"], enabled = true     } },
            { "wMsgJ_WPN_AG_RIP_TIME_200" ,     new CommandInfo { uniqueid = 23329, name = "wMsgJ_WPN_AG_RIP_TIME_200",     displayname = Labels.aicommands["wMsgJ_WPN_AG_RIP_TIME_200"], enabled = true     } },
            { "wMsgJ_WPN_AG_RIP_TIME_500" ,     new CommandInfo { uniqueid = 23330, name = "wMsgJ_WPN_AG_RIP_TIME_500",     displayname = Labels.aicommands["wMsgJ_WPN_AG_RIP_TIME_500"], enabled = true     } },
            { "wMsgJ_WPN_AG_RIP_TIME_990" ,     new CommandInfo { uniqueid = 23331, name = "wMsgJ_WPN_AG_RIP_TIME_990",     displayname = Labels.aicommands["wMsgJ_WPN_AG_RIP_TIME_990"], enabled = true     } },
            { "wMsgJ_WPN_AG_RIP_DIST" ,         new CommandInfo { uniqueid = 23332, name = "wMsgJ_WPN_AG_RIP_DIST",         displayname = Labels.aicommands["wMsgJ_WPN_AG_RIP_DIST"], enabled = true         } },
            { "wMsgJ_WPN_AG_RIP_DIST_STEP" ,    new CommandInfo { uniqueid = 23333, name = "wMsgJ_WPN_AG_RIP_DIST_STEP",    displayname = Labels.aicommands["wMsgJ_WPN_AG_RIP_DIST_STEP"], enabled = true    } },
            { "wMsgJ_WPN_AG" ,                  new CommandInfo { uniqueid = 23334, name = "wMsgJ_WPN_AG",                  displayname = Labels.aicommands["wMsgJ_WPN_AG"]   } }, // root, dont use
            { "wMsgJ_WPN_AA" ,                  new CommandInfo { uniqueid = 23335, name = "wMsgJ_WPN_AA",                  displayname = Labels.aicommands["wMsgJ_WPN_AA"] } }, //root, dont use
            { "wMsgJ_WPN_AG_RIP" ,              new CommandInfo { uniqueid = 23336, name = "wMsgJ_WPN_AG_RIP",              displayname = Labels.aicommands["wMsgJ_WPN_AG_RIP"]  } }, // root, dont use
            { "wMsgJ_WPN_AG_JETT" ,             new CommandInfo { uniqueid = 23337, name = "wMsgJ_WPN_AG_JETT",             displayname = Labels.aicommands["wMsgJ_WPN_AG_JETT"], enabled = true            } },
            { "wMsgJ_WPN_AG_RIP_DIST_5" ,       new CommandInfo { uniqueid = 23338, name = "wMsgJ_WPN_AG_RIP_DIST_5",       displayname = Labels.aicommands["wMsgJ_WPN_AG_RIP_DIST_5"], enabled = true      } },
            { "wMsgJ_WPN_AG_RIP_DIST_10" ,      new CommandInfo { uniqueid = 23339, name = "wMsgJ_WPN_AG_RIP_DIST_10",      displayname = Labels.aicommands["wMsgJ_WPN_AG_RIP_DIST_10"], enabled = true     } },
            { "wMsgJ_WPN_AG_RIP_DIST_25" ,      new CommandInfo { uniqueid = 23340, name = "wMsgJ_WPN_AG_RIP_DIST_25",      displayname = Labels.aicommands["wMsgJ_WPN_AG_RIP_DIST_25"], enabled = true     } },
            { "wMsgJ_WPN_AG_RIP_DIST_50" ,      new CommandInfo { uniqueid = 23341, name = "wMsgJ_WPN_AG_RIP_DIST_50",      displayname = Labels.aicommands["wMsgJ_WPN_AG_RIP_DIST_50"], enabled = true     } },
            { "wMsgJ_WPN_AG_RIP_DIST_100" ,     new CommandInfo { uniqueid = 23342, name = "wMsgJ_WPN_AG_RIP_DIST_100",     displayname = Labels.aicommands["wMsgJ_WPN_AG_RIP_DIST_100"], enabled = true    } },
            { "wMsgJ_WPN_AG_RIP_DIST_200" ,     new CommandInfo { uniqueid = 23343, name = "wMsgJ_WPN_AG_RIP_DIST_200",     displayname = Labels.aicommands["wMsgJ_WPN_AG_RIP_DIST_200"], enabled = true    } },
            { "wMsgJ_WPN_AG_RIP_DIST_400" ,     new CommandInfo { uniqueid = 23344, name = "wMsgJ_WPN_AG_RIP_DIST_400",     displayname = Labels.aicommands["wMsgJ_WPN_AG_RIP_DIST_400"], enabled = true    } },
            { "wMsgJ_WPN_AG_UTIL_LANTIRN" ,     new CommandInfo { uniqueid = 23345, name = "wMsgJ_WPN_AG_UTIL_LANTIRN",     displayname = Labels.aicommands["wMsgJ_WPN_AG_UTIL_LANTIRN"], enabled = true    } }, // gone??
            { "wMsgJ_WPN_AG_SORDN_WPN_9" ,      new CommandInfo { uniqueid = 23346, name = "wMsgJ_WPN_AG_SORDN_WPN_9",      displayname = Labels.aicommands["wMsgJ_WPN_AG_SORDN_WPN_9"], enabled = true     } },
            { "wMsgJ_WPN_AG_SORDN_WPN_10" ,     new CommandInfo { uniqueid = 23346, name = "wMsgJ_WPN_AG_SORDN_WPN_10",     displayname = Labels.aicommands["wMsgJ_WPN_AG_SORDN_WPN_10"], enabled = true    } },
            { "wMsgJ_WPN_AG_SET_SNGL" ,         new CommandInfo { uniqueid = 23347, name = "wMsgJ_WPN_AG_SET_SNGL",         displayname = Labels.aicommands["wMsgJ_WPN_AG_SET_SNGL"], enabled = true        } },
            { "wMsgJ_WPN_AG_SET_COMP_PILOT" ,   new CommandInfo { uniqueid = 23348, name = "wMsgJ_WPN_AG_SET_COMP_PILOT",   displayname = Labels.aicommands["wMsgJ_WPN_AG_SET_COMP_PILOT"] , enabled = true } },
            { "wMsgJ_WPN_AG_DROP_TANKS" ,       new CommandInfo { uniqueid = 23285, name = "wMsgJ_WPN_AG_DROP_TANKS",       displayname = Labels.aicommands["wMsgJ_WPN_AG_DROP_TANKS"], enabled = true     } },

            { "wMsgJ_WPN_AG_STN" ,              new CommandInfo { uniqueid = 23349, name = "wMsgJ_WPN_AG_STN",              displayname = Labels.aicommands["wMsgJ_WPN_AG_STN"], enabled = true          } },
            { "wMsgJ_WPN_AG_STN_18" ,           new CommandInfo { uniqueid = 23350, name = "wMsgJ_WPN_AG_STN_18",           displayname = Labels.aicommands["wMsgJ_WPN_AG_STN_18"], enabled = true          } },
            { "wMsgJ_WPN_AG_STN_27" ,           new CommandInfo { uniqueid = 23351, name = "wMsgJ_WPN_AG_STN_27",           displayname = Labels.aicommands["wMsgJ_WPN_AG_STN_27"], enabled = true          } },
            { "wMsgJ_WPN_AG_STN_3456" ,         new CommandInfo { uniqueid = 23352, name = "wMsgJ_WPN_AG_STN_3456",         displayname = Labels.aicommands["wMsgJ_WPN_AG_STN_3456"], enabled = true          } },
            { "wMsgJ_WPN_AG_STN_36" ,           new CommandInfo { uniqueid = 23353, name = "wMsgJ_WPN_AG_STN_36",           displayname = Labels.aicommands["wMsgJ_WPN_AG_STN_36"], enabled = true          } },
            { "wMsgJ_WPN_AG_STN_45" ,           new CommandInfo { uniqueid = 23354, name = "wMsgJ_WPN_AG_STN_45",           displayname = Labels.aicommands["wMsgJ_WPN_AG_STN_45"], enabled = true          } },

            { "wMsgJ_WPN_AG_JDAM_WPT_TO_GGW" ,   new CommandInfo { uniqueid = 23355, name = "wMsgJ_WPN_AG_JDAM_WPT_TO_GGW",   displayname = Labels.aicommands["wMsgJ_WPN_AG_JDAM_WPT_TO_GGW"], enabled = true   } },
            { "wMsgJ_WPN_AG_JDAM_SEL_PP" ,       new CommandInfo { uniqueid = 23356, name = "wMsgJ_WPN_AG_JDAM_SEL_PP",       displayname = Labels.aicommands["wMsgJ_WPN_AG_JDAM_SEL_PP"], enabled = true       } },
            { "wMsgJ_WPN_AG_JDAM_PP_ALL_STN" ,   new CommandInfo { uniqueid = 23357, name = "wMsgJ_WPN_AG_JDAM_PP_ALL_STN",   displayname = Labels.aicommands["wMsgJ_WPN_AG_JDAM_PP_ALL_STN"], enabled = true   } },
            { "wMsgJ_WPN_AG_JDAM_DESIG_TO_STN" , new CommandInfo { uniqueid = 23358, name = "wMsgJ_WPN_AG_JDAM_DESIG_TO_STN", displayname = Labels.aicommands["wMsgJ_WPN_AG_JDAM_DESIG_TO_STN"], enabled = true } },

            //{ "wMsgPlaceHolder155" ,            new CommandInfo { uniqueid = 23355, name = "wMsgPlaceHolder155",            displayname = Labels.aicommands["wMsgPlaceHolder155"]          } },
            //{ "wMsgPlaceHolder156" ,            new CommandInfo { uniqueid = 23356, name = "wMsgPlaceHolder156",            displayname = Labels.aicommands["wMsgPlaceHolder156"]          } },
            //{ "wMsgPlaceHolder157" ,            new CommandInfo { uniqueid = 23357, name = "wMsgPlaceHolder157",            displayname = Labels.aicommands["wMsgPlaceHolder157"]          } },
            //{ "wMsgPlaceHolder158" ,            new CommandInfo { uniqueid = 23358, name = "wMsgPlaceHolder158",            displayname = Labels.aicommands["wMsgPlaceHolder158"]          } },
            //{ "wMsgPlaceHolder159" ,            new CommandInfo { uniqueid = 23359, name = "wMsgPlaceHolder159",            displayname = Labels.aicommands["wMsgPlaceHolder159"]          } },
            //{ "wMsgPlaceHolder160" ,            new CommandInfo { uniqueid = 23360, name = "wMsgPlaceHolder160",            displayname = Labels.aicommands["wMsgPlaceHolder160"]          } },
            //{ "wMsgPlaceHolder161" ,            new CommandInfo { uniqueid = 23361, name = "wMsgPlaceHolder161",            displayname = Labels.aicommands["wMsgPlaceHolder161"]          } },
            //{ "wMsgPlaceHolder162" ,            new CommandInfo { uniqueid = 23362, name = "wMsgPlaceHolder162",            displayname = Labels.aicommands["wMsgPlaceHolder162"]          } },
            //{ "wMsgPlaceHolder163" ,            new CommandInfo { uniqueid = 23363, name = "wMsgPlaceHolder163",            displayname = Labels.aicommands["wMsgPlaceHolder163"]          } },
            //{ "wMsgPlaceHolder164" ,            new CommandInfo { uniqueid = 23364, name = "wMsgPlaceHolder164",            displayname = Labels.aicommands["wMsgPlaceHolder164"]          } },
            //{ "wMsgPlaceHolder165" ,            new CommandInfo { uniqueid = 23365, name = "wMsgPlaceHolder165",            displayname = Labels.aicommands["wMsgPlaceHolder165"]          } },
            //{ "wMsgPlaceHolder166" ,            new CommandInfo { uniqueid = 23366, name = "wMsgPlaceHolder166",            displayname = Labels.aicommands["wMsgPlaceHolder166"]          } },
            //{ "wMsgPlaceHolder167" ,            new CommandInfo { uniqueid = 23367, name = "wMsgPlaceHolder167",            displayname = Labels.aicommands["wMsgPlaceHolder167"]          } },
            //{ "wMsgPlaceHolder168" ,            new CommandInfo { uniqueid = 23368, name = "wMsgPlaceHolder168",            displayname = Labels.aicommands["wMsgPlaceHolder168"]          } },
            //{ "wMsgPlaceHolder169" ,            new CommandInfo { uniqueid = 23369, name = "wMsgPlaceHolder169",            displayname = Labels.aicommands["wMsgPlaceHolder169"]          } },
            //{ "wMsgPlaceHolder170" ,            new CommandInfo { uniqueid = 23370, name = "wMsgPlaceHolder170",            displayname = Labels.aicommands["wMsgPlaceHolder170"]          } },
            //{ "wMsgPlaceHolder171" ,            new CommandInfo { uniqueid = 23371, name = "wMsgPlaceHolder171",            displayname = Labels.aicommands["wMsgPlaceHolder171"]          } },
            //{ "wMsgPlaceHolder172" ,            new CommandInfo { uniqueid = 23372, name = "wMsgPlaceHolder172",            displayname = Labels.aicommands["wMsgPlaceHolder172"]          } },
            //{ "wMsgPlaceHolder173" ,            new CommandInfo { uniqueid = 23373, name = "wMsgPlaceHolder173",            displayname = Labels.aicommands["wMsgPlaceHolder173"]          } },
            //{ "wMsgPlaceHolder174" ,            new CommandInfo { uniqueid = 23374, name = "wMsgPlaceHolder174",            displayname = Labels.aicommands["wMsgPlaceHolder174"]          } },
            //{ "wMsgPlaceHolder175" ,            new CommandInfo { uniqueid = 23375, name = "wMsgPlaceHolder175",            displayname = Labels.aicommands["wMsgPlaceHolder175"]          } },
            //{ "wMsgPlaceHolder176" ,            new CommandInfo { uniqueid = 23376, name = "wMsgPlaceHolder176",            displayname = Labels.aicommands["wMsgPlaceHolder176"]          } },
            //{ "wMsgPlaceHolder177" ,            new CommandInfo { uniqueid = 23377, name = "wMsgPlaceHolder177",            displayname = Labels.aicommands["wMsgPlaceHolder177"]          } },
            //{ "wMsgPlaceHolder178" ,            new CommandInfo { uniqueid = 23378, name = "wMsgPlaceHolder178",            displayname = Labels.aicommands["wMsgPlaceHolder178"]          } },
            //{ "wMsgPlaceHolder179" ,            new CommandInfo { uniqueid = 23379, name = "wMsgPlaceHolder179",            displayname = Labels.aicommands["wMsgPlaceHolder179"]          } },
            //{ "wMsgPlaceHolder180" ,            new CommandInfo { uniqueid = 23380, name = "wMsgPlaceHolder180",            displayname = Labels.aicommands["wMsgPlaceHolder180"]          } },
            //{ "wMsgPlaceHolder181" ,            new CommandInfo { uniqueid = 23381, name = "wMsgPlaceHolder181",            displayname = Labels.aicommands["wMsgPlaceHolder181"]          } },
            //{ "wMsgPlaceHolder182" ,            new CommandInfo { uniqueid = 23382, name = "wMsgPlaceHolder182",            displayname = Labels.aicommands["wMsgPlaceHolder182"]          } },
            //{ "wMsgPlaceHolder183" ,            new CommandInfo { uniqueid = 23383, name = "wMsgPlaceHolder183",            displayname = Labels.aicommands["wMsgPlaceHolder183"]          } },
            //{ "wMsgPlaceHolder184" ,            new CommandInfo { uniqueid = 23384, name = "wMsgPlaceHolder184",            displayname = Labels.aicommands["wMsgPlaceHolder184"]          } },
            //{ "wMsgPlaceHolder185" ,            new CommandInfo { uniqueid = 23385, name = "wMsgPlaceHolder185",            displayname = Labels.aicommands["wMsgPlaceHolder185"]          } },
            //{ "wMsgPlaceHolder186" ,            new CommandInfo { uniqueid = 23386, name = "wMsgPlaceHolder186",            displayname = Labels.aicommands["wMsgPlaceHolder186"]          } },
            //{ "wMsgPlaceHolder187" ,            new CommandInfo { uniqueid = 23387, name = "wMsgPlaceHolder187",            displayname = Labels.aicommands["wMsgPlaceHolder187"]          } },
            { "wMsgJ_WPN_AG_RIP_QTY_16" ,       new CommandInfo { uniqueid = 23388, name = "wMsgJ_WPN_AG_RIP_QTY_16",       displayname = Labels.aicommands["wMsgJ_WPN_AG_RIP_QTY_16"], enabled = true        } },
            { "wMsgJ_WPN_AG_RIP_QTY_28" ,       new CommandInfo { uniqueid = 23389, name = "wMsgJ_WPN_AG_RIP_QTY_28",       displayname = Labels.aicommands["wMsgJ_WPN_AG_RIP_QTY_28"], enabled = true        } },
            // end of weapons

            // spare table
            { "wMsgJ_RAD_TCN_TAC_STEN" ,            new CommandInfo { uniqueid = 23390, name = "wMsgJ_RAD_TCN_TAC_STEN",            displayname = Labels.aicommands["wMsgJ_RAD_TCN_TAC_STEN"], enabled = true            } },
            { "wMsgJ_RAD_TCN_TAC_ARCO" ,            new CommandInfo { uniqueid = 23391, name = "wMsgJ_RAD_TCN_TAC_ARCO",            displayname = Labels.aicommands["wMsgJ_RAD_TCN_TAC_ARCO"], enabled = true            } },
            { "wMsgJ_RAD_TCN_TAC_SHEL" ,            new CommandInfo { uniqueid = 23392, name = "wMsgJ_RAD_TCN_TAC_SHEL",            displayname = Labels.aicommands["wMsgJ_RAD_TCN_TAC_SHEL"], enabled = true            } },
            { "wMsgJ_RAD_TCN_TAC_TEXA" ,            new CommandInfo { uniqueid = 23393, name = "wMsgJ_RAD_TCN_TAC_TEXA",            displayname = Labels.aicommands["wMsgJ_RAD_TCN_TAC_TEXA"], enabled = true            } },
            { "wMsgJ_RAD_TCN_TAC_WASH" ,            new CommandInfo { uniqueid = 23394, name = "wMsgJ_RAD_TCN_TAC_WASH",            displayname = Labels.aicommands["wMsgJ_RAD_TCN_TAC_WASH"], enabled = true           } },
            { "wMsgJ_RAD_TCN_TAC_ROOS" ,            new CommandInfo { uniqueid = 23395, name = "wMsgJ_RAD_TCN_TAC_ROOS",            displayname = Labels.aicommands["wMsgJ_RAD_TCN_TAC_ROOS"], enabled = true           } },
            { "wMsgJ_RAD_TCN_TAC_LINC" ,            new CommandInfo { uniqueid = 23396, name = "wMsgJ_RAD_TCN_TAC_LINC",            displayname = Labels.aicommands["wMsgJ_RAD_TCN_TAC_LINC"], enabled = true           } },
            { "wMsgJ_RAD_TCN_TAC_TRUM" ,            new CommandInfo { uniqueid = 23397, name = "wMsgJ_RAD_TCN_TAC_TRUM",            displayname = Labels.aicommands["wMsgJ_RAD_TCN_TAC_TRUM"], enabled = true           } },
            { "wMsgJ_RAD_TCN_TAC_FORE" ,            new CommandInfo { uniqueid = 23398, name = "wMsgJ_RAD_TCN_TAC_FORE",            displayname = Labels.aicommands["wMsgJ_RAD_TCN_TAC_FORE"], enabled = true           } },
            //{ "wMsgPlaceHolder198" ,            new CommandInfo { uniqueid = 23398, name = "wMsgPlaceHolder198",            displayname = Labels.aicommands["wMsgPlaceHolder198"]          } },
            //{ "wMsgPlaceHolder199" ,            new CommandInfo { uniqueid = 23399, name = "wMsgPlaceHolder199",            displayname = Labels.aicommands["wMsgPlaceHolder199"]          } },

            // block: radio id 23400-23499
            { "wMsgJ_RAD_159" ,                 new CommandInfo { uniqueid = 23400, name = "wMsgJ_RAD_159",                 displayname = Labels.aicommands["wMsgJ_RAD_159"]                    } },
            { "wMsgJ_RAD_159_USE_GUARD" ,       new CommandInfo { uniqueid = 23401, name = "wMsgJ_RAD_159_USE_GUARD",       displayname = Labels.aicommands["wMsgJ_RAD_159_USE_GUARD"]          } },
            { "wMsgJ_RAD_159_USE_MANUAL" ,      new CommandInfo { uniqueid = 23402, name = "wMsgJ_RAD_159_USE_MANUAL",      displayname = Labels.aicommands["wMsgJ_RAD_159_USE_MANUAL"]         } },
            { "wMsgJ_RAD_159_USE_CHAN" ,        new CommandInfo { uniqueid = 23403, name = "wMsgJ_RAD_159_USE_CHAN",        displayname = Labels.aicommands["wMsgJ_RAD_159_USE_CHAN"]           } },
            { "wMsgJ_RAD_159_USE_CHAN_1" ,      new CommandInfo { uniqueid = 23404, name = "wMsgJ_RAD_159_USE_CHAN_1",      displayname = Labels.aicommands["wMsgJ_RAD_159_USE_CHAN_1"]         } },
            { "wMsgJ_RAD_159_USE_CHAN_2" ,      new CommandInfo { uniqueid = 23405, name = "wMsgJ_RAD_159_USE_CHAN_2",      displayname = Labels.aicommands["wMsgJ_RAD_159_USE_CHAN_2"]         } },
            { "wMsgJ_RAD_159_USE_CHAN_3" ,      new CommandInfo { uniqueid = 23406, name = "wMsgJ_RAD_159_USE_CHAN_3",      displayname = Labels.aicommands["wMsgJ_RAD_159_USE_CHAN_3"]         } },
            { "wMsgJ_RAD_159_USE_CHAN_4" ,      new CommandInfo { uniqueid = 23407, name = "wMsgJ_RAD_159_USE_CHAN_4",      displayname = Labels.aicommands["wMsgJ_RAD_159_USE_CHAN_4"]         } },
            { "wMsgJ_RAD_159_USE_CHAN_5" ,      new CommandInfo { uniqueid = 23408, name = "wMsgJ_RAD_159_USE_CHAN_5",      displayname = Labels.aicommands["wMsgJ_RAD_159_USE_CHAN_5"]         } },
            { "wMsgJ_RAD_159_USE_CHAN_6" ,      new CommandInfo { uniqueid = 23409, name = "wMsgJ_RAD_159_USE_CHAN_6",      displayname = Labels.aicommands["wMsgJ_RAD_159_USE_CHAN_6"]         } },
            { "wMsgJ_RAD_159_USE_CHAN_7" ,      new CommandInfo { uniqueid = 23410, name = "wMsgJ_RAD_159_USE_CHAN_7",      displayname = Labels.aicommands["wMsgJ_RAD_159_USE_CHAN_7"]         } },
            { "wMsgJ_RAD_159_USE_CHAN_8" ,      new CommandInfo { uniqueid = 23411, name = "wMsgJ_RAD_159_USE_CHAN_8",      displayname = Labels.aicommands["wMsgJ_RAD_159_USE_CHAN_8"]         } },
            { "wMsgJ_RAD_159_TUNE_MAN" ,        new CommandInfo { uniqueid = 23412, name = "wMsgJ_RAD_159_TUNE_MAN",        displayname = Labels.aicommands["wMsgJ_RAD_159_TUNE_MAN"]           } },
            { "wMsgJ_RAD_159_SELECT_CHAN" ,     new CommandInfo { uniqueid = 23413, name = "wMsgJ_RAD_159_SELECT_CHAN",     displayname = Labels.aicommands["wMsgJ_RAD_159_SELECT_CHAN"]        } },
            { "wMsgJ_RAD_159_SELECT_MODE" ,     new CommandInfo { uniqueid = 23414, name = "wMsgJ_RAD_159_SELECT_MODE",     displayname = Labels.aicommands["wMsgJ_RAD_159_SELECT_MODE"]        } },
            { "wMsgJ_RAD_159_TUNE_ATC" ,        new CommandInfo { uniqueid = 23415, name = "wMsgJ_RAD_159_TUNE_ATC",        displayname = Labels.aicommands["wMsgJ_RAD_159_TUNE_ATC"]           } },
            { "wMsgJ_RAD_159_TUNE_TAC" ,        new CommandInfo { uniqueid = 23416, name = "wMsgJ_RAD_159_TUNE_TAC",        displayname = Labels.aicommands["wMsgJ_RAD_159_TUNE_TAC"]           } },

            { "wMsgJ_RAD_182" ,                 new CommandInfo { uniqueid = 23417, name = "wMsgJ_RAD_182",                 displayname = Labels.aicommands["wMsgJ_RAD_182"]           } },
            { "wMsgJ_RAD_182_USE_GUARD" ,       new CommandInfo { uniqueid = 23418, name = "wMsgJ_RAD_182_USE_GUARD",       displayname = Labels.aicommands["wMsgJ_RAD_182_USE_GUARD"], enabled = true           } },
            { "wMsgJ_RAD_182_USE_MANUAL" ,      new CommandInfo { uniqueid = 23419, name = "wMsgJ_RAD_182_USE_MANUAL",      displayname = Labels.aicommands["wMsgJ_RAD_182_USE_MANUAL"]          } },
            { "wMsgJ_RAD_182_USE_CHAN" ,        new CommandInfo { uniqueid = 23420, name = "wMsgJ_RAD_182_USE_CHAN",        displayname = Labels.aicommands["wMsgJ_RAD_182_USE_CHAN"]         } },
            { "wMsgJ_RAD_182_USE_CHAN_1 " ,     new CommandInfo { uniqueid = 23421, name = "wMsgJ_RAD_182_USE_CHAN_1 ",     displayname = Labels.aicommands["wMsgJ_RAD_182_USE_CHAN_1"]          } },
            { "wMsgJ_RAD_182_USE_CHAN_2" ,      new CommandInfo { uniqueid = 23422, name = "wMsgJ_RAD_182_USE_CHAN_2",      displayname = Labels.aicommands["wMsgJ_RAD_182_USE_CHAN_2"]          } },
            { "wMsgJ_RAD_182_USE_CHAN_3" ,      new CommandInfo { uniqueid = 23423, name = "wMsgJ_RAD_182_USE_CHAN_3",      displayname = Labels.aicommands["wMsgJ_RAD_182_USE_CHAN_3"]         } },
            { "wMsgJ_RAD_182_USE_CHAN_4" ,      new CommandInfo { uniqueid = 23424, name = "wMsgJ_RAD_182_USE_CHAN_4",      displayname = Labels.aicommands["wMsgJ_RAD_182_USE_CHAN_4"]          } },
            { "wMsgJ_RAD_182_USE_CHAN_5" ,      new CommandInfo { uniqueid = 23425, name = "wMsgJ_RAD_182_USE_CHAN_5",      displayname = Labels.aicommands["wMsgJ_RAD_182_USE_CHAN_5"]          } },
            { "wMsgJ_RAD_182_USE_CHAN_6" ,      new CommandInfo { uniqueid = 23426, name = "wMsgJ_RAD_182_USE_CHAN_6",      displayname = Labels.aicommands["wMsgJ_RAD_182_USE_CHAN_6"]           } },
            { "wMsgJ_RAD_182_USE_CHAN_7" ,      new CommandInfo { uniqueid = 23427, name = "wMsgJ_RAD_182_USE_CHAN_7",      displayname = Labels.aicommands["wMsgJ_RAD_182_USE_CHAN_7"]         } },
            { "wMsgJ_RAD_182_USE_CHAN_8" ,      new CommandInfo { uniqueid = 23428, name = "wMsgJ_RAD_182_USE_CHAN_8",      displayname = Labels.aicommands["wMsgJ_RAD_182_USE_CHAN_8"]           } },
            { "wMsgJ_RAD_182_TUNE_MAN" ,        new CommandInfo { uniqueid = 23429, name = "wMsgJ_RAD_182_TUNE_MAN",        displayname = Labels.aicommands["wMsgJ_RAD_182_TUNE_MAN"] } },
            { "wMsgJ_RAD_182_SELECT_CHAN" ,     new CommandInfo { uniqueid = 23430, name = "wMsgJ_RAD_182_SELECT_CHAN",     displayname = Labels.aicommands["wMsgJ_RAD_182_SELECT_CHAN"]   } },
            { "wMsgJ_RAD_182_SELECT_MODE" ,     new CommandInfo { uniqueid = 23431, name = "wMsgJ_RAD_182_SELECT_MODE",     displayname = Labels.aicommands["wMsgJ_RAD_182_SELECT_MODE"] } }, // use MODe below
            { "wMsgJ_RAD_182_TUNE_ATC" ,        new CommandInfo { uniqueid = 23432, name = "wMsgJ_RAD_182_TUNE_ATC",        displayname = Labels.aicommands["wMsgJ_RAD_182_TUNE_ATC"]  } },
            { "wMsgJ_RAD_182_TUNE_TAC" ,        new CommandInfo { uniqueid = 23433, name = "wMsgJ_RAD_182_TUNE_TAC",        displayname = Labels.aicommands["wMsgJ_RAD_182_TUNE_TAC"]  } },

            { "wMsgJ_RAD_DL" ,                  new CommandInfo { uniqueid = 23435, name = "wMsgJ_RAD_DL",                  displayname = Labels.aicommands["wMsgJ_RAD_DL"]} }, // root, not endpoint -> disable
            { "wMsgJ_RAD_DL_SET_MODE" ,         new CommandInfo { uniqueid = 23436, name = "wMsgJ_RAD_DL_SET_MODE",         displayname = Labels.aicommands["wMsgJ_RAD_DL_SET_MODE"], enabled = true    } },
            { "wMsgJ_RAD_DL_SET_FREQ_PRST" ,    new CommandInfo { uniqueid = 23437, name = "wMsgJ_RAD_DL_SET_FREQ_PRST",    displayname = Labels.aicommands["wMsgJ_RAD_DL_SET_FREQ_PRST"], enabled = true   } },
            { "wMsgJ_RAD_DL_SET_HOST" ,         new CommandInfo { uniqueid = 23438, name = "wMsgJ_RAD_DL_SET_HOST",         displayname = Labels.aicommands["wMsgJ_RAD_DL_SET_HOST"], enabled = true    } }, // hint
            { "wMsgJ_RAD_DL_TACT" ,             new CommandInfo { uniqueid = 23444, name = "wMsgJ_RAD_DL_TACT",             displayname = Labels.aicommands["wMsgJ_RAD_DL_TACT"], enabled = true           } },
            { "wMsgJ_RAD_DL_OFF" ,              new CommandInfo { uniqueid = 23439, name = "wMsgJ_RAD_DL_OFF",              displayname = Labels.aicommands["wMsgJ_RAD_DL_OFF"], enabled = true           } },
            { "wMsgJ_RAD_DL_FGHT" ,             new CommandInfo { uniqueid = 23440, name = "wMsgJ_RAD_DL_FGHT",             displayname = Labels.aicommands["wMsgJ_RAD_DL_FGHT"], enabled = true           } },


            { "wMsgJ_RAD_DL_SET_FREQ_1" ,       new CommandInfo { uniqueid = 23445, name = "wMsgJ_RAD_DL_SET_FREQ_1",       displayname = Labels.aicommands["wMsgJ_RAD_DL_SET_FREQ_1"], enabled = true            } },
            { "wMsgJ_RAD_DL_SET_FREQ_2" ,       new CommandInfo { uniqueid = 23446, name = "wMsgJ_RAD_DL_SET_FREQ_2",       displayname = Labels.aicommands["wMsgJ_RAD_DL_SET_FREQ_2"], enabled = true            } },
            { "wMsgJ_RAD_DL_SET_FREQ_3" ,       new CommandInfo { uniqueid = 23447, name = "wMsgJ_RAD_DL_SET_FREQ_3",       displayname = Labels.aicommands["wMsgJ_RAD_DL_SET_FREQ_3"], enabled = true            } },
            { "wMsgJ_RAD_DL_SET_FREQ_4" ,       new CommandInfo { uniqueid = 23448, name = "wMsgJ_RAD_DL_SET_FREQ_4",       displayname = Labels.aicommands["wMsgJ_RAD_DL_SET_FREQ_4"], enabled = true            } },
            { "wMsgJ_RAD_DL_SET_FREQ_5" ,       new CommandInfo { uniqueid = 23449, name = "wMsgJ_RAD_DL_SET_FREQ_5",       displayname = Labels.aicommands["wMsgJ_RAD_DL_SET_FREQ_5"], enabled = true            } },
            { "wMsgJ_RAD_DL_SET_FREQ_6" ,       new CommandInfo { uniqueid = 23450, name = "wMsgJ_RAD_DL_SET_FREQ_6",       displayname = Labels.aicommands["wMsgJ_RAD_DL_SET_FREQ_6"], enabled = true            } },
            { "wMsgJ_RAD_DL_SET_FREQ_7" ,       new CommandInfo { uniqueid = 23451, name = "wMsgJ_RAD_DL_SET_FREQ_7",       displayname = Labels.aicommands["wMsgJ_RAD_DL_SET_FREQ_7"], enabled = true            } },
            { "wMsgJ_RAD_DL_SET_FREQ_8" ,       new CommandInfo { uniqueid = 23452, name = "wMsgJ_RAD_DL_SET_FREQ_8",       displayname = Labels.aicommands["wMsgJ_RAD_DL_SET_FREQ_8"], enabled = true            } },

            { "wMsgJ_RAD_TCN" ,                 new CommandInfo { uniqueid = 23434, name = "wMsgJ_RAD_TCN",                 displayname = Labels.aicommands["wMsgJ_RAD_TCN"] } },// root: not endpoint
            { "wMsgJ_RAD_TCN_MODE" ,            new CommandInfo { uniqueid = 23453, name = "wMsgJ_RAD_TCN_MODE",            displayname = Labels.aicommands["wMsgJ_RAD_TCN_MODE"], enabled = true             } }, // not endpoint
            { "wMsgJ_RAD_TCN_MODE_OFF" ,        new CommandInfo { uniqueid = 23454, name = "wMsgJ_RAD_TCN_MODE_OFF",        displayname = Labels.aicommands["wMsgJ_RAD_TCN_MODE_OFF"], enabled = true             } },
            { "wMsgJ_RAD_TCN_MODE_REC" ,        new CommandInfo { uniqueid = 23455, name = "wMsgJ_RAD_TCN_MODE_REC",        displayname = Labels.aicommands["wMsgJ_RAD_TCN_MODE_REC"], enabled = true             } },
            { "wMsgJ_RAD_TCN_MODE_TR" ,         new CommandInfo { uniqueid = 23456, name = "wMsgJ_RAD_TCN_MODE_TR",         displayname = Labels.aicommands["wMsgJ_RAD_TCN_MODE_TR"], enabled = true             } },
            { "wMsgJ_RAD_TCN_MODE_AA" ,         new CommandInfo { uniqueid = 23457, name = "wMsgJ_RAD_TCN_MODE_AA",         displayname = Labels.aicommands["wMsgJ_RAD_TCN_MODE_AA"], enabled = true             } },
            { "wMsgJ_RAD_TCN_MODE_BCN" ,        new CommandInfo { uniqueid = 23458, name = "wMsgJ_RAD_TCN_MODE_BCN",        displayname = Labels.aicommands["wMsgJ_RAD_TCN_MODE_BCN"], enabled = true             } },

            { "wMsgJ_RAD_TCN_SEL_CHAN" ,        new CommandInfo { uniqueid = 23441, name = "wMsgJ_RAD_TCN_SEL_CHAN",        displayname = Labels.aicommands["wMsgJ_RAD_TCN_SEL_CHAN"], enabled = true           } },
            { "wMsgJ_RAD_TCN_SEL_GND_STN" ,     new CommandInfo { uniqueid = 23442, name = "wMsgJ_RAD_TCN_SEL_GND_STN",     displayname = Labels.aicommands["wMsgJ_RAD_TCN_SEL_GND_STN"], enabled = true           } },
            { "wMsgJ_RAD_TCN_TUNE_TAC" ,        new CommandInfo { uniqueid = 23443, name = "wMsgJ_RAD_TCN_TUNE_TAC",        displayname = Labels.aicommands["wMsgJ_RAD_TCN_TUNE_TAC"], enabled = true    } }, // hint

            { "wMsgJ_RAD_TCN_T_CS_TSK" ,            new CommandInfo { uniqueid = 23459, name = "wMsgJ_RAD_TCN_T_CS_TSK",            displayname = Labels.aicommands["wMsgJ_RAD_TCN_T_CS_TSK"], enabled = true           } },
            { "wMsgJ_RAD_TCN_T_CS_KBL" ,            new CommandInfo { uniqueid = 23460, name = "wMsgJ_RAD_TCN_T_CS_KBL",            displayname = Labels.aicommands["wMsgJ_RAD_TCN_T_CS_KBL"], enabled = true           } },
            { "wMsgJ_RAD_TCN_T_CS_BTM" ,            new CommandInfo { uniqueid = 23461, name = "wMsgJ_RAD_TCN_T_CS_BTM",            displayname = Labels.aicommands["wMsgJ_RAD_TCN_T_CS_BTM"], enabled = true           } },
            { "wMsgJ_RAD_TCN_T_CS_KTS" ,            new CommandInfo { uniqueid = 23462, name = "wMsgJ_RAD_TCN_T_CS_KTS",            displayname = Labels.aicommands["wMsgJ_RAD_TCN_T_CS_KTS"], enabled = true           } },
            { "wMsgJ_RAD_TCN_T_CS_GTB" ,            new CommandInfo { uniqueid = 23463, name = "wMsgJ_RAD_TCN_T_CS_GTB",            displayname = Labels.aicommands["wMsgJ_RAD_TCN_T_CS_GTB"], enabled = true           } },
            { "wMsgJ_RAD_TCN_T_CS_VAS" ,            new CommandInfo { uniqueid = 23464, name = "wMsgJ_RAD_TCN_T_CS_VAS",            displayname = Labels.aicommands["wMsgJ_RAD_TCN_T_CS_VAS"], enabled = true           } },
            { "wMsgJ_RAD_TCN_T_NV_LSV" ,            new CommandInfo { uniqueid = 23465, name = "wMsgJ_RAD_TCN_T_NV_LSV",            displayname = Labels.aicommands["wMsgJ_RAD_TCN_T_NV_LSV"], enabled = true           } },
            { "wMsgJ_RAD_TCN_T_NV_LAS" ,            new CommandInfo { uniqueid = 23466, name = "wMsgJ_RAD_TCN_T_NV_LAS",            displayname = Labels.aicommands["wMsgJ_RAD_TCN_T_NV_LAS"], enabled = true           } },
            { "wMsgJ_RAD_TCN_T_NV_BLD" ,            new CommandInfo { uniqueid = 23467, name = "wMsgJ_RAD_TCN_T_NV_BLD",            displayname = Labels.aicommands["wMsgJ_RAD_TCN_T_NV_BLD"], enabled = true           } },
            { "wMsgJ_RAD_TCN_T_NV_INS" ,            new CommandInfo { uniqueid = 23468, name = "wMsgJ_RAD_TCN_T_NV_INS",            displayname = Labels.aicommands["wMsgJ_RAD_TCN_T_NV_INS"], enabled = true           } },
            { "wMsgJ_RAD_TCN_T_NV_MMM" ,            new CommandInfo { uniqueid = 23469, name = "wMsgJ_RAD_TCN_T_NV_MMM",            displayname = Labels.aicommands["wMsgJ_RAD_TCN_T_NV_MMM"], enabled = true           } },
            { "wMsgJ_RAD_TCN_T_NV_GFS" ,            new CommandInfo { uniqueid = 23470, name = "wMsgJ_RAD_TCN_T_NV_GFS",            displayname = Labels.aicommands["wMsgJ_RAD_TCN_T_NV_GFS"], enabled = true           } },
            { "wMsgJ_RAD_TCN_T_NV_GRL" ,            new CommandInfo { uniqueid = 23471, name = "wMsgJ_RAD_TCN_T_NV_GRL",            displayname = Labels.aicommands["wMsgJ_RAD_TCN_T_NV_GRL"], enabled = true           } },
            { "wMsgJ_RAD_TCN_T_NV_PGS" ,            new CommandInfo { uniqueid = 23472, name = "wMsgJ_RAD_TCN_T_NV_PGS",            displayname = Labels.aicommands["wMsgJ_RAD_TCN_T_NV_PGS"], enabled = true           } },
            { "wMsgJ_RAD_TCN_T_NV_BTY" ,            new CommandInfo { uniqueid = 23473, name = "wMsgJ_RAD_TCN_T_NV_BTY",            displayname = Labels.aicommands["wMsgJ_RAD_TCN_T_NV_BTY"], enabled = true           } },
            { "wMsgJ_RAD_TCN_T_NV_EED" ,            new CommandInfo { uniqueid = 23474, name = "wMsgJ_RAD_TCN_T_NV_EED",            displayname = Labels.aicommands["wMsgJ_RAD_TCN_T_NV_EED"], enabled = true           } },
            { "wMsgJ_RAD_TCN_T_NV_DAG" ,            new CommandInfo { uniqueid = 23475, name = "wMsgJ_RAD_TCN_T_NV_DAG",            displayname = Labels.aicommands["wMsgJ_RAD_TCN_T_NV_DAG"], enabled = true           } },
            { "wMsgJ_RAD_TCN_T_NV_HEC" ,            new CommandInfo { uniqueid = 23476, name = "wMsgJ_RAD_TCN_T_NV_HEC",            displayname = Labels.aicommands["wMsgJ_RAD_TCN_T_NV_HEC"], enabled = true           } },
            { "wMsgJ_RAD_TCN_T_NV_OAL" ,            new CommandInfo { uniqueid = 23477, name = "wMsgJ_RAD_TCN_T_NV_OAL",            displayname = Labels.aicommands["wMsgJ_RAD_TCN_T_NV_OAL"], enabled = true           } },
            { "wMsgJ_RAD_TCN_T_NV_BIH" ,            new CommandInfo { uniqueid = 23478, name = "wMsgJ_RAD_TCN_T_NV_BIH",            displayname = Labels.aicommands["wMsgJ_RAD_TCN_T_NV_BIH"], enabled = true           } },
            { "wMsgJ_RAD_TCN_T_NV_MVA" ,            new CommandInfo { uniqueid = 23479, name = "wMsgJ_RAD_TCN_T_NV_MVA",            displayname = Labels.aicommands["wMsgJ_RAD_TCN_T_NV_MVA"], enabled = true           } },
            { "wMsgJ_RAD_TCN_T_PG_KCK" ,            new CommandInfo { uniqueid = 23480, name = "wMsgJ_RAD_TCN_T_PG_KCK",            displayname = Labels.aicommands["wMsgJ_RAD_TCN_T_PG_KCK"], enabled = true           } },
            { "wMsgJ_RAD_TCN_T_PG_KSB" ,            new CommandInfo { uniqueid = 23481, name = "wMsgJ_RAD_TCN_T_PG_KSB",            displayname = Labels.aicommands["wMsgJ_RAD_TCN_T_PG_KSB"], enabled = true           } },
            { "wMsgJ_RAD_TCN_T_PG_HDR" ,            new CommandInfo { uniqueid = 23482, name = "wMsgJ_RAD_TCN_T_PG_HDR",            displayname = Labels.aicommands["wMsgJ_RAD_TCN_T_PG_HDR"], enabled = true           } },
            { "wMsgJ_RAD_TCN_T_PG_MA" ,             new CommandInfo { uniqueid = 23483, name = "wMsgJ_RAD_TCN_T_PG_MA",             displayname = Labels.aicommands["wMsgJ_RAD_TCN_T_PG_MA"], enabled = true            } },
            { "wMsgJ_RAD_TCN_T_PG_SYZI" ,           new CommandInfo { uniqueid = 23484, name = "wMsgJ_RAD_TCN_T_PG_SYZI",           displayname = Labels.aicommands["wMsgJ_RAD_TCN_T_PG_SYZI"], enabled = true          } },
            { "wMsgJ_RAD_TCN_T_STN" ,               new CommandInfo { uniqueid = 23485, name = "wMsgJ_RAD_TCN_T_STN",               displayname = Labels.aicommands["wMsgJ_RAD_TCN_T_STN"],         } }, // disabled for now

            { "wMsgJ_RAD_182_MODE_TR" ,             new CommandInfo { uniqueid = 23486, name = "wMsgJ_RAD_182_MODE_TR",             displayname = Labels.aicommands["wMsgJ_RAD_182_MODE_TR"], enabled = true          } },
            { "wMsgJ_RAD_182_MODE_TRG" ,            new CommandInfo { uniqueid = 23487, name = "wMsgJ_RAD_182_MODE_TRG",            displayname = Labels.aicommands["wMsgJ_RAD_182_MODE_TRG"], enabled = true          } },
            { "wMsgJ_RAD_182_MODE_DF" ,             new CommandInfo { uniqueid = 23488, name = "wMsgJ_RAD_182_MODE_DF",             displayname = Labels.aicommands["wMsgJ_RAD_182_MODE_DF"], enabled = true          } },
            { "wMsgJ_RAD_182_MODE_TEST" ,           new CommandInfo { uniqueid = 23489, name = "wMsgJ_RAD_182_MODE_TEST",           displayname = Labels.aicommands["wMsgJ_RAD_182_MODE_TEST"], enabled = true          } },
            // end of radio

            // spare table
            { "wMsgJ_RAD_182_MODE_AM" ,             new CommandInfo { uniqueid = 23490, name = "wMsgJ_RAD_182_MODE_AM",             displayname = Labels.aicommands["wMsgJ_RAD_182_MODE_AM"], enabled = true          } },
            { "wMsgJ_RAD_182_MODE_FM" ,             new CommandInfo { uniqueid = 23491, name = "wMsgJ_RAD_182_MODE_FM",             displayname = Labels.aicommands["wMsgJ_RAD_182_MODE_FM"], enabled = true          } },
            { "wMsgJ_RAD_182_MODE" ,                new CommandInfo { uniqueid = 23492, name = "wMsgJ_RAD_182_MODE",                displayname = Labels.aicommands["wMsgJ_RAD_182_MODE"], enabled = true          } },
            { "wMsgJ_RAD_182_MODE_OFF" ,            new CommandInfo { uniqueid = 23493, name = "wMsgJ_RAD_182_MODE_OFF",            displayname = Labels.aicommands["wMsgJ_RAD_182_MODE_OFF"], enabled = true             } },

            { "wMsgJ_RAD_DL_HOST_STEN" ,            new CommandInfo { uniqueid = 23494, name = "wMsgJ_RAD_DL_HOST_STEN",            displayname = Labels.aicommands["wMsgJ_RAD_DL_HOST_STEN"], enabled = true           } },
            { "wMsgJ_RAD_DL_HOST_DARK" ,            new CommandInfo { uniqueid = 23495, name = "wMsgJ_RAD_DL_HOST_DARK",            displayname = Labels.aicommands["wMsgJ_RAD_DL_HOST_DARK"], enabled = true           } },
            { "wMsgJ_RAD_DL_HOST_FOCS" ,            new CommandInfo { uniqueid = 23496, name = "wMsgJ_RAD_DL_HOST_FOCS",            displayname = Labels.aicommands["wMsgJ_RAD_DL_HOST_FOCS"], enabled = true           } },
            { "wMsgJ_RAD_DL_HOST_MAGC" ,            new CommandInfo { uniqueid = 23497, name = "wMsgJ_RAD_DL_HOST_MAGC",            displayname = Labels.aicommands["wMsgJ_RAD_DL_HOST_MAGC"], enabled = true           } },
            { "wMsgJ_RAD_DL_HOST_OVRL" ,            new CommandInfo { uniqueid = 23498, name = "wMsgJ_RAD_DL_HOST_OVRL",            displayname = Labels.aicommands["wMsgJ_RAD_DL_HOST_OVRL"], enabled = true           } },
            { "wMsgJ_RAD_DL_HOST_WIZR" ,            new CommandInfo { uniqueid = 23499, name = "wMsgJ_RAD_DL_HOST_WIZR",            displayname = Labels.aicommands["wMsgJ_RAD_DL_HOST_WIZR"], enabled = true           } },
            // continues at 23537

            // block: utility id 23500-23599
            { "wMsgJ_UTIL_NAV" ,                new CommandInfo { uniqueid = 23500, name = "wMsgJ_UTIL_NAV",                displayname = Labels.aicommands["wMsgJ_UTIL_NAV"]               } }, // root, disable
            { "wMsgJ_UTIL_NAV_SEL_DEST_SPT" ,   new CommandInfo { uniqueid = 23501, name = "wMsgJ_UTIL_NAV_SEL_DEST_SPT",   displayname = Labels.aicommands["wMsgJ_UTIL_NAV_SEL_DEST_SPT"]    } }, // not used
            { "wMsgJ_UTIL_NAV_REST_MSN_SPT" ,   new CommandInfo { uniqueid = 23502, name = "wMsgJ_UTIL_NAV_REST_MSN_SPT",   displayname = Labels.aicommands["wMsgJ_UTIL_NAV_REST_MSN_SPT"]     } }, // not endpoint
            { "wMsgJ_UTIL_NAV_MAN_ENT_SPT" ,    new CommandInfo { uniqueid = 23503, name = "wMsgJ_UTIL_NAV_MAN_ENT_SPT",    displayname = Labels.aicommands["wMsgJ_UTIL_NAV_MAN_ENT_SPT"]     } }, //na
            { "wMsgJ_UTIL_NAV_MAP_SPT" ,        new CommandInfo { uniqueid = 23504, name = "wMsgJ_UTIL_NAV_MAP_SPT",        displayname = Labels.aicommands["wMsgJ_UTIL_NAV_MAP_SPT"], enabled = true    } }, //hint
            { "wMsgJ_UTIL_NAV_MAP_SPT_1" ,      new CommandInfo { uniqueid = 23505, name = "wMsgJ_UTIL_NAV_MAP_SPT_1",      displayname = Labels.aicommands["wMsgJ_UTIL_NAV_MAP_SPT_1"], enabled = true         } },
            { "wMsgJ_UTIL_NAV_MAP_SPT_2" ,      new CommandInfo { uniqueid = 23506, name = "wMsgJ_UTIL_NAV_MAP_SPT_2",      displayname = Labels.aicommands["wMsgJ_UTIL_NAV_MAP_SPT_2"], enabled = true         } },
            { "wMsgJ_UTIL_NAV_MAP_SPT_3" ,      new CommandInfo { uniqueid = 23507, name = "wMsgJ_UTIL_NAV_MAP_SPT_3",      displayname = Labels.aicommands["wMsgJ_UTIL_NAV_MAP_SPT_3"], enabled = true         } },
            { "wMsgJ_UTIL_NAV_MAP_SPT_4" ,      new CommandInfo { uniqueid = 23508, name = "wMsgJ_UTIL_NAV_MAP_SPT_4",      displayname = Labels.aicommands["wMsgJ_UTIL_NAV_MAP_SPT_4"]        } }, //na
            { "wMsgJ_UTIL_NAV_MAP_SPT_5" ,      new CommandInfo { uniqueid = 23509, name = "wMsgJ_UTIL_NAV_MAP_SPT_5",      displayname = Labels.aicommands["wMsgJ_UTIL_NAV_MAP_SPT_5"]       } }, //na
            { "wMsgJ_UTIL_NAV_MAP_SPT_6" ,      new CommandInfo { uniqueid = 23510, name = "wMsgJ_UTIL_NAV_MAP_SPT_6",      displayname = Labels.aicommands["wMsgJ_UTIL_NAV_MAP_SPT_6"]         } }, //na
            { "wMsgJ_UTIL_NAV_MAP_FIX_PNT" ,    new CommandInfo { uniqueid = 23513, name = "wMsgJ_UTIL_NAV_MAP_FIX_PNT",    displayname = Labels.aicommands["wMsgJ_UTIL_NAV_MAP_FIX_PNT"], enabled = true       } },
            { "wMsgJ_UTIL_NAV_MAP_INIT_PNT" ,   new CommandInfo { uniqueid = 23514, name = "wMsgJ_UTIL_NAV_MAP_INIT_PNT",   displayname = Labels.aicommands["wMsgJ_UTIL_NAV_MAP_INIT_PNT"], enabled = true      } },
            { "wMsgJ_UTIL_NAV_SURF_TGT" ,       new CommandInfo { uniqueid = 23515, name = "wMsgJ_UTIL_NAV_SURF_TGT",       displayname = Labels.aicommands["wMsgJ_UTIL_NAV_SURF_TGT"], enabled = true          } },
            { "wMsgJ_UTIL_NAV_HOME_BASE" ,      new CommandInfo { uniqueid = 23516, name = "wMsgJ_UTIL_NAV_HOME_BASE",      displayname = Labels.aicommands["wMsgJ_UTIL_NAV_HOME_BASE"], enabled = true         } },

            { "wMsgJ_UTIL_NAV_REST_MORE" ,      new CommandInfo { uniqueid = 23517, name = "wMsgJ_UTIL_NAV_REST_MORE",      displayname = Labels.aicommands["wMsgJ_UTIL_NAV_REST_MORE"], enabled = true          } }, // not endpoint, disable            
            { "wMsgJ_UTIL_NAV_REST_MSN_SPT_1" , new CommandInfo { uniqueid = 23520, name = "wMsgJ_UTIL_NAV_REST_MSN_SPT_1", displayname = Labels.aicommands["wMsgJ_UTIL_NAV_REST_MSN_SPT_1"], enabled = true    } },
            { "wMsgJ_UTIL_NAV_REST_MSN_SPT_2" , new CommandInfo { uniqueid = 23521, name = "wMsgJ_UTIL_NAV_REST_MSN_SPT_2", displayname = Labels.aicommands["wMsgJ_UTIL_NAV_REST_MSN_SPT_2"], enabled = true    } },
            { "wMsgJ_UTIL_NAV_REST_MSN_SPT_3" , new CommandInfo { uniqueid = 23532, name = "wMsgJ_UTIL_NAV_REST_MSN_SPT_3", displayname = Labels.aicommands["wMsgJ_UTIL_NAV_REST_MSN_SPT_3"], enabled = true    } },
            { "wMsgJ_UTIL_NAV_REST_INIT_PT_1" , new CommandInfo { uniqueid = 23523, name = "wMsgJ_UTIL_NAV_REST_INIT_PT_1", displayname = Labels.aicommands["wMsgJ_UTIL_NAV_REST_INIT_PT_1"], enabled = true    } },
            { "wMsgJ_UTIL_NAV_REST_INIT_PT_2" , new CommandInfo { uniqueid = 23524, name = "wMsgJ_UTIL_NAV_REST_INIT_PT_2", displayname = Labels.aicommands["wMsgJ_UTIL_NAV_REST_INIT_PT_2"]    } }, // na
            { "wMsgJ_UTIL_NAV_REST_FIX_PT" ,    new CommandInfo { uniqueid = 23525, name = "wMsgJ_UTIL_NAV_REST_FIX_PT",    displayname = Labels.aicommands["wMsgJ_UTIL_NAV_REST_FIX_PT"], enabled = true       } },
            { "wMsgJ_UTIL_NAV_REST_MN_FIX_PT" , new CommandInfo { uniqueid = 23526, name = "wMsgJ_UTIL_NAV_REST_MN_FIX_PT", displayname = Labels.aicommands["wMsgJ_UTIL_NAV_REST_MN_FIX_PT"]   } }, // na
            { "wMsgJ_UTIL_NAV_REST_STGT_1" ,    new CommandInfo { uniqueid = 23527, name = "wMsgJ_UTIL_NAV_REST_STGT_1",    displayname = Labels.aicommands["wMsgJ_UTIL_NAV_REST_STGT_1"], enabled = true           } },
            { "wMsgJ_UTIL_NAV_REST_HOME" ,      new CommandInfo { uniqueid = 23528, name = "wMsgJ_UTIL_NAV_REST_HOME",      displayname = Labels.aicommands["wMsgJ_UTIL_NAV_REST_HOME"], enabled = true           } },
            { "wMsgJ_UTIL_NAV_DEF_PNT" ,        new CommandInfo { uniqueid = 23511, name = "wMsgJ_UTIL_NAV_DEF_PNT",        displayname = Labels.aicommands["wMsgJ_UTIL_NAV_DEF_PNT"], enabled = true           } },
            { "wMsgJ_UTIL_NAV_HSTZONE" ,        new CommandInfo { uniqueid = 23512, name = "wMsgJ_UTIL_NAV_HSTZONE",        displayname = Labels.aicommands["wMsgJ_UTIL_NAV_HSTZONE"] , enabled = true         } },

            { "wMsgJ_UTIL_CONTR" ,              new CommandInfo { uniqueid = 23518, name = "wMsgJ_UTIL_CONTR",              displayname = Labels.aicommands["wMsgJ_UTIL_CONTR"]               } }, // not endpoint, disable
            { "wMsgJ_UTIL_CONTR_NO_TALK" ,      new CommandInfo { uniqueid = 23519, name = "wMsgJ_UTIL_CONTR_NO_TALK",      displayname = Labels.aicommands["wMsgJ_UTIL_CONTR_NO_TALK"], enabled = true         } },
            { "wMsgJ_UTIL_CONTR_TALK" ,         new CommandInfo { uniqueid = 23529, name = "wMsgJ_UTIL_CONTR_TALK",         displayname = Labels.aicommands["wMsgJ_UTIL_CONTR_TALK"], enabled = true            } },
            { "wMsgJ_UTIL_CONTR_EJECT_BTH" ,    new CommandInfo { uniqueid = 23530, name = "wMsgJ_UTIL_CONTR_EJECT_BTH",    displayname = Labels.aicommands["wMsgJ_UTIL_CONTR_EJECT_BTH"], enabled = true            } },
            { "wMsgJ_UTIL_CONTR_EJECT_SNG" ,    new CommandInfo { uniqueid = 23532, name = "wMsgJ_UTIL_CONTR_EJECT_SNG",    displayname = Labels.aicommands["wMsgJ_UTIL_CONTR_EJECT_SNG"], enabled = true            } },
            { "wMsgJ_UTIL_CONTR_ACTIVE" ,       new CommandInfo { uniqueid = 23533, name = "wMsgJ_UTIL_CONTR_ACTIVE",       displayname = Labels.aicommands["wMsgJ_UTIL_CONTR_ACTIVE"], enabled = true          } }, // na
            { "wMsgJ_UTIL_CONTR_INACTIVE" ,     new CommandInfo { uniqueid = 23534, name = "wMsgJ_UTIL_CONTR_INACTIVE",     displayname = Labels.aicommands["wMsgJ_UTIL_CONTR_INACTIVE"], enabled = true          } }, //na
            { "wMsgJ_UTIL_CONTR_CALL" ,         new CommandInfo { uniqueid = 23535, name = "wMsgJ_UTIL_CONTR_CALL",         displayname = Labels.aicommands["wMsgJ_UTIL_CONTR_CALL"], enabled = true           } },
            { "wMsgJ_UTIL_CONTR_NO_CALL" ,      new CommandInfo { uniqueid = 23531, name = "wMsgJ_UTIL_CONTR_NO_CALL",      displayname = Labels.aicommands["wMsgJ_UTIL_CONTR_NO_CALL"], enabled = true           } },
            { "wMsgJ_UTIL_CONTR_AUTO_EXPAND" ,  new CommandInfo { uniqueid = 23584, name = "wMsgJ_UTIL_CONTR_AUTO_EXPAND",  displayname = Labels.aicommands["wMsgJ_UTIL_CONTR_AUTO_EXPAND"], enabled = true        } },
            { "wMsgJ_UTIL_CONTR_AUTO_VID" ,     new CommandInfo { uniqueid = 23585, name = "wMsgJ_UTIL_CONTR_AUTO_VID",     displayname = Labels.aicommands["wMsgJ_UTIL_CONTR_AUTO_VID"], enabled = true           } },
            { "wMsgJ_UTIL_CONTR_NO_IFF_REPLY_BANDIT" , new CommandInfo { uniqueid = 23586, name = "wMsgJ_UTIL_CONTR_NO_IFF_REPLY_BANDIT", displayname = Labels.aicommands["wMsgJ_UTIL_CONTR_NO_IFF_REPLY_BANDIT"], enabled = true } },
            { "wMsgJ_UTIL_CONTR_DISABLE_AUTO_EXPAND" , new CommandInfo { uniqueid = 23587, name = "wMsgJ_UTIL_CONTR_DISABLE_AUTO_EXPAND", displayname = Labels.aicommands["wMsgJ_UTIL_CONTR_DISABLE_AUTO_EXPAND"], enabled = true } },
            { "wMsgJ_UTIL_CONTR_ENABLE_AUTO_VID" ,    new CommandInfo { uniqueid = 23588, name = "wMsgJ_UTIL_CONTR_ENABLE_AUTO_VID", displayname = Labels.aicommands["wMsgJ_UTIL_CONTR_ENABLE_AUTO_VID"], enabled = true } },
            { "wMsgJ_UTIL_CONTR_NO_IFF_REPLY_BOGEY" , new CommandInfo { uniqueid = 23589, name = "wMsgJ_UTIL_CONTR_NO_IFF_REPLY_BOGEY", displayname = Labels.aicommands["wMsgJ_UTIL_CONTR_NO_IFF_REPLY_BOGEY"], enabled = true } },

            { "wMsgJ_RESET" ,                   new CommandInfo { uniqueid = 23536, name = "wMsgJ_RESET",                   displayname = Labels.aicommands["wMsgJ_RESET"], enabled = true           } },

            // Supercarriers
            { "wMsgJ_RAD_DL_HOST_WASH" ,        new CommandInfo { uniqueid = 23537, name = "wMsgJ_RAD_DL_HOST_WASH",        displayname = Labels.aicommands["wMsgJ_RAD_DL_HOST_WASH"], enabled = true            } },
            { "wMsgJ_RAD_DL_HOST_ROOS" ,        new CommandInfo { uniqueid = 23538, name = "wMsgJ_RAD_DL_HOST_ROOS",        displayname = Labels.aicommands["wMsgJ_RAD_DL_HOST_ROOS"], enabled = true            } },
            { "wMsgJ_RAD_DL_HOST_LINC" ,        new CommandInfo { uniqueid = 23539, name = "wMsgJ_RAD_DL_HOST_LINC",        displayname = Labels.aicommands["wMsgJ_RAD_DL_HOST_LINC"], enabled = true            } },
            { "wMsgJ_RAD_DL_HOST_TRUM" ,        new CommandInfo { uniqueid = 23540, name = "wMsgJ_RAD_DL_HOST_TRUM",        displayname = Labels.aicommands["wMsgJ_RAD_DL_HOST_TRUM"], enabled = true            } },
            { "wMsgJ_RAD_DL_HOST_TICO" ,        new CommandInfo { uniqueid = 23541, name = "wMsgJ_RAD_DL_HOST_TICO",        displayname = Labels.aicommands["wMsgJ_RAD_DL_HOST_TICO"], enabled = true            } },
            { "wMsgJ_RAD_DL_HOST_FORE" ,        new CommandInfo { uniqueid = 23541, name = "wMsgJ_RAD_DL_HOST_FORE",        displayname = Labels.aicommands["wMsgJ_RAD_DL_HOST_FORE"], enabled = true            } }, // Add Forrestal
            { "wMsgJ_RAD_DL_HOST_BURK" ,        new CommandInfo { uniqueid = 23583, name = "wMsgJ_RAD_DL_HOST_BURK",        displayname = Labels.aicommands["wMsgJ_RAD_DL_HOST_BURK"], enabled = true            } }, // Add Arleigh Burke

            { "wMsgJESTER_Steerpoint_SP1" ,     new CommandInfo { uniqueid = 23542, name = "wMsgJESTER_Steerpoint_SP1",     displayname = Labels.aicommands["wMsgJESTER_Steerpoint_SP1"], enabled = true             } },//
            { "wMsgJESTER_Steerpoint_SP2" ,     new CommandInfo { uniqueid = 23543, name = "wMsgJESTER_Steerpoint_SP2",     displayname = Labels.aicommands["wMsgJESTER_Steerpoint_SP2"], enabled = true             } },//
            { "wMsgJESTER_Steerpoint_SP3" ,     new CommandInfo { uniqueid = 23544, name = "wMsgJESTER_Steerpoint_SP3",     displayname = Labels.aicommands["wMsgJESTER_Steerpoint_SP3"], enabled = true             } },//
            { "wMsgJESTER_Steerpoint_FP" ,      new CommandInfo { uniqueid = 23545, name = "wMsgJESTER_Steerpoint_FP",      displayname = Labels.aicommands["wMsgJESTER_Steerpoint_FP"] , enabled = true            } },//
            { "wMsgJESTER_Steerpoint_IP" ,      new CommandInfo { uniqueid = 23546, name = "wMsgJESTER_Steerpoint_IP",      displayname = Labels.aicommands["wMsgJESTER_Steerpoint_IP"] , enabled = true            } },//
            { "wMsgJESTER_Steerpoint_ST" ,      new CommandInfo { uniqueid = 23547, name = "wMsgJESTER_Steerpoint_ST",      displayname = Labels.aicommands["wMsgJESTER_Steerpoint_ST"] , enabled = true            } },//
            { "wMsgJESTER_Steerpoint_HB" ,      new CommandInfo { uniqueid = 23548, name = "wMsgJESTER_Steerpoint_HB",      displayname = Labels.aicommands["wMsgJESTER_Steerpoint_HB"] , enabled = true            } },//
            { "wMsgJESTER_Steerpoint_MAN" ,     new CommandInfo { uniqueid = 23549, name = "wMsgJESTER_Steerpoint_MAN",     displayname = Labels.aicommands["wMsgJESTER_Steerpoint_MAN"], enabled = true             } },//
            { "wMsgLANTIRN_GPSZero" ,           new CommandInfo { uniqueid = 23550, name = "wMsgLANTIRN_GPSZero",           displayname = Labels.aicommands["wMsgLANTIRN_GPSZero"], enabled = true          } },//
            { "wMsgLANTIRN_ToggleFOV" ,         new CommandInfo { uniqueid = 23551, name = "wMsgLANTIRN_ToggleFOV",         displayname = Labels.aicommands["wMsgLANTIRN_ToggleFOV"], enabled = true          } },//
            { "wMsgJ_UTIL_NAV_MAP_MARKER" ,     new CommandInfo { uniqueid = 23552, name = "wMsgJ_UTIL_NAV_MAP_MARKER",     displayname = Labels.aicommands["wMsgJ_UTIL_NAV_MAP_MARKER"]   } },
            { "wMsgJ_UTIL_NAV_GRD_ENABLE" ,     new CommandInfo { uniqueid = 23553, name = "wMsgJ_UTIL_NAV_GRD_ENABLE",     displayname = Labels.aicommands["wMsgJ_UTIL_NAV_GRD_ENABLE"], enabled = true    } },
            { "wMsgJ_UTIL_NAV_GRD_DSABLE" ,     new CommandInfo { uniqueid = 23554, name = "wMsgJ_UTIL_NAV_GRD_DSABLE",     displayname = Labels.aicommands["wMsgJ_UTIL_NAV_GRD_DSABLE"], enabled = true    } },
            { "wMsgJ_UTIL_NAV_GRD_CENTER" ,     new CommandInfo { uniqueid = 23555, name = "wMsgJ_UTIL_NAV_GRD_CENTER",     displayname = Labels.aicommands["wMsgJ_UTIL_NAV_GRD_CENTER"], enabled = true    } },
            { "wMsgJ_UTIL_NAV_GRD_REL_180" ,    new CommandInfo { uniqueid = 23556, name = "wMsgJ_UTIL_NAV_GRD_REL_180",    displayname = Labels.aicommands["wMsgJ_UTIL_NAV_GRD_REL_180"], enabled = true   } },
            { "wMsgJ_UTIL_NAV_GRD_REL_u30" ,    new CommandInfo { uniqueid = 23557, name = "wMsgJ_UTIL_NAV_GRD_REL_u30",    displayname = Labels.aicommands["wMsgJ_UTIL_NAV_GRD_REL_u30"], enabled = true   } },
            { "wMsgJ_UTIL_NAV_GRD_REL_u90" ,    new CommandInfo { uniqueid = 23558, name = "wMsgJ_UTIL_NAV_GRD_REL_u90",    displayname = Labels.aicommands["wMsgJ_UTIL_NAV_GRD_REL_u90"], enabled = true   } },
            { "wMsgJ_UTIL_NAV_GRD_REL_u120" ,   new CommandInfo { uniqueid = 23559, name = "wMsgJ_UTIL_NAV_GRD_REL_u120",   displayname = Labels.aicommands["wMsgJ_UTIL_NAV_GRD_REL_u120"], enabled = true  } },
            { "wMsgJ_UTIL_NAV_GRD_REL_d120" ,   new CommandInfo { uniqueid = 23560, name = "wMsgJ_UTIL_NAV_GRD_REL_d120",   displayname = Labels.aicommands["wMsgJ_UTIL_NAV_GRD_REL_d120"], enabled = true  } },
            { "wMsgJ_UTIL_NAV_GRD_REL_d90" ,    new CommandInfo { uniqueid = 23561, name = "wMsgJ_UTIL_NAV_GRD_REL_d90",    displayname = Labels.aicommands["wMsgJ_UTIL_NAV_GRD_REL_d90"] , enabled = true  } },
            { "wMsgJ_UTIL_NAV_GRD_REL_d30" ,    new CommandInfo { uniqueid = 23562, name = "wMsgJ_UTIL_NAV_GRD_REL_d30",    displayname = Labels.aicommands["wMsgJ_UTIL_NAV_GRD_REL_d30"], enabled = true   } },
            { "wMsgJ_UTIL_NAV_GRD_ABS_0" ,      new CommandInfo { uniqueid = 23563, name = "wMsgJ_UTIL_NAV_GRD_ABS_0",      displayname = Labels.aicommands["wMsgJ_UTIL_NAV_GRD_ABS_0"] , enabled = true    } },
            { "wMsgJ_UTIL_NAV_GRD_ABS_45" ,     new CommandInfo { uniqueid = 23564, name = "wMsgJ_UTIL_NAV_GRD_ABS_45",     displayname = Labels.aicommands["wMsgJ_UTIL_NAV_GRD_ABS_45"] , enabled = true   } },
            { "wMsgJ_UTIL_NAV_GRD_ABS_90" ,     new CommandInfo { uniqueid = 23565, name = "wMsgJ_UTIL_NAV_GRD_ABS_90",     displayname = Labels.aicommands["wMsgJ_UTIL_NAV_GRD_ABS_90"] , enabled = true   } },
            { "wMsgJ_UTIL_NAV_GRD_ABS_135" ,    new CommandInfo { uniqueid = 23566, name = "wMsgJ_UTIL_NAV_GRD_ABS_135",    displayname = Labels.aicommands["wMsgJ_UTIL_NAV_GRD_ABS_135"], enabled = true   } },
            { "wMsgJ_UTIL_NAV_GRD_ABS_180" ,    new CommandInfo { uniqueid = 23567, name = "wMsgJ_UTIL_NAV_GRD_ABS_180",    displayname = Labels.aicommands["wMsgJ_UTIL_NAV_GRD_ABS_180"] , enabled = true  } },
            { "wMsgJ_UTIL_NAV_GRD_ABS_225" ,    new CommandInfo { uniqueid = 23568, name = "wMsgJ_UTIL_NAV_GRD_ABS_225",    displayname = Labels.aicommands["wMsgJ_UTIL_NAV_GRD_ABS_225"] , enabled = true  } },
            { "wMsgJ_UTIL_NAV_GRD_ABS_270" ,    new CommandInfo { uniqueid = 23569, name = "wMsgJ_UTIL_NAV_GRD_ABS_270",    displayname = Labels.aicommands["wMsgJ_UTIL_NAV_GRD_ABS_270"], enabled = true   } },
            { "wMsgJ_UTIL_NAV_GRD_ABS_315" ,    new CommandInfo { uniqueid = 23570, name = "wMsgJ_UTIL_NAV_GRD_ABS_315",    displayname = Labels.aicommands["wMsgJ_UTIL_NAV_GRD_ABS_315"] , enabled = true  } },
            { "wMsgJ_UTIL_NAV_GRD_COV_30" ,     new CommandInfo { uniqueid = 23571, name = "wMsgJ_UTIL_NAV_GRD_COV_30",     displayname = Labels.aicommands["wMsgJ_UTIL_NAV_GRD_COV_30"] , enabled = true   } },
            { "wMsgJ_UTIL_NAV_GRD_COV_60" ,     new CommandInfo { uniqueid = 23572, name = "wMsgJ_UTIL_NAV_GRD_COV_60",     displayname = Labels.aicommands["wMsgJ_UTIL_NAV_GRD_COV_60"]  , enabled = true  } },
            { "wMsgJ_UTIL_NAV_GRD_COV_90" ,     new CommandInfo { uniqueid = 23573, name = "wMsgJ_UTIL_NAV_GRD_COV_90",     displayname = Labels.aicommands["wMsgJ_UTIL_NAV_GRD_COV_90"]  , enabled = true  } },
            { "wMsgJ_UTIL_NAV_GRD_COV_120" ,    new CommandInfo { uniqueid = 23574, name = "wMsgJ_UTIL_NAV_GRD_COV_120",    displayname = Labels.aicommands["wMsgJ_UTIL_NAV_GRD_COV_120"] , enabled = true  } },
            { "wMsgJ_UTIL_NAV_GRD_COV_180" ,    new CommandInfo { uniqueid = 23575, name = "wMsgJ_UTIL_NAV_GRD_COV_180",    displayname = Labels.aicommands["wMsgJ_UTIL_NAV_GRD_COV_180"] , enabled = true  } },
            { "wMsgJ_UTIL_NAV_GRD_1SCTR" ,            new CommandInfo { uniqueid = 23576, name = "wMsgJ_UTIL_NAV_GRD_1SCTR",            displayname = Labels.aicommands["wMsgJ_UTIL_NAV_GRD_1SCTR"] , enabled = true          } },
            { "wMsgJ_UTIL_NAV_GRD_2SCTR" ,            new CommandInfo { uniqueid = 23577, name = "wMsgJ_UTIL_NAV_GRD_2SCTR",            displayname = Labels.aicommands["wMsgJ_UTIL_NAV_GRD_2SCTR"] , enabled = true          } },
            { "wMsgJ_UTIL_NAV_GRD_3SCTR" ,            new CommandInfo { uniqueid = 23578, name = "wMsgJ_UTIL_NAV_GRD_3SCTR",            displayname = Labels.aicommands["wMsgJ_UTIL_NAV_GRD_3SCTR"]  , enabled = true         } },
            { "wMsgJ_UTIL_NAV_GRD_4SCTR" ,            new CommandInfo { uniqueid = 23579, name = "wMsgJ_UTIL_NAV_GRD_4SCTR",            displayname = Labels.aicommands["wMsgJ_UTIL_NAV_GRD_4SCTR"]  , enabled = true         } },
            { "wMsgJ_UTIL_NAV_GRD_5SCTR" ,            new CommandInfo { uniqueid = 23580, name = "wMsgJ_UTIL_NAV_GRD_5SCTR",            displayname = Labels.aicommands["wMsgJ_UTIL_NAV_GRD_5SCTR"]  , enabled = true         } },
            { "wMsgJ_UTIL_NAV_GRD_6SCTR" ,            new CommandInfo { uniqueid = 23581, name = "wMsgJ_UTIL_NAV_GRD_6SCTR",            displayname = Labels.aicommands["wMsgJ_UTIL_NAV_GRD_6SCTR"]  , enabled = true         } },
            { "wMsgJ_UTIL_NAV_GRD_MARKER" ,            new CommandInfo { uniqueid = 23582, name = "wMsgJ_UTIL_NAV_GRD_MARKER",            displayname = Labels.aicommands["wMsgJ_UTIL_NAV_GRD_MARKER"]          } },
            //{ "wMsgPlaceHolder383" ,            new CommandInfo { uniqueid = 23583, name = "wMsgPlaceHolder383",            displayname = Labels.aicommands["wMsgPlaceHolder383"]          } },
            //{ "wMsgPlaceHolder384" ,            new CommandInfo { uniqueid = 23584, name = "wMsgPlaceHolder384",            displayname = Labels.aicommands["wMsgPlaceHolder384"]          } },
            //{ "wMsgPlaceHolder385" ,            new CommandInfo { uniqueid = 23585, name = "wMsgPlaceHolder385",            displayname = Labels.aicommands["wMsgPlaceHolder385"]          } },
            //{ "wMsgPlaceHolder386" ,            new CommandInfo { uniqueid = 23586, name = "wMsgPlaceHolder386",            displayname = Labels.aicommands["wMsgPlaceHolder386"]          } },
            //{ "wMsgPlaceHolder387" ,            new CommandInfo { uniqueid = 23587, name = "wMsgPlaceHolder387",            displayname = Labels.aicommands["wMsgPlaceHolder387"]          } },
            //{ "wMsgPlaceHolder388" ,            new CommandInfo { uniqueid = 23588, name = "wMsgPlaceHolder388",            displayname = Labels.aicommands["wMsgPlaceHolder388"]          } },
            //{ "wMsgPlaceHolder389" ,            new CommandInfo { uniqueid = 23589, name = "wMsgPlaceHolder389",            displayname = Labels.aicommands["wMsgPlaceHolder389"]          } },
            // end of utility

            // spare table : WALKMAN 
            { "wMsgJ_WLKMN_PLAY" ,            new CommandInfo { uniqueid = 23590, name = "wMsgJ_WLKMN_PLAY",            displayname = Labels.aicommands["wMsgJ_WLKMN_PLAY"], enabled = true           } },
            { "wMsgJ_WLKMN_STOP" ,            new CommandInfo { uniqueid = 23591, name = "wMsgJ_WLKMN_STOP",            displayname = Labels.aicommands["wMsgJ_WLKMN_STOP"], enabled = true           } },
            { "wMsgJ_WLKMN_NEXT" ,            new CommandInfo { uniqueid = 23592, name = "wMsgJ_WLKMN_NEXT",            displayname = Labels.aicommands["wMsgJ_WLKMN_NEXT"], enabled = true           } },
            { "wMsgJ_WLKMN_PREV" ,            new CommandInfo { uniqueid = 23593, name = "wMsgJ_WLKMN_PREV",            displayname = Labels.aicommands["wMsgJ_WLKMN_PREV"], enabled = true           } },
            { "wMsgJ_RDR_ASP_BEAM" ,          new CommandInfo { uniqueid = 23594, name = "wMsgJ_RDR_ASP_BEAM",            displayname = Labels.aicommands["wMsgJ_RDR_ASP_BEAM"], enabled = true            } },
            { "wMsgJ_RDR_ASP_NOSE" ,          new CommandInfo { uniqueid = 23595, name = "wMsgJ_RDR_ASP_NOSE",            displayname = Labels.aicommands["wMsgJ_RDR_ASP_NOSE"], enabled = true            } },
            { "wMsgJ_RDR_ASP_TAIL" ,          new CommandInfo { uniqueid = 23596, name = "wMsgJ_RDR_ASP_TAIL",            displayname = Labels.aicommands["wMsgJ_RDR_ASP_TAIL"], enabled = true            } },
            //{ "wMsgPlaceHolder397" ,            new CommandInfo { uniqueid = 23597, name = "wMsgPlaceHolder397",            displayname = Labels.aicommands["wMsgPlaceHolder397"]          } },
            //{ "wMsgPlaceHolder398" ,            new CommandInfo { uniqueid = 23598, name = "wMsgPlaceHolder398",            displayname = Labels.aicommands["wMsgPlaceHolder398"]          } },
            //{ "wMsgPlaceHolder399" ,            new CommandInfo { uniqueid = 23599, name = "wMsgPlaceHolder399",            displayname = Labels.aicommands["wMsgPlaceHolder399"]          } },

            // block: defensive id 23600-23699
            { "wMsgJ_DEF_CMS_MOD" ,             new CommandInfo { uniqueid = 23600, name = "wMsgJ_DEF_CMS_MOD",             displayname = Labels.aicommands["wMsgJ_DEF_CMS_MOD"], enabled = true            } },
            { "wMsgJ_DEF_CMS_MOD_OFF" ,         new CommandInfo { uniqueid = 23601, name = "wMsgJ_DEF_CMS_MOD_OFF",         displayname = Labels.aicommands["wMsgJ_DEF_CMS_MOD_OFF"], enabled = true        } },
            { "wMsgJ_DEF_CMS_MOD_MAN" ,         new CommandInfo { uniqueid = 23602, name = "wMsgJ_DEF_CMS_MOD_MAN",         displayname = Labels.aicommands["wMsgJ_DEF_CMS_MOD_MAN"], enabled = true        } },
            { "wMsgJ_DEF_CMS_MOD_AUTO" ,        new CommandInfo { uniqueid = 23603, name = "wMsgJ_DEF_CMS_MOD_AUTO",        displayname = Labels.aicommands["wMsgJ_DEF_CMS_MOD_AUTO"], enabled = true       } },

            { "wMsgJ_DEF_FLR_MOD" ,             new CommandInfo { uniqueid = 23604, name = "wMsgJ_DEF_FLR_MOD",             displayname = Labels.aicommands["wMsgJ_DEF_FLR_MOD"], enabled = true            } },
            { "wMsgJ_DEF_FLR_MOD_PILOT" ,       new CommandInfo { uniqueid = 23605, name = "wMsgJ_DEF_FLR_MOD_PILOT",       displayname = Labels.aicommands["wMsgJ_DEF_FLR_MOD_PILOT"], enabled = true      } },
            { "wMsgJ_DEF_FLR_MOD_NORM" ,        new CommandInfo { uniqueid = 23606, name = "wMsgJ_DEF_FLR_MOD_NORM",        displayname = Labels.aicommands["wMsgJ_DEF_FLR_MOD_NORM"], enabled = true       } },
            { "wMsgJ_DEF_FLR_MOD_MULTI" ,       new CommandInfo { uniqueid = 23607, name = "wMsgJ_DEF_FLR_MOD_MULTI",       displayname = Labels.aicommands["wMsgJ_DEF_FLR_MOD_MULTI"], enabled = true      } },

            { "wMsgJ_DEF_CHF_PGM" ,             new CommandInfo { uniqueid = 23608, name = "wMsgJ_DEF_CHF_PGM",             displayname = Labels.aicommands["wMsgJ_DEF_CHF_PGM"], enabled = true            } },
            { "wMsgJ_DEF_CHF_PGM_RR_12" ,       new CommandInfo { uniqueid = 23609, name = "wMsgJ_DEF_CHF_PGM_RR_12",       displayname = Labels.aicommands["wMsgJ_DEF_CHF_PGM_RR_12"], enabled = true      } },
            { "wMsgJ_DEF_CHF_PGM_RR_46" ,       new CommandInfo { uniqueid = 23610, name = "wMsgJ_DEF_CHF_PGM_RR_46",       displayname = Labels.aicommands["wMsgJ_DEF_CHF_PGM_RR_46"], enabled = true      } },
            { "wMsgJ_DEF_CHF_PGM_RR_86" ,       new CommandInfo { uniqueid = 23611, name = "wMsgJ_DEF_CHF_PGM_RR_86",       displayname = Labels.aicommands["wMsgJ_DEF_CHF_PGM_RR_86"], enabled = true      } },
            { "wMsgJ_DEF_CHF_PGM_20_44" ,       new CommandInfo { uniqueid = 23612, name = "wMsgJ_DEF_CHF_PGM_20_44",       displayname = Labels.aicommands["wMsgJ_DEF_CHF_PGM_20_44"], enabled = true      } },
            { "wMsgJ_DEF_CHF_PGM_20_84" ,       new CommandInfo { uniqueid = 23613, name = "wMsgJ_DEF_CHF_PGM_20_84",       displayname = Labels.aicommands["wMsgJ_DEF_CHF_PGM_20_84"], enabled = true      } },
            { "wMsgJ_DEF_CHF_PGM_40_44" ,       new CommandInfo { uniqueid = 23614, name = "wMsgJ_DEF_CHF_PGM_40_44",       displayname = Labels.aicommands["wMsgJ_DEF_CHF_PGM_40_44"], enabled = true      } },
            { "wMsgJ_DEF_CHF_PGM_40_84" ,       new CommandInfo { uniqueid = 23615, name = "wMsgJ_DEF_CHF_PGM_40_84",       displayname = Labels.aicommands["wMsgJ_DEF_CHF_PGM_40_84"], enabled = true      } },
            { "wMsgJ_DEF_CHF_PGM_R1_12" ,       new CommandInfo { uniqueid = 23616, name = "wMsgJ_DEF_CHF_PGM_R1_12",       displayname = Labels.aicommands["wMsgJ_DEF_CHF_PGM_R1_12"], enabled = true      } },

            { "wMsgJ_DEF_RWR_DSP_TYP" ,         new CommandInfo { uniqueid = 23619, name = "wMsgJ_DEF_RWR_DSP_TYP",         displayname = Labels.aicommands["wMsgJ_DEF_RWR_DSP_TYP"], enabled = true        } }, // not endpoint, hint
            { "wMsgJ_DEF_RWR_AIRB" ,            new CommandInfo { uniqueid = 23620, name = "wMsgJ_DEF_RWR_AIRB",            displayname = Labels.aicommands["wMsgJ_DEF_RWR_AIRB"], enabled = true           } },
            { "wMsgJ_DEF_RWR_NORM" ,            new CommandInfo { uniqueid = 23621, name = "wMsgJ_DEF_RWR_NORM",            displayname = Labels.aicommands["wMsgJ_DEF_RWR_NORM"], enabled = true           } },
            { "wMsgJ_DEF_RWR_AAA" ,             new CommandInfo { uniqueid = 23642, name = "wMsgJ_DEF_RWR_AAA",             displayname = Labels.aicommands["wMsgJ_DEF_RWR_AAA"], enabled = true           } },
            { "wMsgJ_DEF_RWR_UNK" ,             new CommandInfo { uniqueid = 23623, name = "wMsgJ_DEF_RWR_UNK",             displayname = Labels.aicommands["wMsgJ_DEF_RWR_UNK"], enabled = true           } },
            { "wMsgJ_DEF_RWR_FRND" ,            new CommandInfo { uniqueid = 23624, name = "wMsgJ_DEF_RWR_FRND",            displayname = Labels.aicommands["wMsgJ_DEF_RWR_FRND"], enabled = true           } },

            { "wMsgJ_DEF_JMR_ON" ,              new CommandInfo { uniqueid = 23640, name = "wMsgJ_DEF_JMR_ON",              displayname = Labels.aicommands["wMsgJ_DEF_JMR_ON"], enabled = true             } },
            { "wMsgJ_DEF_JMR_SBY" ,             new CommandInfo { uniqueid = 23625, name = "wMsgJ_DEF_JMR_SBY",             displayname = Labels.aicommands["wMsgJ_DEF_JMR_SBY"], enabled = true            } },

            { "wMsgJ_DEF_CMS_CTL_ORD" ,         new CommandInfo { uniqueid = 23618, name = "wMsgJ_DEF_CMS_CTL_ORD",         displayname = Labels.aicommands["wMsgJ_DEF_CMS_CTL_ORD"], enabled = true        } }, // not endpoint, hint
            { "wMsgJ_DEF_CMS_CHF_PGM" ,         new CommandInfo { uniqueid = 23626, name = "wMsgJ_DEF_CMS_CHF_PGM",         displayname = Labels.aicommands["wMsgJ_DEF_CMS_CHF_PGM"], enabled = true          } },
            { "wMsgJ_DEF_CMS_CHF_SNGL" ,        new CommandInfo { uniqueid = 23627, name = "wMsgJ_DEF_CMS_CHF_SNGL",        displayname = Labels.aicommands["wMsgJ_DEF_CMS_CHF_SNGL"], enabled = true          } },
            { "wMsgJ_DEF_CMS_CHF_TGHT" ,        new CommandInfo { uniqueid = 23628, name = "wMsgJ_DEF_CMS_CHF_TGHT",        displayname = Labels.aicommands["wMsgJ_DEF_CMS_CHF_TGHT"], enabled = true          } },
            { "wMsgJ_DEF_CMS_FLR_PGM" ,         new CommandInfo { uniqueid = 23629, name = "wMsgJ_DEF_CMS_FLR_PGM",         displayname = Labels.aicommands["wMsgJ_DEF_CMS_FLR_PGM"], enabled = true          } },
            { "wMsgJ_DEF_CMS_FLR_SNGL" ,        new CommandInfo { uniqueid = 23630, name = "wMsgJ_DEF_CMS_FLR_SNGL",        displayname = Labels.aicommands["wMsgJ_DEF_CMS_FLR_SNGL"], enabled = true          } },
            { "wMsgJ_DEF_CMS_FLR_TGHT" ,        new CommandInfo { uniqueid = 23631, name = "wMsgJ_DEF_CMS_FLR_TGHT",        displayname = Labels.aicommands["wMsgJ_DEF_CMS_FLR_TGHT"], enabled = true          } },

            { "wMsgJ_DEF_FLR_PGM" ,             new CommandInfo { uniqueid = 23617, name = "wMsgJ_DEF_FLR_PGM",             displayname = Labels.aicommands["wMsgJ_DEF_FLR_PGM"], enabled = true            } }, // not endpoint
            { "wMsgJ_DEF_FLR_PGM_1" ,           new CommandInfo { uniqueid = 23632, name = "wMsgJ_DEF_FLR_PGM_1",           displayname = Labels.aicommands["wMsgJ_DEF_FLR_PGM_1"], enabled = true          } },
            { "wMsgJ_DEF_FLR_PGM_2" ,           new CommandInfo { uniqueid = 23633, name = "wMsgJ_DEF_FLR_PGM_2",           displayname = Labels.aicommands["wMsgJ_DEF_FLR_PGM_2"], enabled = true          } },
            { "wMsgJ_DEF_FLR_PGM_3" ,           new CommandInfo { uniqueid = 23634, name = "wMsgJ_DEF_FLR_PGM_3",           displayname = Labels.aicommands["wMsgJ_DEF_FLR_PGM_3"], enabled = true          } },
            { "wMsgJ_DEF_FLR_PGM_4" ,           new CommandInfo { uniqueid = 23635, name = "wMsgJ_DEF_FLR_PGM_4",           displayname = Labels.aicommands["wMsgJ_DEF_FLR_PGM_4"], enabled = true          } },
            { "wMsgJ_DEF_FLR_PGM_5" ,           new CommandInfo { uniqueid = 23636, name = "wMsgJ_DEF_FLR_PGM_5",           displayname = Labels.aicommands["wMsgJ_DEF_FLR_PGM_5"], enabled = true          } },
            { "wMsgJ_DEF_FLR_PGM_6" ,           new CommandInfo { uniqueid = 23637, name = "wMsgJ_DEF_FLR_PGM_6",           displayname = Labels.aicommands["wMsgJ_DEF_FLR_PGM_6"], enabled = true          } },
            { "wMsgJ_DEF_FLR_PGM_7" ,           new CommandInfo { uniqueid = 23638, name = "wMsgJ_DEF_FLR_PGM_7",           displayname = Labels.aicommands["wMsgJ_DEF_FLR_PGM_7"], enabled = true          } },
            { "wMsgJ_DEF_FLR_PGM_8" ,           new CommandInfo { uniqueid = 23639, name = "wMsgJ_DEF_FLR_PGM_8",           displayname = Labels.aicommands["wMsgJ_DEF_FLR_PGM_8"], enabled = true          } },


            { "wMsgLANTIRN_Srch_Any" ,          new CommandInfo { uniqueid = 23641, name = "wMsgLANTIRN_Srch_Any",              displayname = Labels.aicommands["wMsgLANTIRN_Srch_Any"]   , enabled = true         } },
            { "wMsgLANTIRN_Srch_Any_Active" ,   new CommandInfo { uniqueid = 23642, name = "wMsgLANTIRN_Srch_Any_Active",       displayname = Labels.aicommands["wMsgLANTIRN_Srch_Any_Active"], enabled = true            } },
            { "wMsgLANTIRN_Srch_Air" ,          new CommandInfo { uniqueid = 23643, name = "wMsgLANTIRN_Srch_Air",              displayname = Labels.aicommands["wMsgLANTIRN_Srch_Air"]     , enabled = true       } },
            { "wMsgLANTIRN_Srch_Air_Active" ,   new CommandInfo { uniqueid = 23644, name = "wMsgLANTIRN_Srch_Air_Active",       displayname = Labels.aicommands["wMsgLANTIRN_Srch_Air_Active"]  , enabled = true          } },
            { "wMsgLANTIRN_Srch_SAM_Active" ,   new CommandInfo { uniqueid = 23645, name = "wMsgLANTIRN_Srch_SAM_Active",       displayname = Labels.aicommands["wMsgLANTIRN_Srch_SAM_Active"]  , enabled = true          } },
            { "wMsgLANTIRN_Srch_Armor_Active" , new CommandInfo { uniqueid = 23646, name = "wMsgLANTIRN_Srch_Armor_Active",     displayname = Labels.aicommands["wMsgLANTIRN_Srch_Armor_Active"] , enabled = true           } },
            { "wMsgLANTIRN_Srch_Vehicle" ,      new CommandInfo { uniqueid = 23647, name = "wMsgLANTIRN_Srch_Vehicle",          displayname = Labels.aicommands["wMsgLANTIRN_Srch_Vehicle"]    , enabled = true        } },
            { "wMsgLANTIRN_Ships_Active" ,      new CommandInfo { uniqueid = 23648, name = "wMsgLANTIRN_Ships_Active",          displayname = Labels.aicommands["wMsgLANTIRN_Ships_Active"]   , enabled = true         } },
            //{ "wMsgPlaceHolder449" ,            new CommandInfo { uniqueid = 23649, name = "wMsgPlaceHolder449",            displayname = Labels.aicommands["wMsgPlaceHolder449"]          } },
            //{ "wMsgPlaceHolder450" ,            new CommandInfo { uniqueid = 23650, name = "wMsgPlaceHolder450",            displayname = Labels.aicommands["wMsgPlaceHolder450"]          } },
            //{ "wMsgPlaceHolder451" ,            new CommandInfo { uniqueid = 23651, name = "wMsgPlaceHolder451",            displayname = Labels.aicommands["wMsgPlaceHolder451"]          } },
            //{ "wMsgPlaceHolder452" ,            new CommandInfo { uniqueid = 23652, name = "wMsgPlaceHolder452",            displayname = Labels.aicommands["wMsgPlaceHolder452"]          } },
            //{ "wMsgPlaceHolder453" ,            new CommandInfo { uniqueid = 23653, name = "wMsgPlaceHolder453",            displayname = Labels.aicommands["wMsgPlaceHolder453"]          } },
            //{ "wMsgPlaceHolder454" ,            new CommandInfo { uniqueid = 23654, name = "wMsgPlaceHolder454",            displayname = Labels.aicommands["wMsgPlaceHolder454"]          } },
            //{ "wMsgPlaceHolder455" ,            new CommandInfo { uniqueid = 23655, name = "wMsgPlaceHolder455",            displayname = Labels.aicommands["wMsgPlaceHolder455"]          } },
            //{ "wMsgPlaceHolder456" ,            new CommandInfo { uniqueid = 23656, name = "wMsgPlaceHolder456",            displayname = Labels.aicommands["wMsgPlaceHolder456"]          } },
            //{ "wMsgPlaceHolder457" ,            new CommandInfo { uniqueid = 23657, name = "wMsgPlaceHolder457",            displayname = Labels.aicommands["wMsgPlaceHolder457"]          } },
            //{ "wMsgPlaceHolder458" ,            new CommandInfo { uniqueid = 23658, name = "wMsgPlaceHolder458",            displayname = Labels.aicommands["wMsgPlaceHolder458"]          } },
            //{ "wMsgPlaceHolder459" ,            new CommandInfo { uniqueid = 23659, name = "wMsgPlaceHolder459",            displayname = Labels.aicommands["wMsgPlaceHolder459"]          } },
            //{ "wMsgPlaceHolder460" ,            new CommandInfo { uniqueid = 23660, name = "wMsgPlaceHolder460",            displayname = Labels.aicommands["wMsgPlaceHolder460"]          } },
            //{ "wMsgPlaceHolder461" ,            new CommandInfo { uniqueid = 23661, name = "wMsgPlaceHolder461",            displayname = Labels.aicommands["wMsgPlaceHolder461"]          } },
            //{ "wMsgPlaceHolder462" ,            new CommandInfo { uniqueid = 23662, name = "wMsgPlaceHolder462",            displayname = Labels.aicommands["wMsgPlaceHolder462"]          } },
            //{ "wMsgPlaceHolder463" ,            new CommandInfo { uniqueid = 23663, name = "wMsgPlaceHolder463",            displayname = Labels.aicommands["wMsgPlaceHolder463"]          } },
            //{ "wMsgPlaceHolder464" ,            new CommandInfo { uniqueid = 23664, name = "wMsgPlaceHolder464",            displayname = Labels.aicommands["wMsgPlaceHolder464"]          } },
            //{ "wMsgPlaceHolder465" ,            new CommandInfo { uniqueid = 23665, name = "wMsgPlaceHolder465",            displayname = Labels.aicommands["wMsgPlaceHolder465"]          } },
            //{ "wMsgPlaceHolder466" ,            new CommandInfo { uniqueid = 23666, name = "wMsgPlaceHolder466",            displayname = Labels.aicommands["wMsgPlaceHolder466"]          } },
            //{ "wMsgPlaceHolder467" ,            new CommandInfo { uniqueid = 23667, name = "wMsgPlaceHolder467",            displayname = Labels.aicommands["wMsgPlaceHolder467"]          } },
            //{ "wMsgPlaceHolder468" ,            new CommandInfo { uniqueid = 23668, name = "wMsgPlaceHolder468",            displayname = Labels.aicommands["wMsgPlaceHolder468"]          } },
            //{ "wMsgPlaceHolder469" ,            new CommandInfo { uniqueid = 23669, name = "wMsgPlaceHolder469",            displayname = Labels.aicommands["wMsgPlaceHolder469"]          } },
            //{ "wMsgPlaceHolder470" ,            new CommandInfo { uniqueid = 23670, name = "wMsgPlaceHolder470",            displayname = Labels.aicommands["wMsgPlaceHolder470"]          } },
            //{ "wMsgPlaceHolder471" ,            new CommandInfo { uniqueid = 23671, name = "wMsgPlaceHolder471",            displayname = Labels.aicommands["wMsgPlaceHolder471"]          } },
            //{ "wMsgPlaceHolder472" ,            new CommandInfo { uniqueid = 23672, name = "wMsgPlaceHolder472",            displayname = Labels.aicommands["wMsgPlaceHolder472"]          } },
            //{ "wMsgPlaceHolder473" ,            new CommandInfo { uniqueid = 23673, name = "wMsgPlaceHolder473",            displayname = Labels.aicommands["wMsgPlaceHolder473"]          } },
            //{ "wMsgPlaceHolder474" ,            new CommandInfo { uniqueid = 23674, name = "wMsgPlaceHolder474",            displayname = Labels.aicommands["wMsgPlaceHolder474"]          } },
            //{ "wMsgPlaceHolder475" ,            new CommandInfo { uniqueid = 23675, name = "wMsgPlaceHolder475",            displayname = Labels.aicommands["wMsgPlaceHolder475"]          } },
            //{ "wMsgPlaceHolder476" ,            new CommandInfo { uniqueid = 23676, name = "wMsgPlaceHolder476",            displayname = Labels.aicommands["wMsgPlaceHolder476"]          } },
            //{ "wMsgPlaceHolder477" ,            new CommandInfo { uniqueid = 23677, name = "wMsgPlaceHolder477",            displayname = Labels.aicommands["wMsgPlaceHolder477"]          } },
            //{ "wMsgPlaceHolder478" ,            new CommandInfo { uniqueid = 23678, name = "wMsgPlaceHolder478",            displayname = Labels.aicommands["wMsgPlaceHolder478"]          } },
            //{ "wMsgPlaceHolder479" ,            new CommandInfo { uniqueid = 23679, name = "wMsgPlaceHolder479",            displayname = Labels.aicommands["wMsgPlaceHolder479"]          } },
            //{ "wMsgPlaceHolder480" ,            new CommandInfo { uniqueid = 23680, name = "wMsgPlaceHolder480",            displayname = Labels.aicommands["wMsgPlaceHolder480"]          } },
            //{ "wMsgPlaceHolder481" ,            new CommandInfo { uniqueid = 23681, name = "wMsgPlaceHolder481",            displayname = Labels.aicommands["wMsgPlaceHolder481"]          } },
            //{ "wMsgPlaceHolder482" ,            new CommandInfo { uniqueid = 23682, name = "wMsgPlaceHolder482",            displayname = Labels.aicommands["wMsgPlaceHolder482"]          } },
            //{ "wMsgPlaceHolder483" ,            new CommandInfo { uniqueid = 23683, name = "wMsgPlaceHolder483",            displayname = Labels.aicommands["wMsgPlaceHolder483"]          } },
            //{ "wMsgPlaceHolder484" ,            new CommandInfo { uniqueid = 23684, name = "wMsgPlaceHolder484",            displayname = Labels.aicommands["wMsgPlaceHolder484"]          } },
            //{ "wMsgPlaceHolder485" ,            new CommandInfo { uniqueid = 23685, name = "wMsgPlaceHolder485",            displayname = Labels.aicommands["wMsgPlaceHolder485"]          } },
            //{ "wMsgPlaceHolder486" ,            new CommandInfo { uniqueid = 23686, name = "wMsgPlaceHolder486",            displayname = Labels.aicommands["wMsgPlaceHolder486"]          } },
            //{ "wMsgPlaceHolder487" ,            new CommandInfo { uniqueid = 23687, name = "wMsgPlaceHolder487",            displayname = Labels.aicommands["wMsgPlaceHolder487"]          } },
            //{ "wMsgPlaceHolder488" ,            new CommandInfo { uniqueid = 23688, name = "wMsgPlaceHolder488",            displayname = Labels.aicommands["wMsgPlaceHolder488"]          } },
            //{ "wMsgPlaceHolder489" ,            new CommandInfo { uniqueid = 23689, name = "wMsgPlaceHolder489",            displayname = Labels.aicommands["wMsgPlaceHolder489"]          } },
            // end of defensive

            // spare table
            //{ "wMsgPlaceHolder490" ,            new CommandInfo { uniqueid = 23690, name = "wMsgPlaceHolder490",            displayname = Labels.aicommands["wMsgPlaceHolder490"]          } },
            //{ "wMsgPlaceHolder491" ,            new CommandInfo { uniqueid = 23691, name = "wMsgPlaceHolder491",            displayname = Labels.aicommands["wMsgPlaceHolder491"]          } },
            //{ "wMsgPlaceHolder492" ,            new CommandInfo { uniqueid = 23692, name = "wMsgPlaceHolder492",            displayname = Labels.aicommands["wMsgPlaceHolder492"]          } },
            //{ "wMsgPlaceHolder493" ,            new CommandInfo { uniqueid = 23693, name = "wMsgPlaceHolder493",            displayname = Labels.aicommands["wMsgPlaceHolder493"]          } },
            //{ "wMsgPlaceHolder494" ,            new CommandInfo { uniqueid = 23694, name = "wMsgPlaceHolder494",            displayname = Labels.aicommands["wMsgPlaceHolder494"]          } },
            //{ "wMsgPlaceHolder495" ,            new CommandInfo { uniqueid = 23695, name = "wMsgPlaceHolder495",            displayname = Labels.aicommands["wMsgPlaceHolder495"]          } },
            //{ "wMsgPlaceHolder496" ,            new CommandInfo { uniqueid = 23696, name = "wMsgPlaceHolder496",            displayname = Labels.aicommands["wMsgPlaceHolder496"]          } },
            //{ "wMsgPlaceHolder497" ,            new CommandInfo { uniqueid = 23697, name = "wMsgPlaceHolder497",            displayname = Labels.aicommands["wMsgPlaceHolder497"]          } },
            //{ "wMsgPlaceHolder498" ,            new CommandInfo { uniqueid = 23698, name = "wMsgPlaceHolder498",            displayname = Labels.aicommands["wMsgPlaceHolder498"]          } },
            //{ "wMsgPlaceHolder499" ,            new CommandInfo { uniqueid = 23699, name = "wMsgPlaceHolder499",            displayname = Labels.aicommands["wMsgPlaceHolder499"]          } }, 

            // block 500-600 RIO misc

             // startup
            { "wMsgJ_STRT_ABORT" ,              new CommandInfo { uniqueid = 23700, name = "wMsgJ_STRT_ABORT",              displayname = Labels.aicommands["wMsgJ_STRT_ABORT"], enabled = true         } },
            { "wMsgJ_STRT_INS_FINE" ,           new CommandInfo { uniqueid = 23701, name = "wMsgJ_STRT_INS_FINE",           displayname = Labels.aicommands["wMsgJ_STRT_INS_FINE"], enabled = true      } },
            { "wMsgJ_STRT_INS_MIN_WPN" ,        new CommandInfo { uniqueid = 23702, name = "wMsgJ_STRT_INS_MIN_WPN",        displayname = Labels.aicommands["wMsgJ_STRT_INS_MIN_WPN"], enabled = true   } },
            { "wMsgJ_STRT_INS_COARSE" ,         new CommandInfo { uniqueid = 23703, name = "wMsgJ_STRT_INS_COARSE",         displayname = Labels.aicommands["wMsgJ_STRT_INS_COARSE"], enabled = true    } },
            { "wMsgJ_STRT_INS_NOW" ,            new CommandInfo { uniqueid = 23704, name = "wMsgJ_STRT_INS_NOW",            displayname = Labels.aicommands["wMsgJ_STRT_INS_NOW"], enabled = true       } },

            { "wMsgJ_STRT_CHECK" ,              new CommandInfo { uniqueid = 23705, name = "wMsgJ_STRT_CHECK",              displayname = Labels.aicommands["wMsgJ_STRT_CHECK"], enabled = true         } },
            { "wMsgJ_STRT_LOUD_CLR" ,           new CommandInfo { uniqueid = 23706, name = "wMsgJ_STRT_LOUD_CLR",           displayname = Labels.aicommands["wMsgJ_STRT_LOUD_CLR"], enabled = true      } },
            { "wMsgJ_STRT_PAUSE" ,              new CommandInfo { uniqueid = 23707, name = "wMsgJ_STRT_PAUSE",              displayname = Labels.aicommands["wMsgJ_STRT_PAUSE"]        } },
            { "wMsgJ_STRT_STARTUP" ,            new CommandInfo { uniqueid = 23708, name = "wMsgJ_STRT_STARTUP",            displayname = Labels.aicommands["wMsgJ_STRT_STARTUP"], enabled = true       } },
            { "wMsgJ_STRT_ASSISTED" ,           new CommandInfo { uniqueid = 23709, name = "wMsgJ_STRT_ASSISTED",           displayname = Labels.aicommands["wMsgJ_STRT_ASSISTED"], enabled = true      } }, 

            // 600-700 AI pilot 23800-23999
            { "wMsgI_ALT" ,                     new CommandInfo { uniqueid = 23800, name = "wMsgI_ALT",                     displayname = Labels.aicommands["wMsgI_ALT"], enabled = true          } },
            { "wMsgI_ALT_ANG_1" ,               new CommandInfo { uniqueid = 23801, name = "wMsgI_ALT_ANG_1",               displayname = Labels.aicommands["wMsgI_ALT_ANG_1"], enabled = true          } },
            { "wMsgI_ALT_ANG_5" ,               new CommandInfo { uniqueid = 23802, name = "wMsgI_ALT_ANG_5",               displayname = Labels.aicommands["wMsgI_ALT_ANG_5"], enabled = true          } },
            { "wMsgI_ALT_ANG_10" ,              new CommandInfo { uniqueid = 23803, name = "wMsgI_ALT_ANG_10",              displayname = Labels.aicommands["wMsgI_ALT_ANG_10"], enabled = true          } },
            { "wMsgI_ALT_ANG_15" ,              new CommandInfo { uniqueid = 23804, name = "wMsgI_ALT_ANG_15",              displayname = Labels.aicommands["wMsgI_ALT_ANG_15"], enabled = true          } },
            { "wMsgI_ALT_ANG_20" ,              new CommandInfo { uniqueid = 23805, name = "wMsgI_ALT_ANG_20",              displayname = Labels.aicommands["wMsgI_ALT_ANG_20"], enabled = true          } },
            { "wMsgI_ALT_ANG_25" ,              new CommandInfo { uniqueid = 23806, name = "wMsgI_ALT_ANG_25",              displayname = Labels.aicommands["wMsgI_ALT_ANG_25"], enabled = true          } },
            { "wMsgI_ALT_ANG_30" ,              new CommandInfo { uniqueid = 23807, name = "wMsgI_ALT_ANG_30",              displayname = Labels.aicommands["wMsgI_ALT_ANG_30"], enabled = true          } },
            { "wMsgI_ALT_ANG_35" ,              new CommandInfo { uniqueid = 23808, name = "wMsgI_ALT_ANG_35",              displayname = Labels.aicommands["wMsgI_ALT_ANG_35"], enabled = true          } },
            { "wMsgI_ALT_CHG" ,                 new CommandInfo { uniqueid = 23809, name = "wMsgI_ALT_CHG",                 displayname = Labels.aicommands["wMsgI_ALT_CHG"], enabled = true          } },
            { "wMsgI_ALT_DESC_10K" ,            new CommandInfo { uniqueid = 23810, name = "wMsgI_ALT_DESC_10K",            displayname = Labels.aicommands["wMsgI_ALT_DESC_10K"], enabled = true          } },
            { "wMsgI_ALT_DESC_5K" ,             new CommandInfo { uniqueid = 23811, name = "wMsgI_ALT_DESC_5K",             displayname = Labels.aicommands["wMsgI_ALT_DESC_5K"], enabled = true          } },
            { "wMsgI_ALT_DESC_1K" ,             new CommandInfo { uniqueid = 23812, name = "wMsgI_ALT_DESC_1K",             displayname = Labels.aicommands["wMsgI_ALT_DESC_1K"], enabled = true          } },
            { "wMsgI_ALT_DESC_500" ,            new CommandInfo { uniqueid = 23813, name = "wMsgI_ALT_DESC_500",            displayname = Labels.aicommands["wMsgI_ALT_DESC_500"], enabled = true          } },
            { "wMsgI_ALT_CLMB_500" ,            new CommandInfo { uniqueid = 23814, name = "wMsgI_ALT_CLMB_500",            displayname = Labels.aicommands["wMsgI_ALT_CLMB_500"], enabled = true          } },
            { "wMsgI_ALT_CLMB_1K" ,             new CommandInfo { uniqueid = 23815, name = "wMsgI_ALT_CLMB_1K",             displayname = Labels.aicommands["wMsgI_ALT_CLMB_1K"], enabled = true          } },
            { "wMsgI_ALT_CLMB_5K" ,             new CommandInfo { uniqueid = 23816, name = "wMsgI_ALT_CLMB_5K",             displayname = Labels.aicommands["wMsgI_ALT_CLMB_5K"], enabled = true          } },
            { "wMsgI_ALT_CLMB_10K" ,            new CommandInfo { uniqueid = 23817, name = "wMsgI_ALT_CLMB_10K",            displayname = Labels.aicommands["wMsgI_ALT_CLMB_10K"], enabled = true          } },
            { "wMsgI_SPD_MINUS_200" ,           new CommandInfo { uniqueid = 23818, name = "wMsgI_SPD_MINUS_200",           displayname = Labels.aicommands["wMsgI_SPD_MINUS_200"], enabled = true          } },
            { "wMsgI_SPD_MINUS_100" ,           new CommandInfo { uniqueid = 23819, name = "wMsgI_SPD_MINUS_100",           displayname = Labels.aicommands["wMsgI_SPD_MINUS_100"], enabled = true          } },
            { "wMsgI_SPD_MINUS_50" ,            new CommandInfo { uniqueid = 23820, name = "wMsgI_SPD_MINUS_50",            displayname = Labels.aicommands["wMsgI_SPD_MINUS_50"], enabled = true          } },
            { "wMsgI_SPD_PLUS_50" ,             new CommandInfo { uniqueid = 23821, name = "wMsgI_SPD_PLUS_50",             displayname = Labels.aicommands["wMsgI_SPD_PLUS_50"], enabled = true          } },
            { "wMsgI_SPD_PLUS_100" ,            new CommandInfo { uniqueid = 23822, name = "wMsgI_SPD_PLUS_100",            displayname = Labels.aicommands["wMsgI_SPD_PLUS_100"], enabled = true          } },
            { "wMsgI_SPD_PLUS_200" ,            new CommandInfo { uniqueid = 23823, name = "wMsgI_SPD_PLUS_200",            displayname = Labels.aicommands["wMsgI_SPD_PLUS_200"], enabled = true          } },
            { "wMsgI_DIR_N" ,                   new CommandInfo { uniqueid = 23824, name = "wMsgI_DIR_N",                   displayname = Labels.aicommands["wMsgI_DIR_N"], enabled = true          } },
            { "wMsgI_DIR_NE" ,                  new CommandInfo { uniqueid = 23825, name = "wMsgI_DIR_NE",                  displayname = Labels.aicommands["wMsgI_DIR_NE"], enabled = true          } },
            { "wMsgI_DIR_E" ,                   new CommandInfo { uniqueid = 23826, name = "wMsgI_DIR_E",                   displayname = Labels.aicommands["wMsgI_DIR_E"], enabled = true          } },
            { "wMsgI_DIR_SE" ,                  new CommandInfo { uniqueid = 23827, name = "wMsgI_DIR_SE",                  displayname = Labels.aicommands["wMsgI_DIR_SE"], enabled = true          } },
            { "wMsgI_DIR_S" ,                   new CommandInfo { uniqueid = 23828, name = "wMsgI_DIR_S",                   displayname = Labels.aicommands["wMsgI_DIR_S"], enabled = true          } },
            { "wMsgI_DIR_SW" ,                  new CommandInfo { uniqueid = 23829, name = "wMsgI_DIR_SW",                  displayname = Labels.aicommands["wMsgI_DIR_SW"], enabled = true          } },
            { "wMsgI_DIR_W" ,                   new CommandInfo { uniqueid = 23830, name = "wMsgI_DIR_W",                   displayname = Labels.aicommands["wMsgI_DIR_W"], enabled = true          } },
            { "wMsgI_DIR_NW" ,                  new CommandInfo { uniqueid = 23831, name = "wMsgI_DIR_NW",                  displayname = Labels.aicommands["wMsgI_DIR_NW"], enabled = true          } },
            { "wMsgI_DIR" ,                     new CommandInfo { uniqueid = 23832, name = "wMsgI_DIR",                     displayname = Labels.aicommands["wMsgI_DIR"], enabled = true          } },
            { "wMsgI_DIR_CHG_L45" ,             new CommandInfo { uniqueid = 23833, name = "wMsgI_DIR_CHG_L45",             displayname = Labels.aicommands["wMsgI_DIR_CHG_L45"], enabled = true          } },
            { "wMsgI_DIR_CHG_L30" ,             new CommandInfo { uniqueid = 23834, name = "wMsgI_DIR_CHG_L30",             displayname = Labels.aicommands["wMsgI_DIR_CHG_L30"], enabled = true          } },
            { "wMsgI_DIR_CHG_L10" ,             new CommandInfo { uniqueid = 23835, name = "wMsgI_DIR_CHG_L10",             displayname = Labels.aicommands["wMsgI_DIR_CHG_L10"], enabled = true          } },
            { "wMsgI_DIR_CHG_L5" ,              new CommandInfo { uniqueid = 23836, name = "wMsgI_DIR_CHG_L5",              displayname = Labels.aicommands["wMsgI_DIR_CHG_L5"], enabled = true          } },
            { "wMsgI_DIR_CHG_R5" ,              new CommandInfo { uniqueid = 23837, name = "wMsgI_DIR_CHG_R5",              displayname = Labels.aicommands["wMsgI_DIR_CHG_R5"], enabled = true          } },
            { "wMsgI_DIR_CHG_R10" ,             new CommandInfo { uniqueid = 23838, name = "wMsgI_DIR_CHG_R10",             displayname = Labels.aicommands["wMsgI_DIR_CHG_R10"], enabled = true          } },
            { "wMsgI_DIR_CHG_R30" ,             new CommandInfo { uniqueid = 23839, name = "wMsgI_DIR_CHG_R30",             displayname = Labels.aicommands["wMsgI_DIR_CHG_R30"], enabled = true          } },
            { "wMsgI_DIR_CHG_R45" ,             new CommandInfo { uniqueid = 23840, name = "wMsgI_DIR_CHG_R45",             displayname = Labels.aicommands["wMsgI_DIR_CHG_R45"], enabled = true          } },
            { "wMsgI_DIR_HOLD_CRS" ,            new CommandInfo { uniqueid = 23841, name = "wMsgI_DIR_HOLD_CRS",            displayname = Labels.aicommands["wMsgI_DIR_HOLD_CRS"], enabled = true       } },
            { "wMsgI_SPD" ,                     new CommandInfo { uniqueid = 23842, name = "wMsgI_SPD",                     displayname = Labels.aicommands["wMsgI_SPD"], enabled = true             } },
            { "wMsgI_DIR_CHG" ,                 new CommandInfo { uniqueid = 23843, name = "wMsgI_DIR_CHG",                 displayname = Labels.aicommands["wMsgI_DIR_CHG"], enabled = true            } },

            { "wMsgI_SPT_FLYTO" ,               new CommandInfo { uniqueid = 23844, name = "wMsgI_SPT_FLYTO",               displayname = Labels.aicommands["wMsgI_SPT_FLYTO"], enabled = true            } },
            { "wMsgI_SPT_ORBIT" ,               new CommandInfo { uniqueid = 23845, name = "wMsgI_SPT_ORBIT",               displayname = Labels.aicommands["wMsgI_SPT_ORBIT"], enabled = true            } },
            //{ "wMsgPlaceHolder646" ,            new CommandInfo { uniqueid = 23846, name = "wMsgPlaceHolder646",            displayname = Labels.aicommands["wMsgPlaceHolder646"]          } },
            //{ "wMsgPlaceHolder647" ,            new CommandInfo { uniqueid = 23847, name = "wMsgPlaceHolder647",            displayname = Labels.aicommands["wMsgPlaceHolder647"]          } },
            //{ "wMsgPlaceHolder648" ,            new CommandInfo { uniqueid = 23848, name = "wMsgPlaceHolder648",            displayname = Labels.aicommands["wMsgPlaceHolder648"]          } },
            //{ "wMsgPlaceHolder649" ,            new CommandInfo { uniqueid = 23849, name = "wMsgPlaceHolder649",            displayname = Labels.aicommands["wMsgPlaceHolder649"]          } },
            //{ "wMsgPlaceHolder650" ,            new CommandInfo { uniqueid = 23850, name = "wMsgPlaceHolder650",            displayname = Labels.aicommands["wMsgPlaceHolder650"]          } },
            //{ "wMsgPlaceHolder651" ,            new CommandInfo { uniqueid = 23851, name = "wMsgPlaceHolder651",            displayname = Labels.aicommands["wMsgPlaceHolder651"]          } },
            //{ "wMsgPlaceHolder652" ,            new CommandInfo { uniqueid = 23852, name = "wMsgPlaceHolder652",            displayname = Labels.aicommands["wMsgPlaceHolder652"]          } },
            //{ "wMsgPlaceHolder653" ,            new CommandInfo { uniqueid = 23853, name = "wMsgPlaceHolder653",            displayname = Labels.aicommands["wMsgPlaceHolder653"]          } },
            //{ "wMsgPlaceHolder654" ,            new CommandInfo { uniqueid = 23854, name = "wMsgPlaceHolder654",            displayname = Labels.aicommands["wMsgPlaceHolder654"]          } },
            //{ "wMsgPlaceHolder655" ,            new CommandInfo { uniqueid = 23855, name = "wMsgPlaceHolder655",            displayname = Labels.aicommands["wMsgPlaceHolder655"]          } },
            //{ "wMsgPlaceHolder656" ,            new CommandInfo { uniqueid = 23856, name = "wMsgPlaceHolder656",            displayname = Labels.aicommands["wMsgPlaceHolder656"]          } },
            //{ "wMsgPlaceHolder657" ,            new CommandInfo { uniqueid = 23857, name = "wMsgPlaceHolder657",            displayname = Labels.aicommands["wMsgPlaceHolder657"]          } },
            //{ "wMsgPlaceHolder658" ,            new CommandInfo { uniqueid = 23858, name = "wMsgPlaceHolder658",            displayname = Labels.aicommands["wMsgPlaceHolder658"]          } },
            //{ "wMsgPlaceHolder659" ,            new CommandInfo { uniqueid = 23859, name = "wMsgPlaceHolder659",            displayname = Labels.aicommands["wMsgPlaceHolder659"]          } },
            //{ "wMsgPlaceHolder660" ,            new CommandInfo { uniqueid = 23860, name = "wMsgPlaceHolder660",            displayname = Labels.aicommands["wMsgPlaceHolder660"]          } },
            //{ "wMsgPlaceHolder661" ,            new CommandInfo { uniqueid = 23861, name = "wMsgPlaceHolder661",            displayname = Labels.aicommands["wMsgPlaceHolder661"]          } },
            //{ "wMsgPlaceHolder662" ,            new CommandInfo { uniqueid = 23862, name = "wMsgPlaceHolder662",            displayname = Labels.aicommands["wMsgPlaceHolder662"]          } },
            //{ "wMsgPlaceHolder663" ,            new CommandInfo { uniqueid = 23863, name = "wMsgPlaceHolder663",            displayname = Labels.aicommands["wMsgPlaceHolder663"]          } },
            //{ "wMsgPlaceHolder664" ,            new CommandInfo { uniqueid = 23864, name = "wMsgPlaceHolder664",            displayname = Labels.aicommands["wMsgPlaceHolder664"]          } },
            //{ "wMsgPlaceHolder665" ,            new CommandInfo { uniqueid = 23865, name = "wMsgPlaceHolder665",            displayname = Labels.aicommands["wMsgPlaceHolder665"]          } },
            //{ "wMsgPlaceHolder666" ,            new CommandInfo { uniqueid = 23866, name = "wMsgPlaceHolder666",            displayname = Labels.aicommands["wMsgPlaceHolder666"]          } },
            //{ "wMsgPlaceHolder667" ,            new CommandInfo { uniqueid = 23867, name = "wMsgPlaceHolder667",            displayname = Labels.aicommands["wMsgPlaceHolder667"]          } },
            //{ "wMsgPlaceHolder668" ,            new CommandInfo { uniqueid = 23868, name = "wMsgPlaceHolder668",            displayname = Labels.aicommands["wMsgPlaceHolder668"]          } },
            //{ "wMsgPlaceHolder669" ,            new CommandInfo { uniqueid = 23869, name = "wMsgPlaceHolder669",            displayname = Labels.aicommands["wMsgPlaceHolder669"]          } },
            //{ "wMsgPlaceHolder670" ,            new CommandInfo { uniqueid = 23870, name = "wMsgPlaceHolder670",            displayname = Labels.aicommands["wMsgPlaceHolder670"]          } },
            //{ "wMsgPlaceHolder671" ,            new CommandInfo { uniqueid = 23871, name = "wMsgPlaceHolder671",            displayname = Labels.aicommands["wMsgPlaceHolder671"]          } },
            //{ "wMsgPlaceHolder672" ,            new CommandInfo { uniqueid = 23872, name = "wMsgPlaceHolder672",            displayname = Labels.aicommands["wMsgPlaceHolder672"]          } },
            //{ "wMsgPlaceHolder673" ,            new CommandInfo { uniqueid = 23873, name = "wMsgPlaceHolder673",            displayname = Labels.aicommands["wMsgPlaceHolder673"]          } },
            //{ "wMsgPlaceHolder674" ,            new CommandInfo { uniqueid = 23874, name = "wMsgPlaceHolder674",            displayname = Labels.aicommands["wMsgPlaceHolder674"]          } },
            //{ "wMsgPlaceHolder675" ,            new CommandInfo { uniqueid = 23875, name = "wMsgPlaceHolder675",            displayname = Labels.aicommands["wMsgPlaceHolder675"]          } },
            //{ "wMsgPlaceHolder676" ,            new CommandInfo { uniqueid = 23876, name = "wMsgPlaceHolder676",            displayname = Labels.aicommands["wMsgPlaceHolder676"]          } },
            //{ "wMsgPlaceHolder677" ,            new CommandInfo { uniqueid = 23877, name = "wMsgPlaceHolder677",            displayname = Labels.aicommands["wMsgPlaceHolder677"]          } },
            //{ "wMsgPlaceHolder678" ,            new CommandInfo { uniqueid = 23878, name = "wMsgPlaceHolder678",            displayname = Labels.aicommands["wMsgPlaceHolder678"]          } },
            //{ "wMsgPlaceHolder679" ,            new CommandInfo { uniqueid = 23879, name = "wMsgPlaceHolder679",            displayname = Labels.aicommands["wMsgPlaceHolder679"]          } },
            //{ "wMsgPlaceHolder680" ,            new CommandInfo { uniqueid = 23880, name = "wMsgPlaceHolder680",            displayname = Labels.aicommands["wMsgPlaceHolder680"]          } },
            //{ "wMsgPlaceHolder681" ,            new CommandInfo { uniqueid = 23881, name = "wMsgPlaceHolder681",            displayname = Labels.aicommands["wMsgPlaceHolder681"]          } },
            //{ "wMsgPlaceHolder682" ,            new CommandInfo { uniqueid = 23882, name = "wMsgPlaceHolder682",            displayname = Labels.aicommands["wMsgPlaceHolder682"]          } },
            //{ "wMsgPlaceHolder683" ,            new CommandInfo { uniqueid = 23883, name = "wMsgPlaceHolder683",            displayname = Labels.aicommands["wMsgPlaceHolder683"]          } },
            //{ "wMsgPlaceHolder684" ,            new CommandInfo { uniqueid = 23884, name = "wMsgPlaceHolder684",            displayname = Labels.aicommands["wMsgPlaceHolder684"]          } },
            //{ "wMsgPlaceHolder685" ,            new CommandInfo { uniqueid = 23885, name = "wMsgPlaceHolder685",            displayname = Labels.aicommands["wMsgPlaceHolder685"]          } },
            //{ "wMsgPlaceHolder686" ,            new CommandInfo { uniqueid = 23886, name = "wMsgPlaceHolder686",            displayname = Labels.aicommands["wMsgPlaceHolder686"]          } },
            //{ "wMsgPlaceHolder687" ,            new CommandInfo { uniqueid = 23887, name = "wMsgPlaceHolder687",            displayname = Labels.aicommands["wMsgPlaceHolder687"]          } },
            //{ "wMsgPlaceHolder688" ,            new CommandInfo { uniqueid = 23888, name = "wMsgPlaceHolder688",            displayname = Labels.aicommands["wMsgPlaceHolder688"]          } },
            //{ "wMsgPlaceHolder689" ,            new CommandInfo { uniqueid = 23889, name = "wMsgPlaceHolder689",            displayname = Labels.aicommands["wMsgPlaceHolder689"]          } },
            // end of AI pilot

            // spare table
            //{ "wMsgPlaceHolder690" ,            new CommandInfo { uniqueid = 23890, name = "wMsgPlaceHolder690",            displayname = Labels.aicommands["wMsgPlaceHolder690"]          } },
            //{ "wMsgPlaceHolder691" ,            new CommandInfo { uniqueid = 23891, name = "wMsgPlaceHolder691",            displayname = Labels.aicommands["wMsgPlaceHolder691"]          } },
            //{ "wMsgPlaceHolder692" ,            new CommandInfo { uniqueid = 23892, name = "wMsgPlaceHolder692",            displayname = Labels.aicommands["wMsgPlaceHolder692"]          } },
            //{ "wMsgPlaceHolder693" ,            new CommandInfo { uniqueid = 23893, name = "wMsgPlaceHolder693",            displayname = Labels.aicommands["wMsgPlaceHolder693"]          } },
            //{ "wMsgPlaceHolder694" ,            new CommandInfo { uniqueid = 23894, name = "wMsgPlaceHolder694",            displayname = Labels.aicommands["wMsgPlaceHolder694"]          } },
            //{ "wMsgPlaceHolder695" ,            new CommandInfo { uniqueid = 23895, name = "wMsgPlaceHolder695",            displayname = Labels.aicommands["wMsgPlaceHolder695"]          } },
            //{ "wMsgPlaceHolder696" ,            new CommandInfo { uniqueid = 23896, name = "wMsgPlaceHolder696",            displayname = Labels.aicommands["wMsgPlaceHolder696"]          } },
            //{ "wMsgPlaceHolder697" ,            new CommandInfo { uniqueid = 23897, name = "wMsgPlaceHolder697",            displayname = Labels.aicommands["wMsgPlaceHolder697"]          } },
            //{ "wMsgPlaceHolder698" ,            new CommandInfo { uniqueid = 23898, name = "wMsgPlaceHolder698",            displayname = Labels.aicommands["wMsgPlaceHolder698"]          } },
            //{ "wMsgPlaceHolder699" ,            new CommandInfo { uniqueid = 23899, name = "wMsgPlaceHolder699",            displayname = Labels.aicommands["wMsgPlaceHolder699"]          } },

        };
    }
}
