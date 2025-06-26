# Publishing
![CI](https://github.com/example/publishing/actions/workflows/ci.yml/badge.svg)

This repository hosts a simple publishing workflow demo. Database schema is maintained with Entity Framework Core migrations.

## 🚀 Quick Start

### Prerequisites

* Docker Desktop with at least **4 GB RAM**
* [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download)
* [Git 2.34](https://git-scm.com/) or newer

For Windows Home users follow [Microsoft's guide](https://learn.microsoft.com/en-us/visualstudio/containers/docker-desktop) to enable Linux containers.

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

A GitHub Actions workflow under `.github/workflows/ci.yml` demonstrates how to build the solution and run tests automatically during CI.  The pipeline runs on Linux and Windows, publishing a NuGet package with custom analyzers.  Code coverage fails the build if it drops below **85%**.

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

## UI Notifications

User-facing messages are routed through `IUiNotifier`. Windows clients now show
standard message boxes while console services write the text to `stdout`.
When the application runs without a graphical environment, only log entries are
produced.

Register the notifier directly:

```csharp
var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<IUiNotifier, MessageBoxNotifier>();
    })
    .Build();
```

Call the appropriate method depending on context:

| Method | Usage |
| --- | --- |
| `NotifyInfo` | Successful actions |
| `NotifyWarning` | Validation or minor issues |
| `NotifyError` | Exceptions and failures |

In unit tests `IUiNotifier` can be replaced with a stub:

```csharp
var factory = new WebApplicationFactory<Program>()
    .WithWebHostBuilder(b =>
    {
        b.ConfigureTestServices(services =>
        {
            services.AddSingleton<IUiNotifier, StubNotifier>();
        });
    });
```


## Static Analysis

Custom Roslyn analyzers enforce the notifier registration and forbid APIs like `MessageBox.Show` or `NotifyIcon.ShowBalloonTip`. The `ForbiddenApiAnalyzer` (rule ID `PUB002`) triggers errors when these calls appear outside the `MessageBoxNotifier` class. Run `dotnet format analyzers` to apply automatic fixes before committing.

Additionally rule `PUB003` forbids `#if WINDOWS` conditional directives. Instead of platform checks wrap Windows-specific code behind abstractions so it can be tested on other platforms. The following snippet would trigger the rule:

```csharp
#if WINDOWS
var path = "c:/temp";
#endif
```

## Localization & Accessibility

Notification texts reside in `Notifications.resx` with Ukrainian translations in `Notifications.uk-UA.resx`. Create new culture-specific files to extend the UI. Access strings via `ResourceManager`. Tests ensure translations exist for every key.


## Microservices setup

Each service resides in `src/Publishing.<Name>.Service` with its own `Dockerfile`.
Start the whole stack using:

```bash
docker-compose up --build
```

The API gateway project under `src/ApiGateway` routes requests to the services. Swagger is enabled for each service and the gateway itself at `/swagger`. Health checks are exposed at `/health`. The compose stack also launches **Elasticsearch**, **Kibana**, **Jaeger** and **Prometheus** for centralized logging and tracing.
Copy `.env.example` to `.env` and adjust the connection strings and JWT settings before starting the stack. Required variables are `SA_PASSWORD`, `ACCEPT_EULA`, `ORDERS_DB_CONN`, `PROFILE_DB_CONN`, `ORGANIZATION_DB_CONN`, `REDIS_CONN`, `RABBIT_CONN`, `CONSUL_URL`, `OIDC_AUTHORITY`, `OIDC_AUDIENCE`, `ELASTIC_URL`, `JWT__Issuer`, `JWT__Audience` and `JWT__SigningKey`.
Elasticsearch runs on `http://localhost:9200` with Kibana at `http://localhost:5601`. Jaeger UI is available at `http://localhost:16686` and Prometheus at `http://localhost:9090` when using the compose stack.
See `docs/kibana.md` for hints on visualising logs with Kibana.
Keycloak is used as an OAuth2 provider. Import the realm configuration under `keycloak/realm-export.json` with:

```bash
docker exec keycloak /opt/keycloak/bin/kc.sh import --file=/config/realm-export.json
```

The compose file already mounts this configuration so the command runs once at start up. After importing, obtain a token by visiting `http://localhost:8080/realms/publishing/protocol/openid-connect/token`.
The signing key must be at least 32 characters long when using the default HS256 algorithm. The issuer, audience and key must match the values used to sign JWT tokens consumed by the services.
Set `ASPNETCORE_ENVIRONMENT=Development` in the compose file (or `.env`) to enable Swagger inside the containers. Change or remove this variable to run the services in production.
Database migrations now apply **asynchronously** during startup and each service reports readiness via `/health`.
All API routes require an `Authorization: Bearer <token>` header containing a JWT signed with the configured key.
When browsing Swagger, use the **Authorize** button to provide a token for authenticated requests.
Persistent volumes `db-data` and `redis-data` preserve SQL Server and Redis data between restarts. The services automatically apply EF Core migrations using their respective connection strings (`ORDERS_DB_CONN`, `PROFILE_DB_CONN` and `ORGANIZATION_DB_CONN`). All containers join the `micro-net` Docker network so the gateway can resolve service names. Swagger can also be reached through the gateway under `/orders/swagger`, `/profile/swagger` and `/organization/swagger`.
A replica container `cache-replica` follows the master for higher availability.
The gateway exposes an aggregation endpoint at `/api/aggregation/person/{id}` returning combined order, profile and organization data. Prometheus metrics from each service are available at `/metrics`. The profile and organization services now expose full CRUD APIs via `/api/profile` and `/api/org`.
RabbitMQ runs as `rabbit` on port `5672` for publishing order events used by statistics.

Services now register with Consul using HTTP health checks. Optional tags can be provided via `SERVICE_TAGS` and key-value metadata via `SERVICE_META`. RabbitMQ queues are created as durable and include the `traceparent` header so traces flow through message handling.
The `OrderSaga` stores the created order's ID and deletes that specific row on compensation instead of removing the latest order for a person.

Each service enables Redis caching via `REDIS_CONN`, OpenTelemetry tracing with Jaeger and Prometheus exporters and logs via Serilog to Elasticsearch. Services register with Consul for discovery and secure endpoints using OAuth2 bearer authentication. Contract tests live under `src/tests/Publishing.Contracts.Tests` and verify API compatibility using Pact. These tests run automatically via the `contracts.yml` GitHub Actions workflow.
Kubernetes manifests for all components, including the database, Redis, RabbitMQ, Jaeger, Prometheus and a gateway ingress, can be found under the `k8s/` directory.
The ingress relies on cert-manager to automatically provision TLS certificates. Adjust the host name in `gateway-ingress.yaml` and ensure a cluster issuer is configured.
For easier deployment there is also a Helm chart in `helm/publishing` covering the gateway, orders, profile and organization services. Customize the values file and install with `helm install publishing helm/publishing`.
Example overrides can be provided with `helm install -f my-values.yaml publishing helm/publishing` where `my-values.yaml` defines new image tags or secret references:
```yaml
orders:
  image: myrepo/orders:latest
gateway:
  oidcAuthority: https://auth.example.com/realms/publishing
```
Grafana dashboards are located under `docs/grafana` and can be imported to visualise metrics stored in Prometheus.
Alert rules for Prometheus reside in `prometheus-alerts.yml` and can be imported into Grafana to trigger notifications when error rates spike.
See [docs/secrets.md](docs/secrets.md) for guidance on managing sensitive configuration with Vault.

## CI/CD

Several additional GitHub workflows build Docker images for the services and push
them to Docker Hub. To allow these workflows to authenticate, create repository
secrets named `DOCKER_USERNAME` and `DOCKER_PASSWORD` containing your Docker Hub
credentials. Without these secrets the `Login & Push` steps will fail with an
"incorrect username or password" error.

For details on running UI tests see [docs/ui-testing.md](docs/ui-testing.md).
The Windows job in CI executes these tests with WinAppDriver. When testing locally install WinAppDriver via winget or the MSI package as it is not included with the NuGet package.

### Publishing new analyzer version

Custom analyzers live in `src/Publishing.Analyzers`. Create a new NuGet package with:

```bash
dotnet pack src/Publishing.Analyzers/Publishing.Analyzers.csproj -c Release -o ./artifacts
```

Then push the generated `.nupkg` to your feed:

```bash
dotnet nuget push artifacts/*.nupkg --source <url> --api-key <key>
```

The `pack-analyzers` job in CI runs the same commands and publishes packages automatically on merges to `main`.
To consume the package in other projects reference it as:

```xml
<PackageReference Include="Publishing.Analyzers" Version="1.1.0" PrivateAssets="all" />
```

## Load testing

Sample JMeter plans reside in the `load-tests` directory. Run `jmeter -n -t gateway.jmx` to stress the aggregation endpoint through the gateway. Other scripts cover individual services.
