using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using VAICOM.Database;
using VAICOM.FileManager;
using VAICOM.Servers;
using VAICOM.Static;
using StaticColors = VAICOM.Static.Colors;


namespace VAICOM
{
    namespace UI
    {

        public partial class ConfigWindow : Window
        {
            // ------------  KEYWORDS EDITOR ------------------------------

            private void RefreshEditor(object sender, MouseButtonEventArgs e)
            {
                Reflectrequiresreload();
                Reflectunsavedchanges();
            }

            private void InitAddNewAlias(object sender, EventArgs e)
            {
                newalias.IsEnabled = true;
            }
            private void InitDeleteCurrentAlias(object sender, EventArgs e)
            {
                deletealias.IsEnabled = true;
            }
            private void InitcomboBoxAlias_TextRevert(object sender, EventArgs e)
            {
                Revert_Button.IsEnabled = true;
            }
            private void InitcomboBoxAlias_TextUpdate(object sender, EventArgs e)
            {
                Apply_Button.IsEnabled = true;
            }
            private void InitKeywordsReloadOriginalDB(object sender, EventArgs e)
            {
                Restore_Button.IsEnabled = true;
                if (Restore_Button.IsEnabled)
                {
                    Reflectrequiresreload();
                }
            }
            private void InitExportCSV(object sender, EventArgs e)
            {
                ExportCSVbutton.IsEnabled = true;
            }
            private void InitNextAlias(object sender, EventArgs e)
            {
                AliasCycle.IsEnabled = true;
            }
            private void InitKeywordsExport(object sender, EventArgs e)
            {
                KeywordsExport.IsEnabled = true;
                Reflectunsavedchanges();
            }
            private void InitCancel(object sender, EventArgs e)
            {
                Cancel.IsEnabled = true;
            }

            private void ComboSetInitialValue(object sender, EventArgs e)
            {
                try
                {
                    comboBoxAlias.IsEnabled = true;
                    comboBoxAlias.IsEditable = true;
                    comboBoxAlias.Text = "--Select Keyword--";
                }
                catch
                {
                }
            }

            private void InitMessage(object sender, EventArgs e)
            {
                Message.Text = "";
            }


            private void Toggle_VSPX_Light(object sender, EventArgs e)
            {
                Toggle_VSPX_Light();
            }
            public void Toggle_VSPX_Light()
            {
                if (State.activeconfig.UseNewRecognitionModel)
                {
                    VSPX_light_On.Visibility = Visibility.Visible;
                    VSPX_light_Off.Visibility = Visibility.Hidden;
                }
                else
                {
                    VSPX_light_Off.Visibility = Visibility.Visible;
                    VSPX_light_On.Visibility = Visibility.Hidden;
                }
            }

            public void SetAliasCounter()
            {
                try
                {
                    aliascounter.Text = State.aliascount.ToString();
                }
                catch
                {
                }
            }

            private void SetAliasCounter(object sender, EventArgs e)
            {
                SetAliasCounter();
            }

            public int Getaliascount()
            {
                int aliascount = 0;

                try
                {
                    State.editorcurrentaliases = new List<string>();
                    foreach (KeyValuePair<string, string> pair in Aliases.master)
                    {
                        if (pair.Value.Equals(State.editorcurrentkeyword))
                        {
                            State.editorcurrentaliases.Add(pair.Key);
                            aliascount = aliascount + 1;
                        }
                    }
                    string displaycounter = "(" + aliascount.ToString() + ")";
                    AliasCycle.Content = displaycounter;
                }
                catch
                {
                }

                return aliascount;
            }

            public void Reflectrequiresreload()
            {
                if (State.activeconfig.Editorrequiresreload)
                {
                    Restore_Button.BorderBrush = System.Windows.Media.Brushes.LightYellow;// DarkOrange;
                    Restore_Button.Background = System.Windows.Media.Brushes.Orange;
                    Restore_Button.Foreground = System.Windows.Media.Brushes.LightYellow;  // black
                }
                else
                {
                    Restore_Button.BorderBrush = System.Windows.Media.Brushes.DarkGray;
                    Restore_Button.Background = System.Windows.Media.Brushes.LightGray;
                    Restore_Button.Foreground = System.Windows.Media.Brushes.Black;
                }

            }

            public void Reflectunsavedchanges()
            {
                if (State.activeconfig.Editorunsavedchanges)
                {
                    KeywordsExport.BorderBrush = System.Windows.Media.Brushes.LightYellow; //.DarkOrange; 
                    KeywordsExport.Background = System.Windows.Media.Brushes.Orange;
                    KeywordsExport.Foreground = System.Windows.Media.Brushes.LightYellow;  // black
                }
                else
                {
                    KeywordsExport.BorderBrush = System.Windows.Media.Brushes.DarkGray;
                    KeywordsExport.Background = System.Windows.Media.Brushes.LightGray;
                    KeywordsExport.Foreground = System.Windows.Media.Brushes.Black;
                }

            }

            private void EditorDeleteCurrentAlias(object sender, RoutedEventArgs e)
            {
                try
                {
                    if (State.editorcurrentalias != null)
                    {
                        int aliascount = Getaliascount();
                        if (aliascount > 1 || State.editorcurrentsourcetable.Equals("importedmenus") || State.editorcurrentsourcetable.Equals("importedatcs"))
                        {
                            Aliases.categories[State.editorcurrentsourcetable].Remove(State.editorcurrentalias);

                            if (State.editorcurrentsourcetable.Equals("importedmenus"))
                            {
                                Server.auxmenuitems.Remove(State.editorcurrentalias);
                                Aliases.importedmenus.Remove(State.editorcurrentalias);

                                FileHandler.Database.WriteAllCategoriesToFile(true);
                                FileHandler.Database.WriteAuxMenuItemsToFile(true);

                            }

                            if (State.editorcurrentsourcetable.Equals("importedatcs"))
                            {
                                Labels.airecipients.Remove(State.editorcurrentalias);
                                Aliases.airecipients.Remove(State.editorcurrentalias);

                                FileHandler.Database.WriteAllCategoriesToFile(true);
                            }

                            Message.Text = "Command phrase deleted from database: '" + State.editorcurrentalias + "'.";
                            Aliases.BuildNewMasterTable();
                            comboBoxAlias.ItemsSource = null;
                            comboBoxAlias.ItemsSource = Aliases.master;
                            aliascounter.Text = State.aliascount.ToString();
                            Getaliascount();
                            if (State.editorcurrentaliases.Count > 0)
                            {
                                State.editorselectedalias = State.editorselectedalias - 1;
                                if (State.editorselectedalias < 0) { State.editorselectedalias = State.editorcurrentaliases.Count - 1; }
                            }
                            else
                            {
                                comboBoxAlias.SelectedIndex = State.editorcurrentindex - 1;

                            }
                            State.activeconfig.Editorunsavedchanges = true;
                            Reflectunsavedchanges();

                            State.editorcurrentalias = State.editorcurrentaliases[State.editorselectedalias];

                            comboBoxAlias.Text = State.editorcurrentalias;

                        }
                        else
                        {
                            Message.Text = "Cannot delete: at least one command phrase is required for " + Labels.categories[State.editorcurrentsourcetable][State.editorcurrentkeyword] + ".";
                        }
                    }
                    else
                    {
                        Message.Text = "(first select a command phrase)";
                    }
                }
                catch
                {
                }
            }

            private void NextAlias(object sender, RoutedEventArgs e)
            {
                try
                {
                    if (State.editorcurrentaliases.Count > 1)
                    {
                        State.editorselectedalias = State.editorselectedalias + 1;
                        if (State.editorselectedalias >= State.editorcurrentaliases.Count) { State.editorselectedalias = 0; }

                        State.editorcurrentalias = State.editorcurrentaliases[State.editorselectedalias];

                        comboBoxAlias.Text = State.editorcurrentalias;
                    }
                }
                catch
                {
                }
            }

            private void EditorAddNewAlias(object sender, RoutedEventArgs e)
            {
                try
                {
                    if (State.editorcurrentalias != null)
                    {
                        string newalias = Helpers.Common.TrimmedString(Helpers.Common.RemoveIllegalCharsForDB(State.editorcurrentalias));

                        while (Aliases.master.ContainsKey(newalias))
                        {
                            newalias = newalias + "1";
                        }

                        Aliases.categories[State.editorcurrentsourcetable].Add(newalias, State.editorcurrentkeyword);
                        Aliases.BuildNewMasterTable();
                        comboBoxAlias.ItemsSource = null;
                        comboBoxAlias.ItemsSource = Aliases.master;
                        comboBoxAlias.Text = newalias;
                        aliascounter.Text = State.aliascount.ToString();
                        Getaliascount();
                        Message.Text = "New alias added for " + Labels.categories[State.editorcurrentsourcetable][State.editorcurrentkeyword] + ": '" + newalias + "'";
                        State.activeconfig.Editorunsavedchanges = true;
                        Reflectunsavedchanges();

                        // select next
                        State.editorselectedalias = State.editorselectedalias + 1;
                        if (State.editorselectedalias >= State.editorcurrentaliases.Count) { State.editorselectedalias = 0; }

                        State.editorcurrentalias = State.editorcurrentaliases[State.editorselectedalias];

                        comboBoxAlias.Text = State.editorcurrentalias;

                    }
                    else
                    {
                        Message.Text = "(first select a command phrase)";
                    }

                }
                catch
                {
                }
            }
            private void comboBoxAlias_TextRevert(object sender, RoutedEventArgs e)
            {
                try
                {
                    string previousstate = comboBoxAlias.Text;
                    comboBoxAlias.Text = State.editorcurrentalias;
                    Log.Write("Command phrase reset.", Static.Colors.Text);
                    Message.Text = "Command phrase '" + previousstate + "' reset to '" + State.editorcurrentalias + "'";

                }
                catch
                {
                }
            }

            private void comboBoxAlias_TextUpdate(object sender, RoutedEventArgs e)
            {
                try
                {
                    State.editorcurrenttext = Helpers.Common.TrimmedString(Helpers.Common.RemoveIllegalCharsForDB(comboBoxAlias.Text));
                    string message;


                    if (State.editorcurrenttext != "")
                    {
                        try
                        {
                            Aliases.categories[State.editorcurrentsourcetable].Add(State.editorcurrenttext, State.editorcurrentkeyword);
                            Aliases.categories[State.editorcurrentsourcetable].Remove(State.editorcurrentalias);
                            message = "Changed " + State.editorcurrentalias + " to " + State.editorcurrenttext;
                            Message.Text = message;
                            Log.Write(message, StaticColors.Text);
                            Aliases.BuildNewMasterTable();
                            comboBoxAlias.ItemsSource = null;
                            comboBoxAlias.ItemsSource = Aliases.master;
                            comboBoxAlias.Text = State.editorcurrenttext;
                            State.activeconfig.Editorunsavedchanges = true;
                            Reflectunsavedchanges();
                        }
                        catch (Exception)
                        {
                            message = "Command phrase '" + State.editorcurrenttext + "' is already in use!";
                            Message.Text = message;
                            Log.Write(message, StaticColors.Text);
                        }
                    }
                    else
                    {
                        message = "(first enter a new command phrase)";
                        Message.Text = message;
                        Log.Write(message, StaticColors.Text);
                    }

                }
                catch
                {
                }
            }

            public void ReloadDB()
            {
                try
                {
                    FileHandler.Database.ReadAllCategoriesFromFile();
                    Server.SetImportedATCsAsRecipients();
                    Aliases.BuildNewMasterTable();
                    Labels.BuildNewMasterTable();

                    Message.Text = "Keywords database restored to last saved version.";

                    comboBoxAlias.ItemsSource = null;
                    comboBoxAlias.ItemsSource = Aliases.master;
                    aliascounter.Text = State.aliascount.ToString();
                    comboBoxAlias.Text = State.editorcurrentalias;
                    Getaliascount();

                    State.activeconfig.Editorrequiresreload = false;
                    Reflectunsavedchanges();
                    Reflectrequiresreload();
                }
                catch
                {
                }
            }

            private void KeywordsReloadOriginalDB(object sender, RoutedEventArgs e)
            {
                ReloadDB();
            }

            public void comboBoxAlias_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                string foundcommand = "----";
                string foundcategory = "";
                string maincat = "";

                try
                {

                    State.editorcurrentalias = ((KeyValuePair<string, string>)comboBoxAlias.SelectedItem).Key;
                    State.editorcurrentkeyword = ((KeyValuePair<string, string>)comboBoxAlias.SelectedItem).Value;
                    State.editorcurrentindex = comboBoxAlias.SelectedIndex;

                    State.editorcurrentsourcetable = "";
                    foreach (KeyValuePair<string, Dictionary<string, string>> subset in Aliases.categories)
                    {
                        Dictionary<string, string> set = subset.Value;
                        if (set.ContainsValue(State.editorcurrentkeyword))
                        {
                            State.editorcurrentsourcetable = subset.Key;
                        }
                    }

                    Getaliascount();

                    Labels.master.TryGetValue(comboBoxAlias.SelectedValue.ToString(), out foundcommand);


                    if (Commands.Table.ContainsKey(comboBoxAlias.SelectedValue.ToString()))
                    {
                        foundcategory = Commands.Table[comboBoxAlias.SelectedValue.ToString()].RecipientClass().Name;
                        if (Commands.Table[comboBoxAlias.SelectedValue.ToString()].dcsid.StartsWith("wMsgGeorge", StringComparison.OrdinalIgnoreCase))
                        {
                            foundcategory = "CPG";
                        }
                        if (EditorCatLabels.Commands.ContainsKey(Commands.Table[comboBoxAlias.SelectedValue.ToString()].category))
                        {
                            maincat = EditorCatLabels.Commands[Commands.Table[comboBoxAlias.SelectedValue.ToString()].category];
                        }
                    }
                    if (Recipients.Table.ContainsKey(comboBoxAlias.SelectedValue.ToString()))
                    {
                        foundcategory = Recipients.Table[comboBoxAlias.SelectedValue.ToString()].RecipientClass().Name;
                        if (EditorCatLabels.Recipients.ContainsKey(Recipients.Table[comboBoxAlias.SelectedValue.ToString()].category))
                        {
                            maincat = EditorCatLabels.Recipients[Recipients.Table[comboBoxAlias.SelectedValue.ToString()].category];
                        }
                    }
                    if (Aliases.appendiceswpn.ContainsValue(comboBoxAlias.SelectedValue.ToString()))
                    {
                        maincat = "(Engage) Appendix Weapon";
                    }
                    if (Aliases.appendicesdir.ContainsValue(comboBoxAlias.SelectedValue.ToString()))
                    {
                        maincat = "(Engage) Appendix Direction";
                    }
                    if (Aliases.aicues.ContainsValue(comboBoxAlias.SelectedValue.ToString()))
                    {
                        maincat = "Cue Words";
                    }
                    if (Aliases.playercallsigns.ContainsValue(comboBoxAlias.SelectedValue.ToString()))
                    {
                        maincat = "Player Group Callsigns";
                    }
                    if (Aliases.importedatcs.ContainsValue(comboBoxAlias.SelectedValue.ToString()))
                    {
                        maincat = "Imported Theaters";
                    }
                    if (Aliases.importedmenus.ContainsValue(comboBoxAlias.SelectedValue.ToString()))
                    {
                        maincat = "Imported F10 Menu items";
                    }
                    if (Aliases.cockpitcontrol.ContainsValue(comboBoxAlias.SelectedValue.ToString()))
                    {
                        maincat = "Cockpit Device Control";
                    }

                    if (foundcategory == "")
                    {
                        showcommandname.Text = foundcommand;
                        showcategoryname.Text = maincat;
                    }
                    else
                    {
                        showcommandname.Text = "(" + foundcategory + ") " + foundcommand;
                        showcategoryname.Text = maincat;
                    }
                }
                catch
                {
                }
            }

            private void EnableMicIcon(object sender, EventArgs e)
            {
                Microphone.IsEnabled = true;
                Microphone.Visibility = Visibility.Visible;
            }
            private void DisableTrainIcon(object sender, EventArgs e)
            {
                Microphone_train.IsEnabled = false;
                Microphone_train.Visibility = Visibility.Hidden;
            }

            private void StartMSVR(object sender, MouseButtonEventArgs e)
            {
                if (Microphone.IsEnabled)
                {
                    Microphone.IsEnabled = false;
                    Microphone.Visibility = Visibility.Hidden;
                    Message.Text = "Keywords Training Mode is active.";
                    Microphone_train.IsEnabled = true;
                    Microphone_train.Visibility = Visibility.Visible;
                }
                else
                {
                    Message.Text = "";
                    Microphone.IsEnabled = true;
                    Microphone.Visibility = Visibility.Visible;
                    Microphone_train.IsEnabled = false;
                    Microphone_train.Visibility = Visibility.Hidden;
                }
                AliasEditor.TrainingStartStop();
            }

            private void FinishKeywords(object sender, RoutedEventArgs e)
            {
                try
                {
                    string keywords = FileHandler.Database.ExportMasterKeywordString();
                    if (!string.IsNullOrEmpty(keywords))
                    {
                        FileHandler.Database.WriteAllCategoriesToFile(true);
                        // Copy the keywords to the Windows clipboard
                        System.Windows.Clipboard.SetDataObject(keywords);
                        // Set the keywords into the variable used in the VoiceAttack profile
                        State.Proxy.SetText("vaicom.keywords", keywords, true);
                        Message.Text = "VoiceAttack profile keywords updated.";
                        string caption = "VAICOM database keywords updated";
                        string message = "The VoiceAttack profile has been updated with the VAICOM keywords. The profile will be automatically reloaded.\n";
                        System.Windows.MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Information);
                        Message.Text = "";
                        State.activeconfig.Editorunsavedchanges = false;
                        Settings.ConfigFile.WriteConfigToFile(true);
                        Reflectunsavedchanges();
                        // Reload the current profile to pick up the variable change
                        State.Proxy.Profile.Reset();
                    }
                    else
                    {
                        Log.Write("No keywords located in database", Colors.Warning);
                    }
                }
                catch
                {
                }
            }

            private void ExportCSV(object sender, RoutedEventArgs e)
            {

                if (FileHandler.Database.ExportKeywordsAsCSV())
                {
                    Message.Text = "keywords.csv file published to Export folder.";
                }
                else
                {
                    Message.Text = "there was an error exporting the .csv file.";
                }

            }
            private void ImportCSV(object sender, RoutedEventArgs e)
            {

                if (FileHandler.Database.ImportCSVAsKeywords())
                {

                    Message.Text = "The file was imported. Use FINISH steps to update the VA profile.";
                    State.activeconfig.Editorunsavedchanges = true;
                    Settings.ConfigFile.WriteConfigToFile(true);

                    // reload db
                    FileHandler.Database.ReadAllCategoriesFromFile();
                    Server.SetImportedATCsAsRecipients();
                    Aliases.BuildNewMasterTable();
                    Labels.BuildNewMasterTable();
                    comboBoxAlias.ItemsSource = null;
                    comboBoxAlias.ItemsSource = Aliases.master;
                    aliascounter.Text = State.aliascount.ToString();
                    comboBoxAlias.Text = State.editorcurrentalias;
                    Getaliascount();

                    Reflectunsavedchanges();
                }
                else
                {
                    Message.Text = "No keywords were imported from file.";
                }


            }
            private void Toggle_RIO_DB_On_Init(object sender, EventArgs e)
            {

            }
            private void Toggle_RIO_DB_Off_Init(object sender, EventArgs e)
            {

            }
            private void Toggle_Expander_On(object sender, RoutedEventArgs e)
            {
                ExportCSVbutton.Visibility = Visibility.Hidden;
                ImportCSVbutton_Add.Visibility = Visibility.Visible;
            }
            private void Toggle_Expander_Off(object sender, RoutedEventArgs e)
            {
                ExportCSVbutton.Visibility = Visibility.Visible;
                ImportCSVbutton_Add.Visibility = Visibility.Hidden;
            }
            private void Import_Init(object sender, EventArgs e)
            {
                ImportCSVbutton_Add.IsEnabled = true;

            }
            private void Test_VA_Profile(object sender, MouseButtonEventArgs e)
            {
                UI.Playsound.Commandcomplete();
                Message.Text = "Inspecting profile..";

                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    string result = FileHandler.Root.CheckProfileContent(false);
                    if (State.configwindowopen && (State.configurationwindow != null))
                    {
                        State.configurationwindow.Dispatcher.BeginInvoke((MethodInvoker)delegate
                        {
                            Message.Text = result;
                            Reflectunsavedchanges();
                        });
                    }


                }).Start();


            }


            // ------------------------------------------------------------

        }
    }
}



