using System;

namespace WSO.Helpers
{
    public static class Logger
    {
        public static void Log(string message)
        {
            Console.WriteLine($"[WSO] {message}");
        }

        public static void LogError(string message)
        {
            Console.WriteLine($"[WSO ERROR] {message}");
        }
    }
}