# Changelog

## [1.1.0] - 2025-06-24
### Added
- Analyzer rule `PUB002` forbids calls to `MessageBox.Show` and `NotifyIcon.ShowBalloonTip`.
- Analyzer rule `PUB003` detects `#if WINDOWS` directives.

### Removed
- Legacy `PUB001` rule and tray balloon notifier implementation.
