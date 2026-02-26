using System;
using System.Collections.Generic;

namespace VAICOM
{
    namespace Servers
    {

        public static partial class Server
        {

            public class RadioDevice
            {
                public int deviceid;
                public bool isavailable;
                public bool isselected;
                public string displayName;
                public bool AM;
                public bool FM;
                public bool on;
                public string frequency;
                public string modulation;
                public bool intercom;
                public string volume;
                public List<Server.DcsUnit> tunedunits;

            }

            public class Vector
            {
                public double x;
                public double y;
                public double z;
            }

            public class FreqMod
            {
                public double frequency;
                public string modulation;
            }

            public class FreqModTbl
            {
                public int transiverId;
                public FreqMod freqMod;
            }

            public class DcsSoundConfig
            {
                public bool GBreathEffect;
                public int cockpit;
                public int gui;
                public int headphones;
                public bool headphones_on_external_views;
                public bool hear_in_helmet;
                public string hp_output;
                public string main_output;
                public int microphone_use;
                public int music;
                public bool radioSpeech;
                public bool subtitles;
                public int switches;
                public bool voice_chat;
                public string voice_chat_input;
                public string voice_chat_output;
                public int volume;
                public int world;

                public DcsSoundConfig()
                {
                    volume = 50;
                    cockpit = 50;
                    gui = 50;
                    headphones = 50;
                    world = 50;
                    music = 50;
                    switches = 100;
                    headphones_on_external_views = true;
                    hear_in_helmet = true;
                }
            }

            public class DcsUnit
            {
                public int index;
                public int id_;
                public string callsign;
                public int range;
                public Vector pos;
                public string reccat;
                public string descr;
                public string fullname;
                public string coalition;
                public string freq;
                public List<string> altfreq;
                public string mod;
                public string status;
                public bool ishuman;
                public bool allowtuning;
                public string playerid;

                public DcsUnit()
                {
                    id_ = -1;
                    allowtuning = true;
                }

                public string getrangestr()
                {
                    string rangestr = (range / 1852).ToString();
                    return rangestr;
                }

                public string getbearingstr()
                {
                    Server.DcsUnit playerunit = new Server.DcsUnit();
                    try
                    {
                        playerunit = State.currentstate.availablerecipients["Player"][0];
                    }
                    catch
                    {
                    }
                    string bearing = ("00" + Helpers.Common.Modulo((int)((Math.Round((Math.Atan2((pos.z - playerunit.pos.z), (pos.x - playerunit.pos.x))) * (180 / Math.PI)))), 360).ToString());
                    bearing = bearing.Substring(bearing.Length - 3);
                    return bearing;
                }

                public string getaltstr()
                {
                    string elevstr = (100 * Math.Round((pos.y) * 3.2808399 / 100)).ToString();
                    return elevstr;
                }

                public string getmodstr()
                {
                    string modul = mod;
                    if (mod.Equals("XX"))
                    {
                        modul = "AM";
                    }
                    return modul;
                }

                public string getfreqstr()
                {
                    string normalizedfreq = Helpers.Common.NormalizeFreqString(freq); //+" MHz"
                    return normalizedfreq;
                }

                public string isunittuned()
                {
                    return "";
                }

                public string gethumanname()
                {
                    if (ishuman)
                    {
                        return playerid;
                    }
                    else
                    {
                        return "";
                    }
                }
            }


            public class DcsPluginsConfig
            {
                public DcsVaicomConfig VAICOM;
            }

            public class DcsVaicomConfig
            {
                public bool VAICOMDebugModeEnabled;
                public string VAICOMClientIP;
            }

            public class DcsCarrierConfig
            {
                // tbd
            }

            public class DcsOptions
            {
                public DcsSoundConfig sound;
                public DcsPluginsConfig plugins;

                public DcsOptions()
                {
                    sound = new DcsSoundConfig();
                    plugins = new DcsPluginsConfig();
                }

            }

            public class TomcatState
            {
                public bool canopy;
                public bool rdr;
                public bool pdstt;
                public bool pstt;
                public bool amt;
                public double tcn;
                public bool ics;
                public bool sngl;
                public bool jmr;
                public bool AM182;
                public bool ejsn;
                public int markers;

            }

            public class payloadcannon
            {
                public int shells;
            }

            public class payloaddescr
            {
                public int level1;
                public int level2;
                public int level3;
                public int level4;
            }

            public class payloadstation
            {
                public string CLSID;
                public bool container;
                public int count;
                public payloaddescr weapon;
                public payloaddescr wstype;
                public payloaddescr adapter;
            }

            public class payload
            {
                public payloadcannon Cannon;
                public int CurrentStation;
                public List<payloadstation> Stations;
            }

            public class campos
            {
                public int type;
                public Vector loc;
            }


            public class ServerState
            {

                public string client;
                public string clientversion;
                public string mode;
                public string type;
                public int chunkid;

                public string dcsversion;
                public string root;
                public bool multiplayer;
                public bool vrmode;
                public bool easycomms;
                public bool pausebasestate;
                public string theatre;
                public string sortie;
                public string task;
                public string country;
                public DcsOptions options;

                public string missiontitle;
                public string missionbriefing;
                public string missiondetails;

                public double timer;
                public double tod;
                public string id;
                public string playerusername;
                public string playercallsign;
                public string playercoalition;
                public int playerunitid;
                public string playerunitcat;
                public bool airborne;
                public TomcatState riostate;
                public payload payload;
                public Vector bpos;
                public campos cpos;
                public string fsmstate;

                public bool viewexternal;
                public bool soundsallowexternal;

                public ServerAuxmenu menuaux;
                public ServerAuxmenu menucargo;

                public object mission;
                public object missioncmds;

                public int intercom;
                public string selectedradio;
                public List<RadioDevice> radios;

                public Dictionary<string, List<DcsUnit>> availablerecipients;
                public List<DcsUnit> TACANunits;
                public List<DcsUnit> DLunits;

                public ServerState()
                {
                    dcsversion = "";
                    playercallsign = "";
                    availablerecipients = new Dictionary<string, List<DcsUnit>>()
                    {
                        {"Player",  new List<DcsUnit>()},
                        {"Flight",  new List<DcsUnit>()},
                        {"JTAC",    new List<DcsUnit>()},
                        {"ATC",     new List<DcsUnit>()},
                        {"AWACS",   new List<DcsUnit>()},
                        {"Tanker",  new List<DcsUnit>()},
                        {"Crew",    new List<DcsUnit>()},
                        {"Aux",     new List<DcsUnit>()},
                        {"Cargo",   new List<DcsUnit>()},
                        {"Allies",  new List<DcsUnit>()},
                    };
                    TACANunits = new List<DcsUnit>();
                    DLunits = new List<DcsUnit>();
                    id = "----";
                    radios = new List<RadioDevice>();
                    menuaux = null;
                    menucargo = null;
                    mission = null;
                    bpos = new Vector(); // added
                    cpos = new campos();
                    options = new DcsOptions();

                }
            }

            public class ServerMessage
            {
                public int cid;

                public string client;
                public string clientversion;
                public string mode;
                public string type;

                public string dcsversion;
                public string root;
                public bool multiplayer;
                public bool vrmode;
                public bool easycomms;
                public bool pausebasestate;
                public string theatre;
                public string sortie;
                public string task;
                public string country;
                public DcsOptions options;

                public double timer;
                public double tod;
                public string id;
                public string playerusername;
                public string playercallsign;
                public string playercoalition;
                public int playerunitid;
                public string playerunitcat;
                public bool airborne;
                public TomcatState riostate;
                public payload payload;
                public Vector bpos;
                public campos cpos;
                public string fsmstate;
                public int intercom;
                public string selectedradio;
                public List<RadioDevice> radios;

                public string missiontitle;
                public string missionbriefing;
                public string missiondetails;

                public object mission;
                public object missioncmds;

                public Dictionary<string, List<DcsUnit>> availablerecipients;

                public ServerAuxmenu menuaux;
                public ServerAuxmenu menucargo;


            }

        }
    }
}
