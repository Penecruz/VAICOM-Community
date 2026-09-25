using System;
using VAICOM.Static;

namespace VAICOM
{

    namespace Client
    {

        public partial class DcsClient
        {

            public static partial class Message
            {

                public static void RadioControl_TuneFreq()
                {
                    try
                    {

                        RadioTuneMessage SendMessage = new RadioTuneMessage();
                        SendMessage.tgtdevid = Message.GetSendDeviceId();

                        string band = State.Proxy.Utility.ParseTokens("{CMDSEGMENT:1}").Replace(" ", "");

                        switch (band)
                        {
                            case "am":
                                SendMessage.tunemod = 0;
                                break;
                            case "fm":
                                SendMessage.tunemod = 1;
                                break;
                            default:
                                SendMessage.tunemod = null;
                                break;
                        }

                        //string currentmodulation = State.currentTXnode.radios[0].modulation;
                        //SendMessage.tunemod = !currentmodulation.Equals(band.ToUpper())? 1 : 0;

                        Log.Write("Tunemod = " + SendMessage.tunemod, Colors.Inline);

                        // freq first 3 digits
                        string majval1 = State.Proxy.Utility.ParseTokens("{CMDSEGMENT:2}");
                        SendRadioControlMessage(SendMessage);

                        string majval2 = State.Proxy.Utility.ParseTokens("{CMDSEGMENT:3}");
                        SendRadioControlMessage(SendMessage);

                        string majval3 = State.Proxy.Utility.ParseTokens("{CMDSEGMENT:4}");
                        SendRadioControlMessage(SendMessage);

                        // decimal = {CMDSEGMENT:5}

                        string minval1 = State.Proxy.Utility.ParseTokens("{CMDSEGMENT:6}");
                        string minval2 = State.Proxy.Utility.ParseTokens("{CMDSEGMENT:7}").Replace(" ", "");

                        string combinedfreq = (majval1 + majval2 + majval3 + minval1 + minval2 + "000000000").Substring(0, 9);

                        SendMessage.tunefrq.Add(combinedfreq);

                        SendRadioControlMessage(SendMessage);

                        Log.Write("Select Frequency " + band.ToUpper() + "" + majval1 + majval2 + majval3 + "." + minval1 + minval2 + " MHz", Colors.Message);

                        UI.Playsound.Commandcomplete();

                        //State.MessageReset();

                    }
                    catch (Exception e)
                    {
                        Log.Write("Error setting Radio: " + e.Message, Colors.Inline);
                    }
                }

            }
        }
    }
}



