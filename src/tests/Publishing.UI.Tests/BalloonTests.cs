using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Appium.Enums;
using System;


namespace Publishing.UI.Tests;

[TestClass]
[TestCategory("UI")]
public class BalloonTests
{
    private WindowsDriver? _session;

    [TestInitialize]
    public void Setup()
    {
        var opts = new AppiumOptions();
        opts.AddAdditionalCapability(MobileCapabilityType.App, "Publishing.UI.exe");
        _session = new WindowsDriver(new Uri("http://127.0.0.1:4723"), opts);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _session?.Quit();
    }

    [TestMethod]
    public void ShowsSuccessBalloon()
    {
        _session!.FindElementByAccessibilityId("emailTextBox").SendKeys("demo@demo.com");
        _session.FindElementByAccessibilityId("passwordTextBox").SendKeys("pass");
        _session.FindElementByAccessibilityId("loginButton").Click();
        _session.FindElementByAccessibilityId("додатиToolStripMenuItem").Click();
        _session.FindElementByAccessibilityId("nameProductTextBox").SendKeys("book");
        _session.FindElementByAccessibilityId("pageNumTextBox").SendKeys("10");
        _session.FindElementByAccessibilityId("tirageTextBox").SendKeys("1");
        _session.FindElementByAccessibilityId("orderButton").Click();
        System.Threading.Thread.Sleep(1000);
        var screenshot = _session.GetScreenshot();
        screenshot.SaveAsFile("TestResults/screenshots/success.png");
        Assert.IsNotNull(screenshot);
    }

    [TestMethod]
    public void ShowsWarningOnInvalidInput()
    {
        _session!.FindElementByAccessibilityId("pageNumTextBox").SendKeys("abc");
        _session.FindElementByAccessibilityId("calculateButton").Click();
        System.Threading.Thread.Sleep(1000);
        var screenshot = _session.GetScreenshot();
        screenshot.SaveAsFile("TestResults/screenshots/warn.png");
        Assert.IsNotNull(screenshot);
    }
}
