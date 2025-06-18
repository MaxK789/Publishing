using System;

namespace Publishing.Core.Interfaces
{
    public interface ILogger
    {
        void LogInformation(string message);
        void LogError(string message, Exception ex);
    }
}
