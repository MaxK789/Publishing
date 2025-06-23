# UI Testing

WinForms screens are tested using WinAppDriver. Install the driver from the official site and start it before running the tests.

```powershell
WinAppDriver.exe
```

Example test checking for a balloon tip:

```csharp
var session = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), caps);
// ... click the Create button
var notification = session.FindElementByName("Publishing");
Assert.IsNotNull(notification);
```

These tests run only on Windows agents. In GitHub Actions the `windows-latest` job runs them automatically.

Screenshots are saved to `TestResults/screenshots` and uploaded as workflow artifacts. Set `NOTIFICATIONS_DISABLED=false` when debugging locally so balloon tips appear.
