using System;
using System.IO;
using VAICOM.Static;

namespace VAICOM
{

    public static class Log
    {

        public static void Reset()
        {
            try
            {
                State.logfile = State.VA_APPS + "\\" + Products.Products.Families.Vaicom.VaicomProPlugin.rootfoldername + "\\" + AppData.SubFolders["logfiles"] + "\\" + "VAICOMPRO.log";

                if (State.activeconfig.Debugmode || State.trainerrunning)
                {
                    try
                    {
                        if (File.Exists(State.logfile))
                        {
                            File.Delete(State.logfile);
                        }
                    }
                    catch
                    {
                    }

                    using (StreamWriter writer = new StreamWriter(State.logfile, true)) { writer.Write("VAICOM PRO logfile" + "\n"); };
                }
            }
            catch
            {
            }
        }

        public static void Write(string writestring, string color)
        {
            try
            {
                if (color == Colors.Warning)
                {
                    Extensions.Kneeboard.OpenKneeboardBridge.UpdateStatus(writestring, "warning");
                }
                else if (color == Colors.Critical)
                {
                    Extensions.Kneeboard.OpenKneeboardBridge.UpdateStatus(writestring, "error");
                }
            }
            catch
            {
            }

            if (State.startup || (!State.activeconfig.Debugmode & !State.trainerrunning))
            {
                if (State.startup && State.deepdebugmode)
                {
                    State.Proxy.WriteToLog(writestring, color);
                }
                else
                {
                    if (color != Colors.Text && color != Colors.Text && color != Colors.Notification && color != Colors.Alert && color != Colors.Inline && color != Colors.Recognition)
                    {
                        State.Proxy.WriteToLog(writestring, color);
                    }
                }
            }
            else
            {
                if (State.deepdebugmode || color != Colors.Inline)
                {
                    State.Proxy.WriteToLog(writestring, color);
                    try
                    {
                        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        using (StreamWriter writer = new StreamWriter(State.logfile, true)) { writer.Write($"{timestamp} {writestring}" + "\n"); };
                    }
                    catch
                    {
                    }
                }
            }
        }
    }
}
