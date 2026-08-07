using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using VAICOM.Servers;
using VAICOM.Static;

namespace VAICOM
{
    namespace FileManager
    {
        public static partial class FileHandler
        {
            public static void ExplorerOpenFolder(string foldername)
            {
                string basepath = State.VA_APPS + "\\" + AppData.RootFolder + "\\" + AppData.SubFolders["export"];
                try
                {
                    Process.Start(basepath);
                }
                catch
                {
                }
            }

            public static partial class Lua
            {

                public static void LuaFiles_Export()
                {

                    string basepath = State.VA_APPS + "\\" + AppData.RootFolder + "\\" + AppData.SubFolders["export"];

                    try
                    {

                        foreach (KeyValuePair<string, Server.LuaFile> serverfile in LuaFiles)
                        {
                            if (serverfile.Value.export)
                            {
                                Server.LuaFile thisfile = serverfile.Value;
                                string path = basepath + "\\" + thisfile.filename;
                                if (File.Exists(path)) { File.Delete(path); }
                                string writestring = thisfile.source;
                                using (StreamWriter writer = new StreamWriter(path, true)) { writer.Write(writestring); };
                                Log.Write("Exported file: " + thisfile.filename, Colors.Text);
                            }
                        }

                    }
                    catch
                    {
                        Log.Write("There was a problem during lua files export.", Colors.Text);
                    }

                    try
                    {
                        //VAICOM PRO for DCS World
                        string filename = "VAICOM PRO for DCS World.vap";
                        string filepath = basepath + "\\" + filename;
                        if (File.Exists(filepath)) { File.Delete(filepath); }
                        byte[] filewritebytes = Properties.Resources.VAICOM_PRO_for_DCS_World;
                        File.WriteAllBytes(filepath, filewritebytes);
                        Log.Write("Exported file: " + filename, Colors.Text);

                    }
                    catch
                    {
                        Log.Write("There was a problem during profiles export.", Colors.Text);
                    }

                    UI.Playsound.Startup();

                    ExplorerOpenFolder("export");

                    Log.Write("Server-side files were exported to the Export folder.", Colors.System);
                    Log.Write("Check User Manual for manual installation guidelines.", Colors.System);

                }
            }
        }
    }
}
