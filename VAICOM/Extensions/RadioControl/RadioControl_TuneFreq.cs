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

                        // Frequency digits, however the speech engine segmented them:
                        // three MHz digits followed by up to three fractional digits.
                        string freqdigits = Extensions.CommandNumbers.Digits();

                        if (freqdigits.Length < 3)
                        {
                            Log.Write("Radio frequency: expected at least 3 digits, got '" + freqdigits + "'", Colors.Inline);
                            return;
                        }

                        SendRadioControlMessage(SendMessage);
                        SendRadioControlMessage(SendMessage);
                        SendRadioControlMessage(SendMessage);

                        // pad a partially spoken frequency out to MHz.kHz
                        string freqmhz = (freqdigits + "000000").Substring(0, 6);

                        string combinedfreq = (freqmhz + "000").Substring(0, 9);

                        SendMessage.tunefrq.Add(combinedfreq);

                        SendRadioControlMessage(SendMessage);

                        Log.Write("Select Frequency " + band.ToUpper() + "" + freqmhz.Substring(0, 3) + "." + freqmhz.Substring(3, 3) + " MHz", Colors.Message);

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



