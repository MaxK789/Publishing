using Microsoft.VisualStudio.TestTools.UnitTesting;
using Publishing.Services;

namespace Publishing.UI.Tests
{
[TestClass]
[TestCategory("UI")]
public class WinFormsUiNotifierTests
    {
        [TestMethod]
        public void NotifyInfo_SetsBalloonProperties()
        {
            using var notifier = new WinFormsUiNotifier();
            notifier.NotifyInfo("hello");

            var field = typeof(WinFormsUiNotifier).GetField("_notifyIcon", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Assert.IsNotNull(field);
            var icon = (System.Windows.Forms.NotifyIcon)field!.GetValue(notifier)!;
            Assert.AreEqual("Publishing", icon.BalloonTipTitle);
            Assert.AreEqual("hello", icon.BalloonTipText);
        }
    }
}
