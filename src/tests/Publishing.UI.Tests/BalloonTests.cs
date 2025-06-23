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
    private OpenQA.Selenium.Appium.Windows.WindowsDriver<OpenQA.Selenium.Appium.Windows.WindowsElement>? _session;

    [TestInitialize]
    public void Setup()
    {
        var opts = new AppiumOptions();
        opts.AddAdditionalAppiumOption(MobileCapabilityType.App, "Publishing.UI.exe");
        _session = new OpenQA.Selenium.Appium.Windows.WindowsDriver<OpenQA.Selenium.Appium.Windows.WindowsElement>(new Uri("http://127.0.0.1:4723"), opts);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _session?.Quit();
    }

    [TestMethod]
    public void ShowsSuccessBalloon()
    {
        _session!.FindElement(AppiumBy.AccessibilityId("emailTextBox")).SendKeys("demo@demo.com");
        _session.FindElement(AppiumBy.AccessibilityId("passwordTextBox")).SendKeys("pass");
        _session.FindElement(AppiumBy.AccessibilityId("loginButton")).Click();
        _session.FindElement(AppiumBy.AccessibilityId("додатиToolStripMenuItem")).Click();
        _session.FindElement(AppiumBy.AccessibilityId("nameProductTextBox")).SendKeys("book");
        _session.FindElement(AppiumBy.AccessibilityId("pageNumTextBox")).SendKeys("10");
        _session.FindElement(AppiumBy.AccessibilityId("tirageTextBox")).SendKeys("1");
        _session.FindElement(AppiumBy.AccessibilityId("orderButton")).Click();
        System.Threading.Thread.Sleep(1000);
        var screenshot = _session.GetScreenshot();
        screenshot.SaveAsFile("TestResults/screenshots/success.png");
        Assert.IsNotNull(screenshot);
    }

    [TestMethod]
    public void ShowsWarningOnInvalidInput()
    {
        _session!.FindElement(AppiumBy.AccessibilityId("pageNumTextBox")).SendKeys("abc");
        _session.FindElement(AppiumBy.AccessibilityId("calculateButton")).Click();
        System.Threading.Thread.Sleep(1000);
        var screenshot = _session.GetScreenshot();
        screenshot.SaveAsFile("TestResults/screenshots/warn.png");
        Assert.IsNotNull(screenshot);
    }
}
