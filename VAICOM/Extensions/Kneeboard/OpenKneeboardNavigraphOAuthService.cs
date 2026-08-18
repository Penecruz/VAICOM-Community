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

                public static async Task<ConnectResult> ConnectWithDeviceFlowAsync(CancellationToken cancellationToken)
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

                        if (!string.IsNullOrWhiteSpace(launchUrl))
                        {
                            TryOpenBrowser(launchUrl);
                        }

                        TokenResponse token = await PollForTokenAsync(config, device, cancellationToken).ConfigureAwait(false);
                        if (token == null || string.IsNullOrWhiteSpace(token.AccessToken))
                        {
                            result.Success = false;
                            result.Message = "No access token returned from Navigraph.";
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

                        OpenKneeboardNavigraphEfbState.SetEncryptedAuthBlob(payload.ToString(Newtonsoft.Json.Formatting.None));

                        result.Success = true;
                        result.Message = "Navigraph authentication completed.";
                        return result;
                    }
                    catch (OperationCanceledException)
                    {
                        result.Success = false;
                        result.Message = "Navigraph authentication canceled.";
                        return result;
                    }
                    catch (Exception ex)
                    {
                        result.Success = false;
                        result.Message = "Navigraph authentication failed: " + ex.Message;
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
                        config.Scope = "openid charts offline_access";
                    }

                    return config;
                }

                private static async Task<DeviceAuthorizationResponse> RequestDeviceAuthorizationAsync(OAuthConfig config, CancellationToken cancellationToken)
                {
                    List<KeyValuePair<string, string>> payload = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("client_id", config.ClientId),
                        new KeyValuePair<string, string>("scope", config.Scope),
                    };
                    if (!string.IsNullOrWhiteSpace(config.ClientSecret))
                    {
                        payload.Add(new KeyValuePair<string, string>("client_secret", config.ClientSecret));
                    }

                    using (HttpClient client = new HttpClient())
                    using (FormUrlEncodedContent content = new FormUrlEncodedContent(payload))
                    {
                        HttpResponseMessage response = await client.PostAsync(config.DeviceAuthorizationEndpoint, content, cancellationToken).ConfigureAwait(false);
                        string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        if (!response.IsSuccessStatusCode)
                        {
                            throw new InvalidOperationException("Device authorization failed: " + json);
                        }

                        JObject obj = JObject.Parse(json);
                        return new DeviceAuthorizationResponse
                        {
                            DeviceCode = (string)obj["device_code"] ?? "",
                            UserCode = (string)obj["user_code"] ?? "",
                            VerificationUri = (string)obj["verification_uri"] ?? "",
                            VerificationUriComplete = (string)obj["verification_uri_complete"] ?? "",
                            ExpiresIn = (int?)obj["expires_in"] ?? 600,
                            Interval = (int?)obj["interval"] ?? 5,
                        };
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
                        };
                        if (!string.IsNullOrWhiteSpace(config.ClientSecret))
                        {
                            payload.Add(new KeyValuePair<string, string>("client_secret", config.ClientSecret));
                        }

                        using (HttpClient client = new HttpClient())
                        using (FormUrlEncodedContent content = new FormUrlEncodedContent(payload))
                        {
                            HttpResponseMessage response = await client.PostAsync(config.TokenEndpoint, content, cancellationToken).ConfigureAwait(false);
                            string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

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

                private static void TryOpenBrowser(string url)
                {
                    if (string.IsNullOrWhiteSpace(url))
                    {
                        return;
                    }

                    try
                    {
                        Process.Start(url);
                    }
                    catch
                    {
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
                }

                private class TokenResponse
                {
                    public string AccessToken { get; set; }
                    public string RefreshToken { get; set; }
                    public string TokenType { get; set; }
                    public string Scope { get; set; }
                    public int ExpiresIn { get; set; }
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
