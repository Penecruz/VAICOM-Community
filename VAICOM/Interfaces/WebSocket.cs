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
            private static readonly int ServerPort = 33495;

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
                        if (!WSOActionCache.TryHandleActionCacheMessage(receivedMessage) && !WSOActionCache.TryHandleJesterMenuCacheLine(receivedMessage))
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
        }
    }
}
