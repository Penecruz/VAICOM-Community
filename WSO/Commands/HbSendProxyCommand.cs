using System;

namespace WSO.Commands
{
    public static class HbSendProxyCommand
    {
        /// <summary>
        /// Sends a command to the Jester wheel backend using hb_send_proxy.
        /// </summary>
        /// <param name="category">The category of the command (e.g., "groundcrew").</param>
        /// <param name="action">The specific action to perform (e.g., "connect_air_supply").</param>
        /// <param name="value">Optional value to pass with the action.</param>
        public static void SendCommand(string category, string action, string value = "")
        {
            try
            {
                // Construct the hb_send_proxy command
                string script = $"hb_send_proxy('{category}', '{action}', '{value}');";

                // Send the command to the DCS backend
                ExecuteDcsScript(script);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending hb_send_proxy command: {ex.Message}");
            }
        }

        /// <summary>
        /// Executes a script in the DCS backend.
        /// </summary>
        /// <param name="script">The script to execute.</param>
        private static void ExecuteDcsScript(string script)
        {
            // Placeholder for integration with VAICOM's script execution logic
            Console.WriteLine($"Executing script: {script}");
            // TODO: Implement the actual integration with VAICOM's script execution mechanism
        }
    }
}