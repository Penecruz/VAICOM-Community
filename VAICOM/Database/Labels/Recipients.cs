using System;
using System.Collections.Generic;

namespace VAICOM
{
    namespace Database
    {

        public static partial class Labels
        {

            public static Dictionary<string, string> airecipients = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {

                { "",                       " "                     },

                { "Undefined",              "Undefined"             },

                { "flight",                 "Flight"                },
                { "element",                "Element"               },
                { "wingman1",               "Wingman 1"             },
                { "wingman2",               "Wingman 2"             },
                { "wingman3",               "Wingman 3"             },
                { "wingman4",               "Wingman 4"             },

                { "jtac",                   "JTAC"                  },

                { "axeman",                 "Axeman"                },
                { "darknight",              "Darknight"             },
                { "eyeball",                "Eyeball"               },
                { "finger",                 "Finger"                },
                { "firefly",                "Firefly"               },
                { "moonbeam",               "Moonbeam"              },
                { "playboy",                "Playboy"               },
                { "pointer",                "Pointer"               },
                { "warrior",                "Warrior"               },
                { "whiplash",               "Whiplash"              },

                { "pinpoint",               "Pinpoint"              },
                { "ferret",                 "Ferret"                },
                { "shaba",                  "Shaba"                 },
                { "hammer",                 "Hammer"                },
                { "jaguar",                 "Jaguar"                },
                { "deathstar",              "Deathstar"             },
                { "anvil",                  "Anvil"                 },
                { "mantis",                 "Mantis"                },
                { "badger",                 "Badger"                },

                { "boar",                   "Boar"                  },
                { "chevy",                  "Chevy"                 },
                { "colt",                   "Colt"                  },
                { "dodge",                  "Dodge"                 },
                { "enfield",                "Enfield"               },
                { "ford",                   "Ford"                  },
                { "hawg",                   "Hawg"                  },
                { "pig",                    "Pig"                   },
                { "pontiac",                "Pontiac"               },
                { "springfield",            "Springfield"           },
                { "tusk",                   "Tusk"                  },
                { "uzi",                    "Uzi"                   },

                { "nearestjtac",            "Nearest JTAC"          },

                // ATC general

                { "atc",                    "Air Traffic Controller" },
                { "nearestairfield",        "Nearest Airfield"      },
                //{ "alternateatc",           "Alternate Airfield"    },
                
                
                // Caucuases map
                
                { "wAIUnitATCCaucasusNull", "wAIUnitATCCaucasusNull"},
                { "Anapa-Vityazevo",        "Anapa-Vityazevo"       },
                { "Batumi",                 "Batumi"                },
                { "Beslan",                 "Beslan"                },
                { "Gelendzhik",             "Gelendzhik"            },
                { "Gudauta",                "Gudauta"               },
                { "Maykop-Khanskaya",       "Maykop-Khanskaya"      },
                { "Kobuleti",               "Kobuleti"              },
                { "Senaki-Kolkhi",          "Senaki-Kolkhi"         },
                { "Krasnodar-Center",       "Krasnodar-Center"      },
                { "Krymsk",                 "Krymsk"                },
                { "Kutaisi",                "Kutaisi"               },
                { "Tbilisi-Lochini",        "Tbilisi-Lochini"       },
                { "Mineralnye Vody",        "Mineralnye Vody"       },
                { "Mozdok",                 "Mozdok"                },
                { "Nalchik",                "Nalchik"               },
                { "Novorossiysk",           "Novorossiysk"          },
                { "Krasnodar-Pashkovsky",   "Krasnodar-Pashkovsky"  },
                { "Sochi-Adler",            "Sochi-Adler"           },
                { "Soganlug",               "Soganlug"              },
                { "Sukhumi-Babushara",      "Sukhumi-Babushara"     },
                { "Vaziani",                "Vaziani"               },

                // Marianas map

                { "Andersen AFB",               "Andersen AFB"             },
                { "Antonio B. Won Pat Intl",    "Antonio B. Won Pat Intl"  },
                { "Rota Intl",                  "Rota Intl"                },
                { "Tinian Intl",                "Tinian Intl"              },
                { "Saipan Intl",                "Saipan Intl"              },
                
                // Nevanda map

                { "Creech",                 "Creech"                 },
                { "Henderson Executive",    "Henderson Executive"    },
                { "McCarran International", "McCarran International" },
                { "Laughlin",               "Laughlin"               },
                { "North Las Vegas",        "North Las Vegas"        },
                { "Tonopah Test Range",     "Tonopah Test Range"     },
                { "Groom Lake",             "Groom Lake"             },
                { "Nellis",                 "Nellis"                 },
                { "Boulder City",           "Boulder City"           },

                // Normanday map

                { "Beny-sur-Mer",           "Beny-sur-Mer"                  },
                { "Sainte-Croix-sur-Mer",   "Sainte-Croix-sur-Mer"          },
                { "Lantheuil",              "Lantheuil"                     },
                { "Bazenville",             "Bazenville"                    },
                { "Sommervieu",             "Sommervieu"                    },
                { "Longues-sur-Mer",        "Longues-sur-Mer"               },
                { "Le Molay",               "Le Molay"                      },
                { "Sainte-Laurent-sur-Mer", "Sainte-Laurent-sur-Mer"        },
                { "Saint Pierre du Mont",   "Saint Pierre du Mont"          },
                { "Deux Jumeaux",           "Deux Jumeaux"                  },
                { "Chippelle",              "Chippelle"                     },
                { "Cricqueville-en-Bessin", "Cricqueville-en-Bessin"        },
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
                { "Needs Oar Point",        "Needs Oar Point"               },

                // Persian Gulf Map

                { "Al Maktoum Intl",        "Al Maktoum Intl"               },
                { "Al Minhad AFB",          "Al Minhad AFB"                 },
                { "Dubai Intl",             "Dubai Intl"                    },
                { "Sharjah Intl",           "Sharjah Intl"                  },
                { "Abu Musa Island",        "Abu Musa Island"               },
                { "Sirri Island",           "Sirri Island"                  },
                { "Fujairah Intl",          "Fujairah Intl"                 },
                { "Bandar Lengeh",          "Bandar Lengeh"                 },
                { "Khasab",                 "Khasab"                        },
                { "Qeshm Island",           "Qeshm Island"                  },
                { "Havadarya",              "Havadarya"                     },
                { "Bandar Abbas Intl",      "Bandar Abbas Intl"             },
                { "Lar",                    "Lar"                           },
                { "Kerman",                 "Kerman"                        },
                { "Shiraz Intl",            "Shiraz Intl"                   },
                { "Al Dhafra AFB",          "Al Dhafra AFB"                 },
                { "Al-Bateen",              "Al-Bateen"                     },
                { "Kish Intl",              "Kish Intl"                     },
                { "Lavan Island",           "Lavan Island"                  },
                { "Al Ain Intl",            "Al Ain Intl"                   },
                { "Bandar-e-Jask",          "Bandar-e-Jask"                 },
                { "Abu Dhabi Intl",         "Abu Dhabi Intl"                },
                { "Sas Al Nakheel",         "Sas Al Nakheel"                },
                { "Jiroft",                 "Jiroft"                        },
                { "Liwa AFB",               "Liwa AFB"                      },
                { "Ras Al Khaimah Intl",    "Ras Al Khaimah Intl"           },
                { "Sir Abu Nuayr",          "Sir Abu Nuayr"                 },

                //Channel map
                { "Dunkirk Mardyck",        "Dunkirk Mardyck" },
                { "Hawkinge",               "Hawkinge" },
                { "Saint Omer Longuenesse", "Saint Omer Longuenesse" },
                { "Merville Calonne",       "Merville Calonne" },
                { "High Halden",            "High Halden" },
                { "Detling",                "Detling" },
                { "Abbeville Drucat",       "Abbeville Drucat" },
                { "Lympne",                 "Lympne" },
                { "Manston",                "Manston" },

                // Syria map

                { "Beirut-Rafic Hariri",    "Beirut-Rafic Hariri"   },
                { "Rayak",                  "Rayak"                 },
                { "Wujah Al Hajar",         "Wujah Al Hajar"        },
                { "Kiryat Shmona",          "Kiryat Shmona"         },
                { "Mezzeh",                 "Mezzeh"                },
                { "Qabr as Sitt",           "Qabr as Sitt"          },
                { "Rene Mouawad",           "Rene Mouawad"          },
                { "Marj as Sultan North",   "Marj as Sultan North"  },
                { "Marj as Sultan South",   "Marj as Sultan South"  },
                { "Marj Ruhayyil",          "Marj Ruhayyil"         },
                { "Al-Dumayr",              "Al-Dumayr"             },
                { "Haifa",                  "Haifa"                 },
                { "An Nasiriyah",           "An Nasiriyah"          },
                { "Al Qusayr",              "Al Qusayr"             },
                { "Khalkhalah",             "Khalkhalah"            },
                { "Ramat David",            "Ramat David"           },
                { "Megiddo",                "Megiddo"               },
                { "Eyn Shemer",             "Eyn Shemer"            },
                { "Bassel Al-Assad",        "Bassel Al-Assad"       },
                { "Abu al-Duhur",           "Abu al-Duhur"          },
                { "Taftanaz",               "Taftanaz"              },
                { "Hatay",                  "Hatay"                 },
                { "Kuweires",               "Kuweires"              },
                { "Minakh",                 "Minakh"                },
                { "Jirah",                  "Jirah"                 },
                { "Adana Sakirpasa",        "Adana Sakirpasa"       },
                { "Incirlik",               "Incirlik"              },
                { "Damascus",               "Damascus"              }, //new additions
                { "Tha'lah",                "Tha'lah"               },
                { "Larnaca",                "Larnaca"               },
                { "Akrotiri",               "Akrotiri"              },
                { "King Hussein Air College",  "King Hussein Air College"  },
                { "At Tanf",                "At Tanf"               },
                { "H3",                     "H3"                    },
                { "H3 Northwest",           "H3 Northwest"          },
                { "H3 Southwest",           "H3 Southwest"          },
                { "H4",                     "H4"                    },
                { "Paphos",                 "Paphos"                },
                { "Lakatamia",              "Lakatamia"             },
                { "Ercan",                  "Ercan"                 },
                { "Muwaffaq Salti",         "Muwaffaq Salti"        },
                { "King Abdullah II",       "King Abdullah II"      },
                //{ "Tel Nof",                "Tel Nof"               }, //Already defined in Sinai
                //{ "Hatzor",                 "Hatzor"                }, //Already defined in Sinai
                //{ "Palmachim",              "Palmachim"             }, //Already defined in Sinai
                //{ "Ben Gurion",             "Ben Gurion"            }, //Already defined in Sinai
                { "Marka",                  "Marka"                 },
                { "Herzliya",               "Herzliya"              },
                { "Prince Hassan",          "Prince Hassan"         },

                // Sinai map

                { "Al Mansurah",            "Al Mansurah"           },
                { "AzZaqaziq",              "AzZaqaziq"             },
                { "As Salihiyah",           "As Salihiyah"          },
                { "Bilbeis Air Base",       "Bilbeis Air Base"      },
                { "Inshas Airbase",         "Inshas Airbase"        },
                { "Abu Suwayr",             "Abu Suwayr"            },
                { "Al Ismailiyah",          "Al Ismailiyah"         },//
                { "Cairo International Airport", "Cairo International Airport"  },
                { "Difarsuwar Airfield",    "Difarsuwar Airfield"   },//
                { "Wadi al Jandali",        "Wadi al Jandali"       },
                { "Fayed",                  "Fayed"                 },
                { "Baluza",                 "Baluza"                },
                { "Cairo West",             "Cairo West"            },
                { "Kibrit Air Base",        "Kibrit Air Base"       },
                { "Melez",                  "Melez"                 },
                { "Bir Hasanah",            "Bir Hasanah"           },
                { "El Arish",               "El Arish"              },
                { "El Gora",                "El Gora"               },
                { "Abu Rudeis",             "Abu Rudeis"            },
                { "Kedem",                  "Kedem"                 },
                { "Ramon Airbase",          "Ramon Airbase"         },
                { "Hatzerim",               "Hatzerim"              },
                { "Hatzor",                 "Hatzor"                },
                { "Palmachim",              "Palmachim"             },
                { "Tel Nof",                "Tel Nof"               },
                { "Nevatim",                "Nevatim"               },
                { "Sde Dov",                "Sde Dov"               },
                { "Ben-Gurion",             "Ben-Gurion"            },
                { "Ovda",                   "Ovda"                  },
                { "St Catherine",           "St Catherine"          },
                { "Wadi Abu Rish",          "Wadi Abu Rish"         },
                { "Ramon International Airport",              "Ramon International Airport"            },
                { "Sharm El Sheikh International Airport",    "Sharm El Sheikh International Airport"  },
                { "Hurghada International Airport",           "Hurghada International Airport"         },
                { "Al Bahr al Ahmar",                         "Al Bahr al Ahmar"                       },
                { "El Minya",              "El Minya"               },
                { "Beni Suef",             "Beni Suef"              },
                { "Kom Awshim",            "St Catherine"           },
                { "Quwaysina",             "Kom Awshim"             },
                { "Birma Air Base",        "Birma Air Base"         },
                { "Gebel El Basur Air Base",                 "Gebel El Basur Air Base"                },
                { "Al Rahmaniyah Air Base",                  "Al Rahmaniyah Air Base"                 },
                { "Jiyanklis Air Base",                      "Jiyanklis Air Base"                     },
                { "Borj El Arab International Airport",      "Borj El Arab International Airport"     },

                // South Atlantic map

                { "Mount Pleasant",         "Mount Pleasant"        },
                { "Goose Green",            "Goose Green"           },
                { "San Carlos FOB",         "San Carlos FOB"        },
                { "Port Stanley",           "Port Stanley"          },
                { "Aerodromo De Tolhuin",   "Aerodromo De Tolhuin"  },
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
                { "Comandante Luis Piedrabuena",  "Comandante Luis Piedrabuena"        },
                { "Porvenir Airfield",      "Porvenir Airfield"     },
                { "Punta Arenas",           "Punta Arenas"          },
                { "Aeropuerto de Gobernador Gregores",  "Aeropuerto de Gobernador Gregores"    },
                { "Rio Turbio",             "Rio Turbio"            },
                { "El Calafate",            "El Calafate"           },
                { "Puerto Natales",         "Puerto Natales"        },
                { "Aerodromo O'Higgins",    "Aerodromo O'Higgins"   },

                // Kola Peninsular map

                { "BAS100",                 "BAS 100"               },
                { "Kemi Tornio",            "Kemi Tornio"           },
                { "Rovaniemi",              "Rovaniemi"             },
                { "Bodo",                   "Bodo"                  },
                { "Lakselv",                "Lakselv"               },
                { "Jokkmokk",               "Jokkmokk"              },
                { "Kiruna",                 "Kiruna"                },
                { "Kalixfors",              "Kalixfors"             },
                { "Severomorsk1",           "Severomorsk-1"         },
                { "Severomorsk3",           "Severomorsk-3"         },
                { "Monchegorsk",            "Monchegorsk"           },
                { "Murmansk International", "Murmansk International"   },
                { "Olenegorsk",             "Olenegorsk"            },
                
                // Afghanistan map
                    // South
                { "Tarinkot",               "Tarinkot"              },
                { "kandahar",               "kandahar"              },
                { "Kandahar Heliport",      "Kandahar Heliport"     },
                { "Chaghcharan",            "Chaghcharan"           },
                { "Bost",                   "Bost"                  },
                { "Dwyer",                  "Dwyer"                 },
                { "Camp Bastion",           "Camp Bastion"          },
                { "Camp Bastion Heliport",  "Camp Bastion Heliport" },
                { "Qala i Naw",             "Qala i Naw"            },
                { "Herat",                  "Herat"                 },
                { "Shindand",               "Shindand"              },
                { "Shindand Heliport",      "Shindand Heliport"     },
                { "Farah",                  "Farah"                 },
                { "Nimroz",                 "Nimroz"                },
                     // East
                { "Bagram",                "Bagram"                 },
                { "Bamyan",                "Bamyan"                 },
                { "Ghazni Heliport",       "Ghazni Heliport"        },
                { "Gardez",                "Gardez"                 },
                { "Jalalabad",             "Jalalabad"              },
                { "Kabul",                 "Kabul"                  },
                { "Khost",                 "Khost"                  },
                { "Khost Heliport",        "Khost Heliport"         },
                { "Sharan",                "Sharan"                 },
                { "Urgoon Heliport",       "Urgoon Heliport"        },
                { "Maymana Zahiraddin Faryabi", "Maymana Zahiraddin Faryabi"     },
                { "Mary",                  "Mary"                                },
                { "Mary North",            "Mary North"                          },

                // Iraq map
                    // North
                { "Al-Asad Airbase",                           "Al-Asad Airbase"                     },
                { "Al-Sahra Airport",                          "Al-Sahra Airport"                    },
                { "Al-Salam Airbase",                          "Al-Salam Airbase"                    },
                { "Al-Taji Airport",                           "Al-Taji Airport"                     },
                { "Al-Taquddum Airport",                       "Al-Taquddum Airport"                 },
                { "Baghdad International Airport",             "Baghdad International Airport"       },
                { "Balad Airbase",                             "Balad Airbase"                       },
                { "Bashur Airport",                            "Bashur Airport"                      },
                { "Erbil International Airport",               "Erbil International Airport"         },
                { "K1 Base",                                   "K1 Base"                             },
                { "Kirkuk International Airport",              "Kirkuk International Airport"        },
                { "Qayyarah Airfield West",                    "Qayyarah Airfield West"              },
                { "Sulaimaniyah International Airport",        "Sulaimaniyah International Airport"  },

                // FARP

                { "platform",               "Helipad"               },

                { "berlin",                 "Berlin"                },
                { "dallas",                 "Dallas"                },
                { "dublin",                 "Dublin"                },
                { "london",                 "London"                },
                { "madrid",                 "Madrid"                },
                { "moscow",                 "Moscow"                },
                { "paris",                  "Paris"                 },
                { "perth",                  "Perth"                 },
                { "rome",                   "Rome"                  },
                { "warsaw",                 "Warsaw"                },

                { "kaemka",                 "Kaemka"                },
                { "kalitka",                "Kalitka"               },
                { "kapel",                  "Kapel"                 },
                { "otkrytka",               "Otkrytka"              },
                { "podkova",                "Podkova"               },
                { "shpora",                 "Shpora"                },
                { "skala",                  "Skala"                 },
                { "torba",                  "Torba"                 },
                { "vetka",                  "Vetka"                 },
                { "yunga",                  "Yunga"                 },

                { "carrier",                        "Carrier"                       },
                { "nearestcarrier",                 "Carrier Operations"            },
                { "CV 1143.5 Admiral Kuznetsov",    "CV 1143.5 Admiral Kuznetsov"   },
                { "CVN-70 Carl Vinson",             "CVN-70 Carl Vinson"            },
                { "LHA-1 Tarawa",                   "LHA-1 Tarawa"                  },
                { "FFG-7CL Oliver Hazzard Perry",   "FFG-7CL Oliver Hazzard Perry"  },
                { "CVN-74 John C. Stennis",         "CVN-74 John C. Stennis"        },
                { "CG-60 Normandy",                 "CG-60 Normandy"                },
                                
                // new carriers
                { "CVN-71 Theodore Roosevelt",                          "CVN-71 Theodore Roosevelt"             },
                { "CVN-72 Abraham Lincoln",                             "CVN-72 Abraham Lincoln"                },
                { "CVN-73 George Washington",                           "CVN-73 George Washington"              },
                { "CVN-75 Harry S. Truman",                             "CVN-75 Harry S. Truman"                },

                // Non Supercarriers ATC Comms enabled Carriers
                { "CV-59 Forrestal",                                    "CV-59 Forrestal"                       },

                // new roles

                { "CarrierDeparture", "Carrier Departure" },
                { "CarrierMarshal",   "Carrier Marshal" },
                { "CarrierApproachTower", "Carrier Approach" },
                { "CarrierLSO", "Carrier LSO" },

                { "awacs",                  "AWACS"                 },
                { "darkstar",               "Darkstar"              },
                { "focus",                  "Focus"                 },
                { "magic",                  "Magic"                 },
                { "overlord",               "Overlord"              },
                { "wizard",                 "Wizard"                },
                { "nearestawacs",           "Nearest AWACS"         },

                { "tanker",                 "Tanker"                },
                { "texaco",                 "Texaco"                },
                { "shell",                  "Shell"                 },
                { "arco",                   "Arco"                  },
                { "Bloodhound",             "Bloodhound"            },
                { "Mauler",                 "Mauler"                },
                { "Navy One",               "Navy One"              },
                { "nearesttanker",          "Nearest Tanker"        },

                { "crew",                   "Ground Crew"           },

                { "aocs",                   "AOCS"                  },

                { "aux",                    "F10 Menu"              },
                //{ "AI_ATC",                 "AI_ATC"                },

                { "cargo",                  "Cargo"                 },
                { "descent",                "Descent"               },

                { "kneeboard",              "Kneeboard"             },
                //{ "kneeboardnotes",         "Flip to Notes tab"     },

                 //Moose Airboss
                { "Moose",                  "Moose F10 Menu"        },

            };

        }
    }
}
