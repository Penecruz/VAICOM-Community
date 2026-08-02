using System;
using System.Collections.Generic;
using System.Windows.Forms;
using VAICOM.Extensions.RIO;
using VAICOM.Static;

namespace VAICOM
{

    namespace Client
    {

        public partial class DcsClient
        {
            public class DeviceAction
            {
                public int device;
                public int command;
                public double value;
            }

            public static void UpdateRIOState()
            {

                try
                {
                    if (State.currentstate.riostate.ics && State.activeconfig.ICShotmic_useswitch)
                    {
                        State.activeconfig.ICShotmic = true;
                    }
                    else
                    {
                        State.activeconfig.ICShotmic = false;
                    }
                    if (State.configwindowopen && (State.configurationwindow != null))
                    {
                        State.configurationwindow.Dispatcher.BeginInvoke((MethodInvoker)delegate
                        {
                            State.configurationwindow.CheckBoxHotMic();
                            State.configurationwindow.Dictate_set_relhot_Light(State.activeconfig.ICShotmic);
                        });
                    }

                }
                catch
                {
                }

                if (State.currentstate.airborne || State.currentstate.fsmstate.ToLower().Contains("taxi"))
                {
                    tables.menustate[tables.menucats.STARTUP] = tables.menustates.Not_Starting;
                }
                else
                {
                }

                // WAKING UP? YES/NO
                //if (!State.currentstate.airborne && State.currentstate.timer < 15)
                //{
                //    //tables.menustate[tables.menucats.CREW] = tables.menustates.Crew_Asleep;
                //}
                //else
                //{
                //    //tables.menustate[tables.menucats.CREW] = tables.menustates.Crew_Awake;
                //}

                // STT locked?
                if (State.currentstate.riostate.pdstt || State.currentstate.riostate.pstt)
                {
                    tables.menustate[tables.menucats.RDR_STT] = tables.menustates.STT_locked;
                }
                else
                {
                    tables.menustate[tables.menucats.RDR_STT] = tables.menustates.Not_STT_Locked;
                }

                if (State.currentstate.cpos.type.Equals(0)) // only evaluate when internal view!
                {
                    try
                    {
                        if (helper.seatposition().Equals(2))
                        {
                            tables.menustate[tables.menucats.PLAYERSEAT] = tables.menustates.RIO;
                        }
                        else
                        {
                            tables.menustate[tables.menucats.PLAYERSEAT] = tables.menustates.Pilot;
                        }
                    }
                    catch
                    {
                    }
                }
            }

            public static bool CheckLANTIRN()
            {
                foreach (VAICOM.Servers.Server.payloadstation stat in State.currentstate.payload.Stations)
                {
                    if (stat.CLSID.Contains("LANTIRN"))
                    {
                        return true;
                    }
                }
                return false;
            }

            public static bool ManipulateCommands()
            {
                //-------------APPLY FILTERS/MANIPULATIONS BASED ON STATE -------------------------------------------------------

                if (State.currentkey["command"].Equals("wMsgJ_UTIL_CONTR_ACTIVE") || State.currentkey["command"].Equals("wMsgJ_UTIL_CONTR_INACTIVE"))
                {
                    riospeech.riospeakrandom(1); //OK
                }

                // block commands for wrong seat
                if (State.currentkey["command"].StartsWith("wMsgJ_") && tables.menustate[tables.menucats.PLAYERSEAT].Equals(tables.menustates.RIO))
                {
                    //if (!State.activeconfig.RIO_Allow_Seat_Override)
                    //{
                    State.currentmessage.dspmsg = "AIRIO : You are in Jester's seat!\n";
                    State.currentmessage.msgdur = 5;
                    State.currentmessage.extsequence = new List<Extensions.RIO.DeviceAction>(); // empty
                    return false;
                    //}
                }

                if (State.currentkey["command"].StartsWith("wMsgI_") && tables.menustate[tables.menucats.PLAYERSEAT].Equals(tables.menustates.Pilot))
                {
                    //if (!State.activeconfig.RIO_Allow_Seat_Override)
                    //{
                    State.currentmessage.dspmsg = "AIRIO : You are in Iceman's seat!\n";
                    State.currentmessage.msgdur = 5;
                    State.currentmessage.extsequence = new List<Extensions.RIO.DeviceAction>(); // empty
                    return false;
                    //}
                }

                // block commands while asleep
                //if (tables.menustate[tables.menucats.CREW] == tables.menustates.Crew_Asleep)
                //{                    
                //    State.currentmessage.dspmsg = "AIRIO : Jester's taking a nap.\n"; //not ready yet, wait 10 seconds
                //    State.currentmessage.msgdur = 2;
                //    State.currentmessage.extsequence = new List<Extensions.RIO.DeviceAction>(); // empty
                //    return false;
                //}

                // block startup commands while not starting up
                if (tables.menustate[tables.menucats.STARTUP] == tables.menustates.Not_Starting)
                {
                    if (State.currentkey["command"].Equals("wMsgJ_STRT_ABORT") || State.currentkey["command"].StartsWith("wMsgJ_STRT_INS_")) // or others..
                    {
                        State.currentmessage.dspmsg = "AIRIO: Not starting up!\n";
                        State.currentmessage.msgdur = 5;
                        State.currentmessage.extsequence = new List<Extensions.RIO.DeviceAction>(); // empty
                        return false;
                    }
                }



                // for LANTIRN
                if (State.currentkey["command"].Equals("wMsgJ_WPN_AG_UTIL_LANTIRN"))
                {
                    try
                    {
                        State.currentmessage.extsequence = new List<Extensions.RIO.DeviceAction>();
                        if (CheckLANTIRN())
                        {
                            State.currentmessage.extsequence.AddRange(DeviceActionsLibrary.Sequences.Macro.Seq_J_WPN_AG_UTIL_LANTIRN);
                            State.currentmessage.dspmsg = "VAICOM PRO : RIO | Switch LANTIRN";
                            State.currentmessage.msgdur = 5;
                            riospeech.riospeakrandom(1); //OK
                        }
                        else
                        {
                            State.currentmessage.dspmsg = "AIRIO : LANTIRN not loaded.\n";
                            State.currentmessage.msgdur = 5;
                            riospeech.riospeakrandom(2); //NOPE
                        }
                    }
                    catch
                    {
                    }
                }

                // FOR RADAR: manipulate as required

                bool inradarrange = State.currentkey["command"].StartsWith("wMsgJ_RDR");

                if (inradarrange) // radarcommand
                {

                    if (State.currentkey["command"].Equals("wMsgJ_RDR_GO_ACTIVE"))
                    {
                        tables.menustate[tables.menucats.RDR_BVR] = tables.menustates.Radar_Active;
                    }
                    if (State.currentkey["command"].Equals("wMsgJ_RDR_GO_SILENT"))
                    {
                        tables.menustate[tables.menucats.RDR_BVR] = tables.menustates.Radar_Passive;
                    }

                    if (tables.menustate[tables.menucats.RDR_STT].Equals(tables.menustates.STT_locked))
                    {

                        if (!State.currentkey["command"].Equals("wMsgJ_RDR_BREAK_LOCK") && !State.currentkey["command"].Equals("wMsgJ_RDR_TO_PSTT") && !State.currentkey["command"].Equals("wMsgJ_RDR_TOGGLE_STT"))
                        {

                            if (State.currentkey["command"].Equals("wMsgJ_WPN_AG_DROP_TANKS"))
                            {
                                // shift from 1 to 3
                                State.currentmessage.extsequence[5].command += 2; // move to two positions higher
                            }
                            else
                            {
                                State.currentmessage.dspmsg = "AIRIO : N/A: Radar is in Lock state.\n";
                                State.currentmessage.msgdur = 2;
                                State.currentmessage.extsequence = new List<Extensions.RIO.DeviceAction>();
                                riospeech.riospeakrandom(2); //no
                                return false;
                            }
                        }
                        else
                        {
                            // do nothing
                        }
                    }
                    else // not STT locked
                    {

                        bool radarstate = (tables.menustate[tables.menucats.RDR_BVR].Equals(tables.menustates.Radar_Active));
                        bool shift = !radarstate && !State.currentkey["command"].Equals("wMsgJ_RDR_GO_SILENT") && !State.currentkey["command"].Equals("wMsgJ_RDR_GO_ACTIVE");

                        if (shift)
                        {
                            try
                            {
                                Log.Write("Applying -1 menu shift for command: " + State.currentkey["command"], Colors.Warning);
                                State.currentmessage.extsequence[4].command -= 1; // move to one position lower
                            }
                            catch (Exception e)
                            {
                                Log.Write("Error setting -1: " + e.Message, Colors.Warning);
                            }
                        }
                    }
                }

                // ----end radar

                if (State.currentkey["command"].Equals("wMsgJ_WPN_AG_DROP_TANKS"))
                {
                    if (!helper.havetank())
                    {
                        State.currentmessage.dspmsg = "AIRIO : There are no drop tanks loaded.\n";
                        State.currentmessage.msgdur = 5;
                        State.currentmessage.extsequence = new List<Extensions.RIO.DeviceAction>();
                        riospeech.riospeakrandom(3); //hmm
                        return false;
                    }
                    if (!State.currentstate.airborne)
                    {
                        State.currentmessage.dspmsg = "AIRIO : You're on the ground!\n";
                        State.currentmessage.msgdur = 5;
                        State.currentmessage.extsequence = new List<Extensions.RIO.DeviceAction>();
                        riospeech.riospeakrandom(3); //hmm
                        return false;
                    }
                }

                bool inTACANtunerange = State.currentkey["command"].StartsWith("wMsgJ_RAD_TCN_T_");

                if (inTACANtunerange)
                {
                    if (
                        (State.currentkey["command"].StartsWith("wMsgJ_RAD_TCN_T_CS") && !State.currentstate.theatre.Equals("Caucasus")) ||
                        (State.currentkey["command"].StartsWith("wMsgJ_RAD_TCN_T_NV") && !State.currentstate.theatre.Equals("Nevada")) ||
                        (State.currentkey["command"].StartsWith("wMsgJ_RAD_TCN_T_PG") && !State.currentstate.theatre.Equals("PersianGulf"))
                        )
                    {
                        State.currentmessage.dspmsg = "AIRIO : Info: TACAN beacon not located in current theatre.\n";
                        State.currentmessage.msgdur = 5;
                    }

                    riospeech.riospeakrandom(1); //ok
                }

                // jammer
                if ((State.currentkey["command"].Equals("wMsgJ_DEF_JMR_ON") && State.currentstate.riostate.jmr || (State.currentkey["command"].Equals("wMsgJ_DEF_JMR_SBY") && !State.currentstate.riostate.jmr)))
                {
                    State.currentmessage.dspmsg = "AIRIO : Already done!\n";
                    State.currentmessage.msgdur = 5;
                    State.currentmessage.extsequence = new List<Extensions.RIO.DeviceAction>();
                    riospeech.riospeakrandom(3); //hmm
                    return false;
                }

                // TACAN TAC select
                bool TACANTACselect = State.currentkey["command"].StartsWith("wMsgJ_RAD_TCN_TAC_");
                if (TACANTACselect)
                {
                    helper.getTACANstate();

                    Extensions.RIO.DeviceAction action = new Extensions.RIO.DeviceAction();

                    string unitstr = "";

                    try
                    {
                        switch (State.currentkey["command"])
                        {
                            case "wMsgJ_RAD_TCN_TAC_STEN": //
                                unitstr = "Stennis";
                                break;
                            case "wMsgJ_RAD_TCN_TAC_WASH": //
                                unitstr = "Washington";
                                break;
                            case "wMsgJ_RAD_TCN_TAC_ROOS": //
                                unitstr = "Roosevelt";
                                break;
                            case "wMsgJ_RAD_TCN_TAC_LINC": //
                                unitstr = "Lincoln";
                                break;
                            case "wMsgJ_RAD_TCN_TAC_TRUM": //
                                unitstr = "Truman";
                                break;
                            case "wMsgJ_RAD_TCN_TAC_FORE": //
                                unitstr = "Forrestal";
                                break;
                            case "wMsgJ_RAD_TCN_TAC_ARCO": // 
                                unitstr = "Arco";
                                break;
                            case "wMsgJ_RAD_TCN_TAC_SHEL": // 
                                unitstr = "Shell";
                                break;
                            case "wMsgJ_RAD_TCN_TAC_TEXA": // 
                                unitstr = "Texaco";
                                break;
                        }

                        action = helper.TACANstate[unitstr];

                        if (!action.Equals(DeviceActionsLibrary.RIO.Atom_J_VOID))
                        {
                            State.currentmessage.extsequence = new List<Extensions.RIO.DeviceAction>();
                            State.currentmessage.extsequence.AddRange(DeviceActionsLibrary.Sequences.Macro.Seq_J_MENU_MAIN);
                            State.currentmessage.extsequence.AddRange(DeviceActionsLibrary.Sequences.Macro.Seq_J_RAD_TCN_TUNE_TAC);
                            State.currentmessage.extsequence.Add(action);

                        }
                        else // called but unit is not there
                        {
                            State.currentmessage.dspmsg = string.Format("AIRIO : Unit {0} is not available at this time.", unitstr);
                            State.currentmessage.msgdur = 5;
                            State.currentmessage.extsequence = new List<Extensions.RIO.DeviceAction>();
                            riospeech.riospeakrandom(2); //negative
                            return false;
                        }
                    }
                    catch
                    {
                    }

                }


                // DL host select
                bool DLHostselect = State.currentkey["command"].StartsWith("wMsgJ_RAD_DL_HOST_");
                if (DLHostselect)
                {

                    helper.getDLstate();

                    Extensions.RIO.DeviceAction action = new Extensions.RIO.DeviceAction();

                    string unitstr = "";

                    try
                    {
                        switch (State.currentkey["command"])
                        {
                            case "wMsgJ_RAD_DL_HOST_STEN": //
                                unitstr = "Stennis";
                                break;
                            case "wMsgJ_RAD_DL_HOST_WASH": //
                                unitstr = "Washington";
                                break;
                            case "wMsgJ_RAD_DL_HOST_ROOS": //
                                unitstr = "Roosevelt";
                                break;
                            case "wMsgJ_RAD_DL_HOST_LINC": //
                                unitstr = "Lincoln";
                                break;
                            case "wMsgJ_RAD_DL_HOST_TRUM": //
                                unitstr = "Truman";
                                break;
                            case "wMsgJ_RAD_DL_HOST_TICO": //
                                unitstr = "Ticonderoga";
                                break;
                            case "wMsgJ_RAD_DL_HOST_BURK": //
                                unitstr = "ArleighBurke";
                                break;
                            case "wMsgJ_RAD_DL_HOST_FORE": // 
                                unitstr = "Forrestal";
                                break;
                            case "wMsgJ_RAD_DL_HOST_DARK": // 
                                unitstr = "Darkstar";
                                break;
                            case "wMsgJ_RAD_DL_HOST_FOCS": // 
                                unitstr = "Focus";
                                break;
                            case "wMsgJ_RAD_DL_HOST_MAGC": // 
                                unitstr = "Magic";
                                break;
                            case "wMsgJ_RAD_DL_HOST_OVRL": // 
                                unitstr = "Overlord";
                                break;
                            case "wMsgJ_RAD_DL_HOST_WIZR": // 
                                unitstr = "Wizard";
                                break;
                        }

                        action = helper.DLstate[unitstr];

                        if (!action.Equals(DeviceActionsLibrary.RIO.Atom_J_VOID))
                        {
                            State.currentmessage.extsequence = new List<Extensions.RIO.DeviceAction>();
                            State.currentmessage.extsequence.AddRange(DeviceActionsLibrary.Sequences.Macro.Seq_J_MENU_MAIN);
                            State.currentmessage.extsequence.AddRange(DeviceActionsLibrary.Sequences.Macro.Seq_J_RAD_DL_SET_HOST);
                            State.currentmessage.extsequence.Add(action);

                        }
                        else // called but unit is not there
                        {
                            State.currentmessage.dspmsg = string.Format("AIRIO : Unit {0} is not available at this time.", unitstr);
                            State.currentmessage.msgdur = 5;
                            State.currentmessage.extsequence = new List<Extensions.RIO.DeviceAction>();
                            riospeech.riospeakrandom(2); //negative
                            return false;
                        }
                    }
                    catch
                    {
                    }

                }

                // weapons select
                bool weaponselect = State.currentkey["command"].StartsWith("wMsgJ_WPN_AG_SORDN_WPN");
                if (weaponselect)
                {
                    helper.getAGweaponsstate();

                    Extensions.RIO.DeviceAction action = new Extensions.RIO.DeviceAction();

                    string weaponstr = "";

                    try
                    {

                        switch (State.currentkey["command"])
                        {
                            case "wMsgJ_WPN_AG_SORDN_WPN_1": //
                                weaponstr = "Zuni";
                                break;
                            case "wMsgJ_WPN_AG_SORDN_WPN_2": // 
                                weaponstr = "Mk84";
                                break;
                            case "wMsgJ_WPN_AG_SORDN_WPN_3": // 
                                weaponstr = "Mk83";
                                break;
                            case "wMsgJ_WPN_AG_SORDN_WPN_4": // 
                                weaponstr = "Mk82";
                                break;
                            case "wMsgJ_WPN_AG_SORDN_WPN_5": // 
                                weaponstr = "Mk81";
                                break;
                            case "wMsgJ_WPN_AG_SORDN_WPN_6": // 
                                weaponstr = "Mk20";
                                break;
                            case "wMsgJ_WPN_AG_SORDN_WPN_7": // 
                                weaponstr = "LUU2";
                                break;
                            case "wMsgJ_WPN_AG_SORDN_WPN_8": // 
                                weaponstr = "GBU";
                                break;
                            case "wMsgJ_WPN_AG_SORDN_WPN_9": // 
                                weaponstr = "BDU33";
                                break;
                            case "wMsgJ_WPN_AG_SORDN_WPN_10": // 
                                weaponstr = "TALD";
                                break;
                            case "wMsgJ_WPN_AG_SORDN_WPN_11": //
                                weaponstr = "JDAM";
                                break;

                        }

                        if (weaponstr.Equals("GBU"))
                        {
                            action = DeviceActionsLibrary.RIO.Atom_J_VOID;

                            Extensions.RIO.DeviceAction action_GBU10 = helper.AGweaponsstate["GBU10"];
                            Extensions.RIO.DeviceAction action_GBU12 = helper.AGweaponsstate["GBU12"];
                            Extensions.RIO.DeviceAction action_GBU16 = helper.AGweaponsstate["GBU16"];
                            Extensions.RIO.DeviceAction action_GBU24 = helper.AGweaponsstate["GBU24"];

                            bool have_GBU10 = !action_GBU10.Equals(DeviceActionsLibrary.RIO.Atom_J_VOID);
                            bool have_GBU12 = !action_GBU12.Equals(DeviceActionsLibrary.RIO.Atom_J_VOID);
                            bool have_GBU16 = !action_GBU16.Equals(DeviceActionsLibrary.RIO.Atom_J_VOID);
                            bool have_GBU24 = !action_GBU24.Equals(DeviceActionsLibrary.RIO.Atom_J_VOID);

                            if (have_GBU24) { action = action_GBU24; }
                            if (have_GBU16) { action = action_GBU16; }
                            if (have_GBU12) { action = action_GBU12; }
                            if (have_GBU10) { action = action_GBU10; }
                        }

                        if (weaponstr.Equals("JDAM"))
                        {
                            action = DeviceActionsLibrary.RIO.Atom_J_VOID;

                            Extensions.RIO.DeviceAction action_GBU31 = helper.AGweaponsstate["GBU31"];
                            Extensions.RIO.DeviceAction action_GBU38 = helper.AGweaponsstate["GBU38"];

                            bool have_GBU31 = !action_GBU31.Equals(DeviceActionsLibrary.RIO.Atom_J_VOID);
                            bool have_GBU38 = !action_GBU38.Equals(DeviceActionsLibrary.RIO.Atom_J_VOID);

                            if (have_GBU38) { action = action_GBU38; }
                            if (have_GBU31) { action = action_GBU31; }
                        }

                        if (weaponstr.Equals("Mk82"))
                        {
                            action = DeviceActionsLibrary.RIO.Atom_J_VOID;

                            Extensions.RIO.DeviceAction action_Mk82 = helper.AGweaponsstate["Mk82"];
                            Extensions.RIO.DeviceAction action_Mk82A = helper.AGweaponsstate["Mk82A"];
                            Extensions.RIO.DeviceAction action_Mk82SE = helper.AGweaponsstate["Mk82SE"];

                            bool have_Mk82 = !action_Mk82.Equals(DeviceActionsLibrary.RIO.Atom_J_VOID);
                            bool have_Mk82A = !action_Mk82A.Equals(DeviceActionsLibrary.RIO.Atom_J_VOID);
                            bool have_Mk82SE = !action_Mk82SE.Equals(DeviceActionsLibrary.RIO.Atom_J_VOID);

                            if (have_Mk82SE) { action = action_Mk82SE; }
                            if (have_Mk82A) { action = action_Mk82A; }
                            if (have_Mk82) { action = action_Mk82; }
                        }
                        else
                        {
                            if (!weaponstr.Equals("GBU") && !weaponstr.Equals("JDAM"))
                            {
                                action = helper.AGweaponsstate[weaponstr];
                            }
                        }

                        if (!action.Equals(DeviceActionsLibrary.RIO.Atom_J_VOID))
                        {
                            // create sequence from scratch:

                            State.currentmessage.extsequence = new List<Extensions.RIO.DeviceAction>();
                            State.currentmessage.extsequence.AddRange(DeviceActionsLibrary.Sequences.Macro.Seq_J_MENU_MAIN);
                            State.currentmessage.extsequence.AddRange(DeviceActionsLibrary.Sequences.Macro.Seq_J_WPN_AG_SORDN);
                            State.currentmessage.extsequence.Add(action);

                        }
                        else // called but weapon is not there
                        {
                            State.currentmessage.dspmsg = string.Format("AIRIO : Requested stores type {0} is not loaded.", weaponstr);
                            State.currentmessage.msgdur = 5;
                            State.currentmessage.extsequence = new List<Extensions.RIO.DeviceAction>();
                            riospeech.riospeakrandom(2); //negative
                            return false;
                        }
                    }
                    catch
                    {
                    }

                }

                // reject single / pairs
                if ((State.currentstate.riostate.sngl && State.currentkey["command"].Equals("wMsgJ_WPN_AG_SET_SNGL")) || (!State.currentstate.riostate.sngl && State.currentkey["command"].Equals("wMsgJ_WPN_AG_SET_PAIRS")))
                {
                    State.currentmessage.dspmsg = "AIRIO : Already done!\n";
                    State.currentmessage.msgdur = 2;
                    State.currentmessage.extsequence = new List<Extensions.RIO.DeviceAction>();
                    riospeech.riospeakrandom(3); //hmm
                    return false;
                }

                // reject canopy
                if ((State.currentstate.riostate.canopy && State.currentkey["command"].Equals("wMsgJ_CANOPY_OPEN")) || (!State.currentstate.riostate.canopy && State.currentkey["command"].Equals("wMsgJ_CANOPY_CLOSE")))
                {
                    State.currentmessage.dspmsg = "AIRIO : Already done!\n";
                    State.currentmessage.msgdur = 5;
                    State.currentmessage.extsequence = new List<Extensions.RIO.DeviceAction>();
                    riospeech.riospeakrandom(3); //hmm
                    return false;
                }

                // reject attack mode
                if (State.currentstate.riostate.amt && State.currentkey["command"].Equals("wMsgJ_WPN_AG_SET_COMP_TGT"))
                {
                    State.currentmessage.dspmsg = "AIRIO : Already done!\n";
                    State.currentmessage.msgdur = 5;
                    State.currentmessage.extsequence = new List<Extensions.RIO.DeviceAction>();
                    riospeech.riospeakrandom(3); //hmm
                    return false;
                }

                // Toggled states (contract)

                bool t1 = State.currentkey["command"].Equals("wMsgJ_UTIL_CONTR_NO_TALK") && tables.menustate[tables.menucats.CONTR_TALK].Equals(tables.menustates.No_Talk);
                bool t2 = State.currentkey["command"].Equals("wMsgJ_UTIL_CONTR_TALK") && tables.menustate[tables.menucats.CONTR_TALK].Equals(tables.menustates.Talk);
                bool t3 = State.currentkey["command"].Equals("wMsgJ_UTIL_CONTR_CALL") && tables.menustate[tables.menucats.CONTR_TALK].Equals(tables.menustates.Call);
                bool t4 = State.currentkey["command"].Equals("wMsgJ_UTIL_CONTR_NO_CALL") && tables.menustate[tables.menucats.CONTR_TALK].Equals(tables.menustates.No_Call);
                bool t5 = State.currentkey["command"].Equals("wMsgJ_UTIL_CONTR_EJECT_BTH") && !State.currentstate.riostate.ejsn;
                bool t6 = State.currentkey["command"].Equals("wMsgJ_UTIL_CONTR_EJECT_SNG") && State.currentstate.riostate.ejsn;

                if (t1 || t2 || t3 || t4 || t5 || t6)
                {
                    State.currentmessage.dspmsg = "AIRIO : Already done!\n";
                    State.currentmessage.msgdur = 5;
                    State.currentmessage.extsequence = new List<Extensions.RIO.DeviceAction>();
                    riospeech.riospeakrandom(3); //hmm
                    return false;
                }

                // AM/FM switching (182)
                if ((State.currentstate.riostate.AM182 && State.currentkey["command"].Equals("wMsgJ_RAD_182_MODE_AM")) || (!State.currentstate.riostate.AM182 && State.currentkey["command"].Equals("wMsgJ_RAD_182_MODE_FM")))
                {
                    State.currentmessage.dspmsg = "AIRIO : Already done!\n";
                    State.currentmessage.msgdur = 5;
                    State.currentmessage.extsequence = new List<Extensions.RIO.DeviceAction>();
                    riospeech.riospeakrandom(3); //hmm
                    return false;
                }

                if (State.currentkey["command"].Equals("wMsgJ_WLKMN_PLAY") || State.currentkey["command"].Equals("wMsgJ_WLKMN_STOP") || State.currentkey["command"].StartsWith("wMsgJ_CANOPY"))
                {
                    riospeech.riospeakrandom(1); // OK
                }

                return true;

            }

            public static void UpdateStateBasedOnCommand(string input)
            {

                switch (input)

                // for states that cannot be retreived from switch/lights positions
                {


                    //Startup state
                    case "wMsgJ_STRT_ABORT":
                        tables.menustate[tables.menucats.STARTUP] = tables.menustates.Not_Starting; // when airborne switched off
                        break;
                    case "wMsgJ_STRT_STARTUP":
                        tables.menustate[tables.menucats.STARTUP] = tables.menustates.Starting_Up;
                        break;
                    case "wMsgJ_STRT_ASSISTED":
                        tables.menustate[tables.menucats.STARTUP] = tables.menustates.Starting_Up;
                        break;

                    // radar state
                    case "wMsgJ_RDR_GO_ACTIVE":
                        tables.menustate[tables.menucats.RDR_BVR] = tables.menustates.Radar_Active;
                        break;
                    case "wMsgJ_RDR_GO_SILENT":
                        tables.menustate[tables.menucats.RDR_BVR] = tables.menustates.Radar_Passive;
                        break;

                    // contract talking
                    case "wMsgJ_UTIL_CONTR_NO_TALK":
                        tables.menustate[tables.menucats.CONTR_TALK] = tables.menustates.No_Talk;
                        break;
                    case "wMsgJ_UTIL_CONTR_TALK":
                        tables.menustate[tables.menucats.CONTR_TALK] = tables.menustates.Talk;
                        break;

                    // contract callouts
                    case "wMsgJ_UTIL_CONTR_CALL":
                        tables.menustate[tables.menucats.CALLOUTS] = tables.menustates.Call;
                        break;
                    case "wMsgJ_UTIL_CONTR_NO_CALL":
                        tables.menustate[tables.menucats.CALLOUTS] = tables.menustates.No_Call;
                        break;

                    // contract callouts
                    case "wMsgJ_UTIL_CONTR_EJECT_BTH":
                        tables.menustate[tables.menucats.CONTR_EJECT] = tables.menustates.Eject_Both;
                        break;
                    case "wMsgJ_UTIL_CONTR_EJECT_SNG":
                        tables.menustate[tables.menucats.CONTR_EJECT] = tables.menustates.Eject_Single;
                        break;

                    default:
                        break;
                }

                // changes rio state based on command just sent (assume succesful)

            }

            public static partial class Message
            {
                private static bool IsF14BUActive()
                {
                    try
                    {
                        string stateId = (State.currentstate != null && !string.IsNullOrWhiteSpace(State.currentstate.id))
                            ? State.currentstate.id.Trim()
                            : string.Empty;

                        if (!string.IsNullOrWhiteSpace(stateId))
                        {
                            if (stateId.Equals("F-14BU", StringComparison.OrdinalIgnoreCase)
                                || stateId.Equals("F-14B(U)", StringComparison.OrdinalIgnoreCase)
                                || stateId.Equals("F14BU", StringComparison.OrdinalIgnoreCase)
                                || (stateId.IndexOf("F-14B", StringComparison.OrdinalIgnoreCase) >= 0
                                    && stateId.IndexOf("U", StringComparison.OrdinalIgnoreCase) >= 0))
                            {
                                return true;
                            }
                        }

                        string rioMod = !string.IsNullOrWhiteSpace(State.riomod)
                            ? State.riomod.Trim()
                            : string.Empty;

                        if (!string.IsNullOrWhiteSpace(rioMod)
                            && (rioMod.Equals("F-14BU", StringComparison.OrdinalIgnoreCase)
                                || rioMod.Equals("F-14B(U)", StringComparison.OrdinalIgnoreCase)
                                || rioMod.Equals("F14BU", StringComparison.OrdinalIgnoreCase)))
                        {
                            return true;
                        }
                    }
                    catch
                    {
                    }

                    return false;
                }

                public static void SetRioDeviceSequence()
                {
                    try
                    {

                        string cmd = State.currentkey["command"];
                        Dictionary<string, List<List<Extensions.RIO.DeviceAction>>> commandTable = Extensions.RIO.DeviceActionsLibrary.Sequences.RioCommands;

                        bool isF14BU = IsF14BUActive();

                        if (isF14BU)
                        {
                            string overrideKey = "F-14BU:" + cmd;
                            if (Extensions.RIO.DeviceActionsLibrary.Sequences.AuxCommands.ContainsKey(overrideKey))
                            {
                                commandTable = Extensions.RIO.DeviceActionsLibrary.Sequences.AuxCommands;
                                cmd = overrideKey;

                                if (commandTable[cmd].Count == 0)
                                {
                                    State.currentmessage.dspmsg = "AIRIO : Command N/A to F-14B(U)\n";
                                    State.currentmessage.msgdur = 3;
                                    State.currentmessage.extsequence = new List<Extensions.RIO.DeviceAction>();
                                    State.currentmessage.extsequence.Add(new Extensions.RIO.DeviceAction()
                                    {
                                        device = 62,
                                        command = 3725,
                                        value = 1
                                    });
                                    return;
                                }
                            }
                        }
                        else
                        {
                            string buOnlyKey = "F-14BU:" + cmd;
                            bool hasBUOverride = Extensions.RIO.DeviceActionsLibrary.Sequences.AuxCommands.ContainsKey(buOnlyKey);
                            bool hasABEntry = commandTable.ContainsKey(cmd);
                            bool isABEmpty = hasABEntry && commandTable[cmd].Count == 0;

                            if (hasBUOverride && (!hasABEntry || isABEmpty))
                            {
                                State.currentmessage.dspmsg = "AIRIO : Command N/A to F-14A/B\n";
                                State.currentmessage.msgdur = 3;
                                State.currentmessage.extsequence = new List<Extensions.RIO.DeviceAction>();
                                State.currentmessage.extsequence.Add(new Extensions.RIO.DeviceAction()
                                {
                                    device = 62,
                                    command = 3725,
                                    value = 1
                                });
                                return;
                            }
                        }

                        if (!commandTable.ContainsKey(cmd))
                        {
                            if (isF14BU)
                            {
                                State.currentmessage.dspmsg = "AIRIO : Command N/A to F-14B(U)\n";
                                State.currentmessage.msgdur = 3;
                                State.currentmessage.extsequence = new List<Extensions.RIO.DeviceAction>();
                                State.currentmessage.extsequence.Add(new Extensions.RIO.DeviceAction()
                                {
                                    device = 62,
                                    command = 3725,
                                    value = 1
                                });
                                return;
                            }
                            return;
                        }

                        List<Extensions.RIO.DeviceAction> cache = new List<Extensions.RIO.DeviceAction>();
                        foreach (List<Extensions.RIO.DeviceAction> actionlist in commandTable[cmd])
                        {
                            cache.AddRange(actionlist);
                        }

                        // always close menu wheel: add at the end


                        for (int i = 0; i < cache.Count; i++)
                        {
                            State.currentmessage.extsequence.Add(new Extensions.RIO.DeviceAction());
                            State.currentmessage.extsequence[i].command = cache[i].command;
                            State.currentmessage.extsequence[i].device = cache[i].device;
                            State.currentmessage.extsequence[i].value = cache[i].value;
                        }

                        List<Extensions.RIO.DeviceAction> closemenu = new List<Extensions.RIO.DeviceAction>();
                        Extensions.RIO.DeviceAction closemenuaction = new Extensions.RIO.DeviceAction()
                        {
                            device = 62,
                            command = 3725, // closemenu
                            value = 1
                        };

                        closemenu.Add(closemenuaction);

                        if (!State.clientmode.Equals(ClientModes.Debug))
                        {
                            State.currentmessage.extsequence.AddRange(closemenu);
                        }
                        // sequence ready

                        ManipulateCommands();
                        UpdateStateBasedOnCommand(State.currentkey["command"]);

                        // DEBUG:
                        if (State.clientmode.Equals(ClientModes.Debug))
                        {

                            string list = "";
                            foreach (Extensions.RIO.DeviceAction action in State.currentmessage.extsequence)
                            {
                                int num = action.command - 3550;
                                list = list + " " + num.ToString();
                            }
                        }

                    }
                    catch (Exception e)
                    {
                        Log.Write("Error setting RIO command sequence: " + e.Message, Colors.Inline);
                    }
                }
            }
        }
    }
}



