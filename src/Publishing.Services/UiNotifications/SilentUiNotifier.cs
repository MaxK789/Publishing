using System;

namespace Publishing.Services
{
    public class SilentUiNotifier : IUiNotifier
    {
        public void NotifyInfo(string message) { }
        public void NotifyWarning(string message) { }
        public void NotifyError(string message, string? details = null) { }
    }
}
