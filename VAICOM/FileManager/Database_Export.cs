using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
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
                // txt file export
                public static void ExportMasterKeywordString()
                {
                    try
                    {
                        string writestring = "";
                        if (State.activeconfig.UseNewRecognitionModel)
                        {
                            writestring = Aliases.CreateMasterKeywordStringVSPX();
                        }
                        else
                        {
                            writestring = Aliases.CreateMasterKeywordString();
                        }
                        string filename = "keywords.txt";
                        string path = State.VA_APPS + "\\" + AppData.RootFolder + "\\" + AppData.SubFolders["export"] + "\\" + filename;
                        File.WriteAllText(path, writestring);
                        ExportKeywordsAsHTML();
                        System.Windows.Clipboard.SetDataObject(writestring);
                        Log.Write("File export success.", Colors.Text);

                    }
                    catch (Exception a)
                    {
                        Log.Write("There was a problem during file export." + a.Message, Colors.Warning);
                        UI.Playsound.Error();
                    }
                }

                // csv file export
                public static bool ExportKeywordsAsCSV()
                {
                    try
                    {
                        string writestring = "";
                        writestring = Aliases.CreateCSVFile();
                        string filename = "keywords.csv";
                        string path = State.VA_APPS + "\\" + AppData.RootFolder + "\\" + AppData.SubFolders["export"] + "\\" + filename;
                        File.WriteAllText(path, writestring);
                        ExportKeywordsAsHTML();
                        Log.Write("File export success.", Colors.Text);
                        UI.Playsound.Commandcomplete();
                        ExportMasterKeywordString();
                        ExplorerOpenFolder("export");
                        return true;
                    }
                    catch
                    {
                        Log.Write("There was a problem during file export.", Colors.Text);
                        UI.Playsound.Error();
                        return false;
                    }
                }

                public static bool ExportKeywordsAsHTML()
                {
                    try
                    {
                        string writestring = Aliases.CreateHTMLFile();
                        string filename = "keywords.html";
                        string path = State.VA_APPS + "\\" + AppData.RootFolder + "\\" + AppData.SubFolders["export"] + "\\" + filename;
                        File.WriteAllText(path, writestring);
                        Extensions.Kneeboard.OpenKneeboardBridge.RegisterKeywordsHtmlPlugin(path);
                        Log.Write("HTML keywords export success.", Colors.Text);
                        return true;
                    }
                    catch
                    {
                        Log.Write("There was a problem during HTML keywords export.", Colors.Text);
                        return false;
                    }
                }

                public static string Getcat(string Value)
                {
                    string cat = "";

                    if (Aliases.appendiceswpn.ContainsValue(Value))
                    {
                        cat = "aiappendiceswpn";
                    }
                    if (Aliases.appendicesdir.ContainsValue(Value))
                    {
                        cat = "aiappendicesdir";
                    }
                    if (Aliases.aicues.ContainsValue(Value))
                    {
                        cat = "aicues";
                    }
                    if (Aliases.aicommands.ContainsValue(Value))
                    {
                        cat = "aicommands";
                    }
                    if (Aliases.airecipients.ContainsValue(Value))
                    {
                        cat = "airecipients";
                    }
                    if (Aliases.cockpitcontrol.ContainsValue(Value))
                    {
                        cat = "cockpitcontrol";
                    }
                    if (Aliases.importedatcs.ContainsValue(Value))
                    {
                        cat = "importedatcs";
                    }
                    if (Aliases.importedmenus.ContainsValue(Value))
                    {
                        cat = "importedmenus";
                    }
                    if (Aliases.playercallsigns.ContainsValue(Value))
                    {
                        cat = "playercallsigns";
                    }
                    if (Aliases.simcontrol.ContainsValue(Value))
                    {
                        cat = "simcontrol";
                    }

                    return cat;
                }

                public static bool ImportCSVAsKeywords()
                {
                    bool success = false;

                    try
                    {

                        OpenFileDialog dialog = new OpenFileDialog();
                        dialog.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
                        dialog.Title = "Select a keywords file to import.";

                        if (dialog.ShowDialog() == DialogResult.OK)
                        {

                            string caption = "Import keywords to database";
                            string message = "";
                            MessageBoxImage Image = new MessageBoxImage();

                            var readheader = new StreamReader(dialog.FileName);
                            string firstline = readheader.ReadLine();
                            string[] headervalues = firstline.Split(';');
                            readheader.Close();

                            int columnkey = 1;
                            int columnvalue = 3;

                            bool validatedkey = false;
                            bool validatedvalue = false;
                            bool validated = false;

                            for (int i = 0; i < headervalues.Length; i++)
                            {
                                if (headervalues[i].ToLower().Contains("voice command phrase"))
                                {
                                    columnkey = i;
                                    validatedkey = true;
                                }
                                if (headervalues[i].ToLower().Contains("command segment"))
                                {
                                    columnvalue = i;
                                    validatedvalue = true;
                                }
                            }

                            validated = validatedkey && validatedvalue;

                            if (validated)
                            {
                                message = "Ready to import keywords from file.\n\nFor matching entries:\ndo you want to replace your current aliases?\n\nYES = replace aliases (overwrite)\nNO = add aliases (and keep existing)\n";
                                Image = MessageBoxImage.Information;
                            }
                            else // does't look good
                            {
                                message = "WARNING: The file appears not to be in the correct format.\nPress Cancel to abort or YES/NO to proceed anyway.\n\nFor matching entries:\ndo you want to replace your current aliases?\n\nYES = replace aliases (overwrite)\nNO = add aliases (and keep existing)\n";
                                Image = MessageBoxImage.Warning;
                            }

                            MessageBoxResult choice = System.Windows.MessageBox.Show(message, caption, MessageBoxButton.YesNoCancel, Image);

                            if (choice.Equals(MessageBoxResult.Cancel))
                            {
                                success = false;
                            }
                            else
                            {

                                Dictionary<string, string> filecapture = new Dictionary<string, string>();

                                String filecontents = "";

                                using (var reader = new StreamReader(dialog.FileName))
                                {

                                    while (!reader.EndOfStream)
                                    {
                                        var line = reader.ReadLine();
                                        var values = line.Split(';');

                                        try
                                        {
                                            var FoundKey = Labels.master.FirstOrDefault(x => x.Value == values[columnvalue]).Key;
                                            if (!FoundKey.Equals(null))
                                            {
                                                // don't allow empty fields
                                                if (!FoundKey.Replace(" ", "").Equals("") && !values[columnkey].Replace(" ", "").Equals(""))
                                                {
                                                    filecapture.Add(values[columnkey], FoundKey);
                                                    filecontents += FoundKey + "\n";
                                                }
                                            }
                                        }
                                        catch
                                        {
                                        }

                                    }

                                    reader.Close();
                                }

                                int importcounter = 0;

                                foreach (KeyValuePair<string, string> entry in filecapture)
                                {

                                    try
                                    {

                                        string newalias = Helpers.Common.TrimmedString(Helpers.Common.RemoveIllegalCharsForDB(entry.Key));
                                        // get category first:
                                        string maincat = Getcat(entry.Value);
                                        if (maincat.Equals(""))
                                        {
                                            //Log.Write(maincat + ": no cat found for " + entry.Value, Colors.Warning);
                                        }
                                        else
                                        {

                                            Aliases.categories[maincat].Add(newalias, entry.Value);
                                            Log.Write("Import: adding new alias " + newalias + " for " + Labels.master[entry.Value], Colors.Text);
                                            importcounter += 1;

                                            // if overwrite delete all existing alias entries for keyword (except new)

                                            if (choice.Equals(MessageBoxResult.Yes))
                                            {

                                                foreach (KeyValuePair<string, string> existingentry in Aliases.categories[maincat])
                                                {
                                                    if (existingentry.Value.Equals(entry.Value))
                                                    {

                                                        try
                                                        {
                                                            if (!filecapture.ContainsKey(existingentry.Key))
                                                            {
                                                                Aliases.categories[maincat].Remove(existingentry.Key);
                                                                //Log.Write(maincat + ": removing " + existingentry.Key, Colors.Warning);
                                                            }

                                                        }
                                                        catch (Exception e)
                                                        {
                                                            Log.Write(maincat + ":  " + e.Message, Colors.Warning);
                                                        }
                                                    }

                                                }

                                            }
                                        }
                                    }
                                    catch
                                    {
                                    }

                                }

                                FileHandler.Database.WriteAllCategoriesToFile(true);

                                string report = "";
                                if (importcounter.Equals(0))
                                {
                                    report = "No new aliases were imported.";
                                    success = false;
                                }
                                else
                                {
                                    report = "Added " + importcounter + " new aliases.\nFollow FINISH steps to update the profile.";
                                    success = true;
                                }

                                System.Windows.MessageBox.Show(report, "Import complete!", MessageBoxButton.OK, MessageBoxImage.Information);


                            }
                        }

                    }
                    catch (Exception e)
                    {
                        Log.Write("There was a problem reading the file contents, no new aliases were added.", Colors.Warning);
                    }

                    return success;
                }
            }

        }
    }
}
