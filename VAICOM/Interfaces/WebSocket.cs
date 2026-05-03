using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using VAICOM.Static;

namespace VAICOM
{
    namespace Interfaces
    {

        public partial class Network
        {
            private static readonly string ServerAddress = "127.0.0.1";
            private static readonly int ServerPort = 33495;
            private static readonly object WsoNavCacheBulkSignatureLock = new object();
            private static string WsoNavCacheBulkLastSignature = "";


            public static void WebSocketSetup()
            {
                // do stuff for config, currently hardcoded
            }

            public static void WebSocketStart()
            {
                Task WebSocketServer = new Task(WebSocketServerStart);
                WebSocketServer.Start();
            }

            public static async void WebSocketServerStart()
            {
                string webSocketEndpoint = $"http://{ServerAddress}:{ServerPort}/vaicom/wso/";
                HttpListener listener = new HttpListener();
                listener.Prefixes.Add(webSocketEndpoint);
                listener.Start();
                Log.Write($"WebSocket server started at {webSocketEndpoint}", Colors.Text);

                while (true)
                {
                    try
                    {
                        HttpListenerContext context = await listener.GetContextAsync();

                        // Check if it's a WebSocket request
                        if (context.Request.IsWebSocketRequest)
                        {
                            _ = HandleWebSocketAsync(context); // Fire and forget
                        }
                        else
                        {
                            context.Response.StatusCode = 400;
                            context.Response.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Write($"[Server Error] {ex.Message}", Colors.Text);
                    }
                }
            }

            private static async Task HandleWebSocketAsync(HttpListenerContext context)
            {
                WebSocket webSocket = null;
                try
                {
                    HttpListenerWebSocketContext wsContext = await context.AcceptWebSocketAsync(null);
                    webSocket = wsContext.WebSocket;
                    Log.Write("Client connected.", Colors.Text);
                    State.WebSocketClient = webSocket;

                    byte[] buffer = new byte[1024 * 4];

                    while (webSocket.State == WebSocketState.Open)
                    {
                        StringBuilder messageBuilder = new StringBuilder();
                        WebSocketReceiveResult result;

                        do
                        {
                            result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                            if (result.MessageType == WebSocketMessageType.Close)
                            {
                                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                                Log.Write("Client disconnected.", Colors.Text);
                                break;
                            }

                            if (result.Count > 0)
                            {
                                messageBuilder.Append(Encoding.UTF8.GetString(buffer, 0, result.Count));
                            }
                        }
                        while (!result.EndOfMessage);

                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            continue;
                        }

                        string receivedMessage = messageBuilder.ToString();
                        if (!TryHandleNavCacheMessage(receivedMessage) && !TryHandleJesterMenuCacheLine(receivedMessage))
                        {
                            Log.Write($"Received: {receivedMessage}", Colors.Text);
                        }
                    }
                }

                catch (Exception ex)
                {
                    Log.Write($"WebSocket Error: {ex.Message}", Colors.Text);
                }
                finally
                {
                    webSocket?.Dispose();
                    State.WebSocketClient = null;
                }
            }

            private static bool TryHandleJesterMenuCacheLine(string receivedMessage)
            {
                if (string.IsNullOrWhiteSpace(receivedMessage))
                {
                    return false;
                }

                const string prefix = "Jester Menu:";
                if (!receivedMessage.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }

                string payload = receivedMessage.Substring(prefix.Length).Trim();
                string[] parts = payload.Split('|');
                if (parts.Length < 3)
                {
                    return false;
                }

                string category = parts[0].Trim();
                string action = parts[1].Trim();
                string value = string.Join("|", parts, 2, parts.Length - 2).Trim();

                if (!category.Equals("select", StringComparison.OrdinalIgnoreCase)
                    || string.IsNullOrWhiteSpace(action)
                    || string.IsNullOrWhiteSpace(value)
                    || !value.Contains(";"))
                {
                    return false;
                }

                JObject entry = new JObject
                {
                    ["action"] = action,
                    ["value"] = value
                };

                string index = TryExtractIndexFromValue(value);
                if (!string.IsNullOrWhiteSpace(index))
                {
                    entry["idx"] = index;
                }

                return TryCacheNavEntry(entry);
            }

            private static bool TryHandleNavCacheMessage(string receivedMessage)
            {
                if (string.IsNullOrWhiteSpace(receivedMessage) || !receivedMessage.TrimStart().StartsWith("{"))
                {
                    return false;
                }

                try
                {
                    JObject message = JObject.Parse(receivedMessage);
                    string messageType = message["type"]?.ToString();

                    if (string.IsNullOrWhiteSpace(messageType))
                    {
                        return false;
                    }

                    if (messageType.Equals("nav_cache", StringComparison.OrdinalIgnoreCase))
                    {
                        return TryCacheNavEntry(message);
                    }

                    if (!messageType.Equals("nav_cache_bulk", StringComparison.OrdinalIgnoreCase))
                    {
                        return false;
                    }

                    JArray items = message["items"] as JArray;
                    if (items == null || items.Count == 0)
                    {
                        return false;
                    }

                    WriteWsoNavCacheRawToFile(receivedMessage);

                    if (TrySkipUnchangedNavCacheBulk(items))
                    {
                        return true;
                    }
                    else
                    {
                        // An update has been made to the menu items. Rebuild the cache so that
                        // it is in a consistent state as entries may have been removed.
                        lock (State.WsoNavCacheLock)
                        {
                            Log.Write($"WSO menus have been updated so rebuilding caches", Colors.Text);
                            State.WsoNavCacheByActionAndIndex = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                            State.WsoNavCacheByActionAndName = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                        }
                    }

                    int cachedCount = 0;
                    foreach (JToken item in items)
                    {
                        if (item is JObject entry && TryCacheNavEntry(entry, false))
                        {
                            cachedCount++;
                        }
                    }

                    if (cachedCount > 0)
                    {
                        // Log.Write($"WSO NAV cache updated ({cachedCount} entries).", Colors.Text);
                        // if (State.deepdebugmode)
                        // {
                        //     LogWsoNavCacheSnapshot(50);
                        // }
                        return true;
                    }

                    return false;
                }
                catch
                {
                    return false;
                }
            }

            private static void WriteWsoNavCacheRawToFile(string rawMessage)
            {
                try
                {
                    string logsFolder = State.VA_APPS + "\\" + Products.Products.Families.Vaicom.VaicomProPlugin.rootfoldername + "\\" + AppData.SubFolders["logfiles"];
                    Directory.CreateDirectory(logsFolder);

                    string rawFile = Path.Combine(logsFolder, "WSO_NAV_CACHE_RAW.json");
                    File.WriteAllText(rawFile, rawMessage ?? "");
                }
                catch
                {
                }
            }

            private static bool TrySkipUnchangedNavCacheBulk(JArray items)
            {
                string currentSignature = BuildStableNavCacheBulkSignature(items);

                lock (WsoNavCacheBulkSignatureLock)
                {
                    if (string.Equals(WsoNavCacheBulkLastSignature, currentSignature, StringComparison.Ordinal))
                    {
                        return true;
                    }

                    WsoNavCacheBulkLastSignature = currentSignature;
                    return false;
                }
            }

            private static string BuildStableNavCacheBulkSignature(JArray items)
            {
                List<string> normalizedEntries = new List<string>(items.Count);

                foreach (JToken token in items)
                {
                    if (!(token is JObject entry))
                    {
                        continue;
                    }

                    string action = entry["action"]?.ToString() ?? "";
                    string value = entry["value"]?.ToString() ?? "";
                    string name = entry["name"]?.ToString() ?? "";
                    string path = entry["path"]?.ToString() ?? "";
                    string index = entry["idx"]?.ToString() ?? "";

                    // Exclude any entries for diverting to assets as those
                    // can have constantly changing lat long coordinates which
                    // break the signature causing continuous cache rebuilds.
                    if (path.Contains("Divert To > Assets"))
                    {
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(index))
                    {
                        index = TryExtractIndexFromValue(value);
                        if (string.IsNullOrWhiteSpace(index))
                        {
                            index = TryExtractIndexFromNameOrPath(name, path);
                        }
                    }

                    normalizedEntries.Add(string.Concat(
                        action, "|",
                        index, "|",
                        name, "|",
                        path, "|",
                        value));
                }

                normalizedEntries.Sort(StringComparer.Ordinal);
                return string.Join("\n", normalizedEntries);
            }

            private static void LogWsoNavCacheSnapshot(int maxEntries)
            {
                int printed = 0;

                lock (State.WsoNavCacheLock)
                {
                    foreach (KeyValuePair<string, string> entry in State.WsoNavCacheByActionAndIndex)
                    {
                        Log.Write($"WSO NAV cache [idx] {entry.Key} => {entry.Value}", Colors.Text);
                        printed++;
                        if (printed >= maxEntries)
                        {
                            break;
                        }
                    }

                    if (printed < maxEntries)
                    {
                        foreach (KeyValuePair<string, string> entry in State.WsoNavCacheByActionAndName)
                        {
                            Log.Write($"WSO NAV cache [name] {entry.Key} => {entry.Value}", Colors.Text);
                            printed++;
                            if (printed >= maxEntries)
                            {
                                break;
                            }
                        }
                    }
                }
            }

            private static bool TryCacheNavEntry(JObject message, bool logSingleEntry = true)
            {
                string action = message["action"]?.ToString();
                string value = message["value"]?.ToString();
                string name = message["name"]?.ToString();
                string index = message["idx"]?.ToString();
                string path = message["path"]?.ToString();

                if (string.IsNullOrWhiteSpace(action) || string.IsNullOrWhiteSpace(value))
                {
                    return false;
                }

                if (string.Equals(action, "radio_tune_atc") && !string.IsNullOrEmpty(name))
                {
                    int freqIndex = name.IndexOf('(');
                    if (freqIndex > 0)
                    {
                        lock (State.WsoNavCacheLock)
                        {
                            string airfield = name.Substring(0, freqIndex - 1);
                            State.WsoNavCacheByActionAndName[$"{action}|{airfield.ToLowerInvariant()}"] = value;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (string.Equals(action, "nav_tacan_tr"))
                {
                    if (!path.Contains("Tune Assets"))
                    {
                        lock (State.WsoNavCacheLock)
                        {
                            string station = name.Substring(0, 3);
                            State.WsoNavCacheByActionAndName[$"{action}|{station.ToLowerInvariant()}"] = value;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(index))
                    {
                        index = TryExtractIndexFromValue(value);
                        if (string.IsNullOrWhiteSpace(index))
                        {
                            index = TryExtractIndexFromNameOrPath(name, path);
                        }
                    }

                    lock (State.WsoNavCacheLock)
                    {
                        if (!string.IsNullOrWhiteSpace(index))
                        {
                            State.WsoNavCacheByActionAndIndex[$"{action}|{index}"] = value;

                            string flightPlan = TryExtractFlightPlanFromPath(path);
                            if (!string.IsNullOrWhiteSpace(flightPlan))
                            {
                                State.WsoNavCacheByActionAndIndex[$"{action}|fp{flightPlan}|{index}"] = value;
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(name))
                        {
                            State.WsoNavCacheByActionAndName[$"{action}|{name.ToLowerInvariant()}"] = value;
                        }
                    }
                }


                if (logSingleEntry)
                {
                    List<string> keyParts = new List<string>();
                    if (!string.IsNullOrWhiteSpace(index))
                    {
                        keyParts.Add($"idx={index}");
                    }
                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        keyParts.Add($"name={name}");
                    }

                    string keyInfo = keyParts.Count > 0 ? string.Join(", ", keyParts) : "no key";
                    // Log.Write($"WSO NAV cache entry: action={action}, {keyInfo}, value={value}", Colors.Text);
                }

                return true;
            }

            private static string TryExtractIndexFromValue(string value)
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return "";
                }

                string[] tokens = value.Split(';');
                if (tokens.Length < 3)
                {
                    return "";
                }

                string candidate = tokens[tokens.Length - 1].Trim();
                return int.TryParse(candidate, out int parsed) ? parsed.ToString() : "";
            }

            private static string TryExtractIndexFromNameOrPath(string name, string path)
            {
                string fromName = TryExtractLeadingIndex(name);
                if (!string.IsNullOrWhiteSpace(fromName))
                {
                    return fromName;
                }

                if (string.IsNullOrWhiteSpace(path))
                {
                    return "";
                }

                string[] parts = path.Split('>');
                for (int i = parts.Length - 1; i >= 0; i--)
                {
                    string fromPart = TryExtractLeadingIndex(parts[i]);
                    if (!string.IsNullOrWhiteSpace(fromPart))
                    {
                        return fromPart;
                    }
                }

                return "";
            }

            private static string TryExtractLeadingIndex(string text)
            {
                if (string.IsNullOrWhiteSpace(text))
                {
                    return "";
                }

                Match match = Regex.Match(text, @"^\s*\*?\s*(\d{1,3})\s*:");
                if (!match.Success)
                {
                    return "";
                }

                return int.TryParse(match.Groups[1].Value, out int parsed) ? parsed.ToString() : "";
            }

            private static string TryExtractFlightPlanFromPath(string path)
            {
                if (string.IsNullOrWhiteSpace(path))
                {
                    return "";
                }

                if (path.IndexOf("Secondary Flight Plan", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return "2";
                }

                if (path.IndexOf("Primary Flight Plan", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return "1";
                }

                return "";
            }
        }
    }
}
