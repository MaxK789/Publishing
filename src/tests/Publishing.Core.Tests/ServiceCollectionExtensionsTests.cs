using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.DependencyInjection;
using Publishing.Services;
using System;

namespace Publishing.Core.Tests
{
    [TestClass]
    public class ServiceCollectionExtensionsTests
    {
        [TestMethod]
        public void AddUiNotifier_RespectsEnvironmentFlag()
        {
            var services = new ServiceCollection();
            Environment.SetEnvironmentVariable("NOTIFICATIONS_DISABLED", "true");
            services.AddUiNotifier();
            var provider = services.BuildServiceProvider();
            var notifier = provider.GetRequiredService<IUiNotifier>();
            Assert.IsInstanceOfType(notifier, typeof(SilentUiNotifier));
            Environment.SetEnvironmentVariable("NOTIFICATIONS_DISABLED", null);
        }

        [TestMethod]
        public void AddUiNotifier_ReturnsPlatformSpecificImplementation()
        {
            var services = new ServiceCollection();
            services.AddUiNotifier();
            var provider = services.BuildServiceProvider();
            var notifier = provider.GetRequiredService<IUiNotifier>();
            var typeName = notifier.GetType().Name;
            if (OperatingSystem.IsWindows())
                Assert.AreEqual("WinFormsUiNotifier", typeName);
            else
                Assert.AreEqual("ConsoleUiNotifier", typeName);
        }
    }
}
