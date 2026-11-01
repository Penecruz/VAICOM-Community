using System;
using System.Collections.Generic;
using System.Windows.Forms;
using VAICOM.PushToTalk;
using VAICOM.Servers;
using VAICOM.Static;

namespace VAICOM
{
    namespace Client
    {

        public partial class DcsClient
        {

            public static void Assign_Tuned_Units_to_Radios()
            {

                try
                {
                    foreach (Server.RadioDevice radio in State.currentstate.radios)
                    {
                        radio.tunedunits = new List<Server.DcsUnit>();

                        if (!radio.intercom && radio.on)
                        {
                            string radiomod = radio.modulation;
                            string radiofreq = Helpers.Common.NormalizeFreqString(radio.frequency); //is this helper grabbing the correct ,frequency??
                            foreach (KeyValuePair<string, List<Server.DcsUnit>> cat in State.currentstate.availablerecipients)
                            {
                                if (!cat.Key.Equals("Player") & !cat.Key.Equals("Crew") & !cat.Key.Equals("Cargo")) // &!cat.Key.Equals("Aux")
                                {
                                    foreach (Server.DcsUnit unit in cat.Value)
                                    {
                                        bool potentialunit = !unit.freq.Equals(null) & !unit.freq.Equals(0) & !unit.id_.Equals(State.currentstate.availablerecipients["Player"][0].id_) & unit.allowtuning;
                                        if (potentialunit)
                                        {
                                            if (Helpers.Common.NormalizeFreqString(unit.freq).Equals(radiofreq) & (unit.mod.Equals(radiomod) || unit.mod.Equals("XX")))
                                            {
                                                radio.tunedunits.Add(unit);
                                            }

                                            foreach (string altfr in unit.altfreq)
                                            {
                                                if (Helpers.Common.NormalizeFreqString(altfr).Equals(radiofreq) & unit.mod.Equals(radiomod))
                                                {
                                                    radio.tunedunits.Add(unit);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Write("Set tuned unit error: " + e.Message, Colors.Inline);
                }

            }

            public static void CorrectCallsigns(Server.DcsUnit unit)
            {
                if (unit.callsign.Equals("unknown"))
                {
                    unit.callsign = unit.fullname;
                }
                unit.callsign = unit.callsign.Replace("class", "");
                unit.callsign = unit.callsign.Replace("CV 1143.5 ", "");
                unit.callsign = unit.callsign.Replace("CVN-70 ", "");
                unit.callsign = unit.callsign.Replace("LHA-1 ", "");
                unit.callsign = unit.callsign.Replace("FFG-7CL ", "");
                unit.callsign = unit.callsign.Replace("CG-60 ", "");
                unit.callsign = unit.callsign.Replace("CVN-71 ", "");
                unit.callsign = unit.callsign.Replace("CVN-72 ", "");
                unit.callsign = unit.callsign.Replace("CVN-73 ", "");
                unit.callsign = unit.callsign.Replace("CVN-74 ", "");

            }

            public static List<Server.DcsUnit> GetTunedUnitsForTX(PTT.TXNode TX, bool quiet)
            {

                //Log.Write("getting units for " + TX.name, Colors.Text);

                List<Server.DcsUnit> tunedforTX = new List<Server.DcsUnit>();

                if (!TX.enabled || TX.name.Contains("TX5") || TX.name.Contains("TX6"))
                    {

                    TX.tunedforai = TX.name.Contains("TX5");
                    TX.tunedforhuman = false;
                    TX.humanplayers = "";
                    TX.tunecounter = 0;
                    TX.relay = false;

                    return tunedforTX;
                }

                TX.displaytunedunit = "";

                if (State.currentstate.radios.Count != 0) // not fc3
                {

                    try // main block
                    {
                        string logcolor = Colors.Text;

                        TX.tunedforai = false;
                        TX.tunedforhuman = false;
                        TX.humanplayers = "";
                        TX.tunecounter = 0;

                        foreach (PTT.RadioDevice TXradio in TX.radios) // usually just 0, for SEL can be multiple
                        {
                            foreach (Server.RadioDevice modradio in State.currentstate.radios)
                            {
                                if (modradio.deviceid.Equals(TXradio.deviceid) & !modradio.intercom)
                                {

                                    foreach (Server.DcsUnit tunedunit in modradio.tunedunits)
                                    {
                                        tunedforTX.Add(tunedunit);
                                        TX.tunecounter = TX.tunecounter + 1;

                                        string writestr = TX.name + " | " + TX.radios[0].name + " " + modradio.modulation + " " + Helpers.Common.NormalizeFreqString(modradio.frequency) + " MHz tuned for " + tunedunit.descr + " ";
                                        CorrectCallsigns(tunedunit);
                                        writestr = writestr + tunedunit.callsign;
                                        if (tunedunit.ishuman) // for human unit
                                        {
                                            writestr = writestr + " [player " + tunedunit.playerid + "]";
                                            TX.tunedforhuman = true;
                                            TX.humanplayers += " " + tunedunit.playerid + " [" + tunedunit.callsign + "],";
                                        }
                                        else // for AI
                                        {
                                            writestr = writestr + " [AI]";
                                            TX.tunedforai = true;
                                        }

                                        if (!quiet)
                                        {
                                            Log.Write(writestr, logcolor);
                                        }

                                        if (!TX.Equals(PTT.TXNodes.TX4) && TX.displaytunedunit.Equals("")) // show first unit from tuned list
                                        {
                                            TX.displaytunedunit += "[" + tunedunit.callsign + "]";
                                            if (TX.tunedforhuman)
                                            {
                                                TX.displaytunedunit += "*";
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (TX.tunecounter.Equals(0) && !quiet)
                        {
                            Log.Write(TX.name + " | " + TX.radios[0].name + " " + TX.radios[0].modulation + " " + Helpers.Common.NormalizeFreqString(TX.radios[0].frequency) + " MHz: no tuned units.", logcolor);
                        }

                    }
                    catch
                    {
                    }

                }

                return tunedforTX;
            }

            public static void ShowTunedUnitsForTX(PTT.TXNode TX)
            {
                try
                {
                    try
                    {
                        UI.VaicomOverlayWindow.TxInfo1Text = TX.radios[0].name;
                        UI.VaicomOverlayWindow.TxInfo2Text =
                            ((TX.Equals(PTT.TXNodes.TX1) || TX.Equals(PTT.TXNodes.TX2) || TX.Equals(PTT.TXNodes.TX3)) && !TX.radios[0].isSRSserver)
                            ? TX.radios[0].modulation + " " + Helpers.Common.NormalizeFreqString(TX.radios[0].frequency) + " MHz " + TX.displaytunedunit
                            : "----";
                        UI.VaicomOverlayWindow.TxInfoVisible = State.dcsrunning && !State.currentmodule.IsFC;
                    }
                    catch { }

                    if (State.configwindowopen && (State.configurationwindow != null))
                    {
                        State.configurationwindow.Dispatcher.BeginInvoke((MethodInvoker)delegate
                        {

                            State.configurationwindow.TXInfo1.Text = TX.radios[0].name;
                            if ((TX.Equals(PTT.TXNodes.TX1) || TX.Equals(PTT.TXNodes.TX2) || TX.Equals(PTT.TXNodes.TX3)) && !TX.radios[0].isSRSserver)
                            {
                                State.configurationwindow.TXInfo2.Text = TX.radios[0].modulation + " " + Helpers.Common.NormalizeFreqString(TX.radios[0].frequency) + " MHz " + TX.displaytunedunit;
                            }
                            else
                            {
                                State.configurationwindow.TXInfo2.Text = "----";
                            }

                            if (State.dcsrunning)
                            {
                                if (!State.currentmodule.IsFC) // show info only for non fc3
                                {
                                    State.configurationwindow.TXInfo1.Visibility = System.Windows.Visibility.Visible;
                                    State.configurationwindow.TXInfo2.Visibility = System.Windows.Visibility.Visible;
                                    State.configurationwindow.ModuleInfo.Visibility = System.Windows.Visibility.Hidden;
                                    State.configurationwindow.EasyCommsInfo.Visibility = System.Windows.Visibility.Hidden;
                                }
                            }
                            else
                            {
                                State.configurationwindow.TXInfo1.Visibility = System.Windows.Visibility.Hidden;
                                State.configurationwindow.TXInfo2.Visibility = System.Windows.Visibility.Hidden;
                                State.configurationwindow.ModuleInfo.Visibility = System.Windows.Visibility.Visible;
                                State.configurationwindow.EasyCommsInfo.Visibility = System.Windows.Visibility.Visible;
                            }

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

