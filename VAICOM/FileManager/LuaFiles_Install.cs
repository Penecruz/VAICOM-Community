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

                public static void LuaFiles_Install(bool restore, bool forcequiet)
                {
                    // --- finds all DCS install locations and manipulates lua files

                    try
                    {
                        State.dcsinstalled = false;
                        int installcounter = 0;

                        foreach (KeyValuePair<string, string> set in Server.dcsfolders)
                        {
                            try
                            {
                                // ------ find DCS install folder

                                string dcsprogramfilesfolder = "NOT FOUND";

                                bool customoverride = State.activeconfig.UseCustomFolders;

                                bool installfound = false; // for this version

                                // get sliders states

                                bool sliders_standalonerelease = State.activeconfig.Custom_Path_Setting2.Equals(0) && State.activeconfig.Custom_Path_Setting1.Equals(0);
                                bool sliders_standaloneopenbeta = State.activeconfig.Custom_Path_Setting2.Equals(0) && State.activeconfig.Custom_Path_Setting1.Equals(1);
                                bool sliders_steamrelease = State.activeconfig.Custom_Path_Setting2.Equals(1) && State.activeconfig.Custom_Path_Setting1.Equals(0);
                                bool sliders_steamopenbeta = State.activeconfig.Custom_Path_Setting2.Equals(1) && State.activeconfig.Custom_Path_Setting1.Equals(1);

                                // program files folder location options

                                string dcsinstallfolder_fromreg = Helpers.WinReg.GetDCSInstallFolder(set.Value);
                                string dcsinstallfolder_custom = Helpers.Common.TrimmedString(State.activeconfig.DCSfoldername1);
                                string dcsinstallfolder_steamdefault = Helpers.WinReg.GetSteamFolder().Replace("/", "\\") + "\\steamapps\\common\\" + "DCSWorld";
                                string dcsinstallfolder_regular = Environment.GetEnvironmentVariable("ProgramW6432") + "\\" + "Eagle Dynamics" + "\\" + set.Value;

                                // handling current version

                                bool currentcycleis_standalonerelease = set.Key.Equals("2.9");
                                bool currentcycleis_standaloneopenbeta = set.Key.Equals("2.9 OpenBeta");
                                bool currentcycleis_steam = set.Key.Equals("STEAM");

                                // decide whether to place openbeta or release lua code

                                bool uselegacy = currentcycleis_standalonerelease || (currentcycleis_steam && !customoverride) || (currentcycleis_steam && customoverride && sliders_steamrelease);

                                // start with custom folder (overrides others)

                                /// CUSTOM FOLDER

                                if (!installfound && customoverride && Directory.Exists(dcsinstallfolder_custom))
                                {

                                    bool versionmatch = (sliders_standalonerelease && currentcycleis_standalonerelease) || (sliders_standaloneopenbeta && currentcycleis_standaloneopenbeta) || ((sliders_steamrelease || sliders_steamopenbeta) && currentcycleis_steam);

                                    if (versionmatch)
                                    {
                                        string addstring = "";
                                        if (currentcycleis_steam && sliders_steamopenbeta)
                                        {
                                            addstring = " (open beta)";
                                        }

                                        if (!forcequiet)
                                        {
                                            Log.Write("   Using custom install path for " + set.Key + addstring, Colors.Text);
                                        }
                                        dcsprogramfilesfolder = dcsinstallfolder_custom;
                                        installfound = true;
                                    }
                                }

                                // REGISTRY
                                // get from registry (preferred)

                                if (!installfound && !customoverride && dcsinstallfolder_fromreg != null)
                                {
                                    if (!forcequiet)
                                    {
                                        Log.Write("DCS World installation found: version " + set.Key, Colors.Text);
                                        Log.Write("   Using Registry entry for " + set.Key, Colors.Text);
                                    }
                                    dcsprogramfilesfolder = dcsinstallfolder_fromreg;
                                    if (Directory.Exists(dcsprogramfilesfolder))
                                    {
                                        installfound = true;
                                    }
                                    else if (!forcequiet)
                                    {
                                        Log.Write("   Your registry key is invalid, fix it or create a custom path instead!", Colors.Warning);
                                    }
                                }

                                // STEAM DEFAULT FOLDER
                                // steam (standard path)

                                if (!installfound && !customoverride && currentcycleis_steam && Directory.Exists(dcsinstallfolder_steamdefault))
                                {
                                    if (!forcequiet)
                                    {
                                        Log.Write("DCS World installation found: version " + set.Key, Colors.Text);
                                        Log.Write("   Using default Steam install path for " + set.Key, Colors.Text);

                                    }

                                    dcsprogramfilesfolder = dcsinstallfolder_steamdefault;
                                    if (Directory.Exists(dcsprogramfilesfolder))
                                    {
                                        installfound = true;
                                    }
                                    else if (!forcequiet)
                                    {
                                        Log.Write("   Your Steam install path is wrong, set a custom path instead!", Colors.Warning);
                                    }
                                }

                                // REGULAR PATH (FAILSAFE)
                                // regular install path

                                if (!installfound && !customoverride && Directory.Exists(dcsinstallfolder_regular))
                                {
                                    if (!forcequiet)
                                    {
                                        Log.Write("DCS World installation found: version " + set.Key, Colors.Text);
                                        Log.Write("   Using Regular install path for " + set.Key, Colors.Text);
                                    }
                                    dcsprogramfilesfolder = dcsinstallfolder_regular;
                                    if (Directory.Exists(dcsprogramfilesfolder))
                                    {
                                        installfound = true;
                                    }
                                }

                                // ELSE VERSION NOT FOUND

                                //----------------------------------------

                                // VERSION FOUND: INSTALL LUA

                                if (installfound)
                                {

                                    // ---- set some flags ----------------------------

                                    State.dcsinstalled = true;

                                    if (set.Key.Equals("2.9"))
                                    {
                                        State.dcspath_release = dcsprogramfilesfolder;
                                    }
                                    if (set.Key.Equals("2.9 OpenBeta"))
                                    {
                                        State.dcspath_openbeta = dcsprogramfilesfolder;
                                    }
                                    if (set.Key.Equals("STEAM"))
                                    {
                                        State.dcspath_steam = dcsprogramfilesfolder;
                                    }

                                    if (!forcequiet)
                                    {
                                        Log.Write("   Install path = " + dcsprogramfilesfolder, Colors.Text);
                                    }

                                    // --- not used -----------------------------------

                                    State.wingmanspeechpath = dcsprogramfilesfolder + "\\" + "Sounds\\Speech\\Sound\\ENG\\Common\\";

                                    // ----- set Saved Games folder -------------------

                                    string UserSavedGamesFolder = Framework.SpecialFoldersRetrieve.GetSavedGames();
                                    if (!forcequiet)
                                    {
                                        Log.Write("   Saved Games folder = " + UserSavedGamesFolder + "\\" + Server.dcsversion[set.Key], Colors.Text);
                                    }

                                    // ----- do LUA files -------------------
                                    // install/reset the lua files for this DCS version

                                    int updatecounter = 0;

                                    // loop through all lua files

                                    foreach (KeyValuePair<string, Server.LuaFile> serverfile in LuaFiles)
                                    {

                                        // get lua file object

                                        Server.LuaFile thisfile = serverfile.Value;

                                        //don't do kneeboard files
                                        if (thisfile.kneeboard)
                                        {
                                            continue;
                                        }

                                        // construct base install path

                                        string basepath;
                                        string path;

                                        // determine root/SG target location

                                        if (thisfile.root) // to DCS program folder
                                        {
                                            if (uselegacy)
                                            {
                                                basepath = dcsprogramfilesfolder + "\\" + thisfile.installfolder_legacy;
                                            }
                                            else
                                            {
                                                basepath = dcsprogramfilesfolder + "\\" + thisfile.installfolder;
                                            }
                                        }
                                        else // to Saved Games
                                        {
                                            basepath = UserSavedGamesFolder + "\\" + Server.dcsversion[set.Key] + "\\" + thisfile.installfolder;
                                        }

                                        // Validate basepath
                                        if (string.IsNullOrEmpty(basepath) || basepath.Length <= 3)
                                        {
                                            Log.Write($"Invalid basepath: {basepath}. Skipping directory creation.", Colors.Warning);
                                            continue;
                                        }

                                        // Debug logs for paths
                                        //Log.Write($"SavedGamesFolder: {UserSavedGamesFolder}", Colors.Text); // Tuned off for now
                                        //Log.Write($"dcsprogramfilesfolder: {dcsprogramfilesfolder}", Colors.Text);
                                        //Log.Write($"basepath: {basepath}", Colors.Text);

                                        // create folder if doesn't exit yet..

                                        if (!Directory.Exists(basepath))
                                        {
                                            Directory.CreateDirectory(basepath);
                                        }

                                        // construct file path

                                        path = basepath + "\\" + thisfile.filename;

                                        // normal file (i.e. not export.lua)

                                        if (!thisfile.filename.ToLower().Equals("export.lua"))
                                        {
                                            // ----- remove the file first (unless string replace type) ----

                                            if (State.luahardreset && thisfile.hardreset && !thisfile.stringreplace)
                                            {
                                                if (!thisfile.AIRIO || (thisfile.AIRIO && State.jesteractivated))
                                                {
                                                    if (File.Exists(path))
                                                    {
                                                        File.Delete(path);
                                                    }
                                                    if (File.Exists(path + ".old"))
                                                    {
                                                        File.Delete(path + ".old");
                                                    }
                                                }
                                            }

                                            // -----  write the new file -----------------------------------------

                                            string writestring = "";

                                            if (thisfile.AIRIO) // 
                                            {

                                                if (State.jesteractivated)
                                                {
                                                    if (thisfile.stringreplace)
                                                    {

                                                        // repair if left in legacy state

                                                        string searchstr = thisfile.stringsource;
                                                        string replacestr = thisfile.stringorig;


                                                        // write the file
                                                        try
                                                        {
                                                            writestring = File.ReadAllText(path);
                                                            writestring = writestring.Replace(searchstr, replacestr);
                                                            File.WriteAllText(path, writestring);
                                                        }
                                                        catch
                                                        {
                                                        }


                                                    }
                                                    else // normal i.e. not string findreplace: Jester page
                                                    {

                                                        if (!(State.dll_installed_rio && State.activeconfig.RIO_Enabled) || restore)
                                                        {
                                                            // AIRIO disabled: reset functions to original
                                                            writestring = thisfile.orig; // <-- this is used when RIO not enabled
                                                        }
                                                        else // normal, RIO is enabled
                                                        {

                                                            if (thisfile.append)
                                                            {
                                                                writestring = thisfile.orig;

                                                                if (!restore)
                                                                {
                                                                    writestring += "\n" + thisfile.source;
                                                                }
                                                            }
                                                            else // replace type
                                                            {
                                                                if (thisfile.reset || restore)
                                                                {
                                                                    writestring = thisfile.orig;
                                                                }
                                                                else
                                                                {
                                                                    writestring = thisfile.source;
                                                                }
                                                            }
                                                        }

                                                        try
                                                        {
                                                            using (StreamWriter writer = new StreamWriter(path, true))
                                                            {
                                                                writer.Write(writestring);
                                                            }
                                                        }
                                                        catch
                                                        {
                                                        }
                                                    }
                                                }
                                                else // Jester not activated, no action
                                                {
                                                }
                                            }
                                            else //not AIRIO i.e. for regular lua files
                                            {
                                                // append type

                                                if (thisfile.append)
                                                {
                                                    if (uselegacy)
                                                    {
                                                        writestring = thisfile.orig_legacy;

                                                        if (!restore)
                                                        {
                                                            writestring += "\n" + thisfile.source_legacy;
                                                        }
                                                    }
                                                    else // for openbeta
                                                    {
                                                        writestring = thisfile.orig;

                                                        if (!restore)
                                                        {
                                                            writestring += "\n" + thisfile.source;
                                                        }

                                                    }
                                                }
                                                else // replace type
                                                {
                                                    if (thisfile.reset || restore)
                                                    {
                                                        if (uselegacy)
                                                        {
                                                            writestring = thisfile.orig_legacy;
                                                        }
                                                        else
                                                        {
                                                            writestring = thisfile.orig;
                                                        }
                                                    }
                                                    else // not restore
                                                    {
                                                        if (uselegacy)
                                                        {
                                                            writestring = thisfile.orig_legacy + "\n" + thisfile.source_legacy;
                                                        }
                                                        else
                                                        {
                                                            writestring = thisfile.orig + "\n" + thisfile.source;
                                                        }
                                                    }
                                                }

                                                // write the file (append mode)
                                                using (StreamWriter writer = new StreamWriter(path, true))
                                                {
                                                    writer.Write(writestring);
                                                }
                                            }

                                            // dots in log
                                            if (!thisfile.quiet)
                                            {
                                                updatecounter = updatecounter + 1;

                                                if (!forcequiet)
                                                {
                                                    Log.Write("   Reset: " + thisfile.filename, Colors.Recognition);
                                                }
                                            }

                                        }
                                        else // specifcally for export.lua:
                                        {

                                            if (!File.Exists(path)) // no file? create fresh export.lua
                                            {
                                                string writestring = thisfile.orig;

                                                if (!restore)
                                                {
                                                    writestring += "\n" + thisfile.source;
                                                }

                                                using (StreamWriter writer = new StreamWriter(path, true))
                                                {
                                                    writer.Write(writestring);
                                                }

                                                if (!forcequiet)
                                                {
                                                    Log.Write("   Reset: " + thisfile.filename, Colors.Recognition);
                                                }

                                                updatecounter = updatecounter + 1;
                                            }
                                            else // check/fix existing export file
                                            {
                                                string exportmatch = Properties.Resources.Exportmatch;
                                                string exportfile = File.ReadAllText(path);

                                                int desiredentryoccurrences = 1;
                                                if (restore)
                                                {
                                                    desiredentryoccurrences = 0;
                                                }

                                                int matchoccurences = (exportfile.Length - exportfile.Replace(exportmatch, "").Length) / exportmatch.Length;

                                                if (matchoccurences < desiredentryoccurrences) //not enough: need to add fresh entry
                                                {
                                                    string writestring = "\n" + thisfile.source;

                                                    using (StreamWriter writer = new StreamWriter(path, true))
                                                    {
                                                        writer.Write(writestring);
                                                    }

                                                    updatecounter = updatecounter + 1;

                                                    if (!forcequiet)
                                                    {
                                                        Log.Write("   Reset: " + thisfile.filename, Colors.Recognition);
                                                    }
                                                }
                                                else
                                                {
                                                    if (matchoccurences > desiredentryoccurrences) // need to do some repair here
                                                    {
                                                        exportfile = exportfile.Replace(exportmatch, "");

                                                        if (!restore)
                                                        {
                                                            exportfile = exportfile + "\n" + exportmatch;
                                                        }

                                                        string writestring = exportfile;

                                                        using (StreamWriter writer = new StreamWriter(path, false))
                                                        {
                                                            writer.Write(writestring);
                                                        }

                                                        updatecounter = updatecounter + 1;

                                                        if (!forcequiet)
                                                        {
                                                            Log.Write("   Repaired: " + thisfile.filename, Colors.Recognition);
                                                        }
                                                    }
                                                    else // ok, desired number of entry occurrences
                                                    {
                                                        if (!forcequiet)
                                                        {
                                                            Log.Write("   Unchanged: " + thisfile.filename, Colors.Recognition);
                                                        }
                                                    }
                                                }

                                            }

                                        }

                                        if (restore && thisfile.filename.ToLower().Equals("vaicompro.export.lua"))
                                        {
                                            Directory.Delete(basepath, true);
                                        }


                                    } //end foreach


                                    // lua for this DCS version processed

                                    installcounter = installcounter + 1;

                                    if (!forcequiet)
                                    {
                                        Log.Write("   " + updatecounter + "/9 DCS-side files were updated.", Colors.Text);
                                    }

                                    // Done, now install theme for this DCS version:

                                    Install_DCS_Theme(UserSavedGamesFolder + "\\" + Server.dcsversion[set.Key]);

                                }

                            }
                            catch (Exception e)
                            {
                                if (!forcequiet)
                                {
                                    Log.Write("Auto-install could not complete for DCS version " + set.Key, Colors.Text);
                                    Log.Write(e.Message, Colors.Text);
                                    Log.Write(e.StackTrace, Colors.Text);
                                }
                            }

                        }

                        // no installs done at all

                        if (installcounter == 0)
                        {
                            if (!forcequiet)
                            {
                                Log.Write("No DCS World installation was automatically detected on this machine.", Colors.Critical);
                                Log.Write("Use the plugin Configuration page to set a custom DCS install path.", Colors.Critical);
                            }
                        }

                    }
                    catch
                    {
                        if (!forcequiet)
                        {
                            Log.Write("Problems were reported during server files installation.", Colors.Text);
                        }
                    }
                }
            }
        }
    }
}

