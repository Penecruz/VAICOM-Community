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
                public static void logclosecall()
                {

                    try
                    {
                        if (State.currentcommand.isSwitch())
                        {
                            if (State.activeconfig.MP_VoIPUseSwitch && State.activeconfig.MP_AllowSwitchCommand)
                            {
                                if (State.activeconfig.MP_VoIPUseSwitch && !State.activeconfig.MP_VoIPAutoSwitch && State.activeconfig.MP_WarnHumans)
                                {
                                    UI.Playsound.Human_Comms_Active();
                                }
                            }
                            else
                            {
                                UI.Playsound.Sorry();
                            }
                        }
                        else
                        {
                            UI.Playsound.Commandcomplete();
                        }

                        string recipientlabel = "";
                        string senderlabel = "";
                        string cuelabel = "";
                        string commandlabel = "";
                        string labelwpn = "";
                        string labeldir = "";

                        recipientlabel = Labels.airecipients[State.currentkey["recipient"]];

                        if (!recipientlabel.Equals(""))
                        {
                            recipientlabel = "[ " + recipientlabel + " ] , ";
                        }


                        // sender label
                        try
                        {
                            if (State.activeconfig.UseNewRecognitionModel)
                            {
                                if (State.currentrecipientclass.Name.Equals("Flight"))
                                {
                                    senderlabel = "";
                                }
                                else
                                {
                                    senderlabel = "";
                                }
                            }
                            else
                            {
                                senderlabel = Labels.playercallsigns[State.currentkey["sender"]];

                            }

                            if (!senderlabel.Equals("") && !senderlabel.Equals(" "))
                            {
                                senderlabel = "[ " + senderlabel + " ], ";
                            }
                            else
                            {
                                senderlabel = "";
                            }

                        }
                        catch
                        {
                        }

                        // cue
                        try
                        {
                            cuelabel = Labels.aicues[State.currentkey["cue"]];
                        }
                        catch
                        {
                        }

                        if (!cuelabel.Equals("") && !cuelabel.Equals(" "))
                        {
                            cuelabel = "[ " + cuelabel + " ] ";
                        }

                        // command label
                        try
                        {
                            if (State.have["importedmenus"])
                            {
                                commandlabel = Labels.importedmenus[State.currentkey["command"]];
                            }
                            else
                            {
                                commandlabel = Labels.aicommands[State.currentkey["command"]];
                            }
                        }
                        catch
                        {
                        }

                        commandlabel = "[ " + commandlabel + " ] ";

                        try
                        {
                            labelwpn = Labels.aiappendiceswpn[State.currentkey["apxwpn"]];
                        }
                        catch
                        {
                        }
                        if (!labelwpn.Equals("") && !labelwpn.Equals(" "))
                        {
                            labelwpn = "[ " + labelwpn + " ] ";
                        }

                        try
                        {
                            labeldir = Labels.aiappendicesdir[State.currentkey["apxdir"]];
                        }
                        catch
                        {
                        }
                        if (!labeldir.Equals("") && !labeldir.Equals(" "))
                        {
                            labeldir = "[ " + labeldir + " ]";
                        }

                        // write message to log
                        // 
                        // for single ptt
                        if (PTT.IsPTTModeSingle())
                        {
                            Log.Write(State.currentTXnode.name + " | " + PTT.RadioDevices.SEL.name + ": " + recipientlabel + senderlabel + cuelabel + commandlabel + labelwpn + labeldir, Colors.Message);
                        }
                        else // for multi ptt:
                        {
                            Log.Write(State.currentTXnode.name + " | " + State.currentTXnode.radios[0].name + ": " + recipientlabel + senderlabel + cuelabel + commandlabel + labelwpn + labeldir, Colors.Message);
                        }
                    }
                    catch
                    {
                        //Log.Write("Closing call:" + e.StackTrace, Colors.Text);
                    }

                }
            }
        }
    }
}



