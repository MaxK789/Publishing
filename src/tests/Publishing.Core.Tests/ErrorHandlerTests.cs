using Microsoft.VisualStudio.TestTools.UnitTesting;
using Publishing.Services.ErrorHandling;
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

        [TestMethod]
        public void Handle_LogsError()
        {
            var logger = new StubLogger();
            var handler = new ErrorHandler(logger);
            var ex = new InvalidOperationException("boom");

            handler.Handle(ex);

            Assert.AreEqual("boom", logger.Error);
            Assert.AreEqual(ex, logger.LoggedException);
        }

        [TestMethod]
        public void ShowFriendlyError_LogsInfo()
        {
            var logger = new StubLogger();
            var handler = new ErrorHandler(logger);

            handler.ShowFriendlyError("msg");

            Assert.AreEqual("msg", logger.Info);
        }
    }
}
