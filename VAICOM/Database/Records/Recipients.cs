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
                { "pinpoint",               new Recipient { uniqueid = 14013, category = RecipientCategories.aijtac, name = "wAIUnitJTACPinpoint",     displayname = Labels.airecipients["pinpoint"],    blockedforFree = true } },
                { "ferret",                 new Recipient { uniqueid = 14014, category = RecipientCategories.aijtac, name = "wAIUnitJTACFerret",       displayname = Labels.airecipients["ferret"],      blockedforFree = true } },
                { "shaba",                  new Recipient { uniqueid = 14015, category = RecipientCategories.aijtac, name = "wAIUnitJTACShaba",        displayname = Labels.airecipients["shaba"],       blockedforFree = true } },
                { "hammer",                 new Recipient { uniqueid = 14016, category = RecipientCategories.aijtac, name = "wAIUnitJTACHammer",       displayname = Labels.airecipients["hammer"],      blockedforFree = true } },
                { "jaguar",                 new Recipient { uniqueid = 14017, category = RecipientCategories.aijtac, name = "wAIUnitJTACJaguar",       displayname = Labels.airecipients["jaguar"],      blockedforFree = true } },
                { "deathstar",              new Recipient { uniqueid = 14018, category = RecipientCategories.aijtac, name = "wAIUnitJTACDeathstar",    displayname = Labels.airecipients["deathstar"],   blockedforFree = true } },
                { "anvil",                  new Recipient { uniqueid = 14019, category = RecipientCategories.aijtac, name = "wAIUnitJTACAnvil",        displayname = Labels.airecipients["anvil"],       blockedforFree = true } },
                { "mantis",                 new Recipient { uniqueid = 14020, category = RecipientCategories.aijtac, name = "wAIUnitJTACMantis",       displayname = Labels.airecipients["mantis"],      blockedforFree = true } },
                { "badger",                 new Recipient { uniqueid = 14021, category = RecipientCategories.aijtac, name = "wAIUnitJTACBadger",       displayname = Labels.airecipients["badger"],      blockedforFree = true } },

                // JTAC based on flight callsigns

                { "boar",                   new Recipient { uniqueid = 14022, category = RecipientCategories.aijtac, name = "wAIUnitJTACBoar",         displayname = Labels.airecipients["boar"],        blockedforFree = true } },
                { "chevy",                  new Recipient { uniqueid = 14023, category = RecipientCategories.aijtac, name = "wAIUnitJTACChevy",        displayname = Labels.airecipients["chevy"],       blockedforFree = true } },
                { "colt",                   new Recipient { uniqueid = 14024, category = RecipientCategories.aijtac, name = "wAIUnitJTACColt",         displayname = Labels.airecipients["colt"],        blockedforFree = true } },
                { "dodge",                  new Recipient { uniqueid = 14025, category = RecipientCategories.aijtac, name = "wAIUnitJTACDodge",        displayname = Labels.airecipients["dodge"],       blockedforFree = true } },
                { "enfield",                new Recipient { uniqueid = 14026, category = RecipientCategories.aijtac, name = "wAIUnitJTACEnfield",      displayname = Labels.airecipients["enfield"],     blockedforFree = true } },
                { "ford",                   new Recipient { uniqueid = 14027, category = RecipientCategories.aijtac, name = "wAIUnitJTACFord",         displayname = Labels.airecipients["ford"],        blockedforFree = true } },
                { "hawg",                   new Recipient { uniqueid = 14028, category = RecipientCategories.aijtac, name = "wAIUnitJTACHawg",         displayname = Labels.airecipients["hawg"],        blockedforFree = true } },
                { "pig",                    new Recipient { uniqueid = 14029, category = RecipientCategories.aijtac, name = "wAIUnitJTACPig",          displayname = Labels.airecipients["pig"],         blockedforFree = true } },
                { "pontiac",                new Recipient { uniqueid = 14030, category = RecipientCategories.aijtac, name = "wAIUnitJTACPontiac",      displayname = Labels.airecipients["pontiac"],     blockedforFree = true } },
                { "springfield",            new Recipient { uniqueid = 14031, category = RecipientCategories.aijtac, name = "wAIUnitJTACSpringfield",  displayname = Labels.airecipients["springfield"], blockedforFree = true } },
                { "tusk",                   new Recipient { uniqueid = 14032, category = RecipientCategories.aijtac, name = "wAIUnitJTACTusk",         displayname = Labels.airecipients["tusk"],        blockedforFree = true } },
                { "uzi",                    new Recipient { uniqueid = 14033, category = RecipientCategories.aijtac, name = "wAIUnitJTACUzi",          displayname = Labels.airecipients["uzi"],         blockedforFree = true } },

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

                { "wAIUnitATCMarianasNull",           new Recipient { uniqueid = 15100, category = RecipientCategories.aiatc, name = "wAIUnitATCMarianasNull",        } },

                { "Andersen AFB",                   new Recipient { uniqueid = 15101, category = RecipientCategories.aiatc, name = "wAIUnitATCMarianasAndersen",    displayname = Labels.airecipients["Andersen AFB"] } },
                { "Antonio B. Won Pat Intl",        new Recipient { uniqueid = 15102, category = RecipientCategories.aiatc, name = "wAIUnitATCMarianasAntonioB",    displayname = Labels.airecipients["Antonio B. Won Pat Intl"] } },
                { "Rota Intl",                      new Recipient { uniqueid = 15103, category = RecipientCategories.aiatc, name = "wAIUnitATCMarianasRotaIntl",    displayname = Labels.airecipients["Rota Intl"] } },
                { "Tinian Intl",                    new Recipient { uniqueid = 15104, category = RecipientCategories.aiatc, name = "wAIUnitATCMarianasTinianIntl",  displayname = Labels.airecipients["Tinian Intl"] } },
                { "Saipan Intl",                    new Recipient { uniqueid = 15105, category = RecipientCategories.aiatc, name = "wAIUnitATCMarianasSaipanIntl",  displayname = Labels.airecipients["Saipan Intl"] } },

                { "wAIUnitATCMarianasMaximum",      new Recipient { uniqueid = 15110, category = RecipientCategories.aiatc, name = "wAIUnitATCMarianasMaximum",     } },

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

                { "Beny-sur-Mer",                   new Recipient { uniqueid = 15201, category = RecipientCategories.aiatc, name = "wAIUnitATCNormandyBenysurMer",        displayname = Labels.airecipients["Beny-sur-Mer"], blockedforFree = true } },
                { "Sainte-Croix-sur-Mer",           new Recipient { uniqueid = 15202, category = RecipientCategories.aiatc, name = "wAIUnitATCNormandySainteCroixsurMer", displayname = Labels.airecipients["Sainte-Croix-sur-Mer"], blockedforFree = true  } },
                { "Lantheuil",                      new Recipient { uniqueid = 15203, category = RecipientCategories.aiatc, name = "wAIUnitATCNormandyLantheuil",         displayname = Labels.airecipients["Lantheuil"], blockedforFree = true  } },
                { "Bazenville",                     new Recipient { uniqueid = 15204, category = RecipientCategories.aiatc, name = "wAIUnitATCNormandyBazenville",        displayname = Labels.airecipients["Bazenville"], blockedforFree = true  } },
                { "Sommervieu",                     new Recipient { uniqueid = 15205, category = RecipientCategories.aiatc, name = "wAIUnitATCNormandySommervieu",        displayname = Labels.airecipients["Sommervieu"], blockedforFree = true  } },
                { "Longues-sur-Mer",                new Recipient { uniqueid = 15206, category = RecipientCategories.aiatc, name = "wAIUnitATCNormandyLonguessurMer",     displayname = Labels.airecipients["Longues-sur-Mer"] , blockedforFree = true } },
                { "Le Molay",                       new Recipient { uniqueid = 15207, category = RecipientCategories.aiatc, name = "wAIUnitATCNormandyLeMolay",           displayname = Labels.airecipients["Le Molay"], blockedforFree = true  } },
                { "Sainte-Laurent-sur-Mer",         new Recipient { uniqueid = 15208, category = RecipientCategories.aiatc, name ="wAIUnitATCNormandySainteLaurentsurMer",displayname = Labels.airecipients["Sainte-Laurent-sur-Mer"], blockedforFree = true  } },
                { "Saint Pierre du Mont",           new Recipient { uniqueid = 15209, category = RecipientCategories.aiatc, name = "wAIUnitATCNormandySaintPierreduMont", displayname = Labels.airecipients["Saint Pierre du Mont"], blockedforFree = true  } },
                { "Deux Jumeaux",                   new Recipient { uniqueid = 15210, category = RecipientCategories.aiatc, name = "wAIUnitATCNormandyDeuxJumeaux",       displayname = Labels.airecipients["Deux Jumeaux"], blockedforFree = true  } },
                { "Chippelle",                      new Recipient { uniqueid = 15211, category = RecipientCategories.aiatc, name = "wAIUnitATCNormandyChippelle",         displayname = Labels.airecipients["Chippelle"], blockedforFree = true  } },
                { "Cricqueville-en-Bessin",         new Recipient { uniqueid = 15212, category = RecipientCategories.aiatc, name="wAIUnitATCNormandyCricquevilleenBessin",displayname = Labels.airecipients["Cricqueville-en-Bessin"], blockedforFree = true  } },
                { "Cardonville",                    new Recipient { uniqueid = 15213, category = RecipientCategories.aiatc, name = "wAIUnitATCNormandyCardonville",       displayname = Labels.airecipients["Cardonville"], blockedforFree = true  } },
                { "Brucheville",                    new Recipient { uniqueid = 15214, category = RecipientCategories.aiatc, name = "wAIUnitATCNormandyBrucheville",       displayname = Labels.airecipients["Brucheville"], blockedforFree = true  } },
                { "Meautis",                        new Recipient { uniqueid = 15215, category = RecipientCategories.aiatc, name = "wAIUnitATCNormandyMeautis",           displayname = Labels.airecipients["Meautis"], blockedforFree = true  } },
                { "Azeville",                       new Recipient { uniqueid = 15216, category = RecipientCategories.aiatc, name = "wAIUnitATCNormandyAzeville",          displayname = Labels.airecipients["Azeville"], blockedforFree = true  } },
                { "Cretteville",                    new Recipient { uniqueid = 15217, category = RecipientCategories.aiatc, name = "wAIUnitATCNormandyCretteville",       displayname = Labels.airecipients["Cretteville"], blockedforFree = true  } },
                { "Picauville",                     new Recipient { uniqueid = 15218, category = RecipientCategories.aiatc, name = "wAIUnitATCNormandyPicauville",        displayname = Labels.airecipients["Picauville"], blockedforFree = true  } },
                { "Biniville",                      new Recipient { uniqueid = 15219, category = RecipientCategories.aiatc, name = "wAIUnitATCNormandyBiniville",         displayname = Labels.airecipients["Biniville"], blockedforFree = true  } },
                { "Lessay",                         new Recipient { uniqueid = 15220, category = RecipientCategories.aiatc, name = "wAIUnitATCNormandyLessay",            displayname = Labels.airecipients["Lessay"], blockedforFree = true  } },
                { "Maupertus",                      new Recipient { uniqueid = 15221, category = RecipientCategories.aiatc, name = "wAIUnitATCNormandyMaupertus",         displayname = Labels.airecipients["Maupertus"], blockedforFree = true  } },
                { "Evreux",                         new Recipient { uniqueid = 15222, category = RecipientCategories.aiatc, name = "wAIUnitATCNormandyEvreux",            displayname = Labels.airecipients["Evreux"], blockedforFree = true  } },
                { "Forde",                          new Recipient { uniqueid = 15223, category = RecipientCategories.aiatc, name = "wAIUnitATCNormandyForde",             displayname = Labels.airecipients["Forde"], blockedforFree = true  } },
                { "Tangmere",                       new Recipient { uniqueid = 15224, category = RecipientCategories.aiatc, name = "wAIUnitATCNormandyTangmere",          displayname = Labels.airecipients["Tangmere"], blockedforFree = true  } },
                { "Funtington",                     new Recipient { uniqueid = 15225, category = RecipientCategories.aiatc, name = "wAIUnitATCNormandyFuntington",        displayname = Labels.airecipients["Funtington"], blockedforFree = true  } },
                { "Chailey",                        new Recipient { uniqueid = 15226, category = RecipientCategories.aiatc, name = "wAIUnitATCNormandyChailey",           displayname = Labels.airecipients["Chailey"], blockedforFree = true  } },
                { "Needs Oar Point",                new Recipient { uniqueid = 15227, category = RecipientCategories.aiatc, name = "wAIUnitATCNormandyNeedsOarPoint",     displayname = Labels.airecipients["Needs Oar Point"], blockedforFree = true  } },

                { "wAIUnitATCNormandyMaximum",      new Recipient { uniqueid = 15249, category = RecipientCategories.aiatc, name = "wAIUnitATCNormandyMaximum",           } },

                // Persian Gulf map

                { "wAIUnitATCPersianGulfNull",      new Recipient { uniqueid = 15250, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfNull",              } },

                { "Al Maktoum Intl",                new Recipient { uniqueid = 15251, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfAlMaktoumIntl", displayname = Labels.airecipients["Al Maktoum Intl"],           blockedforFree = true  } },
                { "Al Minhad AFB",                  new Recipient { uniqueid = 15252, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfAlMinhadAFB",   displayname = Labels.airecipients["Al Minhad AFB"],             blockedforFree = true  } },
                { "Dubai Intl",                     new Recipient { uniqueid = 15253, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfDubaiIntl",     displayname = Labels.airecipients["Dubai Intl"],                blockedforFree = true  } },
                { "Sharjah Intl",                   new Recipient { uniqueid = 15254, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfSharjahIntl",   displayname = Labels.airecipients["Sharjah Intl"],              blockedforFree = true  } },
                { "Abu Musa Island",                new Recipient { uniqueid = 15255, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfAbuMusaIsland", displayname = Labels.airecipients["Abu Musa Island"],           blockedforFree = true  } },
                { "Sirri Island",                   new Recipient { uniqueid = 15256, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfSirriIsland",   displayname = Labels.airecipients["Sirri Island"],              blockedforFree = true  } },
                { "Fujairah Intl",                  new Recipient { uniqueid = 15257, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfFujairahIntl",  displayname = Labels.airecipients["Fujairah Intl"],             blockedforFree = true  } },
                { "Bandar Lengeh",                  new Recipient { uniqueid = 15258, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfBandarLengeh",  displayname = Labels.airecipients["Bandar Lengeh"],             blockedforFree = true  } },
                { "Khasab",                         new Recipient { uniqueid = 15259, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfKhasab",        displayname = Labels.airecipients["Khasab"],                    blockedforFree = true  } },
                { "Qeshm Island",                   new Recipient { uniqueid = 15260, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfQeshmIsland",   displayname = Labels.airecipients["Qeshm Island"],              blockedforFree = true  } },
                { "Havadarya",                      new Recipient { uniqueid = 15261, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfHavadarya",     displayname = Labels.airecipients["Havadarya"],                 blockedforFree = true  } },
                { "Bandar Abbas Intl",              new Recipient { uniqueid = 15262, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfBandarAbbasIntl",displayname = Labels.airecipients["Bandar Abbas Intl"],        blockedforFree = true  } },
                { "Lar",                            new Recipient { uniqueid = 15263, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfLar",           displayname = Labels.airecipients["Lar"],                       blockedforFree = true  } },
                { "Kerman",                         new Recipient { uniqueid = 15264, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfKerman",        displayname = Labels.airecipients["Kerman"],                    blockedforFree = true  } },
                { "Shiraz Intl",                    new Recipient { uniqueid = 15265, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfShirazIntl",    displayname = Labels.airecipients["Shiraz Intl"],               blockedforFree = true  } },
                { "Al Dhafra AFB",                  new Recipient { uniqueid = 15266, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfAlDhafraAFB",   displayname = Labels.airecipients["Al Dhafra AFB"],             blockedforFree = true  } },
                { "Al-Bateen",                      new Recipient { uniqueid = 15267, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfAlBateen",      displayname = Labels.airecipients["Al-Bateen"],                 blockedforFree = true  } }, //OK
                { "Kish Intl",                      new Recipient { uniqueid = 15268, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfKishIsland",    displayname = Labels.airecipients["Kish Intl"],                 blockedforFree = true  } },
                { "Lavan Island",                   new Recipient { uniqueid = 15269, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfLavanIsland",   displayname = Labels.airecipients["Lavan Island"],              blockedforFree = true  } },
                { "Al Ain Intl",                    new Recipient { uniqueid = 15270, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfAlAinIntl",     displayname = Labels.airecipients["Al Ain Intl"],               blockedforFree = true  } },
                { "Bandar-e-Jask",                  new Recipient { uniqueid = 15271, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfBandareJask",   displayname = Labels.airecipients["Bandar-e-Jask"],             blockedforFree = true  } },
                { "Abu Dhabi Intl",                 new Recipient { uniqueid = 15272, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfAbuDhabi",      displayname = Labels.airecipients["Abu Dhabi Intl"],            blockedforFree = true  } },
                { "Sas Al Nakheel",                 new Recipient { uniqueid = 15273, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfSasAlNakheel",  displayname = Labels.airecipients["Sas Al Nakheel"],            blockedforFree = true  } },
                { "Jiroft",                         new Recipient { uniqueid = 15274, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfJiroft",        displayname = Labels.airecipients["Jiroft"],                    blockedforFree = true  } },
                { "Liwa AFB",                       new Recipient { uniqueid = 15275, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfLiwaAFB",       displayname = Labels.airecipients["Liwa AFB"],                  blockedforFree = true  } },
                { "Ras Al Khaimah Intl",            new Recipient { uniqueid = 15276, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfRasAlKhaimah",  displayname = Labels.airecipients["Ras Al Khaimah Intl"],       blockedforFree = true  } },
                { "Sir Abu Nuayr",                  new Recipient { uniqueid = 15277, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfSirAbuNuayr",   displayname = Labels.airecipients["Sir Abu Nuayr"],             blockedforFree = true  } },

                { "wAIUnitATCPersianGulfMaximum",   new Recipient { uniqueid = 15278, category = RecipientCategories.aiatc, name = "wAIUnitATCPersianGulfMaximum",           } },

                // Channel Map
                
                { "wAIUnitATCChannelNull",      new Recipient { uniqueid = 15280, category = RecipientCategories.aiatc, name = "wAIUnitATCChannelNull",              } },

                { "Dunkirk Mardyck",            new Recipient { uniqueid = 15281, category = RecipientCategories.aiatc, name = "wAIUnitATCChannelDunkirkMardyck",  displayname = Labels.airecipients["Dunkirk Mardyck"], blockedforFree = true  } },
                { "Hawkinge",                   new Recipient { uniqueid = 15282, category = RecipientCategories.aiatc, name = "wAIUnitATCChannelHawkinge",  displayname = Labels.airecipients["Hawkinge"], blockedforFree = true  } },
                { "Saint Omer Longuenesse",     new Recipient { uniqueid = 15283, category = RecipientCategories.aiatc, name = "wAIUnitATCChannelSaintOmerLonguenesse",  displayname = Labels.airecipients["Saint Omer Longuenesse"], blockedforFree = true  } },
                { "Merville Calonne",           new Recipient { uniqueid = 15284, category = RecipientCategories.aiatc, name = "wAIUnitATCChannelMervilleCalonne",  displayname = Labels.airecipients["Merville Calonne"], blockedforFree = true  } },
                { "High Halden",                new Recipient { uniqueid = 15285, category = RecipientCategories.aiatc, name = "wAIUnitATCChannelHighHalden",  displayname = Labels.airecipients["High Halden"], blockedforFree = true  } },
                { "Detling",                    new Recipient { uniqueid = 15286, category = RecipientCategories.aiatc, name = "wAIUnitATCChannelDetling",  displayname = Labels.airecipients["Detling"], blockedforFree = true  } },
                { "Abbeville Drucat",           new Recipient { uniqueid = 15287, category = RecipientCategories.aiatc, name = "wAIUnitATCChannelAbbevilleDrucat",  displayname = Labels.airecipients["Abbeville Drucat"], blockedforFree = true  } },
                { "Lympne",                     new Recipient { uniqueid = 15288, category = RecipientCategories.aiatc, name = "wAIUnitATCChannelLympne",  displayname = Labels.airecipients["Lympne"], blockedforFree = true  } },
                { "Manston",                    new Recipient { uniqueid = 15289, category = RecipientCategories.aiatc, name = "wAIUnitATCChannelManston",  displayname = Labels.airecipients["Manston"], blockedforFree = true  } },

                { "wAIUnitATCChannelMaximum",   new Recipient { uniqueid = 15290, category = RecipientCategories.aiatc, name = "wAIUnitATCChannelMaximum",           } },

                // Syria map
                
                { "wAIUnitATCSyriaNull",        new Recipient { uniqueid = 15291, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaNull",              } },

                { "Beirut-Rafic Hariri",        new Recipient { uniqueid = 15292, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaBeirutRaficHariri",  displayname = Labels.airecipients["Beirut-Rafic Hariri"], blockedforFree = true  } },
                { "Rayak",                      new Recipient { uniqueid = 15293, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaRayak",  displayname = Labels.airecipients["Rayak"], blockedforFree = true  } },
                { "Wujah Al Hajar",             new Recipient { uniqueid = 15294, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaWujahAlHajar",  displayname = Labels.airecipients["Wujah Al Hajar"], blockedforFree = true  } },
                { "Kiryat Shmona",              new Recipient { uniqueid = 15295, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaKiryatShmona",  displayname = Labels.airecipients["Kiryat Shmona"], blockedforFree = true  } },
                { "Mezzeh",                     new Recipient { uniqueid = 15296, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaMezzeh",  displayname = Labels.airecipients["Mezzeh"], blockedforFree = true  } },
                { "Qabr as Sitt",               new Recipient { uniqueid = 15297, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaQabrasSitt",  displayname = Labels.airecipients["Qabr as Sitt"], blockedforFree = true  } },
                { "Rene Mouawad",               new Recipient { uniqueid = 15298, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaReneMouawad",  displayname = Labels.airecipients["Rene Mouawad"], blockedforFree = true  } },
                { "Marj as Sultan North",       new Recipient { uniqueid = 15299, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaMarjasSultanNorth",  displayname = Labels.airecipients["Marj as Sultan North"], blockedforFree = true  } },
                { "Marj as Sultan South",       new Recipient { uniqueid = 15300, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaMarjasSultanSouth",  displayname = Labels.airecipients["Marj as Sultan South"], blockedforFree = true  } },
                { "Marj Ruhayyil",              new Recipient { uniqueid = 15301, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaMarjRuhayyil",  displayname = Labels.airecipients["Marj Ruhayyil"], blockedforFree = true  } },
                { "Al-Dumayr",                  new Recipient { uniqueid = 15302, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaAlDumayr",  displayname = Labels.airecipients["Al-Dumayr"], blockedforFree = true  } },
                { "Haifa",                      new Recipient { uniqueid = 15303, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaHaifa",  displayname = Labels.airecipients["Haifa"], blockedforFree = true  } },
                { "An Nasiriyah",               new Recipient { uniqueid = 15304, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaAnNasiriyah",  displayname = Labels.airecipients["An Nasiriyah"], blockedforFree = true  } },
                { "Al Qusayr",                  new Recipient { uniqueid = 15305, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaAlQusayr",  displayname = Labels.airecipients["Al Qusayr"], blockedforFree = true  } },
                { "Khalkhalah",                 new Recipient { uniqueid = 15306, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaKhalkhalah",  displayname = Labels.airecipients["Khalkhalah"], blockedforFree = true  } },
                { "Ramat David",                new Recipient { uniqueid = 15307, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaRamatDavid",  displayname = Labels.airecipients["Ramat David"], blockedforFree = true  } },
                { "Megiddo",                    new Recipient { uniqueid = 15308, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaMegiddo",  displayname = Labels.airecipients["Megiddo"], blockedforFree = true  } },
                { "Eyn Shemer",                 new Recipient { uniqueid = 15309, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaEynShemer",  displayname = Labels.airecipients["Eyn Shemer"], blockedforFree = true  } },
                { "Bassel Al-Assad",            new Recipient { uniqueid = 15310, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaBasselAlAssad",  displayname = Labels.airecipients["Bassel Al-Assad"], blockedforFree = true  } },
                { "Abu al-Duhur",               new Recipient { uniqueid = 15311, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaAbualDuhur",  displayname = Labels.airecipients["Abu al-Duhur"], blockedforFree = true  } },
                { "Taftanaz",                   new Recipient { uniqueid = 15312, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaTaftanaz",  displayname = Labels.airecipients["Taftanaz"], blockedforFree = true  } },
                { "Hatay",                      new Recipient { uniqueid = 15313, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaHatay",  displayname = Labels.airecipients["Hatay"], blockedforFree = true  } },
                { "Kuweires",                   new Recipient { uniqueid = 15314, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaKuweires",  displayname = Labels.airecipients["Kuweires"], blockedforFree = true  } },
                { "Minakh",                     new Recipient { uniqueid = 15315, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaMinakh",  displayname = Labels.airecipients["Minakh"], blockedforFree = true  } },
                { "Jirah",                      new Recipient { uniqueid = 15316, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaJirah",  displayname = Labels.airecipients["Jirah"], blockedforFree = true  } },
                { "Adana Sakirpasa",            new Recipient { uniqueid = 15317, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaAdanaSakirpasa",  displayname = Labels.airecipients["Adana Sakirpasa"], blockedforFree = true  } },
                { "Incirlik",                   new Recipient { uniqueid = 15318, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaIncirlik",  displayname = Labels.airecipients["Incirlik"], blockedforFree = true  } },
                { "Damascus",                   new Recipient { uniqueid = 15319, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaDamascus",  displayname = Labels.airecipients["Damascus"], blockedforFree = true  } },
                { "Tha'lah",                    new Recipient { uniqueid = 15320, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaThalah",  displayname = Labels.airecipients["Tha'lah"], blockedforFree = true  } },
                { "Larnaca",                    new Recipient { uniqueid = 15321, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaLarnaca",  displayname = Labels.airecipients["Larnaca"], blockedforFree = true  } },
                { "Akrotiri",                   new Recipient { uniqueid = 15322, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaAkrotiri",  displayname = Labels.airecipients["Akrotiri"], blockedforFree = true  } },
                { "King Hussein Air College",   new Recipient { uniqueid = 15323, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaKingHussein",  displayname = Labels.airecipients["King Hussein Air College"], blockedforFree = true  } },
                { "At Tanf",                    new Recipient { uniqueid = 15324, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaAtTanf",  displayname = Labels.airecipients["At Tanf"], blockedforFree = true  } },
                { "H3",                         new Recipient { uniqueid = 15326, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaH3",  displayname = Labels.airecipients["H3"], blockedforFree = true  } },
                { "H3 Northwest",               new Recipient { uniqueid = 15327, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaH3Northwest",  displayname = Labels.airecipients["H3 Northwest"], blockedforFree = true  } },
                { "H3 Southwest",               new Recipient { uniqueid = 15328, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaH3Southwest",  displayname = Labels.airecipients["H3 Southwest"], blockedforFree = true  } },
                { "H4",                         new Recipient { uniqueid = 15329, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaH4",  displayname = Labels.airecipients["H4"], blockedforFree = true  } },
                { "Paphos",                     new Recipient { uniqueid = 15330, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaPaphos",  displayname = Labels.airecipients["Paphos"], blockedforFree = true  } },
                { "Lakatamia",                  new Recipient { uniqueid = 15331, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaLakatamia",  displayname = Labels.airecipients["Lakatamia"], blockedforFree = true  } },
                { "Ercan",                      new Recipient { uniqueid = 15332, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaErcan",  displayname = Labels.airecipients["Ercan"], blockedforFree = true  } },
                { "Muwaffaq Salti",             new Recipient { uniqueid = 15333, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaMuwaffaqSalti",  displayname = Labels.airecipients["Muwaffaq Salti"], blockedforFree = true  } },
                { "King Abdullah II",           new Recipient { uniqueid = 15334, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaKingAbdullahII",  displayname = Labels.airecipients["King Abdullah II"], blockedforFree = true  } },
                //{ "Tel Nof",                    new Recipient { uniqueid = 15335, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaTelNof",  displayname = Labels.airecipients["Tel Nof"], blockedforFree = true  } }, //Already defined in Sinai
                //{ "Hatzor",                     new Recipient { uniqueid = 15336, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaHatzor",  displayname = Labels.airecipients["Hatzor"], blockedforFree = true  } }, //Already defined in Sinai
                //{ "Palmachim",                  new Recipient { uniqueid = 15337, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaPalmachim",  displayname = Labels.airecipients["Palmachim"], blockedforFree = true  } }, //Already defined in Sinai
                //{ "Ben Gurion",                 new Recipient { uniqueid = 15338, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaBenGurion",  displayname = Labels.airecipients["Ben Gurion"], blockedforFree = true  } }, //Already defined in Sinai
                { "Marka",                      new Recipient { uniqueid = 15339, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaMarka",  displayname = Labels.airecipients["Marka"], blockedforFree = true  } },
                { "Herzliya",                   new Recipient { uniqueid = 15340, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaHerzliya",  displayname = Labels.airecipients["Herzliya"], blockedforFree = true  } },
                { "Prince Hassan",              new Recipient { uniqueid = 15341, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaPrinceHassan",  displayname = Labels.airecipients["Prince Hassan"], blockedforFree = true  } },

                { "wAIUnitATCSyriaMaximum",     new Recipient { uniqueid = 15349, category = RecipientCategories.aiatc, name = "wAIUnitATCSyriaMaximum",           } },

                // Sinai map
                
                { "wAIUnitATCSinaiNull",        new Recipient { uniqueid = 15350, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiNull",              } },

                { "Al Mansurah",                new Recipient { uniqueid = 15351, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiAlMansurah",  displayname = Labels.airecipients["Al Mansurah"], blockedforFree = true  } },
                { "AzZaqaziq",                  new Recipient { uniqueid = 15352, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiAzZaqaziq",  displayname = Labels.airecipients["AzZaqaziq"], blockedforFree = true  } },
                { "As Salihiyah",               new Recipient { uniqueid = 15353, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiAsSalihiyah",  displayname = Labels.airecipients["As Salihiyah"], blockedforFree = true  } },
                { "Bilbeis Air Base",           new Recipient { uniqueid = 15354, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiBilbeisAirBase",  displayname = Labels.airecipients["Bilbeis Air Base"], blockedforFree = true  } },
                { "Inshas Airbase",             new Recipient { uniqueid = 15355, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiInshasAirbase",  displayname = Labels.airecipients["Inshas Airbase"], blockedforFree = true  } },
                { "Abu Suwayr",                 new Recipient { uniqueid = 15356, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiAbuSuwayr",  displayname = Labels.airecipients["Abu Suwayr"], blockedforFree = true  } },
                { "Al Ismailiyah",              new Recipient { uniqueid = 15357, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiAlIsmailiyah",  displayname = Labels.airecipients["Al Ismailiyah"], blockedforFree = true  } },
                { "Cairo International Airport",new Recipient { uniqueid = 15358, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiCairoInternationalAirport",  displayname = Labels.airecipients["Cairo International Airport"], blockedforFree = true  } },
                { "Difarsuwar Airfield",        new Recipient { uniqueid = 15359, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiDifarsuwarAirfield",  displayname = Labels.airecipients["Difarsuwar Airfield"], blockedforFree = true  } },
                { "Wadi al Jandali",            new Recipient { uniqueid = 15360, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiWadialJandali",  displayname = Labels.airecipients["Wadi al Jandali"], blockedforFree = true  } },
                { "Fayed",                      new Recipient { uniqueid = 15361, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiFayed",  displayname = Labels.airecipients["Fayed"], blockedforFree = true  } },
                { "Baluza",                     new Recipient { uniqueid = 15362, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiBaluza",  displayname = Labels.airecipients["Baluza"], blockedforFree = true  } },
                { "Cairo West",                 new Recipient { uniqueid = 15363, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiCairoWest",  displayname = Labels.airecipients["Cairo West"], blockedforFree = true  } },
                { "Kibrit Air Base",            new Recipient { uniqueid = 15364, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiKibritAirBase",  displayname = Labels.airecipients["Kibrit Air Base"], blockedforFree = true  } },
                { "Melez",                      new Recipient { uniqueid = 15365, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiMelez",  displayname = Labels.airecipients["Melez"], blockedforFree = true  } },
                { "Bir Hasanah",                new Recipient { uniqueid = 15366, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiBirHasanah",  displayname = Labels.airecipients["Bir Hasanah"], blockedforFree = true  } },
                { "El Arish",                   new Recipient { uniqueid = 15367, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiElArish",  displayname = Labels.airecipients["El Arish"], blockedforFree = true  } },
                { "El Gora",                    new Recipient { uniqueid = 15368, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiElGora",  displayname = Labels.airecipients["El Gora"], blockedforFree = true  } },
                { "Abu Rudeis",                 new Recipient { uniqueid = 15369, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiAbuRudeis",  displayname = Labels.airecipients["Abu Rudeis"], blockedforFree = true  } },
                { "Kedem",                      new Recipient { uniqueid = 15370, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiKedem",  displayname = Labels.airecipients["Kedem"], blockedforFree = true  } },
                { "Ramon Airbase",              new Recipient { uniqueid = 15371, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiRamonAirbase",  displayname = Labels.airecipients["Ramon Airbase"], blockedforFree = true  } },
                { "Hatzerim",                   new Recipient { uniqueid = 15372, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiHatzerim",  displayname = Labels.airecipients["Hatzerim"], blockedforFree = true  } },
                { "Hatzor",                     new Recipient { uniqueid = 15373, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiHatzor",  displayname = Labels.airecipients["Hatzor"], blockedforFree = true  } },
                { "Palmachim",                  new Recipient { uniqueid = 15374, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiPalmachim",  displayname = Labels.airecipients["Palmachim"], blockedforFree = true  } },
                { "Tel Nof",                    new Recipient { uniqueid = 15375, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiTelNof",  displayname = Labels.airecipients["Tel Nof"], blockedforFree = true  } },
                { "Nevatim",                    new Recipient { uniqueid = 15376, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiNevatim",  displayname = Labels.airecipients["Nevatim"], blockedforFree = true  } },
                { "Sde Dov",                    new Recipient { uniqueid = 15377, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiSdeDov",  displayname = Labels.airecipients["Sde Dov"], blockedforFree = true  } },
                { "Ben-Gurion",                 new Recipient { uniqueid = 15378, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiBenGurion",  displayname = Labels.airecipients["Ben-Gurion"], blockedforFree = true  } },
                { "Ovda",                       new Recipient { uniqueid = 15379, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiOvda",  displayname = Labels.airecipients["Ovda"], blockedforFree = true  } },
                { "St Catherine",               new Recipient { uniqueid = 15380, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiStCatherine",  displayname = Labels.airecipients["St Catherine"], blockedforFree = true  } },
                { "Wadi Abu Rish",              new Recipient { uniqueid = 15381, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiWadiAbuRish",  displayname = Labels.airecipients["Wadi Abu Rish"], blockedforFree = true  } },
                { "Ramon International Airport", new Recipient { uniqueid = 15382, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiRamonInternationalAirport",  displayname = Labels.airecipients["Ramon International Airport"], blockedforFree = true  } },
                { "Sharm El Sheikh International Airport",  new Recipient { uniqueid = 15383, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiSharmElSheikh",  displayname = Labels.airecipients["Sharm El Sheikh International Airport"], blockedforFree = true  } },
                { "Hurghada International Airport",  new Recipient { uniqueid = 15384, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiHurghadaInternationalAirport",  displayname = Labels.airecipients["Hurghada International Airport"], blockedforFree = true  } },
                { "Al Bahr al Ahmar",           new Recipient { uniqueid = 15385, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiAlBahralAhmar",  displayname = Labels.airecipients["Al Bahr al Ahmar"], blockedforFree = true  } },
                { "El Minya",                   new Recipient { uniqueid = 15386, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiElMinya",  displayname = Labels.airecipients["El Minya"], blockedforFree = true  } },
                { "Beni Suef",                  new Recipient { uniqueid = 15387, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiBeniSuef",  displayname = Labels.airecipients["Beni Suef"], blockedforFree = true  } },
                { "Kom Awshim",                 new Recipient { uniqueid = 15388, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiKomAwshim",  displayname = Labels.airecipients["Kom Awshim"], blockedforFree = true  } },
                { "Quwaysina",                  new Recipient { uniqueid = 15389, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiQuwaysina",  displayname = Labels.airecipients["Quwaysina"], blockedforFree = true  } },
                { "Birma Air Base",             new Recipient { uniqueid = 15390, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiBirmaAirBase",  displayname = Labels.airecipients["Birma Air Base"], blockedforFree = true  } },
                { "Gebel El Basur Air Base",    new Recipient { uniqueid = 15391, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiGebelElBasur",  displayname = Labels.airecipients["Gebel El Basur Air Base"], blockedforFree = true  } },
                { "Al Rahmaniyah Air Base",     new Recipient { uniqueid = 15392, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiAlRahmaniyah",  displayname = Labels.airecipients["Al Rahmaniyah Air Base"], blockedforFree = true  } },
                { "Jiyanklis Air Base",                  new Recipient { uniqueid = 15393, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiJiyanklis",  displayname = Labels.airecipients["Jiyanklis Air Base"], blockedforFree = true  } },
                { "Borj El Arab International Airport",  new Recipient { uniqueid = 15394, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiJBorjElArab",  displayname = Labels.airecipients["Borj El Arab International Airport"], blockedforFree = true  } },

                { "wAIUnitATCSinaiMaximum",     new Recipient { uniqueid = 15399, category = RecipientCategories.aiatc, name = "wAIUnitATCSinaiMaximum",           } },

                // South Atlantic map

                { "wAIUnitATCSAtlanticNull",      new Recipient { uniqueid = 15400, category = RecipientCategories.aiatc, name = "wAIUnitATCSAtlanticNull",              } },

                { "Mount Pleasant",             new Recipient { uniqueid = 15401, category = RecipientCategories.aiatc, name = "wAIUnitATCCSAtlanticMountPleasant",  displayname = Labels.airecipients["Mount Pleasant"], blockedforFree = true  } },
                { "Goose Green",                new Recipient { uniqueid = 15402, category = RecipientCategories.aiatc, name = "wAIUnitATCSAtlanticGooseGreen",  displayname = Labels.airecipients["Goose Green"], blockedforFree = true  } },
                { "San Carlos FOB",             new Recipient { uniqueid = 15403, category = RecipientCategories.aiatc, name = "wAIUnitATCSAtlanticSanCarlosFOB",  displayname = Labels.airecipients["San Carlos FOB"], blockedforFree = true  } },
                { "Port Stanley",               new Recipient { uniqueid = 15404, category = RecipientCategories.aiatc, name = "wAIUnitATCSAtlanticPortStanley",  displayname = Labels.airecipients["Port Stanley"], blockedforFree = true  } },
                { "Aerodromo De Tolhuin",       new Recipient { uniqueid = 15405, category = RecipientCategories.aiatc, name = "wAIUnitATCSAtlanticAerodromoDeTolhuin",  displayname = Labels.airecipients["Aerodromo De Tolhuin"], blockedforFree = true  } },
                { "Rio Grande",                 new Recipient { uniqueid = 15406, category = RecipientCategories.aiatc, name = "wAIUnitATCSAtlanticRioGrande",  displayname = Labels.airecipients["Rio Grande"], blockedforFree = true  } },
                { "Puerto Williams",            new Recipient { uniqueid = 15407, category = RecipientCategories.aiatc, name = "wAIUnitATCSAtlanticPuertoWilliams",  displayname = Labels.airecipients["Puerto Williams"], blockedforFree = true  } },
                { "San Julian",                 new Recipient { uniqueid = 15408, category = RecipientCategories.aiatc, name = "wAIUnitATCSAtlanticSanJulian",  displayname = Labels.airecipients["San Julian"], blockedforFree = true  } },
                { "Ushuaia Helo Port",          new Recipient { uniqueid = 15409, category = RecipientCategories.aiatc, name = "wAIUnitATCSAtlanticUshuaiaHeloPort",  displayname = Labels.airecipients["Ushuaia Helo Port"], blockedforFree = true  } },
                { "Ushuaia",                    new Recipient { uniqueid = 15410, category = RecipientCategories.aiatc, name = "wAIUnitATCSAtlanticUshuaia",  displayname = Labels.airecipients["Ushuaia"], blockedforFree = true  } },
                { "Pampa Guanaco",              new Recipient { uniqueid = 15411, category = RecipientCategories.aiatc, name = "wAIUnitATCSAtlanticPampaGuanaco",  displayname = Labels.airecipients["Pampa Guanaco"], blockedforFree = true  } },
                { "Puerto Santa Cruz",          new Recipient { uniqueid = 15412, category = RecipientCategories.aiatc, name = "wAIUnitATCSAtlanticPuertoSantaCruz",  displayname = Labels.airecipients["Puerto Santa Cruz"], blockedforFree = true  } },
                { "Rio Chico",                  new Recipient { uniqueid = 15413, category = RecipientCategories.aiatc, name = "wAIUnitATCSAtlanticRioChico",  displayname = Labels.airecipients["Rio Chico"], blockedforFree = true  } },
                { "Rio Gallegos",               new Recipient { uniqueid = 15414, category = RecipientCategories.aiatc, name = "wAIUnitATCSAtlanticRioGallegos",  displayname = Labels.airecipients["Rio Gallegos"], blockedforFree = true  } },
                { "Franco Bianco",              new Recipient { uniqueid = 15415, category = RecipientCategories.aiatc, name = "wAIUnitATCSAtlanticFrancoBianco",  displayname = Labels.airecipients["Franco Bianco"], blockedforFree = true  } },
                { "Comandante Luis Piedrabuena",  new Recipient { uniqueid = 15416, category = RecipientCategories.aiatc, name = "wAIUnitATCSAtlanticComandanteLuisPiedrabuena",  displayname = Labels.airecipients["Comandante Luis Piedrabuena"], blockedforFree = true  } },
                { "Porvenir Airfield",          new Recipient { uniqueid = 15417, category = RecipientCategories.aiatc, name = "wAIUnitATCSAtlanticPorvenirAirfield",  displayname = Labels.airecipients["Porvenir Airfield"], blockedforFree = true  } },
                { "Punta Arenas",               new Recipient { uniqueid = 15418, category = RecipientCategories.aiatc, name = "wAIUnitATCSAtlanticPuntaArenas",  displayname = Labels.airecipients["Punta Arenas"], blockedforFree = true  } },
                { "Aeropuerto de Gobernador Gregores",  new Recipient { uniqueid = 15419, category = RecipientCategories.aiatc, name = "wAIUnitATCSAtlanticAeropuertodeGobernadorGregores",  displayname = Labels.airecipients["Aeropuerto de Gobernador Gregores"], blockedforFree = true  } },
                { "Rio Turbio",                 new Recipient { uniqueid = 15420, category = RecipientCategories.aiatc, name = "wAIUnitATCSAtlanticRioTurbio",  displayname = Labels.airecipients["Rio Turbio"], blockedforFree = true  } },
                { "El Calafate",                new Recipient { uniqueid = 15421, category = RecipientCategories.aiatc, name = "wAIUnitATCSAtlanticElCalafate",  displayname = Labels.airecipients["El Calafate"], blockedforFree = true  } },
                { "Puerto Natales",             new Recipient { uniqueid = 15422, category = RecipientCategories.aiatc, name = "wAIUnitATCSAtlanticPuertoNatales",  displayname = Labels.airecipients["Puerto Natales"], blockedforFree = true  } },
                { "Aerodromo O'Higgins",        new Recipient { uniqueid = 15423, category = RecipientCategories.aiatc, name = "wAIUnitATCSAtlanticAerodromoO'Higgins",  displayname = Labels.airecipients["Aerodromo O'Higgins"], blockedforFree = true  } },

                { "wAIUnitATCSAtlanticMaximum",   new Recipient { uniqueid = 15440, category = RecipientCategories.aiatc, name = "wAIUnitATCSAtlanticMaximum",           } },
                
                // Kola Penisular Map

                { "wAIUnitATCKolaNull",         new Recipient { uniqueid = 15450, category = RecipientCategories.aiatc, name = "wAIUnitATCKolaNull",              } },

                { "BAS100",                     new Recipient { uniqueid = 15451, category = RecipientCategories.aiatc, name = "wAIUnitATCCKolaBAS100",  displayname = Labels.airecipients["BAS100"], blockedforFree = true  } },
                //{ "Kemi",                       new Recipient { uniqueid = 15452, category = RecipientCategories.aiatc, name = "wAIUnitATCKolaKemi",  displayname = Labels.airecipients["Kemi"], blockedforFree = true  } },
                { "Kemi Tornio",                new Recipient { uniqueid = 15453, category = RecipientCategories.aiatc, name = "wAIUnitATCKolaKemiTornio",  displayname = Labels.airecipients["Kemi Tornio"], blockedforFree = true  } },
                { "Rovaniemi",                  new Recipient { uniqueid = 15454, category = RecipientCategories.aiatc, name = "wAIUnitATCKolaRovaniemi",  displayname = Labels.airecipients["Rovaniemi"], blockedforFree = true  } },
                { "Bodo",                       new Recipient { uniqueid = 15455, category = RecipientCategories.aiatc, name = "wAIUnitATCKolaBodo",  displayname = Labels.airecipients["Bodo"], blockedforFree = true  } },
                { "Lakselv",                    new Recipient { uniqueid = 15456, category = RecipientCategories.aiatc, name = "wAIUnitATCKolaLakselv",  displayname = Labels.airecipients["Lakselv"], blockedforFree = true  } },
                { "Jokkmokk",                   new Recipient { uniqueid = 15457, category = RecipientCategories.aiatc, name = "wAIUnitATCKolaJokkmokk",  displayname = Labels.airecipients["Jokkmokk"], blockedforFree = true  } },
                { "Kiruna",                     new Recipient { uniqueid = 15458, category = RecipientCategories.aiatc, name = "wAIUnitATCKolaKiruna",  displayname = Labels.airecipients["Kiruna"], blockedforFree = true  } },
                { "Kalixfors",                  new Recipient { uniqueid = 15459, category = RecipientCategories.aiatc, name = "wAIUnitATCKolaKalixfors",  displayname = Labels.airecipients["Kalixfors"], blockedforFree = true  } },
                { "Severomorsk1",               new Recipient { uniqueid = 15460, category = RecipientCategories.aiatc, name = "wAIUnitATCKolaSeveromorsk1",  displayname = Labels.airecipients["Severomorsk1"], blockedforFree = true  } },
                { "Severomorsk3",               new Recipient { uniqueid = 15461, category = RecipientCategories.aiatc, name = "wAIUnitATCKolaSeveromorsk3",  displayname = Labels.airecipients["Severomorsk3"], blockedforFree = true  } },
                { "Monchegorsk",                new Recipient { uniqueid = 15462, category = RecipientCategories.aiatc, name = "wAIUnitATCKolaMonchegorsk",  displayname = Labels.airecipients["Monchegorsk"], blockedforFree = true  } },
                { "Murmansk International",     new Recipient { uniqueid = 15463, category = RecipientCategories.aiatc, name = "wAIUnitATCKolaMurmanskInternational",  displayname = Labels.airecipients["Murmansk International"], blockedforFree = true  } },
                { "Olenegorsk",                 new Recipient { uniqueid = 15464, category = RecipientCategories.aiatc, name = "wAIUnitATCKolaOlenegorsk",  displayname = Labels.airecipients["Olenegorsk"], blockedforFree = true  } },

                { "wAIUnitATCKolaMaximum",      new Recipient { uniqueid = 15480, category = RecipientCategories.aiatc, name = "wAIUnitATCKolaMaximum",           } },

                // Afghanistan Map

                { "wAIUnitATCAfghanNull",       new Recipient { uniqueid = 15490, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanNull",              } },
                   // South
                { "Tarinkot",                   new Recipient { uniqueid = 15491, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanTarinkot",  displayname = Labels.airecipients["Tarinkot"], blockedforFree = true  } },
                { "kandahar",                   new Recipient { uniqueid = 15492, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghankandahar",  displayname = Labels.airecipients["kandahar"], blockedforFree = true  } },
                { "Kandahar Heliport",          new Recipient { uniqueid = 15493, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanKandaharHeliport",  displayname = Labels.airecipients["Kandahar Heliport"], blockedforFree = true  } },
                { "Chaghcharan",                new Recipient { uniqueid = 15494, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanChaghcharan",  displayname = Labels.airecipients["Chaghcharan"], blockedforFree = true  } },
                { "Bost",                       new Recipient { uniqueid = 15495, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanBost",  displayname = Labels.airecipients["Bost"], blockedforFree = true  } },
                { "Dwyer",                      new Recipient { uniqueid = 15496, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanDwyer",  displayname = Labels.airecipients["Dwyer"], blockedforFree = true  } },
                { "Camp Bastion",               new Recipient { uniqueid = 15497, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanCampBastion",  displayname = Labels.airecipients["Camp Bastion"], blockedforFree = true  } },
                { "Camp Bastion Heliport",      new Recipient { uniqueid = 15498, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanCampBastionHeliport",  displayname = Labels.airecipients["Camp Bastion Heliport"], blockedforFree = true  } },
                { "Qala i Naw",                 new Recipient { uniqueid = 15499, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanQalaiNaw",  displayname = Labels.airecipients["Qala i Naw"], blockedforFree = true  } },
                { "Herat",                      new Recipient { uniqueid = 15500, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanHerat",  displayname = Labels.airecipients["Herat"], blockedforFree = true  } },
                { "Shindand",                   new Recipient { uniqueid = 15501, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanShindand",  displayname = Labels.airecipients["Shindand"], blockedforFree = true  } },
                { "Shindand Heliport",          new Recipient { uniqueid = 15502, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanShindandHeliport",  displayname = Labels.airecipients["Shindand Heliport"], blockedforFree = true  } },
                { "Farah",                      new Recipient { uniqueid = 15503, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanFarah",  displayname = Labels.airecipients["Farah"], blockedforFree = true  } },
                { "Nimroz",                     new Recipient { uniqueid = 15504, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanNimroz",  displayname = Labels.airecipients["Nimroz"], blockedforFree = true  } },
                   // East
                { "Bagram",                     new Recipient { uniqueid = 15505, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanBagram",  displayname = Labels.airecipients["Bagram"], blockedforFree = true  } },
                { "Bamyan",                     new Recipient { uniqueid = 15506, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanBamyan",  displayname = Labels.airecipients["Bamyan"], blockedforFree = true  } },
                { "Ghazni Heliport",            new Recipient { uniqueid = 15507, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanGhazniHeliport",  displayname = Labels.airecipients["Ghazni Heliport"], blockedforFree = true  } },
                { "Gardez",                     new Recipient { uniqueid = 15508, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanGardez",  displayname = Labels.airecipients["Gardez"], blockedforFree = true  } },
                { "Jalalabad",                  new Recipient { uniqueid = 15509, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanJalalabad",  displayname = Labels.airecipients["Jalalabad"], blockedforFree = true  } },
                { "Kabul",                      new Recipient { uniqueid = 15510, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanKabul",  displayname = Labels.airecipients["Kabul"], blockedforFree = true  } },
                { "Khost",                      new Recipient { uniqueid = 15511, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanKhost",  displayname = Labels.airecipients["Khost"], blockedforFree = true  } },
                { "Khost Heliport",             new Recipient { uniqueid = 15512, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanKhostHeliport",  displayname = Labels.airecipients["Khost Heliport"], blockedforFree = true  } },
                { "Sharan",                     new Recipient { uniqueid = 15513, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanSharan",  displayname = Labels.airecipients["Sharan"], blockedforFree = true  } },
                { "Urgoon Heliport",            new Recipient { uniqueid = 15514, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanUrgoonHeliport",  displayname = Labels.airecipients["Urgoon Heliport"], blockedforFree = true  } },
                { "Maymana Zahiraddin Faryabi", new Recipient { uniqueid = 15515, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanMaymanaZahiraddinFaryabi",  displayname = Labels.airecipients["Maymana Zahiraddin Faryabi"], blockedforFree = true  } },
                { "Mary",                       new Recipient { uniqueid = 15516, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanMary",  displayname = Labels.airecipients["Mary"], blockedforFree = true  } },
                { "Mary North",                 new Recipient { uniqueid = 15517, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanMaryNorth",  displayname = Labels.airecipients["Mary North"], blockedforFree = true  } },
                   // North TODO
                { "wAIUnitATCAfghanMaximum",    new Recipient { uniqueid = 15540, category = RecipientCategories.aiatc, name = "wAIUnitATCAfghanMaximum",           } },

                // Iraq
                { "wAIUnitATCIraqNull",         new Recipient { uniqueid = 15550, category = RecipientCategories.aiatc, name = "wAIUnitATCIraqNull",              } }, 
                   // North
                { "Al-Asad Airbase",            new Recipient { uniqueid = 15551, category = RecipientCategories.aiatc, name = "wAIUnitATCIraqAl-AsadAirbase",  displayname = Labels.airecipients["Al-Asad Airbase"], blockedforFree = true  } },
                { "Al-Sahra Airport",           new Recipient { uniqueid = 15552, category = RecipientCategories.aiatc, name = "wAIUnitATCIraqAl-SahraAirport",  displayname = Labels.airecipients["Al-Sahra Airport"], blockedforFree = true  } },
                { "Al-Salam Airbase",           new Recipient { uniqueid = 15553, category = RecipientCategories.aiatc, name = "wAIUnitATCIraqAl-SalamAirbase",  displayname = Labels.airecipients["Al-Salam Airbase"], blockedforFree = true  } },
                { "Al-Taji Airport",            new Recipient { uniqueid = 15554, category = RecipientCategories.aiatc, name = "wAIUnitATCIraqAl-TajiAirport",  displayname = Labels.airecipients["Al-Taji Airport"], blockedforFree = true  } },
                { "Al-Taquddum Airport",        new Recipient { uniqueid = 15555, category = RecipientCategories.aiatc, name = "wAIUnitATCIraqAl-TaquddumAirport",  displayname = Labels.airecipients["Al-Taquddum Airport"], blockedforFree = true  } },
                { "Baghdad International Airport",  new Recipient { uniqueid = 15556, category = RecipientCategories.aiatc, name = "wAIUnitATCIraqBaghdadInternationalAirport",  displayname = Labels.airecipients["Baghdad International Airport"], blockedforFree = true  } },
                { "Balad Airbase",              new Recipient { uniqueid = 15557, category = RecipientCategories.aiatc, name = "wAIUnitATCIraqBaladAirbase",  displayname = Labels.airecipients["Balad Airbase"], blockedforFree = true  } },
                { "Bashur Airport",             new Recipient { uniqueid = 15558, category = RecipientCategories.aiatc, name = "wAIUnitATCIraqBashurAirport",  displayname = Labels.airecipients["Bashur Airport"], blockedforFree = true  } },
                { "Erbil International Airport",    new Recipient { uniqueid = 15559, category = RecipientCategories.aiatc, name = "wAIUnitATCIraqErbilInternationalAirport",  displayname = Labels.airecipients["Erbil International Airport"], blockedforFree = true  } },
                { "K1 Base",                    new Recipient { uniqueid = 15560, category = RecipientCategories.aiatc, name = "wAIUnitATCIraqK1Base",  displayname = Labels.airecipients["K1 Base"], blockedforFree = true  } },
                { "Kirkuk International Airport",   new Recipient { uniqueid = 15561, category = RecipientCategories.aiatc, name = "wAIUnitATCIraqKirkukInternationalAirport",  displayname = Labels.airecipients["Kirkuk International Airport"], blockedforFree = true  } },
                { "Qayyarah Airfield West",     new Recipient { uniqueid = 15562, category = RecipientCategories.aiatc, name = "wAIUnitATCIraqQayyarahAirfieldWest",  displayname = Labels.airecipients["Qayyarah Airfield West"], blockedforFree = true  } },
                { "Sulaimaniyah International Airport",     new Recipient { uniqueid = 15563, category = RecipientCategories.aiatc, name = "wAIUnitATCIraqSulaimaniyahInternationalAirport",  displayname = Labels.airecipients["Sulaimaniyah International Airport"], blockedforFree = true  } },
                   // South TODO
                { "wAIUnitATCIraqMaximum",      new Recipient { uniqueid = 15590, category = RecipientCategories.aiatc, name = "wAIUnitATCIraqMaximum",           } },
                
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
                { "CVN-70 Carl Vinson",         new Recipient { uniqueid = 15704, category = RecipientCategories.aiship, name = "wAIUnitATCCarlVinson",        displayname = Labels.airecipients["CVN-70 Carl Vinson"], blockedforFree = true } },
                { "CVN-74 John C. Stennis",     new Recipient { uniqueid = 15705, category = RecipientCategories.aiship, name = "wAIUnitATCJohnCStennis",      displayname = Labels.airecipients["CVN-74 John C. Stennis"], blockedforFree = true } },
                { "LHA-1 Tarawa",               new Recipient { uniqueid = 15706, category = RecipientCategories.aiunit, name = "wAIUnitATCTarawa",            displayname = Labels.airecipients["LHA-1 Tarawa"], blockedforFree = true } }, // was 15505 ship
                { "FFG-7CL Oliver Hazzard Perry",new Recipient{ uniqueid = 15707, category = RecipientCategories.aiunit, name = "wAIUnitATCHazzardPerry",      displayname = Labels.airecipients["FFG-7CL Oliver Hazzard Perry"], blockedforFree = true } },// was 15506 ship
                { "CG-60 Normandy",             new Recipient { uniqueid = 15708, category = RecipientCategories.aiunit, name = "wAIUnitATCCGNormandy",        displayname = Labels.airecipients["CG-60 Normandy"], blockedforFree = true } },// was 15508 ship
                
                // new carriers
                
                { "CVN-71 Theodore Roosevelt",  new Recipient { uniqueid = 15710, category = RecipientCategories.aiship, name = "wAIUnitATCRoosevelt",         displayname = Labels.airecipients["CVN-71 Theodore Roosevelt"], blockedforFree = true, requiresrealatc = true } },
                { "CVN-72 Abraham Lincoln",     new Recipient { uniqueid = 15711, category = RecipientCategories.aiship, name = "wAIUnitATCLincoln",           displayname = Labels.airecipients["CVN-72 Abraham Lincoln"], blockedforFree = true, requiresrealatc = true } },
                { "CVN-73 George Washington",   new Recipient { uniqueid = 15712, category = RecipientCategories.aiship, name = "wAIUnitATCWashington",        displayname = Labels.airecipients["CVN-73 George Washington"], blockedforFree = true, requiresrealatc = true } },
                { "CVN-75 Harry S. Truman",     new Recipient { uniqueid = 15713, category = RecipientCategories.aiship, name = "wAIUnitATCTruman",            displayname = Labels.airecipients["CVN-75 Harry S. Truman"], blockedforFree = true, requiresrealatc = true } },

                // Non Supercarrier ATC comms enabled
                { "CV-59 Forrestal",            new Recipient { uniqueid = 15714, category = RecipientCategories.aiunit, name = "wAIUnitATCForrestal",         displayname = Labels.airecipients["CV-59 Forrestal"], blockedforFree = true } },

                // new roles
                { "wAIUnitCarrierRoleNull",     new Recipient { uniqueid = 15720, category = RecipientCategories.aiship, name = "wAIUnitCarrierRoleNull", }      },

                { "CarrierDeparture",           new Recipient { uniqueid = 15721, category = RecipientCategories.aiship, name = "wAIUnitCarrierDeparture",     displayname = Labels.airecipients["CarrierDeparture"], blockedforFree = true, requiresrealatc = true } },
                { "CarrierMarshal",             new Recipient { uniqueid = 15722, category = RecipientCategories.aiship, name = "wAIUnitCarrierMarshal",       displayname = Labels.airecipients["CarrierMarshal"], blockedforFree = true, requiresrealatc = true } },
                { "CarrierApproachTower",       new Recipient { uniqueid = 15723, category = RecipientCategories.aiship, name = "wAIUnitCarrierApproachTower", displayname = Labels.airecipients["CarrierApproachTower"], blockedforFree = true, requiresrealatc = true } },
                { "CarrierLSO",                 new Recipient { uniqueid = 15724, category = RecipientCategories.aiship, name = "wAIUnitCarrierLSO",           displayname = Labels.airecipients["CarrierLSO"], blockedforFree = true, requiresrealatc = true } },
                { "wAIUnitCarrierRoleMaximum",  new Recipient { uniqueid = 15730, category = RecipientCategories.aiship, name = "wAIUnitCarrierRoleMaximum", }      },


                { "wAIUnitATCCarriersMaximum",  new Recipient { uniqueid = 15790, category = RecipientCategories.aiship, name = "wAIUnitATCCarriersMaximum",   } },

                // Auto Imported 

                { "wAIUnitATCImportedNull",     new Recipient { uniqueid = 15800, category = RecipientCategories.aiatc, name = "wAIUnitATCImportedNull",      } },
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
                { "wAIUnitCrewMaximum",     new Recipient { uniqueid = 18099, category = RecipientCategories.aicrew, name = "wAIUnitCrewMaximum",       } },

                // parked

                { "wAIUnitMaximum",         new Recipient { uniqueid = 18999, category = RecipientCategories.aiunit, name = "wAIUnitMaximum",           } },

                { "wAIUnitAOCSNull",        new Recipient { uniqueid = 19000, category = RecipientCategories.aocs,  name = "wAIUnitAOCSNull",      } },
                { "aocs",                   new Recipient { uniqueid = 19001, category = RecipientCategories.aocs,  name = "wAIUnitAOCS",          displayname = Labels.airecipients["aocs"], blockedforFree = true } },
                { "wAIUnitAOCSMaximum",     new Recipient { uniqueid = 19049, category = RecipientCategories.aocs,  name = "wAIUnitAOCSMaximum",   } },

                { "wAIUnitAuxNull",         new Recipient { uniqueid = 19050, category = RecipientCategories.auxmenu, name = "wAIUnitAuxNull",          } },
                { "aux",                    new Recipient { uniqueid = 19051, category = RecipientCategories.auxmenu, name = "wAIUnitAuxMysteryGuest",  displayname = Labels.airecipients["aux"] } },
                //{ "AI_ATC",                 new Recipient { uniqueid = 19052, category = RecipientCategories.auxmenu, name = "wAI_ATC",  displayname = Labels.airecipients["AI_ATC"] } },
                { "wAIUnitAuxMaximum",      new Recipient { uniqueid = 19099, category = RecipientCategories.auxmenu, name = "wAIUnitAuxMaximum",       } },

                { "wAIUnitCargoNull",       new Recipient { uniqueid = 19100, category = RecipientCategories.cargo, name = "wAIUnitCargoNull",          } },
                { "cargo",                  new Recipient { uniqueid = 19101, category = RecipientCategories.cargo, name = "wAIUnitCargo",              displayname = Labels.airecipients["cargo"], blockedforFree = true } },
                { "wAIUnitCargoMaximum",    new Recipient { uniqueid = 19199, category = RecipientCategories.cargo, name = "wAIUnitCargoMaximum",       } },

                { "wAIUnitDescentNull",       new Recipient { uniqueid = 19200, category = RecipientCategories.cargo, name = "wAIUnitDescentNull",      } },
                { "descent",                  new Recipient { uniqueid = 19201, category = RecipientCategories.cargo, name = "wAIUnitDescent",          displayname = Labels.airecipients["descent"], blockedforFree = true  } },
                { "wAIUnitDescentMaximum",    new Recipient { uniqueid = 19299, category = RecipientCategories.cargo, name = "wAIUnitDescentMaximum",   } },

                // RIO pack
                { "wAIUnitFlightCrewMembersNull",        new Recipient { uniqueid = 19300, category = RecipientCategories.RIO, name = "wAIUnitFlightCrewMembersNull",         } },
                
                { "wAIUnitFlightCrewMembersMaximum",     new Recipient { uniqueid = 19399, category = RecipientCategories.RIO, name = "wAIUnitFlightCrewMembersMaximum",      } },


                // kneeboard extension
                { "wAIUnitKneeboardNull",       new Recipient { uniqueid = 19400, category = RecipientCategories.kneeboard, name = "wAIUnitKneeboardNull",      } },
                { "kneeboard",                  new Recipient { uniqueid = 19401, category = RecipientCategories.kneeboard, name = "wAIUnitKneeboard",          displayname = Labels.airecipients["kneeboard"], blockedforFree = true  } },
                { "wAIUnitKneeboardMaximum",    new Recipient { uniqueid = 19499, category = RecipientCategories.kneeboard, name = "wAIUnitKneeboardMaximum",   } },

                // Moose Ops 
                { "wAIUnitMooseNull" ,          new Recipient { uniqueid = 19500, category = RecipientCategories.Moose,    name = "wAIUnitMooseNull" } },
                { "Moose" ,                     new Recipient { uniqueid = 19501, category = RecipientCategories.Moose,    name = "wAIUnitMoose",displayname = Labels.airecipients["Moose"],blockedforFree = true   } },
                //{ "Radio Check LSO" ,           new Recipient { uniqueid = 19502, category = RecipientCategories.Moose,    name = "wAIUnitRadioChkLSO",displayname = Labels.airecipients["Radio Check LSO"],blockedforFree = true   } },
                { "wAIUnitMooseMaximum" ,       new Recipient { uniqueid = 19999, category = RecipientCategories.Moose,    name = "wAIUnitMooseMaximum" } },

            };
        }
    }
}
