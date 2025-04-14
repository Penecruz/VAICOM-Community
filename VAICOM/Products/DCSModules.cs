using System;
using System.Collections.Generic;
using VAICOM.Client;

namespace VAICOM
{
    namespace Products
    {

        public class DCSmodule
        {
            public string Id;
            public string Name;
            public string Alias;
            public string SpeechAlias;
            public int radiodelay;
            public int chnoffset;
            public bool ProOnly;
            public bool IsFC;
            public bool ApxWpn;
            public bool ApxDir;
            public bool IsHelo;
            public bool Singlehotkey;
            public bool Havedial;
            public string Theme;
            public List<int> Flightmenu;

            public DCSmodule()
            {
                chnoffset = 1;
                radiodelay = 0;
            }
        }

        public static class DCSmodules
        {
            public static DCSmodule findmodulebyid(string id)
            {
                DCSmodule result = null;

                foreach (KeyValuePair<string, DCSmodule> entry in LookupTable)
                {
                    if (id.ToLower().Contains(entry.Value.Id.ToLower()))
                    {
                        result = entry.Value;
                        break;
                    }
                }

                return result;
            }


            public static Dictionary<string, DCSmodule> LookupTable = new Dictionary<string, DCSmodule>(StringComparer.OrdinalIgnoreCase)
            {

                {"----",    new DCSmodule() { Id ="----", Name = "No Module",    Alias = "Detected",         ProOnly = true,  IsFC = false, ApxWpn = false,ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight1 ,Singlehotkey = false, Havedial = false, Theme = "Default",   SpeechAlias = "" } },

                {"A-10A" ,  new DCSmodule() { Id ="A-10A",Name = "A-10A",   Alias = "Warthog",       ProOnly = false, IsFC = true,  ApxWpn = true, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight1 ,Singlehotkey = false, Havedial = false, Theme = "RedFlag",   SpeechAlias = "A..Ten A"  } },
                {"A-10C" ,  new DCSmodule() { Id ="A-10C",Name = "A-10C",   Alias = "Warthog",       ProOnly = false, IsFC = false, ApxWpn = true, ApxDir = true,   IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight2 ,Singlehotkey = false, Havedial = false, Theme = "Afghan",   SpeechAlias = "A..Ten Cee" } },
                {"A-10C_2", new DCSmodule() { Id ="A-10C_2",Name = "A-10C", Alias = "Warthog II",    ProOnly = false, IsFC = false, ApxWpn = true, ApxDir = true,   IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight2 ,Singlehotkey = false, Havedial = false, Theme = "Afghan",   SpeechAlias = "A..Ten Cee two" } },
                {"F-15C" ,  new DCSmodule() { Id ="F-15C",Name = "F-15C",   Alias = "Eagle",         ProOnly = false, IsFC = true,  ApxWpn = true, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight1 ,Singlehotkey = false, Havedial = false, Theme = "RedFlag",   SpeechAlias = "F..Fifteen C"  } },
                {"SU-25",   new DCSmodule() { Id ="Su-25",Name = "SU-25",   Alias = "Frogfoot",      ProOnly = false, IsFC = true,  ApxWpn = true, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight1 ,Singlehotkey = false, Havedial = false, Theme = "Russia",    SpeechAlias = "Sue..Twenty Five"} },
                {"SU-25T",  new DCSmodule() { Id ="Su-25T",Name = "SU-25T",  Alias = "Frogfoot",     ProOnly = false, IsFC = true,  ApxWpn = true, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight1 ,Singlehotkey = false, Havedial = false, Theme = "Russia",    SpeechAlias = "Sue..Twenty Five Tee"} },
                {"F-86F",   new DCSmodule() { Id ="F-86F Sabre",Name = "F-86F",   Alias = "Sabre",   ProOnly = false, IsFC = false, ApxWpn = true, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight1 ,Singlehotkey = true , Havedial = false, Theme = "RedFlag",   SpeechAlias = "F..Eighty Six"} },
                {"KA-50",   new DCSmodule() { Id ="Ka-50",Name = "KA-50",   Alias = "BlackShark 3",  ProOnly = false, IsFC = false, ApxWpn = true, ApxDir = false,  IsHelo = true,  Flightmenu = DcsClient.iCommandsequences.showflight1 ,Singlehotkey = true , Havedial = true,  Theme = "Russia",    SpeechAlias = "Black Shark" } },
                {"Mi-8MT",  new DCSmodule() { Id ="Mi-8MT",Name = "Mi-8MT",  Alias = "Magificent8",  ProOnly = false, IsFC = false, ApxWpn = true, ApxDir = false,  IsHelo = true,  Flightmenu = DcsClient.iCommandsequences.showflight1 ,Singlehotkey = false , Havedial = true,  Theme = "Russia",    SpeechAlias = "M..Eye Eight" } },
                {"UH-1H",   new DCSmodule() { Id ="UH-1H",Name = "UH-1H",   Alias = "Huey",          ProOnly = false, IsFC = false, ApxWpn = true, ApxDir = false,  IsHelo = true,  Flightmenu = DcsClient.iCommandsequences.showflight1 ,Singlehotkey = true , Havedial = true,  Theme = "RedFlag",   SpeechAlias = "Huey"} },
                {"TF-51D",  new DCSmodule() { Id ="TF-51D",Name = "TF-51D",  Alias = "Mustang",      ProOnly = false, IsFC = false, ApxWpn = true, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight2 ,Singlehotkey = true , Havedial = false, Theme = "WWII",      SpeechAlias = "Tee..F Fifty One"} },
                {"MiG-15Bis",new DCSmodule(){ Id ="MiG-15Bis",Name ="MiG-15Bis",Alias = "Fagot",     ProOnly = false, IsFC = false, ApxWpn = true, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight1 ,Singlehotkey = true,  Havedial = false, Theme = "Russia",    SpeechAlias = "Mig..Fifteen" } },
                {"Su-27",   new DCSmodule() { Id ="Su-27",Name = "Su-27",   Alias = "Flanker",       ProOnly = false, IsFC = true,  ApxWpn = true, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight1 ,Singlehotkey = false, Havedial = false, Theme = "Russia",    SpeechAlias = "Sue..Twenty Seven" } },
                {"Hawk",    new DCSmodule() { Id ="Hawk",Name = "T.1A",    Alias = "Hawk",           ProOnly = false, IsFC = false, ApxWpn = true, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight2 ,Singlehotkey = true,  Havedial = false, Theme = "NATO",      SpeechAlias = "Hawk"  } },
                {"M-2000C", new DCSmodule() { Id ="M-2000C",Name = "Mirage", Alias = "2000C",        ProOnly = false, IsFC = false, ApxWpn = true, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight1 ,Singlehotkey = false, Havedial = false, Theme = "RedFlag",   SpeechAlias = "Mirage Two Thousand C"} },
                {"CA",      new DCSmodule() { Id ="artillery_commander",Name = "CA",      Alias = "Combined Arms",ProOnly = true,  IsFC = false, ApxWpn = true, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight1 ,Singlehotkey = true,  Havedial = false, Theme = "RedFlag",   SpeechAlias = "CA"  } },
                {"FA-18C" , new DCSmodule() { Id ="FA-18C_hornet",Name = "F/A-18C",  Alias = "Hornet",       ProOnly = true,  IsFC = false, ApxWpn = true, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight1 ,Singlehotkey = false, Havedial = false, Theme = "Navy",      SpeechAlias = "F A Eighteen C", radiodelay =0  } },
                {"F-14AB" , new DCSmodule() { Id ="F-14", Name = "F-14AB",  Alias = "Tomcat",       ProOnly = true,  IsFC = false, ApxWpn = true, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight1 ,Singlehotkey = false, Havedial = false, Theme = "Navy",      SpeechAlias = "F Fourteen", radiodelay =500, chnoffset = 0  } },
                {"AJS-37",  new DCSmodule() { Id ="AJS37",Name = "AJS-37",  Alias = "Viggen",       ProOnly = true,  IsFC = false, ApxWpn = true, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight2 ,Singlehotkey = false, Havedial = false, Theme = "NATO",      SpeechAlias = "A J S Thirty Seven"  } },
                {"F-5E",    new DCSmodule() { Id ="F-5E-3",Name = "F-5E",    Alias = "Tiger II",    ProOnly = true,  IsFC = false, ApxWpn = true, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight1 ,Singlehotkey = true,  Havedial = false, Theme = "Fallon",   SpeechAlias = "F Five E"  } }, // Includes F-5E Remastered
                {"F-5E_FC", new DCSmodule() { Id ="F-5E-3_FC",Name = "F-5E", Alias = "FC",    ProOnly = true,  IsFC = false, ApxWpn = true, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight1 ,Singlehotkey = true,  Havedial = false, Theme = "Fallon",   SpeechAlias = "F Five FC"  } }, //Add Flaming Cliffs version
                {"P-51D",   new DCSmodule() { Id ="P-51D",Name = "P-51D",   Alias = "Mustang",      ProOnly = true,  IsFC = false, ApxWpn = true, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight2 ,Singlehotkey = true , Havedial = false, Theme = "WWII",      SpeechAlias = "Pee Fifty One" } },
                {"Spitfire",new DCSmodule() { Id ="SpitfireLFMkIX",Name = "Spitfire",Alias = "LFMkIX",       ProOnly = true,  IsFC = false, ApxWpn = true, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight2 ,Singlehotkey = true , Havedial = false, Theme = "WWII",      SpeechAlias = "Spitfire" } },
                {"C-101",   new DCSmodule() { Id ="C-101",Name = "C-101",   Alias = "Aviojet",      ProOnly = true,  IsFC = false, ApxWpn = true, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight2 ,Singlehotkey = true , Havedial = false, Theme = "NATO",      SpeechAlias = "C One Oh One" } },
                {"Bf-109",  new DCSmodule() { Id ="Bf-109K-4",Name ="Bf-109K-4",Alias = "Kurfurst",     ProOnly = true,  IsFC = false, ApxWpn = true, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight2 ,Singlehotkey = true,  Havedial = false, Theme = "WWII",      SpeechAlias = "B F One Oh Nine" } },
                {"Fw-190",  new DCSmodule() { Id ="FW-190D9",Name = "FW-190D9",Alias = "Dora",         ProOnly = true,  IsFC = false, ApxWpn = true, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight2 ,Singlehotkey = true,  Havedial = false, Theme = "WWII",      SpeechAlias = "Focke Wulf One Ninety" } },
                {"Su-33",   new DCSmodule() { Id ="Su-33",Name = "Su-33",   Alias = "Flanker-D",    ProOnly = true,  IsFC = true,  ApxWpn = true, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight1 ,Singlehotkey = false, Havedial = false, Theme = "Russia",    SpeechAlias = "Sue Thirty Three"  } },
                {"MiG-21Bis",new DCSmodule(){ Id ="MiG-21Bis",Name ="MiG-21Bis",Alias = "Fishbed",      ProOnly = true,  IsFC = false, ApxWpn = true, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight1 ,Singlehotkey = false,  Havedial = false, Theme = "Russia",    SpeechAlias = "Mig Twenty One" } },
                {"L-39ZA",  new DCSmodule() { Id ="L-39ZA",Name = "L-39ZA",  Alias = "Albatros",     ProOnly = true,  IsFC = false, ApxWpn = true, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight1 ,Singlehotkey = true,  Havedial = false, Theme = "NATO",      SpeechAlias = "Albatros"   } },
                {"L-39C",   new DCSmodule() { Id ="L-39C",Name = "L-39C",   Alias = "Albatros",     ProOnly = true,  IsFC = false, ApxWpn = true, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight1 ,Singlehotkey = true,  Havedial = false, Theme = "NATO",      SpeechAlias = "Albatros"   } },
                {"SA342M",  new DCSmodule() { Id ="SA342M",Name = "SA342M",  Alias = "Gazelle",      ProOnly = true,  IsFC = false, ApxWpn = true, ApxDir = false,  IsHelo = true,  Flightmenu = DcsClient.iCommandsequences.showflight1 ,Singlehotkey = true,  Havedial = true,  Theme = "RedFlag",   SpeechAlias = "Gazelle"  } },
                {"SA342L",  new DCSmodule() { Id ="SA342L",Name = "SA342L",  Alias = "Gazelle",      ProOnly = true,  IsFC = false, ApxWpn = true, ApxDir = false,  IsHelo = true,  Flightmenu = DcsClient.iCommandsequences.showflight1 ,Singlehotkey = true,  Havedial = true,  Theme = "RedFlag",   SpeechAlias = "Gazelle"   } },
                //{"SA342Minigun",  new DCSmodule() { Id ="SA342Minigun",Name = "SA342Minigun",  Alias = "Gazelle",      ProOnly = true,  IsFC = false, ApxWpn = true, ApxDir = false,  IsHelo = true,  Flightmenu = DcsClient.iCommandsequences.showflight1 ,Singlehotkey = true,  Havedial = true,  Theme = "RedFlag",   SpeechAlias = "Gazelle"   } },
                //{"SA342Mistral",  new DCSmodule() { Id ="SA342Mistral",Name = "SA342Mistral",  Alias = "Gazelle",      ProOnly = true,  IsFC = false, ApxWpn = true, ApxDir = false,  IsHelo = true,  Flightmenu = DcsClient.iCommandsequences.showflight1 ,Singlehotkey = true,  Havedial = true,  Theme = "RedFlag",   SpeechAlias = "Gazelle"   } },
                {"MiG-29" , new DCSmodule() { Id ="MiG-29",Name = "MiG-29",  Alias = "Fulcrum",      ProOnly = true,  IsFC = true,  ApxWpn = true, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight1 ,Singlehotkey = false, Havedial = false, Theme = "Russia",    SpeechAlias = "Mig Twenty Nine"} },
                {"J-11A" ,  new DCSmodule() { Id ="J-11A",Name = "J-11A",   Alias = "Shenyang",      ProOnly = true,  IsFC = true,  ApxWpn = true, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight1 ,Singlehotkey = false, Havedial = false, Theme = "Russia",    SpeechAlias = "Jay Eleven"} },
                {"AV-8B",   new DCSmodule() { Id ="AV8BNA",Name = "AV-8B(NA)", Alias = "Harrier II",      ProOnly = true,  IsFC = false, ApxWpn = true, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight1 ,Singlehotkey = false, Havedial = false, Theme = "Navy",      SpeechAlias = "Harrier"  } },
                {"F-16C_50",new DCSmodule() { Id ="F-16C_50",Name = "F-16C",  Alias = "Viper",       ProOnly = true,  IsFC = false, ApxWpn = false, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight1 ,Singlehotkey = false,  Havedial = false, Theme = "NATO",    SpeechAlias = "Viper"  } },
                {"Yak-52",  new DCSmodule() { Id ="Yak-52",Name = "Yak-52",  Alias = "",             ProOnly = true,  IsFC = false, ApxWpn = true, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight2 ,Singlehotkey = true,  Havedial = false, Theme = "Russia",    SpeechAlias = "Yak Fifty Two"  } },
                {"Mi-24P",  new DCSmodule() { Id ="Mi-24P",Name = "Mi-24P",  Alias = "Hind",     ProOnly = true,  IsFC = false, ApxWpn = true, ApxDir = false,  IsHelo = true, Flightmenu = DcsClient.iCommandsequences.showflight2 ,Singlehotkey = true,  Havedial = true, Theme = "Russia",    SpeechAlias = "Hind"  } },
                {"AH-64D",  new DCSmodule() { Id ="AH-64D",Name = "AH-64D",  Alias = "Apache",     ProOnly = true,  IsFC = false, ApxWpn = true, ApxDir = false,  IsHelo = true, Flightmenu = DcsClient.iCommandsequences.showflight2 ,Singlehotkey = true,  Havedial = true, Theme = "NATO",    SpeechAlias = "Apache"  } },
                {"Mosquito",new DCSmodule() { Id ="MosquitoFBMkVI",Name = "Mosquito",  Alias = "",     ProOnly = true,  IsFC = false, ApxWpn = true, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight2 ,Singlehotkey = false,  Havedial = false, Theme = "WWII",    SpeechAlias = "Mosquito"  } },
                {"MiG-19P", new DCSmodule() { Id ="MiG-19P", Name = "MiG-19P",  Alias = "Farmer", ProOnly = true,  IsFC = false, ApxWpn = false, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight2 ,Singlehotkey = true,  Havedial = false, Theme = "Russia",    SpeechAlias = "Mig..Nineteen"  } },
                {"JF-17",   new DCSmodule() { Id ="JF-17", Name = "JF-17",  Alias = "Thunder", ProOnly = true,  IsFC = false, ApxWpn = false, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight2 ,Singlehotkey = false,  Havedial = false, Theme = "Russia",    SpeechAlias = "J F Seventeen"  } },
                {"I-16",    new DCSmodule() { Id ="I-16", Name = "I-16",  Alias = "",  ProOnly = true, IsFC = false, ApxWpn = false, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight2 ,Singlehotkey = true,  Havedial = false, Theme = "Russia",    SpeechAlias = "Eye Sixteen"  } },
                {"Fw-190A8",new DCSmodule() { Id ="FW-190A8",Name = "FW-190A8", Alias = "Anton", ProOnly = true,  IsFC = false, ApxWpn = true, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight2 ,Singlehotkey = true,  Havedial = false, Theme = "WWII",      SpeechAlias = "Focke Wulf One Ninety Anton" } },
                {"P-47D",   new DCSmodule() { Id ="P-47D",Name = "P-47D", Alias = "Thunderbolt", ProOnly = true,  IsFC = false, ApxWpn = true, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight2 ,Singlehotkey = true,  Havedial = false, Theme = "WWII",      SpeechAlias = "Pee Forty Seven" } },
                {"CE-2",    new DCSmodule() { Id ="CE-2", Name = "Christen",  Alias = "Eagle II", ProOnly = true,  IsFC = false, ApxWpn = false, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight2 ,Singlehotkey = true,  Havedial = false, Theme = "RedFlag",    SpeechAlias = "Christen Eagle"  } },
                
                // VPC Additions
                {"MB-339A",   new DCSmodule() { Id ="MB-339A",Name = "MB-339A", Alias = "", ProOnly = true, IsFC = false, ApxWpn = true, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight1 ,Singlehotkey = true,  Havedial = false, Theme = "NATO", SpeechAlias = "M..Bee Three Three Nine"  } },
                {"Mirage-F1", new DCSmodule() { Id ="Mirage-F1",Name = "Mirage-", Alias = "F1", ProOnly = true, IsFC = false, ApxWpn = true, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight1 ,Singlehotkey = false,  Havedial = false, Theme = "NATO", SpeechAlias = "F One"  } },
                {"F-15ESE",   new DCSmodule() { Id ="F-15ESE",Name = "F-15E", Alias = "Strike Eagle", ProOnly = true, IsFC = false, ApxWpn = true, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight1 ,Singlehotkey = false,  Havedial = false, Theme = "NATO",   SpeechAlias = "F Fifteen E"  } },
                {"KA-50-3",   new DCSmodule() { Id ="Ka-50_3",Name = "KA-50",   Alias = "BlackShark",    ProOnly = false, IsFC = false, ApxWpn = true, ApxDir = false,  IsHelo = true,  Flightmenu = DcsClient.iCommandsequences.showflight1 ,Singlehotkey = true , Havedial = true,  Theme = "Russia",    SpeechAlias = "Black Shark Three" } },
                {"F-4E-45MC", new DCSmodule() { Id ="F-4E-45MC",Name = "F-4E", Alias = "Phantom II", ProOnly = true, IsFC = false, ApxWpn = true, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight1 ,Singlehotkey = false,  Havedial = false, Theme = "NATO",   SpeechAlias = "F Four E"  } },
                {"OH58D",     new DCSmodule() { Id ="OH58D",Name = "OH-58D", Alias = "Kiowa Warrior", ProOnly = true, IsFC = false, ApxWpn = true, ApxDir = false,  IsHelo = true, Flightmenu = DcsClient.iCommandsequences.showflight1 ,Singlehotkey = false,  Havedial = false, Theme = "NATO",   SpeechAlias = "Kiowa Warrior"  } },
                {"CH-47F",    new DCSmodule() { Id ="CH-47F",Name = "CH-47F", Alias = "Chinook", ProOnly = true, IsFC = false, ApxWpn = true, ApxDir = false,  IsHelo = true, Flightmenu = DcsClient.iCommandsequences.showflight1 ,Singlehotkey = false,  Havedial = false, Theme = "Afghan",   SpeechAlias = "Chinook"  } },
                

                // community Mods
                {"A-4E-C",      new DCSmodule() { Id ="A-4E-C", Name = "A-4", Alias = "Skyhawk", ProOnly = true,  IsFC = true, ApxWpn = true, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight2 ,Singlehotkey = false,  Havedial = false, Theme = "Fallon",      SpeechAlias = "Sky Hawk" } },
                {"F-22A",       new DCSmodule() { Id ="F-22A", Name = "F-22A", Alias = "Raptor", ProOnly = true, IsFC = false,  ApxWpn = true, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight1 ,Singlehotkey = false, Havedial = false, Theme = "NATO",    SpeechAlias = "F Twenty Two" } },
                {"UH-60L",      new DCSmodule() { Id ="UH-60L",Name = "UH-60L",  Alias = "Blackhawk",     ProOnly = true,  IsFC = false, ApxWpn = true, ApxDir = false,  IsHelo = true, Flightmenu = DcsClient.iCommandsequences.showflight2 ,Singlehotkey = false,  Havedial = true, Theme = "NATO",    SpeechAlias = "Black Hawk"  } },
                {"Hercules",    new DCSmodule() { Id ="Hercules", Name = "C-130", Alias = "Hurcules", ProOnly = true, IsFC = false,  ApxWpn = true, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight1 ,Singlehotkey = false, Havedial = false, Theme = "NATO",    SpeechAlias = "See One Thirty" } },
                {"T-45",        new DCSmodule() { Id ="T-45", Name = "T-45", Alias = "Goshawk", ProOnly = true, IsFC = false,  ApxWpn = false, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight1 ,Singlehotkey = false, Havedial = false, Theme = "NATO",    SpeechAlias = "Tee Forty Five" } },
                {"A-29B",       new DCSmodule() { Id ="A-29B", Name = "A-29B", Alias = "Super Tucano", ProOnly = true, IsFC = true,  ApxWpn = false, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight1 ,Singlehotkey = false, Havedial = false, Theme = "NATO",    SpeechAlias = "A.. Twenty Nine" } }, // Not inflight comms in mod yet
                {"F-16I",       new DCSmodule() { Id ="F-16I", Name = "F-16I", Alias = "Sufa", ProOnly = true, IsFC = false,  ApxWpn = true, ApxDir = true,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight1 ,Singlehotkey = false, Havedial = false, Theme = "NATO",    SpeechAlias = "F Sixteen eye" } },
                {"F-16D_50",    new DCSmodule() { Id ="F-16D_50",Name = "F-16D",  Alias = "BLK50 Viper",       ProOnly = true,  IsFC = false, ApxWpn = false, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight1 ,Singlehotkey = false,  Havedial = false, Theme = "NATO",    SpeechAlias = "Viper"  } },// IDF Mod Start
                {"F-16D_50_NS", new DCSmodule() { Id ="F-16D_50_NS",Name = "F-16D",  Alias = "BLK50 Viper",       ProOnly = true,  IsFC = false, ApxWpn = false, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight1 ,Singlehotkey = false,  Havedial = false, Theme = "NATO",    SpeechAlias = "Viper"  } },//
                {"F-16D_52_NS", new DCSmodule() { Id ="F-16D_52_NS",Name = "F-16D",  Alias = "BLK52 Viper",       ProOnly = true,  IsFC = false, ApxWpn = false, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight1 ,Singlehotkey = false,  Havedial = false, Theme = "NATO",    SpeechAlias = "Viper"  } },//
                {"F-16D_52",    new DCSmodule() { Id ="F-16D_52",Name = "F-16D",  Alias = "BLK52 Viper",       ProOnly = true,  IsFC = false, ApxWpn = false, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight1 ,Singlehotkey = false,  Havedial = false, Theme = "NATO",    SpeechAlias = "Viper"  } },//
                {"F-16D_Barak_40",new DCSmodule() { Id ="F-16D_Barak_40",Name = "F-16D",  Alias = "Barak 40",       ProOnly = true,  IsFC = false, ApxWpn = false, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight1 ,Singlehotkey = false,  Havedial = false, Theme = "NATO",    SpeechAlias = "Barak"  } },//
                {"F-16D_Barak_30",new DCSmodule() { Id ="F-16D_Barak_30",Name = "F-16D",  Alias = "Barak 30",       ProOnly = true,  IsFC = false, ApxWpn = false, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight1 ,Singlehotkey = false,  Havedial = false, Theme = "NATO",    SpeechAlias = "Barak"  } },// IDF Mod End
                {"FA-18E",new DCSmodule() { Id ="FA-18E",Name = "FA-18E",  Alias = "Super Hornet",       ProOnly = true,  IsFC = false, ApxWpn = false, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight1 ,Singlehotkey = false,  Havedial = false, Theme = "Fallon",    SpeechAlias = "Super Hornet"  } },// SH Mod Start
                {"FA-18F",new DCSmodule() { Id ="FA-18F",Name = "FA-18F",  Alias = "Super Hornet",       ProOnly = true,  IsFC = false, ApxWpn = false, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight1 ,Singlehotkey = false,  Havedial = false, Theme = "Fallon",    SpeechAlias = "Super Hornet"  } },//
                {"EA-18G",new DCSmodule() { Id ="EA-18G",Name = "EA-18G",  Alias = "Growler",       ProOnly = true,  IsFC = false, ApxWpn = false, ApxDir = false,  IsHelo = false, Flightmenu = DcsClient.iCommandsequences.showflight1 ,Singlehotkey = false,  Havedial = false, Theme = "Afghan",    SpeechAlias = "Growler"  } },// SH Mod End
            };
        }
    }
}
