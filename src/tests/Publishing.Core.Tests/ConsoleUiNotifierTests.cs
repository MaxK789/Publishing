using Microsoft.VisualStudio.TestTools.UnitTesting;
using Publishing.Services;
using System;
using System.IO;

namespace Publishing.Core.Tests
{
    [TestClass]
    public class ConsoleUiNotifierTests
    {
        [TestMethod]
        public void NotifyMethods_WritePrefixes()
        {
            var writer = new StringWriter();
            Console.SetOut(writer);
            var notifier = new ConsoleUiNotifier();

            notifier.NotifyInfo("i");
            notifier.NotifyWarning("w");
            notifier.NotifyError("e");

            var expected = string.Join(Environment.NewLine, new[] {
                "INFO: i",
                "WARN: w",
                "ERROR: e",
                ""
            });
            Assert.AreEqual(expected, writer.ToString());
        }

        [TestMethod]
        public void NotifyError_WritesDetails()
        {
            var writer = new StringWriter();
            Console.SetOut(writer);
            var notifier = new ConsoleUiNotifier();
            var details = new string('a', 100);

            notifier.NotifyError("boom", details);

            var output = writer.ToString();
            StringAssert.Contains(output, "ERROR: boom");
            StringAssert.Contains(output, details);
        }

        [TestMethod]
        public void Colors_ResetAfterWrite()
        {
            var original = Console.ForegroundColor;
            var notifier = new ConsoleUiNotifier();
            notifier.NotifyWarning("msg");
            Assert.AreEqual(original, Console.ForegroundColor);
        }
    }
}
