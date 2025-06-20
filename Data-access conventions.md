# Data access conventions

This document describes how query objects are implemented in this repository.

## Query objects

Every database query is represented by a class derived from `SqlQuery<T>` or `SqlCommand`. A query contains the SQL string, optional parameter object and a `Map` method for materialising a result row. Commands only provide the SQL and parameters.

Example query:

```csharp
public sealed class GetActiveOrdersQuery : SqlQuery<Order>
{
    public override string Sql => "SELECT * FROM Orders";
    public override Order Map(IDataReader reader) => new Order { /* mapping */ };
}
```

Unit tests should verify that the SQL string of a query matches the expected statement.

## Dispatchers

`IQueryDispatcher` and `ICommandDispatcher` execute query objects using `IDbConnectionFactory`. They also emit OpenTelemetry activities and log executed SQL statements.

## Logging and caching

`LoggingDbConnection` wraps a real `DbConnection` and logs SQL text and execution time via `ILogger`. For rarely changing reference data queries a `MemoryCacheQueryDispatcher` decorator can be used:

```csharp
var cached = new MemoryCacheQueryDispatcher(inner, cache, TimeSpan.FromMinutes(10));
```
The cache duration depends on how frequently the underlying data changes. A short
TTL of five to ten minutes is usually enough for reference tables that rarely
change.

## Integration testing

Most integration tests run against a temporary SQLite database file. This avoids
starting a SQL Server container and keeps the execution time under a few
seconds. SQLite is fast but does not implement the full SQL Server dialect, so
passing tests here do not guarantee your queries will behave identically under
SQL Server. For scenarios that rely on SQL Server–specific features (like
`SCOPE_IDENTITY()` or `GETDATE()`), add separate tests that use Testcontainers
to spin up a real SQL Server instance.

## Testing strategy

* **Unit tests** use the EF Core InMemory provider to validate repository logic
  without touching a real database.
* **Integration tests** rely on SQLite files to exercise migrations and query
  objects quickly. Tests requiring SQL Server syntax should use Testcontainers.
* **Smoke tests** run a minimal set of queries against either the in-memory
  provider or SQLite to verify the application starts correctly before a full
  test pass.
