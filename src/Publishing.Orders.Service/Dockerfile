# ------------------------------------------------------------
# 1) Build stage using the .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# 1.1) Copy solution and all csproj for restore
COPY ["Publishing.sln", "."]
COPY ["src/Publishing.Core/Publishing.Core.csproj", "src/Publishing.Core/"]
COPY ["src/Publishing.Infrastructure/Publishing.Infrastructure.csproj", "src/Publishing.Infrastructure/"]
COPY ["src/Publishing.Application/Publishing.Application.csproj", "src/Publishing.Application/"]
COPY ["src/Publishing.Orders.Service/Publishing.Orders.Service.csproj", "src/Publishing.Orders.Service/"]
RUN dotnet restore "Publishing.sln"

# 1.2) Copy the remaining source and publish
COPY . .
WORKDIR "/src/src/Publishing.Orders.Service"
RUN dotnet publish "Publishing.Orders.Service.csproj" -c Release -o /app/publish

# ------------------------------------------------------------
# 2) Runtime stage with ASP.NET Core Runtime
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Publishing.Orders.Service.dll"]
