# Publishing

This repository hosts a simple publishing workflow demo. Database schema is maintained with Entity Framework Core migrations.

## Configuration

The `Publishing.UI` project contains an `appsettings.json` file with a default connection string pointing to LocalDB.  The project file copies this file to the output directory so both the application and `dotnet ef` commands can use it automatically. During startup a dedicated initializer applies pending EF Core migrations so the schema stays in sync with the models.

See [Data-access conventions](Data-access conventions.md) for details on the Query Object pattern used for database access.

All EF Core migration files must be compiled so `Database.MigrateAsync()` can locate them during integration tests. If the EF tools added them as `None` items, ensure they are built with:

```xml
<ItemGroup>
  <Compile Update="Migrations\**\*.cs" />
</ItemGroup>
```

### Custom Connection String

`src/Publishing.UI/appsettings.json` defines `ConnectionStrings:DefaultConnection`. Update this value to point to a different SQL Server instance. The default connection string now includes `TrustServerCertificate=True` to suppress SSL certificate warnings when using a self-signed SQL Server certificate:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=My-PC;Initial Catalog=Publishing;Integrated Security=true;TrustServerCertificate=True"
  }
}
```

`Program.cs` also loads environment variables, so you can override the setting at runtime with `ConnectionStrings__DefaultConnection`.

## Working with migrations

1. Install the EF Core tools if needed:

```bash
 dotnet tool install --global dotnet-ef --version 6.0.*
```

2. Add a new migration from the `Publishing.Infrastructure` project directory. For the first migration, name it `InitialCreate`:

```bash
 dotnet ef migrations add <MigrationName> --project src/Publishing.Infrastructure --output-dir Migrations
```

3. Apply migrations to the configured database:

```bash
 dotnet ef database update --project src/Publishing.Infrastructure
```

The `DesignTimeDbContextFactory` allows the EF CLI to resolve `AppDbContext` without requiring the UI project.

In CI pipelines you can apply migrations automatically using:

```bash
dotnet ef database update --project src/Publishing.Infrastructure
```

A GitHub Actions workflow under `.github/workflows/ci.yml` demonstrates how to build the solution and run tests automatically during CI.
The job now runs on Linux and uses a temporary SQLite database so no SQL Server instance is required.

## Testing

* **Unit tests** run with the EF Core InMemory provider.
* **Integration tests** use SQLite files for speed. SQLite's SQL dialect differs
  from SQL Server, so passing tests here should be treated as a quick sanity
  check rather than proof that all queries work with SQL Server.
* A few tests run against SQL Server via Testcontainers when T-SQL features are involved.

## Price calculation and discounts

Orders use a `PriceCalculator` service to determine the final cost. This calculator resolves an `IDiscountPolicy` from the DI container so different promotions can adjust the base price. Register the default policy in `Program.cs`:

```csharp
services.AddScoped<IDiscountPolicy, StandardDiscountPolicy>();
```

New strategies can be provided by registering a different implementation without touching `PriceCalculator`.
Components should obtain `PriceCalculator` via dependency injection rather than constructing it directly.


## Microservices setup

Each service resides in `src/Publishing.<Name>.Service` with its own `Dockerfile`.
Start the whole stack using:

```bash
docker-compose up --build
```

The API gateway project under `src/ApiGateway` routes requests to the services. Swagger is enabled for each service at `/swagger` and health checks are exposed at `/health`.
Copy `.env.example` to `.env` and adjust the connection strings and JWT settings before starting the stack. Required variables are `SA_PASSWORD`, `ACCEPT_EULA`, `DB_CONN`, `REDIS_CONN`, `JWT__Issuer`, `JWT__Audience` and `JWT__SigningKey`.
The issuer, audience and signing key must match the values used to sign JWT tokens consumed by the services.
Set `ASPNETCORE_ENVIRONMENT=Development` in the compose file (or `.env`) to enable Swagger inside the containers. Change or remove this variable to run the services in production.
Database migrations now apply **asynchronously** during startup and each service reports readiness via `/health`.
All API routes require an `Authorization: Bearer <token>` header containing a JWT signed with the configured key.
When browsing Swagger, use the **Authorize** button to provide a token for authenticated requests.
Persistent volumes `db-data` and `redis-data` preserve SQL Server and Redis data between restarts. The services automatically apply EF Core migrations using the `DB_CONN` connection string. All containers join the `micro-net` Docker network so the gateway can resolve service names. Swagger can also be reached through the gateway under `/orders/swagger`, `/profile/swagger` and `/organization/swagger`.

Each service enables Redis caching via `REDIS_CONN`, OpenTelemetry tracing with a console exporter and secures endpoints using CORS and JWT bearer authentication. Contract tests live under `src/tests/Publishing.Contracts.Tests` and verify API compatibility using Pact. These tests run automatically via the `contracts.yml` GitHub Actions workflow.

## CI/CD

Several additional GitHub workflows build Docker images for the services and push
them to Docker Hub. To allow these workflows to authenticate, create repository
secrets named `DOCKER_USERNAME` and `DOCKER_PASSWORD` containing your Docker Hub
credentials. Without these secrets the `Login & Push` steps will fail with an
"incorrect username or password" error.
