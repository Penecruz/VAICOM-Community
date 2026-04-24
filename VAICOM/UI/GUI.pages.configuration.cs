using Microsoft.Win32;
using System;
using System.Windows;
using VAICOM.FileManager;

namespace VAICOM
{
    namespace UI
    {

        public partial class ConfigWindow : Window
        {
            // ------------  CONFIGURATION PAGE ---------------------------

            // debug mode
            private void Setdebugmodeon(object sender, RoutedEventArgs e) { State.activeconfig.Debugmode = true; }
            private void Setdebugmodeoff(object sender, RoutedEventArgs e) { State.activeconfig.Debugmode = false; }
            private void SetCurrentValueDebugMode(object sender, EventArgs e) { RunInDebugMode.IsEnabled = true; RunInDebugMode.IsChecked = State.activeconfig.Debugmode; }
            // remote IP
            private void AutoImportATCon(object sender, RoutedEventArgs e) { State.activeconfig.Checkfornewatcs = true; }
            private void AutoImportATCoff(object sender, RoutedEventArgs e) { State.activeconfig.Checkfornewatcs = false; }
            private void SetCurrentValueAutoImportATC(object sender, EventArgs e) { AutoImportATC.IsEnabled = true; AutoImportATC.IsChecked = State.activeconfig.Checkfornewatcs; }
            // include custom folders
            private void UseCustomFolderson(object sender, RoutedEventArgs e) { State.activeconfig.UseCustomFolders = true; }
            private void UseCustomFoldersoff(object sender, RoutedEventArgs e) { State.activeconfig.UseCustomFolders = false; }
            private void SetCurrentValueUseCustomFolders(object sender, EventArgs e) { UseCustomFolders.IsEnabled = true; UseCustomFolders.IsChecked = State.activeconfig.UseCustomFolders; }

            private void CustomFoldersFixRegOn(object sender, RoutedEventArgs e)
            {
                if (State.activeconfig.Custom_Path_Setting2.Equals(0)) // standalone only
                {
                    UseCustomFixReg.IsChecked = true;
                    State.activeconfig.CustomFoldersFixReg = true;
                    WarnRegFixDialog();
                }
            }
            private void CustomFoldersFixRegOff(object sender, RoutedEventArgs e) { State.activeconfig.CustomFoldersFixReg = false; }
            private void SetCurrentValueFixRegFlag(object sender, EventArgs e) { EnableFixReg(); }
            private void SetCustomFolderFixRegFlag(object sender, RoutedEventArgs e)
            {
                if (UseCustomFixReg.IsChecked == true)
                { State.activeconfig.CustomFoldersFixReg = true; }
                else
                { State.activeconfig.CustomFoldersFixReg = false; }
                Settings.ConfigFile.WriteConfigToFile(true);
            }

            private void UpdateFoldersUse(object sender, RoutedEventArgs e)
            {
                if (UseCustomFolders.IsChecked == true)
                { State.activeconfig.UseCustomFolders = true; }
                else
                { State.activeconfig.UseCustomFolders = false; }
                Settings.ConfigFile.WriteConfigToFile(true);
            }


            public void WarnRegFixDialog()
            {
                string caption = "confirm setting";

                string message = "WARNING: Registry fixing is selected.\nThis will change the DCS installation path in Windows Registry.\nUncheck if this is not what you want.\n\nUse the sliders to set the DCS version.\nPress the SET button to browse to the DCS World program files folder.\n";

                MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Warning);

            }

            private void UpdateFolders(object sender, RoutedEventArgs e)
            {
                var pathdialog = new System.Windows.Forms.FolderBrowserDialog();

                pathdialog.Description += "Select program files folder for ";

                int versionselected;

                if (State.activeconfig.Custom_Path_Setting2.Equals(1)) // steam
                {
                    if (State.activeconfig.Custom_Path_Setting1.Equals(1))
                    {
                        versionselected = 3; // steam open beta
                    }
                    else
                    {
                        versionselected = 2; // steam release
                    }
                }
                else // standalone
                {

                    if (State.activeconfig.Custom_Path_Setting1.Equals(1))
                    {
                        versionselected = 1; // standalone open beta
                    }
                    else
                    {
                        versionselected = 0; // standalone release
                    }

                }

                string selectedversion;

                switch (versionselected)
                {
                    case 0:
                        selectedversion = "DCS World (release version)";
                        break;
                    case 1:
                        selectedversion = "DCS World (open beta)";
                        break;
                    case 2:
                        selectedversion = "DCS World (steam edition)";
                        break;
                    case 3:
                        selectedversion = "DCS World (steam open beta)";
                        break;
                    default:
                        selectedversion = "DCS World";
                        break;
                }

                pathdialog.Description += selectedversion + "\n";

                if (State.activeconfig.CustomFoldersFixReg)
                {
                    pathdialog.Description += "WARNING: Windows Registry fixing selected.";
                }

                pathdialog.ShowNewFolderButton = false;
                System.Windows.Forms.DialogResult result = pathdialog.ShowDialog();

                if (result.Equals(System.Windows.Forms.DialogResult.OK))
                {
                    State.activeconfig.DCSfoldername1 = pathdialog.SelectedPath;
                    DCSfoldername1.Text = State.activeconfig.DCSfoldername1;
                    Log.Write("Custom DCS path set to " + State.activeconfig.DCSfoldername1, Static.Colors.Warning);
                }

                // fix reg
                if (State.activeconfig.CustomFoldersFixReg)
                {
                    DoFixRegistry(selectedversion, State.activeconfig.DCSfoldername1);
                }

                // reset flag
                State.activeconfig.CustomFoldersFixReg = false;
                Settings.ConfigFile.WriteConfigToFile(true);
                ResetRegFixFlag();


            }

            public void DoFixRegistry(string versionname, string pathstring)
            {
                try
                {

                    string storekey = "";
                    string storevalue = pathstring;

                    if (versionname.Equals("DCS World (open beta)"))
                    {
                        storekey = Products.Products.Families.DCSWorld.RegKeyRoot + "DCS World OpenBeta";
                    }
                    else
                    {
                        storekey = Products.Products.Families.DCSWorld.RegKeyRoot + "DCS World";
                    }

                    RegistryKey putkey = Registry.CurrentUser.CreateSubKey(storekey);
                    putkey.SetValue("Path", storevalue);
                    putkey.Close();

                    string showmessage = "Registry entry for " + versionname + " was set to path " + pathstring;
                    Log.Write(showmessage, Static.Colors.Critical);
                    string caption = "Windows registry entry changed";
                    MessageBox.Show(showmessage, caption, MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch
                {
                }

            }

            public void ResetRegFixFlag()
            {
                UseCustomFixReg.IsChecked = false;
            }

            private void GetFolderName1(object sender, EventArgs e)
            {
                DCSfoldername1.Text = State.activeconfig.DCSfoldername1;
            }
            private void InitSetFolders(object sender, EventArgs e)
            {
                SetFolders.IsEnabled = true;
            }

            // sliders for forced dcs version

            private void Custom_Path_Setting1_Init(object sender, EventArgs e)
            {
                Custom_Path_Setting1.IsEnabled = true;
                Custom_Path_Setting1.Value = State.activeconfig.Custom_Path_Setting1;
            }
            private void Custom_Path_Setting1_Change(object sender, RoutedPropertyChangedEventArgs<double> e)
            {
                if (!State.activeconfig.Custom_Path_Setting1.Equals((int)e.NewValue))
                {
                    UI.Playsound.Soft_Click();
                    State.activeconfig.Custom_Path_Setting1 = (int)Custom_Path_Setting1.Value;
                    Settings.ConfigFile.WriteConfigToFile(true);
                }
            }

            private void Custom_Path_Setting2_Init(object sender, EventArgs e)
            {
                Custom_Path_Setting2.IsEnabled = true;
                Custom_Path_Setting2.Value = State.activeconfig.Custom_Path_Setting2;
            }
            private void Custom_Path_Setting2_Change(object sender, RoutedPropertyChangedEventArgs<double> e)
            {
                if (!State.activeconfig.Custom_Path_Setting2.Equals((int)e.NewValue))
                {
                    UI.Playsound.Soft_Click();
                    State.activeconfig.Custom_Path_Setting2 = (int)Custom_Path_Setting2.Value;
                    Settings.ConfigFile.WriteConfigToFile(true);
                    EnableFixReg();
                }
            }

            public void EnableFixReg()
            {
                UseCustomFixReg.IsChecked = false;
                UseCustomFixReg.IsEnabled = State.activeconfig.Custom_Path_Setting2.Equals(0);
            }

            // import new modules
            private void AutoImportModuleson(object sender, RoutedEventArgs e) { State.activeconfig.AutoImportModules = true; }
            private void AutoImportModulesoff(object sender, RoutedEventArgs e) { State.activeconfig.AutoImportModules = false; }
            private void SetCurrentValueAutoImportModules(object sender, EventArgs e) { AutoImportModules.IsEnabled = true; AutoImportModules.IsChecked = State.activeconfig.AutoImportModules; }

            // manual server files
            private void ManualServerFilesOn(object sender, RoutedEventArgs e) { State.activeconfig.ManualInstallServerFiles = true; }
            private void ManualServerFilesOff(object sender, RoutedEventArgs e) { State.activeconfig.ManualInstallServerFiles = false; }
            private void SetCurrentValueManualServerFiles(object sender, EventArgs e) { ManualManageServer.IsEnabled = true; ManualManageServer.IsChecked = State.activeconfig.ManualInstallServerFiles; }
            private void EnableExportServerFiles(object sender, EventArgs e)
            {
                ExportFiles.IsEnabled = true;
            }
            private void ExportServerFiles(object sender, RoutedEventArgs e)
            {
                FileHandler.Lua.LuaFiles_Export();
            }

            // ------------------------------------------------------------
        }
    }
}



