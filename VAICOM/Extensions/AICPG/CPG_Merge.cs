using System;
using System.Collections.Generic;
using VAICOM.Database;
using VAICOM.Static;

namespace VAICOM
{
    namespace Extensions
    {
        namespace CPG
        {
            public partial class ExtImport
            {
                public static RecipientCategories RecipientCatbyId(int id)
                {
                    RecipientCategories output = RecipientCategories.GeorgeCPG;

                    if (id.Equals(19601))
                    {
                        return RecipientCategories.GeorgeCPG;
                    }

                    return output;
                }

                public static CommandCategories CommandCatbyId(int id)
                {
                    CommandCategories output = CommandCategories.AH64D_George;

                    if (id >= 25214 && id <= 25499)
                    {
                        return CommandCategories.AH64D_George_CPG;
                    }
                    if (id >= 25500 && id <= 25999)
                    {
                        return CommandCategories.AH64D_George_PLT;
                    }
                    
                    return output;
                }

                public static int MergeCPG()
                {
                    int count = 0;

                    Log.Write("Importing CPG extension pack...", Colors.Text);

                    foreach (KeyValuePair<string, CPG.RecipientInfo> entry in CPG.Recipients.aicomms)
                    {
                        try
                        {
                            Recipient newrecipient = new Recipient();

                            newrecipient.category = RecipientCatbyId(entry.Value.uniqueid);
                            newrecipient.uniqueid = entry.Value.uniqueid;
                            newrecipient.name = entry.Value.name;
                            newrecipient.displayname = entry.Value.displayname;
                            newrecipient.enabled = entry.Value.enabled;

                            if (newrecipient.enabled)
                            {
                                Database.Recipients.Table.Add(entry.Key, newrecipient);
                                count += 1;
                            }

                        }
                        catch (Exception a)
                        {
                            Log.Write("There was a problem while importing the CP/G extension pack." + entry.Key + " " + a.Message, Colors.Text);
                        }
                    }

                    // add to commands all
                    foreach (KeyValuePair<string, CPG.CommandInfo> entry in CPG.Commands.all)
                    {
                        try
                        {
                            Command newcommand = new Command();

                            newcommand.category = CommandCatbyId(entry.Value.uniqueid);
                            newcommand.uniqueid = entry.Value.uniqueid;
                            newcommand.dcsid = entry.Value.name;
                            newcommand.displayname = entry.Value.displayname;
                            newcommand.eventnumber = entry.Value.eventnumber;
                            newcommand.enabled = entry.Value.enabled;

                            if (newcommand.enabled)
                            {
                                Database.Commands.Table.Add(entry.Key, newcommand);
                                count += 1;
                            }

                        }
                        catch (Exception a)
                        {
                            Log.Write("There was a problem while importing the CPG extension pack." + entry.Key + " " + a.StackTrace, Colors.Text);
                        }
                    }

                    // add aliases (for recipients)
                    foreach (KeyValuePair<string, string> entry in CPG.Aliases.airecipients)
                    {
                        try
                        {
                            if (Database.Recipients.Table.ContainsKey(entry.Value))
                            {
                                Database.Aliases.airecipients.Add(entry.Key, entry.Value);
                                count += 1;
                            }
                        }
                        catch (Exception a)
                        {
                            Log.Write("There was a problem while importing the CPG Keywords." + entry.Key + " " + a.StackTrace, Colors.Text);
                        }
                    }

                    // add aliases (for commands)
                    foreach (KeyValuePair<string, string> entry in CPG.Aliases.aicommands)
                    {
                        try
                        {
                            if (Database.Commands.Table.ContainsKey(entry.Value))
                            {
                                Database.Aliases.aicommands.Add(entry.Key, entry.Value);
                                count += 1;
                            }
                        }
                        catch (Exception a)
                        {
                            Log.Write("There was a problem while importing the CPG Aliases." + entry.Key + " " + a.StackTrace, Colors.Text);
                        }
                    }

                    // add labels (for recipients)
                    foreach (KeyValuePair<string, string> entry in CPG.Labels.airecipients)
                    {
                        try
                        {
                            if (Database.Recipients.Table.ContainsKey(entry.Key))
                            {
                                Database.Labels.airecipients.Add(entry.Key, entry.Value);
                                count += 1;
                            }
                        }
                        catch (Exception a)
                        {
                            Log.Write("There was a problem while importing the CPG labels." + entry.Key + " " + a.StackTrace, Colors.Text);
                        }
                    }

                    // add labels (for commands)
                    foreach (KeyValuePair<string, string> entry in CPG.Labels.aicommands)
                    {
                        try
                        {
                            if (Database.Commands.Table.ContainsKey(entry.Key))
                            {
                                Database.Labels.aicommands.Add(entry.Key, entry.Value);
                                count += 1;
                            }
                        }
                        catch (Exception a)
                        {
                            Log.Write("There was a problem while importing the CPG labels for commands." + entry.Key + " " + a.InnerException, Colors.Text);
                        }
                    }

                    Log.Write("Done.", Colors.Text);

                    return count;

                }
            }
        }
    }
}