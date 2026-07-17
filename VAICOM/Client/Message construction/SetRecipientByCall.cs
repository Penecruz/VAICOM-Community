using System;
using VAICOM.Database;
using VAICOM.Products;
using VAICOM.Servers;
using VAICOM.Static;

namespace VAICOM
{
    namespace Client
    {

        public partial class DcsClient
        {
            public static Server.DcsUnit GetFlightUnit()
            {

                Server.DcsUnit returnunit = new Server.DcsUnit();

                State.currentflightrecipientnumber = Recipients.Table[State.currentkey["recipient"]].flightnumber;

                if (!State.dcsrunning)
                {
                    return returnunit;
                }
                else
                {
                    try
                    {
                        int wingmencounter = State.currentstate.availablerecipients["Flight"].Count;

                        if (wingmencounter < 2) // no wingmen around to call to
                        {
                            //Log.Write("There are currently no Flight recipients available.", Colors.Warning);
                            UI.Playsound.Recipientna();
                            return returnunit;
                        }
                        else
                        {
                            returnunit = State.currentstate.availablerecipients["Flight"][1];
                        }

                        if (Recipients.Table[State.currentkey["recipient"]].name.Equals("wAIUnitFlightWingman1"))
                        {
                            if (wingmencounter >= 1)
                            {
                                returnunit = State.currentstate.availablerecipients["Flight"][0];
                            }
                            else
                            {
                                returnunit = new Server.DcsUnit();
                                //Log.Write("The called recipient is not available at this time.", Colors.Warning);
                                //UI.Playsound.Recipientna();
                            }
                        }

                        if (Recipients.Table[State.currentkey["recipient"]].name.Equals("wAIUnitFlightWingman2"))
                        {
                            if (wingmencounter >= 2)
                            {
                                returnunit = State.currentstate.availablerecipients["Flight"][1];
                            }
                            else
                            {
                                returnunit = new Server.DcsUnit();
                                //Log.Write("The called recipient is not available at this time.", Colors.Warning);
                                //UI.Playsound.Recipientna();
                            }
                        }

                        if (Recipients.Table[State.currentkey["recipient"]].name.Equals("wAIUnitFlightWingman3"))
                        {
                            if (wingmencounter >= 3)
                            {
                                returnunit = State.currentstate.availablerecipients["Flight"][2];
                            }
                            else
                            {
                                returnunit = new Server.DcsUnit();
                                //Log.Write("The called recipient is not available at this time.", Colors.Warning);
                                //UI.Playsound.Recipientna();
                            }
                        }

                        if (Recipients.Table[State.currentkey["recipient"]].Equals("wAIUnitFlightWingman4"))
                        {
                            if (wingmencounter >= 4)
                            {
                                returnunit = State.currentstate.availablerecipients["Flight"][3];
                            }
                            else
                            {
                                returnunit = new Server.DcsUnit();
                                //Log.Write("The called recipient is not available at this time.", Colors.Warning);
                                //UI.Playsound.Recipientna();
                            }
                        }
                    }
                    catch
                    {
                    }

                    return returnunit;
                }

            }

            public static partial class Message
            {

                public static Server.DcsUnit GetCalledUnit(Recipientclass cat, string calledrecipient)
                {
                    Log.Write("Searching called unit " + calledrecipient + " in category " + cat.Name, Colors.Text);

                    Server.DcsUnit result = new Server.DcsUnit();

                    try
                    {
                        if (calledrecipient.Contains("nearest")) // nearestawacs
                        {
                            return State.currentstate.availablerecipients[cat.Name][0];
                        }
                        else
                        {
                            if (cat.Equals(Recipientclasses.Flight))
                            {
                                return GetFlightUnit();
                            }
                            else // not flight
                            {

                                // for generic 'jtac' : use selected unit
                                if (cat.Name.ToLower().Equals(calledrecipient.ToLower()))
                                {
                                    Log.Write("Generic recipient: using selected unit", Colors.Text);
                                    return GetSelectedUnit(cat);
                                }

                                // else..
                                foreach (Server.DcsUnit unit in State.currentstate.availablerecipients[cat.Name])
                                {
                                    if (unit.callsign.ToLower().Contains(calledrecipient.ToLower()) || unit.fullname.ToLower().Contains(calledrecipient.ToLower()))
                                    {
                                        Log.Write("Called unit found.", Colors.Text);
                                        return unit;
                                    }
                                }

                                // not found: returns -1      
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Log.Write("There was a problem searching called unit:" + e.StackTrace, Colors.Inline);
                    }

                    return new Server.DcsUnit();
                }

                public static Server.DcsUnit GetSelectedUnit(Recipientclass cat)
                {

                    Log.Write("Getting selected unit for category " + cat.Name, Colors.Text);

                    if (cat.Equals(Recipientclasses.AOCS))
                    {
                        return State.currentstate.availablerecipients["Aux"].Find(IsAOCS);
                    }

                    if (cat.Equals(Recipientclasses.Flight))
                    {
                        return GetFlightUnit();
                    }

                    try
                    {
                        if (!CatCanHaveUnits(cat))
                        {
                            return State.currentstate.availablerecipients["Player"][0];
                        }
                        else // can have units
                        {

                            if (State.SelectedUnit[cat.Name].id_.Equals(-1))
                            {
                                Log.Write("Selecting nearest unit for: " + cat.Name, Colors.Text);
                                State.SelectedUnit[cat.Name] = State.currentstate.availablerecipients[cat.Name][0];
                            }
                            Log.Write("Identified unit: " + State.SelectedUnit[cat.Name].callsign + " with id " + State.SelectedUnit[cat.Name].id_, Colors.Text);
                            return State.SelectedUnit[cat.Name];
                        }
                    }
                    catch
                    {
                        Log.Write("Getting selected unit failed.", Colors.Text);
                        return new Server.DcsUnit();
                    }

                }

                public static bool CatCanHaveUnits(Recipientclass cat)
                {

                    if (cat.Equals(Recipientclasses.Crew))
                    {
                        return false;
                    }

                    if (cat.Equals(Recipientclasses.Undefined))
                    {
                        return false;
                    }

                    if (cat.Equals(Recipientclasses.RIO))
                    {
                        return false;
                    }

                    if (cat.Equals(Recipientclasses.AI_pilot))
                    {
                        return false;
                    }

                    if (cat.Equals(Recipientclasses.Cargo))
                    {
                        return false;
                    }

                    if (cat.Equals(Recipientclasses.Descent))
                    {
                        return false;
                    }

                    return true;
                }

                public static int SetUnitForVoidClasses(Recipient recipient)
                {
                    // -1 = block/reject, 0 = passthrough, 1 = set to player unit, done

                    // Aux -> use player unit
                    if (recipient.RecipientClass().Equals(Recipientclasses.Aux))
                    {
                        return 1;
                    }


                    // Cargo -> use player unit
                    if (recipient.RecipientClass().Equals(Recipientclasses.Cargo) || recipient.RecipientClass().Equals(Recipientclasses.Descent))
                    {
                        return 1;
                    }

                    // Undefined -> use player unit
                    if (recipient.RecipientClass().Equals(Recipientclasses.Undefined))
                    {
                        return 1;
                    }

                    // Crew -> use player unit
                    if (recipient.RecipientClass().Equals(Recipientclasses.Crew))
                    {
                        if (State.dcsrunning && State.currentstate.airborne)
                        {
                            Log.Write("Ground crew is currently not available.", Colors.Warning);
                            UI.Playsound.Recipientna();
                            return -1;
                        }
                        else
                        {
                            return 1;
                        }
                    }

                    // RIO -> use player unit
                    if (recipient.RecipientClass().Equals(Recipientclasses.RIO) || recipient.RecipientClass().Equals(Recipientclasses.AI_pilot))
                    {
                        if (State.dcsrunning)
                        {
                            if (State.IsAirioTomcatModule())
                            {
                                return 1;
                            }
                            else
                            {
                                Log.Write("RIO recipient is not available at this time.", Colors.Warning);
                                UI.Playsound.Recipientna();
                                return -1;
                            }
                        }
                        else
                        {
                            Log.Write("AIRIO commands are not available at this time.", Colors.Warning);
                            UI.Playsound.Recipientna();
                            return -1;
                        }
                    }

                    // AOCS
                    if (recipient.RecipientClass().Equals(Recipientclasses.AOCS))
                    {
                        try
                        {
                            State.currentmessageunit = State.currentstate.availablerecipients["Aux"].Find(IsAOCS);
                            //State.currentrecipientclass = Recipientclasses.Aux; 
                            return 0;
                        }
                        catch
                        {
                            return -1;
                        }
                    }

                    return 0;
                }

                public static bool SetRecipientByCall()
                {

                    try
                    {
                        Log.Write("Attempt setting recipient by Call ", Colors.Inline);

                        // first try to set unit if recipient class is void (crew, Cargo,..)

                        int getownunit = SetUnitForVoidClasses(Recipients.Table[State.currentkey["recipient"]]);
                        if (getownunit.Equals(-1))
                        {
                            return false; // recipient na!
                        }
                        if (getownunit.Equals(1))
                        {
                            State.currentmessageunit = State.currentstate.availablerecipients["Player"][0];
                            return true;
                        }

                        // called cat name i.e. "awacs"? use currently selected unit (this is also set if no recipient alias was used)

                        if (State.currentkey["recipient"].ToLower().Equals(Commands.Table[State.currentkey["command"]].RecipientClass().Name.ToLower()))
                        {
                            Log.Write("Done, cat equals alias: " + State.currentkey["recipient"], Colors.Inline);
                            Server.DcsUnit Unit = GetSelectedUnit(Commands.Table[State.currentkey["command"]].RecipientClass());

                            if (!Unit.id_.Equals(-1))
                            {
                                State.currentmessageunit = Unit;
                                return true;
                            }
                            else
                            {
                                return false; // no awacs unit found
                            }
                        }

                        // else try find the unit using called repient name:

                        Server.DcsUnit IntendedUnit = GetCalledUnit(Commands.Table[State.currentkey["command"]].RecipientClass(), State.currentkey["recipient"]);

                        // ok if found, else show unit N/A.

                        if (!IntendedUnit.id_.Equals(-1))
                        {
                            if (State.currentstate.easycomms && State.activeconfig.UseInstantSelect)
                            {
                                Log.Write("Instant select: selecting unit " + IntendedUnit.callsign, Colors.Text);
                                State.SelectedUnit[State.currentrecipientclass.Name] = IntendedUnit;
                            }

                            State.currentmessageunit = IntendedUnit;
                            return true;
                        }
                        else
                        {
                            Log.Write("Recipient " + Labels.airecipients[State.currentkey["recipient"]] + " is currently not available.", Colors.Warning);
                            UI.Playsound.Recipientna();
                            return false;
                        }

                    }
                    catch (Exception e)
                    {
                        Log.Write("Error setting recipient by call: " + e.StackTrace, Colors.Inline);
                        return false;
                    }
                }

            }
        }
    }
}