using System;
using System.Collections.Generic;
using VAICOM.Client;
using VAICOM.Static;

namespace VAICOM
{
    namespace Extensions
    {
        namespace RIO
        {

            public partial class helper
            {

                public static bool showingjestermenu = false;  // reset at mission start

                public static void ShowWheel(bool show)
                {

                    if (show && !showingjestermenu)
                    {
                        //try
                        //{

                        //send HUD view first
                        //List<int> HUDview = new List<int>() { 326 };
                        //DcsClient.SendCmdSequence(HUDview, false);

                        //}
                        //catch (Exception e)
                        //{
                        //Log.Write("Error setting command sequence: " + e.StackTrace, Colors.Inline);
                        //}

                        //Thread.Sleep(2000);

                        try
                        {

                            // then rio sequence
                            State.currentmessage = new DcsClient.Message.CommsMessage();
                            State.currentmessage.type = Messagetypes.DeviceControl;
                            State.currentmessage.extsequence = new List<Extensions.RIO.DeviceAction>();

                            List<Extensions.RIO.DeviceAction> openmenu = new List<Extensions.RIO.DeviceAction>();
                            Extensions.RIO.DeviceAction openmenuaction = new Extensions.RIO.DeviceAction()
                            {
                                device = 62,
                                command = 3550,
                                value = -1
                            };
                            openmenu.Add(openmenuaction);

                            List<Extensions.RIO.DeviceAction> closemenu = new List<Extensions.RIO.DeviceAction>();
                            Extensions.RIO.DeviceAction closemenuaction = new Extensions.RIO.DeviceAction()
                            {
                                device = 62,
                                command = 3725,
                                value = 1
                            };
                            closemenu.Add(closemenuaction);

                            State.currentmessage.extsequence.AddRange(closemenu);
                            State.currentmessage.extsequence.AddRange(openmenu);
                            if (tables.menustate[tables.menucats.PLAYERSEAT].Equals(tables.menustates.Pilot))
                            {
                                State.currentmessage.extsequence.AddRange(openmenu);
                            }

                            State.currentmessage.debug = State.activeconfig.Debugmode;
                            State.currentmessage.client = State.client;
                            State.currentmessage.mode = State.clientmode;
                            State.currentmessage.AIRIO = true;

                            DcsClient.SendClientMessage();
                        }
                        catch (Exception e)
                        {
                            Log.Write("Error setting dev sequence: " + e.StackTrace, Colors.Inline);
                        }

                        showingjestermenu = true;
                        return;
                    }

                    if (!show && showingjestermenu)
                    {
                        //try
                        //{

                        //send HUD view first
                        // List<int> Cockpitview = new List<int>() { 326 };
                        // DcsClient.SendCmdSequence(Cockpitview, false);

                        //}
                        //catch (Exception e)
                        //{
                        //Log.Write("Error setting command sequence: " + e.StackTrace, Colors.Inline);
                        //}

                        //Thread.Sleep(2000);

                        try
                        {

                            // then rio sequence
                            State.currentmessage = new DcsClient.Message.CommsMessage();
                            State.currentmessage.type = Messagetypes.DeviceControl;
                            State.currentmessage.extsequence = new List<Extensions.RIO.DeviceAction>();

                            List<Extensions.RIO.DeviceAction> openmenu = new List<Extensions.RIO.DeviceAction>();
                            Extensions.RIO.DeviceAction openmenuaction = new Extensions.RIO.DeviceAction()
                            {
                                device = 62,
                                command = 3550,
                                value = -1
                            };
                            openmenu.Add(openmenuaction);

                            List<Extensions.RIO.DeviceAction> closemenu = new List<Extensions.RIO.DeviceAction>();
                            Extensions.RIO.DeviceAction closemenuaction = new Extensions.RIO.DeviceAction()
                            {
                                device = 62,
                                command = 3725,
                                value = 1
                            };
                            closemenu.Add(closemenuaction);

                            State.currentmessage.extsequence.AddRange(closemenu);
                            State.currentmessage.extsequence.AddRange(closemenu);
                            //State.currentmessage.extsequence.AddRange(openmenu);
                            //State.currentmessage.extsequence.AddRange(openmenu);

                            State.currentmessage.debug = State.activeconfig.Debugmode;
                            State.currentmessage.client = State.client;
                            State.currentmessage.mode = State.clientmode;
                            State.currentmessage.AIRIO = true;

                            DcsClient.SendClientMessage();
                        }
                        catch (Exception e)
                        {
                            Log.Write("Error setting dev sequence: " + e.StackTrace, Colors.Inline);
                        }


                        showingjestermenu = false;
                        return;
                    }

                }

            }

        }
    }
}
