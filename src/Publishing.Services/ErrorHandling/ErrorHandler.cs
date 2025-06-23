using System;
using Publishing.Core.Interfaces;

namespace Publishing.Services
{
    public class ErrorHandler : IErrorHandler
    {
        private readonly ILogger _logger;
        private readonly IUiNotifier _notifier;

        public ErrorHandler(ILogger logger, IUiNotifier notifier)
        {
            _logger = logger;
            _notifier = notifier;
        }

        public void Handle(Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            try
            {
                _notifier.NotifyError(ex.Message, ex.ToString());
            }
            catch
            {
                // ignore notifier failures
            }
        }

    }
}
