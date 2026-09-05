using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace VAICOM
{
    namespace Extensions
    {
        namespace Kneeboard
        {
            public static class OpenKneeboardNavigraphOAuthService
            {
                private const string DeviceCodeGrantType = "urn:ietf:params:oauth:grant-type:device_code";
                private static readonly object MockRefreshSync = new object();
                private static Timer mockRefreshTimer;
                private static string mockRefreshMode = "success";

                public static void SetMockRefreshMode(string mode)
                {
                    lock (MockRefreshSync)
                    {
                        string normalized = string.IsNullOrWhiteSpace(mode) ? "success" : mode.Trim().ToLowerInvariant();
                        if (normalized != "success" && normalized != "fail" && normalized != "expired")
                        {
                            normalized = "success";
                        }
                        mockRefreshMode = normalized;
                    }
                }

                // Diagnostic probe: run full device flow, show raw device and token JSON responses
                public static async Task<ConnectResult> ProbeDeviceFlowAndShowResponsesAsync(CancellationToken cancellationToken)
                {
                    ConnectResult result = new ConnectResult();

                    try
                    {
                        OAuthConfig config = BuildConfig();
                        if (!config.IsValid)
                        {
                            result.Success = false;
                            result.Message = config.ValidationError;
                            return result;
                        }

                        // Device authorization
                        DeviceAuthorizationResponse device = await RequestDeviceAuthorizationAsync(config, cancellationToken).ConfigureAwait(false);
                        if (device == null || string.IsNullOrWhiteSpace(device.DeviceCode))
                        {
                            result.Success = false;
                            result.Message = "Device authorization response was invalid.";
                            return result;
                        }

                        string launchUrl = !string.IsNullOrWhiteSpace(device.VerificationUriComplete) ? device.VerificationUriComplete : device.VerificationUri;
                        result.VerificationUrl = launchUrl;
                        result.UserCode = device.UserCode;

                        // Show the verification URL and user code so user can approve
                        try
                        {
                            System.Windows.MessageBox.Show("Navigate to: " + (launchUrl ?? "(no url)") + "\nand enter code: " + (device.UserCode ?? ""), "Navigraph device authorization", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                        }
                        catch { }

                        // Poll token endpoint until success or terminal error, capturing raw JSON responses
                        DateTime expiryUtc = DateTime.UtcNow.AddSeconds(Math.Max(60, device.ExpiresIn));
                        int intervalSeconds = Math.Max(3, device.Interval);
                        string lastResponseJson = "";

                        while (DateTime.UtcNow < expiryUtc)
                        {
                            cancellationToken.ThrowIfCancellationRequested();

                            List<KeyValuePair<string, string>> payload = new List<KeyValuePair<string, string>>
                            {
                                new KeyValuePair<string, string>("grant_type", DeviceCodeGrantType),
                                new KeyValuePair<string, string>("device_code", device.DeviceCode),
                                new KeyValuePair<string, string>("client_id", config.ClientId),
                            new KeyValuePair<string, string>("scope", config.Scope),
                            };
                            if (!string.IsNullOrWhiteSpace(config.ClientSecret))
                            {
                                payload.Add(new KeyValuePair<string, string>("client_secret", config.ClientSecret));
                            }
                            if (!string.IsNullOrWhiteSpace(device.CodeVerifier))
                            {
                                payload.Add(new KeyValuePair<string, string>("code_verifier", device.CodeVerifier));
                            }

                            using (HttpClient client = new HttpClient())
                            using (FormUrlEncodedContent content = new FormUrlEncodedContent(payload))
                            {
                                HttpResponseMessage response = await client.PostAsync((config.TokenEndpoint ?? ""), content, cancellationToken).ConfigureAwait(false);
                                string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                                lastResponseJson = json ?? "";

                                // Log it
                                // try { Log.Write("[Probe] Navigraph token endpoint response: " + lastResponseJson, Static.Colors.Debug); } catch { }

                                if (response.IsSuccessStatusCode)
                                {
                                    JObject tokenObj = JObject.Parse(json);
                                    var token = new TokenResponse
                                    {
                                        AccessToken = (string)tokenObj["access_token"] ?? "",
                                        RefreshToken = (string)tokenObj["refresh_token"] ?? "",
                                        TokenType = (string)tokenObj["token_type"] ?? "",
                                        Scope = (string)tokenObj["scope"] ?? "",
                                        ExpiresIn = (int?)tokenObj["expires_in"] ?? 0,
                                    };

                                    // Save auth blob like normal
                                    JObject payloadObj = new JObject
                                    {
                                        ["provider"] = "navigraph",
                                        ["token_type"] = token.TokenType ?? "",
                                        ["access_token"] = token.AccessToken ?? "",
                                        ["refresh_token"] = token.RefreshToken ?? "",
                                        ["scope"] = token.Scope ?? "",
                                        ["expires_in"] = token.ExpiresIn,
                                        ["obtained_utc"] = DateTime.UtcNow.ToString("o"),
                                    };

                                    OpenKneeboardNavigraphEfbState.SetEncryptedAuthBlob(payloadObj.ToString(Newtonsoft.Json.Formatting.None));

                                    result.Success = true;
                                    result.Message = "Navigraph authentication completed (probe).";

                                    try
                                    {
                                        System.Windows.MessageBox.Show("Token response:\n" + lastResponseJson, "Navigraph token response", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                                    }
                                    catch { }

                                    return result;
                                }

                                JObject errorObj = null;
                                try { errorObj = JObject.Parse(json); } catch { }
                                string error = errorObj == null ? "" : ((string)errorObj["error"] ?? "");

                                if (string.Equals(error, "authorization_pending", StringComparison.OrdinalIgnoreCase))
                                {
                                    await Task.Delay(intervalSeconds * 1000, cancellationToken).ConfigureAwait(false);
                                    continue;
                                }

                                if (string.Equals(error, "slow_down", StringComparison.OrdinalIgnoreCase))
                                {
                                    intervalSeconds = Math.Min(intervalSeconds + 5, 30);
                                    await Task.Delay(intervalSeconds * 1000, cancellationToken).ConfigureAwait(false);
                                    continue;
                                }

                                // Terminal errors: show raw JSON and return failure
                                try { System.Windows.MessageBox.Show("Token endpoint returned error:\n" + lastResponseJson, "Navigraph token error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error); } catch { }
                                result.Success = false;
                                result.Message = "Device authorization failed: " + lastResponseJson;
                                return result;
                            }
                        }

                        // timed out
                        try { System.Windows.MessageBox.Show("Timed out waiting for Navigraph authentication.", "Navigraph timeout", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning); } catch { }
                        result.Success = false;
                        result.Message = "Timed out waiting for Navigraph authentication.";
                        return result;
                    }
                    catch (Exception ex)
                    {
                        result.Success = false;
                        result.Message = "Navigraph probe failed: " + ex.Message;
                        return result;
                    }
                }

                public static void StartMockRefreshScheduler(int intervalSeconds)
                {
                    int periodMs = Math.Max(5, intervalSeconds) * 1000;

                    lock (MockRefreshSync)
                    {
                        StopMockRefreshScheduler();
                        mockRefreshTimer = new Timer(_ =>
                        {
                            try
                            {
                                RunMockRefreshOnce();
                                OpenKneeboardBridge.UpdateServerData();
                            }
                            catch
                            {
                            }
                        }, null, periodMs, periodMs);
                    }
                }

                public static void StopMockRefreshScheduler()
                {
                    lock (MockRefreshSync)
                    {
                        if (mockRefreshTimer != null)
                        {
                            mockRefreshTimer.Dispose();
                            mockRefreshTimer = null;
                        }
                    }
                }

                public static ConnectResult RunMockRefreshOnce()
                {
                    ConnectResult result = new ConnectResult();

                    try
                    {
                        string authPayload;
                        if (!OpenKneeboardNavigraphEfbState.TryGetDecryptedAuthBlob(out authPayload) || string.IsNullOrWhiteSpace(authPayload))
                        {
                            result.Success = false;
                            result.Message = "No EFB auth payload available for refresh simulation.";
                            return result;
                        }

                        JObject payload = JObject.Parse(authPayload);
                        string mode;
                        lock (MockRefreshSync)
                        {
                            mode = mockRefreshMode;
                        }

                        if (mode == "fail")
                        {
                            result.Success = false;
                            result.Message = "Mock refresh failed by test mode.";
                            return result;
                        }

                        if (mode == "expired")
                        {
                            payload["expires_in"] = 30;
                            payload["obtained_utc"] = DateTime.UtcNow.AddHours(-3).ToString("o");
                            payload["mock"] = true;
                            payload["mock_expired"] = true;
                            OpenKneeboardNavigraphEfbState.SetMockAuthPayloadForTesting(payload.ToString(Newtonsoft.Json.Formatting.None));
                            result.Success = false;
                            result.Message = "Mock refresh set token to expired.";
                            return result;
                        }

                        payload["access_token"] = "MOCK_REFRESHED_" + Guid.NewGuid().ToString("N");
                        if (payload["refresh_token"] == null || string.IsNullOrWhiteSpace((string)payload["refresh_token"]))
                        {
                            payload["refresh_token"] = "MOCK_REFRESH_" + Guid.NewGuid().ToString("N");
                        }
                        payload["expires_in"] = 3600;
                        payload["obtained_utc"] = DateTime.UtcNow.ToString("o");
                        payload["mock"] = true;
                        payload["mock_expired"] = false;

                        OpenKneeboardNavigraphEfbState.SetMockAuthPayloadForTesting(payload.ToString(Newtonsoft.Json.Formatting.None));
                        result.Success = true;
                        result.Message = "Mock refresh succeeded.";
                        return result;
                    }
                    catch (Exception ex)
                    {
                        result.Success = false;
                        result.Message = "Mock refresh error: " + ex.Message;
                        return result;
                    }
                }

                public static bool TryRefreshAccessToken(out string message)
                {
                    message = "";

                    try
                    {
                        string authPayload;
                        if (!OpenKneeboardNavigraphEfbState.TryGetDecryptedAuthBlob(out authPayload) || string.IsNullOrWhiteSpace(authPayload))
                        {
                            message = "No auth payload available.";
                            return false;
                        }

                        JObject existing = JObject.Parse(authPayload);
                        string refreshToken = (string)existing["refresh_token"] ?? "";
                        if (string.IsNullOrWhiteSpace(refreshToken))
                        {
                            message = "Missing refresh token.";
                            return false;
                        }

                        OAuthConfig config = BuildConfig();
                        if (string.IsNullOrWhiteSpace(config.ClientId) || string.IsNullOrWhiteSpace(config.TokenEndpoint))
                        {
                            message = "Missing OAuth client id or token endpoint.";
                            return false;
                        }

                        List<KeyValuePair<string, string>> payload = new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>("grant_type", "refresh_token"),
                            new KeyValuePair<string, string>("client_id", config.ClientId),
                            new KeyValuePair<string, string>("refresh_token", refreshToken),
                        };
                        if (!string.IsNullOrWhiteSpace(config.Scope))
                        {
                            payload.Add(new KeyValuePair<string, string>("scope", config.Scope));
                        }
                        if (!string.IsNullOrWhiteSpace(config.ClientSecret))
                        {
                            payload.Add(new KeyValuePair<string, string>("client_secret", config.ClientSecret));
                        }

                        using (HttpClient client = new HttpClient())
                        using (FormUrlEncodedContent content = new FormUrlEncodedContent(payload))
                        {
                            // try
                            // {
                            //     var logPayload = new List<KeyValuePair<string, string>>(payload);
                            //     for (int i = 0; i < logPayload.Count; i++)
                            //     {
                            //         if (string.Equals(logPayload[i].Key, "client_secret", StringComparison.OrdinalIgnoreCase))
                            //         {
                            //             logPayload[i] = new KeyValuePair<string, string>(logPayload[i].Key, "<redacted>");
                            //         }
                            //     }
                            //     JObject payloadObj = new JObject();
                            //     foreach (var kv in logPayload) payloadObj[kv.Key] = kv.Value;
                            //     Log.Write("Navigraph refresh request payload: " + payloadObj.ToString(Newtonsoft.Json.Formatting.None), Static.Colors.Debug);
                            // }
                            // catch { }

                            HttpResponseMessage response = client.PostAsync(config.TokenEndpoint, content).GetAwaiter().GetResult();
                            string json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                            // try { Log.Write("Navigraph refresh response: " + json, Static.Colors.Debug); } catch { }

                            if (!response.IsSuccessStatusCode)
                            {
                                message = "Refresh failed: " + json;
                                return false;
                            }

                            JObject tokenObj = JObject.Parse(json);
                            string newAccess = (string)tokenObj["access_token"] ?? "";
                            if (string.IsNullOrWhiteSpace(newAccess))
                            {
                                message = "Refresh response missing access token.";
                                return false;
                            }

                            JObject updated = new JObject
                            {
                                ["provider"] = (string)existing["provider"] ?? "navigraph",
                                ["token_type"] = (string)tokenObj["token_type"] ?? (string)existing["token_type"] ?? "Bearer",
                                ["access_token"] = newAccess,
                                ["refresh_token"] = (string)tokenObj["refresh_token"] ?? refreshToken,
                                ["scope"] = (string)tokenObj["scope"] ?? (string)existing["scope"] ?? "",
                                ["expires_in"] = (int?)tokenObj["expires_in"] ?? (int?)existing["expires_in"] ?? 3600,
                                ["obtained_utc"] = DateTime.UtcNow.ToString("o"),
                            };

                            ApplyTileCookiesFromResponse(updated, response);

                            OpenKneeboardNavigraphEfbState.SetEncryptedAuthBlob(updated.ToString(Newtonsoft.Json.Formatting.None));
                            message = "Token refreshed.";
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        message = "Refresh exception: " + ex.Message;
                        return false;
                    }
                }

                public static async Task<ConnectResult> ConnectWithDeviceFlowAsync(CancellationToken cancellationToken)
                {
                    ConnectResult result = new ConnectResult();

                    try
                    {
                        try { Log.Write("Navigraph auth: loading.", VAICOM.Static.Colors.Text); } catch { }

                        OAuthConfig config = BuildConfig();
                        if (!config.IsValid)
                        {
                            result.Success = false;
                            result.Message = config.ValidationError;
                            return result;
                        }

                        DeviceAuthorizationResponse device = await RequestDeviceAuthorizationAsync(config, cancellationToken).ConfigureAwait(false);
                        if (device == null || string.IsNullOrWhiteSpace(device.DeviceCode))
                        {
                            result.Success = false;
                            result.Message = "Device authorization response was invalid.";
                            return result;
                        }

                        string launchUrl = !string.IsNullOrWhiteSpace(device.VerificationUriComplete)
                            ? device.VerificationUriComplete
                            : device.VerificationUri;
                        result.VerificationUrl = launchUrl;
                        result.UserCode = device.UserCode;

                        try
                        {
                            Log.Write("Navigraph auth: waiting for authorization.", VAICOM.Static.Colors.Text);
                        }
                        catch
                        {
                        }

                        if (!string.IsNullOrWhiteSpace(launchUrl))
                        {
                            TryOpenBrowser(launchUrl);
                        }

                        TokenResponse token = await PollForTokenAsync(config, device, cancellationToken).ConfigureAwait(false);
                        if (token == null || string.IsNullOrWhiteSpace(token.AccessToken))
                        {
                            result.Success = false;
                            result.Message = "No access token returned from Navigraph.";
                            try { Log.Write("Navigraph auth failed: no access token returned.", VAICOM.Static.Colors.Warning); } catch { }
                            return result;
                        }

                        JObject payload = new JObject
                        {
                            ["provider"] = "navigraph",
                            ["token_type"] = token.TokenType ?? "",
                            ["access_token"] = token.AccessToken ?? "",
                            ["refresh_token"] = token.RefreshToken ?? "",
                            ["scope"] = token.Scope ?? "",
                            ["expires_in"] = token.ExpiresIn,
                            ["obtained_utc"] = DateTime.UtcNow.ToString("o"),
                        };

                        if (token.RawSetCookies != null && token.RawSetCookies.Count > 0)
                        {
                            ApplyTileCookiesFromSetCookieValues(payload, token.RawSetCookies);
                        }

                        OpenKneeboardNavigraphEfbState.SetEncryptedAuthBlob(payload.ToString(Newtonsoft.Json.Formatting.None));

                        result.Success = true;
                        result.Message = "Navigraph authentication completed.";
                        try { Log.Write("Navigraph auth: received.", VAICOM.Static.Colors.Text); } catch { }
                        return result;
                    }
                    catch (OperationCanceledException)
                    {
                        result.Success = false;
                        result.Message = "Navigraph authentication canceled.";
                        try { Log.Write("Navigraph auth canceled.", VAICOM.Static.Colors.Warning); } catch { }
                        return result;
                    }
                    catch (Exception ex)
                    {
                        // Provide a clearer message when the identity server rejects the requested scopes.
                        try
                        {
                            string msg = ex.Message ?? "";
                            if (msg.IndexOf("invalid_scope", StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                result.Success = false;
                                result.Message = "Navigraph authentication failed: requested scope was rejected by the identity server (invalid_scope).\n" +
                                    "Ensure the client id is authorized to request the 'charts' scope and that the user account has access to Navigraph Charts. See https://developers.navigraph.com/docs/authentication/overview";
                                try { Log.Write("Navigraph auth failed: invalid scope.", VAICOM.Static.Colors.Warning); } catch { }
                                return result;
                            }
                        }
                        catch { }

                        result.Success = false;
                        result.Message = "Navigraph authentication failed: " + ex.Message;
                        try { Log.Write("Navigraph auth failed: " + ex.Message, VAICOM.Static.Colors.Warning); } catch { }
                        return result;
                    }
                }

                private static OAuthConfig BuildConfig()
                {
                    OAuthConfig config = new OAuthConfig
                    {
                        ClientId = (State.activeconfig.OpenKneeboard_EfbOAuthClientId ?? "").Trim(),
                        ClientSecret = (State.activeconfig.OpenKneeboard_EfbOAuthClientSecret ?? "").Trim(),
                        Scope = (State.activeconfig.OpenKneeboard_EfbOAuthScope ?? "").Trim(),
                        DeviceAuthorizationEndpoint = (State.activeconfig.OpenKneeboard_EfbOAuthDeviceAuthEndpoint ?? "").Trim(),
                        TokenEndpoint = (State.activeconfig.OpenKneeboard_EfbOAuthTokenEndpoint ?? "").Trim(),
                    };

                    // If client id/secret not provided in config, try secure stored credentials
                    if (string.IsNullOrWhiteSpace(config.ClientId) || string.IsNullOrWhiteSpace(config.ClientSecret))
                    {
                        try
                        {
                            string credUser, credPass, credClientType;
                            if (OpenKneeboardNavigraphCredentials.TryGetCredentials(out credUser, out credPass, out credClientType))
                            {
                                if (string.IsNullOrWhiteSpace(config.ClientId) && !string.IsNullOrWhiteSpace(credUser)) config.ClientId = credUser.Trim();
                                if (string.IsNullOrWhiteSpace(config.ClientSecret) && !string.IsNullOrWhiteSpace(credPass)) config.ClientSecret = credPass.Trim();
                            }
                        }
                        catch
                        {
                            // ignore and continue with whatever config has
                        }
                    }

                    if (string.IsNullOrWhiteSpace(config.ClientId))
                    {
                        config.ValidationError = "Missing Navigraph OAuth client id in OKB Out settings.";
                    }
                    else if (string.IsNullOrWhiteSpace(config.DeviceAuthorizationEndpoint))
                    {
                        config.ValidationError = "Missing Navigraph device authorization endpoint in OKB Out settings.";
                    }
                    else if (string.IsNullOrWhiteSpace(config.TokenEndpoint))
                    {
                        config.ValidationError = "Missing Navigraph token endpoint in OKB Out settings.";
                    }

                    config.IsValid = string.IsNullOrWhiteSpace(config.ValidationError);
                    if (string.IsNullOrWhiteSpace(config.Scope))
                    {
                        config.Scope = "openid charts tiles offline_access";
                    }

                    return config;
                }

                private static async Task<DeviceAuthorizationResponse> RequestDeviceAuthorizationAsync(OAuthConfig config, CancellationToken cancellationToken)
                {
                    // Generate PKCE code verifier/challenge and include in the device authorization
                    string codeVerifier = GenerateCodeVerifier();
                    string codeChallenge = ComputeCodeChallenge(codeVerifier);

                    List<KeyValuePair<string, string>> payload = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("client_id", config.ClientId),
                        new KeyValuePair<string, string>("scope", config.Scope),
                        new KeyValuePair<string, string>("code_challenge", codeChallenge),
                        new KeyValuePair<string, string>("code_challenge_method", "S256"),
                    };
                    if (!string.IsNullOrWhiteSpace(config.ClientSecret))
                    {
                        payload.Add(new KeyValuePair<string, string>("client_secret", config.ClientSecret));
                    }

                    using (HttpClient client = new HttpClient())
                    using (FormUrlEncodedContent content = new FormUrlEncodedContent(payload))
                    {
                        // Log outgoing payload (mask client_secret)
                        // try
                        // {
                        //     var logPayload = new List<KeyValuePair<string, string>>(payload);
                        //     for (int i = 0; i < logPayload.Count; i++)
                        //     {
                        //         if (string.Equals(logPayload[i].Key, "client_secret", StringComparison.OrdinalIgnoreCase))
                        //         {
                        //             logPayload[i] = new KeyValuePair<string, string>(logPayload[i].Key, "<redacted>");
                        //         }
                        //     }
                        //     JObject payloadObj = new JObject();
                        //     foreach (var kv in logPayload) payloadObj[kv.Key] = kv.Value;
                        //     Log.Write("Navigraph deviceauthorization request payload: " + payloadObj.ToString(Newtonsoft.Json.Formatting.None), Static.Colors.Debug);
                        // }
                        // catch { }

                        HttpResponseMessage response = await client.PostAsync(config.DeviceAuthorizationEndpoint, content, cancellationToken).ConfigureAwait(false);
                        string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                        // Log full response JSON for diagnostics
                        // try { Log.Write("Navigraph deviceauthorization response: " + json, Static.Colors.Debug); } catch { }

                        if (!response.IsSuccessStatusCode)
                        {
                            try
                            {
                                System.Windows.MessageBox.Show("Device authorization failed:\n" + json, "Navigraph device authorization error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                            }
                            catch { }
                            throw new InvalidOperationException("Device authorization failed: " + json);
                        }

                        JObject obj = JObject.Parse(json);
                        var devResp = new DeviceAuthorizationResponse
                        {
                            DeviceCode = (string)obj["device_code"] ?? "",
                            UserCode = (string)obj["user_code"] ?? "",
                            VerificationUri = (string)obj["verification_uri"] ?? "",
                            VerificationUriComplete = (string)obj["verification_uri_complete"] ?? "",
                            ExpiresIn = (int?)obj["expires_in"] ?? 600,
                            Interval = (int?)obj["interval"] ?? 5,
                        };

                        // Attach the code verifier so the token polling can use it
                        devResp.CodeVerifier = codeVerifier;
                        return devResp;
                    }
                }

                private static async Task<TokenResponse> PollForTokenAsync(OAuthConfig config, DeviceAuthorizationResponse device, CancellationToken cancellationToken)
                {
                    DateTime expiryUtc = DateTime.UtcNow.AddSeconds(Math.Max(60, device.ExpiresIn));
                    int intervalSeconds = Math.Max(3, device.Interval);

                    while (DateTime.UtcNow < expiryUtc)
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        List<KeyValuePair<string, string>> payload = new List<KeyValuePair<string, string>>
                        {
                            new KeyValuePair<string, string>("grant_type", DeviceCodeGrantType),
                            new KeyValuePair<string, string>("device_code", device.DeviceCode),
                            new KeyValuePair<string, string>("client_id", config.ClientId),
                            new KeyValuePair<string, string>("scope", config.Scope),
                        };
                        if (!string.IsNullOrWhiteSpace(config.ClientSecret))
                        {
                            payload.Add(new KeyValuePair<string, string>("client_secret", config.ClientSecret));
                        }

                        // If a PKCE code verifier was generated earlier, include it in the
                        // token request so providers requiring PKCE for device flow succeed.
                        if (!string.IsNullOrWhiteSpace(device.CodeVerifier))
                        {
                            payload.Add(new KeyValuePair<string, string>("code_verifier", device.CodeVerifier));
                        }

                        using (HttpClient client = new HttpClient())
                        using (FormUrlEncodedContent content = new FormUrlEncodedContent(payload))
                        {
                            // Log outgoing token request payload (mask client_secret)
                            // try
                            // {
                            //     var logPayload = new List<KeyValuePair<string, string>>(payload);
                            //     for (int i = 0; i < logPayload.Count; i++)
                            //     {
                            //         if (string.Equals(logPayload[i].Key, "client_secret", StringComparison.OrdinalIgnoreCase))
                            //         {
                            //             logPayload[i] = new KeyValuePair<string, string>(logPayload[i].Key, "<redacted>");
                            //         }
                            //     }
                            //     JObject payloadObj = new JObject();
                            //     foreach (var kv in logPayload) payloadObj[kv.Key] = kv.Value;
                            //     Log.Write("Navigraph token request payload: " + payloadObj.ToString(Newtonsoft.Json.Formatting.None), Static.Colors.Debug);
                            // }
                            // catch { }

                            HttpResponseMessage response = await client.PostAsync(config.TokenEndpoint, content, cancellationToken).ConfigureAwait(false);
                            string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                            // Log full response JSON for diagnostics
                            // try { Log.Write("Navigraph token endpoint response: " + json, Static.Colors.Debug); } catch { }

                            if (response.IsSuccessStatusCode)
                            {
                                JObject tokenObj = JObject.Parse(json);
                                return new TokenResponse
                                {
                                    AccessToken = (string)tokenObj["access_token"] ?? "",
                                    RefreshToken = (string)tokenObj["refresh_token"] ?? "",
                                    TokenType = (string)tokenObj["token_type"] ?? "",
                                    Scope = (string)tokenObj["scope"] ?? "",
                                    ExpiresIn = (int?)tokenObj["expires_in"] ?? 0,
                                    RawSetCookies = ReadSetCookieHeaders(response),
                                };
                            }

                            JObject errorObj = null;
                            try
                            {
                                errorObj = JObject.Parse(json);
                            }
                            catch
                            {
                            }

                            string error = errorObj == null ? "" : ((string)errorObj["error"] ?? "");
                            if (string.Equals(error, "authorization_pending", StringComparison.OrdinalIgnoreCase))
                            {
                                await Task.Delay(intervalSeconds * 1000, cancellationToken).ConfigureAwait(false);
                                continue;
                            }

                            if (string.Equals(error, "slow_down", StringComparison.OrdinalIgnoreCase))
                            {
                                intervalSeconds = Math.Min(intervalSeconds + 2, 15);
                                await Task.Delay(intervalSeconds * 1000, cancellationToken).ConfigureAwait(false);
                                continue;
                            }

                            if (string.Equals(error, "access_denied", StringComparison.OrdinalIgnoreCase))
                            {
                                throw new InvalidOperationException("Navigraph sign-in was denied by user.");
                            }

                            if (string.Equals(error, "expired_token", StringComparison.OrdinalIgnoreCase))
                            {
                                throw new InvalidOperationException("Device authorization expired before completion.");
                            }

                            throw new InvalidOperationException("Token polling failed: " + json);
                        }
                    }

                    throw new TimeoutException("Timed out waiting for Navigraph authentication.");
                }

                private static void ApplyTileCookiesFromResponse(JObject payload, HttpResponseMessage response)
                {
                    if (payload == null || response == null)
                    {
                        return;
                    }

                    ApplyTileCookiesFromSetCookieValues(payload, ReadSetCookieHeaders(response));
                }

                private static List<string> ReadSetCookieHeaders(HttpResponseMessage response)
                {
                    var values = new List<string>();
                    try
                    {
                        if (response == null || response.Headers == null)
                        {
                            return values;
                        }

                        IEnumerable<string> raw;
                        if (response.Headers.TryGetValues("Set-Cookie", out raw) && raw != null)
                        {
                            values.AddRange(raw);
                        }

                        // try
                        // {
                        //     Log.Write("Navigraph token Set-Cookie header count: " + values.Count, Static.Colors.Debug);
                        // }
                        // catch
                        // {
                        // }
                    }
                    catch
                    {
                    }

                    return values;
                }

                private static void ApplyTileCookiesFromSetCookieValues(JObject payload, IEnumerable<string> setCookieValues)
                {
                    try
                    {
                        if (payload == null || setCookieValues == null)
                        {
                            return;
                        }

                        string policy = null;
                        string signature = null;
                        string keyPairId = null;

                        foreach (var raw in setCookieValues)
                        {
                            string v = (raw ?? "").Trim();
                            if (string.IsNullOrWhiteSpace(v))
                            {
                                continue;
                            }

                            // try
                            // {
                            //     Log.Write("Navigraph Set-Cookie raw preview: " + BuildCookieLogPreview(v), Static.Colors.Debug);
                            // }
                            // catch
                            // {
                            // }

                            if (v.StartsWith("Cloudfront-Policy=", StringComparison.OrdinalIgnoreCase)
                                || v.StartsWith("CloudFront-Policy=", StringComparison.OrdinalIgnoreCase))
                            {
                                policy = ExtractCookieValue(v);
                            }
                            else if (v.StartsWith("CloudFront-Signature=", StringComparison.OrdinalIgnoreCase))
                            {
                                signature = ExtractCookieValue(v);
                            }
                            else if (v.StartsWith("CloudFront-Key-Pair-Id=", StringComparison.OrdinalIgnoreCase))
                            {
                                keyPairId = ExtractCookieValue(v);
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(policy)) payload["tile_cookie_policy"] = policy;
                        if (!string.IsNullOrWhiteSpace(signature)) payload["tile_cookie_signature"] = signature;
                        if (!string.IsNullOrWhiteSpace(keyPairId)) payload["tile_cookie_keypairid"] = keyPairId;
                        if (!string.IsNullOrWhiteSpace(policy) || !string.IsNullOrWhiteSpace(signature) || !string.IsNullOrWhiteSpace(keyPairId))
                        {
                            payload["tile_cookie_obtained_utc"] = DateTime.UtcNow.ToString("o");
                        }

                        // try
                        // {
                        //     Log.Write(
                        //         "Navigraph tile-cookie extraction result: policy=" + (!string.IsNullOrWhiteSpace(policy) ? "1" : "0")
                        //         + " sig=" + (!string.IsNullOrWhiteSpace(signature) ? "1" : "0")
                        //         + " keyPair=" + (!string.IsNullOrWhiteSpace(keyPairId) ? "1" : "0"),
                        //         Static.Colors.Debug);
                        // }
                        // catch
                        // {
                        // }
                    }
                    catch
                    {
                    }
                }

                private static string BuildCookieLogPreview(string rawCookie)
                {
                    try
                    {
                        string v = rawCookie ?? "";
                        int eq = v.IndexOf('=');
                        string name = eq > 0 ? v.Substring(0, eq).Trim() : "<unknown>";
                        string value = eq >= 0 ? v.Substring(eq + 1) : "";
                        int semi = value.IndexOf(';');
                        string token = semi >= 0 ? value.Substring(0, semi) : value;
                        string attrs = semi >= 0 ? value.Substring(semi + 1).Trim() : "";

                        string masked = token;
                        if (!string.IsNullOrEmpty(token) && token.Length > 14)
                        {
                            masked = token.Substring(0, 8) + "..." + token.Substring(token.Length - 4);
                        }

                        if (attrs.Length > 140)
                        {
                            attrs = attrs.Substring(0, 140) + "...";
                        }

                        return name + "=" + masked + (string.IsNullOrWhiteSpace(attrs) ? "" : "; " + attrs);
                    }
                    catch
                    {
                        return "<cookie-preview-error>";
                    }
                }

                private static string ExtractCookieValue(string rawSetCookie)
                {
                    try
                    {
                        string text = rawSetCookie ?? "";
                        int idx = text.IndexOf('=');
                        if (idx < 0)
                        {
                            return "";
                        }

                        string tail = text.Substring(idx + 1);
                        int semi = tail.IndexOf(';');
                        if (semi >= 0)
                        {
                            tail = tail.Substring(0, semi);
                        }

                        return tail.Trim();
                    }
                    catch
                    {
                        return "";
                    }
                }

                private static void TryOpenBrowser(string url)
                {
                    if (string.IsNullOrWhiteSpace(url))
                    {
                        return;
                    }

                    try
                    {
                        var psi = new ProcessStartInfo
                        {
                            FileName = url,
                            UseShellExecute = true,
                        };
                        Process.Start(psi);
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            // Log and show a fallback message box with the verification URL so
                            // the user can manually open it. This improves reliability across
                            // different Windows environments where Process.Start(url) may fail.
                            Log.Write("Failed to open browser automatically: " + ex.Message, VAICOM.Static.Colors.Warning);
                            System.Windows.MessageBox.Show("Open this URL in your browser to complete Navigraph login:\n\n" + url, "Navigraph Authentication", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                        }
                        catch
                        {
                            // swallow any secondary errors
                        }
                    }
                }

                private static string GenerateCodeVerifier()
                {
                    try
                    {
                        // 64 bytes -> base64url ~86 chars, safe length
                        byte[] bytes = new byte[64];
                        using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
                        {
                            rng.GetBytes(bytes);
                        }
                        string s = Convert.ToBase64String(bytes)
                            .TrimEnd('=')
                            .Replace('+', '-')
                            .Replace('/', '_');
                        return s;
                    }
                    catch
                    {
                        return Guid.NewGuid().ToString("N");
                    }
                }

                private static string ComputeCodeChallenge(string codeVerifier)
                {
                    try
                    {
                        using (var sha = System.Security.Cryptography.SHA256.Create())
                        {
                            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(codeVerifier);
                            byte[] hash = sha.ComputeHash(bytes);
                            string s = Convert.ToBase64String(hash)
                                .TrimEnd('=')
                                .Replace('+', '-')
                                .Replace('/', '_');
                            return s;
                        }
                    }
                    catch
                    {
                        return codeVerifier;
                    }
                }

                private class OAuthConfig
                {
                    public string ClientId { get; set; }
                    public string ClientSecret { get; set; }
                    public string Scope { get; set; }
                    public string DeviceAuthorizationEndpoint { get; set; }
                    public string TokenEndpoint { get; set; }
                    public string ValidationError { get; set; }
                    public bool IsValid { get; set; }
                }

                private class DeviceAuthorizationResponse
                {
                    public string DeviceCode { get; set; }
                    public string UserCode { get; set; }
                    public string VerificationUri { get; set; }
                    public string VerificationUriComplete { get; set; }
                    public int ExpiresIn { get; set; }
                    public int Interval { get; set; }
                    public string CodeVerifier { get; set; }
                }

                private class TokenResponse
                {
                    public string AccessToken { get; set; }
                    public string RefreshToken { get; set; }
                    public string TokenType { get; set; }
                    public string Scope { get; set; }
                    public int ExpiresIn { get; set; }
                    public List<string> RawSetCookies { get; set; }
                }

                public class ConnectResult
                {
                    public bool Success { get; set; }
                    public string Message { get; set; } = "";
                    public string VerificationUrl { get; set; } = "";
                    public string UserCode { get; set; } = "";
                }
            }
        }
    }
}
