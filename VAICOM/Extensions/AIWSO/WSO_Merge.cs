using System;
using System.Collections.Generic;
using VAICOM.Database;
using VAICOM.Static;   

namespace VAICOM
{
    namespace Extensions
    {
        namespace WSO
        {
            public partial class ExtImport
            {
                public static RecipientCategories RecipientCatbyId(int id)
                {
                    RecipientCategories output = RecipientCategories.WSO;

                    if (id.Equals(19501))
                    {
                        return RecipientCategories.WSO;
                    }                    

                    return output;
                }

                public static CommandCategories CommandCatbyId(int id)
                {
                    CommandCategories output = CommandCategories.WSO;

                    if (id >= 24000 && id <= 24099)
                    {
                        return CommandCategories.WSO_menu;
                    }
                    if (id >= 24100 && id <= 24199)
                    {
                        return CommandCategories.WSO_radar;
                    }
                    if (id >= 24200 && id <= 24299)
                    {
                        return CommandCategories.WSO_weapons;
                    }
                    if (id >= 24300 && id <= 24399)
                    {
                        return CommandCategories.WSO_radio;
                    }
                    if (id >= 24400 && id <= 24499)
                    {
                        return CommandCategories.WSO_utility;
                    }
                    if (id >= 24500 && id <= 24599)
                    {
                        return CommandCategories.WSO_defensive;
                    }
                    if (id >= 24600 && id <= 24999)
                    {
                        return CommandCategories.WSO_misc;
                    }
                    return output;
                }

                public static int MergeWSO()
                {
                    int count = 0;

                    Log.Write("Importing WSO extension pack...", Colors.Text);

                    foreach (KeyValuePair<string, WSO.RecipientInfo> entry in WSO.Recipients.aicomms)
                    {
                        try
                        {
                            Recipient newrecipient = new Recipient();

                            newrecipient.category = RecipientCatbyId(entry.Value.uniqueid);
                            newrecipient.uniqueid = entry.Value.uniqueid;
                            newrecipient.name = entry.Value.name;
                            newrecipient.displayname = entry.Value.displayname;
                            newrecipient.requiresWSO = entry.Value.requiresWSO;
                            newrecipient.enabled = entry.Value.enabled;
                            newrecipient.blockedforFree = entry.Value.blockedforFree;

                            if (newrecipient.enabled)
                            {
                                Database.Recipients.Table.Add(entry.Key, newrecipient);
                                count += 1;
                            }

                        }
                        catch (Exception a)
                        {
                            Log.Write("There was a problem while importing the WSO extension pack." + entry.Key + " " + a.Message, Colors.Text);
                        }
                    }

                    // add to commands all
                    foreach (KeyValuePair<string, WSO.CommandInfo> entry in WSO.Commands.all)
                    {
                        try
                        {
                            Command newcommand = new Command();

                            newcommand.category = CommandCatbyId(entry.Value.uniqueid);
                            newcommand.uniqueid = entry.Value.uniqueid;
                            newcommand.dcsid = entry.Value.name;
                            newcommand.displayname = entry.Value.displayname;
                            newcommand.eventnumber = entry.Value.eventnumber;
                            newcommand.requiresWSO = entry.Value.requiresWSO;
                            newcommand.enabled = entry.Value.enabled;
                            newcommand.blockedforFree = entry.Value.blockedforFree;

                            if (newcommand.enabled)
                            {
                                Database.Commands.Table.Add(entry.Key, newcommand);
                                count += 1;
                            }

                        }
                        catch (Exception a)
                        {
                            Log.Write("There was a problem while importing the WSO extension pack." + entry.Key + " " + a.StackTrace, Colors.Text);
                        }
                    }


                    // KEYWORDS

                    // add aliases (for recipients)
                    foreach (KeyValuePair<string, string> entry in WSO.Aliases.airecipients)
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
                            Log.Write("There was a problem while importing the WSO Keywords." + entry.Key + " " + a.StackTrace, Colors.Text);
                        }
                    }

                    // add aliases (for commands)
                    foreach (KeyValuePair<string, string> entry in WSO.Aliases.aicommands)
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
                            Log.Write("There was a problem while importing the WSO Aliases." + entry.Key + " " + a.StackTrace, Colors.Text);
                        }
                    }

                    // add labels (for recipients)
                    foreach (KeyValuePair<string, string> entry in WSO.Labels.airecipients)
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
                            Log.Write("There was a problem while importing the WSO labels." + entry.Key + " " + a.StackTrace, Colors.Text);
                        }
                    }

                    // add labels (for commands)
                    foreach (KeyValuePair<string, string> entry in WSO.Labels.aicommands)
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
                            Log.Write("There was a problem while importing the WSO labels for commands." + entry.Key + " " + a.InnerException, Colors.Text);
                        }
                    }

                    Log.Write("Done.", Colors.Text);

                    return count;

                }
            }
        }
    }
}