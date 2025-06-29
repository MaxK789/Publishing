# Changelog

## [1.1.0] - 2025-06-24
### Added
- Analyzer rule `PUB002` forbids calls to `MessageBox.Show` and `NotifyIcon.ShowBalloonTip`.
- Analyzer rule `PUB003` detects `#if WINDOWS` directives.

## [1.2.0] - 2025-06-26
### Added
- Consul registration now supports health checks, tags and metadata and runs asynchronously.
- RabbitMQ queues are durable and include the `traceparent` header.
- Redis now runs with a replica for high availability.
- Order saga tracks created order IDs to perform targeted compensation.

### Removed
- Legacy `PUB001` rule and tray balloon notifier implementation.

## [1.2.1] - 2025-06-26
### Fixed
- Docker builds copy the full `src/` tree so `Publishing.Analyzers` resolves
- Prometheus exporter downgraded to `1.6.0-rc.1` for .NET 6 compatibility
