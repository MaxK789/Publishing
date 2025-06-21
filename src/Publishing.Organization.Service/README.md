# Publishing Organization Service

This service manages organization data.

## Running locally

```bash
dotnet run --project Publishing.Organization.Service.csproj
```

Swagger UI is available at `http://localhost:5003/swagger` when running via Docker Compose.
The service applies EF Core migrations automatically at startup. Redis caching, OpenTelemetry tracing and JWT bearer authentication are enabled by default. Use Swagger's **Authorize** button to supply a JWT when trying endpoints.
