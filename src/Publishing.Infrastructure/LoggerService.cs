using System;
using Publishing.Core.Interfaces;

namespace Publishing.Infrastructure
{
    public class LoggerService : ILogger
    {
        public void LogInformation(string message)
        {
            // In real life, log to file or console
        }

        public void LogError(string message, Exception ex)
        {
            // In real life, log error details
        }
    }
}
