﻿using System;
using System.Collections.Generic;

namespace VAICOM
{
    namespace Database
    {

        public static partial class Aliases
        {

            public static Dictionary<string, string> airecipients = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
            
                // flight

                { "Flight",                 "flight"                },
                { "Element",                "element"               },
                { "Section",                "element"               },
                { "One",                    "wingman1"              },
                { "Lead",                   "wingman1"              },
                { "Winger",                 "wingman2"              },
                { "Two",                    "wingman2"              },
                { "Three",                  "wingman3"              },
                { "Four",                   "wingman4"              },

                { "Gopher",                 "wingman2"              },
                { "Pyro",                   "wingman3"              },
                { "Bozo",                   "wingman4"              },

                // JTAC

                { "JTAC",                   "jtac"                  },
                { "Patrol",                 "jtac"                  },
                { "Operator",               "jtac"                  },

                { "Axeman",                 "axeman"                },
                { "Darknight",              "darknight"             },
                { "Eyeball",                "eyeball"               },
                { "Finger",                 "finger"                },
                { "Firefly",                "firefly"               },
                { "Moonbeam",               "moonbeam"              },
                { "Playboy",                "playboy"               },
                { "Pointer",                "pointer"               },
                { "Warrior",                "warrior"               },
                { "Whiplash",               "whiplash"              },

                { "Pinpoint",               "pinpoint"              },
                { "Ferret",                 "ferret"                },
                { "Shaba",                  "shaba"                 },
                { "Hammer",                 "hammer"                },
                { "Jaguar",                 "jaguar"                },
                { "Deathstar",              "deathstar"             },
                { "Anvil",                  "anvil"                 },
                { "Mantis",                 "mantis"                },
                { "Badger",                 "badger"                },

                { "Boar",                   "boar"                  },
                { "Chevy",                  "chevy"                 },
                { "Colt",                   "colt"                  },
                { "Dodge",                  "dodge"                 },
                { "Enfield",                "enfield"               },
                { "Ford",                   "ford"                  },
                { "Hawg",                   "hawg"                  },
                { "Pig",                    "pig"                   },
                { "Pontiac",                "pontiac"               },
                { "Springfield",            "springfield"           },
                { "Tusk",                   "tusk"                  },
                { "Uzi",                    "uzi"                   },

                { "Nearest Patrol",         "nearestjtac"           },

                //ATC

                { "ATC",                    "atc"                   },
                { "Tower",                  "atc"                   },
                { "Traffic",                "atc"                   },
                { "Nearest ATC",            "nearestairfield"       },
                { "Nearest Airfield",       "nearestairfield"       },
                { "Proxy",                  "nearestairfield"       },
                //{ "Alternate",              "alternateatc"          },

                // caucasus

                { "Anapa",                  "Anapa-Vityazevo"       },
                { "Batumi",                 "Batumi"                },
                { "Beslan",                 "Beslan"                },
                { "Gelendzhik",             "Gelendzhik"            },
                { "Gudauta",                "Gudauta"               },
                { "Khanskaya",              "Maykop-Khanskaya"      },
                { "Kobuleti",               "Kobuleti"              },
                { "Kolkhi",                 "Senaki-Kolkhi"         },
                { "Senaki",                 "Senaki-Kolkhi"         },
                { "Krasnodar",              "Krasnodar-Center"      },
                { "Krymsk",                 "Krymsk"                },
                { "Kutaisi",                "Kutaisi"               },
                { "Lochini",                "Tbilisi-Lochini"       },
                { "Tbilisi",                "Tbilisi-Lochini"       },
                { "Minvody",                "Mineralnye Vody"       },
                { "Mozdok",                 "Mozdok"                },
                { "Nalchik",                "Nalchik"               },
                { "Novorossiysk",           "Novorossiysk"          },
                { "Pashkovsky",             "Krasnodar-Pashkovsky"  },
                { "Sochi",                  "Sochi-Adler"           },
                { "Soganlug",               "Soganlug"              },
                { "Sukhumi",                "Sukhumi-Babushara"     },
                { "Vaziani",                "Vaziani"               },
                
                // Marianas

                { "Andersen",               "Andersen AFB"             },
                { "Boonie",                 "Andersen AFB"             },
                { "Guam",                   "Antonio B. Won Pat Intl"  },
                { "Agana Tower",            "Antonio B. Won Pat Intl"  },
                { "Guam Centre",            "Antonio B. Won Pat Intl"  },
                { "Rota",                   "Rota Intl"                },
                { "Tinian",                 "Tinian Intl"              },
                { "Saipan",                 "Saipan Intl"              },

                // Nevada

                { "Creech",                 "Creech"                },
                { "Indian Springs",         "Creech"                },
                { "North Las Vegas",        "North Las Vegas"       },
                { "Graceland",              "North Las Vegas"       },
                { "Henderson",              "Henderson Executive"   },
                { "McCarran",               "McCarran International"},
                { "Las Vegas",              "McCarran International"},
                { "Laughlin",               "Laughlin"              },
                { "Bullhead",               "Laughlin"              },
                { "Tonopah",                "Tonopah Test Range"    },
                { "Silverbow",              "Tonopah Test Range"    },
                { "Groom Lake",             "Groom Lake"            },
                { "Dreamland",              "Groom Lake"            },
                { "Nellis",                 "Nellis"                },
                { "NellisControl",          "Nellis"                },
                { "Boulder",                "Boulder City"          },
                { "Boulder City",           "Boulder City"          },

                // Normandy

                { "Beny",                   "Beny-sur-Mer"                  },
                { "Sainte Croix",           "Sainte-Croix-sur-Mer"          },
                { "Lantheuil",              "Lantheuil"                     },
                { "Bazenville",             "Bazenville"                    },
                { "Sommervieu",             "Sommervieu"                    },
                { "Longues",                "Longues-sur-Mer"               },
                { "Molay",                  "Le Molay"                      },
                { "Saint Laurent",          "Sainte-Laurent-sur-Mer"        },
                { "Saint Pierre",           "Saint Pierre du Mont"          },
                { "Deux Jumeaux",           "Deux Jumeaux"                  },
                { "Chippelle",              "Chippelle"                     },
                { "Cricqueville",           "Cricqueville-en-Bessin"        },
                { "Cardonville",            "Cardonville"                   },
                { "Brucheville",            "Brucheville"                   },
                { "Meautis",                "Meautis"                       },
                { "Azeville",               "Azeville"                      },
                { "Cretteville",            "Cretteville"                   },
                { "Picauville",             "Picauville"                    },
                { "Biniville",              "Biniville"                     },
                { "Lessay",                 "Lessay"                        },
                { "Maupertus",              "Maupertus"                     },
                { "Evreux",                 "Evreux"                        },
                { "Forde",                  "Forde"                         },
                { "Tangmere",               "Tangmere"                      },
                { "Funtington",             "Funtington"                    },
                { "Chailey",                "Chailey"                       },
                { "Needs Oar",              "Needs Oar Point"               },

                // Persian Gulf

                { "Al Maktoum",             "Al Maktoum Intl"               },
                { "OMDW",                   "Al Maktoum Intl"               },
                { "Al Minhad",              "Al Minhad AFB"                 },
                { "OMDM",                   "Al Minhad AFB"                 },
                { "Dubai",                  "Dubai Intl"                    },
                { "DubaiControl",           "Dubai Intl"                    },
                { "Sharjah",                "Sharjah Intl"                  },
                { "OMSJ",                   "Sharjah Intl"                  },
                { "Abu Musa",               "Abu Musa Island"               },
                { "Sirri",                  "Sirri Island"                  },
                { "Sirri Island",           "Sirri Island"                  },
                { "Fujairah",               "Fujairah Intl"                 },
                { "OMFJ",                   "Fujairah Intl"                 },
                { "Bandar Lengeh",          "Bandar Lengeh"                 },
                { "Lengeh",                 "Bandar Lengeh"                 },
                { "Khasab",                 "Khasab"                        },
                { "Qeshm Island",           "Qeshm Island"                  },
                { "Havadarya",              "Havadarya"                     },
                { "Bandar Abbas",           "Bandar Abbas Intl"             },
                { "OIKB",                   "Bandar Abbas Intl"             },
                { "Lar Airbase",            "Lar"                           },
                { "Lar",                    "Lar"                           },
                { "Kerman Airport",         "Kerman"                        },
                { "KERMAN",                 "Kerman"                        },
                { "Shiraz International",   "Shiraz Intl"                   },
                { "Shiraz",                 "Shiraz Intl"                   },
                { "Al Dhafra",              "Al Dhafra AFB"                 },
                { "OMAM",                   "Al Dhafra AFB"                 },
                { "Al Bateen",              "Al-Bateen"                     },
                { "OMAD",                   "Al-Bateen"                     },
                { "Kish",                   "Kish Intl"                     },
                { "Lavan Island",           "Lavan Island"                  },
                { "LAV",                    "Lavan Island"                  },
                { "Al Ain International",   "Al Ain Intl"                   },
                { "OMAL",                   "Al Ain Intl"                   },
                { "Bandar e Jask",          "Bandar-e-Jask"                 },
                { "Abu Dhabi",              "Abu Dhabi Intl"                },
                { "OMAA",                   "Abu Dhabi Intl"                },
                { "Sas Al Nakheel",         "Sas Al Nakheel"                },
                { "OMNK",                   "Sas Al Nakheel"                },
                { "Jiroft",                 "Jiroft"                        },
                { "OIJK",                   "Jiroft"                        },
                { "Liwa Airbase",           "Liwa AFB"                      },
                { "Al Safran",              "Liwa AFB"                      },
                { "Ras Al Khaimah",         "Ras Al Khaimah Intl"           },
                { "Ras Tower",              "Ras Al Khaimah Intl"           },
                { "Sir Abu Nuayr",          "Sir Abu Nuayr"                 },

                //Channel map
                
                { "Dunkirk",                "Dunkirk Mardyck"        },
                { "Hawkinge",               "Hawkinge"               },
                { "Saint Omer",             "Saint Omer Longuenesse" },
                { "Calonne",                "Merville Calonne"       },
                { "High Halden",            "High Halden"            },
                { "Detling",                "Detling"                },
                { "Drucat",                 "Abbeville Drucat"       },
                { "Lympne",                 "Lympne"                 },
                { "Manston",                "Manston"                },

                // Syria map

                { "Hariri",                 "Beirut-Rafic Hariri"   },
                { "Beirut",                 "Beirut-Rafic Hariri"   },
                { "Riyaq",                  "Rayak"                 },
                { "Hamat",                  "Wujah Al Hajar"        },
                { "Kiryat",                 "Kiryat Shmona"         },
                { "Mezzeh",                 "Mezzeh"                },
                { "Qabr as Sitt",           "Qabr as Sitt"          },
                { "Rene Mouawad",           "Rene Mouawad"          },
                { "Marj as Sultan",         "Marj as Sultan North"  },
                { "Der Salman",             "Marj as Sultan South"  },
                { "Marj Ruhayyil",          "Marj Ruhayyil"         },
                { "Dumayr",                 "Al-Dumayr"             },
                { "Haifa",                  "Haifa"                 },
                { "An Nasiriyah",           "An Nasiriyah"          },
                { "Al Qusayr",              "Al Qusayr"             },
                { "Khalkhalah",             "Khalkhalah"            },
                { "Ramat David",            "Ramat David"           },
                { "Megiddo",                "Megiddo"               },
                { "Eyn Shemer",             "Eyn Shemer"            },
                { "Shemer",                 "Eyn Shemer"            },
                { "Latakia",                "Bassel Al-Assad"       },
                { "Abu al Duhur",           "Abu al-Duhur"          },
                { "Taftanaz",               "Taftanaz"              },
                { "Hatay",                  "Hatay"                 },
                { "Rasin",                  "Kuweires"              },
                { "Minakh",                 "Minakh"                },
                { "Jirah",                  "Jirah"                 },
                { "Adana",                  "Adana Sakirpasa"       },
                { "Incirlik",               "Incirlik"              },
                { "Damascus",               "Damascus"              }, //new additions
                { "Thalah",                 "Tha'lah"               },
                { "Larnaca",                "Larnaca"               },
                { "Akrotiri",               "Akrotiri"              },
                { "King Hussein",           "King Hussein Air College"  },
                { "At Tanf",                "At Tanf"               },
                { "H 3",                    "H3"                    },
                { "H 3 Northwest",          "H3 Northwest"          },
                { "H 3 Southwest",          "H3 Southwest"          },
                { "Paphos",                 "Paphos"                },
                { "Lakatamia",              "Lakatamia"             },
                { "Ercan",                  "Ercan"                 },

                // Sinai map

                { "Al Mansurah",            "Al Mansurah"           },
                { "AzZaqaziq",              "AzZaqaziq"             },
                { "Salihiyah",              "As Salihiyah"          },
                { "Bilbeis Air Base",       "Bilbeis Air Base"      },
                { "Bilbeis",                "Bilbeis Air Base"      },
                { "Inshas Airbase",         "Inshas Airbase"        },
                { "Abu Suwayr",             "Abu Suwayr"            },
                { "Ismailiyah",             "Al Ismailiyah"         },
                { "Cairo International Airport", "Cairo International Airport"  },
                { "Cairo",                  "Cairo International Airport"  },
                { "AbuSultan",              "Difarsuwar Airfield"         },
                { "Jandali",                "Wadi al Jandali"             },
                { "Fayed",                  "Fayed"                 },
                { "Baluza",                 "Baluza"                },
                //{ "BAZL",                   "Baluza"                },
                { "Cairo West",             "Cairo West"            },
                //{ "SPX",                    "Cairo West"            },
                { "Kibrit Air Base",        "Kibrit Air Base"       },
                { "Kibrit",                 "Kibrit Air Base"       },
                { "Melez",                  "Melez"                 },
                { "Bir Hasanah",            "Bir Hasanah"           },
                { "Hasanah",                "Bir Hasanah"           },
                { "El Arish",               "El Arish"              },
                //{ "RIH",                    "El Arish"              },
                { "El Gora",                "El Gora"               },
                //{ "ELGO",                   "El Gora"               },
                { "Abu Rudeis",             "Abu Rudeis"            },
                { "Kedem",                  "Kedem"                 },
                { "Ramon Airbase",          "Ramon Airbase"         },
                { "Ramon",                  "Ramon Airbase"         },
                { "Hatzerim",               "Hatzerim"              },
                { "Hatzor",                 "Hatzor"                },
                { "Hatsor",                 "Hatzor"                },
                { "Palmahim",               "Palmahim"              },
                { "Tel Nof",                "Tel Nof"               },
                //{ "TELN",                   "Tel Nof"               },
                { "Nevatim",                "Nevatim"               },
                { "Sdee Dov",                "Sde Dov"               },
                //{ "SDE",                    "Sde Dov"               },
                { "Ben Gurion",             "Ben-Gurion"            },
                //{ "BGN",                    "Ben-Gurion"            },
                { "Ovda",                   "Ovda"                  },
                { "Saint Catherine",        "St Catherine"          },

                // South Atlantic map

                { "Mount Pleasant",         "Mount Pleasant"        },
                { "Goose Green",            "Goose Green"           },
                { "San Carlos",             "San Carlos FOB"        },
                { "Port Stanley",           "Port Stanley"          },
                { "De Tolhuin",             "Aerodromo De Tolhuin"  },
                { "Rio Grande",             "Rio Grande"            },
                { "Puerto Williams",        "Puerto Williams"       },
                { "San Julian",             "San Julian"            },
                { "Ushuaia Helo Port",      "Ushuaia Helo Port"     },
                { "Ushuaia",                "Ushuaia"               },
                { "Pampa Guanaco",          "Pampa Guanaco"         },
                { "Puerto Santa Cruz",      "Puerto Santa Cruz"     },
                { "Rio Chico",              "Rio Chico"             },
                { "Rio Gallegos",           "Rio Gallegos"          },
                { "Franco Bianco",          "Franco Bianco"         },
                { "Luis Piedrabuena",       "Comandante Luis Piedrabuena"        },
                { "Porvenir",               "Porvenir Airfield"     },
                { "Punta Arenas",           "Punta Arenas"          },
                { "Aeropuerto de Gobernador Gregores",  "Aeropuerto de Gobernador Gregores"    },
                { "de Gobernador Gregores",   "Aeropuerto de Gobernador Gregores"    },
                { "Rio Turbio",             "Rio Turbio"            },
                { "El Calafate",            "El Calafate"           },
                { "Puerto Natales",         "Puerto Natales"        },
                { "O'Higgins",              "Aerodromo O'Higgins"   },

                // Kola Peninsular map

                { "BAS 100",                "BAS100"                },
                { "Kemi",                   "Kemi Tornio"           },
                { "Kemi Tornio",            "Kemi Tornio"           },
                { "Rovaniemi",              "Rovaniemi"             },
                { "Bodo",                   "Bodo"                  },
                { "Lakselv",                "Lakselv"               },
                { "Jokkmokk",               "Jokkmokk"              },
                { "Kiruna",                 "Kiruna"                },
                { "Kalixfors",              "Kalixfors"             },
                { "Severomorsk One",        "Severomorsk1"          },
                { "Severomorsk Three",      "Severomorsk3"          },
                { "Monchegorsk",            "Monchegorsk"           },
                { "Murmansk",               "Murmansk International"  },
                { "Murmansk International", "Murmansk International"  },
                { "Olenegorsk",             "Olenegorsk"            },

                // farp

                { "Platform",               "platform"              },
                { "FARP",                   "platform"              },
                //{ "Nearest Platform",       "nearestfarp"           },

                // farps (blue)

                { "Berlin",                 "berlin"                },
                { "Dallas",                 "dallas"                },
                { "Dublin",                 "dublin"                },
                { "London",                 "london"                },
                { "Madrid",                 "madrid"                },
                { "Moscow",                 "moscow"                },
                { "Paris",                  "paris"                 },
                { "Perth",                  "perth"                 },
                { "Rome",                   "rome"                  },
                { "Warsaw",                 "warsaw"                },

                // farps (red)

                { "Kaemka",                 "kaemka"                },
                { "Kalitka",                "kalitka"               },
                { "Kapel",                  "kapel"                 },
                { "Otkrytka",               "otkrytka"              },
                { "Podkova",                "podkova"               },
                { "Shpora",                 "shpora"                },
                { "Skala",                  "skala"                 },
                { "Torba",                  "torba"                 },
                { "Vetka",                  "vetka"                 },
                { "Yunga",                  "yunga"                 },

                { "Carrier",                "carrier"                   },
                { "Nearest Carrier",        "nearestcarrier"            },

                { "Admiral Kuznetsov",      "CV 1143.5 Admiral Kuznetsov"   },
                { "Carl Vinson",            "CVN-70 Carl Vinson"            },
                { "Gold Eagle",             "CVN-70 Carl Vinson"            },
                { "Tarawa",                 "LHA-1 Tarawa"                  },
                { "Perry",                  "FFG-7CL Oliver Hazzard Perry"  },
                { "Normandy",               "CG-60 Normandy"                },
                
                // supercarriers
                { "Roosevelt",             "CVN-71 Theodore Roosevelt"      },
                { "Rough Rider",           "CVN-71 Theodore Roosevelt"      },

                { "Lincoln",               "CVN-72 Abraham Lincoln"         },
                { "Union",                 "CVN-72 Abraham Lincoln"         },

                { "Washington",            "CVN-73 George Washington"       },
                { "Warfighter",            "CVN-73 George Washington"       },

                { "Stennis",               "CVN-74 John C. Stennis"         },
                { "Courage",               "CVN-74 John C. Stennis"         },

                { "Truman",                "CVN-75 Harry S. Truman"         },
                { "Lone Warrior",          "CVN-75 Harry S. Truman"         },

                // Non Supercarriers ATC Comms enabled Carriers
                { "Forrestal",             "CV-59 Forrestal"                },
                { "Hand Book",             "CV-59 Forrestal"                },

                //new roles

                { "Cats","nearestcarrier" }, // was ATC
                { "Marshal","nearestcarrier" }, // 
                { "Approach","nearestcarrier" }, // 
                { "LSO", "nearestcarrier" }, //
                { "Paddles", "nearestcarrier" }, //

                // awacs

                { "Awacs",                  "awacs"                 },
                { "Darkstar",               "darkstar"              },
                { "Focus",                  "focus"                 },
                { "Magic",                  "magic"                 },
                { "Overlord",               "overlord"              },
                { "Wizard",                 "wizard"                },
                { "Nearest AWACS",          "nearestawacs"          },

                // tanker

                { "Tanker",                 "tanker"                },
                { "Texaco",                 "texaco"                },
                { "Shell",                  "shell"                 },
                { "Arco",                   "arco"                  },
                { "Nearest Tanker",         "nearesttanker"         },

                   // crew

                { "Crew",                   "crew"                   },
                { "Chief",                  "crew"                   },
                { "Sarge",                  "crew"                   },
                { "Ground",                 "crew"                   },

                // AOCS

                { "Crystal Palace",         "aocs"                   },

                // other

                { "Server",                 "aux"                    },
                { "Mystery",                "aux"                    },
                { "Mission",                "aux"                    },

                { "Cargo",                  "cargo"                  },
                { "Descent",                "descent"                },

                { "Kneeboard",              "kneeboard"              },

                // Moose Airboss
                { "Air Boss",                "Moose"                  },


            };

        }

    }

}