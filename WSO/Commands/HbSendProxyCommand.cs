using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;

namespace VAICOM.WSO
{
    public class WsoMessage
    {
        public string category;
        public string action;
        public string value;

        public WsoMessage(string category, string action, string value)
        {
            this.category = category;
            this.action = action;
            this.value = value;
        }
    }

    public class WsoDialogMessage
    {
        public string action;
        public string command;

        public WsoDialogMessage(string action, string command)
        {
            this.action = action;
            this.command = command;
        }
    }

    public static class HbSendProxyCommand
    {
        /// <summary>
        /// Sends a WSO command using the CommandMap.
        /// </summary>
        /// <param name="commandKey">The key of the command to send.</param>
        public static void SendWsoCommand(WebSocket webSocket, string commandKey)
        {
            if (WSOCommandMappings.CommandMap.TryGetValue(commandKey, out var command))
            {
                if (command.valueRequired && string.IsNullOrEmpty(command.value))
                {
                    Console.WriteLine($"Command '{commandKey}' missing required value.");
                }
                else
                {
                    SendCommand(webSocket, command.category, command.action, command.value);
                }
            }
            else
            {
                Console.WriteLine($"Command '{commandKey}' not found in CommandMap.");
            }
        }

        /// <summary>
        /// Sends a WSO command using the CommandMap and a dynamic value.
        /// </summary>
        /// <param name="commandKey">The key of the command to send.</param>
        /// <param name="value">The value for the command to send.</param>
        public static void SendWsoCommand(WebSocket webSocket, string commandKey, string value)
        {
            if (WSOCommandMappings.CommandMap.TryGetValue(commandKey, out var command))
            {
                if (command.valueRequired && string.IsNullOrEmpty(value))
                {
                    Console.WriteLine($"Command '{commandKey}' missing required value.");
                }
                else
                {
                    SendCommand(webSocket, command.category, command.action, value);
                }
            }
            else
            {
                Console.WriteLine($"Command '{commandKey}' not found in CommandMap.");
            }
        }

        /// <summary>
        /// Sends a command directly to the backend using the hb_send_proxy function.
        /// </summary>
        /// <param name="webSocket">The WebSocket to send the command to.</param>
        /// <param name="category">The category of the command.</param>
        /// <param name="action">The specific action to perform.</param>
        /// <param name="value">Optional value to pass with the action.</param>
        public static async void SendCommand(WebSocket webSocket, string category, string action, string value)
        {
            // Ensure value is not null or undefined
            value = value ?? "";

            if (webSocket != null && webSocket.State == WebSocketState.Open)
            {
                WsoMessage wsoMessage = new WsoMessage(category, action, value);
                string message = JsonConvert.SerializeObject(wsoMessage);
                byte[] messageBuffer = Encoding.UTF8.GetBytes(message);
                Console.WriteLine($"Sending wheel message to web socket client: {message}");
                await webSocket.SendAsync(
                    new ArraySegment<byte>(messageBuffer),
                    WebSocketMessageType.Text,
                    true,
                    CancellationToken.None);
            }
            else
            {
                Console.WriteLine("Jester wheel websocket was null or closed");
            }
        }

        /// <summary>
        /// Sends a command directly to the backend using the hb_send_proxy function.
        /// </summary>
        /// <param name="webSocket">The WebSocket to send the command to.</param>
        /// <param name="category">The category of the command.</param>
        /// <param name="action">The specific action to perform.</param>
        /// <param name="value">Optional value to pass with the action.</param>
        public static async void SendDialogCommand(WebSocket webSocket, string action, string command)
        {
            // Ensure value is not null or undefined
            command = command ?? "";

            if (webSocket != null && webSocket.State == WebSocketState.Open)
            {
                //WsoDialogMessage wsoMessage = new WsoDialogMessage(action, command);
                WsoDialogMessage wsoMessage = new WsoDialogMessage("action", action);
                string message = JsonConvert.SerializeObject(wsoMessage);
                byte[] messageBuffer = Encoding.UTF8.GetBytes(message);
                Console.WriteLine($"Sending dialog message to web socket client: {message}");
                await webSocket.SendAsync(
                    new ArraySegment<byte>(messageBuffer),
                    WebSocketMessageType.Text,
                    true,
                    CancellationToken.None);
            }
            else
            {
                Console.WriteLine("Jester dialog websocket was null or closed");
            }
        }
    }
}