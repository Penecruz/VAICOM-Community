using System;

namespace VAICOM.WSO
{
    public static class HbSendProxyCommand
    {
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
        /// Sends a command to the backend.
        /// </summary>
        /// <param name="category">The category of the command.</param>
        /// <param name="action">The specific action to perform.</param>
        /// <param name="value">Optional value to pass with the action.</param>
        public static void SendCommand(string category, string action, string value = "")
        {
            // Implementation for sending the command to the backend.
            Console.WriteLine($"Sending command: Category='{category}', Action='{action}', Value='{value}'");
            // Add actual backend communication logic here.
        }
    }
}