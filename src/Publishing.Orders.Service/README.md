# Publishing Orders Service

This service exposes order management HTTP APIs.

## Running locally

```bash
dotnet run --project Publishing.Orders.Service.csproj
```

Swagger UI is available at `http://localhost:5001/swagger` when running via Docker Compose.
The service applies EF Core migrations automatically at startup. Requests are cached in Redis, traced with OpenTelemetry and protected by JWT bearer authentication. Use the Swagger **Authorize** button to send a JWT when exploring endpoints.
