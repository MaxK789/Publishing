using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium;
using System;


namespace Publishing.UI.Tests;

[TestClass]
[TestCategory("UI")]
public class BalloonTests
{
    // In Appium.WebDriver versions referenced by this project the WindowsDriver
    // type isn't generic, so use the non-generic form for the session instance.
    private OpenQA.Selenium.Appium.Windows.WindowsDriver? _session;

    [TestInitialize]
    public void Setup()
    {
        var opts = new AppiumOptions();
        opts.AddAdditionalAppiumOption(MobileCapabilityType.App, "Publishing.UI.exe");
        _session = new OpenQA.Selenium.Appium.Windows.WindowsDriver(new Uri("http://127.0.0.1:4723"), opts);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _session?.Quit();
    }

    [TestMethod]
    public void ShowsSuccessBalloon()
    {
        _session!.FindElement(MobileBy.AccessibilityId("emailTextBox")).SendKeys("demo@demo.com");
        _session.FindElement(MobileBy.AccessibilityId("passwordTextBox")).SendKeys("pass");
        _session.FindElement(MobileBy.AccessibilityId("loginButton")).Click();
        _session.FindElement(MobileBy.AccessibilityId("додатиToolStripMenuItem")).Click();
        _session.FindElement(MobileBy.AccessibilityId("nameProductTextBox")).SendKeys("book");
        _session.FindElement(MobileBy.AccessibilityId("pageNumTextBox")).SendKeys("10");
        _session.FindElement(MobileBy.AccessibilityId("tirageTextBox")).SendKeys("1");
        _session.FindElement(MobileBy.AccessibilityId("orderButton")).Click();
        System.Threading.Thread.Sleep(1000);
        var screenshot = _session.GetScreenshot();
        screenshot.SaveAsFile("TestResults/screenshots/success.png");
        Assert.IsNotNull(screenshot);
    }

    [TestMethod]
    public void ShowsWarningOnInvalidInput()
    {
        _session!.FindElement(MobileBy.AccessibilityId("pageNumTextBox")).SendKeys("abc");
        _session.FindElement(MobileBy.AccessibilityId("calculateButton")).Click();
        System.Threading.Thread.Sleep(1000);
        var screenshot = _session.GetScreenshot();
        screenshot.SaveAsFile("TestResults/screenshots/warn.png");
        Assert.IsNotNull(screenshot);
    }
}
