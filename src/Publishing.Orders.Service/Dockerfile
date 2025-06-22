# ------------------------------------------------------------
# 1) Build stage using the .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# 1.1) Copy dependent projects (code + csproj)
COPY src/Publishing.Core/.           "Publishing.Core/"
COPY src/Publishing.Infrastructure/. "Publishing.Infrastructure/"
COPY src/Publishing.Application/.    "Publishing.Application/"

# 1.2) Copy and restore Orders.Service itself
COPY src/Publishing.Orders.Service/. "Publishing.Orders.Service/"
RUN dotnet restore "Publishing.Orders.Service/Publishing.Orders.Service.csproj"

# 1.3) Publish
WORKDIR "/src/Publishing.Orders.Service"
RUN dotnet publish "Publishing.Orders.Service.csproj" -c Release -o /app/publish

# ------------------------------------------------------------
# 2) Runtime stage with ASP.NET Core Runtime
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

RUN apt-get update \
 && apt-get install -y curl \
 && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Publishing.Orders.Service.dll"]
