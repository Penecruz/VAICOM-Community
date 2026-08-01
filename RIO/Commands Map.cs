using System.Collections.Generic;

namespace VAICOM.Extensions.RIO
{


    public static partial class DeviceActionsLibrary
    {

        public partial class Sequences // strings using base commands
        {

            // central table referenced by command calls: links command unique name to sequence.
            public static Dictionary<string, List<List<DeviceAction>>> RioCommands = new Dictionary<string, List<List<DeviceAction>>>()
            {

            //  example
            {"wMsgJ_CANOPY_OPEN",               new List<List<DeviceAction>> {Macro.Seq_J_CANOPY_OPEN }                },
            {"wMsgJ_CANOPY_CLOSE",              new List<List<DeviceAction>> {Macro.Seq_J_CANOPY_CLOSE}                },

            // unique command name      // unique macro name

            // block: menu control
            {"wMsgJ_MENU_TOGGLE",           new List<List<DeviceAction>> {Macro.Seq_J_MENU_TOGGLE   }       }, // used only indirectly (commands disbaled)
            {"wMsgJ_MENU_OPTION_1",         new List<List<DeviceAction>> {Macro.Seq_J_MENU_OPTION_1 }       },
            {"wMsgJ_MENU_OPTION_2",         new List<List<DeviceAction>> {Macro.Seq_J_MENU_OPTION_2 }       },
            {"wMsgJ_MENU_OPTION_3",         new List<List<DeviceAction>> {Macro.Seq_J_MENU_OPTION_3 }       },
            {"wMsgJ_MENU_OPTION_4",         new List<List<DeviceAction>> {Macro.Seq_J_MENU_OPTION_4 }       },
            {"wMsgJ_MENU_OPTION_5",         new List<List<DeviceAction>> {Macro.Seq_J_MENU_OPTION_5 }       },
            {"wMsgJ_MENU_OPTION_6",         new List<List<DeviceAction>> {Macro.Seq_J_MENU_OPTION_6 }       },
            {"wMsgJ_MENU_OPTION_7",         new List<List<DeviceAction>> {Macro.Seq_J_MENU_OPTION_7 }       },
            {"wMsgJ_MENU_OPTION_8",         new List<List<DeviceAction>> {Macro.Seq_J_MENU_OPTION_8 }       },

            {"wMsgJ_MENU_DIR_D",            new List<List<DeviceAction>> {Macro.Seq_J_MENU_DIR_D }          }, // this block not used (commands disbaled)
            {"wMsgJ_MENU_DIR_DL",           new List<List<DeviceAction>> {Macro.Seq_J_MENU_DIR_DL}          },
            {"wMsgJ_MENU_DIR_DR",           new List<List<DeviceAction>> {Macro.Seq_J_MENU_DIR_DR }         },
            {"wMsgJ_MENU_DIR_L",            new List<List<DeviceAction>> {Macro.Seq_J_MENU_DIR_L }          },
            {"wMsgJ_MENU_DIR_R",            new List<List<DeviceAction>> {Macro.Seq_J_MENU_DIR_R }          },
            {"wMsgJ_MENU_DIR_U",            new List<List<DeviceAction>> {Macro.Seq_J_MENU_DIR_U }          },
            {"wMsgJ_MENU_DIR_UL",           new List<List<DeviceAction>> {Macro.Seq_J_MENU_DIR_UL}          },
            {"wMsgJ_MENU_DIR_UR",           new List<List<DeviceAction>> {Macro.Seq_J_MENU_DIR_UR}          },
            {"wMsgJ_MENU_OPEN",             new List<List<DeviceAction>> {Macro.Seq_J_MENU_OPEN }           },
            {"wMsgJ_MENU_CLOSE",            new List<List<DeviceAction>> {Macro.Seq_J_MENU_CLOSE}           },

            {"wMsgJ_MENU_CONTEXT",          new List<List<DeviceAction>> {Macro.Seq_J_MENU_CONTEXT          }}, // indirect only: commands disbaled
            {"wMsgJ_MENU_MAIN",             new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN             }},
            {"wMsgJ_MENU_CTXT_CLOSE",       new List<List<DeviceAction>> {Macro.Seq_J_MENU_CTXT_CLOSE       }},
            {"wMsgJ_MENU_MAIN_CLOSE",       new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN_CLOSE       }},

            {"wMsgJESTER_LANTIRN_inhibit_auto_designate",   new List<List<DeviceAction>> {Macro.Seq_JESTER_LANTIRN_inhibit_auto_designate }},
            {"wMsgJESTER_LANTIRN_track_target_id",          new List<List<DeviceAction>> {Macro.Seq_JESTER_LANTIRN_track_target_id        }},
            {"wMsgJESTER_LANTIRN_track_zone_id",            new List<List<DeviceAction>> {Macro.Seq_JESTER_LANTIRN_track_zone_id          }},
            {"wMsgJESTER_LANTIRN_designate",                new List<List<DeviceAction>> {Macro.Seq_JESTER_LANTIRN_designate         }},
            //{"wMsgKNEEBOARD_Laser100",                      new List<List<DeviceAction>> {Macro.Seq_KNEEBOARD_Laser100         }},
            //{"wMsgKNEEBOARD_Laser10",                       new List<List<DeviceAction>> {Macro.Seq_KNEEBOARD_Laser10         }},
            //{"wMsgKNEEBOARD_Laser1",                        new List<List<DeviceAction>> {Macro.Seq_KNEEBOARD_Laser1         }},
            // end of menu control

            // spare block
            {"wMsgLANTIRN_ToggleWHOTBHOT",          new List<List<DeviceAction>> {Macro.Seq_LANTIRN_ToggleWHOTBHOT          }},
            {"wMsgLANTIRN_LaserLatched",            new List<List<DeviceAction>> {Macro.Seq_LANTIRN_LaserLatched            }},
            {"wMsgLANTIRN_Laser_ARM",               new List<List<DeviceAction>> {Macro.Seq_LANTIRN_Laser_ARM               }},
            {"wMsgLANTIRN_Laser_ARM_Toggle",        new List<List<DeviceAction>> {Macro.Seq_LANTIRN_Laser_ARM_Toggle        }},
            {"wMsgLANTIRN_Undesignate",             new List<List<DeviceAction>> {Macro.Seq_LANTIRN_Undesignate             }},
            {"wMsgLANTIRN_QHUD_QADL",               new List<List<DeviceAction>> {Macro.Seq_LANTIRN_QHUD_QADL               }},
            {"wMsgLANTIRN_QSNO",                    new List<List<DeviceAction>> {Macro.Seq_LANTIRN_QSNO                    }},
            {"wMsgLANTIRN_QDES",                    new List<List<DeviceAction>> {Macro.Seq_LANTIRN_QDES                    }},
            {"wMsgLANTIRN_QWP_Minus",               new List<List<DeviceAction>> {Macro.Seq_LANTIRN_QWP_Minus               }},
            {"wMsgLANTIRN_QWP_Plus",                new List<List<DeviceAction>> {Macro.Seq_LANTIRN_QWP_Plus                }},

            // block: radar
            {"wMsgJ_RDR_GO_SILENT",                 new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_GO_SILENT       }},
            {"wMsgJ_RDR_SPOT",                      new List<List<DeviceAction>> { Macro.Seq_J_RDR_SPOT            }},
            {"wMsgJ_RDR_BREAK_LOCK",                new List<List<DeviceAction>> { Macro.Seq_J_RDR_BREAK_LOCK      }}, // when STT locked
            {"wMsgJ_RDR_TO_PSTT",                   new List<List<DeviceAction>> { Macro.Seq_J_RDR_TO_PSTT         }}, // when STT locked
            {"wMsgJ_RDR_SCAN_ELEV",                 new List<List<DeviceAction>> { }},  // not endpoint
            {"wMsgJ_RDR_SCAN_AZ",                   new List<List<DeviceAction>> { }},  // not endpoint
            {"wMsgJ_RDR_SCAN_DIST",                 new List<List<DeviceAction>> { }},  // not endpoint
            {"wMsgJ_RDR_TOGGLE_STT",                new List<List<DeviceAction>> { Macro.Seq_J_RDR_TOGGLE_STT      }},
            {"wMsgJ_RDR_VSL_HIGH",                  new List<List<DeviceAction>> { Macro.Seq_J_RDR_VSL_HIGH        }},
            {"wMsgJ_RDR_VSL_LOW",                   new List<List<DeviceAction>> { Macro.Seq_J_RDR_VSL_LOW         }},
            {"wMsgJ_RDR_STT_TGT_AHEAD",             new List<List<DeviceAction>> {Macro.Seq_J_RDR_STT_TGT_AHEAD           }},
            {"wMsgJ_RDR_STT_ENMY_TGT_AHEAD",        new List<List<DeviceAction>> {Macro.Seq_J_RDR_STT_ENMY_TGT_AHEAD      }},
            {"wMsgJ_RDR_STT_FRNDLY_TGT_AHEAD",      new List<List<DeviceAction>> {Macro.Seq_J_RDR_STT_FRNDLY_TGT_AHEAD    }},
            {"wMsgJ_RDR_STT_CHOOSE_SPECIFIC_TGT",   new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_STT_CHOOSE_SPECIFIC_TGT }},
            {"wMsgJ_RDR_STT_FIRST_TWS_TGT",         new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_STT_FIRST_TWS_TGT       }},
            {"wMsgJ_RDR_STT_TWS_TGT_NUM",           new List<List<DeviceAction>> { }}, // not endpoint, show hint
            {"wMsgJ_RDR_BVR",                       new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_BVR                     }}, // not endpoint (root)
            {"wMsgJ_RDR_GO_ACTIVE",                 new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_GO_ACTIVE               }},
            {"wMsgJ_RDR_STT_LOCK",                  new List<List<DeviceAction>> { }},  // not endpoint, show hint
            {"wMsgJ_RDR_AUTO",                      new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_AUTO                    }},
            {"wMsgJ_RDR_RNG_100",                   new List<List<DeviceAction>> { Macro.Seq_J_RDR_RNG_100                     }},
            {"wMsgJ_RDR_RNG_200",                   new List<List<DeviceAction>> { Macro.Seq_J_RDR_RNG_200                     }},
            {"wMsgJ_RDR_RNG_400",                   new List<List<DeviceAction>> { Macro.Seq_J_RDR_RNG_400                     }},
            {"wMsgJ_RDR_POS",                       new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_POS                     }}, // not endpoint
            {"wMsgJ_RDR_POS_CTR",                   new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_POS_CTR                 }},
            {"wMsgJ_RDR_POS_CTR_L",                 new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_POS_CTR_L               }},
            {"wMsgJ_RDR_POS_CTR_R",                 new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_POS_CTR_R               }},
            {"wMsgJ_RDR_POS_L",                     new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_POS_L                   }},
            {"wMsgJ_RDR_POS_R",                     new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_POS_R                   }},
            {"wMsgJ_RDR_POS_L20",                   new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_POS_L20                 }},
            {"wMsgJ_RDR_POS_R20",                   new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_POS_R20                 }},
            {"wMsgJ_RDR_POS_L55",                   new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_POS_L55                 }},
            {"wMsgJ_RDR_POS_R55",                   new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_POS_R55                 }},
            {"wMsgJ_RDR_POS_HI",                    new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_POS_HI                  }},
            {"wMsgJ_RDR_POS_LO",                    new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_POS_LO                  }},
            {"wMsgJ_RDR_POS_MID",                   new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_POS_MID                 }},
            {"wMsgJ_RDR_POS_MID_HI",                new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_POS_MID_HI              }},
            {"wMsgJ_RDR_POS_MID_LO",                new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_POS_MID_LO              }},
            {"wMsgJ_RDR_ELEV_CLOSE_LOW",            new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_ELEV_CLOSE_LOW          }},
            {"wMsgJ_RDR_ELEV_CLOSE_MID_LO",         new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_ELEV_CLOSE_MID_LO       }},
            {"wMsgJ_RDR_ELEV_CLOSE_MID",            new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_ELEV_CLOSE_MID          }},
            {"wMsgJ_RDR_ELEV_CLOSE_MID_HI",         new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_ELEV_CLOSE_MID_HI       }},
            {"wMsgJ_RDR_ELEV_CLOSE_HI",             new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_ELEV_CLOSE_HI           }},
            {"wMsgJ_RDR_ELEV_MID_LOW",              new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_ELEV_MID_LOW            }},
            {"wMsgJ_RDR_ELEV_MID_MID_LO",           new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_ELEV_MID_MID_LO         }},
            {"wMsgJ_RDR_ELEV_MID_MID",              new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_ELEV_MID_MID            }},
            {"wMsgJ_RDR_ELEV_MID_MID_HI",           new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_ELEV_MID_MID_HI         }},
            {"wMsgJ_RDR_ELEV_MID_HI",               new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_ELEV_MID_HI             }},
            {"wMsgJ_RDR_ELEV_LONG_LOW",             new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_ELEV_LONG_LOW           }},
            {"wMsgJ_RDR_ELEV_LONG_MID_LO",          new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_ELEV_LONG_MID_LO        }},
            {"wMsgJ_RDR_ELEV_LONG_MID",             new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_ELEV_LONG_MID           }},
            {"wMsgJ_RDR_ELEV_LONG_MID_HI",          new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_ELEV_LONG_MID_HI        }},
            {"wMsgJ_RDR_ELEV_LONG_HI",              new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_ELEV_LONG_HI            }},
            {"wMsgJ_RDR_STT_TWS_TGT_1",             new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_STT_TWS_TGT_1         }},
            {"wMsgJ_RDR_STT_TWS_TGT_2",             new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_STT_TWS_TGT_2         }},
            {"wMsgJ_RDR_STT_TWS_TGT_3",             new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_STT_TWS_TGT_3         }},
            {"wMsgJ_RDR_STT_TWS_TGT_4",             new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_STT_TWS_TGT_4         }},
            {"wMsgJ_RDR_STT_TWS_TGT_5",             new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_STT_TWS_TGT_5         }},
            {"wMsgJ_RDR_STT_TWS_TGT_6",             new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_STT_TWS_TGT_6         }},
            {"wMsgJ_RDR_STT_TWS_TGT_7",             new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_STT_TWS_TGT_7         }},
            {"wMsgJ_RDR_STT_TWS_TGT_8",             new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_STT_TWS_TGT_8         }},
            {"wMsgJ_RDR_RNG_25",                    new List<List<DeviceAction>> { Macro.Seq_J_RDR_RNG_25                }},
            {"wMsgJ_RDR_RNG_50",                    new List<List<DeviceAction>> { Macro.Seq_J_RDR_RNG_50                }},
            {"wMsgJ_RDR_RNG_AUTO",                  new List<List<DeviceAction>> { Macro.Seq_J_RDR_RNG_AUTO              }},


            {"wMsgJ_RDR_MODE_AUTO",                 new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_MODE_AUTO       }},
            {"wMsgJ_RDR_MODE_TWS",                  new List<List<DeviceAction>> { Macro.Seq_J_RDR_MODE_TWS        }},
            {"wMsgJ_RDR_MODE_RWS",                  new List<List<DeviceAction>> { Macro.Seq_J_RDR_MODE_RWS        }},
            {"wMsgJ_RDR_MODE",                      new List<List<DeviceAction>> {  }},// not endpoint, hint
            // end of radar

            // spare block
            {"wMsgJ_RDR_STAB",                  new List<List<DeviceAction>> { }}, // not endpoint, hint
            {"wMsgJ_RDR_STAB_15",               new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_STAB_15        }},
            {"wMsgJ_RDR_STAB_30",               new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_STAB_30        }},
            {"wMsgJ_RDR_STAB_60",               new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_STAB_60        }},
            {"wMsgJ_RDR_STAB_120",              new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_STAB_120       }},
            {"wMsgJ_RDR_STAB_INDEF",            new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_STAB_INDEF     }},
            {"wMsgJ_RDR_STAB_GROUND",           new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_J_RDR_STAB_GROUND }},

            {"wMsgLANTIRN_Head_Eyeball",        new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_LANTIRN_Head_Eyeball }},
            {"wMsgLANTIRN_Head_Head",           new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_LANTIRN_Head_Head }},
            //{"wMsgPlaceHolder098",           new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_PlaceHolder098 }},
            //{"wMsgPlaceHolder099",           new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_PlaceHolder099 }},

            //NEW:
            {"wMsgJ_RDR_MODE_TWS_MAN",          new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_MODE_TWS_MAN      }},
            {"wMsgJ_RDR_MODE_SIZE_M",           new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_MODE_SIZE_M       }},
            {"wMsgJ_RDR_MODE_SIZE_L",           new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_MODE_SIZE_L       }},
            {"wMsgJ_RDR_MODE_SIZE_S",           new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_MODE_SIZE_S       }},

            // block: weapons
            // section: AG
            {"wMsgJ_WPN_AG_SORDN",              new List<List<DeviceAction>> { }},  // not endpoint, show hint
            {"wMsgJ_WPN_AG_SORDN_WPN_1",        new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_WPN_AG_SORDN_WPN_1          }},
            {"wMsgJ_WPN_AG_SORDN_WPN_2",        new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_WPN_AG_SORDN_WPN_2          }},
            {"wMsgJ_WPN_AG_SORDN_WPN_3",        new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_WPN_AG_SORDN_WPN_3          }},
            {"wMsgJ_WPN_AG_SORDN_WPN_4",        new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_WPN_AG_SORDN_WPN_4          }},
            {"wMsgJ_WPN_AG_SORDN_WPN_5",        new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_WPN_AG_SORDN_WPN_5          }},
            {"wMsgJ_WPN_AG_SORDN_WPN_6",        new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_WPN_AG_SORDN_WPN_6          }},
            {"wMsgJ_WPN_AG_SORDN_WPN_7",        new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_WPN_AG_SORDN_WPN_7          }},
            {"wMsgJ_WPN_AG_SORDN_WPN_8",        new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_WPN_AG_SORDN_WPN_8          }},
            {"wMsgJ_WPN_AG_SPOT",               new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_WPN_AG_SPOT                 }}, // not used
            {"wMsgJ_WPN_AG_SET_COMP_TGT",       new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_WPN_AG_SET_COMP_TGT         }},
            {"wMsgJ_WPN_AG_SET_PAIRS",          new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_WPN_AG_SET_PAIRS            }},
            {"wMsgJ_WPN_AG_SETFUSE",            new List<List<DeviceAction>> { }},// not endpoint, show hint
            {"wMsgJ_WPN_AG_SETFUSE_NOSETAIL",   new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_WPN_AG_SETFUSE_NOSETAIL     }},
            {"wMsgJ_WPN_AG_SETFUSE_NOSE",       new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_WPN_AG_SETFUSE_NOSE         }},
            {"wMsgJ_WPN_AG_SETFUSE_SAFE",       new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_WPN_AG_SETFUSE_SAFE         }},
            {"wMsgJ_WPN_AG_RIP_QTY",            new List<List<DeviceAction>> { }},// not endpoint, show hint
            {"wMsgJ_WPN_AG_RIP_QTY_STEP",       new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_WPN_AG_RIP_QTY_STEP         }},
            {"wMsgJ_WPN_AG_RIP_QTY_2",          new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_WPN_AG_RIP_QTY_2            }},
            {"wMsgJ_WPN_AG_RIP_QTY_5",          new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_WPN_AG_RIP_QTY_5            }},
            {"wMsgJ_WPN_AG_RIP_QTY_10",         new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_WPN_AG_RIP_QTY_10           }},
            {"wMsgJ_WPN_AG_RIP_QTY_20",         new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_WPN_AG_RIP_QTY_20           }},
            {"wMsgJ_WPN_AG_RIP_QTY_30",         new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_WPN_AG_RIP_QTY_30           }},
            {"wMsgJ_WPN_AG_RIP_TIME",           new List<List<DeviceAction>> {  }},// not endpoint, show hint
            {"wMsgJ_WPN_AG_RIP_TIME_STEP",      new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_WPN_AG_RIP_TIME_STEP        }},
            {"wMsgJ_WPN_AG_RIP_TIME_10",        new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_WPN_AG_RIP_TIME_10          }},
            {"wMsgJ_WPN_AG_RIP_TIME_20",        new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_WPN_AG_RIP_TIME_20          }},
            {"wMsgJ_WPN_AG_RIP_TIME_50",        new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_WPN_AG_RIP_TIME_50          }},
            {"wMsgJ_WPN_AG_RIP_TIME_100",       new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_WPN_AG_RIP_TIME_100         }},
            {"wMsgJ_WPN_AG_RIP_TIME_200",       new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_WPN_AG_RIP_TIME_200         }},
            {"wMsgJ_WPN_AG_RIP_TIME_500",       new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_WPN_AG_RIP_TIME_500         }},
            {"wMsgJ_WPN_AG_RIP_TIME_990",       new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_WPN_AG_RIP_TIME_990         }},
            {"wMsgJ_WPN_AG_RIP_DIST",           new List<List<DeviceAction>> {  }}, // not endpoint, show hint
            {"wMsgJ_WPN_AG_RIP_DIST_STEP",      new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_WPN_AG_RIP_DIST_STEP        }}, // not in menu!
            {"wMsgJ_WPN_AG",                    new List<List<DeviceAction>> { }}, // not used 
            {"wMsgJ_WPN_AA",                    new List<List<DeviceAction>> { }}, // not used 
            {"wMsgJ_WPN_AG_RIP",                new List<List<DeviceAction>> { }}, // not endpoint, disabled
            {"wMsgJ_WPN_AG_JETT",               new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_WPN_AG_JETT           }},
            {"wMsgJ_WPN_AG_RIP_DIST_5",         new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_WPN_AG_RIP_DIST_5     }},
            {"wMsgJ_WPN_AG_RIP_DIST_10",        new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_WPN_AG_RIP_DIST_10    }},
            {"wMsgJ_WPN_AG_RIP_DIST_25",        new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_WPN_AG_RIP_DIST_25    }},
            {"wMsgJ_WPN_AG_RIP_DIST_50",        new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_WPN_AG_RIP_DIST_50    }},
            {"wMsgJ_WPN_AG_RIP_DIST_100",       new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_WPN_AG_RIP_DIST_100   }},
            {"wMsgJ_WPN_AG_RIP_DIST_200",       new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_WPN_AG_RIP_DIST_200   }},
            {"wMsgJ_WPN_AG_RIP_DIST_400",       new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_WPN_AG_RIP_DIST_400   }},
            {"wMsgJ_WPN_AG_UTIL_LANTIRN",       new List<List<DeviceAction>> { Macro.Seq_J_WPN_AG_UTIL_LANTIRN   }},
            {"wMsgJ_WPN_AG_SORDN_WPN_9",        new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_WPN_AG_SORDN_WPN_9    }},
            {"wMsgJ_WPN_AG_SORDN_WPN_10",       new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_WPN_AG_SORDN_WPN_10   }},
            {"wMsgJ_WPN_AG_SET_SNGL",           new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_WPN_AG_SET_SNGL       }},
            {"wMsgJ_WPN_AG_SET_COMP_PILOT",     new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_WPN_AG_SET_COMP_PILOT }},
            {"wMsgJ_WPN_AG_DROP_TANKS",         new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_WPN_AG_DROP_TANKS     }},

            {"wMsgJ_WPN_AG_STN",                new List<List<DeviceAction>> {  }},// not endpoint, show hint
            {"wMsgJ_WPN_AG_STN_18",             new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_WPN_AG_STN_18        }},
            {"wMsgJ_WPN_AG_STN_27",             new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_WPN_AG_STN_27        }},
            {"wMsgJ_WPN_AG_STN_3456",           new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_WPN_AG_STN_3456      }},
            {"wMsgJ_WPN_AG_STN_36",             new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_WPN_AG_STN_36        }},
            {"wMsgJ_WPN_AG_STN_45",             new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_WPN_AG_STN_45        }},

            {"wMsgJ_WPN_AG_JDAM_WPT_TO_GGW",    new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_WPN_AG_JDAM_WPT_TO_GGW   }},
            {"wMsgJ_WPN_AG_JDAM_SEL_PP",        new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_WPN_AG_JDAM_SEL_PP       }},
            {"wMsgJ_WPN_AG_JDAM_PP_ALL_STN",    new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_WPN_AG_JDAM_PP_ALL_STN   }},
            {"wMsgJ_WPN_AG_JDAM_DESIG_TO_STN",  new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_WPN_AG_JDAM_DESIG_TO_STN }},

            //{"wMsgPlaceHolder155",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder155        }},
            //{"wMsgPlaceHolder156",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder156        }},
            //{"wMsgPlaceHolder157",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder157        }},
            //{"wMsgPlaceHolder158",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder158        }},
            //{"wMsgPlaceHolder159",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder159        }},
            //{"wMsgPlaceHolder160",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder160        }},
            //{"wMsgPlaceHolder161",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder161        }},
            //{"wMsgPlaceHolder162",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder162        }},
            //{"wMsgPlaceHolder163",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder163        }},
            //{"wMsgPlaceHolder164",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder164        }},
            //{"wMsgPlaceHolder165",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder165        }},
            //{"wMsgPlaceHolder166",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder166        }},
            //{"wMsgPlaceHolder167",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder167        }},
            //{"wMsgPlaceHolder168",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder168        }},
            //{"wMsgPlaceHolder169",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder169        }},
            //{"wMsgPlaceHolder170",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder170        }},
            //{"wMsgPlaceHolder171",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder171        }},
            //{"wMsgPlaceHolder172",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder172        }},
            //{"wMsgPlaceHolder173",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder173        }},
            //{"wMsgPlaceHolder174",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder174        }},
            //{"wMsgPlaceHolder175",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder175        }},
            //{"wMsgPlaceHolder176",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder176        }},
            //{"wMsgPlaceHolder177",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder177        }},
            //{"wMsgPlaceHolder178",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder178        }},
            //{"wMsgPlaceHolder179",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder179        }},
            //{"wMsgPlaceHolder180",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder180        }},
            //{"wMsgPlaceHolder181",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder181        }},
            //{"wMsgPlaceHolder182",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder182        }},
            //{"wMsgPlaceHolder183",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder183        }},
            //{"wMsgPlaceHolder184",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder184        }},
            //{"wMsgPlaceHolder185",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder185        }},
            //{"wMsgPlaceHolder186",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder186        }},
            //{"wMsgPlaceHolder187",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder187        }},
            {"wMsgJ_WPN_AG_RIP_QTY_16",     new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_WPN_AG_RIP_QTY_16 }},
            {"wMsgJ_WPN_AG_RIP_QTY_28",     new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_WPN_AG_RIP_QTY_28 }},
            // end of weapons

            // spare block
            {"wMsgJ_RAD_TCN_TAC_STEN",          new List<List<DeviceAction>> {Macro.Seq_J_RAD_TCN_TAC_STEN        }},
            {"wMsgJ_RAD_TCN_TAC_ARCO",          new List<List<DeviceAction>> {Macro.Seq_J_RAD_TCN_TAC_ARCO        }},
            {"wMsgJ_RAD_TCN_TAC_SHEL",          new List<List<DeviceAction>> {Macro.Seq_J_RAD_TCN_TAC_SHEL        }},
            {"wMsgJ_RAD_TCN_TAC_TEXA",          new List<List<DeviceAction>> {Macro.Seq_J_RAD_TCN_TAC_TEXA        }},

            {"wMsgJ_RAD_TCN_TAC_WASH",          new List<List<DeviceAction>> {Macro.Seq_J_RAD_TCN_TAC_WASH        }},
            {"wMsgJ_RAD_TCN_TAC_ROOS",          new List<List<DeviceAction>> {Macro.Seq_J_RAD_TCN_TAC_ROOS        }},
            {"wMsgJ_RAD_TCN_TAC_LINC",          new List<List<DeviceAction>> {Macro.Seq_J_RAD_TCN_TAC_LINC        }},
            {"wMsgJ_RAD_TCN_TAC_TRUM",          new List<List<DeviceAction>> {Macro.Seq_J_RAD_TCN_TAC_TRUM        }},
            {"wMsgJ_RAD_TCN_TAC_FORE",          new List<List<DeviceAction>> {Macro.Seq_J_RAD_TCN_TAC_FORE        }},
            //{"wMsgPlaceHolder198",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder198        }},
            //{"wMsgPlaceHolder199",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder199        }},

            // block: radio
            {"wMsgJ_RAD_159",               new List<List<DeviceAction>> {Macro.Seq_J_RAD_159             }},
            {"wMsgJ_RAD_159_USE_GUARD",     new List<List<DeviceAction>> {Macro.Seq_J_RAD_159_USE_GUARD   }},
            {"wMsgJ_RAD_159_USE_MANUAL",    new List<List<DeviceAction>> {Macro.Seq_J_RAD_159_USE_MANUAL  }},
            {"wMsgJ_RAD_159_USE_CHAN",      new List<List<DeviceAction>> {Macro.Seq_J_RAD_159_USE_CHAN    }},
            {"wMsgJ_RAD_159_USE_CHAN_1",    new List<List<DeviceAction>> {Macro.Seq_J_RAD_159_USE_CHAN_1  }},
            {"wMsgJ_RAD_159_USE_CHAN_2",    new List<List<DeviceAction>> {Macro.Seq_J_RAD_159_USE_CHAN_2  }},
            {"wMsgJ_RAD_159_USE_CHAN_3",    new List<List<DeviceAction>> {Macro.Seq_J_RAD_159_USE_CHAN_3  }},
            {"wMsgJ_RAD_159_USE_CHAN_4",    new List<List<DeviceAction>> {Macro.Seq_J_RAD_159_USE_CHAN_4  }},
            {"wMsgJ_RAD_159_USE_CHAN_5",    new List<List<DeviceAction>> {Macro.Seq_J_RAD_159_USE_CHAN_5  }},
            {"wMsgJ_RAD_159_USE_CHAN_6",    new List<List<DeviceAction>> {Macro.Seq_J_RAD_159_USE_CHAN_6  }},
            {"wMsgJ_RAD_159_USE_CHAN_7",    new List<List<DeviceAction>> {Macro.Seq_J_RAD_159_USE_CHAN_7  }},
            {"wMsgJ_RAD_159_USE_CHAN_8",    new List<List<DeviceAction>> {Macro.Seq_J_RAD_159_USE_CHAN_8  }},
            {"wMsgJ_RAD_159_TUNE_MAN",      new List<List<DeviceAction>> {Macro.Seq_J_RAD_159_TUNE_MAN    }},
            {"wMsgJ_RAD_159_SELECT_CHAN",   new List<List<DeviceAction>> {Macro.Seq_J_RAD_159_SELECT_CHAN }},
            {"wMsgJ_RAD_159_SELECT_MODE",   new List<List<DeviceAction>> {Macro.Seq_J_RAD_159_SELECT_MODE }},
            {"wMsgJ_RAD_159_TUNE_ATC",      new List<List<DeviceAction>> {Macro.Seq_J_RAD_159_TUNE_ATC    }},
            {"wMsgJ_RAD_159_TUNE_TAC",      new List<List<DeviceAction>> {Macro.Seq_J_RAD_159_TUNE_TAC    }},
            {"wMsgJ_RAD_182",               new List<List<DeviceAction>> {Macro.Seq_J_RAD_182             }},
            {"wMsgJ_RAD_182_USE_GUARD",     new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RAD_182_USE_GUARD   }},
            {"wMsgJ_RAD_182_USE_MANUAL",    new List<List<DeviceAction>> {Macro.Seq_J_RAD_182_USE_MANUAL  }},
            {"wMsgJ_RAD_182_USE_CHAN",      new List<List<DeviceAction>> {Macro.Seq_J_RAD_182_USE_CHAN    }},
            {"wMsgJ_RAD_182_USE_CHAN_1 ",   new List<List<DeviceAction>> {Macro.Seq_J_RAD_182_USE_CHAN_1  }},
            {"wMsgJ_RAD_182_USE_CHAN_2",    new List<List<DeviceAction>> {Macro.Seq_J_RAD_182_USE_CHAN_2  }},
            {"wMsgJ_RAD_182_USE_CHAN_3",    new List<List<DeviceAction>> {Macro.Seq_J_RAD_182_USE_CHAN_3  }},
            {"wMsgJ_RAD_182_USE_CHAN_4",    new List<List<DeviceAction>> {Macro.Seq_J_RAD_182_USE_CHAN_4  }},
            {"wMsgJ_RAD_182_USE_CHAN_5",    new List<List<DeviceAction>> {Macro.Seq_J_RAD_182_USE_CHAN_5  }},
            {"wMsgJ_RAD_182_USE_CHAN_6",    new List<List<DeviceAction>> {Macro.Seq_J_RAD_182_USE_CHAN_6  }},
            {"wMsgJ_RAD_182_USE_CHAN_7",    new List<List<DeviceAction>> {Macro.Seq_J_RAD_182_USE_CHAN_7  }},
            {"wMsgJ_RAD_182_USE_CHAN_8",    new List<List<DeviceAction>> {Macro.Seq_J_RAD_182_USE_CHAN_8  }},
            {"wMsgJ_RAD_182_TUNE_MAN",      new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RAD_182_TUNE_MAN    }},
            {"wMsgJ_RAD_182_SELECT_CHAN",   new List<List<DeviceAction>> {Macro.Seq_J_RAD_182_SELECT_CHAN }},
            {"wMsgJ_RAD_182_SELECT_MODE",   new List<List<DeviceAction>> {Macro.Seq_J_RAD_182_SELECT_MODE }},
            {"wMsgJ_RAD_182_TUNE_ATC",      new List<List<DeviceAction>> {Macro.Seq_J_RAD_182_TUNE_ATC    }},
            {"wMsgJ_RAD_182_TUNE_TAC",      new List<List<DeviceAction>> {Macro.Seq_J_RAD_182_TUNE_TAC    }},

            {"wMsgJ_RAD_DL",                new List<List<DeviceAction>> { }}, // not endpoint
            {"wMsgJ_RAD_DL_SET_MODE",       new List<List<DeviceAction>> { }}, // 1 not endpoint
            {"wMsgJ_RAD_DL_SET_FREQ_PRST",  new List<List<DeviceAction>> { }}, // 2 not endpoint
            {"wMsgJ_RAD_DL_SET_HOST",       new List<List<DeviceAction>> { }},// 3 not endpoint
            {"wMsgJ_RAD_DL_TACT",           new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RAD_DL_TACT         }},
            {"wMsgJ_RAD_DL_OFF",            new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RAD_DL_OFF          }},
            {"wMsgJ_RAD_DL_FGHT",           new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RAD_DL_FGHT         }},

            {"wMsgJ_RAD_DL_SET_FREQ_1",     new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RAD_DL_SET_FREQ_1        }},
            {"wMsgJ_RAD_DL_SET_FREQ_2",     new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RAD_DL_SET_FREQ_2        }},
            {"wMsgJ_RAD_DL_SET_FREQ_3",     new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RAD_DL_SET_FREQ_3        }},
            {"wMsgJ_RAD_DL_SET_FREQ_4",     new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RAD_DL_SET_FREQ_4        }},
            {"wMsgJ_RAD_DL_SET_FREQ_5",     new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RAD_DL_SET_FREQ_5        }},
            {"wMsgJ_RAD_DL_SET_FREQ_6",     new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RAD_DL_SET_FREQ_6        }},
            {"wMsgJ_RAD_DL_SET_FREQ_7",     new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RAD_DL_SET_FREQ_7        }},
            {"wMsgJ_RAD_DL_SET_FREQ_8",     new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RAD_DL_SET_FREQ_8        }},

            {"wMsgJ_RAD_TCN",               new List<List<DeviceAction>> {  }},// disabled
            {"wMsgJ_RAD_TCN_MODE",          new List<List<DeviceAction>> {  }},// not endpoint
            {"wMsgJ_RAD_TCN_MODE_OFF",      new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RAD_TCN_MODE_OFF      }},
            {"wMsgJ_RAD_TCN_MODE_REC",      new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RAD_TCN_MODE_REC      }},
            {"wMsgJ_RAD_TCN_MODE_TR",       new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RAD_TCN_MODE_TR       }},
            {"wMsgJ_RAD_TCN_MODE_AA",       new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RAD_TCN_MODE_AA       }},
            {"wMsgJ_RAD_TCN_MODE_BCN",      new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RAD_TCN_MODE_BCN      }},

            {"wMsgJ_RAD_TCN_SEL_CHAN",      new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RAD_TCN_SEL_CHAN      }},
            {"wMsgJ_RAD_TCN_SEL_GND_STN",   new List<List<DeviceAction>> {  }},  // not endpoint
            {"wMsgJ_RAD_TCN_TUNE_TAC",      new List<List<DeviceAction>> {  }}, // not endpoint

            {"wMsgJ_RAD_TCN_T_CS_TSK",          new List<List<DeviceAction>> {Macro.Seq_J_RAD_TCN_T_CS_TSK        }},
            {"wMsgJ_RAD_TCN_T_CS_KBL",          new List<List<DeviceAction>> {Macro.Seq_J_RAD_TCN_T_CS_KBL        }},
            {"wMsgJ_RAD_TCN_T_CS_BTM",          new List<List<DeviceAction>> {Macro.Seq_J_RAD_TCN_T_CS_BTM        }},
            {"wMsgJ_RAD_TCN_T_CS_KTS",          new List<List<DeviceAction>> {Macro.Seq_J_RAD_TCN_T_CS_KTS        }},
            {"wMsgJ_RAD_TCN_T_CS_GTB",          new List<List<DeviceAction>> {Macro.Seq_J_RAD_TCN_T_CS_GTB        }},
            {"wMsgJ_RAD_TCN_T_CS_VAS",          new List<List<DeviceAction>> {Macro.Seq_J_RAD_TCN_T_CS_VAS        }},
            {"wMsgJ_RAD_TCN_T_NV_LSV",          new List<List<DeviceAction>> {Macro.Seq_J_RAD_TCN_T_NV_LSV        }},
            {"wMsgJ_RAD_TCN_T_NV_LAS",          new List<List<DeviceAction>> {Macro.Seq_J_RAD_TCN_T_NV_LAS        }},
            {"wMsgJ_RAD_TCN_T_NV_BLD",          new List<List<DeviceAction>> {Macro.Seq_J_RAD_TCN_T_NV_BLD        }},
            {"wMsgJ_RAD_TCN_T_NV_INS",          new List<List<DeviceAction>> {Macro.Seq_J_RAD_TCN_T_NV_INS        }},
            {"wMsgJ_RAD_TCN_T_NV_MMM",          new List<List<DeviceAction>> {Macro.Seq_J_RAD_TCN_T_NV_MMM        }},
            {"wMsgJ_RAD_TCN_T_NV_GFS",          new List<List<DeviceAction>> {Macro.Seq_J_RAD_TCN_T_NV_GFS        }},
            {"wMsgJ_RAD_TCN_T_NV_GRL",          new List<List<DeviceAction>> {Macro.Seq_J_RAD_TCN_T_NV_GRL        }},
            {"wMsgJ_RAD_TCN_T_NV_PGS",          new List<List<DeviceAction>> {Macro.Seq_J_RAD_TCN_T_NV_PGS        }},
            {"wMsgJ_RAD_TCN_T_NV_BTY",          new List<List<DeviceAction>> {Macro.Seq_J_RAD_TCN_T_NV_BTY        }},
            {"wMsgJ_RAD_TCN_T_NV_EER",          new List<List<DeviceAction>> {Macro.Seq_J_RAD_TCN_T_NV_EER        }},
            {"wMsgJ_RAD_TCN_T_NV_DAG",          new List<List<DeviceAction>> {Macro.Seq_J_RAD_TCN_T_NV_DAG        }},
            {"wMsgJ_RAD_TCN_T_NV_HEC",          new List<List<DeviceAction>> {Macro.Seq_J_RAD_TCN_T_NV_HEC        }},
            {"wMsgJ_RAD_TCN_T_NV_OAL",          new List<List<DeviceAction>> {Macro.Seq_J_RAD_TCN_T_NV_OAL        }},
            {"wMsgJ_RAD_TCN_T_NV_BIH",          new List<List<DeviceAction>> {Macro.Seq_J_RAD_TCN_T_NV_BIH        }},
            {"wMsgJ_RAD_TCN_T_NV_MVA",          new List<List<DeviceAction>> {Macro.Seq_J_RAD_TCN_T_NV_MVA        }},
            {"wMsgJ_RAD_TCN_T_PG_KCK",          new List<List<DeviceAction>> {Macro.Seq_J_RAD_TCN_T_PG_KCK        }},
            {"wMsgJ_RAD_TCN_T_PG_KSB",          new List<List<DeviceAction>> {Macro.Seq_J_RAD_TCN_T_PG_KSB        }},
            {"wMsgJ_RAD_TCN_T_PG_HDR",          new List<List<DeviceAction>> {Macro.Seq_J_RAD_TCN_T_PG_HDR        }},
            {"wMsgJ_RAD_TCN_T_PG_MA",           new List<List<DeviceAction>> {Macro.Seq_J_RAD_TCN_T_PG_MA         }},
            {"wMsgJ_RAD_TCN_T_PG_SYZI",         new List<List<DeviceAction>> {Macro.Seq_J_RAD_TCN_T_PG_SYZI       }},
            {"wMsgJ_RAD_TCN_T_STN",             new List<List<DeviceAction>> {Macro.Seq_J_RAD_TCN_T_STN           }},

            {"wMsgJ_RAD_182_MODE_TR",           new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RAD_182_MODE_TR           }},
            {"wMsgJ_RAD_182_MODE_TRG",          new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RAD_182_MODE_TRG          }},
            {"wMsgJ_RAD_182_MODE_DF",           new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RAD_182_MODE_DF           }},
            {"wMsgJ_RAD_182_MODE_TEST",         new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RAD_182_MODE_TEST         }},
            // end of radio

            // spare block
            {"wMsgJ_RAD_182_MODE_AM",          new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RAD_182_MODE_AM            }},
            {"wMsgJ_RAD_182_MODE_FM",          new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RAD_182_MODE_FM            }},
            {"wMsgJ_RAD_182_MODE",             new List<List<DeviceAction>> {  }},// not endpoint
            {"wMsgJ_RAD_182_MODE_OFF",         new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RAD_182_MODE_OFF           }},

            {"wMsgJ_RAD_DL_HOST_STEN",          new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RAD_DL_HOST_STEN        }},
            {"wMsgJ_RAD_DL_HOST_DARK",          new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RAD_DL_HOST_DARK        }},
            {"wMsgJ_RAD_DL_HOST_FOCS",          new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RAD_DL_HOST_FOCS        }},
            {"wMsgJ_RAD_DL_HOST_MAGC",          new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RAD_DL_HOST_MAGC        }},
            {"wMsgJ_RAD_DL_HOST_OVRL",          new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RAD_DL_HOST_OVRL        }},
            {"wMsgJ_RAD_DL_HOST_WIZR",          new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RAD_DL_HOST_WIZR        }},
            
            // block: utility
            {"wMsgJ_UTIL_NAV",              new List<List<DeviceAction>> {   }}, // not endpoint
            {"wMsgJ_UTIL_NAV_SEL_DEST_SPT", new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_SEL_DEST_SPT   }}, //not used
            {"wMsgJ_UTIL_NAV_REST_MSN_SPT", new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_REST_MSN_SPT   }}, //na
            {"wMsgJ_UTIL_NAV_MAN_ENT_SPT",  new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_MAN_ENT_SPT    }}, // not used / not endpoint (only for hostile zone)
            {"wMsgJ_UTIL_NAV_MAP_SPT",      new List<List<DeviceAction>> {  }}, // not endpoint, show hint
            {"wMsgJ_UTIL_NAV_MAP_SPT_1",    new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_MAP_SPT_1      }},
            {"wMsgJ_UTIL_NAV_MAP_SPT_2",    new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_MAP_SPT_2      }},
            {"wMsgJ_UTIL_NAV_MAP_SPT_3",    new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_MAP_SPT_3      }},
            {"wMsgJ_UTIL_NAV_MAP_SPT_4",    new List<List<DeviceAction>> { Macro.Seq_J_UTIL_NAV_MAP_SPT_4      }}, //na
            {"wMsgJ_UTIL_NAV_MAP_SPT_5",    new List<List<DeviceAction>> { Macro.Seq_J_UTIL_NAV_MAP_SPT_5      }}, //na
            {"wMsgJ_UTIL_NAV_MAP_SPT_6",    new List<List<DeviceAction>> { Macro.Seq_J_UTIL_NAV_MAP_SPT_6      }}, //na
            {"wMsgJ_UTIL_NAV_MAP_SPT_7",    new List<List<DeviceAction>> { }}, // not used unless overide for B(U)
            {"wMsgJ_UTIL_NAV_MAP_SPT_8",    new List<List<DeviceAction>> { }},
            {"wMsgJ_UTIL_NAV_MAP_SPT_9",    new List<List<DeviceAction>> { }},
            {"wMsgJ_UTIL_NAV_MAP_SPT_10",   new List<List<DeviceAction>> { }},
            {"wMsgJ_UTIL_NAV_MAP_SPT_11",   new List<List<DeviceAction>> { }},
            {"wMsgJ_UTIL_NAV_MAP_SPT_12",   new List<List<DeviceAction>> { }},
            {"wMsgJ_UTIL_NAV_MAP_SPT_13",   new List<List<DeviceAction>> { }},
            {"wMsgJ_UTIL_NAV_MAP_SPT_14",   new List<List<DeviceAction>> { }},
            {"wMsgJ_UTIL_NAV_MAP_FIX_PNT",  new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_MAP_FIX_PNT    }},
            {"wMsgJ_UTIL_NAV_MAP_INIT_PNT", new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_MAP_INIT_PNT   }},
            {"wMsgJ_UTIL_NAV_SURF_TGT",     new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_SURF_TGT       }},
            {"wMsgJ_UTIL_NAV_HOME_BASE",    new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_HOME_BASE      }},

            {"wMsgJ_UTIL_NAV_REST_MORE",        new List<List<DeviceAction>> { }}, //show hint
            {"wMsgJ_UTIL_NAV_REST_MSN_SPT_1",   new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_REST_MSN_SPT, Macro.Seq_J_UTIL_NAV_REST_MSN_SPT_1, Macro.Seq_J_UTIL_NAV_REST_MSN_SPT_1  }}, // do last twice
            {"wMsgJ_UTIL_NAV_REST_MSN_SPT_2",   new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_REST_MSN_SPT, Macro.Seq_J_UTIL_NAV_REST_MSN_SPT_2, Macro.Seq_J_UTIL_NAV_REST_MSN_SPT_2 }},
            {"wMsgJ_UTIL_NAV_REST_MSN_SPT_3",   new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_REST_MSN_SPT, Macro.Seq_J_UTIL_NAV_REST_MSN_SPT_3, Macro.Seq_J_UTIL_NAV_REST_MSN_SPT_3 }},
            {"wMsgJ_UTIL_NAV_REST_INIT_PT_1",   new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_REST_MSN_SPT, Macro.Seq_J_UTIL_NAV_REST_INIT_PT_1, Macro.Seq_J_UTIL_NAV_REST_INIT_PT_1 }},
            {"wMsgJ_UTIL_NAV_REST_INIT_PT_2",   new List<List<DeviceAction>> { Macro.Seq_J_UTIL_NAV_REST_INIT_PT_2 }},//na
            {"wMsgJ_UTIL_NAV_REST_FIX_PT",      new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_REST_MSN_SPT, Macro.Seq_J_UTIL_NAV_REST_FIX_PT, Macro.Seq_J_UTIL_NAV_REST_FIX_PT     }},
            {"wMsgJ_UTIL_NAV_REST_MN_FIX_PT",   new List<List<DeviceAction>> { Macro.Seq_J_UTIL_NAV_REST_MN_FIX_PT }}, // na
            {"wMsgJ_UTIL_NAV_REST_STGT_1",      new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_REST_MSN_SPT, Macro.Seq_J_UTIL_NAV_REST_STGT_1 , Macro.Seq_J_UTIL_NAV_REST_STGT_1    }},
            {"wMsgJ_UTIL_NAV_REST_HOME",        new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_REST_MSN_SPT, Macro.Seq_J_UTIL_NAV_REST_HOME, Macro.Seq_J_UTIL_NAV_REST_HOME        }},
            {"wMsgJ_UTIL_NAV_DEF_PNT",          new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_MAN_ENT_SPT,  Macro.Seq_J_UTIL_NAV_DEF_PNT, Macro.Seq_J_UTIL_NAV_DEF_PNT     }},
            {"wMsgJ_UTIL_NAV_HSTZONE",          new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_MAN_ENT_SPT,  Macro.Seq_J_UTIL_NAV_HSTZONE, Macro.Seq_J_UTIL_NAV_HSTZONE      }},

            {"wMsgJ_UTIL_CONTR",            new List<List<DeviceAction>> {Macro.Seq_J_UTIL_CONTR                }},
            {"wMsgJ_UTIL_CONTR_NO_TALK",    new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_CONTR_NO_TALK        }},
            {"wMsgJ_UTIL_CONTR_TALK",       new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_CONTR_TALK           }},
            {"wMsgJ_UTIL_CONTR_EJECT_BTH",  new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_CONTR_EJECT_BTH      }},
            {"wMsgJ_UTIL_CONTR_EJECT_SNG",  new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_CONTR_EJECT_SNG      }},
            {"wMsgJ_UTIL_CONTR_ACTIVE",     new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_CONTR_ACTIVE         }}, //new
            {"wMsgJ_UTIL_CONTR_INACTIVE",   new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_CONTR_INACTIVE       }}, //new
            {"wMsgJ_UTIL_CONTR_CALL",       new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_CONTR_CALL           }},
            {"wMsgJ_UTIL_CONTR_NO_CALL",    new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_CONTR_NO_CALL        }},
            {"wMsgJ_UTIL_CONTR_AUTO_EXPAND",        new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_CONTR_AUTO_EXPAND}},
            {"wMsgJ_UTIL_CONTR_AUTO_VID",           new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_CONTR_AUTO_VID}},
            {"wMsgJ_UTIL_CONTR_NO_IFF_REPLY_BANDIT",new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_CONTR_NO_IFF_REPLY_BANDIT}},
            {"wMsgJ_UTIL_CONTR_DISABLE_AUTO_EXPAND",new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_CONTR_AUTO_EXPAND}},
            {"wMsgJ_UTIL_CONTR_ENABLE_AUTO_VID",    new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_CONTR_AUTO_VID}},
            {"wMsgJ_UTIL_CONTR_NO_IFF_REPLY_BOGEY", new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_CONTR_NO_IFF_REPLY_BANDIT}},

            {"wMsgJ_RESET",                 new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RESET        }},

            {"wMsgJ_RAD_DL_HOST_WASH",          new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RAD_DL_HOST_WASH     }},
            {"wMsgJ_RAD_DL_HOST_ROOS",          new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RAD_DL_HOST_ROOS     }},
            {"wMsgJ_RAD_DL_HOST_LINC",          new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RAD_DL_HOST_LINC     }},
            {"wMsgJ_RAD_DL_HOST_TRUM",          new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RAD_DL_HOST_TRUM     }},
            {"wMsgJ_RAD_DL_HOST_TICO",          new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RAD_DL_HOST_TICO     }},
            {"wMsgJ_RAD_DL_HOST_FORE",          new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RAD_DL_HOST_FORE     }},
            {"wMsgJ_RAD_DL_HOST_BURK",          new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RAD_DL_HOST_BURK     }},

            {"wMsgJESTER_Steerpoint_SP1",       new List<List<DeviceAction>> { Macro.Seq_JESTER_Steerpoint_SP1     }},
            {"wMsgJESTER_Steerpoint_SP2",       new List<List<DeviceAction>> { Macro.Seq_JESTER_Steerpoint_SP2     }},
            {"wMsgJESTER_Steerpoint_SP3",       new List<List<DeviceAction>> { Macro.Seq_JESTER_Steerpoint_SP3     }},
            {"wMsgJESTER_Steerpoint_FP",        new List<List<DeviceAction>> { Macro.Seq_JESTER_Steerpoint_FP      }},
            {"wMsgJESTER_Steerpoint_IP",        new List<List<DeviceAction>> { Macro.Seq_JESTER_Steerpoint_IP      }},
            {"wMsgJESTER_Steerpoint_ST",        new List<List<DeviceAction>> { Macro.Seq_JESTER_Steerpoint_ST      }},
            {"wMsgJESTER_Steerpoint_HB",        new List<List<DeviceAction>> { Macro.Seq_JESTER_Steerpoint_HB      }},
            {"wMsgJESTER_Steerpoint_MAN",       new List<List<DeviceAction>> { Macro.Seq_JESTER_Steerpoint_MAN     }},
            {"wMsgLANTIRN_GPSZero",             new List<List<DeviceAction>> {Macro.Seq_LANTIRN_GPSZero           }},
            {"wMsgLANTIRN_ToggleFOV",           new List<List<DeviceAction>> {Macro.Seq_LANTIRN_ToggleFOV         }},
            {"wMsgJ_UTIL_NAV_MAP_MARKER",          new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_MAP_MARKER        }},
            {"wMsgJ_UTIL_NAV_GRD_ENABLE",          new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_GRD_ENABLE        }},
            {"wMsgJ_UTIL_NAV_GRD_DSABLE",          new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_GRD_DSABLE        }},
            {"wMsgJ_UTIL_NAV_GRD_CENTER",          new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_GRD_CENTER        }},
            {"wMsgJ_UTIL_NAV_GRD_REL_180",         new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_GRD_REL_180       }},
            {"wMsgJ_UTIL_NAV_GRD_REL_u30",         new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_GRD_REL_u30       }},
            {"wMsgJ_UTIL_NAV_GRD_REL_u90",         new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_GRD_REL_u90       }},
            {"wMsgJ_UTIL_NAV_GRD_REL_u120",        new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_GRD_REL_u120      }},
            {"wMsgJ_UTIL_NAV_GRD_REL_d120",        new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_GRD_REL_d120      }},
            {"wMsgJ_UTIL_NAV_GRD_REL_d90",         new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_GRD_REL_d90       }},
            {"wMsgJ_UTIL_NAV_GRD_REL_d30",         new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_GRD_REL_d30       }},
            {"wMsgJ_UTIL_NAV_GRD_ABS_0",           new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_GRD_ABS_0         }},
            {"wMsgJ_UTIL_NAV_GRD_ABS_45",          new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_GRD_ABS_45        }},
            {"wMsgJ_UTIL_NAV_GRD_ABS_90",          new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_GRD_ABS_90        }},
            {"wMsgJ_UTIL_NAV_GRD_ABS_135",         new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_GRD_ABS_135       }},
            {"wMsgJ_UTIL_NAV_GRD_ABS_180",         new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_GRD_ABS_180       }},
            {"wMsgJ_UTIL_NAV_GRD_ABS_225",         new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_GRD_ABS_225       }},
            {"wMsgJ_UTIL_NAV_GRD_ABS_270",         new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_GRD_ABS_270       }},
            {"wMsgJ_UTIL_NAV_GRD_ABS_315",         new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_GRD_ABS_315       }},
            {"wMsgJ_UTIL_NAV_GRD_COV_30",          new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_GRD_COV_30        }},
            {"wMsgJ_UTIL_NAV_GRD_COV_60",          new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_GRD_COV_60        }},
            {"wMsgJ_UTIL_NAV_GRD_COV_90",          new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_GRD_COV_90        }},
            {"wMsgJ_UTIL_NAV_GRD_COV_120",          new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_GRD_COV_120       }},
            {"wMsgJ_UTIL_NAV_GRD_COV_180",          new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_GRD_COV_180       }},
            {"wMsgJ_UTIL_NAV_GRD_1SCTR",          new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_GRD_1SCTR        }},
            {"wMsgJ_UTIL_NAV_GRD_2SCTR",          new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_GRD_2SCTR        }},
            {"wMsgJ_UTIL_NAV_GRD_3SCTR",          new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_GRD_3SCTR        }},
            {"wMsgJ_UTIL_NAV_GRD_4SCTR",          new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_GRD_4SCTR        }},
            {"wMsgJ_UTIL_NAV_GRD_5SCTR",          new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_GRD_5SCTR        }},
            {"wMsgJ_UTIL_NAV_GRD_6SCTR",          new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_GRD_6SCTR        }},
            {"wMsgJ_UTIL_NAV_GRD_MARKER",          new List<List<DeviceAction>> {Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_GRD_MARKER        }},
            {"wMsgJ_UTIL_NAV_LOAD_FLT_PLAN_1",     new List<List<DeviceAction>> { }},
            {"wMsgJ_UTIL_NAV_LOAD_FLT_PLAN_2",     new List<List<DeviceAction>> { }},
            {"wMsgJ_UTIL_NAV_LOAD_FLT_PLAN_3",     new List<List<DeviceAction>> { }},
            {"wMsgJ_UTIL_NAV_LOAD_FLT_PLAN_4",     new List<List<DeviceAction>> { }},
            {"wMsgJ_UTIL_NAV_LOAD_FLT_PLAN_5",     new List<List<DeviceAction>> { }},
            {"wMsgJ_UTIL_NAV_LOAD_FLT_PLAN_6",     new List<List<DeviceAction>> { }},
            {"wMsgJ_UTIL_NAV_LOAD_FLT_PLAN_7",     new List<List<DeviceAction>> { }},
            {"wMsgJ_UTIL_NAV_RELOAD_FLT_PLAN",     new List<List<DeviceAction>> { }},
            {"wMsgJ_UTIL_NAV_DIR_SPT_1",           new List<List<DeviceAction>> { }},
            {"wMsgJ_UTIL_NAV_DIR_SPT_2",           new List<List<DeviceAction>> { }},
            {"wMsgJ_UTIL_NAV_DIR_SPT_3",           new List<List<DeviceAction>> { }},
            {"wMsgJ_UTIL_NAV_DIR_SPT_4",           new List<List<DeviceAction>> { }},
            {"wMsgJ_UTIL_NAV_DIR_SPT_5",           new List<List<DeviceAction>> { }},
            {"wMsgJ_UTIL_NAV_DIR_SPT_6",           new List<List<DeviceAction>> { }},
            {"wMsgJ_UTIL_NAV_DIR_SPT_7",           new List<List<DeviceAction>> { }},
            {"wMsgJ_UTIL_NAV_DIR_SPT_8",           new List<List<DeviceAction>> { }},
            {"wMsgJ_UTIL_NAV_DIR_SPT_9",           new List<List<DeviceAction>> { }},
            {"wMsgJ_UTIL_NAV_DIR_SPT_10",          new List<List<DeviceAction>> { }},
            {"wMsgJ_UTIL_NAV_DIR_SPT_11",          new List<List<DeviceAction>> { }},
            {"wMsgJ_UTIL_NAV_DIR_SPT_12",          new List<List<DeviceAction>> { }},
            {"wMsgJ_UTIL_NAV_DIR_SPT_13",          new List<List<DeviceAction>> { }},
            {"wMsgJ_UTIL_NAV_DIR_SPT_14",          new List<List<DeviceAction>> { }},
            {"wMsgJ_UTIL_NAV_MODE_EGI",            new List<List<DeviceAction>> { }},
            {"wMsgJ_UTIL_NAV_MODE_DEST",           new List<List<DeviceAction>> { }},

            //{"wMsgPlaceHolder387",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder387        }},
            //{"wMsgPlaceHolder388",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder388        }},
            //{"wMsgPlaceHolder389",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder389        }},
            // end of utility

            // spare block
            {"wMsgJ_WLKMN_PLAY",          new List<List<DeviceAction>> {Macro.Seq_J_WLKMN_PLAY        }},
            {"wMsgJ_WLKMN_STOP",          new List<List<DeviceAction>> {Macro.Seq_J_WLKMN_STOP        }},
            {"wMsgJ_WLKMN_NEXT",          new List<List<DeviceAction>> {Macro.Seq_J_WLKMN_NEXT        }},
            {"wMsgJ_WLKMN_PREV",          new List<List<DeviceAction>> {Macro.Seq_J_WLKMN_PREV        }},

            {"wMsgJ_RDR_ASP_BEAM",          new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_ASP_BEAM        }},
            {"wMsgJ_RDR_ASP_NOSE",          new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_ASP_NOSE        }},
            {"wMsgJ_RDR_ASP_TAIL",          new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_ASP_TAIL        }},
            //{"wMsgPlaceHolder397",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder397        }},
            //{"wMsgPlaceHolder398",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder398        }},
            //{"wMsgPlaceHolder399",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder399        }},

            // block: defensive
            {"wMsgJ_DEF_CMS_MOD",           new List<List<DeviceAction>> { }}, // not endpoint, show hint
            {"wMsgJ_DEF_CMS_MOD_OFF",       new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_DEF_CMS_MOD_OFF     }},
            {"wMsgJ_DEF_CMS_MOD_MAN",       new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_DEF_CMS_MOD_MAN     }},
            {"wMsgJ_DEF_CMS_MOD_AUTO",      new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_DEF_CMS_MOD_AUTO    }},

            {"wMsgJ_DEF_FLR_MOD",           new List<List<DeviceAction>> { }}, // not endpoint
            {"wMsgJ_DEF_FLR_MOD_PILOT",     new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_DEF_FLR_MOD_PILOT   }},
            {"wMsgJ_DEF_FLR_MOD_NORM",      new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_DEF_FLR_MOD_NORM    }},
            {"wMsgJ_DEF_FLR_MOD_MULTI",     new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_DEF_FLR_MOD_MULTI   }},

            {"wMsgJ_DEF_CHF_PGM",           new List<List<DeviceAction>> {  }}, // not endpoint
            {"wMsgJ_DEF_CHF_PGM_RR_12",     new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_DEF_CHF_PGM_RR_12   }},
            {"wMsgJ_DEF_CHF_PGM_RR_46",     new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_DEF_CHF_PGM_RR_46   }},
            {"wMsgJ_DEF_CHF_PGM_RR_86",     new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_DEF_CHF_PGM_RR_86   }},
            {"wMsgJ_DEF_CHF_PGM_20_44",     new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_DEF_CHF_PGM_20_44   }},
            {"wMsgJ_DEF_CHF_PGM_20_84",     new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_DEF_CHF_PGM_20_84   }},
            {"wMsgJ_DEF_CHF_PGM_40_44",     new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_DEF_CHF_PGM_40_44   }},
            {"wMsgJ_DEF_CHF_PGM_40_84",     new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_DEF_CHF_PGM_40_84   }},
            {"wMsgJ_DEF_CHF_PGM_R1_12",     new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_DEF_CHF_PGM_R1_12   }},

            {"wMsgJ_DEF_RWR_DSP_TYP",       new List<List<DeviceAction>> { }}, // show RWR options
            {"wMsgJ_DEF_RWR_AIRB",          new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_DEF_RWR_AIRB        }},
            {"wMsgJ_DEF_RWR_NORM",          new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_DEF_RWR_NORM        }},
            {"wMsgJ_DEF_RWR_AAA",           new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_DEF_RWR_AAA         }},
            {"wMsgJ_DEF_RWR_UNK",           new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_DEF_RWR_UNK         }},
            {"wMsgJ_DEF_RWR_FRND",          new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_DEF_RWR_FRND        }},

            {"wMsgJ_DEF_JMR_ON",            new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_DEF_JMR_ON          }},
            {"wMsgJ_DEF_JMR_SBY",           new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_DEF_JMR_SBY         }},

            {"wMsgJ_DEF_CMS_CTL_ORD",       new List<List<DeviceAction>> { }}, // not endpoint, hint for control order
            {"wMsgJ_DEF_CMS_CHF_PGM",       new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_DEF_CMS_CHF_PGM     }},
            {"wMsgJ_DEF_CMS_CHF_SNGL",      new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_DEF_CMS_CHF_SNGL    }},
            {"wMsgJ_DEF_CMS_CHF_TGHT",      new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_DEF_CMS_CHF_TGHT    }},
            {"wMsgJ_DEF_CMS_FLR_PGM",       new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_DEF_CMS_FLR_PGM     }},
            {"wMsgJ_DEF_CMS_FLR_SNGL",      new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_DEF_CMS_FLR_SNGL    }},
            {"wMsgJ_DEF_CMS_FLR_TGHT",      new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_DEF_CMS_FLR_TGHT    }},

            {"wMsgJ_DEF_FLR_PGM",           new List<List<DeviceAction>> { }}, // hint
            {"wMsgJ_DEF_FLR_PGM_1",         new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_DEF_FLR_PGM_1        }},
            {"wMsgJ_DEF_FLR_PGM_2",         new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_DEF_FLR_PGM_2        }},
            {"wMsgJ_DEF_FLR_PGM_3",         new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_DEF_FLR_PGM_3        }},
            {"wMsgJ_DEF_FLR_PGM_4",         new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_DEF_FLR_PGM_4        }},
            {"wMsgJ_DEF_FLR_PGM_5",         new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_DEF_FLR_PGM_5        }},
            {"wMsgJ_DEF_FLR_PGM_6",         new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_DEF_FLR_PGM_6        }},
            {"wMsgJ_DEF_FLR_PGM_7",         new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_DEF_FLR_PGM_7        }},
            {"wMsgJ_DEF_FLR_PGM_8",         new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_DEF_FLR_PGM_8        }},


            {"wMsgLANTIRN_Srch_Any",            new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_LANTIRN_Srch_Any        }},
            {"wMsgLANTIRN_Srch_Any_Active",     new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_LANTIRN_Srch_Any_Active        }},
            {"wMsgLANTIRN_Srch_Air",            new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_LANTIRN_Srch_Air        }},
            {"wMsgLANTIRN_Srch_Air_Active",     new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_LANTIRN_Srch_Air_Active        }},
            {"wMsgLANTIRN_Srch_SAM_Active",     new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_LANTIRN_Srch_SAM_Active        }},
            {"wMsgLANTIRN_Srch_Armor_Active",   new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_LANTIRN_Srch_Armor_Active        }},
            {"wMsgLANTIRN_Srch_Vehicle",        new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_LANTIRN_Srch_Vehicle        }},
            {"wMsgLANTIRN_Ships_Active",        new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_LANTIRN_Ships_Active        }},
            //{"wMsgPlaceHolder449",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder449        }},
            //{"wMsgPlaceHolder450",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder450        }},
            //{"wMsgPlaceHolder451",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder451        }},
            //{"wMsgPlaceHolder452",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder452        }},
            //{"wMsgPlaceHolder453",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder453        }},
            //{"wMsgPlaceHolder454",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder454        }},
            //{"wMsgPlaceHolder455",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder455        }},
            //{"wMsgPlaceHolder456",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder456        }},
            //{"wMsgPlaceHolder457",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder457        }},
            //{"wMsgPlaceHolder458",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder458        }},
            //{"wMsgPlaceHolder459",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder459        }},
            //{"wMsgPlaceHolder460",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder460        }},
            //{"wMsgPlaceHolder461",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder461        }},
            //{"wMsgPlaceHolder462",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder462        }},
            //{"wMsgPlaceHolder463",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder463        }},
            //{"wMsgPlaceHolder464",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder464        }},
            //{"wMsgPlaceHolder465",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder465        }},
            //{"wMsgPlaceHolder466",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder466        }},
            //{"wMsgPlaceHolder467",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder467        }},
            //{"wMsgPlaceHolder468",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder468        }},
            //{"wMsgPlaceHolder469",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder469        }},
            //{"wMsgPlaceHolder470",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder470        }},
            //{"wMsgPlaceHolder471",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder471        }},
            //{"wMsgPlaceHolder472",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder472        }},
            //{"wMsgPlaceHolder473",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder473        }},
            //{"wMsgPlaceHolder474",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder474        }},
            //{"wMsgPlaceHolder475",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder475        }},
            //{"wMsgPlaceHolder476",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder476        }},
            //{"wMsgPlaceHolder477",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder477        }},
            //{"wMsgPlaceHolder478",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder478        }},
            //{"wMsgPlaceHolder479",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder479        }},
            //{"wMsgPlaceHolder480",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder480        }},
            //{"wMsgPlaceHolder481",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder481        }},
            //{"wMsgPlaceHolder482",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder482        }},
            //{"wMsgPlaceHolder483",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder483        }},
            //{"wMsgPlaceHolder484",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder484        }},
            //{"wMsgPlaceHolder485",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder485        }},
            //{"wMsgPlaceHolder486",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder486        }},
            //{"wMsgPlaceHolder487",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder487        }},
            //{"wMsgPlaceHolder488",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder488        }},
            //{"wMsgPlaceHolder489",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder489        }},
            // end of defensive

            // spare block
            //{"wMsgPlaceHolder490",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder490        }},
            //{"wMsgPlaceHolder491",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder491        }},
            //{"wMsgPlaceHolder492",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder492        }},
            //{"wMsgPlaceHolder493",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder493        }},
            //{"wMsgPlaceHolder494",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder494        }},
            //{"wMsgPlaceHolder495",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder495        }},
            //{"wMsgPlaceHolder496",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder496        }},
            //{"wMsgPlaceHolder497",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder497        }},
            //{"wMsgPlaceHolder498",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder498        }},
            //{"wMsgPlaceHolder499",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder499        }},

            // RIO misc 500-600

            // startup
            {"wMsgJ_STRT_ABORT",            new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_J_STRT_ABORT          }},
            {"wMsgJ_STRT_INS_FINE",         new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_J_STRT_INS_FINE       }},
            {"wMsgJ_STRT_INS_MIN_WPN",      new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_J_STRT_INS_MIN_WPN    }},
            {"wMsgJ_STRT_INS_COARSE",       new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_J_STRT_INS_COARSE     }},
            {"wMsgJ_STRT_INS_NOW",          new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_J_STRT_INS_NOW        }},

            {"wMsgJ_STRT_CHECK",            new List<List<DeviceAction>> { Macro.Seq_J_STRT_CHECK        }},
            {"wMsgJ_STRT_LOUD_CLR",         new List<List<DeviceAction>> { Macro.Seq_J_STRT_LOUD_CLR        }},
            {"wMsgJ_STRT_PAUSE",            new List<List<DeviceAction>> { Macro.Seq_J_STRT_PAUSE        }},
            {"wMsgJ_STRT_STARTUP",          new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_J_STRT_STARTUP        }},
            {"wMsgJ_STRT_ASSISTED",         new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_J_STRT_ASSISTED       }},
            
            // block 600-700 AI pilot
            {"wMsgI_ALT",                   new List<List<DeviceAction>> { }},// hint
            {"wMsgI_ALT_ANG_1",             new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_I_ALT_ANG_1           }},
            {"wMsgI_ALT_ANG_5",             new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_I_ALT_ANG_5           }},
            {"wMsgI_ALT_ANG_10",            new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_I_ALT_ANG_10          }},
            {"wMsgI_ALT_ANG_15",            new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_I_ALT_ANG_15          }},
            {"wMsgI_ALT_ANG_20",            new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_I_ALT_ANG_20          }},
            {"wMsgI_ALT_ANG_25",            new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_I_ALT_ANG_25          }},
            {"wMsgI_ALT_ANG_30",            new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_I_ALT_ANG_30          }},
            {"wMsgI_ALT_ANG_35",            new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_I_ALT_ANG_35          }},
            {"wMsgI_ALT_CHG",               new List<List<DeviceAction>> { }},// hint
            {"wMsgI_ALT_DESC_10K",          new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_I_ALT_DESC_10K        }},
            {"wMsgI_ALT_DESC_5K",           new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_I_ALT_DESC_5K         }},
            {"wMsgI_ALT_DESC_1K",           new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_I_ALT_DESC_1K         }},
            {"wMsgI_ALT_DESC_500",          new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_I_ALT_DESC_500        }},
            {"wMsgI_ALT_CLMB_500",          new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_I_ALT_CLMB_500        }},
            {"wMsgI_ALT_CLMB_1K",           new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_I_ALT_CLMB_1K         }},
            {"wMsgI_ALT_CLMB_5K",           new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_I_ALT_CLMB_5K         }},
            {"wMsgI_ALT_CLMB_10K",          new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_I_ALT_CLMB_10K        }},
            {"wMsgI_SPD",                   new List<List<DeviceAction>> { }},// hint
            {"wMsgI_SPD_MINUS_200",         new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_I_SPD_MINUS_200       }},
            {"wMsgI_SPD_MINUS_100",         new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_I_SPD_MINUS_100       }},
            {"wMsgI_SPD_MINUS_50",          new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_I_SPD_MINUS_50        }},
            {"wMsgI_SPD_PLUS_50",           new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_I_SPD_PLUS_50         }},
            {"wMsgI_SPD_PLUS_100",          new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_I_SPD_PLUS_100        }},
            {"wMsgI_SPD_PLUS_200",          new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_I_SPD_PLUS_200        }},
            {"wMsgI_DIR",                   new List<List<DeviceAction>> { }}, // hint
            {"wMsgI_DIR_N",                 new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_I_DIR_N               }},
            {"wMsgI_DIR_NE",                new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_I_DIR_NE              }},
            {"wMsgI_DIR_E",                 new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_I_DIR_E               }},
            {"wMsgI_DIR_SE",                new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_I_DIR_SE              }},
            {"wMsgI_DIR_S",                 new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_I_DIR_S               }},
            {"wMsgI_DIR_SW",                new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_I_DIR_SW              }},
            {"wMsgI_DIR_W",                 new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_I_DIR_W               }},
            {"wMsgI_DIR_NW",                new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_I_DIR_NW              }},
            {"wMsgI_DIR_CHG",               new List<List<DeviceAction>> { }},// hint
            {"wMsgI_DIR_CHG_L45",           new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_I_DIR_CHG_L45         }},
            {"wMsgI_DIR_CHG_L30",           new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_I_DIR_CHG_L30         }},
            {"wMsgI_DIR_CHG_L10",           new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_I_DIR_CHG_L10         }},
            {"wMsgI_DIR_CHG_L5",            new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_I_DIR_CHG_L5          }},
            {"wMsgI_DIR_CHG_R5",            new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_I_DIR_CHG_R5          }},
            {"wMsgI_DIR_CHG_R10",           new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_I_DIR_CHG_R10         }},
            {"wMsgI_DIR_CHG_R30",           new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_I_DIR_CHG_R30         }},
            {"wMsgI_DIR_CHG_R45",           new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_I_DIR_CHG_R45         }},
            {"wMsgI_DIR_HOLD_CRS",          new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_I_DIR_HOLD_CRS        }},

            {"wMsgI_SPT_FLYTO",             new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_I_SPT_FLYTO        }},
            {"wMsgI_SPT_ORBIT",             new List<List<DeviceAction>> { Macro.Seq_J_MENU_CONTEXT, Macro.Seq_I_SPT_ORBIT        }},
            //{"wMsgPlaceHolder646",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder646        }},
            //{"wMsgPlaceHolder647",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder647        }},
            //{"wMsgPlaceHolder648",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder648        }},
            //{"wMsgPlaceHolder649",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder649        }},
            //{"wMsgPlaceHolder650",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder650        }},
            //{"wMsgPlaceHolder651",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder651        }},
            //{"wMsgPlaceHolder652",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder652        }},
            //{"wMsgPlaceHolder653",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder653        }},
            //{"wMsgPlaceHolder654",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder654        }},
            //{"wMsgPlaceHolder655",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder655        }},
            //{"wMsgPlaceHolder656",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder656        }},
            //{"wMsgPlaceHolder657",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder657        }},
            //{"wMsgPlaceHolder658",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder658        }},
            //{"wMsgPlaceHolder659",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder659        }},
            //{"wMsgPlaceHolder660",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder660        }},
            //{"wMsgPlaceHolder661",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder661        }},
            //{"wMsgPlaceHolder662",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder662        }},
            //{"wMsgPlaceHolder663",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder663        }},
            //{"wMsgPlaceHolder664",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder664        }},
            //{"wMsgPlaceHolder665",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder665        }},
            //{"wMsgPlaceHolder666",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder666        }},
            //{"wMsgPlaceHolder667",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder667        }},
            //{"wMsgPlaceHolder668",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder668        }},
            //{"wMsgPlaceHolder669",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder669        }},
            //{"wMsgPlaceHolder670",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder670        }},
            //{"wMsgPlaceHolder671",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder671        }},
            //{"wMsgPlaceHolder672",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder672        }},
            //{"wMsgPlaceHolder673",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder673        }},
            //{"wMsgPlaceHolder674",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder674        }},
            //{"wMsgPlaceHolder675",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder675        }},
            //{"wMsgPlaceHolder676",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder676        }},
            //{"wMsgPlaceHolder677",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder677        }},
            //{"wMsgPlaceHolder678",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder678        }},
            //{"wMsgPlaceHolder679",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder679        }},
            //{"wMsgPlaceHolder680",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder680        }},
            //{"wMsgPlaceHolder681",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder681        }},
            //{"wMsgPlaceHolder682",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder682        }},
            //{"wMsgPlaceHolder683",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder683        }},
            //{"wMsgPlaceHolder684",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder684        }},
            //{"wMsgPlaceHolder685",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder685        }},
            //{"wMsgPlaceHolder686",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder686        }},
            //{"wMsgPlaceHolder687",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder687        }},
            //{"wMsgPlaceHolder688",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder688        }},
            //{"wMsgPlaceHolder689",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder689        }},
            // end of AI pilot

            // spare block
            //{"wMsgPlaceHolder690",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder690        }},
            //{"wMsgPlaceHolder691",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder691        }},
            //{"wMsgPlaceHolder692",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder692        }},
            //{"wMsgPlaceHolder693",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder693        }},
            //{"wMsgPlaceHolder694",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder694        }},
            //{"wMsgPlaceHolder695",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder695        }},
            //{"wMsgPlaceHolder696",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder696        }},
            //{"wMsgPlaceHolder697",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder697        }},
            //{"wMsgPlaceHolder698",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder698        }},
            //{"wMsgPlaceHolder699",          new List<List<DeviceAction>> {Macro.Seq_PlaceHolder699        }},


            };


            public static Dictionary<string, List<List<DeviceAction>>> AuxCommands = new Dictionary<string, List<List<DeviceAction>>>()
            {
                {"F-14BU:wMsgJ_RDR_GO_ACTIVE", new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_GO_ACTIVE_BU }},  //F-14B(U) overides list
                {"F-14BU:wMsgJ_RDR_GO_SILENT", new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_RDR_GO_SILENT_BU }},
                {"F-14BU:wMsgJ_UTIL_NAV_MAP_SPT_1", new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_MAP_SPT_1_BU }},
                {"F-14BU:wMsgJ_UTIL_NAV_MAP_SPT_2", new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_MAP_SPT_2_BU }},
                {"F-14BU:wMsgJ_UTIL_NAV_MAP_SPT_3", new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_MAP_SPT_3_BU }},
                {"F-14BU:wMsgJ_UTIL_NAV_MAP_SPT_4", new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_MAP_SPT_4_BU }},
                {"F-14BU:wMsgJ_UTIL_NAV_MAP_SPT_5", new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_MAP_SPT_5_BU }},
                {"F-14BU:wMsgJ_UTIL_NAV_MAP_SPT_6", new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_MAP_SPT_6_BU }},
                {"F-14BU:wMsgJ_UTIL_NAV_MAP_SPT_7", new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_MAP_SPT_7_BU }},
                {"F-14BU:wMsgJ_UTIL_NAV_MAP_SPT_8", new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_MAP_SPT_8_BU }},
                {"F-14BU:wMsgJ_UTIL_NAV_MAP_SPT_9", new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_MAP_SPT_9_BU }},
                {"F-14BU:wMsgJ_UTIL_NAV_MAP_SPT_10", new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_MAP_SPT_10_BU }},
                {"F-14BU:wMsgJ_UTIL_NAV_MAP_SPT_11", new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_MAP_SPT_11_BU }},
                {"F-14BU:wMsgJ_UTIL_NAV_MAP_SPT_12", new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_MAP_SPT_12_BU }},
                {"F-14BU:wMsgJ_UTIL_NAV_MAP_SPT_13", new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_MAP_SPT_13_BU }},
                {"F-14BU:wMsgJ_UTIL_NAV_MAP_SPT_14", new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_MAP_SPT_14_BU }},
                {"F-14BU:wMsgJ_UTIL_NAV_MAP_FIX_PNT", new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_MAP_FIX_PNT_BU }},
                {"F-14BU:wMsgJ_UTIL_NAV_SURF_TGT", new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_SURF_TGT_BU }},
                {"F-14BU:wMsgJ_UTIL_NAV_LOAD_FLT_PLAN_1", new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_LOAD_FLT_PLAN_1_BU }},
                {"F-14BU:wMsgJ_UTIL_NAV_LOAD_FLT_PLAN_2", new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_LOAD_FLT_PLAN_2_BU }},
                {"F-14BU:wMsgJ_UTIL_NAV_LOAD_FLT_PLAN_3", new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_LOAD_FLT_PLAN_3_BU }},
                {"F-14BU:wMsgJ_UTIL_NAV_LOAD_FLT_PLAN_4", new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_LOAD_FLT_PLAN_4_BU }},
                {"F-14BU:wMsgJ_UTIL_NAV_LOAD_FLT_PLAN_5", new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_LOAD_FLT_PLAN_5_BU }},
                {"F-14BU:wMsgJ_UTIL_NAV_LOAD_FLT_PLAN_6", new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_LOAD_FLT_PLAN_6_BU }},
                {"F-14BU:wMsgJ_UTIL_NAV_LOAD_FLT_PLAN_7", new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_LOAD_FLT_PLAN_7_BU }},
                {"F-14BU:wMsgJ_UTIL_NAV_RELOAD_FLT_PLAN", new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_RELOAD_FLT_PLAN_BU }},
                {"F-14BU:wMsgJ_UTIL_NAV_DIR_SPT_1", new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_DIR_SPT_1_BU }},
                {"F-14BU:wMsgJ_UTIL_NAV_DIR_SPT_2", new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_DIR_SPT_2_BU }},
                {"F-14BU:wMsgJ_UTIL_NAV_DIR_SPT_3", new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_DIR_SPT_3_BU }},
                {"F-14BU:wMsgJ_UTIL_NAV_DIR_SPT_4", new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_DIR_SPT_4_BU }},
                {"F-14BU:wMsgJ_UTIL_NAV_DIR_SPT_5", new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_DIR_SPT_5_BU }},
                {"F-14BU:wMsgJ_UTIL_NAV_DIR_SPT_6", new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_DIR_SPT_6_BU }},
                {"F-14BU:wMsgJ_UTIL_NAV_DIR_SPT_7", new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_DIR_SPT_7_BU }},
                {"F-14BU:wMsgJ_UTIL_NAV_DIR_SPT_8", new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_DIR_SPT_8_BU }},
                {"F-14BU:wMsgJ_UTIL_NAV_DIR_SPT_9", new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_DIR_SPT_9_BU }},
                {"F-14BU:wMsgJ_UTIL_NAV_DIR_SPT_10", new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_DIR_SPT_10_BU }},
                {"F-14BU:wMsgJ_UTIL_NAV_DIR_SPT_11", new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_DIR_SPT_11_BU }},
                {"F-14BU:wMsgJ_UTIL_NAV_DIR_SPT_12", new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_DIR_SPT_12_BU }},
                {"F-14BU:wMsgJ_UTIL_NAV_DIR_SPT_13", new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_DIR_SPT_13_BU }},
                {"F-14BU:wMsgJ_UTIL_NAV_DIR_SPT_14", new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_DIR_SPT_14_BU }},
                {"F-14BU:wMsgJ_UTIL_NAV_MODE_EGI", new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_MODE_EGI_BU }},
                {"F-14BU:wMsgJ_UTIL_NAV_MODE_DEST", new List<List<DeviceAction>> { Macro.Seq_J_MENU_MAIN, Macro.Seq_J_UTIL_NAV_MODE_DEST_BU }},
                {"F-14BU:wMsgJ_UTIL_NAV_MAP_INIT_PNT", new List<List<DeviceAction>>()},
                {"F-14BU:wMsgJ_UTIL_NAV_HOME_BASE", new List<List<DeviceAction>>()},
                {"F-14BU:wMsgJ_UTIL_NAV_REST_MORE", new List<List<DeviceAction>>()},
                {"F-14BU:wMsgJ_UTIL_NAV_REST_MSN_SPT_1", new List<List<DeviceAction>>()},
                {"F-14BU:wMsgJ_UTIL_NAV_REST_MSN_SPT_2", new List<List<DeviceAction>>()},
                {"F-14BU:wMsgJ_UTIL_NAV_REST_MSN_SPT_3", new List<List<DeviceAction>>()},
                {"F-14BU:wMsgJ_UTIL_NAV_REST_INIT_PT_1", new List<List<DeviceAction>>()},
                {"F-14BU:wMsgJ_UTIL_NAV_REST_INIT_PT_2", new List<List<DeviceAction>>()},
                {"F-14BU:wMsgJ_UTIL_NAV_REST_FIX_PT", new List<List<DeviceAction>>()},
                {"F-14BU:wMsgJ_UTIL_NAV_REST_MN_FIX_PT", new List<List<DeviceAction>>()},
                {"F-14BU:wMsgJ_UTIL_NAV_REST_STGT_1", new List<List<DeviceAction>>()},
                {"F-14BU:wMsgJ_UTIL_NAV_REST_HOME", new List<List<DeviceAction>>()},
                {"F-14BU:wMsgJ_UTIL_NAV_DEF_PNT", new List<List<DeviceAction>>()},
                {"F-14BU:wMsgJ_UTIL_NAV_HSTZONE", new List<List<DeviceAction>>()},
            };

        }
    }
}
