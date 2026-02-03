using System;
using System.Net.Sockets;
using System.Text;

namespace VAICOM.WSO
{
    public static class HbSendProxyCommand
    {
        private static readonly string ServerAddress = "127.0.0.1"; // Address of the Lua socket
        private static readonly int ServerPort = 33491; // Port of the Lua socket

        /// <summary>
        /// Sends a WSO command using the CommandMap.
        /// </summary>
        /// <param name="commandKey">The key of the command to send.</param>
        public static void SendWsoCommand(string commandKey)
        {
            if (WSOCommandMappings.CommandMap.TryGetValue(commandKey, out var command))
            {
                SendCommand(command.category, command.action, command.value);
            }
            else
            {
                Console.WriteLine($"Command '{commandKey}' not found in CommandMap.");
            }
        }

        /// <summary>
        /// Sends a command directly to the backend using the hb_send_proxy function.
        /// </summary>
        /// <param name="category">The category of the command.</param>
        /// <param name="action">The specific action to perform.</param>
        /// <param name="value">Optional value to pass with the action.</param>
        public static void SendCommand(string category, string action, string value = "")
        {
            // Ensure value is not null or undefined
            value = value ?? "";

            // Construct the hb_send_proxy command
            string commandString = $"{category}|{action}|{value}";

            // Send the command to the Lua socket
            using (var udpClient = new UdpClient())
            {
                try
                {
                    udpClient.Connect(ServerAddress, ServerPort);
                    byte[] data = Encoding.UTF8.GetBytes(commandString);
                    udpClient.Send(data, data.Length);
                    Console.WriteLine($"Sent hb_send_proxy command to Lua socket: {commandString}");                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending hb_send_proxy command to Lua socket: {ex.Message}");
                }
            }
        }
    }
}