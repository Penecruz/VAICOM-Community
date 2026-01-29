using System;
using System.Collections.Generic;
using VAICOM.Database;
using VAICOM.Extensions.Kneeboard;
using VAICOM.Extensions.WorldAudio;
using VAICOM.PushToTalk;
using VAICOM.Servers;
using VAICOM.Static;

namespace VAICOM
{
    namespace Client
    {
        public partial class DcsClient
        {
            public static partial class Message
            {

                public static bool havedelayedmessage = false;

                public static Recipientclass getrecipientclass()
                {
                    Recipientclass foundclass = Recipientclasses.Undefined;

                    Recipientclass classfromrecipient = Recipientclasses.Undefined;
                    if (State.have["recipient"])
                    {
                        classfromrecipient = Recipients.Table[State.currentkey["recipient"]].RecipientClass();
                    }

                    Recipientclass classfromcommand = Commands.Table[State.currentkey["command"]].RecipientClass();

                    if (Commands.Table[State.currentkey["command"]].isSpecial()) // void cmds eventnumber.Equals(4000))
                    {

                        if (State.have["recipient"])
                        {
                            foundclass = classfromrecipient;
                            State.calledisclass = Recipients.Table[State.currentkey["recipient"]].RecipientClass().Name.ToLower().Equals(State.currentkey["recipient"].ToLower());
                        }
                        else
                        {
                            State.calledisclass = true;
                            // keep as undefined
                        }
                    }
                    else // normal commands
                    {
                        // always base on command (overrule recipient)
                        foundclass = classfromcommand;
                        State.calledisclass = !State.have["recipient"] || Recipients.Table[State.currentkey["recipient"]].RecipientClass().Name.ToLower().Equals(State.currentkey["recipient"].ToLower());
                    }

                    return foundclass;
                }

                public static bool noTX()
                {
                    if (State.elapsedsincelastpttrelease > 2)
                    {
                        // void hotkey
                        if (!State.transmitting && !State.currentrecipientclass.Equals(Recipientclasses.Crew) && !(State.currentmodule.Equals(Products.DCSmodules.LookupTable[State.riomod]) && State.activeconfig.ICShotmic))
                        {
                            Log.Write("PTT: use an active TX node", Colors.Warning);
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }

                public static bool blocked()
                {
                    // block for training mode
                    if (State.trainerrunning)
                    {
                        Log.Write("Keyword Training Mode active: command processing disabled.", Colors.Warning);
                        return true;
                    }

                    // block for PRO
                    if (State.blockedmodule)
                    {
                        Log.Write("PRO license is required for this module.", Colors.Warning);
                        return true;
                    }
                    // temporary block
                    if (State.tempblockedcommands)
                    {
                        Log.Write("(plugin commands are currently not available)", Colors.Warning);
                        return true;
                    }
                    // script conflicts
                    if (State.blockallcommands)
                    {
                        Log.Write("(server-side script error: inputs blocked. re-install DCS-side lua files)", Colors.Critical);
                        return true;
                    }

                    return false;
                }

                public static void filterconflicts()
                {
                    try
                    {
                        if (State.usedalias["command"].ToLower().Contains(State.usedalias["recipient"].ToLower()))
                        {
                            State.currentkey["recipient"] = State.currentcommand.RecipientClass().Name.ToLower();
                            State.have["recipient"] = false;
                        }
                    }
                    catch
                    {
                    }
                }

                public static void correctforimportedobjects()
                {
                    // special case: imported atcs

                    try
                    {
                        if (State.have["importedatcs"])
                        {
                            Log.Write("Matched imported atc.", Colors.Text);

                            State.currentkey["recipient"] = State.currentkey["importedatcs"];
                            State.usedalias["recipient"] = State.usedalias["importedatcs"];
                            State.have["recipient"] = true;
                        }
                    }
                    catch
                    {
                    }

                    // special case: imported F10 menus

                    try
                    {
                        if (State.have["importedmenus"])
                        {
                            Log.Write("Matched imported F10 menu command.", Colors.Text);

                            State.currentkey["command"] = State.currentkey["importedmenus"];
                            State.usedalias["command"] = State.usedalias["importedmenus"];
                            State.currentkey["recipient"] = "aux";
                            State.currentrecipientclass = Recipientclasses.Aux;
                            State.have["recipient"] = true;
                            State.have["command"] = true;
                        }
                    }
                    catch
                    {
                    }

                }

                public static void scanforkeywords()
                {
                    try
                    {
                        foreach (KeyValuePair<string, Dictionary<string, string>> category in Aliases.inputscancats)
                        {
                            if (!State.have[category.Key])
                            {
                                // Skip sender identification if Kneeboard is recognized
                                if (category.Key == "sender" && State.have["recipient"] && State.currentkey["recipient"].Equals("kneeboard", StringComparison.OrdinalIgnoreCase))
                                {
                                    continue;
                                }

                                State.have[category.Key] = scanfor(category.Key);
                            }
                        }
                    }
                    catch
                    {
                    }
                }

                public static void waitformoreinput()
                {
                    if (!State.valistening && State.AIRIOactive && State.activeconfig.ICShotmic)
                    {
                        State.MessageReset();
                    }
                    else
                    {
                        // Handle Kneeboard recipient
                        try
                        {
                            if (Database.Recipients.Table[State.currentkey["recipient"]].name.Equals("wAIUnitKneeboard"))
                            {
                                // Handle Kneeboard recipient
                                try
                                {
                                    if (State.kneeboardactivated && State.activeconfig.Kneeboard_Enabled)
                                    {
                                        KneeboardToggle();
                                    }
                                    else
                                    {
                                        Log.Write("Interactive Kneeboard is disabled.", Colors.Warning);
                                        UI.Playsound.Error();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Log.Write($"Error handling Kneeboard command: {ex.Message}", Colors.Warning);
                                }
                            }
                            else
                            {
                                if (State.activeconfig.UIaddhints)
                                {
                                    UI.Playsound.Proceed();
                                }
                                Log.Write("(awaiting additional input)", Colors.Message);
                            }
                        }
                        catch
                        {
                        }
                    }
                }

                public static bool getunitforspecialcommands()
                {

                    // OPTIONS
                    if (State.currentcommand.isOptions() || State.currentcommand.isMenu())
                    {
                        if (State.have["recipient"])
                        {
                            Server.DcsUnit IntendedUnit = GetCalledUnit(State.currentrecipientclass, State.currentkey["recipient"]);
                            if (!IntendedUnit.id_.Equals(-1))
                            {
                                State.currentmessageunit = IntendedUnit;
                            }
                        }
                        else
                        {
                            // continue ok with no messageunit, class = undefined
                        }
                        return true;
                    }

                    // SELECT
                    if (State.currentcommand.isSelect())
                    {
                        Server.DcsUnit IntendedUnit = GetCalledUnit(State.currentrecipientclass, State.currentkey["recipient"]);
                        if (!IntendedUnit.id_.Equals(-1))
                        {
                            Log.Write("Selecting unit ", Colors.Text);
                            State.SelectedUnit[State.currentrecipientclass.Name] = IntendedUnit;
                            State.currentmessageunit = IntendedUnit;
                            return true;
                        }
                        else
                        {
                            Log.Write("Recipient " + Labels.airecipients[State.currentkey["recipient"]] + " is currently not available.", Colors.Warning);
                            UI.Playsound.Recipientna();
                            return false;
                        }
                    }

                    // STATE
                    if (State.currentcommand.isState())
                    {
                        if (State.have["recipient"])
                        {
                            Server.DcsUnit IntendedUnit = GetCalledUnit(State.currentrecipientclass, State.currentkey["recipient"]);
                            if (!IntendedUnit.id_.Equals(-1))
                            {
                                State.currentmessageunit = IntendedUnit;
                                State.calledisclass = (State.currentrecipientclass.Name.ToLower().Equals(State.currentkey["recipient"].ToLower()));
                            }
                            else // no units defaults to class --> will state no units available
                            {
                                State.currentkey["recipient"] = State.currentrecipientclass.Name.ToLower();
                                State.calledisclass = true;
                            }
                            State.genericstaterequest = false; // true if no recipient called
                        }
                        else // generic, targets ACOS
                        {
                            State.currentmessageunit = State.currentstate.availablerecipients["Aux"].Find(IsAOCS);
                            State.genericstaterequest = true;
                            State.currentrecipientclass = Recipientclasses.Aux; // --> make correction because was AOCS     
                        }
                        return true;
                    }

                    // REPEAT
                    if (State.currentcommand.dcsid.Equals("wMsgReplySayAgain"))
                    {
                        if (State.have["recipient"])
                        {
                            //State.currentrecipientclass = State.currentrecipientclass;
                        }
                        else
                        {
                            State.currentrecipientclass = State.previousrecipientclass;
                        }
                        return true;
                    }

                    // REPLIES
                    // (void, no action needed). no unit, recipientclass= undefined
                    return true;

                }

                public static bool checkifanyunitsincat()
                {
                    return !CatCanHaveUnits(Commands.Table[State.currentkey["command"]].RecipientClass()) || State.currentstate.availablerecipients[State.currentrecipientclass.Name].Count > 0;
                }

                public static bool getunitforregularcommands()
                {

                    // if no recipient alias used: correct to generic recipient based on command: e.g. awacs
                    if (!State.have["recipient"] || !State.currentrecipientclass.Equals(State.currentcommand.RecipientClass()))
                    {
                        State.currentkey["recipient"] = State.currentcommand.RecipientClass().Name.ToLower();
                    }

                    if (!AllowRecipient(Recipients.Table[State.currentkey["recipient"]]))
                    {
                        return false;
                    }

                    // get rid if any inconsistencies
                    filterconflicts();

                    // is its worth even continuing?
                    if (!checkifanyunitsincat())
                    {
                        Log.Write("There are currently no " + Commands.Table[State.currentkey["command"]].RecipientClass().Name + " recipients available.", Colors.Warning);
                        UI.Playsound.Recipientna();
                        return false;
                    }

                    // at least one unit exists in cat
                    // preferred: try set recipient based on freq (for eay comms off)
                    if (!SetRecipientByFreq())
                    {
                        // easy comms on, fc3 or not tuned:
                        if (!SetRecipientByCall()) // tries to use called unit alias and/or command
                        {
                            return false;
                        }
                        else
                        {
                            Log.Write("Recipient for " + State.currentcommand.RecipientClass().Name + " set by call contents.", Colors.Text);
                            return true;
                        }
                    }
                    else
                    {
                        Log.Write("Recipient for " + State.currentcommand.RecipientClass().Name + " set by frequency.", Colors.Text);
                        return true;
                    }

                }

                public static void sendmessage()
                {
                    Log.Write("Ready, sending message for recipient class " + State.currentrecipientclass.Name + ", calledisclass = " + State.calledisclass, Colors.Inline);

                    ConstructMessage();
                    SendNewMessage();

                    State.previousmessageunit = State.currentmessageunit;
                    State.previousrecipientclass = State.currentrecipientclass;

                    Log.Write("Message sent successfully for recipient class " + State.currentrecipientclass.Name + ".", Colors.Inline);

                    // for ics hotmic:
                    if (State.AIRIOactive && State.activeconfig.ICShotmic)
                    {
                        if (!State.valistening)
                        {
                            State.MessageReset();
                            State.processlocked = false;
                        }

                        if (!State.currentcommand.isMenu() && !State.currentcommand.isOptions())
                        {
                            Extensions.RIO.helper.ShowWheel(false);
                            Extensions.RIO.helper.showingjestermenu = false;
                        }
                    }
                }

                public static void sendvoid()
                {
                    Log.Write("Void command.", Colors.Inline);

                    try
                    {

                        // for kneeboard commands
                        if (State.currentcommand.isKneeboard() || State.currentcommand.uniqueid.Equals(23004) || State.currentcommand.uniqueid.Equals(23005))
                        {
                            if (State.kneeboardactivated)
                            {

                                switch (State.currentcommand.dcsid)
                                {
                                    case "wMsgShowKneeboardTab":
                                        try
                                        {
                                            string cat = Database.Recipients.Table[State.currentkey["recipient"]].RecipientClass().Name;
                                            KneeboardUpdater.SwitchPage(cat);
                                            UI.Playsound.Commandcomplete();
                                        }
                                        catch
                                        {
                                        }
                                        break;
                                    case "wMsgClearKneeboardTab": // not used
                                        try
                                        {
                                            string cat = Database.Recipients.Table[State.currentkey["recipient"]].RecipientClass().Name;
                                            KneeboardUpdater.SwitchPage(cat);
                                            UI.Playsound.Commandcomplete();
                                        }
                                        catch
                                        {
                                        }
                                        break;
                                    case "wMsgKneeboardDictateStart":
                                        State.Proxy.Dictation.Start(out string buffer);
                                        KneeboardUpdater.SwitchPage("NOTES");
                                        PTT.PTT_Manage_Listen_States_OnPressRelease(true, false);
                                        //UI.Playsound.Commandcomplete();
                                        //vaProxy.WriteToLog("Start: buffer = " + buffer, Colors.Critical);
                                        break;
                                    case "wMsgKneeboardDictateStop":
                                        PTT.PTT_Manage_Listen_States_OnPressRelease(false, false);
                                        State.Proxy.Dictation.Stop();
                                        //UI.Playsound.Commandcomplete();
                                        break;
                                    case "wMsgKneeboardCorrection":
                                        State.Proxy.Dictation.ClearBuffer(true, out String Message1);
                                        UI.Playsound.Commandcomplete();
                                        KneeboardUpdater.SwitchPage("NOTES");
                                        break;
                                    case "wMsgKneeboardClearNotes":
                                        State.Proxy.Dictation.ClearBuffer(false, out String Message2);
                                        State.kneeboardcurrentbuffer = "";
                                        if (State.Proxy.Dictation.IsOn())
                                        {
                                            State.kneeboardlastdictbuffer = State.Proxy.Utility.ParseTokens("{DICTATION:NEWLINE}");
                                        }
                                        else
                                        {
                                            State.kneeboardlastdictbuffer = "";
                                        }
                                        UI.Playsound.Commandcomplete();
                                        KneeboardUpdater.SwitchPage("NOTES");
                                        KneeboardUpdater.RefreshCurrentPage(); // Force immediate refresh
                                        KneeboardUpdater.SendHeartBeatCycle();
                                        break;
                                    case "wMsgKneeboardShowNotes":
                                        KneeboardUpdater.SwitchPage("NOTES");
                                        UI.Playsound.Proceed();
                                        break;
                                    case "wMsgKneeboardShowLog":
                                        KneeboardUpdater.SwitchPage("LOG");
                                        UI.Playsound.Proceed();
                                        break;
                                    default:
                                        break;
                                }
                            }
                            else
                            {
                                Log.Write("Interactive Kneeboard is disabled.Please enable in the Vaicom Pro UI extension tab", Colors.Warning);
                                UI.Playsound.Error();
                            }
                        }
                    }
                    catch
                    {
                    }


                    if (State.currentcommand.isSwitch())
                    {
                        if (State.activeconfig.MP_VoIPUseSwitch && State.activeconfig.MP_AllowSwitchCommand)
                        {
                            SwapSRSListeningStates();
                        }
                        else
                        {
                            Log.Write("Switch voice command is disabled in Preferences.", Colors.Warning);
                        }
                    }

                    if (State.currentcommand.dcsid.Equals("wMsgReplySayAgain"))
                    {

                        Processor.commcat sendercat = Processor.commcat.ALL;

                        try
                        {
                            sendercat = Processor.CategoryMap[State.currentrecipientclass];
                        }
                        catch
                        {
                        }

                        Log.Write(State.lastmessage[sendercat].text, Colors.Message);

                        try
                        {
                            State.receivedtx = Processor.GetReceivingTXFromUnitID(sendercat, State.lastmessage[sendercat].pMsgSender.id_);
                        }
                        catch
                        {
                        }

                        //Processor.SpeakMessageFromFiles(State.lastmessage[sendercat], 6); // 6x250ms delay

                    }

                }

                public static void SwapSRSListeningStates()
                {
                    if (State.activeconfig.MP_VoIPUseSwitch)
                    {
                        PTT.PTT_Manage_Listen_States_OnSwitch();
                    }
                    else
                    {
                        Log.Write("VoIP switching is disabled in Preferences.", Colors.Warning);
                    }
                }

                public static bool dospeech()
                {
                    return State.currentcommand.isState() && State.activeconfig.DeepInterrogate && Server.tunedforAOCS;
                }

                // processes voice command, called directly from plugin
                public static bool processcommand()
                {
                    // thread-safe locking
                    if (State.processlocked)
                    {
                        return false;
                    }
                    else
                    {
                        State.processlocked = true;
                    }

                    // ------------------
                    if (blocked())
                    {
                        State.processlocked = false;
                        return false;
                    }
                    // no block, continue..:

                    havedelayedmessage = false;

                    bool options = false;
                    bool menu = false;

                    try
                    {
                        // get voice input
                        getinputsentence();
                        scanforkeywords();
                        correctforimportedobjects();

                        // check if have enough data to send command?
                        State.haveinputscomplete = setcommand();

                        // if not: wait..
                        if (!State.haveinputscomplete)
                        {
                            waitformoreinput();
                            State.processlocked = false;
                            return false;
                        }

                        //----------------------------------------------------------------------------------------------------
                        // have command complete!: now process - check contents

                        State.currentrecipientclass = getrecipientclass();

                        if (noTX())
                        {
                            State.processlocked = false;
                            return true; //?? or false
                        }

                        bool selectcommand = State.currentcommand.isSelect();
                        bool switchcommand = State.currentcommand.isSwitch();

                        //----------------------------------------------------------------------------------------------------
                        // Need to get: State.currentmessageunit
                        // Already have State.currentrecipientclass for recipient category (undefined if no recipient)

                        if (State.currentcommand.isSpecial()) //eventnumber.Equals(4000)
                        {
                            // Special commands:  Options / Take / Select / State / Repeat 
                            if (!getunitforspecialcommands() && State.dcsrunning)
                            {
                                State.MessageReset();
                                State.processlocked = false;
                                return !State.currentcommand.isOptions() && !State.currentcommand.isMenu();
                            }
                        }
                        else
                        {
                            // Normal commands
                            if (!getunitforregularcommands() && State.dcsrunning)
                            {
                                //didn't get a unit
                                State.MessageReset();
                                State.processlocked = false;
                                return true;
                            }

                            if (!checkcorrectradiouse() && State.dcsrunning)
                            {
                                // wrong radio
                                State.MessageReset();
                                State.processlocked = false;
                                return true;
                            }

                            if (State.currentcommand.isCarrier() && !State.currentmessageunit.descr.ToLower().Contains("supercarrier"))
                                if (State.currentcommand.isCarrier() && !State.currentmessageunit.fullname.ToLower().Contains("cvn-"))
                                {
                                    Log.Write("Selected recipient is not a Supercarrier unit.", Colors.Warning); // Pene changes to allow non SC units if module is installed
                                    UI.Playsound.Error();
                                    return true; //true
                                }
                        }

                        // ---------------------------  
                        // SEND MESSAGE

                        logclosecall();

                        // switch kneeboard page if autobrowse on
                        try
                        {
                            if (State.activeconfig.KneeboardlinkPTT)
                            {
                                string cat = Database.Recipients.Table[State.currentkey["recipient"]].RecipientClass().Name;
                                KneeboardUpdater.SwitchPage(cat);
                            }
                        }
                        catch
                        {
                        }

                        bool riocommand = State.AIRIOactive && State.currentcommand.isRIO();
                        bool optionscommand = State.currentcommand.isOptions();
                        bool menucommand = State.currentcommand.isMenu();

                        options = optionscommand;
                        menu = menucommand;

                        if (State.currentcommand.isVoid()) // i.e. replies, switch and repeat
                        {
                            sendvoid();
                        }
                        else
                        {
                            if (riocommand || selectcommand || optionscommand || menucommand ||
                                !((State.activeconfig.MP_VoIPUseSwitch || State.activeconfig.MP_VoIPParallel) && State.activeconfig.MP_DelayTransmit)) //  || !State.currentTXnode.tunedforhuman 
                            {
                                sendmessage();
                            }
                            else
                            {
                                havedelayedmessage = true;
                            }
                        }

                        if (dospeech())
                        {
                            Extensions.AOCS.AOCSProvider.AOCS_CallDeepInterrogate();
                        }
                        else
                        {
                            if (!havedelayedmessage)
                            {
                                State.MessageReset();
                            }
                        }


                        // setting: auto switch to VoIP when call finished
                        if (!riocommand && !optionscommand && !menucommand && State.activeconfig.MP_VoIPUseSwitch && State.activeconfig.MP_VoIPAutoSwitch && !switchcommand && !(selectcommand && State.activeconfig.MP_IgnoreSelect))
                        {
                            PTT.PTT_Manage_Listen_States_OnSwitch();

                            if (State.activeconfig.MP_WarnHumans)
                            {
                                UI.Playsound.Human_Comms_Active();
                            }

                        }


                        // Handle WSO commands
                        if (State.currentcommand.isWSO())
                        {
                            if (!State.currentmodule.Id.Equals("F-4E-45MC", StringComparison.OrdinalIgnoreCase))
                            {
                                Log.Write("WSO commands are only available for the F-4E-45MC module.", Colors.Warning);
                                State.processlocked = false;
                                return false; // WSO command not processed
                            }

                            if (!Message.ProcessIfWSO())
                            {
                                State.processlocked = false;
                                return false; // WSO command processing failed
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Log.Write("Voice command processing error: " + e.Message, Colors.Critical);
                        State.processlocked = false;
                        return false;
                    }

                    State.processlocked = false;
                    return true;
                }
            }
        }
    }
}