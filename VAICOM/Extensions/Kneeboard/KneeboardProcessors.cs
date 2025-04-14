using System;
using System.Collections.Generic;
using VAICOM.Servers;
using VAICOM.Static;


namespace VAICOM
{
    namespace Extensions
    {
        namespace Kneeboard
        {

            public static partial class KneeboardHelper
            {

                public static string theatercode(string theater)
                {
                    string returnstring = "";
                    try
                    {
                        switch (theater.ToLower())
                        {
                            case "caucasus":
                                returnstring += "BLKS";
                                break;
                            case "nevada":
                                returnstring += "NTTR";
                                break;
                            case "normandy":
                                returnstring += "NRMY";
                                break;
                            case "persiangulf":
                                returnstring += "GULF";
                                break;
                            case "thechannel":
                                returnstring += "CHAN";
                                break;
                            case "syria":
                                returnstring += "SYRI";
                                break;
                            case "sinai":
                                returnstring += "SINI";
                                break;
                            case "marianas":
                                returnstring += "MARI";
                                break;
                            case "falklands":
                                returnstring += "SATL";
                                break;
                            case "afghanistan":
                                returnstring += "AFGN";
                                break;
                            case "iraq":
                                returnstring += "IRAQ";
                                break;
                            default:
                                returnstring += "THTR";
                                break;
                        }
                    }
                    catch
                    {
                    }
                    return returnstring;
                }
                public static string theatertimezonestring(string theater)
                {
                    string returnstring = "";
                    try
                    {

                        switch (theater.ToLower())
                        {
                            case "caucasus":
                                returnstring += "D(Z+4)";
                                break;
                            case "nevada":
                                returnstring += "T(Z-7)";
                                break;
                            case "normandy":
                                returnstring += "A(Z+1)";
                                break;
                            case "persiangulf":
                                returnstring += "C(Z+3)";
                                break;
                            case "thechannel":
                                returnstring += "Z";
                                break;
                            case "syria":
                                returnstring += "C(Z+3)";
                                break;
                            case "sinai":
                                returnstring += "SP(Z+2)";
                                break;
                            case "marianas":
                                returnstring += "ChST(Z+10)";
                                break;
                            case "falklands":
                                returnstring += "FIST(Z-3)";
                                break;
                            case "afghanistan":
                                returnstring += "AFT(Z+4:30)";
                                break;
                            case "iraq":
                                returnstring += "AST(Z+3)";
                                break;
                            default:
                                returnstring += "Z";
                                break;
                        }
                    }
                    catch
                    {
                    }
                    return returnstring;
                }
                public static string reconstructplayercallsign()
                {
                    string returntext = State.currentstate.playercallsign;
                    try
                    {
                        string playercallsign = State.currentstate.playercallsign;  //Boar21 or 401
                        string groupcallsign = Helpers.Common.RemoveDigits(State.currentstate.playercallsign); //Boar or "" 
                        string digits = playercallsign.Replace(groupcallsign, ""); //21 or 401
                        string reconstructplayercallsign;
                        if (groupcallsign.TrimStart().TrimEnd().Equals(""))
                        {
                            reconstructplayercallsign = digits; // 401
                        }
                        else
                        {
                            reconstructplayercallsign = groupcallsign + " " + digits.Substring(0, 1) + "-" + digits.Substring(1, 1); // Boar 2-1
                        }
                        returntext = reconstructplayercallsign;
                    }
                    catch
                    {

                    }
                    return returntext;
                }
                public static string JTAC_processnineline(string text, string sendercallsign)
                {
                    string returntext = text;
                    try
                    {
                        returntext = text.Replace("(" + sendercallsign + ")", "").Replace("Marked by", "Mark").Replace("Heading", "Hdg").Replace("Distance", "Dist").Replace("nautical", "NM").Replace(".", " -").Replace("line is as follows\n", "").Replace("[", "").Replace("]", "").Replace("1, 2, 3 N/A", "1 -\n2 -\n3 -").Replace(":", "").Replace("Elevation", "Elev").Replace("feet", "ft").Replace("Target", "Tgt").Replace("Coordinates", "Coord").Replace("Egress", "Egr").Replace("Friendlies", "Troops").Replace("south", "S").Replace("north", "N").Replace("east", "E").Replace("west", "W");
                    }
                    catch
                    {
                    }
                    return returntext;
                }
                public static string JTAC_processremarks(string text, string sendercallsign)
                {
                    string returntext = text;
                    try
                    {
                        returntext = text.Replace("(" + sendercallsign + "):", "").Replace("\nuse ", "pref ").Replace("\nrequest ", "pref ").Replace("Final attack heading:", "in ").Replace("make your attack heading:", "in ").Replace("meters per second", "m/s").Replace(" at ", " @").Replace(" and ", " + ").Replace("nautical", "NM").Replace("south", "S").Replace("north", "N").Replace("east", "E").Replace("west", "W").Replace("wind", "wnd").Replace(":", "");
                    }
                    catch
                    {
                    }
                    return returntext;
                }
                public static string JTAC_processgeneral(string text, string sendercallsign)
                {
                    string returntext = text;
                    try
                    {
                        returntext = text.Replace("(" + sendercallsign + "):", "").Replace(sendercallsign + ",", "").Replace(reconstructplayercallsign() + ",", "");
                        returntext = returntext.Replace("your target is ", "tgt ");
                        returntext = returntext.TrimStart().TrimEnd();
                    }
                    catch
                    {
                    }
                    return returntext;
                }
                public static string AWACS_processgeneral(string text, string sendercallsign)
                {
                    string returntext = text;
                    try
                    {
                        returntext = returntext.Replace("(" + sendercallsign + "):", "").Replace(sendercallsign + ",", "").Replace(reconstructplayercallsign() + ",", "");
                        returntext = returntext.Replace("home plate ", "home ").Replace("at bulls ", "@").Replace("at bullseye ", "@").Replace("bullseye ", "bulls @").Replace("for", "/");
                        returntext = returntext.Replace("BRA", "");
                        returntext = returntext.Replace(", at ", "@");
                        returntext = returntext.Replace("flanking", "flnk");
                        returntext = returntext.Replace(",", " ");
                        returntext = returntext.Replace("000", "k");
                        returntext = returntext.TrimStart().TrimEnd();
                    }
                    catch
                    {
                    }
                    return returntext;
                }
                public static string ATC_processgeneral(string text, string sendercallsign)
                {
                    string returntext = text;
                    try
                    {
                        returntext = returntext.Replace(reconstructplayercallsign() + ",", "").Replace(sendercallsign + ",", sendercallsign.Substring(0, 3).ToUpper() + " ").Replace("(" + sendercallsign + "):", "").Replace("Ship-ADF,", "");
                        returntext = returntext.Replace(", your heading", " @hdg").Replace("at QFE", "QFE").Replace("climb ", "+").Replace("you are cleared for takeoff when ready,", "TO").Replace("cleared for startup,", "gnd clr").Replace("wind ", "wnd ").Replace("at ", "@").Replace("meters per second", "m/s");
                        returntext = returntext.Replace("fly heading", "hdg").Replace(" for ", " / ").Replace("runway ", "L").Replace("to pattern altitude", "");
                        returntext = returntext.Replace("check landing gear", "");
                        returntext = returntext.Replace("cleared to taxi to", "clr taxi");
                        returntext = returntext.Replace(",", "");
                        returntext = returntext.TrimStart().TrimEnd();
                    }
                    catch
                    {
                    }
                    return returntext;
                }
                public static string Flight_processgeneral(string text, string sendercallsign)
                {
                    string returntext = text;
                    try
                    {
                        returntext = returntext.Replace(reconstructplayercallsign() + ",", "").Replace(sendercallsign + ",", sendercallsign.Substring(0, 3).ToUpper() + " ").Replace("(" + sendercallsign + "):", "").Replace("Ship-ADF,", "");
                        returntext = returntext.Replace("spike", "tgt");
                        returntext = returntext.Replace("spike", "tgt");
                        returntext = returntext.Replace("SPIKE", "spk");
                        returntext = returntext.Replace("contact target", "tgt");
                        returntext = returntext.Replace("contact bearing", "spk");
                        returntext = returntext.Replace("contact", "");
                        returntext = returntext.Replace("target", "tgt");
                        returntext = returntext.Replace("o'clock", "`");
                        returntext = returntext.Replace("for", "/");
                        returntext = returntext.Replace("1,", "").Replace("2,", "").Replace("3,", "").Replace("4,", "");
                        returntext = returntext.Replace(",", "");
                        returntext = returntext.TrimStart().TrimEnd();
                    }
                    catch
                    {
                    }
                    return returntext;
                }
                public static string Carrier_processgeneral(string text, string sendercallsign)
                {
                    string returntext = "init recov\n";
                    try
                    {
                        //308, mother's weather is visibility ten plus miles, 
                        //scattered clouds at 3000, altimeter is 29.93, 
                        // CASE I recovery, expected BRC 354, report see me at 10.

                        // 403, Courage marshal, CASE III recovery, CV-1 approach, expected final bearing 358, altimeter is 29.93, 403, Marshal mother 's 178 radial, 21 DME, angels 6, Expected approach time is 39


                        string[] phrases = text.Split(',');

                        foreach (string phrase in phrases)
                        {

                            // get recovery type
                            List<string> cases = new List<string>() { "CASE III", "CASE II", "CASE I" };
                            foreach (string thiscase in cases)
                            {
                                if (phrase.Contains(thiscase))
                                {
                                    returntext += thiscase + "\n";
                                    break;
                                }
                            }

                            // get clouds type          
                            if (phrase.Contains("clouds"))
                            {
                                returntext += "clouds " + phrase.Replace(Helpers.Common.RemoveDigits(phrase), "") + "\n";
                            }

                            if (phrase.Contains("altimeter"))
                            {
                                returntext += "alti " + phrase.Replace(Helpers.Common.RemoveDigits(phrase.Replace(".", "")), "") + "\n";
                            }

                            if (phrase.Contains("visibility"))
                            {
                                returntext += "vis +" + phrase.Replace(Helpers.Common.RemoveDigits(phrase), "") + "\n";
                            }

                            if (phrase.Contains("radar contact"))
                            {
                                returntext += "rdr " + phrase.Replace(Helpers.Common.RemoveDigits(phrase.Replace(" miles", "")), "") + " NM\n";
                            }

                            if (phrase.Contains("BRC"))
                            {
                                returntext += "BRC " + phrase.Replace(Helpers.Common.RemoveDigits(phrase), "") + "\n";
                            }

                            if (phrase.ToLower().Contains("expected final bearing"))
                            {
                                returntext += "bearing " + phrase.Replace(Helpers.Common.RemoveDigits(phrase), "") + "\n";
                            }

                            if (phrase.ToLower().Contains("radial"))
                            {
                                returntext += "radial" + phrase.Replace(" radial", "").Replace("Marshal mother 's", "") + "\n";
                            }

                            if (phrase.Contains("DME"))
                            {
                                returntext += phrase.Replace(Helpers.Common.RemoveDigits(phrase), "") + "\n";
                            }

                            if (phrase.ToLower().Contains("angels"))
                            {
                                returntext += phrase.Replace(Helpers.Common.RemoveDigits(phrase), "") + "k ft" + "\n";
                            }

                            if (phrase.ToLower().Contains("expected approach time"))
                            {
                                returntext += "time " + phrase.Replace(Helpers.Common.RemoveDigits(phrase), "") + "\n";
                            }

                        }

                    }
                    catch
                    {
                    }
                    return returntext;
                }
                public static string Crew_processgeneral(string text, string sendercallsign)
                {
                    string returntext = text;
                    try
                    {
                        returntext = returntext.Replace("Ground Crew: ", "").Replace("rearming complete", "stores signoff").Replace("refueling complete", "fuel max lbs");
                        returntext = returntext.TrimStart().TrimEnd();

                    }
                    catch
                    {
                    }
                    return returntext;
                }
                public static string Tanker_processgeneral(string text, string sendercallsign)
                {
                    string returntext = text;
                    try
                    {
                        returntext = returntext.Replace("(" + sendercallsign + "):", "").Replace(sendercallsign + ",", "").Replace(reconstructplayercallsign() + ",", "");
                        returntext = returntext.Replace("proceed to pre-contact at ", "AAR").Replace("at velocity", "/kts");
                        returntext = returntext.TrimStart().TrimEnd();

                    }
                    catch
                    {
                    }
                    return returntext;
                }
                public static string ProcessMessageByEvent(Server.ServerCommsMessage message)
                {

                    try
                    {

                        string summarynote = "";
                        string sendercallsign = State.currentstate.playercallsign; //default

                        string[] split1 = message.text.Split('(', ')');
                        if (split1.Length > 1)
                        {
                            sendercallsign = split1[1];
                        }
                        else
                        {
                            string[] split2 = message.text.Split(':');
                            if (split2.Length > 1)
                            {
                                sendercallsign = split2[1];
                            }
                            else
                            {
                                sendercallsign = "";
                            }
                        }

                        string dcsid = message.eventkey;
                        switch (dcsid)
                        {

                            // TANKER -----------------------------------------------

                            case "wMsgTankerClearedPreContact": //4370
                                summarynote = "*" + Tanker_processgeneral(message.text, sendercallsign);
                                break;
                            case "wMsgTankerRefuelingComplete": // 4376:
                                summarynote = "*AAR max lbs";
                                break;

                            // JTAC -----------------------------------------------

                            // cleared hot
                            case "wMsgFAC_CLERED_HOT": // 4384:
                                summarynote = "*clr hot";
                                break;
                            case "wMsgFAC_CLEARED_TO_ENGAGE": // 4385: 
                                summarynote = "*clr hot";
                                break;
                            case "wMsgFAC_CLEARED_TO_DEPART": //4386:
                                summarynote = "*exit";
                                break;
                            //case 4387:
                            //   summarynote = "*exit";
                            //   break;
                            case "wMsgFACNoTaskingAvailable": // 4388:
                                summarynote = "*exit";
                                break;
                            // type X in effect
                            case "wMsgFACType1InEffectAdviseWhenReadyFor9Line": // 4389: 
                                summarynote = "*type 1";
                                break;
                            case "wMsgFACType2InEffectAdviseWhenReadyFor9Line": // 4390: 
                                summarynote = "*type 2";
                                break;
                            case "wMsgFACType3InEffectAdviseWhenReadyFor9Line": // 4391: 
                                summarynote = "*type 3";
                                break;
                            // (9-line)
                            case "wMsgFAC9lineBrief": // 4402: 
                                summarynote = JTAC_processnineline(message.text, sendercallsign);
                                break;
                            case "wMsgFAC9lineBriefWP": //  4403: 
                                summarynote = JTAC_processnineline(message.text, sendercallsign);
                                break;
                            case "wMsgFAC9lineBriefWPLaser": // 4404:
                                summarynote = JTAC_processnineline(message.text, sendercallsign);
                                break;
                            case "wMsgFAC9lineBriefIRPointer": // 4405: 
                                summarynote = JTAC_processnineline(message.text, sendercallsign);
                                break;
                            case "wMsgFAC9lineBriefLaser": // 4406: 
                                summarynote = JTAC_processnineline(message.text, sendercallsign);
                                break;
                            // remarks
                            case "wMsgFAC9lineRemarks": // 4407: 
                                summarynote = JTAC_processremarks(message.text, sendercallsign);
                                break;
                            // your target is..
                            case "wMsgFACTargetDescription": /// 4408:
                                summarynote = JTAC_processgeneral(message.text, sendercallsign);
                                break;
                            // BDA
                            case "wMsgFACTargetHit": // 4409:
                                summarynote = "tgt impact";
                                break;
                            case "wMsgFACTargetDestroyed": // 4410:
                                summarynote = "*BDA destr";
                                break;
                            case "wMsgFACTargetPartiallyDestroyed": // 4411:
                                summarynote = "*BDA %";
                                break;
                            case "wMsgFACTargetNotDestroyed": // 4412:
                                summarynote = "*BDA miss";
                                break;
                            // use weapon..
                            case "wMsgUseWeapon": // 4413:
                                summarynote = JTAC_processgeneral(message.text, sendercallsign);
                                break;
                            // from the mark..
                            case "wMsgFACFromTheMark": // 4415:
                                summarynote = JTAC_processgeneral(message.text, sendercallsign);
                                break;

                            // AWACS -----------------------------------------------

                            // vctr bullseye
                            case "wMsgAWACSVectorToBullseye": // 4356:
                                summarynote = "*" + AWACS_processgeneral(message.text, sendercallsign);
                                break;
                            // bandit bearing for miles
                            case "wMsgAWACSBanditBearingForMiles": // 4357:
                                summarynote = "*" + AWACS_processgeneral(message.text, sendercallsign);
                                break;
                            // vector to bandit
                            case "wMsgAWACSVectorToNearestBandit": // 4358:
                                summarynote = "*" + AWACS_processgeneral(message.text, sendercallsign);
                                break;
                            // popup group
                            case "wMsgAWACSPopUpGroup": // 4359:
                                summarynote = "*" + AWACS_processgeneral(message.text, sendercallsign);
                                break;
                            // vector home
                            case "wMsgAWACSHomeBearing": // 4360:
                                summarynote = "*" + AWACS_processgeneral(message.text, sendercallsign);
                                break;
                            // vector tanker
                            case "wMsgAWACSTankerBearing": // 4361:
                                summarynote = "*" + AWACS_processgeneral(message.text, sendercallsign);
                                break;
                            // picture
                            case "wMsgAWACSPicture": // 4367:
                                summarynote = "*" + AWACS_processgeneral(message.text, sendercallsign);
                                break;


                            // ATC -----------------------------------------------

                            // cleared for startup
                            case "wMsgATCClearedForEngineStartUp": // 4267:
                                summarynote = "*" + ATC_processgeneral(message.text, sendercallsign);
                                break;
                            // cleared taxi runway
                            case "wMsgATCClearedToTaxiRunWay": // 4269:
                                summarynote = "*" + ATC_processgeneral(message.text, sendercallsign);
                                break;
                            // cleared for takeoff
                            case "wMsgATCYouAreClearedForTO": // 4272:
                                summarynote = "*" + ATC_processgeneral(message.text, sendercallsign);
                                break;
                            // traffic bearing
                            case "wMsgATCTrafficBearing": //  4275:
                                summarynote = "*" + ATC_processgeneral(message.text, sendercallsign);
                                break;
                            // fly heading
                            case "wMsgATCFlyHeading": // 4285:
                                summarynote = "*" + ATC_processgeneral(message.text, sendercallsign);
                                break;
                            // azimuth
                            case "wMsgATCAzimuth": //  4286:
                                summarynote = "*" + ATC_processgeneral(message.text, sendercallsign);
                                break;

                            // chck landing gear
                            case "wMsgATCCheckLandingGear": // 4290:
                                summarynote = "*" + ATC_processgeneral(message.text, sendercallsign);
                                break;

                            // CARRIER -----------------------------------------------
                            // carrier
                            // mother weather...
                            case "wMsgATCMarshallCopyInbound": // 4296:
                                summarynote = "*" + Carrier_processgeneral(message.text, sendercallsign);
                                break;
                            case "wMsgATCMarshallCopyInbound2and3":
                                summarynote = "*" + Carrier_processgeneral(message.text, sendercallsign);
                                break;
                            // final bearing
                            case "wMsgATCTowerFinalBearing": // 4306:
                                summarynote = "*" + Carrier_processgeneral(message.text, sendercallsign);
                                break;

                            // CREW -----------------------------------------------
                            // crew
                            // rearm complete
                            case "wMsgGroundCrewReloadDone": // 4441:
                                summarynote = "*" + "REF " + Crew_processgeneral(message.text, sendercallsign);
                                break;
                            // rearm complete
                            case "wMsgGroundCrewRefuelDone":
                                summarynote = "*" + "REF " + Crew_processgeneral(message.text, sendercallsign);
                                break;

                            // FLIGHT -----------------------------------------------

                            case "wMsgWingmenRadarContact": // 4187: //radar contact
                                summarynote = "*" + Flight_processgeneral(message.text, sendercallsign);
                                break;

                            case "wMsgWingmenContact": // 4188: //contact
                                summarynote = "*" + Flight_processgeneral(message.text, sendercallsign);
                                break;

                            case "wMsgWingmenTallyBandit": //  4189: //tally bandit
                                summarynote = "*" + Flight_processgeneral(message.text, sendercallsign);
                                break;

                            case "wMsgWingmenNails": //  4190: //nails
                                summarynote = "*" + Flight_processgeneral(message.text, sendercallsign);
                                break;

                            case "wMsgWingmenSpike": // 4191: //spike
                                summarynote = "*" + Flight_processgeneral(message.text, sendercallsign);
                                break;

                            case "wMsgWingmenMudSpike": // 4192: //mudspike
                                summarynote = "*" + Flight_processgeneral(message.text, sendercallsign);
                                break;

                            case "wMsgWingmenBanditDestroyed": // 4241: //bandit destr
                                summarynote = "*" + Flight_processgeneral(message.text, sendercallsign);
                                break;

                            case "wMsgWingmenTargetDestroyed": // 4242: //tgt destr
                                summarynote = "*" + Flight_processgeneral(message.text, sendercallsign);
                                break;

                            case "wMsgWingmenSplashMyBandit": // 4245: //splash
                                summarynote = "*" + Flight_processgeneral(message.text, sendercallsign);
                                break;

                            default: // no processing for this mesage
                                break;
                        }

                        string sendercategory = Database.Dcs.SenderCatByString(dcsid).ToString().ToUpper();
                        summarynote = summarynote.Replace(sendercategory, "").TrimStart().TrimEnd();

                        Log.Write("(sum note):" + summarynote, Colors.Inline);

                        return summarynote;

                    }
                    catch (Exception e)
                    {
                        Log.Write(e.Message + e.StackTrace, Colors.Inline);
                        return "";
                    }

                }

            }

        }
    }
}