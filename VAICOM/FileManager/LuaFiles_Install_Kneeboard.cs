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

                public static void LuaFiles_Install_Kneeboard(bool restore, bool forcequiet)
                {

                    // install kneeboard for all DCS versions
                    foreach (KeyValuePair<string, string> set in Server.dcsfolders)
                    {

                        // paths for this DCS version
                        string dcsprogramfolder = "";
                        string SavedGamesFolder = "";

                        // ------ device_init ---------------------------------------------------------------------
                        // first, manipulate device_init files for all aircraft modules

                        try
                        {

                            // Determine the DCS program folder
                            string dcs_path = "NOT INSTALLED";

                            if (set.Key.Equals("2.9"))
                            {
                                dcs_path = State.dcspath_release;
                            }
                            else if (set.Key.Equals("2.9 OpenBeta"))
                            {
                                dcs_path = State.dcspath_openbeta;
                            }
                            else if (set.Key.Equals("STEAM"))
                            {
                                dcs_path = State.dcspath_steam;
                            }

                            // Validate dcs_path
                            if (string.IsNullOrEmpty(dcs_path) || dcs_path.Equals("NOT INSTALLED"))
                            {
                                Log.Write($"Invalid DCS path for version {set.Key}. Skipping file installation. If you sure you have this version? Then manually assign the path in the Config Tab of the Vaicom UI.", Colors.Recognition);
                                continue;
                            }

                            // Set program folder paths
                            dcsprogramfolder = dcs_path;
                            SavedGamesFolder = Framework.SpecialFoldersRetrieve.GetSavedGames() + "\\" + Server.dcsversion[set.Key];

                            // Debug logging
                            Log.Write($"dcsprogramfolder: {dcsprogramfolder}", Colors.Recognition);
                            Log.Write($"SavedGamesFolder: {SavedGamesFolder}", Colors.Recognition);

                            // ----------------------

                            string modsubfolder = "Mods\\aircraft";

                            string[] filesinrootdir = new string[0];

                            try
                            {
                                filesinrootdir = Directory.GetDirectories(dcs_path + "\\" + modsubfolder);
                            }
                            catch
                            {
                            }

                            string[] filesinsavedgames = new string[0];
                            try
                            {
                                filesinsavedgames = Directory.GetDirectories(SavedGamesFolder + "\\" + modsubfolder);        
                            }
                            catch
                            {
                            }

                            List<string> filepaths = new List<string>();
                            filepaths.AddRange(filesinrootdir);
                            filepaths.AddRange(filesinsavedgames);

                            string lookforfile = "device_init.lua";

                            List<string> initsubfolders = new List<string>();

                            initsubfolders.Add("Cockpit");
                            initsubfolders.Add("Cockpit\\Scripts");
                            initsubfolders.Add("Cockpit\\KneeboardLeft");
                            initsubfolders.Add("Cockpit\\KneeboardRight");
                            initsubfolders.Add("Cockpit\\C-101CC");
                            initsubfolders.Add("Cockpit\\C-101EB");
                            initsubfolders.Add("Cockpit\\A10A");
                            initsubfolders.Add("Cockpit\\A10C_2");
                            initsubfolders.Add("Cockpit\\Mirage-F1\\Mirage-F1BE");
                            initsubfolders.Add("Cockpit\\Mirage-F1\\Mirage-F1CE");
                            initsubfolders.Add("Cockpit\\Mirage-F1\\Mirage-F1EE");

                            foreach (string subdir in filepaths) // both root and Saved Games
                            {

                                string initfile = "";

                                foreach (string deepsubdir in initsubfolders)
                                {
                                    string initpath = subdir + "\\" + deepsubdir;

                                    if (Directory.Exists(initpath) && File.Exists(initpath + "\\" + lookforfile))
                                    {
                                        initfile = initpath + "\\" + lookforfile;
                                    }

                                    if (!initfile.Equals(""))
                                    {

                                        try
                                        {
                                            // have init file for mod, append if needed.

                                            string matchstring = Properties.Resources.Append_Kneeboard_device_init;
                                            string modinitfilecontent = File.ReadAllText(initfile);

                                            int desiredentryoccurrences = 0;
                                            if (State.activeconfig.Kneeboard_Enabled)
                                            {
                                                desiredentryoccurrences = 1;
                                            }

                                            if (restore)
                                            {
                                                desiredentryoccurrences = 0;
                                            }

                                            int matchoccurences = (modinitfilecontent.Length - modinitfilecontent.Replace(matchstring, "").Length) / matchstring.Length;

                                            if (matchoccurences < desiredentryoccurrences) //not enough, need to add fresh entry
                                            {
                                                string writestring = matchstring; //"\n" + 

                                                using (StreamWriter writer = new StreamWriter(initfile, true))
                                                {
                                                    writer.Write(writestring);
                                                }

                                                if (!forcequiet)
                                                {
                                                    Log.Write("   Reset: " + initfile, Colors.Inline);
                                                }
                                            }
                                            else
                                            {
                                                if (matchoccurences > desiredentryoccurrences) // need to do some repair here
                                                {
                                                    modinitfilecontent = modinitfilecontent.Replace(matchstring, ""); // rebuild

                                                    if (!restore)
                                                    {
                                                        modinitfilecontent = modinitfilecontent + "\n" + matchstring;
                                                    }

                                                    string writestring = modinitfilecontent;

                                                    using (StreamWriter writer = new StreamWriter(initfile, false))
                                                    {
                                                        writer.Write(writestring);
                                                    }

                                                    if (!forcequiet)
                                                    {
                                                        Log.Write("   Repaired: " + initfile, Colors.Inline);
                                                    }
                                                }
                                                else // ok, desired number of entry occurrences
                                                {
                                                    if (!forcequiet)
                                                    {
                                                        Log.Write("   Unchanged: " + initfile, Colors.Inline);
                                                    }
                                                }
                                            }

                                        }
                                        catch (Exception e)
                                        {
                                            Log.Write(e.Message + "\n" + e.StackTrace, Colors.Inline);
                                        }
                                    }

                                }
                            }
                        }
                        catch (Exception e)
                        {
                            if (!forcequiet)
                            {
                                Log.Write(e.Message, Colors.Text);
                            }
                        }

                        // device_init files done.
                        // ------ device_init ---------------------------------------------------------------------

                        // now loop through all other lua files for kneeboard

                        foreach (KeyValuePair<string, Server.LuaFile> serverfile in LuaFiles)
                        {

                            // get lua file object

                            Server.LuaFile thisfile = serverfile.Value;

                            //skip non-kneeboard files
                            if (!thisfile.kneeboard)
                            {
                                continue;
                            }

                            // construct base install path

                            string basepath;
                            string path;

                            // determine root/SavedGames target location

                            // Construct basepath
                            if (thisfile.root) // to DCS program folder
                            {
                                basepath = dcsprogramfolder + "\\" + thisfile.installfolder;
                            }
                            else // to Saved Games
                            {
                                basepath = SavedGamesFolder + "\\" + thisfile.installfolder;
                            }

                            // Validate basepath
                            if (string.IsNullOrEmpty(basepath) || basepath.StartsWith("\\"))
                            {
                                Log.Write($"Invalid basepath: {basepath}. Skipping directory creation.", Colors.Warning);
                                continue;
                            }

                            //Log.Write($"SavedGamesFolder: {SavedGamesFolder}", Colors.Warning); //turned off for now
                            //Log.Write($"dcsprogramfolder: {dcsprogramfolder}", Colors.Warning);
                            //Log.Write($"basepath: {basepath}", Colors.Warning);

                            // create the folder if doesn't exit yet..

                            if (!Directory.Exists(basepath))
                            {
                                if (!restore)
                                {
                                    Directory.CreateDirectory(basepath);
                                }
                            }

                            // construct file path for this file

                            path = basepath + "\\" + thisfile.filename;

                            // ----- remove the file first ----

                            if (File.Exists(path + ".old"))
                            {
                                File.Delete(path + ".old");
                            }
                            if (File.Exists(path))
                            {
                                // make a backup copy if there's already some other 1.lua there
                                if (thisfile.filename.Equals("1.lua") && !File.ReadAllText(path).Equals(thisfile.source))
                                {
                                    File.Move(path, basepath + "\\" + "1.old.lua");
                                }
                                File.Delete(path);
                            }

                            // -----  write the new file -----------------------------------------

                            if (thisfile.binary)
                            {
                                byte[] writestring = thisfile.binarysource;
                                if (!restore)
                                {
                                    File.WriteAllBytes(path, writestring);
                                }

                            }
                            else // text files 
                            {

                                string writestring = "";

                                if (thisfile.reset || restore)
                                {
                                    writestring = thisfile.orig;
                                }
                                else // new write
                                {
                                    writestring = thisfile.source;
                                }

                                if (!restore)
                                {
                                    // write the file
                                    using (StreamWriter writer = new StreamWriter(path, false))
                                    {
                                        writer.Write(writestring); // don't restore for kneeboard files                              
                                    }
                                }

                            }

                            if (!forcequiet)
                            {
                                Log.Write("   Reset: " + thisfile.filename, Colors.Inline);
                            }

                        }
                    }
                }
            }
        }
    }
}

