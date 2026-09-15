using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using VAICOM.Extensions.AICPG;
using VAICOM.Static;

namespace VAICOM
{

    namespace Servers
    {

        public static partial class Server
        {

            private static bool DetectFastOwnshipState(string receivedString)
            {
                const string prefix = "missiondata.update.ownship";
                receivedString = (receivedString ?? "").Trim();
                if (string.IsNullOrEmpty(receivedString) || !receivedString.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }

                try
                {
                    string[] parts = receivedString.Split(';');
                    Dictionary<string, string> values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                    for (int i = 1; i < parts.Length; i++)
                    {
                        string part = parts[i];
                        int idx = part.IndexOf('=');
                        if (idx <= 0 || idx >= part.Length - 1)
                        {
                            continue;
                        }

                        string key = part.Substring(0, idx).Trim();
                        string val = part.Substring(idx + 1).Trim();
                        if (key.Length > 0)
                        {
                            values[key] = val;
                        }
                    }

                    double x;
                    double y;
                    double z;
                    if (!double.TryParse(values.ContainsKey("x") ? values["x"] : "", System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out x)
                        || !double.TryParse(values.ContainsKey("y") ? values["y"] : "", System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out y)
                        || !double.TryParse(values.ContainsKey("z") ? values["z"] : "", System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out z))
                    {
                        return true;
                    }

                    double heading;
                    double? headingOpt = null;
                    if (double.TryParse(values.ContainsKey("hdg") ? values["hdg"] : "", System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out heading))
                    {
                        headingOpt = heading;
                    }

                    double parsed;
                    double? groundSpeedKnotsOpt = null;
                    if (double.TryParse(values.ContainsKey("gs_kts") ? values["gs_kts"] : "", System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out parsed)
                        || double.TryParse(values.ContainsKey("gskt") ? values["gskt"] : "", System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out parsed)
                        || double.TryParse(values.ContainsKey("gskts") ? values["gskts"] : "", System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out parsed))
                    {
                        if (!double.IsNaN(parsed) && !double.IsInfinity(parsed) && parsed >= 0)
                        {
                            groundSpeedKnotsOpt = parsed;
                        }
                    }
                    else if (double.TryParse(values.ContainsKey("gs_ms") ? values["gs_ms"] : "", System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out parsed)
                        || double.TryParse(values.ContainsKey("groundspeed_ms") ? values["groundspeed_ms"] : "", System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out parsed)
                        || double.TryParse(values.ContainsKey("groundspeed") ? values["groundspeed"] : "", System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out parsed)
                        || double.TryParse(values.ContainsKey("gs") ? values["gs"] : "", System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out parsed))
                    {
                        if (!double.IsNaN(parsed) && !double.IsInfinity(parsed) && parsed >= 0)
                        {
                            groundSpeedKnotsOpt = parsed * 1.9438444924406;
                        }
                    }

                    int wowState = -1;
                    if (int.TryParse(values.ContainsKey("wow") ? values["wow"] : "", out wowState))
                    {
                        wowState = wowState != 0 ? 1 : 0;
                    }
                    else
                    {
                        wowState = -1;
                    }

                    if (State.IsAH64D)
                    {
                        AH64GeorgeState.GroundSpeed = groundSpeedKnotsOpt ?? 0;
                    }
                    Extensions.Kneeboard.OpenKneeboardBridge.UpdateFastOwnship(x, y, z, headingOpt, groundSpeedKnotsOpt, wowState);
                }
                catch (Exception e)
                {
                    Log.Write("Problem parsing ownship state message: " + e.Message, Colors.Inline);
                }

                return true;
            }

            public static void ProcessRawServerMessage(string receivedString)
            {
                try
                {
                    string trimmed = (receivedString ?? "").Trim();
                    if (trimmed == "4000")
                    {
                        return;
                    }

                    Extensions.Kneeboard.OpenKneeboardBridge.AppendRawServerMessage(trimmed);

                    if (DetectAH64State(trimmed))
                    {
                        return;
                    }

                    if (DetectFastOwnshipState(trimmed))
                    {
                        return;
                    }

                    if (!ValidateRaw(trimmed))
                    {
                        Log.Write("VOID SERVER MESSAGE: " + trimmed, Static.Colors.Inline);
                        Extensions.Kneeboard.OpenKneeboardBridge.UpdateStatus("Warning: waiting for mission data...", "warning");
                        return;
                    }

                    if (DetectEndMission(trimmed))
                    {
                        EndMission();
                        return;
                    }

                    if (!trimmed.StartsWith("{", StringComparison.Ordinal))
                    {
                        Log.Write("NON-JSON mission update ignored: " + trimmed, Static.Colors.Inline);
                        return;
                    }

                    ServerMessage decodedMessage = DecodeRawMessage(trimmed);
                    if (decodedMessage == null)
                    {
                        Log.Write("NOT DECODED: " + receivedString, Static.Colors.Inline);
                        Extensions.Kneeboard.OpenKneeboardBridge.UpdateStatus("Error: server message decode failed.", "error");
                        return;
                    }

                    UpdateServerState(decodedMessage);
                    Extensions.Kneeboard.OpenKneeboardBridge.UpdateStatus("Command sent successfully.", "sent");

                }
                catch (Exception e)
                {
                    Log.Write("There was a problem processing server message: " + e.StackTrace, Static.Colors.Inline);
                    Extensions.Kneeboard.OpenKneeboardBridge.UpdateStatus("Error: problem processing server message.", "error");
                }
            }

            public static bool DetectAH64State(string receivedString)
            {
                const string prefix = "missiondata.update.ah64state";
                receivedString = (receivedString ?? "").Trim();
                if (string.IsNullOrEmpty(receivedString) || !receivedString.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }

                try
                {
                    string[] parts = receivedString.Split(';');
                    Dictionary<string, string> values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                    for (int i = 1; i < parts.Length; i++)
                    {
                        string part = parts[i];
                        int idx = part.IndexOf('=');
                        if (idx <= 0 || idx >= part.Length - 1)
                        {
                            continue;
                        }

                        string key = part.Substring(0, idx).Trim();
                        string val = part.Substring(idx + 1).Trim();
                        if (key.Length > 0)
                        {
                            values[key] = val;
                        }
                    }

                    bool apuOn = values.TryGetValue("apu", out string apuValue) && apuValue.Equals("1");
                    bool cmwsArmed = values.TryGetValue("cmwsArmed", out string cmwsArmedValue) && cmwsArmedValue.Equals("1");
                    bool cmwsBypass = values.TryGetValue("cmwsBypass", out string cmwsBypassValue) && cmwsBypassValue.Equals("1");
                    bool gunAvailable = values.TryGetValue("gun", out string gunValue) && gunValue.Equals("1");
                    bool rocketsAvailable = values.TryGetValue("rockets", out string rocketsValue) && rocketsValue.Equals("1");
                    bool missilesAvailable = values.TryGetValue("missiles", out string missilesValue) && missilesValue.Equals("1");
                    bool wow = values.TryGetValue("wow", out string wowValue) && wowValue.Equals("1");

                    int navigationLights = values.TryGetValue("navigationLights", out string navigationLightsValue) && int.TryParse(navigationLightsValue, out int navLights) ? navLights : 0;
                    int antiCollisionLights = values.TryGetValue("antiCollisionLights", out string antiCollisionLightsValue) && int.TryParse(antiCollisionLightsValue, out int acLights) ? acLights : 0;
                    int formationLights = values.TryGetValue("formationLights", out string formationLightsValue) && int.TryParse(formationLightsValue, out int formLights) ? formLights : 0;

                    if (double.TryParse(values.ContainsKey("rpmR") ? values["rpmR"] : "", System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double engineRpm))
                    {
                        if (!double.IsNaN(engineRpm) && !double.IsInfinity(engineRpm) && engineRpm >= 0)
                        {
                            AH64GeorgeState.EngineRpm = engineRpm;
                        }
                    }

                    AH64GeorgeState.ApuOnOffState = apuOn ? AH64Apu.On : AH64Apu.Off;
                    AH64GeorgeState.SelectedCMWSArmSafe = cmwsArmed ? AH64CMWSArmSafe.Armed : AH64CMWSArmSafe.Safe;
                    AH64GeorgeState.SelectedCMWSMode = cmwsBypass ? AH64CMWSMode.Bypass : AH64CMWSMode.Auto;
                    AH64GeorgeState.GunAvailable = gunAvailable;
                    AH64GeorgeState.RocketsAvailable = rocketsAvailable;
                    AH64GeorgeState.MissilesAvailable = missilesAvailable;
                    AH64GeorgeState.WeaponStateValid = true;

                    AH64GeorgeState.SetExteriorLightsMode(navigationLights, antiCollisionLights, formationLights);

                    if (!WeaponStillAvailable(AH64GeorgeState.SelectedWeapon))
                    {
                        AH64GeorgeState.SelectedWeapon = AH64WeaponMode.NoWeapon;
                    }

                    AH64GeorgeState.WeightOnWheels = wow;
                    State.currentstate.airborne = !wow;
                }
                catch (Exception e)
                {
                    Log.Write("Problem parsing AH64 state message: " + e.Message, Colors.Inline);
                }

                return true;
            }

            private static bool WeaponStillAvailable(AH64WeaponMode mode)
            {
                switch (mode)
                {
                    case AH64WeaponMode.Gun:
                        return AH64GeorgeState.GunAvailable;
                    case AH64WeaponMode.Missiles:
                        return AH64GeorgeState.MissilesAvailable;
                    case AH64WeaponMode.Rockets:
                        return AH64GeorgeState.RocketsAvailable;
                    case AH64WeaponMode.NoWeapon:
                    case AH64WeaponMode.Unknown:
                    default:
                        return true;
                }
            }

            public static bool ValidateRaw(string receivedString)
            {
                string inputfilter = "missiondata.update";
                if (string.IsNullOrWhiteSpace(receivedString) || receivedString.IndexOf(inputfilter, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            public static ServerMessage DecodeRawMessage(string receivedString)
            {
                try
                {
                    JToken token = JToken.Parse(receivedString);
                    JObject obj = token as JObject;
                    if (obj != null)
                    {
                        NormalizeDictionaryToken(obj, "atcmetars");
                        NormalizeDictionaryToken(obj, "atcicaotypes");
                        return obj.ToObject<ServerMessage>();
                    }

                    return JsonConvert.DeserializeObject<ServerMessage>(receivedString);
                }
                catch (Exception e)
                {
                    Log.Write("JSON eror - server message decoding failed: " + e.Message, Colors.Inline);
                    return null;
                }

            }

            private static void NormalizeDictionaryToken(JObject obj, string propertyName)
            {
                if (obj == null || string.IsNullOrWhiteSpace(propertyName))
                {
                    return;
                }

                if (!obj.TryGetValue(propertyName, StringComparison.OrdinalIgnoreCase, out JToken value) || value == null)
                {
                    return;
                }

                if (value.Type == JTokenType.Array)
                {
                    obj[propertyName] = new JObject();
                }
            }


        }
    }
}
