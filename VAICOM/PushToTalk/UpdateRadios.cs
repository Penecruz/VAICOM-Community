using VAICOM.Products;
using VAICOM.Servers;
using static VAICOM.Extensions.RIO.DeviceActionsLibrary;

namespace VAICOM
{

    namespace PushToTalk
    {

        public partial class PTT
        {

            public static void PTT_SetConfigMulti()
            {

                if (State.activeconfig.UseSRSmapping)
                {
                    PTT_SetConfigMulti_SRS();
                    return;
                }

                try
                {

                    PTT_ResetConfig();

                    if (State.currentstate.radios == null || State.currentstate.radios.Count.Equals(0)) // FC3
                    {
                        PTT_TXAssignmentDefault();
                        State.radiocount = 3;
                        TXNodes.TX4.enabled = State.currentstate.easycomms;
                        return;
                    }
                    else // non-FC3
                    {

                        bool allocatedAM = false;
                        bool allocatedUHF = false;
                        bool allocatedFM = false;
                        bool allocatedINT = false;

                        State.radiocount = 0;

                        // business logic to assign the aircraft radios (TX1-3 + TX5)

                        bool harrier = State.currentmodule.Equals(DCSmodules.LookupTable["AV-8B"]);
                        bool viper = State.currentstate.id.Contains("F-16");
                        bool tomcat = State.currentstate.id.Contains("F-14");
                        bool strike = State.currentmodule.Equals(DCSmodules.LookupTable["F-15ESE"]);
                        bool viggen = State.currentstate.id.Contains("AJS37");
                        bool tigerFC = State.currentmodule.Equals(DCSmodules.LookupTable["F-5E_FC"]);

                        foreach (Server.RadioDevice radiounit in State.currentstate.radios)
                        {

                            bool harriercomm1 = (harrier && radiounit.deviceid.Equals(2));
                            bool harriercomm2 = (harrier && radiounit.deviceid.Equals(3));
                            bool harrierfm = (harrier & radiounit.displayName.ToLower().Contains("fm"));

                            bool vipervhf = (viper & radiounit.deviceid.Equals(38));
                            bool viperuhf = (viper & radiounit.deviceid.Equals(36));

                            bool tomcat182 = (tomcat && radiounit.deviceid.Equals(4));
                            bool tomcat159 = (tomcat && radiounit.deviceid.Equals(3));

                            bool strikecomm1 = (strike && radiounit.deviceid.Equals(7));
                            bool strikecomm2 = (strike && radiounit.deviceid.Equals(8));

                            bool viggenfr24 = (viggen & radiounit.deviceid.Equals(30));//AM radio in testradio folder //maybe change radiounit.displayname here?
                            bool viggenfr22 = (viggen & radiounit.deviceid.Equals(31));//AM radio in testradio2 folder

                            bool tigerFCcomm1 = (tigerFC && radiounit.deviceid.Equals(22)); //try adding device ID for F-5E flamming cliis radio

                            bool deviceallocated = false;

                            // Interphone -> TX5
                            if (!deviceallocated & !allocatedINT & radiounit.intercom)
                            {
                                TXNodes.TX5.enabled = true;
                                TXNodes.TX5.number = radiounit.deviceid;
                                TXNodes.TX5.radios = TXConfigs.SNGL_RADIO_INT;

                                RadioDevices.INT.isavailable = radiounit.isavailable;
                                RadioDevices.INT.deviceid = radiounit.deviceid;
                                RadioDevices.INT.name = radiounit.displayName;
                                RadioDevices.INT.intercom = radiounit.intercom;
                                RadioDevices.INT.AM = radiounit.AM;
                                RadioDevices.INT.FM = radiounit.FM;
                                RadioDevices.INT.on = radiounit.on;
                                RadioDevices.INT.frequency = radiounit.frequency.ToString(); // was string
                                RadioDevices.INT.modulation = radiounit.modulation;

                                deviceallocated = true;
                                allocatedINT = true;
                                // int is not part of radio count
                            }

                            bool uhfcandidate = tomcat182 || vipervhf || harriercomm2 || strikecomm2 || viggenfr22 || (!viperuhf && !harriercomm1 && !viggenfr24 && (!tomcat && (radiounit.displayName.ToLower().Contains("uhf") || radiounit.AM & !radiounit.displayName.ToLower().Contains("vhf"))));
                            // UHF -> TX2
                            if (!deviceallocated && !allocatedUHF && uhfcandidate)
                            {
                                TXNodes.TX2.enabled = true;
                                TXNodes.TX2.number = radiounit.deviceid;
                                TXNodes.TX2.radios = TXConfigs.SNGL_RADIO_Radio2;

                                RadioDevices.Radio2.isavailable = radiounit.isavailable;
                                RadioDevices.Radio2.deviceid = radiounit.deviceid;
                                if (radiounit.displayName.Length > 16)
                                {
                                    RadioDevices.Radio2.name = radiounit.displayName.Substring(radiounit.displayName.Length - 16, 16);
                                }
                                else
                                {
                                    RadioDevices.Radio2.name = radiounit.displayName;
                                }
                                RadioDevices.Radio2.intercom = radiounit.intercom;
                                RadioDevices.Radio2.AM = radiounit.AM;
                                RadioDevices.Radio2.FM = radiounit.FM;
                                RadioDevices.Radio2.on = radiounit.on;
                                RadioDevices.Radio2.frequency = radiounit.frequency.ToString();
                                RadioDevices.Radio2.modulation = radiounit.modulation;

                                deviceallocated = true;
                                allocatedUHF = true;
                                State.radiocount = State.radiocount + 1;
                            }

                            bool amcandidate = tomcat159 || viperuhf || harriercomm1 || strikecomm1 || tigerFCcomm1 || viggenfr24 || (!harrier & radiounit.AM);
                            // VHF AM -> TX1 
                            if (!deviceallocated & !allocatedAM & amcandidate)
                            {

                                TXNodes.TX1.enabled = true;
                                TXNodes.TX1.number = radiounit.deviceid;
                                TXNodes.TX1.radios = TXConfigs.SNGL_RADIO_Radio1;

                                RadioDevices.Radio1.isavailable = radiounit.isavailable;
                                RadioDevices.Radio1.deviceid = radiounit.deviceid;
                                if (radiounit.displayName.Length > 16)
                                {
                                    RadioDevices.Radio1.name = radiounit.displayName.Substring(radiounit.displayName.Length - 16, 16);
                                }
                                else
                                {
                                    RadioDevices.Radio1.name = radiounit.displayName;
                                }

                                RadioDevices.Radio1.intercom = radiounit.intercom;
                                RadioDevices.Radio1.AM = radiounit.AM;
                                RadioDevices.Radio1.FM = radiounit.FM;
                                RadioDevices.Radio1.on = radiounit.on;
                                RadioDevices.Radio1.frequency = radiounit.frequency.ToString();
                                RadioDevices.Radio1.modulation = radiounit.modulation;

                                deviceallocated = true;
                                allocatedAM = true;
                                State.radiocount = State.radiocount + 1;
                            }

                            bool fmcandidate = (harrierfm || (!harrier & radiounit.FM));

                            // VHF FM -> TX3
                            if (!deviceallocated & !allocatedFM & fmcandidate)
                            {
                                TXNodes.TX3.enabled = true;
                                TXNodes.TX3.number = radiounit.deviceid;
                                TXNodes.TX3.radios = TXConfigs.SNGL_RADIO_Radio3;

                                RadioDevices.Radio3.isavailable = radiounit.isavailable;
                                RadioDevices.Radio3.deviceid = radiounit.deviceid;
                                if (radiounit.displayName.Length > 16)
                                {
                                    RadioDevices.Radio3.name = radiounit.displayName.Substring(radiounit.displayName.Length - 16, 16);
                                }
                                else
                                {
                                    RadioDevices.Radio3.name = radiounit.displayName;
                                }
                                RadioDevices.Radio3.intercom = radiounit.intercom;
                                RadioDevices.Radio3.AM = radiounit.AM;
                                RadioDevices.Radio3.FM = radiounit.FM;
                                RadioDevices.Radio3.on = radiounit.on;
                                RadioDevices.Radio3.frequency = radiounit.frequency.ToString();
                                RadioDevices.Radio3.modulation = radiounit.modulation;

                                deviceallocated = true;
                                allocatedFM = true;
                                State.radiocount = State.radiocount + 1;
                            }

                        }

                        // TX4
                        TXNodes.TX4.radios = TXConfigs.ALL_RADIOS_AUTO;
                        TXNodes.TX4.enabled = State.currentstate.easycomms;

                        //TX6
                        TXNodes.TX6.radios = TXConfigs.SNGL_RADIO_AUX;
                        TXNodes.TX6.enabled = true;

                    }
                }
                catch
                {
                }
            }

            public static void PTT_SetConfigSingle()
            {

                if (State.activeconfig.UseSRSmapping)
                {
                    PTT_SetConfigSingle_SRS();
                    return;
                }


                PTT_SetConfigMulti();

                TXNodes.TX1.enabled = false;
                TXNodes.TX2.enabled = false;
                TXNodes.TX3.enabled = false;
                TXNodes.TX4.enabled = false;
                TXNodes.TX5.enabled = false; //false
                TXNodes.TX6.enabled = false;

                switch (State.activeconfig.SingleHotkey)
                {
                    case "TX1":
                        TXNodes.TX1 = new TXNode() { name = "TX1", enabled = true, radios = TXConfigs.ALL_RADIOS_SEL };
                        break;
                    case "TX2":
                        TXNodes.TX2 = new TXNode() { name = "TX2", enabled = true, radios = TXConfigs.ALL_RADIOS_SEL };
                        break;
                    case "TX3":
                        TXNodes.TX3 = new TXNode() { name = "TX3", enabled = true, radios = TXConfigs.ALL_RADIOS_SEL };
                        break;
                    case "TX4":
                        TXNodes.TX4 = new TXNode() { name = "TX4", enabled = true, radios = TXConfigs.ALL_RADIOS_SEL };
                        break;
                    case "TX5":
                        TXNodes.TX5 = new TXNode() { name = "TX5", enabled = true, radios = TXConfigs.ALL_RADIOS_SEL };
                        break;
                    case "TX6":
                        TXNodes.TX6 = new TXNode() { name = "TX6", enabled = true, radios = TXConfigs.ALL_RADIOS_SEL };
                        break;
                    default:
                        TXNodes.TX1 = new TXNode() { name = "TX1", enabled = true, radios = TXConfigs.ALL_RADIOS_SEL };
                        break;
                }
            }

        }
    }
}
