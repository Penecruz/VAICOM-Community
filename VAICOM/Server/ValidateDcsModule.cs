using System;
using System.Collections.Generic;
using VAICOM.FileManager;
using VAICOM.Products;
using VAICOM.Static;

namespace VAICOM
{
    namespace Servers
    {


        public static partial class Server
        {

            public static void DisplayCurrentModule()
            {
                Log.Write("Player " + State.currentstate.playerusername + " entered module " + State.currentmodule.Name + " " + State.currentmodule.Alias + ", unit callsign " + State.currentstate.playercallsign, Colors.Message);
                try
                {
                    Log.Write("Nearest ATC: " + State.currentstate.availablerecipients["ATC"][0].fullname + ".", Colors.Message);
                }
                catch
                {
                }
            }

            public static void ValidateDcsModule(bool silent)
            {

                try
                {

                    // reset to unknown

                    bool moduleresolved = false;
                    State.currentmodule = DCSmodules.LookupTable["----"];

                    // check for existing modules

                    foreach (KeyValuePair<string, DCSmodule> mod in DCSmodules.LookupTable)
                    {
                        if (State.currentstate.id.ToLower().Contains(mod.Key.ToLower()))
                        {
                            if (!silent)
                            {
                                Log.Write("Found module " + State.currentstate.id, Colors.Text);
                            }
                            State.currentmodule = mod.Value;
                            moduleresolved = true;
                        }
                    }

                    // guess any unknowns

                    if (!moduleresolved)
                    {
                        moduleresolved = SetKnownModule();
                    }

                    // still not resolved? import to table as new and store

                    if (!moduleresolved & State.activeconfig.AutoImportModules)
                    {

                        string id = State.currentstate.id;
                        string cat = State.currentstate.playerunitcat;
                        DCSmodule newmod = new DCSmodule() { Name = id, Alias = "", ProOnly = true, IsFC = false, ApxWpn = true, ApxDir = true, IsHelo = (cat == "Helicopters"), Singlehotkey = (cat == "Helicopters") };
                        DCSmodules.LookupTable.Add(id, newmod);
                        State.importeddcsmodules.Add(newmod);
                        if (!silent)
                        {
                            Log.Write("Adding new " + cat.ToLower() + " module " + id + " to database.", Colors.Text);

                        }
                        FileHandler.Database.WriteModulesToFile(true);
                        State.currentmodule = DCSmodules.LookupTable[id];
                    }

                    // module is resolved, check if allowed

                    if (!State.PRO & State.currentmodule.ProOnly)
                    {
                        if (!silent)
                        {
                            Log.Write("DCS module " + State.currentmodule.Name + " is available with PRO license only.", Colors.Warning);

                        }
                        State.blockedmodule = false;
                        UI.Playsound.Sorry();
                    }
                    else
                    {
                        if (!silent)
                        {
                            DisplayCurrentModule();
                        }
                        State.blockedmodule = false;
                    }

                }

                catch (Exception) // when error revert to default
                {
                    if (State.PRO)
                    {
                        State.currentmodule = DCSmodules.LookupTable["Module Error"];
                        if (!silent)
                        {
                            DisplayCurrentModule();
                        }
                    }

                }
            }

            public static bool SetKnownModule()
            {
                // new ones..
                // Mig-19P
                if (State.currentstate.id.ToLower().Contains("19") && State.currentstate.id.ToLower().Contains("p"))
                {
                    State.currentmodule = DCSmodules.LookupTable["MiG-19P"];
                    return true;
                }

                // JF-17
                if (State.currentstate.id.ToLower().Contains("jf") && State.currentstate.id.ToLower().Contains("17"))
                {
                    State.currentmodule = DCSmodules.LookupTable["JF-17"];
                    return true;
                }

                // I-16
                if (State.currentstate.id.ToLower().Contains("i") && State.currentstate.id.ToLower().Contains("-") && State.currentstate.id.ToLower().Contains("16"))
                {
                    State.currentmodule = DCSmodules.LookupTable["I-16"];
                    return true;
                }

                // Fw-190A8
                if ((State.currentstate.id.ToLower().Contains("fw") & State.currentstate.id.ToLower().Contains("190")) && (State.currentstate.id.ToLower().Contains("a")))
                {
                    State.currentmodule = DCSmodules.LookupTable["Fw-190A8"];
                    return true;
                }

                // P-47D
                if ((State.currentstate.id.ToLower().Contains("p") & State.currentstate.id.ToLower().Contains("47")))
                {
                    State.currentmodule = DCSmodules.LookupTable["P-47D"];
                    return true;
                }

                // Christen Eagle
                if ((State.currentstate.id.ToLower().Contains("christen") || State.currentstate.id.ToLower().Contains("ce")))
                {
                    State.currentmodule = DCSmodules.LookupTable["CE-2"];
                    return true;
                }

                // F-16C Viper
                if (State.currentstate.id.ToLower().Contains("f") && State.currentstate.id.ToLower().Contains("16") && State.currentstate.id.ToLower().Contains("c"))
                {
                    State.currentmodule = DCSmodules.LookupTable["F-16C_50"];
                    return true;
                }
                // CA
                if (State.currentstate.id.ToLower().Contains("commander"))
                {
                    State.currentmodule = DCSmodules.LookupTable["CA"];
                    return true;
                }
                // Spitfire
                if (State.currentstate.id.ToLower().Contains("spitfire"))
                {
                    State.currentmodule = DCSmodules.LookupTable["Spitfire"];
                    return true;
                }
                // Mosquito
                if (State.currentstate.id.ToLower().Contains("mosquito"))
                {
                    State.currentmodule = DCSmodules.LookupTable["Mosquito"];
                    return true;
                }
                // Mig-29
                if (State.currentstate.id.ToLower().Contains("-29") || (State.currentstate.id.ToLower().Contains("fulcrum")))
                {
                    State.currentmodule = DCSmodules.LookupTable["MiG-29"];
                    return true;
                }
                // J-11A
                if (State.currentstate.id.ToLower().Contains("j") && (State.currentstate.id.ToLower().Contains("11")))
                {
                    State.currentmodule = DCSmodules.LookupTable["J-11A"];
                    return true;
                }
                // Hornet
                if ((State.currentstate.id.ToLower().Contains("f") & State.currentstate.id.ToLower().Contains("18")) || (State.currentstate.id.ToLower().Contains("hornet")))
                {
                    State.currentmodule = DCSmodules.LookupTable["FA-18C"];
                    return true;
                }
                // Tomcat
                if ((State.currentstate.id.ToLower().Contains("f") & State.currentstate.id.ToLower().Contains("14")) || (State.currentstate.id.ToLower().Contains("tomcat")))
                {
                    State.currentmodule = DCSmodules.LookupTable["F-14AB"];
                    return true;
                }
                // Gazelle
                if (State.currentstate.id.ToLower().Contains("342") || (State.currentstate.id.ToLower().Contains("gazelle")))
                {
                    State.currentmodule = DCSmodules.LookupTable["SA342M"];
                    return true;
                }
                //Viggen
                if (State.currentstate.id.ToLower().Contains("37") || (State.currentstate.id.ToLower().Contains("viggen")))
                {
                    State.currentmodule = DCSmodules.LookupTable["AJS-37"];
                    return true;
                }
                //F-5E_FC Flaming Cliffs version
                if ((State.currentstate.id.ToLower().Contains("f") & State.currentstate.id.ToLower().Contains("5E-3_FC"))) //|| (State.currentstate.id.ToLower().Contains("FC"))) //try removing the or statement on both versions??
                {
                    State.currentmodule = DCSmodules.LookupTable["F-5E_FC"];
                    return true;
                }
                //F-5E
                if ((State.currentstate.id.ToLower().Contains("f") & State.currentstate.id.ToLower().Contains("5E-3"))) //|| (State.currentstate.id.ToLower().Contains("tiger")))
                {
                    State.currentmodule = DCSmodules.LookupTable["F-5E-3"];
                    return true;
                }
                
                //Aviojet
                if (State.currentstate.id.ToLower().Contains("101") || (State.currentstate.id.ToLower().Contains("avio")))
                {
                    State.currentmodule = DCSmodules.LookupTable["C-101"];
                    return true;
                }
                //Kurfurst
                if ((State.currentstate.id.ToLower().Contains("bf") & State.currentstate.id.ToLower().Contains("109")) || (State.currentstate.id.ToLower().Contains("kurfurst")))
                {
                    State.currentmodule = DCSmodules.LookupTable["Bf-109"];
                    return true;
                }
                //
                if ((State.currentstate.id.ToLower().Contains("fw") & State.currentstate.id.ToLower().Contains("190")) && (State.currentstate.id.ToLower().Contains("d")))
                {
                    State.currentmodule = DCSmodules.LookupTable["Fw-190"];
                    return true;
                }
                // Flanker-D
                if ((State.currentstate.id.ToLower().Contains("su") & State.currentstate.id.ToLower().Contains("33")) || (State.currentstate.id.ToLower().Contains("flanker")))
                {
                    State.currentmodule = DCSmodules.LookupTable["Su-33"];
                    return true;
                }
                // Harrier
                if (State.currentstate.id.ToLower().Contains("av") & (State.currentstate.id.ToLower().Contains("8b")))
                {
                    State.currentmodule = DCSmodules.LookupTable["AV-8B"];
                    return true;
                }
                // Mustang P51
                if (State.currentstate.id.ToLower().Contains("p") & (State.currentstate.id.ToLower().Contains("51")))
                {
                    State.currentmodule = DCSmodules.LookupTable["P-51D"];
                    return true;
                }
                //Skyhawk
                if (State.currentstate.id.ToLower().Contains("a4e") || (State.currentstate.id.ToLower().Contains("skyhawk")))
                {
                    State.currentmodule = DCSmodules.LookupTable["A-4E-C"];
                    return true;
                }
                //F-15E Strike Eagle
                if (State.currentstate.id.ToLower().Contains("f") & State.currentstate.id.ToLower().Contains("15") && (State.currentstate.id.ToLower().Contains("e")))
                {
                    State.currentmodule = DCSmodules.LookupTable["F-15ESE"];
                    return true;
                }
                //F-22 Raptor
                if (State.currentstate.id.ToLower().Contains("f") & State.currentstate.id.ToLower().Contains("22") && (State.currentstate.id.ToLower().Contains("a")))
                {
                    State.currentmodule = DCSmodules.LookupTable["F-22A"];
                    return true;
                }
                //F-4E Phantom II
                if (State.currentstate.id.ToLower().Contains("f") & State.currentstate.id.ToLower().Contains("4") && (State.currentstate.id.ToLower().Contains("e")))
                {
                    State.currentmodule = DCSmodules.LookupTable["F-4E-45MC"];
                    return true;
                }
                //OH-58D Kiowa Warrier
                if (State.currentstate.id.ToLower().Contains("OH") & State.currentstate.id.ToLower().Contains("58") && (State.currentstate.id.ToLower().Contains("d")))
                {
                    State.currentmodule = DCSmodules.LookupTable["OH58D"];
                    return true;
                }
                //Goshawk
                // if (State.currentstate.id.ToLower().Contains("t") & (State.currentstate.id.ToLower().Contains("45")))
                //{
                // State.currentmodule = DCSmodules.LookupTable["T-45"];
                // return true;     
                //}

                return false;

            }

        }
    }

}

