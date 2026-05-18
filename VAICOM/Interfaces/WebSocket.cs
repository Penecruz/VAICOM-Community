using System;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VAICOM.Static;
using VAICOM.Extensions.AIWSO;

namespace VAICOM
{
    namespace Interfaces
    {

        public partial class Network
        {
            private static readonly string ServerAddress = "127.0.0.1";
            private static readonly int WheelServerPort = 33495;
            private static readonly int DialogServerPort = 33496;

            public static void WebSocketSetup()
            {
                // do stuff for config, currently hardcoded
            }

            public static void WebSocketStart()
            {
                Task WebSocketServerWheel = new Task(WebSocketServerStartWheel);
                WebSocketServerWheel.Start();
                Task WebSocketServerDialog = new Task(WebSocketServerStartDialog);
                WebSocketServerDialog.Start();
            }

            public static async void WebSocketServerStartWheel()
            {
                string webSocketEndpoint = $"http://{ServerAddress}:{WheelServerPort}/vaicom/wso/wheel/";
                HttpListener listener = new HttpListener();
                listener.Prefixes.Add(webSocketEndpoint);
                listener.Start();
                Log.Write($"WebSocket server for Jester wheel started at {webSocketEndpoint}", Colors.Text);

                while (true)
                {
                    try
                    {
                        HttpListenerContext context = await listener.GetContextAsync();

                        // Check if it's a WebSocket request
                        if (context.Request.IsWebSocketRequest)
                        {
                            _ = HandleWebSocketWheelAsync(context); // Fire and forget
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

            private static async Task HandleWebSocketWheelAsync(HttpListenerContext context)
            {
                WebSocket webSocket = null;
                try
                {
                    HttpListenerWebSocketContext wsContext = await context.AcceptWebSocketAsync(null);
                    webSocket = wsContext.WebSocket;
                    Log.Write("Jester wheel client connected.", Colors.Text);
                    State.WsoWheelClient = webSocket;

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
                                Log.Write("Jester wheel client disconnected.", Colors.Text);
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
                        if (!WSOActionCache.TryHandleActionCacheMessage(receivedMessage) && !WSOActionCache.TryHandleJesterMenuActionLine(receivedMessage))
                        {
                            Log.Write($"Received Jester wheel message: {receivedMessage}", Colors.Text);
                        }
                    }
                }

                catch (Exception ex)
                {
                    Log.Write($"Jester wheel WebSocket Error: {ex.Message}", Colors.Text);
                }
                finally
                {
                    webSocket?.Dispose();
                    State.WsoWheelClient = null;
                }
            }

            public static async void WebSocketServerStartDialog()
            {
                string webSocketEndpoint = $"http://{ServerAddress}:{DialogServerPort}/vaicom/wso/dialog/";
                HttpListener listener = new HttpListener();
                listener.Prefixes.Add(webSocketEndpoint);
                listener.Start();
                Log.Write($"WebSocket server for Jester dialog started at {webSocketEndpoint}", Colors.Text);

                while (true)
                {
                    try
                    {
                        HttpListenerContext context = await listener.GetContextAsync();

                        // Check if it's a WebSocket request
                        if (context.Request.IsWebSocketRequest)
                        {
                            _ = HandleWebSocketDialogAsync(context); // Fire and forget
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

            private static async Task HandleWebSocketDialogAsync(HttpListenerContext context)
            {
                WebSocket webSocket = null;
                try
                {
                    HttpListenerWebSocketContext wsContext = await context.AcceptWebSocketAsync(null);
                    webSocket = wsContext.WebSocket;
                    Log.Write("Jester dialog client connected.", Colors.Text);
                    State.WsoDialogClient = webSocket;

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
                                Log.Write("Jester dialog client disconnected.", Colors.Text);
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
                        Log.Write($"Received Jester dialog message: {receivedMessage}", Colors.Text);
                    }
                }

                catch (Exception ex)
                {
                    Log.Write($"Jester dialog WebSocket Error: {ex.Message}", Colors.Text);
                }
                finally
                {
                    webSocket?.Dispose();
                    State.WsoDialogClient = null;
                }
            }
        }
    }
}
