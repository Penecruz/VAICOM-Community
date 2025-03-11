﻿using System;
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
                { "Request Taxi for Takeoff","reqtaxifortakeoff"    },
                { "Taxi Clearance",          "reqtaxifortakeoff"    },
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
                { "Checking In" ,              "wMsgLeaderCheckingIn"                  },// CASE III //fixed? Pene testing
                { "Approach Check In" ,        "wMsgLeaderCheckingIn"                  }, //forced
                { "Platform" ,                 "wMsgLeaderPlatform"                    },// CASE III

                { "Needles" ,                  "wMsgLeaderSayNeedle"                   }, // forced add


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
                { "Switch",                "switch"                },
                { "Select",                 "select"                },
                { "Options",                "options"               },

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
                    // Airboss
                { "Airboss Marshal Check",          "Radio Check Marshal"                             }, //used in Imported script Dynamic F10 menu Commands for Moose Airboss Script
                { "Airboss LSO Check",              "Radio Check LSO"                                 }, //These aliases must be unique and different from the standard DCS aliases!!
                { "Request Commence",               "Request Commence"                                },
                { "Emergency Landing",              "Emergency Landing"                               },

                    // Range
                { "Illuminate the Range",           "Action Illuminate Range"                         },
                { "Smoke the strafe pits",          "Action Smoke Strafe Pits"                        },
                { "Smoke the gunnery targets",      "Action Smoke Strafe Tgts"                        },
                { "Smoke the Bomb targets",         "Action Smoke Bomb Tgts"                          },
                { "I'll be marking blue",           "Action Blue Smoke"                               },
                { "I'll be marking green",          "Action Green Smoke"                              },
                { "I'll be marking orange",         "Action Orange Smoke"                             },
                { "I'll be marking red",            "Action Red Smoke"                                },
                { "I'll be marking white",          "Action White Smoke"                              },

                // Hound TODO
                //{ "Airboss Marshal Check",          "Radio Check Marshal"                             }, //used in Imported script Dynamic F10 menu Commands for the Hound Script
                //{ "Airboss LSO Check",              "Radio Check LSO"                                 }, //These aliases must be unique and different from the standard DCS aliases!!
                //{ "Request Commence",               "Request Commence"                                },
                //{ "Emergency Landing",              "Emergency Landing"                               },

                // AI_ATC
                    //Ground
                { "Request To Start",               "Action Request Engine Start"                     }, //used in Imported script Dynamic F10 menu Commands for the AI_ATC project
                { "Engine Start",                   "Action Request Engine Start"                     }, //These aliases must be unique and different from the standard DCS aliases!!
                { "Requesting Start",               "Action Request Engine Start"                     },
                { "Request Taxi Clearance",         "Action Request Taxi Clearance"                   } ,
                { "Request Taxi",                   "Action Request Taxi Clearance"                   },
                { "Taxi to Active",                 "Action Request Taxi Clearance"                   },
                    //Clearance Delivery
                { "Request a FLEX NORTH",           "Action FLEX NORTH"                               },
                { "Request FLEX NORTH Transition",  "Action FLEX NORTH"                               },
                { "Request a FLEX WEST",            "Action FLEX WEST"                                },
                { "Request FLEX WEST Transition",   "Action FLEX WEST"                                },
                { "Delivery Chaos two one Request FLEX WEST Transition",   "Action FLEX WEST"                                }, // testing long commands
                { "Request a three MIKE",           "Action FLEX MMM TRANSITION"                      },
                { "Request a three M Transition",   "Action FLEX MMM TRANSITION"                      },
                { "Request a Dream Seven",          "Action DREAM 7"                                  },
                { "Request IFR Dream Seven",        "Action DREAM 7"                                  },
                { "Request a FYTTR Seven",          "Action FYTTR 7"                                  },
                { "Request IFR FYTTR Seven",        "Action FYTTR 7"                                  },
                { "Request a fighter Seven",        "Action FYTTR 7"                                  },
                { "Request IFR fighter Seven",      "Action FYTTR 7"                                  },
                { "Request a Mesa Eight",           "Action MORMON MESA 8"                            },
                { "Request IFR Mesa Eight",         "Action MORMON MESA 8"                            },
                { "Say Again last",                 "Action Repeat last transmission"                 },
                    //Tower
                { "Clearance to Cross three Right", "Action Request clearance to cross 03R"           },
                { "Clearance to Cross three Left",  "Action Request clearance to cross 03L"           },
                { "Requesting Takeoff",             "Action Request Takeoff"                          },
                { "Ready in turn",                  "Action Request Takeoff"                          },
                { "Ready number one",               "Action Request Takeoff"                          },
                { "Five Miles",                     "Action Report 5 mile"                            },
                { "Request Straight In",            "Action Straight In"                              },
                { "Request Overhead",               "Action Overhead"                                 },
                { "Request back to departure",      "Action Request Departure"                        },
                { "Request Instrument Final",       "Action Instrument Straight In"                   },
                { "Request back to radar",          "Action Request Back to Radar"                    },
                { "Full Stop",                      "Action Full Stop"                                },
                { "Touch and Go",                   "Action Touch and Go"                             },
                { "Request low approach",           "Action Low Approach"                             },
                { "Request Closed traffic",         "Action Request closed traffic"                   },
                { "Making missed Appraoch",         "Action Missed Approach"                          },
                    //Departure
                { "Departure with you",             "Action Check in"                                 },
                { "Flash",                          "Action Flash"                                    },
                { "Passing FYTTR",                  "Action Report FYTTR"                             },
                { "Passing JUNNO",                  "Action Report JUNNO"                             },
                    //Arrival
                { "Checking In V F R",              "Action VFR"                                      },
                { "Checking In I F R",              "Action IFR"                                      },
                { "Request Hi TACAN Zulu",          "Action HI-TAC Z"                                 },
                { "Request TACAN two one left",     "Action TAC21L"                                   },
                { "Request I L S",                  "Action ILS\\LOC"                                 },
                { "Request Localizer",              "Action ILS\\LOC"                                 },
                { "Request Hi TACAN Yankee",        "Action HI-TAC Y"                                 },
                { "Request I F R Pickup",           "Action Request IFR Pickup"                       },
                { "Cancel I F R",                   "Action Cancel IFR"                               },

                    //Blackjack
                { "Blackjack entry gate one",       "Action Gate 1"                                   },
                { "Blackjack entry gate two",       "Action Gate 2"                                   },
                { "Blackjack entry gate three",     "Action Gate 3"                                   },
                { "Blackjack entry gate four",      "Action Gate 4"                                   },
                { "Blackjack over NIXON",           "Action NIXON"                                    },
                { "Blackjack over SARAH",           "Action SARAH"                                    },
                { "Blackjack Over Beatty",          "Action Beatty"                                   },
                    //NATCF
                { "Sally egress gate one",      "Action Request Egress at gate 1"                 },
                { "Sally egress gate two",      "Action Request Egress at gate 2"                 },
                { "Sally egress gate three",    "Action Request Egress at gate 3"                 },
                { "Sally egress gate four",     "Action Request Egress at gate 4"                 },
                { "Sally egress at NIXON",      "Action Request Egress at NIXON"                  },
                { "Sally egress at SARAH",      "Action Request Egress at SARAH"                  },
                { "Lee egress gate one",      "Action Request Egress at gate 1"                 },
                { "Lee egress gate two",      "Action Request Egress at gate 2"                 },
                { "Lee egress gate three",    "Action Request Egress at gate 3"                 },
                { "Lee egress gate four",     "Action Request Egress at gate 4"                 },
                { "Lee egress at NIXON",      "Action Request Egress at NIXON"                  },
                { "Lee egress at SARAH",      "Action Request Egress at SARAH"                  },

            };

        }
    }

}
