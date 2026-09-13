using System.Collections.Generic;
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
        ReturnFire,
        WeaponsFree,
        HoldFire
    }

    public enum AH64ExteriorLightsMode
    {
        Off,
        Day,
        NightBright,
        NightDim,
        Formation
    }

    public class AH64GeorgeState
    {
        public static AH64Apu ApuOnOffState { get; set; } = AH64Apu.Off;
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

        public static AH64ExteriorLightsMode SelectedExteriorLightsMode { get; set; } = AH64ExteriorLightsMode.Off;
        public static AH64ROEMode SelectedROEMode { get; set; } = AH64ROEMode.HoldFire;
        public static AH64WeaponMode SelectedWeapon { get; set; } = AH64WeaponMode.Unknown;
        
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

        public static void UpdateWeaponState()
        {
            bool hadValidWeaponState = WeaponStateValid;
            bool previousGunAvailable = GunAvailable;
            bool previousMissilesAvailable = MissilesAvailable;
            bool previousRocketsAvailable = RocketsAvailable;

            RefreshWeaponAvailabilityFromPayloadProbe();
            ForceNoWeaponOnDepletedSelection(hadValidWeaponState, previousGunAvailable, previousMissilesAvailable, previousRocketsAvailable);

            if (State.currentstate != null && !State.currentstate.airborne && SelectedWeapon != AH64WeaponMode.NoWeapon)
            {
                ForceNoWeaponLocalSync("weight-on-wheels", false);
            }
        }

        private static void RefreshWeaponAvailabilityFromPayloadProbe()
        {
            try
            {
                var payload = State.currentstate != null ? State.currentstate.payload : null;
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
    }
}
