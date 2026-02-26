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
                public static System.Timers.Timer FrequencyMonitorTimer { get; set; }
                public static bool Created { get; set; }
                public static bool CurrentPlayStatus { get; set; }
                public static bool ManuallyStopped { get; set; } = false; // Tracks if chatter was manually stopped

                public static void Chatter_Initialize()
                {
                    try
                    {
                        // Default values (if load based on theme fails)
                        State.chatterintervalmin = 4000;
                        State.chatterintervalmax = 22000;

                        // Get themepack resource managers table
                        Log.Write("Adding themepack collections", Colors.Text);
                        ChatterCollection = new Dictionary<string, ResourceManager>();
                        ChatterCollection.Add("Default", Themepack.RedFlag.ResourceManager);
                        ChatterCollection.Add("NATO", Themepack.NATO.ResourceManager);
                        ChatterCollection.Add("Russia", Themepack.Russia.ResourceManager);
                        ChatterCollection.Add("Navy", Themepack.Navy.ResourceManager);
                        ChatterCollection.Add("RedFlag", Themepack.RedFlag.ResourceManager);
                        ChatterCollection.Add("Fallon", Themepack.Fallon.ResourceManager);
                        ChatterCollection.Add("Afghan", Themepack.Afghanistan.ResourceManager);
                        ChatterCollection.Add("Andersen", Themepack.Andersen.ResourceManager);
                        ChatterCollection.Add("WWII", Themepack.WWII.ResourceManager);
                        State.chatterthemes = new List<string>();
                        foreach (KeyValuePair<string, ResourceManager> theme in ChatterCollection)
                        {
                            State.chatterthemes.Add(theme.Key);
                        }

                        // Set the name of the current collection (from config setting)
                        string currenttheme;
                        if (State.activeconfig.ChatterFolder.Equals("(AUTO)"))
                        {
                            currenttheme = State.currentmodule.Theme;
                        }
                        else
                        {
                            currenttheme = State.activeconfig.ChatterFolder;
                        }

                        // Catch if somehow there was a change
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

                        // Create list of resource names
                        Log.Write("Adding chatter resources.. " + currenttheme, Colors.Text);
                        State.chatterresources = ChatterCollection[currenttheme].GetResourceSet(CultureInfo.CurrentUICulture, true, true);
                        State.chattersoundfiles = new List<string>();

                        // Add them to soundfiles table
                        foreach (DictionaryEntry entry in State.chatterresources)
                        {
                            State.chattersoundfiles.Add(entry.Key.ToString());
                        }
                        Log.Write("Resources added. ", Colors.Text);

                        // Initialize frequency monitor
                        InitializeFrequencyMonitor();

                        // Auto start chatter if enabled
                        if (State.activeconfig.ChatterAutostart)
                        {
                            Log.Write("Chatter auto-start enabled. Starting chatter...", Colors.Text);
                            Chatter_TimerStart();
                            FrequencyMonitorTimer.Start(); // Start frequency monitoring
                        }

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
                            FrequencyMonitorTimer.Start(); // Ensure frequency monitoring starts
                            ManuallyStopped = false; // Reset manual stop flag
                        }
                        else
                        {
                            Log.Write("Chatter stop.", Colors.Text);
                            Chatter_TimerStop();
                            FrequencyMonitorTimer.Stop(); // Ensure frequency monitoring stops
                            ManuallyStopped = true; // Set manual stop flag
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

                private static bool hasLoggedChatterCondition = false; // Flag to track if the "conditions not met" message has been logged
                private static bool hasLoggedChatterPlaying = false;   // Flag to track if the "conditions met" message has been logged

private static void Chatter_Timer_Elapsed_Handler(object sender, ElapsedEventArgs e)
{
    try
    {
        Random randomsoundfile = new Random();
        int filenumber = randomsoundfile.Next(State.chattersoundfiles.Count);
        double currentduration = 0; // default if nothing there

        bool chatterextviewblocked = State.currentstate.viewexternal && !State.currentstate.soundsallowexternal;

        // Check if any radio is tuned to 281.000 MHz
        string targetFrequency = "281000"; // 281.000 MHz in 6-digit format
        bool isFrequencyMatched = false;

        if (State.activeconfig.RequireFrequency281000) // Check if frequency requirement is enabled
        {
            foreach (var radio in State.currentstate.radios)
            {
                if (radio.on || !State.activeconfig.ChatterSilentOffline) // Ignore radio power if "Req Radio on & Freq" is unchecked
                {
                    string normalizedFrequency = NormalizeFrequency(radio.frequency);

                    if (normalizedFrequency == targetFrequency)
                    {
                        isFrequencyMatched = true;
                        break;
                    }
                }
            }
        }
        else
        {
            isFrequencyMatched = true; // If frequency requirement is disabled, always match
        }

        if (!chatterextviewblocked &&
            (State.dcsrunning || !State.activeconfig.ChatterSilentOffline) &&
            (State.chattersoundfiles.Count > 0 && (State.oneradioactive || !State.activeconfig.ChatterSilentOffline)) &&
            isFrequencyMatched)
        {
            if (!hasLoggedChatterPlaying) // Log only if the message hasn't been logged yet
            {
                Log.Write("Conditions met for playing chatter. Starting playback.", Colors.Text);
                hasLoggedChatterPlaying = true; // Set the flag to true after logging
            }

            hasLoggedChatterCondition = false; // Reset the "conditions not met" flag

            object playbackfile = State.chatterresources.GetObject(State.chattersoundfiles[filenumber]);
            if (playbackfile == null)
            {
                Log.Write("DEBUG: playbackfile is null. Unable to play chatter.", Colors.Text);
                return;
            }

            Stream fragment = (Stream)playbackfile;

            fragment.Seek(0, SeekOrigin.Begin);
            WaveFileReader reader = new WaveFileReader(fragment);
            currentduration = reader.TotalTime.TotalMilliseconds;

            var sampleProvider = reader.ToSampleProvider(); // Add new sample provider offer greater flexibility for processing (resampling, panning, volume, etc)
                            if (sampleProvider.WaveFormat.Channels > 1)
            {
                sampleProvider = new NAudio.Wave.SampleProviders.StereoToMonoSampleProvider(sampleProvider);
            }

            if (sampleProvider.WaveFormat.SampleRate != 22050)
            {
                sampleProvider = new NAudio.Wave.SampleProviders.WdlResamplingSampleProvider(sampleProvider, 22050);
            }

            var volumeSampleProvider = new NAudio.Wave.SampleProviders.VolumeSampleProvider(sampleProvider);
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
        else
        {
            if (!hasLoggedChatterCondition) // Log only if the message hasn't been logged yet
            {
                Log.Write("Conditions not met for playing chatter. Check Radio on and Frequency", Colors.Text);
                hasLoggedChatterCondition = true; // Set the flag to true after logging
            }

            hasLoggedChatterPlaying = false; // Reset the "conditions met" flag
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

                public static void InitializeFrequencyMonitor()
                {
                    try
                    {
                        FrequencyMonitorTimer = new System.Timers.Timer(2000); // Check every 2 seconds
                        FrequencyMonitorTimer.Elapsed += FrequencyMonitor_Elapsed_Handler;
                        Log.Write("Frequency Monitor Timer initialized.", Colors.Text);
                    }
                    catch (Exception e)
                    {
                        Log.Write("Frequency Monitor Timer init: " + e.Message, Colors.Text);
                    }
                }

                // Local frequency normalization function
                private static string NormalizeFrequency(string inputFrequency)
                {
                    if (string.IsNullOrWhiteSpace(inputFrequency))
                    {
                        return string.Empty;
                    }

                    // Remove any non-numeric characters
                    string sanitizedInput = System.Text.RegularExpressions.Regex.Replace(inputFrequency, @"[^\d]", "");

                    // Check if the input is a valid number
                    if (!long.TryParse(sanitizedInput, out long frequency))
                    {
                        Log.Write($"Invalid frequency format: {inputFrequency}", Colors.Inline);
                        return string.Empty;
                    }

                    // Truncate to 6 digits if the frequency is too long
                    if (sanitizedInput.Length > 6)
                    {
                        sanitizedInput = sanitizedInput.Substring(0, 6);
                    }

                    return sanitizedInput;
                }

                private static void FrequencyMonitor_Elapsed_Handler(object sender, ElapsedEventArgs e)
                {
                    try
                    {
                        if (!State.activeconfig.RequireFrequency281000 || ManuallyStopped)
                        {
                            return; // Skip monitoring if frequency requirement is disabled or chatter was manually stopped
                        }

                        string targetFrequency = "281000"; // 281.000 MHz in 6-digit format
                        bool isFrequencyMatched = false;

                        foreach (var radio in State.currentstate.radios)
                        {
                            if (radio.on || !State.activeconfig.ChatterSilentOffline) // Ignore radio power if "Req Radio on & Freq" is unchecked
                            {
                                string normalizedFrequency = NormalizeFrequency(radio.frequency);
                                // Commenting out the debug log to reduce logging clutter
                                // Log.Write($"DEBUG: Monitoring radio {radio.deviceid} with frequency {normalizedFrequency}", Colors.Text);

                                if (normalizedFrequency == targetFrequency)
                                {
                                    isFrequencyMatched = true;
                                    break;
                                }
                            }
                        }

                        if (isFrequencyMatched)
                        {
                            if (!State.chatteractive && !ManuallyStopped)
                            {
                                Log.Write("Frequency matched: Starting chatter.", Colors.Text);
                                Chatter_TimerStart();
                            }
                        }
                        else
                        {
                            if (State.chatteractive)
                            {
                                Log.Write("Frequency mismatch: Stopping chatter.", Colors.Text);
                                Chatter_TimerStop();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Write("Error in Frequency Monitor Timer: " + ex.Message, Colors.Inline);
                    }
                }
            }
        }
    }
}
