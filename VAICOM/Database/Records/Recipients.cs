using System;
using System.Collections.Generic;

namespace VAICOM
{

    namespace Database
    {

        public static partial class Recipients
        {

            public static Dictionary<string, Recipient> Table = new Dictionary<string, Recipient>(StringComparer.OrdinalIgnoreCase)
            {

                { "iDeviceNull",            new Recipient { uniqueid = 00000, category = RecipientCategories.cockpitdevice, name = "iDeviceNull",        } },
                { "iDeviceMaximum",         new Recipient { uniqueid = 04000, category = RecipientCategories.cockpitdevice, name = "iDeviceMaximum",     } },

                { "undefined",              new Recipient { uniqueid = 10000, category = RecipientCategories.undefined, name = "wAIUnitNull",            } },
                { "wAIUnitNull",            new Recipient { uniqueid = 10000, category = RecipientCategories.aiunit, name = "wAIUnitNull",               } },

                { "wAIUnitPlayerNull",      new Recipient { uniqueid = 11000, category = RecipientCategories.aiunit, name = "wAIUnitPlayerNull",         } },
                { "wAIUnitPlayerMaximum",   new Recipient { uniqueid = 11999, category = RecipientCategories.aiunit, name = "wAIUnitPlayerMaximum",      } },

                { "wAIUnitFlightNull",      new Recipient { uniqueid = 12000, category = RecipientCategories.aiflight, name = "wAIUnitFlightNull",       } },
                { "flight",                 new Recipient { uniqueid = 12001, category = RecipientCategories.aiflight, name = "wAIUnitFlightFlight",     displayname = Labels.airecipients["flight"],   flightnumber = 5 } },
                { "element",                new Recipient { uniqueid = 12002, category = RecipientCategories.aiflight, name = "wAIUnitFlightElement",    displayname = Labels.airecipients["element"],  flightnumber = 4 } },
                { "wingman1",               new Recipient { uniqueid = 12003, category = RecipientCategories.aiflight, name = "wAIUnitFlightWingman1",   displayname = Labels.airecipients["wingman1"], flightnumber = 0 } },
                { "wingman2",               new Recipient { uniqueid = 12004, category = RecipientCategories.aiflight, name = "wAIUnitFlightWingman2",   displayname = Labels.airecipients["wingman2"], flightnumber = 1 } },
                { "wingman3",               new Recipient { uniqueid = 12005, category = RecipientCategories.aiflight, name = "wAIUnitFlightWingman3",   displayname = Labels.airecipients["wingman3"], flightnumber = 2 } },
                { "wingman4",               new Recipient { uniqueid = 12006, category = RecipientCategories.aiflight, name = "wAIUnitFlightWingman4",   displayname = Labels.airecipients["wingman4"], flightnumber = 3 } },
                { "wAIUnitFlightMaximum",   new Recipient { uniqueid = 12999, category = RecipientCategories.aiflight, name = "wAIUnitFlightMaximum",    } },

                // JTAC

                { "wAIUnitJTACNull",        new Recipient { uniqueid = 14000, category = RecipientCategories.aijtac, name = "wAIUnitJTACNull",         } },
                { "jtac",                   new Recipient { uniqueid = 14001, category = RecipientCategories.aijtac, name = "wAIUnitJTACJTAC",         displayname = Labels.airecipients["jtac"] } },

                { "axeman",                 new Recipient { uniqueid = 14002, category = RecipientCategories.aijtac, name = "wAIUnitJTACAxeman",       displayname = Labels.airecipients["axeman"] } },
                { "darknight",              new Recipient { uniqueid = 14003, category = RecipientCategories.aijtac, name = "wAIUnitJTACDarknight",    displayname = Labels.airecipients["darknight"] } },
                { "eyeball",                new Recipient { uniqueid = 14004, category = RecipientCategories.aijtac, name = "wAIUnitJTACEyeball",      displayname = Labels.airecipients["eyeball"] } },
                { "finger",                 new Recipient { uniqueid = 14005, category = RecipientCategories.aijtac, name = "wAIUnitJTACFinger",       displayname = Labels.airecipients["finger"] } },
                { "firefly",                new Recipient { uniqueid = 14006, category = RecipientCategories.aijtac, name = "wAIUnitJTACFirefly",      displayname = Labels.airecipients["firefly"] } },
                { "moonbeam",               new Recipient { uniqueid = 14007, category = RecipientCategories.aijtac, name = "wAIUnitJTACMoonbeam",     displayname = Labels.airecipients["moonbeam"] } },
                { "playboy",                new Recipient { uniqueid = 14008, category = RecipientCategories.aijtac, name = "wAIUnitJTACPlayboy",      displayname = Labels.airecipients["playboy"]  } },
                { "pointer",                new Recipient { uniqueid = 14009, category = RecipientCategories.aijtac, name = "wAIUnitJTACPointer",      displayname = Labels.airecipients["pointer"] } },
                { "warrior",                new Recipient { uniqueid = 14011, category = RecipientCategories.aijtac, name = "wAIUnitJTACWarrior",      displayname = Labels.airecipients["warrior"] } },
                { "whiplash",               new Recipient { uniqueid = 14012, category = RecipientCategories.aijtac, name = "wAIUnitJTACWhiplash",     displayname = Labels.airecipients["whiplash"] } },

                // new JTACS
                { "pinpoint",               new Recipient { uniqueid = 14013, category = RecipientCategories.aijtac, name = "wAIUnitJTACPinpoint",     displayname = Labels.airecipients["pinpoint"] } },
                { "ferret",                 new Recipient { uniqueid = 14014, category = RecipientCategories.aijtac, name = "wAIUnitJTACFerret",       displayname = Labels.airecipients["ferret"] } },
                { "shaba",                  new Recipient { uniqueid = 14015, category = RecipientCategories.aijtac, name = "wAIUnitJTACShaba",        displayname = Labels.airecipients["shaba"] } },
                { "hammer",                 new Recipient { uniqueid = 14016, category = RecipientCategories.aijtac, name = "wAIUnitJTACHammer",       displayname = Labels.airecipients["hammer"] } },
                { "jaguar",                 new Recipient { uniqueid = 14017, category = RecipientCategories.aijtac, name = "wAIUnitJTACJaguar",       displayname = Labels.airecipients["jaguar"] } },
                { "deathstar",              new Recipient { uniqueid = 14018, category = RecipientCategories.aijtac, name = "wAIUnitJTACDeathstar",    displayname = Labels.airecipients["deathstar"] } },
                { "anvil",                  new Recipient { uniqueid = 14019, category = RecipientCategories.aijtac, name = "wAIUnitJTACAnvil",        displayname = Labels.airecipients["anvil"] } },
                { "mantis",                 new Recipient { uniqueid = 14020, category = RecipientCategories.aijtac, name = "wAIUnitJTACMantis",       displayname = Labels.airecipients["mantis"] } },
                { "badger",                 new Recipient { uniqueid = 14021, category = RecipientCategories.aijtac, name = "wAIUnitJTACBadger",       displayname = Labels.airecipients["badger"] } },

                // JTAC based on flight callsigns

                { "boar",                   new Recipient { uniqueid = 14022, category = RecipientCategories.aijtac, name = "wAIUnitJTACBoar",         displayname = Labels.airecipients["boar"] } },
                { "chevy",                  new Recipient { uniqueid = 14023, category = RecipientCategories.aijtac, name = "wAIUnitJTACChevy",        displayname = Labels.airecipients["chevy"] } },
                { "colt",                   new Recipient { uniqueid = 14024, category = RecipientCategories.aijtac, name = "wAIUnitJTACColt",         displayname = Labels.airecipients["colt"] } },
                { "dodge",                  new Recipient { uniqueid = 14025, category = RecipientCategories.aijtac, name = "wAIUnitJTACDodge",        displayname = Labels.airecipients["dodge"] } },
                { "enfield",                new Recipient { uniqueid = 14026, category = RecipientCategories.aijtac, name = "wAIUnitJTACEnfield",      displayname = Labels.airecipients["enfield"] } },
                { "ford",                   new Recipient { uniqueid = 14027, category = RecipientCategories.aijtac, name = "wAIUnitJTACFord",         displayname = Labels.airecipients["ford"] } },
                { "hawg",                   new Recipient { uniqueid = 14028, category = RecipientCategories.aijtac, name = "wAIUnitJTACHawg",         displayname = Labels.airecipients["hawg"] } },
                { "pig",                    new Recipient { uniqueid = 14029, category = RecipientCategories.aijtac, name = "wAIUnitJTACPig",          displayname = Labels.airecipients["pig"] } },
                { "pontiac",                new Recipient { uniqueid = 14030, category = RecipientCategories.aijtac, name = "wAIUnitJTACPontiac",      displayname = Labels.airecipients["pontiac"] } },
                { "springfield",            new Recipient { uniqueid = 14031, category = RecipientCategories.aijtac, name = "wAIUnitJTACSpringfield",  displayname = Labels.airecipients["springfield"] } },
                { "tusk",                   new Recipient { uniqueid = 14032, category = RecipientCategories.aijtac, name = "wAIUnitJTACTusk",         displayname = Labels.airecipients["tusk"] } },
                { "uzi",                    new Recipient { uniqueid = 14033, category = RecipientCategories.aijtac, name = "wAIUnitJTACUzi",          displayname = Labels.airecipients["uzi"] } },

                { "nearestjtac",            new Recipient { uniqueid = 14034, category = RecipientCategories.aijtac, name = "wAIUnitJTACNearest",      displayname = Labels.airecipients["nearestjtac"] } },
                { "wAIUnitJTACMaximum",     new Recipient { uniqueid = 14999, category = RecipientCategories.aijtac, name = "wAIUnitJTACMaximum",      } },

                { "wAIUnitATCNull",         new Recipient { uniqueid = 15000, category = RecipientCategories.aiatc, name = "wAIUnitATCNull",          } },
                { "atc",                    new Recipient { uniqueid = 15001, category = RecipientCategories.aiatc, name = "wAIUnitATCATC",           displayname = Labels.airecipients["atc"] } },
                { "nearestairfield",        new Recipient { uniqueid = 15002, category = RecipientCategories.aiatc, name ="wAIUnitATCNearestAirfield",displayname = Labels.airecipients["nearestairfield"] } },
                //{ "alternateatc",           new Recipient { uniqueid = 15003, category = tgtcat.aiatc, name = "wAIUnitATCAlternateATC",  displayname = Labels.airecipients["alternateatc"] } },

                { "wAIUnitATCCaucasusNull", new Recipient { uniqueid = 15050, category = RecipientCategories.aiatc, name = "wAIUnitATCCaucasusNull",      } },
                { "Anapa-Vityazevo",        new Recipient { uniqueid = 15051, category = RecipientCategories.aiatc, name = "wAIUnitATCCaucasusAnapa",     displayname = Labels.airecipients["Anapa-Vityazevo"] } },
                { "Batumi",                 new Recipient { uniqueid = 15052, category = RecipientCategories.aiatc, name = "wAIUnitATCCaucasusBatumi",    displayname = Labels.airecipients["Batumi"] } },
                { "Beslan",                 new Recipient { uniqueid = 15053, category = RecipientCategories.aiatc, name = "wAIUnitATCCaucasusBeslan",    displayname = Labels.airecipients["Beslan"] } },
                { "Gelendzhik",             new Recipient { uniqueid = 15054, category = RecipientCategories.aiatc, name = "wAIUnitATCCaucasusGelendzhik",displayname = Labels.airecipients["Gelendzhik"] } },
                { "Gudauta",                new Recipient { uniqueid = 15055, category = RecipientCategories.aiatc, name = "wAIUnitATCCaucasusGudauta",   displayname = Labels.airecipients["Gudauta"] } },
                { "Maykop-Khanskaya",       new Recipient { uniqueid = 15056, category = RecipientCategories.aiatc, name = "wAIUnitATCCaucasusKhanskaya", displayname = Labels.airecipients["Maykop-Khanskaya"] } },
                { "Kobuleti",               new Recipient { uniqueid = 15057, category = RecipientCategories.aiatc, name = "wAIUnitATCCaucasusKobuleti",  displayname = Labels.airecipients["Kobuleti"] } },
                { "Senaki-Kolkhi",          new Recipient { uniqueid = 15058, category = RecipientCategories.aiatc, name = "wAIUnitATCCaucasusKolkhi",    displayname = Labels.airecipients["Senaki-Kolkhi"] } },
                { "Krasnodar-Center",       new Recipient { uniqueid = 15059, category = RecipientCategories.aiatc, name = "wAIUnitATCCaucasusKrasnodar", displayname = Labels.airecipients["Krasnodar-Center"] } },
                { "Krymsk",                 new Recipient { uniqueid = 15060, category = RecipientCategories.aiatc, name = "wAIUnitATCCaucasusKrymsk",    displayname = Labels.airecipients["Krymsk"] } },
                { "Kutaisi",                new Recipient { uniqueid = 15061, category = RecipientCategories.aiatc, name = "wAIUnitATCCaucasusKutaisi",   displayname = Labels.airecipients["Kutaisi"] } },
                { "Tbilisi-Lochini",        new Recipient { uniqueid = 15062, category = RecipientCategories.aiatc, name = "wAIUnitATCCaucasusLochini",   displayname = Labels.airecipients["Tbilisi-Lochini"] } },
                { "Mineralnye Vody",        new Recipient { uniqueid = 15063, category = RecipientCategories.aiatc, name = "wAIUnitATCCaucasusMinvody",   displayname = Labels.airecipients["Mineralnye Vody"] } },
                { "Mozdok",                 new Recipient { uniqueid = 15064, category = RecipientCategories.aiatc, name = "wAIUnitATCCaucasusMozdok",    displayname = Labels.airecipients["Mozdok"] } },
                { "Nalchik",                new Recipient { uniqueid = 15065, category = RecipientCategories.aiatc, name = "wAIUnitATCCaucasusNalchik",   displayname = Labels.airecipients["Nalchik"] } },
                { "Novorossiysk",           new Recipient { uniqueid = 15066, category = RecipientCategories.aiatc, name="wAIUnitATCCaucasusNovorossiysk",displayname = Labels.airecipients["Novorossiysk"] } },
                { "Krasnodar-Pashkovsky",   new Recipient { uniqueid = 15067, category = RecipientCategories.aiatc, name = "wAIUnitATCCaucasusPashkovsky",displayname = Labels.airecipients["Krasnodar-Pashkovsky"] } },
                { "Sochi-Adler",            new Recipient { uniqueid = 15069, category = RecipientCategories.aiatc, name = "wAIUnitATCCaucasusSochi",     displayname = Labels.airecipients["Sochi-Adler"] } },
                { "Soganlug",               new Recipient { uniqueid = 15070, category = RecipientCategories.aiatc, name = "wAIUnitATCCaucasusSoganlug",  displayname = Labels.airecipients["Soganlug"] } },
                { "Sukhumi-Babushara",      new Recipient { uniqueid = 15071, category = RecipientCategories.aiatc, name = "wAIUnitATCCaucasusSukhumi",   displayname = Labels.airecipients["Sukhumi-Babushara"] } },
                { "Vaziani",                new Recipient { uniqueid = 15073, category = RecipientCategories.aiatc, name = "wAIUnitATCCaucasusVaziani",   displayname = Labels.airecipients["Vaziani"] } },
                { "wAIUnitATCCaucasusMaximum",new Recipient{uniqueid = 15099, category = RecipientCategories.aiatc, name = "wAIUnitATCCaucasusMaximum",  } },

                // Marianas Map

                { "wAIUnitATCMarianasNull",         new Recipient { uniqueid = 15100, category = RecipientCategories.aiatc, name = "wAIUnitATCMarianasNull",        } },

                { "Andersen AFB",                   new Recipient { uniqueid = 15101, category = RecipientCategories.aiatc, name = "wAIUnitATCMarianasAndersen",    displayname = Labels.airecipients["Andersen AFB"] } },
                { "Antonio B. Won Pat Intl",        new Recipient { uniqueid = 15102, category = RecipientCategories.aiatc, name = "wAIUnitATCMarianasAntonioB",    displayname = Labels.airecipients["Antonio B. Won Pat Intl"] } },
                { "Rota Intl",                      new Recipient { uniqueid = 15103, category = RecipientCategories.aiatc, name = "wAIUnitATCMarianasRotaIntl",    displayname = Labels.airecipients["Rota Intl"] } },
                { "Tinian Intl",                    new Recipient { uniqueid = 15104, category = RecipientCategories.aiatc, name = "wAIUnitATCMarianasTinianIntl",  displayname = Labels.airecipients["Tinian Intl"] } },
                { "Saipan Intl",                    new Recipient { uniqueid = 15105, category = RecipientCategories.aiatc, name = "wAIUnitATCMarianasSaipanIntl",  displayname = Labels.airecipients["Saipan Intl"] } },

                { "wAIUnitATCMarianasMaximum",      new Recipient { uniqueid = 15110, category = RecipientCategories.aiatc, name = "wAIUnitATCMarianasMaximum",     } },

                // Marianas WWII Map

                { "wAIUnitATCMarianasWWIINull",     new Recipient { uniqueid = 15111, category = RecipientCategories.aiatc, name = "wAIUnitATCMarianasWWIINull",        } },

                { "Pagan",                          new Recipient { uniqueid = 15113, category = RecipientCategories.aiatc, name = "wAIUnitATCMarianasWWIIPagan",          displayname = Labels.airecipients["Pagan"] } },
                { "Marpi",                          new Recipient { uniqueid = 15114, category = RecipientCategories.aiatc, name = "wAIUnitATCMarianasWWIIMarpi",          displayname = Labels.airecipients["Marpi"] } },
                { "Kagman",                         new Recipient { uniqueid = 15115, category = RecipientCategories.aiatc, name = "wAIUnitATCMarianasWWIIKagman",         displayname = Labels.airecipients["Kagman"] } },
                { "Charon Kanoa",                   new Recipient { uniqueid = 15116, category = RecipientCategories.aiatc, name = "wAIUnitATCMarianasWWIICharonKanoa",    displayname = Labels.airecipients["Charon Kanoa"] } },
                { "Isley",                          new Recipient { uniqueid = 15117, category = RecipientCategories.aiatc, name = "wAIUnitATCMarianasWWIIIsley",          displayname = Labels.airecipients["Isley"] } },
                { "Ushi",                           new Recipient { uniqueid = 15118, category = RecipientCategories.aiatc, name = "wAIUnitATCMarianasWWIIUshi",           displayname = Labels.airecipients["Ushi"] } },
                { "Airfield 3",                     new Recipient { uniqueid = 15119, category = RecipientCategories.aiatc, name = "wAIUnitATCMarianasWWIIAirfield3",      displayname = Labels.airecipients["Airfield 3"] } },
                { "Gurguan Point",                  new Recipient { uniqueid = 15120, category = RecipientCategories.aiatc, name = "wAIUnitATCMarianasWWIIGurguanPoint",   displayname = Labels.airecipients["Gurguan Point"] } },
                { "Rota",                           new Recipient { uniqueid = 15121, category = RecipientCategories.aiatc, name = "wAIUnitATCMarianasWWIIRota",           displayname = Labels.airecipients["Rota"] } },
                { "Agana",                          new Recipient { uniqueid = 15122, category = RecipientCategories.aiatc, name = "wAIUnitATCMarianasWWIIAgana",          displayname = Labels.airecipients["Agana"] } },
                { "Orote",                          new Recipient { uniqueid = 15123, category = RecipientCategories.aiatc, name = "wAIUnitATCMarianasWWIIOrote",          displayname = Labels.airecipients["Orote"] } },
                

                { "wAIUnitATCMarianasWWIIMaximum",  new Recipient { uniqueid = 15130, category = RecipientCategories.aiatc, name = "wAIUnitATCMarianasWWIIMaximum",     } },

                // Nevada NTTR Map

                { "wAIUnitATCNevadaNull",           new Recipient { uniqueid = 15150, category = RecipientCategories.aiatc, name = "wAIUnitATCNevadaNull",        } },

                { "Creech",                         new Recipient { uniqueid = 15151, category = RecipientCategories.aiatc, name = "wAIUnitATCNevadaCreech",      displayname = Labels.airecipients["Creech"] } },
                { "Henderson Executive",            new Recipient { uniqueid = 15152, category = RecipientCategories.aiatc, name = "wAIUnitATCNevadaHenderson",   displayname = Labels.airecipients["Henderson Executive"] } },
                { "McCarran International",         new Recipient { uniqueid = 15153, category = RecipientCategories.aiatc, name = "wAIUnitATCNevadaMccarran",    displayname = Labels.airecipients["McCarran International"] } },
                { "Laughlin",                       new Recipient { uniqueid = 15154, category = RecipientCategories.aiatc, name = "wAIUnitATCNevadaLaughlin",    displayname = Labels.airecipients["Laughlin"] } },
                { "North Las Vegas",                new Recipient { uniqueid = 15155, category = RecipientCategories.aiatc, name = "wAIUnitATCNevadaVegas",       displayname = Labels.airecipients["North Las Vegas"] } },
                { "Tonopah Test Range",             new Recipient { uniqueid = 15156, category = RecipientCategories.aiatc, name = "wAIUnitATCNevadaTonopah",     displayname = Labels.airecipients["Tonopah Test Range"] } },
                { "Groom Lake",                     new Recipient { uniqueid = 15157, category = RecipientCategories.aiatc, name = "wAIUnitATCNevadaGroomLake",   displayname = Labels.airecipients["Groom Lake"] } },
                { "Nellis",                         new Recipient { uniqueid = 15158, category = RecipientCategories.aiatc, name = "wAIUnitATCNevadaNellis",      displayname = Labels.airecipients["Nellis"] } },
                { "Boulder City",                   new Recipient { uniqueid = 15159, category = RecipientCategories.aiatc, name = "wAIUnitATCNevadaBoulder",     displayname = Labels.airecipients["Boulder City"] } },

                { "wAIUnitATCNevadaMaximum",        new Recipient { uniqueid = 15199, category = RecipientCategories.aiatc, name = "wAIUnitATCNevadaMaximum",     } },

                // Normandy Map

                { "wAIUnitATCNormandyNull",         new Recipient { uniqueid = 15200, category = RecipientCategories.aiatc, name = "wAIUnitATCNormandyNull",              } },

                { "Beny-sur-Mer",                   new Recipient { uniqueid = 15201, category = RecipientCategories.aiatc, name = "wAIUnitATCNormandyBenysurMer",        displayname = Labels.airecipients["Beny-sur-Mer"] } },
                { "Sainte-Croix-sur-Mer",           new Recipient { uniqueid = 15202, category = RecipientCategories.aiatc, name = "wAIUnitATCNormandySainteCroixsurMer", displayname = Labels.airecipients["Sainte-Croix-sur-Mer"]  } },
                { "Lantheuil",                      new Recipient { uniqueid = 15203, category = RecipientCategories.aiatc, name = "wAIUnitATCNormandyLantheuil",         displayname = Labels.airecipients["Lantheuil"]  } },
                { "Bazenville",                     new Recipient { uniqueid = 15204, category = RecipientCategories.aiatc, name = "wAIUnitATCNormandyBazenville",        displayname = Labels.airecipients["Bazenville"]  } },
                { "Sommervieu",                     new Recipient { uniqueid = 15205, category = RecipientCategories.aiatc, name = "wAIUnitATCNormandySommervieu",        displayname = Labels.airecipients["Sommervieu"]  } },
                { "Longues-sur-Mer",                new Recipient { uniqueid = 15206, category = RecipientCategories.aiatc, name = "wAIUnitATCNormandyLonguessurMer",     displayname = Labels.airecipients["Longues-sur-Mer"]  } },
                { "Le Molay",                       new Recipient { uniqueid = 15207, category = RecipientCategories.aiatc, name = "wAIUnitATCNormandyLeMolay",           displayname = Labels.airecipients["Le Molay"]  } },
                { "Sainte-Laurent-sur-Mer",         new Recipient { uniqueid = 15208, category = RecipientCategories.aiatc, name ="wAIUnitATCNormandySainteLaurentsurMer",displayname = Labels.airecipients["Sainte-Laurent-sur-Mer"]  } },
                { "Saint Pierre du Mont",           new Recipient { uniqueid = 15209, category = RecipientCategories.aiatc, name = "wAIUnitATCNormandySaintPierreduMont", displayname = Labels.airecipients["Saint Pierre du Mont"]  } },
                { "Deux Jumeaux",                   new Recipient { uniqueid = 15210, category = RecipientCategories.aiatc, name = "wAIUnitATCNormandyDeuxJumeaux",       displayname = Labels.airecipients["Deux Jumeaux"]  } },
                { "Chippelle",                      new Recipient { uniqueid = 15211, category = RecipientCategories.aiatc, name = "wAIUnitATCNormandyChippelle",         displayname = Labels.airecipients["Chippelle"]  } },
                { "Cricqueville-en-Bessin",         new Recipient { uniqueid = 15212, category = RecipientCategories.aiatc, name="wAIUnitATCNormandyCricquevilleenBessin",displayname = Labels.airecipients["Cricqueville-en-Bessin"]  } },
                { "Cardonville",                    new Recipient { uniqueid = 15213, category = RecipientCategories.aiatc, name = "wAIUnitATCNormandyCardonville",       displayname = Labels.airecipients["Cardonville"]  } },
                { "Brucheville",                    new Recipient { uniqueid = 15214, category = RecipientCategories.aiatc, name = "wAIUnitATCNormandyBrucheville",       displayname = Labels.airecipients["Brucheville"]  } },
                { "Meautis",                        new Recipient { uniqueid = 15215, category = RecipientCategories.aiatc, name = "wAIUnitATCNormandyMeautis",           displayname = Labels.airecipients["Meautis"]  } },
                { "Azeville",                       new Recipient { uniqueid = 15216, category = RecipientCategories.aiatc, name = "wAIUnitATCNormandyAzeville",          displayname = Labels.airecipients["Azeville"]  } },
                { "Cretteville",                    new Recipient { uniqueid = 15217, category = RecipientCategories.aiatc, name = "wAIUnitATCNormandyCretteville",       displayname = Labels.airecipients["Cretteville"]  } },
                { "Picauville",                     new Recipient { uniqueid = 15218, category = RecipientCategories.aiatc, name = "wAIUnitATCNormandyPicauville",        displayname = Labels.airecipients["Picauville"]  } },
                { "Biniville",                      new Recipient { uniqueid = 15219, category = RecipientCategories.aiatc, name = "wAIUnitATCNormandyBiniville",         displayname = Labels.airecipients["Biniville"]  } },
                { "Lessay",                         new Recipient { uniqueid = 15220, category = RecipientCategories.aiatc, name = "wAIUnitATCNormandyLessay",            displayname = Labels.airecipients["Lessay"]  } },
                { "Maupertus",                      new Recipient { uniqueid = 15221, category = RecipientCategories.aiatc, name = "wAIUnitATCNormandyMaupertus",         displayname = Labels.airecipients["Maupertus"]  } },
                { "Evreux",                         new Recipient { uniqueid = 15222, category = RecipientCategories.aiatc, name = "wAIUnitATCNormandyEvreux",            displayname = Labels.airecipients["Evreux"]  } },
                { "Forde",                          new Recipient { uniqueid = 15223, category = RecipientCategories.aiatc, name = "wAIUnitATCNormandyForde",             displayname = Labels.airecipients["Forde"]  } },
                { "Tangmere",                       new Recipient { uniqueid = 15224, category = RecipientCategories.aiatc, name = "wAIUnitATCNormandyTangmere",          displayname = Labels.airecipients["Tangmere"]  } },
                { "Funtington",                     new Recipient { uniqueid = 15225, category = RecipientCategories.aiatc, name = "wAIUnitATCNormandyFuntington",        displayname = Labels.airecipients["Funtington"]  } },
                { "Chailey",                        new Recipient { uniqueid = 15226, category = RecipientCategories.aiatc, name = "wAIUnitATCNormandyChailey",           displayname = Labels.airecipients["Chailey"]  } },
                { "Needs Oar Point",                new Recipient { uniqueid = 15227, category = RecipientCategories.aiatc, name = "wAIUnitATCNormandyNeedsOarPoint",     displayname = Labels.airecipients["Needs Oar Point"]  } },

                { "wAIUnitATCNormandyMaximum",      new Recipient { uniqueid = 15249, category = RecipientCategories.aiatc, name = "wAIUnitATCNormandyMaximum",           } },

                // Persian Gulf map

                { "wAIUnitATCPersianGulfNull",      new Recipient { uniqueid = 15250, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfNull",              } },

                { "Al Maktoum Intl",                new Recipient { uniqueid = 15251, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfAlMaktoumIntl", displayname = Labels.airecipients["Al Maktoum Intl"] } },
                { "Al Minhad AFB",                  new Recipient { uniqueid = 15252, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfAlMinhadAFB",   displayname = Labels.airecipients["Al Minhad AFB"] } },
                { "Dubai Intl",                     new Recipient { uniqueid = 15253, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfDubaiIntl",     displayname = Labels.airecipients["Dubai Intl"]  } },
                { "Sharjah Intl",                   new Recipient { uniqueid = 15254, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfSharjahIntl",   displayname = Labels.airecipients["Sharjah Intl"] } },
                { "Abu Musa Island",                new Recipient { uniqueid = 15255, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfAbuMusaIsland", displayname = Labels.airecipients["Abu Musa Island"] } },
                { "Sirri Island",                   new Recipient { uniqueid = 15256, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfSirriIsland",   displayname = Labels.airecipients["Sirri Island"] } },
                { "Fujairah Intl",                  new Recipient { uniqueid = 15257, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfFujairahIntl",  displayname = Labels.airecipients["Fujairah Intl"]  } },
                { "Bandar Lengeh",                  new Recipient { uniqueid = 15258, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfBandarLengeh",  displayname = Labels.airecipients["Bandar Lengeh"]  } },
                { "Khasab",                         new Recipient { uniqueid = 15259, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfKhasab",        displayname = Labels.airecipients["Khasab"] } },
                { "Qeshm Island",                   new Recipient { uniqueid = 15260, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfQeshmIsland",   displayname = Labels.airecipients["Qeshm Island"] } },
                { "Havadarya",                      new Recipient { uniqueid = 15261, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfHavadarya",     displayname = Labels.airecipients["Havadarya"] } },
                { "Bandar Abbas Intl",              new Recipient { uniqueid = 15262, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfBandarAbbasIntl",displayname = Labels.airecipients["Bandar Abbas Intl"]  } },
                { "Lar",                            new Recipient { uniqueid = 15263, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfLar",           displayname = Labels.airecipients["Lar"] } },
                { "Kerman",                         new Recipient { uniqueid = 15264, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfKerman",        displayname = Labels.airecipients["Kerman"] } },
                { "Shiraz Intl",                    new Recipient { uniqueid = 15265, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfShirazIntl",    displayname = Labels.airecipients["Shiraz Intl"] } },
                { "Al Dhafra AFB",                  new Recipient { uniqueid = 15266, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfAlDhafraAFB",   displayname = Labels.airecipients["Al Dhafra AFB"]  } },
                { "Al-Bateen",                      new Recipient { uniqueid = 15267, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfAlBateen",      displayname = Labels.airecipients["Al-Bateen"] } }, //OK
                { "Kish Intl",                      new Recipient { uniqueid = 15268, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfKishIsland",    displayname = Labels.airecipients["Kish Intl"] } },
                { "Lavan Island",                   new Recipient { uniqueid = 15269, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfLavanIsland",   displayname = Labels.airecipients["Lavan Island"]  } },
                { "Al Ain Intl",                    new Recipient { uniqueid = 15270, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfAlAinIntl",     displayname = Labels.airecipients["Al Ain Intl"] } },
                { "Bandar-e-Jask",                  new Recipient { uniqueid = 15271, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfBandareJask",   displayname = Labels.airecipients["Bandar-e-Jask"]  } },
                { "Abu Dhabi Intl",                 new Recipient { uniqueid = 15272, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfAbuDhabi",      displayname = Labels.airecipients["Abu Dhabi Intl"] } },
                { "Sas Al Nakheel",                 new Recipient { uniqueid = 15273, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfSasAlNakheel",  displayname = Labels.airecipients["Sas Al Nakheel"] } },
                { "Jiroft",                         new Recipient { uniqueid = 15274, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfJiroft",        displayname = Labels.airecipients["Jiroft"] } },
                { "Liwa AFB",                       new Recipient { uniqueid = 15275, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfLiwaAFB",       displayname = Labels.airecipients["Liwa AFB"] } },
                { "Ras Al Khaimah Intl",            new Recipient { uniqueid = 15276, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfRasAlKhaimah",  displayname = Labels.airecipients["Ras Al Khaimah Intl"]  } },
                { "Sir Abu Nuayr",                  new Recipient { uniqueid = 15277, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfSirAbuNuayr",   displayname = Labels.airecipients["Sir Abu Nuayr"]  } },

                { "wAIUnitATCPersianGulfMaximum",   new Recipient { uniqueid = 15278, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfMaximum",           } },

                // Channel Map
                
                { "wAIUnitATCChannelNull",      new Recipient { uniqueid = 15280, category = RecipientCategories.aiatc, name = "wAIUnitATCChannelNull",              } },

                { "Dunkirk Mardyck",            new Recipient { uniqueid = 15281, category = RecipientCategories.aiatc, name = "wAIUnitATCChannelDunkirkMardyck",  displayname = Labels.airecipients["Dunkirk Mardyck"]  } },
                { "Hawkinge",                   new Recipient { uniqueid = 15282, category = RecipientCategories.aiatc, name = "wAIUnitATCChannelHawkinge",  displayname = Labels.airecipients["Hawkinge"]  } },
                { "Saint Omer Longuenesse",     new Recipient { uniqueid = 15283, category = RecipientCategories.aiatc, name = "wAIUnitATCChannelSaintOmerLonguenesse",  displayname = Labels.airecipients["Saint Omer Longuenesse"]  } },
                { "Merville Calonne",           new Recipient { uniqueid = 15284, category = RecipientCategories.aiatc, name = "wAIUnitATCChannelMervilleCalonne",  displayname = Labels.airecipients["Merville Calonne"]  } },
                { "High Halden",                new Recipient { uniqueid = 15285, category = RecipientCategories.aiatc, name = "wAIUnitATCChannelHighHalden",  displayname = Labels.airecipients["High Halden"]  } },
                { "Detling",                    new Recipient { uniqueid = 15286, category = RecipientCategories.aiatc, name = "wAIUnitATCChannelDetling",  displayname = Labels.airecipients["Detling"]  } },
                { "Abbeville Drucat",           new Recipient { uniqueid = 15287, category = RecipientCategories.aiatc, name = "wAIUnitATCChannelAbbevilleDrucat",  displayname = Labels.airecipients["Abbeville Drucat"]  } },
                { "Lympne",                     new Recipient { uniqueid = 15288, category = RecipientCategories.aiatc, name = "wAIUnitATCChannelLympne",  displayname = Labels.airecipients["Lympne"]  } },
                { "Manston",                    new Recipient { uniqueid = 15289, category = RecipientCategories.aiatc, name = "wAIUnitATCChannelManston",  displayname = Labels.airecipients["Manston"]  } },

                { "wAIUnitATCChannelMaximum",   new Recipient { uniqueid = 15290, category = RecipientCategories.aiatc, name = "wAIUnitATCChannelMaximum",           } },

                // Syria map
                
                { "wAIUnitATCSyriaNull",        new Recipient { uniqueid = 15291, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaNull",              } },

                { "Beirut-Rafic Hariri",        new Recipient { uniqueid = 15292, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaBeirutRaficHariri",  displayname = Labels.airecipients["Beirut-Rafic Hariri"]  } },
                { "Rayak",                      new Recipient { uniqueid = 15293, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaRayak",  displayname = Labels.airecipients["Rayak"]  } },
                { "Wujah Al Hajar",             new Recipient { uniqueid = 15294, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaWujahAlHajar",  displayname = Labels.airecipients["Wujah Al Hajar"]  } },
                { "Kiryat Shmona",              new Recipient { uniqueid = 15295, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaKiryatShmona",  displayname = Labels.airecipients["Kiryat Shmona"]  } },
                { "Mezzeh",                     new Recipient { uniqueid = 15296, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaMezzeh",  displayname = Labels.airecipients["Mezzeh"]  } },
                { "Qabr as Sitt",               new Recipient { uniqueid = 15297, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaQabrasSitt",  displayname = Labels.airecipients["Qabr as Sitt"]  } },
                { "Rene Mouawad",               new Recipient { uniqueid = 15298, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaReneMouawad",  displayname = Labels.airecipients["Rene Mouawad"]  } },
                { "Marj as Sultan North",       new Recipient { uniqueid = 15299, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaMarjasSultanNorth",  displayname = Labels.airecipients["Marj as Sultan North"]  } },
                { "Marj as Sultan South",       new Recipient { uniqueid = 15300, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaMarjasSultanSouth",  displayname = Labels.airecipients["Marj as Sultan South"]  } },
                { "Marj Ruhayyil",              new Recipient { uniqueid = 15301, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaMarjRuhayyil",  displayname = Labels.airecipients["Marj Ruhayyil"]  } },
                { "Al-Dumayr",                  new Recipient { uniqueid = 15302, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaAlDumayr",  displayname = Labels.airecipients["Al-Dumayr"]  } },
                { "Haifa",                      new Recipient { uniqueid = 15303, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaHaifa",  displayname = Labels.airecipients["Haifa"]  } },
                { "An Nasiriyah",               new Recipient { uniqueid = 15304, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaAnNasiriyah",  displayname = Labels.airecipients["An Nasiriyah"]  } },
                { "Al Qusayr",                  new Recipient { uniqueid = 15305, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaAlQusayr",  displayname = Labels.airecipients["Al Qusayr"]  } },
                { "Khalkhalah",                 new Recipient { uniqueid = 15306, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaKhalkhalah",  displayname = Labels.airecipients["Khalkhalah"]  } },
                { "Ramat David",                new Recipient { uniqueid = 15307, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaRamatDavid",  displayname = Labels.airecipients["Ramat David"]  } },
                { "Megiddo",                    new Recipient { uniqueid = 15308, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaMegiddo",  displayname = Labels.airecipients["Megiddo"]  } },
                { "Eyn Shemer",                 new Recipient { uniqueid = 15309, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaEynShemer",  displayname = Labels.airecipients["Eyn Shemer"]  } },
                { "Bassel Al-Assad",            new Recipient { uniqueid = 15310, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaBasselAlAssad",  displayname = Labels.airecipients["Bassel Al-Assad"]  } },
                { "Abu al-Duhur",               new Recipient { uniqueid = 15311, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaAbualDuhur",  displayname = Labels.airecipients["Abu al-Duhur"]  } },
                { "Taftanaz",                   new Recipient { uniqueid = 15312, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaTaftanaz",  displayname = Labels.airecipients["Taftanaz"]  } },
                { "Hatay",                      new Recipient { uniqueid = 15313, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaHatay",  displayname = Labels.airecipients["Hatay"]  } },
                { "Kuweires",                   new Recipient { uniqueid = 15314, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaKuweires",  displayname = Labels.airecipients["Kuweires"]  } },
                { "Minakh",                     new Recipient { uniqueid = 15315, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaMinakh",  displayname = Labels.airecipients["Minakh"]  } },
                { "Jirah",                      new Recipient { uniqueid = 15316, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaJirah",  displayname = Labels.airecipients["Jirah"]  } },
                { "Adana Sakirpasa",            new Recipient { uniqueid = 15317, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaAdanaSakirpasa",  displayname = Labels.airecipients["Adana Sakirpasa"]  } },
                { "Incirlik",                   new Recipient { uniqueid = 15318, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaIncirlik",  displayname = Labels.airecipients["Incirlik"]  } },
                { "Damascus",                   new Recipient { uniqueid = 15319, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaDamascus",  displayname = Labels.airecipients["Damascus"]  } },
                { "Tha'lah",                    new Recipient { uniqueid = 15320, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaThalah",  displayname = Labels.airecipients["Tha'lah"]  } },
                { "Larnaca",                    new Recipient { uniqueid = 15321, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaLarnaca",  displayname = Labels.airecipients["Larnaca"]  } },
                { "Akrotiri",                   new Recipient { uniqueid = 15322, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaAkrotiri",  displayname = Labels.airecipients["Akrotiri"]  } },
                { "King Hussein Air College",   new Recipient { uniqueid = 15323, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaKingHussein",  displayname = Labels.airecipients["King Hussein Air College"]  } },
                { "At Tanf",                    new Recipient { uniqueid = 15324, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaAtTanf",  displayname = Labels.airecipients["At Tanf"]  } },
                { "H3",                         new Recipient { uniqueid = 15326, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaH3",  displayname = Labels.airecipients["H3"]  } },
                { "H3 Northwest",               new Recipient { uniqueid = 15327, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaH3Northwest",  displayname = Labels.airecipients["H3 Northwest"]  } },
                { "H3 Southwest",               new Recipient { uniqueid = 15328, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaH3Southwest",  displayname = Labels.airecipients["H3 Southwest"]  } },
                { "H4",                         new Recipient { uniqueid = 15329, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaH4",  displayname = Labels.airecipients["H4"]  } },
                { "Paphos",                     new Recipient { uniqueid = 15330, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaPaphos",  displayname = Labels.airecipients["Paphos"]  } },
                { "Lakatamia",                  new Recipient { uniqueid = 15331, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaLakatamia",  displayname = Labels.airecipients["Lakatamia"]  } },
                { "Ercan",                      new Recipient { uniqueid = 15332, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaErcan",  displayname = Labels.airecipients["Ercan"]  } },
                { "Muwaffaq Salti",             new Recipient { uniqueid = 15333, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaMuwaffaqSalti",  displayname = Labels.airecipients["Muwaffaq Salti"]  } },
                { "King Abdullah II",           new Recipient { uniqueid = 15334, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaKingAbdullahII",  displayname = Labels.airecipients["King Abdullah II"]  } },
                //{ "Tel Nof",                    new Recipient { uniqueid = 15335, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaTelNof",  displayname = Labels.airecipients["Tel Nof"]  } }, //Already defined in Sinai
                //{ "Hatzor",                     new Recipient { uniqueid = 15336, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaHatzor",  displayname = Labels.airecipients["Hatzor"]  } }, //Already defined in Sinai
                //{ "Palmachim",                  new Recipient { uniqueid = 15337, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaPalmachim",  displayname = Labels.airecipients["Palmachim"]  } }, //Already defined in Sinai
                //{ "Ben Gurion",                 new Recipient { uniqueid = 15338, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaBenGurion",  displayname = Labels.airecipients["Ben Gurion"]  } }, //Already defined in Sinai
                { "Marka",                      new Recipient { uniqueid = 15339, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaMarka",  displayname = Labels.airecipients["Marka"]  } },
                { "Herzliya",                   new Recipient { uniqueid = 15340, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaHerzliya",  displayname = Labels.airecipients["Herzliya"]  } },
                { "Prince Hassan",              new Recipient { uniqueid = 15341, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaPrinceHassan",  displayname = Labels.airecipients["Prince Hassan"]  } },

                { "wAIUnitATCSyriaMaximum",     new Recipient { uniqueid = 15349, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaMaximum",           } },

                // Sinai map
                
                { "wAIUnitATCSinaiNull",        new Recipient { uniqueid = 15350, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiNull",              } },

                { "Al Mansurah",                new Recipient { uniqueid = 15351, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiAlMansurah",  displayname = Labels.airecipients["Al Mansurah"]  } },
                { "AzZaqaziq",                  new Recipient { uniqueid = 15352, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiAzZaqaziq",  displayname = Labels.airecipients["AzZaqaziq"]  } },
                { "As Salihiyah",               new Recipient { uniqueid = 15353, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiAsSalihiyah",  displayname = Labels.airecipients["As Salihiyah"]  } },
                { "Bilbeis Air Base",           new Recipient { uniqueid = 15354, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiBilbeisAirBase",  displayname = Labels.airecipients["Bilbeis Air Base"]  } },
                { "Inshas Airbase",             new Recipient { uniqueid = 15355, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiInshasAirbase",  displayname = Labels.airecipients["Inshas Airbase"]  } },
                { "Abu Suwayr",                 new Recipient { uniqueid = 15356, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiAbuSuwayr",  displayname = Labels.airecipients["Abu Suwayr"]  } },
                { "Al Ismailiyah",              new Recipient { uniqueid = 15357, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiAlIsmailiyah",  displayname = Labels.airecipients["Al Ismailiyah"]  } },
                { "Cairo International Airport",new Recipient { uniqueid = 15358, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiCairoInternationalAirport",  displayname = Labels.airecipients["Cairo International Airport"]  } },
                { "Difarsuwar Airfield",        new Recipient { uniqueid = 15359, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiDifarsuwarAirfield",  displayname = Labels.airecipients["Difarsuwar Airfield"]  } },
                { "Wadi al Jandali",            new Recipient { uniqueid = 15360, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiWadialJandali",  displayname = Labels.airecipients["Wadi al Jandali"]  } },
                { "Fayed",                      new Recipient { uniqueid = 15361, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiFayed",  displayname = Labels.airecipients["Fayed"]  } },
                { "Baluza",                     new Recipient { uniqueid = 15362, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiBaluza",  displayname = Labels.airecipients["Baluza"]  } },
                { "Cairo West",                 new Recipient { uniqueid = 15363, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiCairoWest",  displayname = Labels.airecipients["Cairo West"]  } },
                { "Kibrit Air Base",            new Recipient { uniqueid = 15364, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiKibritAirBase",  displayname = Labels.airecipients["Kibrit Air Base"]  } },
                { "Melez",                      new Recipient { uniqueid = 15365, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiMelez",  displayname = Labels.airecipients["Melez"]  } },
                { "Bir Hasanah",                new Recipient { uniqueid = 15366, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiBirHasanah",  displayname = Labels.airecipients["Bir Hasanah"]  } },
                { "El Arish",                   new Recipient { uniqueid = 15367, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiElArish",  displayname = Labels.airecipients["El Arish"]  } },
                { "El Gora",                    new Recipient { uniqueid = 15368, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiElGora",  displayname = Labels.airecipients["El Gora"]  } },
                { "Abu Rudeis",                 new Recipient { uniqueid = 15369, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiAbuRudeis",  displayname = Labels.airecipients["Abu Rudeis"]  } },
                { "Kedem",                      new Recipient { uniqueid = 15370, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiKedem",  displayname = Labels.airecipients["Kedem"]  } },
                { "Ramon Airbase",              new Recipient { uniqueid = 15371, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiRamonAirbase",  displayname = Labels.airecipients["Ramon Airbase"]  } },
                { "Hatzerim",                   new Recipient { uniqueid = 15372, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiHatzerim",  displayname = Labels.airecipients["Hatzerim"]  } },
                { "Hatzor",                     new Recipient { uniqueid = 15373, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiHatzor",  displayname = Labels.airecipients["Hatzor"]  } },
                { "Palmachim",                  new Recipient { uniqueid = 15374, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiPalmachim",  displayname = Labels.airecipients["Palmachim"]  } },
                { "Tel Nof",                    new Recipient { uniqueid = 15375, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiTelNof",  displayname = Labels.airecipients["Tel Nof"]  } },
                { "Nevatim",                    new Recipient { uniqueid = 15376, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiNevatim",  displayname = Labels.airecipients["Nevatim"]  } },
                { "Sde Dov",                    new Recipient { uniqueid = 15377, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiSdeDov",  displayname = Labels.airecipients["Sde Dov"]  } },
                { "Ben-Gurion",                 new Recipient { uniqueid = 15378, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiBenGurion",  displayname = Labels.airecipients["Ben-Gurion"]  } },
                { "Ovda",                       new Recipient { uniqueid = 15379, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiOvda",  displayname = Labels.airecipients["Ovda"]  } },
                { "St Catherine",               new Recipient { uniqueid = 15380, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiStCatherine",  displayname = Labels.airecipients["St Catherine"]  } },
                { "Wadi Abu Rish",              new Recipient { uniqueid = 15381, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiWadiAbuRish",  displayname = Labels.airecipients["Wadi Abu Rish"]  } },
                { "Ramon International Airport", new Recipient { uniqueid = 15382, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiRamonInternationalAirport",  displayname = Labels.airecipients["Ramon International Airport"]  } },
                { "Sharm El Sheikh International Airport",  new Recipient { uniqueid = 15383, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiSharmElSheikh",  displayname = Labels.airecipients["Sharm El Sheikh International Airport"]  } },
                { "Hurghada International Airport",  new Recipient { uniqueid = 15384, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiHurghadaInternationalAirport",  displayname = Labels.airecipients["Hurghada International Airport"]  } },
                { "Al Bahr al Ahmar",           new Recipient { uniqueid = 15385, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiAlBahralAhmar",  displayname = Labels.airecipients["Al Bahr al Ahmar"]  } },
                { "El Minya",                   new Recipient { uniqueid = 15386, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiElMinya",  displayname = Labels.airecipients["El Minya"]  } },
                { "Beni Suef",                  new Recipient { uniqueid = 15387, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiBeniSuef",  displayname = Labels.airecipients["Beni Suef"]  } },
                { "Kom Awshim",                 new Recipient { uniqueid = 15388, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiKomAwshim",  displayname = Labels.airecipients["Kom Awshim"]  } },
                { "Quwaysina",                  new Recipient { uniqueid = 15389, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiQuwaysina",  displayname = Labels.airecipients["Quwaysina"]  } },
                { "Birma Air Base",             new Recipient { uniqueid = 15390, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiBirmaAirBase",  displayname = Labels.airecipients["Birma Air Base"]  } },
                { "Gebel El Basur Air Base",    new Recipient { uniqueid = 15391, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiGebelElBasur",  displayname = Labels.airecipients["Gebel El Basur Air Base"]  } },
                { "Al Rahmaniyah Air Base",     new Recipient { uniqueid = 15392, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiAlRahmaniyah",  displayname = Labels.airecipients["Al Rahmaniyah Air Base"]  } },
                { "Jiyanklis Air Base",                  new Recipient { uniqueid = 15393, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiJiyanklis",  displayname = Labels.airecipients["Jiyanklis Air Base"]  } },
                { "Borj El Arab International Airport",  new Recipient { uniqueid = 15394, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiJBorjElArab",  displayname = Labels.airecipients["Borj El Arab International Airport"]  } },

                { "wAIUnitATCSinaiMaximum",     new Recipient { uniqueid = 15399, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiMaximum",           } },

                // South Atlantic map

                { "wAIUnitATCSAtlanticNull",      new Recipient { uniqueid = 15400, category = RecipientCategories.aiatc, name = "wAIUnitATCSAtlanticNull",              } },

                { "Mount Pleasant",             new Recipient { uniqueid = 15401, category = RecipientCategories.aiatc, name = "wAIUnitATCCSAtlanticMountPleasant",  displayname = Labels.airecipients["Mount Pleasant"]  } },
                { "Goose Green",                new Recipient { uniqueid = 15402, category = RecipientCategories.aiatc, name = "wAIUnitATCSAtlanticGooseGreen",  displayname = Labels.airecipients["Goose Green"]  } },
                { "San Carlos FOB",             new Recipient { uniqueid = 15403, category = RecipientCategories.aiatc, name = "wAIUnitATCSAtlanticSanCarlosFOB",  displayname = Labels.airecipients["San Carlos FOB"]  } },
                { "Port Stanley",               new Recipient { uniqueid = 15404, category = RecipientCategories.aiatc, name = "wAIUnitATCSAtlanticPortStanley",  displayname = Labels.airecipients["Port Stanley"]  } },
                { "Aerodromo De Tolhuin",       new Recipient { uniqueid = 15405, category = RecipientCategories.aiatc, name = "wAIUnitATCSAtlanticAerodromoDeTolhuin",  displayname = Labels.airecipients["Aerodromo De Tolhuin"]  } },
                { "Rio Grande",                 new Recipient { uniqueid = 15406, category = RecipientCategories.aiatc, name = "wAIUnitATCSAtlanticRioGrande",  displayname = Labels.airecipients["Rio Grande"]  } },
                { "Puerto Williams",            new Recipient { uniqueid = 15407, category = RecipientCategories.aiatc, name = "wAIUnitATCSAtlanticPuertoWilliams",  displayname = Labels.airecipients["Puerto Williams"]  } },
                { "San Julian",                 new Recipient { uniqueid = 15408, category = RecipientCategories.aiatc, name = "wAIUnitATCSAtlanticSanJulian",  displayname = Labels.airecipients["San Julian"]  } },
                { "Ushuaia Helo Port",          new Recipient { uniqueid = 15409, category = RecipientCategories.aiatc, name = "wAIUnitATCSAtlanticUshuaiaHeloPort",  displayname = Labels.airecipients["Ushuaia Helo Port"]  } },
                { "Ushuaia",                    new Recipient { uniqueid = 15410, category = RecipientCategories.aiatc, name = "wAIUnitATCSAtlanticUshuaia",  displayname = Labels.airecipients["Ushuaia"]  } },
                { "Pampa Guanaco",              new Recipient { uniqueid = 15411, category = RecipientCategories.aiatc, name = "wAIUnitATCSAtlanticPampaGuanaco",  displayname = Labels.airecipients["Pampa Guanaco"]  } },
                { "Puerto Santa Cruz",          new Recipient { uniqueid = 15412, category = RecipientCategories.aiatc, name = "wAIUnitATCSAtlanticPuertoSantaCruz",  displayname = Labels.airecipients["Puerto Santa Cruz"]  } },
                { "Rio Chico",                  new Recipient { uniqueid = 15413, category = RecipientCategories.aiatc, name = "wAIUnitATCSAtlanticRioChico",  displayname = Labels.airecipients["Rio Chico"]  } },
                { "Rio Gallegos",               new Recipient { uniqueid = 15414, category = RecipientCategories.aiatc, name = "wAIUnitATCSAtlanticRioGallegos",  displayname = Labels.airecipients["Rio Gallegos"]  } },
                { "Franco Bianco",              new Recipient { uniqueid = 15415, category = RecipientCategories.aiatc, name = "wAIUnitATCSAtlanticFrancoBianco",  displayname = Labels.airecipients["Franco Bianco"]  } },
                { "Comandante Luis Piedrabuena",  new Recipient { uniqueid = 15416, category = RecipientCategories.aiatc, name = "wAIUnitATCSAtlanticComandanteLuisPiedrabuena",  displayname = Labels.airecipients["Comandante Luis Piedrabuena"]  } },
                { "Porvenir Airfield",          new Recipient { uniqueid = 15417, category = RecipientCategories.aiatc, name = "wAIUnitATCSAtlanticPorvenirAirfield",  displayname = Labels.airecipients["Porvenir Airfield"]  } },
                { "Punta Arenas",               new Recipient { uniqueid = 15418, category = RecipientCategories.aiatc, name = "wAIUnitATCSAtlanticPuntaArenas",  displayname = Labels.airecipients["Punta Arenas"]  } },
                { "Aeropuerto de Gobernador Gregores",  new Recipient { uniqueid = 15419, category = RecipientCategories.aiatc, name = "wAIUnitATCSAtlanticAeropuertodeGobernadorGregores",  displayname = Labels.airecipients["Aeropuerto de Gobernador Gregores"]  } },
                { "Rio Turbio",                 new Recipient { uniqueid = 15420, category = RecipientCategories.aiatc, name = "wAIUnitATCSAtlanticRioTurbio",  displayname = Labels.airecipients["Rio Turbio"]  } },
                { "El Calafate",                new Recipient { uniqueid = 15421, category = RecipientCategories.aiatc, name = "wAIUnitATCSAtlanticElCalafate",  displayname = Labels.airecipients["El Calafate"]  } },
                { "Puerto Natales",             new Recipient { uniqueid = 15422, category = RecipientCategories.aiatc, name = "wAIUnitATCSAtlanticPuertoNatales",  displayname = Labels.airecipients["Puerto Natales"]  } },
                { "Aerodromo O'Higgins",        new Recipient { uniqueid = 15423, category = RecipientCategories.aiatc, name = "wAIUnitATCSAtlanticAerodromoO'Higgins",  displayname = Labels.airecipients["Aerodromo O'Higgins"]  } },

                { "wAIUnitATCSAtlanticMaximum", new Recipient { uniqueid = 15440, category = RecipientCategories.aiatc, name = "wAIUnitATCSAtlanticMaximum",           } },
                
                // Kola Penisular Map

                { "wAIUnitATCKolaNull",         new Recipient { uniqueid = 15450, category = RecipientCategories.aiatc, name = "wAIUnitATCKolaNull",              } },
                
                { "Andoya",                     new Recipient { uniqueid = 15451, category = RecipientCategories.aiatc, name = "wAIUnitATCKolaAndoya", displayname = Labels.airecipients["Andoya"] } },
                { "Bodo",                       new Recipient { uniqueid = 15452, category = RecipientCategories.aiatc, name = "wAIUnitATCKolaBodo", displayname = Labels.airecipients["Bodo"] } },
                { "Bardufoss",                  new Recipient { uniqueid = 15453, category = RecipientCategories.aiatc, name = "wAIUnitATCKolaBardufoss", displayname = Labels.airecipients["Bardufoss"] } },
                { "Evenes",                     new Recipient { uniqueid = 15454, category = RecipientCategories.aiatc, name = "wAIUnitATCKolaEvenes", displayname = Labels.airecipients["Evenes"] } },
                { "Alta",                       new Recipient { uniqueid = 15455, category = RecipientCategories.aiatc, name = "wAIUnitATCKolaAlta", displayname = Labels.airecipients["Alta"] } },
                { "Banak",                      new Recipient { uniqueid = 15456, category = RecipientCategories.aiatc, name = "wAIUnitATCKolaBanak", displayname = Labels.airecipients["Banak"] } },
                { "Enontekio",                  new Recipient { uniqueid = 15457, category = RecipientCategories.aiatc, name = "wAIUnitATCKolaEnontekio", displayname = Labels.airecipients["Enontekio"] } },
                { "Kiruna",                     new Recipient { uniqueid = 15458, category = RecipientCategories.aiatc, name = "wAIUnitATCKolaKiruna", displayname = Labels.airecipients["Kiruna"] } },
                { "Kalixfors",                  new Recipient { uniqueid = 15459, category = RecipientCategories.aiatc, name = "wAIUnitATCKolaKalixfors", displayname = Labels.airecipients["Kalixfors"] } },
                { "Jokkmokk",                   new Recipient { uniqueid = 15460, category = RecipientCategories.aiatc, name = "wAIUnitATCKolaJokkmokk", displayname = Labels.airecipients["Jokkmokk"] } },
                { "Vidsel",                     new Recipient { uniqueid = 15461, category = RecipientCategories.aiatc, name = "wAIUnitATCKolaVidsel", displayname = Labels.airecipients["Vidsel"] } },
                { "Kallax",                     new Recipient { uniqueid = 15462, category = RecipientCategories.aiatc, name = "wAIUnitATCKolaKallax", displayname = Labels.airecipients["Kallax"] } },
                { "Kemi Torino",                new Recipient { uniqueid = 15463, category = RecipientCategories.aiatc, name = "wAIUnitATCKolaKemiTorino", displayname = Labels.airecipients["Kemi Torino"] } },
                { "Hosio",                      new Recipient { uniqueid = 15464, category = RecipientCategories.aiatc, name = "wAIUnitATCKolaHosio", displayname = Labels.airecipients["Hosio"] } },
                { "Rovaniemi",                  new Recipient { uniqueid = 15465, category = RecipientCategories.aiatc, name = "wAIUnitATCKolaRovaniemi", displayname = Labels.airecipients["Rovaniemi"] } },
                { "Vuojarvi",                   new Recipient { uniqueid = 15466, category = RecipientCategories.aiatc, name = "wAIUnitATCKolaVuojarvi", displayname = Labels.airecipients["Vuojarvi"] } },
                { "Sodankyla",                  new Recipient { uniqueid = 15467, category = RecipientCategories.aiatc, name = "wAIUnitATCKolaSodankyla", displayname = Labels.airecipients["Sodankyla"] } },
                { "Kittila",                    new Recipient { uniqueid = 15468, category = RecipientCategories.aiatc, name = "wAIUnitATCKolaKittila", displayname = Labels.airecipients["Kittila"] } },
                { "Ivalo",                      new Recipient { uniqueid = 15469, category = RecipientCategories.aiatc, name = "wAIUnitATCKolaIvalo", displayname = Labels.airecipients["Ivalo"] } },
                { "Kirkenes",                   new Recipient { uniqueid = 15470, category = RecipientCategories.aiatc, name = "wAIUnitATCKolaKirkenes", displayname = Labels.airecipients["Kirkenes"] } },
                { "Severomorsk-1",              new Recipient { uniqueid = 15471, category = RecipientCategories.aiatc, name = "wAIUnitATCKolaSeveromorsk1", displayname = Labels.airecipients["Severomorsk-1"] } },
                { "Severomorsk-3",              new Recipient { uniqueid = 15472, category = RecipientCategories.aiatc, name = "wAIUnitATCKolaSeveromorsk3", displayname = Labels.airecipients["Severomorsk-3"] } },
                { "Murmansk International",     new Recipient { uniqueid = 15473, category = RecipientCategories.aiatc, name = "wAIUnitATCKolaMurmanskInternational", displayname = Labels.airecipients["Murmansk International"] } },
                { "Olenya",                     new Recipient { uniqueid = 15474, category = RecipientCategories.aiatc, name = "wAIUnitATCKolaOlenya", displayname = Labels.airecipients["Olenya"] } },
                { "Monchegorsk",                new Recipient { uniqueid = 15475, category = RecipientCategories.aiatc, name = "wAIUnitATCKolaMonchegorsk", displayname = Labels.airecipients["Monchegorsk"] } },
                { "Alakurtti",                  new Recipient { uniqueid = 15476, category = RecipientCategories.aiatc, name = "wAIUnitATCKolaAlakurtti", displayname = Labels.airecipients["Alakurtti"] } },
                { "Kuusamo",                    new Recipient { uniqueid = 15477, category = RecipientCategories.aiatc, name = "wAIUnitATCKolaKuusamo", displayname = Labels.airecipients["Kuusamo"] } },

                { "wAIUnitATCKolaMaximum",      new Recipient { uniqueid = 15480, category = RecipientCategories.aiatc, name = "wAIUnitATCKolaMaximum",           } },

                // Afghanistan Map

                { "wAIUnitATCAfghanNull",       new Recipient { uniqueid = 15490, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanNull",              } },
                   // South
                { "Tarinkot",                   new Recipient { uniqueid = 15491, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanTarinkot",  displayname = Labels.airecipients["Tarinkot"]  } },
                { "kandahar",                   new Recipient { uniqueid = 15492, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghankandahar",  displayname = Labels.airecipients["kandahar"]  } },
                { "Kandahar Heliport",          new Recipient { uniqueid = 15493, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanKandaharHeliport",  displayname = Labels.airecipients["Kandahar Heliport"]  } },
                { "Chaghcharan",                new Recipient { uniqueid = 15494, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanChaghcharan",  displayname = Labels.airecipients["Chaghcharan"]  } },
                { "Bost",                       new Recipient { uniqueid = 15495, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanBost",  displayname = Labels.airecipients["Bost"]  } },
                { "Dwyer",                      new Recipient { uniqueid = 15496, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanDwyer",  displayname = Labels.airecipients["Dwyer"]  } },
                { "Camp Bastion",               new Recipient { uniqueid = 15497, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanCampBastion",  displayname = Labels.airecipients["Camp Bastion"]  } },
                { "Camp Bastion Heliport",      new Recipient { uniqueid = 15498, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanCampBastionHeliport",  displayname = Labels.airecipients["Camp Bastion Heliport"]  } },
                { "Qala i Naw",                 new Recipient { uniqueid = 15499, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanQalaiNaw",  displayname = Labels.airecipients["Qala i Naw"]  } },
                { "Herat",                      new Recipient { uniqueid = 15500, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanHerat",  displayname = Labels.airecipients["Herat"]  } },
                { "Shindand",                   new Recipient { uniqueid = 15501, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanShindand",  displayname = Labels.airecipients["Shindand"]  } },
                { "Shindand Heliport",          new Recipient { uniqueid = 15502, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanShindandHeliport",  displayname = Labels.airecipients["Shindand Heliport"]  } },
                { "Farah",                      new Recipient { uniqueid = 15503, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanFarah",  displayname = Labels.airecipients["Farah"]  } },
                { "Nimroz",                     new Recipient { uniqueid = 15504, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanNimroz",  displayname = Labels.airecipients["Nimroz"]  } },
                   // East
                { "Bagram",                     new Recipient { uniqueid = 15505, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanBagram",  displayname = Labels.airecipients["Bagram"]  } },
                { "Bamyan",                     new Recipient { uniqueid = 15506, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanBamyan",  displayname = Labels.airecipients["Bamyan"]  } },
                { "Ghazni Heliport",            new Recipient { uniqueid = 15507, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanGhazniHeliport",  displayname = Labels.airecipients["Ghazni Heliport"]  } },
                { "Gardez",                     new Recipient { uniqueid = 15508, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanGardez",  displayname = Labels.airecipients["Gardez"]  } },
                { "Jalalabad",                  new Recipient { uniqueid = 15509, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanJalalabad",  displayname = Labels.airecipients["Jalalabad"]  } },
                { "Kabul",                      new Recipient { uniqueid = 15510, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanKabul",  displayname = Labels.airecipients["Kabul"]  } },
                { "Khost",                      new Recipient { uniqueid = 15511, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanKhost",  displayname = Labels.airecipients["Khost"]  } },
                { "Khost Heliport",             new Recipient { uniqueid = 15512, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanKhostHeliport",  displayname = Labels.airecipients["Khost Heliport"]  } },
                { "Sharan",                     new Recipient { uniqueid = 15513, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanSharan",  displayname = Labels.airecipients["Sharan"]  } },
                { "Urgoon Heliport",            new Recipient { uniqueid = 15514, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanUrgoonHeliport",  displayname = Labels.airecipients["Urgoon Heliport"]  } },
                { "Maymana Zahiraddin Faryabi", new Recipient { uniqueid = 15515, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanMaymanaZahiraddinFaryabi",  displayname = Labels.airecipients["Maymana Zahiraddin Faryabi"]  } },
                { "Mary",                       new Recipient { uniqueid = 15516, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanMary",  displayname = Labels.airecipients["Mary"]  } },
                { "Mary North",                 new Recipient { uniqueid = 15517, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanMaryNorth",  displayname = Labels.airecipients["Mary North"]  } },
                   // North TODO
                { "wAIUnitATCAfghanMaximum",    new Recipient { uniqueid = 15540, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanMaximum",           } },

                // Iraq
                { "wAIUnitATCIraqNull",         new Recipient { uniqueid = 15550, category = RecipientCategories.aiatc, name = "wAIUnitATCIraqNull",              } }, 
                   // North
                { "Al-Asad Airbase",            new Recipient { uniqueid = 15551, category = RecipientCategories.aiatc, name = "wAIUnitATCIraqAl-AsadAirbase",  displayname = Labels.airecipients["Al-Asad Airbase"]  } },
                { "Al-Sahra Airport",           new Recipient { uniqueid = 15552, category = RecipientCategories.aiatc, name = "wAIUnitATCIraqAl-SahraAirport",  displayname = Labels.airecipients["Al-Sahra Airport"]  } },
                { "Al-Salam Airbase",           new Recipient { uniqueid = 15553, category = RecipientCategories.aiatc, name = "wAIUnitATCIraqAl-SalamAirbase",  displayname = Labels.airecipients["Al-Salam Airbase"]  } },
                { "Al-Taji Airport",            new Recipient { uniqueid = 15554, category = RecipientCategories.aiatc, name = "wAIUnitATCIraqAl-TajiAirport",  displayname = Labels.airecipients["Al-Taji Airport"]  } },
                { "Al-Taquddum Airport",        new Recipient { uniqueid = 15555, category = RecipientCategories.aiatc, name = "wAIUnitATCIraqAl-TaquddumAirport",  displayname = Labels.airecipients["Al-Taquddum Airport"]  } },
                { "Baghdad International Airport",  new Recipient { uniqueid = 15556, category = RecipientCategories.aiatc, name = "wAIUnitATCIraqBaghdadInternationalAirport",  displayname = Labels.airecipients["Baghdad International Airport"]  } },
                { "Balad Airbase",              new Recipient { uniqueid = 15557, category = RecipientCategories.aiatc, name = "wAIUnitATCIraqBaladAirbase",  displayname = Labels.airecipients["Balad Airbase"]  } },
                { "Bashur Airport",             new Recipient { uniqueid = 15558, category = RecipientCategories.aiatc, name = "wAIUnitATCIraqBashurAirport",  displayname = Labels.airecipients["Bashur Airport"]  } },
                { "Erbil International Airport",    new Recipient { uniqueid = 15559, category = RecipientCategories.aiatc, name = "wAIUnitATCIraqErbilInternationalAirport",  displayname = Labels.airecipients["Erbil International Airport"]  } },
                { "K1 Base",                    new Recipient { uniqueid = 15560, category = RecipientCategories.aiatc, name = "wAIUnitATCIraqK1Base",  displayname = Labels.airecipients["K1 Base"]  } },
                { "Kirkuk International Airport",   new Recipient { uniqueid = 15561, category = RecipientCategories.aiatc, name = "wAIUnitATCIraqKirkukInternationalAirport",  displayname = Labels.airecipients["Kirkuk International Airport"]  } },
                { "Qayyarah Airfield West",     new Recipient { uniqueid = 15562, category = RecipientCategories.aiatc, name = "wAIUnitATCIraqQayyarahAirfieldWest",  displayname = Labels.airecipients["Qayyarah Airfield West"]  } },
                { "Sulaimaniyah International Airport",     new Recipient { uniqueid = 15563, category = RecipientCategories.aiatc, name = "wAIUnitATCIraqSulaimaniyahInternationalAirport",  displayname = Labels.airecipients["Sulaimaniyah International Airport"]  } },
                   // South TODO
                { "wAIUnitATCIraqMaximum",      new Recipient { uniqueid = 15590, category = RecipientCategories.aiatc, name = "wAIUnitATCIraqMaximum",           } },

                // Germany Cold War                
                { "wAIUnitATCColdWarNull",      new Recipient { uniqueid = 15750, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarNull",              } }, 
                   // Nato
                { "Airracing Frankfurt",        new Recipient { uniqueid = 15751, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarAirracingFrankfurt",  displayname = Labels.airecipients["Airracing Frankfurt"]  } },
                { "Airracing Koblenz",          new Recipient { uniqueid = 15752, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarAirracingKoblenz",  displayname = Labels.airecipients["Airracing Koblenz"]  } },
                { "Airracing Lubeck",           new Recipient { uniqueid = 15753, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarAirracingLubeck",  displayname = Labels.airecipients["Airracing Lubeck"]  } },
                { "Bad Durkheim",               new Recipient { uniqueid = 15754, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarBadDurkheim",  displayname = Labels.airecipients["Bad Durkheim"]  } },
                { "Bienenfarm",                 new Recipient { uniqueid = 15755, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarBienenfarm",  displayname = Labels.airecipients["Bienenfarm"]  } },
                { "Dedelow",                    new Recipient { uniqueid = 15756, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarDedelow",  displayname = Labels.airecipients["Dedelow"]  } },
                { "Gardelegen",                 new Recipient { uniqueid = 15757, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarGardelegen",  displayname = Labels.airecipients["Gardelegen"]  } },
                { "Gelnhausen",                 new Recipient { uniqueid = 15758, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarGelnhausen",  displayname = Labels.airecipients["Gelnhausen"]  } },
                { "Glindbruchkippe",            new Recipient { uniqueid = 15759, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarGlindbruchkippe",  displayname = Labels.airecipients["Glindbruchkippe"]  } },
                { "GrosseWiese",                new Recipient { uniqueid = 15760, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarGrosseWiese",  displayname = Labels.airecipients["GrosseWiese"]  } },
                { "Hasselfelde",                new Recipient { uniqueid = 15761, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarHasselfelde", displayname = Labels.airecipients["Hasselfelde"] } },
                { "Herrenteich",                new Recipient { uniqueid = 15762, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarHerrenteich", displayname = Labels.airecipients["Herrenteich"] } },
                { "Hockenheim",                 new Recipient { uniqueid = 15763, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarHockenheim", displayname = Labels.airecipients["Hockenheim"] } },
                { "Kammermark",                 new Recipient { uniqueid = 15764, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarKammermark", displayname = Labels.airecipients["Kammermark"] } },
                { "Langenselbold",              new Recipient { uniqueid = 15765, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarLangenselbold", displayname = Labels.airecipients["Langenselbold"] } },
                { "Luneburg",                   new Recipient { uniqueid = 15766, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarLuneburg", displayname = Labels.airecipients["Luneburg"] } },
                { "Albstadt Degerfeld",         new Recipient { uniqueid = 15767, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarAlbstadtDegerfeld", displayname = Labels.airecipients["Albstadt Degerfeld"] } },
                { "Ober-Morlen",                new Recipient { uniqueid = 15768, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarOberMorlen", displayname = Labels.airecipients["Ober-Morlen"] } },
                { "Pinnow",                     new Recipient { uniqueid = 15769, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarPinnow", displayname = Labels.airecipients["Pinnow"] } },
                { "Pottschutthohe",             new Recipient { uniqueid = 15770, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarPottschutthohe", displayname = Labels.airecipients["Pottschutthohe"] } },
                { "Rinteln",                    new Recipient { uniqueid = 15771, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarRinteln", displayname = Labels.airecipients["Rinteln"] } },
                { "Uelzen",                     new Recipient { uniqueid = 15772, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarUelzen", displayname = Labels.airecipients["Uelzen"] } },
                { "Uetersen",                   new Recipient { uniqueid = 15773, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarUetersen", displayname = Labels.airecipients["Uetersen"] } },
                { "Ummern",                     new Recipient { uniqueid = 15774, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarUmmern", displayname = Labels.airecipients["Ummern"] } },
                { "Verden-Scharnhorst",         new Recipient { uniqueid = 15775, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarVerdenScharnhorst", displayname = Labels.airecipients["Verden-Scharnhorst"] } },
                { "Waren_Vielist",              new Recipient { uniqueid = 15776, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarWarenVielist", displayname = Labels.airecipients["Waren_Vielist"] } },
                { "Weser Wumme",                new Recipient { uniqueid = 15777, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarWeserWumme", displayname = Labels.airecipients["Weser Wumme"] } },
                { "Wismar",                     new Recipient { uniqueid = 15778, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarWismar", displayname = Labels.airecipients["Wismar"] } },
                { "Worms",                      new Recipient { uniqueid = 15779, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarWorms", displayname = Labels.airecipients["Worms"] } },
                { "Wadajom",                    new Recipient { uniqueid = 15780, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarWadajom", displayname = Labels.airecipients["Wadajom"] } },
                { "Lexija",                     new Recipient { uniqueid = 15781, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarLexija", displayname = Labels.airecipients["Lexija"] } },
                { "Barth",                      new Recipient { uniqueid = 15782, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarBarth", displayname = Labels.airecipients["Barth"] } },
                { "Proima",                     new Recipient { uniqueid = 15783, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarProima", displayname = Labels.airecipients["Proima"] } },
                { "Bitburg",                    new Recipient { uniqueid = 15784, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarBitburg", displayname = Labels.airecipients["Bitburg"] } },
                { "Braunschweig",               new Recipient { uniqueid = 15785, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarBraunschweig", displayname = Labels.airecipients["Braunschweig"] } },
                { "Bremen",                     new Recipient { uniqueid = 15786, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarBremen", displayname = Labels.airecipients["Bremen"] } },
                { "Unrecht",                    new Recipient { uniqueid = 15787, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarUnrecht", displayname = Labels.airecipients["Unrecht"] } },
                { "Buchel",                     new Recipient { uniqueid = 15788, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarBuchel", displayname = Labels.airecipients["Buchel"] } },
                { "Buckeburg",                  new Recipient { uniqueid = 15789, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarBuckeburg", displayname = Labels.airecipients["Buckeburg"] } },
                { "Celle",                      new Recipient { uniqueid = 15790, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarCelle", displayname = Labels.airecipients["Celle"] } },
                { "Sadovij",                    new Recipient { uniqueid = 15791, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarSadovij", displayname = Labels.airecipients["Sadovij"] } },
                { "Melodrama",                  new Recipient { uniqueid = 15792, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarMelodrama", displayname = Labels.airecipients["Melodrama"] } },
                { "Wurzen",                     new Recipient { uniqueid = 15793, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarWurzen", displayname = Labels.airecipients["Wurzen"] } },
                { "Fassberg",                   new Recipient { uniqueid = 15794, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarFassberg", displayname = Labels.airecipients["Fassberg"] } },
                { "Narsan",                     new Recipient { uniqueid = 15795, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarNarsan", displayname = Labels.airecipients["Narsan"] } },
                { "Frankfurt",                  new Recipient { uniqueid = 15796, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarFrankfurt", displayname = Labels.airecipients["Frankfurt"] } },
                { "Fritzlar",                   new Recipient { uniqueid = 15797, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarFritzlar", displayname = Labels.airecipients["Fritzlar"] } },
                { "Fulda",                      new Recipient { uniqueid = 15798, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarFulda", displayname = Labels.airecipients["Fulda"] } },
                { "Gesetz",                     new Recipient { uniqueid = 15799, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarGesetz", displayname = Labels.airecipients["Gesetz"] } },
                { "Gatow",                      new Recipient { uniqueid = 15800, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarGatow", displayname = Labels.airecipients["Gatow"] } },
                { "Giebelstadt",                new Recipient { uniqueid = 15801, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarGiebelstadt", displayname = Labels.airecipients["Giebelstadt"] } },
                { "Greif",                      new Recipient { uniqueid = 15802, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarGreif", displayname = Labels.airecipients["Greif"] } },
                { "Gutersloh",                  new Recipient { uniqueid = 15803, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarGutersloh", displayname = Labels.airecipients["Gutersloh"] } },
                { "Hahn",                       new Recipient { uniqueid = 15804, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarHahn", displayname = Labels.airecipients["Hahn"] } },
                { "Haina",                      new Recipient { uniqueid = 15805, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarHaina", displayname = Labels.airecipients["Haina"] } },
                { "Hamburg",                    new Recipient { uniqueid = 15806, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarHamburg", displayname = Labels.airecipients["Hamburg"] } },
                { "Hamburg Finkenwerder",       new Recipient { uniqueid = 15807, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarHamburgFinkenwerder", displayname = Labels.airecipients["Hamburg Finkenwerder"] } },
                { "Hannover",                   new Recipient { uniqueid = 15808, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarHannover", displayname = Labels.airecipients["Hannover"] } },
                { "Hildesheim",                 new Recipient { uniqueid = 15809, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarHildesHeim", displayname = Labels.airecipients["HildesHeim"] } },
                { "Halifax",                    new Recipient { uniqueid = 15810, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarHalifax", displayname = Labels.airecipients["Halifax"] } },
                { "Semelni",                    new Recipient { uniqueid = 15811, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarSemelni", displayname = Labels.airecipients["Semelni"] } },
                { "Lektorstart",                new Recipient { uniqueid = 15812, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarLektorstart", displayname = Labels.airecipients["Lektorstart"] } },
                { "Gusar",                      new Recipient { uniqueid = 15813, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarGusar", displayname = Labels.airecipients["Gusar"] } },
                { "Latex",                      new Recipient { uniqueid = 15814, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarLatex", displayname = Labels.airecipients["Latex"] } },
                { "Leipzig Mockau",             new Recipient { uniqueid = 15815, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarLeipzigMockau", displayname = Labels.airecipients["Leipzig Mockau"] } },
                { "Lubeck",                     new Recipient { uniqueid = 15816, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarLubeckNormal", displayname = Labels.airecipients["Lubeck"] } },
                { "Osenniy",                    new Recipient { uniqueid = 15817, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarOsenniy", displayname = Labels.airecipients["Osenniy"] } },
                { "Sembach",                    new Recipient { uniqueid = 15818, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarSembach", displayname = Labels.airecipients["Sembach"] } },
                { "Mendig",                     new Recipient { uniqueid = 15819, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarMendig", displayname = Labels.airecipients["Mendig"] } },
                { "Muskat",                     new Recipient { uniqueid = 15820, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarMuskat", displayname = Labels.airecipients["Muskat"] } },
                { "Defekt",                     new Recipient { uniqueid = 15821, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarDefekt", displayname = Labels.airecipients["Defekt"] } },
                { "Gidrostat",                  new Recipient { uniqueid = 15822, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarGidrostat", displayname = Labels.airecipients["Gidrostat"] } },
                { "Pestrij",                    new Recipient { uniqueid = 15823, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarPestrij", displayname = Labels.airecipients["Pestrij"] } },
                { "Klejonka",                   new Recipient { uniqueid = 15824, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarKlejonka", displayname = Labels.airecipients["Klejonka"] } },
                { "Narcoze",                    new Recipient { uniqueid = 15825, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarNarcoze", displayname = Labels.airecipients["Narcoze"] } },
                { "Pferdsfeld",                 new Recipient { uniqueid = 15826, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarPferdsfeld", displayname = Labels.airecipients["Pferdsfeld"] } },
                { "Ramstein",                   new Recipient { uniqueid = 15827, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarRamstein", displayname = Labels.airecipients["Ramstein"] } },
                { "Schonefeld",                 new Recipient { uniqueid = 15828, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarSchonefeld", displayname = Labels.airecipients["Schonefeld"] } },
                { "Schweinfurt",                new Recipient { uniqueid = 15829, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarSchweinfurt", displayname = Labels.airecipients["Schweinfurt"] } },                
                { "Spangdahlem",                new Recipient { uniqueid = 15830, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarSpangdahlem", displayname = Labels.airecipients["Spangdahlem"] } },
                { "Souvenir",                   new Recipient { uniqueid = 15831, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarSouvenir", displayname = Labels.airecipients["Souvenir"] } },
                { "Kukan",                      new Recipient { uniqueid = 15832, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarKukan", displayname = Labels.airecipients["Kukan"] } },
                { "Tegel",                      new Recipient { uniqueid = 15833, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarTegel", displayname = Labels.airecipients["Tegel"] } },
                { "Tempelhof",                  new Recipient { uniqueid = 15834, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarTempelhof", displayname = Labels.airecipients["Tempelhof"] } },
                { "Leopard",                    new Recipient { uniqueid = 15835, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarLeopard", displayname = Labels.airecipients["Leopard"] } },
                { "Baath",                      new Recipient { uniqueid = 15836, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarBaath", displayname = Labels.airecipients["Baath"] } },
                { "Werneuchen",                 new Recipient { uniqueid = 15837, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarWerneuchen", displayname = Labels.airecipients["Werneuchen"] } },
                { "Wiesbaden",                  new Recipient { uniqueid = 15838, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarWiesbaden", displayname = Labels.airecipients["Wiesbaden"] } },
                { "Gazovij",                    new Recipient { uniqueid = 15839, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarGazovij", displayname = Labels.airecipients["Gazovij"] } },
                { "Wunstorf",                   new Recipient { uniqueid = 15840, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarWunstorf", displayname = Labels.airecipients["Wunstorf"] } },
                { "Zerbst",                     new Recipient { uniqueid = 15841, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarZerbst", displayname = Labels.airecipients["Zerbst"] } },
                { "Zweibrucken",                new Recipient { uniqueid = 15842, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarZweibrucken", displayname = Labels.airecipients["Zweibrucken"] } },
                { "Merseburg",                  new Recipient { uniqueid = 15843, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarMerseburg", displayname = Labels.airecipients["Merseburg"] } },
                { "Leipzig Halle",              new Recipient { uniqueid = 15844, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarLeipzigHalle", displayname = Labels.airecipients["Leipzig Halle"] } },
                { "Garz",                       new Recipient { uniqueid = 15845, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarGarz", displayname = Labels.airecipients["Garz"] } },

                { "wAIUnitATCColdWarMaximum",   new Recipient { uniqueid = 15899, category = RecipientCategories.aiatc, name = "wAIUnitATCColdWarMaximum",           } },
                
                // Farps

                { "wAIUnitATCFarpsNull",        new Recipient { uniqueid = 15600, category = RecipientCategories.aifarp, name = "wAIUnitATCFarpsNull",         } },
                { "platform",                   new Recipient { uniqueid = 15601, category = RecipientCategories.aifarp, name = "wAIUnitATCFarpsFarp",         displayname = Labels.airecipients["platform"] } },
               // { "nearestfarp",                new Recipient { uniqueid = 15602, category = tgtcat.aifarp, name = "wAIUnitATCFarpsNearestFarp",  displayname = Labels.airecipients["nearestfarp"] } },
                { "wAIUnitATCFarpsBlueNull",    new Recipient { uniqueid = 15610, category = RecipientCategories.aifarp, name = "wAIUnitATCFarpsBlueNull",     } },
                { "berlin",                     new Recipient { uniqueid = 15611, category = RecipientCategories.aifarp, name = "wAIUnitATCFarpsBlueBerlin",   displayname = Labels.airecipients["berlin"] }   },
                { "dallas",                     new Recipient { uniqueid = 15612, category = RecipientCategories.aifarp, name = "wAIUnitATCFarpsBlueDallas",   displayname = Labels.airecipients["dallas"] }   },
                { "dublin",                     new Recipient { uniqueid = 15613, category = RecipientCategories.aifarp, name = "wAIUnitATCFarpsBlueDublin",   displayname = Labels.airecipients["dublin"] }   },
                { "london",                     new Recipient { uniqueid = 15614, category = RecipientCategories.aifarp, name = "wAIUnitATCFarpsBlueLondon",   displayname = Labels.airecipients["london"] }   },
                { "madrid",                     new Recipient { uniqueid = 15615, category = RecipientCategories.aifarp, name = "wAIUnitATCFarpsBlueMadrid",   displayname = Labels.airecipients["madrid"] }   },
                { "moscow",                     new Recipient { uniqueid = 15616, category = RecipientCategories.aifarp, name = "wAIUnitATCFarpsBlueMoscow",   displayname = Labels.airecipients["moscow"] }   },
                { "paris",                      new Recipient { uniqueid = 15617, category = RecipientCategories.aifarp, name = "wAIUnitATCFarpsBlueParis",    displayname = Labels.airecipients["paris"] }    },
                { "perth",                      new Recipient { uniqueid = 15618, category = RecipientCategories.aifarp, name = "wAIUnitATCFarpsBluePerth",    displayname = Labels.airecipients["perth"] }    },
                { "rome",                       new Recipient { uniqueid = 15619, category = RecipientCategories.aifarp, name = "wAIUnitATCFarpsBlueRome",     displayname = Labels.airecipients["rome"] }     },
                { "warsaw",                     new Recipient { uniqueid = 15620, category = RecipientCategories.aifarp, name = "wAIUnitATCFarpsBlueWarsaw",   displayname = Labels.airecipients["warsaw"] }   },

                { "wAIUnitATCFarpsBlueMaximum", new Recipient { uniqueid = 15650, category = RecipientCategories.aifarp, name = "wAIUnitATCFarpsBlueMaximum",  } },
                { "wAIUnitATCFarpsRedNull",     new Recipient { uniqueid = 15651, category = RecipientCategories.aifarp, name = "wAIUnitATCFarpsRedNull",      } },
                { "kaemka",                     new Recipient { uniqueid = 15652, category = RecipientCategories.aifarp, name = "wAIUnitATCFarpsRedKaemka",    displayname = Labels.airecipients["kaemka"] }   },
                { "kalitka",                    new Recipient { uniqueid = 15653, category = RecipientCategories.aifarp, name = "wAIUnitATCFarpsRedKalitka",   displayname = Labels.airecipients["kalitka"] }  },
                { "kapel",                      new Recipient { uniqueid = 15654, category = RecipientCategories.aifarp, name = "wAIUnitATCFarpsRedKapel",     displayname = Labels.airecipients["kapel"] }   },
                { "otkrytka",                   new Recipient { uniqueid = 15655, category = RecipientCategories.aifarp, name = "wAIUnitATCFarpsRedOtkrytka",  displayname = Labels.airecipients["otkrytka"] }   },
                { "podkova",                    new Recipient { uniqueid = 15656, category = RecipientCategories.aifarp, name = "wAIUnitATCFarpsRedPodkova",   displayname = Labels.airecipients["podkova"] }   },
                { "shpora",                     new Recipient { uniqueid = 15657, category = RecipientCategories.aifarp, name = "wAIUnitATCFarpsRedShpora",    displayname = Labels.airecipients["shpora"] }   },
                { "skala",                      new Recipient { uniqueid = 15658, category = RecipientCategories.aifarp, name = "wAIUnitATCFarpsRedSkala",     displayname = Labels.airecipients["skala"] }    },
                { "torba",                      new Recipient { uniqueid = 15659, category = RecipientCategories.aifarp, name = "wAIUnitATCFarpsRedTorba",     displayname = Labels.airecipients["torba"] }    },
                { "vetka",                      new Recipient { uniqueid = 15660, category = RecipientCategories.aifarp, name = "wAIUnitATCFarpsRedVetka",     displayname = Labels.airecipients["vetka"] }     },
                { "yunga",                      new Recipient { uniqueid = 15661, category = RecipientCategories.aifarp, name = "wAIUnitATCFarpsRedYunga",     displayname = Labels.airecipients["yunga"] }   },
                { "wAIUnitATCFarpsRedMaximum",  new Recipient { uniqueid = 15698, category = RecipientCategories.aifarp, name = "wAIUnitATCFarpsRedMaximum",   } },
                { "wAIUnitATCFarpsMaximum",     new Recipient { uniqueid = 15699, category = RecipientCategories.aifarp, name = "wAIUnitATCFarpsMaximum",      } },

                // Aircraft Carriers
                
                { "wAIUnitATCCarriersNull",     new Recipient { uniqueid = 15700, category = RecipientCategories.aiship, name = "wAIUnitATCCarriersNull",      } },
                { "carrier",                    new Recipient { uniqueid = 15701, category = RecipientCategories.aiship, name = "wAIUnitATCCarrier",           displayname = Labels.airecipients["carrier"] } },
                { "nearestcarrier",             new Recipient { uniqueid = 15702, category = RecipientCategories.aiship, name = "wAIUnitATCCarrierNearest",    displayname = Labels.airecipients["nearestcarrier"] } },
                { "CV 1143.5 Admiral Kuznetsov",new Recipient { uniqueid = 15703, category = RecipientCategories.aiship, name = "wAIUnitATCAdmiralKuznetsov",  displayname = Labels.airecipients["CV 1143.5 Admiral Kuznetsov"] } },
                { "CVN-70 Carl Vinson",         new Recipient { uniqueid = 15704, category = RecipientCategories.aiship, name = "wAIUnitATCCarlVinson",        displayname = Labels.airecipients["CVN-70 Carl Vinson"] } },
                { "CVN-74 John C. Stennis",     new Recipient { uniqueid = 15705, category = RecipientCategories.aiship, name = "wAIUnitATCJohnCStennis",      displayname = Labels.airecipients["CVN-74 John C. Stennis"] } },
                { "LHA-1 Tarawa",               new Recipient { uniqueid = 15706, category = RecipientCategories.aiunit, name = "wAIUnitATCTarawa",            displayname = Labels.airecipients["LHA-1 Tarawa"] } }, // was 15505 ship
                { "FFG-7CL Oliver Hazzard Perry",new Recipient{ uniqueid = 15707, category = RecipientCategories.aiunit, name = "wAIUnitATCHazzardPerry",      displayname = Labels.airecipients["FFG-7CL Oliver Hazzard Perry"] } },// was 15506 ship
                { "CG-60 Normandy",             new Recipient { uniqueid = 15708, category = RecipientCategories.aiunit, name = "wAIUnitATCCGNormandy",        displayname = Labels.airecipients["CG-60 Normandy"] } },// was 15508 ship
                
                // new carriers
                
                { "CVN-71 Theodore Roosevelt",  new Recipient { uniqueid = 15710, category = RecipientCategories.aiship, name = "wAIUnitATCRoosevelt",         displayname = Labels.airecipients["CVN-71 Theodore Roosevelt"]  } },
                { "CVN-72 Abraham Lincoln",     new Recipient { uniqueid = 15711, category = RecipientCategories.aiship, name = "wAIUnitATCLincoln",           displayname = Labels.airecipients["CVN-72 Abraham Lincoln"]  } },
                { "CVN-73 George Washington",   new Recipient { uniqueid = 15712, category = RecipientCategories.aiship, name = "wAIUnitATCWashington",        displayname = Labels.airecipients["CVN-73 George Washington"]  } },
                { "CVN-75 Harry S. Truman",     new Recipient { uniqueid = 15713, category = RecipientCategories.aiship, name = "wAIUnitATCTruman",            displayname = Labels.airecipients["CVN-75 Harry S. Truman"]  } },

                // Non Supercarrier ATC comms enabled
                { "CV-59 Forrestal",            new Recipient { uniqueid = 15714, category = RecipientCategories.aiunit, name = "wAIUnitATCForrestal",         displayname = Labels.airecipients["CV-59 Forrestal"] } },

                // new roles
                { "wAIUnitCarrierRoleNull",     new Recipient { uniqueid = 15720, category = RecipientCategories.aiship, name = "wAIUnitCarrierRoleNull", }      },

                { "CarrierDeparture",           new Recipient { uniqueid = 15721, category = RecipientCategories.aiship, name = "wAIUnitCarrierDeparture",     displayname = Labels.airecipients["CarrierDeparture"]  } },
                { "CarrierMarshal",             new Recipient { uniqueid = 15722, category = RecipientCategories.aiship, name = "wAIUnitCarrierMarshal",       displayname = Labels.airecipients["CarrierMarshal"]  } },
                { "CarrierApproachTower",       new Recipient { uniqueid = 15723, category = RecipientCategories.aiship, name = "wAIUnitCarrierApproachTower", displayname = Labels.airecipients["CarrierApproachTower"]  } },
                { "CarrierLSO",                 new Recipient { uniqueid = 15724, category = RecipientCategories.aiship, name = "wAIUnitCarrierLSO",           displayname = Labels.airecipients["CarrierLSO"]  } },
                { "wAIUnitCarrierRoleMaximum",  new Recipient { uniqueid = 15730, category = RecipientCategories.aiship, name = "wAIUnitCarrierRoleMaximum", }      },


                { "wAIUnitATCCarriersMaximum",  new Recipient { uniqueid = 15740, category = RecipientCategories.aiship, name = "wAIUnitATCCarriersMaximum",   } },

                // Auto Imported 

                { "wAIUnitATCImportedNull",     new Recipient { uniqueid = 15900, category = RecipientCategories.aiatc, name = "wAIUnitATCImportedNull",      } },
                { "wAIUnitATCImportedMaximum",  new Recipient { uniqueid = 15998, category = RecipientCategories.aiatc, name = "wAIUnitATCImportedMaximum",   } },

                { "wAIUnitATCMaximum",          new Recipient { uniqueid = 15999, category = RecipientCategories.aiatc, name = "wAIUnitATCMaximum",           } },

                // Tankers

                { "wAIUnitTankerNull",      new Recipient { uniqueid = 16000, category = RecipientCategories.aitanker, name = "wAIUnitTankerNull",       } },
                { "tanker",                 new Recipient { uniqueid = 16001, category = RecipientCategories.aitanker, name = "wAIUnitTankerTanker",     displayname = Labels.airecipients["tanker"] } },
                { "texaco",                 new Recipient { uniqueid = 16002, category = RecipientCategories.aitanker, name = "wAIUnitTankerTexaco",     displayname = Labels.airecipients["texaco"] } },
                { "shell",                  new Recipient { uniqueid = 16003, category = RecipientCategories.aitanker, name = "wAIUnitTankerShell",      displayname = Labels.airecipients["shell"] } },
                { "arco",                   new Recipient { uniqueid = 16004, category = RecipientCategories.aitanker, name = "wAIUnitTankerArco",       displayname = Labels.airecipients["arco"] } },
                { "Bloodhound",             new Recipient { uniqueid = 16005, category = RecipientCategories.aitanker, name = "wAIUnitTankerBloodhound",    displayname = Labels.airecipients["Bloodhound"] } },
                { "Mauler",                 new Recipient { uniqueid = 16006, category = RecipientCategories.aitanker, name = "wAIUnitTankerMauler",    displayname = Labels.airecipients["Mauler"] } },
                { "Navy One",               new Recipient { uniqueid = 16007, category = RecipientCategories.aitanker, name = "wAIUnitTankerNavyone",    displayname = Labels.airecipients["Navy One"] } },
                { "nearesttanker",          new Recipient { uniqueid = 16008, category = RecipientCategories.aitanker, name = "wAIUnitTankerNearest",    displayname = Labels.airecipients["nearesttanker"] } },

                { "wAIUnitTankerMaximum",   new Recipient { uniqueid = 16999, category = RecipientCategories.aitanker, name = "wAIUnitTankerMaximum",    } },

                // AWACS
                
                { "wAIUnitAWACSNull",       new Recipient { uniqueid = 17000, category = RecipientCategories.aiawacs, name = "wAIUnitAWACSNull",        } },

                { "awacs",                  new Recipient { uniqueid = 17001, category = RecipientCategories.aiawacs, name = "wAIUnitAWACSAWACS",       displayname = Labels.airecipients["awacs"] } },
                { "darkstar",               new Recipient { uniqueid = 17002, category = RecipientCategories.aiawacs, name = "wAIUnitAWACSDarkstar",    displayname = Labels.airecipients["darkstar"] } },
                { "focus",                  new Recipient { uniqueid = 17003, category = RecipientCategories.aiawacs, name = "wAIUnitAWACSFocus",       displayname = Labels.airecipients["focus"] } },
                { "magic",                  new Recipient { uniqueid = 17004, category = RecipientCategories.aiawacs, name = "wAIUnitAWACSMagic",       displayname = Labels.airecipients["magic"] } },
                { "overlord",               new Recipient { uniqueid = 17005, category = RecipientCategories.aiawacs, name = "wAIUnitAWACSOverlord",    displayname = Labels.airecipients["overlord"] } },
                { "wizard",                 new Recipient { uniqueid = 17006, category = RecipientCategories.aiawacs, name = "wAIUnitAWACSWizard",      displayname = Labels.airecipients["wizard"] } },
                { "nearestawacs",           new Recipient { uniqueid = 17007, category = RecipientCategories.aiawacs, name = "wAIUnitAWACSNearest",     displayname = Labels.airecipients["nearestawacs"] } },

                { "wAIUnitAWACSMaximum",    new Recipient { uniqueid = 17999, category = RecipientCategories.aiawacs, name = "wAIUnitAWACSMaximum",     } },

                // Ai Crew
                
                { "wAIUnitCrewNull",        new Recipient { uniqueid = 18000, category = RecipientCategories.aicrew, name = "wAIUnitCrewNull",          } },
                { "crew",                   new Recipient { uniqueid = 18001, category = RecipientCategories.aicrew, name = "wAIUnitCrewCrew",          displayname = Labels.airecipients["crew"] } },
                { "aicrew",                 new Recipient { uniqueid = 18002, category = RecipientCategories.aicrew, name = "wAIUnitCrewAICrew",        displayname = Labels.airecipients["aicrew"] } },
                { "wAIUnitCrewMaximum",     new Recipient { uniqueid = 18099, category = RecipientCategories.aicrew, name = "wAIUnitCrewMaximum",       } },

                // parked

                { "wAIUnitMaximum",         new Recipient { uniqueid = 18999, category = RecipientCategories.aiunit, name = "wAIUnitMaximum",           } },

                { "wAIUnitAOCSNull",        new Recipient { uniqueid = 19000, category = RecipientCategories.aocs,  name = "wAIUnitAOCSNull",      } },
                { "aocs",                   new Recipient { uniqueid = 19001, category = RecipientCategories.aocs,  name = "wAIUnitAOCS",          displayname = Labels.airecipients["aocs"] } },
                { "wAIUnitAOCSMaximum",     new Recipient { uniqueid = 19049, category = RecipientCategories.aocs,  name = "wAIUnitAOCSMaximum",   } },

                { "wAIUnitAuxNull",         new Recipient { uniqueid = 19050, category = RecipientCategories.auxmenu, name = "wAIUnitAuxNull",          } },
                { "aux",                    new Recipient { uniqueid = 19051, category = RecipientCategories.auxmenu, name = "wAIUnitAuxMysteryGuest",  displayname = Labels.airecipients["aux"] } },                
                { "wAIUnitAuxMaximum",      new Recipient { uniqueid = 19099, category = RecipientCategories.auxmenu, name = "wAIUnitAuxMaximum",       } },

                { "wAIUnitCargoNull",       new Recipient { uniqueid = 19100, category = RecipientCategories.cargo, name = "wAIUnitCargoNull",          } },
                { "cargo",                  new Recipient { uniqueid = 19101, category = RecipientCategories.cargo, name = "wAIUnitCargo",              displayname = Labels.airecipients["cargo"] } },
                { "wAIUnitCargoMaximum",    new Recipient { uniqueid = 19199, category = RecipientCategories.cargo, name = "wAIUnitCargoMaximum",       } },

                { "wAIUnitDescentNull",       new Recipient { uniqueid = 19200, category = RecipientCategories.cargo, name = "wAIUnitDescentNull",      } },
                { "descent",                  new Recipient { uniqueid = 19201, category = RecipientCategories.cargo, name = "wAIUnitDescent",          displayname = Labels.airecipients["descent"]  } },
                { "wAIUnitDescentMaximum",    new Recipient { uniqueid = 19299, category = RecipientCategories.cargo, name = "wAIUnitDescentMaximum",   } },

                // RIO pack
                { "wAIUnitFlightCrewMembersNull",        new Recipient { uniqueid = 19300, category = RecipientCategories.RIO, name = "wAIUnitFlightCrewMembersNull",         } },
                
                { "wAIUnitFlightCrewMembersMaximum",     new Recipient { uniqueid = 19399, category = RecipientCategories.RIO, name = "wAIUnitFlightCrewMembersMaximum",      } },


                // kneeboard extension
                { "wAIUnitKneeboardNull",       new Recipient { uniqueid = 19400, category = RecipientCategories.kneeboard, name = "wAIUnitKneeboardNull",      } },
                { "kneeboard",                  new Recipient { uniqueid = 19401, category = RecipientCategories.kneeboard, name = "wAIUnitKneeboard",          displayname = Labels.airecipients["kneeboard"]  } },
                { "wAIUnitKneeboardMaximum",    new Recipient { uniqueid = 19499, category = RecipientCategories.kneeboard, name = "wAIUnitKneeboardMaximum",   } },

                // WSO pack
                { "wAIUnitFlightCrewMembersWSONull", new Recipient { uniqueid = 19500, category = RecipientCategories.WSO, name = "wAIUnitFlightCrewMembersWSONull", } },
                
                { "wAIUnitFlightCrewMembersWSOMaximum", new Recipient { uniqueid = 19599, category = RecipientCategories.WSO, name = "wAIUnitFlightCrewMembersWSOMaximum", } },
            };
        }
    }
}
