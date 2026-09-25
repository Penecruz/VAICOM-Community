using System.Collections.Generic;
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

                /// <summary>
                /// Reports a rejected AIRIO input back to the player.
                ///
                /// Input errors previously reached the VoiceAttack log only, which the player
                /// cannot see from the cockpit — in VR especially, a rejected command was
                /// indistinguishable from one that silently did nothing. Sending it as a
                /// message puts the reason on screen alongside the other AIRIO responses,
                /// with an empty action sequence so no device is actuated.
                /// </summary>
                /// <param name="displaymessage">Shown to the player. "AIRIO : " is prefixed.</param>
                /// <param name="logmessage">Written to the VoiceAttack log.</param>
                private static void ReportRioInputError(string displaymessage, string logmessage)
                {
                    Log.Write(logmessage, Colors.Warning);

                    State.currentmessage.dspmsg = "AIRIO : " + displaymessage + "\n";
                    State.currentmessage.msgdur = 5;
                    State.currentmessage.extsequence = new List<Extensions.RIO.DeviceAction>();

                    UI.Playsound.Sorry();

                    SendNewMessage();
                    State.MessageReset();
                }

            }
        }
    }
}
