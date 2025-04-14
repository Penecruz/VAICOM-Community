using VAICOM.Servers;
using VAICOM.Static;

namespace VAICOM
{

    namespace Database
    {

        public class Command
        {
            public int uniqueid;

            public string displayname;
            public CommandCategories category;
            public bool enabled;

            public int eventnumber;
            public string dcsid;
            public int geteventnumber()
            {
                return Database.Dcs.GetEventNumber(dcsid);
            }

            public bool hasparameter;

            public bool engagecondition;
            public bool on;
            public int type;
            public int device;
            public int power_source;
            public int recoverycase;
            public string parametername;
            public bool value;
            public bool close;
            public double volume;
            public int parameters;
            public bool readback;
            public Server.Vector point;

            public bool useappendix;
            public bool useweapon;
            public int weapon;
            public bool usedirection;
            public double direction;

            public bool blockedforFC;
            public bool blockedforFCnonPro;
            public bool blockedforFree;
            public bool requiresJester;
            public bool requiresrealatc;

            public string servername;
            public string menuitemname;
            public int actionIndex;

            public Command()
            {
                uniqueid = 0;
                eventnumber = 0;
                dcsid = "";
            }

            public bool RequiresFlightNumInsert()
            {
                bool value = false;
                if (this.RecipientClass().Equals(Recipientclasses.Flight)) { value = true; }
                return value;
            }

            public bool isReply()
            {
                bool value = false;
                if ((this.uniqueid >= Commands.Table["wMsgReplyNull"].uniqueid) & (this.uniqueid <= Commands.Table["wMsgReplyMaximum"].uniqueid)) { value = true; }
                return value;
            }


            public bool isState()
            {
                bool value = false;
                if (this.dcsid.Equals("wMsgProvideState") || this.dcsid.Equals("wMsgReadBriefing")) { value = true; }
                return value;
            }

            public bool isSelect()
            {
                bool value = false;
                if (this.dcsid.Equals("wMsgSelectRecipient")) { value = true; }
                return value;
            }

            public bool isOptions()
            {
                bool value = false;
                if (this.dcsid.Equals("wMsgShowOptions")) { value = true; }
                return value;
            }

            public bool isSwitch()
            {
                return this.dcsid.Equals("wMsgSwitchListening");
            }

            public bool isMenu()
            {
                bool value = false;
                if ((this.uniqueid >= Commands.Table["menu01"].uniqueid) & (this.uniqueid <= Commands.Table["menu12"].uniqueid)) { value = true; }
                return value;
            }

            public bool isSpecial()
            {
                return this.eventnumber.Equals(4000);
            }

            public bool isEngage()
            {
                bool value = false;
                if ((this.uniqueid >= Commands.Table["groundtarget"].uniqueid) & (this.uniqueid <= Commands.Table["ship"].uniqueid)) { value = true; }
                return value;
            }


            public bool isInputcommand()
            {
                bool value = false;
                if ((this.uniqueid >= Commands.Table["iCommandNull"].uniqueid) & (this.uniqueid < Commands.Table["iCommandMaximum"].uniqueid)) { value = true; }
                return value;
            }

            public bool hasAppendix()
            {
                bool value = false;
                if ((this.uniqueid >= Commands.Table["groundtarget"].uniqueid) & (this.uniqueid <= Commands.Table["ship"].uniqueid)) { value = true; }
                return value;
            }

            public bool isCarrier()
            {
                return (this.uniqueid >= Commands.Table["wMsgLeaderToNavyATCNull"].uniqueid) & (this.uniqueid <= Commands.Table["wMsgLeaderToNavyATCMaximum"].uniqueid);
            }

            public Recipientclass SenderCat()
            {
                Recipientclass value = Recipientclasses.Undefined;
                if ((this.uniqueid >= Commands.Table["wMsgLeaderNull"].uniqueid) & (this.uniqueid <= Commands.Table["wMsgLeaderMaximum"].uniqueid)) { value = Recipientclasses.Player; }
                return value;
            }

            public Recipientclass RecipientClass()
            {
                Recipientclass value = Recipientclasses.Undefined;

                if ((this.uniqueid >= Commands.Table["iCommandNull"].uniqueid) & (this.uniqueid <= Commands.Table["iCommandMaximum"].uniqueid)) { value = Recipientclasses.Cockpit; }

                if ((this.uniqueid >= Commands.Table["wMsgReplyNull"].uniqueid) & (this.uniqueid <= Commands.Table["wMsgReplyMaximum"].uniqueid)) { value = Recipientclasses.Undefined; }
                if ((this.uniqueid >= Commands.Table["wMsgMenuNull"].uniqueid) & (this.uniqueid <= Commands.Table["wMsgMenuMaximum"].uniqueid)) { value = Recipientclasses.Undefined; }

                if ((this.uniqueid >= Commands.Table["wMsgLeaderToWingmenNull"].uniqueid) & (this.uniqueid <= Commands.Table["wMsgLeaderToWingmenMaximum"].uniqueid)) { value = Recipientclasses.Flight; }
                if ((this.uniqueid >= Commands.Table["wMsgLeaderToFACNull"].uniqueid) & (this.uniqueid <= Commands.Table["wMsgLeaderToFACMaximum"].uniqueid)) { value = Recipientclasses.JTAC; }
                if ((this.uniqueid >= Commands.Table["wMsgLeaderToATCNull"].uniqueid) & (this.uniqueid <= Commands.Table["wMsgLeaderToATCMaximum"].uniqueid)) { value = Recipientclasses.ATC; }
                if ((this.uniqueid >= Commands.Table["wMsgLeaderToAWACSNull"].uniqueid) & (this.uniqueid <= Commands.Table["wMsgLeaderToAWACSMaximum"].uniqueid)) { value = Recipientclasses.AWACS; }
                if ((this.uniqueid >= Commands.Table["wMsgLeaderToTankerNull"].uniqueid) & (this.uniqueid <= Commands.Table["wMsgLeaderToTankerMaximum"].uniqueid)) { value = Recipientclasses.Tanker; }
                if ((this.uniqueid >= Commands.Table["wMsgLeaderToGroundCrewNull"].uniqueid) & (this.uniqueid <= Commands.Table["wMsgLeaderToGroundCrewMaximum"].uniqueid)) { value = Recipientclasses.Crew; }
                if ((this.uniqueid >= Commands.Table["wMsgAOCSCmndsNull"].uniqueid) & (this.uniqueid <= Commands.Table["wMsgAOCSCmndsMaximum"].uniqueid)) { value = Recipientclasses.AOCS; }
                if ((this.uniqueid >= Commands.Table["wMsgLeaderToAuxNull"].uniqueid) & (this.uniqueid <= Commands.Table["wMsgLeaderToAuxMaximum"].uniqueid)) { value = Recipientclasses.Aux; }
                if ((this.uniqueid >= Commands.Table["wMsgLeaderToCargoNull"].uniqueid) & (this.uniqueid <= Commands.Table["wMsgLeaderToCargoMaximum"].uniqueid)) { value = Recipientclasses.Cargo; }
                if ((this.uniqueid >= Commands.Table["wMsgLeaderToDescentNull"].uniqueid) & (this.uniqueid <= Commands.Table["wMsgLeaderToDescentMaximum"].uniqueid)) { value = Recipientclasses.Descent; }

                // new carrier comms: ATC 
                if ((this.uniqueid >= Commands.Table["wMsgLeaderToNavyATCNull"].uniqueid) & (this.uniqueid <= Commands.Table["wMsgLeaderToNavyATCMaximum"].uniqueid)) { value = Recipientclasses.ATC; }

                // RIO extension
                if ((this.uniqueid >= Commands.Table["wMsgRIOCmndsNull"].uniqueid) & (this.uniqueid <= Commands.Table["wMsgRIOCmndsMaximum"].uniqueid)) { value = Recipientclasses.RIO; }
                if ((this.uniqueid >= Commands.Table["wMsgAIPilotCmndsNull"].uniqueid) & (this.uniqueid <= Commands.Table["wMsgAIPilotCmndsMaximum"].uniqueid)) { value = Recipientclasses.AI_pilot; }

                // Kneeboard
                if ((this.uniqueid >= Commands.Table["wMsgKneeboardCmndsNull"].uniqueid) & (this.uniqueid <= Commands.Table["wMsgKneeboardCmndsMaximum"].uniqueid)) { value = Recipientclasses.Kneeboard; }

                // Moose
                if ((this.uniqueid >= Commands.Table["wMsgLeaderToMooseCmndsNull"].uniqueid) & (this.uniqueid <= Commands.Table["wMsgLeaderToMooseCmndsMaximum"].uniqueid)) { value = Recipientclasses.Moose; } //Leader??

                return value;
            }

            public string ApplicationType()

            {
                string value = Messagetypes.Undefined;

                if ((this.uniqueid >= Commands.Table["iCommandNull"].uniqueid) & (this.uniqueid < Commands.Table["iCommandMaximum"].uniqueid)) { value = Messagetypes.DeviceControl; }
                if ((this.uniqueid >= Commands.Table["wMsgNull"].uniqueid) & (this.uniqueid < Commands.Table["wMsgMaximum"].uniqueid)) { value = Messagetypes.CommsMessage; }
                if ((this.uniqueid == Commands.Table["select"].uniqueid)) { value = Messagetypes.CommsMessage; }

                return value;
            }

            //RIO
            public bool isRIO()
            {
                bool value = false;
                if ((this.uniqueid >= Commands.Table["wMsgRIOCmndsNull"].uniqueid) & (this.uniqueid <= Commands.Table["wMsgAIPilotCmndsMaximum"].uniqueid)) { value = true; }
                return value;
            }

            public bool isVoid()
            {

                return (this.isSpecial() & !this.isOptions() & !this.isSelect() & !this.isMenu() & !this.isState() & !this.isRIO());

            }


            public bool RequiresInsert()
            {
                bool value = false;

                if (this.RecipientClass().Equals(Recipientclasses.JTAC)) { value = true; }
                if (this.RecipientClass().Equals(Recipientclasses.ATC)) { value = true; }
                if (this.RecipientClass().Equals(Recipientclasses.Tanker)) { value = true; }
                if (this.RecipientClass().Equals(Recipientclasses.AWACS)) { value = true; }

                return value;
            }


            public bool isKneeboard()
            {
                return ((this.uniqueid >= Commands.Table["wMsgKneeboardCmndsNull"].uniqueid) & (this.uniqueid <= Commands.Table["wMsgKneeboardCmndsMaximum"].uniqueid));
            }
        }

        public enum CommandCategories
        {
            undefined,
            aicomms,
            aicommsflight,
            aicommsflightengage,
            aicommsflightmaneuver,
            aicommsflighttactical,
            aicommsflightformation,
            aicommsjtac,
            aicommsatc,
            aicommscarrier,
            aicommscarrier_LSO,
            aicommscarrier_Marshal,
            aicommscarrier_Approach,
            aicommscarrier_Departure,
            aicommsawacs,
            aicommstanker,
            aicommscrew,
            aicommsaocs,
            cockpit,
            reply,
            auxmenu,
            menu,
            cargocontrol,
            special,
            RIO,
            RIO_menu,
            RIO_radar,
            RIO_weapons,
            RIO_radio,
            RIO_utility,
            RIO_defensive,
            RIO_misc,
            AI_pilot,
            kneeboard,
            Moose,
        }

    }
}
