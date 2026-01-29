using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using VAICOM.Database;
using VAICOM.Products;
using VAICOM.Static;

namespace VAICOM
{
    namespace FileManager
    {

        public partial class FileHandler
        {

            public partial class Database
            {

                // restores / writes imported modules file
                public static void WriteModulesToFile(bool overwrite)
                {
                    string filename;
                    if (State.databaseencrypted)
                    { filename = Aliases.scrambleddbfilenames["importedmodules"]; }
                    else
                    { filename = "importedmodules" + ".json"; }

                    List<DCSmodule> WriteObject = new List<DCSmodule>();
                    try
                    {
                        string path = State.VA_APPS + "\\" + AppData.RootFolder + "\\" + AppData.SubFolders["database"] + "\\" + filename;
                        WriteObject = State.importeddcsmodules;
                        string jsonstring;
                        if (State.databaseencrypted)
                        {
                            jsonstring = Helpers.Crypto.Encrypt(JsonConvert.SerializeObject(WriteObject, Formatting.Indented));
                        }
                        else
                        {
                            jsonstring = JsonConvert.SerializeObject(WriteObject, Formatting.Indented);
                        }
                        if (overwrite || !File.Exists(path))
                        {
                            File.WriteAllText(path, jsonstring);
                            Log.Write("Writing imported modules file.", Colors.Text);
                        }
                    }
                    catch
                    {
                    }
                }

                // reads dcs modules from file (on init)
                public static List<DCSmodule> ReadModulesFromFile()
                {
                    string filename;
                    if (State.databaseencrypted)
                    { filename = Aliases.scrambleddbfilenames["importedmodules"]; }
                    else
                    { filename = "importedmodules" + ".json"; }
                    List<DCSmodule> ReturnObject = new List<DCSmodule>();
                    try
                    {
                        string path = State.VA_APPS + "\\" + AppData.RootFolder + "\\" + AppData.SubFolders["database"] + "\\" + filename;

                        if (!File.Exists(path)) { Log.Write("No new DCS modules to import.", Colors.Text); return ReturnObject; }

                        if (State.databaseencrypted)
                        {
                            ReturnObject = JsonConvert.DeserializeObject<List<DCSmodule>>(Helpers.Crypto.Decrypt(File.ReadAllText(path)));
                        }
                        else
                        {
                            ReturnObject = JsonConvert.DeserializeObject<List<DCSmodule>>(File.ReadAllText(path));
                        }

                        foreach (DCSmodule mod in ReturnObject)
                        {
                            DCSmodules.LookupTable.Add(mod.Name, mod);
                            Log.Write("Adding imported module: " + mod.Name, Colors.Text);
                        }
                        Log.Write("Loaded imported modules from file.", Colors.Text);
                    }
                    catch
                    {
                    }

                    return (ReturnObject);

                }
                
            }
        }
    }
}
