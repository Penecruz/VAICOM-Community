using System.Collections.Generic;
using System.Text.RegularExpressions;
using VAICOM.Database;
using VAICOM.Static;


namespace VAICOM
{
    namespace Client
    {

        public partial class DcsClient
        {
            public static partial class Message
            {

                // scans for keywords from category database in current sentence
                public static bool scanfor(string category)
                {
                    if (!State.have[category])
                    {

                        bool haveresult = false;

                        // if not have something already do raw search for this category

                        string cat = category.ToLower();
                        string searchinput = State.currentfullsentence.ToLower();

                        if (category.Equals("command") && Regex.IsMatch(searchinput, @"^\s*gunner\b", RegexOptions.IgnoreCase))
                        {
                            searchinput = Regex.Replace(searchinput, @"^\s*gunner[\s,]*", "george ", RegexOptions.IgnoreCase);
                        }

                        //Log.Write("scanning "+ cat +" search input = " + searchinput, colors.Text);

                        Dictionary<string, string> localresults = new Dictionary<string, string>();

                        foreach (KeyValuePair<string, string> set in Aliases.inputscancats[category])
                        {

                            if (searchinput.Contains(set.Key.Replace("*", "").ToLower()))
                            {
                                if (State.dcsrunning && category.Equals("command") && Commands.Table[set.Value].isRIO() && !State.AIRIOactive)
                                {
                                    // Log.Write("AIRIO Command rejected: " + set.Key, Colors.Text);
                                    // AIRO commands are rejected: don't add to results
                                }
                                else
                                {
                                    localresults.Add(set.Key.Replace("*", ""), set.Value);
                                }
                            }
                        }

                        // from results, gather final result (ie longest)

                        string usedalias = "";
                        string finalresult = "";

                        int longest = 0;

                        foreach (KeyValuePair<string, string> set in localresults)
                        {
                            // Add bias for WSO and Two in calls
                            if (set.Value.Equals("WSO") || set.Key.ToLower().Equals("two")) 
                            {
                                usedalias = set.Key;
                                finalresult = set.Value;
                                break;
                            }
                            else
                            {
                                if (set.Key.Length > longest)
                                {
                                    usedalias = set.Key;
                                    finalresult = set.Value;
                                    longest = set.Key.Length;
                                }
                            }
                        }

                        //

                        // evaluate final result in this category, exit false if moot:

                        if (finalresult.Equals(""))
                        {
                            //Log.Write("No result for " + category, colors.Text);
                            haveresult = false;
                        }
                        else // found something for this cat
                        {
                            haveresult = true;

                            bool callsignmatch = State.currentstate.playercallsign.ToLower().Contains(usedalias.ToLower());

                            if (category.Equals("sender"))
                            {

                                if (callsignmatch || !State.usedalias["recipient"].ToLower().Equals(usedalias.ToLower()))
                                {
                                    Log.Write("Have result, identified as " + category + ": " + usedalias, Colors.Text);

                                    State.currentkey[category] = finalresult;
                                    State.usedalias[category] = usedalias;

                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }

                            if (category.Equals("recipient"))
                            {

                                if (callsignmatch)
                                {
                                    return false;
                                }
                                else
                                {

                                    Log.Write("Have result, identified as " + category + ": " + usedalias, Colors.Text);
                                    State.currentkey[category] = finalresult;
                                    State.usedalias[category] = usedalias;
                                    return true;

                                }
                            }

                            // other cats

                            State.currentkey[category] = finalresult;
                            State.usedalias[category] = usedalias;

                            Log.Write("Have result, identified as " + category + ": " + usedalias, Colors.Text);

                        }

                        return haveresult;
                    }
                    else
                    {
                        return true; // already have solution
                    }
                }

            }
        }
    }
}



