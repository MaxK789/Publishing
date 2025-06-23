using Microsoft.VisualStudio.TestTools.UnitTesting;
using Publishing.Services;
using Publishing.Core.Interfaces;
using System;

namespace Publishing.Core.Tests
{
    [TestClass]
    public class ErrorHandlerTests
    {
        private class StubLogger : ILogger
        {
            public string? Info;
            public Exception? LoggedException;
            public string? Error;
            public void LogInformation(string message) => Info = message;
            public void LogError(string message, Exception ex)
            {
                Error = message;
                LoggedException = ex;
            }
        }

        private class StubNotifier : IUiNotifier
        {
            public string? Info;
            public string? Warning;
            public string? Error;
            public string? Details;
            public void NotifyInfo(string message) => Info = message;
            public void NotifyWarning(string message) => Warning = message;
            public void NotifyError(string message, string? details = null)
            {
                Error = message;
                Details = details;
            }
        }

        [TestMethod]
        public void Handle_LogsError()
        {
            var logger = new StubLogger();
            var notifier = new StubNotifier();
            var handler = new ErrorHandler(logger, notifier);
            var ex = new InvalidOperationException("boom");

            handler.Handle(ex);

            Assert.AreEqual("boom", logger.Error);
            Assert.AreEqual(ex, logger.LoggedException);
        }

        [TestMethod]
        public void Handle_ProducesUiError()
        {
            var logger = new StubLogger();
            var notifier = new StubNotifier();
            var handler = new ErrorHandler(logger, notifier);
            var ex = new InvalidOperationException("oops");

            handler.Handle(ex);

            Assert.AreEqual("oops", notifier.Error);
            Assert.IsTrue(notifier.Details!.Contains("InvalidOperationException"));
        }

        private class FailingNotifier : IUiNotifier
        {
            public void NotifyInfo(string message) { }
            public void NotifyWarning(string message) { }
            public void NotifyError(string message, string? details = null) => throw new InvalidOperationException();
        }

        [TestMethod]
        public void Handle_DoesNotThrowWhenNotifierFails()
        {
            var logger = new StubLogger();
            var notifier = new FailingNotifier();
            var handler = new ErrorHandler(logger, notifier);

            handler.Handle(new Exception("x"));
        }

    }
}
