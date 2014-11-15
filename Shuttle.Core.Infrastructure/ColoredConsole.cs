using System;

namespace Shuttle.Core.Infrastructure
{
    public static class ColoredConsole
    {
        private static readonly object padlock = new object();

        public static void WriteLine(ConsoleColor color, string format, params object[] arg)
        {
            lock (padlock)
            {
                var foregroundColor = Console.ForegroundColor;

                Console.ForegroundColor = color;
                Console.WriteLine(format, arg);
                Console.ForegroundColor = foregroundColor;
            }
        }
    }
}