using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using VAICOM.Extensions.RIO;
using VAICOM.Products;
using VAICOM.Static;


namespace VAICOM
{

    namespace Helpers
    {

        public partial class Common
        {

            public static bool IsF14RioSeatActive()
            {
                if (State.currentmodule == null || !State.currentmodule.Id.Equals("F-14", StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }

                try
                {
                    return tables.menustate[tables.menucats.PLAYERSEAT].Equals(tables.menustates.RIO);
                }
                catch
                {
                    return false;
                }
            }

            public static string GetCurrentModuleDisplayText() //change to module name + alias, and if F-14, add RIO or Pilot as appropriate.
            {
                if (State.currentmodule == null || State.currentmodule.Name.Equals("----"))
                {
                    return "";
                }

                if (State.currentmodule.Id.Equals("F-14", StringComparison.OrdinalIgnoreCase))
                {
                    return IsF14RioSeatActive() ? "F-14 Tomcat RIO" : "F-14 Tomcat Pilot";
                }

                return State.currentmodule.Name + " " + State.currentmodule.Alias;
            }

            public static string RemoveDigits(string q)
            {
                return Regex.Replace(q, @"\d", "");
            }

            public static string ReadFrequency(string q) //grabbing frequency list and creating a sorted list.
            {
                //input must be normalized freq string
                string returnstr = q;

                try
                {
                    string freqbase = returnstr.Substring(0, 1) + " " + returnstr.Substring(1, 1) + " " + returnstr.Substring(2, 1);
                    string freqdec = returnstr.Substring(4, 1) + returnstr.Substring(5, 1) + returnstr.Substring(6, 1);

                    returnstr = freqbase + " decimal " + freqdec;
                    returnstr = returnstr.Replace("000", "zero");
                }
                catch
                {
                }

                return returnstr;
            }


            public static List<string> SortFrequencies(List<string> a)
            {
                List<string> SortedList = new List<string>();

                SortedList = a.OrderBy(q => q).ToList(); // (q => q)?? or reverse

                return SortedList;
            }

            public static string Reverse(string s)
            {
                char[] charArray = s.ToCharArray();
                Array.Reverse(charArray);
                return new string(charArray);
            }

            public static int Modulo(int x, int m)
            {
                int r = x % m;
                return r < 0 ? r + m : r;
            }

            public static string RemoveIllegalChars(string inputstr)
            {

                return inputstr.Replace("{", "").Replace("}", "").Replace("[", "").Replace("]", "").Replace("@", "").Replace("$", "").Replace("#", "").Replace(";", "").Replace(":", "").Replace("\"", "").Replace("\'", "").Replace("*", "").Replace(".", " ").Replace("-", " ").Replace("(", "").Replace(")", "");
            }

            public static string RemoveIllegalCharsForDB(string inputstr)
            {
                string outputstr = inputstr.Replace("{", "").Replace("}", "").Replace("[", "").Replace("]", "").Replace("@", "").Replace("$", "").Replace("#", "").Replace(";", "").Replace(":", "").Replace("\"", "").Replace("\'", "").Replace("!", "").Replace(".", "");

                if (!outputstr.Contains("*"))
                {
                    return outputstr;
                }
                else
                {
                    if (State.activeconfig.UseNewRecognitionModel)
                    {
                        bool addhead = false;
                        bool addtail = false;

                        if (outputstr.StartsWith("*"))
                        {
                            addhead = true;
                        }
                        if (outputstr.EndsWith("*"))
                        {
                            addtail = true;
                        }

                        outputstr = outputstr.Replace("*", "");

                        if (addhead)
                        {
                            outputstr = "*" + outputstr;
                        }

                        if (addtail)
                        {
                            outputstr = outputstr + "*";
                        }

                        return outputstr;
                    }
                    else
                    {
                        outputstr = outputstr.Replace("*", "");
                        return outputstr;
                    }
                }

            }

            public static string StringArrayToMultiString(ICollection<string> stringArray)
            {
                StringBuilder multiString = new StringBuilder();
                if (stringArray != null)
                {
                    foreach (string s in stringArray)
                    {
                        multiString.Append(s);
                        multiString.Append('\0');
                    }
                }
                return multiString.ToString();
            }

            public static int GetRange(Servers.Server.Vector A, Servers.Server.Vector B)
            {
                double Range = Math.Round(Math.Sqrt(Math.Pow((A.x - B.x), 2) + Math.Pow((A.z - B.z), 2)), 0);
                return (int)Range;
            }

            public static string NormalizeFreqString(string inputstring)
            {
                string normalizedinput;
                if (inputstring.Equals(null))
                {
                    normalizedinput = "";
                }
                else
                {
                    normalizedinput = inputstring;
                }

                if (normalizedinput.Contains("."))
                {
                    normalizedinput = normalizedinput.Substring(0, normalizedinput.IndexOf("."));
                }

                normalizedinput = Common.Reverse(Common.Reverse(("000000000" + normalizedinput)).Substring(0, 9));

                string RawValMain = normalizedinput.Substring(0, 3);
                int RawValDec = Int32.Parse(normalizedinput.Substring(3, 3));
                double RoundedValDec = 25 * Math.Round((double)RawValDec / 25);
                string RoundedDec = Common.Reverse(Common.Reverse(("000" + RoundedValDec)).Substring(0, 3));

                normalizedinput = RawValMain + "." + RoundedDec; // + " MHz";

                return normalizedinput;
            }


            public static string StringNormalize(string inputstring)
            {
                string normalizedinput = inputstring;
                normalizedinput = normalizedinput.TrimEnd();
                normalizedinput = normalizedinput.TrimStart();
                normalizedinput = normalizedinput.ToLower();
                return normalizedinput;
            }

            public static string TrimmedString(string inputstring)
            {
                string trimmedinput = inputstring;
                trimmedinput = trimmedinput.TrimEnd();
                trimmedinput = trimmedinput.TrimStart();
                return trimmedinput;
            }


            public static string ProcessBrevity(string inputstr)
            {

                string processtring = inputstr;

                try
                {

                    processtring = processtring.Replace("CV 1143.5 ", "");
                    processtring = processtring.Replace("CVN-70 ", "");
                    processtring = processtring.Replace("LHA-1 ", "");
                    processtring = processtring.Replace("FFG-7CL ", "");
                    processtring = processtring.Replace("CVN-74 ", "");
                    processtring = processtring.Replace("CG-60 ", "");
                    processtring = processtring.Replace("CV-59 ", "");

                    foreach (KeyValuePair<string, DCSmodule> mod in DCSmodules.LookupTable)
                    {
                        processtring = processtring.Replace(mod.Key, mod.Value.SpeechAlias);
                    }

                    // decimal numbers

                    processtring = processtring.Replace(".000", "decimal zero");
                    processtring = processtring.Replace(".00", "decimal zero");
                    processtring = processtring.Replace(".0", "decimal zero");

                    processtring = processtring.Replace(".100", "decimal one");
                    processtring = processtring.Replace(".10", "decimal one");
                    processtring = processtring.Replace(".1", "decimal one");

                    processtring = processtring.Replace(".200", "decimal two");
                    processtring = processtring.Replace(".20", "decimal two");
                    processtring = processtring.Replace(".2", "decimal two");

                    processtring = processtring.Replace(".300", "decimal three");
                    processtring = processtring.Replace(".30", "decimal three");
                    processtring = processtring.Replace(".3", "decimal three");

                    processtring = processtring.Replace(".400", "decimal four");
                    processtring = processtring.Replace(".40", "decimal four");
                    processtring = processtring.Replace(".4", "decimal four");

                    processtring = processtring.Replace(".500", "decimal five");
                    processtring = processtring.Replace(".50", "decimal five");
                    processtring = processtring.Replace(".5", "decimal five");

                    processtring = processtring.Replace(".600", "decimal six");
                    processtring = processtring.Replace(".60", "decimal six");
                    processtring = processtring.Replace(".6", "decimal six");

                    processtring = processtring.Replace(".700", "decimal seven");
                    processtring = processtring.Replace(".70", "decimal seven");
                    processtring = processtring.Replace(".7", "decimal seven");

                    processtring = processtring.Replace(".800", "decimal eight");
                    processtring = processtring.Replace(".80", "decimal eight");
                    processtring = processtring.Replace(".8", "decimal eight");

                    processtring = processtring.Replace(".900", "decimal nine");
                    processtring = processtring.Replace(".90", "decimal nine");
                    processtring = processtring.Replace(".9", "decimal nine");

                    //general
                    processtring = processtring.Replace("NM", "miles");
                    processtring = processtring.Replace(" nm", " miles");
                    processtring = processtring.Replace("WP", "waypoint");
                    processtring = processtring.Replace("AB", "airbase");
                    processtring = processtring.Replace("AFB", "airbase");
                    processtring = processtring.Replace("RTB", "return to base");
                    processtring = processtring.Replace("T/O", "takeoff");
                    processtring = processtring.Replace("ROE", "engagement rules");
                    processtring = processtring.Replace("RoE", "engagement rules");
                    processtring = processtring.Replace("PVE", "player versus environment");
                    processtring = processtring.Replace("PvE", "player versus environment");
                    processtring = processtring.Replace("PVP", "player versus player");
                    processtring = processtring.Replace("PvP", "player versus player");
                    processtring = processtring.Replace("Intl", "international");
                    processtring = processtring.Replace("intl", "international");

                    processtring = processtring.Replace("RWY", "runway");
                    processtring = processtring.Replace("SAMs", "Sem units");
                    processtring = processtring.Replace("SAM", "Sem");

                    //remove rubbish
                    processtring = processtring.Replace("#", "");
                    processtring = processtring.Replace("$", "");
                    processtring = processtring.Replace("*", "");
                    processtring = processtring.Replace("/", " ");
                    processtring = processtring.Replace("(", " ");
                    processtring = processtring.Replace(")", " ");
                    processtring = processtring.Replace("-", " ");
                    processtring = processtring.Replace("_", " ");
                    processtring = processtring.Replace("=", ",");

                    processtring = processtring.Replace("1x", "1 ");
                    processtring = processtring.Replace("2x", "2 ");
                    processtring = processtring.Replace("3x", "3 ");
                    processtring = processtring.Replace("4x", "4 ");

                    //abbrevs
                    processtring = processtring.Replace("AWACS", "A wackz");
                    processtring = processtring.Replace("ATC", "A T C");
                    processtring = processtring.Replace("JTAC", "Jay Tack");
                    processtring = processtring.Replace("Helo", "Hee Low");
                    processtring = processtring.Replace("Tanker", "Air Refuel");

                    // callsigns
                    processtring = processtring.Replace("Boar", "boar");
                    processtring = processtring.Replace("Hawg", "hawg");
                    processtring = processtring.Replace("Dodge", "dodsh");
                    processtring = processtring.Replace("Focus", "focuss");
                    processtring = processtring.Replace("Darkstar", "dark star");
                    processtring = processtring.Replace("Darknight", "dark night");
                    processtring = processtring.Replace("Deathstar", "death star");
                    processtring = processtring.Replace("Moonbeam", "moon beem");

                    // mssion types
                    processtring = processtring.Replace("CAP", "combat air patrol");
                    processtring = processtring.Replace("DCA", "defensive counter air");
                    processtring = processtring.Replace("BARCAP", "barrier combat air patrol");
                    processtring = processtring.Replace("HAVCAP", "high value asset protection");
                    processtring = processtring.Replace("TARCAP", "target combat air patrol");
                    processtring = processtring.Replace("RESCAP", "protect rescue helicopters");
                    processtring = processtring.Replace("sweep", "sweep");
                    processtring = processtring.Replace("intercept", "intercept");
                    processtring = processtring.Replace("escort", "escort");
                    processtring = processtring.Replace("SEAD", "suppression of air defense");
                    processtring = processtring.Replace("OCA", "offensive counter air");
                    processtring = processtring.Replace("FAC", "forward air control");
                    processtring = processtring.Replace("CAS", "close air support");
                    processtring = processtring.Replace("interdiction", "interdiction");
                    processtring = processtring.Replace("BDA", "damage assessment");
                    processtring = processtring.Replace("CSAR", "Search and Rescue");
                    processtring = processtring.Replace("SAR", "Search and Rescue");
                    processtring = processtring.Replace("MANPAD", "man pad");

                    processtring = processtring.Replace("A2G", "Air To Ground");
                    processtring = processtring.Replace("A2A", "Air To Air");

                    processtring = processtring.Replace("IFV", "I F V ");
                    processtring = processtring.Replace("APC", "A P C ");

                    processtring = processtring.Replace("AM", "A M ");
                    processtring = processtring.Replace("FM", "F M ");

                    // for callsign numbers
                    for (int i = 0; i < 10; i++)
                    {
                        processtring = processtring.Replace(i.ToString(), " " + i.ToString());
                    }

                    //atcs
                    processtring = processtring.Replace("FARP", "forward airbase");
                    processtring = processtring.Replace("SoH", "persian gulf");

                    processtring = processtring.Replace("Krymsk", "Krimsk");
                    processtring = processtring.Replace("UAE", "Emirates");
                    processtring = processtring.Replace("lasvegas", "Las Vegas");
                    processtring = processtring.Replace("Vaziani", "Vaaziaanee");

                }
                catch (Exception e)
                {
                    Log.Write("Error" + e.StackTrace, Colors.Inline);
                }

                return processtring;
            }

        }

    }

}
