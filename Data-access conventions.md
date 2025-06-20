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

## Integration testing

Integration tests spin up a SQL Server container with Testcontainers and execute the query objects against a real database to validate SQL syntax.
