using System;
using System.Collections.Generic;

namespace VAICOM.Extensions.RIO
{

    public static partial class Aliases
    {
        // aliases for recipients (get added to recipient aliases)
        public static Dictionary<string, string> airecipients = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "RIO",                        "RIO"                               },
            { "Jester",                     "RIO"                               },
            { "Ice",                        "Iceman"                            },
        };

        // command Aliases (these get added to command aliases on import)
        public static Dictionary<string, string> aicommands = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            //example
            { "Open Canopy",                "wMsgJ_CANOPY_OPEN"                     },
            { "Close Canopy",               "wMsgJ_CANOPY_CLOSE"                    },

            // alias                        // command unique internal name

            // block: menu control
            {"Toggle Menu",                 "wMsgJ_MENU_TOGGLE"                 }, // block disabled: aliases not used
            {"Do Option 1",                 "wMsgJ_MENU_OPTION_1"               },
            {"Do Option 2",                 "wMsgJ_MENU_OPTION_2"               },
            {"Do Option 3",                 "wMsgJ_MENU_OPTION_3"               },
            {"Do Option 4",                 "wMsgJ_MENU_OPTION_4"               },
            {"Do Option 5",                 "wMsgJ_MENU_OPTION_5"               },
            {"Do Option 6",                 "wMsgJ_MENU_OPTION_6"               },
            {"Do Option 7",                 "wMsgJ_MENU_OPTION_7"               },
            {"Do Option 8",                 "wMsgJ_MENU_OPTION_8"               },
            {"Do Menu Down",                "wMsgJ_MENU_DIR_D"                  },
            {"Do Menu Down Left",           "wMsgJ_MENU_DIR_DL"                 },
            {"Do Menu Down Right",          "wMsgJ_MENU_DIR_DR"                 },
            {"Do Menu Left",                "wMsgJ_MENU_DIR_L"                  },
            {"Do Menu Right",               "wMsgJ_MENU_DIR_R"                  },
            {"Do Menu Up",                  "wMsgJ_MENU_DIR_U"                  },
            {"Do Menu Up Left",             "wMsgJ_MENU_DIR_UL"                 },
            {"Do Menu Up Right",            "wMsgJ_MENU_DIR_UR"                 },
            {"Open Menu",                   "wMsgJ_MENU_OPEN"                   },
            {"Close Menu",                  "wMsgJ_MENU_CLOSE"                  },
            {"Open Context Menu",           "wMsgJ_MENU_CONTEXT"                },
            {"Open Main Menu",              "wMsgJ_MENU_MAIN"                   },
            {"Close Context Menu",          "wMsgJ_MENU_CTXT_CLOSE"             },
            {"Close Main Menu",             "wMsgJ_MENU_MAIN_CLOSE"             },

            {"Don't Auto Designate",        "wMsgJESTER_LANTIRN_inhibit_auto_designate"         },
            {"Track Point",                 "wMsgJESTER_LANTIRN_track_target_id"                }, //na
            {"Track Area",                  "wMsgJESTER_LANTIRN_track_zone_id"                  }, //na
            {"Designate",                   "wMsgJESTER_LANTIRN_designate"                      },
            //{"Laser 100",                   "wMsgKNEEBOARD_Laser100"                            },
            //{"Laser 10",                    "wMsgKNEEBOARD_Laser10"                             },
            //{"Laser 1",                     "wMsgKNEEBOARD_Laser1"                              },
            // end of menu control

            // spare block
            {"Black Hot",                   "wMsgLANTIRN_ToggleWHOTBHOT"                }, // TESTED OK
            {"White Hot",                   "wMsgLANTIRN_ToggleWHOTBHOT"                }, // TESTED OK
            {"Latch Laser",                 "wMsgLANTIRN_LaserLatched"                  }, // TESTED OK
            {"Arm Laser",                   "wMsgLANTIRN_Laser_ARM"                     }, // TESTED OK
            {"Toggle Laser",                "wMsgLANTIRN_Laser_ARM_Toggle"              }, // TESTED OK
            {"Undesignate",                 "wMsgLANTIRN_Undesignate"                   }, // TESTED OK
            {"Track Boresight",             "wMsgLANTIRN_QHUD_QADL"                     }, // TESTED OK
            {"Snow Plow",                   "wMsgLANTIRN_QSNO"                          }, // TESTED OK
            {"Track Designation",           "wMsgLANTIRN_QDES"                          },
            {"Previous Target",             "wMsgLANTIRN_QWP_Minus"                     }, // not used
            {"Next Target",                 "wMsgLANTIRN_QWP_Plus"                      },

            // block: radar (DONE)

            {"Go BVR",                      "wMsgJ_RDR_BVR"                     },// disabled
            {"Go Active",                   "wMsgJ_RDR_GO_ACTIVE"               },
            {"Go Standby",                  "wMsgJ_RDR_GO_SILENT"               },
            {"Radar Spot",                  "wMsgJ_RDR_SPOT"                    }, //na
            {"Break Lock",                  "wMsgJ_RDR_BREAK_LOCK"              },
            {"Go To PSTT",                  "wMsgJ_RDR_TO_PSTT"                 },// disabled

            {"Switch STT",                  "wMsgJ_RDR_TOGGLE_STT"              },
            {"VSL High",                    "wMsgJ_RDR_VSL_HIGH"                },
            {"VSL Low",                     "wMsgJ_RDR_VSL_LOW"                 },

            {"Track Single",                "wMsgJ_RDR_STT_LOCK"                }, // show hint
            {"Track Single Contact Ahead",  "wMsgJ_RDR_STT_TGT_AHEAD"           },
            {"Track Single Bogey Ahead",    "wMsgJ_RDR_STT_ENMY_TGT_AHEAD"      },
            {"Track Single Friendly Ahead", "wMsgJ_RDR_STT_FRNDLY_TGT_AHEAD"    },
            {"Track Single First",          "wMsgJ_RDR_STT_FIRST_TWS_TGT"       },
            {"Track Single Target",         "wMsgJ_RDR_STT_TWS_TGT_NUM"         }, // not endpoint //
            {"Track Single Target 1",       "wMsgJ_RDR_STT_TWS_TGT_1"           },
            {"Track Single Target 2",       "wMsgJ_RDR_STT_TWS_TGT_2"           },
            {"Track Single Target 3",       "wMsgJ_RDR_STT_TWS_TGT_3"           },
            {"Track Single Target 4",       "wMsgJ_RDR_STT_TWS_TGT_4"           },
            {"Track Single Target 5",       "wMsgJ_RDR_STT_TWS_TGT_5"           },
            {"Track Single Target 6",       "wMsgJ_RDR_STT_TWS_TGT_6"           },
            {"Track Single Target 7",       "wMsgJ_RDR_STT_TWS_TGT_7"           },
            {"Track Single Target 8",       "wMsgJ_RDR_STT_TWS_TGT_8"           },
            {"Track Single Target X",       "wMsgJ_RDR_STT_CHOOSE_SPECIFIC_TGT" }, // disabled //

            {"Scan Range",                  "wMsgJ_RDR_SCAN_DIST"               },// not endpoint //
            {"Scan Range Auto",             "wMsgJ_RDR_RNG_AUTO"                },// for Range
            {"Scan Range 25",               "wMsgJ_RDR_RNG_25"                  },
            {"Scan Range 50",               "wMsgJ_RDR_RNG_50"                  },
            {"Scan Range 100",              "wMsgJ_RDR_RNG_100"                 },
            {"Scan Range 200",              "wMsgJ_RDR_RNG_200"                 },
            {"Scan Range 400",              "wMsgJ_RDR_RNG_400"                 },

            {"Scan Azimuth",                "wMsgJ_RDR_SCAN_AZ"                 },// not endpoint //
            {"Scan Azimuth Auto",           "wMsgJ_RDR_POS"                     },// for Azimuth
            {"Scan Center Left",            "wMsgJ_RDR_POS_CTR_L"               },
            {"Scan Center Right",           "wMsgJ_RDR_POS_CTR_R"               },
            {"Scan Left",                   "wMsgJ_RDR_POS_L"                   },
            {"Scan Right",                  "wMsgJ_RDR_POS_R"                   },
            {"Scan Left 20",                "wMsgJ_RDR_POS_L20"                 },
            {"Scan Right 20",               "wMsgJ_RDR_POS_R20"                 },
            {"Scan Hard Left",              "wMsgJ_RDR_POS_L55"                 },
            {"Scan Hard Right",             "wMsgJ_RDR_POS_R55"                 },

            {"Scan Elevation",              "wMsgJ_RDR_SCAN_ELEV"               },// not endpoint, hint
            {"Scan Elevation Auto",         "wMsgJ_RDR_AUTO"                    }, // for Elevation
            {"Scan Close Low",              "wMsgJ_RDR_ELEV_CLOSE_LOW"          },
            {"Scan Close Level Low",        "wMsgJ_RDR_ELEV_CLOSE_MID_LO"       },
            {"Scan Close Level",            "wMsgJ_RDR_ELEV_CLOSE_MID"          },
            {"Scan Close Level High",       "wMsgJ_RDR_ELEV_CLOSE_MID_HI"       },
            {"Scan Close High",             "wMsgJ_RDR_ELEV_CLOSE_HI"           },
            {"Scan Low",                    "wMsgJ_RDR_ELEV_MID_LOW"            },
            {"Scan Level Low",              "wMsgJ_RDR_ELEV_MID_MID_LO"         },
            {"Scan Level",                  "wMsgJ_RDR_ELEV_MID_MID"            },
            {"Scan Level High",             "wMsgJ_RDR_ELEV_MID_MID_HI"         },
            {"Scan High",                   "wMsgJ_RDR_ELEV_MID_HI"             },
            {"Scan Long Low",               "wMsgJ_RDR_ELEV_LONG_LOW"           },
            {"Scan Long Level Low",         "wMsgJ_RDR_ELEV_LONG_MID_LO"        },
            {"Scan Long Level",             "wMsgJ_RDR_ELEV_LONG_MID"           },
            {"Scan Long Level High",        "wMsgJ_RDR_ELEV_LONG_MID_HI"        },
            {"Scan Long High",              "wMsgJ_RDR_ELEV_LONG_HI"            },

            {"Radar Mode",                  "wMsgJ_RDR_MODE"                    }, // Not Endpoint
            {"Radar Mode Automatic",        "wMsgJ_RDR_MODE_AUTO"               }, //Modes
            {"Radar Mode TWS",              "wMsgJ_RDR_MODE_TWS"                },
            {"Radar Mode RWS",              "wMsgJ_RDR_MODE_RWS"                },         
            {"Radar Mode TWS Manual",       "wMsgJ_RDR_MODE_TWS_MAN"            },
            {"Look For A regular Target",   "wMsgJ_RDR_MODE_SIZE_M"             },
            {"Look For A Big Target",       "wMsgJ_RDR_MODE_SIZE_L"             },
            {"Look For A Small Target",     "wMsgJ_RDR_MODE_SIZE_S"             },
            // end of radar

            // LANTIRN
            {"Stabilize",                   "wMsgJ_RDR_STAB"                    },
            {"Stabilize 15 Seconds",        "wMsgJ_RDR_STAB_15"                 },
            {"Stabilize 30 Seconds",        "wMsgJ_RDR_STAB_30"                 },
            {"Stabilize 1 Minute",          "wMsgJ_RDR_STAB_60"                 },
            {"Stabilize 2 Minutes",         "wMsgJ_RDR_STAB_120"                },
            {"Stabilize Hold",              "wMsgJ_RDR_STAB_INDEF"              },
            {"Area Track",                  "wMsgJ_RDR_STAB_GROUND"             },

            {"Eyeballs",                    "wMsgLANTIRN_Head_Eyeball"         },
            {"Head Control",                 "wMsgLANTIRN_Head_Head"                },
            //{"PlaceHolder098",              "wMsgPlaceHolder098"                },
            //{"PlaceHolder099",              "wMsgPlaceHolder099"                },


            // block: weapons (DONE)
            // section: AG
            {"Select Stores",               "wMsgJ_WPN_AG_SORDN"                },
            {"Select Zunis",                "wMsgJ_WPN_AG_SORDN_WPN_1"          },
            {"Select Mark Eightyfours",     "wMsgJ_WPN_AG_SORDN_WPN_2"          },
            {"Select Mark Eightythrees",    "wMsgJ_WPN_AG_SORDN_WPN_3"          },
            {"Select Mark Eightytwos",      "wMsgJ_WPN_AG_SORDN_WPN_4"          },
            {"Select Mark Eightyones",      "wMsgJ_WPN_AG_SORDN_WPN_5"          },
            {"Select Rockeyes",             "wMsgJ_WPN_AG_SORDN_WPN_6"          },
            {"Select Lose",                 "wMsgJ_WPN_AG_SORDN_WPN_7"          },
            {"Select Paveways",             "wMsgJ_WPN_AG_SORDN_WPN_8"          },
            {"Select Bee Dee Yous",         "wMsgJ_WPN_AG_SORDN_WPN_9"          },
            {"Select TALD",                 "wMsgJ_WPN_AG_SORDN_WPN_10"         },

            {"AG Weapon Spot",              "wMsgJ_WPN_AG_SPOT"                 }, // not used

            {"Attack Mode Target",          "wMsgJ_WPN_AG_SET_COMP_TGT"         },
            {"Attack Mode Pilot",           "wMsgJ_WPN_AG_SET_COMP_PILOT"       },

            {"Set Release Single",          "wMsgJ_WPN_AG_SET_SNGL"             },
            {"Set Release Pairs",           "wMsgJ_WPN_AG_SET_PAIRS"            },

            {"Set Fuse",                    "wMsgJ_WPN_AG_SETFUSE"              }, // not endpoint
            {"Set Fuse Nose Tail",          "wMsgJ_WPN_AG_SETFUSE_NOSETAIL"     },
            {"Set Fuse Nose",               "wMsgJ_WPN_AG_SETFUSE_NOSE"         },
            {"Set Fuse Safe",               "wMsgJ_WPN_AG_SETFUSE_SAFE"         },

            {"Set Ripple",                  "wMsgJ_WPN_AG_RIP"                  }, // not endpoint, show hint

            {"Set Ripple Quantity",         "wMsgJ_WPN_AG_RIP_QTY"              }, // not endpoint
            {"Set Ripple Quantity Step",    "wMsgJ_WPN_AG_RIP_QTY_STEP"         },
            {"Set Ripple Quantity 2",       "wMsgJ_WPN_AG_RIP_QTY_2"            }, //2
            {"Set Ripple Quantity 3",       "wMsgJ_WPN_AG_RIP_QTY_5"            }, //3
            {"Set Ripple Quantity 4",       "wMsgJ_WPN_AG_RIP_QTY_10"           }, //4
            {"Set Ripple Quantity 6",       "wMsgJ_WPN_AG_RIP_QTY_20"           }, //6
            {"Set Ripple Quantity 8",       "wMsgJ_WPN_AG_RIP_QTY_30"           }, //8

            {"Set Ripple Time",             "wMsgJ_WPN_AG_RIP_TIME"             }, // not endpoint
            {"Set Ripple Time Step",        "wMsgJ_WPN_AG_RIP_TIME_STEP"        },
            {"Set Ripple Time 10",          "wMsgJ_WPN_AG_RIP_TIME_10"          },
            {"Set Ripple Time 20",          "wMsgJ_WPN_AG_RIP_TIME_20"          },
            {"Set Ripple Time 50",          "wMsgJ_WPN_AG_RIP_TIME_50"          },
            {"Set Ripple Time 100",         "wMsgJ_WPN_AG_RIP_TIME_100"         },
            {"Set Ripple Time 200",         "wMsgJ_WPN_AG_RIP_TIME_200"         },
            {"Set Ripple Time 500",         "wMsgJ_WPN_AG_RIP_TIME_500"         },
            {"Set Ripple Time 990",         "wMsgJ_WPN_AG_RIP_TIME_990"         },

            {"Set Ripple Distance",         "wMsgJ_WPN_AG_RIP_DIST"             }, // not endpoint
            {"Set Ripple Distance Step",    "wMsgJ_WPN_AG_RIP_DIST_STEP"        }, // not used
            {"Set Ripple Distance 5",       "wMsgJ_WPN_AG_RIP_DIST_5"           },
            {"Set Ripple Distance 10",      "wMsgJ_WPN_AG_RIP_DIST_10"          },
            {"Set Ripple Distance 25",      "wMsgJ_WPN_AG_RIP_DIST_25"          },
            {"Set Ripple Distance 50",      "wMsgJ_WPN_AG_RIP_DIST_50"          },
            {"Set Ripple Distance 100",     "wMsgJ_WPN_AG_RIP_DIST_100"         },
            {"Set Ripple Distance 200",     "wMsgJ_WPN_AG_RIP_DIST_200"         },
            {"Set Ripple Distance 400",     "wMsgJ_WPN_AG_RIP_DIST_400"         },

            {"Drop Weapons",                "wMsgJ_WPN_AG_JETT"                 }, //Jettison
            {"Drop Tanks",                  "wMsgJ_WPN_AG_DROP_TANKS"           },

            {"Switch Lantern",              "wMsgJ_WPN_AG_UTIL_LANTIRN"         },

            {"Go Air to Ground",            "wMsgJ_WPN_AG"                      }, // not use
            {"Go Air to Air",               "wMsgJ_WPN_AA"                      }, // not used

            {"Select Stations",             "wMsgJ_WPN_AG_STN"                  },
            {"Select Stations 1 8",         "wMsgJ_WPN_AG_STN_18"               },
            {"Select Stations 2 7",         "wMsgJ_WPN_AG_STN_27"               },
            {"Select Stations 3 4 5 6",     "wMsgJ_WPN_AG_STN_3456"             },
            {"Select Stations 3 6",         "wMsgJ_WPN_AG_STN_36"               },
            {"Select Stations 4 5",         "wMsgJ_WPN_AG_STN_45"               },

            {"Send Waypoint to GGW",        "wMsgJ_WPN_AG_JDAM_WPT_TO_GGW"      },
            {"Select GGW Pre Planned",      "wMsgJ_WPN_AG_JDAM_SEL_PP"          },
            {"Send Pre Planned to all Stations", "wMsgJ_WPN_AG_JDAM_PP_ALL_STN" },
            {"Send Designation to Station", "wMsgJ_WPN_AG_JDAM_DESIG_TO_STN"    },

            //{"PlaceHolder155",              "wMsgPlaceHolder155"                },
            //{"PlaceHolder156",              "wMsgPlaceHolder156"                },
            //{"PlaceHolder157",              "wMsgPlaceHolder157"                },
            //{"PlaceHolder158",              "wMsgPlaceHolder158"                },
            //{"PlaceHolder159",              "wMsgPlaceHolder159"                },
            //{"PlaceHolder160",              "wMsgPlaceHolder160"                },
            //{"PlaceHolder161",              "wMsgPlaceHolder161"                },
            //{"PlaceHolder162",              "wMsgPlaceHolder162"                },
            //{"PlaceHolder163",              "wMsgPlaceHolder163"                },
            //{"PlaceHolder164",              "wMsgPlaceHolder164"                },
            //{"PlaceHolder165",              "wMsgPlaceHolder165"                },
            //{"PlaceHolder166",              "wMsgPlaceHolder166"                },
            //{"PlaceHolder167",              "wMsgPlaceHolder167"                },
            //{"PlaceHolder168",              "wMsgPlaceHolder168"                },
            //{"PlaceHolder169",              "wMsgPlaceHolder169"                },
            //{"PlaceHolder170",              "wMsgPlaceHolder170"                },
            //{"PlaceHolder171",              "wMsgPlaceHolder171"                },
            //{"PlaceHolder172",              "wMsgPlaceHolder172"                },
            //{"PlaceHolder173",              "wMsgPlaceHolder173"                },
            //{"PlaceHolder174",              "wMsgPlaceHolder174"                },
            //{"PlaceHolder175",              "wMsgPlaceHolder175"                },
            //{"PlaceHolder176",              "wMsgPlaceHolder176"                },
            //{"PlaceHolder177",              "wMsgPlaceHolder177"                },
            //{"PlaceHolder178",              "wMsgPlaceHolder178"                },
            //{"PlaceHolder179",              "wMsgPlaceHolder179"                },
            //{"PlaceHolder180",              "wMsgPlaceHolder180"                },
            //{"PlaceHolder181",              "wMsgPlaceHolder181"                },
            //{"PlaceHolder182",              "wMsgPlaceHolder182"                },
            //{"PlaceHolder183",              "wMsgPlaceHolder183"                },
            //{"PlaceHolder184",              "wMsgPlaceHolder184"                },
            //{"PlaceHolder185",              "wMsgPlaceHolder185"                },
            //{"PlaceHolder186",              "wMsgPlaceHolder186"                },
            //{"PlaceHolder187",              "wMsgPlaceHolder187"                },
            {"Set Ripple Quantity 16",      "wMsgJ_WPN_AG_RIP_QTY_16"           },
            {"Set Ripple Quantity 28",      "wMsgJ_WPN_AG_RIP_QTY_28"           },
            // end of weapons

            // spare table

            //{"PlaceHolder194",              "wMsgPlaceHolder194"                },
            //{"PlaceHolder195",              "wMsgPlaceHolder195"                },
            //{"PlaceHolder196",              "wMsgPlaceHolder196"                },
            //{"PlaceHolder197",              "wMsgPlaceHolder197"                },
            //{"PlaceHolder198",              "wMsgPlaceHolder198"                },
            //{"PlaceHolder199",              "wMsgPlaceHolder199"                },

            // block: radio
            {"Radio 1",                     "wMsgJ_RAD_159"                     },
            {"Radio 1 Use Guard",           "wMsgJ_RAD_159_USE_GUARD"           },
            {"Radio 1 Use Manual",          "wMsgJ_RAD_159_USE_MANUAL"          },
            {"Radio 1 Use Channel",         "wMsgJ_RAD_159_USE_CHAN"            },
            {"Radio 1 Use Channel 1",       "wMsgJ_RAD_159_USE_CHAN_1"          },
            {"Radio 1 Use Channel 2",       "wMsgJ_RAD_159_USE_CHAN_2"          },
            {"Radio 1 Use Channel 3",       "wMsgJ_RAD_159_USE_CHAN_3"          },
            {"Radio 1 Use Channel 4",       "wMsgJ_RAD_159_USE_CHAN_4"          },
            {"Radio 1 Use Channel 5",       "wMsgJ_RAD_159_USE_CHAN_5"          },
            {"Radio 1 Use Channel 6",       "wMsgJ_RAD_159_USE_CHAN_6"          },
            {"Radio 1 Use Channel 7",       "wMsgJ_RAD_159_USE_CHAN_7"          },
            {"Radio 1 Use Channel 8",       "wMsgJ_RAD_159_USE_CHAN_8"          },
            {"Radio 1 Tune Manual",         "wMsgJ_RAD_159_TUNE_MAN"            },
            {"Radio 1 Select Channel",      "wMsgJ_RAD_159_SELECT_CHAN"         },
            {"Radio 1 Select Mode",         "wMsgJ_RAD_159_SELECT_MODE"         },
            {"Radio 1 Tune ATC",            "wMsgJ_RAD_159_TUNE_ATC"            },
            {"Radio 1 Tune TAC",            "wMsgJ_RAD_159_TUNE_TAC"            },
            {"Radio 2",                     "wMsgJ_RAD_182"                     },
            {"Radio Use Guard",             "wMsgJ_RAD_182_USE_GUARD"           },
            {"Radio 2 Use Manual",          "wMsgJ_RAD_182_USE_MANUAL"          },
            {"Radio Use Channel",         "wMsgJ_RAD_182_USE_CHAN"            },
            {"Radio Use Channel 1",       "wMsgJ_RAD_182_USE_CHAN_1"          },
            {"Radio Use Channel 2",       "wMsgJ_RAD_182_USE_CHAN_2"          },
            {"Radio Use Channel 3",       "wMsgJ_RAD_182_USE_CHAN_3"          },
            {"Radio Use Channel 4",       "wMsgJ_RAD_182_USE_CHAN_4"          },
            {"Radio Use Channel 5",       "wMsgJ_RAD_182_USE_CHAN_5"          },
            {"Radio Use Channel 6",       "wMsgJ_RAD_182_USE_CHAN_6"          },
            {"Radio Use Channel 7",       "wMsgJ_RAD_182_USE_CHAN_7"          },
            {"Radio Use Channel 8",       "wMsgJ_RAD_182_USE_CHAN_8"          },
            {"Radio 2 Tune Manual",         "wMsgJ_RAD_182_TUNE_MAN"            },
            {"Radio 2 Select Channel",      "wMsgJ_RAD_182_SELECT_CHAN"         },
            {"Radio 2 Select Mode",         "wMsgJ_RAD_182_SELECT_MODE"         },
            {"Radio 2 Tune ATC",            "wMsgJ_RAD_182_TUNE_ATC"            },
            {"Radio 2 Tune TAC",            "wMsgJ_RAD_182_TUNE_TAC"            },

            // Datalink
            {"Link",                        "wMsgJ_RAD_DL"                      },
            {"Link Mode",                   "wMsgJ_RAD_DL_SET_MODE"             },
            {"Link Preset",                 "wMsgJ_RAD_DL_SET_FREQ_PRST"        },
            {"Link Host",                   "wMsgJ_RAD_DL_SET_HOST"             },
            {"Link Mode Tactical",          "wMsgJ_RAD_DL_TACT"                 },
            {"Link Mode Off",               "wMsgJ_RAD_DL_OFF"                  },
            {"Link Mode Fighter",           "wMsgJ_RAD_DL_FGHT"                 },

            {"Link Host Stennis",           "wMsgJ_RAD_DL_HOST_STEN"            },
            {"Link Host Darkstar",          "wMsgJ_RAD_DL_HOST_DARK"            },
            {"Link Host Focus",             "wMsgJ_RAD_DL_HOST_FOCS"            },
            {"Link Host Magic",             "wMsgJ_RAD_DL_HOST_MAGC"            },
            {"Link Host Overlord",          "wMsgJ_RAD_DL_HOST_OVRL"            },
            {"Link Host Wizard",            "wMsgJ_RAD_DL_HOST_WIZR"            },

            {"Link Decimal 0",                "wMsgJ_RAD_DL_SET_FREQ_1"           },
            {"Link Decimal 10",               "wMsgJ_RAD_DL_SET_FREQ_2"           },
            {"Link Decimal 20",               "wMsgJ_RAD_DL_SET_FREQ_3"           },
            {"Link Decimal 30",               "wMsgJ_RAD_DL_SET_FREQ_4"           },
            {"Link Decimal 40",               "wMsgJ_RAD_DL_SET_FREQ_5"           },
            {"Link Decimal 50",               "wMsgJ_RAD_DL_SET_FREQ_6"           },
            {"Link Decimal 60",               "wMsgJ_RAD_DL_SET_FREQ_7"           },
            {"Link Decimal 70",               "wMsgJ_RAD_DL_SET_FREQ_8"           },

            //TACAN 
            {"TACAN",                       "wMsgJ_RAD_TCN"                     }, //disabled
            {"TACAN Mode",                  "wMsgJ_RAD_TCN_MODE"                },
            {"TACAN Mode Off",              "wMsgJ_RAD_TCN_MODE_OFF"            },
            {"TACAN Mode Rec",              "wMsgJ_RAD_TCN_MODE_REC"            },
            {"TACAN Mode Transmit",         "wMsgJ_RAD_TCN_MODE_TR"             },
            {"TACAN Mode AA",               "wMsgJ_RAD_TCN_MODE_AA"             },
            {"TACAN Mode Beacon",           "wMsgJ_RAD_TCN_MODE_BCN"            },
            {"TACAN Select Channel",        "wMsgJ_RAD_TCN_SEL_CHAN"            },
            {"TACAN Ground Station",        "wMsgJ_RAD_TCN_SEL_GND_STN"         },
            {"TACAN Tune",                  "wMsgJ_RAD_TCN_TUNE_TAC"            },

            {"TACAN Tune Stennis",          "wMsgJ_RAD_TCN_TAC_STEN"                },
            {"TACAN Tune Arco",             "wMsgJ_RAD_TCN_TAC_ARCO"                },
            {"TACAN Tune Shell",            "wMsgJ_RAD_TCN_TAC_SHEL"                },
            {"TACAN Tune Texaco",           "wMsgJ_RAD_TCN_TAC_TEXA"                },

            {"TACAN Tango Sierra Kilo",         "wMsgJ_RAD_TCN_T_CS_TSK"                },
            {"TACAN Kilo Bravo Lima",           "wMsgJ_RAD_TCN_T_CS_KBL"                },
            {"TACAN Bravo Tango Mike",          "wMsgJ_RAD_TCN_T_CS_BTM"                },
            {"TACAN Kilo Tango Sierra",         "wMsgJ_RAD_TCN_T_CS_KTS"                },
            {"TACAN Golf Tango Bravo",          "wMsgJ_RAD_TCN_T_CS_GTB"                },
            {"TACAN Victor Alfa Sierra",        "wMsgJ_RAD_TCN_T_CS_VAS"                },
            {"TACAN Lima Sierra Victor",        "wMsgJ_RAD_TCN_T_NV_LSV"                },
            {"TACAN Lima Alfa Sierra",          "wMsgJ_RAD_TCN_T_NV_LAS"                },
            {"TACAN Bravo Lima Delta",          "wMsgJ_RAD_TCN_T_NV_BLD"                },
            {"TACAN India November Sierra",     "wMsgJ_RAD_TCN_T_NV_INS"                },
            {"TACAN Mike Mike Mike",            "wMsgJ_RAD_TCN_T_NV_MMM"                },
            {"TACAN Golf Foxtrot Sierra",       "wMsgJ_RAD_TCN_T_NV_GFS"                },
            {"TACAN Golf Romeo Lima",           "wMsgJ_RAD_TCN_T_NV_GRL"                },
            {"TACAN Papa Golf Sierra",          "wMsgJ_RAD_TCN_T_NV_PGS"                },
            {"TACAN Bravo Tango Yankee",        "wMsgJ_RAD_TCN_T_NV_BTY"                },
            {"TACAN Echo Echo Delta",           "wMsgJ_RAD_TCN_T_NV_EED"                },
            {"TACAN Delta Alfa Golf",           "wMsgJ_RAD_TCN_T_NV_DAG"                },
            {"TACAN Hotel Echo Charlie",        "wMsgJ_RAD_TCN_T_NV_HEC"                },
            {"TACAN Oscar Alfa Lima",           "wMsgJ_RAD_TCN_T_NV_OAL"                },
            {"TACAN Bravo India Hotel",         "wMsgJ_RAD_TCN_T_NV_BIH"                },
            {"TACAN Mike Victor Alfa",          "wMsgJ_RAD_TCN_T_NV_MVA"                },
            {"TACAN Kilo Charlie Kilo",         "wMsgJ_RAD_TCN_T_PG_KCK"                },
            {"TACAN Kilo Sierra Bravo",         "wMsgJ_RAD_TCN_T_PG_KSB"                },
            {"TACAN Hotel Delta Romeo",         "wMsgJ_RAD_TCN_T_PG_HDR"                },
            {"TACAN Mike Alfa",                 "wMsgJ_RAD_TCN_T_PG_MA"                 },
            {"TACAN Sierra Yankee Zulu",        "wMsgJ_RAD_TCN_T_PG_SYZI"               },
            {"TACAN Sierra Tango November",     "wMsgJ_RAD_TCN_T_STN"                   },

            {"Radio Mode TR",                   "wMsgJ_RAD_182_MODE_TR"                 },
            {"Radio Mode TRG",                  "wMsgJ_RAD_182_MODE_TRG"                },
            {"Radio Mode DF",                   "wMsgJ_RAD_182_MODE_DF"                 },
            {"Radio Mode Test",                 "wMsgJ_RAD_182_MODE_TEST"               },
            // end of radio

            // spare table
            {"Radio Mode AM",               "wMsgJ_RAD_182_MODE_AM"                     },
            {"Radio Mode FM",               "wMsgJ_RAD_182_MODE_FM"                     },
            {"Radio Mode",                  "wMsgJ_RAD_182_MODE"                        },
            {"Radio Mode Off",              "wMsgJ_RAD_182_MODE_OFF"                    },



            // block:utility NAV
            {"Utility Navigation",              "wMsgJ_UTIL_NAV"                }, //na
            {"Select Destination Steerpoint",   "wMsgJ_UTIL_NAV_SEL_DEST_SPT"   }, //na
            {"Restore Mission Point",           "wMsgJ_UTIL_NAV_REST_MSN_SPT"   }, //na
            {"Navigate Steerpoint Manual",      "wMsgJ_UTIL_NAV_MAN_ENT_SPT"    }, //na

            {"Navigate",                        "wMsgJ_UTIL_NAV_MAP_SPT"        }, // show hint
            {"Navigate Steerpoint 1",           "wMsgJ_UTIL_NAV_MAP_SPT_1"      },
            {"Navigate Steerpoint 2",           "wMsgJ_UTIL_NAV_MAP_SPT_2"      },
            {"Navigate Steerpoint 3",           "wMsgJ_UTIL_NAV_MAP_SPT_3"      },
            {"Navigate Steerpoint 4",           "wMsgJ_UTIL_NAV_MAP_SPT_4"      }, // 4-14 not used by F-14A/B 
            {"Navigate Steerpoint 5",           "wMsgJ_UTIL_NAV_MAP_SPT_5"      }, 
            {"Navigate Steerpoint 6",           "wMsgJ_UTIL_NAV_MAP_SPT_6"      }, 
            {"Navigate Steerpoint 7",           "wMsgJ_UTIL_NAV_MAP_SPT_7"      },
            {"Navigate Steerpoint 8",           "wMsgJ_UTIL_NAV_MAP_SPT_8"      },
            {"Navigate Steerpoint 9",           "wMsgJ_UTIL_NAV_MAP_SPT_9"      },
            {"Navigate Steerpoint 10",          "wMsgJ_UTIL_NAV_MAP_SPT_10"     },
            {"Navigate Steerpoint 11",          "wMsgJ_UTIL_NAV_MAP_SPT_11"     },
            {"Navigate Steerpoint 12",          "wMsgJ_UTIL_NAV_MAP_SPT_12"     },
            {"Navigate Steerpoint 13",          "wMsgJ_UTIL_NAV_MAP_SPT_13"     },
            {"Navigate Steerpoint 14",          "wMsgJ_UTIL_NAV_MAP_SPT_14"     },

            {"Navigate Fixed Point",            "wMsgJ_UTIL_NAV_MAP_FIX_PNT"    },
            {"Navigate Initial Point",          "wMsgJ_UTIL_NAV_MAP_INIT_PNT"   },
            {"Navigate Target",                 "wMsgJ_UTIL_NAV_SURF_TGT"       },
            {"Navigate Home Base",              "wMsgJ_UTIL_NAV_HOME_BASE"      },

            {"Direct Steerpoint 1",             "wMsgJ_UTIL_NAV_DIR_SPT_1"     }, //F-14B(U) only 1-14 for EGI navigation
            {"Direct Steerpoint 2",             "wMsgJ_UTIL_NAV_DIR_SPT_2"     },
            {"Direct Steerpoint 3",             "wMsgJ_UTIL_NAV_DIR_SPT_3"     },
            {"Direct Steerpoint 4",             "wMsgJ_UTIL_NAV_DIR_SPT_4"     },
            {"Direct Steerpoint 5",             "wMsgJ_UTIL_NAV_DIR_SPT_5"     },
            {"Direct Steerpoint 6",             "wMsgJ_UTIL_NAV_DIR_SPT_6"     },
            {"Direct Steerpoint 7",             "wMsgJ_UTIL_NAV_DIR_SPT_7"     },
            {"Direct Steerpoint 8",             "wMsgJ_UTIL_NAV_DIR_SPT_8"     },
            {"Direct Steerpoint 9",             "wMsgJ_UTIL_NAV_DIR_SPT_9"     },
            {"Direct Steerpoint 10",            "wMsgJ_UTIL_NAV_DIR_SPT_10"    },
            {"Direct Steerpoint 11",            "wMsgJ_UTIL_NAV_DIR_SPT_11"    },
            {"Direct Steerpoint 12",            "wMsgJ_UTIL_NAV_DIR_SPT_12"    },
            {"Direct Steerpoint 13",            "wMsgJ_UTIL_NAV_DIR_SPT_13"    },
            {"Direct Steerpoint 14",            "wMsgJ_UTIL_NAV_DIR_SPT_14"    },

            {"Nav Mode E G I",                  "wMsgJ_UTIL_NAV_MODE_EGI"       },
            {"Nav Mode G P S",                  "wMsgJ_UTIL_NAV_MODE_EGI"       },//F-14B(U) Nav mode selection
            {"Nav Mode Destination",            "wMsgJ_UTIL_NAV_MODE_DEST"      },

            {"Restore",                         "wMsgJ_UTIL_NAV_REST_MORE"      }, // show hint
            {"Restore Steerpoint 1",            "wMsgJ_UTIL_NAV_REST_MSN_SPT_1" },
            {"Restore Steerpoint 2",            "wMsgJ_UTIL_NAV_REST_MSN_SPT_2" },
            {"Restore Steerpoint 3",            "wMsgJ_UTIL_NAV_REST_MSN_SPT_3" },
            {"Restore Initial Point",           "wMsgJ_UTIL_NAV_REST_INIT_PT_1" },
            {"Restore Initial Point 2",         "wMsgJ_UTIL_NAV_REST_INIT_PT_2" }, //na
            {"Restore Fixed Point",             "wMsgJ_UTIL_NAV_REST_FIX_PT"    },
            {"Restore Mission Steerpoint",      "wMsgJ_UTIL_NAV_REST_MN_FIX_PT" }, //na
            {"Restore Target",                  "wMsgJ_UTIL_NAV_REST_STGT_1"    },
            {"Restore Home Base",               "wMsgJ_UTIL_NAV_REST_HOME"      },
            {"Restore Defense Point",           "wMsgJ_UTIL_NAV_DEF_PNT"        }, // testc?
            {"Restore Hostile Zone",            "wMsgJ_UTIL_NAV_HSTZONE"        }, //

            {"Load Flight Plan 1",          "wMsgJ_UTIL_NAV_LOAD_FLT_PLAN_1"    }, //F-14B(U) Flight Plan management
            {"Load Flight Plan 2",          "wMsgJ_UTIL_NAV_LOAD_FLT_PLAN_2"    },
            {"Load Flight Plan 3",          "wMsgJ_UTIL_NAV_LOAD_FLT_PLAN_3"    },
            {"Load Flight Plan 4",          "wMsgJ_UTIL_NAV_LOAD_FLT_PLAN_4"    },
            {"Load Flight Plan 5",          "wMsgJ_UTIL_NAV_LOAD_FLT_PLAN_5"    },
            {"Load Flight Plan 6",          "wMsgJ_UTIL_NAV_LOAD_FLT_PLAN_6"    },
            {"Load Flight Plan 7",          "wMsgJ_UTIL_NAV_LOAD_FLT_PLAN_7"    },
            {"Reload Flight Plan",          "wMsgJ_UTIL_NAV_RELOAD_FLT_PLAN"    },

            // Contract
            {"Contract",                        "wMsgJ_UTIL_CONTR"              }, // not endpoint, disable
            {"Keep it quiet back there",        "wMsgJ_UTIL_CONTR_NO_TALK"      },
            {"No Talking",                  "wMsgJ_UTIL_CONTR_NO_TALK"      },
            {"You can talk again",              "wMsgJ_UTIL_CONTR_TALK"         },
            {"Talk to me Jester",                 "wMsgJ_UTIL_CONTR_TALK"         },
            {"Set Eject for Both",              "wMsgJ_UTIL_CONTR_EJECT_BTH"    },
            {"Set Eject Both",               "wMsgJ_UTIL_CONTR_EJECT_BTH"    },
            {"Set Eject for Single",            "wMsgJ_UTIL_CONTR_EJECT_SNG"    },
            {"Set Eject Single",             "wMsgJ_UTIL_CONTR_EJECT_SNG"    },
            {"OK for landing calls",            "wMsgJ_UTIL_CONTR_CALL"         },
            {"Landing Callouts Enable",  "wMsgJ_UTIL_CONTR_CALL"         },
            {"No landing calls",                "wMsgJ_UTIL_CONTR_NO_CALL"      },
            {"Landing Callouts Disable", "wMsgJ_UTIL_CONTR_NO_CALL"      },

            {"Back to work",                    "wMsgJ_UTIL_CONTR_ACTIVE"       }, //new
            {"Knock it off",                    "wMsgJ_UTIL_CONTR_INACTIVE"     }, //new
            {"Enable Auto Expand",              "wMsgJ_UTIL_CONTR_AUTO_EXPAND"  },
            {"Disable Auto VID",                "wMsgJ_UTIL_CONTR_AUTO_VID"     },
            {"Treat No-Reply as Bandit",        "wMsgJ_UTIL_CONTR_NO_IFF_REPLY_BANDIT" },
            {"Disable Auto Expand",             "wMsgJ_UTIL_CONTR_DISABLE_AUTO_EXPAND" },
            {"Enable Auto VID",                 "wMsgJ_UTIL_CONTR_ENABLE_AUTO_VID" },
            {"Treat No-Reply as Bogey",         "wMsgJ_UTIL_CONTR_NO_IFF_REPLY_BOGEY" },

            {"Wake Up",                         "wMsgJ_RESET"                   },
            
            // Supercarriers
            {"Link Host Washington",        "wMsgJ_RAD_DL_HOST_WASH"                },
            {"Link Host Roosevelt",         "wMsgJ_RAD_DL_HOST_ROOS"                },
            {"Link Host Lincoln",           "wMsgJ_RAD_DL_HOST_LINC"                },
            {"Link Host Truman",            "wMsgJ_RAD_DL_HOST_TRUM"                },
            {"Link Host Ticonderoga",       "wMsgJ_RAD_DL_HOST_TICO"                },
            {"Link Host Forrestal",         "wMsgJ_RAD_DL_HOST_FORE"                }, //Forrestal
            {"Link Host Arleigh Burke",     "wMsgJ_RAD_DL_HOST_BURK"                },
            {"Link Host Arliegh Burke",     "wMsgJ_RAD_DL_HOST_BURK"                },

            {"TACAN Tune Washington",       "wMsgJ_RAD_TCN_TAC_WASH"                },
            {"TACAN Tune Roosevelt",        "wMsgJ_RAD_TCN_TAC_ROOS"                },
            {"TACAN Tune Lincoln",          "wMsgJ_RAD_TCN_TAC_LINC"                },
            {"TACAN Tune Truman",           "wMsgJ_RAD_TCN_TAC_TRUM"                },
            {"TACAN Tune Forrestal",        "wMsgJ_RAD_TCN_TAC_FORE"                }, // Forrestal
            {"Track Steerpoint 1",          "wMsgJESTER_Steerpoint_SP1"             }, // TESTED OK
            {"Track Steerpoint 2",          "wMsgJESTER_Steerpoint_SP2"             }, // TESTED OK
            {"Track Steerpoint 3",          "wMsgJESTER_Steerpoint_SP3"             }, // TESTED OK
            {"Track Fixed Point",           "wMsgJESTER_Steerpoint_FP"              }, // TESTED OK
            {"Track IP",                    "wMsgJESTER_Steerpoint_IP"              }, // TESTED OK
            {"Track Surface Target",        "wMsgJESTER_Steerpoint_ST"              }, // TESTED OK
            {"Track Home Base",             "wMsgJESTER_Steerpoint_HB"              }, // TESTED OK
            {"Track Defense Point",         "wMsgJESTER_Steerpoint_MAN"             }, // TESTED OK
            {"Reset Lantern",               "wMsgLANTIRN_GPSZero"                   }, // TESTED OK
            {"Toggle View",                 "wMsgLANTIRN_ToggleFOV"                 }, // TESTED OK
            {"J_UTIL_NAV_MAP_MARKER",       "wMsgJ_UTIL_NAV_MAP_MARKER"         },
            {"Grid Enable",                 "wMsgJ_UTIL_NAV_GRD_ENABLE"         },
            {"Grid Disable",                "wMsgJ_UTIL_NAV_GRD_DSABLE"         },
            {"Grid Center",                 "wMsgJ_UTIL_NAV_GRD_CENTER"         },
            {"Grid Relative 180",           "wMsgJ_UTIL_NAV_GRD_REL_180"        },
            {"Grid Relative Plus 30",       "wMsgJ_UTIL_NAV_GRD_REL_u30"        },
            {"Grid Relative Plus 90",       "wMsgJ_UTIL_NAV_GRD_REL_u90"        },
            {"Grid Relative Plus 120",      "wMsgJ_UTIL_NAV_GRD_REL_u120"       },
            {"Grid Relative Minus 120",     "wMsgJ_UTIL_NAV_GRD_REL_d120"       },
            {"Grid Relative Minus 90",      "wMsgJ_UTIL_NAV_GRD_REL_d90"        },
            {"Grid Relative Minus 30",      "wMsgJ_UTIL_NAV_GRD_REL_d30"        },
            {"Grid Absolute 0",             "wMsgJ_UTIL_NAV_GRD_ABS_0"          },
            {"Grid Absolute 45",            "wMsgJ_UTIL_NAV_GRD_ABS_45"         },
            {"Grid Absolute 90",            "wMsgJ_UTIL_NAV_GRD_ABS_90"         },
            {"Grid Absolute 135",           "wMsgJ_UTIL_NAV_GRD_ABS_135"        },
            {"Grid Absolute 180",           "wMsgJ_UTIL_NAV_GRD_ABS_180"        },
            {"Grid Absolute 225",           "wMsgJ_UTIL_NAV_GRD_ABS_225"        },
            {"Grid Absolute 270",           "wMsgJ_UTIL_NAV_GRD_ABS_270"        },
            {"Grid Absolute 315",           "wMsgJ_UTIL_NAV_GRD_ABS_315"        },
            {"Grid Coverage 30",            "wMsgJ_UTIL_NAV_GRD_COV_30"         },
            {"Grid Coverage 60",            "wMsgJ_UTIL_NAV_GRD_COV_60"         },
            {"Grid Coverage 90",            "wMsgJ_UTIL_NAV_GRD_COV_90"         },
            {"Grid Coverage 120",           "wMsgJ_UTIL_NAV_GRD_COV_120"        },
            {"Grid Coverage 180",           "wMsgJ_UTIL_NAV_GRD_COV_180"        },
            {"Grid 1 Sector",               "wMsgJ_UTIL_NAV_GRD_1SCTR"                },
            {"Grid 2 Sectors",              "wMsgJ_UTIL_NAV_GRD_2SCTR"                },
            {"Grid 3 Sectors",              "wMsgJ_UTIL_NAV_GRD_3SCTR"                },
            {"Grid 4 Sectors",              "wMsgJ_UTIL_NAV_GRD_4SCTR"                },
            {"Grid 5 Sectors",              "wMsgJ_UTIL_NAV_GRD_5SCTR"                },
            {"Grid 6 Sectors",              "wMsgJ_UTIL_NAV_GRD_6SCTR"                },
            {"J_UTIL_NAV_GRD_MARKER",       "wMsgJ_UTIL_NAV_GRD_MARKER"               },
            
            {"PlaceHolder383",              "wMsgPlaceHolder383"                },
            {"PlaceHolder384",              "wMsgPlaceHolder384"                },
            {"PlaceHolder385",              "wMsgPlaceHolder385"                },
            {"PlaceHolder386",              "wMsgPlaceHolder386"                },
            {"PlaceHolder387",              "wMsgPlaceHolder387"                },
            {"PlaceHolder388",              "wMsgPlaceHolder388"                },
            {"PlaceHolder389",              "wMsgPlaceHolder389"                },
            // end of utility

            // WALKMAN
            {"Rock and Roll",               "wMsgJ_WLKMN_PLAY"                  },
            {"Cut it out",                  "wMsgJ_WLKMN_STOP"                  },
            {"Skip this part",              "wMsgJ_WLKMN_NEXT"                  },
            {"Go back a little",            "wMsgJ_WLKMN_PREV"                  },

            {"Aspect Switch Beam",              "wMsgJ_RDR_ASP_BEAM"                },
            {"Aspect Switch Nose",              "wMsgJ_RDR_ASP_NOSE"                },
            {"Aspect Switch Tail",              "wMsgJ_RDR_ASP_TAIL"                },
            {"PlaceHolder397",              "wMsgPlaceHolder397"                },
            {"PlaceHolder398",              "wMsgPlaceHolder398"                },
            {"PlaceHolder399",              "wMsgPlaceHolder399"                },

            // block:defensive
            {"Countermeasures Mode",        "wMsgJ_DEF_CMS_MOD"                 }, //
            {"Countermeasures Off",         "wMsgJ_DEF_CMS_MOD_OFF"             },
            {"Countermeasures Manual",      "wMsgJ_DEF_CMS_MOD_MAN"             },
            {"Countermeasures Auto",        "wMsgJ_DEF_CMS_MOD_AUTO"            },

            {"Flares Mode",                 "wMsgJ_DEF_FLR_MOD"                 }, //
            {"Flares Mode Pilot",           "wMsgJ_DEF_FLR_MOD_PILOT"           },
            {"Flares Mode Normal",          "wMsgJ_DEF_FLR_MOD_NORM"            },
            {"Flares Mode Multi",           "wMsgJ_DEF_FLR_MOD_MULTI"           },

            {"Chaff Program",               "wMsgJ_DEF_CHF_PGM"                 }, // hint
            {"Chaff Program 1",             "wMsgJ_DEF_CHF_PGM_RR_12"           },
            {"Chaff Program 2",             "wMsgJ_DEF_CHF_PGM_RR_46"           },
            {"Chaff Program 3",             "wMsgJ_DEF_CHF_PGM_RR_86"           },
            {"Chaff Program 4",             "wMsgJ_DEF_CHF_PGM_20_44"           },
            {"Chaff Program 5",             "wMsgJ_DEF_CHF_PGM_20_84"           },
            {"Chaff Program 6",             "wMsgJ_DEF_CHF_PGM_40_44"           },
            {"Chaff Program 7",             "wMsgJ_DEF_CHF_PGM_40_84"           },
            {"Chaff Program 8",             "wMsgJ_DEF_CHF_PGM_R1_12"           },

            {"Display",                     "wMsgJ_DEF_RWR_DSP_TYP"             }, // show hint: RWR options: Airborne/Normal/AAA/Unknown/Friendly
            {"Display Airborne",            "wMsgJ_DEF_RWR_AIRB"                },
            {"Display Normal",              "wMsgJ_DEF_RWR_NORM"                },
            {"Display AAA",                 "wMsgJ_DEF_RWR_AAA"                 },
            {"Display Unknown",             "wMsgJ_DEF_RWR_UNK"                 },
            {"Display Friendly",            "wMsgJ_DEF_RWR_FRND"                },

            {"Jammer On",                   "wMsgJ_DEF_JMR_ON"                  },
            {"Jammer Standby",              "wMsgJ_DEF_JMR_SBY"                 },

            {"Dispense Order",              "wMsgJ_DEF_CMS_CTL_ORD"             }, // not endpoint, show hint
            {"Dispense Order Chaff Program","wMsgJ_DEF_CMS_CHF_PGM"             },
            {"Dispense Order Chaff Single", "wMsgJ_DEF_CMS_CHF_SNGL"            },
            {"Dispense Order Chaff Tight",  "wMsgJ_DEF_CMS_CHF_TGHT"            },
            {"Dispense Order Flare Program","wMsgJ_DEF_CMS_FLR_PGM"             },
            {"Dispense Order Flare Single", "wMsgJ_DEF_CMS_FLR_SNGL"            },
            {"Dispense Order Flare Tight",  "wMsgJ_DEF_CMS_FLR_TGHT"            },

            {"Flares Program",                      "wMsgJ_DEF_FLR_PGM"                  }, // hint
            {"Flares Program 2 by 2",               "wMsgJ_DEF_FLR_PGM_1"                },
            {"Flares Program 4 by 2",               "wMsgJ_DEF_FLR_PGM_2"                },
            {"Flares Program 10 by 2",              "wMsgJ_DEF_FLR_PGM_3"                },
            {"Flares Program 4 by 6",               "wMsgJ_DEF_FLR_PGM_4"                },
            {"Flares Program 8 by 6",               "wMsgJ_DEF_FLR_PGM_5"                },
            {"Flares Program 10 by 6",              "wMsgJ_DEF_FLR_PGM_6"                },
            {"Flares Program 6 by 10",              "wMsgJ_DEF_FLR_PGM_7"                },
            {"Flares Program 10 by 10",             "wMsgJ_DEF_FLR_PGM_8"                },


            {"Look for Ground Targets",             "wMsgLANTIRN_Srch_Any"                  },
            {"Look for Activity",                   "wMsgLANTIRN_Srch_Any_Active"           },
            {"Look for Bogeys",                     "wMsgLANTIRN_Srch_Air"                  },
            {"Look for Active Bogeys",              "wMsgLANTIRN_Srch_Air_Active"           },
            {"Look for SAMs",                       "wMsgLANTIRN_Srch_SAM_Active"           },
            {"Look for Tanks",                      "wMsgLANTIRN_Srch_Armor_Active"         },
            {"Look for Movers",                     "wMsgLANTIRN_Srch_Vehicle"              },
            {"Look for Ships",                      "wMsgLANTIRN_Ships_Active"              },
            //{"PlaceHolder449",              "wMsgPlaceHolder449"                },
            //{"PlaceHolder450",              "wMsgPlaceHolder450"                },
            //{"PlaceHolder451",              "wMsgPlaceHolder451"                },
            //{"PlaceHolder452",              "wMsgPlaceHolder452"                },
            //{"PlaceHolder453",              "wMsgPlaceHolder453"                },
            //{"PlaceHolder454",              "wMsgPlaceHolder454"                },
            //{"PlaceHolder455",              "wMsgPlaceHolder455"                },
            //{"PlaceHolder456",              "wMsgPlaceHolder456"                },
            //{"PlaceHolder457",              "wMsgPlaceHolder457"                },
            //{"PlaceHolder458",              "wMsgPlaceHolder458"                },
            //{"PlaceHolder459",              "wMsgPlaceHolder459"                },
            //{"PlaceHolder460",              "wMsgPlaceHolder460"                },
            //{"PlaceHolder461",              "wMsgPlaceHolder461"                },
            //{"PlaceHolder462",              "wMsgPlaceHolder462"                },
            //{"PlaceHolder463",              "wMsgPlaceHolder463"                },
            //{"PlaceHolder464",              "wMsgPlaceHolder464"                },
            //{"PlaceHolder465",              "wMsgPlaceHolder465"                },
            //{"PlaceHolder466",              "wMsgPlaceHolder466"                },
            //{"PlaceHolder467",              "wMsgPlaceHolder467"                },
            //{"PlaceHolder468",              "wMsgPlaceHolder468"                },
            //{"PlaceHolder469",              "wMsgPlaceHolder469"                },
            //{"PlaceHolder470",              "wMsgPlaceHolder470"                },
            //{"PlaceHolder471",              "wMsgPlaceHolder471"                },
            //{"PlaceHolder472",              "wMsgPlaceHolder472"                },
            //{"PlaceHolder473",              "wMsgPlaceHolder473"                },
            //{"PlaceHolder474",              "wMsgPlaceHolder474"                },
            //{"PlaceHolder475",              "wMsgPlaceHolder475"                },
            //{"PlaceHolder476",              "wMsgPlaceHolder476"                },
            //{"PlaceHolder477",              "wMsgPlaceHolder477"                },
            //{"PlaceHolder478",              "wMsgPlaceHolder478"                },
            //{"PlaceHolder479",              "wMsgPlaceHolder479"                },
            //{"PlaceHolder480",              "wMsgPlaceHolder480"                },
            //{"PlaceHolder481",              "wMsgPlaceHolder481"                },
            //{"PlaceHolder482",              "wMsgPlaceHolder482"                },
            //{"PlaceHolder483",              "wMsgPlaceHolder483"                },
            //{"PlaceHolder484",              "wMsgPlaceHolder484"                },
            //{"PlaceHolder485",              "wMsgPlaceHolder485"                },
            //{"PlaceHolder486",              "wMsgPlaceHolder486"                },
            //{"PlaceHolder487",              "wMsgPlaceHolder487"                },
            //{"PlaceHolder488",              "wMsgPlaceHolder488"                },
            //{"PlaceHolder489",              "wMsgPlaceHolder489"                },
            // end of defensive

            // spare table
            //{"PlaceHolder490",              "wMsgPlaceHolder490"                },
            //{"PlaceHolder491",              "wMsgPlaceHolder491"                },
            //{"PlaceHolder492",              "wMsgPlaceHolder492"                },
            //{"PlaceHolder493",              "wMsgPlaceHolder493"                },
            //{"PlaceHolder494",              "wMsgPlaceHolder494"                },
            //{"PlaceHolder495",              "wMsgPlaceHolder495"                },
            //{"PlaceHolder496",              "wMsgPlaceHolder496"                },
            //{"PlaceHolder497",              "wMsgPlaceHolder497"                },
            //{"PlaceHolder498",              "wMsgPlaceHolder498"                },
            //{"PlaceHolder499",              "wMsgPlaceHolder499"                },

            // 500-600 RIO misc
            
            // startup
            {"Abort Startup",               "wMsgJ_STRT_ABORT"                  },
            {"Align Fine",                  "wMsgJ_STRT_INS_FINE"               },
            {"Align Minimum",               "wMsgJ_STRT_INS_MIN_WPN"            },
            {"Align Coarse",                "wMsgJ_STRT_INS_COARSE"             },
            {"Align Now",                   "wMsgJ_STRT_INS_NOW"                },

            {"Check",                       "wMsgJ_STRT_CHECK"                  },
            {"Loud and Clear",              "wMsgJ_STRT_LOUD_CLR"               },
            {"Hold it",                     "wMsgJ_STRT_PAUSE"                  },
            {"Startup",                     "wMsgJ_STRT_STARTUP"                },
            {"Assisted Startup",            "wMsgJ_STRT_ASSISTED"               },

            // 600-700 AI pilot
            {"Set Altitude",                "wMsgI_ALT"                         },
            {"Go Angels 1",                 "wMsgI_ALT_ANG_1"                   },
            {"Go Angels 5",                 "wMsgI_ALT_ANG_5"                   },
            {"Go Angels 10",                "wMsgI_ALT_ANG_10"                  },
            {"Go Angels 15",                "wMsgI_ALT_ANG_15"                  },
            {"Go Angels 20",                "wMsgI_ALT_ANG_20"                  },
            {"Go Angels 25",                "wMsgI_ALT_ANG_25"                  },
            {"Go Angels 30",                "wMsgI_ALT_ANG_30"                  },
            {"Go Angels 35",                "wMsgI_ALT_ANG_35"                  },
            {"Change Altitude",             "wMsgI_ALT_CHG"                     },
            {"Descent 10000",               "wMsgI_ALT_DESC_10K"                },
            {"Descent 5000",                "wMsgI_ALT_DESC_5K"                 },
            {"Descent 1000",                "wMsgI_ALT_DESC_1K"                 },
            {"Descent 500",                 "wMsgI_ALT_DESC_500"                },
            {"Climb 500",                   "wMsgI_ALT_CLMB_500"                },
            {"Climb 1000",                  "wMsgI_ALT_CLMB_1K"                 },
            {"Climb 5000",                  "wMsgI_ALT_CLMB_5K"                 },
            {"Climb 10000",                 "wMsgI_ALT_CLMB_10K"                },
            {"Slow Down 200",               "wMsgI_SPD_MINUS_200"               },
            {"Slow Down 100",               "wMsgI_SPD_MINUS_100"               },
            {"Slow Down 50",                "wMsgI_SPD_MINUS_50"                },
            {"Speed Up 50",                 "wMsgI_SPD_PLUS_50"                 },
            {"Speed Up 100",                "wMsgI_SPD_PLUS_100"                },
            {"Speed Up 200",                "wMsgI_SPD_PLUS_200"                },
            {"Head Straight",               "wMsgI_DIR_HOLD_CRS"                },
            {"Heading North",                  "wMsgI_DIR_N"                    },
            {"Heading NorthEast",              "wMsgI_DIR_NE"                   },
            {"Heading East",                   "wMsgI_DIR_E"                    },
            {"Heading SouthEast",              "wMsgI_DIR_SE"                   },
            {"Heading South",                  "wMsgI_DIR_S"                    },
            {"Heading SouthWest",              "wMsgI_DIR_SW"                   },
            {"Heading West",                   "wMsgI_DIR_W"                    },
            {"Heading NorthWest",              "wMsgI_DIR_NW"                   },
            {"Set Heading",                 "wMsgI_DIR"                         },
            {"Turn",                        "wMsgI_DIR_CHG"                     },
            {"Turn Left 45",                "wMsgI_DIR_CHG_L45"                 },
            {"Turn Left 30",                "wMsgI_DIR_CHG_L30"                 },
            {"Turn Left 10",                "wMsgI_DIR_CHG_L10"                 },
            {"Turn Left 5",                 "wMsgI_DIR_CHG_L5"                  },
            {"Turn Right 5",                "wMsgI_DIR_CHG_R5"                  },
            {"Turn Right 10",               "wMsgI_DIR_CHG_R10"                 },
            {"Turn Right 30",               "wMsgI_DIR_CHG_R30"                 },
            {"Turn Right 45",               "wMsgI_DIR_CHG_R45"                 },
            {"Change Speed",                "wMsgI_SPD"                         },

            {"Fly to Destination",                "wMsgI_SPT_FLYTO"                },
            {"Orbit Destination",              "wMsgI_SPT_ORBIT"                },
            //{"PlaceHolder646",              "wMsgPlaceHolder646"                },
            //{"PlaceHolder647",              "wMsgPlaceHolder647"                },
            //{"PlaceHolder648",              "wMsgPlaceHolder648"                },
            //{"PlaceHolder649",              "wMsgPlaceHolder649"                },
            //{"PlaceHolder650",              "wMsgPlaceHolder650"                },
            //{"PlaceHolder651",              "wMsgPlaceHolder651"                },
            //{"PlaceHolder652",              "wMsgPlaceHolder652"                },
            //{"PlaceHolder653",              "wMsgPlaceHolder653"                },
            //{"PlaceHolder654",              "wMsgPlaceHolder654"                },
            //{"PlaceHolder655",              "wMsgPlaceHolder655"                },
            //{"PlaceHolder656",              "wMsgPlaceHolder656"                },
            //{"PlaceHolder657",              "wMsgPlaceHolder657"                },
            //{"PlaceHolder658",              "wMsgPlaceHolder658"                },
            //{"PlaceHolder659",              "wMsgPlaceHolder659"                },
            //{"PlaceHolder660",              "wMsgPlaceHolder660"                },
            //{"PlaceHolder661",              "wMsgPlaceHolder661"                },
            //{"PlaceHolder662",              "wMsgPlaceHolder662"                },
            //{"PlaceHolder663",              "wMsgPlaceHolder663"                },
            //{"PlaceHolder664",              "wMsgPlaceHolder664"                },
            //{"PlaceHolder665",              "wMsgPlaceHolder665"                },
            //{"PlaceHolder666",              "wMsgPlaceHolder666"                },
            //{"PlaceHolder667",              "wMsgPlaceHolder667"                },
            //{"PlaceHolder668",              "wMsgPlaceHolder668"                },
            //{"PlaceHolder669",              "wMsgPlaceHolder669"                },
            //{"PlaceHolder670",              "wMsgPlaceHolder670"                },
            //{"PlaceHolder671",              "wMsgPlaceHolder671"                },
            //{"PlaceHolder672",              "wMsgPlaceHolder672"                },
            //{"PlaceHolder673",              "wMsgPlaceHolder673"                },
            //{"PlaceHolder674",              "wMsgPlaceHolder674"                },
            //{"PlaceHolder675",              "wMsgPlaceHolder675"                },
            //{"PlaceHolder676",              "wMsgPlaceHolder676"                },
            //{"PlaceHolder677",              "wMsgPlaceHolder677"                },
            //{"PlaceHolder678",              "wMsgPlaceHolder678"                },
            //{"PlaceHolder679",              "wMsgPlaceHolder679"                },
            //{"PlaceHolder680",              "wMsgPlaceHolder680"                },
            //{"PlaceHolder681",              "wMsgPlaceHolder681"                },
            //{"PlaceHolder682",              "wMsgPlaceHolder682"                },
            //{"PlaceHolder683",              "wMsgPlaceHolder683"                },
            //{"PlaceHolder684",              "wMsgPlaceHolder684"                },
            //{"PlaceHolder685",              "wMsgPlaceHolder685"                },
            //{"PlaceHolder686",              "wMsgPlaceHolder686"                },
            //{"PlaceHolder687",              "wMsgPlaceHolder687"                },
            //{"PlaceHolder688",              "wMsgPlaceHolder688"                },
            //{"PlaceHolder689",              "wMsgPlaceHolder689"                },
            // end of AI pilot

            // spare table
            //{"PlaceHolder690",              "wMsgPlaceHolder690"                },
            //{"PlaceHolder691",              "wMsgPlaceHolder691"                },
            //{"PlaceHolder692",              "wMsgPlaceHolder692"                },
            //{"PlaceHolder693",              "wMsgPlaceHolder693"                },
            //{"PlaceHolder694",              "wMsgPlaceHolder694"                },
            //{"PlaceHolder695",              "wMsgPlaceHolder695"                },
            //{"PlaceHolder696",              "wMsgPlaceHolder696"                },
            //{"PlaceHolder697",              "wMsgPlaceHolder697"                },
            //{"PlaceHolder698",              "wMsgPlaceHolder698"                },
            //{"PlaceHolder699",              "wMsgPlaceHolder699"                },
        };

    }

}
