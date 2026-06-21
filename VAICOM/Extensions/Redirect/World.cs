using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using VAICOM.Database;
using VAICOM.PushToTalk;
using VAICOM.Static;

namespace VAICOM
{
    namespace Extensions
    {
        namespace WorldAudio
        {

            public static partial class Processor
            {
                private static Delegate ttsPlaybackStoppedHandler;

                public class AudioDeviceObject
                {
                    public int number;
                    public string id;
                    public string name;
                }

                public static int GetPanValue(PushToTalk.PTT.TXNode node)
                {
                    int value = 0;
                    if (State.activeconfig.Redirect_World_Speech)
                    {
                        try
                        {
                            if (node.name.Equals("TX1")) { return State.activeconfig.PanSetting_TX1; }
                            if (node.name.Equals("TX2")) { return State.activeconfig.PanSetting_TX2; }
                            if (node.name.Equals("TX3")) { return State.activeconfig.PanSetting_TX3; }
                            if (node.name.Equals("TX4")) { return State.activeconfig.PanSetting_TX4; }
                            if (node.name.Equals("TX5")) { return State.activeconfig.PanSetting_TX5; }
                            if (node.name.Equals("TX6")) { return State.activeconfig.PanSetting_TX6; }
                        }
                        catch
                        {
                        }
                    }
                    return value;
                }

                private static void TryAttachPlaybackStoppedHandler()
                {
                    try
                    {
                        if (State.ttsoutput == null)
                        {
                            return;
                        }

                        EventInfo evt = State.ttsoutput.GetType().GetEvent("PlaybackStopped");
                        if (evt == null || evt.EventHandlerType == null)
                        {
                            return;
                        }

                        MethodInfo method = typeof(Processor).GetMethod("audioOutput_PlaybackStopped", BindingFlags.Public | BindingFlags.Static);
                        if (method == null)
                        {
                            return;
                        }

                        Delegate handler = Delegate.CreateDelegate(evt.EventHandlerType, method, false);
                        if (handler == null)
                        {
                            return;
                        }

                        evt.AddEventHandler(State.ttsoutput, handler);
                        ttsPlaybackStoppedHandler = handler;
                    }
                    catch
                    {
                    }
                }

                private static void TryDetachPlaybackStoppedHandler()
                {
                    try
                    {
                        if (State.ttsoutput == null || ttsPlaybackStoppedHandler == null)
                        {
                            return;
                        }

                        EventInfo evt = State.ttsoutput.GetType().GetEvent("PlaybackStopped");
                        if (evt == null)
                        {
                            ttsPlaybackStoppedHandler = null;
                            return;
                        }

                        evt.RemoveEventHandler(State.ttsoutput, ttsPlaybackStoppedHandler);
                    }
                    catch
                    {
                    }
                    finally
                    {
                        ttsPlaybackStoppedHandler = null;
                    }
                }

                public static void InsertResourcesForPlayback(List<string> resourcelist)
                {
                    List<ISampleProvider> streams = new List<ISampleProvider>();
                    for (int i = 0; i < resourcelist.Count; i++)
                    {
                        try
                        {
                            Stream fragment = (Stream)State.wingmanresources.GetObject(resourcelist[i]);
                            WaveFileReader reader = new WaveFileReader(fragment);
                            var upsampler = new WaveFormatConversionStream(new WaveFormat(22050, 16, 1), reader);
                            var vol = new VolumeSampleProvider(upsampler.ToSampleProvider());
                            vol.Volume = SetTTSVolume();
                            var panned = new PanningSampleProvider(vol);
                            panned.Pan = GetPanValue(State.receivedtx);
                            streams.Add(panned);
                        }
                        catch
                        {
                        }
                    }
                    var merged = new ConcatenatingSampleProvider(streams);
                    State.ttsmixer.AddMixerInput(merged);
                }

                public static List<string> SelectResourcesForSpeech(List<string> input)
                {
                    // input is "2-bright.wav", returns random rsource string for each
                    List<string> output = new List<string>();
                    //Log.Write("input count = " + input.Count, Colors.Warning);

                    string groupcallsign = Helpers.Common.RemoveDigits(State.currentstate.playercallsign);
                    //Log.Write("Group callsign = " + groupcallsign, Colors.Warning);

                    foreach (string entry in input) // entry: "2-bright"
                    {
                        if (Extensions.WorldAudio.Processor.wavfileroots.ContainsKey(entry.ToLower())) // it is listed
                        {

                            string root = Extensions.WorldAudio.Processor.wavfileroots[entry.ToLower()]; // root: Value like like _4_bright

                            //Log.Write("Have listed key root: " + root, Colors.Warning);
                            List<string> collectionkeys = new List<string>();

                            foreach (string keyword in State.wingmansoundfiles) // keyword: like _4_bright_1_
                            {
                                if (!entry.StartsWith("2-") && keyword.ToLower().StartsWith(root.ToLower())) // don't use regular '2' without callsign (preference)
                                {
                                    collectionkeys.Add(keyword);
                                    //Log.Write("Found match: " + keyword, Colors.Warning);
                                }

                                if (entry.StartsWith("2-") && keyword.ToLower().StartsWith("callsign_flight_" + groupcallsign.ToLower())) // 2-colt.wav
                                {
                                    collectionkeys.Add(keyword);
                                    //Log.Write("Found match: " + keyword, Colors.Warning);
                                }
                            }
                            // collectionkeys is list of valid resource names, pick random one
                            int sel = State.random2.Next(0, collectionkeys.Count);
                            output.Add(collectionkeys[sel]);
                            //Log.Write("Adding resource: " + collectionkeys[sel], Colors.Warning);
                        }

                    }

                    return output;
                }

                public static void SpeakLocalizedMessage(Servers.Server.ServerCommsMessage message)
                {
                    string matchtxt = message.text;
                    List<string> cleanedmessage = CleanedUpMessage(message.speech.files);
                    List<string> speechcontent = SelectResourcesForSpeech(cleanedmessage);
                    InsertResourcesForPlayback(speechcontent);
                }

                public static void audioOutput_PlaybackStopped(object sender, StoppedEventArgs args)
                {
                    if (State.configwindowopen && (State.configurationwindow != null))
                    {
                        State.configurationwindow.Dispatcher.BeginInvoke((MethodInvoker)delegate
                        {
                            State.configurationwindow.RCV_Bug.Visibility = System.Windows.Visibility.Hidden;
                        });
                    }
                }

                public static void SpeakMessageFromFiles(Servers.Server.ServerCommsMessage message, int delay)
                {
                    string matchtxt = message.text;
                    string basepath = message.speech.directory.Replace("REDIRECT__", "");
                    InsertFilesForPlayback(basepath, message.speech.files, delay);
                }

                public static void InsertFilesForPlayback(string basepath, List<string> filelist, int delay)
                {
                    bool usingopenbeta = State.currentstate.root.Contains("openbeta");
                    string pathroot = "";

                    if (State.dcsinstalled)
                    {
                        if (usingopenbeta)
                        {
                            pathroot = State.dcspath_openbeta + "\\";
                        }
                        else
                        {
                            pathroot = State.dcspath_release + "\\";
                        }
                    }
                    else
                    {
                        // DCS not installed!
                    }

                    List<ISampleProvider> streams = new List<ISampleProvider>();

                    // first insert delay silence pieces (250ms)
                    for (int i = 0; i < delay; i++)
                    {
                        try
                        {
                            Stream silence = (Stream)Properties.Resources.Silence_22kHz_Mono_250ms;
                            WaveFileReader reader = new WaveFileReader(silence);
                            var upsampler = new WaveFormatConversionStream(new WaveFormat(22050, 16, 2), reader);
                            streams.Add(upsampler.ToSampleProvider());
                        }
                        catch
                        {
                        }
                    }
                    // .. then actual content

                    for (int i = 0; i < filelist.Count; i++)
                    {
                        try
                        {
                            string filebase = pathroot + basepath.Replace("/", "\\") + filelist[i].Replace("/", "\\");

                            if (File.Exists(filebase + ".ogg"))
                            {
                                // for ogg: create wav file if its not there yet..
                                if (!File.Exists(filebase + ".wav"))
                                {

                                    var oggfile = new NAudio.Vorbis.VorbisWaveReader(filebase + ".ogg");
                                    long length = oggfile.Length;
                                    float[] buffer = new float[length];
                                    int result = oggfile.Read(buffer, 0, (int)length);

                                    WaveFileWriter writer = new WaveFileWriter(filebase + ".wav", new WaveFormat((int)oggfile.WaveFormat.SampleRate, 16, (int)oggfile.WaveFormat.Channels)); // 22050, 16, 1
                                    writer.WriteSamples(buffer, 0, result); //buffer.Length
                                    writer.Close();
                                }

                                var reader = new WaveFileReader(filebase + ".wav");
                                var upsampler = new WaveFormatConversionStream(new WaveFormat(22050, 16, 1), reader);
                                var vol = new VolumeSampleProvider(upsampler.ToSampleProvider());
                                var panned = new PanningSampleProvider(vol);
                                panned.Pan = 0; // VMU / Jester etc. always central pan
                                streams.Add(panned);

                            }
                            else // standard (.wav)
                            {

                                WaveFileReader reader = new WaveFileReader(filebase + ".wav");
                                var upsampler = new WaveFormatConversionStream(new WaveFormat(22050, 16, 1), reader);
                                var vol = new VolumeSampleProvider(upsampler.ToSampleProvider());
                                vol.Volume = SetTTSVolume();
                                var panned = new PanningSampleProvider(vol);
                                panned.Pan = GetPanValue(State.receivedtx);
                                streams.Add(panned);

                            }
                        }
                        catch
                        {
                        }
                    }


                    var merged = new ConcatenatingSampleProvider(streams);

                    if (!(State.currentstate.viewexternal && !State.currentstate.soundsallowexternal))
                    {

                        if (State.activeconfig.Redirect_World_Speech)
                        {
                            try
                            {
                                State.ttsmixer.AddMixerInput(merged);
                            }
                            catch (Exception e)
                            {
                                Log.Write("playback error: " + e.Message, Colors.Inline);
                            }
                        }
                        else
                        {
                            State.ttsoutput.Init(merged);
                            TryAttachPlaybackStoppedHandler();
                            State.ttsoutput.Play();
                        }
                    }
                }

                public static float SetTTSVolume()
                {
                    float volume;

                    try
                    {
                        int mastervolume = State.currentstate.options.sound.volume;
                        int headphonesvolume = State.currentstate.options.sound.headphones;
                        bool hear_in_helmet = State.currentstate.options.sound.hear_in_helmet;

                        float basevolume = ((float)mastervolume * (float)headphonesvolume) / 10000f;

                        if (hear_in_helmet)
                        {
                            basevolume = 0.25f * basevolume;
                        }

                        if (State.activeconfig.Redirect_World_Speech)
                        {
                            volume = (State.activeconfig.TTSVolume / 0.5f) * basevolume;
                        }
                        else
                        {
                            volume = basevolume;
                        }

                    }
                    catch
                    {
                        volume = 0.25f;
                    }

                    return volume;
                }

                public static void LoadAudioDevicesAlt()
                {
                    try
                    {

                        var enumerator = new MMDeviceEnumerator();

                        State.audiodeviceobjects = new List<Processor.AudioDeviceObject>();
                        State.audiodeviceobjects.Add(new AudioDeviceObject() { number = -1, id = "0", name = "(default)" });

                        int i = 0;

                        foreach (var endpoint in enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active)) //(DataFlow.Render, DeviceState.Active)
                        {
                            int devicenumber = i;
                            string deviceid = endpoint.ID;
                            string devicename = endpoint.FriendlyName;

                            State.audiodeviceobjects.Add(new AudioDeviceObject() { number = devicenumber, id = deviceid, name = devicename });

                            i += 1;
                        }

                    }
                    catch (Exception e)
                    {
                        Log.Write(e.Message, Colors.Warning);
                    }
                }

                public static void LogAudioDevices()
                {

                    foreach (AudioDeviceObject endpoint in State.audiodeviceobjects)
                    {
                        Log.Write("Audio Dev number " + endpoint.number + ": " + endpoint.name, Colors.Critical);
                    }
                }

                public static void LoadAudioDevices()
                {
                    try
                    {

                        var enumerator = new MMDeviceEnumerator();

                        State.audiodeviceobjects = new List<Processor.AudioDeviceObject>();
                        State.audiodeviceobjects.Add(new AudioDeviceObject() { number = -1, id = "0", name = "(default)" });

                        int i = 0;

                        foreach (var endpoint in enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active)) //(DataFlow.Render, DeviceState.Active)
                        {
                            int devicenumber = i;
                            string deviceid = endpoint.ID;
                            string devicename = endpoint.FriendlyName;

                            State.audiodeviceobjects.Add(new AudioDeviceObject() { number = devicenumber, id = deviceid, name = devicename });

                            i += 1;
                        }

                    }
                    catch (Exception e)
                    {
                        Log.Write(e.Message, Colors.Warning);
                    }
                }

                public static void LoadAudioDevicesSafe()
                {

                    LoadAudioDevicesAlt();

                    if (State.audiodeviceobjects.Count == 0)
                    {
                        State.activeconfig.AudioDeviceNumber = -1;
                        Settings.ConfigFile.WriteConfigToFile(true);
                    }
                    if (State.activeconfig.AudioDeviceNumber < -1)
                    {
                        State.activeconfig.AudioDeviceNumber = -1;
                        Settings.ConfigFile.WriteConfigToFile(true);
                    }
                    if (State.activeconfig.AudioDeviceNumber > State.audiodeviceobjects.Count - 1)
                    {
                        State.activeconfig.AudioDeviceNumber = State.audiodeviceobjects.Count - 1;
                        Settings.ConfigFile.WriteConfigToFile(true);
                    }

                }

                public static void InitTTSPlaybackStream()
                {
                    try
                    {
                        int devicecount = WaveOut.DeviceCount;

                        LoadAudioDevicesSafe();

                        if (State.ttsoutput != null)
                        {
                            try
                            {
                                TryDetachPlaybackStoppedHandler();
                                State.ttsoutput.Dispose();
                            }
                            catch
                            {
                            }
                        }

                        State.ttsoutput = new NAudio.Wave.WaveOut();
                        State.ttsoutput.DeviceNumber = State.activeconfig.AudioDeviceNumber;
                        State.ttsoutput.NumberOfBuffers = 2;
                        State.ttsoutput.DesiredLatency = 100;

                        State.ttsmixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(22050, 2));
                        State.ttsmixer.ReadFully = true;

                        State.ttsoutput.Init(State.ttsmixer);
                        State.ttsoutput.Play();
                        TryAttachPlaybackStoppedHandler();
                        State.currentaudiodevicevalid = true;

                        //Log.Write("Initialized new stream for device " + State.activeconfig.AudioDeviceNumber, Colors.Warning);

                    }
                    catch (Exception e)
                    {
                        string deviceName = "Default audio device";
                        int configuredDevice = State.activeconfig.AudioDeviceNumber;
                        if (configuredDevice >= 0 && configuredDevice < State.audiodeviceobjects.Count)
                        {
                            deviceName = State.audiodeviceobjects[configuredDevice].name;
                        }

                        Log.Write("Device " + deviceName + " could not be initialized: " + e.Message, Colors.Text);
                        State.currentaudiodevicevalid = false;

                        try
                        {
                            if (State.ttsoutput != null)
                            {
                                State.ttsoutput.Dispose();
                            }
                        }
                        catch
                        {
                        }

                        State.ttsoutput = null;
                        State.ttsmixer = null;
                    }
                }

                public static void CloseTTSPlaybackStream()
                {
                    try
                    {
                        TryDetachPlaybackStoppedHandler();
                        State.ttsoutput.Dispose();
                    }
                    catch
                    {
                    }
                }

                public static commcat SenderCatByEvent(int eventid) //also do recipientcat
                {
                    commcat commcat = commcat.UNKNOWN;

                    // sender PLAYER:
                    // block 1: sender = player
                    if (eventid > Commands.Table["wMsgLeaderNull"].geteventnumber() && eventid < Commands.Table["wMsgLeaderMaximum"].geteventnumber()) { return commcat.PLAYER; }
                    // block 2: sender = Player (NAVY_Player)
                    if (eventid > Commands.Table["wMsgLeaderToNavyATCNull"].geteventnumber() && eventid < Commands.Table["wMsgLeaderToNavyATCMaximum"].geteventnumber()) { return commcat.PLAYER; }

                    // SENDER OTHER UNITS:
                    // sender = wingman
                    if (eventid > Commands.Table["wMsgWingmenNull"].geteventnumber() && eventid < Commands.Table["wMsgWingmenMaximum"].geteventnumber()) { return commcat.WINGMAN; }
                    // sender = AlliedFlight
                    if (eventid > Commands.Table["wMsgFlightNull"].geteventnumber() && eventid < Commands.Table["wMsgFlightMaximum"].geteventnumber()) { return commcat.ALLIED_FLIGHT; }
                    // sender = ATC
                    if (eventid > Commands.Table["wMsgATCNull"].geteventnumber() && eventid < Commands.Table["wMsgATCMaximum"].geteventnumber()) { return commcat.ATC; }

                    // sender = Carrier ATC -Departure
                    if (eventid > Commands.Table["wMsgATCNAVYDepartureNull"].geteventnumber() && eventid < Commands.Table["wMsgATCNAVYDepartureMaximum"].geteventnumber()) { return commcat.ATC_NAVY_Departure; }
                    // sender = Carrier ATC -Marshal
                    if (eventid > Commands.Table["wMsgATCNAVYMarshalNull"].geteventnumber() && eventid < Commands.Table["wMsgATCNAVYMarshalMaximum"].geteventnumber()) { return commcat.ATC_NAVY_Marshal; }
                    // sender = Carrier ATC -Approach Tower
                    if (eventid > Commands.Table["wMsgATCNAVYApproachTowerNull"].geteventnumber() && eventid < Commands.Table["wMsgATCNAVYApproachTowerMaximum"].geteventnumber()) { return commcat.ATC_NAVY_Approach_Tower; }
                    // sender = Carrier ATC -LSO
                    if (eventid > Commands.Table["wMsgATCNAVYLSONull"].geteventnumber() && eventid < Commands.Table["wMsgATCNAVYLSOMaximum"].geteventnumber()) { return commcat.ATC_NAVY_LSO; }

                    // sender = AWACS
                    if (eventid > Commands.Table["wMsgAWACSNull"].geteventnumber() && eventid < Commands.Table["wMsgAWACSMaximum"].geteventnumber()) { return commcat.AWACS; }
                    // sender = Tanker
                    if (eventid > Commands.Table["wMsgTankerNull"].geteventnumber() && eventid < Commands.Table["wMsgTankerMaximum"].geteventnumber()) { return commcat.TANKER; }
                    // sender = FAC
                    if (eventid > Commands.Table["wMsgFACNull"].geteventnumber() && eventid < Commands.Table["wMsgFACMaximum"].geteventnumber()) { return commcat.JTAC; }
                    // sender = CCC
                    if (eventid > Commands.Table["wMsgCCCNull"].geteventnumber() && eventid < Commands.Table["wMsgCCCMaximum"].geteventnumber()) { return commcat.CCC; }
                    // sender = crew
                    if (eventid > Commands.Table["wMsgGroundCrewNull"].geteventnumber() && eventid < Commands.Table["wMsgGroundCrewMaximum"].geteventnumber()) { return commcat.GROUND_CREW; }
                    // sender = Betty
                    if (eventid > Commands.Table["wMsgBettyNull"].geteventnumber() && eventid < Commands.Table["wMsgBettyMaximum"].geteventnumber()) { return commcat.Betty; }
                    // sender = ALMAZ
                    if (eventid > Commands.Table["wMsgALMAZ_Null"].geteventnumber() && eventid < Commands.Table["wMsgALMAZ_Maximum"].geteventnumber()) { return commcat.ALMAZ; }
                    // sender = RI65
                    if (eventid > Commands.Table["wMsgRI65_Null"].geteventnumber() && eventid < Commands.Table["wMsgRI65_Maximum"].geteventnumber()) { return commcat.RI65; }
                    // sender = cargo
                    if (eventid > Commands.Table["wMsgExternalCargo_Null"].geteventnumber() && eventid < Commands.Table["wMsgExternalCargo_Maximum"].geteventnumber()) { return commcat.ExternalCargo; }
                    /// (Mi8 checklist)
                    // sender = A10_VMU
                    if (eventid > Commands.Table["wMsgA10_VMU_Null"].geteventnumber() && eventid < Commands.Table["wMsgA10_VMU_Maximum"].geteventnumber()) { return commcat.A10_VMU; }

                    return commcat;
                }

                public static commcat ReceiverCatByEvent(int eventid) //also do recipientcat
                {
                    commcat commcat = commcat.UNKNOWN;

                    // for calls sent sent by player:

                    // receiver = wingman
                    if (eventid > Commands.Table["wMsgLeaderToWingmenNull"].geteventnumber() && eventid < Commands.Table["wMsgLeaderToWingmenMaximum"].geteventnumber()) { return commcat.WINGMAN; }
                    // receiver = atc
                    if (eventid > Commands.Table["wMsgLeaderToATCNull"].geteventnumber() && eventid < Commands.Table["wMsgLeaderToATCMaximum"].geteventnumber()) { return commcat.ATC; }
                    // receiver = awacs
                    if (eventid > Commands.Table["wMsgLeaderToAWACSNull"].geteventnumber() && eventid < Commands.Table["wMsgLeaderToAWACSMaximum"].geteventnumber()) { return commcat.AWACS; }
                    // receiver = tanker
                    if (eventid > Commands.Table["wMsgLeaderToTankerNull"].geteventnumber() && eventid < Commands.Table["wMsgLeaderToTankerMaximum"].geteventnumber()) { return commcat.TANKER; }
                    // receiver = jtac
                    if (eventid > Commands.Table["wMsgLeaderToFACNull"].geteventnumber() && eventid < Commands.Table["wMsgLeaderToFACMaximum"].geteventnumber()) { return commcat.JTAC; }
                    // receiver = crew
                    if (eventid > Commands.Table["wMsgLeaderToGroundCrewNull"].geteventnumber() && eventid < Commands.Table["wMsgLeaderToGroundCrewMaximum"].geteventnumber()) { return commcat.GROUND_CREW; }
                    // receiver = NAVY (carrier) (can break down)
                    if (eventid > Commands.Table["wMsgLeaderToNavyATCNull"].geteventnumber() && eventid < Commands.Table["wMsgLeaderToNavyATCMaximum"].geteventnumber()) { return commcat.ATC_NAVY_CARRIER; }
                    // sent by others:
                    if (eventid > Commands.Table["wMsgWingmenNull"].geteventnumber() && eventid < Commands.Table["wMsgMaximum"].geteventnumber()) { return commcat.PLAYER; }

                    return commcat;
                }

                public static PushToTalk.PTT.TXNode GetReceivingTXFromUnitID(Processor.commcat sendercat, int senderid)
                {
                    PushToTalk.PTT.TXNode result = State.currentTXnode; // base assumption (may not be valid)  

                    if (sendercat.Equals(commcat.ALLIED_FLIGHT))
                    {
                        //Log.Write("allied flight: ", Colors.Warning);
                        return PTT.TXNodes.TX4;
                    }

                    if (sendercat.Equals(commcat.GROUND_CREW) || sendercat.Equals(commcat.ExternalCargo))
                    {
                        return PTT.TXNodes.TX5;
                    }

                    if (State.currentmodule.IsFC)
                    {
                        if (sendercat.Equals(commcat.ATC)) { return PTT.TXNodes.TX1; }
                        if (sendercat.Equals(commcat.AWACS)) { return PTT.TXNodes.TX1; }
                        if (sendercat.Equals(commcat.TANKER)) { return PTT.TXNodes.TX1; }
                        if (sendercat.Equals(commcat.WINGMAN)) { return PTT.TXNodes.TX2; }
                        if (sendercat.Equals(commcat.JTAC)) { return PTT.TXNodes.TX3; }

                        if (sendercat.Equals(commcat.ATC_NAVY_CARRIER)) { return PTT.TXNodes.TX1; }
                        if (sendercat.Equals(commcat.ATC_NAVY_Approach_Tower)) { return PTT.TXNodes.TX1; }
                        if (sendercat.Equals(commcat.ATC_NAVY_Departure)) { return PTT.TXNodes.TX1; }
                        if (sendercat.Equals(commcat.ATC_NAVY_LSO)) { return PTT.TXNodes.TX1; }
                        if (sendercat.Equals(commcat.ATC_NAVY_Marshal)) { return PTT.TXNodes.TX1; }
                    }
                    //else scan between TX1-TX3
                    foreach (Servers.Server.RadioDevice device in State.currentstate.radios)
                    {
                        foreach (Servers.Server.DcsUnit unit in device.tunedunits)
                        {
                            if (unit.id_.Equals(senderid))
                            {
                                if (PTT.TXNodes.TX1.radios[0].deviceid.Equals(device.deviceid)) { return PTT.TXNodes.TX1; }
                                if (PTT.TXNodes.TX2.radios[0].deviceid.Equals(device.deviceid)) { return PTT.TXNodes.TX2; }
                                if (PTT.TXNodes.TX3.radios[0].deviceid.Equals(device.deviceid)) { return PTT.TXNodes.TX3; }
                            }
                        }
                    }

                    return result;
                }

                public static string MMDeviceName(int index)
                {
                    foreach (AudioDeviceObject device in State.audiodeviceobjects)
                    {
                        if (device.number.Equals(index))
                        {
                            return (device.name);
                        }
                    }
                    return "";
                }

                public static void WorldAudioRedirect()
                {
                    try
                    {
                        if (!State.activeconfig.Redirect_World_Speech)
                        {
                            State.currentaudiodevicevalid = false;
                            return;
                        }

                        if (State.ttsmixer == null)
                        {
                            State.currentaudiodevicevalid = false;
                            Log.Write("World audio redirect unavailable: playback mixer is not initialized.", Colors.Warning);
                            return;
                        }

                        if (State.ttsoutput != null)
                        {
                            try
                            {
                                State.ttsoutput.Stop();
                            }
                            catch
                            {
                            }

                            try
                            {
                                TryDetachPlaybackStoppedHandler();
                                State.ttsoutput.Dispose();
                            }
                            catch
                            {
                            }
                        }

                        State.ttsoutput = new NAudio.Wave.WaveOut();

                        //Log.Write("Starting new output at device number " + State.activeconfig.AudioDeviceNumber, Static.Colors.Critical);
                        string devname = MMDeviceName(State.activeconfig.AudioDeviceNumber);
                        //Log.Write("Device name: " + devname, Static.Colors.Critical);

                        int targetdevicenum = -1;
                        // find correct waveout device
                        for (int n = -1; n < WaveOut.DeviceCount; n++)
                        {
                            var caps = WaveOut.GetCapabilities(n);
                            //Log.Write($"{n}: {caps.ProductName}", Static.Colors.Critical);
                            if (devname.Contains(caps.ProductName))
                            {
                                targetdevicenum = n;
                                break;
                            }
                        }

                        State.ttsoutput.DeviceNumber = targetdevicenum;
                        State.ttsoutput.Init(State.ttsmixer);
                        TryAttachPlaybackStoppedHandler();
                        State.ttsoutput.Play();
                        State.currentaudiodevicevalid = true;
                    }
                    catch (Exception e)
                    {
                        string deviceName = "Default audio device";
                        try
                        {
                            int configuredDevice = State.activeconfig.AudioDeviceNumber;
                            if (configuredDevice >= 0 && configuredDevice < State.audiodeviceobjects.Count)
                            {
                                deviceName = State.audiodeviceobjects[configuredDevice].name;
                            }
                        }
                        catch
                        {
                        }

                        Log.Write("Device " + deviceName + " could not be initialized: " + e.Message, Colors.Text);
                        State.currentaudiodevicevalid = false;
                    }

                }

                public enum commcat
                {
                    ALL,
                    UNKNOWN,
                    PLAYER,
                    NAVY_PLAYER,
                    WINGMAN,
                    ATC,
                    AWACS,
                    TANKER,
                    JTAC,
                    CCC,
                    ALLIED_FLIGHT,
                    Betty,
                    ALMAZ,
                    RI65,
                    ExternalCargo,
                    A10_VMU,
                    GROUND_CREW,
                    RIO,
                    ATC_NAVY_CARRIER,
                    ATC_NAVY_Approach_Tower,
                    ATC_NAVY_Departure,
                    ATC_NAVY_LSO,
                    ATC_NAVY_Marshal,
                }

                public enum wingmanstate
                {
                    NEUTRAL,
                    ENGAGING,
                    DEFENSIVE,
                    DISTRESS,
                    TRACKING,
                    GROUND,
                    REJOYCE
                }

                public static Dictionary<Recipientclass, commcat> CategoryMap = new Dictionary<Recipientclass, commcat>()
                {

                    { Recipientclasses.Undefined,   commcat.ALL         },
                    { Recipientclasses.Cockpit,     commcat.ALL         },
                    { Recipientclasses.Player,      commcat.PLAYER      },
                    { Recipientclasses.Navy_Player, commcat.NAVY_PLAYER },
                    { Recipientclasses.Flight,      commcat.WINGMAN     },
                    { Recipientclasses.JTAC,        commcat.JTAC        },
                    { Recipientclasses.ATC,         commcat.ATC         },
                    { Recipientclasses.Tanker,      commcat.TANKER      },
                    { Recipientclasses.AWACS,       commcat.AWACS       },
                    { Recipientclasses.Crew,        commcat.GROUND_CREW },
                    { Recipientclasses.AOCS,        commcat.ALL         },
                    { Recipientclasses.Aux,         commcat.ALL         },
                    { Recipientclasses.Cargo,       commcat.ExternalCargo   },
                    { Recipientclasses.Descent,     commcat.ExternalCargo   },
                    { Recipientclasses.RIO,         commcat.RIO         },

                    { Recipientclasses.ATC_NAVY_CARRIER,            commcat.ATC_NAVY_CARRIER            },
                    { Recipientclasses.ATC_NAVY_Approach_Tower,     commcat.ATC_NAVY_Approach_Tower     },
                    { Recipientclasses.ATC_NAVY_Departure,          commcat.ATC_NAVY_Departure          },
                    { Recipientclasses.ATC_NAVY_LSO,                commcat.ATC_NAVY_LSO                },
                    { Recipientclasses.ATC_NAVY_Marshal,            commcat.ATC_NAVY_Marshal            },

                };

                public static List<string> CleanedUpMessage(List<string> input)
                {
                    List<string> output = input;
                    if (input.Count > 1)
                    {
                        if (input[input.Count - 1].EndsWith("-end") && input[input.Count - 2].EndsWith("-begin"))
                        {
                            string combinedstring = output[input.Count - 2].Replace("-begin", "").Replace("Digits/", "") + output[input.Count - 1].Replace("-end", "").Replace("Digits/", "");
                            if (wavfileroots.ContainsKey(combinedstring))
                            {
                                output.RemoveAt(input.Count - 1);
                                output.RemoveAt(input.Count - 1);
                                output.Add(combinedstring);
                            }
                        }
                    }
                    return output;
                }
            }
        }
    }
}


