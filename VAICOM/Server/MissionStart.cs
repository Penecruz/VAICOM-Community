using System;
using System.Windows.Forms;
using VAICOM.Extensions.Kneeboard;
using VAICOM.Extensions.RIO;
using VAICOM.PushToTalk;
using VAICOM.Static;


namespace VAICOM
{
    namespace Servers
    {

        public static partial class Server
        {

            public static bool newmissionflag;
            public static Vector homebaselocation;

            public static bool DetectNewMission()
            {
                bool newmissiondetect = ((State.previousstate.id != State.currentstate.id) ||
                                         (State.previousstate.playerunitid != State.currentstate.playerunitid) ||
                                         (State.previousstate.missiontitle != State.currentstate.missiontitle) ||
                                         (State.previousstate.easycomms != State.currentstate.easycomms));

                if (!newmissionflag && newmissiondetect)
                {
                    Log.Write("------------------------------------------", Colors.Message);
                    Log.Write("DCS mission | " + State.currentstate.missiontitle, Colors.Message);
                    //Log.Write($"Mission restart detected. Previous State: {State.previousstate.id}, Current State: {State.currentstate.id}", Colors.Debug);

                    State.dcsrunning = true;
                    newmissionflag = true;
                    return true;
                }
                else
                {
                    newmissionflag = false;
                    return false;
                }

            }

            public static void GUI_InitNewMission()
            {
                if (State.configwindowopen && (State.configurationwindow != null))
                {
                    State.configurationwindow.Dispatcher.BeginInvoke((MethodInvoker)delegate
                    {
                        State.configurationwindow.RIO_Enable_Box.IsEnabled = false;
                        State.configurationwindow.Dictate_set_relhot_Light(State.activeconfig.ReleaseHot);
                    });
                }
            }

            public static void InitNewMission()
            {
                Log.Write("Initializing new mission...", Colors.Text);

                State.allowairioswitching = false;
                State.beaconlocked = State.oneradioactive;
                State.ResetSelectedUnits();
                State.menuauximported = false;
                Extensions.RIO.helper.showingjestermenu = false;

                DisplayCurrentModule();
                ScanNewTheater();

                State.messagelog = "";
                State.lastmessagelog = "";
                State.nineline = "";

                State.F14WheelChocksState = State.WheelChocksState.Unknown;
                State.F14WheelChocksStateAssumed = false;
                if (State.IsAirioTomcatModule())
                {
                    State.F14WheelChocksState = State.WheelChocksState.On;
                    State.F14WheelChocksStateAssumed = true;
                    Log.Write("F-14 wheel chocks state initialized to ON (assumed).", Colors.Text);
                }

                tables.resetriomenustate();
                helper.getAGweaponsstate();

                Log.Write("Resetting home base location.", Colors.Text);
                homebaselocation = new Vector();
                try
                {
                    homebaselocation = State.currentstate.availablerecipients["ATC"][0].pos;
                }
                catch (Exception ex)
                {
                    Log.Write($"Failed to set home base location: {ex.Message}", Colors.Warning);
                }

                PTT.PTT_Manage_Listen_States_OnPressRelease(false, false);

                try
                {
                    Extensions.Chatter.AudioTimer.Chatter_Initialize();
                }
                catch (Exception ex)
                {
                    Log.Write($"Chatter theme initialization failed: {ex.Message}", Colors.Warning);
                }

                try
                {
                    if (State.activeconfig.AutoBrief)
                    {
                        Extensions.AOCS.AOCSProvider.AOCS_ReadBriefing(true);
                    }
                }
                catch (Exception ex)
                {
                    Log.Write($"Briefing readout initialization failed: {ex.Message}", Colors.Warning);
                }

                GUI_InitNewMission();

                try
                {
                    Client.DcsClient.UpdateRIOState();
                }
                catch (Exception ex)
                {
                    Log.Write($"Failed to update RIO state: {ex.Message}", Colors.Warning);
                }

                try
                {
                    State.KneeboardState = new KneeboardState();
                    State.kneeboardcurrentbuffer = "";
                    State.Proxy.Dictation.ClearBuffer(false, out string Message2);
                }
                catch (Exception ex)
                {
                    Log.Write($"Failed to reset kneeboard contents: {ex.Message}", Colors.Warning);
                }

                Log.Write("New mission initialization complete.", Colors.Text);
            }
        }
    }
}