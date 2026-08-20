using System;
using System.Collections.Generic;

namespace VAICOM.Extensions.RIO
{

    public static partial class Labels
    {
        // labels (recipients)
        public static Dictionary<string, string> airecipients = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "RIO",                            "F-14 AI RIO"                   },
            { "Iceman",                         "F-14 AI Pilot"                 },
        };


        // labels (commands)
        public static Dictionary<string, string> aicommands = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {

            // example
            { "wMsgJ_CANOPY_OPEN" ,             "RIO Open Canopy"               },
            { "wMsgJ_CANOPY_CLOSE" ,            "RIO Close Canopy"              },
            //...

            // command unique internal name   // friendly label

            // block: menu control
            {"wMsgJ_MENU_TOGGLE",               "Do Menu Toggle"                }, // block disabled: labels not used
            {"wMsgJ_MENU_OPTION_1",             "Do Menu Option 1"              },
            {"wMsgJ_MENU_OPTION_2",             "Do Menu Option 2"              },
            {"wMsgJ_MENU_OPTION_3",             "Do Menu Option 3"              },
            {"wMsgJ_MENU_OPTION_4",             "Do Menu Option 4"              },
            {"wMsgJ_MENU_OPTION_5",             "Do Menu Option 5"              },
            {"wMsgJ_MENU_OPTION_6",             "Do Menu Option 6"              },
            {"wMsgJ_MENU_OPTION_7",             "Do Menu Option 7"              },
            {"wMsgJ_MENU_OPTION_8",             "Do Menu Option 8"              },
            {"wMsgJ_MENU_DIR_D",                "Do Menu Down"                  },
            {"wMsgJ_MENU_DIR_DL",               "Do Menu Down Left"             },
            {"wMsgJ_MENU_DIR_DR",               "Do Menu Down Right"            },
            {"wMsgJ_MENU_DIR_L",                "Do Menu Left"                  },
            {"wMsgJ_MENU_DIR_R",                "Do Menu Right"                 },
            {"wMsgJ_MENU_DIR_U",                "Do Menu Up"                    },
            {"wMsgJ_MENU_DIR_UL",               "Do Menu Up Left"               },
            {"wMsgJ_MENU_DIR_UR",               "Do Menu Up Right"              },
            {"wMsgJ_MENU_OPEN",                 "Open Menu"                     },
            {"wMsgJ_MENU_CLOSE",                "Close Menu"                    },
            {"wMsgJ_MENU_CONTEXT",              "Context Menu"                  },
            {"wMsgJ_MENU_MAIN",                 "Main Menu"                     },
            {"wMsgJ_MENU_CTXT_CLOSE",           "Close Context Menu"            },
            {"wMsgJ_MENU_MAIN_CLOSE",           "Close Main Menu"               },

            {"wMsgJESTER_LANTIRN_inhibit_auto_designate",       "Inhibit Auto Designate"            },
            {"wMsgJESTER_LANTIRN_track_target_id",              "Track LANTIRN Target"              },
            {"wMsgJESTER_LANTIRN_track_zone_id",                "Track LANTIRN Zone"                },
            {"wMsgJESTER_LANTIRN_designate",                    "LANTIRN Designate"                 },
            //{"wMsgKNEEBOARD_Laser100",                          "Laser 100"                         },
            //{"wMsgKNEEBOARD_Laser10",                           "Laser 10"                          },
            //{"wMsgKNEEBOARD_Laser1",                            "Laser 1"                           },
            // end of menu control

            // spare block
            {"wMsgLANTIRN_ToggleWHOTBHOT",                      "Black White Hot Toggle"        },
            {"wMsgLANTIRN_LaserLatched",                        "Laser Latch"                   },
            {"wMsgLANTIRN_Laser_ARM",                           "Laser Arm"                     },
            {"wMsgLANTIRN_Laser_ARM_Toggle",                    "Laser Toggle"                  },
            {"wMsgLANTIRN_Undesignate",                         "LANTIRN Undesignate"           },
            {"wMsgLANTIRN_QHUD_QADL",                           "LANTIRN QHUD QADL"             },
            {"wMsgLANTIRN_QSNO",                                "LANTIRN QSNO"                  },
            {"wMsgLANTIRN_QDES",                                "LANTIRN QDES"                  },
            {"wMsgLANTIRN_QWP_Minus",                           "LANTIRN Previous Target"             },
            {"wMsgLANTIRN_QWP_Plus",                            "LANTIRN Next Target"           },         

            // block: radar

            {"wMsgJ_RDR_SPOT",                      "Radar Spot"                        }, //na
            {"wMsgJ_RDR_BREAK_LOCK",                "Break Lock"                        },
            {"wMsgJ_RDR_TO_PSTT",                   "Go To PSTT"                        }, //na

            {"wMsgJ_RDR_TOGGLE_STT",                "Toggle PDSTT/PSTT"                 },
            {"wMsgJ_RDR_VSL_HIGH",                  "VSL High"                          },
            {"wMsgJ_RDR_VSL_LOW",                   "VSL Low"                           },

            {"wMsgJ_RDR_STT_LOCK",                  "STT Lock"                      },
            {"wMsgJ_RDR_STT_TGT_AHEAD",             "STT Lock Contact Ahead"        },
            {"wMsgJ_RDR_STT_ENMY_TGT_AHEAD",        "STT Lock Bogey Ahead"          },
            {"wMsgJ_RDR_STT_FRNDLY_TGT_AHEAD",      "STT Lock Friendly Ahead"       },
            {"wMsgJ_RDR_STT_CHOOSE_SPECIFIC_TGT",   "Track Single Target.."         }, //na
            {"wMsgJ_RDR_STT_FIRST_TWS_TGT",         "STT Lock First"                },
            {"wMsgJ_RDR_STT_TWS_TGT_NUM",           "STT Lock Target"               }, // not endpoint, show hint         
            {"wMsgJ_RDR_STT_TWS_TGT_1",             "STT Lock Target #1"            },
            {"wMsgJ_RDR_STT_TWS_TGT_2",             "STT Lock Target #2"            },
            {"wMsgJ_RDR_STT_TWS_TGT_3",             "STT Lock Target #3"            },
            {"wMsgJ_RDR_STT_TWS_TGT_4",             "STT Lock Target #4"            },
            {"wMsgJ_RDR_STT_TWS_TGT_5",             "STT Lock Target #5"            },
            {"wMsgJ_RDR_STT_TWS_TGT_6",             "STT Lock Target #6"            },
            {"wMsgJ_RDR_STT_TWS_TGT_7",             "STT Lock Target #7"            },
            {"wMsgJ_RDR_STT_TWS_TGT_8",             "STT Lock Target #8"            },

            {"wMsgJ_RDR_BVR",                       "Go BVR"                            }, // not endpoint, disable

            {"wMsgJ_RDR_GO_ACTIVE",                 "Switch Radar Transmit"             },
            {"wMsgJ_RDR_GO_SILENT",                 "Switch Radar Standby"              },

            {"wMsgJ_RDR_SCAN_DIST",                 "TID Range"                        }, // not endpoint, show hint
            {"wMsgJ_RDR_RNG_AUTO",                  "TID Range Automatic"              },
            {"wMsgJ_RDR_RNG_25",                    "TID Range 25"                     },
            {"wMsgJ_RDR_RNG_50",                    "TID Range 50"                     },
            {"wMsgJ_RDR_RNG_100",                   "TID Range 100"                    },
            {"wMsgJ_RDR_RNG_200",                   "TID Range 200"                    },
            {"wMsgJ_RDR_RNG_400",                   "TID Range 400"                    },

            {"wMsgJ_RDR_SCAN_AZ",                   "Scan Azimuth"                      }, // not endpoint, show hint
            {"wMsgJ_RDR_POS",                       "Scan Azimuth Automatic"            },
            {"wMsgJ_RDR_POS_CTR",                   "Scan Azimuth Center"               },
            {"wMsgJ_RDR_POS_CTR_L",                 "Scan Azimuth Center Left"          },
            {"wMsgJ_RDR_POS_CTR_R",                 "Scan Azimuth Center Right"         },
            {"wMsgJ_RDR_POS_L",                     "Scan Azimuth Left"                 },
            {"wMsgJ_RDR_POS_R",                     "Scan Azimuth Right"                },
            {"wMsgJ_RDR_POS_L20",                   "Scan Azimuth Left 20"              },
            {"wMsgJ_RDR_POS_R20",                   "Scan Azimuth Right 20"             },
            {"wMsgJ_RDR_POS_L55",                   "Scan Azimuth Hard Left"            },
            {"wMsgJ_RDR_POS_R55",                   "Scan Azimuth Hard Right"           },

            {"wMsgJ_RDR_SCAN_ELEV",                 "Scan Elevation"                    }, // not endpoint, show hint
            {"wMsgJ_RDR_AUTO",                      "Scan Elevation Automatic"          },
            {"wMsgJ_RDR_POS_HI",                    "Scan Elevation High"               },
            {"wMsgJ_RDR_POS_LO",                    "Scan Elevation Low"                },
            {"wMsgJ_RDR_POS_MID",                   "Scan Elevation Level"              },
            {"wMsgJ_RDR_POS_MID_HI",                "Scan Elevation Level High"         },
            {"wMsgJ_RDR_POS_MID_LO",                "Scan Elevation Level Low"          },
            {"wMsgJ_RDR_ELEV_CLOSE_LOW",            "Scan Close Low"                    },
            {"wMsgJ_RDR_ELEV_CLOSE_MID_LO",         "Scan Close Level Low"              },
            {"wMsgJ_RDR_ELEV_CLOSE_MID",            "Scan Close Level"                  },
            {"wMsgJ_RDR_ELEV_CLOSE_MID_HI",         "Scan Close Level High"             },
            {"wMsgJ_RDR_ELEV_CLOSE_HI",             "Scan Close High"                   },
            {"wMsgJ_RDR_ELEV_MID_LOW",              "Scan Mid Low"                      },
            {"wMsgJ_RDR_ELEV_MID_MID_LO",           "Scan Mid Level Low"                },
            {"wMsgJ_RDR_ELEV_MID_MID",              "Scan Mid Level"                    },
            {"wMsgJ_RDR_ELEV_MID_MID_HI",           "Scan Mid Level High"               },
            {"wMsgJ_RDR_ELEV_MID_HI",               "Scan Mid High"                     },
            {"wMsgJ_RDR_ELEV_LONG_LOW",             "Scan Long Low"                     },
            {"wMsgJ_RDR_ELEV_LONG_MID_LO",          "Scan Long Level Low"               },
            {"wMsgJ_RDR_ELEV_LONG_MID",             "Scan Long Level"                   },
            {"wMsgJ_RDR_ELEV_LONG_MID_HI",          "Scan Long Level High"              },
            {"wMsgJ_RDR_ELEV_LONG_HI",              "Scan Long High"                    },

            {"wMsgJ_RDR_MODE_AUTO",                 "Radar Mode Automatic"          },
            {"wMsgJ_RDR_MODE_TWS",                  "Radar Mode TWS"                },
            {"wMsgJ_RDR_MODE_RWS",                  "Radar Mode RWS"                },
            {"wMsgJ_RDR_MODE",                      "Radar Mode"                    },
            // end of radar

            // spare block
            {"wMsgJ_RDR_STAB",                      "Radar Stabilize"               },
            {"wMsgJ_RDR_STAB_15",                   "Radar Stabilize 15s"           },
            {"wMsgJ_RDR_STAB_30",                   "Radar Stabilize 30s"           },
            {"wMsgJ_RDR_STAB_60",                   "Radar Stabilize 60s"           },
            {"wMsgJ_RDR_STAB_120",                  "Radar Stabilize 120s"          },
            {"wMsgJ_RDR_STAB_INDEF",                "Radar Stabilize Indefinitely"  },
            {"wMsgJ_RDR_STAB_GROUND",               "LANTIRN Stabilize"             },

            {"wMsgLANTIRN_Head_Eyeball",            "LANTIRN Control Eyeballs"      },
            {"wMsgLANTIRN_Head_Head",               "LANTIRN Control Head"          },
            //{"wMsgPlaceHolder098",              "PlaceHolder098"                },
            //{"wMsgPlaceHolder099",              "PlaceHolder099"                },
            
            //NEW 
            {"wMsgJ_RDR_MODE_TWS_MAN",              "TWS Mode Manual"                },
            {"wMsgJ_RDR_MODE_SIZE_M",               "Target Size Normal"             },
            {"wMsgJ_RDR_MODE_SIZE_L",               "Target Size Large"              },
            {"wMsgJ_RDR_MODE_SIZE_S",               "Target Size Small"              },
            {"wMsgJ_RDR_TID_EXP",                   "TID Expand Toggle"              },

            // block: weapons
            {"wMsgJ_WPN_AG_SORDN",              "AG Select Stores"              }, //hint
            {"wMsgJ_WPN_AG_SORDN_WPN_1",        "AG Select Zunis"               },
            {"wMsgJ_WPN_AG_SORDN_WPN_2",        "AG Select Mk84s"               },
            {"wMsgJ_WPN_AG_SORDN_WPN_3",        "AG Select Mk83s"               },
            {"wMsgJ_WPN_AG_SORDN_WPN_4",        "AG Select Mk82s"               },
            {"wMsgJ_WPN_AG_SORDN_WPN_5",        "AG Select Mk81s"               },
            {"wMsgJ_WPN_AG_SORDN_WPN_6",        "AG Select Mk20s"               },
            {"wMsgJ_WPN_AG_SORDN_WPN_7",        "AG Select LUUs"                },
            {"wMsgJ_WPN_AG_SORDN_WPN_8",        "AG Select GBUs"                },
            {"wMsgJ_WPN_AG_SORDN_WPN_9",        "AG Select BDUs"                },
            {"wMsgJ_WPN_AG_SORDN_WPN_10",       "AG Select TALD"                },
            {"wMsgJ_WPN_AG_SORDN_WPN_11",       "AG Select JDAM"                },
            {"wMsgJ_WPN_AG_SORDN_WPN_12",       "AG Select GBU31"               },
            {"wMsgJ_WPN_AG_SORDN_WPN_13",       "AG Select GBU38"               },

            {"wMsgJ_WPN_AG_SPOT",               "AG Weapon Spot"                }, // not used

            {"wMsgJ_WPN_AG_SET_COMP_TGT",       "AG Attack Mode Target"         },
            {"wMsgJ_WPN_AG_SET_COMP_IP",        "AG Attack Mode IP"             },
            {"wMsgJ_WPN_AG_SET_COMP_PILOT",     "AG Attack Mode Pilot"          },
            {"wMsgJ_WPN_AG_SET_MAN",            "AG Attack Mode Manual"         },

            {"wMsgJ_WPN_AG_SET_SNGL",           "Set Release Single"            },
            {"wMsgJ_WPN_AG_SET_PAIRS",          "Set Release Pairs"             },

            {"wMsgJ_WPN_AG_SETFUSE",            "Set Fuse"                      },
            {"wMsgJ_WPN_AG_SETFUSE_NOSETAIL",   "Set Fuse Nose Tail"            },
            {"wMsgJ_WPN_AG_SETFUSE_NOSE",       "Set Fuse Nose"                 },
            {"wMsgJ_WPN_AG_SETFUSE_SAFE",       "Set Fuse Safe"                 },

            {"wMsgJ_WPN_AG_RIP",                "Set Ripple"                    }, //not used (root)

            {"wMsgJ_WPN_AG_RIP_QTY",            "Set Ripple Quantity"           },
            {"wMsgJ_WPN_AG_RIP_QTY_STEP",       "Set Ripple Quantity Step"      },
            {"wMsgJ_WPN_AG_RIP_QTY_2",          "Set Ripple Quantity 2"         }, //2
            {"wMsgJ_WPN_AG_RIP_QTY_5",          "Set Ripple Quantity 3"         }, //3
            {"wMsgJ_WPN_AG_RIP_QTY_10",         "Set Ripple Quantity 4"         }, //4
            {"wMsgJ_WPN_AG_RIP_QTY_20",         "Set Ripple Quantity 6"         }, //6
            {"wMsgJ_WPN_AG_RIP_QTY_30",         "Set Ripple Quantity 8"         }, //8

            {"wMsgJ_WPN_AG_RIP_TIME",           "Set Ripple Time"               },
            {"wMsgJ_WPN_AG_RIP_TIME_STEP",      "Set Ripple Time Step"          },
            {"wMsgJ_WPN_AG_RIP_TIME_10",        "Set Ripple Time 10"            },
            {"wMsgJ_WPN_AG_RIP_TIME_20",        "Set Ripple Time 20"            },
            {"wMsgJ_WPN_AG_RIP_TIME_50",        "Set Ripple Time 50"            },
            {"wMsgJ_WPN_AG_RIP_TIME_100",       "Set Ripple Time 100"           },
            {"wMsgJ_WPN_AG_RIP_TIME_200",       "Set Ripple Time 200"           },
            {"wMsgJ_WPN_AG_RIP_TIME_500",       "Set Ripple Time 500"           },
            {"wMsgJ_WPN_AG_RIP_TIME_990",       "Set Ripple Time 990"           },

            {"wMsgJ_WPN_AG_RIP_DIST",           "Set Ripple Distance"           },
            {"wMsgJ_WPN_AG_RIP_DIST_STEP",      "Set Ripple Distance Step"      }, //na
            {"wMsgJ_WPN_AG_RIP_DIST_5",         "Set Ripple Distance 5"         },
            {"wMsgJ_WPN_AG_RIP_DIST_10",        "Set Ripple Distance 10"        },
            {"wMsgJ_WPN_AG_RIP_DIST_25",        "Set Ripple Distance 25"        },
            {"wMsgJ_WPN_AG_RIP_DIST_50",        "Set Ripple Distance 50"        },
            {"wMsgJ_WPN_AG_RIP_DIST_100",       "Set Ripple Distance 100"       },
            {"wMsgJ_WPN_AG_RIP_DIST_200",       "Set Ripple Distance 200"       },
            {"wMsgJ_WPN_AG_RIP_DIST_400",       "Set Ripple Distance 400"       },

            {"wMsgJ_WPN_AG_UTIL_LANTIRN",       "Switch LANTIRN"                },

            {"wMsgJ_WPN_AG_JETT",               "Jettison Selected"             },
            {"wMsgJ_WPN_AG_DROP_TANKS",         "Jettison Drop Tanks"           },

            {"wMsgJ_WPN_AG",                    "Mode AG"                       },//na
            {"wMsgJ_WPN_AA",                    "Mode AA"                       },//na

            {"wMsgJ_WPN_AG_STN",                "Select Stations"               },
            {"wMsgJ_WPN_AG_STN_18",             "Select Stations 1,8"           },
            {"wMsgJ_WPN_AG_STN_27",             "Select Stations 2,7"           },
            {"wMsgJ_WPN_AG_STN_3456",           "Select Stations 3,4,5,6"       },
            {"wMsgJ_WPN_AG_STN_36",             "Select Stations 3,6"           },
            {"wMsgJ_WPN_AG_STN_45",             "Select Stations 4,5"           },

            {"wMsgJ_WPN_AG_JDAM_WPT_TO_GGW",    "Send Waypoint to GGW"          },
            {"wMsgJ_WPN_AG_JDAM_DESIG_TO_STN",  "Send Designation to Station"   },
            {"wMsgJ_WPN_AG_JDAM_PP_ALL_STN_1",  "PP 1 to GGW ALL STN"           },
            {"wMsgJ_WPN_AG_JDAM_PP_ALL_STN_2",  "PP 2 to GGW ALL STN"           },
            {"wMsgJ_WPN_AG_JDAM_PP_ALL_STN_3",  "PP 3 to GGW ALL STN"           },
            {"wMsgJ_WPN_AG_JDAM_PP_ALL_STN_4",  "PP 4 to GGW ALL STN"           },
            {"wMsgJ_WPN_AG_JDAM_PP_ALL_STN_5",  "PP 5 to GGW ALL STN"           },
            {"wMsgJ_WPN_AG_JDAM_PP_ALL_STN_6",  "PP 6 to GGW ALL STN"           },
            {"wMsgJ_WPN_AG_JDAM_PP_ALL_STN_7",  "PP 7 to GGW ALL STN"           },
            {"wMsgJ_WPN_AG_JDAM_PP_ALL_STN_8",  "PP 8 to GGW ALL STN"           },

            //{"wMsgPlaceHolder155",              "PlaceHolder155"                },
            //{"wMsgPlaceHolder156",              "PlaceHolder156"                },
            //{"wMsgPlaceHolder157",              "PlaceHolder157"                },
            //{"wMsgPlaceHolder158",              "PlaceHolder158"                },
            //{"wMsgPlaceHolder159",              "PlaceHolder159"                },
            //{"wMsgPlaceHolder160",              "PlaceHolder160"                },
            //{"wMsgPlaceHolder161",              "PlaceHolder161"                },
            //{"wMsgPlaceHolder162",              "PlaceHolder162"                },
            //{"wMsgPlaceHolder163",              "PlaceHolder163"                },
            //{"wMsgPlaceHolder164",              "PlaceHolder164"                },
            //{"wMsgPlaceHolder165",              "PlaceHolder165"                },
            //{"wMsgPlaceHolder166",              "PlaceHolder166"                },
            //{"wMsgPlaceHolder167",              "PlaceHolder167"                },
            //{"wMsgPlaceHolder168",              "PlaceHolder168"                },
            //{"wMsgPlaceHolder169",              "PlaceHolder169"                },
            //{"wMsgPlaceHolder170",              "PlaceHolder170"                },
            //{"wMsgPlaceHolder171",              "PlaceHolder171"                },
            //{"wMsgPlaceHolder172",              "PlaceHolder172"                },
            //{"wMsgPlaceHolder173",              "PlaceHolder173"                },
            //{"wMsgPlaceHolder174",              "PlaceHolder174"                },
            //{"wMsgPlaceHolder175",              "PlaceHolder175"                },
            //{"wMsgPlaceHolder176",              "PlaceHolder176"                },
            //{"wMsgPlaceHolder177",              "PlaceHolder177"                },
            //{"wMsgPlaceHolder178",              "PlaceHolder178"                },
            //{"wMsgPlaceHolder179",              "PlaceHolder179"                },
            //{"wMsgPlaceHolder180",              "PlaceHolder180"                },
            //{"wMsgPlaceHolder181",              "PlaceHolder181"                },
            //{"wMsgPlaceHolder182",              "PlaceHolder182"                },
            //{"wMsgPlaceHolder183",              "PlaceHolder183"                },
            //{"wMsgPlaceHolder184",              "PlaceHolder184"                },
            //{"wMsgPlaceHolder185",              "PlaceHolder185"                },
            //{"wMsgPlaceHolder186",              "PlaceHolder186"                },
            //{"wMsgPlaceHolder187",              "PlaceHolder187"                },
            {"wMsgJ_WPN_AG_RIP_QTY_16",         "Set Ripple Quantity 16"        },
            {"wMsgJ_WPN_AG_RIP_QTY_28",         "Set Ripple Quantity 28"        },
            // end of weapons

            // spare table

            //{"wMsgPlaceHolder194",              "PlaceHolder194"                },
            //{"wMsgPlaceHolder195",              "PlaceHolder195"                },
            //{"wMsgPlaceHolder196",              "PlaceHolder196"                },
            //{"wMsgPlaceHolder197",              "PlaceHolder197"                },
            //{"wMsgPlaceHolder198",              "PlaceHolder198"                },
            //{"wMsgPlaceHolder199",              "PlaceHolder199"                },

            // block: radio
            {"wMsgJ_RAD_182_USE_GUARD",         "AN/ARC-182 Use Guard"          },
            {"wMsgJ_RAD_182_USE_MANUAL",        "Radio Use Manual"              },
            {"wMsgJ_RAD_182_USE_CHAN",          "Radio Use Preset"              },

            // Datalink
            {"wMsgJ_RAD_DL",                    "Datalink"                      },
            {"wMsgJ_RAD_DL_SET_MODE",           "Datalink Mode"                 },
            {"wMsgJ_RAD_DL_SET_FREQ_PRST",      "Datalink Preset"               },
            {"wMsgJ_RAD_DL_SET_HOST",           "Datalink Set Host"             },
            {"wMsgJ_RAD_DL_TACT",               "Datalink Tactical"             },
            {"wMsgJ_RAD_DL_OFF",                "Datalink Off"                  },
            {"wMsgJ_RAD_DL_FGHT",               "Datalink Fighter"              },

            {"wMsgJ_RAD_DL_SET_FREQ_1",         "DL Preset 300.00 Mhz"             },
            {"wMsgJ_RAD_DL_SET_FREQ_2",         "DL Preset 300.10 Mhz"             },
            {"wMsgJ_RAD_DL_SET_FREQ_3",         "DL Preset 300.20 Mhz"             },
            {"wMsgJ_RAD_DL_SET_FREQ_4",         "DL Preset 300.30 Mhz"             },
            {"wMsgJ_RAD_DL_SET_FREQ_5",         "DL Preset 300.40 Mhz"             },
            {"wMsgJ_RAD_DL_SET_FREQ_6",         "DL Preset 300.50 Mhz"             },
            {"wMsgJ_RAD_DL_SET_FREQ_7",         "DL Preset 300.60 Mhz"             },
            {"wMsgJ_RAD_DL_SET_FREQ_8",         "DL Preset 300.70 Mhz"             },

            // TACAN
            {"wMsgJ_RAD_TCN",                   "Radio TACAN"               }, //disabled

            {"wMsgJ_RAD_TCN_MODE",              "TACAN Mode"                },
            {"wMsgJ_RAD_TCN_MODE_OFF",          "TACAN Mode Off"            },
            {"wMsgJ_RAD_TCN_MODE_REC",          "TACAN Mode REC"            },
            {"wMsgJ_RAD_TCN_MODE_TR",           "TACAN Mode TR"             },
            {"wMsgJ_RAD_TCN_MODE_AA",           "TACAN Mode AA"             },
            {"wMsgJ_RAD_TCN_MODE_BCN",          "TACAN Mode BCN"            },

            {"wMsgJ_RAD_TCN_SEL_CHAN",          "TACAN Select Channel"          },
            {"wMsgJ_RAD_TCN_SEL_GND_STN",       "TACAN Ground Station"          },
            {"wMsgJ_RAD_TCN_TUNE_TAC",          "TACAN Tune TAC"                },

            {"wMsgJ_RAD_TCN_TAC_STEN",              "TACAN Tune Stennis"                },
            {"wMsgJ_RAD_TCN_TAC_ARCO",              "TACAN Tune Arco"                },
            {"wMsgJ_RAD_TCN_TAC_SHEL",              "TACAN Tune Shell"                },
            {"wMsgJ_RAD_TCN_TAC_TEXA",              "TACAN Tune Texaco"                },

            {"wMsgJ_RAD_TCN_T_CS_TSK",              "TACAN Tune TSK (31X)"                },
            {"wMsgJ_RAD_TCN_T_CS_KBL",              "TACAN Tune KBL (67X)"                },
            {"wMsgJ_RAD_TCN_T_CS_BTM",              "TACAN Tune BTM (16X)"                },
            {"wMsgJ_RAD_TCN_T_CS_KTS",              "TACAN Tune KTS (44X)"                },
            {"wMsgJ_RAD_TCN_T_CS_GTB",              "TACAN Tune GTB (25X)"                },
            {"wMsgJ_RAD_TCN_T_CS_VAS",              "TACAN Tune VAS (22X)"                },
            {"wMsgJ_RAD_TCN_T_NV_LSV",              "TACAN Tune LSV (12X)"                },
            {"wMsgJ_RAD_TCN_T_NV_LAS",              "TACAN Tune LAS (116X)"               },
            {"wMsgJ_RAD_TCN_T_NV_BLD",              "TACAN Tune BLD (114X)"               },
            {"wMsgJ_RAD_TCN_T_NV_INS",              "TACAN Tune INS (87X)"                },
            {"wMsgJ_RAD_TCN_T_NV_MMM",              "TACAN Tune MMM (90X)"                },
            {"wMsgJ_RAD_TCN_T_NV_GFS",              "TACAN Tune GFS (91X)"                },
            {"wMsgJ_RAD_TCN_T_NV_GRL",              "TACAN Tune GRL (18X)"                },
            {"wMsgJ_RAD_TCN_T_NV_PGS",              "TACAN Tune PGS (57X)"                },
            {"wMsgJ_RAD_TCN_T_NV_BTY",              "TACAN Tune BTY (94X)"                },
            {"wMsgJ_RAD_TCN_T_NV_EED",              "TACAN Tune EED (99X)"                },
            {"wMsgJ_RAD_TCN_T_NV_DAG",              "TACAN Tune DAG (79X)"                },
            {"wMsgJ_RAD_TCN_T_NV_HEC",              "TACAN Tune HEC (74X)"                },
            {"wMsgJ_RAD_TCN_T_NV_OAL",              "TACAN Tune OAL (124X)"               },
            {"wMsgJ_RAD_TCN_T_NV_BIH",              "TACAN Tune BIH (33X)"                },
            {"wMsgJ_RAD_TCN_T_NV_MVA",              "TACAN Tune MVA (98X)"                },
            {"wMsgJ_RAD_TCN_T_PG_KCK",              "TACAN Tune KCK (89X)"                },
            {"wMsgJ_RAD_TCN_T_PG_KSB",              "TACAN Tune KSB (84X)"                },
            {"wMsgJ_RAD_TCN_T_PG_HDR",              "TACAN Tune HDR (47X)"                },
            {"wMsgJ_RAD_TCN_T_PG_MA",               "TACAN Tune MA (96X)"                 },
            {"wMsgJ_RAD_TCN_T_PG_SYZI",             "TACAN Tune SYZI (94X)"               },
            {"wMsgJ_RAD_TCN_T_STN",                 "TACAN Tune STN (74X)"                },


            {"wMsgJ_RAD_182_MODE_TR",               "AN/ARC-182 Mode TR"            },
            {"wMsgJ_RAD_182_MODE_TRG",              "AN/ARC-182 Mode TRG"           },
            {"wMsgJ_RAD_182_MODE_DF",               "AN/ARC-182 Mode DF"            },
            {"wMsgJ_RAD_182_MODE_TEST",             "AN/ARC-182 Mode Test"          },
            // end of radio

            // spare table
            {"wMsgJ_RAD_182_MODE_AM",               "AN/ARC-182 Mode AM"            },
            {"wMsgJ_RAD_182_MODE_FM",               "AN/ARC-182 Mode FM"            },
            {"wMsgJ_RAD_182_MODE",                  "AN/ARC-182 Mode "              },
            {"wMsgJ_RAD_182_MODE_OFF",              "AN/ARC-182 Mode Off"           },

            {"wMsgJ_RAD_DL_HOST_STEN",              "DL Host Stennis"               },
            {"wMsgJ_RAD_DL_HOST_DARK",              "DL Host Darkstar"              },
            {"wMsgJ_RAD_DL_HOST_FOCS",              "DL Host Focus"                 },
            {"wMsgJ_RAD_DL_HOST_MAGC",              "DL Host Magic"                 },
            {"wMsgJ_RAD_DL_HOST_OVRL",              "DL Host Overlord"              },
            {"wMsgJ_RAD_DL_HOST_WIZR",              "DL Host Wizard"                },

            // block: utility
            {"wMsgJ_UTIL_NAV",                  "Utility Navigation"            }, //na
            {"wMsgJ_UTIL_NAV_SEL_DEST_SPT",     "Select Destination Steerpoint" },//na
            {"wMsgJ_UTIL_NAV_REST_MSN_SPT",     "Restore Mission Steerpoint"    },//na
            {"wMsgJ_UTIL_NAV_MAN_ENT_SPT",      "Map Manual Steerpoint"         },//na
            {"wMsgJ_UTIL_NAV_MAP_SPT",          "Navigate Steerpoint"                },
            {"wMsgJ_UTIL_NAV_MAP_SPT_1",        "Navigate Steerpoint 1"              },
            {"wMsgJ_UTIL_NAV_MAP_SPT_2",        "Navigate Steerpoint 2"              },
            {"wMsgJ_UTIL_NAV_MAP_SPT_3",        "Navigate Steerpoint 3"              },
            {"wMsgJ_UTIL_NAV_MAP_SPT_4",        "Map Steerpoint 4"              }, // 4-8 not used
            {"wMsgJ_UTIL_NAV_MAP_SPT_5",        "Map Steerpoint 5"              },
            {"wMsgJ_UTIL_NAV_MAP_SPT_6",        "Map Steerpoint 6"              },
            {"wMsgJ_UTIL_NAV_MAP_SPT_7",        "Map Steerpoint 7"              },
            {"wMsgJ_UTIL_NAV_MAP_SPT_8",        "Map Steerpoint 8"              },
            {"wMsgJ_UTIL_NAV_MAP_SPT_9",        "Map Steerpoint 9"              },
            {"wMsgJ_UTIL_NAV_MAP_SPT_10",       "Map Steerpoint 10"             },
            {"wMsgJ_UTIL_NAV_MAP_SPT_11",       "Map Steerpoint 11"             },
            {"wMsgJ_UTIL_NAV_MAP_SPT_12",       "Map Steerpoint 12"             },
            {"wMsgJ_UTIL_NAV_MAP_SPT_13",       "Map Steerpoint 13"             },
            {"wMsgJ_UTIL_NAV_MAP_SPT_14",       "Map Steerpoint 14"             },

            {"wMsgJ_UTIL_NAV_MAP_FIX_PNT",      "Navigate Fixed Point"          },
            {"wMsgJ_UTIL_NAV_MAP_INIT_PNT",     "Navigate Initial Point"        },
            {"wMsgJ_UTIL_NAV_SURF_TGT",         "Navigate Surface Target"       },
            {"wMsgJ_UTIL_NAV_HOME_BASE",        "Navigate Home Base"            },

            {"wMsgJ_UTIL_NAV_REST_MORE",        "Restore"                       }, //hint only
            {"wMsgJ_UTIL_NAV_REST_MSN_SPT_1",   "Restore Steerpoint 1"          },
            {"wMsgJ_UTIL_NAV_REST_MSN_SPT_2",   "Restore Steerpoint 2"          },
            {"wMsgJ_UTIL_NAV_REST_MSN_SPT_3",   "Restore Steerpoint 3"          },
            {"wMsgJ_UTIL_NAV_REST_INIT_PT_1",   "Restore Initial Point"         },
            {"wMsgJ_UTIL_NAV_REST_INIT_PT_2",   "Restore Initial Point 2"       }, //na
            {"wMsgJ_UTIL_NAV_REST_FIX_PT",      "Restore Fixed Point"           },
            {"wMsgJ_UTIL_NAV_REST_MN_FIX_PT",   "Restore Mission Point"         }, //na
            {"wMsgJ_UTIL_NAV_REST_STGT_1",      "Restore Target"                },
            {"wMsgJ_UTIL_NAV_REST_HOME",        "Restore Home Base"             },
            {"wMsgJ_UTIL_NAV_DEF_PNT",          "Restore Defense Point"         },
            {"wMsgJ_UTIL_NAV_HSTZONE",          "Restore Hostile Zone"          }, //

            {"wMsgJ_UTIL_NAV_LOAD_FLT_PLAN_1",  "Load Flight Plan 1"            }, //f-14B(U) Flight Plan managment
            {"wMsgJ_UTIL_NAV_LOAD_FLT_PLAN_2",  "Load Flight Plan 2"            },
            {"wMsgJ_UTIL_NAV_LOAD_FLT_PLAN_3",  "Load Flight Plan 3"            },
            {"wMsgJ_UTIL_NAV_LOAD_FLT_PLAN_4",  "Load Flight Plan 4"            },
            {"wMsgJ_UTIL_NAV_LOAD_FLT_PLAN_5",  "Load Flight Plan 5"            },
            {"wMsgJ_UTIL_NAV_LOAD_FLT_PLAN_6",  "Load Flight Plan 6"            },
            {"wMsgJ_UTIL_NAV_LOAD_FLT_PLAN_7",  "Load Flight Plan 7"            },
            {"wMsgJ_UTIL_NAV_RELOAD_FLT_PLAN",  "Reload Flight Plan"            },
            {"wMsgJ_UTIL_NAV_DIR_SPT_1",        "Direct Steerpoint 1"           },
            {"wMsgJ_UTIL_NAV_DIR_SPT_2",        "Direct Steerpoint 2"           },
            {"wMsgJ_UTIL_NAV_DIR_SPT_3",        "Direct Steerpoint 3"           },
            {"wMsgJ_UTIL_NAV_DIR_SPT_4",        "Direct Steerpoint 4"           },
            {"wMsgJ_UTIL_NAV_DIR_SPT_5",        "Direct Steerpoint 5"           },
            {"wMsgJ_UTIL_NAV_DIR_SPT_6",        "Direct Steerpoint 6"           },
            {"wMsgJ_UTIL_NAV_DIR_SPT_7",        "Direct Steerpoint 7"           },
            {"wMsgJ_UTIL_NAV_DIR_SPT_8",        "Direct Steerpoint 8"           },
            {"wMsgJ_UTIL_NAV_DIR_SPT_9",        "Direct Steerpoint 9"           },
            {"wMsgJ_UTIL_NAV_DIR_SPT_10",       "Direct Steerpoint 10"          },
            {"wMsgJ_UTIL_NAV_DIR_SPT_11",       "Direct Steerpoint 11"          },
            {"wMsgJ_UTIL_NAV_DIR_SPT_12",       "Direct Steerpoint 12"          },
            {"wMsgJ_UTIL_NAV_DIR_SPT_13",       "Direct Steerpoint 13"          },
            {"wMsgJ_UTIL_NAV_DIR_SPT_14",       "Direct Steerpoint 14"          },
            {"wMsgJ_UTIL_NAV_MODE_EGI",         "Nav Mode EGI"                  },
            {"wMsgJ_UTIL_NAV_MODE_DEST",        "Nav Mode Destination"          },
            {"wMsgJ_UTIL_NAV_SEQ_AUTO",         "Nav Sequence Auto"             },
            {"wMsgJ_UTIL_NAV_SEQ_OFLY",         "Nav Sequence Overfly"          },
            {"wMsgJ_UTIL_NAV_SEQ_MAN",          "Nav Sequence Manual"           },
            {"wMsgJ_UTIL_NAV_BHDI_FLY",         "Nav Backup to Selected"        },

            {"wMsgJ_UTIL_CONTR",                "Utility Contract"              }, //na
            {"wMsgJ_UTIL_CONTR_NO_TALK",        "No Talking"                    },
            {"wMsgJ_UTIL_CONTR_TALK",           "Talking Allowed"               },
            {"wMsgJ_UTIL_CONTR_EJECT_BTH",      "Ejection handle both"          },
            {"wMsgJ_UTIL_CONTR_EJECT_SNG",      "Ejection handle RIO only"      },
            {"wMsgJ_UTIL_CONTR_CALL",           "Landing callouts allowed"      },
            {"wMsgJ_UTIL_CONTR_NO_CALL",        "No landing callouts"           },
            {"wMsgJ_UTIL_CONTR_ACTIVE",         "Jester resume"                 },//na
            {"wMsgJ_UTIL_CONTR_INACTIVE",       "Jester suspend"                },//na
            {"wMsgJ_UTIL_CONTR_AUTO_EXPAND",    "Enable Auto Expand"            },
            {"wMsgJ_UTIL_CONTR_AUTO_VID",       "Disable Auto VID"              },
            {"wMsgJ_UTIL_CONTR_NO_IFF_REPLY_BANDIT", "Treat no-reply as bandit" },
            {"wMsgJ_UTIL_CONTR_DISABLE_AUTO_EXPAND", "Disable Auto Expand"       },
            {"wMsgJ_UTIL_CONTR_ENABLE_AUTO_VID", "Enable Auto VID"              },
            {"wMsgJ_UTIL_CONTR_NO_IFF_REPLY_BOGEY", "Treat no-reply as bogey"   },

            {"wMsgJ_RESET",                     "Reset"                         },
            // Supercarriers
            {"wMsgJ_RAD_DL_HOST_WASH",          "DL Host Washington"            },
            {"wMsgJ_RAD_DL_HOST_ROOS",          "DL Host Roosevelt"             },
            {"wMsgJ_RAD_DL_HOST_LINC",          "DL Host Lincoln"               },
            {"wMsgJ_RAD_DL_HOST_TRUM",          "DL Host Truman"                },
            {"wMsgJ_RAD_DL_HOST_TICO",          "DL Host Ticonderoga"           },
            {"wMsgJ_RAD_DL_HOST_FORE",          "DL Host Forrestal"             }, // Add Forrestal
            {"wMsgJ_RAD_DL_HOST_BURK",          "DL Host Arleigh Burke"         },
            {"wMsgJ_RAD_TCN_TAC_WASH" ,         "TACAN Tune Washington"                     },
            {"wMsgJ_RAD_TCN_TAC_ROOS",          "TACAN Tune Roosevelt"                      },
            {"wMsgJ_RAD_TCN_TAC_LINC" ,         "TACAN Tune Lincoln"                        },
            {"wMsgJ_RAD_TCN_TAC_TRUM",          "TACAN Tune Truman"                         },
            {"wMsgJ_RAD_TCN_TAC_FORE",          "TACAN Tune Forrestal"                      }, // Add Forrestal

            {"wMsgJESTER_Steerpoint_SP1",       "LANTIRN Track Steerpoint 1"                },
            {"wMsgJESTER_Steerpoint_SP2",       "LANTIRN Track Steerpoint 2"                },
            {"wMsgJESTER_Steerpoint_SP3",       "LANTIRN Track Steerpoint 3"                },
            {"wMsgJESTER_Steerpoint_FP",        "LANTIRN Track FP"                          },
            {"wMsgJESTER_Steerpoint_IP",        "LANTIRN Track IP"                          },
            {"wMsgJESTER_Steerpoint_ST",        "LANTIRN Track ST"                          },
            {"wMsgJESTER_Steerpoint_HB",        "LANTIRN Track HB"                          },
            {"wMsgJESTER_Steerpoint_MAN",       "LANTIRN Track DP"                          },
            {"wMsgLANTIRN_GPSZero",             "LANTIRN Reset"                             },
            {"wMsgLANTIRN_ToggleFOV",           "LANTIRN Toggle FOV"                        },
            {"wMsgJ_UTIL_NAV_MAP_MARKER",       "J_UTIL_NAV_MAP_MARKER"         },
            {"wMsgJ_UTIL_NAV_GRD_ENABLE",       "Enable NAVGRID"                },
            {"wMsgJ_UTIL_NAV_GRD_DSABLE",       "Disable NAVGRID"               },
            {"wMsgJ_UTIL_NAV_GRD_CENTER",       "Grid Center Aircraft"          },
            {"wMsgJ_UTIL_NAV_GRD_REL_180",      "Grid Relative 180"             },
            {"wMsgJ_UTIL_NAV_GRD_REL_u30",      "Grid Relative +30"             },
            {"wMsgJ_UTIL_NAV_GRD_REL_u90",      "Grid Relative +90"             },
            {"wMsgJ_UTIL_NAV_GRD_REL_u120",     "Grid Relative +120"            },
            {"wMsgJ_UTIL_NAV_GRD_REL_d120",     "Grid Relative -120"            },
            {"wMsgJ_UTIL_NAV_GRD_REL_d90",      "Grid Relative -90"             },
            {"wMsgJ_UTIL_NAV_GRD_REL_d30",      "Grid Relative -30"             },
            {"wMsgJ_UTIL_NAV_GRD_ABS_0",        "Grid Absolute 0"               },
            {"wMsgJ_UTIL_NAV_GRD_ABS_45",       "Grid Absolute 45"              },
            {"wMsgJ_UTIL_NAV_GRD_ABS_90",       "Grid Absolute 90"              },
            {"wMsgJ_UTIL_NAV_GRD_ABS_135",      "Grid Absolute 135"             },
            {"wMsgJ_UTIL_NAV_GRD_ABS_180",      "Grid Absolute 180"             },
            {"wMsgJ_UTIL_NAV_GRD_ABS_225",      "Grid Absolute 225"             },
            {"wMsgJ_UTIL_NAV_GRD_ABS_270",      "Grid Absolute 270"             },
            {"wMsgJ_UTIL_NAV_GRD_ABS_315",      "Grid Absolute 315"             },
            {"wMsgJ_UTIL_NAV_GRD_COV_30",       "Grid Coverage 30"              },
            {"wMsgJ_UTIL_NAV_GRD_COV_60",       "Grid Coverage 60"              },
            {"wMsgJ_UTIL_NAV_GRD_COV_90",       "Grid Coverage 90"              },
            {"wMsgJ_UTIL_NAV_GRD_COV_120",      "Grid Coverage 120"             },
            {"wMsgJ_UTIL_NAV_GRD_COV_180",      "Grid Coverage 180"             },
            {"wMsgJ_UTIL_NAV_GRD_1SCTR",        "Grid 1 Sector"                 },
            {"wMsgJ_UTIL_NAV_GRD_2SCTR",        "Grid 2 Sectors"                },
            {"wMsgJ_UTIL_NAV_GRD_3SCTR",        "Grid 3 Sectors"                },
            {"wMsgJ_UTIL_NAV_GRD_4SCTR",        "Grid 4 Sectors"                },
            {"wMsgJ_UTIL_NAV_GRD_5SCTR",        "Grid 5 Sectors"                },
            {"wMsgJ_UTIL_NAV_GRD_6SCTR",        "Grid 6 Sectors"                },
            {"wMsgJ_UTIL_NAV_GRD_MARKER",       "J_UTIL_NAV_GRD_MARKER"         },
            
            //{"wMsgPlaceHolder383",              "PlaceHolder383"                },
            //{"wMsgPlaceHolder384",              "PlaceHolder384"                },
            //{"wMsgPlaceHolder385",              "PlaceHolder385"                },
            //{"wMsgPlaceHolder386",              "PlaceHolder386"                },
            //{"wMsgPlaceHolder387",              "PlaceHolder387"                },
            //{"wMsgPlaceHolder388",              "PlaceHolder388"                },
            //{"wMsgPlaceHolder389",              "PlaceHolder389"                },
            // end of radio

            // WALKMAN
            {"wMsgJ_WLKMN_PLAY",                "Play the Walkman"              },
            {"wMsgJ_WLKMN_STOP",                "Stop the Walkman"              },
            {"wMsgJ_WLKMN_NEXT",                "Fast Forward"                  },
            {"wMsgJ_WLKMN_PREV",                "Rewind"                        },

            {"wMsgJ_RDR_ASP_BEAM",              "Aspect Switch Beam"            },
            {"wMsgJ_RDR_ASP_NOSE",              "Aspect Switch Nose"            },
            {"wMsgJ_RDR_ASP_TAIL",              "Aspect Switch Tail"            },
            //{"wMsgPlaceHolder397",              "PlaceHolder397"                },
            //{"wMsgPlaceHolder398",              "PlaceHolder398"                },
            //{"wMsgPlaceHolder399",              "PlaceHolder399"                },

            // block: defensive
            {"wMsgJ_DEF_CMS_MOD",               "CMS/CMDS Mode"                 }, // indirect
            {"wMsgJ_DEF_CMS_MOD_OFF",           "CMS/CMDS Mode Off"             },
            {"wMsgJ_DEF_CMS_MOD_MAN",           "CMS/CMDS Mode Manual"          },
            {"wMsgJ_DEF_CMS_MOD_AUTO",          "CMS/CMDS Mode Auto"            },
            {"wMsgJ_DEF_CMS_MOD_SBY",           "CMDS Mode Standby"             },
            {"wMsgJ_DEF_CMS_MOD_SEMI",          "CMDS Mode Semi"                },
            {"wMsgJ_DEF_CMS_MOD_BYP",           "CMDS Mode Bypass"              },

            {"wMsgJ_DEF_FLR_MOD",               "Flares Mode"                   }, // not endpoint
            {"wMsgJ_DEF_FLR_MOD_PILOT",         "Flares Mode to Pilot"          },
            {"wMsgJ_DEF_FLR_MOD_NORM",          "Flares Mode to Normal"         },
            {"wMsgJ_DEF_FLR_MOD_MULTI",         "Flares Mode to Multi"          },

            {"wMsgJ_DEF_MAN_PGM",               "Manual CMDS Program"                },
            {"wMsgJ_DEF_MAN_PGM_1",             "Manual CMDS Program 1"              },
            {"wMsgJ_DEF_MAN_PGM_2",             "Manual CMDS Program 2"              },
            {"wMsgJ_DEF_MAN_PGM_3",             "Manual CMDS Program 3"              },
            {"wMsgJ_DEF_MAN_PGM_4",             "Manual CMDS Program 4"              },

            {"wMsgJ_DEF_CHF_PGM",               "Chaff Program"                         },
            {"wMsgJ_DEF_CHF_PGM_RR_12",         "Chaff Program 1 (RR 12)"               },
            {"wMsgJ_DEF_CHF_PGM_RR_46",         "Chaff Program 2 (RR 46)"               },
            {"wMsgJ_DEF_CHF_PGM_RR_86",         "Chaff Program 3 (RR 86)"               },
            {"wMsgJ_DEF_CHF_PGM_20_44",         "Chaff Program 4 (20 44)"               },
            {"wMsgJ_DEF_CHF_PGM_20_84",         "Chaff Program 5 (20 84)"               },
            {"wMsgJ_DEF_CHF_PGM_40_44",         "Chaff Program 6 (40 44)"               },
            {"wMsgJ_DEF_CHF_PGM_40_84",         "Chaff Program 7 (40 84)"               },
            {"wMsgJ_DEF_CHF_PGM_R1_82",         "Chaff Program 8 (R1 82)"               },
            {"wMsgJ_DEF_CMDS_IHBT_1",           "CMDS Inhibit Chaff"                    },
            {"wMsgJ_DEF_CMDS_IHBT_2",           "CMDS Inhibit Flares"                   },
            {"wMsgJ_DEF_CMDS_IHBT_3",           "CMDS Inhibit 01"                       },
            {"wMsgJ_DEF_CMDS_IHBT_4",           "CMDS Inhibit 02"                       },
            {"wMsgJ_DEF_CMDS_IHBT_5",           "CMDS Inhibit RWR"                      },
            {"wMsgJ_DEF_CMDS_IHBT_6",           "CMDS Inhibit MWS"                      },
            {"wMsgJ_DEF_CMDS_IHBT_7",           "CMDS Inhibit Jammer"                   },

            {"wMsgJ_DEF_RWR_DSP_TYP",           "RWR Display Type"              },
            {"wMsgJ_DEF_RWR_AIRB",              "RWR Mode Airborne"             },
            {"wMsgJ_DEF_RWR_NORM",              "RWR Mode Normal"               },
            {"wMsgJ_DEF_RWR_AAA",               "RWR Mode AAA"                  },
            {"wMsgJ_DEF_RWR_UNK",               "RWR Mode Unknown"              },
            {"wMsgJ_DEF_RWR_FRND",              "RWR Mode Friendly"             },

            {"wMsgJ_DEF_JMR_ON",                "Jammer On"                     },
            {"wMsgJ_DEF_JMR_SBY",               "Jammer Standby"                },

            {"wMsgJ_DEF_CMS_CTL_ORD",           "Dispense Order"                 }, // not endpoint
            {"wMsgJ_DEF_CMS_CHF_PGM",           "Disp Order Chaff Program"       },
            {"wMsgJ_DEF_CMS_CHF_SNGL",          "Disp Order Chaff Single"        },
            {"wMsgJ_DEF_CMS_CHF_TGHT",          "Disp Order Chaff Tight"         },
            {"wMsgJ_DEF_CMS_FLR_PGM",           "Disp Order Flare Program"       },
            {"wMsgJ_DEF_CMS_FLR_SNGL",          "Disp Order Flare Single"        },
            {"wMsgJ_DEF_CMS_FLR_TGHT",          "Disp Order Flare Tight"         },

            {"wMsgJ_DEF_FLR_PGM",               "Flares Program"                }, //hint
            {"wMsgJ_DEF_FLR_PGM_1",             "Flares Program (2x2sec)"       },
            {"wMsgJ_DEF_FLR_PGM_2",             "Flares Program (4x2sec)"       },
            {"wMsgJ_DEF_FLR_PGM_3",             "Flares Program (10x2sec)"      },
            {"wMsgJ_DEF_FLR_PGM_4",             "Flares Program (4x6sec)"       },
            {"wMsgJ_DEF_FLR_PGM_5",             "Flares Program (8x6sec)"       },
            {"wMsgJ_DEF_FLR_PGM_6",             "Flares Program (10x6sec)"      },
            {"wMsgJ_DEF_FLR_PGM_7",             "Flares Program (6x10sec)"      },
            {"wMsgJ_DEF_FLR_PGM_8",             "Flares Program (10x10sec)"     },


            {"wMsgLANTIRN_Srch_Any",            "LANTIRN Search All"            },
            {"wMsgLANTIRN_Srch_Any_Active",     "LANTIRN Search Active"         },
            {"wMsgLANTIRN_Srch_Air",            "LANTIRN Search Bogeys"         },
            {"wMsgLANTIRN_Srch_Air_Active",     "LANTIRN Search Active Bogeys"  },
            {"wMsgLANTIRN_Srch_SAM_Active",     "LANTIRN Search SAMs"           },
            {"wMsgLANTIRN_Srch_Armor_Active",   "LANTIRN Search Armor"          },
            {"wMsgLANTIRN_Srch_Vehicle",        "LANTIRN Search Vehicles"       },
            {"wMsgLANTIRN_Ships_Active",        "LANTIRN Search Ships"          },
            //{"wMsgPlaceHolder449",              "PlaceHolder449"                },
            //{"wMsgPlaceHolder450",              "PlaceHolder450"                },
            //{"wMsgPlaceHolder451",              "PlaceHolder451"                },
            //{"wMsgPlaceHolder452",              "PlaceHolder452"                },
            //{"wMsgPlaceHolder453",              "PlaceHolder453"                },
            //{"wMsgPlaceHolder454",              "PlaceHolder454"                },
            //{"wMsgPlaceHolder455",              "PlaceHolder455"                },
            //{"wMsgPlaceHolder456",              "PlaceHolder456"                },
            //{"wMsgPlaceHolder457",              "PlaceHolder457"                },
            //{"wMsgPlaceHolder458",              "PlaceHolder458"                },
            //{"wMsgPlaceHolder459",              "PlaceHolder459"                },
            //{"wMsgPlaceHolder460",              "PlaceHolder460"                },
            //{"wMsgPlaceHolder461",              "PlaceHolder461"                },
            //{"wMsgPlaceHolder462",              "PlaceHolder462"                },
            //{"wMsgPlaceHolder463",              "PlaceHolder463"                },
            //{"wMsgPlaceHolder464",              "PlaceHolder464"                },
            //{"wMsgPlaceHolder465",              "PlaceHolder465"                },
            //{"wMsgPlaceHolder466",              "PlaceHolder466"                },
            //{"wMsgPlaceHolder467",              "PlaceHolder467"                },
            //{"wMsgPlaceHolder468",              "PlaceHolder468"                },
            //{"wMsgPlaceHolder469",              "PlaceHolder469"                },
            //{"wMsgPlaceHolder470",              "PlaceHolder470"                },
            //{"wMsgPlaceHolder471",              "PlaceHolder471"                },
            //{"wMsgPlaceHolder472",              "PlaceHolder472"                },
            //{"wMsgPlaceHolder473",              "PlaceHolder473"                },
            //{"wMsgPlaceHolder474",              "PlaceHolder474"                },
            //{"wMsgPlaceHolder475",              "PlaceHolder475"                },
            //{"wMsgPlaceHolder476",              "PlaceHolder476"                },
            //{"wMsgPlaceHolder477",              "PlaceHolder477"                },
            //{"wMsgPlaceHolder478",              "PlaceHolder478"                },
            //{"wMsgPlaceHolder479",              "PlaceHolder479"                },
            //{"wMsgPlaceHolder480",              "PlaceHolder480"                },
            //{"wMsgPlaceHolder481",              "PlaceHolder481"                },
            //{"wMsgPlaceHolder482",              "PlaceHolder482"                },
            //{"wMsgPlaceHolder483",              "PlaceHolder483"                },
            //{"wMsgPlaceHolder484",              "PlaceHolder484"                },
            //{"wMsgPlaceHolder485",              "PlaceHolder485"                },
            //{"wMsgPlaceHolder486",              "PlaceHolder486"                },
            //{"wMsgPlaceHolder487",              "PlaceHolder487"                },
            //{"wMsgPlaceHolder488",              "PlaceHolder488"                },
            //{"wMsgPlaceHolder489",              "PlaceHolder489"                },
            // end of defensive

            // spare table
            //{"wMsgPlaceHolder490",              "PlaceHolder490"                },
            //{"wMsgPlaceHolder491",              "PlaceHolder491"                },
            //{"wMsgPlaceHolder492",              "PlaceHolder492"                },
            //{"wMsgPlaceHolder493",              "PlaceHolder493"                },
            //{"wMsgPlaceHolder494",              "PlaceHolder494"                },
            //{"wMsgPlaceHolder495",              "PlaceHolder495"                },
            //{"wMsgPlaceHolder496",              "PlaceHolder496"                },
            //{"wMsgPlaceHolder497",              "PlaceHolder497"                },
            //{"wMsgPlaceHolder498",              "PlaceHolder498"                },
            //{"wMsgPlaceHolder499",              "PlaceHolder499"                },

            //RIO 500-600

            // startup
            {"wMsgJ_STRT_ABORT",                "Cancel Procedure"              },
            {"wMsgJ_STRT_INS_FINE",             "INS Alignment Fine"            },
            {"wMsgJ_STRT_INS_MIN_WPN",          "INS Alignment Minimum"         },
            {"wMsgJ_STRT_INS_COARSE",           "INS Alignment Coarse"          },
            {"wMsgJ_STRT_INS_NOW",              "INS Alignment Now"             },

            {"wMsgJ_STRT_CHECK",                "Confirm Checkpoint"            },
            {"wMsgJ_STRT_LOUD_CLR",             "Confirm ICS test"              },
            {"wMsgJ_STRT_PAUSE",                "Pause"                         },
            {"wMsgJ_STRT_STARTUP",              "Perform Start Procedure"       },
            {"wMsgJ_STRT_ASSISTED",             "Assisted Start Procedure"      },
            {"wMsgJ_SDWN_SHUTDOWN",             "Postflight Shutdown"           },

            // 600-700 AI pilot
            {"wMsgI_ALT",                       "Go Altitude"                           },
            {"wMsgI_ALT_ANG_1",                 "Go Altitude 1.000ft"                   },
            {"wMsgI_ALT_ANG_5",                 "Go Altitude 5.000ft"                   },
            {"wMsgI_ALT_ANG_10",                "Go Altitude 10.000ft"                  },
            {"wMsgI_ALT_ANG_15",                "Go Altitude 15.000ft"                  },
            {"wMsgI_ALT_ANG_20",                "Go Altitude 20.000ft"                  },
            {"wMsgI_ALT_ANG_25",                "Go Altitude 25.000ft"                  },
            {"wMsgI_ALT_ANG_30",                "Go Altitude 30.000ft"                  },
            {"wMsgI_ALT_ANG_35",                "Go Altitude 35.000ft"                  },
            {"wMsgI_ALT_CHG",                   "Change Altitude"               },
            {"wMsgI_ALT_DESC_10K",              "Descent 10000"                 },
            {"wMsgI_ALT_DESC_5K",               "Descent 5000"                  },
            {"wMsgI_ALT_DESC_1K",               "Descent 1000"                  },
            {"wMsgI_ALT_DESC_500",              "Descent 500"                   },
            {"wMsgI_ALT_CLMB_500",              "Climb 500"                     },
            {"wMsgI_ALT_CLMB_1K",               "Climb 1000"                    },
            {"wMsgI_ALT_CLMB_5K",               "Climb 5000"                    },
            {"wMsgI_ALT_CLMB_10K",              "Climb 10000"                   },
            {"wMsgI_SPD_MINUS_200",             "Decelerate 200kts"             },
            {"wMsgI_SPD_MINUS_100",             "Decelerate 100kts"             },
            {"wMsgI_SPD_MINUS_50",              "Decelerate 50kts"              },
            {"wMsgI_SPD_PLUS_50",               "Accelerate 50kts"              },
            {"wMsgI_SPD_PLUS_100",              "Accelerate 100kts"             },
            {"wMsgI_SPD_PLUS_200",              "Accelerate 200kts"             },
            {"wMsgI_DIR_N",                     "Head North"                    },
            {"wMsgI_DIR_NE",                    "Head NorthEast"                },
            {"wMsgI_DIR_E",                     "Head East"                     },
            {"wMsgI_DIR_SE",                    "Head SouthEast"                },
            {"wMsgI_DIR_S",                     "Head South"                    },
            {"wMsgI_DIR_SW",                    "Head SouthWest"                },
            {"wMsgI_DIR_W",                     "Head West"                     },
            {"wMsgI_DIR_NW",                    "Head NorthWest"                },
            {"wMsgI_DIR",                       "Set Heading"                   },
            {"wMsgI_DIR_CHG_L45",               "Turn Left 45"                  },
            {"wMsgI_DIR_CHG_L30",               "Turn Left 30"                  },
            {"wMsgI_DIR_CHG_L10",               "Turn Left 10"                  },
            {"wMsgI_DIR_CHG_L5",                "Turn Left 5"                   },
            {"wMsgI_DIR_CHG_R5",                "Turn Right 5"                  },
            {"wMsgI_DIR_CHG_R10",               "Turn Right 10"                 },
            {"wMsgI_DIR_CHG_R30",               "Turn Right 30"                 },
            {"wMsgI_DIR_CHG_R45",               "Turn Right 45"                 },
            {"wMsgI_DIR_HOLD_CRS",              "Maintain Course"               },
            {"wMsgI_SPD",                       "Change Speed"                  },
            {"wMsgI_DIR_CHG",                   "Change Direction"              },

            {"wMsgI_SPT_FLYTO",                 "Fly to Destination"                  },
            {"wMsgI_SPT_ORBIT",                 "Orbit Destination"                },
            //{"wMsgPlaceHolder646",              "PlaceHolder646"                },
            //{"wMsgPlaceHolder647",              "PlaceHolder647"                },
            //{"wMsgPlaceHolder648",              "PlaceHolder648"                },
            //{"wMsgPlaceHolder649",              "PlaceHolder649"                },
            //{"wMsgPlaceHolder650",              "PlaceHolder650"                },
            //{"wMsgPlaceHolder651",              "PlaceHolder651"                },
            //{"wMsgPlaceHolder652",              "PlaceHolder652"                },
            //{"wMsgPlaceHolder653",              "PlaceHolder653"                },
            //{"wMsgPlaceHolder654",              "PlaceHolder654"                },
            //{"wMsgPlaceHolder655",              "PlaceHolder655"                },
            //{"wMsgPlaceHolder656",              "PlaceHolder656"                },
            //{"wMsgPlaceHolder657",              "PlaceHolder657"                },
            //{"wMsgPlaceHolder658",              "PlaceHolder658"                },
            //{"wMsgPlaceHolder659",              "PlaceHolder659"                },
            //{"wMsgPlaceHolder660",              "PlaceHolder660"                },
            //{"wMsgPlaceHolder661",              "PlaceHolder661"                },
            //{"wMsgPlaceHolder662",              "PlaceHolder662"                },
            //{"wMsgPlaceHolder663",              "PlaceHolder663"                },
            //{"wMsgPlaceHolder664",              "PlaceHolder664"                },
            //{"wMsgPlaceHolder665",              "PlaceHolder665"                },
            //{"wMsgPlaceHolder666",              "PlaceHolder666"                },
            //{"wMsgPlaceHolder667",              "PlaceHolder667"                },
            //{"wMsgPlaceHolder668",              "PlaceHolder668"                },
            //{"wMsgPlaceHolder669",              "PlaceHolder669"                },
            //{"wMsgPlaceHolder670",              "PlaceHolder670"                },
            //{"wMsgPlaceHolder671",              "PlaceHolder671"                },
            //{"wMsgPlaceHolder672",              "PlaceHolder672"                },
            //{"wMsgPlaceHolder673",              "PlaceHolder673"                },
            //{"wMsgPlaceHolder674",              "PlaceHolder674"                },
            //{"wMsgPlaceHolder675",              "PlaceHolder675"                },
            //{"wMsgPlaceHolder676",              "PlaceHolder676"                },
            //{"wMsgPlaceHolder677",              "PlaceHolder677"                },
            //{"wMsgPlaceHolder678",              "PlaceHolder678"                },
            //{"wMsgPlaceHolder679",              "PlaceHolder679"                },
            //{"wMsgPlaceHolder680",              "PlaceHolder680"                },
            //{"wMsgPlaceHolder681",              "PlaceHolder681"                },
            //{"wMsgPlaceHolder682",              "PlaceHolder682"                },
            //{"wMsgPlaceHolder683",              "PlaceHolder683"                },
            //{"wMsgPlaceHolder684",              "PlaceHolder684"                },
            //{"wMsgPlaceHolder685",              "PlaceHolder685"                },
            //{"wMsgPlaceHolder686",              "PlaceHolder686"                },
            //{"wMsgPlaceHolder687",              "PlaceHolder687"                },
            //{"wMsgPlaceHolder688",              "PlaceHolder688"                },
            //{"wMsgPlaceHolder689",              "PlaceHolder689"                },
            // end of AI pilot

            // spare table
            //{"wMsgPlaceHolder690",              "PlaceHolder690"                },
            //{"wMsgPlaceHolder691",              "PlaceHolder691"                },
            //{"wMsgPlaceHolder692",              "PlaceHolder692"                },
            //{"wMsgPlaceHolder693",              "PlaceHolder693"                },
            //{"wMsgPlaceHolder694",              "PlaceHolder694"                },
            //{"wMsgPlaceHolder695",              "PlaceHolder695"                },
            //{"wMsgPlaceHolder696",              "PlaceHolder696"                },
            //{"wMsgPlaceHolder697",              "PlaceHolder697"                },
            //{"wMsgPlaceHolder698",              "PlaceHolder698"                },
            //{"wMsgPlaceHolder699",              "PlaceHolder699"                },


        };

    }


}
