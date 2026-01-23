using System;
using WSO.Commands;

namespace WSO.Services
{
    public class WsoService
    {
        /// <summary>
        /// Initializes the WSO service.
        /// </summary>
        public void Initialize()
        {
            Console.WriteLine("WSO Service initialized.");
        }

        /// <summary>
        /// Example method to send a command to the Jester wheel backend.
        /// </summary>
        public void ConnectAirSupply()
        {
            HbSendProxyCommand.SendCommand("groundcrew", "connect_air_supply");
        }

        /// <summary>
        /// Example method to send a custom command.
        /// </summary>
        /// <param name="category">The category of the command.</param>
        /// <param name="action">The specific action to perform.</param>
        /// <param name="value">Optional value to pass with the action.</param>
        public void SendCustomCommand(string category, string action, string value = "")
        {
            HbSendProxyCommand.SendCommand(category, action, value);
        }
    }
}