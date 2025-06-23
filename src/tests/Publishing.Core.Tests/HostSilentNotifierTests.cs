using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Publishing.Services;
using System;

namespace Publishing.Core.Tests;

[TestClass]
public class HostSilentNotifierTests
{
    [TestMethod]
    public void WebHost_UsesSilentNotifierWhenDisabled()
    {
        Environment.SetEnvironmentVariable("NOTIFICATIONS_DISABLED", "true");
        using var host = Host.CreateDefaultBuilder()
            .ConfigureServices(s => s.AddUiNotifier())
            .Build();
        var notifier = host.Services.GetService(typeof(IUiNotifier));
        Assert.IsInstanceOfType(notifier, typeof(SilentUiNotifier));
        Environment.SetEnvironmentVariable("NOTIFICATIONS_DISABLED", null);
    }
}
