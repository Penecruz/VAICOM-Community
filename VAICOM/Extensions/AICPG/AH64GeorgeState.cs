using System;
using System.Collections.Generic;
using System.Linq;
using VAICOM.Static;

namespace VAICOM.Extensions.AICPG
{
    public enum  AH64Apu
    {
        On,
        Off
    }

    public enum AH64CMWSArmSafe
    {
        Armed,
        Safe
    }

    public enum  AH64CMWSMode
    {
        Auto,
        Bypass
    }

    public enum AH64CMDispenseMode
    {
        None,
        Chaff,
        Flares,
        ChaffAndFlares
    }

    public enum AH64EvadeMode
    {
        Off,
        Level,
        Vertical,
        Mask
    }

    public enum AH64WeaponMode
    {
        Unknown,
        NoWeapon,
        Gun,
        Missiles,
        Rockets
    }

    public enum AH64ROEMode
    {
        HoldFire,
        ReturnFire,
        WeaponsFree
    }

    public enum AH64ExteriorLightsMode
    {
        Off,
        Day,
        NightBright,
        NightDim,
        Formation
    }

    public enum AH64MenuMode
    {
        Unknown,
        Ground,
        Hover,
        Flight,
        Combat,
        Defense
    }

    public class AH64GeorgeState
    {
        private static AH64Apu _ApuOnOffState = AH64Apu.Off;
        public static AH64Apu ApuOnOffState
        {
            get => _ApuOnOffState;
            set
            {
                if (_ApuOnOffState == value) return;

                _ApuOnOffState = value;

                // If the APU is turned on while on the ground, force Ground and prevent selecting Hover.
                if (_ApuOnOffState == AH64Apu.On && WeightOnWheels)
                {
                    if (!CurrentMenuMode.Equals(AH64MenuMode.Ground))
                    {
                        CurrentMenuMode = AH64MenuMode.Ground;
                    }
                }
            }
        }

        public static AH64CMWSArmSafe SelectedCMWSArmSafe { get; set; } = AH64CMWSArmSafe.Safe;
        public static AH64CMDispenseMode SelectedCMDispenseMode { get; set; } = AH64CMDispenseMode.None;

        private static AH64CMWSMode _SelectedCMWSMode = AH64CMWSMode.Auto;
        public static AH64CMWSMode SelectedCMWSMode
        {
            get => _SelectedCMWSMode;
            set
            {
                _SelectedCMWSMode = value;

                // If on Flares and Chaff and CMWS is changed to Auto then the dispense mode will be
                // automatically changed to Chaff. If it was on Flares then it will be changed to None.
                if (value.Equals(AH64CMWSMode.Auto))
                {
                    if (SelectedCMDispenseMode.Equals(AH64CMDispenseMode.Flares))
                    {
                        SelectedCMDispenseMode = AH64CMDispenseMode.None;
                    }
                    else if (SelectedCMDispenseMode.Equals(AH64CMDispenseMode.ChaffAndFlares))
                    {
                        SelectedCMDispenseMode = AH64CMDispenseMode.Chaff;
                    }
                }
            }
        }

        public static AH64EvadeMode SelectedEvadeMode { get; set; } = AH64EvadeMode.Off;
        public static AH64ExteriorLightsMode SelectedExteriorLightsMode { get; set; } = AH64ExteriorLightsMode.Off;
        public static AH64ROEMode SelectedROEMode { get; set; } = AH64ROEMode.HoldFire;
        public static AH64WeaponMode SelectedWeapon { get; set; } = AH64WeaponMode.Unknown;

        // Menu modes can only be set internally so that logic can be applied to them.
        private static AH64MenuMode _CurrentedMenuMode = AH64MenuMode.Unknown;
        public static AH64MenuMode CurrentMenuMode
        {
            get => _CurrentedMenuMode;
            private set
            {
                PreviousMenuMode = _CurrentedMenuMode;
                _CurrentedMenuMode = value;

                if (!IsGeorgeCPG())
                {
                    Log.Write($"Switched to {value} mode", Colors.Text);
                }
            }
        }

        public static AH64MenuMode PreviousMenuMode { get; private set; } = AH64MenuMode.Unknown;

        private static double _GroundSpeed = 0;
        public static double GroundSpeed
        {
            get => _GroundSpeed;
            set
            {
                if (_GroundSpeed == value) return;

                // Set the value so IsHoverAvailable() uses up-to-date data
                _GroundSpeed = value;

                // TODO: think is now redundant and wrong????
                // if we're in hover and speed rises above the hover threshold, move to flight
                if (value > 10 && CurrentMenuMode.Equals(AH64MenuMode.Hover))
                {
                    CurrentMenuMode = AH64MenuMode.Flight;
                }
            }
        }

        private static double _EngineRpm = 0;
        public static double EngineRpm
        {
            get => _EngineRpm;
            set
            {
                _EngineRpm = value;
                // Only force Ground when engines drop below threshold while on the ground.
                if (value < 70 && WeightOnWheels && !CurrentMenuMode.Equals(AH64MenuMode.Ground))
                {
                    CurrentMenuMode = AH64MenuMode.Ground;
                }

                // Do NOT auto-switch from Ground to Hover when RPM rises.
                // Switching to Hover remains a user action (or occurs on landing via WeightOnWheels).
            }
        }

        private static bool _WeightOnWheels = true;
        public static bool WeightOnWheels
        {
            private get => _WeightOnWheels;
            set
            {
                if (value == _WeightOnWheels)
                {
                    return;
                }

                bool wasAirborne = !_WeightOnWheels;

                if (value) // landing
                {
                    // if we just landed, sync weapons (de-WAS)
                    if (wasAirborne)
                    {
                        ForceNoWeaponLocalSync("weight-on-wheels", AH64GeorgeState.SelectedWeapon != AH64WeaponMode.Unknown);
                    }

                    // Determine which mode to automatically switch to based on current sensors.
                    // Use the incoming value to determine whether hover can be selected when landed.
                    bool hoverAvailable = IsHoverAvailable();
                    bool hoverSelectable = CanSelectHover(value);
                    var desired = (hoverAvailable && hoverSelectable) ? AH64MenuMode.Hover : AH64MenuMode.Ground;

                    // Log when hover would be available but is blocked due to APU on while on ground.
                    if (hoverAvailable && !hoverSelectable)
                    {
                        Log.Write("Hover selection blocked on landing: APU is on while on the ground. Forcing Ground mode.", Colors.Recognition);
                    }

                    if (!CurrentMenuMode.Equals(desired))
                    {
                        CurrentMenuMode = desired;
                    }
                }
                else // taking off
                {
                    // Pick desired mode using current sensors: prefer Hover when available, otherwise Flight
                    // Note: CanSelectHover() matters only for ground; in air allow Hover if available.
                    var desired = IsHoverAvailable() ? AH64MenuMode.Hover : AH64MenuMode.Flight;

                    if (!CurrentMenuMode.Equals(desired))
                    {
                        CurrentMenuMode = desired;
                    }
                }

                _WeightOnWheels = value;
            }
        }

        public static bool HasBattlePosition { get; set; } = false;

        public static bool GunAvailable;
        public static bool RocketsAvailable;
        public static bool MissilesAvailable;
        public static bool WeaponStateValid;

        private static readonly List<AH64CMDispenseMode> cmDispenseOrder = new List<AH64CMDispenseMode>
        {
            AH64CMDispenseMode.None,
            AH64CMDispenseMode.Chaff,
            AH64CMDispenseMode.Flares,
            AH64CMDispenseMode.ChaffAndFlares
        };

        private static readonly List<AH64EvadeMode> evadeOrder = new List<AH64EvadeMode>
        {
            AH64EvadeMode.Off,
            AH64EvadeMode.Level,
            AH64EvadeMode.Vertical,
            AH64EvadeMode.Mask
        };

        private static readonly List<AH64ROEMode> rulesOfEngagementOrder = new List<AH64ROEMode>
        {
            AH64ROEMode.HoldFire,
            AH64ROEMode.ReturnFire,
            AH64ROEMode.WeaponsFree
        };

        private static readonly List<AH64ExteriorLightsMode> exteriorLightsOrder = new List<AH64ExteriorLightsMode>
        {
            AH64ExteriorLightsMode.Off,
            AH64ExteriorLightsMode.Day,
            AH64ExteriorLightsMode.NightBright,
            AH64ExteriorLightsMode.NightDim,
            AH64ExteriorLightsMode.Formation
        };
        
        public static void InitializeState()
        {
            WeaponStateValid = false;
            GunAvailable = false;
            RocketsAvailable = false;
            MissilesAvailable = false;
            SelectedEvadeMode = AH64EvadeMode.Off;
            SelectedROEMode = AH64ROEMode.HoldFire;
            SelectedWeapon = AH64WeaponMode.Unknown;
            PreviousMenuMode = AH64MenuMode.Unknown;
            CurrentMenuMode = AH64MenuMode.Unknown;
            HasBattlePosition = false;
        }

        public static void UpdateWeaponState()
        {
            bool hadValidWeaponState = WeaponStateValid;
            bool previousGunAvailable = GunAvailable;
            bool previousMissilesAvailable = MissilesAvailable;
            bool previousRocketsAvailable = RocketsAvailable;

            RefreshWeaponAvailabilityFromPayloadProbe();
            ForceNoWeaponOnDepletedSelection(hadValidWeaponState, previousGunAvailable, previousMissilesAvailable, previousRocketsAvailable);

            if (!IsAirbourne() && SelectedWeapon != AH64WeaponMode.NoWeapon)
            {
                ForceNoWeaponLocalSync("weight-on-wheels", false);
            }
        }

        private static void RefreshWeaponAvailabilityFromPayloadProbe()
        {
            try
            {
                var payload = State.currentstate?.payload;
                if (payload == null)
                {
                    return;
                }

                GunAvailable = payload.Cannon != null && payload.Cannon.shells > 0;

                bool missilesAvailable = false;
                bool rocketsAvailable = false;

                if (payload.Stations != null)
                {
                    foreach (var station in payload.Stations)
                    {
                        if (station == null || string.IsNullOrEmpty(station.CLSID))
                        {
                            continue;
                        }

                        string clsid = station.CLSID.ToUpperInvariant();
                        bool hasCount = station.count > 0;

                        if (IsMissileStation(clsid))
                        {
                            if (hasCount)
                            {
                                missilesAvailable = true;
                            }
                        }

                        if (IsRocketStation(clsid))
                        {
                            if (hasCount)
                            {
                                rocketsAvailable = true;
                            }
                        }
                    }
                }

                MissilesAvailable = missilesAvailable;
                RocketsAvailable = rocketsAvailable;
                WeaponStateValid = true;
            }
            catch
            {
            }
        }

        private static bool IsMissileStation(string clsid)
        {
            if (string.IsNullOrEmpty(clsid))
            {
                return false;
            }

            if (clsid.Contains("EMPTY") || clsid.Contains("INERT") || clsid.Contains("DUMMY"))
            {
                return false;
            }

            return clsid.Contains("AGM_114")
                || clsid.Contains("HELLFIRE")
                || clsid.Contains("88D18A5E-99C8-4B04-B40B-1C02F2018B6E")
                || clsid.Contains("M299")
                || clsid.Contains("M310");
        }

        private static bool IsRocketStation(string clsid)
        {
            return clsid.Contains("HYDRA")
                || clsid.Contains("M261")
                || clsid.Contains("M260")
                || clsid.Contains("FFAR")
                || clsid.Contains("APKWS")
                || clsid.Contains("M151")
                || clsid.Contains("M229");
        }

        private static void ForceNoWeaponOnDepletedSelection(bool hadValidWeaponState, bool previousGunAvailable, bool previousMissilesAvailable, bool previousRocketsAvailable)
        {
            if (!WeaponStateValid)
            {
                return;
            }

            var selected = SelectedWeapon;
            if (selected == AH64WeaponMode.Unknown || selected == AH64WeaponMode.NoWeapon)
            {
                return;
            }

            if (WeaponAvailable(selected))
            {
                return;
            }

            if (!hadValidWeaponState || !WeaponAvailableFromSnapshot(selected, previousGunAvailable, previousMissilesAvailable, previousRocketsAvailable))
            {
                return;
            }

            if (selected == AH64WeaponMode.Missiles && HasEmptyM299Stations())
            {
                return;
            }

            CPGCommandHandler.AddGeorgeAction(AH64GeorgeButton.Left, 1.0, 2000);
            CPGCommandHandler.AddGeorgeAction(AH64GeorgeButton.Left, 0.0, 80);
            SelectedWeapon = AH64WeaponMode.NoWeapon;
            if (State.activeconfig.RIO_Messages)
            {
                State.currentmessage.dspmsg = "GEORGE ammo sync:\nSelected weapon depleted. Forcing No WPN (de-WAS).";
                State.currentmessage.msgdur = 5;
            }
            Log.Write("AH-64D George ammo sync: " + selected + " depleted, forcing No WPN with 2s delay.", Colors.Warning);
        }

        private static bool HasEmptyM299Stations()
        {
            var payload = State.currentstate != null ? State.currentstate.payload : null;
            if (payload == null || payload.Stations == null)
            {
                return false;
            }

            foreach (var station in payload.Stations)
            {
                if (station == null || string.IsNullOrEmpty(station.CLSID))
                {
                    continue;
                }

                string clsid = station.CLSID.ToUpperInvariant();
                if (clsid.Contains("M299_EMPTY"))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool WeaponAvailableFromSnapshot(AH64WeaponMode mode, bool gunAvailable, bool missilesAvailable, bool rocketsAvailable)
        {
            switch (mode)
            {
                case AH64WeaponMode.Gun:
                    return gunAvailable;
                case AH64WeaponMode.Missiles:
                    return missilesAvailable;
                case AH64WeaponMode.Rockets:
                    return rocketsAvailable;
                case AH64WeaponMode.NoWeapon:
                case AH64WeaponMode.Unknown:
                default:
                    return true;
            }
        }

        public static bool ForceNoWeaponLocalSync(string reason, bool logWhenAlreadyNoWeapon)
        {
            var previous = SelectedWeapon;
            if (previous != AH64WeaponMode.NoWeapon)
            {
                SelectedWeapon = AH64WeaponMode.NoWeapon;
                return true;
            }

            return false;
        }

        public static int GetWeaponCycleSteps(AH64WeaponMode from, AH64WeaponMode to)
        {
            if (from == to)
            {
                return 0;
            }

            var order = GetWeaponCycleOrder();
            int fromIndex = order.IndexOf(from);
            int toIndex = order.IndexOf(to);

            if (fromIndex < 0)
            {
                fromIndex = 0;
            }

            if (toIndex < 0)
            {
                return 0;
            }

            if (toIndex >= fromIndex)
            {
                return toIndex - fromIndex;
            }

            return (order.Count - fromIndex) + toIndex;
        }

        public static List<AH64WeaponMode> GetWeaponCycleOrder()
        {
            var order = new List<AH64WeaponMode>
                    {
                        AH64WeaponMode.NoWeapon
                    };

            if (GunAvailable)
            {
                order.Add(AH64WeaponMode.Gun);
            }

            if (MissilesAvailable)
            {
                order.Add(AH64WeaponMode.Missiles);
            }

            if (RocketsAvailable)
            {
                order.Add(AH64WeaponMode.Rockets);
            }

            return order;
        }


        public static bool WeaponAvailable(AH64WeaponMode mode)
        {
            if (!WeaponStateValid)
            {
                return true;
            }

            switch (mode)
            {
                case AH64WeaponMode.NoWeapon:
                    return true;
                case AH64WeaponMode.Gun:
                    return GunAvailable;
                case AH64WeaponMode.Missiles:
                    return MissilesAvailable;
                case AH64WeaponMode.Rockets:
                    return RocketsAvailable;
                default:
                    return false;
            }
        }

        public static List<AH64MenuMode> GetAvailableMenuModes()
        {
            // The Defense mode is always available so is not in this list as it is displayed
            // using a different action, i.e. a long press of the menu button. The other modes
            // depend on the current state of the aircraft.
            var order = new List<AH64MenuMode>();

            // Menu modes are available based on the current state of the aircraft.
            // NOTE: These MUST be ordered alphabetically!!!
            if (!IsAirbourne())
            {
                if (IsHoverAvailable() && CanSelectHover())
                {
                    order.Add(AH64MenuMode.Hover);
                }
                order.Add(AH64MenuMode.Ground);
                return order;
            }

            if (IsHoverAvailable())
            {
                order.Add(AH64MenuMode.Hover);
            }
            order.Add(AH64MenuMode.Combat);
            order.Add(AH64MenuMode.Flight);

            return order;
        }

        public static bool SetNextMenuMode()
        {
            var availableModes = GetAvailableMenuModes();

            // If currently in DEFN mode, switch to the first available mode in the list.
            if (CurrentMenuMode == AH64MenuMode.Defense)
            {
                CurrentMenuMode = availableModes[0];
                return true;
            }

            // Handle the case where we only have one mode available, excluding DEFN mode.
            // For example, when on the ground and engines are not in FLY, in which case only GND mode is available.
            if (availableModes.Count() == 1)
            {
                return false;
            }
            
            int currentIndex = availableModes.IndexOf(CurrentMenuMode);
            // Wrap to the first entry in the list if the current mode is not found or is the last entry in the list.
            int nextIndex = (currentIndex + 1) % availableModes.Count;
            
            CurrentMenuMode = availableModes[nextIndex];

            return true;
        }

        public static int SetMenuMode(AH64MenuMode target)
        {
            if (CurrentMenuMode.Equals(target))
            {
                return 0;
            }
            
            // Don't allow selecting Hover if APU is on while on the ground.
            if (target == AH64MenuMode.Hover && !CanSelectHover())
            {
                Log.Write("Hover selection blocked: APU is on while on the ground.", Colors.Recognition);
                UI.Playsound.Sorry();
                return 0;
            }

            var order = GetAvailableMenuModes();
            int fromIndex = order.IndexOf(CurrentMenuMode);
            int toIndex = order.IndexOf(target);

            if (fromIndex < 0)
            {
                fromIndex = 0;
            }

            if (toIndex < 0)
            {
                return 0;
            }

            int steps;
            if (toIndex >= fromIndex)
            {
                steps = toIndex - fromIndex;
            }
            else
            {
                steps = (order.Count - fromIndex) + toIndex;
            }

            // If we are currently in the DEFN menu then add 1 to the steps as that menu is always available.
            // This is so that it steps away from the DEFN menu first before the steps for the modes in the standard menu.
            if (CurrentMenuMode == AH64MenuMode.Defense)
            {
                steps++;
            }

            CurrentMenuMode = target;

            return steps;
        }

        public static void SetDefenseMenuMode()
        {
            CurrentMenuMode = AH64MenuMode.Defense;
        }

        public static int GetCMDispenseSteps(AH64CMDispenseMode from, AH64CMDispenseMode to)
        {
            if (from == to)
            {
                return 0;
            }

            var order = GetCMDispenseOrder();
            int fromIndex = order.IndexOf(from);
            int toIndex = order.IndexOf(to);

            if (fromIndex < 0)
            {
                fromIndex = 0;
            }

            if (toIndex < 0)
            {
                return 0;
            }

            if (toIndex >= fromIndex)
            {
                return toIndex - fromIndex;
            }

            return (cmDispenseOrder.Count - fromIndex) + toIndex;
        }

        private static List<AH64CMDispenseMode> GetCMDispenseOrder()
        {
            // When CMWS is in Bypass mode all dispense modes are available.
            if (SelectedCMWSMode.Equals(AH64CMWSMode.Bypass))
            {
                return cmDispenseOrder;
            }

            // When CMWS is in Auto mode, only None and Chaff are available.
            var order = new List<AH64CMDispenseMode>
            {
                AH64CMDispenseMode.None,
                AH64CMDispenseMode.Chaff,
            };

            return order;
        }

        public static int GetEvadeSteps(AH64EvadeMode from, AH64EvadeMode to)
        {
            if (from == to)
            {
                return 0;
            }

            int fromIndex = evadeOrder.IndexOf(from);
            int toIndex = evadeOrder.IndexOf(to);

            if (fromIndex < 0)
            {
                fromIndex = 0;
            }

            if (toIndex < 0)
            {
                return 0;
            }

            if (toIndex >= fromIndex)
            {
                return toIndex - fromIndex;
            }

            return (evadeOrder.Count - fromIndex) + toIndex;
        }

        public static int GetExteriorLightsSteps(AH64ExteriorLightsMode from, AH64ExteriorLightsMode to)
        {
            if (from == to)
            {
                return 0;
            }

            int fromIndex = exteriorLightsOrder.IndexOf(from);
            int toIndex = exteriorLightsOrder.IndexOf(to);

            if (fromIndex < 0)
            {
                fromIndex = 0;
            }

            if (toIndex < 0)
            {
                return 0;
            }

            if (toIndex >= fromIndex)
            {
                return toIndex - fromIndex;
            }

            return (exteriorLightsOrder.Count - fromIndex) + toIndex;
        }

        public static int GetRulesOfEngagementSteps(AH64ROEMode from, AH64ROEMode to)
        {
            if (from == to)
            {
                return 0;
            }

            int fromIndex = rulesOfEngagementOrder.IndexOf(from);
            int toIndex = rulesOfEngagementOrder.IndexOf(to);

            if (fromIndex < 0)
            {
                fromIndex = 0;
            }

            if (toIndex < 0)
            {
                return 0;
            }

            if (toIndex >= fromIndex)
            {
                return toIndex - fromIndex;
            }

            return (rulesOfEngagementOrder.Count - fromIndex) + toIndex;
        }

        public static void SetExteriorLightsMode(int navigationLights, int antiCollisionLights, int formationLights)
        {
            if (navigationLights == 0 && antiCollisionLights == 1 && formationLights == 0)
            {
                SelectedExteriorLightsMode = AH64ExteriorLightsMode.Day;
            }
            else if (navigationLights == 1 && antiCollisionLights == -1 && formationLights == 0)
            {
                SelectedExteriorLightsMode = AH64ExteriorLightsMode.NightBright;
            }
            else if (navigationLights == -1 && antiCollisionLights == 0 && formationLights == 1)
            {
                SelectedExteriorLightsMode = AH64ExteriorLightsMode.NightDim;
            }
            else if (navigationLights == 0 && antiCollisionLights == 0 && formationLights == 1)
            {
                SelectedExteriorLightsMode = AH64ExteriorLightsMode.Formation;
            }
            else
            {
                SelectedExteriorLightsMode = AH64ExteriorLightsMode.Off;
            }
        }

        public static bool IsAirbourne()
        {
            return !WeightOnWheels;
        }

        public static bool IsHoverAvailable()
        {
            return GroundSpeed < 10 && EngineRpm > 70;
        }

        private static bool CanSelectHover()
        {
            return CanSelectHover(_WeightOnWheels);
        }

        private static bool CanSelectHover(bool weightOnWheels)
        {
            // Hover cannot be selected when on the ground and the APU is on.
            return !(ApuOnOffState == AH64Apu.On && weightOnWheels);
        }

        public static bool IsGeorgeCPG()
        {
            return Helpers.Common.IsAH64PilotSeatActive();
        }
    }
}
