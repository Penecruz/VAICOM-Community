using System;
using System.Collections.Generic;
using System.IO;
using VAICOM.Servers;
using VAICOM.Static;

namespace VAICOM
{
    namespace FileManager
    {

        public static partial class FileHandler
        {
            public static partial class Lua
            {

                public static void Remove_DCS_Theme()
                {
                    string UserSavedGamesFolder = Framework.SpecialFoldersRetrieve.GetSavedGames();

                    foreach (KeyValuePair<string, string> set in Server.dcsfolders)
                    {
                        string path = UserSavedGamesFolder + "\\" + Server.dcsversion[set.Key];
                        string installpath = path + "\\Mods\\tech\\VAICOMPRO\\";
                        if (Directory.Exists(installpath))
                        {
                            try
                            {
                                Log.Write("Deleting folder " + installpath, Colors.Inline);
                                Directory.Delete(installpath, true);
                            }
                            catch (Exception e)
                            {
                                Log.Write("Theme Delete failed " + e.Message, Colors.Inline);
                            }
                        }
                        else
                        {
                            Log.Write("Not found: " + installpath, Colors.Inline);
                        }
                    }
                }


                public static void Install_DCS_Theme(string path)
                {

                    try
                    {
                        string installpath = path + "\\Mods\\tech\\VAICOMPRO\\";
                        string optionspath = installpath + "Options" + "\\";
                        string themepath = installpath + "Theme" + "\\";
                        string MEpath = themepath + "ME" + "\\";

                        if (!Directory.Exists(installpath))
                        {
                            Directory.CreateDirectory(installpath);
                        }

                        if (!Directory.Exists(optionspath))
                        {
                            Directory.CreateDirectory(optionspath);
                        }

                        if (!Directory.Exists(themepath))
                        {
                            Directory.CreateDirectory(themepath);
                        }

                        if (!Directory.Exists(MEpath))
                        {
                            Directory.CreateDirectory(MEpath);
                        }


                        byte[] resource;
                        string resourcestring;
                        string file;

                        // entry lua

                        resourcestring = Properties.Resources.entry;
                        file = installpath + "entry.lua";
                        File.WriteAllText(file, resourcestring);

                        // theme icons

                        resource = Properties.Resources.icon;
                        file = themepath + "icon.png";
                        File.WriteAllBytes(file, resource);

                        resource = Properties.Resources.icon_active;
                        file = themepath + "icon_active.png";
                        File.WriteAllBytes(file, resource);

                        resource = Properties.Resources.icon_select;
                        file = themepath + "icon_select.png";
                        File.WriteAllBytes(file, resource);

                        resource = Properties.Resources.icon_buy;
                        file = themepath + "icon_buy.png";
                        File.WriteAllBytes(file, resource);

                        resource = Properties.Resources.icon_76x76;
                        file = themepath + "icon 76x76.png";
                        File.WriteAllBytes(file, resource);

                        resource = Properties.Resources.icon_38x38;
                        file = themepath + "icon-38x38.png";
                        File.WriteAllBytes(file, resource);

                        // Theme background

                        resource = Properties.Resources.MainMenulogo;
                        file = MEpath + "MainMenulogo.png";
                        File.WriteAllBytes(file, resource);

                        resource = Properties.Resources.base_menu_window;
                        file = MEpath + "base-menu-window.png";
                        File.WriteAllBytes(file, resource);

                        // options files

                        resourcestring = Properties.Resources.options;
                        file = optionspath + "options.dlg";
                        File.WriteAllText(file, resourcestring);

                        resourcestring = Properties.Resources.optionsData;

                        string insert;
                        insert = "VAICOM Community Edition - interactive voice control plugin";
                        resourcestring = resourcestring.Replace("$HEADER$", insert);
                        insert = "Version: " + State.pluginversionnumber;
                        resourcestring = resourcestring.Replace("$VERSION$", insert);
                        insert = "License: " + State.currentlicense;
                        resourcestring = resourcestring.Replace("$LICENSE$", insert);

                        insert = "VAICOMPRO.dll " + State.dll_version_plugin;
                        resourcestring = resourcestring.Replace("$DLLPLUGIN$", insert);
                        insert = "Chatter.dll " + State.dll_version_chatter;
                        resourcestring = resourcestring.Replace("$DLLCHATTER$", insert);
                        insert = "AIRIO.dll " + State.dll_version_rio;
                        resourcestring = resourcestring.Replace("$DLLRIO$", insert);

                        insert = "Client IP config: ";
                        resourcestring = resourcestring.Replace("$CLIENTIPTXT$", insert);

                        insert = "IP config: ";
                        if (!State.activeconfig.UseRemoteIP)
                        {
                            insert += "127.0.0.1";
                        }
                        else
                        {
                            insert += State.activeconfig.ClientSendIP;
                        }
                        resourcestring = resourcestring.Replace("$IPCONFIG$", insert);

                        insert = "DISCLAIMER: VAICOM Community Edition is not an official ED partner module. Eagle Dynamics cannot be contacted with questions in relation to this software.";
                        resourcestring = resourcestring.Replace("$FOOTER$", insert);

                        file = optionspath + "optionsData.lua";
                        File.WriteAllText(file, resourcestring);

                        resourcestring = Properties.Resources.optionsDb;
                        file = optionspath + "optionsDb.lua";
                        File.WriteAllText(file, resourcestring);

                    }
                    catch (Exception a)
                    {
                        Log.Write("DCS theme installation: " + a.Message + a.StackTrace, Colors.Warning);
                    }
                }
            }
        }
    }
}

