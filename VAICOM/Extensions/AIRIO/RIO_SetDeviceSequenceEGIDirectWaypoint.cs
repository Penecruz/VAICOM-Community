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

                    if (!IsF14BUActive())
                    {
                        validationError = "AIRIO : EGI Direct Waypoint is available only in F-14B(U).";
                        return false;
                    }

                    if (waypointNumber < 1 || waypointNumber > 50)
                    {
                        validationError = "AIRIO : Waypoint out of range. Valid range is 1 to 50.";
                        return false;
                    }

                    if (State.currentmessage.extsequence == null)
                    {
                        State.currentmessage.extsequence = new List<Extensions.RIO.DeviceAction>();
                    }
                    else
                    {
                        State.currentmessage.extsequence.Clear();
                    }

                    double waypointValue = waypointNumber / 100.0;

                    State.currentmessage.extsequence.Add(Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_PROXY_BU_SELECT_WAYPOINT(waypointNumber));
                    State.currentmessage.extsequence.Add(new Extensions.RIO.DeviceAction()
                    {
                        device = Extensions.RIO.DeviceActionsLibrary.Devices.PROXY,
                        command = 10037,
                        value = waypointValue,
                        delayMs = 60
                    });
                    State.currentmessage.extsequence.Add(Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_PROXY_BU_DIRECT_TO_SELECTED_DELAYED(160));
                    State.currentmessage.extsequence.Add(Extensions.RIO.DeviceActionsLibrary.RIO.Atom_J_PROXY_BU_DIRECT_TO_SELECTED_DELAYED(320));

                    return true;
                }

                private static bool TryGetEgiWaypointFromSegments(out int waypointNumber)
                {
                    waypointNumber = 0;

                    bool found = false;
                    List<string> segmentDebug = new List<string>();

                    for (int i = 1; i <= 5; i++)
                    {
                        string segment = State.Proxy.Utility.ParseTokens("{CMDSEGMENT:" + i.ToString() + "}");
                        string safeSegment = segment ?? string.Empty;
                        segmentDebug.Add("S" + i.ToString() + "='" + safeSegment + "'");

                        int parsed;
                        if (Int32.TryParse(safeSegment, out parsed) && parsed > 0)
                        {
                            waypointNumber = parsed;
                            found = true;
                            continue;
                        }

                        string digitsOnly = string.Empty;
                        for (int c = 0; c < safeSegment.Length; c++)
                        {
                            char ch = safeSegment[c];
                            if (ch >= '0' && ch <= '9')
                            {
                                digitsOnly += ch;
                            }
                        }

                        if (digitsOnly.Length > 0 && Int32.TryParse(digitsOnly, out parsed) && parsed > 0)
                        {
                            waypointNumber = parsed;
                            found = true;
                        }
                    }

                    if (State.activeconfig != null && State.activeconfig.Debugmode)
                    {
                        Log.Write("AIRIO EGI direct parse | " + string.Join(", ", segmentDebug) + " | resolved=" + waypointNumber.ToString(), Colors.Inline);
                    }

                    return found;
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

                        bool repeatedWaypointRequest = (lastEgiDirectedWaypoint == waypointNumber);

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
                        if (repeatedWaypointRequest)
                        {
                            riospeech.riospeakrandom(3);
                        }
                        else
                        {
                            riospeech.riospeakrandom(1);
                        }
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
