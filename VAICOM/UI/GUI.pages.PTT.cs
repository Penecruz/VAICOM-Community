using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using VAICOM.Client;
using VAICOM.Helpers;
using VAICOM.Products;
using VAICOM.PushToTalk;
using VAICOM.Servers;

namespace VAICOM
{

    namespace UI
    {

        public partial class ConfigWindow : Window
        {
            // --- PTT CONFIG PAGE ----------------------------------------
            private const string WagsFundraiserPrompt = "Support Matt Wags Wagners Cancer Fight - Click Here";
            private DispatcherTimer wagsFundraiserMarqueeTimer;
            private int wagsFundraiserMarqueeOffset;

            // bugs & radio colors 

            public void UpdateAllbugs()
            {
                Changechatterbug();
                ChangeOKHostbug();
                Changetransmitbug();
                Changedatabug();
                ChangeS2Mbug();
                ChangeM2Sbug();
                Changesquelchbug();
                Changecounterbug();
                ChangeEasycommsbug();
                setMicKeysInfotext();
                ChangeIselbug();
                ChangeSRSbug();
                ChangeDialbug();
            }
            public Brush RadioColor(PTT.TXNode node)
            {
                Brush returncolor;
                if (!node.enabled || !node.radios[0].on)
                {
                    returncolor = Brushes.DimGray;
                }
                else
                {
                    if (node.radios[0].isSRSserver)
                    {
                        returncolor = Brushes.DimGray;
                    }
                    else
                    {
                        returncolor = Brushes.LightGray;
                    }
                }
                return returncolor;
            }

            public void setTXcolor()
            {

                UpdateAllbugs();

                switch (State.currentTXnode.name)
                {
                    case "TX1":
                        TX1info.Foreground = Brushes.White;
                        break;
                    case "TX2":
                        TX2info.Foreground = Brushes.White;
                        break;
                    case "TX3":
                        TX3info.Foreground = Brushes.White;
                        break;
                    case "TX4":
                        TX4info.Foreground = Brushes.White;
                        break;
                    case "TX5":
                        TX5info.Foreground = Brushes.White;
                        break;
                    case "TX6":
                        TX6info.Foreground = Brushes.White;
                        break;
                }
            }
            public void resetTXcolor()
            {

                UpdateAllbugs();

                switch (State.currentTXnode.name)
                {
                    case "TX1":
                        TX1info.Foreground = RadioColor(PTT.TXNodes.TX1);
                        break;
                    case "TX2":
                        TX2info.Foreground = RadioColor(PTT.TXNodes.TX2);
                        break;
                    case "TX3":
                        TX3info.Foreground = RadioColor(PTT.TXNodes.TX3);
                        break;
                    case "TX4":
                        TX4info.Foreground = RadioColor(PTT.TXNodes.TX4);
                        break;
                    case "TX5":
                        TX5info.Foreground = RadioColor(PTT.TXNodes.TX5);
                        break;
                    case "TX6":
                        TX6info.Foreground = RadioColor(PTT.TXNodes.TX6);
                        break;
                }
            }
            public void resetTXcolorall()
            {

                UpdateAllbugs();

                TX1info.Foreground = RadioColor(PTT.TXNodes.TX1);
                TX2info.Foreground = RadioColor(PTT.TXNodes.TX2);
                TX3info.Foreground = RadioColor(PTT.TXNodes.TX3);
                TX4info.Foreground = RadioColor(PTT.TXNodes.TX4);
                TX5info.Foreground = RadioColor(PTT.TXNodes.TX5);
                TX6info.Foreground = RadioColor(PTT.TXNodes.TX6);
            }
            public void resetTXcolorsafe()
            {

                UpdateAllbugs();

                if (!TX1info.Foreground.Equals(Brushes.White))
                {
                    TX1info.Foreground = RadioColor(PTT.TXNodes.TX1);
                }
                if (!TX2info.Foreground.Equals(Brushes.White))
                {
                    TX2info.Foreground = RadioColor(PTT.TXNodes.TX2);
                }
                if (!TX3info.Foreground.Equals(Brushes.White))
                {
                    TX3info.Foreground = RadioColor(PTT.TXNodes.TX3);
                }
                if (!TX4info.Foreground.Equals(Brushes.White))
                {
                    TX4info.Foreground = RadioColor(PTT.TXNodes.TX4);
                }
                if (!TX5info.Foreground.Equals(Brushes.White))
                {
                    TX5info.Foreground = RadioColor(PTT.TXNodes.TX5);
                }
                if (!TX6info.Foreground.Equals(Brushes.White))
                {
                    TX6info.Foreground = RadioColor(PTT.TXNodes.TX6);
                }
            }

            // bugs

            private void Setchatterbug(object sender, EventArgs e)
            {
                Changechatterbug();
            }
            public void Changechatterbug()
            {
                if (State.chatteractive)
                {
                    Chatterbug.Visibility = Visibility.Visible;
                }
                else
                {
                    Chatterbug.Visibility = Visibility.Hidden;
                }
            }

            private void SetOKHostbug(object sender, EventArgs e)
            {
                ChangeOKHostbug();
            }
            public void ChangeOKHostbug()
            {
                if (State.activeconfig.OpenKneeboard_Out && Extensions.Kneeboard.OpenKneeboardBridge.IsHostRunning)
                {
                    OK_Host.Visibility = Visibility.Visible;
                    OK_Host.Text = "OKB";
                }
                else
                {
                    OK_Host.Visibility = Visibility.Hidden;
                    OK_Host.Text = "OKB";
                }
            }

            public void AlternateOKHostbug()
            {
                if (!(State.activeconfig.OpenKneeboard_Out && Extensions.Kneeboard.OpenKneeboardBridge.IsHostRunning))
                {
                    OK_Host.Text = "OKB";
                    return;
                }

                if (Extensions.Kneeboard.OpenKneeboardBridge.HasActiveConnection)
                {
                    OK_Host.Text = "OKB";
                    return;
                }

                if (OK_Host.Text.Equals(""))
                {
                    OK_Host.Text = "OKB";
                }
                else
                {
                    OK_Host.Text = "";
                }
            }

            private void Settransmitbug(object sender, EventArgs e)
            {
                Changetransmitbug();
            }
            public void Changetransmitbug()
            {
                if (State.transmitting)
                {
                    Transmitbug.Visibility = Visibility.Visible;
                }
                else
                {
                    Transmitbug.Visibility = Visibility.Hidden;
                }
            }

            private void SetS2Mbug(object sender, EventArgs e)
            {
                ChangeS2Mbug();
            }
            public void ChangeS2Mbug()
            {
                if (State.activeconfig.ForceMultiHotkey)
                {
                    S2Mbug.Visibility = Visibility.Visible;
                }
                else
                {
                    S2Mbug.Visibility = Visibility.Hidden;
                }
            }

            private void SetM2Sbug(object sender, EventArgs e)
            {
                ChangeM2Sbug();
            }
            public void ChangeM2Sbug()
            {
                if (State.activeconfig.ForceSingleHotkey)
                {
                    M2Sbug.Visibility = Visibility.Visible;
                }
                else
                {
                    M2Sbug.Visibility = Visibility.Hidden;
                }
            }

            private void Setsquelchbug(object sender, EventArgs e)
            {
                Changesquelchbug();
            }
            public void Changesquelchbug()
            {
                if (State.activeconfig.DisableSquelch)
                {
                    Squelchbug.Visibility = Visibility.Hidden;
                }
                else
                {
                    Squelchbug.Visibility = Visibility.Visible;
                }
            }

            private void Setbeaconbug(object sender, EventArgs e)
            {
                Changebeaconbug();
            }
            public void Changebeaconbug()
            {
                if (State.beaconactive)
                {
                    Beaconbug.Visibility = Visibility.Visible;
                }
                else
                {
                    Beaconbug.Visibility = Visibility.Hidden;
                }
            }
            public void Alternatebeaconbug()
            {
                if (Beaconbug.Text.Equals(""))
                {
                    Beaconbug.Text = "BCN";
                }
                else
                {
                    if (State.beaconlocked)
                    {
                        Beaconbug.Text = "BCN";
                    }
                    else
                    {
                        Beaconbug.Text = "";
                    }
                }
            }

            private void Seteasycommsbug(object sender, EventArgs e)
            {
                ChangeEasycommsbug();
            }
            public void ChangeEasycommsbug()
            {
                if (State.currentstate.easycomms)
                {
                    Easycommsbug.Text = "AUTO";
                }
                else
                {
                    Easycommsbug.Text = "MAN";
                }
            }

            private void SetIselbug(object sender, EventArgs e)
            {
                ChangeIselbug();
            }
            public void ChangeIselbug()
            {
                if (State.activeconfig.UseInstantSelect && 
                    !(State.currentmodule.Equals(DCSmodules.LookupTable["F-5E-3"]) || 
                      State.currentmodule.Equals(DCSmodules.LookupTable["F-5E-3_FC"])))
                {
                    Iselbug.Text = "I-SEL";
                }
                else
                {
                    Iselbug.Text = "";
                }
            }

            private void SetSRSbug(object sender, EventArgs e)
            {
                ChangeSRSbug();
            }
            public void ChangeSRSbug()
            {
                if (State.activeconfig.UseSRSmapping)
                {
                    SRSbug.Text = "SRS";
                }
                else
                {
                    SRSbug.Text = "";
                }
            }

            private void SetDialbug(object sender, EventArgs e)
            {
                ChangeDialbug();
            }
            public void ChangeDialbug()
            {
                if (State.currentmodule.Havedial && (State.currentstate.easycomms || State.activeconfig.OperateDial)) // (State.currentmodule.Singlehotkey && !State.activeconfig.ForceMultiHotkey) 
                {
                    Dialbug.Text = "SLCTR";
                }
                else
                {
                    Dialbug.Text = "";
                }
            }

            private void Setdatabug(object sender, EventArgs e)
            {
                Changedatabug();
            }
            public void Changedatabug()
            {
                string modebug1;
                string modebug2;

                if (State.currentstate.multiplayer)
                {
                    modebug1 = "MP";
                }
                else
                {
                    modebug1 = "SP";
                }

                if (State.currentstate.vrmode)
                {
                    modebug2 = "VR";
                }
                else
                {
                    modebug2 = "DP";
                }


                Databug.Text = modebug1 + " | " + modebug2 + " | " + State.currentstate.dcsversion;
            }

            private void Setcounterbug(object sender, EventArgs e)
            {
                Changecounterbug();
            }
            public void Changecounterbug()
            {
                int counter = 0;

                foreach (KeyValuePair<string, List<Server.DcsUnit>> set in State.currentstate.availablerecipients)
                {
                    if (!set.Key.Equals("Player"))
                    {
                        counter = counter + set.Value.Count;
                    }
                }

                Counterbug.Text = counter.ToString();
            }
            public void Voidcounterbug()
            {
                Counterbug.Text = "";
            }

            private void UpdateSquelch(object sender, RoutedEventArgs e)
            {
                Settings.ConfigFile.WriteConfigToFile(true);
                Changesquelchbug();
            }
            // TX viewers
            private void setTX1text(object sender, EventArgs e)
            {
                string keyname = "TX1";
                string keyenabled;
                string radioname;
                string radioid;

                if (PTT.TXNodes.TX1.enabled)
                {

                    TX1info.Foreground = RadioColor(PTT.TXNodes.TX1);

                    if (PTT.TXNodes.TX1.radios[0].isavailable)
                    {
                        keyenabled = "";
                    }
                    else
                    {
                        keyenabled = "";
                    }
                    radioname = "" + PTT.TXNodes.TX1.radios[0].name;
                    radioid = "";
                }
                else
                {
                    keyenabled = "----";
                    radioname = "";
                    radioid = "";
                }

                string TXinfo = keyname + " - " + keyenabled + "" + radioname + " " + radioid;
                TX1info.Text = TXinfo;

            }
            private void setTX2text(object sender, EventArgs e)
            {
                string keyname = "TX2";
                string keyenabled;
                string radioname;
                string radioid;

                if (PTT.TXNodes.TX2.enabled)
                {
                    TX2info.Foreground = RadioColor(PTT.TXNodes.TX2);

                    if (PTT.TXNodes.TX2.radios[0].isavailable)
                    {
                        keyenabled = "";
                    }
                    else
                    {
                        keyenabled = "";
                    }
                    radioname = "" + PTT.TXNodes.TX2.radios[0].name;
                    radioid = "";
                }
                else
                {
                    keyenabled = "----";
                    radioname = "";
                    radioid = "";
                }

                string TXinfo = keyname + " - " + keyenabled + "" + radioname + " " + radioid;
                TX2info.Text = TXinfo;

            }
            private void setTX3text(object sender, EventArgs e)
            {
                string keyname = "TX3";
                string keyenabled;
                string radioname;
                string radioid;

                if (PTT.TXNodes.TX3.enabled)
                {
                    TX3info.Foreground = RadioColor(PTT.TXNodes.TX3);

                    if (PTT.TXNodes.TX3.radios[0].isavailable)
                    {
                        keyenabled = "";
                    }
                    else
                    {
                        keyenabled = "";
                    }
                    radioname = "" + PTT.TXNodes.TX3.radios[0].name;
                    radioid = "";
                }
                else
                {
                    keyenabled = "----";
                    radioname = "";
                    radioid = "";
                }

                string TXinfo = keyname + " - " + keyenabled + "" + radioname + " " + radioid;
                TX3info.Text = TXinfo;

            }
            private void setTX4text(object sender, EventArgs e)
            {
                string keyname = "TX4";
                string keyenabled;
                string radioname;
                string radioid;

                if (PTT.TXNodes.TX4.enabled)
                {
                    if (PTT.TXNodes.TX4.radios[0].isavailable)
                    {
                        keyenabled = "";
                    }
                    else
                    {
                        keyenabled = "";
                    }
                    radioname = "" + PTT.TXNodes.TX4.radios[0].name;
                    radioid = "";
                }
                else
                {
                    keyenabled = "----";
                    radioname = "";
                    radioid = "";
                }

                string TXinfo = keyname + " - " + keyenabled + "" + radioname + " " + radioid;
                TX4info.Text = TXinfo;

            }
            private void setTX5text(object sender, EventArgs e)
            {
                string keyname = "TX5";
                string keyenabled;
                string radioname;
                string radioid;

                if (PTT.TXNodes.TX5.enabled)
                {
                    if (PTT.TXNodes.TX5.radios[0].isavailable)
                    {
                        keyenabled = "";
                    }
                    else
                    {
                        keyenabled = "";
                    }
                    radioname = "" + PTT.TXNodes.TX5.radios[0].name;
                    radioid = "";
                }
                else
                {
                    keyenabled = "----";
                    radioname = "";
                    radioid = "";
                }

                string TXinfo = keyname + " - " + keyenabled + "" + radioname + " " + radioid;
                TX5info.Text = TXinfo;

            }
            private void setTX6text(object sender, EventArgs e)
            {
                string keyname = "TX6";
                string keyenabled;
                string radioname;
                string radioid;

                if (PTT.TXNodes.TX6.enabled)
                {
                    if (PTT.TXNodes.TX6.radios[0].isavailable)
                    {
                        keyenabled = "";
                    }
                    else
                    {
                        keyenabled = "";
                    }
                    radioname = "" + PTT.TXNodes.TX6.radios[0].name;
                    radioid = "";
                }
                else
                {
                    keyenabled = "----";
                    radioname = "";
                    radioid = "";
                }

                string TXinfo = keyname + " - " + keyenabled + "" + radioname + " " + radioid;
                TX6info.Text = TXinfo;

            }

            private void setModuleInfotext(object sender, EventArgs e)
            {
                string info = Common.GetCurrentModuleDisplayText();
                bool showFundraiser = ShouldShowWagsFundraiserPrompt();
                if (showFundraiser)
                {
                    StartWagsFundraiserMarquee();
                }
                else
                {
                    StopWagsFundraiserMarquee();
                }

                if (!showFundraiser)
                {
                    ModuleInfo.Text = info;
                }

                ModuleInfo.Cursor = showFundraiser ? Cursors.Hand : Cursors.Arrow;
            }

            private void StartWagsFundraiserMarquee()
            {
                if (wagsFundraiserMarqueeTimer == null)
                {
                    wagsFundraiserMarqueeTimer = new DispatcherTimer();
                    wagsFundraiserMarqueeTimer.Interval = TimeSpan.FromMilliseconds(180);
                    wagsFundraiserMarqueeTimer.Tick += WagsFundraiserMarqueeTimer_Tick;
                }

                if (!wagsFundraiserMarqueeTimer.IsEnabled)
                {
                    wagsFundraiserMarqueeOffset = 0;
                    wagsFundraiserMarqueeTimer.Start();
                    WagsFundraiserMarqueeTimer_Tick(null, EventArgs.Empty);
                }
            }

            private void StopWagsFundraiserMarquee()
            {
                if (wagsFundraiserMarqueeTimer != null && wagsFundraiserMarqueeTimer.IsEnabled)
                {
                    wagsFundraiserMarqueeTimer.Stop();
                }
            }

            private void WagsFundraiserMarqueeTimer_Tick(object sender, EventArgs e)
            {
                if (ModuleInfo == null)
                {
                    return;
                }

                string marqueeSource = WagsFundraiserPrompt + "     ";
                string loop = marqueeSource + marqueeSource;
                int viewLength = Math.Min(30, marqueeSource.Length);

                if (wagsFundraiserMarqueeOffset >= marqueeSource.Length)
                {
                    wagsFundraiserMarqueeOffset = 0;
                }

                ModuleInfo.Text = loop.Substring(wagsFundraiserMarqueeOffset, viewLength);
                wagsFundraiserMarqueeOffset++;
            }

            private static bool ShouldShowWagsFundraiserPrompt()
            {
                if (State.activeconfig == null)
                {
                    return false;
                }

                if (State.activeconfig.WagsFundraiserDismissed)
                {
                    return false;
                }

                if (State.currentmodule == null)
                {
                    return true;
                }

                return State.currentmodule.Id.Equals("----", StringComparison.OrdinalIgnoreCase);
            }

            private void ModuleInfo_OpenWagsFundraiser(object sender, MouseButtonEventArgs e)
            {
                try
                {
                    if (!ShouldShowWagsFundraiserPrompt())
                    {
                        return;
                    }

                    const string url = "https://www.gofundme.com/f/support-matt-wags-wagners-cancer-fight?attribution_id=sl:036f7eae-fcd2-4fff-aa34-fdc683faf9a6&lang=en_US&ts=1784293424&utm_campaign=fp_sharesheet&utm_content=amp20_t1&utm_medium=customer&utm_source=copy_link";
                    Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });

                    State.activeconfig.WagsFundraiserDismissed = true;
                    Settings.ConfigFile.WriteConfigToFile(true);
                    StopWagsFundraiserMarquee();

                    if (ModuleInfo != null)
                    {
                        ModuleInfo.Text = Common.GetCurrentModuleDisplayText();
                        ModuleInfo.Cursor = Cursors.Arrow;
                    }
                }
                catch (Exception ex)
                {
                    Log.Write("Unable to open Wags fundraiser link: " + ex.Message, Static.Colors.Warning);
                }
            }

            private void setMicKeysInfotext(object sender, EventArgs e)
            {
                setMicKeysInfotext();
            }
            public void setMicKeysInfotext()
            {


                if (State.currentmodule.Singlehotkey)
                {
                    if (State.activeconfig.ForceMultiHotkey)
                    {

                        MultiMode.Visibility = Visibility.Visible;
                        SingleMode.Visibility = Visibility.Hidden;
                    }
                    else
                    {

                        MultiMode.Visibility = Visibility.Hidden;
                        SingleMode.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    if (State.activeconfig.ForceSingleHotkey)
                    {

                        MultiMode.Visibility = Visibility.Hidden;
                        SingleMode.Visibility = Visibility.Visible;
                    }
                    else
                    {

                        MultiMode.Visibility = Visibility.Visible;
                        SingleMode.Visibility = Visibility.Hidden;
                    }
                }

            }


            private void setEasyCommsText(object sender, EventArgs e)
            {
                string easycomms;

                if (State.currentstate.easycomms)
                { easycomms = "ON"; }
                else
                { easycomms = "OFF"; }

                string info = "Easy Communication: " + easycomms;
                EasyCommsInfo.Text = info;
            }

            private void requestupdate(object sender, EventArgs e)
            {
                DcsClient.SendUpdateRequest();
            }

            private void requestupdate(object sender, MouseButtonEventArgs e)
            {
                DcsClient.SendUpdateRequest();
            }

            private void updateptt(object sender, MouseButtonEventArgs e)
            {
                PTT.PTT_ApplyNewConfig();
                updatepttinfo();
            }

            public void updatepttinfo()
            {
                try
                {

                    string hwhotkeys;

                    if (State.currentmodule.Singlehotkey)
                    { hwhotkeys = "PTT key setup: Single"; }
                    else
                    { hwhotkeys = "PTT key setup: Multi"; }

                    string info1 = hwhotkeys;

                    setMicKeysInfotext();

                    string easycomms;

                    if (State.currentstate.easycomms)
                    { easycomms = "ON"; }
                    else
                    { easycomms = "OFF"; }

                    string info2 = "Easy Communication: " + easycomms;
                    EasyCommsInfo.Text = info2;

                    string info3 = Common.GetCurrentModuleDisplayText();
                    bool showFundraiser = ShouldShowWagsFundraiserPrompt();
                    if (showFundraiser)
                    {
                        StartWagsFundraiserMarquee();
                    }
                    else
                    {
                        StopWagsFundraiserMarquee();
                    }

                    if (!showFundraiser)
                    {
                        ModuleInfo.Text = info3;
                    }

                    ModuleInfo.Cursor = showFundraiser ? Cursors.Hand : Cursors.Arrow;

                    string keyname = "TX1";
                    string keyenabled;
                    string radioname;
                    string radioid;

                    if (PTT.TXNodes.TX1.enabled)
                    {
                        if (PTT.TXNodes.TX1.radios[0].isavailable)
                        {
                            keyenabled = "";
                        }
                        else
                        {
                            keyenabled = "";
                        }
                        radioname = "" + PTT.TXNodes.TX1.radios[0].name;
                        radioid = "";
                    }
                    else
                    {
                        keyenabled = "----";
                        radioname = "";
                        radioid = "";
                    }
                    string TXinfo1 = keyname + " - " + keyenabled + "" + radioname + " " + radioid;
                    TX1info.Text = TXinfo1;

                    keyname = "TX2";
                    if (PTT.TXNodes.TX2.enabled)
                    {
                        if (PTT.TXNodes.TX2.radios[0].isavailable)
                        {
                            keyenabled = "";
                        }
                        else
                        {
                            keyenabled = "";
                        }
                        radioname = "" + PTT.TXNodes.TX2.radios[0].name;
                        radioid = "";
                    }
                    else
                    {
                        keyenabled = "----";
                        radioname = "";
                        radioid = "";
                    }
                    string TXinfo2 = keyname + " - " + keyenabled + "" + radioname + " " + radioid;
                    TX2info.Text = TXinfo2;

                    keyname = "TX3";
                    if (PTT.TXNodes.TX3.enabled)
                    {
                        if (PTT.TXNodes.TX3.radios[0].isavailable)
                        {
                            keyenabled = "";
                        }
                        else
                        {
                            keyenabled = "";
                        }
                        radioname = "" + PTT.TXNodes.TX3.radios[0].name;
                        radioid = "";
                    }
                    else
                    {
                        keyenabled = "----";
                        radioname = "";
                        radioid = "";
                    }
                    string TXinfo3 = keyname + " - " + keyenabled + "" + radioname + " " + radioid;
                    TX3info.Text = TXinfo3;

                    keyname = "TX4";
                    if (PTT.TXNodes.TX4.enabled)
                    {
                        if (PTT.TXNodes.TX4.radios[0].isavailable)
                        {
                            keyenabled = "";
                        }
                        else
                        {
                            keyenabled = "";
                        }
                        radioname = "" + PTT.TXNodes.TX4.radios[0].name;
                        radioid = "";
                    }
                    else
                    {
                        keyenabled = "----";
                        radioname = "";
                        radioid = "";
                    }
                    string TXinfo4 = keyname + " - " + keyenabled + "" + radioname + " " + radioid;
                    TX4info.Text = TXinfo4;

                    keyname = "TX5";
                    if (PTT.TXNodes.TX5.enabled)
                    {
                        if (PTT.TXNodes.TX5.radios[0].isavailable)
                        {
                            keyenabled = "";
                        }
                        else
                        {
                            keyenabled = "";
                        }
                        radioname = "" + PTT.TXNodes.TX5.radios[0].name;
                        radioid = "";
                    }
                    else
                    {
                        keyenabled = "----";
                        radioname = "";
                        radioid = "";
                    }
                    string TXinfo5 = keyname + " - " + keyenabled + "" + radioname + " " + radioid;
                    TX5info.Text = TXinfo5;


                    keyname = "TX6";
                    if (PTT.TXNodes.TX6.enabled)
                    {
                        if (PTT.TXNodes.TX6.radios[0].isavailable)
                        {
                            keyenabled = "";
                        }
                        else
                        {
                            keyenabled = "";
                        }
                        radioname = "" + PTT.TXNodes.TX6.radios[0].name;
                        radioid = "";
                    }
                    else
                    {
                        keyenabled = "----";
                        radioname = "";
                        radioid = "";
                    }
                    string TXinfo6 = keyname + " - " + keyenabled + "" + radioname + " " + radioid;
                    TX6info.Text = TXinfo6;

                    resetTXcolorsafe();

                }
                catch
                {
                }

            }
            // force multi hotkey
            private void ForceMultiHotkeyOn(object sender, RoutedEventArgs e) { State.activeconfig.ForceMultiHotkey = true; PTT.PTT_ApplyNewConfig(); Settings.ConfigFile.WriteConfigToFile(true); updatepttinfo(); setMicKeysInfotext(); ChangeDialbug(); }
            private void ForceMultiHotkeyOff(object sender, RoutedEventArgs e) { State.activeconfig.ForceMultiHotkey = false; PTT.PTT_ApplyNewConfig(); Settings.ConfigFile.WriteConfigToFile(true); updatepttinfo(); setMicKeysInfotext(); ChangeDialbug(); }
            private void SetCurrentValueForceMultiHotkey(object sender, EventArgs e) { ForceMultiHotkey.IsEnabled = true; ForceMultiHotkey.IsChecked = State.activeconfig.ForceMultiHotkey; setMicKeysInfotext(); ChangeDialbug(); }
            // force single hotkey
            private void ForceSingleHotkeyOn(object sender, RoutedEventArgs e) { State.activeconfig.ForceSingleHotkey = true; PTT.PTT_ApplyNewConfig(); Settings.ConfigFile.WriteConfigToFile(true); updatepttinfo(); setMicKeysInfotext(); ChangeDialbug(); }
            private void ForceSingleHotkeyOff(object sender, RoutedEventArgs e) { State.activeconfig.ForceSingleHotkey = false; PTT.PTT_ApplyNewConfig(); Settings.ConfigFile.WriteConfigToFile(true); updatepttinfo(); setMicKeysInfotext(); ChangeDialbug(); }
            private void SetCurrentValueForceSingleHotkey(object sender, EventArgs e) { ForceSingleHotkey.IsEnabled = true; ForceSingleHotkey.IsChecked = State.activeconfig.ForceSingleHotkey; setMicKeysInfotext(); ChangeDialbug(); }

            public void HotkeyUpdateMulti(object sender, RoutedEventArgs e)
            {
                ChangeS2Mbug();
                setMicKeysInfotext();
                ChangeDialbug();
            }

            public void HotkeyUpdateSingle(object sender, RoutedEventArgs e)
            {
                ChangeM2Sbug();
                setMicKeysInfotext();
                ChangeDialbug();
            }
            private void UpdateSingleHotkey(object sender, SelectionChangedEventArgs e)
            {
                try
                {
                    State.activeconfig.SingleHotkey = HotkeySelection.SelectedValue.ToString();
                    PTT.PTT_ApplyNewConfig();
                    Settings.ConfigFile.WriteConfigToFile(true);
                    updatepttinfo();
                }
                catch
                {
                }
            }

            private void HotkeySetInitialValue(object sender, EventArgs e)
            {
                HotkeySelection.IsEnabled = true;
                string displaytext = "---";
                try
                {
                    displaytext = State.activeconfig.SingleHotkey;
                }
                catch
                { }
                HotkeySelection.Text = displaytext;
            }
            private void OperateDialOff(object sender, RoutedEventArgs e)
            {
                State.activeconfig.OperateDial = false;
                ChangeDialbug();
            }
            private void OperateDialOn(object sender, RoutedEventArgs e)
            {
                State.activeconfig.OperateDial = true;
                ChangeDialbug();
            }
            private void SetCurrentValueOperateDial(object sender, EventArgs e)
            {
                OperateDial.IsEnabled = true; OperateDial.IsChecked = State.activeconfig.OperateDial; ChangeDialbug();
            }

            private void Dial_SetMode_Up(object sender, EventArgs e)
            {
                Dial_SetMode_Up();
            }
            private void Dial_SetMode_Dn(object sender, EventArgs e)
            {
                Dial_SetMode_Dn();
            }
            public void Dial_SetMode_Up()
            {

                if (State.activeconfig.OperateDial)
                {
                    Dial_Sw_Up.Visibility = Visibility.Visible;
                }
                else
                {
                    Dial_Sw_Up.Visibility = Visibility.Hidden;
                }
            }
            public void Dial_SetMode_Dn()
            {

                if (State.activeconfig.OperateDial)
                {
                    Dial_Sw_Dn.Visibility = Visibility.Hidden;
                }
                else
                {
                    Dial_Sw_Dn.Visibility = Visibility.Visible;
                }
            }

            private void Selector_Init(object sender, EventArgs e)
            {
                if (State.activeconfig.SelectorMode == null)
                {
                    State.activeconfig.SelectorMode = 2;

                    if (State.activeconfig.ForceMultiHotkey)
                    {
                        State.activeconfig.SelectorMode = 3;
                    }
                    if (State.activeconfig.ForceSingleHotkey)
                    {
                        State.activeconfig.SelectorMode = 4;
                    }
                    if (State.activeconfig.ForceMultiHotkey && State.activeconfig.ForceSingleHotkey)
                    {
                        State.activeconfig.SelectorMode = 1;
                    }

                }

                Selector_SetMode();
            }
            public void Selector_SetMode()
            {

                if (State.activeconfig.SelectorMode.Equals(1)) // INV
                {
                    PTT_BG.Visibility = Visibility.Hidden;
                    PTT_BG_INV.Visibility = Visibility.Visible;
                    PTT_BG_NORM.Visibility = Visibility.Hidden;
                    PTT_BG_MULTI.Visibility = Visibility.Hidden;
                    PTT_BG_SNGL.Visibility = Visibility.Hidden;

                    State.activeconfig.ForceSingleHotkey = true;
                    State.activeconfig.ForceMultiHotkey = true;

                }

                if (State.activeconfig.SelectorMode.Equals(2))
                {
                    PTT_BG.Visibility = Visibility.Hidden;
                    PTT_BG_INV.Visibility = Visibility.Hidden;
                    PTT_BG_NORM.Visibility = Visibility.Visible;
                    PTT_BG_MULTI.Visibility = Visibility.Hidden;
                    PTT_BG_SNGL.Visibility = Visibility.Hidden;


                    State.activeconfig.ForceSingleHotkey = false;
                    State.activeconfig.ForceMultiHotkey = false;

                }

                if (State.activeconfig.SelectorMode.Equals(3)) // MULTI
                {
                    PTT_BG.Visibility = Visibility.Hidden;
                    PTT_BG_INV.Visibility = Visibility.Hidden;
                    PTT_BG_NORM.Visibility = Visibility.Hidden;
                    PTT_BG_MULTI.Visibility = Visibility.Visible;
                    PTT_BG_SNGL.Visibility = Visibility.Hidden;

                    State.activeconfig.ForceSingleHotkey = false;
                    State.activeconfig.ForceMultiHotkey = true;

                }

                if (State.activeconfig.SelectorMode.Equals(4)) // SNGL
                {
                    PTT_BG.Visibility = Visibility.Hidden;
                    PTT_BG_INV.Visibility = Visibility.Hidden;
                    PTT_BG_NORM.Visibility = Visibility.Hidden;
                    PTT_BG_MULTI.Visibility = Visibility.Hidden;
                    PTT_BG_SNGL.Visibility = Visibility.Visible;

                    State.activeconfig.ForceSingleHotkey = true;
                    State.activeconfig.ForceMultiHotkey = false;

                }

                Settings.ConfigFile.WriteConfigToFile(true);
                PTT.PTT_ApplyNewConfig();
                updatepttinfo();
                UpdateAllbugs();

            }
            private void Selector_Prev(object sender, MouseButtonEventArgs e)
            {
                Selector_Prev();
            }
            public void Selector_Prev()
            {
                if (State.activeconfig.SelectorMode > 1)
                {
                    State.activeconfig.SelectorMode = State.activeconfig.SelectorMode - 1;
                    Selector_SetMode();
                    UI.Playsound.Dial_Click();
                }
            }
            private void Selector_Next(object sender, MouseButtonEventArgs e)
            {
                Selector_Next();
            }
            public void Selector_Next()
            {

                if (State.activeconfig.SelectorMode < 4)
                {
                    State.activeconfig.SelectorMode = State.activeconfig.SelectorMode + 1;
                    Selector_SetMode();
                    UI.Playsound.Dial_Click();
                }
            }

            private void Page_Up(object sender, MouseButtonEventArgs e)
            {
                Page_Up();
            }
            public void Page_Up()
            {

                if (PTT.IsPTTModeSingle())
                {

                    try
                    {
                        Int32 txnumber;
                        Int32.TryParse(State.activeconfig.SingleHotkey.Replace("TX", ""), out txnumber);

                        if (txnumber > 1)
                        {
                            txnumber = txnumber - 1;
                            State.activeconfig.SingleHotkey = "TX" + txnumber.ToString();
                        }

                        PTT.PTT_ApplyNewConfig();
                        Settings.ConfigFile.WriteConfigToFile(true);
                        UI.Playsound.Gum_Soft();
                        updatepttinfo();
                    }
                    catch
                    {
                    }
                }
                else // multi
                {
                    UI.Playsound.Gum_Soft();
                }
            }
            private void Page_Dn(object sender, MouseButtonEventArgs e)
            {
                Page_Dn();
            }
            public void Page_Dn()
            {

                if (PTT.IsPTTModeSingle())
                {
                    try
                    {
                        Int32 txnumber;
                        Int32.TryParse(State.activeconfig.SingleHotkey.Replace("TX", ""), out txnumber);

                        if (txnumber < 6)
                        {
                            txnumber = txnumber + 1;
                            State.activeconfig.SingleHotkey = "TX" + txnumber.ToString();
                        }

                        PTT.PTT_ApplyNewConfig();
                        Settings.ConfigFile.WriteConfigToFile(true);
                        UI.Playsound.Gum_Soft();
                        updatepttinfo();
                    }
                    catch
                    {
                    }
                }
                else // multi
                {
                    UI.Playsound.Gum_Soft();
                }

            }

            private void Dial_Sw_to_Down(object sender, MouseButtonEventArgs e)
            {
                Dial_Sw_to_Down();
            }
            public void Dial_Sw_to_Down()
            {
                Dial_Sw_Up.Visibility = Visibility.Hidden;
                Dial_Sw_Dn.Visibility = Visibility.Visible;
                UI.Playsound.Soft_Switch();
                State.activeconfig.OperateDial = false;
                DcsClient.SendSettingsChange();
                Settings.ConfigFile.WriteConfigToFile(true);
                ChangeDialbug();
            }
            private void Dial_Sw_to_Up(object sender, MouseButtonEventArgs e)
            {
                Dial_Sw_to_Up();
            }
            public void Dial_Sw_to_Up()
            {
                Dial_Sw_Up.Visibility = Visibility.Visible;
                Dial_Sw_Dn.Visibility = Visibility.Hidden;
                UI.Playsound.Soft_Switch();
                State.activeconfig.OperateDial = true;
                DcsClient.SendSettingsChange();
                Settings.ConfigFile.WriteConfigToFile(true);
                ChangeDialbug();
            }

            public void Dictate_set_relay_Light(bool on)
            {
                if (on)
                {
                    Vox_Relay_Hot.Visibility = Visibility.Visible;
                    Vox_Relay_Cold.Visibility = Visibility.Hidden;
                }
                else
                {
                    Vox_Relay_Hot.Visibility = Visibility.Hidden;
                    Vox_Relay_Cold.Visibility = Visibility.Visible;
                }
            }

            public void Dictate_set_relhot_Light(bool on)
            {
                if (on)
                {
                    Hotmic_Sw_Up.Visibility = Visibility.Visible;
                    Hotmic_Sw_Dn.Visibility = Visibility.Hidden;
                }
                else
                {
                    Hotmic_Sw_Up.Visibility = Visibility.Hidden;
                    Hotmic_Sw_Dn.Visibility = Visibility.Visible;
                }
            }

            public void PTT_Release_Hot_Toggle()
            {
                if (State.activeconfig.ReleaseHot)
                {
                    Hotmic_Sw_to_Down();
                }
                else
                {
                    Hotmic_Sw_to_Up();
                }

            }

            private void Hotmic_Sw_Dn_init(object sender, EventArgs e)
            {
                Hotmic_SetMode_Dn();
            }
            private void Hotmic_Sw_Up_init(object sender, EventArgs e)
            {
                Hotmic_SetMode_Up();
            }

            public void Hotmic_SetMode_Up()
            {

                if (State.activeconfig.ReleaseHot)
                {
                    Hotmic_Sw_Up.Visibility = Visibility.Visible;
                }
                else
                {
                    Hotmic_Sw_Up.Visibility = Visibility.Hidden;
                }
            }
            public void Hotmic_SetMode_Dn()
            {
                if (State.activeconfig.ReleaseHot)
                {
                    Hotmic_Sw_Dn.Visibility = Visibility.Hidden;
                }
                else
                {
                    Hotmic_Sw_Dn.Visibility = Visibility.Visible;
                }
            }

            private void Hotmic_Sw_to_Dn(object sender, MouseButtonEventArgs e)
            {
                Hotmic_Sw_to_Down();
            }
            public void Hotmic_Sw_to_Down()
            {

                Hotmic_Sw_Up.Visibility = Visibility.Hidden;
                Hotmic_Sw_Dn.Visibility = Visibility.Visible;
                UI.Playsound.Soft_Switch();
                State.activeconfig.ReleaseHot = false;
                Settings.ConfigFile.WriteConfigToFile(true);
                State.Proxy.Command.SetSessionEnabledByCategory("Keyword Collections", true);
                State.Proxy.Command.SetSessionEnabledByCategory("Extension packs", true);

                if (State.IsCrewHotMicActive()) //  
                {
                    State.Proxy.State.SetListeningEnabled(true);
                }
                else
                {
                    State.Proxy.State.SetListeningEnabled(false);
                }
            }

            private void Hotmic_Sw_to_Up(object sender, MouseButtonEventArgs e)
            {
                Hotmic_Sw_to_Up();
            }
            public void Hotmic_Sw_to_Up()
            {
                Hotmic_Sw_Up.Visibility = Visibility.Visible;
                Hotmic_Sw_Dn.Visibility = Visibility.Hidden;
                UI.Playsound.Soft_Switch();
                State.activeconfig.ReleaseHot = true;
                Settings.ConfigFile.WriteConfigToFile(true);
                State.Proxy.State.SetListeningEnabled(true);

                if (State.IsCrewHotMicActive()) //  
                {
                    State.Proxy.Command.SetSessionEnabledByCategory("Keyword Collections", true);
                    State.Proxy.Command.SetSessionEnabledByCategory("Extension packs", true);
                }
                else
                {
                    State.Proxy.Command.SetSessionEnabledByCategory("Keyword Collections", false);
                    State.Proxy.Command.SetSessionEnabledByCategory("Extension packs", false);
                    if (State.Proxy.GetProfileName().ToLower().Contains(State.defProfileName.ToLower()))
                    {
                        State.Proxy.Command.SetSessionEnabled("Chatter", true);
                    }
                }
            }

            private void SwitchPage_Push(object sender, MouseButtonEventArgs e)
            {
                Databug.Text = "";
                Easycommsbug.Text = "";
            }
            private void SwitchPage_Release(object sender, MouseButtonEventArgs e)
            {
                UpdateAllbugs();
            }

            private void Toggle_SRS_Mode_On_Init(object sender, EventArgs e)
            {
                Toggle_SRS_Mode_On_Init();
            }
            public void Toggle_SRS_Mode_On_Init()
            {
                SRS_Mode_Toggle_On.IsEnabled = true;
                if (State.activeconfig.UseSRSmapping)
                {
                    SRS_Mode_Toggle_On.Visibility = Visibility.Visible;
                }
                else
                {
                    SRS_Mode_Toggle_On.Visibility = Visibility.Hidden;
                }
            }

            private void Toggle_SRS_Mode_Off_Init(object sender, EventArgs e)
            {
                Toggle_SRS_Mode_Off_Init();
            }
            public void Toggle_SRS_Mode_Off_Init()
            {
                SRS_Mode_Toggle_Off.IsEnabled = true;
                if (State.activeconfig.UseSRSmapping)
                {
                    SRS_Mode_Toggle_Off.Visibility = Visibility.Hidden;
                }
                else
                {
                    SRS_Mode_Toggle_Off.Visibility = Visibility.Visible;
                }
            }

            private void Toggle_SRS_Mode(object sender, MouseButtonEventArgs e)
            {
                Toggle_SRS_Mode();
            }
            public void Switch_SRS()
            {

                if (State.activeconfig.UseSRSmapping)
                {
                    SRS_Mode_Toggle_On.Visibility = Visibility.Visible;
                    SRS_Mode_Toggle_Off.Visibility = Visibility.Hidden;
                }
                else
                {
                    SRS_Mode_Toggle_Off.Visibility = Visibility.Visible;
                    SRS_Mode_Toggle_On.Visibility = Visibility.Hidden;
                }

            }
            public void Toggle_SRS_Mode()
            {
                State.activeconfig.UseSRSmapping = !State.activeconfig.UseSRSmapping;
                Settings.ConfigFile.WriteConfigToFile(true);
                UseSRSMapping.IsChecked = State.activeconfig.UseSRSmapping;
                UI.Playsound.Soft_Click();
                Switch_SRS();
                PTT.PTT_ApplyNewConfig();
                updatepttinfo();
                ChangeSRSbug();
            }

            private void Set_Volume_init(object sender, EventArgs e)
            {
                Set_Volume_init();
            }
            public void Set_Volume_init()
            {
                Volume_Knob.IsEnabled = true;
                Double vol = State.activeconfig.ChatterVolume;
                RotateTransform RotateTransform = new RotateTransform();
                RotateTransform.Angle = -150 + 300 * vol;
                Volume_Knob.RenderTransform = RotateTransform;
                Volume_Knob.ToolTip = "Chatter volume";
            }

            private void Set_Volume_Start(object sender, MouseButtonEventArgs e)
            {
                Mouse.Capture(Volume_Knob);

            }
            private void Set_Volume_Stop(object sender, MouseButtonEventArgs e)
            {
                Mouse.Capture(null);
            }
            private void Set_Volume_Change(object sender, MouseEventArgs e)
            {
                if (Mouse.Captured == Volume_Knob)
                {

                    Point currentLocation = Mouse.GetPosition(this);
                    Point knobCenter = new Point(Volume_Knob.Margin.Left + (Volume_Knob.ActualWidth / 2), Volume_Knob.Margin.Top + (Volume_Knob.ActualHeight / 2));

                    double delta = knobCenter.Y - currentLocation.Y;

                    if (delta > 150)
                    {
                        delta = 150;
                    }

                    if (delta < -150)
                    {
                        delta = -150;
                    }

                    Volume_Knob.RenderTransform = new RotateTransform() { Angle = delta };

                    State.activeconfig.ChatterVolume = (float)(0.5 + (delta / 300));
                    Settings.ConfigFile.WriteConfigToFile(true);
                }
            }

            private void Button_Init_TX1(object sender, EventArgs e)
            {
                Button_Top_TX1.IsEnabled = true;
            }
            private void Button_Press_TX1(object sender, MouseButtonEventArgs e)
            {
                UI.Playsound.Gum_Soft();
                if (State.activeconfig.MouseExternalTX)
                {
                    State.Proxy.ExecuteCommand("Transmit TX1 press");
                }
                else
                {
                    PTT.PTT_Handler(State.Proxy, PTT.TXNodes.TX1, true, true);
                }
            }
            private void Button_Press_TX1(object sender, MouseEventArgs e)
            {
                if (e.LeftButton.Equals(MouseButtonState.Pressed) || e.RightButton.Equals(MouseButtonState.Pressed))
                {
                    if (State.activeconfig.MouseExternalTX)
                    {
                        State.Proxy.ExecuteCommand("Transmit TX1 press");
                    }
                    else
                    {
                        PTT.PTT_Handler(State.Proxy, PTT.TXNodes.TX1, true, true);
                    }
                }
            }
            private void Button_Release_TX1(object sender, MouseButtonEventArgs e)
            {
                if (State.activeconfig.MouseExternalTX)
                {
                    State.Proxy.ExecuteCommand("Transmit TX1 release");
                }
                else
                {
                    PTT.PTT_Handler(State.Proxy, PTT.TXNodes.TX1, false, true);
                }

            }
            private void Button_Release_TX1(object sender, MouseEventArgs e)
            {
                if (e.LeftButton.Equals(MouseButtonState.Pressed) || e.RightButton.Equals(MouseButtonState.Pressed))
                {
                    if (State.activeconfig.MouseExternalTX)
                    {
                        State.Proxy.ExecuteCommand("Transmit TX1 release");
                    }
                    else
                    {
                        PTT.PTT_Handler(State.Proxy, PTT.TXNodes.TX1, false, true);
                    }
                }
                resetTXcolorall();
            }

            private void Button_Init_TX2(object sender, EventArgs e)
            {
                Button_Top_TX2.IsEnabled = true;
            }
            private void Button_Press_TX2(object sender, MouseButtonEventArgs e)
            {
                UI.Playsound.Gum_Soft();
                if (State.activeconfig.MouseExternalTX)
                {
                    State.Proxy.ExecuteCommand("Transmit TX2 press");
                }
                else
                {
                    PTT.PTT_Handler(State.Proxy, PTT.TXNodes.TX2, true, true);
                }
            }
            private void Button_Press_TX2(object sender, MouseEventArgs e)
            {
                if (e.LeftButton.Equals(MouseButtonState.Pressed) || e.RightButton.Equals(MouseButtonState.Pressed))
                {

                    if (State.activeconfig.MouseExternalTX)
                    {
                        State.Proxy.ExecuteCommand("Transmit TX2 press");
                    }
                    else
                    {
                        PTT.PTT_Handler(State.Proxy, PTT.TXNodes.TX2, true, true);
                    }
                }
            }
            private void Button_Release_TX2(object sender, MouseButtonEventArgs e)
            {
                if (State.activeconfig.MouseExternalTX)
                {
                    State.Proxy.ExecuteCommand("Transmit TX2 release");
                }
                else
                {
                    PTT.PTT_Handler(State.Proxy, PTT.TXNodes.TX2, false, true);
                }
            }
            private void Button_Release_TX2(object sender, MouseEventArgs e)
            {
                if (e.LeftButton.Equals(MouseButtonState.Pressed) || e.RightButton.Equals(MouseButtonState.Pressed))
                {
                    if (State.activeconfig.MouseExternalTX)
                    {
                        State.Proxy.ExecuteCommand("Transmit TX2 release");
                    }
                    else
                    {
                        PTT.PTT_Handler(State.Proxy, PTT.TXNodes.TX2, false, true);
                    }
                }
                resetTXcolorall();
            }

            private void Button_Init_TX3(object sender, EventArgs e)
            {
                Button_Top_TX3.IsEnabled = true;
            }
            private void Button_Press_TX3(object sender, MouseButtonEventArgs e)
            {
                UI.Playsound.Gum_Soft();

                if (State.activeconfig.MouseExternalTX)
                {
                    State.Proxy.ExecuteCommand("Transmit TX3 press");
                }
                else
                {
                    PTT.PTT_Handler(State.Proxy, PTT.TXNodes.TX3, true, true);
                }
            }
            private void Button_Press_TX3(object sender, MouseEventArgs e)
            {
                if (e.LeftButton.Equals(MouseButtonState.Pressed) || e.RightButton.Equals(MouseButtonState.Pressed))
                {
                    if (State.activeconfig.MouseExternalTX)
                    {
                        State.Proxy.ExecuteCommand("Transmit TX3 press");
                    }
                    else
                    {
                        PTT.PTT_Handler(State.Proxy, PTT.TXNodes.TX3, true, true);
                    }
                }
            }
            private void Button_Release_TX3(object sender, MouseButtonEventArgs e)
            {

                if (State.activeconfig.MouseExternalTX)
                {
                    State.Proxy.ExecuteCommand("Transmit TX3 release");
                }
                else
                {
                    PTT.PTT_Handler(State.Proxy, PTT.TXNodes.TX3, false, true);
                }
            }
            private void Button_Release_TX3(object sender, MouseEventArgs e)
            {
                if (e.LeftButton.Equals(MouseButtonState.Pressed) || e.RightButton.Equals(MouseButtonState.Pressed))
                {
                    if (State.activeconfig.MouseExternalTX)
                    {
                        State.Proxy.ExecuteCommand("Transmit TX3 release");
                    }
                    else
                    {
                        PTT.PTT_Handler(State.Proxy, PTT.TXNodes.TX3, false, true);
                    }
                }
                resetTXcolorall();
            }

            private void Button_Init_TX4(object sender, EventArgs e)
            {
                Button_Top_TX4.IsEnabled = true;
            }
            private void Button_Press_TX4(object sender, MouseButtonEventArgs e)
            {
                UI.Playsound.Gum_Soft();
                if (State.activeconfig.MouseExternalTX)
                {
                    State.Proxy.ExecuteCommand("Transmit TX4 press");
                }
                else
                {
                    PTT.PTT_Handler(State.Proxy, PTT.TXNodes.TX4, true, true);
                }
            }
            private void Button_Press_TX4(object sender, MouseEventArgs e)
            {
                if (e.LeftButton.Equals(MouseButtonState.Pressed) || e.RightButton.Equals(MouseButtonState.Pressed))
                {
                    if (State.activeconfig.MouseExternalTX)
                    {
                        State.Proxy.ExecuteCommand("Transmit TX4 press");
                    }
                    else
                    {
                        PTT.PTT_Handler(State.Proxy, PTT.TXNodes.TX4, true, true);
                    }
                }
            }
            private void Button_Release_TX4(object sender, MouseButtonEventArgs e)
            {
                if (State.activeconfig.MouseExternalTX)
                {
                    State.Proxy.ExecuteCommand("Transmit TX4 release");
                }
                else
                {
                    PTT.PTT_Handler(State.Proxy, PTT.TXNodes.TX4, false, true);
                }
            }
            private void Button_Release_TX4(object sender, MouseEventArgs e)
            {
                if (e.LeftButton.Equals(MouseButtonState.Pressed) || e.RightButton.Equals(MouseButtonState.Pressed))
                {
                    if (State.activeconfig.MouseExternalTX)
                    {
                        State.Proxy.ExecuteCommand("Transmit TX4 release");
                    }
                    else
                    {
                        PTT.PTT_Handler(State.Proxy, PTT.TXNodes.TX4, false, true);
                    }
                }
                resetTXcolorall();
            }

            private void Button_Init_TX5(object sender, EventArgs e)
            {
                Button_Top_TX5.IsEnabled = true;
            }
            private void Button_Press_TX5(object sender, MouseButtonEventArgs e)
            {
                UI.Playsound.Gum_Soft();
                if (State.activeconfig.MouseExternalTX)
                {
                    State.Proxy.ExecuteCommand("Transmit TX5 press");
                }
                else
                {
                    PTT.PTT_Handler(State.Proxy, PTT.TXNodes.TX5, true, true);
                }
            }
            private void Button_Press_TX5(object sender, MouseEventArgs e)
            {
                if (e.LeftButton.Equals(MouseButtonState.Pressed) || e.RightButton.Equals(MouseButtonState.Pressed))
                {
                    if (State.activeconfig.MouseExternalTX)
                    {
                        State.Proxy.ExecuteCommand("Transmit TX5 press");
                    }
                    else
                    {
                        PTT.PTT_Handler(State.Proxy, PTT.TXNodes.TX5, true, true);
                    }
                }
            }
            private void Button_Release_TX5(object sender, MouseButtonEventArgs e)
            {
                if (State.activeconfig.MouseExternalTX)
                {
                    State.Proxy.ExecuteCommand("Transmit TX5 release");
                }
                else
                {
                    PTT.PTT_Handler(State.Proxy, PTT.TXNodes.TX5, false, true);
                }
            }
            private void Button_Release_TX5(object sender, MouseEventArgs e)
            {
                if (e.LeftButton.Equals(MouseButtonState.Pressed) || e.RightButton.Equals(MouseButtonState.Pressed))
                {
                    if (State.activeconfig.MouseExternalTX)
                    {
                        State.Proxy.ExecuteCommand("Transmit TX5 release");
                    }
                    else
                    {
                        PTT.PTT_Handler(State.Proxy, PTT.TXNodes.TX5, false, true);
                    }
                }
                resetTXcolorall();
            }

            private void Button_Init_TX6(object sender, EventArgs e)
            {
                Button_Top_TX6.IsEnabled = true;
            }
            private void Button_Press_TX6(object sender, MouseButtonEventArgs e)
            {
                UI.Playsound.Gum_Soft();
                if (State.activeconfig.MouseExternalTX)
                {
                    State.Proxy.ExecuteCommand("Transmit TX6 press");
                }
                else
                {
                    PTT.PTT_Handler(State.Proxy, PTT.TXNodes.TX6, true, true);
                }
            }
            private void Button_Press_TX6(object sender, MouseEventArgs e)
            {
                if (e.LeftButton.Equals(MouseButtonState.Pressed) || e.RightButton.Equals(MouseButtonState.Pressed))
                {
                    if (State.activeconfig.MouseExternalTX)
                    {
                        State.Proxy.ExecuteCommand("Transmit TX6 press");
                    }
                    else
                    {
                        PTT.PTT_Handler(State.Proxy, PTT.TXNodes.TX6, true, true);
                    }
                }
            }
            private void Button_Release_TX6(object sender, MouseButtonEventArgs e)
            {
                if (State.activeconfig.MouseExternalTX)
                {
                    State.Proxy.ExecuteCommand("Transmit TX6 release");
                }
                else
                {
                    PTT.PTT_Handler(State.Proxy, PTT.TXNodes.TX6, false, true);
                }
            }
            private void Button_Release_TX6(object sender, MouseEventArgs e)
            {
                if (e.LeftButton.Equals(MouseButtonState.Pressed) || e.RightButton.Equals(MouseButtonState.Pressed))
                {
                    if (State.activeconfig.MouseExternalTX)
                    {
                        State.Proxy.ExecuteCommand("Transmit TX6 release");
                    }
                    else
                    {
                        PTT.PTT_Handler(State.Proxy, PTT.TXNodes.TX6, false, true);
                    }
                }
                resetTXcolorall();
            }

            // ------------------------------------------------------------
        }
    }
}



