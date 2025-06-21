# API Gateway

This gateway routes requests to the underlying services using Ocelot.

## Running locally

```bash
dotnet run --project ApiGateway.csproj
```

The gateway exposes a health endpoint at `/health`. Swagger for each service is reachable via `/orders/swagger`, `/profile/swagger` and `/organization/swagger`.
Requests are cached with Redis, traced via OpenTelemetry and secured using JWT bearer authentication.
