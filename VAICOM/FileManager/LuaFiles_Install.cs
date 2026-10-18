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

                private static void ApplyDynamicLuaFileSettings()
                {
                    if (LuaFiles.ContainsKey("2.8 ATC.lua"))
                    {
                        var atcFile = LuaFiles["2.8 ATC.lua"];
                        bool carrierSuppressAutoEnabled = State.activeconfig != null && State.activeconfig.CarrierSuppressAuto;

                        if (carrierSuppressAutoEnabled)
                        {
                            atcFile.source = Properties.Resources.Append_Core_ATC;
                            atcFile.source_legacy = Properties.Resources.Append_Core_ATC;
                        }
                        else
                        {
                            atcFile.source = string.Empty;
                            atcFile.source_legacy = string.Empty;
                        }
                    }

                    if (!LuaFiles.ContainsKey("2.9 WSO Renderer.js"))
                    {
                        return;
                    }

                    var rendererFile = LuaFiles["2.9 WSO Renderer.js"];
                    bool hideDialog = State.activeconfig != null && State.activeconfig.HideF4EDialog;

                    if (hideDialog)
                    {
                        rendererFile.source = Properties.Resources.Append_F4E_Jester_Renderer;
                        rendererFile.source_legacy = Properties.Resources.Append_F4E_Jester_Renderer;
                        rendererFile.stringsource = Properties.Resources.Append_F4E_Jester_Renderer;
                    }
                    else
                    {
                        rendererFile.source = "";
                        rendererFile.source_legacy = "";
                        rendererFile.stringsource = "";
                    }
                }

                private static void EnsureIcaoOverridesFile(string savedGamesRoot, string dcsVersionFolder, bool forcequiet)
                {
                    try
                    {
                        string targetFolder = Path.Combine(savedGamesRoot, dcsVersionFolder, "Scripts", "VAICOMPRO");
                        if (!Directory.Exists(targetFolder))
                        {
                            Directory.CreateDirectory(targetFolder);
                        }

                        string targetPath = Path.Combine(targetFolder, "ICAOOverrides.lua");
                        string content = Properties.Resources.ResourceManager.GetString("ICAOOverrides_lua");
                        if (string.IsNullOrWhiteSpace(content))
                        {
                            Log.Write("ICAOOverrides.lua resource missing, skipping ICAO override file install.", Colors.Warning);
                            return;
                        }

                        File.WriteAllText(targetPath, content);

                        if (!forcequiet)
                        {
                            Log.Write("   Reset: ICAOOverrides.lua", Colors.Recognition);
                        }
                    }
                    catch (Exception e)
                    {
                        if (!forcequiet)
                        {
                            Log.Write("Could not install ICAOOverrides.lua: " + e.Message, Colors.Warning);
                        }
                    }
                }

                private static bool UsesLiveOriginalBackup(Server.LuaFile thisfile)
                {
                    if (thisfile == null || thisfile.binary || thisfile.kneeboard || !thisfile.hardreset)
                    {
                        return false;
                    }

                    if (thisfile.filename.Equals("Export.lua", StringComparison.OrdinalIgnoreCase)
                        || thisfile.filename.Equals("VAICOMPRO.export.lua", StringComparison.OrdinalIgnoreCase))
                    {
                        return false;
                    }

                    return !string.IsNullOrWhiteSpace(thisfile.orig) || !string.IsNullOrWhiteSpace(thisfile.orig_legacy);
                }

                private static string GetLuaBackupStoreRoot(string savedGamesRoot, string dcsVersionFolder)
                {
                    return Path.Combine(savedGamesRoot, dcsVersionFolder, "Scripts", "VAICOMPRO", "Backups");
                }

                private static string GetLuaBackupPath(string savedGamesRoot, string dcsVersionFolder, string targetRootPath, string targetPath)
                {
                    string normalizedTargetPath = Path.GetFullPath(targetPath);
                    string relativePath = normalizedTargetPath;

                    if (!string.IsNullOrWhiteSpace(targetRootPath))
                    {
                        string normalizedRootPath = Path.GetFullPath(targetRootPath)
                            .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                        string rootPrefix = normalizedRootPath + Path.DirectorySeparatorChar;

                        if (normalizedTargetPath.StartsWith(rootPrefix, StringComparison.OrdinalIgnoreCase))
                        {
                            relativePath = normalizedTargetPath.Substring(rootPrefix.Length);
                        }
                    }

                    char[] invalidChars = Path.GetInvalidFileNameChars();
                    char[] fileNameChars = relativePath.ToCharArray();
                    for (int i = 0; i < fileNameChars.Length; i++)
                    {
                        char currentChar = fileNameChars[i];
                        if (currentChar == Path.DirectorySeparatorChar
                            || currentChar == Path.AltDirectorySeparatorChar
                            || Array.IndexOf(invalidChars, currentChar) >= 0)
                        {
                            fileNameChars[i] = '_';
                        }
                    }

                    string backupFileName = new string(fileNameChars) + ".bak";
                    return Path.Combine(GetLuaBackupStoreRoot(savedGamesRoot, dcsVersionFolder), backupFileName);
                }

                private static string GetLuaLogName(Server.LuaFile thisfile)
                {
                    if (thisfile == null)
                    {
                        return "<unknown>";
                    }

                    return string.IsNullOrWhiteSpace(thisfile.installfolder)
                        ? thisfile.filename
                        : thisfile.installfolder + "\\" + thisfile.filename;
                }

                private static bool IsJesterModInstall(Server.LuaFile thisfile)
                {
                    return !string.IsNullOrWhiteSpace(thisfile.installfolder)
                        && thisfile.installfolder.StartsWith("jester\\mods", StringComparison.OrdinalIgnoreCase);
                }

                private static void ResolveLuaTargetPaths(Server.LuaFile thisfile, bool uselegacy, string dcsProgramFilesFolder, string savedGamesRoot, string dcsVersionFolder, out string basepath, out string path, out string backupVersionFolder, out string targetRootPath)
                {
                    if (thisfile.root)
                    {
                        backupVersionFolder = dcsVersionFolder;
                        targetRootPath = dcsProgramFilesFolder;
                        string installFolder = uselegacy ? thisfile.installfolder_legacy : thisfile.installfolder;
                        basepath = Path.Combine(dcsProgramFilesFolder, installFolder);
                    }
                    else
                    {
                        backupVersionFolder = IsJesterModInstall(thisfile) ? "DCS_F4E" : dcsVersionFolder;
                        targetRootPath = Path.Combine(savedGamesRoot, backupVersionFolder);
                        basepath = Path.Combine(targetRootPath, thisfile.installfolder);
                    }

                    path = Path.Combine(basepath, thisfile.filename);
                }

                private static string GetEffectiveOriginalContent(Server.LuaFile thisfile, bool uselegacy)
                {
                    return uselegacy && !string.IsNullOrEmpty(thisfile.orig_legacy)
                        ? thisfile.orig_legacy
                        : thisfile.orig;
                }

                private static string GetEffectiveSourceContent(Server.LuaFile thisfile, bool uselegacy)
                {
                    return uselegacy && !string.IsNullOrEmpty(thisfile.source_legacy)
                        ? thisfile.source_legacy
                        : thisfile.source;
                }

                private static List<string> GetKnownPatchContents(Server.LuaFile thisfile, bool uselegacy)
                {
                    var patchContents = new List<string>();

                    void AddPatchContent(string content)
                    {
                        if (!string.IsNullOrWhiteSpace(content) && !patchContents.Contains(content))
                        {
                            patchContents.Add(content);
                        }
                    }

                    AddPatchContent(GetEffectiveSourceContent(thisfile, uselegacy));
                    AddPatchContent(thisfile.source);
                    AddPatchContent(thisfile.source_legacy);

                    if (thisfile.filename.Equals("ATC.lua", StringComparison.OrdinalIgnoreCase))
                    {
                        AddPatchContent(Properties.Resources.Append_Core_ATC);
                    }

                    if (thisfile.filename.Equals("renderer.js", StringComparison.OrdinalIgnoreCase)
                        && !string.IsNullOrWhiteSpace(thisfile.installfolder)
                        && thisfile.installfolder.IndexOf("F-4E\\UI\\JesterDialog\\js", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        AddPatchContent(Properties.Resources.Append_F4E_Jester_Renderer);
                    }

                    return patchContents;
                }

                private static bool IsLikelyVaicomPatched(Server.LuaFile thisfile, string currentContent, bool uselegacy)
                {
                    if (string.IsNullOrEmpty(currentContent))
                    {
                        return false;
                    }

                    foreach (string patchContent in GetKnownPatchContents(thisfile, uselegacy))
                    {
                        if (currentContent.Contains(patchContent))
                        {
                            return true;
                        }
                    }

                    return thisfile.stringreplace
                        && !string.IsNullOrWhiteSpace(thisfile.stringsource)
                        && currentContent.Contains(thisfile.stringsource);
                }

                private static bool CanStripPatchedContent(Server.LuaFile thisfile)
                {
                    return thisfile.append || !thisfile.AIRIO;
                }

                private static bool TryRemoveKnownPatchContent(Server.LuaFile thisfile, string currentContent, bool uselegacy, out string strippedContent)
                {
                    strippedContent = currentContent;
                    if (!CanStripPatchedContent(thisfile) || string.IsNullOrEmpty(currentContent))
                    {
                        return false;
                    }

                    foreach (string patchContent in GetKnownPatchContents(thisfile, uselegacy))
                    {
                        string updated = strippedContent;
                        while (true)
                        {
                            string next = updated
                                .Replace("\r\n" + patchContent, "\r\n")
                                .Replace("\n" + patchContent, "\n")
                                .Replace(patchContent + "\r\n", string.Empty)
                                .Replace(patchContent + "\n", string.Empty)
                                .Replace(patchContent, string.Empty);

                            if (string.Equals(next, updated, StringComparison.Ordinal))
                            {
                                break;
                            }

                            updated = next;
                        }

                        strippedContent = updated;
                    }

                    return !string.Equals(strippedContent, currentContent, StringComparison.Ordinal);
                }

                private static string GetPreferredLuaBackupContent(Server.LuaFile thisfile, string currentContent, bool uselegacy, out string backupMode)
                {
                    backupMode = "live";
                    if (!IsLikelyVaicomPatched(thisfile, currentContent, uselegacy))
                    {
                        return currentContent;
                    }

                    if (TryRemoveKnownPatchContent(thisfile, currentContent, uselegacy, out string strippedContent)
                        && !string.IsNullOrWhiteSpace(strippedContent))
                    {
                        backupMode = "derived";
                        return strippedContent;
                    }

                    backupMode = "embedded";
                    return GetEffectiveOriginalContent(thisfile, uselegacy);
                }

                private static void NormalizeLiveLuaFileBeforeFirstBackup(Server.LuaFile thisfile, string path, string currentContent, string desiredBackupContent, string backupMode, bool forcequiet)
                {
                    if (string.Equals(currentContent, desiredBackupContent, StringComparison.Ordinal)
                        || string.IsNullOrEmpty(desiredBackupContent))
                    {
                        return;
                    }

                    File.WriteAllText(path, desiredBackupContent);

                    if (!forcequiet)
                    {
                        if (backupMode.Equals("derived", StringComparison.Ordinal))
                        {
                            Log.Write("   Normalized live file from patched content: " + GetLuaLogName(thisfile), Colors.Text);
                        }
                        else if (backupMode.Equals("embedded", StringComparison.Ordinal))
                        {
                            Log.Write("   Normalized live file from embedded original: " + GetLuaLogName(thisfile), Colors.Warning);
                        }
                    }
                }

                private static string ResolveLuaBackupPath(string path, string savedGamesRoot, string dcsVersionFolder, string targetRootPath)
                {
                    return GetLuaBackupPath(savedGamesRoot, dcsVersionFolder, targetRootPath, path);
                }

                private static bool TryReadLuaBackupContent(string path, string savedGamesRoot, string dcsVersionFolder, string targetRootPath, out string backupContent)
                {
                    string backupPath = ResolveLuaBackupPath(path, savedGamesRoot, dcsVersionFolder, targetRootPath);
                    if (File.Exists(backupPath))
                    {
                        backupContent = File.ReadAllText(backupPath);
                        return true;
                    }

                    backupContent = null;
                    return false;
                }

                private static void LogLuaBackupWrite(Server.LuaFile thisfile, string backupMode, bool hadExistingBackup)
                {
                    if (backupMode.Equals("derived", StringComparison.Ordinal))
                    {
                        Log.Write((hadExistingBackup ? "   Refreshed clean backup: " : "   Derived clean backup: ") + GetLuaLogName(thisfile), Colors.Text);
                    }
                    else if (backupMode.Equals("embedded", StringComparison.Ordinal))
                    {
                        Log.Write((hadExistingBackup ? "   Refreshed backup from embedded original: " : "   Seeded backup from embedded original: ") + GetLuaLogName(thisfile), Colors.Warning);
                    }
                    else if (hadExistingBackup)
                    {
                        Log.Write("   Refreshed backup: " + GetLuaLogName(thisfile), Colors.Text);
                    }
                    else
                    {
                        Log.Write("   Backed up: " + GetLuaLogName(thisfile), Colors.Text);
                    }
                }

                private static void EnsureLuaBackupExists(Server.LuaFile thisfile, string path, bool uselegacy, string savedGamesRoot, string dcsVersionFolder, string targetRootPath, bool forcequiet)
                {
                    if (!UsesLiveOriginalBackup(thisfile) || !File.Exists(path))
                    {
                        return;
                    }

                    string backupPath = ResolveLuaBackupPath(path, savedGamesRoot, dcsVersionFolder, targetRootPath);
                    string currentContent = File.ReadAllText(path);
                    string desiredBackupContent = GetPreferredLuaBackupContent(thisfile, currentContent, uselegacy, out string backupMode);
                    bool hadExistingBackup = TryReadLuaBackupContent(path, savedGamesRoot, dcsVersionFolder, targetRootPath, out string backupContent);

                    if (!hadExistingBackup)
                    {
                        NormalizeLiveLuaFileBeforeFirstBackup(thisfile, path, currentContent, desiredBackupContent, backupMode, forcequiet);
                    }
                    else if (string.Equals(desiredBackupContent, backupContent, StringComparison.Ordinal))
                    {
                        return;
                    }

                    Directory.CreateDirectory(Path.GetDirectoryName(backupPath));
                    File.WriteAllText(backupPath, desiredBackupContent);

                    if (!forcequiet)
                    {
                        LogLuaBackupWrite(thisfile, backupMode, hadExistingBackup);
                    }
                }

                private static string GetLuaBaseContent(Server.LuaFile thisfile, string path, bool uselegacy, string savedGamesRoot, string dcsVersionFolder, string targetRootPath, bool forcequiet, string operation)
                {
                    bool usesLiveBackup = UsesLiveOriginalBackup(thisfile);
                    if (usesLiveBackup && TryReadLuaBackupContent(path, savedGamesRoot, dcsVersionFolder, targetRootPath, out string backupContent))
                    {
                        if (!forcequiet)
                        {
                            Log.Write("   Using live backup for " + GetLuaLogName(thisfile) + " during " + operation + ".", Colors.Text);
                        }

                        return backupContent;
                    }

                    string fallbackContent = GetEffectiveOriginalContent(thisfile, uselegacy);
                    if (usesLiveBackup)
                    {
                        Log.Write("No live backup found for " + GetLuaLogName(thisfile) + "; using embedded original during " + operation + ".", Colors.Warning);
                    }
                    return fallbackContent;
                }

                public static void LuaFiles_Install(bool restore, bool forcequiet)
                {
                    // --- finds all DCS install locations and manipulates lua files

                    try
                    {
                        ApplyDynamicLuaFileSettings();

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

                                    if (!restore)
                                    {
                                        EnsureIcaoOverridesFile(UserSavedGamesFolder, Server.dcsversion[set.Key], forcequiet);
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
                                        string backupVersionFolder;
                                        string targetRootPath;

                                        ResolveLuaTargetPaths(thisfile, uselegacy, dcsprogramfilesfolder, UserSavedGamesFolder, Server.dcsversion[set.Key], out basepath, out path, out backupVersionFolder, out targetRootPath);

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

                                            bool isJesterModInstall = !string.IsNullOrWhiteSpace(thisfile.installfolder)
                                                && thisfile.installfolder.StartsWith("jester\\mods", StringComparison.OrdinalIgnoreCase);

                                            if (!restore)
                                            {
                                                EnsureLuaBackupExists(thisfile, path, uselegacy, UserSavedGamesFolder, backupVersionFolder, targetRootPath, forcequiet);
                                            }

                                            if ((State.luahardreset || isJesterModInstall) && thisfile.hardreset && !thisfile.stringreplace)
                                            {
                                                if (File.Exists(path))
                                                {
                                                    File.Delete(path);
                                                }
                                            }

                                            // -----  write the new file -----------------------------------------

                                            string writestring = "";
                                            string baseContent = GetLuaBaseContent(thisfile, path, uselegacy, UserSavedGamesFolder, backupVersionFolder, targetRootPath, forcequiet, restore ? "restore" : "patch");
                                            string effectiveSource = GetEffectiveSourceContent(thisfile, uselegacy);

                                            if (thisfile.AIRIO) // 
                                            {
                                                bool isF14JesterMiniWheelFile =
                                                    (!string.IsNullOrWhiteSpace(thisfile.installfolder)
                                                     && thisfile.installfolder.Equals("Mods\\aircraft\\F14\\Cockpit\\Scripts\\JesterAI", StringComparison.OrdinalIgnoreCase)
                                                     && (thisfile.filename.Equals("JesterAI_Page.lua", StringComparison.OrdinalIgnoreCase)
                                                         || thisfile.filename.Equals("init.lua", StringComparison.OrdinalIgnoreCase)))
                                                    || (!string.IsNullOrWhiteSpace(thisfile.installfolder)
                                                        && thisfile.installfolder.Equals("Mods\\aircraft\\F14\\Cockpit", StringComparison.OrdinalIgnoreCase)
                                                        && thisfile.filename.Equals("proxy_device.lua", StringComparison.OrdinalIgnoreCase));

                                                bool airioFeatureEnabledForFile = State.dll_installed_rio
                                                    && (isF14JesterMiniWheelFile
                                                        ? State.activeconfig.RIO_MiniWheel_Enabled
                                                        : State.activeconfig.RIO_Enabled);

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
                                                    if (!airioFeatureEnabledForFile || restore)
                                                    {
                                                        writestring = baseContent;
                                                    }
                                                    else if (thisfile.append)
                                                    {
                                                        writestring = baseContent + "\n" + effectiveSource;
                                                    }
                                                    else if (thisfile.reset)
                                                    {
                                                        writestring = baseContent;
                                                    }
                                                    else
                                                    {
                                                        writestring = effectiveSource;
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
                                            else //not AIRIO i.e. for regular lua files
                                            {
                                                if (thisfile.append)
                                                {
                                                    writestring = baseContent;

                                                    if (!restore)
                                                    {
                                                        writestring += "\n" + effectiveSource;
                                                    }
                                                }
                                                else if (thisfile.reset || restore)
                                                {
                                                    writestring = baseContent;
                                                }
                                                else if (thisfile.filename.Equals("JesterAI_Page.lua", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    writestring = effectiveSource;
                                                }
                                                else
                                                {
                                                    writestring = baseContent + "\n" + effectiveSource;
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
                                                    Log.Write("   Reset: " + GetLuaLogName(thisfile), Colors.Recognition);
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
                                                    Log.Write("   Reset: " + GetLuaLogName(thisfile), Colors.Recognition);
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
                                                        Log.Write("   Reset: " + GetLuaLogName(thisfile), Colors.Recognition);
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
                                                            Log.Write("   Repaired: " + GetLuaLogName(thisfile), Colors.Recognition);
                                                        }
                                                    }
                                                    else // ok, desired number of entry occurrences
                                                    {
                                                        if (!forcequiet)
                                                        {
                                                            Log.Write("   Unchanged: " + GetLuaLogName(thisfile), Colors.Recognition);
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

