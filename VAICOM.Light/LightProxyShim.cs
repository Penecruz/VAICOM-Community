using System;
using System.Collections.Generic;

namespace VAICOM.Light
{
    public sealed class LightProxyShim
    {
        public sealed class LightDictationShim
        {
            public bool IsOn()
            {
                return false;
            }
        }

        public Dictionary<string, object> SessionState { get; }
        public LightDictationShim Dictation { get; }

        public LightProxyShim(string appsRoot)
        {
            SessionState = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
            {
                ["VA_APPS"] = appsRoot ?? string.Empty,
            };

            Dictation = new LightDictationShim();
        }

        public string GetProfileName()
        {
            return "VAICOM LIGHT";
        }

        public void WriteToLog(string writestring, string color)
        {
            // Intentionally no-op in standalone Light host.
        }

        public void WriteToLog(object writestring, object color)
        {
            // Intentionally no-op in standalone Light host.
        }
    }
}
