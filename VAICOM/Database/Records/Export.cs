using System;
using System.Collections.Generic;
using VAICOM.Extensions.WSO;
using VAICOM.Static;

namespace VAICOM
{

    namespace Database
    {

        public static partial class Aliases
        {

            private static void AppendAliasWithGunnerVariant(ref string target, string alias, ref int counter)
            {
                if (string.IsNullOrWhiteSpace(alias))
                {
                    return;
                }

                target += alias + "; ";
                counter = counter + 1;

                if (alias.StartsWith("George", StringComparison.OrdinalIgnoreCase)
                    && (alias.Length == 6 || char.IsWhiteSpace(alias[6]) || alias[6] == ','))
                {
                    target += "Gunner" + alias.Substring(6) + "; ";
                    counter = counter + 1;
                }
            }

            //VSPX
            public static string CreateMasterKeywordStringVSPX()
            {
                int counter = 0;
                string outputstring = "";
                string wsoOutputCommandString = "";
                string wsoOutputRecipientString = "";

                //----------------------------------------------------------
                // 1) get recipients
                Dictionary<string, string> recipientstrings = new Dictionary<string, string>();
                Dictionary<string, string> wsoRecipientStrings = new Dictionary<string, string>();
                foreach (string cat in RecipientCategories.GetNames(typeof(RecipientCategories)))
                {
                    if (cat.StartsWith("WSO"))
                    {
                        wsoRecipientStrings.Add(cat, "");
                    }
                    else
                    {
                        recipientstrings.Add(cat, "");
                    }
                }
                foreach (KeyValuePair<string, Recipient> dbentry in Recipients.Table) // go through recipients list
                {
                    if (!dbentry.Value.name.Contains("Null") && !dbentry.Value.name.Contains("Maximum")) // skip any voids
                    {
                        foreach (string cat in RecipientCategories.GetNames(typeof(RecipientCategories)))
                        {
                            if (dbentry.Value.category.ToString().Equals(cat))
                            {
                                foreach (KeyValuePair<string, string> alias in Aliases.airecipients)
                                {
                                    if (false) // disabled (Aliases.playercallsigns.ContainsValue(alias.Value)) // for specific JTAC names: add to player category
                                    {
                                        //if (alias.Value.Equals(dbentry.Key))
                                        //{
                                        //    //Log.Write("Alias " + alias.Key + " found for " + alias.Value, Colors.Warning);
                                        //    recipientstrings["player"] += alias.Key + "; ";
                                        //    counter = counter + 1;
                                        //}
                                    }
                                    else // normal
                                    {
                                        if ((alias.Value).Equals(dbentry.Key))
                                        {

                                            // LOG ADDED
                                            //Log.Write("Alias " + alias.Key + " found for " + alias.Value, Colors.Warning);
                                            if (cat.StartsWith("WSO"))
                                            {
                                                wsoRecipientStrings[cat] += alias.Key + "; ";
                                            }
                                            else
                                            {
                                                recipientstrings[cat] += alias.Key + "; ";
                                            }
                                            counter = counter + 1;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                recipientstrings["aiatc"] = recipientstrings["aiatc"] + recipientstrings["aifarp"];
                recipientstrings["aiatc"] = recipientstrings["aiatc"] + recipientstrings["aiship"];

                //----------------------------------------------------------

                //----------------------------------------------------------
                // 2) get sender
                string outputsenderstring = "";
                foreach (KeyValuePair<string, string> alias in Aliases.playercallsigns)
                {
                    outputsenderstring += alias.Key + "; ";
                    //counter = counter + 1;
                }
                //outputsenderstring = "[" + outputsenderstring;
                //outputsenderstring = outputsenderstring.TrimEnd("; ".ToCharArray());
                //outputsenderstring += ";]"; // block is optional
                //recipientstrings["aiflight"] = recipientstrings["aiflight"] + outputsenderstring;

                //----------------------------------------------------------

                //----------------------------------------------------------
                // 3) get cues
                string outputcuestring = "";
                foreach (KeyValuePair<string, string> alias in Aliases.aicues)
                {
                    outputcuestring += alias.Key + "; ";
                    counter = counter + 1;
                }
                outputcuestring = "[" + outputcuestring;
                outputcuestring = outputcuestring.TrimEnd("; ".ToCharArray());
                outputcuestring += ";]"; // block is optional
                //----------------------------------------------------------

                //----------------------------------------------------------
                // 4) get commands

                Dictionary<string, string> commandstrings = new Dictionary<string, string>();
                Dictionary<string, string> wsoCommandStrings = new Dictionary<string, string>();
                foreach (string cat in CommandCategories.GetNames(typeof(CommandCategories)))
                {
                    // WSO commands are handled separately
                    if (cat.StartsWith("WSO"))
                    {
                        wsoCommandStrings.Add(cat, "");
                    }
                    else
                    {
                        commandstrings.Add(cat, "");
                    }
                }
                foreach (KeyValuePair<string, Command> dbentry in Commands.Table) // go through recipients list
                {
                    if (!dbentry.Value.dcsid.Contains("Null") && !dbentry.Value.dcsid.Contains("Maximum")) // skip any voids
                    {
                        foreach (string cat in CommandCategories.GetNames(typeof(CommandCategories)))
                        {
                            if (dbentry.Value.category.ToString().Equals(cat))
                            {
                                // have match entry, now get all its aliases
                                foreach (KeyValuePair<string, string> alias in Aliases.aicommands)
                                {
                                    if ((alias.Value).Equals(dbentry.Key))
                                    {
                                        // WSO aliases are handled seperately
                                        if (cat.StartsWith("WSO"))
                                        {
                                            string commandstring = wsoCommandStrings[cat];
                                            AppendAliasWithGunnerVariant(ref commandstring, alias.Key, ref counter);
                                            wsoCommandStrings[cat] = commandstring;
                                        }
                                        else
                                        {
                                            string commandstring = commandstrings[cat];
                                            AppendAliasWithGunnerVariant(ref commandstring, alias.Key, ref counter);
                                            commandstrings[cat] = commandstring;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                //----------------------------------------------------------

                //----------------------------------------------------------
                // 5) merge for each recipient category
                foreach (string recipientcat in RecipientCategories.GetNames(typeof(RecipientCategories)))
                {
                    if (!recipientcat.Equals("aifarp") && !recipientcat.Equals("aiship"))
                    {
                        string cat = recipientcat;
                        string outputcommandstring = "";
                        //add the appropriate command blocks
                        foreach (string commandcat in CommandCategories.GetNames(typeof(CommandCategories)))
                        {
                            // FLIGHT
                            if (cat.Equals("aiflight") && (commandcat.Contains("aicommsflight") || commandcat.Contains("special")))
                            {
                                outputcommandstring = outputcommandstring + commandstrings[commandcat];
                            }

                            //JTAC
                            if (cat.Equals("aijtac") && (commandcat.Contains("aicommsjtac") || commandcat.Contains("special")))
                            {
                                outputcommandstring = outputcommandstring + commandstrings[commandcat];
                            }

                            //ATC
                            if (cat.Equals("aiatc") && (commandcat.Equals("aicommsatc") || commandcat.Contains("aicommscarrier") || commandcat.Contains("special")))
                            {
                                outputcommandstring = outputcommandstring + commandstrings[commandcat];
                            }
                            //Carrier

                            //if (cat.Equals("aiship") && ( || commandcat.Equals("special")))
                            //{
                            //    outputcommandstring = outputcommandstring + commandstrings[commandcat];
                            //}

                            //AWACS
                            if (cat.Equals("aiawacs") && (commandcat.Contains("aicommsawacs") || commandcat.Contains("special")))
                            {
                                outputcommandstring = outputcommandstring + commandstrings[commandcat];
                            }

                            //Tanker
                            if (cat.Equals("aitanker") && (commandcat.Contains("aicommstanker") || commandcat.Contains("special")))
                            {
                                outputcommandstring = outputcommandstring + commandstrings[commandcat];
                            }

                            //Crew
                            if (cat.Equals("aicrew") && (commandcat.Contains("aicommscrew") || commandcat.Contains("special")))
                            {
                                outputcommandstring = outputcommandstring + commandstrings[commandcat];
                            }

                            //AOCS
                            if (cat.Equals("aocs") && (commandcat.Contains("aicommsaocs") || commandcat.Contains("special")))
                            {
                                outputcommandstring = outputcommandstring + commandstrings[commandcat];
                            }

                            //RIO
                            if (cat.Equals("RIO") && commandcat.Contains("RIO"))
                            {
                                outputcommandstring = outputcommandstring + commandstrings[commandcat] + "Options;";
                            }

                            //RIO
                            if (cat.Equals("AI_pilot") && commandcat.Contains("AI_pilot"))
                            {
                                outputcommandstring = outputcommandstring + commandstrings[commandcat] + "Options;";
                            }

                            // George AI
                            if (cat.Equals("aicrew") && commandcat.Contains("AH64D_GeorgeAI"))
                            {
                                outputcommandstring = outputcommandstring + commandstrings[commandcat];
                            }

                            // WSO
                            if (cat.Equals("WSO") && commandcat.Contains("WSO"))
                            {
                                wsoOutputCommandString = wsoOutputCommandString + wsoCommandStrings[commandcat];
                            }

                            //kneeboard commands
                            if (cat.Equals("kneeboard") && commandcat.Contains("kneeboard"))
                            {
                                outputcommandstring = outputcommandstring + commandstrings[commandcat];
                            }
                        }
                        // set commandblock
                        if (outputcommandstring.Length > 0)
                        {
                            outputcommandstring = "[" + outputcommandstring;
                            outputcommandstring = outputcommandstring.TrimEnd("; ".ToCharArray());
                            outputcommandstring += "]"; // no ; here 
                        }

                        // set recipient block
                        string outputrecipientstring = "";
                        // Skip the WSO recipient category as this is handled seperately
                        if (!cat.StartsWith("WSO"))
                        {
                            outputrecipientstring = recipientstrings[cat];
                            outputrecipientstring = "[" + outputrecipientstring;
                            outputrecipientstring = outputrecipientstring.TrimEnd("; ".ToCharArray());
                            outputrecipientstring += ";]"; // recipient block is optional

                            if (cat.Equals("aiatc"))
                            {
                                // WSO only uses AI ATC receipients for tuning radio and diverting to airfields.
                                wsoOutputRecipientString = recipientstrings[cat];
                                wsoOutputRecipientString = "[" + wsoOutputRecipientString;
                                wsoOutputRecipientString = wsoOutputRecipientString.TrimEnd("; ".ToCharArray());
                                wsoOutputRecipientString += ";]"; // recipient block is optional
                            }

                            if (recipientstrings[cat].Length > 0 && outputcommandstring.Length > 0) // have aliases for recipient and commands
                            {
                                // construct:
                                string blockstring = "";

                                blockstring += outputrecipientstring;
                                if (cat.StartsWith("WSO") || cat.Equals("aijtac") || cat.Equals("aiatc") || cat.Equals("aifarp") || cat.Equals("aiship") || cat.Equals("aitanker") || cat.Equals("aiawacs") || cat.Equals("aocs")) //,
                                {
                                    //blockstring += outputsenderstring;
                                }
                                if (cat.Equals("aiflight"))
                                {
                                    blockstring += outputcuestring;
                                }
                                blockstring += outputcommandstring;
                                blockstring += ";";

                                outputstring += blockstring;
                            }
                        }
                    }
                }

                // Set WSO commands
                if (wsoOutputCommandString.Length > 0)
                {
                    wsoOutputCommandString = "[" + wsoOutputCommandString;
                    wsoOutputCommandString = wsoOutputCommandString.TrimEnd("; ".ToCharArray());
                    wsoOutputCommandString += "]"; // no ; here 
                }

                string outputappendixwpnstring = "";
                foreach (KeyValuePair<string, string> alias in Aliases.appendiceswpn)
                {
                    outputappendixwpnstring += "with " + alias.Key;
                    if (!alias.Key.EndsWith("*"))
                    {
                        outputappendixwpnstring += "";
                    }
                    outputappendixwpnstring += "; ";
                    counter = counter + 1;
                }

                string outputappendixdirstring = "";
                foreach (KeyValuePair<string, string> alias in Aliases.appendicesdir)
                {
                    if (!alias.Key.StartsWith("*"))
                    {
                        outputappendixdirstring += "";
                    }
                    outputappendixdirstring += alias.Key;
                    outputappendixdirstring += "; ";
                    counter = counter + 1;
                }

                // pragmatic add of Switch for vspx 
                string outputspecialcmdstring = "";
                foreach (KeyValuePair<string, string> alias in Aliases.aicommands)
                {
                    try
                    {
                        if (alias.Value.Equals("switch"))
                        {
                            outputspecialcmdstring += alias.Key + "; ";
                            counter = counter + 1;
                        }
                    }
                    catch
                    {
                    }
                }

                // commandstrings["special"];
                //outputspecialcmdstring += commandstrings["reply"];

                string outputmenucontrolcmds = "";
                foreach (KeyValuePair<string, string> alias in Aliases.aicommands)
                {
                    try
                    {
                        if (Commands.Table[alias.Value].category.Equals(CommandCategories.menu))
                        {
                            outputmenucontrolcmds += alias.Key + "; ";
                            counter = counter + 1;
                        }
                    }
                    catch
                    {
                    }
                }

                outputstring += outputappendixwpnstring;
                outputstring += outputappendixdirstring;
                outputstring += outputmenucontrolcmds;
                outputstring += commandstrings["reply"];
                //outputstring += outputsenderstring;
                outputstring += outputspecialcmdstring;

                // pragmatic add of Kneeboard for vspx 
                foreach (KeyValuePair<string, string> alias in Aliases.airecipients)
                {
                    try
                    {
                        if (alias.Value.Equals("kneeboard"))
                        {
                            outputstring += alias.Key + "; ";
                            counter = counter + 1;
                        }
                    }
                    catch
                    {
                    }
                }

                // pragmatic add of F10 Menu Items in the recipients list for vspx 
                foreach (KeyValuePair<string, string> alias in Aliases.airecipients)
                {
                    try
                    {
                        if (alias.Value.Equals("aux"))
                        {
                            outputstring += alias.Key + "; ";
                            counter = counter + 1;
                        }
                    }
                    catch
                    {
                    }
                }

                // pragmatic add of Dynamic matched F10 Menu commands for vspx 
                foreach (KeyValuePair<string, string> alias in Aliases.aicommands)
                {
                    try
                    {
                        if (alias.Value.Contains("Action"))
                        {
                            outputstring += alias.Key + "; ";
                            counter = counter + 1;
                        }
                    }
                    catch
                    {
                    }
                }

                // pragmatic add of imported F10 Menu commands for vspx 
                foreach (KeyValuePair<string, string> alias in Aliases.importedmenus)
                {
                    try
                    {
                        if (alias.Value.Contains("Action"))
                        {
                            outputstring += alias.Key + "; ";
                            counter = counter + 1;
                        }
                    }
                    catch
                    {
                    }
                }

                string wsoAliases = "[" + String.Join("; ", wsoRecipientStrings.Values) + "]";
                string wsoOutputString = wsoAliases + wsoOutputCommandString + wsoOutputRecipientString + ";";   // ; required here to start all others as new commands.

                Log.Write("Exported aliases to keywords.txt, keyword count = " + counter.ToString(), Colors.Text);

                // We return the WSO output string before the rest as the specific order is required.
                return wsoOutputString + outputstring;
            }


            // OLD VERSION: creates up-to-date string to reflect database
            public static string CreateMasterKeywordString()
            {
                int counter = 0;
                string outputstring = "";
                try
                {
                    HashSet<string> exportedphrases = new HashSet<string>(System.StringComparer.OrdinalIgnoreCase);

                    foreach (KeyValuePair<string, string> entry in master)
                    {
                        string cleanalias = entry.Key.Replace("*", "").Trim();

                        if (!cleanalias.Replace(" ", "").Equals("")) // skip any empties / blanks
                        {
                            if (exportedphrases.Add(cleanalias))
                            {
                                string insertstr = "*";
                                insertstr = insertstr + cleanalias;
                                insertstr = insertstr + "*";
                                insertstr = insertstr + "; ";

                                outputstring = outputstring + insertstr;
                                counter = counter + 1;
                            }

                            if (cleanalias.StartsWith("George", System.StringComparison.OrdinalIgnoreCase)
                                && (cleanalias.Length == 6 || char.IsWhiteSpace(cleanalias[6]) || cleanalias[6] == ','))
                            {
                                string gunneralias = "Gunner" + cleanalias.Substring(6);
                                if (exportedphrases.Add(gunneralias))
                                {
                                    string insertstr = "*";
                                    insertstr = insertstr + gunneralias;
                                    insertstr = insertstr + "*";
                                    insertstr = insertstr + "; ";

                                    outputstring = outputstring + insertstr;
                                    counter = counter + 1;
                                }
                            }
                        }
                    }
                    outputstring = outputstring.TrimEnd("; ".ToCharArray()); // finalizer, removes last ; closes string
                    Log.Write("Exported aliases to keywords.txt, keyword count = " + counter.ToString(), Colors.Text);
                }
                catch
                {
                }
                return outputstring;
            }
        }
    }
}