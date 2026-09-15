using System;
using System.Linq;
using VAICOM.Static;

namespace VAICOM.Extensions.AICPG
{
    public class CPGCommandHandler
    {
        public static void ProcessCommand(string commandId)
        {
            // Common George commands.
            switch (commandId)
            {
                // Show/Hide George Overlay
                case "wMsgGeorgeShowHide":
                    // Handle when in CP/G seat and DEFN menu open so that it defaults
                    // the menu mode back to the correct mode when closing the DEFN menu.
                    if (AH64GeorgeState.IsGeorgePilot()
                        && AH64GeorgeState.CurrentMenuMode.Equals(AH64MenuMode.Defense))
                    {
                        CloseDefenseMenu();
                    }
                    else
                    {
                        AddGeorgeButton(AH64GeorgeButton.Menu);
                    }
                    return;

                // Generic menu short and long presses
                case "wMsgGeorgeUp":
                    AddGeorgeButton(AH64GeorgeButton.Up);
                    return;
                case "wMsgGeorgeUpLong":
                    AddGeorgeLongButton(AH64GeorgeButton.Up);
                    return;
                case "wMsgGeorgeDown":
                    AddGeorgeButton(AH64GeorgeButton.Down);
                    return;
                case "wMsgGeorgeDownLong":
                    AddGeorgeLongButton(AH64GeorgeButton.Down);
                    return;
                case "wMsgGeorgeLeft":
                    AddGeorgeButton(AH64GeorgeButton.Left);
                    return;
                case "wMsgGeorgeLeftLong":
                    AddGeorgeLongButton(AH64GeorgeButton.Left);
                    return;
                case "wMsgGeorgeRight":
                    AddGeorgeButton(AH64GeorgeButton.Right);
                    return;
                case "wMsgGeorgeRightLong":
                    AddGeorgeLongButton(AH64GeorgeButton.Right);
                    return;
                case "wMsgGeorgeCenter":
                    AddGeorgeButton(AH64GeorgeButton.Multifunction);
                    return;
                case "wMsgGeorgeCenterLong":
                    AddGeorgeLongButton(AH64GeorgeButton.Multifunction);
                    return;
            }

            if (Helpers.Common.IsAH64PilotSeatActive())
            {
                HandleCPGCommand(commandId);
            }
            else 
            {
                HandlePilotCommand(commandId);
            }
        }

        private static void HandleCPGCommand(string commandId)
        {
            switch (commandId)
            {
                // Up Short Presses
                case "wMsgGeorgePreviuousTarget":
                case "wMsgGeorgePreviousItem":
                    AddGeorgeButton(AH64GeorgeButton.Up);
                    return;

                // Down Short Presses
                case "wMsgGeorgeNextTarget":
                case "wMsgGeorgeNextItem":
                case "wMsgGeorgeAPUOnly":
                case "wMsgGeorgeShutdownEngines":
                case "wMsgGeorgeSlowDown":
                case "wMsgGeorgeHoldPosition":
                case "wMsgGeorgeReturnToBattlePosition":
                case "wMsgGeorgeHoverDownTenFeet":
                    AddGeorgeButton(AH64GeorgeButton.Down);
                    return;

                // Left Short Presses
                case "wMsgGeorgeNextWeapon":
                case "wMsgGeorgeExitList":
                    if (commandId.Equals("wMsgGeorgeNextWeapon", StringComparison.OrdinalIgnoreCase) && !CanChangeWeaponSelection())
                    {
                        return;
                    }

                    AddGeorgeButton(AH64GeorgeButton.Left);

                    if (commandId.Equals("wMsgGeorgeNextWeapon", StringComparison.OrdinalIgnoreCase))
                    {
                        SelectNextWeapon();
                    }
                    return;

                // Right Short Presses
                case "wMsgGeorgeTrackTarget":
                case "wMsgGeorgeLaseTarget":
                case "wMsgGeorgeLaserOn":
                case "wMsgGeorgeLaserOff":
                case "wMsgGeorgeBurstLimit":
                case "wMsgGeorgeRocketQuantity":
                case "wMsgGeorgeLOBL":
                case "wMsgGeorgeLOAL":
                case "wMsgGeorgeListItemSelect":
                    AddGeorgeButton(AH64GeorgeButton.Right);
                    return;

                // Multifunction Short Presses
                case "wMsgGeorgeClearedFire":
                case "wMsgGeorgeTadsFov":
                case "wMsgGeorgeSelectTarget":
                case "wMsgGeorgelastStoredTarget":
                case "wMsgGeorgePointSearch":
                    AddGeorgeButton(AH64GeorgeButton.Multifunction);
                    return;

                // Up Long Presses                         
                case "wMsgGeorgeTadsZoomIn":
                case "wMsgGeorgeTargetListZoomIn":
                case "wMsgGeorgeWeaponsFree":
                case "wMsgGeorgeHoldFire":
                    AddGeorgeLongButton(AH64GeorgeButton.Up);
                    return;

                // Down Long Presses
                case "wMsgGeorgeTadsZoomOut":
                case "wMsgGeorgeTargetListZoomOut":
                case "wMsgGeorgeLastFoundTarget":
                case "wMsgGeorgeShutdownFull":
                    AddGeorgeLongButton(AH64GeorgeButton.Down);
                    return;

                // Left Long Presses
                case "wMsgGeorgeTargetListFilter":
                case "wMsgGeorgePointListFilterMode":
                case "wMsgGeorgeNextRkt":
                case "wMsgGeorgeNextMSL":
                case "wMsgGeorgeAreaSelect":
                    AddGeorgeLongButton(AH64GeorgeButton.Left);
                    return;

                // Right Long Presses
                case "wMsgGeorgePointListFilterThreat":
                case "wMsgGeorgeMslTraj":
                case "wMsgGeorgePointSelect":
                    AddGeorgeLongButton(AH64GeorgeButton.Right);
                    return;

                // Multifunction Long Presses
                case "wMsgGeorgeStartUp":
                case "wMsgGeorgeShutdown":
                case "wMsgGeorgeAdjustAim":
                case "wMsgGeorgeTadsSensor":
                case "wMsgGeorgeAreaSearch":
                    AddGeorgeLongButton(AH64GeorgeButton.Multifunction);
                    return;

                // Request Control when in the CPG seat
                case "wMsgGeorgeControlRequest":
                    AddGeorgeAction(AH64GeorgeButton.RequestControl, 1.0);
                    return;
                // Store Target 
                case "wMsgGeorgeStoreTarget":
                    AddGeorgeAction(AH64GeorgeButton.StoreTarget, 1.0);
                    return;

                //Search Tasks
                //Direct Searches
                case "wMsgGeorgeMacroPHSsearch":
                    AddGeorgeButton(AH64GeorgeButton.Up);
                    return;
                case "wMsgGeorgeMacroTADSLOS":
                    AddGeorgeLongButton(AH64GeorgeButton.Down);
                    return;
                //Area Search Macros PHS, FWD, PFZ and hide overlay
                case "wMsgGeorgeMacroNextSearch":
                    AddGeorgeLongButton(AH64GeorgeButton.Left, 120);
                    AddGeorgeButton(AH64GeorgeButton.Down, 80);
                    AddGeorgeButton(AH64GeorgeButton.Right, 80);
                    AddGeorgeButton(AH64GeorgeButton.Menu);
                    return;
                case "wMsgGeorgeMacroPreviousSearch":
                    AddGeorgeLongButton(AH64GeorgeButton.Left, 200);
                    AddGeorgeButton(AH64GeorgeButton.Up, 150);
                    AddGeorgeButton(AH64GeorgeButton.Right, 80);
                    AddGeorgeButton(AH64GeorgeButton.Menu);
                    return;
                //Point Search Macros and hide overlay
                case "wMsgGeorgeMacroNextPoint":
                    AddGeorgeLongButton(AH64GeorgeButton.Right, 200);
                    AddGeorgeButton(AH64GeorgeButton.Down, 150);
                    AddGeorgeButton(AH64GeorgeButton.Right, 80);
                    AddGeorgeButton(AH64GeorgeButton.Menu);
                    return;
                case "wMsgGeorgeMacroPreviousPoint":
                    AddGeorgeLongButton(AH64GeorgeButton.Right, 200);
                    AddGeorgeButton(AH64GeorgeButton.Up, 150);
                    AddGeorgeButton(AH64GeorgeButton.Right, 80);
                    AddGeorgeButton(AH64GeorgeButton.Menu);
                    return;
                //Target List and Track macros
                case "wMsgGeorgeMacroAddTwoTargetsTrack": //Add and Track Top 2 targets in list
                    AddGeorgeButton(AH64GeorgeButton.Multifunction, 100);
                    AddGeorgeButton(AH64GeorgeButton.Down, 100);
                    AddGeorgeButton(AH64GeorgeButton.Multifunction, 100);
                    AddGeorgeButton(AH64GeorgeButton.Right);
                    return;
                case "wMsgGeorgeMacroAddThreeTargetsTrack": //Add and Track Top 3 targets in list
                    AddGeorgeButton(AH64GeorgeButton.Multifunction, 100);
                    AddGeorgeButton(AH64GeorgeButton.Down, 100);
                    AddGeorgeButton(AH64GeorgeButton.Multifunction, 100);
                    AddGeorgeButton(AH64GeorgeButton.Down, 100);
                    AddGeorgeButton(AH64GeorgeButton.Multifunction, 100);
                    AddGeorgeButton(AH64GeorgeButton.Right);
                    return;
                case "wMsgGeorgeMacroAddFourTargetsTrack": //Add and Track Top 4 targets in list
                    AddGeorgeButton(AH64GeorgeButton.Multifunction, 100);
                    AddGeorgeButton(AH64GeorgeButton.Down, 100);
                    AddGeorgeButton(AH64GeorgeButton.Multifunction, 100);
                    AddGeorgeButton(AH64GeorgeButton.Down, 100);
                    AddGeorgeButton(AH64GeorgeButton.Multifunction, 100);
                    AddGeorgeButton(AH64GeorgeButton.Down, 100);
                    AddGeorgeButton(AH64GeorgeButton.Multifunction, 100);
                    AddGeorgeButton(AH64GeorgeButton.Right);
                    return;
                case "wMsgGeorgeMacroTrackEngage": //Tracks current target and give engage command if ROE is Weapons Hold
                    AddGeorgeButton(AH64GeorgeButton.Right, 100);
                    AddGeorgeButton(AH64GeorgeButton.Multifunction, 100);
                    return;
                case "wMsgGeorgeMacroSelectGun":
                    SelectWeapon(AH64WeaponMode.Gun);
                    return;
                case "wMsgGeorgeMacroSelectMissiles":
                    SelectWeapon(AH64WeaponMode.Missiles);
                    return;
                case "wMsgGeorgeMacroSelectRockets":
                    SelectWeapon(AH64WeaponMode.Rockets);
                    return;
                case "wMsgGeorgeMacroSelectNoWeapon":
                    SelectWeapon(AH64WeaponMode.NoWeapon);
                    return;
            }

            // This shouldn't occur, unless due to a coding bug.
            Log.Write("Command not valid for George AI CP/G", Colors.Warning);
        }

        private static void HandlePilotCommand(string commandId)
        {
            // TODO: test and verify commands then organise by menu and then by press.
            // This may introduce duplication as some items appear in multiple menus, e.g. Add/Delete battle position.
            // Those may need to be in a separate multiple menu mode if condition.

            // TODO: if fall through all checks then should display message stating that "Command is not available in <AH64GeorgeState.CurrentMenuMode> mode"

            switch (commandId)
            {
                // Change menu modes
                case "wMsgGeorgeMenuCombatMode":
                case "wMsgGeorgeMenuFlightMode":
                case "wMsgGeorgeMenuGroundMode":
                case "wMsgGeorgeMenuHoverMode":
                case "wMsgGeorgeMenuDefenseMode":
                case "wMsgGeorgeMenuNextMode":
                    SelectMenuMode(commandId);
                    return;

                // Up short presses
                case "wMsgGeorgeStartUpEnginesFly":
                    if (InGroundMode())
                    {
                        AddGeorgeButton(AH64GeorgeButton.Up);
                    }
                    return;
                case "wMsgGeorgeSpeedUp":
                    if (InFlightMode())
                    {
                        AddGeorgeButton(AH64GeorgeButton.Up);
                    }
                    return;
                case "wMsgGeorgeAlignToTADS":
                case "wMsgGeorgeAlignToNTS":
                    if (InCombatMode())
                    {
                        AddGeorgeButton(AH64GeorgeButton.Up);
                    }
                    return;
                case "wMsgGeorgeHoverUpTenFeet":
                    if (InHoverMode())
                    {
                        AddGeorgeButton(AH64GeorgeButton.Up);
                    }
                    return;

                // Down short presses
                case "wMsgGeorgeAPUStart":
                case "wMsgGeorgeAPUStop":
                    if (InGroundMode())
                    {
                        ToggleApuOnOff(commandId.Equals("wMsgGeorgeAPUStart", StringComparison.OrdinalIgnoreCase) ? AH64Apu.On : AH64Apu.Off);
                    }
                    return;
                case "wMsgGeorgeAPUOnly":
                case "wMsgGeorgeShutdownEngines":
                    if (InGroundMode())
                    {
                        AddGeorgeButton(AH64GeorgeButton.Down);
                    }
                    return;
                case "wMsgGeorgeSlowDown":
                    if (InFlightMode())
                    {
                        AddGeorgeButton(AH64GeorgeButton.Down);
                    }
                    return;
                case "wMsgGeorgeHoverDownTenFeet":
                    if (InHoverMode())
                    {
                        AddGeorgeButton(AH64GeorgeButton.Down);
                    }
                    return;
                case "wMsgGeorgeHoldPosition":
                case "wMsgGeorgeReturnToBattlePosition":
                    if (InCombatMode())
                    {
                        AddGeorgeButton(AH64GeorgeButton.Down);
                    }
                    return;
                
                // Right short presses
                case "wMsgGeorgeStartUpEnginesIdle":
                    if (InGroundMode())
                    {
                        AddGeorgeButton(AH64GeorgeButton.Right);
                    }
                    return;
                case "wMsgGeorgeFollowWaypoints":
                    if (InFlightMode())
                    {
                        AddGeorgeButton(AH64GeorgeButton.Right);
                    }
                    return;
                case "wMsgGeorgeTurnToGHS":
                    if (InHoverMode())
                    {
                        AddGeorgeButton(AH64GeorgeButton.Right);
                    }
                    return;

                // Multifunction short presses
                case "wMsgGeorgeTakeOff":
                    // Only allow takeoff -> Flight when:
                    // - currently on the ground,
                    // - hover capability exists,
                    // - hover can actually be selected (APU off while on ground),
                    // - and current menu is Ground, Hover or Defense (valid takeoff contexts).
                    var currentMode = AH64GeorgeState.CurrentMenuMode;
                    var hoverSelectable = AH64GeorgeState.GetAvailableMenuModes().Contains(AH64MenuMode.Hover);
                    if (!AH64GeorgeState.IsAirbourne()
                        && AH64GeorgeState.IsHoverAvailable()
                        && hoverSelectable
                        && (currentMode == AH64MenuMode.Ground || currentMode == AH64MenuMode.Hover || currentMode == AH64MenuMode.Defense))
                    {
                        AddGeorgeButton(AH64GeorgeButton.Multifunction);
                        // The menu automatically switches to FLT mode when
                        // taking off and previously on GND/HOV/DEFN.
                        AH64GeorgeState.SetMenuMode(AH64MenuMode.Flight);
                    }
                    return;
                case "wMsgGeorgeSetAirSpeedRef":
                case "wMsgGeorgeSetGroundSpeedRef":
                    if (InFlightMode())
                    {
                        AddGeorgeButton(AH64GeorgeButton.Multifunction);
                    }
                    return;
                case "wMsgGeorgeMaskPosition":
                    if (AH64GeorgeState.IsAirbourne()
                        && (InCombatMode() || InHoverMode() || InDefenseMode()))
                    {
                        AddGeorgeButton(AH64GeorgeButton.Multifunction);
                    }
                    return;

                // Up long presses
                case "wMsgGeorgeStartUpFull":
                    if (InGroundMode())
                    {
                        AddGeorgeLongButton(AH64GeorgeButton.Up);
                    }
                    return;
                case "wMsgGeorgeOrbitOverhead":
                    if (InCombatMode())
                    {
                        AddGeorgeLongButton(AH64GeorgeButton.Up);
                    }
                    return;
                case "wMsgGeorgeHoverForward":
                    if (InHoverMode())
                    {
                        AddGeorgeLongHoldButton(AH64GeorgeButton.Up, 1000);
                    }
                    return;
                case "wMsgGeorgeIncreaseAltitude":
                    if (InFlightMode())
                    {
                        AddGeorgeLongHoldButton(AH64GeorgeButton.Up, 1000);
                    }
                    return;
                    
                // Down long presses
                case "wMsgGeorgeShutdownFull":
                    if (InGroundMode())
                    {
                        AddGeorgeLongButton(AH64GeorgeButton.Down);
                    }
                    return;
                case "wMsgGeorgeBreakOneEighty":
                    if (InCombatMode())
                    {
                        AddGeorgeLongButton(AH64GeorgeButton.Down);
                    }
                    return;
                case "wMsgGeorgeThreatWarningsOn":
                case "wMsgGeorgeThreatWarningsOff":
                    AddGeorgeLongButton(AH64GeorgeButton.Down);
                    return;
                case "wMsgGeorgeHoverBack":
                    if (InHoverMode())
                    {
                        AddGeorgeLongHoldButton(AH64GeorgeButton.Down, 1000);
                    }
                    return;
                case "wMsgGeorgeDecreaseAltitude":
                    if (InFlightMode())
                    {
                        AddGeorgeLongHoldButton(AH64GeorgeButton.Down, 1000);
                    }
                    return;

                // Left long presses
                case "wMsgGeorgeBreakLeft":
                    if (InCombatMode())
                    {
                        AddGeorgeLongButton(AH64GeorgeButton.Left);
                    }
                    return;
                case "wMsgGeorgeComeLeft":
                    if (InFlightMode())
                    {
                        AddGeorgeLongHoldButton(AH64GeorgeButton.Left, 1000);
                    }
                    return;
                case "wMsgGeorgeHoverLeft":
                    if (InHoverMode())
                    {
                        AddGeorgeLongHoldButton(AH64GeorgeButton.Left, 1000);
                    }
                    return;

                // Right long presses
                case "wMsgGeorgeCMWSOn":
                case "wMsgGeorgeCMWSOff":
                    if (InGroundMode())
                    {
                        AddGeorgeLongButton(AH64GeorgeButton.Right);
                    }
                    return;
                case "wMsgGeorgeBreakRight":
                    if (InCombatMode())
                    {
                        AddGeorgeLongButton(AH64GeorgeButton.Right);
                    }
                    return;
                case "wMsgGeorgeComeRight":
                    if (InFlightMode())
                    {
                        AddGeorgeLongHoldButton(AH64GeorgeButton.Right, 1000);
                    }
                    return;
                case "wMsgGeorgeHoverRight":
                    if (InHoverMode())
                    {
                        AddGeorgeLongHoldButton(AH64GeorgeButton.Right, 1000);
                    }
                    return;

                // Multifunction long presses
                case "wMsgGeorgeSetRadarAltitude":
                case "wMsgGeorgeSetBarometricAltitude":
                    if (InFlightMode())
                    {
                        AddGeorgeLongButton(AH64GeorgeButton.Multifunction);
                    }
                    return;
                case "wMsgGeorgeAddBattlePosition":
                case "wMsgGeorgeDeleteBattlePosition":
                    if (InCombatMode() || InHoverMode() || InDefenseMode())
                    {
                        AddGeorgeLongButton(AH64GeorgeButton.Multifunction);
                    }
                    return;

                // George PLT Defense Mode items
                // No menu mode checks required for these ones as these open
                // the DEFN menu prior to performing the command.
                case "wMsgGeorgeCMWSArm":
                    SelectCMWSArmSafe(AH64CMWSArmSafe.Armed);
                    return;
                case "wMsgGeorgeCMWSSafe":
                    SelectCMWSArmSafe(AH64CMWSArmSafe.Safe);
                    return;
                case "wMsgGeorgeCMWSAuto":
                    SelectCMWSMode(AH64CMWSMode.Auto);
                    return;
                case "wMsgGeorgeCMWSBypass":
                    SelectCMWSMode(AH64CMWSMode.Bypass);
                    return;
                case "wMsgGeorgeEvadeOff":              // TODO: Fix these, they need to track their state
                case "wMsgGeorgeEvadeLevel":
                case "wMsgGeorgeEvadeVertical":
                case "wMsgGeorgeEvadeMask":
                    OpenDefenseMenu();
                    AddGeorgeLongButton(AH64GeorgeButton.Left);
                    CloseDefenseMenu();
                    return;
                case "wMsgGeorgeCMDispenseNone":
                    SelectCMDispenseMode(AH64CMDispenseMode.None);
                    return;
                case "wMsgGeorgeCMDispenseChaff":
                    SelectCMDispenseMode(AH64CMDispenseMode.Chaff);
                    return;
                case "wMsgGeorgeCMDispenseFlares":
                    SelectCMDispenseMode(AH64CMDispenseMode.Flares);
                    return;
                case "wMsgGeorgeCMDispenseChaffAndFlares":
                    SelectCMDispenseMode(AH64CMDispenseMode.ChaffAndFlares);
                    return;
                case "wMsgGeorgeExtLightsOff":
                    SelectExteriorLightsMode(AH64ExteriorLightsMode.Off);
                    return;
                case "wMsgGeorgeExtLightsDay":
                    SelectExteriorLightsMode(AH64ExteriorLightsMode.Day);
                    return;
                case "wMsgGeorgeExtLightsNightBright":
                    SelectExteriorLightsMode(AH64ExteriorLightsMode.NightBright);
                    return;
                case "wMsgGeorgeExtLightsNightDim":
                    SelectExteriorLightsMode(AH64ExteriorLightsMode.NightDim);
                    return;
                case "wMsgGeorgeExtLightsFormation":
                    SelectExteriorLightsMode(AH64ExteriorLightsMode.Formation);
                    return;
                case "wMsgGeorgeWeaponsHold":
                    SelectRulesOfEngagementMode(AH64ROEMode.HoldFire);
                    return;
                case "wMsgGeorgeReturnFire":
                    SelectRulesOfEngagementMode(AH64ROEMode.ReturnFire);
                    return;
                case "wMsgGeorgeWeaponsFree":
                    SelectRulesOfEngagementMode(AH64ROEMode.WeaponsFree);
                    return;

                // Request Control when in the CPG seat
                case "wMsgGeorgeControlRequest":
                    AddGeorgeAction(AH64GeorgeButton.RequestControl, 1.0);
                    return;
            }

            Log.Write($"Command not available in {AH64GeorgeState.CurrentMenuMode} mode, or current AH-64 state", Colors.Warning);
            UI.Playsound.Sorry();
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
        public static void AddGeorgeLongHoldButton(AH64GeorgeButton button, int holdDurationMs)
        {
            AddGeorgeLongHoldButton((int)button, holdDurationMs);
        }

        public static void AddGeorgeLongHoldButton(int command, int holdDurationMs)
        {
            AddGeorgeAction(command, 1.0, 1200 + holdDurationMs);
            AddGeorgeAction(command, 0.0);
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

        private static void SelectMenuMode(string commandId)
        {
            if (commandId.Equals("wMsgGeorgeMenuNextMode"))
            {
                // If we were able to switch to the next mode then action this.
                if (AH64GeorgeState.SetNextMenuMode())
                {
                    AddGeorgeButton(AH64GeorgeButton.Left);
                    UI.Playsound.Commandcomplete();
                }
                
                return;
            }
            
            if (commandId.Equals("wMsgGeorgeMenuDefenseMode"))
            {
                AH64GeorgeState.SetDefenseMenuMode();
                OpenDefenseMenu();
                UI.Playsound.Commandcomplete();
                return;
            }

            var target = AH64MenuMode.Unknown;
            switch (commandId)
            {
                case "wMsgGeorgeMenuCombatMode":
                    target = AH64MenuMode.Combat;
                    break;
                case "wMsgGeorgeMenuFlightMode":
                    target = AH64MenuMode.Flight;
                    break;
                case "wMsgGeorgeMenuGroundMode":
                    target = AH64MenuMode.Ground;
                    break;
                case "wMsgGeorgeMenuHoverMode":
                    target = AH64MenuMode.Hover;
                    break;
            }

            var availableMenuModes = AH64GeorgeState.GetAvailableMenuModes();
            if (!availableMenuModes.Contains(target))
            {
                Log.Write("George menu mode " + target + " is not available in current menu modes.", Colors.Recognition);
                UI.Playsound.Sorry();
                return;
            }

            // Get the number of steps that were required to reach the menu that was set.
            int steps = AH64GeorgeState.SetMenuMode(target);
            for (int i  = 0; i < steps; i ++)
            {
                AddGeorgeButton(AH64GeorgeButton.Left);
            }

            UI.Playsound.Commandcomplete();
        }

        private static void ToggleApuOnOff(AH64Apu target)
        {
            if (!AH64GeorgeState.ApuOnOffState.Equals(target))
            {
                // We don't change the internal state here as we will receive a server
                // message to update this indicating if it's on or off.
                AddGeorgeButton(AH64GeorgeButton.Up);
            }
        }

        private static void SelectCMWSArmSafe(AH64CMWSArmSafe target)
        {
            // CMWS arm/safe is toggable so the commands for these don't set specific
            // values. We check the current state and command to prevent toggling with wrong commands.
            // Whilst we pre-emptively toggle the state here, the actual in-cockpit position
            // will be synched in the next server state update.
            if (!AH64GeorgeState.SelectedCMWSArmSafe.Equals(target))
            {
                OpenDefenseMenu();
                AddGeorgeButton(AH64GeorgeButton.Up);
                AddGeorgeButton(AH64GeorgeButton.Menu);

                AH64GeorgeState.SelectedCMWSArmSafe = target;
            }
        }

        private static void SelectCMWSMode(AH64CMWSMode target)
        {
            // CMWS arm/safe and auto/bypass are toggable, the commands for these don't set specific
            // values. We check the current state and command to prevent toggling with wrong commands.
            // Whilst we pre-emptively toggle the state here, the actual in-cockpit position
            // will be synched in the next server state update.
            if (!AH64GeorgeState.SelectedCMWSMode.Equals(target))
            {
                OpenDefenseMenu();
                AddGeorgeButton(AH64GeorgeButton.Down);
                CloseDefenseMenu();

                AH64GeorgeState.SelectedCMWSMode = target;
            }
        }

        private static void SelectCMDispenseMode(AH64CMDispenseMode target)
        {
            // The flares and chaff/flares options are not available if the CMWS is set to Auto.
            if (AH64GeorgeState.SelectedCMWSMode.Equals(AH64CMWSMode.Auto) 
                && (target.Equals(AH64CMDispenseMode.Flares) || target.Equals(AH64CMDispenseMode.ChaffAndFlares)))
            {
                Log.Write("Dispense mode " + target + " is not available with CMWS in Auto.", Colors.Recognition);
                UI.Playsound.Sorry();
                return;
            }

            var current = AH64GeorgeState.SelectedCMDispenseMode;
            if (!current.Equals(target))
            {
                int steps = AH64GeorgeState.GetCMDispenseSteps(current, target);

                OpenDefenseMenu();
                for (int i = 0; i < steps; i++)
                {
                    AddGeorgeLongButton(AH64GeorgeButton.Right, 80);
                }
                CloseDefenseMenu();

                AH64GeorgeState.SelectedCMDispenseMode = target;
            }
        }

        private static void SelectExteriorLightsMode(AH64ExteriorLightsMode target)
        {
            var current = AH64GeorgeState.SelectedExteriorLightsMode;
            if (!current.Equals(target))
            {
                int steps = AH64GeorgeState.GetExteriorLightsSteps(current, target);

                OpenDefenseMenu();
                for (int i = 0; i < steps; i++)
                {
                    AddGeorgeButton(AH64GeorgeButton.Right, 80);
                }
                CloseDefenseMenu();

                AH64GeorgeState.SelectedExteriorLightsMode = target;
            }
        }

        private static void SelectRulesOfEngagementMode(AH64ROEMode target)
        {
            var current = AH64GeorgeState.SelectedROEMode;
            if (!current.Equals(target))
            {
                int steps = AH64GeorgeState.GetRulesOfEngagementSteps(current, target);

                OpenDefenseMenu();
                for (int i = 0; i < steps; i++)
                {
                    AddGeorgeLongButton(AH64GeorgeButton.Up, 80);
                }
                CloseDefenseMenu();

                AH64GeorgeState.SelectedROEMode = target;
            }
        }

        private static void OpenDefenseMenu()
        {
            AddGeorgeLongButton(AH64GeorgeButton.Menu);
            AH64GeorgeState.SetDefenseMenuMode();
        }

        private static void CloseDefenseMenu()
        {
            AddGeorgeButton(AH64GeorgeButton.Menu);

            // When the DEFN menu is closed it defaults back to a mode based on the current aircraft state.
            if (AH64GeorgeState.IsAirbourne())
            {
                AH64GeorgeState.SetMenuMode(AH64MenuMode.Flight);
            }
            else if (AH64GeorgeState.IsHoverAvailable())
            {
                AH64GeorgeState.SetMenuMode(AH64MenuMode.Hover);
            }
            else
            {
                AH64GeorgeState.SetMenuMode(AH64MenuMode.Ground);
            }
        }

        private static bool InGroundMode()
        {
            return AH64GeorgeState.CurrentMenuMode.Equals(AH64MenuMode.Ground);
        }

        private static bool InHoverMode()
        {
            return AH64GeorgeState.CurrentMenuMode.Equals(AH64MenuMode.Hover);
        }

        private static bool InFlightMode()
        {
            return AH64GeorgeState.CurrentMenuMode.Equals(AH64MenuMode.Flight);
        }

        private static bool InCombatMode()
        {
            return AH64GeorgeState.CurrentMenuMode.Equals(AH64MenuMode.Combat);
        }

        private static bool InDefenseMode()
        {
            return AH64GeorgeState.CurrentMenuMode.Equals(AH64MenuMode.Defense);
        }
    }

}
