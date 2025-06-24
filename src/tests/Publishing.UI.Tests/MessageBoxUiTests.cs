using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium;
using System;


namespace Publishing.UI.Tests;

[TestClass]
[TestCategory("UI")]
public class MessageBoxUiTests
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
    [Ignore("Requires WinAppDriver")]
    public void Login_WithEmptyCredentials_ShowsMessageBox()
    {
        try
        {
            _session!.FindElement(MobileBy.AccessibilityId("loginButton")).Click();
            var dialog = _session.FindElementByName("Publishing");
            Assert.IsNotNull(dialog.FindElementByName("Invalid email or password"));
            dialog.FindElementByName("OK").Click();
        }
        catch
        {
            System.IO.Directory.CreateDirectory("TestResults/screenshots");
            _session?.GetScreenshot().SaveAsFile("TestResults/screenshots/login_error.png");
            throw;
        }
    }
}
