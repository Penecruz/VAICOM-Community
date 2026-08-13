using System;
using System.Collections.Generic;
using System.Threading;
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
                private static int EstimateShutdownSequenceDurationMs(List<Extensions.RIO.DeviceAction> sequence)
                {
                    if (sequence == null || sequence.Count == 0)
                    {
                        return 0;
                    }

                    int totalDelayMs = 0;
                    for (int i = 0; i < sequence.Count; i++)
                    {
                        Extensions.RIO.DeviceAction action = sequence[i];
                        if (action == null)
                        {
                            continue;
                        }

                        if (action.delayMs > 0)
                        {
                            totalDelayMs += action.delayMs;
                        }
                    }

                    return totalDelayMs;
                }

                private static void QueueShutdownCompletionCue(List<Extensions.RIO.DeviceAction> sequence)
                {
                    int estimatedMs = EstimateShutdownSequenceDurationMs(sequence);
                    if (estimatedMs <= 0)
                    {
                        return;
                    }

                    ThreadPool.QueueUserWorkItem(delegate
                    {
                        try
                        {
                            Thread.Sleep(estimatedMs + 250);
                            riospeech.riospeakrandom(4);
                        }
                        catch
                        {
                        }
                    });
                }

                private static void PlayShutdownStartVoiceAck()
                {
                    try
                    {
                        bool hadNoTalkState = tables.menustate[tables.menucats.CONTR_TALK].Equals(tables.menustates.No_Talk);
                        if (hadNoTalkState)
                        {
                            tables.menustate[tables.menucats.CONTR_TALK] = tables.menustates.Talk;
                        }

                        riospeech.riospeakrandom(1);

                        if (hadNoTalkState)
                        {
                            tables.menustate[tables.menucats.CONTR_TALK] = tables.menustates.No_Talk;
                        }
                    }
                    catch
                    {
                    }
                }

                private static void AddClonedAction(List<Extensions.RIO.DeviceAction> sequence, Extensions.RIO.DeviceAction source, int extraDelayMs)
                {
                    if (sequence == null || source == null)
                    {
                        return;
                    }

                    int delayMs = source.delayMs + (extraDelayMs > 0 ? extraDelayMs : 0);

                    sequence.Add(new Extensions.RIO.DeviceAction()
                    {
                        device = source.device,
                        command = source.command,
                        value = source.value,
                        delayMs = delayMs
                    });
                }

                private static void AddTomcatShutdownStep(List<Extensions.RIO.DeviceAction> sequence, List<Extensions.RIO.DeviceAction> step, int startDelayMs)
                {
                    if (sequence == null || step == null)
                    {
                        return;
                    }

                    List<Extensions.RIO.DeviceAction> block = new List<Extensions.RIO.DeviceAction>();
                    block.AddRange(DeviceActionsLibrary.Sequences.Macro.Seq_J_MENU_MAIN);
                    block.AddRange(step);

                    for (int i = 0; i < block.Count; i++)
                    {
                        AddClonedAction(sequence, block[i], i == 0 ? startDelayMs : 0);
                    }
                }

                private static void AddTomcatCrewTalkControl(List<Extensions.RIO.DeviceAction> sequence, bool enableTalk, int startDelayMs)
                {
                    if (sequence == null)
                    {
                        return;
                    }

                    List<Extensions.RIO.DeviceAction> block = new List<Extensions.RIO.DeviceAction>();
                    block.AddRange(DeviceActionsLibrary.Sequences.Macro.Seq_J_MENU_MAIN);
                    block.AddRange(enableTalk
                        ? DeviceActionsLibrary.Sequences.Macro.Seq_J_UTIL_CONTR_TALK
                        : DeviceActionsLibrary.Sequences.Macro.Seq_J_UTIL_CONTR_NO_TALK);

                    for (int i = 0; i < block.Count; i++)
                    {
                        AddClonedAction(sequence, block[i], i == 0 ? startDelayMs : 0);
                    }
                }

                private static void BuildTomcatShutdownSequence(List<Extensions.RIO.DeviceAction> sequence, bool isTomcatBU)
                {
                    const int BetweenStepDelayMs = 2000;

                    // Single acknowledgement for "No Talking" first, then perform shutdown silently.
                    AddTomcatCrewTalkControl(sequence, false, 0);

                    AddTomcatShutdownStep(
                        sequence,
                        isTomcatBU
                            ? DeviceActionsLibrary.Sequences.Macro.Seq_J_RAD_DL_OFF_BU
                            : DeviceActionsLibrary.Sequences.Macro.Seq_J_RAD_DL_OFF,
                        300);

                    AddTomcatShutdownStep(
                        sequence,
                        DeviceActionsLibrary.Sequences.Macro.Seq_J_RAD_TCN_MODE_OFF,
                        BetweenStepDelayMs);

                    AddTomcatShutdownStep(
                        sequence,
                        isTomcatBU
                            ? DeviceActionsLibrary.Sequences.Macro.Seq_J_RAD_182_MODE_OFF_BU
                            : DeviceActionsLibrary.Sequences.Macro.Seq_J_RAD_182_MODE_OFF,
                        BetweenStepDelayMs);

                    AddTomcatShutdownStep(
                        sequence,
                        isTomcatBU
                            ? DeviceActionsLibrary.Sequences.Macro.Seq_J_DEF_CMS_MOD_OFF_BU
                            : DeviceActionsLibrary.Sequences.Macro.Seq_J_DEF_CMS_MOD_OFF,
                        BetweenStepDelayMs);

                    AddTomcatShutdownStep(
                        sequence,
                        isTomcatBU
                            ? DeviceActionsLibrary.Sequences.Macro.Seq_J_DEF_JMR_SBY_BU
                            : DeviceActionsLibrary.Sequences.Macro.Seq_J_DEF_JMR_SBY,
                        BetweenStepDelayMs);

                    // Re-enable speech at the end ("You can talk again").
                    AddTomcatCrewTalkControl(sequence, true, 300);
                    AddClonedAction(sequence, DeviceActionsLibrary.RIO.Atom_J_MENU_CLOSE, 150);
                }

                public static void SetRioDeviceSequence_Shutdown()
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
                        setdefaultmessageparams();
                        State.currentmessage.type = Messagetypes.DeviceControl;
                        State.currentmessage.extsequence = new List<Extensions.RIO.DeviceAction>();

                        bool isTomcatBU = IsF14BUActive();

                        BuildTomcatShutdownSequence(State.currentmessage.extsequence, isTomcatBU);

                        if (State.activeconfig != null && State.activeconfig.Debugmode)
                        {
                            string stateIdForLog = (State.currentstate != null && !string.IsNullOrWhiteSpace(State.currentstate.id))
                                ? State.currentstate.id
                                : "<null>";

                            Log.Write("AIRIO shutdown seq | state.id=" + stateIdForLog + " | isF14BU=" + isTomcatBU.ToString() + " | actions=" + State.currentmessage.extsequence.Count.ToString(), Colors.Inline);
                        }

                        if (State.activeconfig.RIO_Messages && !State.activeconfig.RIO_Hints_Only)
                        {
                            State.currentmessage.dspmsg = "AIRIO : Shutdown sequence queued (DL, TACAN, ARC-182, CMS, Jammer).";
                            State.currentmessage.msgdur = 5;
                        }

                        if (!State.clientmode.Equals(ClientModes.Debug) && tables.menustate[tables.menucats.PLAYERSEAT].Equals(tables.menustates.RIO))
                        {
                            State.currentmessage.dspmsg = "AIRIO : You are in Jester's seat!\n";
                            State.currentmessage.msgdur = 5;
                            State.currentmessage.extsequence = new List<Extensions.RIO.DeviceAction>();
                        }

                        if (State.currentmessage.extsequence != null && State.currentmessage.extsequence.Count > 0)
                        {
                            PlayShutdownStartVoiceAck();
                            QueueShutdownCompletionCue(State.currentmessage.extsequence);
                        }

                        SendNewMessage();

                        if (PTT.IsPTTModeSingle())
                        {
                            Log.Write(State.currentTXnode.name + " | " + PTT.RadioDevices.SEL.name + ": [ RIO ],[  ],[  ] Shutdown Sequence [  ] [  ]", Colors.Message);
                        }
                        else
                        {
                            Log.Write(State.currentTXnode.name + " | " + State.currentTXnode.radios[0].name + ": [ RIO ],[  ],[  ] Shutdown Sequence [  ] [  ]", Colors.Message);
                        }
                    }
                    catch (Exception e)
                    {
                        Log.Write("Error setting RIO shutdown sequence: " + e.StackTrace + e.InnerException, Colors.Inline);
                    }
                }
            }
        }
    }
}
