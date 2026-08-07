using System;
using System.Collections.Generic;
using VAICOM.Extensions.WorldAudio;
using VAICOM.Servers;
using VAICOM.Static;

namespace VAICOM
{
    namespace Extensions
    {
        namespace RIO
        {

            public partial class helper
            {

                public static Dictionary<string, DeviceAction> AGweaponsstate = new Dictionary<string, DeviceAction>()
                {

                    {"Mk81",        null }, //DeviceActionsLibrary.RIO.Atom_J_VOID  },
                    {"Mk82",        null }, //DeviceActionsLibrary.RIO.Atom_J_VOID  },
                    {"Mk83",        null }, //DeviceActionsLibrary.RIO.Atom_J_VOID  },
                    {"Mk84",        null }, //DeviceActionsLibrary.RIO.Atom_J_VOID  },
                    {"Zuni",        null }, //DeviceActionsLibrary.RIO.Atom_J_VOID  },
                    {"GBU10",       null }, //DeviceActionsLibrary.RIO.Atom_J_VOID  },
                    {"GBU12",       null }, //DeviceActionsLibrary.RIO.Atom_J_VOID  },
                    {"GBU16",       null }, // DeviceActionsLibrary.RIO.Atom_J_VOID  },
                    {"GBU24",       null }, //DeviceActionsLibrary.RIO.Atom_J_VOID  },
                    {"GBU31",       null }, //DeviceActionsLibrary.RIO.Atom_J_VOID  },
                    {"GBU38",       null }, //DeviceActionsLibrary.RIO.Atom_J_VOID  },
                    {"Mk20",        null }, //DeviceActionsLibrary.RIO.Atom_J_VOID  },
                    {"LUU2",        null }, //DeviceActionsLibrary.RIO.Atom_J_VOID  },
                    {"BDU33",       null }, //DeviceActionsLibrary.RIO.Atom_J_VOID  },
                    {"Mk82A",       null }, //DeviceActionsLibrary.RIO.Atom_J_VOID  },
                    {"Mk82SE",      null }, //DeviceActionsLibrary.RIO.Atom_J_VOID  },
                    {"TALD",        null }, //DeviceActionsLibrary.RIO.Atom_J_VOID  },
                    {"default",     null }, //DeviceActionsLibrary.RIO.Atom_J_VOID  },

                };

                private static readonly string[] AGweaponsmenuorder = new string[]
                {
                    "Mk81",
                    "Mk82",
                    "Mk83",
                    "Mk84",
                    "Zuni",
                    "GBU10",
                    "GBU12",
                    "GBU16",
                    "GBU24",
                    "GBU31",
                    "GBU38",
                    "Mk20",
                    "LUU2",
                    "BDU33",
                    "Mk82A",
                    "Mk82SE",
                    "TALD"
                };

                public static DeviceAction GetAtom(int num)
                {
                    DeviceAction value = DeviceActionsLibrary.RIO.Atom_J_VOID;

                    if (num.Equals(1)) { return DeviceActionsLibrary.RIO.Atom_J_MENU_OPTION_1; }
                    if (num.Equals(2)) { return DeviceActionsLibrary.RIO.Atom_J_MENU_OPTION_2; }
                    if (num.Equals(3)) { return DeviceActionsLibrary.RIO.Atom_J_MENU_OPTION_3; }
                    if (num.Equals(4)) { return DeviceActionsLibrary.RIO.Atom_J_MENU_OPTION_4; }
                    if (num.Equals(5)) { return DeviceActionsLibrary.RIO.Atom_J_MENU_OPTION_5; }
                    if (num.Equals(6)) { return DeviceActionsLibrary.RIO.Atom_J_MENU_OPTION_6; }
                    if (num.Equals(7)) { return DeviceActionsLibrary.RIO.Atom_J_MENU_OPTION_7; }
                    if (num.Equals(8)) { return DeviceActionsLibrary.RIO.Atom_J_MENU_OPTION_8; }

                    return value;
                }

                public static void resetAGweapons()
                {
                    AGweaponsstate = new Dictionary<string, DeviceAction>();


                    AGweaponsstate["Mk81"] = DeviceActionsLibrary.RIO.Atom_J_VOID;
                    AGweaponsstate["Mk82"] = DeviceActionsLibrary.RIO.Atom_J_VOID;
                    AGweaponsstate["Mk83"] = DeviceActionsLibrary.RIO.Atom_J_VOID;
                    AGweaponsstate["Mk84"] = DeviceActionsLibrary.RIO.Atom_J_VOID;
                    AGweaponsstate["Zuni"] = DeviceActionsLibrary.RIO.Atom_J_VOID;
                    AGweaponsstate["GBU10"] = DeviceActionsLibrary.RIO.Atom_J_VOID;
                    AGweaponsstate["GBU12"] = DeviceActionsLibrary.RIO.Atom_J_VOID;
                    AGweaponsstate["GBU16"] = DeviceActionsLibrary.RIO.Atom_J_VOID;
                    AGweaponsstate["GBU24"] = DeviceActionsLibrary.RIO.Atom_J_VOID;
                    AGweaponsstate["GBU31"] = DeviceActionsLibrary.RIO.Atom_J_VOID;
                    AGweaponsstate["GBU38"] = DeviceActionsLibrary.RIO.Atom_J_VOID;
                    AGweaponsstate["Mk20"] = DeviceActionsLibrary.RIO.Atom_J_VOID;
                    AGweaponsstate["LUU2"] = DeviceActionsLibrary.RIO.Atom_J_VOID;
                    AGweaponsstate["BDU33"] = DeviceActionsLibrary.RIO.Atom_J_VOID;
                    AGweaponsstate["Mk82A"] = DeviceActionsLibrary.RIO.Atom_J_VOID;
                    AGweaponsstate["Mk82SE"] = DeviceActionsLibrary.RIO.Atom_J_VOID;
                    AGweaponsstate["TALD"] = DeviceActionsLibrary.RIO.Atom_J_VOID;
                    AGweaponsstate["default"] = DeviceActionsLibrary.RIO.Atom_J_VOID;


                }

                public static string extractweapon(string classid)
                {
                    string result = "";

                    if (classid.Contains("MK") && classid.Contains("81")) { return "Mk81"; }
                    if (classid.Contains("MK") && classid.Contains("82"))
                    {
                        if (classid.ToLower().Contains("air"))
                        {
                            return "Mk82A";
                        }
                        if (classid.ToLower().Contains("se"))
                        {
                            return "Mk82SE";
                        }
                        return "Mk82";
                    }
                    if (classid.Contains("MK") && classid.Contains("83")) { return "Mk83"; }
                    if (classid.Contains("MK") && classid.Contains("84")) { return "Mk84"; }
                    if (classid.Contains("LAU") && classid.Contains("10")) { return "Zuni"; }
                    if (classid.Contains("GBU") && classid.Contains("10")) { return "GBU10"; }
                    if (classid.Contains("GBU") && classid.Contains("12")) { return "GBU12"; }
                    if (classid.Contains("GBU") && classid.Contains("16")) { return "GBU16"; }
                    if (classid.Contains("GBU") && classid.Contains("24")) { return "GBU24"; }
                    if (classid.Contains("GBU") && classid.Contains("31")) { return "GBU31"; }
                    if (classid.Contains("GBU") && classid.Contains("38")) { return "GBU38"; }
                    if (classid.Contains("MK") && classid.Contains("20")) { return "Mk20"; }
                    if (classid.Contains("SUU") && classid.Contains("25")) { return "LUU2"; }
                    if (classid.Contains("BDU")) { return "BDU33"; }
                    if (classid.Contains("ADM141")) { return "TALD"; }

                    return result;
                }

                public static bool havetank()
                {
                    bool result = false;

                    foreach (Server.payloadstation station in State.currentstate.payload.Stations)
                    {
                        if (station.CLSID.Contains("300gal") && station.count > 0)
                        {
                            return true;
                        }
                    }

                    return result;
                }

                public static void getAGweaponsstate()
                {
                    try
                    {

                        resetAGweapons();
                        int counter = 0;

                        foreach (string weapon in AGweaponsmenuorder)
                        {
                            if (AGweaponsstate[weapon].Equals(DeviceActionsLibrary.RIO.Atom_J_VOID)) // wpn not identified yet
                            {
                                foreach (Server.payloadstation station in State.currentstate.payload.Stations)
                                {
                                    string wpn = extractweapon(station.CLSID);
                                    if (weapon.Equals(wpn) && station.count > 0)
                                    {
                                        Log.Write("found new AG weapon!" + wpn, Colors.Inline);
                                        counter += 1;
                                        AGweaponsstate[wpn] = GetAtom(counter);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                        Log.Write("could not update weapons state.", Colors.Inline);
                    }
                }

                public static void getAGweaponsstate_OLD()
                {

                    resetAGweapons();
                    int counter = 0;

                    foreach (Server.payloadstation station in State.currentstate.payload.Stations)
                    {
                        string wpn = extractweapon(station.CLSID);
                        if (AGweaponsstate.ContainsKey(wpn) && station.count > 0)
                        {
                            if (AGweaponsstate[wpn].Equals(DeviceActionsLibrary.RIO.Atom_J_VOID))
                            {
                                Log.Write("found new AG weapon!" + wpn, Colors.Inline);
                                counter += 1;
                                AGweaponsstate[wpn] = GetAtom(counter);
                            }
                        }
                    }
                }

                public static Server.Vector diffvector()
                {
                    Server.Vector dp = new Server.Vector();

                    Server.Vector cam = State.currentstate.cpos.loc;
                    Server.Vector acbody = State.currentstate.bpos;

                    dp.x = cam.x - acbody.x;
                    dp.y = cam.y - acbody.y;
                    dp.z = cam.z - acbody.z;

                    return dp;
                }

                public static Server.Vector shiftedcamvector(double factor)
                {
                    Server.Vector campos = new Server.Vector();

                    Server.Vector acbody = State.currentstate.bpos;
                    Server.Vector dp = diffvector();

                    dp.x = factor * dp.x;
                    dp.y = factor * dp.y;
                    dp.z = factor * dp.z;

                    campos.x = acbody.x + dp.x;
                    campos.y = acbody.y + dp.y;
                    campos.z = acbody.z + dp.z;

                    return campos;
                }

                public static int seatposition()
                {
                    int seatpos = 1; // default, 2 for RIO seat

                    Server.Vector dp = diffvector();

                    double length = Math.Sqrt((dp.x * dp.x) + (dp.y * dp.y) + (dp.z * dp.z));

                    //Log.Write("Length = " + length, Colors.Inline);

                    if (length < 6.25)
                    {
                        seatpos = 2;
                        //Log.Write("RIO!", Colors.Inline);
                    }
                    else
                    {
                        seatpos = 1;
                        //Log.Write("Pilot!", Colors.Inline);
                    }
                    return seatpos;
                }
            }

            public class riospeech
            {

                public static void riospeakrandom(int type)
                {

                    if (tables.menustate[tables.menucats.CONTR_TALK].Equals(tables.menustates.No_Talk))
                    {
                        return;
                    }

                    // 1= OK
                    // 2= NOK
                    // 3 =hmm

                    List<string> filelist = new List<string>();

                    //Random rnd0 = new Random();
                    int checkmax0 = 2;
                    int dice0 = State.random1.Next(1, 1 + checkmax0);
                    Log.Write("random = " + dice0, Colors.Inline);
                    string filename;

                    switch (dice0)
                    {
                        case 1:
                            filename = "";
                            if (type.Equals(1))
                            {
                                filename = "misc/roger";
                            }
                            if (type.Equals(2))
                            {
                                filename = "misc/unable";
                            }
                            if (type.Equals(3))
                            {
                                filename = "misc/uhm";
                            }
                            break;

                        default:
                            filename = "";
                            if (type.Equals(1))
                            {
                                filename = "misc/copy";
                            }
                            if (type.Equals(2))
                            {
                                filename = "misc/cantdo";
                            }
                            if (type.Equals(3))
                            {
                                filename = "misc/uhm"; // nothing
                            }
                            break;

                    }

                    //now add random extension (1-17)
                    //Random rnd = new Random();
                    int checkmax = 17;
                    int dice = State.random2.Next(1, 1 + checkmax);
                    string append = dice.ToString();

                    filelist.Add(filename + append);

                    // filenames: wilco1.ogg 1-17 unable1.ogg 1-27 cantdo1.ogg 1-17.ogg copy1.ogg 1-17

                    string basepath = "Mods/aircraft/F14/Sounds/Jester/"; //check path to audio files 
                    int delay = 2; // 0,5sec

                    // get random samples to play with some delay. L<> = single file.
                    Processor.InsertFilesForPlayback(basepath, filelist, delay);
                }

                //YES
                public static List<string> riowavfiles_OK = new List<string>()
                {
                    "",
                };

                // NO
                public static List<string> riowavfiles_NOK = new List<string>()
                {
                    "",
                };

            }

            public class menuhelper
            {
                public static Dictionary<string, string> optionhints = new Dictionary<string, string>()
                {

                    //Radio

                    // Radar
                    { "wMsgJ_RDR_STT_LOCK"          ,"\"Track Single [ Contact Ahead | Bogey Ahead | Friendly Ahead | First | Target [1-8]]\"\nSelect STT Target to track" },
                    { "wMsgJ_RDR_STT_TWS_TGT_NUM"   ,"\"Track Single Target [ Ahead | First | [1-8] ]\"\nSelect STT Target to track" }, // <-- disabled
                    { "wMsgJ_RDR_SCAN_ELEV"         ,"\"Scan [ Auto | <Middle> [High | Low] ]\"\nSelect Radar Scan Elevation" },
                    { "wMsgJ_RDR_SCAN_AZ"           ,"\"Scan [ Auto | <Center> [Left | Right] ]\"\nSelect Radar Scan Azimuth"   },
                    { "wMsgJ_RDR_SCAN_DIST"         ,"\"TID Range [ Auto | 25 | 50 | 100 | 200 | 400 ]\"\nSelect TID Range"   },
                    { "wMsgJ_RDR_STAB"              ,"\"Stabilize [ 15 Seconds | 30 Seconds | 1 Minute | 2 Minutes | Hold | Ground ]\"\nStabilize radar for time period."   },
                    { "wMsgJ_RDR_MODE"              ,"\"Radar Mode [ Automatic | TWS | RWS ] \"\nSet radar scan / search mode" },
                    // Weapon
                    { "wMsgJ_WPN_AG_SORDN"         ,"\"Select [ Mk81s | Mk82s | Mk83s | Mk84s | Zunis | Paveways | Rockeyes | LUUs | BDUs | TALD | JDAM | 31s | 38s ]\"\nSelect AG Weapon."   },
                    { "wMsgJ_WPN_AG_RIP"           ,"\"Set Ripple [ Quantity.. | Time.. | Distance.. ]\"\nSet Ripple Parameters"   },
                    { "wMsgJ_WPN_AG_RIP_QTY"       ,"\"Set Ripple Quantity [ 2 | 3 | 4 | 6 | 8 | 16 | 28 ]\"\nSet Ripple Quantity parameter"   },
                    { "wMsgJ_WPN_AG_RIP_TIME"      ,"\"Set Ripple Time [ 10 | 20 | 50 | 100 | 200 | 500 | 990 ]\"\nSet Ripple Timer parameter (in ms)"   },
                    { "wMsgJ_WPN_AG_RIP_DIST"      ,"\"Set Ripple Distance [ 5 | 10 | 25 | 50 | 100 | 200 | 400 ]\"\nSet Ripple Spread parameter"   },
                    { "wMsgJ_WPN_AG_SETFUSE"       ,"\"Set Fuse [ Nose | Nose Tail | Safe ]\"\nSet AG weapon fuse parameter"   },
                    { "wMsgJ_WPN_AG_STN"           ,"\"Select Stations [ 1 8 | 2 7 | 3 4 5 6 | 3 6 | 4 5 ]\"\nSelect hardpoints"   },
                    
                    // Radio
                    { "wMsgJ_RAD_182_MODE"         ,"\"Radio Mode [ Off | TR | TRG | DF | Test | AM | FM ]\"\nSet AN/ARC-182 Mode"   },
                   
                    // Navigate + TACAN
                    { "wMsgJ_UTIL_NAV_MAP_SPT"      ,"\"Navigate [ Steerpoint [1-3] | Fixed Point | Initial Point | Target | Home Base ]\"\nNavigate to steerpoint"   },
                    { "wMsgJ_UTIL_NAV_REST_MORE"    ,"\"Restore [ Steerpoint [1-3] | Fixed Point | Initial Point | Target | Home Base | Defense Point | Hostile Zone ]\"\nRestore steerpoint"   },
                    { "wMsgJ_RAD_TCN_MODE"          ,"\"TACAN Mode [ Off | Rec | Transmit | AA | Beacon ]\"\nSelect TACAN operation mode"   },
                    { "wMsgJ_RAD_TCN_SEL_GND_STN"   ,"\"TACAN [ Ground Station ID ] (e.g. TACAN Oscar Alfa Lima)\"\nSelect TACAN Ground Beacon"   },
                    { "wMsgJ_RAD_TCN_TUNE_TAC"      ,"\"TACAN Tune [ Stennis | Washington | Roosevelt | Lincoln | Truman | Forrestal | Arco | Shell | Texaco ] or [ X-Ray | Yankee [0-9][0-9][0-9] ]\"\nSelect TACAN TAC unit (Carrier, Air Refuel) or specific channel."   },             

                    // Defensive
                    { "wMsgJ_DEF_RWR_DSP_TYP"       ,"\"Display [ Airborne | Normal | AAA | Friendly | Unknown ]\"\nSelect RWR display mode" },
                    { "wMsgJ_DEF_CMS_CTL_ORD"       ,"\"Dispense Order [ Chaff | Flare ] [ Program | Single | Tight ] \"\nSet sequence order for chaff/flares countermeasures" },
                    { "wMsgJ_DEF_FLR_PGM"           ,"\"Flares Program [ 2x2 | 4x2 | 10x2 | 4x6 | 8x6 | 10x6 | 6x10 | 10x10 ]\"\nSet Flares countermeasures program (qty by sec)" },
                    { "wMsgJ_DEF_CHF_PGM"           ,"\"Chaff Program [1-8]\"\nSet Chaff countermeasures program number" },
                    { "wMsgJ_DEF_CMS_MOD"           ,"\"Countermeasures [ Off | Manual | Auto ] \"\nSets CMS operation mode" },
                    { "wMsgJ_DEF_FLR_MOD"           ,"\"Flares Mode [ Pilot | Normal | Multi ] \"\nSets Flares dispense mode" },

                    //Datalink
                    { "wMsgJ_RAD_DL_SET_MODE"       ,"\"Link Mode [ Tactical| Fighter | Off ] \"\nSets Datalink mode" },
                    { "wMsgJ_RAD_DL_SET_FREQ_PRST"  ,"\"Link Preset [1-8] \"\nSelects Datalink frequency preset (1 = default)" },
                    { "wMsgJ_RAD_DL_SET_HOST"       ,"\"Link Host [ Stennis | Washington | Roosevelt | Lincoln | Ticonderoga | Arleigh Burke | Truman | Forrestal | Darkstar | Focus | Magic | Overlord | Wizard ]\"\nSelect Datalink (Carrier,AWACS) host unit." },

                    //Iceman
                    { "wMsgI_ALT"                   ,"\"Go Angels [ 1 | 5 | 10 | 15 | 20 | 25 | 30 | 35 ] \"\nSets absolute altitude" },
                    { "wMsgI_ALT_CHG"               ,"\"Descent | Climb  [ 500 | 1000 | 5000 | 10000 ] \"\nSets relative altitude" },
                    { "wMsgI_SPD"                   ,"\"Slow Down | Speed Up [ 50 | 100 | 200 ] \"\nCommand speed change" },
                    { "wMsgI_DIR"                   ,"\"Heading [ Straight | North | NorthEast | East | SouthEast | South | SouthWest | West | NorthWest ] \"\nSelects heading" },
                    { "wMsgI_DIR_CHG"               ,"\"Turn [ Left | Right ] [ 5 | 10 | 30 | 45 ] \"\nCommand turn (angle)" },
                };
            }

        }
    }
}
