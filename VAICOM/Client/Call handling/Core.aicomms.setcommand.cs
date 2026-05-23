using System;
using VAICOM.Database;
using VAICOM.Static;

namespace VAICOM
{
    namespace Client
    {

        public partial class DcsClient
        {
            public static partial class Message
            {

                public static bool setcommand()
                {
                    bool result;

                    if (!State.have["command"])
                    {
                        return false;
                    }
                    else
                    {
                        // Set State.currentcommand:
                        if (!Commands.Table.ContainsKey(State.currentkey["command"])) // some internal error, default to void command
                        {
                            State.currentkey["command"] = "wMsgNull";
                        }

                        State.currentcommand = Commands.Table[State.currentkey["command"]]; // normal, have command
                        result = true;

                        // Handle Kneeboard recipient no need to wait for additional input
                        if (State.have["recipient"] && State.currentkey["recipient"].Equals("kneeboard", StringComparison.OrdinalIgnoreCase))
                        {
                            State.haveinputscomplete = true;
                            return true;
                        }

                        if (!State.activeconfig.AllowAddCommands)
                        {
                            Log.Write("Extended command set is currently disabled in preferences.", Colors.Warning);
                            return false;
                        }

                        // Options
                        if (State.currentcommand.isOptions() && !State.activeconfig.AllowOptions)
                        {
                            Log.Write("Options command is currently disabled in preferences.", Colors.Warning);
                            UI.Playsound.Recipientna();
                            return false;
                        }

                        // Reject if AIRIO but not in F14.
                        if (State.currentcommand.isRIO() && !State.AIRIOactive)
                        {
                            Log.Write("AIRIO commands are not available.", Colors.Warning);
                            UI.Playsound.Recipientna();
                            return false;
                        }

                        // FC3
                        if (State.currentcommand.blockedforFC & State.currentmodule.IsFC)
                        {
                            Log.Write("(this command is not available for " + State.currentmodule.Name + " module)", Colors.Warning);
                            UI.Playsound.Sorry();
                            return false;
                        }

                        // Multiplayer check
                        if (State.currentstate.multiplayer & !State.activeconfig.MP_UsePluginWithMultiplayer)
                        {
                            Log.Write("Use with multiplayer is currently disabled in preferences.", Colors.Warning);
                            return false;
                        }

                        // Cue
                        if (State.currentcommand.isEngage() & State.activeconfig.Engagecuerequired & !State.have["cue"])
                        {
                            return false;
                        }

                        // Select command using Flight
                        if (State.currentcommand.isSelect() & !State.have["recipient"])
                        {
                            if (State.have["sender"]) // addressing flight i.e. 'enfield' 
                            {
                                State.currentkey["recipient"] = "flight";
                                State.usedalias["recipient"] = State.usedalias["sender"];
                                State.have["recipient"] = true;
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }

                        // F-4E WSO
                        if (State.currentcommand.isWSO())
                        {
                            // Check if this command requires a recipient to have been provided, e.g. an ATC
                            // when diverting to an airfield or tuning radio.
                            if (State.currentcommand.RequiresWSOCommandRecipient() && State.currentWSOCommandRecipient == null)
                            {
                                if (State.have["wsocmdrecipient"])
                                {
                                    State.currentWSOCommandRecipient = Recipients.Table[State.currentkey["wsocmdrecipient"]];
                                }
                                else if (State.have["importedatcs"])
                                {
                                    State.currentWSOCommandRecipient = Recipients.Table[State.currentkey["importedatcs"]];
                                }
                                else
                                {
                                    if (State.activeconfig.UIaddhints)
                                    {
                                        UI.Playsound.Proceed();
                                    }
                                    Log.Write("(awaiting additional ATC, tanker, or asset for WSO command)", Colors.Message);
                                    return false;
                                }
                            }

                            // Explicitely set the WSO recipient to override any other that may have been in the spoken command,
                            // e.g. when diverting to an airfield the airfield name gets set as the recipient.
                            // This means that logging and output will corectly reflect that this was a WSO command.
                            State.currentkey["recipient"] = "WSO";
                            State.usedalias["recipient"] = "WSO";
                            State.have["recipient"] = true;

                            return true;
                        }
                    }

                    return result;
                }
            }
        }
    }
}