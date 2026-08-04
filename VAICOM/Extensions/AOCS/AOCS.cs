using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Speech.AudioFormat;
using System.Speech.Synthesis;
using System.Threading;
using System.Windows.Forms;
using VAICOM.Extensions.Kneeboard;
using VAICOM.Extensions.WorldAudio;
using VAICOM.Servers;
using VAICOM.Static;

namespace VAICOM
{
    namespace Extensions
    {


        namespace AOCS
        {

            public static class AOCSProvider
            {

                public static void AddAOCSUnit()
                {
                    // add virtual AOCS unit to Aux recipients cat.
                    try
                    {
                        Server.DcsUnit AOCS = new Server.DcsUnit();

                        if (State.currentstate.timer < 5) // get only at start of mission
                        {
                            try
                            {
                                Server.homebaselocation = State.currentstate.availablerecipients["ATC"][0].pos;
                            }
                            catch
                            {
                            }
                        }

                        AOCS.index = 1;
                        AOCS.id_ = 123456789;
                        AOCS.reccat = "Aux";
                        AOCS.callsign = State.aocscallsign;
                        AOCS.descr = "auxiliary unit";
                        AOCS.pos = Server.homebaselocation;
                        AOCS.fullname = "AOCS";
                        AOCS.coalition = State.currentstate.availablerecipients["Player"][0].coalition;
                        AOCS.freq = "284000000";
                        AOCS.altfreq = new List<string>() { "142000000", "071000000" };
                        AOCS.mod = "AM"; // AM
                        AOCS.status = "tuned";
                        AOCS.range = (int)(Helpers.Common.GetRange(AOCS.pos, State.currentstate.availablerecipients["Player"][0].pos));

                        State.currentstate.availablerecipients["Aux"].Add(AOCS);
                        State.SelectedUnit["Aux"] = AOCS;
                    }
                    catch
                    {
                    }
                }

                public static void AOCS_SpeakPhraseAsync(string str)
                {

                    Thread.Sleep(500);

                    try
                    {
                        // generate speech adio..
                        var wavstream = new MemoryStream();
                        var outformat = new SpeechAudioFormatInfo(8000, AudioBitsPerSample.Sixteen, AudioChannel.Mono); //8000 //11025

                        State.synth.SetOutputToWaveStream(wavstream);
                        var mi = State.synth.GetType().GetMethod("SetOutputStream", BindingFlags.Instance | BindingFlags.NonPublic);
                        mi.Invoke(State.synth, new object[] { wavstream, outformat, true, true });

                        PromptBuilder builder = new PromptBuilder();
                        builder.AppendText(str);
                        State.synth.Speak(builder);
                        wavstream.Seek(0, SeekOrigin.Begin);

                        // add to mixer
                        WaveFileReader reader = new WaveFileReader(wavstream);
                        var upsampler = new WaveFormatConversionStream(new WaveFormat(22050, 16, 1), reader);
                        var vol = new NAudio.Wave.SampleProviders.VolumeSampleProvider(upsampler.ToSampleProvider());
                        vol.Volume = 0.8f; //Processor.SetTTSVolume();
                        var pan = new NAudio.Wave.SampleProviders.PanningSampleProvider(vol);
                        pan.Pan = State.activeconfig.PanSetting_AOCS; // change

                        if (State.activeconfig.Redirect_World_Speech)
                        {
                            State.ttsmixer.AddMixerInput(pan);
                        }
                        else
                        {
                            State.ttsoutput.Init(pan);
                            State.ttsoutput.PlaybackStopped += new EventHandler<StoppedEventArgs>(Processor.audioOutput_PlaybackStopped);
                            State.ttsoutput.Play();
                        }


                        State.briefinginprogress = true;

                    }
                    catch (Exception e)
                    {
                        Log.Write("TTS error: " + e.StackTrace, Colors.Inline);
                    }
                }

                public static void AOCS_OnSpeechStopped(object sender, StoppedEventArgs args)
                {
                    UI.Playsound.Endofcall(); // radio click

                    try // light receive msg bug
                    {
                        if (State.configwindowopen && (State.configurationwindow != null))
                        {
                            State.configurationwindow.Dispatcher.BeginInvoke((MethodInvoker)delegate
                            {
                                State.configurationwindow.RCV_Bug.Visibility = System.Windows.Visibility.Visible;
                            });
                        }
                    }
                    catch
                    {
                    }

                    //State.ttsoutput.Dispose();
                    State.briefinginprogress = false;
                }

                // init speech synth
                public static void AOCS_InitSpeechSynth()
                {

                    State.synth.Volume = 90;
                    //State.synth.Volume  = 305*(State.currentstate.options.sound.headphones * State.currentstate.options.sound.volume)/10000;

                    int basevolume = State.synth.Volume;
                    int volume;

                    if (State.activeconfig.Redirect_World_Speech)
                    {
                        volume = (int)(State.activeconfig.TTSVolume / 0.5f) * basevolume;
                        if (volume < 0)
                        {
                            volume = 0;
                        }
                        if (volume > 100)
                        {
                            volume = 100;
                        }

                    }
                    else
                    {
                        volume = basevolume;
                    }

                    State.synth.Rate = 0;

                    try
                    {

                        int countmale = 0;
                        int countfemale = 0;

                        foreach (InstalledVoice Voice in State.synth.GetInstalledVoices())
                        {
                            if (Voice.VoiceInfo.Gender.Equals(VoiceGender.Male))
                            {
                                countmale = countmale + 1;
                            }
                            if (Voice.VoiceInfo.Gender.Equals(VoiceGender.Female))
                            {
                                countfemale = countfemale + 1;
                            }
                        }

                        if (countfemale > 0) // prefer female AOCS operator if available
                        {
                            State.synth.SelectVoiceByHints(VoiceGender.Female);
                        }
                        else
                        {
                            if (countmale > 0)
                            {
                                State.synth.SelectVoiceByHints(VoiceGender.Male);
                            }
                        }

                    }
                    catch (Exception e)
                    {
                        Log.Write(e.Message, Colors.Inline);
                    }
                }

                // read mission briefing
                public static void AOCS_ReadBriefing(bool readfromauto)
                {
                    try
                    {
                        if (State.briefinginprogress)
                        {
                            Log.Write("AOCS: Briefing readout stopped.", Colors.Message);
                            if (!State.activeconfig.Redirect_World_Speech)
                            {
                                State.ttsoutput.Stop();
                            }
                            State.briefinginprogress = false;
                        }
                        else
                        {

                            State.synth.SpeakAsyncCancelAll();

                            if (State.dcsrunning)
                            {
                                Log.Write("AOCS: Starting mission briefing readout.", Colors.Message);
                                string briefing = "";

                                // get player unit
                                Server.DcsUnit playerunit = new Server.DcsUnit();
                                string playercallsign = "";
                                try
                                {
                                    playerunit = State.currentstate.availablerecipients["Player"][0];
                                    playercallsign = playerunit.callsign;
                                }
                                catch
                                {
                                }

                                briefing = briefing + Helpers.Common.ProcessBrevity(playercallsign) + ", " + State.aocscallsign + ", Mission Brief: ";
                                briefing = briefing + Helpers.Common.ProcessBrevity(State.currentstate.missiontitle) + ".\n" + Helpers.Common.ProcessBrevity(State.currentstate.missionbriefing);

                                if (!State.activeconfig.BriefConcise)
                                {
                                    briefing = briefing + "\n" + Helpers.Common.ProcessBrevity(State.currentstate.missiondetails);
                                }

                                State.synth.SpeakCompleted += AOCS_FinishBriefing;
                                Thread.Sleep(1000);
                                AOCS_InitSpeechSynth();
                                AOCS_SpeakPhraseAsync(briefing);
                                State.briefinginprogress = true;
                            }
                            else
                            {
                                Log.Write("AOCS: Briefing: no active mission.", Colors.Message);
                                Thread.Sleep(1000);
                                AOCS_InitSpeechSynth();
                                AOCS_SpeakPhraseAsync("There is currently no active mission.");
                                State.briefinginprogress = false;
                            }
                        }

                    }
                    catch (Exception e)
                    {
                        Log.Write("Briefing error: " + e.Message, Colors.Inline);
                    }
                }

                // called on readout finish briefing
                public static void AOCS_FinishBriefing(object sender, SpeakCompletedEventArgs args)
                {
                    State.briefinginprogress = false;
                    return;
                }

                // triggered by "interrogate" or "briefing" command -> reads briefing or interrogate results
                public static void AOCS_CallDeepInterrogate()
                {
                    try
                    {
                        // briefing command
                        if (State.currentkey["command"].Equals("readbriefing"))
                        {
                            //UI.Playsound.Commandcomplete();
                            if (State.currentstate.multiplayer && !State.activeconfig.MP_AOCS)
                            {
                                UI.Playsound.Sorry();
                                Log.Write("AOCS is deactivated for Multiplayer in Preferences.", Colors.Warning);
                            }
                            else
                            {
                                AOCS_ReadBriefing(false);
                            }
                        }

                        //interrogate command
                        if (State.currentkey["command"].Equals("state"))
                        {
                            if (State.have["recipient"]) // for specific unit or cat
                            {
                                if (State.activeconfig.DeepInterrogate)
                                {
                                    if (State.currentstate.multiplayer && !State.activeconfig.MP_AOCS)
                                    {
                                        UI.Playsound.Sorry();
                                        Log.Write("AOCS is deactivated for Multiplayer in Preferences.", Colors.Warning);
                                    }
                                    else
                                    {
                                        //UI.Playsound.Commandcomplete();
                                        string classstr = State.currentrecipientclass.Name;
                                        List<Server.DcsUnit> unitlist = new List<Server.DcsUnit>();

                                        bool singleunit;

                                        if (!State.currentkey["recipient"].Equals("aocs") & !State.currentkey["recipient"].Equals("aux") & !State.currentkey["recipient"].Equals("cargo") & !State.currentkey["recipient"].Equals("crew"))
                                        {

                                            if (!State.calledisclass) // called for just a single unit
                                            {
                                                unitlist.Add(State.currentmessageunit);
                                                singleunit = true;
                                            }
                                            else // called for an entire recipient class
                                            {
                                                unitlist = State.currentstate.availablerecipients[classstr];
                                                singleunit = false;
                                            }

                                            AOCS_ReadUnits(unitlist, classstr, singleunit);
                                        }
                                        else // aocs was called: do general mission status
                                        {
                                            AOCS_ReadMissionOverview();
                                        }
                                    }
                                }
                                else
                                {
                                    Log.Write("Deep Interrogate is disabled in preferences.", Colors.Warning);
                                    UI.Playsound.Sorry();
                                }
                            }
                            else
                            {
                                if (State.genericstaterequest)
                                {
                                    Log.Write("Generic state requested.", Colors.Inline);

                                    if (State.activeconfig.DeepInterrogate)
                                    {
                                        if (State.currentstate.multiplayer && !State.activeconfig.MP_AOCS)
                                        {
                                            UI.Playsound.Sorry();
                                            Log.Write("AOCS is deactivated for Multiplayer in Preferences.", Colors.Warning);
                                        }
                                        else
                                        {
                                            AOCS_ReadMissionOverview();
                                        }
                                    }
                                    else
                                    {
                                        UI.Playsound.Sorry();
                                        Log.Write("Deep Interrogate is disabled in preferences.", Colors.Warning);
                                    }
                                }
                            }
                        }

                    }
                    catch (Exception e)
                    {
                        Log.Write("Interrogate: " + e.StackTrace, Colors.Inline);
                    }

                    return;
                }

                // mission status overview (all cats) for interrogate without unit/category
                public static void AOCS_ReadMissionOverview()
                {

                    if (!State.dcsrunning)
                    {
                        return;
                    }

                    try
                    {

                        //UI.Playsound.Commandcomplete();
                        string kneeboardbriefing = "";
                        string unitsbriefing = "";
                        State.synth.SpeakAsyncCancelAll();

                        Log.Write("AOCS: Reading mission units summary.", Colors.Message);

                        // get player unit
                        Server.DcsUnit playerunit = new Server.DcsUnit();
                        string playercallsign = "";
                        try
                        {
                            playerunit = State.currentstate.availablerecipients["Player"][0];
                            playercallsign = playerunit.callsign;
                        }
                        catch
                        {
                        }

                        unitsbriefing = unitsbriefing + Helpers.Common.ProcessBrevity(playercallsign) + ", " + State.aocscallsign + ", ";
                        unitsbriefing = unitsbriefing + "Units status.\n";

                        foreach (KeyValuePair<string, List<Server.DcsUnit>> category in State.currentstate.availablerecipients)
                        {

                            if (category.Key.Equals("Flight") || category.Key.Equals("JTAC") || category.Key.Equals("Tanker") || category.Key.Equals("AWACS") || category.Key.Equals("ATC"))
                            {

                                string counterstring;

                                if (category.Value.Count == 0)
                                {
                                    counterstring = "No units";
                                }
                                else
                                {
                                    if (category.Value.Count == 1)
                                    {
                                        counterstring = "Single Unit";
                                    }
                                    else
                                    {
                                        counterstring = category.Value.Count.ToString() + " Units";
                                    }
                                }

                                string classstr = category.Key;

                                unitsbriefing = unitsbriefing + Helpers.Common.ProcessBrevity(classstr) + ": " + counterstring;

                                if (category.Key.Equals("ATC"))
                                {
                                    try
                                    {
                                        unitsbriefing = unitsbriefing + ", nearest airfield: " + Helpers.Common.ProcessBrevity(category.Value[0].fullname + ", at " + (category.Value[0].range / 1852).ToString() + " miles.");
                                    }
                                    catch
                                    {
                                    }
                                }
                                else
                                {
                                    foreach (Server.DcsUnit unit in category.Value)
                                    {
                                        string indexstr = unit.index.ToString() + ": ";

                                        if (category.Value.Count < 2)
                                        {
                                            indexstr = "";
                                        }
                                        kneeboardbriefing += "- " + unit.callsign.Replace("-", "") + "\n";
                                        unitsbriefing = unitsbriefing + ", " + indexstr + Helpers.Common.ProcessBrevity(unit.callsign);
                                    }
                                }

                                unitsbriefing = unitsbriefing + ".\n";
                            }
                        }

                        State.synth.SpeakCompleted += AOCS_FinishBriefing;
                        Thread.Sleep(1000);
                        AOCS_InitSpeechSynth();
                        AOCS_SpeakPhraseAsync(unitsbriefing);
                        State.briefinginprogress = true;

                        // kneeboard! (with delay)

                        KneeboardUpdater.UpdateMessagelogForCat("AOCS", "*" + kneeboardbriefing);


                    }
                    catch (Exception e)
                    {
                        Log.Write(e.StackTrace, Colors.Inline);
                        State.briefinginprogress = false;
                    }

                }

                // details for individual units/cats
                public static void AOCS_ReadUnits(List<Server.DcsUnit> unitlist, string classstr, bool single)
                {
                    if (!State.dcsrunning)
                    {
                        return;
                    }

                    string unitsbriefing = "";

                    try
                    {
                        State.synth.SpeakAsyncCancelAll();

                        Log.Write("AOCS: reading unit details for " + classstr, Colors.Message);

                        // get player unit
                        Server.DcsUnit playerunit = new Server.DcsUnit();
                        string playercallsign = "";
                        try
                        {
                            playerunit = State.currentstate.availablerecipients["Player"][0];
                            playercallsign = playerunit.callsign;
                        }
                        catch
                        {
                        }

                        unitsbriefing = unitsbriefing + Helpers.Common.ProcessBrevity(playercallsign) + ", " + State.aocscallsign + ",.. ";

                        string counterstring = "";

                        if (!single)
                        {

                            if (unitlist.Count == 0)
                            {
                                counterstring = "No units";
                            }
                            if (unitlist.Count == 1)
                            {
                                counterstring = "Single unit";
                            }
                            else
                            {
                                counterstring = unitlist.Count.ToString() + " Units";
                            }
                        }

                        if (classstr.Equals("Flight"))
                        {
                            classstr = "Your " + classstr;
                        }

                        unitsbriefing = unitsbriefing + Helpers.Common.ProcessBrevity(classstr) + ": " + counterstring + ", ";

                        int speechcounter = 0;

                        List<string> kneeboardsummaries = new List<string>();

                        foreach (Server.DcsUnit unit in unitlist)
                        {

                            if (!unit.id_.Equals(playerunit.id_) & (speechcounter < 3))
                            {

                                speechcounter = speechcounter + 1;

                                string normalizedfeq = Helpers.Common.NormalizeFreqString(unit.freq); //"100.000"

                                // get sorted frequencies table
                                string altfreqs = "";
                                List<string> freqlistsorted = new List<string>();
                                freqlistsorted.Add(normalizedfeq);
                                foreach (string f in unit.altfreq)
                                {
                                    freqlistsorted.Add(Helpers.Common.NormalizeFreqString(f));
                                }
                                freqlistsorted = Helpers.Common.SortFrequencies(freqlistsorted);

                                foreach (string f in freqlistsorted)
                                {
                                    altfreqs = altfreqs + " " + f;
                                }

                                string primaryfreq;
                                if (freqlistsorted.Count > 1)
                                {
                                    primaryfreq = freqlistsorted[freqlistsorted.Count - 2];
                                }
                                else
                                {
                                    primaryfreq = freqlistsorted[0];
                                }

                                string freqbase = normalizedfeq.Substring(0, 1) + " " + normalizedfeq.Substring(1, 1) + " " + normalizedfeq.Substring(2, 1);
                                string freqprnt = normalizedfeq.Substring(0, 1) + normalizedfeq.Substring(1, 1) + normalizedfeq.Substring(2, 1);
                                string freqdec = normalizedfeq.Substring(4, 1) + normalizedfeq.Substring(5, 1) + normalizedfeq.Substring(6, 1);

                                string indexstr = unit.index.ToString() + ": ";
                                string elevstr = (100 * Math.Round((unit.pos.y) * 3.2808399 / 100)).ToString();

                                string bearingraw = ("00" + Helpers.Common.Modulo((int)((Math.Round((Math.Atan2((unit.pos.z - playerunit.pos.z), (unit.pos.x - playerunit.pos.x))) * (180 / Math.PI)))), 360).ToString());
                                bearingraw = bearingraw.Substring(bearingraw.Length - 3);
                                string bearing = bearingraw.Substring(0, 1) + " " + bearingraw.Substring(1, 1) + " " + bearingraw.Substring(2, 1);

                                string rangestr = (unit.range / 1852).ToString();

                                string modstr = unit.getmodstr();
                                string speakmodstr = "";
                                if (modstr.Equals("FM"))
                                {
                                    speakmodstr = "F M";
                                }

                                string readstr;
                                string printstring;
                                string kneeboardstring = "";
                                string speechfreqdec = freqdec;

                                if (freqdec.Equals("000"))
                                {
                                    speechfreqdec = "zero";
                                }

                                if (unitlist.Count < 2)
                                {
                                    indexstr = "";
                                }

                                string playerstr = State.currentstate.playerusername;

                                string namer = "";
                                string elevationstr = "";
                                if (!classstr.Equals("ATC"))
                                {
                                    namer = unit.callsign;
                                    elevationstr = ", altitude " + elevstr; //change to altitude Pene testing
                                }
                                else
                                {
                                    namer = unit.fullname;
                                }

                                readstr = indexstr + Helpers.Common.ProcessBrevity(namer) + ". Bearing " + bearing + " for " + rangestr + elevationstr + ", contact " + speakmodstr + " " + Helpers.Common.ReadFrequency(primaryfreq) + ".\n";
                                printstring = " " + indexstr + " " + namer + ". Bearing " + bearingraw + " for " + rangestr + elevationstr + ", contact " + modstr + altfreqs + " MHz";
                                kneeboardstring = namer + " @ " + bearingraw + "/" + rangestr; //unit.index.ToString() + " " + + "/" + elevstr                           
                                if (classstr.Equals("Tanker") || classstr.Equals("AWACS"))
                                {
                                    kneeboardstring += "/" + elevstr;
                                }
                                kneeboardstring = (kneeboardstring + " ").Replace("000 ", "k") + "\n" + modstr + altfreqs;

                                kneeboardsummaries.Add(kneeboardstring);

                                unitsbriefing = unitsbriefing + readstr;
                                Log.Write(printstring, Colors.Message);

                            }
                        }

                        // send to kneeboard (ewith delay)
                        string logupdate = " AOCS: ";
                        foreach (string str in kneeboardsummaries)
                        {
                            logupdate += str + "\n";
                        }

                        State.synth.SpeakCompleted += AOCS_FinishBriefing;
                        Thread.Sleep(1000);
                        AOCS_InitSpeechSynth();
                        AOCS_SpeakPhraseAsync(unitsbriefing);
                        State.briefinginprogress = true;

                        KneeboardUpdater.UpdateMessagelogForCat(classstr.ToUpper(), logupdate);

                    }
                    catch (Exception e)
                    {
                        State.briefinginprogress = false;
                        Log.Write(e.StackTrace, Colors.Inline);
                    }
                }

            }
        }
    }
}


