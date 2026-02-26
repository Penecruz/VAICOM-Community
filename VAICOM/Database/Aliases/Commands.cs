using System;
using System.Collections.Generic;

namespace VAICOM
{
    namespace Database
    {

        public static partial class Aliases
        {

            public static Dictionary<string, string> aicommands = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {

                //flight
                { "My Target",              "mytarget"              },
                { "My Contact",             "mytarget"              },
                { "Bandit",                 "bandit"                },
                { "Bandits",                "bandit"                },
                { "Bogey",                  "bandit"                },
                { "Bogeys",                 "bandit"                },
                { "Outlaw",                 "bandit"                },
                { "Outlaws",                "bandit"                },
                { "Hostile",                "bandit"                },
                { "Hostiles",               "bandit"                },
                { "Ground Target",          "groundtarget"          },
                { "Ground Targets",         "groundtarget"          },
                { "Group",                  "groundtarget"          },
                { "Armor",                  "armor"                 },
                { "Tanks",                  "armor"                 },
                { "Movers",                 "armor"                 },
                { "Column",                 "armor"                 },
                { "Artillery",              "artillery"             },
                { "Air Defense",            "airdefense"            },
                { "Air Defenses",           "airdefense"            },
                { "AAA",                    "airdefense"            },
                { "SAM",                    "airdefense"            },
                { "SAMs",                   "airdefense"            },
                { "Utility",                "utility"               },
                { "Utilities",              "utility"               },
                { "Vehicles",               "utility"               },
                { "Trucks",                 "utility"               },
                { "Infantry",               "infantry"              },
                { "Soldiers",               "infantry"              },
                { "Troops",                 "infantry"              },
                { "Ship",                   "ship"                  },
                { "Ships",                  "ship"                  },
                { "Vessel",                 "ship"                  },
                { "Vessels",                "ship"                  },
                { "Skunk",                  "ship"                  },
                { "Skunks",                 "ship"                  },
                { "D-link Target",          "dlinktarget"           },
                { "D-link Targets",         "dlinktargets"          },
                { "D-link Target by Type",  "dlinktargetbytype"     },
                { "D-link Targets by Type", "dlinktargetsbytype"    },
                { "Complete and Rejoin",    "completeandrejoin"     },
                { "Complete and RTB",       "completeandrtb"        },
                { "Ray Target",             "raytarget"             },
                { "My Enemy",               "myenemy"               },
                { "Clear my Six",           "myenemy"               },
                { "Cover Me",               "coverme"               },
                { "Radio Check",            "flightcheckin"         },
                { "Heads Up",               "flightcheckin"         },
                { "Pincer Right",           "pincerright"           },
                { "Pincer Left",            "pincerleft"            },
                { "Pincer High",            "pincerhigh"            },
                { "Pincer Low",             "pincerlow"             },
                { "Break Right",            "breakright"            },
                { "Break Left",             "breakleft"             },
                { "Break High",             "breakhigh"             },
                { "Break Low",              "breaklow"              },
                { "Clear Right",            "clearright"            },
                { "Clear Starboard",        "clearright"            },
                { "Clear Left",             "clearleft"             },
                { "Clear Port",             "clearleft"             },
                { "Pump",                   "pump"                  },
                { "Anchor Here",            "anchorhere"            },
                { "Hold Position",          "anchorhere"            },
                { "Reference My Steerpoint","anchoratsteerpoint"    },
                { "Reference My Spee",      "anchoratspi"           },
                { "Reference Point",        "anchoratpoint"         },
                { "Return To Base",         "returntobase"          },
                { "Go Home",                "returntobase"          },
                { "RTB",                    "returntobase"          },
                { "Go to Tanker",           "flytotanker"           },
                { "Head to the Tanker",     "flytotanker"           },
                { "Rejoin",                 "joinup"                },
                { "Join Up",                "joinup"                },
                { "Join On Me",             "joinup"                },
                { "Fly Route",              "flyroute"              },
                { "Continue",               "flyroute"              },
                { "Kick out to 1 mile",     "makerecon1"            },
                { "Kick out to 2 miles",    "makerecon2"            },
                { "Kick out to 3 miles",    "makerecon3"            },
                { "Kick out to 5 miles",    "makerecon5"            },
                { "Kick out to 8 miles",    "makerecon8"            },
                { "Kick out to 10 miles",   "makerecon10"           },
                { "Check my Spee",          "makereconatpoint"      },
                { "Radar On",               "radaron"               },
                { "Radar Off",              "radaroff"              },
                { "ECM On",                 "ecmon"                 },
                { "Music On",               "ecmon"                 },
                { "ECM Off",                "ecmoff"                },
                { "Music Off",              "ecmoff"                },
                { "Smoke On",               "smokeon"               },
                { "Smoke Off",              "smokeoff"              },
                { "Jettison Stores",        "jettisonweapons"       },
                { "Fence In",               "fencein"               },
                { "Fence Out",              "fenceout"              },
                { "Out Cold",               "out"                   },
                { "Off Cold",               "out"                   },
                { "Go Line Abreast",        "golineabreast"         },
                { "Go Trail",               "gotrail"               },
                { "Go Wedge",               "gowedge"               },
                { "Go Echelon Right",       "goechelonright"        },
                { "Go Starboard",           "goechelonright"        },
                { "Go Echelon Left",        "goechelonleft"         },
                { "Go Port",                "goechelonleft"         },
                { "Go Finger Four",         "gofingerfour"          },
                { "Go Spread Four",         "gospreadfour"          },
                { "Close Formation",        "closeformation"        },
                { "Close Up",               "closeformation"        },
                { "Move Closer",            "closeformation"        },
                { "Keep It Tight",          "closeformation"        },
                { "Open Formation",         "openformation"         },
                { "Open Up",                "openformation"         },
                { "Go Wide",                "openformation"         },
                { "Spread Out",             "openformation"         },
                { "Close Group",            "closegroupformation"   },
                { "Go Heavy",               "helogoheavy"           },
                { "Helos go Echelon",       "helogoechelon"         },
                { "Helos go Spread",        "helogospread"          },
                { "Helos go Trail",         "helogotrail"           },
                { "Go Overwatch",           "helogooverwatch"       },
                { "Go Helo Left",           "helogoleft"            },
                { "Go Helo Right",          "helogoright"           },
                { "Go Helo Tight",          "helogotight"           },
                { "Go Cruise",              "helogocruise"          },
                { "Go Combat",              "helogocombat"          },
                // tac turns
                { "30 Left Go",                   "30leftgo"          },
                { "Thirty Left Go",               "30leftgo"          },
                { "Check Left",                   "30leftgo"          }, // USAF
                { "30 Right Go",                  "30rightgo"         },
                { "Thirty Right Go",              "30rightgo"         },
                { "Check Right",                  "30rightgo"         }, // USAF
                { "45 Left Go",                   "45leftgo"          },
                { "Forty Five Left Go",           "45leftgo"          },
                { "45 Right Go",                  "45rightgo"         },
                { "Forty Five Right Go",          "45rightgo"         },
                { "60 Left Go",                   "60leftgo"          },
                { "Sixty Left Go",                "60leftgo"          },
                { "60 Right Go",                  "60rightgo"         },
                { "Sixty Right Go",               "60rightgo"         },
                { "90 Left Go",                   "90leftgo"          },
                { "Ninety Left Go",               "90leftgo"          },
                { "Tac Left",                     "90leftgo"          }, // USAF
                { "90 Right Go",                  "90rightgo"         },
                { "Ninety Right Go",              "90rightgo"         },
                { "Tac Right",                    "90rightgo"         }, // USAF
                { "Turnabout Left Go",            "turnaboutleftgo"   },
                { "Hook Left",                    "turnaboutleftgo"   }, // USAF
                { "In Place Left",                "turnaboutleftgo"   }, // USN
                { "Turnabout Right Go",           "turnaboutrightgo"  },
                { "Hook Right",                   "turnaboutrightgo"  }, // USAF
                { "In Place Right",               "turnaboutrightgo"  }, // USN
                { "Rotate Go",                    "rotatego"          },
                { "Cross Turn",                   "rotatego"          }, //USAF/USN
                { "Shackle Go",                   "shacklego"         },
                { "Heading North Go",             "HeadingN"          },
                { "Flow North",                   "HeadingN"          },
                { "Heading North East Go",        "HeadingNE"         },
                { "Flow North East",              "HeadingNE"         },
                { "Heading East Go",              "HeadingE"          },
                { "Flow East",                    "HeadingE"          },
                { "Heading South East Go",        "HeadingSE"         },
                { "Flow South East",              "HeadingSE"         },
                { "Heading South Go",             "HeadingS"          },
                { "Flow South",                   "HeadingS"          },
                { "Heading South West Go",        "HeadingSW"         },
                { "Flow South West",              "HeadingSW"         },
                { "Heading West Go",              "HeadingW"          },
                { "Flow West Go",                 "HeadingW"          },
                { "Heading North West Go",        "HeadingNW"         },
                { "Flow North West",              "HeadingNW"         },
                { "Widen Out Go",                 "Widen"             },
                { "Kick Out",                     "Widen"             }, //USAF/USN
                { "Float",                        "Widen"             }, //USN
                { "Close Up Go",                  "CloseUp"           },

                // ATC
                { "Request Engines Start",  "requestenginesstart"   },
                { "Request Startup",        "requestenginesstart"   },
                { "Request Hover",          "requesthover"          },
                { "Request Taxi to Runway", "requesttaxitorunway"   },
                { "Request Taxi for Takeoff","requesttaxitorunway"  },
                { "Taxi Clearance",         "requesttaxitorunway"   },
                { "Request Takeoff",        "requesttakeoff"        },
                { "Request Departure",      "requesttakeoff"        },
                { "Ready for Takeoff",      "requesttakeoff"        },
                { "Ready for Departure",    "requesttakeoff"        },
                { "Tower Request Takeoff",  "towerreqtakeoff"       },
                { "Ready",                  "towerreqtakeoff"       },
                { "Abort Takeoff",          "aborttakeoff"          },
                { "Cancel Departure",       "aborttakeoff"          },
                { "Directions to Final",    "requestazimuth"        },
                { "Vectors to Final",       "requestazimuth"        },
                { "Request Vector",         "requestazimuth"        },
                { "Inbound",                "inbound"               },
                { "Inbound for Landing",    "inbound"               },
                { "Abort Inbound",          "abortinbound"          },
                { "Cancel Approach",        "abortinbound"          },
                { "Request Landing",        "requestlanding"        },                
                { "Request Taxi to Parking","reqtaxitoparking"      },
                { "For parking Stand",      "reqtaxitoparking"      },
                { "Inbound Straight",       "inboundstraight"       },
                { "Overhead Approach",      "approachoverhead"      },
                { "Straight Approach",      "approachstraight"      },
                { "Instrument Approach",    "approachinstrument"    },
                //AWACS
                { "Request Vector to Bullseye", "vectortobullseye"  },
                { "Request Vector to Bandit",   "vectortobandit"    },
                { "Bogey Dope",             "vectortobandit"        },
                { "Request Bogey Dope",     "vectortobandit"        },
                { "Request Vector to Base", "vectortobase"          },
                { "Directions to Base",     "vectortobase"          },
                { "Request Directions to Base",  "vectortobase"     },
                { "Request Vector to Tanker","vectortotanker"       },
                { "Directions to Tanker",   "vectortotanker"        },
                { "Request Directions to Tanker", "vectortotanker"  },
                { "Declare",                "declare"               },
                { "Request Picture",        "requestpicture"        },
                //tanker
                { "Approaching for Refuel", "intenttorefuel"        },
                { "Approaching Nose Cold", "intenttorefuel"         },
                { "Approaching Switches Safe", "intenttorefuel"     },
                { "Abort Refuel",           "abortrefuel"           },
                { "Breakaway",              "abortrefuel"           },
                { "Returning Observation",  "abortrefuel"           },
                { "Ready Precontact",       "readyprecontact"       },
                { "Astern Centre",          "readyprecontact"       },
                { "Astern Left",            "readyprecontact"       },
                { "Astern Right",           "readyprecontact"       },
                { "Stop Refueling",         "stoprefueling"         },
                { "Fuel is Good ",          "stoprefueling"         },
                { "Stop Fuel",              "stoprefueling"         },
                //JTAC
                { "Playtime 5 minutes",     "checkin05"             },
                { "Playtime 10 minutes",    "checkin10"             },
                { "Playtime 15 minutes",    "checkin15"             },
                { "Playtime 20 minutes",    "checkin20"             },
                { "Playtime 25 minutes",    "checkin25"             },
                { "Playtime 30 minutes",    "checkin30"             },
                { "Playtime 35 minutes",    "checkin35"             },
                { "Playtime 40 minutes",    "checkin40"             },
                { "Playtime 45 minutes",    "checkin45"             },
                { "Playtime 50 minutes",    "checkin50"             },
                { "Playtime 55 minutes",    "checkin55"             },
                { "Playtime 60 minutes",    "checkin60"             },
                { "Check Out",              "checkout"              },
                { "Checking Out",           "checkout"              },
                { "Game Over",              "checkout"              },
                { "Ready to Copy",          "readytocopy"           },
                { "Remarks",                "readyforremarks"       },
                { "Say Remarks",            "readyforremarks"       },
                { "Ready for Remarks",      "readyforremarks"       },
                { "Reading Back",           "ninelinereadback"      },
                { "Copy Nine Line",         "ninelinereadback"      },
                { "Request Tasking",        "requesttasking"        },
                { "Ready for Tasking",      "requesttasking"        },
                { "Request New Target",     "requesttasking"        },
                { "Send New Target",        "requesttasking"        },
                { "Request BDA",            "requestbda"            },
                { "Damage Report",          "requestbda"            },
                { "Send Damage Report",     "requestbda"            },
                { "What is my Target",      "requesttargetdescr"    },
                { "Unable",                 "unabletocomply"        },
                { "Showstopper",           "unabletocomply"        },
                { "IP Inbound",             "ipinbound"             },
                { "Copy Miss",              "ipinbound"             },
                { "One Minute",             "oneminute"             },
                { "In Hot",                 "in"                    },
                { "In from the North",      "in"                    },
                { "In from the NorthEast",  "in"                    },
                { "In from the East",       "in"                    },
                { "In from the SouthEast",  "in"                    },
                { "In from the South",      "in"                    },
                { "In from the SouthWest",  "in"                    },
                { "In from the West",       "in"                    },
                { "In from the NorthWest",  "in"                    },
                { "Exit Area",              "off"                   },
                { "Copy Kill",              "attackcomplete"        },
                { "Attack Complete",        "attackcomplete"        },
                { "Advise Ready for BDA",   "advisereadyforbda"     },
                { "Contact",                "contact"               },
                { "Target Visual",          "contact"               },
                { "No Joy",                 "nojoy"                 },
                { "Contact the Mark",       "contactthemark"        },
                { "Tally Mark",             "contactthemark"        },
                { "Tally Smoke",            "contactthemark"        },
                { "Tracking Smoke",         "contactthemark"        },
                { "Sparkle",                "sparkle"               },
                { "Snake",                  "snake"                 },
                { "Pulse",                  "pulse"                 },
                { "Steady",                 "steady"                },
                { "Rope",                   "rope"                  },
                { "Contact Sparkle",        "contactsparkle"        },
                { "Tally Sparkle",          "contactsparkle"        },
                { "Tracking Sparkle",       "contactsparkle"        },
                { "Stop",                   "stop"                  },
                { "Ten Seconds",            "tenseconds"            },
                { "Laser On",               "laseron"               },
                { "Shift Beam",             "shift"                 },
                { "Spot",                   "spot"                  },
                { "Tally Ray",              "spot"                  },
                //{ "Tracking Ray",           "spot"                  },
                { "Tally Beam",             "spot"                  },
                { "Tracking Beam",          "spot"                  },
                { "Terminate",              "terminate"             },
                { "Guns Guns Guns",         "gunsgunsguns"          },
                { "Bombs Away",             "bombsaway"             },
                { "Rifle",                  "rifles"                },
                { "Rockets",                "rockets"               },
                { "Standby for BDA",        "bda"                   },
                { "Standby for Report",     "inflightrep"           },
                { "Request Refuel",         "requestrefueling"      },
                { "Request Refueling",      "requestrefueling"      },
                { "Load Cannon",            "requestcannonreload"   },
                //ground crew
                { "Request Rearming",       "requestrearming"       },
                { "Apply Air",              "applyair"              },
                { "Start valve Open",       "applyair"              },
                { "External Air On",        "applyair"              },
                { "Request Repair",         "requestrepair"         },
                { "Stow the Boarding Ladder","stowboardingladder"   },
                { "Run Inertial Starter",   "runinertialstarter"    },
                { "Request HMD",            "requesthmd"            },
                { "Request NVG",            "requestnvg"            },
                { "Load Water",             "loadwater"             },
                { "Turbo On",               "turboon"               },
                { "Turbo Off",              "turbooff"              },
                { "Ground Power On",        "groundpoweron"         },
                { "External Power On",      "groundpoweron"         },
                { "Ground Power Off",       "groundpoweroff"        },
                { "External Power Off",     "groundpoweroff"        },
                { "Place the Wheelchocks",  "wheelchocksplace"      },
                { "Chocks In",              "wheelchocksplace"      },
                { "Remove the Wheelchocks", "wheelchocksremove"     },
                { "Pull the Chocks",        "wheelchocksremove"     },
                { "Chocks Out",             "wheelchocksremove"     },
                { "Open the Canopy",        "Seq_J_CANOPY_OPEN"     },
                { "Close the Canopy",       "Seq_J_CANOPY_CLOSE"    },
                { "Connect Air Supply",     "airsupplyconnect"      },
                { "Connect Ground Air",     "airsupplyconnect"      },
                { "Disconnect Air Supply",  "airsupplydisconnect"   },
                { "Pull the Ground Air",    "airsupplydisconnect"   },

                // Carrier Comms

                // CASE I
                // --> Marshal

                { "Inbound for Carrier" ,      "wMsgLeaderInboundCarrier"              },// CASE I
                { "Marking Moms" ,             "wMsgLeaderInboundCarrier"              },// CASE I // forced alias
                { "See You At Ten" ,           "wMsgLeaderSeeYouAtTen"                 },// CASE I
                // --> TOWER

                { "Tower Overhead" ,           "wMsgLeaderTowerOverhead"               },
                { "Overhead" ,                 "wMsgLeaderTowerOverhead"               },// forced alias

                //(to flight for break))
                { "Flight Kiss Off" ,          "wMsgLeaderFlightKissOff"               },// CASE I // force remove
                { "Kiss Off" ,                 "wMsgLeaderFlightKissOff"               },// CASE I // forced alias
                
                // --> LSO , in groove

                { "Hornet Ball" ,              "wMsgLeaderHornetBall"                  },
                { "Tomcat Ball" ,              "wMsgLeaderHornetBall"                  },
                { "Hawkeye Ball" ,             "wMsgLeaderHornetBall"                  },
                { "Viking Ball" ,              "wMsgLeaderHornetBall"                  },
                { "Phantom Ball" ,             "wMsgLeaderHornetBall"                  },
                { "Greyhound Ball" ,           "wMsgLeaderHornetBall"                  },
                { "Intruder Ball" ,            "wMsgLeaderHornetBall"                  },
                { "Lightning Ball" ,           "wMsgLeaderHornetBall"                  },
                { "Prowler Ball" ,             "wMsgLeaderHornetBall"                  },
                { "Skyhawk Ball" ,             "wMsgLeaderHornetBall"                  },
                { "Goshawk Ball" ,             "wMsgLeaderHornetBall"                  },

                { "Ball" ,                     "wMsgLeaderHornetBall"                  }, // forced alias

                { "Clara" ,                    "wMsgLeaderCLARA"                       },


                { "Confirm" ,                  "wMsgLeaderConfirm"                     },

                { "Confirm Remaining Fuel" ,   "wMsgLeaderConfirmRemainingFuel"        },
                { "Low State" ,                "wMsgLeaderConfirmRemainingFuel"        },// forced

                { "Inbound Marshall Respond" ,  "wMsgLeaderInboundMarshallRespond"     }, // force remove
                { "Expect On Time" ,            "wMsgLeaderInboundMarshallRespond"     }, // forced
                
                { "Established" ,              "wMsgLeaderEsteblished"                 },
                { "Commencing" ,               "wMsgLeaderCommencing"                  },// CASE III
                { "Checking In" ,              "wMsgLeaderCheckingIn"                  },// CASE III 
                { "Approach Check In" ,        "wMsgLeaderCheckingIn"                  }, //forced
                { "Platform" ,                 "wMsgLeaderPlatform"                    },// CASE III

                { "Needles" ,                  "wMsgLeaderSayNeedle"                   }, // forced add
                { "Up and On" ,                "wMsgLeaderSayNeedle"                   },
                { "Up and Right" ,             "wMsgLeaderSayNeedle"                   },
                { "Up and Left" ,              "wMsgLeaderSayNeedle"                   },

                { "Meatball" ,                 "wMsgLeaderBall"                        },// CASE I //forced

                { "Salute" ,                   "crewsalute"                            },
                { "Request Launch" ,           "crewrequestcatlaunch"                  },

                { "Airborne"   ,                "wMsgLeaderAirborn"                    },
                { "Passing 2 5 Kilo",           "wMsgLeaderPassing2_5Kilo"             },

                // Replies

                { "Roger",                  "roger"                 },
                { "Copy",                   "copy"                  },
                { "Affirm",                 "affirm"                },
                { "Wilco",                  "wilco"                 },
                { "Negative",               "negative"              },
                { "Repeat",                 "repeat"                },
                { "Say Again",              "repeat"                },

                // menu navigation commands
                { "Take 1",                 "menu01"                },
                { "Take 2",                 "menu02"                },
                { "Take 3",                 "menu03"                },
                { "Take 4",                 "menu04"                },
                { "Take 5",                 "menu05"                },
                { "Take 6",                 "menu06"                },
                { "Take 7",                 "menu07"                },
                { "Take 8",                 "menu08"                },
                { "Take 9",                 "menu09"                },
                { "Take 10",                "menu10"                },
                { "Take 11",                "menu11"                },
                { "Take 12",                "menu12"                },
                { "Take One",               "menu01"                },// Try to reduce Flight confusion and accent fix
                { "Take Two",               "menu02"                },
                { "Take Three",             "menu03"                },
                { "Take Four",              "menu04"                },
                { "Take Five",              "menu05"                },
                { "Take Six",               "menu06"                },
                { "Take Seven",             "menu07"                },
                { "Take Eight",             "menu08"                },
                { "Take Nine",              "menu09"                },
                { "Take Ten",               "menu10"                },
                { "Take Eleven",            "menu11"                },
                { "Take Twelve",            "menu12"                },

                // special
                { "Switch",                 "switch"                },
                { "Select",                 "select"                },
                { "Options",                "options"               },
                { "View Loadout",           "loadout"               },

                // aocs
                { "Interrogate",            "state"                 },
                { "Status",                 "state"                 },
                { "Briefing",               "readbriefing"          },

                //kneeboard
                { "Show Notes",            "wMsgKneeboardShowNotes"       },
                { "Clear Notes",           "wMsgKneeboardClearNotes"      },
                { "Start Dictate",         "wMsgKneeboardDictateStart"    },
                { "End Dictate",           "wMsgKneeboardDictateStop"     },
                { "Correction",            "wMsgKneeboardCorrection"      },
                { "Show Log",              "wMsgKneeboardShowLog"         },
                { "Show Tasking Order",    "wMsgKneeboardShowLog"         },

                { "Page",                  "wMsgShowKneeboardTab"         },

                // Moose
                    // Moose Airboss In flight commands
                { "Airboss Mother Comms Check",      "Action Radio Check Marshal"                      }, //Only one istance of a CVN LHA or a Range can be present in the mission.
                { "Mother Comms Check",              "Action Radio Check Marshal"                      }, //used in Imported script Dynamic F10 menu Commands for Moose Airboss Script
                { "Airboss LSO Comms Check",         "Action Radio Check LSO"                          }, //These aliases must be unique and different from the standard DCS aliases!!
                { "LSO Comms Check",                 "Action Radio Check LSO"                          },
                { "Airboss Request Marshal",         "Action Request Marshal"                          },
                { "Inbound Mother Request Marshal",  "Action Request Marshal"                          },
                { "Airboss Commencing",              "Action Request Commence"                         },
                { "Proceeding Initial",              "Action Request Commence"                         },
                { "Passing Platform",                "Action Request Commence"                         },
                { "Airboss Request the tanker",      "Action Request Refueling"                        },
                { "Request To Hit the tanker",       "Action Request Refueling"                        },
                { "Airboss Spinning",                "Action Spinning"                                 },
                { "Spinning",                        "Action Spinning"                                 },
                { "Airboss Emergency Landing",       "Action Emergency Landing"                        },
                { "Declaring an Emergency",          "Action Emergency Landing"                        },
                { "Airboss Training Pattern",        "Action Emergency Landing"                        },
                        //Ship commands used while in flight
                { "Skipper Start Case One",          "Action Start CASE I"                             },
                { "Skipper Start Case Two",          "Action Start CASE II"                            },
                { "Skipper Start Case Three",        "Action Start CASE III"                           },
                { "Skipper Stop Recovery",           "Action Stop Recovery"                            },
                        //Airboss Config Commands ---Do we actually want these included??
                { "Airboss set Flight Student",      "Action Flight Student"                           }, 
                { "Airboss Set Naval Aviator",       "Action Naval Aviator"                            },
                { "Airboss Set Topgun Graduate",     "Action TOPGUN Graduate"                          },
                { "Airboss Hints Toggle",            "Action Hints On/Off"                             },
                { "Airboss subtitles Toggle",        "Action Subtitles On/Off"                         },
                { "Airboss Trap Sheet Toggle",       "Action Trapsheet On/Off"                         },
                { "Airboss Show Greenie Board",      "Action Greenie Board"                            },
                { "Airboss Show my Grades",          "Action My LSO Grades"                            },
                { "Airboss show last debrief",       "Action Last Debrief"                             },
                { "Airboss Information",             "Action Carrier Info"                             },
                { "Airboss Weather",                 "Action Weather Report"                           },
                { "Airboss Set our Group ",          "Action Set Section"                              },
                { "Airboss Set our Section",         "Action Set Section"                              },
                { "Airboss Display Marshal Queue",   "Action Marshal Queue"                            },
                { "Airboss Display Pattern Queue",   "Action Pattern Queue"                            },
                { "Airboss Display Waiting Queue",   "Action Waiting Queue"                            },

                    // Moose Range
                { "Illuminate the Range",            "Action Illuminate Range"                         },
                { "Smoke the strafe pits",           "Action Smoke Strafe Pits"                        },
                { "Smoke the gunnery targets",       "Action Smoke Strafe Tgts"                        },
                { "Smoke the Bomb targets",          "Action Smoke Bomb Tgts"                          },
                { "I'll be marking blue",            "Action Blue Smoke"                               },
                { "I'll be marking green",           "Action Green Smoke"                              },
                { "I'll be marking orange",          "Action Orange Smoke"                             },
                { "I'll be marking red",             "Action Red Smoke"                                },
                { "I'll be marking white",           "Action White Smoke"                              },
                { "Range Weather",                   "Action Weather Report"                           },
                    
                    // Moose AWACS
                { "Chalice Checking In",             "Action Check In"                                 }, // difficult due to conflicts with DCS aliases.
                { "Chalice Say Bogey Dope",          "Action Bogey Dope"                               }, 
                { "Chalice Say Picture",             "Action Picture"                                  }, 
                { "Chalice Declare",                 "Action Declare"                                  },
                { "Chalice request tasking",         "Action Showtask"                                 }, 
                { "Chalice Committing",              "Action Commit"                                   }, 
                { "Chalice Unable",                  "Action Unable"                                   }, 
                { "Chalice off task",                "Action Abort"                                    },
                { "Chalice say status",              "Action Awacs Info"                               }, 
                { "Chalice Checking Out",            "Action Check Out"                                },

                // Hound TODO
                //{ "Place Holder",                    "Place Holder"                                  }, //used in Imported script Dynamic F10 menu Commands for the Hound Script
                //{ "Place Holder",                    "Place Holder"                                  }, //These aliases must be unique and different from the standard DCS aliases!!
                //{ "Place Holder",                    "Place Holder"                                  }, //Hound is tricky due to the number of dynamic commands what is important?
                //{ "Place Holder",                    "Place Holder"                                  },

                // Campaign Pacific Sentry Testing WIP
                //M02 A Red Horizon
                { "I don't see any cruise missiles",     "Action I dont see any cruise missiles"         }, //used in Imported script Dynamic F10 menu Commands for the Sentry Pacific Campaign
                { "Yes I see cruise missiles!",          "Action Yes I see cruise missiles!"             }, //These aliases must be unique and different from the standard DCS aliases!!
                { "Captured Bogey Bull 045 120",         "Action Captured Bogey Bull 045/120"            },
                { "Captured Bogey Bull 055 180",         "Action Captured Bogey Bull 055/180"            },
                { "Venom is Bingo Fuel and need to RTB", "Action Venon is Bingo Fuel and need to RTB"    },
                { "Venom is declaring emergency fuel!",  "Action Venom is declaring emergency fuel!"     },
                { "We can hold for a few more minutes",  "Action Venom is min fuel - we can hold for a few more minutes"  },
                //M11 Work Hard,Play hard
                { "Incentive Ride Over Testing Only",    "Action INCENTIVE RIDE OVER TESTING ONLY"       }, 
                //{ "Place Holder",                    "Place Holder"                                  },

                // AI_ATC
                        // Transition Repeat Last
                                                                                                          //used in Imported script Dynamic F10 menu Commands for the AI_ATC project
                { "Say Again last",                 "Action Repeat last transmission"                 }, //These aliases must be unique and different from the standard DCS aliases!!

                       //Clearance Delivery
                        //IFR Sub menu
                { "Request a FLEX NORTH",           "Action FLEX NORTH"                               },
                { "Request FLEX NORTH Transition",  "Action FLEX NORTH"                               },
                { "Request a FLEX WEST",            "Action FLEX WEST"                                },
                { "Request FLEX WEST Transition",   "Action FLEX WEST"                                },
                { "Request a three MIKE",           "Action FLEX MMM TRANSITION"                      },
                { "Request a three M Transition",   "Action FLEX MMM TRANSITION"                      },
                { "Request a Dream Seven",          "Action DREAM 7"                                  },
                { "Request I F R Dream Seven",      "Action DREAM 7"                                  },
                { "Request a FYTTR Seven",          "Action FYTTR 7"                                  },
                { "Request I F R FYTTR Seven",      "Action FYTTR 7"                                  },
                { "Request a fighter Seven",        "Action FYTTR 7"                                  },
                { "Request I F R fighter Seven",    "Action FYTTR 7"                                  },
                { "Request a Mesa Eight",           "Action MORMON MESA 8"                            },
                { "Request I F R Mesa Eight",       "Action MORMON MESA 8"                            },
                        //VFR Sub Menu
                { "Request V F R for GASS PEAK",    "Action GASS_PEAK"                                },
                { "Request V F R for DRY LAKE",     "Action DRY_LAKE"                                 },
                { "Request V F R for RED HORSE",    "Action RED_HORSE"                                },
                { "Request V F R for SUNRISE",      "Action SUNRISE"                                  },

                        //Clearance Readback
                { "Squawk One Zero Zero One",       "Action Squawk 1001"                              },
                { "Squawk Two Zero Zero One",       "Action Squawk 2001"                              },
                { "Squawk Three Zero Zero One",     "Action Squawk 3001"                              },
                { "Squawk Four Zero Zero One",      "Action Squawk 4001"                              },
                { "Squawk Five Zero Zero One",      "Action Squawk 5001"                              },
                { "Squawk Six Zero Zero One",       "Action Squawk 6001"                              },
                { "Squawk Seven Zero Zero One",     "Action Squawk 7001"                              },

                    //Ground
                { "Request To Start Engines",       "Action Request Engine Start"                     },
                { "Engine Start",                   "Action Request Engine Start"                     },
                { "Requesting Start",               "Action Request Engine Start"                     },
                { "Requesting Taxi Clearance",      "Action Request Taxi Clearance"                   } ,
                { "Requesting Taxi to Active",      "Action Request Taxi Clearance"                   },
                { "Taxi to Active",                 "Action Request Taxi Clearance"                   },
                { "Clear of Runway",                "Action Taxi to Parking"                          },
                { "Ground with you",                "Action Taxi to Parking"                          },                

                    //Tower
                      //Runway Crossing
                { "Clearance to Cross three Right", "Action Request clearance to cross 03R"           }, //Nellis
                { "Clearance to Cross three Left",  "Action Request clearance to cross 03L"           }, //Nellis
                { "Clearance to Cross Two One Left","Action Request clearance to cross 21L"           }, //Nellis
                { "Clearance to Cross Two One Right","Action Request clearance to cross 21R"          }, //Nellis
                { "Clearance to Cross Six Left",    "Action Request clearance to cross 06L"           }, //Andersen
                { "Clearance to Cross Six Right",   "Action Request clearance to cross 06R"           }, //Andersen
                { "Clearance to Cross Two Four Left", "Action Request clearance to cross 24L"         }, //Andersen
                { "Clearance to Cross Two Four Right","Action Request clearance to cross 24R"         }, //Andersen
                    //Takeoff
                { "Requesting Takeoff Clearance",   "Action Request Takeoff"                          },
                { "Ready in turn",                  "Action Request Takeoff"                          },
                { "Ready at the Hold",              "Action Request Takeoff"                          },
                    //Request Unrestricted climb
                { "Requesting Unrestricted Climb 5000",                               "Action 5000"   },
                { "Requesting Unrestricted Climb 6000",                               "Action 6000"   },
                { "Requesting Unrestricted Climb 7000",                               "Action 7000"   },
                { "Requesting Unrestricted Climb 8000",                               "Action 8000"   },
                { "Requesting Unrestricted Climb 9000",                               "Action 9000"   },
                { "Requesting Unrestricted Climb 10000",                              "Action 10000"  },
                { "Requesting Unrestricted Climb 11000",                              "Action 11000"  },
                { "Requesting Unrestricted Climb 12000",                              "Action 12000"  },
                { "Requesting Unrestricted Climb 13000",                              "Action 13000"  },
                { "Requesting Unrestricted Climb 14000",                              "Action 14000"  },
                { "Requesting Unrestricted Climb 15000",                              "Action 15000"  },
                { "Requesting Unrestricted Climb 16000",                              "Action 16000"  },
                { "Requesting Unrestricted Climb 17000",                              "Action 17000"  },
                { "Requesting Unrestricted Climb One Eight Zero",                     "Action FL180"  },
                { "Requesting Unrestricted Climb One Nine Zero",                      "Action FL190"  },
                { "Requesting Unrestricted Climb Two Zero Zero",                      "Action FL200"  },
                { "Requesting Unrestricted Climb Two One Zero",                       "Action FL210"  },
                { "Requesting Unrestricted Climb Two Two Zero",                       "Action FL220"  },
                { "Requesting Unrestricted Climb Two Three Zero",                     "Action FL230"  },
                { "Requesting Unrestricted Climb Two Four Zero",                      "Action FL240"  },
                { "Requesting Unrestricted Climb Two Five Zero",                      "Action FL250"  },
                { "Requesting Unrestricted Climb Two Six Zero",                       "Action FL260"  },
                { "Requesting Unrestricted Climb Two Seven Zero",                     "Action FL270"  },
                { "Requesting Unrestricted Climb Two Eight Zero",                     "Action FL280"  },
                { "Requesting Unrestricted Climb Two Nine Zero",                      "Action FL290"  },
                { "Requesting Unrestricted Climb Three Zero Zero",                    "Action FL300"  },
                { "Requesting Unrestricted Climb Three One Zero",                     "Action FL310"  },
                { "Requesting Unrestricted Climb Three Two Zero",                     "Action FL320"  },
                { "Requesting Unrestricted Climb Three Three Zero",                   "Action FL330"  },
                { "Requesting Unrestricted Climb Three Four Zero",                    "Action FL340"  },
                     //Tower Landing Sub Menu
                { "Request Straight In",            "Action Straight In"                              },
                { "Request Overhead",               "Action Overhead"                                 },
                { "Simulated Flame Out",            "Action SFO"                                      },// Simulated Flameout 
                { "P A R Rollout",                  "Action PAR"                                      },// Precision Approach Radar
                { "I L S Final",                  "Action ILS"                                      },
                    //Tower Options Sub Menu
                { "Request Low Approach",           "Action Low Approach"                             },
                { "Request back to departure",      "Action Request Departure"                        },
                { "Request Back to Radar",          "Action Request Back to Radar"                    },
                { "Request Instrument Final",       "Action Instrument Straight In"                   },
                    //Tower Report responses
                { "Five Miles To land",             "Action Full Stop"                                },
                { "Five Miles Touch and Go",        "Action Touch and Go"                             },
                { "Five Miles Request low approach","Action Low Approach"                             },
                { "Base To Land",                   "Action Full Stop"                                },
                { "Base Touch and Go",              "Action Touch and Go"                             },
                { "Base Request low approach",      "Action Low Approach"                             },
                { "Request Closed traffic",         "Action Request closed traffic"                   },
                { "Making Missed Approach",         "Action Missed Approach"                          },
                { "Request DUCK",                   "Action Request DUCK"                             },
                { "Request FLEX",                   "Action Request FLEX"                             },
                { "Passing DUCK",                   "Action Report DUCK"                              },
                { "Passing FLEX",                   "Action Report FLEX"                              },
                { "High Key",                       "Action Report High Key"                          },//use with Simulated Flameout
                { "Low Key",                        "Action Report Low Key"                           },//use with Simulated Flameout
                    //Tower Ancillary
                { "Traffic in sight",               "Action Traffic in sight"                         },
                { "Nothing Seen",                   "Action No Joy"                                   },
                { "Roger Extend Downwind",          "Action Acknowledge Extend Downwind"              },
                { "Say Winds",                      "Action Say Winds"                                },
                { "Break Out",                      "Action Break Out"                                },

                    //Departure
                { "Departure with you",             "Action Check in"                                 },
                { "Flash",                          "Action Flash"                                    },
                { "Request back to Approach",       "Action Request Approach"                         },

                    //Approach
                       //Check In
                { "Checking In V F R",                 "Action VFR"                                   },
                { "Checking In I F R",                 "Action IFR"                                   },
                { "Checking In Again",                 "Action Check In"                              },//Back to radar after tower
                       //IFR Sub Menu
                { "Request Hi TACAN Zulu",          "Action HI-TAC Z"                                 },
                { "Request TACAN Two One left",     "Action TAC21L"                                   },
                { "Request I L S Approach",         "Action ILS\\LOC"                                 },
                { "Request Localizer Approach",     "Action ILS\\LOC"                                 },
                { "Request Hi TACAN Yankee",        "Action HI-TAC Y"                                 },
                { "Cancel I F R",                   "Action Cancel IFR"                               },
                { "Request G C A",                  "Action Request GCA"                              },// for PAR
    
                    //Approach NAV Assist
                     // VFR Nav points
                { "Vector for NIXON",                "Action Vector for NIXON"                         },
                { "Vector for APEX",                 "Action Vector for APEX"                          },
                { "Vector for PHINN",                "Action Vector for PHINN"                         },
                { "Vector for GASS_PEAK",            "Action Vector for GASS_PEAK"                     },
                { "Vector for SARAH",                "Action Vector for SARAH"                         },
                { "Vector for STRYK",                "Action Vector for STRYK"                         },
                { "Vector for ACTON",                "Action Vector for ACTON"                         },
                { "Vector for ARCOE",                "Action Vector for ARCOE"                         },
                { "Vector for FLEX",                 "Action Vector for FLEX"                          },
                { "Vector for DRY LAKE",             "Action Vector for DRY_LAKE"                      },
                { "Vector for NORTH GAP",            "Action Vector for NORTH_GAP"                     },
                { "Vector for SAND DUNES",           "Action Vector for SAND_DUNES"                    },
                { "Vector for I Fifteen",            "Action Vector for I Fifteen"                     },
                { "Vector for SOUTH GAP",            "Action Vector for SOUTH_GAP"                     },
                { "Vector for FYTTR",                "Action Vector for FYTTR"                         },
                { "Vector for DREAM",                "Action Vector for DREAM"                         },
                { "Vector for MMM",                  "Action Vector for MMM"                           },

                    // VFR Report points
                { "Passing STRYK",                  "Action Report STRYK"                             },
                { "Passing ACTON",                  "Action Report ACTON"                             },
                { "Passing ARCOE",                  "Action Report ARCOE"                             },
                { "Passing DUDBE",                  "Action Report DUDBE"                             },
                { "Passing GASS PEAK",              "Action Report GASS_PEAK"                         },
                { "Passing Dry Lake",               "Action Report DRY_LAKE"                          },
                { "Passing Red Horse",              "Action Report RED_HORSE"                         },
                { "Passing Sunrise",                "Action Report SUNRISE"                           },
                { "Passing FYTTR",                  "Action Report FYTTR"                             },
                { "Passing JUNNO",                  "Action Report JUNNO"                             },
                { "Passing DREAM",                  "Action Report DREAM"                             },
                { "Passing MMM",                    "Action Report MMM"                               },
                { "Passing Three Mike",             "Action Report MMM"                               },
                { "Passing SAND DUNES",             "Action Report SAND_DUNES"                        },

                    // IFR Nav points
                { "Vectors for ACTON",               "Action Vectors for ACTON"                        },
                { "Vectors for DUDBE",               "Action Vectors for DUDBE"                        },
                { "Vectors for SECRT",               "Action Vectors for SECRT"                        },
                { "Vectors for HOKUM",               "Action Vectors for HOKUM"                        },
                { "Vectors for HULPU",               "Action Vectors for HULPU"                        },
                { "Vectors for JENAR",               "Action Vectors for JENAR"                        },
                { "Vectors for JOGEV",               "Action Vectors for JOGEV"                        },
                { "Vectors for WILIE",               "Action Vectors for WILIE"                        },
                { "Vectors for KITCH",               "Action Vectors for KITCH"                        },
                { "Vectors for ARCOE",               "Action Vectors for ARCOE"                        },
                { "Vectors for RONKY",               "Action Vectors for RONKY"                        },
                { "Vectors for WISTO",               "Action Vectors for WISTO"                        },
                { "Vectors for OLNIE",               "Action Vectors for OLNIE"                        },
                { "Vectors for KUTME",               "Action Vectors for KUTME"                        },
                { "Vectors for SHEET",               "Action Vectors for SHEET"                        },
                { "Vectors for ROTSE",               "Action Vectors for ROTSE"                        },
                { "Vectors for JELIR",               "Action Vectors for JELIR"                        },
                { "Vectors for CADOS",               "Action Vectors for CADOS"                        },
                { "Vectors for SESDE",               "Action Vectors for SESDE"                        },
                { "Vectors for KRYSS",               "Action Vectors for KRYSS"                        },
                { "Vectors for HEREM",               "Action Vectors for HEREM"                        },
                { "Vectors for JUNNO",               "Action Vectors for JUNNO"                        },
                { "Vectors for ATALF",               "Action Vectors for ATALF"                        },
                { "Vectors for CUVAX",               "Action Vectors for CUVAX"                        },
                { "Vectors for ZINAX",               "Action Vectors for ZINAX"                        },

                    //Vectors to Airports Nevada
                { "Request Vectors for Beatty",              "Action Beatty"                          },
                { "Request Vectors for Boulder City",        "Action Boulder City"                    },
                { "Request Vectors for Creech",              "Action Creech"                          },
                { "Request Vectors for Echo Bay",            "Action Echo Bay"                        },
                { "Request Vectors for Groom Lake",          "Action Groom Lake"                      },
                { "Request Vectors for Henderson Executive", "Action Henderson Executive"             },
                { "Request Vectors for Jean",                "Action Jean"                            },
                { "Request Vectors for Laughlin",            "Action Laughlin"                        },
                { "Request Vectors for Lincoln County",      "Action Lincoln County"                  },
                { "Request Vectors for McCarran International", "Action McCarran International"       },
                { "Request Vectors for Mesquite",            "Action Mesquite"                        },
                { "Request Vectors for Mina",                "Action Mina"                            },
                { "Request Vectors for Nellis",              "Action Nellis"                          },
                { "Request Vectors for North Las Vegas",     "Action North Las Vegas"                 },
                { "Request Vectors for Pahute Mesa",         "Action Pahute Mesa"                     },
                { "Request Vectors for Tonopah",             "Action Tonopah"                         },
                { "Request Vectors for Tonopah Test Range",  "Action Tonopah Test Range"              },

                    //Range Control
                      //Blackjack Entry/Exit
                { "Blackjack entry gate one",       "Action Check in Gate 1"                          },
                { "Blackjack entry gate two",       "Action Check in Gate 2"                          },
                { "Blackjack entry gate three",     "Action Check in Gate 3"                          },
                { "Blackjack entry gate four",      "Action Check in Gate 4"                          },
                { "Blackjack entry NIXON",          "Action Check in NIXON"                           },
                { "Blackjack entry SARAH",          "Action Check in SARAH"                           },
                { "Blackjack exit gate one",        "Action Check Out Gate 1"                         },
                { "Blackjack exit gate two",        "Action Check Out Gate 2"                         },
                { "Blackjack exit gate three",      "Action Check Out Gate 3"                         },
                { "Blackjack exit gate four",       "Action Check Out Gate 4"                         },
                { "Blackjack exit NIXON",           "Action Check Out NIXON"                          },
                { "Blackjack exit SARAH",           "Action Check Out SARAH"                          },
                   // Request vectors from Blackjack
                { "Blackjack vector to gate one",   "Action Vector to Gate 1"                         },
                { "Blackjack vector to gate two",   "Action Vector to Gate 2"                         },
                { "Blackjack vector to NIXON",      "Action Vector to NIXON"                          },
                { "Blackjack vector to SARAH",      "Action Vector to SARAH"                          },
                { "Blackjack vector to gate three", "Action Vector to Gate 3"                         },
                { "Blackjack vector to gate four",  "Action Vector to Gate 4"                         },


                   //NATCF
                    // Report egressing
                { "Sally Gate One Outbound",        "Action Request Egress at gate 1"                 },
                { "Sally Gate Two Outbound",        "Action Request Egress at gate 2"                 },
                { "Sally Gate Three Outbound",      "Action Request Egress at gate 3"                 },
                { "Sally Gate Four Outbound",       "Action Request Egress at gate 4"                 },
                { "Sally NIXON Outbound",           "Action Request Egress at NIXON"                  },
                { "Sally SARAH Outbound",           "Action Request Egress at SARAH"                  },
                { "Lee Gate One Outbound",          "Action Request Egress at gate 1"                 },
                { "Lee Gate Two Outbound",          "Action Request Egress at gate 2"                 },
                { "Lee Gate Three Outbound",        "Action Request Egress at gate 3"                 },
                { "Lee Gate Four Outbound",         "Action Request Egress at gate 4"                 },
                { "Lee NIXON Outbound",             "Action Request Egress at NIXON"                  },
                { "Lee SARAH Outbound",             "Action Request Egress at SARAH"                  },
                   //NATCF Request vectors
                { "Vector to Gate 1",               "Action Vector to Gate_1"                         },
                { "Vector to FLUSH",                "Action Vector to FLUSH"                          },
                { "Vector to JAYSN",                "Action Vector to JAYSN"                          },
                { "Vector to STRYK",                "Action Vector to STRYK"                          },
                { "Vector to Gate 2",               "Action Vector to Gate_2"                         },
                { "Vector to BTY",                  "Action Vector to BTY"                            },
                { "Vector to Gate 3",               "Action Vector to Gate_3"                         },
                { "Vector to DREAM",                "Action Vector to DREAM"                          },
                { "Vector to ARCOE",                "Action Vector to ARCOE"                          },
                { "Vector to Gate 4",               "Action Vector to Gate_4"                         },
                { "Vector to ELKXX",                "Action Vector to ELKXX"                          },
                { "Vector to MOOSE",                "Action Vector to MOOSE"                          },
                { "Vector to ACTON",                "Action Vector to ACTON"                          },
                { "Vector to SARAH",                "Action Vector to SARAH"                          },
                { "Vector to GASS PEAK",            "Action Vector to GASS_PEAK"                      },
                { "Vector to NIXON",                "Action Vector to NIXON"                          },
                { "Vector to BLANK",                "Action Vector to BLANK"                          },
            //};

            //public static Dictionary<string, string> cockpitcontrol = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            //{

                /// Direct Cockpit Commnads
                { "Gear Up",                          "gearup"                                          },
                { "Gear Down",                        "geardown"                                        },
                //{ "View Loadout",                     "loadout"                                         },
                //{ "Emergency Landing",               "Emergency Landing"                                  },

            };

        }
    }

}
