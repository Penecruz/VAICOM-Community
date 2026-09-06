using System.Windows.Forms;
using VAICOM.Extensions.CPG;
using VAICOM.Extensions.Kneeboard;
using VAICOM.Products;
using VAICOM.PushToTalk;

namespace VAICOM
{

    namespace Servers
    {

        public static partial class Server
        {

            public static string missionendstring = "missiondata.update.beacon.unlock";

            public static bool DetectEndMission(string detectstr)
            {
                return detectstr.Equals(missionendstring);
            }

            public static void EndMission()
            {


                State.beaconlocked = false;
                State.dcsrunning = false;
                State.AIRIOactive = false;
                State.currentmodule = DCSmodules.LookupTable["----"];
                State.currentstate = new ServerState();
                State.currentstate.dcsversion = "";
                State.currentstate.easycomms = State.previousstate.easycomms;
                PTT.PTT_ResetConfig();
                PTT.PTT_TXAssignmentDefault();
                PTT.PTT_ApplyNewConfig();
                State.synth.SpeakAsyncCancelAll();
                State.briefinginprogress = false;
                State.allowairioswitching = true;
                PTT.PTT_Manage_Listen_States_OnPressRelease(false, false);
                //Server.fetchingunits = false;
                Extensions.RIO.helper.showingjestermenu = false;

                EndMissionUpdateGUI();

                State.KneeboardState = new KneeboardState();

                AH64GeorgeState.WeaponStateValid = false;
                AH64GeorgeState.GunAvailable = false;
                AH64GeorgeState.RocketsAvailable = false;
                AH64GeorgeState.MissilesAvailable = false;
                AH64GeorgeState.SelectedWeapon = AH64WeaponMode.Unknown;
            }

            public static void EndMissionUpdateGUI()
            {
                try
                {
                    if (State.configwindowopen && (State.configurationwindow != null))
                    {
                        State.configurationwindow.Dispatcher.BeginInvoke((MethodInvoker)delegate
                        {
                            State.configurationwindow.Changedatabug();
                            State.configurationwindow.UpdateAllbugs();
                            State.configurationwindow.Voidcounterbug();
                            State.configurationwindow.updatepttinfo();
                            //State.configurationwindow.RIO_Enable_Box.IsEnabled = true;
                            State.configurationwindow.CheckBoxHotMic();
                            State.configurationwindow.Dictate_set_relhot_Light(State.activeconfig.ReleaseHot);

                        });
                    }
                }
                catch
                {
                }
            }

        }
    }
}
