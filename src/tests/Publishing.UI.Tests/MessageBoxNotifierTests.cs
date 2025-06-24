using Microsoft.VisualStudio.TestTools.UnitTesting;
using Publishing.Services;
using System.Windows.Forms;

namespace Publishing.UI.Tests;

[TestClass]
[TestCategory("UI")]
public class MessageBoxNotifierTests
{
    [TestMethod]
    public void NotifyInfo_ShowsMessageBox()
    {
        var notifier = new MessageBoxNotifier();
        System.Threading.Tasks.Task.Run(() =>
        {
            System.Threading.Thread.Sleep(500);
            System.Windows.Forms.SendKeys.SendWait("{ENTER}");
        });
        notifier.NotifyInfo("hello");
    }
}
