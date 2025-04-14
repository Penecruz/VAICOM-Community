namespace VAICOM
{

    namespace Database
    {

        public class Recipient
        {

            public int uniqueid;
            public string name;
            public string displayname;
            public RecipientCategories category;

            public bool requiresJester;
            public bool requiresrealatc;
            public bool enabled;
            public bool blockedforFree;
            public bool isallowed;
            public bool hasunit;
            public int unitid;
            public int flightnumber;

            public Recipientclass RecipientClass()
            {
                Recipientclass value = Recipientclasses.Undefined;

                if ((this.uniqueid >= Recipients.Table["iDeviceNull"].uniqueid) & (this.uniqueid <= Recipients.Table["iDeviceMaximum"].uniqueid)) { value = Recipientclasses.Cockpit; }

                if ((this.uniqueid >= Recipients.Table["wAIUnitFlightNull"].uniqueid) & (this.uniqueid <= Recipients.Table["wAIUnitFlightMaximum"].uniqueid)) { value = Recipientclasses.Flight; }
                if ((this.uniqueid >= Recipients.Table["wAIUnitJTACNull"].uniqueid) & (this.uniqueid <= Recipients.Table["wAIUnitJTACMaximum"].uniqueid)) { value = Recipientclasses.JTAC; }
                if ((this.uniqueid >= Recipients.Table["wAIUnitATCNull"].uniqueid) & (this.uniqueid < Recipients.Table["wAIUnitATCMaximum"].uniqueid)) { value = Recipientclasses.ATC; }
                if ((this.uniqueid >= Recipients.Table["wAIUnitAWACSNull"].uniqueid) & (this.uniqueid <= Recipients.Table["wAIUnitAWACSMaximum"].uniqueid)) { value = Recipientclasses.AWACS; }
                if ((this.uniqueid >= Recipients.Table["wAIUnitTankerNull"].uniqueid) & (this.uniqueid <= Recipients.Table["wAIUnitTankerMaximum"].uniqueid)) { value = Recipientclasses.Tanker; }
                if ((this.uniqueid >= Recipients.Table["wAIUnitCrewNull"].uniqueid) & (this.uniqueid <= Recipients.Table["wAIUnitCrewMaximum"].uniqueid)) { value = Recipientclasses.Crew; }
                if ((this.uniqueid >= Recipients.Table["wAIUnitAOCSNull"].uniqueid) & (this.uniqueid <= Recipients.Table["wAIUnitAOCSMaximum"].uniqueid)) { value = Recipientclasses.AOCS; }
                if ((this.uniqueid >= Recipients.Table["wAIUnitAuxNull"].uniqueid) & (this.uniqueid <= Recipients.Table["wAIUnitAuxMaximum"].uniqueid)) { value = Recipientclasses.Aux; }
                if ((this.uniqueid >= Recipients.Table["wAIUnitCargoNull"].uniqueid) & (this.uniqueid <= Recipients.Table["wAIUnitCargoMaximum"].uniqueid)) { value = Recipientclasses.Cargo; }
                if ((this.uniqueid >= Recipients.Table["wAIUnitDescentNull"].uniqueid) & (this.uniqueid <= Recipients.Table["wAIUnitDescentMaximum"].uniqueid)) { value = Recipientclasses.Descent; }

                // new carrier comms
                // in atc range
                if ((this.uniqueid >= Recipients.Table["wAIUnitATCCarriersNull"].uniqueid) & (this.uniqueid < Recipients.Table["wAIUnitATCCarriersMaximum"].uniqueid)) { value = Recipientclasses.ATC; }

                // RIO
                if ((this.uniqueid >= Recipients.Table["wAIUnitFlightCrewMembersNull"].uniqueid) & (this.uniqueid <= Recipients.Table["wAIUnitFlightCrewMembersMaximum"].uniqueid)) { value = Recipientclasses.RIO; }

                //Kneeboard
                if ((this.uniqueid >= Recipients.Table["wAIUnitKneeboardNull"].uniqueid) & (this.uniqueid <= Recipients.Table["wAIUnitKneeboardMaximum"].uniqueid)) { value = Recipientclasses.Kneeboard; }

                // Moose Ops
                if ((this.uniqueid >= Recipients.Table["wAIUnitMooseNull"].uniqueid) & (this.uniqueid <= Recipients.Table["wAIUnitMooseMaximum"].uniqueid)) { value = Recipientclasses.Moose; }

                return value;
            }

            public bool isCarrier()
            {
                return (this.uniqueid >= Recipients.Table["wAIUnitATCCarriersNull"].uniqueid) & (this.uniqueid <= Recipients.Table["wAIUnitATCCarriersMaximum"].uniqueid);
            }

            public bool isKneeboard()
            {
                return (this.uniqueid >= Recipients.Table["wAIUnitKneeboardNull"].uniqueid) & (this.uniqueid <= Recipients.Table["wAIUnitKneeboardMaximum"].uniqueid);
            }

        }

        public class Recipientclass
        {
            public string Name;
        }

        public static class Recipientclasses
        {
            public static Recipientclass GetByName(string cat)
            {
                Recipientclass returnclass;

                switch (cat.ToLower())
                {
                    case ("undefined"):
                        returnclass = Undefined;
                        break;
                    case ("cockpit"):
                        returnclass = Cockpit;
                        break;
                    case ("player"):
                        returnclass = Player;
                        break;
                    case ("navy_player"):
                        returnclass = Navy_Player;
                        break;
                    case ("flight"):
                        returnclass = Flight;
                        break;
                    case ("jtac"):
                        returnclass = JTAC;
                        break;
                    case ("atc"):
                        returnclass = ATC;
                        break;
                    case ("tanker"):
                        returnclass = Tanker;
                        break;
                    case ("awacs"):
                        returnclass = AWACS;
                        break;
                    case ("crew"):
                        returnclass = Crew;
                        break;
                    case ("aocs"):
                        returnclass = AOCS;
                        break;
                    case ("aux"):
                        returnclass = Aux;
                        break;
                    case ("cargo"):
                        returnclass = Cargo;
                        break;
                    case ("descent"):
                        returnclass = Descent;
                        break;
                    case ("atc_navy_carrier"):
                        returnclass = ATC_NAVY_CARRIER;
                        break;
                    case ("atc_navy_approach_tower"):
                        returnclass = ATC_NAVY_Approach_Tower;
                        break;
                    case ("atc_navy_departure"):
                        returnclass = ATC_NAVY_Departure;
                        break;
                    case ("atc_navy_lso"):
                        returnclass = ATC_NAVY_LSO;
                        break;
                    case ("atc_navy_marshal"):
                        returnclass = ATC_NAVY_Marshal;
                        break;
                    case ("rio"):
                        returnclass = RIO;
                        break;
                    case ("ai_pilot"):
                        returnclass = AI_pilot;
                        break;
                    case ("kneeboard"):
                        returnclass = Kneeboard;
                        break;

                    case ("Moose"):
                        returnclass = Moose;
                        break;

                    default:
                        returnclass = Undefined;
                        break;


                }

                return returnclass;
            }


            public static Recipientclass Undefined = new Recipientclass { Name = "Undefined" };

            public static Recipientclass Cockpit = new Recipientclass { Name = "Cockpit" };

            public static Recipientclass Player = new Recipientclass { Name = "Player" };
            public static Recipientclass Navy_Player = new Recipientclass { Name = "Navy_Player" };
            public static Recipientclass Flight = new Recipientclass { Name = "Flight" };
            public static Recipientclass JTAC = new Recipientclass { Name = "JTAC" };
            public static Recipientclass ATC = new Recipientclass { Name = "ATC" };
            public static Recipientclass Tanker = new Recipientclass { Name = "Tanker" };
            public static Recipientclass AWACS = new Recipientclass { Name = "AWACS" };
            public static Recipientclass Crew = new Recipientclass { Name = "Crew" };
            public static Recipientclass AOCS = new Recipientclass { Name = "AOCS" };
            public static Recipientclass Aux = new Recipientclass { Name = "Aux" };
            public static Recipientclass Cargo = new Recipientclass { Name = "Cargo" };
            public static Recipientclass Descent = new Recipientclass { Name = "Descent" };

            // new carrier comms: add own recipient class?
            public static Recipientclass ATC_NAVY_CARRIER = new Recipientclass { Name = "Carrier ATC" };
            public static Recipientclass ATC_NAVY_Approach_Tower = new Recipientclass { Name = "Carrier ATC Approach Tower" };
            public static Recipientclass ATC_NAVY_Departure = new Recipientclass { Name = "Carrier ATC Departure" };
            public static Recipientclass ATC_NAVY_LSO = new Recipientclass { Name = "Carrier ATC LSO" };
            public static Recipientclass ATC_NAVY_Marshal = new Recipientclass { Name = "Carrier ATC Marshal" };

            public static Recipientclass RIO = new Recipientclass { Name = "RIO" };
            public static Recipientclass AI_pilot = new Recipientclass { Name = "Iceman" };

            public static Recipientclass Kneeboard = new Recipientclass { Name = "Kneeboard" };

            // Moose direct integrated comms
            public static Recipientclass Moose = new Recipientclass { Name = "Moose" };

        }



        public enum RecipientCategories
        {
            player,
            undefined,
            aiunit,
            aiflight,
            aijtac,
            aiatc,
            aifarp,
            aiship,
            aitanker,
            aiawacs,
            aicrew,
            aocs,
            cockpitdevice,
            auxmenu,
            cargo,
            RIO,
            AI_pilot,
            ally,
            kneeboard,
            Moose,
        }


    }

}
