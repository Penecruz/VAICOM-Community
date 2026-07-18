using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using VAICOM.Static;

namespace VAICOM
{

    namespace Servers
    {

        public static partial class Server
        {

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

                    if (DetectAH64WeaponState(trimmed))
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

            public static bool DetectAH64WeaponState(string receivedString)
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

                    bool gunAvailable = values.TryGetValue("gun", out string gunValue) && gunValue.Equals("1");
                    bool rocketsAvailable = values.TryGetValue("rockets", out string rocketsValue) && rocketsValue.Equals("1");
                    bool missilesAvailable = values.TryGetValue("missiles", out string missilesValue) && missilesValue.Equals("1");
                    bool wow = values.TryGetValue("wow", out string wowValue) && wowValue.Equals("1");

                    State.AH64GeorgeGunAvailable = gunAvailable;
                    State.AH64GeorgeRocketsAvailable = rocketsAvailable;
                    State.AH64GeorgeMissilesAvailable = missilesAvailable;
                    State.AH64GeorgeWeaponStateValid = true;
                    State.AH64GeorgeWowFromExport = wow;

                    if (!WeaponStillAvailable(State.AH64GeorgeSelectedWeapon))
                    {
                        State.AH64GeorgeSelectedWeapon = State.AH64GeorgeWeaponMode.NoWeapon;
                    }
                }
                catch (Exception e)
                {
                    Log.Write("Problem parsing AH64 state message: " + e.Message, Colors.Inline);
                }

                return true;
            }

            private static bool WeaponStillAvailable(State.AH64GeorgeWeaponMode mode)
            {
                switch (mode)
                {
                    case State.AH64GeorgeWeaponMode.Gun:
                        return State.AH64GeorgeGunAvailable;
                    case State.AH64GeorgeWeaponMode.Missiles:
                        return State.AH64GeorgeMissilesAvailable;
                    case State.AH64GeorgeWeaponMode.Rockets:
                        return State.AH64GeorgeRocketsAvailable;
                    case State.AH64GeorgeWeaponMode.NoWeapon:
                    case State.AH64GeorgeWeaponMode.Unknown:
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
