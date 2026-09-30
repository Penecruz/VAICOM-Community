using System;
using System.Collections.Generic;
using VAICOM.Extensions.RIO;
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
                private static int lastEgiDirectedWaypoint = -1;

                private static bool TryBuildRioEgiDirectWaypointSequence(int waypointNumber, out string validationError)
                {
                    validationError = string.Empty;

                    // Step 1: guard against unsupported module/runtime.
                    if (!IsF14BUActive())
                    {
                        validationError = "AIRIO : EGI Direct Waypoint is available only in F-14B(U).";
                        return false;
                    }

                    // Step 2: validate supported spoken/index range.
                    if (waypointNumber < 1 || waypointNumber > 50)
                    {
                        validationError = "AIRIO : Waypoint out of range. Valid range is 1 to 50.";
                        return false;
                    }

                    // Step 3: reset outbound action list.
                    if (State.currentmessage.extsequence == null)
                    {
                        State.currentmessage.extsequence = new List<Extensions.RIO.DeviceAction>();
                    }
                    else
                    {
                        State.currentmessage.extsequence.Clear();
                    }

                    double waypointValue = waypointNumber / 100.0;

                    // Step 4: normalize nav list by reloading BU flight plan.
                    State.currentmessage.extsequence.AddRange(Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_MENU_MAIN);
                    State.currentmessage.extsequence.AddRange(Extensions.RIO.DeviceActionsLibrary.Sequences.Macro.Seq_J_UTIL_NAV_RELOAD_FLT_PLAN_BU);
                    State.currentmessage.extsequence.Add(Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_MENU_CLOSE);

                    // Step 5: wait, then select requested waypoint with retry pulses.
                    // Command 10037 only sets the BU "selected" waypoint for subsequent BU actions.
                    State.currentmessage.extsequence.Add(new Extensions.RIO.DeviceAction()
                    {
                        device = Extensions.RIO.DeviceActionsLibrary.Devices.PROXY,
                        command = 10037,
                        value = waypointValue,
                        delayMs = 2000
                    });
                    State.currentmessage.extsequence.Add(new Extensions.RIO.DeviceAction()
                    {
                        device = Extensions.RIO.DeviceActionsLibrary.Devices.PROXY,
                        command = 10037,
                        value = waypointValue,
                        delayMs = 120
                    });
                    State.currentmessage.extsequence.Add(new Extensions.RIO.DeviceAction()
                    {
                        device = Extensions.RIO.DeviceActionsLibrary.Devices.PROXY,
                        command = 10037,
                        value = waypointValue,
                        delayMs = 260
                    });

                    // Step 6: apply BU direct-to-selected to make the requested waypoint active.
                    State.currentmessage.extsequence.Add(Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_PROXY_BU_DIRECT_TO_SELECTED_DELAYED(320));

                    // Step 7: re-assert active entry after BU direct so the requested waypoint
                    // remains active if the module briefly inserts a conditional waypoint.
                    State.currentmessage.extsequence.Add(new Extensions.RIO.DeviceAction()
                    {
                        device = Extensions.RIO.DeviceActionsLibrary.Devices.PROXY,
                        command = 10018,
                        value = waypointValue,
                        delayMs = 1200
                    });
                    State.currentmessage.extsequence.Add(new Extensions.RIO.DeviceAction()
                    {
                        device = Extensions.RIO.DeviceActionsLibrary.Devices.PROXY,
                        command = 10018,
                        value = waypointValue,
                        delayMs = 1700
                    });

                    if (State.activeconfig != null && State.activeconfig.Debugmode)
                    {
                        List<string> sequenceDebug = new List<string>();
                        for (int i = 0; i < State.currentmessage.extsequence.Count; i++)
                        {
                            Extensions.RIO.DeviceAction a = State.currentmessage.extsequence[i];
                            sequenceDebug.Add("#" + i.ToString() + " d=" + a.device.ToString() + " c=" + a.command.ToString() + " v=" + a.value.ToString("0.##") + " t=" + a.delayMs.ToString());
                        }
                        Log.Write("AIRIO EGI direct seq | wp=" + waypointNumber.ToString() + " | " + string.Join(" | ", sequenceDebug), Colors.Inline);
                    }

                    return true;
                }

                private static bool TryGetEgiWaypointFromSegments(out int waypointNumber)
                {
                    waypointNumber = 0;

                    // Step 1: parse the spoken segments in order and keep deterministic precedence.
                    // Prefer the first explicit value after waypoint/steerpoint over any trailing artifacts.
                    List<string> segmentDebug = new List<string>();
                    List<string> segments = new List<string>();

                    for (int i = 1; i <= 5; i++)
                    {
                        string segment = State.Proxy.Utility.ParseTokens("{CMDSEGMENT:" + i.ToString() + "}");
                        string safeSegment = segment ?? string.Empty;
                        segmentDebug.Add("S" + i.ToString() + "='" + safeSegment + "'");
                        segments.Add(safeSegment.Trim());
                    }

                    for (int i = 0; i < segments.Count - 1; i++)
                    {
                        string token = segments[i];
                        if (token.Equals("waypoint", StringComparison.OrdinalIgnoreCase)
                            || token.Equals("steerpoint", StringComparison.OrdinalIgnoreCase)
                            || token.Equals("wpt", StringComparison.OrdinalIgnoreCase)
                            || token.Equals("spt", StringComparison.OrdinalIgnoreCase))
                        {
                            int contextualValue;
                            if (TryParsePositiveNumberToken(segments[i + 1], out contextualValue))
                            {
                                waypointNumber = contextualValue;
                                return true;
                            }
                        }
                    }

                    for (int i = 0; i < segments.Count; i++)
                    {
                        int parsed;
                        if (TryParsePositiveNumberToken(segments[i], out parsed))
                        {
                            waypointNumber = parsed;
                            return true;
                        }
                    }

                    string commandDigits = VAICOM.Extensions.CommandNumbers.Digits();
                    int digitsParsed = 0;
                    bool foundFromDigits = false;
                    if (commandDigits.Length > 0)
                    {
                        int parsedDigits;
                        if (Int32.TryParse(commandDigits, out parsedDigits) && parsedDigits > 0)
                        {
                            digitsParsed = parsedDigits;
                            foundFromDigits = true;
                        }
                    }
                    if (foundFromDigits)
                    {
                        waypointNumber = digitsParsed;
                    }

                    if (State.activeconfig != null && State.activeconfig.Debugmode)
                    {
                        Log.Write("AIRIO EGI direct parse | " + string.Join(", ", segmentDebug) + " | txtnum='" + commandDigits + "' | resolved=" + waypointNumber.ToString(), Colors.Inline);
                    }

                    return foundFromDigits;
                }

                private static bool TryParsePositiveNumberToken(string token, out int parsed)
                {
                    parsed = 0;
                    if (string.IsNullOrWhiteSpace(token))
                    {
                        return false;
                    }

                    string safeToken = token.Trim();
                    if (Int32.TryParse(safeToken, out parsed) && parsed > 0)
                    {
                        return true;
                    }

                    string digitsOnly = string.Empty;
                    for (int c = 0; c < safeToken.Length; c++)
                    {
                        char ch = safeToken[c];
                        if (ch >= '0' && ch <= '9')
                        {
                            digitsOnly += ch;
                        }
                    }

                    return digitsOnly.Length > 0 && Int32.TryParse(digitsOnly, out parsed) && parsed > 0;
                }

                public static void SetRioDeviceSequence_EGI_DirectWaypoint()
                {
                    try
                    {
                        if (!State.dll_installed_rio || !State.activeconfig.RIO_Enabled || !State.IsAirioTomcatModule())
                        {
                            Log.Write("AIRIO commands are not available at this time.", Colors.Warning);
                            UI.Playsound.Recipientna();
                            return;
                        }

                        State.currentmessage = new CommsMessage();
                        State.currentmessage.debug = State.activeconfig.Debugmode;
                        State.currentmessage.client = State.client;
                        State.currentmessage.mode = State.clientmode;
                        State.currentmessage.type = Messagetypes.DeviceControl;
                        State.currentmessage.tgtdevid = Message.GetSendDeviceId();
                        State.currentmessage.tgtdevname = State.currentradiodevicename;
                        State.currentmessage.command = 4000;
                        State.currentmessage.dcsid = "airio.egi.direct";
                        State.currentmessage.reccat = "RIO";
                        State.currentmessage.insert = false;
                        State.currentmessage.selectrecipient = null;
                        State.currentmessage.selectunit = -1;
                        State.currentmessage.havedial = false;
                        State.currentmessage.forcetune = false;
                        State.currentmessage.redirect_world_speech = State.activeconfig.Redirect_World_Speech;
                        State.currentmessage.disableplayervoice = State.activeconfig.DisablePlayerVoice;
                        State.currentmessage.hideonscreentext = State.activeconfig.HideOnScreenText;
                        State.currentmessage.forcelanguage = State.activeconfig.ForceLanguage;
                        State.currentmessage.forcedlanguage = Languages.localization.languages[State.activeconfig.ForcedLanguage];
                        State.currentmessage.menuinvisible = State.activeconfig.HideMenus;
                        State.currentmessage.forcenatoprotocol = State.activeconfig.EnforceATCProtocol;
                        State.currentmessage.forcecallsigns = State.activeconfig.EnforceCallsigns;
                        State.currentmessage.forcedcallsigns = Languages.localization.languages[State.activeconfig.CallsignsLanguage];
                        State.currentmessage.operatedial = State.activeconfig.OperateDial;
                        State.currentmessage.AIRIO = State.IsAirioTomcatModule();
                        State.currentmessage.extsequence = new List<Extensions.RIO.DeviceAction>();

                        int waypointNumber;
                        if (!TryGetEgiWaypointFromSegments(out waypointNumber))
                        {
                            State.currentmessage.dspmsg = "AIRIO : Say Direct [Waypoint; Steerpoint] [1..50].";
                            State.currentmessage.msgdur = 5;
                            riospeech.riospeakrandom(2);
                            SendNewMessage();
                            return;
                        }

                        string validationError;
                        if (!TryBuildRioEgiDirectWaypointSequence(waypointNumber, out validationError))
                        {
                            State.currentmessage.dspmsg = validationError;
                            State.currentmessage.msgdur = 5;
                            State.currentmessage.extsequence = new List<Extensions.RIO.DeviceAction>();
                            riospeech.riospeakrandom(2);
                            SendNewMessage();
                            return;
                        }

                        if (State.activeconfig.RIO_Messages && !State.activeconfig.RIO_Hints_Only)
                        {
                            State.currentmessage.dspmsg = "AIRIO : EGI Direct Waypoint " + waypointNumber.ToString();
                            State.currentmessage.msgdur = 5;
                        }

                        UI.Playsound.Commandcomplete();
                        lastEgiDirectedWaypoint = waypointNumber;

                        if (!State.clientmode.Equals(ClientModes.Debug) && tables.menustate[tables.menucats.PLAYERSEAT].Equals(tables.menustates.RIO))
                        {
                            State.currentmessage.dspmsg = "AIRIO : You are in Jester's seat!\n";
                            State.currentmessage.msgdur = 5;
                            State.currentmessage.extsequence = new List<Extensions.RIO.DeviceAction>();
                        }

                        SendNewMessage();

                        if (PTT.IsPTTModeSingle())
                        {
                            Log.Write(State.currentTXnode.name + " | " + PTT.RadioDevices.SEL.name + ": [ RIO ],[  ],[  ] EGI Direct Waypoint " + waypointNumber.ToString() + " [  ] [  ]", Colors.Message);
                        }
                        else
                        {
                            Log.Write(State.currentTXnode.name + " | " + State.currentTXnode.radios[0].name + ": [ RIO ],[  ],[  ] EGI Direct Waypoint " + waypointNumber.ToString() + " [  ] [  ]", Colors.Message);
                        }
                    }
                    catch (Exception e)
                    {
                        Log.Write("Error setting EGI direct waypoint sequence: " + e.StackTrace + e.InnerException, Colors.Inline);
                    }
                }
            }
        }
    }
}
