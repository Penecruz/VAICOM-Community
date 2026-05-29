using System;
using System.Collections.Generic;
using System.Windows.Forms;
using VAICOM.Database;
using VAICOM.Extensions.Kneeboard;
using VAICOM.PushToTalk;
using VAICOM.Static;
using VAICOM.WSO;

namespace VAICOM
{
    namespace Interfaces
    {
        public class API
        {
            public static void PTT_Mode_Page_Up(dynamic vaProxy)
            {
                if (State.configwindowopen && (State.configurationwindow != null))
                {
                    State.configurationwindow.Dispatcher.BeginInvoke((MethodInvoker)delegate
                    {
                        State.configurationwindow.Page_Up();
                        vaProxy.WriteToLog("PTT: TX node SNGL set to " + State.activeconfig.SingleHotkey, Colors.Warning);
                    });
                }
                else
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
                    }
                    catch
                    {
                    }
                    vaProxy.WriteToLog("PTT: TX node SNGL set to " + State.activeconfig.SingleHotkey, Colors.Warning);
                }
            }

            public static void PTT_Mode_Page_Dn(dynamic vaProxy)
            {
                if (State.configwindowopen && (State.configurationwindow != null))
                {
                    State.configurationwindow.Dispatcher.BeginInvoke((MethodInvoker)delegate
                    {
                        State.configurationwindow.Page_Dn();
                        vaProxy.WriteToLog("PTT: TX node SNGL set to " + State.activeconfig.SingleHotkey, Colors.Warning);
                    });
                }
                else
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
                    }
                    catch
                    {
                    }
                    vaProxy.WriteToLog("PTT: TX node SNGL set to " + State.activeconfig.SingleHotkey, Colors.Warning);
                }
            }

            public static void Chatter_Vol_Up(dynamic vaProxy)
            {
                State.activeconfig.ChatterVolume = State.activeconfig.ChatterVolume + 0.05f;
                if (State.activeconfig.ChatterVolume > 1)
                {
                    State.activeconfig.ChatterVolume = 1;
                }
                else
                {
                    if (State.configwindowopen && (State.configurationwindow != null))
                    {
                        State.configurationwindow.Dispatcher.BeginInvoke((MethodInvoker)delegate
                        {
                            State.configurationwindow.Set_Volume_init();

                        }
                        );
                    }
                }
                vaProxy.WriteToLog("Chatter volume set to " + Math.Round((double)State.activeconfig.ChatterVolume * 100) + "%", Colors.Warning);
            }

            public static void Chatter_Vol_Dn(dynamic vaProxy)
            {
                State.activeconfig.ChatterVolume = State.activeconfig.ChatterVolume - 0.05f;
                if (State.activeconfig.ChatterVolume < 0)
                {
                    State.activeconfig.ChatterVolume = 0;
                }
                else
                {
                    if (State.configwindowopen && (State.configurationwindow != null))
                    {
                        State.configurationwindow.Dispatcher.BeginInvoke((MethodInvoker)delegate
                        {
                            State.configurationwindow.Set_Volume_init();
                        }
                        );
                    }
                }
                vaProxy.WriteToLog("Chatter volume set to " + Math.Round((double)State.activeconfig.ChatterVolume * 100) + "%", Colors.Warning);
            }

            public static void Operate_Dial(dynamic vaProxy)
            {
                State.activeconfig.OperateDial = !State.activeconfig.OperateDial;
                if (State.configwindowopen && (State.configurationwindow != null))
                {
                    State.configurationwindow.Dispatcher.BeginInvoke((MethodInvoker)delegate
                    {
                        if (!State.activeconfig.OperateDial)
                        {
                            State.configurationwindow.Dial_Sw_to_Down();
                        }
                        else
                        {
                            State.configurationwindow.Dial_Sw_to_Up();
                        }
                    }
                    );
                }
                vaProxy.WriteToLog("PTT: Operate Dial set to " + State.activeconfig.OperateDial, Colors.Warning);
            }

            public static void SRS_Mapping(dynamic vaProxy)
            {
                if (State.configwindowopen && (State.configurationwindow != null))
                {
                    State.configurationwindow.Dispatcher.BeginInvoke((MethodInvoker)delegate
                    {
                        State.configurationwindow.Toggle_SRS_Mode();
                    }
                    );
                }
                else
                {
                    State.activeconfig.UseSRSmapping = !State.activeconfig.UseSRSmapping;
                }
                vaProxy.WriteToLog("PTT: SRS Mapping set to " + State.activeconfig.UseSRSmapping, Colors.Warning);
            }

            public static void PTT_Release_Hot(dynamic vaProxy)
            {
                if (State.configwindowopen && (State.configurationwindow != null))
                {
                    State.configurationwindow.Dispatcher.BeginInvoke((MethodInvoker)delegate
                    {
                        State.configurationwindow.PTT_Release_Hot_Toggle();
                    }
                    );
                }
                else
                {
                    State.activeconfig.ReleaseHot = !State.activeconfig.ReleaseHot;
                }
                vaProxy.WriteToLog("PTT: Release Hot set to " + State.activeconfig.ReleaseHot, Colors.Warning);
            }

            public static void PTT_Mode_Prv(dynamic vaProxy)
            {
                State.activeconfig.SelectorMode = State.activeconfig.SelectorMode + 1;
                if (State.activeconfig.SelectorMode > 4)
                {
                    State.activeconfig.SelectorMode = 4;
                }
                else
                {
                    if (State.configwindowopen && (State.configurationwindow != null))
                    {
                        State.configurationwindow.Dispatcher.BeginInvoke((MethodInvoker)delegate
                        {
                            State.configurationwindow.Selector_SetMode();
                            UI.Playsound.Dial_Click();
                        }
                        );
                    }

                }
                string[] modes = new string[] { "INV", "NORM", "MULTI", "SNGL" };
                vaProxy.WriteToLog("PTT: Mode set to " + modes[(int)State.activeconfig.SelectorMode - 1], Colors.Warning);
            }

            public static void PTT_Mode_Nxt(dynamic vaProxy)
            {
                State.activeconfig.SelectorMode = State.activeconfig.SelectorMode - 1;
                if (State.activeconfig.SelectorMode < 1)
                {
                    State.activeconfig.SelectorMode = 1;
                }
                else
                {
                    if (State.configwindowopen && (State.configurationwindow != null))
                    {
                        State.configurationwindow.Dispatcher.BeginInvoke((MethodInvoker)delegate
                        {
                            State.configurationwindow.Selector_SetMode();
                            UI.Playsound.Dial_Click();
                        }
                        );
                    }
                }
                string[] modes = new string[] { "INV", "NORM", "MULTI", "SNGL" };
                vaProxy.WriteToLog("PTT: Mode set to " + modes[(int)State.activeconfig.SelectorMode - 1], Colors.Warning);
            }

            public static void PTT_Mode_Inv(dynamic vaProxy)
            {
                State.activeconfig.SelectorMode = 1;
                if (State.configwindowopen && (State.configurationwindow != null))
                {
                    State.configurationwindow.Dispatcher.BeginInvoke((MethodInvoker)delegate
                    {
                        State.configurationwindow.Selector_SetMode();
                        UI.Playsound.Dial_Click();

                    }
                    );
                }
                vaProxy.WriteToLog("PTT: Mode set to INV", Colors.Warning);
            }

            public static void PTT_Mode_Norm(dynamic vaProxy)
            {
                State.activeconfig.SelectorMode = 2;
                if (State.configwindowopen && (State.configurationwindow != null))
                {
                    State.configurationwindow.Dispatcher.BeginInvoke((MethodInvoker)delegate
                    {
                        State.configurationwindow.Selector_SetMode();
                        UI.Playsound.Dial_Click();
                    }
                    );
                }
                vaProxy.WriteToLog("PTT: Mode set to NORM", Colors.Warning);
            }

            public static void PTT_Mode_Multi(dynamic vaProxy)
            {
                State.activeconfig.SelectorMode = 3;
                if (State.configwindowopen && (State.configurationwindow != null))
                {
                    State.configurationwindow.Dispatcher.BeginInvoke((MethodInvoker)delegate
                    {
                        State.configurationwindow.Selector_SetMode();
                        UI.Playsound.Dial_Click();
                    }
                    );
                }
                vaProxy.WriteToLog("PTT: Mode set to MULTI", Colors.Warning);
            }


            public static void PTT_Mode_Single(dynamic vaProxy)
            {
                State.activeconfig.SelectorMode = 4;
                if (State.configwindowopen && (State.configurationwindow != null))
                {
                    State.configurationwindow.Dispatcher.BeginInvoke((MethodInvoker)delegate
                    {
                        State.configurationwindow.Selector_SetMode();
                        UI.Playsound.Dial_Click();
                    }
                    );
                }
                vaProxy.WriteToLog("PTT: Mode set to SNGL", Colors.Warning);
            }

            public static List<string> tabcats = new List<string>()
            { "ALL","LOG", "AWACS","JTAC", "ATC", "TANKER", "FLIGHT", "AOCS" ,"REF","NOTES" };

            private static readonly List<string> okbTabOrder = new List<string>()
            { "LOG", "ATC", "AWACS", "JTAC", "TANKER", "AOCS", "FLIGHT", "AI CREW", "GND CREW", "NOTES" };

            private static string okbActiveTab = "LOG";

            public static string NormalizeKneeboardContext(string input)
            {
                if (string.IsNullOrWhiteSpace(input))
                {
                    return input;
                }

                string normalized = input.Trim().ToLowerInvariant();
                const string okbTabPrefix = "vaicom-community;okb-out;tab-";
                if (!normalized.StartsWith(okbTabPrefix, StringComparison.Ordinal))
                {
                    return normalized;
                }

                return normalized;
            }

            public static bool IsOpenKneeboardTabActionContext(string input)
            {
                return !string.IsNullOrWhiteSpace(input)
                    && input.StartsWith("vaicom-community;okb-out;tab-", StringComparison.OrdinalIgnoreCase);
            }

            public static void ControlOpenKneeboardOut(dynamic vaProxy, string actionContext)
            {
                if (!IsOpenKneeboardTabActionContext(actionContext))
                {
                    return;
                }

                string action = actionContext.Trim().ToLowerInvariant().Substring("vaicom-community;okb-out;tab-".Length);
                switch (action)
                {
                    case "next":
                        CycleOkbTabs(vaProxy, 1);
                        break;
                    case "prev":
                        CycleOkbTabs(vaProxy, -1);
                        break;
                    case "log":
                        SelectOkbTab(vaProxy, "LOG");
                        break;
                    case "atc":
                        SelectOkbTab(vaProxy, "ATC");
                        break;
                    case "awacs":
                        SelectOkbTab(vaProxy, "AWACS");
                        break;
                    case "jtac":
                        SelectOkbTab(vaProxy, "JTAC");
                        break;
                    case "tanker":
                        SelectOkbTab(vaProxy, "TANKER");
                        break;
                    case "aocs":
                        SelectOkbTab(vaProxy, "AOCS");
                        break;
                    case "flight":
                        SelectOkbTab(vaProxy, "FLIGHT");
                        break;
                    case "notes":
                        SelectOkbTab(vaProxy, "NOTES");
                        break;
                    case "gnd-crew":
                    case "ref":
                        SelectOkbTab(vaProxy, "GND CREW");
                        break;
                    case "ai-crew":
                        SelectOkbTab(vaProxy, "AI CREW");
                        break;
                    default:
                        break;
                }
            }

            private static void CycleOkbTabs(dynamic vaProxy, int direction)
            {
                string current = okbActiveTab;
                if (string.IsNullOrWhiteSpace(current) || okbTabOrder.IndexOf(current) < 0)
                {
                    current = "LOG";
                }

                int index = okbTabOrder.IndexOf(current);
                if (index < 0)
                {
                    index = 0;
                }

                int next = (index + (direction > 0 ? 1 : -1) + okbTabOrder.Count) % okbTabOrder.Count;
                SelectOkbTab(vaProxy, okbTabOrder[next]);
            }

            private static void SelectOkbTab(dynamic vaProxy, string tab)
            {
                string normalized = string.IsNullOrWhiteSpace(tab) ? "LOG" : tab.ToUpperInvariant();
                okbActiveTab = normalized;
                OpenKneeboardBridge.UpdateActiveCategory(normalized);
            }

            public static void CycleTabs(dynamic vaProxy, int ud)
            {
                int catnum = tabcats.IndexOf(State.KneeboardState.activecat.ToUpper()) + ud;
                if (catnum < 1)
                {
                    catnum = tabcats.Count - 1;
                }
                if (catnum > tabcats.Count - 1)
                {
                    catnum = 1;
                }
                string cat = tabcats[catnum];
                ControlKneeboard(vaProxy, "kneeboard.tab." + cat.ToLower());

            }

            public static void ControlKneeboard(dynamic vaProxy, string apicommand)
            {
                try
                {

                    switch (apicommand.ToLower())
                    {

                        case "kneeboard.tab.all":
                            State.KneeboardState.activecat = "ALL";
                            KneeboardUpdater.SwitchPage("ALL");
                            //KneeboardUpdater.SendDeviceCommand(255, 3010, 1);
                            break;

                        case "kneeboard.tab.log":
                            State.KneeboardState.activecat = "LOG";
                            KneeboardUpdater.SwitchPage("LOG");
                            //KneeboardUpdater.SendDeviceCommand(255, 3011, 1);
                            break;

                        case "kneeboard.tab.awacs":
                            State.KneeboardState.activecat = "AWACS";
                            KneeboardUpdater.SwitchPage("AWACS");
                            //KneeboardUpdater.SendDeviceCommand(255, 3012, 1);
                            break;

                        case "kneeboard.tab.jtac":
                            State.KneeboardState.activecat = "JTAC";
                            KneeboardUpdater.SwitchPage("JTAC");
                            //KneeboardUpdater.SendDeviceCommand(255, 3013, 1);
                            break;

                        case "kneeboard.tab.atc":
                            State.KneeboardState.activecat = "ATC";
                            KneeboardUpdater.SwitchPage("ATC");
                            //KneeboardUpdater.SendDeviceCommand(255, 3014, 1);
                            break;

                        case "kneeboard.tab.tanker":
                            State.KneeboardState.activecat = "TANKER";
                            KneeboardUpdater.SwitchPage("Tanker");
                            //KneeboardUpdater.SendDeviceCommand(255, 3015, 1);
                            break;

                        case "kneeboard.tab.flight":
                            State.KneeboardState.activecat = "FLIGHT";
                            KneeboardUpdater.SwitchPage("Flight");
                            //KneeboardUpdater.SendDeviceCommand(255, 3016, 1);
                            break;

                        case "kneeboard.tab.aocs":
                            State.KneeboardState.activecat = "AOCS";
                            KneeboardUpdater.SwitchPage("AOCS");
                            //KneeboardUpdater.SendDeviceCommand(255, 3017, 1);
                            break;

                        case "kneeboard.tab.ref":
                            State.KneeboardState.activecat = "REF";
                            KneeboardUpdater.SwitchPage("REF");
                            //KneeboardUpdater.SendDeviceCommand(255, 3018, 1);
                            break;

                        case "kneeboard.tab.notes":
                            State.KneeboardState.activecat = "NOTES";
                            KneeboardUpdater.SwitchPage("NOTES");
                            //KneeboardUpdater.SendDeviceCommand(255, 3019, 1);
                            break;

                        case "kneeboard.tab.prv":
                            CycleTabs(vaProxy, -1);
                            break;

                        case "kneeboard.tab.nxt":
                            CycleTabs(vaProxy, 1);
                            break;

                        case "kneeboard.opac.up":
                            State.activeconfig.KneeboardOpacity += 0.05;
                            if (State.activeconfig.KneeboardOpacity > 1)
                            {
                                State.activeconfig.KneeboardOpacity = 1;
                            }
                            KneeboardUpdater.SendDeviceCommand(255, 3020, State.activeconfig.KneeboardOpacity);
                            Settings.ConfigFile.WriteConfigToFile(true);
                            if (State.configwindowopen && (State.configurationwindow != null))
                            {
                                State.configurationwindow.Dispatcher.BeginInvoke((MethodInvoker)delegate
                                {
                                    State.configurationwindow.KneeboardOpacity.Value = State.activeconfig.KneeboardOpacity;
                                }
                               );
                            }
                            break;

                        case "kneeboard.opac.dn":
                            State.activeconfig.KneeboardOpacity -= 0.05;
                            if (State.activeconfig.KneeboardOpacity < 0)
                            {
                                State.activeconfig.KneeboardOpacity = 0;
                            }
                            KneeboardUpdater.SendDeviceCommand(255, 3020, State.activeconfig.KneeboardOpacity);
                            Settings.ConfigFile.WriteConfigToFile(true);
                            if (State.configwindowopen && (State.configurationwindow != null))
                            {
                                State.configurationwindow.Dispatcher.BeginInvoke((MethodInvoker)delegate
                                {
                                    State.configurationwindow.KneeboardOpacity.Value = State.activeconfig.KneeboardOpacity;
                                }
                               );
                            }
                            break;

                        default:
                            break;
                    }

                }
                catch (Exception e)
                {
                    vaProxy.WriteToLog("Dictate: failed." + e.Message, Colors.Inline);
                }

                //;
            }


            public static void API_Test(dynamic vaProxy)
            {
                try
                {
                    string output = "no return.";
                    int bufsize = 0;
                    vaProxy.WriteToLog("API: output " + output + ":" + bufsize, Colors.Warning);
                }
                catch (Exception e)
                {
                    vaProxy.WriteToLog("API: test failed " + e.Message, Colors.Warning);
                }

                vaProxy.WriteToLog("API: test executed.", Colors.Warning);
                UI.Playsound.Commandcomplete();
            }
        }
    }
}
