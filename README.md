# Publishing

This repository hosts a simple publishing workflow demo. Database schema is maintained with Entity Framework Core migrations.

## Configuration

The `Publishing.UI` project contains an `appsettings.json` file with a default connection string pointing to LocalDB.  The project file copies this file to the output directory so both the application and `dotnet ef` commands can use it automatically. During startup a dedicated initializer applies pending EF Core migrations so the schema stays in sync with the models.

All EF Core migration files must be compiled so `Database.MigrateAsync()` can locate them during integration tests. If the EF tools added them as `None` items, ensure they are built with:

```xml
<ItemGroup>
  <Compile Update="Migrations\**\*.cs" />
</ItemGroup>
```

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

A GitHub Actions workflow under `.github/workflows/ci.yml` demonstrates how to build the solution, run tests and apply migrations automatically during CI.
The workflow installs the EF Core CLI tool and runs on Windows runners so that SQL
Server LocalDB is available.

## Price calculation and discounts

Orders use a `PriceCalculator` service to determine the final cost. This calculator resolves an `IDiscountPolicy` from the DI container so different promotions can adjust the base price. Register the default policy in `Program.cs`:

```csharp
services.AddScoped<IDiscountPolicy, StandardDiscountPolicy>();
```

New strategies can be provided by registering a different implementation without touching `PriceCalculator`.
Components should obtain `PriceCalculator` via dependency injection rather than constructing it directly.

