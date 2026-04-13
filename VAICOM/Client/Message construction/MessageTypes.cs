using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using VAICOM.Extensions.Kneeboard;
using VAICOM.Static;

namespace VAICOM
{
    namespace Client
    {

        public partial class DcsClient
        {

            public static partial class Message
            {

                // ---------------------------------
                // CLASS DEFINTIONS

                // regular aicomms command message
                public class CommsMessage
                {

                    public bool debug;
                    public string client;
                    public string mode;
                    public string type;
                    public int tgtdevid;
                    public string tgtdevname;
                    public string dspmsg;
                    public int msgdur;
                    public List<double> shift;

                    public string dcsid; //"wMsgLeaderEngageMyTarget"
                    public int command;
                    public string reccat;
                    public bool insert;
                    public string selectrecipient = "";
                    public int selectunit;
                    public object parameters;
                    public List<int> cmdsequence;
                    public List<int> actionsequence;
                    public List<DeviceAction> devsequence;
                    public List<Extensions.RIO.DeviceAction> extsequence;
                    public bool forcetune;
                    public bool showmenu;
                    public bool redirect_world_speech;
                    public bool fc3;
                    public bool AIRIO;
                    public bool carriersuppressauto;
                    public int? kneeboard;
                    public bool? dictmode;

                    public bool? tunenum;
                    public int tunechn;
                    public int? tunemod;
                    public List<string> tunefrq;

                    public bool disableplayervoice;
                    public bool hideonscreentext;
                    public bool forcelanguage;
                    public string forcedlanguage;
                    public bool menuinvisible;
                    public bool forcenatoprotocol;
                    public bool forcecallsigns;
                    public string forcedcallsigns;
                    public bool havedial;
                    public bool operatedial;
                    public bool importmenus;

                    public CommsMessage()
                    {
                        parameters = new List<object>();
                        cmdsequence = new List<int>();
                        actionsequence = new List<int>();
                        devsequence = new List<DeviceAction>();
                        extsequence = new List<Extensions.RIO.DeviceAction>();
                        showmenu = false;
                        tunefrq = new List<string>();
                        dictmode = State.Proxy.Dictation.IsOn();
                    }
                }

                // request to retrieve DCS server mission data
                public class UpdateRequest
                {
                    public bool debug;
                    public string client;
                    public string mode;
                    public string type;
                    public int command;
                    public string tgtdevname;
                    public string dspmsg;
                    public int msgdur;
                    public List<double> shift;

                    public bool disableplayervoice;
                    public bool hideonscreentext;
                    public bool forcelanguage;
                    public string forcedlanguage;
                    public bool menuinvisible;
                    public bool forcenatoprotocol;
                    public bool forcecallsigns;
                    public string forcedcallsigns;
                    public bool importmenus;
                    public bool operatedial;
                    public bool redirect_world_speech;
                    public bool fc3;
                    public bool AIRIO;
                    public bool carriersuppressauto;
                    public int? kneeboard;
                    public bool? dictmode;

                    public UpdateRequest()
                    {
                        debug = State.activeconfig.Debugmode;
                        client = State.currentlicense;
                        mode = State.clientmode;
                        type = Messagetypes.RequestUpdate;
                        command = 4000;
                        tgtdevname = State.currentradiodevicename.Replace(":", " ");
                        importmenus = State.activeconfig.ImportOtherMenu;
                        dictmode = State.Proxy.Dictation.IsOn();
                        //kneeboard = 1; // show kneeboard on PTT press, test
                    }

                }

                // send change settings message
                public class SettingsChange
                {
                    public bool debug;
                    public string client;
                    public string mode;
                    public string type;
                    public int command;
                    public string dspmsg;
                    public int msgdur;
                    public List<double> shift;

                    //public int      tgtdevid;
                    public bool disableplayervoice;
                    public bool hideonscreentext;
                    public bool forcelanguage;
                    public string forcedlanguage;
                    public bool menuinvisible;
                    public bool forcenatoprotocol;
                    public bool forcecallsigns;
                    public string forcedcallsigns;
                    public bool importmenus;
                    public bool operatedial;
                    public bool redirect_world_speech;
                    public bool fc3;
                    public bool AIRIO;
                    public bool carriersuppressauto;
                    public int? kneeboard;
                    public bool? dictmode;

                    public SettingsChange()
                    {
                        debug = State.activeconfig.Debugmode;
                        client = State.currentlicense;
                        mode = State.clientmode;
                        type = Messagetypes.SettingsChange;
                        command = 4000;
                        importmenus = State.activeconfig.ImportOtherMenu;
                        dictmode = State.Proxy.Dictation.IsOn();
                    }
                }

                // comd sequence as in iCommands 0000-4000 (e.g. for Options menu) -- can we expand this for icommands?
                public class iCommandSequence
                {
                    public bool debug;
                    public bool showmenu;
                    public string client;
                    public string mode;
                    public string type;
                    public string tgtdevname;
                    public int command;
                    public List<int> cmdsequence;
                    public bool importmenus;
                    public string dspmsg;
                    public int msgdur;
                    public List<double> shift;
                    public bool carriersuppressauto;
                    public int? kneeboard;
                    public bool? dictmode;

                    public iCommandSequence()
                    {
                        debug = State.activeconfig.Debugmode;
                        showmenu = true;
                        client = State.currentlicense;
                        mode = State.clientmode;
                        type = Messagetypes.iCommandSequence;
                        tgtdevname = State.currentradiodevicename.Replace(":", " ");
                        command = 4000;
                        cmdsequence = new List<int>();
                        importmenus = State.activeconfig.ImportOtherMenu;
                        shift = new List<double>();
                        dictmode = State.Proxy.Dictation.IsOn();
                    }
                }

                // action sequence as for imported F10 menu actions (Index)
                public class ActionIndexSequence
                {
                    public bool debug;
                    public string client;
                    public string mode;
                    public string type;
                    public int command;
                    public List<int> actionsequence;
                    public bool importmenus;
                    public string dspmsg;
                    public int msgdur;
                    public List<double> shift;
                    public bool carriersuppressauto;
                    public int? kneeboard;
                    public bool? dictmode;

                    public ActionIndexSequence()
                    {
                        debug = State.activeconfig.Debugmode;
                        client = State.currentlicense;
                        mode = State.clientmode;
                        type = Messagetypes.ActionIndexSequence;
                        command = 4000;
                        importmenus = State.activeconfig.ImportOtherMenu;
                        dictmode = State.Proxy.Dictation.IsOn();
                    }
                }

                // send specific debug message
                public class DebugMsg
                {
                    public bool debug;
                    public string client;
                    public string mode;
                    public string exec;
                    public string dspmsg;
                    public int msgdur;
                    public List<double> shift;
                    public bool carriersuppressauto;
                    public int? kneeboard;
                    public bool? dictmode;

                    public DebugMsg()
                    {
                        debug = true;
                        client = State.currentlicense;
                        mode = ClientModes.Debug;
                        exec = "";
                        dictmode = State.Proxy.Dictation.IsOn();
                    }
                }

                // for RIO macros and cockpit control - not defined atm.
                public class DeviceActionSequence
                {

                }

                // for manual radio tuning.
                public class RadioTuneMessage
                {
                    public bool debug;
                    public string client;
                    public string mode;
                    public string type;
                    public int tgtdevid;
                    public int command;
                    public string exec;
                    public string dspmsg;
                    public int msgdur;
                    public bool carriersuppressauto;
                    public int? kneeboard;
                    public bool? dictmode;

                    public bool? tunenum;
                    public string tunechn;
                    public int? tunemod;
                    public List<string> tunefrq;

                    public RadioTuneMessage()
                    {
                        debug = State.activeconfig.Debugmode;
                        client = State.currentlicense;
                        mode = State.clientmode;
                        type = Messagetypes.SettingsChange;
                        command = 4000;
                        tunenum = true;
                        tunefrq = new List<string>();
                        dictmode = State.Proxy.Dictation.IsOn();
                    }

                }


            }


            // ---------------------------------
            // METHODS (for sending message types):

            // for sending kneeboard message
            public static void SendKneeboardMessage(KneeboardMessage msg)
            {
                try
                {

                    if (!State.kneeboardactivated && State.activeconfig.Kneeboard_Enabled)
                    {
                        msg = new KneeboardMessage();
                        msg.logdata = new LogData("", "Please enable kneeboard extension to use this page.");
                    }

                    string formatmessage = JsonConvert.SerializeObject(msg);
                    byte[] sendbuffer = Encoding.UTF8.GetBytes(formatmessage);
                    State.Kneeboard_SendSocket.SendTo(sendbuffer, State.Kneeboard_SendIpEndPoint);
                }
                catch (Exception e)
                {
                    Log.Write("there was a problem sending kneeboard message:" + e.Message + e.ToString(), Colors.Inline);

                }
            }


            // for sending normal aicomms message
            public static string SendClientMessage()
            {
                try
                {
                    string formatmessage = JsonConvert.SerializeObject(State.currentmessage);
                    byte[] sendbuffer = Encoding.UTF8.GetBytes(formatmessage);
                    State.SendSocket.SendTo(sendbuffer, State.SendIpEndPoint);

                    // Log WSO-specific commands
                    if (State.currentmessage.type == "WSOCommand")
                    {
                        Log.Write($"WSO Command sent: {formatmessage}", Colors.Inline);
                    }

                    return formatmessage;
                }
                catch (Exception e)
                {
                    Log.Write("There was a problem sending the client message: " + e.Message + e.ToString(), Colors.Inline);
                    return "";
                }
            }

            // for sending (PTT) message to SRS Client
            public static string SendSRSMessage(Extensions.SRS.SRSclient.SRS_PTT_Message newmessage)
            {
                try
                {
                    string formatmessage = JsonConvert.SerializeObject(newmessage) + "\n";
                    byte[] sendbuffer = Encoding.UTF8.GetBytes(formatmessage);
                    State.SRS_SendSocket.SendTo(sendbuffer, State.SRS_SendIpEndPoint);
                    return formatmessage;
                }
                catch (Exception e)
                {
                    Log.Write("there was a problem sending the SRS message:" + e.Message + e.ToString(), Colors.Inline);
                    return "";
                }
            }

            // request update (used by open ptt etc.)
            public static string SendUpdateRequest()
            {
                try
                {
                    Message.UpdateRequest updaterequest = new Message.UpdateRequest();

                    updaterequest.disableplayervoice = State.activeconfig.DisablePlayerVoice;
                    updaterequest.hideonscreentext = State.activeconfig.HideOnScreenText;
                    updaterequest.forcelanguage = State.activeconfig.ForceLanguage;
                    updaterequest.forcedlanguage = Languages.localization.languages[State.activeconfig.ForcedLanguage];
                    updaterequest.menuinvisible = State.activeconfig.HideMenus;
                    updaterequest.forcenatoprotocol = State.activeconfig.EnforceATCProtocol && (State.activeconfig.EnforcedATCProtocol == 0);
                    updaterequest.forcecallsigns = State.activeconfig.EnforceCallsigns;
                    updaterequest.forcedcallsigns = Languages.localization.languages[State.activeconfig.CallsignsLanguage];
                    updaterequest.operatedial = State.activeconfig.OperateDial;
                    updaterequest.redirect_world_speech = State.activeconfig.Redirect_World_Speech;
                    updaterequest.fc3 = State.currentmodule.IsFC;
                    updaterequest.AIRIO = State.currentmodule.Equals(Products.DCSmodules.LookupTable[State.riomod]);
                    updaterequest.carriersuppressauto = State.activeconfig.CarrierSuppressAuto;

                    string formatmessage = JsonConvert.SerializeObject(updaterequest);
                    byte[] sendbuffer = Encoding.UTF8.GetBytes(formatmessage);
                    State.SendSocket.SendTo(sendbuffer, State.SendIpEndPoint);
                    //Log.Write("Message sent: " + formatmessage, Colors.Inline);
                    State.lastupdaterequesttimer = 0;

                    //Log.Write("SENT NEW UPDATE REQUEST", Colors.Critical);

                    return formatmessage;
                }
                catch (Exception e)
                {
                    Log.Write("there was a problem sending state update request:" + e.Message + e.ToString(), Colors.Text);
                    return "";
                }
            }

            public class heartbeatmessage
            {
                public string messagetype;
                public string status;

                public heartbeatmessage()
                {
                    this.messagetype = "heartbeat";
                    this.status = "OK";

                    string profilename = "VAICOM PRO Community Edition for DCS World";
                    if (!State.Proxy.GetProfileName().ToLower().Contains(profilename.ToLower()))
                    {
                        status = "profile error";
                    }
                }
            }

            // heartbeattotray
            public static void SendHeartBeatToTray()
            {
                string formatmessage = JsonConvert.SerializeObject(new heartbeatmessage());
                byte[] sendbuffer = Encoding.UTF8.GetBytes(formatmessage);
                State.SendSocket.SendTo(sendbuffer, State.SendingIpTrayMessages);
            }

            // debug
            public static void SendDebugCode(Message.DebugMsg senddebugmessage)
            {
                string formatmessage = JsonConvert.SerializeObject(senddebugmessage);
                byte[] sendbuffer = Encoding.UTF8.GetBytes(formatmessage);
                State.SendSocket.SendTo(sendbuffer, State.SendIpEndPoint);
            }

            // send changed settings (e.g. used by config window)
            public static string SendSettingsChange()
            {
                try
                {
                    Message.SettingsChange updaterequest = new Message.SettingsChange();

                    Log.Write("Updating settings", Colors.Text);

                    updaterequest.disableplayervoice = State.activeconfig.DisablePlayerVoice;
                    updaterequest.hideonscreentext = State.activeconfig.HideOnScreenText;
                    updaterequest.forcelanguage = State.activeconfig.ForceLanguage;
                    updaterequest.forcedlanguage = Languages.localization.languages[State.activeconfig.ForcedLanguage];
                    updaterequest.menuinvisible = State.activeconfig.HideMenus;
                    updaterequest.forcenatoprotocol = State.activeconfig.EnforceATCProtocol && (State.activeconfig.EnforcedATCProtocol == 0);
                    updaterequest.forcecallsigns = State.activeconfig.EnforceCallsigns;
                    updaterequest.forcedcallsigns = Languages.localization.languages[State.activeconfig.CallsignsLanguage];
                    updaterequest.operatedial = State.activeconfig.OperateDial;
                    updaterequest.redirect_world_speech = State.activeconfig.Redirect_World_Speech;
                    updaterequest.fc3 = State.currentmodule.IsFC;
                    updaterequest.AIRIO = State.currentmodule.Equals(Products.DCSmodules.LookupTable[State.riomod]);
                    updaterequest.carriersuppressauto = State.activeconfig.CarrierSuppressAuto;

                    string formatmessage = JsonConvert.SerializeObject(updaterequest);
                    byte[] sendbuffer = Encoding.UTF8.GetBytes(formatmessage); // was ASCII
                    State.SendSocket.SendTo(sendbuffer, State.SendIpEndPoint);
                    return formatmessage;
                }
                catch (Exception e)
                {
                    Log.Write("there was a problem sending changed settings:" + e.Message + e.ToString(), Colors.Text);
                    return "";
                }
            }

            public static void KneeboardToggle()
            {
                List<int> sequence = new List<int>();
                sequence.Add(1587); //iCommandPlaneShowKneeboard
                SendCmdSequence(sequence, false); // don't show menu
            }

            public static void SendSettingsChangeOnRecipientSelected(bool press)
            {
                //return;

                try
                {
                    Message.SettingsChange update = new Message.SettingsChange();

                    if (press)
                    {
                        update.disableplayervoice = State.activeconfig.DisablePlayerVoice; // set player speech to setting
                        if (State.activeconfig.KneeboardlinkPTT)
                        {
                            update.kneeboard = 1; // open kneeboard on press
                        }
                    }
                    else
                    {
                        update.disableplayervoice = false; // set player speech back to normal (for multiplayer voices)
                        if (State.activeconfig.KneeboardlinkPTT)
                        {
                            update.kneeboard = -1; // close kneeboard on release
                        }
                    }

                    string formatmessage = JsonConvert.SerializeObject(update);
                    byte[] sendbuffer = Encoding.UTF8.GetBytes(formatmessage);
                    State.SendSocket.SendTo(sendbuffer, State.SendIpEndPoint);
                }
                catch (Exception e)
                {
                    Log.Write("there was a problem sending changed settings:" + e.Message + e.ToString(), Colors.Text);
                }
            }

            // stand-alone cmd sequence (e.g. used on close ptt to close options menu):
            public static string SendCmdSequence(List<int> sequence, bool showmenu)
            {
                try
                {
                    Message.iCommandSequence sendsequence = new Message.iCommandSequence();
                    sendsequence.cmdsequence = sequence;
                    sendsequence.showmenu = showmenu;
                    string formatmessage = JsonConvert.SerializeObject(sendsequence);
                    byte[] sendbuffer = Encoding.UTF8.GetBytes(formatmessage); // ASCII
                    State.SendSocket.SendTo(sendbuffer, State.SendIpEndPoint);
                    return formatmessage;
                }
                catch (Exception e)
                {
                    Log.Write("there was a problem sending command sequence:" + e.Message + e.ToString(), Colors.Text);
                    return "";
                }
            }

            // SendCmdSequence overload version with message (used by MP/VoiceChat integration)
            public static string SendCmdSequence(List<int> sequence, bool showmenu, string message)
            {
                try
                {
                    Message.iCommandSequence sendsequence = new Message.iCommandSequence();
                    sendsequence.cmdsequence = sequence;
                    sendsequence.showmenu = showmenu;
                    sendsequence.dspmsg = message;
                    sendsequence.msgdur = 5;
                    string formatmessage = JsonConvert.SerializeObject(sendsequence);
                    byte[] sendbuffer = Encoding.UTF8.GetBytes(formatmessage);
                    State.SendSocket.SendTo(sendbuffer, State.SendIpEndPoint);
                    return formatmessage;
                }
                catch (Exception e)
                {
                    Log.Write("there was a problem sending command sequence:" + e.Message + e.ToString(), Colors.Text);
                    return "";
                }
            }

            public static string SendRadioControlMessage(Message.RadioTuneMessage message)
            {
                string formatmessage = JsonConvert.SerializeObject(message);
                byte[] sendbuffer = Encoding.UTF8.GetBytes(formatmessage); // was ASCII
                State.SendSocket.SendTo(sendbuffer, State.SendIpEndPoint);
                return formatmessage;
            }

            // ---------------------------------

        }
    }

}
