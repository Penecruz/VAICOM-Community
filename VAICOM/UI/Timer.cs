using System;
using System.Timers;
using System.Windows.Forms;
using VAICOM.Client;
using VAICOM.Extensions.Kneeboard;
using VAICOM.Servers;
using VAICOM.Static;

namespace VAICOM
{
    namespace UI
    {
        public static partial class Timers
        {

            public static System.Timers.Timer UI_Timer { get; set; }
            public static bool Created { get; set; }
            public static bool CurrentPlayStatus { get; set; }

            public static void UI_Timer_Initialize()
            {
                try
                {
                    State.uitimerinterval = 1000;
                    Log.Write("UI timer initialized. ", Colors.Inline);
                    State.uitimerinitalized = true;
                }
                catch (Exception e)
                {
                    Log.Write("UI timer init: " + e.Message, Colors.Inline);
                    State.uitimerinitalized = false;
                }
            }

            public static void UI_Timer_Playtoggle()
            {
                try
                {
                    if (!State.uitimerinitalized)
                    {
                        UI_Timer_Initialize();
                    }
                    if (State.uitimeractive == false)
                    {
                        Log.Write("UI timer start.", Colors.Inline);
                        UI_Timer_Start();
                    }
                    else
                    {
                        Log.Write("UI timer stop.", Colors.Inline);
                        UI_Timer_Stop();
                    }

                }
                catch (Exception e)
                {
                    Log.Write("Problems were reported with UI timer toggle. " + e.Message, Colors.Inline);
                }
            }

            // start beacon playback timer

            public static void UI_Timer_Start()
            {
                try
                {
                    if (!Created)
                    {
                        UI_Timer = new System.Timers.Timer(State.uitimerinterval);
                    }

                    UI_Timer.Start();
                    UI_Timer.Elapsed += UI_Timer_Elapsed_Handler;
                    CurrentPlayStatus = true;
                    State.uitimeractive = true;

                }
                catch (Exception e)
                {
                    Log.Write("Problems were reported with UI timer start. " + e.Message, Colors.Inline);
                }
            }

            // stop chatter playback timer

            public static void UI_Timer_Stop()
            {
                try
                {
                    UI_Timer.Elapsed -= UI_Timer_Elapsed_Handler;
                    UI_Timer.Stop();
                    CurrentPlayStatus = false;
                    State.uitimeractive = false;
                }
                catch
                {
                }
            }

            private static void UI_Timer_Elapsed_Handler(object sender, ElapsedEventArgs e)
            {
                State.random2 = new Random();

                State.lastreceivedmessagetimer = State.lastreceivedmessagetimer + State.uitimerinterval / 1000; // 1sec
                State.lastupdatewingmantimer = State.lastupdatewingmantimer + State.uitimerinterval / 1000; // 1sec
                State.elapsedsincelastpttrelease = State.elapsedsincelastpttrelease + State.uitimerinterval / 1000; // 1sec
                State.lasthearbeatsent += State.uitimerinterval / 1000;
                State.lastupdaterequesttimer = State.lastupdaterequesttimer + State.uitimerinterval / 1000; // 1sec

                // delayed send for kneeboard
                if (State.havekneeboardupdate)
                {
                    State.kneeboardmessagetimer = State.kneeboardmessagetimer + State.uitimerinterval / 1000; // 1sec

                    double messagedur = State.receivedmessageforkneeboard.speech.duration;

                    bool domessage = State.kneeboardmessagetimer > 0.4 * messagedur || State.kneeboardmessagetimer > 3; // max 4 sec wait

                    if (domessage)
                    {
                        // send...
                        KneeboardUpdater.UpdateFromReceivedMessage(State.receivedmessageforkneeboard);
                        //... and reset
                        State.havekneeboardupdate = false;
                        State.kneeboardmessagetimer = 0;
                        State.receivedmessageforkneeboard = new Server.ServerCommsMessage();
                    }
                }
                else
                {
                    State.kneeboardmessagetimer = 0;
                }

                if (State.lastupdaterequesttimer > 59)
                {
                    Log.Write("Update timer elapsed, beacon pulse.", Colors.Inline);
                    State.lastupdaterequesttimer = 0;
                    if (!State.currentstate.id.Equals("UH-1H")) // don't update for Huey //Why?? Pene
                    {
                        DcsClient.SendUpdateRequest();
                    }
                }

                // KNEEBOARD UPDATE
                try
                {
                    KneeboardUpdater.SendHeartBeatCycle(); // do every second
                }
                catch
                {
                }

                try
                {
                    if (State.lasthearbeatsent > 2)
                    {
                        Client.DcsClient.SendHeartBeatToTray();
                        State.lasthearbeatsent = 0;
                    }
                }
                catch
                {
                }

                try
                {
                    // update PTT page in UI thread
                    if (State.configwindowopen && (State.configurationwindow != null))
                    {
                        State.configurationwindow.Dispatcher.BeginInvoke((MethodInvoker)delegate
                        {
                            State.configurationwindow.Alternatebeaconbug();
                            State.configurationwindow.AlternateOKHostbug();
                            State.configurationwindow.Alternateupdatebug();
                            State.configurationwindow.AlternateWorldLight();
                        });
                    }

                }
                catch (Exception a)
                {
                    Log.Write("Problems were reported with the UI timer handler. " + a.Message, Colors.Inline);
                }
            }
        }
    }
}


