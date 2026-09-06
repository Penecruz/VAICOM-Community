using System;
using System.Collections.Generic;
using VAICOM.Static;

namespace VAICOM.Extensions.AICPG
{
    public class CPGCommandHandler
    {
        public static void ProcessCommand(string commandId)
        {
            switch (commandId)
            {
                //Show/Hide George Overlay
                case "wMsgGeorgeShowHide":
                    AddGeorgeButton(AH64GeorgeButton.Menu);
                    break;
                // Long show/hide press
                case "wMsgGeorgeMenuDefenseMode":
                    AddGeorgeLongButton(AH64GeorgeButton.Menu);
                    break;
                // Up Short Presses
                case "wMsgGeorgeUp":
                case "wMsgGeorgePreviuousTarget":
                case "wMsgGeorgePreviousItem":
                case "wMsgGeorgeStartUpEnginesFly":
                case "wMsgGeorgeSpeedUp":
                case "wMsgGeorgeAlignToTADS":
                case "wMsgGeorgeAlignToNTS":
                case "wMsgGeorgeHoverUpTenFeet":
                    AddGeorgeButton(AH64GeorgeButton.Up);
                    break;
                // Down Short Presses
                case "wMsgGeorgeDown":
                case "wMsgGeorgeNextTarget":
                case "wMsgGeorgeNextItem":
                case "wMsgGeorgeStartUpAPU":
                case "wMsgGeorgeShutdownEngines":
                case "wMsgGeorgeSlowDown":
                case "wMsgGeorgeReturnToBattlePosition":
                case "wMsgGeorgeHoverDownTenFeet":
                    AddGeorgeButton(AH64GeorgeButton.Down);
                    break;
                // Left Short Presses
                case "wMsgGeorgeLeft":
                case "wMsgGeorgeNextWeapon":
                case "wMsgGeorgeExitList":
                case "wMsgGeorgeMenuCombatMode":
                case "wMsgGeorgeMenuFlightMode":
                case "wMsgGeorgeMenuGroundMode":
                case "wMsgGeorgeMenuHoverMode":
                case "wMsgGeorgeMenuNextMode":
                    if (State.currentcommand.dcsid.Equals("wMsgGeorgeNextWeapon", StringComparison.OrdinalIgnoreCase) && !CanChangeWeaponSelection())
                    {
                        break;
                    }

                    AddGeorgeButton(AH64GeorgeButton.Left);

                    if (State.currentcommand.dcsid.Equals("wMsgGeorgeNextWeapon", StringComparison.OrdinalIgnoreCase))
                    {
                        SelectNextWeapon();
                    }
                    break;
                // Right Short Presses
                case "wMsgGeorgeRight":
                case "wMsgGeorgeTrackTarget":
                case "wMsgGeorgeLaseTarget":
                case "wMsgGeorgeLaserOn":
                case "wMsgGeorgeLaserOff":
                case "wMsgGeorgeBurstLimit":
                case "wMsgGeorgeRocketQuantity":
                case "wMsgGeorgeLOBL":
                case "wMsgGeorgeLOAL":
                case "wMsgGeorgeListItemSelect":
                case "wMsgGeorgeStartUpEnginesIdle":
                case "wMsgGeorgeFollowWaypoints":
                case "wMsgGeorgeTurnToGHS":
                    AddGeorgeButton(AH64GeorgeButton.Right);
                    break;
                // Multifunction Short Presses
                case "wMsgGeorgeCenter":
                case "wMsgGeorgeClearedFire":
                case "wMsgGeorgeTadsFov":
                case "wMsgGeorgeSelectTarget":
                case "wMsgGeorgelastStoredTarget":
                case "wMsgGeorgePointSearch":
                case "wMsgGeorgeTakeOff":
                case "wMsgGeorgeSetAirSpeedRef":
                case "wMsgGeorgeSetGroundSpeedRef":
                case "wMsgGeorgeMaskPosition":
                    AddGeorgeButton(AH64GeorgeButton.Multifunction);
                    break;
                // Request Control when in the CPG seat
                case "wMsgGeorgeControlRequest":
                    AddGeorgeAction(AH64GeorgeButton.RequestControl, 1.0);
                    break;
                // Store Target 
                case "wMsgGeorgeStoreTarget":
                    AddGeorgeAction(AH64GeorgeButton.StoreTarget, 1.0);
                    break;
                // Up Long Presses                         
                case "wMsgGeorgeUpLong":
                case "wMsgGeorgeTadsZoomIn":
                case "wMsgGeorgeTargetListZoomIn":
                case "wMsgGeorgeIncreaseAltitude":
                case "wMsgGeorgeOrbitOverhead":
                case "wMsgGeorgeHoverForward":
                    AddGeorgeLongButton(AH64GeorgeButton.Up);
                    break;
                // Down Long Presses
                case "wMsgGeorgeDownLong":
                case "wMsgGeorgeTadsZoomOut":
                case "wMsgGeorgeTargetListZoomOut":
                case "wMsgGeorgeLastFoundTarget":
                case "wMsgGeorgeShutdownFull":
                case "wMsgGeorgeDecreaseAltitude":
                case "wMsgGeorgeBreakOneEighty":
                case "wMsgGeorgeThreatWarningsOn":
                case "wMsgGeorgeThreatWarningsOff":
                case "wMsgGeorgeHoverBack":
                    AddGeorgeLongButton(AH64GeorgeButton.Down);
                    break;
                // Left Long Presses
                case "wMsgGeorgeLeftLong":
                case "wMsgGeorgeTargetListFilter":
                case "wMsgGeorgePointListFilterMode":
                case "wMsgGeorgeNextRkt":
                case "wMsgGeorgeNextMSL":
                case "wMsgGeorgeAreaSelect":
                case "wMsgGeorgeComeLeft":
                case "wMsgGeorgeBreakLeft":
                case "wMsgGeorgeHoverLeft":
                    AddGeorgeLongButton(AH64GeorgeButton.Left);
                    break;
                // Right Long Presses
                case "wMsgGeorgeRightLong":
                case "wMsgGeorgePointListFilterThreat":
                case "wMsgGeorgeMslTraj":
                case "wMsgGeorgePointSelect":
                case "wMsgGeorgeComeRight":
                case "wMsgGeorgeBreakRight":
                case "wMsgGeorgeHoverRight":
                    AddGeorgeLongButton(AH64GeorgeButton.Right);
                    break;
                // Multifunction Long Presses
                case "wMsgGeorgeCenterLong":
                case "wMsgGeorgeStartUp":
                case "wMsgGeorgeShutdown":
                case "wMsgGeorgeAdjustAim":
                case "wMsgGeorgeTadsSensor":
                case "wMsgGeorgeAreaSearch":
                case "wMsgGeorgeStartUpFull":
                case "wMsgGeorgeSetRadarAltitude":
                case "wMsgGeorgeSetBarometricAltitude":
                case "wMsgGeorgeAddBattlePosition":
                case "wMsgGeorgeDeleteBattlePosition":
                    AddGeorgeLongButton(AH64GeorgeButton.Multifunction);
                    break;

                // George PLT Defense Mode items
                case "wMsgGeorgeCMWSArm":
                case "wMsgGeorgeCMWSSafe":
                    AddGeorgeLongButton(AH64GeorgeButton.Menu);
                    AddGeorgeButton(AH64GeorgeButton.Up);
                    AddGeorgeButton(AH64GeorgeButton.Menu);
                    break;
                case "wMsgGeorgeCMWSAuto":
                case "wMsgGeorgeCMWSBypass":
                    AddGeorgeLongButton(AH64GeorgeButton.Menu);
                    AddGeorgeButton(AH64GeorgeButton.Down);
                    AddGeorgeButton(AH64GeorgeButton.Menu);
                    break;
                case "wMsgGeorgeEvadeOff":
                case "wMsgGeorgeEvadeLevel":
                case "wMsgGeorgeEvadeVertical":
                case "wMsgGeorgeEvadeMask":
                    AddGeorgeLongButton(AH64GeorgeButton.Menu);
                    AddGeorgeButton(AH64GeorgeButton.Left);
                    AddGeorgeButton(AH64GeorgeButton.Menu);
                    break;
                case "wMsgGeorgeCMDispenseNone":
                    SelectCMDispenseMode(AH64CMDispenseMode.None);
                    break;
                case "wMsgGeorgeCMDispenseChaff":
                    SelectCMDispenseMode(AH64CMDispenseMode.Chaff);
                    break;
                case "wMsgGeorgeCMDispenseFlares":
                    SelectCMDispenseMode(AH64CMDispenseMode.Flares);
                    break;
                case "wMsgGeorgeCMDispenseChaffAndFlares":
                    SelectCMDispenseMode(AH64CMDispenseMode.ChaffAndFlares);
                    break;

                // George ROE
                case "wMsgGeorgeReturnFire":
                    AddGeorgeLongButton(AH64GeorgeButton.Menu);
                    AddGeorgeLongButton(AH64GeorgeButton.Up);
                    AddGeorgeButton(AH64GeorgeButton.Menu);
                    break;
                case "wMsgGeorgeWeaponsFree":
                    if (Helpers.Common.IsAH64PilotSeatActive())
                    {
                        // George as CP/G
                        AddGeorgeLongButton(AH64GeorgeButton.Up);
                    }
                    else
                    {
                        // George as pilot
                        AddGeorgeLongButton(AH64GeorgeButton.Menu);
                        AddGeorgeLongButton(AH64GeorgeButton.Up);
                        AddGeorgeButton(AH64GeorgeButton.Menu);
                    }
                    break;
                case "wMsgGeorgeHoldFire":
                    if (Helpers.Common.IsAH64PilotSeatActive())
                    {
                        // George as CP/G
                        AddGeorgeLongButton(AH64GeorgeButton.Up);
                    }
                    else
                    {
                        // George as pilot
                        AddGeorgeLongButton(AH64GeorgeButton.Menu);
                        AddGeorgeLongButton(AH64GeorgeButton.Up);
                        AddGeorgeButton(AH64GeorgeButton.Menu);
                    }
                    break;

                //Search Tasks
                //Direct Searches
                case "wMsgGeorgeMacroPHSsearch":
                    AddGeorgeButton(AH64GeorgeButton.Up);
                    break;
                case "wMsgGeorgeMacroTADSLOS":
                    AddGeorgeLongButton(AH64GeorgeButton.Down);
                    break;
                //Area Search Macros PHS, FWD, PFZ and hide overlay
                case "wMsgGeorgeMacroNextSearch":
                    AddGeorgeLongButton(AH64GeorgeButton.Left, 120);
                    AddGeorgeButton(AH64GeorgeButton.Down, 80);
                    AddGeorgeButton(AH64GeorgeButton.Right, 80);
                    AddGeorgeButton(AH64GeorgeButton.Menu);
                    break;
                case "wMsgGeorgeMacroPreviousSearch":
                    AddGeorgeLongButton(AH64GeorgeButton.Left, 200);
                    AddGeorgeButton(AH64GeorgeButton.Up, 150);
                    AddGeorgeButton(AH64GeorgeButton.Right, 80);
                    AddGeorgeButton(AH64GeorgeButton.Menu);
                    break;
                //Point Search Macros and hide overlay
                case "wMsgGeorgeMacroNextPoint":
                    AddGeorgeLongButton(AH64GeorgeButton.Right, 200);
                    AddGeorgeButton(AH64GeorgeButton.Down, 150);
                    AddGeorgeButton(AH64GeorgeButton.Right, 80);
                    AddGeorgeButton(AH64GeorgeButton.Menu);
                    break;
                case "wMsgGeorgeMacroPreviousPoint":
                    AddGeorgeLongButton(AH64GeorgeButton.Right, 200);
                    AddGeorgeButton(AH64GeorgeButton.Up, 150);
                    AddGeorgeButton(AH64GeorgeButton.Right, 80);
                    AddGeorgeButton(AH64GeorgeButton.Menu);
                    break;
                //Target List and Track macros
                case "wMsgGeorgeMacroAddTwoTargetsTrack": //Add and Track Top 2 targets in list
                    AddGeorgeButton(AH64GeorgeButton.Multifunction, 100);
                    AddGeorgeButton(AH64GeorgeButton.Down, 100);
                    AddGeorgeButton(AH64GeorgeButton.Multifunction, 100);
                    AddGeorgeButton(AH64GeorgeButton.Right);
                    break;
                case "wMsgGeorgeMacroAddThreeTargetsTrack": //Add and Track Top 3 targets in list
                    AddGeorgeButton(AH64GeorgeButton.Multifunction, 100);
                    AddGeorgeButton(AH64GeorgeButton.Down, 100);
                    AddGeorgeButton(AH64GeorgeButton.Multifunction, 100);
                    AddGeorgeButton(AH64GeorgeButton.Down, 100);
                    AddGeorgeButton(AH64GeorgeButton.Multifunction, 100);
                    AddGeorgeButton(AH64GeorgeButton.Right);
                    break;
                case "wMsgGeorgeMacroAddFourTargetsTrack": //Add and Track Top 4 targets in list
                    AddGeorgeButton(AH64GeorgeButton.Multifunction, 100);
                    AddGeorgeButton(AH64GeorgeButton.Down, 100);
                    AddGeorgeButton(AH64GeorgeButton.Multifunction, 100);
                    AddGeorgeButton(AH64GeorgeButton.Down, 100);
                    AddGeorgeButton(AH64GeorgeButton.Multifunction, 100);
                    AddGeorgeButton(AH64GeorgeButton.Down, 100);
                    AddGeorgeButton(AH64GeorgeButton.Multifunction, 100);
                    AddGeorgeButton(AH64GeorgeButton.Right);
                    break;
                case "wMsgGeorgeMacroTrackEngage": //Tracks current target and give engage command if ROE is Weapons Hold
                    AddGeorgeButton(AH64GeorgeButton.Right, 100);
                    AddGeorgeButton(AH64GeorgeButton.Multifunction, 100);
                    break;
                case "wMsgGeorgeMacroSelectGun":
                    SelectWeapon(AH64WeaponMode.Gun);
                    break;
                case "wMsgGeorgeMacroSelectMissiles":
                    SelectWeapon(AH64WeaponMode.Missiles);
                    break;
                case "wMsgGeorgeMacroSelectRockets":
                    SelectWeapon(AH64WeaponMode.Rockets);
                    break;
                case "wMsgGeorgeMacroSelectNoWeapon":
                    SelectWeapon(AH64WeaponMode.NoWeapon);
                    break;
            }
        }

        public static void AddGeorgeLongButton(int command)
        {
            AddGeorgeAction(command, 1.0, 1200);
            AddGeorgeAction(command, 0.0);
        }

        public static void AddGeorgeLongButton(int command, int postDelayMs)
        {
            AddGeorgeAction(command, 1.0, 1200);
            AddGeorgeAction(command, 0.0, postDelayMs);
        }

        // Enum overloads so callers can use AH64DGeorgeButton
        public static void AddGeorgeLongButton(AH64GeorgeButton button)
        {
            AddGeorgeLongButton((int)button);
        }

        public static void AddGeorgeLongButton(AH64GeorgeButton button, int postDelayMs)
        {
            AddGeorgeLongButton((int)button, postDelayMs);
        }

        public static void AddGeorgeButton(int command)
        {
            AddGeorgeAction(command, 1.0);
            AddGeorgeAction(command, 0.0);
        }

        public static void AddGeorgeButton(int command, int postDelayMs)
        {
            AddGeorgeAction(command, 1.0);
            AddGeorgeAction(command, 0.0, postDelayMs);
        }

        // Enum overloads so callers can use AH64DGeorgeButton
        public static void AddGeorgeButton(AH64GeorgeButton button)
        {
            AddGeorgeButton((int)button);
        }

        public static void AddGeorgeButton(AH64GeorgeButton button, int postDelayMs)
        {
            AddGeorgeButton((int)button, postDelayMs);
        }

        public static void AddGeorgeAction(int command, double value, int delayMs = 0)
        {
            State.currentmessage.extsequence.Add(new Extensions.RIO.DeviceAction
            {
                device = 87, // George AI
                command = command,
                value = value,
                delayMs = delayMs
            });
        }

        // Enum overload for AddGeorgeAction
        public static void AddGeorgeAction(AH64GeorgeButton button, double value, int delayMs = 0)
        {
            AddGeorgeAction((int)button, value, delayMs);
        }

        private static void SelectWeapon(AH64WeaponMode target)
        {
            if (!CanChangeWeaponSelection())
            {
                return;
            }

            if (!AH64GeorgeState.WeaponAvailable(target))
            {
                Log.Write("Requested George weapon is not available with current payload.", Colors.Inline);
                return;
            }

            var current = AH64GeorgeState.SelectedWeapon;
            if (current == AH64WeaponMode.Unknown)
            {
                current = AH64WeaponMode.NoWeapon;
            }

            int steps = AH64GeorgeState.GetWeaponCycleSteps(current, target);
            for (int i = 0; i < steps; i++)
            {
                AddGeorgeButton(AH64GeorgeButton.Left, 80);
            }

            AH64GeorgeState.SelectedWeapon = target;
        }

        private static void SelectNextWeapon()
        {
            var order = AH64GeorgeState.GetWeaponCycleOrder();
            int idx = order.IndexOf(AH64GeorgeState.SelectedWeapon);
            if (idx < 0)
            {
                AH64GeorgeState.SelectedWeapon = order[0];
            }

            AH64GeorgeState.SelectedWeapon = order[(idx + 1) % order.Count];
        }

        private static bool CanChangeWeaponSelection()
        {
            if (State.currentstate != null && !State.currentstate.airborne)
            {
                bool changed = AH64GeorgeState.ForceNoWeaponLocalSync("selection blocked by weight-on-wheels", true);
                if (State.activeconfig.RIO_Messages)
                {
                    if (changed)
                    {
                        State.currentmessage.dspmsg = "GEORGE:\nWeapon selection synced to No WPN with Weight on Wheels.";
                    }
                    else
                    {
                        State.currentmessage.dspmsg = "GEORGE:\nWeapon selection not available with Weight on Wheels.";
                    }
                    State.currentmessage.msgdur = 4;
                }
                Log.Write("George weapon selection is not available with Weight on Wheels. Command Sent " + changed + "; selected=" + AH64GeorgeState.SelectedWeapon + ".", Colors.Recognition);
                return false;
            }

            return true;
        }

        private static void SelectCMDispenseMode(AH64CMDispenseMode target)
        {
            var current = AH64GeorgeState.SelectedCMDispenseMode;
            int steps = AH64GeorgeState.GetCMDispenseSteps(current, target);

            AddGeorgeLongButton(AH64GeorgeButton.Menu);

            for (int i = 0; i < steps; i++)
            {
                AddGeorgeLongButton(AH64GeorgeButton.Right, 80);
            }

            AddGeorgeButton(AH64GeorgeButton.Menu);

            AH64GeorgeState.SelectedCMDispenseMode = target;
        }
    }
}
