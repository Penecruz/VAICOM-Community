using VAICOM.Database;
using VAICOM.PushToTalk;
using VAICOM.Static;


namespace VAICOM
{
    namespace Client
    {

        public partial class DcsClient
        {

            public static partial class Message
            {

                public static bool checkcorrectradiouse()
                {

                    if (!State.dcsrunning || State.currentcommand.isState() || State.activeconfig.UseSRSmapping)
                    {
                        return true;
                    }

                    bool calledcrew = State.currentrecipientclass.Equals(Recipientclasses.Crew);
                    bool calledaux = State.currentrecipientclass.Equals(Recipientclasses.Aux);
                    bool calledcargo = State.currentrecipientclass.Equals(Recipientclasses.Cargo);
                    bool calleddescent = State.currentrecipientclass.Equals(Recipientclasses.Descent);
                    bool calledRIO = State.currentrecipientclass.Equals(Recipientclasses.RIO) || State.currentrecipientclass.Equals(Recipientclasses.AI_pilot);
                    bool calledmenu = State.currentcommand.category.Equals(CommandCategories.menu);
                    bool calledreply = State.currentcommand.isReply();
                    bool optionsroot = (State.currentcommand.isOptions() || State.currentcommand.isJesterContext()) & !State.have["recipient"];

                    bool singlemode = (State.currentmodule.Singlehotkey & !State.activeconfig.ForceMultiHotkey) || (!State.currentmodule.Singlehotkey & State.activeconfig.ForceSingleHotkey);

                    bool noradio = State.radiocount.Equals(0);
                    bool singleradio = State.radiocount.Equals(1) || singlemode;
                    bool dualradio = State.radiocount.Equals(2);
                    bool tripleradio = State.radiocount.Equals(3);

                    bool acfc3 = State.currentstate.radios.Count == 0; // count = 0 for FC aircraft
                    bool acnonfc3 = State.currentstate.radios.Count > 0;
                    bool easycommsoff = !State.currentstate.easycomms;
                    bool easycommson = State.currentstate.easycomms;


                    // all ok without further checking
                    if (acnonfc3 || calledRIO || calledcrew || calledaux || calledmenu || calledcargo || calleddescent || calledreply || noradio || singleradio || optionsroot)
                    {
                        return true;
                    }

                    // ONLY FOR FC3 AC
                    bool wrongradio = acfc3 & !State.currentTXnode.radios.Contains(PTT.DefaultRadio[State.currentrecipientclass]) & !State.currentTXnode.radios.Contains(PTT.AlternativeRadio[State.currentrecipientclass]);

                    if (wrongradio)
                    {
                        if (State.currentstate.easycomms)
                        {
                            if (State.activeconfig.UIaddhints)
                            {
                                Log.Write("Invalid radio: for recipient class " + State.currentrecipientclass.Name + " use radio " + PTT.DefaultRadio[State.currentrecipientclass].name + " instead of " + State.currentTXnode.radios[0].name, Colors.Warning);
                                UI.Playsound.Error();
                            }
                            else
                            {
                                // fake beep i.e. do not warn
                                Log.Write("Invalid radio (hints off).", Colors.Text);
                                UI.Playsound.Commandcomplete();
                            }
                            return false;
                        }
                        else
                        {
                            // fake beep i.e. do not warn
                            UI.Playsound.Commandcomplete();
                            return false;
                        }
                    }
                    else // used radio was valid
                    {
                        return true;
                    }
                }
            }
        }
    }
}



