using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VAICOM.Extensions.CPG
{
    public enum AH64CMDispenseMode
    {
        None,
        Chaff,
        Flares,
        ChaffAndFlares
    }

    public class AH64GeorgeState
    {
        public static AH64CMDispenseMode AH64GeorgeSelectedCMDispenseMode = AH64CMDispenseMode.None;

        private static List<AH64CMDispenseMode> GetGeorgeCMDispenseOrder()
        {
            return new List<AH64CMDispenseMode>
                    {
                        AH64CMDispenseMode.None,
                        AH64CMDispenseMode.Chaff,
                        AH64CMDispenseMode.Flares,
                        AH64CMDispenseMode.ChaffAndFlares
                    };
        }

        private static int GetGeorgeCMDispenseSteps(AH64CMDispenseMode from, AH64CMDispenseMode to)
        {
            if (from == to)
            {
                return 0;
            }

            var order = GetGeorgeCMDispenseOrder();
            int fromIndex = order.IndexOf(from);
            int toIndex = order.IndexOf(to);

            if (fromIndex < 0)
            {
                fromIndex = 0;
            }

            if (toIndex < 0)
            {
                return 0;
            }

            if (toIndex >= fromIndex)
            {
                return toIndex - fromIndex;
            }

            return (order.Count - fromIndex) + toIndex;
        }

        public static List<AH64GeorgeButton> SelectGeorgeCMDispenseMode(AH64CMDispenseMode target)
        {
            List<AH64GeorgeButton> actions = new List<AH64GeorgeButton>();

            var current = AH64GeorgeSelectedCMDispenseMode;
            int steps = GetGeorgeCMDispenseSteps(current, target);
            for (int i = 0; i < steps; i++)
            {
                actions.Add(AH64GeorgeButton.Right);
            }

            AH64GeorgeSelectedCMDispenseMode = target;

            return actions;
        }
    }
}
