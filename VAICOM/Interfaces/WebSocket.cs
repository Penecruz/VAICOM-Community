using System;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VAICOM.Static;

namespace VAICOM
{
    namespace Interfaces
    {

        public partial class Network
        {

            public static void WebSocketSetup()
            {
                // do stuff in for config, currently hardcoded
            }

            public static void WebSocketStart()
            {
                Task WebSocketServer = new Task(WebSocketServerStart);
                WebSocketServer.Start();
            }

            public static async void WebSocketServerStart()
            {
                HttpListener listener = new HttpListener();
                listener.Prefixes.Add("http://localhost:5000/vaicom/");
                listener.Start();
                Log.Write("WebSocket server started at ws://localhost:5000/vaicom/", Colors.Text);

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
                        WebSocketReceiveResult result = await webSocket.ReceiveAsync(
                            new ArraySegment<byte>(buffer), CancellationToken.None);

                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                            Log.Write("Client disconnected.", Colors.Text);
                        }
                        else
                        {
                            string receivedMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
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
