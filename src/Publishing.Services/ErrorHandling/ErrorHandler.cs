#if WINDOWS
using System.Windows.Forms;
#endif
using System;
using Publishing.Core.Interfaces;

namespace Publishing.Services
{
    public class ErrorHandler : IErrorHandler
    {
        private readonly ILogger _logger;

        public ErrorHandler(ILogger logger)
        {
            _logger = logger;
        }

        public void Handle(Exception ex)
        {
            _logger.LogError(ex.Message, ex);
#if WINDOWS
            MessageBox.Show(ex.Message, "Помилка");
#endif
        }

        public void ShowFriendlyError(string message)
        {
#if WINDOWS
            MessageBox.Show(message, "Помилка");
#else
            _logger.LogInformation(message);
#endif
        }
    }
}
