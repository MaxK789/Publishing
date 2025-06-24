# UI Testing

WinForms screens are tested using WinAppDriver. The tests rely on the `Appium.WebDriver` NuGet package for the Selenium bindings, but the WinAppDriver executable itself is not included. Install it via **winget** or download the MSI from GitHub and start it before running the tests.

```
winget install WinAppDriver
WinAppDriver.exe
```

Enable Windows developer mode and allow access to port **4723** so the driver can accept connections.

Example test checking for a message box:

```csharp
var session = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), caps);
// ... click the Create button
var dialog = session.FindElementByName("Publishing");
Assert.IsNotNull(dialog);
```

These tests run only on Windows agents. In GitHub Actions the `windows-latest` job runs them automatically.

Screenshots are saved to `TestResults/screenshots` and uploaded as workflow artifacts.

The analyzer rule `PUB003` also checks for `#if WINDOWS` directives in test code. Platform-specific logic should be hidden behind abstraction layers instead of conditional compilation.
