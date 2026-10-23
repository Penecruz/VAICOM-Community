using System;
using System.Threading;
using VAICOM.Static;

namespace VAICOM
{
    namespace Client
    {

        public partial class DcsClient
        {
            public static partial class Message
            {
                public static void SendNewMessage() // for aicomms
                {
                    try
                    {
                        string outputstring = SendClientMessage();
                        Log.Write("CLIENT MESSAGE SENT: " + outputstring, Colors.Inline);
                        Log.Write("Message sent successfully for recipient class " + State.currentrecipientclass.Name + ".", Colors.Inline);

                        if (State.currentmessage.command.Equals(4000))
                        {
                            // for Select command
                            int delay = 0;
                            if (!State.currentmodule.radiodelay.Equals(null))
                            {
                                delay = State.currentmodule.radiodelay; // allow some time for radio to tune
                            }
                            else
                            {
                                delay = 0;
                            }
                            Thread.Sleep(delay);
                            DcsClient.SendUpdateRequest(); // get an update directly after
                        }
                    }
                    catch (Exception)
                    {
                        Log.Write("A problem occured while sending client JSON message.", Colors.Inline);
                        //UI.Playsound.error();
                    }
                }

            }
        }
    }

}



