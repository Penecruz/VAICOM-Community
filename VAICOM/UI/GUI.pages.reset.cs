using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;
using VAICOM.Database;
using VAICOM.FileManager;
using VAICOM.Servers;
using VAICOM.Static;


namespace VAICOM
{
    namespace UI
    {

        public partial class ConfigWindow : Window
        {

            // ------RESET PAGE -------------------------------------------

            private void Resetimportedon(object sender, RoutedEventArgs e) { State.activeconfig.Resetimported = true; }
            private void Resetimportedoff(object sender, RoutedEventArgs e) { State.activeconfig.Resetimported = false; }
            private void SetCurrentValueResetimported(object sender, EventArgs e) { clearimported.IsChecked = State.activeconfig.Resetimported; }

            private void resetdbon(object sender, RoutedEventArgs e) { State.activeconfig.Resetdb = true; }
            private void resetdboff(object sender, RoutedEventArgs e) { State.activeconfig.Resetdb = false; }
            private void SetCurrentValueResetDb(object sender, EventArgs e) { cleardb.IsChecked = State.activeconfig.Resetdb; }

            private void resetoptionson(object sender, RoutedEventArgs e) { State.activeconfig.Resetoptions = true; }
            private void resetoptionsoff(object sender, RoutedEventArgs e) { State.activeconfig.Resetoptions = false; }
            private void SetCurrentValueResetOptions(object sender, EventArgs e) { clearsettings.IsChecked = State.activeconfig.Resetoptions; }

            private void resetprofileon(object sender, RoutedEventArgs e) { State.activeconfig.Resetprofile = true; }
            private void resetprofileoff(object sender, RoutedEventArgs e) { State.activeconfig.Resetprofile = false; }
            private void SetCurrentValueResetProfile(object sender, EventArgs e) { clearprofile.IsChecked = State.activeconfig.Resetprofile; }

            private void resetthemeon(object sender, RoutedEventArgs e) { State.activeconfig.Resettheme = true; }
            private void resetthemeoff(object sender, RoutedEventArgs e) { State.activeconfig.Resettheme = false; }
            private void SetCurrentValueResetTheme(object sender, EventArgs e) { cleartheme.IsChecked = State.activeconfig.Resettheme; }

            private void resetluaon(object sender, RoutedEventArgs e) { State.activeconfig.Resetlua = true; }
            private void resetluaoff(object sender, RoutedEventArgs e) { State.activeconfig.Resetlua = false; }
            private void SetCurrentValueResetLua(object sender, EventArgs e) { clearlua.IsChecked = State.activeconfig.Resetlua; }

            private void RepairLuas(object sender, RoutedEventArgs e)
            {
                try
                {
                    string caption = "Repair Lua files";
                    string message = "LUA repair:\nOriginal lua files have been restored,\nVAICOM PRO lua code was re-installed.\n";
                    MessageBoxResult selectedchoice = System.Windows.MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch
                {
                }
            }
            //

            private void ResetToFactoryDefaults(object sender, RoutedEventArgs e)
            {
                try
                {
                    string caption = "Master Reset";
                    string message = "FACTORY DEFAULTS\n\nPressing OK will reset the selected settings\nto their factory defaults.\n\nAfter reset you will need to restart VoiceAttack.\nAre you sure you want to proceed?\n";
                    MessageBoxResult selectedchoice = MessageBox.Show(message, caption, MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    if (selectedchoice.Equals(MessageBoxResult.OK))
                    {
                        // execute

                        State.activeconfig.Debugmode = true;

                        if (State.activeconfig.Resetprofile)
                        {
                            try
                            {
                                // replace profile file 
                                FileHandler.Root.CheckProfile(true);
                                Log.Write("VA profile file restored.", Static.Colors.Text);
                            }
                            catch
                            {
                            }
                        }

                        if (State.activeconfig.Resetdb)
                        {
                            try
                            {
                                // clear / restore Database folder
                                FileHandler.Root.DeleteDatabaseFolder();
                                FileHandler.Root.CheckSubFolders();
                                // put db
                                Clearkeywordsdb();
                                Aliases.ResetDatabase();
                                Labels.ResetDatabase();
                                Aliases.BuildNewMasterTable();
                                Labels.BuildNewMasterTable();
                                State.activeconfig.Editorunsavedchanges = true;
                                State.activeconfig.Editorrequiresreload = true;
                                Reflectunsavedchanges();
                                Reflectrequiresreload();
                                Log.Write("Full Database cleared.", Static.Colors.Text);
                            }
                            catch
                            {
                            }
                        }

                        if (State.activeconfig.Resetimported)
                        {
                            try
                            {
                                // Check the Folders for the DB
                                FileHandler.Root.CheckSubFolders();
                                //Clear imported encrypted DB, remove imported aliases and rebuild master table
                                ClearImporteddb();                                
                                FileHandler.Database.WriteCategoryToFile("importedatcs", Aliases.importedatcs, true);
                                FileHandler.Database.WriteCategoryToFile("importedmenus", Aliases.importedmenus, true);
                                FileHandler.Database.WriteAuxMenuItemsToFile(true);
                                Aliases.ResetImported();
                                Aliases.BuildNewMasterTable();
                                State.activeconfig.Editorunsavedchanges = true;
                                State.activeconfig.Editorrequiresreload = true;
                                Reflectunsavedchanges();
                                Reflectrequiresreload();
                                Log.Write("Imported Menu Items and ATC Recipients cleared.", Static.Colors.Text);
                            }
                            catch
                            {
                            }
                        }

                        if (State.activeconfig.Resettheme)
                        {
                            FileHandler.Lua.Remove_DCS_Theme();
                        }

                        if (State.activeconfig.Resetlua)
                        {
                            try
                            {
                                FileHandler.Lua.LuaFiles_Remove(); // <-- Remove all export.lua VAICOM blocks (current and legacy)
                                FileHandler.Lua.LuaFiles_Install(true, true); // reset quietly;
                                State.datawasreset = true;

                                Log.Write("Lua code cleared.", Static.Colors.Text);
                            }
                            catch
                            {
                            }
                        }

                        if (State.activeconfig.Resetoptions)
                        {
                            try
                            {

                                FileHandler.Root.DeleteConfigFolder();
                                FileHandler.Root.CheckSubFolders();
                                State.activeconfig = Settings.Configs.Defaultconfig;
                                Settings.ConfigFile.WriteConfigToFile(false);
                                State.activeconfig = Settings.ConfigFile.ReadConfigFromFile();
                                State.activeconfig.Debugmode = true;
                                Log.Write("Config restored to default.", Static.Colors.Text);
                            }
                            catch
                            {
                            }
                        }

                        string readymessage = "Data cleared.\nYou must now restart VoiceAttack.\n";
                        MessageBoxResult finish = MessageBox.Show(readymessage, caption, MessageBoxButton.OK, MessageBoxImage.Warning);

                    }
                }
                catch
                {
                }
            }
            private void removeallactivelicenses()
            {
                try
                {
                    RegistryKey getkey = Registry.CurrentUser.OpenSubKey(Products.Products.Families.Vaicom.RegKeyBase, true);
                    if (getkey != null)
                    {
                        RegistryKey testkey = Registry.CurrentUser.OpenSubKey(Products.Products.Families.Vaicom.RegKeyRoot);
                        if (testkey != null)
                        {
                            string keyname = "VAICOMPRO";
                            Log.Write("Deleting reg key for " + keyname, Static.Colors.System);
                            getkey.DeleteSubKeyTree(keyname, true);
                        }
                    }
                    getkey.Close();
                }
                catch (Exception a)
                {
                    Log.Write(a.Message, Static.Colors.System);
                }

                Products.Products.CheckActiveLicenses();
                resetenabledfeatures();

                string productidstring = Products.Products.Families.Vaicom.VaicomProPlugin.product_id.ToString();

                if (State.activelicenses.ContainsKey(productidstring))
                {
                    State.PRO = true;
                }
                else
                {
                    State.PRO = false;
                }

                productidstring = Products.Products.Families.Vaicom.ChatterThemePack.product_id.ToString();

                if (State.activelicenses.ContainsKey(productidstring))
                {
                    State.chatterthemesactivated = true;
                }
                else
                {
                    State.chatterthemesactivated = false;
                }

                Products.Products.UpdateClientLicense();
                showproductname();
                showprolight();

            }
            private void Clearkeywordsdb()
            {
                try
                {
                    Aliases.appendiceswpn = new Dictionary<string, string>();
                    Aliases.appendicesdir = new Dictionary<string, string>();
                    Aliases.aicues = new Dictionary<string, string>();
                    Aliases.aicommands = new Dictionary<string, string>();
                    Aliases.airecipients = new Dictionary<string, string>();
                    Aliases.cockpitcontrol = new Dictionary<string, string>();
                    Aliases.importedatcs = new Dictionary<string, string>();
                    Aliases.playercallsigns = new Dictionary<string, string>();
                    Aliases.simcontrol = new Dictionary<string, string>();
                }
                catch
                {
                }
            }

            private void ClearImporteddb()
            {
                try
                {
                    Aliases.importedatcs = new Dictionary<string, string>();
                    Aliases.importedmenus = new Dictionary<string, string>();
                    Server.auxmenuitems = new Dictionary<string, Server.MenuItem>();
                }
                catch
                {
                }
            }

            private void Test_Install(object sender, MouseButtonEventArgs e)
            {
                string caption = "VAICOM Community Edition Installation Details";
                string message = "";
                MessageBoxImage BoxImage = new MessageBoxImage();

                message += "---- VAICOM DIAGNOSTICS SUMMARY ---- \n\n";

                message += "VoiceAttack and plugin files\n\n";

                System.Version VAcurrentversion = State.Proxy.VAVersion;
                System.Version VAminversion = Version.Parse(State.vaminversion);

                message += "> VoiceAttack version: " + VAcurrentversion + " ";
                if (VAcurrentversion < VAminversion)
                {
                    message += "[OUTDATED VERSION, VA REQUIRES UPDATE]";
                }
                else
                {
                    message += "(plugin compatible)";
                }
                message += "\n";

                message += "> VA plugins path: " + State.VA_APPS + "\n";

                message += "\n";

                message += "> VAICOMPRO.dll version " + State.dll_version_plugin + " ";

                if (State.updateavailable_plugin)
                {
                    message += "[UPDATE AVAILABLE]";
                }
                else
                {
                    message += "(up to date)";
                }

                message += "\n";

                if (State.dll_installed_chatter)
                {
                    message += "> Chatter.dll version " + State.dll_version_chatter + " ";

                    if (State.updateavailable_chatter)
                    {
                        message += "[UPDATE AVAILABLE]";
                    }
                    else
                    {
                        message += "(up to date)";
                    }

                    message += "\n";

                }
                else
                {
                    message += "> Chatter.dll [NOT INSTALLED]\n";
                }

                if (State.dll_installed_rio)
                {
                    message += "> AIRIO.dll version " + State.dll_version_rio + " ";

                    if (State.updateavailable_rio)
                    {
                        message += "[UPDATE AVAILABLE]";
                    }
                    else
                    {
                        message += "(up to date)";
                    }

                    message += "\n";

                }
                else
                {
                    message += "> AIRIO.dll [NOT INSTALLED]\n";
                }

                message += "\n";

                message += "If some of these files are not up to date, get the latest versions from GitHub for manual install.\n";

                message += "\n";

                message += "DCS World installation\n\n";

                if (State.dcsinstalled)
                {
                    string UserSavedGamesFolder = Framework.SpecialFoldersRetrieve.GetSavedGames();
                    string steamfolder = Helpers.WinReg.GetSteamFolder().Replace("/", "\\") + "\\steamapps\\common\\" + "DCSWorld";

                    BoxImage = MessageBoxImage.Information;

                    message += "DCS World installations were detected on this machine." + "\n";
                    message += "The following install paths are used by the plugin:" + "\n" + "\n";

                    string pathrelease = "[NOT FOUND]";
                    if (!State.dcspath_release.Equals(""))
                    {
                        pathrelease = State.dcspath_release + "\n";
                        pathrelease += "   Saved Games folder " + UserSavedGamesFolder + "\\" + Server.dcsversion["2.9"] + "\n";
                    }
                    message += "> DCS World (release version):" + "\n";
                    message += "   " + pathrelease + "\n";

                    string pathopenbeta = "[NOT FOUND]";
                    if (!State.dcspath_openbeta.Equals(""))
                    {
                        pathopenbeta = State.dcspath_openbeta + "\n";
                        pathopenbeta += "   Saved Games folder " + UserSavedGamesFolder + "\\" + Server.dcsversion["2.9 OpenBeta"] + "\n";
                    }
                    message += "> DCS World (open beta):" + "\n";
                    message += "   " + pathopenbeta + "\n";

                    string pathsteam = "[NOT FOUND]";
                    if (!State.dcspath_steam.Equals(""))
                    {
                        pathsteam = State.dcspath_steam + "\n";
                        pathsteam += "   Saved Games folder " + UserSavedGamesFolder + "\\" + Server.dcsversion["2.9"] + "\n"; ;
                    }
                    message += "> DCS World (STEAM):" + "\n";
                    message += "   " + pathsteam + "\n";

                    message += "If some of these paths are not correct, use Custom Path option (Config page). ";
                    message += "Path should only go as far as /Eagle Dynamics/DCS folder, NOT to the .exe ";
                    message += "Optionally select Fix Registry for permanent correction." + "\n";

                }
                else
                {
                    BoxImage = MessageBoxImage.Exclamation;

                    message += "No DCS World installations were detected on this machine." + "\n";
                    message += "Use Custom Path (Config tab) to set the DCS World path." + "\n" + "\n";

                }

                MessageBox.Show(message, caption, MessageBoxButton.OK, BoxImage);
            }


            // ------------------------------------------------------------
        }
    }
}





