# ------------------------------------------------------------
# 1) Build stage using the .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# 1.1) Copy solution and all csproj for restore
COPY ["Publishing.sln", "."]
COPY ["src/ApiGateway/ApiGateway.csproj", "src/ApiGateway/"]
RUN dotnet restore "Publishing.sln"

# 1.2) Copy the remaining source and publish
COPY . .
WORKDIR "/src/src/ApiGateway"
RUN dotnet publish "ApiGateway.csproj" -c Release -o /app/publish

# ------------------------------------------------------------
# 2) Runtime stage with ASP.NET Core Runtime
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "ApiGateway.dll"]
