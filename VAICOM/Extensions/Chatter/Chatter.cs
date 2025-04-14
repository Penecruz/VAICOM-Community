using NAudio.Wave;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Timers;
using System.Windows.Forms;
using VAICOM.Static;

namespace VAICOM
{
    namespace Extensions
    {
        namespace Chatter
        {

            public static partial class AudioTimer
            {

                public static Dictionary<string, ResourceManager> ChatterCollection;
                public static System.Timers.Timer PlaybackTimer { get; set; }
                public static bool Created { get; set; }
                public static bool CurrentPlayStatus { get; set; }

                public static void Chatter_Initialize()
                {
                    try
                    {

                        // default values (if load based on theme fails)
                        State.chatterintervalmin = 4000;
                        State.chatterintervalmax = 22000;

                        // get themepack resource managers table
                        Log.Write("Adding themepack collections", Colors.Text);
                        ChatterCollection = new Dictionary<string, ResourceManager>();
                        ChatterCollection.Add("Default", Themepack.RedFlag.ResourceManager);
                        ChatterCollection.Add("NATO", Themepack.NATO.ResourceManager);
                        ChatterCollection.Add("Russia", Themepack.Russia.ResourceManager);
                        ChatterCollection.Add("Navy", Themepack.Navy.ResourceManager);
                        ChatterCollection.Add("RedFlag", Themepack.RedFlag.ResourceManager);
                        ChatterCollection.Add("Fallon", Themepack.Fallon.ResourceManager);
                        ChatterCollection.Add("Afghan", Themepack.Afghanistan.ResourceManager);
                        ChatterCollection.Add("WWII", Themepack.WWII.ResourceManager);
                        State.chatterthemes = new List<string>();
                        foreach (KeyValuePair<string, ResourceManager> theme in ChatterCollection)
                        {
                            State.chatterthemes.Add(theme.Key);
                        }

                        // set the name of the current collection (from config setting)
                        string currenttheme;
                        if (State.activeconfig.ChatterFolder.Equals("(AUTO)"))
                        {
                            currenttheme = State.currentmodule.Theme;
                        }
                        else
                        {
                            currenttheme = State.activeconfig.ChatterFolder;
                        }

                        // catch if somehow there was a change
                        if (!State.chatterthemes.Contains(currenttheme))
                        {
                            Log.Write("Chatter theme mismatch for " + currenttheme, Colors.Text);
                            State.activeconfig.ChatterFolder = State.chatterthemes[0]; // Default
                            currenttheme = State.activeconfig.ChatterFolder;
                        }

                        if (State.chatterthemesactivated)
                        {
                            Log.Write("Chatter theme set to " + currenttheme, Colors.Text);
                        }

                        // create list of resource names,
                        Log.Write("Adding chatter resources.. " + currenttheme, Colors.Text);
                        State.chatterresources = ChatterCollection[currenttheme].GetResourceSet(CultureInfo.CurrentUICulture, true, true);
                        State.chattersoundfiles = new List<string>();

                        //..add them to soundfiles table.
                        foreach (DictionaryEntry entry in State.chatterresources)
                        {
                            State.chattersoundfiles.Add(entry.Key.ToString());
                        }
                        Log.Write("Resources added. ", Colors.Text);

                        // initialize ready.
                        Log.Write("Chatter initialized. ", Colors.Text);
                        State.chatterinitalized = true;
                    }
                    catch (Exception e)
                    {
                        Log.Write("Chatter theme init: " + e.Message, Colors.Text);
                        State.chatterinitalized = false;
                    }
                }

                public static void Chatter_TimerPlayToggle()
                {
                    try
                    {
                        if (!State.chatterinitalized)
                        {
                            Chatter_Initialize();
                        }
                        if (CurrentPlayStatus == false)
                        {
                            Log.Write("Chatter start.", Colors.Text);
                            Chatter_TimerStart();
                        }
                        else
                        {
                            Log.Write("Chatter stop.", Colors.Text);
                            Chatter_TimerStop();
                        }
                        if (State.configwindowopen && (State.configurationwindow != null))
                        {
                            State.configurationwindow.Dispatcher.BeginInvoke((MethodInvoker)delegate
                            {
                                State.configurationwindow.Changechatterbug();
                            });
                        }
                    }
                    catch (Exception e)
                    {
                        Log.Write("Problems were reported with Chatter toggle. " + e.Message, Colors.Inline);
                    }
                }

                public static void Chatter_TimerStart()
                {
                    try
                    {
                        if (!Created)
                        {
                            PlaybackTimer = new System.Timers.Timer(1000);
                        }

                        PlaybackTimer.Start();
                        PlaybackTimer.Elapsed += Chatter_Timer_Elapsed_Handler;
                        CurrentPlayStatus = true;
                        State.chatteractive = true;

                    }
                    catch (Exception e)
                    {
                        Log.Write("Problems were reported with Chatter timer start. " + e.Message, Colors.Inline);
                    }
                }

                public static void Chatter_TimerStop()
                {
                    try
                    {
                        PlaybackTimer.Elapsed -= Chatter_Timer_Elapsed_Handler;
                        PlaybackTimer.Stop();
                        CurrentPlayStatus = false;
                        State.chatteractive = false;
                    }
                    catch (Exception e)
                    {
                        Log.Write("Problems were reported with Chatter timer stop. " + e.Message, Colors.Inline);
                    }
                }

                private static void Chatter_Timer_Elapsed_Handler(object sender, ElapsedEventArgs e)
                {
                    try
                    {

                        Random randomsoundfile = new Random();
                        int filenumber = randomsoundfile.Next(State.chattersoundfiles.Count);
                        double currentduration = 0; // default if nothing there

                        bool chatterextviewblocked = State.currentstate.viewexternal && !State.currentstate.soundsallowexternal;

                        if (!chatterextviewblocked && ((State.dcsrunning || !State.activeconfig.ChatterSilentOffline) && ((State.chattersoundfiles.Count > 0) & State.oneradioactive)))
                        {

                            object playbackfile = State.chatterresources.GetObject(State.chattersoundfiles[filenumber]);
                            Stream fragment = (Stream)playbackfile;
                            currentduration = (fragment.Length / 8); // for 8 bit 8khz mono

                            fragment.Seek(0, SeekOrigin.Begin);
                            WaveFileReader reader = new WaveFileReader(fragment);
                            var upsampler = new WaveFormatConversionStream(new WaveFormat(22050, 16, 1), reader); // was 8000

                            var volumeSampleProvider = new NAudio.Wave.SampleProviders.VolumeSampleProvider(upsampler.ToSampleProvider());

                            volumeSampleProvider.Volume = 3 * State.activeconfig.ChatterVolume;

                            var panningSampleProvider = new NAudio.Wave.SampleProviders.PanningSampleProvider(volumeSampleProvider);

                            int pan = 0;

                            switch (State.activeconfig.ChatterPanSetting)
                            {
                                case 2:
                                    Random rnd = new Random();
                                    int dice = rnd.Next(1, 2 + 1);
                                    switch (dice)
                                    {
                                        case 1:
                                            pan = -1;
                                            break;
                                        case 2:
                                            pan = +1;
                                            break;
                                    }
                                    break;
                                default:
                                    pan = State.activeconfig.ChatterPanSetting;
                                    break;
                            }

                            panningSampleProvider.Pan = pan;

                            if (State.activeconfig.Redirect_World_Speech)
                            {
                                State.ttsmixer.AddMixerInput(panningSampleProvider);
                            }
                            else
                            {
                                State.chatteroutput.Init(panningSampleProvider);
                                State.chatteroutput.Play();
                            }

                        }

                        // ...set new random interval for next snippet event.
                        Random randompausetime = new Random();
                        double newinterval = randompausetime.Next(State.chatterintervalmin, State.chatterintervalmax);
                        PlaybackTimer.Interval = currentduration + newinterval;
                        PlaybackTimer.Start();

                    }
                    catch (Exception a)
                    {
                        Log.Write("Problems were reported with the Chatter timer handler. " + a.Message, Colors.Inline);
                    }
                }


            }
        }
    }
}


