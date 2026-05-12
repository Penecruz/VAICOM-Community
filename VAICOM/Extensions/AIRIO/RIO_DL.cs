using System;
using System.Collections.Generic;
using VAICOM.Static;

namespace VAICOM
{
    namespace Extensions
    {
        namespace RIO
        {

            public partial class helper
            {

                public static Dictionary<string, DeviceAction> DLstate = new Dictionary<string, DeviceAction>()
                {
                    {"Stennis",    null }, //DeviceActionsLibrary.RIO.Atom_J_VOID  },
                    {"Washington", null }, //DeviceActionsLibrary.RIO.Atom_J_VOID  },
                    {"Roosevelt",  null }, //DeviceActionsLibrary.RIO.Atom_J_VOID  },
                    {"Lincoln",    null }, //DeviceActionsLibrary.RIO.Atom_J_VOID  },
                    {"Truman",     null }, //DeviceActionsLibrary.RIO.Atom_J_VOID  },
                    {"Forrestal",  null }, //DeviceActionsLibrary.RIO.Atom_J_VOID  },
                    {"Ticonderoga",null }, //DeviceActionsLibrary.RIO.Atom_J_VOID  },
                    {"ArleighBurke",null }, //DeviceActionsLibrary.RIO.Atom_J_VOID  },
                    {"Darkstar",   null }, //DeviceActionsLibrary.RIO.Atom_J_VOID  },
                    {"Focus",      null }, //DeviceActionsLibrary.RIO.Atom_J_VOID  },
                    {"Magic",      null }, //DeviceActionsLibrary.RIO.Atom_J_VOID  },
                    {"Overlord",   null }, //DeviceActionsLibrary.RIO.Atom_J_VOID  },
                    {"Wizard",     null }, // DeviceActionsLibrary.RIO.Atom_J_VOID  },

                };

                public static void resetDLstate()
                {
                    DLstate = new Dictionary<string, DeviceAction>();

                    DLstate["Stennis"] = DeviceActionsLibrary.RIO.Atom_J_VOID;
                    DLstate["Washington"] = DeviceActionsLibrary.RIO.Atom_J_VOID;
                    DLstate["Roosevelt"] = DeviceActionsLibrary.RIO.Atom_J_VOID;
                    DLstate["Lincoln"] = DeviceActionsLibrary.RIO.Atom_J_VOID;
                    DLstate["Truman"] = DeviceActionsLibrary.RIO.Atom_J_VOID;
                    DLstate["Forrestal"] = DeviceActionsLibrary.RIO.Atom_J_VOID;
                    DLstate["Ticonderoga"] = DeviceActionsLibrary.RIO.Atom_J_VOID;
                    DLstate["ArleighBurke"] = DeviceActionsLibrary.RIO.Atom_J_VOID;
                    DLstate["Darkstar"] = DeviceActionsLibrary.RIO.Atom_J_VOID;
                    DLstate["Focus"] = DeviceActionsLibrary.RIO.Atom_J_VOID;
                    DLstate["Magic"] = DeviceActionsLibrary.RIO.Atom_J_VOID;
                    DLstate["Overlord"] = DeviceActionsLibrary.RIO.Atom_J_VOID;
                    DLstate["Wizard"] = DeviceActionsLibrary.RIO.Atom_J_VOID;
                }

                public static string extractDLunit(string unitcallsign, string fullname)
                {
                    string result = "";

                    string callsign = unitcallsign ?? string.Empty;
                    string name = fullname ?? string.Empty;
                    string unitName = callsign + " " + name;

                    if (unitName.IndexOf("Stennis", StringComparison.OrdinalIgnoreCase) >= 0) { return "Stennis"; } //Pene validate?
                    if (unitName.IndexOf("Washington", StringComparison.OrdinalIgnoreCase) >= 0) { return "Washington"; }
                    if (unitName.IndexOf("Roosevelt", StringComparison.OrdinalIgnoreCase) >= 0) { return "Roosevelt"; }
                    if (unitName.IndexOf("Lincoln", StringComparison.OrdinalIgnoreCase) >= 0) { return "Lincoln"; }
                    if (unitName.IndexOf("Truman", StringComparison.OrdinalIgnoreCase) >= 0) { return "Truman"; }
                    if (unitName.IndexOf("Forrestal", StringComparison.OrdinalIgnoreCase) >= 0) { return "Forrestal"; } //Add USS Forrestal
                    if (unitName.IndexOf("Ticonderoga", StringComparison.OrdinalIgnoreCase) >= 0) { return "Ticonderoga"; }
                    if (unitName.IndexOf("Arleigh Burke", StringComparison.OrdinalIgnoreCase) >= 0) { return "ArleighBurke"; }
                    if (unitName.IndexOf("Darkstar", StringComparison.OrdinalIgnoreCase) >= 0) { return "Darkstar"; }
                    if (unitName.IndexOf("Focus", StringComparison.OrdinalIgnoreCase) >= 0) { return "Focus"; }
                    if (unitName.IndexOf("Magic", StringComparison.OrdinalIgnoreCase) >= 0) { return "Magic"; }
                    if (unitName.IndexOf("Overlord", StringComparison.OrdinalIgnoreCase) >= 0) { return "Overlord"; }
                    if (unitName.IndexOf("Wizard", StringComparison.OrdinalIgnoreCase) >= 0) { return "Wizard"; }

                    return result;
                }

                public static void getDLstate()
                {
                    try
                    {
                        resetDLstate();
                        int counter = 0;

                        foreach (Servers.Server.DcsUnit unit in State.currentstate.DLunits)
                        {
                            string callsign = extractDLunit(unit.callsign, unit.fullname);

                            if (string.IsNullOrEmpty(callsign))
                            {
                                continue;
                            }

                            if (!DLstate.ContainsKey(callsign))
                            {
                                continue;
                            }

                            if (!DLstate[callsign].Equals(DeviceActionsLibrary.RIO.Atom_J_VOID))
                            {
                                continue;
                            }

                            if (counter >= 8)
                            {
                                break;
                            }

                            try
                            {
                                counter += 1;
                                DLstate[callsign] = GetAtom(counter);
                            }
                            catch (Exception e)
                            {
                            }

                        }
                    }
                    catch
                    {
                        Log.Write("could not update DL state.", Colors.Inline);
                    }
                }
            }
        }
    }
}
