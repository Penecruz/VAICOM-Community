using System;
using System.Speech.Synthesis;
using System.Threading;
using System.Windows.Forms;
using VAICOM.Client;
using VAICOM.Static;

namespace VAICOM
{


    namespace PushToTalk
    {

        public partial class PTT
        {

            public static bool TXLinkApply;
            public static bool TXLinkToggle;

            public static bool IsPTTModeSingle()
            {
                return (State.currentmodule.Singlehotkey & !State.activeconfig.ForceMultiHotkey) || (!State.currentmodule.Singlehotkey & State.activeconfig.ForceSingleHotkey);
            }

            public static bool IsPTTModeMulti()
            {
                return !IsPTTModeSingle();
            }

            public static bool IsPTTMultiSingle()
            {
                return IsPTTModeSingle() && !String.IsNullOrEmpty(State.currentstate.selectedradio);
            }


            public static void PTT_Handler(dynamic vaProxy, TXNode PTTkey, bool keypress, bool longpress)
            {
                State.currentTXnode = PTTkey;
                State.Proxy = vaProxy;
                State.valistening = vaProxy.State.GetListeningEnabled();

                TXLinkApply = State.PRO && State.activeconfig.MP_UseTXLink && !(State.activeconfig.MP_TXLink_MPOnly && !State.currentstate.multiplayer);
                Log.Write("TXLinkApply = " + TXLinkApply, Colors.Inline);

                bool applylong = !TXLinkApply || longpress;
                Log.Write("apply long press = " + applylong, Colors.Inline);

                if (!PTT_Detect_Blocked(PTTkey, keypress))
                {
                    if (PTTkey.enabled)
                    {
                        PTT_Handle_TX_Enabled(PTTkey, keypress, applylong);
                    }
                    else
                    {
                        PTT_Handle_TX_Disabled(PTTkey, keypress);
                    }
                }
                else
                {
                    return;
                }

                State.MessageReset();
                State.processlocked = false; // accept new voice phrases for processing
            }

            public static bool PTT_Detect_Blocked(TXNode PTTkey, bool keypress)
            {

                try
                {
                    if (State.trainerrunning)
                    {
                        string message = "Keyword Training Mode active: PTT is disabled.";
                        Log.Write(message, Colors.Warning);
                        return true;
                    }

                    if (State.blockedmodule)
                    {
                        string message = "DCS module " + State.currentmodule.Name + " is available with PRO license only.";
                        Log.Write(message, Colors.Warning);
                        State.MessageReset();
                        DcsClient.SendUpdateRequest();
                        return true;
                    }

                    return false;
                }
                catch
                {
                    return true;
                }

            }

            public static void PTT_Handle_TX_Enabled(TXNode PTTkey, bool keypress, bool longpress)
            {

                bool _isVOIP = TXLinkApply && longpress;
                TXLinkToggle = !_isVOIP ? !TXLinkToggle : false;
                bool press = !_isVOIP ? TXLinkToggle : keypress;

                Log.Write("press = " + press, Colors.Inline);

                try
                {
                    if (press)
                    {
                        Log.Write("PTT press: Elapsed = 0.000s", Colors.Inline);
                        State.Stopwatch.Restart();

                        PTT_Manage_Listen_States_OnPressRelease(press, _isVOIP);
                        PTTkey.relay = _isVOIP || (!State.Proxy.Dictation.IsOn() && (State.activeconfig.MP_VoIPParallel) && (State.activeconfig.MP_UseVoiceChatIntegration || State.activeconfig.MP_UseSRSIntegration));

                        if (PTTkey.radios[0].on)
                        {
                            UI.Playsound.Pttnoise(true);
                            State.transmitting = true;
                            State.currentradiodevicename = PTTkey.radios[0].name;
                            PTT_Manage_AOCS_Speech(true);
                            State.chatteroutput.Stop();
                            PTT_Manage_ShowOptions(true);
                        }
                        else
                        {
                            State.transmitting = false;
                        }

                        DcsClient.SendUpdateRequest();

                    }
                    else // hotkey released
                    {

                        State.Stopwatch.Stop();

                        TimeSpan elapsed = State.Stopwatch.Elapsed;
                        Log.Write("PTT release: Elapsed = " + elapsed.Seconds + "." + elapsed.Milliseconds + "s", Colors.Inline);

                        if ((State.activeconfig.MP_VoIPUseSwitch || State.activeconfig.MP_VoIPParallel) && State.activeconfig.MP_DelayTransmit && DcsClient.Message.havedelayedmessage)
                        {
                            DcsClient.Message.sendmessage();
                            DcsClient.Message.havedelayedmessage = false;
                            State.MessageReset();
                        }

                        if (PTTkey.radios[0].on)
                        {
                            UI.Playsound.Pttnoise(false);
                            PTT_Manage_AOCS_Speech(false);
                            PTT_Manage_ShowOptions(false);
                        }

                        PTT_Manage_Listen_States_OnPressRelease(press, _isVOIP);

                        State.transmitting = false;
                        PTTkey.relay = false;
                        TXLinkToggle = false;

                    }
                }
                catch
                {
                }

                PTT_GUI_Update(PTTkey, press);

            }

            public static void PTT_Handle_TX_Disabled(TXNode PTTkey, bool keypress)
            {

                Log.Write("TX is disabled ", Colors.Inline);

                try
                {
                    State.transmitting = false;
                    PTTkey.relay = false;
                    TXLinkToggle = false;
                    if (keypress)
                    {
                        DcsClient.SendUpdateRequest();
                    }
                    else
                    {
                        PTT_Manage_Listen_States_OnPressRelease(keypress, false);
                    }
                }
                catch
                {
                }

            }

            public static void PTT_Manage_Listen_States_OnPressRelease(bool keypress, bool isVOIP)
            {

                if (State.Proxy.Dictation.IsOn())
                {
                    PTT_Manage_Listen_VA(true);
                    PTT_Manage_Listen_VAICOM(true);
                    PTT_Manage_Listen_VC(false);
                    PTT_Manage_Listen_SRS(false);

                    State.currentTXnode.relay = keypress;
                    State.activeconfig.MP_VCHotMic = keypress;

                    try
                    {

                        if (State.configwindowopen && (State.configurationwindow != null))
                        {

                            if (keypress)
                            {

                                State.configurationwindow.Dispatcher.BeginInvoke((MethodInvoker)delegate
                                {
                                    State.configurationwindow.Dictate_set_relay_Light(false);
                                    State.configurationwindow.Dictate_set_relhot_Light(true);

                                });
                            }
                            else
                            {
                                State.configurationwindow.Dispatcher.BeginInvoke((MethodInvoker)delegate
                                {
                                    State.configurationwindow.Dictate_set_relay_Light(false);
                                    State.configurationwindow.Dictate_set_relhot_Light(false);

                                });
                            }

                        }

                    }
                    catch
                    {
                    }
                    return;
                }

                if (keypress)
                {
                    if (isVOIP)
                    {
                        Log.Write("PTT: PRESS - VOIP.", Colors.Inline);
                        PTT_Manage_Listen_VA(false);
                        PTT_Manage_Listen_VAICOM(true);
                        PTT_Manage_Listen_VC(true);
                        PTT_Manage_Listen_SRS(true);

                        if (State.activeconfig.MP_WarnHumans)
                        {
                            new Thread(() =>
                            {
                                Thread.CurrentThread.IsBackground = true;
                                UI.Playsound.Human_Comms_Active();

                            }).Start();
                        }

                    }
                    else // normal, listen fo AI commands
                    {
                        Log.Write("PTT: PRESS - NORMAL.", Colors.Inline);
                        PTT_Manage_Listen_VA(true);
                        PTT_Manage_Listen_VAICOM(true);
                        PTT_Manage_Listen_VC(State.activeconfig.MP_VoIPParallel);
                        PTT_Manage_Listen_SRS(State.activeconfig.MP_VoIPParallel);
                    }
                }
                else //release
                {
                    Log.Write("PTT: RELEASE.", Colors.Inline);
                    PTT.PTT_Manage_Listen_VA(State.activeconfig.ReleaseHot || (State.AIRIOactive && State.activeconfig.ICShotmic));
                    PTT.PTT_Manage_Listen_VAICOM(!State.activeconfig.ReleaseHot);
                    PTT.PTT_Manage_Listen_VC(State.activeconfig.MP_VCHotMic);
                    PTT.PTT_Manage_Listen_SRS(true);
                    State.elapsedsincelastpttrelease = 0;
                }

            }

            public static void PTT_Manage_Listen_States_OnSwitch()
            {

                Log.Write("PTT: SWITCH - switching off VA listening.", Colors.Inline);
                PTT_Manage_Listen_VA(false);
                PTT_Manage_Listen_VAICOM(false);
                PTT_Manage_Listen_VC(true);
                PTT_Manage_Listen_SRS(true);

                State.currentTXnode.relay = true;
                PTT_GUI_Update(State.currentTXnode, true);

            }

            public static void PTT_Manage_Listen_States_OnUpdate(TXNode PTTkey)
            {

                if (State.activeconfig.MP_VoIPParallel || State.activeconfig.MP_UseTXLink)
                {
                    return;
                }

                try
                {

                    // AI is present: listen for AI commands

                    PTTkey.relay = State.activeconfig.MP_VoIPParallel;

                    Log.Write("PTT: UPDATE switching on VA listening.", Colors.Inline);

                    PTT_Manage_Listen_VA(true);
                    PTT_Manage_Listen_VAICOM(true);
                    PTT_Manage_Listen_VC(State.activeconfig.MP_VoIPParallel);
                    PTT_Manage_Listen_SRS(State.activeconfig.MP_VoIPParallel);

                }
                catch
                {
                }

            }

            public static void PTT_Manage_Listen_VA(bool on)
            {

                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    if (!State.Proxy.State.GetListeningEnabled().Equals(on))
                    {
                        State.Proxy.State.SetListeningEnabled(on);
                    }
                    State.valistening = on;

                }).Start();

            }

            public static void PTT_Manage_Listen_VAICOM(bool on)
            {

                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;

                    State.Proxy.Command.SetSessionEnabledByCategory("Keyword Collections", on);
                    State.Proxy.Command.SetSessionEnabledByCategory("Extension packs", on);

                    if (State.Proxy.GetProfileName().ToLower().Contains(State.defProfileName.ToLower()))
                    {
                        State.Proxy.Command.SetSessionEnabled("Chatter", true);
                    }

                }).Start();

            }

            public static void PTT_Manage_Listen_VC(bool on)
            {
                if (State.currentstate.multiplayer && State.activeconfig.MP_UseVoiceChatIntegration)
                {
                    if (on)
                    {
                        DcsClient.SendCmdSequence(DcsClient.iCommandsequences.iCommandPushToTalkEnableVoice, false);
                    }
                    else
                    {
                        DcsClient.SendCmdSequence(DcsClient.iCommandsequences.iCommandPushToTalkDisableVoice, false);
                    }
                }
            }

            public static void PTT_Manage_Listen_SRS(bool on)
            {

                Log.Write("SRS update; " + on, Colors.Inline);

                if (State.activeconfig.MP_UseSRSIntegration)
                {

                    Extensions.SRS.SRSclient.SRS_PTT_Message newmessage = new Extensions.SRS.SRSclient.SRS_PTT_Message();

                    newmessage.InhibitTX = !on;

                    Client.DcsClient.SendSRSMessage(newmessage);

                    if (!on)
                    {
                        if (!Extensions.SRSclient.SRSTimerActive)
                        {
                            Extensions.SRSclient.SRS_Timer_Start(); // start 2 sec timer to keep inhibit on.
                        }
                    }
                    else
                    {
                        Extensions.SRSclient.SRS_Timer_Stop();
                    }
                }
            }

            public static void PTT_On_Screen_Notify(TXNode PTTkey)
            {
                string message = "";

                if (!PTTkey.humanplayers.Equals("") && State.activeconfig.MP_ShowOnScreenMessages)
                {
                    string mod = PTTkey.radios[0].modulation;
                    string frq = Helpers.Common.NormalizeFreqString(PTTkey.radios[0].frequency);
                    string players = PTTkey.humanplayers.TrimEnd(",".ToCharArray());

                    message = "Players tuned to " + mod + " " + frq + "MHz: " + players;
                    DcsClient.SendCmdSequence(DcsClient.iCommandsequences.voidmessage, false, message);
                }
            }

            public static void PTT_Manage_ShowOptions(bool keypress)
            {
                try
                {
                    if (keypress)
                    {
                        State.showingoptions = false;
                    }
                    else
                    {
                        if (State.showingoptions)
                        {

                            DcsClient.SendCmdSequence(DcsClient.iCommandsequences.closemenu, false);
                            State.showingoptions = false;
                        }

                        if (Extensions.RIO.helper.showingjestermenu)
                        {
                            Extensions.RIO.helper.ShowWheel(false);
                            Extensions.RIO.helper.showingjestermenu = false;
                        }

                    }
                }
                catch
                {
                }
            }

            public static void PTT_Manage_AOCS_Speech(bool keypress)
            {
                try
                {
                    if (keypress && State.synth.State.Equals(SynthesizerState.Speaking))
                    {
                        State.synth.Pause();
                    }
                    if (!keypress && State.synth.State.Equals(SynthesizerState.Paused))
                    {
                        State.synth.Resume();
                    }
                }
                catch
                {
                }
            }

            public static void PTT_GUI_Update(TXNode PTTkey, bool keypress)
            {
                try
                {
                    if (State.configwindowopen && (State.configurationwindow != null))
                    {

                        if (keypress)
                        {
                            State.configurationwindow.Dispatcher.BeginInvoke((MethodInvoker)delegate
                            {

                                if (State.currentTXnode.enabled)
                                {
                                    State.configurationwindow.updatepttinfo();
                                    State.configurationwindow.setTXcolor();

                                }
                                else // TX node disabled: reset TX info
                                {

                                    State.configurationwindow.TXInfo1.Visibility = System.Windows.Visibility.Hidden;
                                    State.configurationwindow.TXInfo2.Visibility = System.Windows.Visibility.Hidden;
                                    State.configurationwindow.ModuleInfo.Visibility = System.Windows.Visibility.Visible;
                                    State.configurationwindow.EasyCommsInfo.Visibility = System.Windows.Visibility.Visible;
                                    State.configurationwindow.resetTXcolorall();
                                }

                                if (PTTkey.relay)
                                {
                                    State.configurationwindow.Vox_Relay_Hot.Visibility = System.Windows.Visibility.Visible;
                                    State.configurationwindow.Vox_Relay_Cold.Visibility = System.Windows.Visibility.Hidden;
                                }
                                else
                                {
                                    State.configurationwindow.Vox_Relay_Cold.Visibility = System.Windows.Visibility.Visible;
                                    State.configurationwindow.Vox_Relay_Hot.Visibility = System.Windows.Visibility.Hidden;
                                }

                                State.configurationwindow.UpdateAllbugs();

                            });


                        }
                        else
                        {
                            State.configurationwindow.Dispatcher.BeginInvoke((MethodInvoker)delegate
                            {

                                State.configurationwindow.TXInfo1.Visibility = System.Windows.Visibility.Hidden;
                                State.configurationwindow.TXInfo2.Visibility = System.Windows.Visibility.Hidden;
                                State.configurationwindow.ModuleInfo.Visibility = System.Windows.Visibility.Visible;
                                State.configurationwindow.EasyCommsInfo.Visibility = System.Windows.Visibility.Visible;
                                State.configurationwindow.resetTXcolorall();

                                State.configurationwindow.Vox_Relay_Cold.Visibility = System.Windows.Visibility.Visible;
                                State.configurationwindow.Vox_Relay_Hot.Visibility = System.Windows.Visibility.Hidden;

                                State.configurationwindow.UpdateAllbugs();

                            });

                        }

                    }
                }
                catch
                {
                }

            }

        }
    }
}
