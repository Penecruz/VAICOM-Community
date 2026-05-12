using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using VAICOM.Database;
using VAICOM.Extensions.AIWSO;
using VAICOM.Extensions.Kneeboard;
using VAICOM.Extensions.WorldAudio;
using VAICOM.Interfaces;
using VAICOM.PushToTalk;
using VAICOM.Servers;
using VAICOM.Static;
using VAICOM.WSO;

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
                        if (!State.transmitting && !State.currentrecipientclass.Equals(Recipientclasses.Crew) && !State.IsCrewHotMicActive())
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
                    if (!State.valistening && State.IsCrewHotMicActive()
                        && !(State.currentcommand.RequiresWSOCommandRecipient() && State.currentWSOCommandRecipient == null))
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
                                    if (State.activeconfig.Kneeboard_Enabled || State.activeconfig.OpenKneeboard_Out)
                                    {
                                        KneeboardToggle();
                                    }
                                    else
                                    {
                                        Log.Write("Interactive Kneeboard and OpenKneeboard Out are disabled.", Colors.Warning);
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
                            if (State.activeconfig.Kneeboard_Enabled || State.activeconfig.OpenKneeboard_Out)
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
                                    case "wMsgKneeboardNextTab":
                                        CycleKneeboardTab(1);
                                        UI.Playsound.Proceed();
                                        break;
                                    case "wMsgKneeboardPreviousTab":
                                        CycleKneeboardTab(-1);
                                        UI.Playsound.Proceed();
                                        break;
                                    default:
                                        break;
                                }
                            }
                            else
                            {
                                Log.Write("Interactive Kneeboard and OpenKneeboard Out are disabled. Please enable one in the Vaicom Pro UI extension tab", Colors.Warning);
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

                private static void CycleKneeboardTab(int offset)
                {
                    List<string> tabcats = API.tabcats;
                    int catnum = tabcats.IndexOf(State.KneeboardState.activecat.ToUpper()) + offset;
                    if (catnum < 1)
                    {
                        catnum = tabcats.Count - 1;
                    }
                    if (catnum > tabcats.Count - 1)
                    {
                        catnum = 1;
                    }

                    string cat = tabcats[catnum];
                    switch (cat)
                    {
                        case "TANKER":
                            cat = "Tanker";
                            break;
                        case "FLIGHT":
                            cat = "Flight";
                            break;
                    }

                    KneeboardUpdater.SwitchPage(cat);
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
                    // Check if DCS is running
                    if (!State.dcsrunning)
                    {
                        Log.Write("DCS is not connected. Command processing is disabled.", Colors.Warning);
                        UI.Playsound.Error();
                        return false;
                    }

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
                                string cat = "";

                                if (State.currentcommand != null && State.currentcommand.dcsid.StartsWith("wMsgGeorge", StringComparison.OrdinalIgnoreCase))
                                {
                                    cat = "Crew";
                                }
                                else if (State.have["recipient"])
                                {
                                    cat = Database.Recipients.Table[State.currentkey["recipient"]].RecipientClass().Name;
                                }
                                else if (State.currentrecipientclass != null)
                                {
                                    cat = State.currentrecipientclass.Name;
                                }

                                if (!string.IsNullOrWhiteSpace(cat))
                                {
                                    KneeboardUpdater.SwitchPage(cat);
                                }
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
                        else if (State.currentcommand.isWSO()) // Handle WSO commands
                        {
                            Log.Write("WSO command detected. Starting WSO command processing...", Colors.Text);

                            if (!State.currentmodule.Id.Equals("F-4E-45MC", StringComparison.OrdinalIgnoreCase))
                            {
                                Log.Write("WSO commands are only available for the F-4E-45MC module.", Colors.Warning);
                                State.processlocked = false;
                                return false; // WSO command not processed
                            }

                            if (!Message.ProcessIfWSO())
                            {
                                Log.Write("WSO command processing failed.", Colors.Warning);
                                State.processlocked = false;
                                return false; // WSO command processing failed
                            }

                            Log.Write("WSO command processing succeeded. Sending command to Jester 2.0 API...", Colors.Text);

                            // Send the WSO command to the Jester 2.0 API
                            SendCommandToJesterAPI(State.currentcommand.dcsid);
                        }
                        else
                        {
                            bool immediateHotMicSend = !State.transmitting && State.IsCrewHotMicActive();

                            if (riocommand || selectcommand || optionscommand || menucommand || immediateHotMicSend ||
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
                    }
                    catch (Exception e)
                    {
                        Log.Write("Voice command processing error: " + e.Message + ", " + e.StackTrace, Colors.Critical);
                        State.processlocked = false;
                        return false;
                    }

                    State.processlocked = false;
                    return true;
                }

                public static bool TryResolveDynamicWsoValue(string commandId, string action, string mappedValue, out string resolvedValue)
                {
                    resolvedValue = mappedValue ?? "";

                    if (!string.IsNullOrWhiteSpace(resolvedValue))
                    {
                        return true;
                    }

                    if (TryResolveValueFromActionCache(action, State.currentfullsentence, out string cachedValue, out string cacheKey))
                    {
                        resolvedValue = cachedValue;
                        Log.Write($"WSO command '{commandId}' resolved from action cache [{cacheKey}] => {cachedValue}", Colors.Text);
                        return true;
                    }

                    return true;
                }

                private static bool TryResolveValueFromActionCache(string action, string sentence, out string resolvedValue, out string cacheKey)
                {
                    resolvedValue = "";
                    cacheKey = "";

                    if (string.IsNullOrWhiteSpace(action))
                    {
                        return false;
                    }

                    if (TryExtractIndexFromCommandAlias(out string aliasIndex))
                    {
                        if (action.Equals("divert_tgt1_lat_lon", StringComparison.OrdinalIgnoreCase)
                            && AliasRequestsSecondaryFlightPlan()
                            && WSOActionCache.TryGetByActionAndIndex(action, $"fp2|{aliasIndex}", out string secondaryAliasIndexedValue)
                            && !string.IsNullOrWhiteSpace(secondaryAliasIndexedValue))
                        {
                            resolvedValue = secondaryAliasIndexedValue;
                            cacheKey = $"fp2-alias-idx={aliasIndex}";
                            return true;
                        }

                        if (WSOActionCache.TryGetByActionAndIndex(action, aliasIndex, out string aliasIndexedValue)
                            && !string.IsNullOrWhiteSpace(aliasIndexedValue))
                        {
                            resolvedValue = aliasIndexedValue;
                            cacheKey = $"alias-idx={aliasIndex}";
                            return true;
                        }
                    }

                    if (TryExtractIndexFromSentence(sentence, out string index))
                    {
                        if (action.Equals("divert_tgt1_lat_lon", StringComparison.OrdinalIgnoreCase)
                            && AliasRequestsSecondaryFlightPlan()
                            && WSOActionCache.TryGetByActionAndIndex(action, $"fp2|{index}", out string secondaryIndexedValue)
                            && !string.IsNullOrWhiteSpace(secondaryIndexedValue))
                        {
                            resolvedValue = secondaryIndexedValue;
                            cacheKey = $"fp2-idx={index}";
                            return true;
                        }

                        if (WSOActionCache.TryGetByActionAndIndex(action, index, out string indexedValue)
                            && !string.IsNullOrWhiteSpace(indexedValue))
                        {
                            resolvedValue = indexedValue;
                            cacheKey = $"idx={index}";
                            return true;
                        }
                    }

                    string normalizedSentence = NormalizeActionLookupText(sentence);
                    if (!string.IsNullOrWhiteSpace(normalizedSentence))
                    {
                        string bestValue = "";
                        string bestName = "";
                        int bestMatchLength = -1;

                        lock (WSOActionCache.GetActionCacheLock())
                        {
                            foreach (KeyValuePair<string, string> entry in WSOActionCache.GetActionByNameCacheEntries())
                            {
                                if (!entry.Key.StartsWith(action + "|", StringComparison.OrdinalIgnoreCase))
                                {
                                    continue;
                                }

                                string candidateName = entry.Key.Substring(action.Length + 1);
                                string normalizedCandidate = NormalizeActionLookupText(candidateName);
                                if (string.IsNullOrWhiteSpace(normalizedCandidate))
                                {
                                    continue;
                                }

                                if (!normalizedSentence.Contains(normalizedCandidate) || normalizedCandidate.Length <= bestMatchLength)
                                {
                                    continue;
                                }

                                bestMatchLength = normalizedCandidate.Length;
                                bestName = candidateName;
                                bestValue = entry.Value;
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(bestValue))
                        {
                            resolvedValue = bestValue;
                            cacheKey = $"name={bestName}";
                            return true;
                        }
                    }


                    lock (WSOActionCache.GetActionCacheLock())
                    {
                        HashSet<string> actionValues = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                        foreach (KeyValuePair<string, string> entry in WSOActionCache.GetActionByNameCacheEntries())
                        {
                            if (entry.Key.StartsWith(action + "|", StringComparison.OrdinalIgnoreCase)
                                && !string.IsNullOrWhiteSpace(entry.Value))
                            {
                                actionValues.Add(entry.Value);
                            }
                        }

                        foreach (KeyValuePair<string, string> entry in WSOActionCache.GetActionByIndexCacheEntries())
                        {
                            if (entry.Key.StartsWith(action + "|", StringComparison.OrdinalIgnoreCase)
                                && !string.IsNullOrWhiteSpace(entry.Value))
                            {
                                actionValues.Add(entry.Value);
                            }
                        }

                        if (actionValues.Count == 1)
                        {
                            resolvedValue = actionValues.First();
                            cacheKey = "single-action-value";
                            return true;
                        }
                    }


                    return false;
                }

                private static bool AliasRequestsSecondaryFlightPlan()
                {
                    if (!State.usedalias.ContainsKey("command"))
                    {
                        return false;
                    }

                    string commandAlias = State.usedalias["command"] ?? "";
                    return commandAlias.IndexOf("flight plan 2", StringComparison.OrdinalIgnoreCase) >= 0
                        || commandAlias.IndexOf("flight plan two", StringComparison.OrdinalIgnoreCase) >= 0
                        || commandAlias.IndexOf("secondary flight plan", StringComparison.OrdinalIgnoreCase) >= 0;
                }

                private static bool TryExtractIndexFromCommandAlias(out string index)
                {
                    index = "";

                    if (!State.usedalias.ContainsKey("command"))
                    {
                        return false;
                    }

                    string commandAlias = State.usedalias["command"];
                    if (string.IsNullOrWhiteSpace(commandAlias))
                    {
                        return false;
                    }

                    return TryExtractIndexFromSentence(commandAlias, out index);
                }

                private static bool TryExtractIndexFromSentence(string sentence, out string index)
                {
                    index = "";

                    if (string.IsNullOrWhiteSpace(sentence))
                    {
                        return false;
                    }

                    MatchCollection matches = Regex.Matches(sentence, @"\b\d{1,3}\b");
                    foreach (Match match in matches)
                    {
                        if (!int.TryParse(match.Value, out int parsedIndex))
                        {
                            continue;
                        }

                        if (parsedIndex < 0)
                        {
                            continue;
                        }

                        index = parsedIndex.ToString();
                        return true;
                    }

                    foreach (Match tokenMatch in Regex.Matches(sentence.ToLowerInvariant(), @"[a-z0-9]+"))
                    {
                        if (!TryParseSpokenWaypointIndex(tokenMatch.Value, out int spokenIndex))
                        {
                            continue;
                        }

                        index = spokenIndex.ToString();
                        return true;
                    }

                    return false;
                }

                private static bool TryParseSpokenWaypointIndex(string token, out int index)
                {
                    index = 0;

                    switch (token)
                    {
                        case "one":
                        case "first":
                            index = 1;
                            return true;
                        case "two":
                        case "second":
                            index = 2;
                            return true;
                        case "three":
                        case "third":
                            index = 3;
                            return true;
                        case "four":
                        case "fourth":
                            index = 4;
                            return true;
                        case "five":
                        case "fifth":
                            index = 5;
                            return true;
                        case "six":
                        case "sixth":
                            index = 6;
                            return true;
                        case "seven":
                        case "seventh":
                            index = 7;
                            return true;
                        case "eight":
                        case "eighth":
                            index = 8;
                            return true;
                        case "nine":
                        case "ninth":
                            index = 9;
                            return true;
                        case "ten":
                        case "tenth":
                            index = 10;
                            return true;
                        default:
                            return false;
                    }
                }

                private static bool TryResolveValueForATCAsset(string action, out string value)
                {
                    if (!State.usedalias.ContainsKey("command"))
                    {
                        value = "";
                        return false;
                    }

                    Recipient commandRecipient = State.currentWSOCommandRecipient;
                    if (commandRecipient == null)
                    {
                        Log.Write($"Required WSO command recipient is missing", Colors.Warning);
                        value = "";
                        return false;
                    }

                    string name = commandRecipient.displayname;

                    if (WSOActionCache.TryGetByActionAndName(action, name, out string resolvedValue)
                        && !string.IsNullOrWhiteSpace(resolvedValue))
                    {
                        value = resolvedValue;
                        return true;
                    }
                    else
                    {
                        Log.Write($"Jester menu item not found for '{name}'", Colors.Warning);
                        value = "";
                        return false;
                    }
                }

                private static string NormalizeActionLookupText(string input)
                {
                    if (string.IsNullOrWhiteSpace(input))
                    {
                        return "";
                    }

                    string lowered = input.ToLowerInvariant();
                    lowered = lowered.Replace("int'l", "international");
                    lowered = Regex.Replace(lowered, @"\bintl\b", "international");
                    string normalized = Regex.Replace(lowered, @"[^a-z0-9]+", " ");
                    return Regex.Replace(normalized, @"\s+", " ").Trim();
                }

                //Jester 2.0 API WSO command sender
                private static void SendCommandToJesterAPI(string commandId)
                {
                    try
                    {
                        // Check if the command exists in the WSOCommandMappings
                        if (WSOCommandMappings.CommandMap.TryGetValue(commandId, out var commandDetails))
                        {
                            string category = commandDetails.category;
                            string action = commandDetails.action;
                            string value = commandDetails.value;

                            if ((commandId.Equals("wMsgWSO_Radio_TuneATC")
                                || commandId.Equals("wMsgWSO_Navigation_Divert_Airfield")
                                || commandId.Equals("wMsgWSO_Navigation_Divert_Asset")
                                || commandId.Equals("wMsgWSO_Navigation_TACAN_TuneAsset"))
                                && !TryResolveValueForATCAsset(action, out value))
                            {
                                return;
                            }
                            else if (!TryResolveDynamicWsoValue(commandId, action, value, out value))
                            {
                                return;
                            }

                            if (commandDetails.valueRequired && string.IsNullOrEmpty(value))
                            {
                                Log.Write($"Command '{commandId}' missing required value.", Colors.Warning);
                                return;
                            }

                            // Construct the command string in the format "category|action|value"
                            string commandString = $"{category}|{action}|{value}";

                            // Create a new CommsMessage for the Jester API command
                            State.currentmessage = new Message.CommsMessage
                            {                                
                                type = "WSOCommand", // Indicate this is a WSO command
                                dcsid = commandString, // Pass the constructed command string
                                dspmsg = $"Sending WSO command: {commandString}",
                                msgdur = 5
                            };

                            // Send the command to the Jester 2.0 API using hb_send_proxy fields
                            HbSendProxyCommand.SendCommand(State.WebSocketClient, category, action, value);
                            Log.Write($"Command ID '{commandId}' sent using hb_send_proxy payload.", Colors.Text);
                        }
                        else
                        {
                            Log.Write($"Command ID '{commandId}' not found in WSOCommandMappings.", Colors.Warning);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Write($"Error sending command to Jester 2.0 API: {ex.Message}", Colors.Critical);
                    }
                }
            }
        }
    }
}