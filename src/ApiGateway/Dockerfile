# ------------------------------------------------------------
# 1) Build stage using the .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /repo

# Ensure MSBuild can resolve the RepoRoot property
COPY Directory.Build.props ./
COPY src ./src

RUN dotnet restore "src/ApiGateway/ApiGateway.csproj"

WORKDIR /repo/src/ApiGateway
RUN dotnet publish -c Release -o /app/publish

# ------------------------------------------------------------
# 2) Runtime stage with ASP.NET Core Runtime
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

RUN apt-get update \
 && apt-get install -y curl \
 && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "ApiGateway.dll"]
