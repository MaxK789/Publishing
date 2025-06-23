using System;

namespace Publishing.Services
{
    public class ConsoleUiNotifier : IUiNotifier
    {
        private readonly bool _useColor = !Console.IsOutputRedirected && Environment.GetEnvironmentVariable("CI") != "true";

        public void NotifyInfo(string message)
        {
            Write("INFO", message, ConsoleColor.White);
        }

        public void NotifyWarning(string message)
        {
            Write("WARN", message, ConsoleColor.Yellow);
        }

        public void NotifyError(string message, string? details = null)
        {
            Write("ERROR", message, ConsoleColor.Red);
            if (!string.IsNullOrEmpty(details))
            {
                var truncated = details.Length > 120 ? details[..120] + "..." : details;
                Console.WriteLine(truncated);
            }
        }

        private void Write(string prefix, string message, ConsoleColor color)
        {
            if (_useColor)
            {
                var previous = Console.ForegroundColor;
                Console.ForegroundColor = color;
                Console.WriteLine($"{prefix}: {message}");
                Console.ForegroundColor = previous;
            }
            else
            {
                Console.WriteLine($"{prefix}: {message}");
            }
        }
    }
}
