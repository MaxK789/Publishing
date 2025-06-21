# Publishing Profile Service

This service handles user profile operations.

## Running locally

```bash
dotnet run --project Publishing.Profile.Service.csproj
```

Swagger UI is available at `http://localhost:5002/swagger` when running via Docker Compose.
The service applies EF Core migrations automatically at startup and uses Redis caching, OpenTelemetry tracing and JWT bearer authentication. Use the Swagger **Authorize** button to test secured endpoints.
