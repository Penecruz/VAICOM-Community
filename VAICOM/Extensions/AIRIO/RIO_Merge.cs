using System;
using System.Collections.Generic;
using VAICOM.Database;
using VAICOM.Static;

namespace VAICOM
{

    namespace Extensions
    {

        namespace RIO
        {

            public partial class ExtImport
            {

                public static RecipientCategories RecipientCatbyId(int id)
                {
                    RecipientCategories output = RecipientCategories.RIO;

                    if (id.Equals(19301))
                    {
                        return RecipientCategories.RIO;
                    }
                    if (id.Equals(19302))
                    {
                        return RecipientCategories.AI_pilot;
                    }

                    return output;
                }

                public static CommandCategories CommandCatbyId(int id)
                {
                    CommandCategories output = CommandCategories.RIO;

                    // RIO blocks from RIO/Commands.cs
                    // menu:       23200 - 23239
                    // radar:      23240 - 23329
                    // lantirn:    23330 - 23399
                    // weapons:    23400 - 23499
                    // radio:      23500 - 23599
                    // d-link:     23600 - 23649
                    // utility:    23650 - 23799  (navigation/util + walkman/polaroid)
                    // defensive:  23800 - 23879
                    // rio misc:   23880 - 23919
                    // ai pilot:   23920 - 23999

                    if (id >= 23200 && id <= 23239)
                    {
                        return CommandCategories.RIO_menu;
                    }
                    if (id >= 23240 && id <= 23329)
                    {
                        return CommandCategories.RIO_radar;
                    }
                    if (id >= 23330 && id <= 23399)
                    {
                        return CommandCategories.RIO_lantirn;
                    }
                    if (id >= 23400 && id <= 23499)
                    {
                        return CommandCategories.RIO_weapons;
                    }
                    if (id >= 23500 && id <= 23599)
                    {
                        return CommandCategories.RIO_radio;
                    }
                    if (id >= 23600 && id <= 23649)
                    {
                        return CommandCategories.RIO_dlink;
                    }
                    if (id >= 23650 && id <= 23799)
                    {
                        return CommandCategories.RIO_utility;
                    }
                    if (id >= 23800 && id <= 23879)
                    {
                        return CommandCategories.RIO_defensive;
                    }
                    if (id >= 23880 && id <= 23919)
                    {
                        return CommandCategories.RIO_misc;
                    }
                    if (id >= 23920 && id <= 23999)
                    {
                        return CommandCategories.AI_pilot;
                    }
                    return output;
                }

                public static int MergeRIO()
                {
                    int count = 0;

                    Log.Write("Importing RIO extension pack...", Colors.Text);

                    foreach (KeyValuePair<string, RIO.RecipientInfo> entry in RIO.Recipients.aicomms)
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
                            Log.Write("There was a problem while importing the RIO extension pack." + entry.Key + " " + a.Message, Colors.Text);
                        }
                    }

                    // add to commands all
                    foreach (KeyValuePair<string, RIO.CommandInfo> entry in RIO.Commands.all)
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
                            Log.Write("There was a problem while importing the RIO extension pack." + entry.Key + " " + a.StackTrace, Colors.Text);
                        }
                    }


                    // KEYWORDS

                    // add aliases (for recipients)
                    foreach (KeyValuePair<string, string> entry in RIO.Aliases.airecipients)
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
                            Log.Write("There was a problem while importing the RIO extension pack." + entry.Key + " " + a.StackTrace, Colors.Text);
                        }
                    }

                    // add aliases (for commands)
                    foreach (KeyValuePair<string, string> entry in RIO.Aliases.aicommands)
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
                            Log.Write("There was a problem while importing the RIO extension pack." + entry.Key + " " + a.StackTrace, Colors.Text);
                        }
                    }

                    // add labels (for recipients)
                    foreach (KeyValuePair<string, string> entry in RIO.Labels.airecipients)
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
                            Log.Write("There was a problem while importing the RIO extension pack." + entry.Key + " " + a.StackTrace, Colors.Text);
                        }
                    }

                    // add labels (for commands)
                    foreach (KeyValuePair<string, string> entry in RIO.Labels.aicommands)
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
                            Log.Write("There was a problem while importing the RIO extension pack." + entry.Key + " " + a.InnerException, Colors.Text);
                        }
                    }

                    Log.Write("Done.", Colors.Text);

                    return count;

                }
            }
        }
    }
}
