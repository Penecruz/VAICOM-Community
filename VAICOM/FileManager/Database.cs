using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using VAICOM.Database;
using VAICOM.Static;

namespace VAICOM
{
    namespace FileManager
    {

        public partial class FileHandler
        {

            public partial class Database
            {

                //Writes a single category database to file (used by import: menus, atcs)
                public static void WriteCategoryToFile(string cat, Dictionary<string, string> table, bool overwrite)
                {
                    Dictionary<string, string> WriteObject = table;
                    try
                    {
                        string filename;

                        if (State.databaseencrypted)
                        { filename = Aliases.scrambleddbfilenames[cat]; }
                        else
                        { filename = cat + ".json"; }

                        string path = State.VA_APPS + "\\" + AppData.RootFolder + "\\" + AppData.SubFolders["database"] + "\\" + filename;
                        string jsonstring;
                        if (State.databaseencrypted)
                        {
                            jsonstring = Helpers.Crypto.Encrypt(JsonConvert.SerializeObject(WriteObject, Formatting.Indented));
                        }
                        else
                        {
                            jsonstring = JsonConvert.SerializeObject(WriteObject, Formatting.Indented);
                        }

                        if (overwrite || !File.Exists(path))
                        {
                            File.WriteAllText(path, jsonstring);
                        }
                    }
                    catch (Exception e)
                    {
                        Log.Write("Exception: " + e.ToString(), Colors.Text);
                    }

                }

                // restores/writes all keyword (alias) database files
                public static void WriteAllCategoriesToFile(bool overwrite)
                {
                    try
                    {
                        // Existing logic for writing other categories
                        foreach (KeyValuePair<string, Dictionary<string, string>> entry in Aliases.categories)
                        {
                            string filename = State.databaseencrypted ? Aliases.scrambleddbfilenames[entry.Key] : entry.Key + ".json";
                            string path = State.VA_APPS + "\\" + AppData.RootFolder + "\\" + AppData.SubFolders["database"] + "\\" + filename;

                            string jsonstring = State.databaseencrypted
                                ? Helpers.Crypto.Encrypt(JsonConvert.SerializeObject(entry.Value, Formatting.Indented))
                                : JsonConvert.SerializeObject(entry.Value, Formatting.Indented);

                            if (overwrite || !File.Exists(path))
                            {
                                File.WriteAllText(path, jsonstring);
                            }
                        }                        
                    }

                    EnsureAiCrewPageRecipientAliases(ref warncounter);
                    catch (Exception e)
                    {
                        Log.Write("Exception while writing all categories: " + e.ToString(), Colors.Text);
                    }

                }

                // reads entire database into memory
                public static void ReadAllCategoriesFromFile()
                {
                    Log.Write("Loading keywords database...", Colors.Text);

                    // Read database into reference tables
                    foreach (KeyValuePair<string, Dictionary<string, string>> entry in Aliases.categories)
                    {
                        string filename = State.databaseencrypted ? Aliases.scrambleddbfilenames[entry.Key] : entry.Key + ".json";
                        try
                        {
                            string path = State.VA_APPS + "\\" + AppData.RootFolder + "\\" + AppData.SubFolders["database"] + "\\" + filename;
                            Dictionary<string, string> ReturnObject = State.databaseencrypted
                                ? JsonConvert.DeserializeObject<Dictionary<string, string>>(Helpers.Crypto.Decrypt(File.ReadAllText(path)))
                                : JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(path));

                            Aliases.reference[entry.Key] = new Dictionary<string, string>(ReturnObject, StringComparer.OrdinalIgnoreCase);
                        }
                        catch (Exception e)
                        {
                            Log.Write("Exception while reading category: " + e.Message, Colors.Text);
                        }
                    }

                    int warncounter = 0;

                    foreach (KeyValuePair<string, Dictionary<string, string>> entry in Aliases.categories)
                    {
                        foreach (KeyValuePair<string, string> alias in entry.Value)
                        {
                            try
                            {
                                string keyword = alias.Value;
                                bool diskhasaliasforkeyword = Aliases.reference[entry.Key].ContainsValue(keyword);
                                if (!diskhasaliasforkeyword)
                                {
                                    warncounter++;
                                    foreach (KeyValuePair<string, string> newalias in entry.Value)
                                    {
                                        if (newalias.Value.Equals(keyword))
                                        {
                                            Aliases.reference[entry.Key].Add(newalias.Key, newalias.Value);
                                            Log.Write("   -> " + newalias.Key, Colors.Text);
                                        }
                                    }
                                }

                                if (State.activeconfig.UseNewRecognitionModel)
                                {
                                    bool diskhasaliaswithoutasterisk = alias.Key.Contains("*") && Aliases.reference[entry.Key].ContainsKey(alias.Key.Replace("*", ""));
                                    if (diskhasaliaswithoutasterisk)
                                    {
                                        string keywordstr = Aliases.reference[entry.Key][alias.Key.Replace("*", "")];
                                        Aliases.reference[entry.Key].Remove(alias.Key.Replace("*", ""));
                                        Aliases.reference[entry.Key].Add(alias.Key, keywordstr);
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                Log.Write("There was a problem adding references for " + alias.Key + ": " + e.Message, Colors.Text);
                            }
                        }
                    }

                    Aliases.appendiceswpn = Aliases.reference["aiappendiceswpn"];
                    Aliases.appendicesdir = Aliases.reference["aiappendicesdir"];
                    Aliases.aicues = Aliases.reference["aicues"];
                    Aliases.aicommands = Aliases.reference["aicommands"];
                    Aliases.airecipients = Aliases.reference["airecipients"];
                    Aliases.cockpitcontrol = Aliases.reference["cockpitcontrol"];
                    Aliases.importedatcs = Aliases.reference["importedatcs"];
                    Aliases.importedmenus = Aliases.reference["importedmenus"];
                    Aliases.playercallsigns = Aliases.reference["playercallsigns"];
                    Aliases.simcontrol = Aliases.reference["simcontrol"];

                    Aliases.categories["aiappendiceswpn"] = Aliases.reference["aiappendiceswpn"];
                    Aliases.categories["aiappendicesdir"] = Aliases.reference["aiappendicesdir"];
                    Aliases.categories["aicues"] = Aliases.reference["aicues"];
                    Aliases.categories["aicommands"] = Aliases.reference["aicommands"];
                    Aliases.categories["airecipients"] = Aliases.reference["airecipients"];
                    Aliases.categories["cockpitcontrol"] = Aliases.reference["cockpitcontrol"];
                    Aliases.categories["importedatcs"] = Aliases.reference["importedatcs"];
                    Aliases.categories["importedmenus"] = Aliases.reference["importedmenus"];
                    Aliases.categories["playercallsigns"] = Aliases.reference["playercallsigns"];
                    Aliases.categories["simcontrol"] = Aliases.reference["simcontrol"];

                    Aliases.inputscancats["recipient"] = Aliases.reference["airecipients"];
                    Aliases.inputscancats["importedatcs"] = Aliases.reference["importedatcs"];
                    Aliases.inputscancats["importemenus"] = Aliases.reference["importedmenus"];
                    Aliases.inputscancats["sender"] = Aliases.reference["playercallsigns"];
                    Aliases.inputscancats["cue"] = Aliases.reference["aicues"];
                    Aliases.inputscancats["command"] = Aliases.reference["aicommands"];
                    Aliases.inputscancats["apxwpn"] = Aliases.reference["aiappendiceswpn"];
                    Aliases.inputscancats["apxdir"] = Aliases.reference["aiappendicesdir"];

                    if (warncounter > 0)
                    {
                        Log.Write("Writing updates.", Colors.Text);
                        FileHandler.Database.WriteAllCategoriesToFile(true);
                    }                 

                    Log.Write("Keywords database loaded successfully.", Colors.Text);
                }

                private static void EnsureAiCrewPageRecipientAliases(ref int warncounter)
                {
                    if (!Aliases.reference.ContainsKey("airecipients"))
                    {
                        return;
                    }

                    Dictionary<string, string> recipients = Aliases.reference["airecipients"];
                    var requiredAliases = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                    {
                        { "Boots", "crew" },
                        { "Jester", "crew" },
                        { "Iceman", "crew" },
                        { "George", "crew" },
                        { "WSO", "crew" },
                        { "RIO", "crew" },
                    };

                    foreach (KeyValuePair<string, string> alias in requiredAliases)
                    {
                        if (!recipients.ContainsKey(alias.Key))
                        {
                            recipients.Add(alias.Key, alias.Value);
                            warncounter++;
                            Log.Write("   -> " + alias.Key, Colors.Text);
                        }
                    }
                }
            }
        }
    }
}
