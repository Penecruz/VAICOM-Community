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

                public static void LuaFiles_Remove() // called by reset page of the Vaicom UI Reset Lua Code check box
                {

                    try
                    {


                        foreach (KeyValuePair<string, string> set in Server.dcsfolders)
                        {

                            try
                            {
                                string programfolder64 = Environment.GetEnvironmentVariable("ProgramW6432");

                                string dcsinstallfolder1 = Helpers.WinReg.GetDCSInstallFolder(set.Value);
                                string dcsinstallfolder2 = Helpers.Common.TrimmedString(State.activeconfig.DCSfoldername1); //+ set.Value;
                                string dcsinstallfolder3 = Helpers.WinReg.GetSteamFolder().Replace("/", "\\") + "\\steamapps\\common\\" + "DCSWorld";
                                string dcsinstallfolder4 = programfolder64 + "\\" + "Eagle Dynamics" + "\\" + set.Value;

                                bool installfound = false;
                                string folder = "NOT FOUND";

                                // custom folder (overrides others)
                                if (!installfound && State.activeconfig.UseCustomFolders && !dcsinstallfolder2.Equals(set.Value) && Directory.Exists(dcsinstallfolder2)) // custom
                                {
                                    if ((set.Value.Contains("OpenBeta").Equals(State.activeconfig.CustomFoldersOBFlag)))
                                    {
                                        folder = dcsinstallfolder2;
                                        installfound = true;
                                    }
                                }

                                // get from registry (preferred)
                                if (!installfound && dcsinstallfolder1 != null)
                                {
                                    folder = dcsinstallfolder1;
                                    installfound = true;
                                }

                                // steam (standard path)
                                if (!installfound && !set.Key.Contains("OpenBeta") && Directory.Exists(dcsinstallfolder3))
                                {
                                    folder = dcsinstallfolder3;
                                    installfound = true;
                                }

                                // regular install path
                                if (!installfound && Directory.Exists(dcsinstallfolder4))
                                {
                                    folder = dcsinstallfolder4;
                                    installfound = true;
                                }

                                string UserSavedGamesFolder = Framework.SpecialFoldersRetrieve.GetSavedGames();

                                if (installfound)
                                {

                                    foreach (KeyValuePair<string, Server.LuaFile> serverfile in LuaFiles)
                                    {
                                        if (serverfile.Value.canremove) // do this only for files set to canremove
                                        {

                                            Server.LuaFile thisfile = serverfile.Value;
                                            string basepath;
                                            string path;
                                            string backupVersionFolder;
                                            string targetRootPath;
                                            bool uselegacy = !set.Key.Equals("2.9 OpenBeta");

                                            ResolveLuaTargetPaths(thisfile, uselegacy, folder, UserSavedGamesFolder, Server.dcsversion[set.Key], out basepath, out path, out backupVersionFolder, out targetRootPath);

                                            if (!thisfile.filename.ToLower().Equals("export.lua"))
                                            {
                                                if (State.luahardreset && thisfile.hardreset)
                                                {
                                                    if (File.Exists(path))
                                                    {
                                                        File.Delete(path);
                                                    }

                                                    // put back original

                                                    if (thisfile.filename.ToLower().Equals("vaicompro.export.lua"))
                                                    {
                                                        Directory.Delete(basepath, true);
                                                    }
                                                    else
                                                    {
                                                        string writestring = GetLuaBaseContent(thisfile, path, uselegacy, UserSavedGamesFolder, backupVersionFolder, targetRootPath, false, "restore");

                                                        if (!string.IsNullOrEmpty(writestring))
                                                        {
                                                            using (StreamWriter writer = new StreamWriter(path, false)) { writer.Write(writestring); };
                                                        }
                                                    }

                                                }
                                            }
                                            else // export.lua
                                            {
                                                if (File.Exists(path))
                                                {
                                                    string exportfile = File.ReadAllText(path);

                                                    // Remove both current and legacy Vaicom code blocks in export.lua if present
                                                    var blocksToRemove = new (string Block, string Name)[]
                                                    {
                                                        (Properties.Resources.Exportmatch, "New Vaicom code"),
                                                        (Properties.Resources.LegacyExportmatch, "Legacy Vaicom code"),
                                                        ("local vaicomlfs = require('lfs'); dofile(vaicomlfs.writedir()..[[Scripts\\VAICOMPRO\\VAICOMPRO.export.lua]])", "Legacy Vaicom code with double backslashes"),
                                                        ("local vaicomlfs = require('lfs'); dofile(vaicomlfs.writedir()..[[Scripts\\VAICOMPRO\\VAICOMPRO.export.lua]])".Replace(@"\\", @"\"), "Legacy Vaicom code with single backslashes")
                                                    };

                                                    foreach (var blockInfo in blocksToRemove)
                                                    {
                                                        if (!string.IsNullOrEmpty(blockInfo.Block) && exportfile.Contains(blockInfo.Block))
                                                        {
                                                            exportfile = exportfile.Replace(blockInfo.Block, "");
                                                            Log.Write($"Removed {blockInfo.Name} block from export.lua.", Colors.Text);
                                                        }
                                                    }

                                                    using (StreamWriter writer = new StreamWriter(path, false))
                                                    {
                                                        writer.Write(exportfile);
                                                    }
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
                    }
                    catch
                    {
                        Log.Write("Problems were reported during removal of lua code.", Colors.Text);
                    }
                }
            }
        }
    }
}

