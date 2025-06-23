using Microsoft.VisualStudio.TestTools.UnitTesting;
using Publishing.Services;

namespace Publishing.Core.Tests;

[TestClass]
public class SilentUiNotifierTests
{
    [TestMethod]
    public void Methods_DoNotThrow()
    {
        var notifier = new SilentUiNotifier();
        notifier.NotifyInfo("i");
        notifier.NotifyWarning("w");
        notifier.NotifyError("e", "d");
    }
}
