# ------------------------------------------------------------
# 1) Build stage using the .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# 1.1) Copy only the project needed for ApiGateway
COPY ["src/ApiGateway/ApiGateway.csproj", "ApiGateway/"]
# 1.2) Restore only ApiGateway
RUN dotnet restore "ApiGateway/ApiGateway.csproj"

# 1.3) Copy the remaining source and publish
COPY src/ApiGateway/. "ApiGateway/"
WORKDIR "/src/ApiGateway"
RUN dotnet publish "ApiGateway.csproj" -c Release -o /app/publish

# ------------------------------------------------------------
# 2) Runtime stage with ASP.NET Core Runtime
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "ApiGateway.dll"]
