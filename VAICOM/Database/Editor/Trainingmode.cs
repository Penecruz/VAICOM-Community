using System;
using System.Collections.Generic;
using System.Speech.Recognition;
using System.IO;
using System.Diagnostics;
using VAICOM.Static; // Ensure Log, Colors, and State are accessible
using VAICOM.PushToTalk; // Ensure PTT_Manage_Listen_VA is accessible
using VAICOM.UI; // Ensure Playsound is accessible

namespace VAICOM
{
    namespace Database
    {
        public partial class AliasEditor
        {
            // offline keywords training
            public static void TrainingStartStop()
            {
                if (!State.trainerrunning)
                {
                    SpeechTrainer.Initialize();
                    SpeechTrainer.Start();
                }
                else
                {
                    SpeechTrainer.Stop();
                }
            }

            // trainer functions
            public static class SpeechTrainer
            {
                public static SpeechRecognizer trainer;
                private static bool usingVoiceAccess = false;
                private static bool? previousVALIsteningState;

                public static void Start()
                {
                    try
                    {
                        try
                        {
                            if (State.Proxy != null)
                            {
                                previousVALIsteningState = State.Proxy.State.GetListeningEnabled();
                                if (previousVALIsteningState.Value)
                                {
                                    State.Proxy.State.SetListeningEnabled(false);
                                }
                            }
                        }
                        catch
                        {
                        }

                        if (usingVoiceAccess)
                        {
                            Log.Write("Voice Access is running.", Colors.System);
                        }
                        else if (trainer != null)
                        {
                            Log.Write("Windows Speech Recognition is running.", Colors.System);
                        }
                        else
                        {
                            Log.Write("No speech recognition system is running.", Colors.Warning);
                            return;
                        }

                        Playsound.Commandcomplete();
                        Log.Write("Offline keyword training initialized", Colors.System);
                        Log.Write("Ready for training.", Colors.Message);

                        // Attach event handlers if using Windows Speech Recognition
                        if (!usingVoiceAccess && trainer != null)
                        {
                            trainer.SpeechRecognized += rec_SpeechRecognized;
                            trainer.SpeechRecognitionRejected += rec_SpeechRejected;
                            trainer.StateChanged += rec_StateChanged;
                        }

                        State.trainerrunning = true;
                    }
                    catch (Exception ex)
                    {
                        Log.Write($"Error during Start: {ex.Message}", Colors.Warning);
                    }
                }

                public static void Stop()
                {
                    try
                    {
                        if (usingVoiceAccess)
                        {
                            Log.Write("Voice Access is running. Attempting to stop it...", Colors.System);

                            // Find and stop the Voice Access process
                            foreach (var process in Process.GetProcessesByName("VoiceAccess"))
                            {
                                try
                                {
                                    process.Kill();
                                    Log.Write("Voice Access stopped successfully.", Colors.System);
                                }
                                catch (Exception ex)
                                {
                                    Log.Write($"Failed to stop Voice Access: {ex.Message}", Colors.Warning);
                                }
                            }
                        }
                        else if (trainer != null)
                        {
                            // Detach event handlers for Windows Speech Recognition
                            trainer.SpeechRecognized -= rec_SpeechRecognized;
                            trainer.SpeechRecognitionRejected -= rec_SpeechRejected;
                            trainer.StateChanged -= rec_StateChanged;
                        }

                        State.trainerrunning = false;

                        try
                        {
                            if (State.Proxy != null && previousVALIsteningState.HasValue)
                            {
                                State.Proxy.State.SetListeningEnabled(previousVALIsteningState.Value);
                            }
                        }
                        catch
                        {
                        }
                        finally
                        {
                            previousVALIsteningState = null;
                        }

                        // Log exit messages and reset PTT
                        Log.Write("Keyword training finished.", Colors.Message);
                        Log.Write("----------------------------------", Colors.Message);
                        PTT.PTT_Manage_Listen_VA(State.activeconfig.ReleaseHot);

                        Playsound.Commandcomplete();
                    }
                    catch (Exception ex)
                    {
                        Log.Write($"Error during Stop: {ex.Message}", Colors.Warning);
                    }
                }

                // state changes
                public static void rec_StateChanged(object sender, StateChangedEventArgs e)
                {
                    string result = string.Format("State changed");
                }

                // recognized handler
                public static void rec_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
                {
                    string result = string.Format("Keyword recognized: " + e.Result.Text + " with confidence {0} %", (e.Result.Confidence * 100).ToString().Substring(0, 2));
                    Log.Write(result, Colors.Message);
                    Playsound.Commandcomplete();
                }

                // unrecognized handler
                public static void rec_SpeechRejected(object sender, SpeechRecognitionRejectedEventArgs e)
                {
                    Playsound.Error();
                }

                public static void Exit()
                {
                }

                public static void Terminate()
                {
                    trainer.Dispose();
                }

                // startup
                public static void Initialize()
                {
                    try
                    {
                        Log.Write("----------------------------------", Colors.Message);
                        Log.Write("Initializing Training Mode", Colors.Message);

                        // Check if Voice Access should take priority
                        if (State.UseVoiceAccessPriority && IsVoiceAccessAvailable())
                        {
                            Log.Write("Voice Access priority enabled. Launching Voice Access...", Colors.System);
                            LaunchVoiceAccess();

                            usingVoiceAccess = true; // Using Voice Access
                            State.trainerrunning = true;
                            Log.Write("SpeechTrainer is now running with Voice Access.", Colors.System);
                            return; // Exit early since Voice Access is being used
                        }

                        // Check for Windows Speech Recognition
                        try
                        {
                            trainer = new SpeechRecognizer(); // Attempt to initialize Windows Speech Recognition
                            trainer.UnloadAllGrammars();
                            trainer.MaxAlternates = 0;

                            Log.Write("Windows Speech Recognition detected and initialized.", Colors.System);

                            // Add all aliases to new grammar
                            State.trainerchoices = new Choices();
                            foreach (KeyValuePair<string, Dictionary<string, string>> set in Aliases.categories)
                            {
                                foreach (KeyValuePair<string, string> listing in set.Value)
                                {
                                    State.trainerchoices.Add(listing.Key.Replace("*", ""));
                                }
                            }

                            // Load grammar and start
                            var grammarbuilder = new GrammarBuilder(State.trainerchoices);
                            var grammar = new Grammar(grammarbuilder);
                            trainer.LoadGrammar(grammar);
                            trainer.Enabled = true;

                            Log.Write("VAICOM PRO command phrases loaded", Colors.System);

                            usingVoiceAccess = false; // Using Windows Speech Recognition
                            State.trainerrunning = true;
                            Log.Write("SpeechTrainer is now running with Windows Speech Recognition.", Colors.System);
                            return; // Exit early since Windows Speech Recognition is being used
                        }
                        catch (Exception ex)
                        {
                            Log.Write("Windows Speech Recognition not available: " + ex.Message, Colors.Warning);
                        }

                        // If Windows Speech Recognition is not available, check for Voice Access
                        if (IsVoiceAccessAvailable())
                        {
                            Log.Write("Windows Speech Recognition not found. Voice Access detected. Launching Voice Access...", Colors.System);
                            LaunchVoiceAccess();

                            usingVoiceAccess = true; // Using Voice Access
                            State.trainerrunning = true;
                            Log.Write("SpeechTrainer is now running with Voice Access.", Colors.System);
                            return; // Exit early since Voice Access is being used
                        }

                        // If neither is available, log an error
                        Log.Write("Neither Windows Speech Recognition nor Voice Access is available on this system.", Colors.Critical);
                        State.trainerrunning = false;
                    }
                    catch (Exception ex)
                    {
                        Log.Write($"Error during initialization: {ex.Message}", Colors.Warning);
                    }
                }

                private static bool IsVoiceAccessAvailable()
                {
                    string voiceAccessPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "VoiceAccess.exe");
                    return File.Exists(voiceAccessPath);
                }

                private static void LaunchVoiceAccess()
                {
                    string voiceAccessPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "VoiceAccess.exe");
                    try
                    {
                        Process.Start(voiceAccessPath);
                        Log.Write("Voice Access launched successfully.", Colors.System);
                    }
                    catch (Exception ex)
                    {
                        Log.Write($"Failed to launch Voice Access: {ex.Message}", Colors.Warning);
                    }
                }
            }
        }
    }
}
