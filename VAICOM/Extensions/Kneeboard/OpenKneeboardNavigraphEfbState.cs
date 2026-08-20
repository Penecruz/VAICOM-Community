using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json.Linq;
using VAICOM.Static;

namespace VAICOM
{
    namespace Extensions
    {
        namespace Kneeboard
        {
            public static class OpenKneeboardNavigraphEfbState
            {
                private static readonly byte[] AuthEntropy = Encoding.UTF8.GetBytes("VAICOM.OKB.NAVIGRAPH.EFB.AUTH.v1");
                private const string MockPayloadPrefix = "MOCKJSON:";

                public static void SetEncryptedAuthBlob(string plainAuthPayload)
                {
                    string plain = plainAuthPayload ?? "";
                    if (string.IsNullOrWhiteSpace(plain))
                    {
                        State.activeconfig.OpenKneeboard_EfbAuthBlob = "";
                        return;
                    }

                    byte[] raw = Encoding.UTF8.GetBytes(plain);
                    byte[] protectedBytes = ProtectedData.Protect(raw, AuthEntropy, DataProtectionScope.CurrentUser);
                    State.activeconfig.OpenKneeboard_EfbAuthBlob = Convert.ToBase64String(protectedBytes);
                }

                public static void ClearEncryptedAuthBlob()
                {
                    State.activeconfig.OpenKneeboard_EfbAuthBlob = "";
                }

                public static void SetMockTokenForTesting(bool expired)
                {
                    DateTime obtainedUtc = expired ? DateTime.UtcNow.AddHours(-2) : DateTime.UtcNow;
                    int expiresIn = expired ? 30 : 3600;

                    JObject payload = new JObject
                    {
                        ["provider"] = "navigraph",
                        ["token_type"] = "Bearer",
                        ["access_token"] = "MOCK_ACCESS_" + Guid.NewGuid().ToString("N"),
                        ["refresh_token"] = "MOCK_REFRESH_" + Guid.NewGuid().ToString("N"),
                        ["scope"] = "openid charts offline_access",
                        ["expires_in"] = expiresIn,
                        ["obtained_utc"] = obtainedUtc.ToString("o"),
                        ["mock"] = true,
                        ["mock_expired"] = expired,
                    };

                    SetMockAuthPayloadForTesting(payload.ToString(Newtonsoft.Json.Formatting.None));
                }

                public static void SetMockAuthPayloadForTesting(string plainAuthPayload)
                {
                    string plain = plainAuthPayload ?? "";
                    if (string.IsNullOrWhiteSpace(plain))
                    {
                        State.activeconfig.OpenKneeboard_EfbAuthBlob = "";
                        return;
                    }

                    string encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(plain));
                    State.activeconfig.OpenKneeboard_EfbAuthBlob = MockPayloadPrefix + encoded;
                }

                public static bool ExpireCurrentTokenForTesting()
                {
                    string authPayload;
                    if (!TryGetDecryptedAuthBlob(out authPayload))
                    {
                        return false;
                    }

                    try
                    {
                        JObject payload = JObject.Parse(authPayload);
                        payload["expires_in"] = 30;
                        payload["obtained_utc"] = DateTime.UtcNow.AddHours(-3).ToString("o");
                        payload["mock"] = true;
                        payload["mock_expired"] = true;
                        SetEncryptedAuthBlob(payload.ToString(Newtonsoft.Json.Formatting.None));
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }

                public static bool HasStoredAuth()
                {
                    string payload;
                    return TryGetDecryptedAuthBlob(out payload);
                }

                public static void LogDecryptedAuthForDiagnostics()
                {
                    string payload;
                    if (!TryGetDecryptedAuthBlob(out payload))
                    {
                        Log.Write("OKB EFB decrypted auth: <none>", Colors.Warning);
                        return;
                    }

                    Log.Write("OKB EFB decrypted auth: " + payload, Colors.Warning);
                }

                public static bool TryGetDecryptedAuthBlob(out string plainAuthPayload)
                {
                    plainAuthPayload = "";
                    string encrypted = State.activeconfig.OpenKneeboard_EfbAuthBlob;
                    if (string.IsNullOrWhiteSpace(encrypted))
                    {
                        return false;
                    }

                    if (encrypted.StartsWith(MockPayloadPrefix, StringComparison.Ordinal))
                    {
                        try
                        {
                            string encoded = encrypted.Substring(MockPayloadPrefix.Length);
                            byte[] rawMock = Convert.FromBase64String(encoded);
                            plainAuthPayload = Encoding.UTF8.GetString(rawMock);
                            return !string.IsNullOrWhiteSpace(plainAuthPayload);
                        }
                        catch
                        {
                            return false;
                        }
                    }

                    try
                    {
                        byte[] data = Convert.FromBase64String(encrypted);
                        byte[] raw = ProtectedData.Unprotect(data, AuthEntropy, DataProtectionScope.CurrentUser);
                        plainAuthPayload = Encoding.UTF8.GetString(raw);
                        return !string.IsNullOrWhiteSpace(plainAuthPayload);
                    }
                    catch
                    {
                        return false;
                    }
                }

                public static OpenKneeboardEfbSnapshot BuildSnapshot(OpenKneeboardServerSnapshot server)
                {
                    OpenKneeboardEfbSnapshot model = new OpenKneeboardEfbSnapshot();

                    try
                    {
                        bool featureEnabled = State.activeconfig.OpenKneeboard_EfbEnabled;
                        bool hasAuthBlob = !string.IsNullOrWhiteSpace(State.activeconfig.OpenKneeboard_EfbAuthBlob);
                        string authPayload;
                        bool hasStoredAuth = TryGetDecryptedAuthBlob(out authPayload);
                        TokenMetadata token = ParseTokenMetadata(authPayload);
                        bool devBypass = State.activeconfig.OpenKneeboard_EfbDevBypass;
                        bool liveDcsSession = IsLiveDcsSession(server);

                        model.FeatureEnabled = featureEnabled;
                        model.HasAuthBlob = hasAuthBlob;
                        model.AuthReadable = hasStoredAuth;
                        model.AuthPresent = hasStoredAuth;
                        model.DevBypass = devBypass;
                        model.LiveSession = liveDcsSession;
                        model.ShowTab = featureEnabled && (hasStoredAuth || hasAuthBlob);
                        model.TokenExpiresUtc = token.ExpiresUtc;
                        model.TokenObtainedUtc = token.ObtainedUtc;
                        model.TokenExpired = token.IsExpired;
                        model.HasRefreshToken = token.HasRefreshToken;
                        model.IsMockToken = token.IsMock;

                        if (!featureEnabled || !hasStoredAuth)
                        {
                            if (!featureEnabled)
                            {
                                model.State = "Signed out";
                                model.Message = "Sign in to Navigraph in VAICOM to enable the EFB tab.";
                            }
                            else if (hasAuthBlob)
                            {
                                model.State = "Error";
                                model.Message = "Stored EFB auth was found but could not be decrypted in this session.";
                            }
                            else
                            {
                                model.State = "Signed out";
                                model.Message = "Sign in to Navigraph in VAICOM to enable the EFB tab.";
                            }
                        }
                        else if (!liveDcsSession && !devBypass)
                        {
                            model.State = "Ready/no live DCS";
                            model.Message = "EFB tab is ready. Connect to a live DCS mission to render Navigraph data.";
                        }
                        else if (token.HasAccessToken || string.Equals(authPayload, "TEST_AUTH_PRESENT", StringComparison.Ordinal))
                        {
                            if (token.IsExpired)
                            {
                                model.State = "Signed out";
                                model.Message = "Stored EFB token is expired. Reconnect Navigraph.";
                            }
                            else
                            {
                                model.State = "EFB loaded";
                                model.Message = devBypass
                                    ? "EFB loaded using temporary development bypass."
                                    : "EFB loaded for active simulator session.";
                            }
                        }
                        else
                        {
                            model.State = "Loading";
                            model.Message = "Navigraph auth is available. EFB API wiring is in progress for this staged spike.";
                        }
                    }
                    catch (Exception ex)
                    {
                        model.State = "Error";
                        model.Message = "EFB state error: " + ex.Message;
                    }

                    model.UpdatedUtc = DateTime.UtcNow;
                    return model;
                }

                private static bool HasUsableTokenPayload(string authPayload)
                {
                    return ParseTokenMetadata(authPayload).HasAccessToken;
                }

                private static TokenMetadata ParseTokenMetadata(string authPayload)
                {
                    TokenMetadata meta = new TokenMetadata();
                    if (string.IsNullOrWhiteSpace(authPayload))
                    {
                        return meta;
                    }

                    try
                    {
                        JObject token = JObject.Parse(authPayload);
                        meta.HasAccessToken = !string.IsNullOrWhiteSpace((string)token["access_token"]);
                        meta.HasRefreshToken = !string.IsNullOrWhiteSpace((string)token["refresh_token"]);
                        meta.IsMock = string.Equals((string)token["mock"], "true", StringComparison.OrdinalIgnoreCase)
                            || (bool?)token["mock"] == true;
                        bool mockExpired = string.Equals((string)token["mock_expired"], "true", StringComparison.OrdinalIgnoreCase)
                            || (bool?)token["mock_expired"] == true;

                        DateTimeOffset obtainedOffset;
                        if (DateTimeOffset.TryParse(
                            (string)token["obtained_utc"],
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                            out obtainedOffset))
                        {
                            meta.ObtainedUtc = obtainedOffset.UtcDateTime;
                        }

                        int expiresIn = (int?)token["expires_in"] ?? 0;
                        if (meta.ObtainedUtc.HasValue && expiresIn > 0)
                        {
                            meta.ExpiresUtc = meta.ObtainedUtc.Value.AddSeconds(expiresIn);
                            meta.IsExpired = DateTime.UtcNow >= meta.ExpiresUtc.Value;
                        }

                        if (mockExpired)
                        {
                            meta.IsExpired = true;
                        }
                    }
                    catch
                    {
                    }

                    return meta;
                }

                private static bool IsLiveDcsSession(OpenKneeboardServerSnapshot server)
                {
                    if (server == null)
                    {
                        return false;
                    }

                    return !string.IsNullOrWhiteSpace(server.Aircraft)
                        || !string.IsNullOrWhiteSpace(server.MissionTitle)
                        || !string.IsNullOrWhiteSpace(server.Theater);
                }

                public class OpenKneeboardEfbSnapshot
                {
                    public bool FeatureEnabled { get; set; }
                    public bool HasAuthBlob { get; set; }
                    public bool AuthReadable { get; set; }
                    public bool ShowTab { get; set; }
                    public bool AuthPresent { get; set; }
                    public bool DevBypass { get; set; }
                    public bool LiveSession { get; set; }
                    public string State { get; set; } = "Signed out";
                    public string Message { get; set; } = "Sign in to Navigraph in VAICOM to enable the EFB tab.";
                    public string SelectedAirport { get; set; } = "";
                    public DateTime? TokenObtainedUtc { get; set; }
                    public DateTime? TokenExpiresUtc { get; set; }
                    public bool TokenExpired { get; set; }
                    public bool HasRefreshToken { get; set; }
                    public bool IsMockToken { get; set; }
                    public DateTime UpdatedUtc { get; set; } = DateTime.UtcNow;

                    public OpenKneeboardEfbSnapshot Clone()
                    {
                        return new OpenKneeboardEfbSnapshot
                        {
                            FeatureEnabled = FeatureEnabled,
                            HasAuthBlob = HasAuthBlob,
                            AuthReadable = AuthReadable,
                            ShowTab = ShowTab,
                            AuthPresent = AuthPresent,
                            DevBypass = DevBypass,
                            LiveSession = LiveSession,
                            State = State,
                            Message = Message,
                            SelectedAirport = SelectedAirport,
                            TokenObtainedUtc = TokenObtainedUtc,
                            TokenExpiresUtc = TokenExpiresUtc,
                            TokenExpired = TokenExpired,
                            HasRefreshToken = HasRefreshToken,
                            IsMockToken = IsMockToken,
                            UpdatedUtc = UpdatedUtc,
                        };
                    }
                }

                private class TokenMetadata
                {
                    public bool HasAccessToken { get; set; }
                    public bool HasRefreshToken { get; set; }
                    public bool IsMock { get; set; }
                    public bool IsExpired { get; set; }
                    public DateTime? ObtainedUtc { get; set; }
                    public DateTime? ExpiresUtc { get; set; }
                }
            }
        }
    }
}
