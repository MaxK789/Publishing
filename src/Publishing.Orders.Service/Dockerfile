# ------------------------------------------------------------
# 1) Build stage using the .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# 1.1) Copy only the projects needed for Orders.Service
COPY ["src/Publishing.Core/Publishing.Core.csproj", "Publishing.Core/"]
COPY ["src/Publishing.Infrastructure/Publishing.Infrastructure.csproj", "Publishing.Infrastructure/"]
COPY ["src/Publishing.Application/Publishing.Application.csproj", "Publishing.Application/"]
COPY ["src/Publishing.Orders.Service/Publishing.Orders.Service.csproj", "Publishing.Orders.Service/"]
# 1.2) Restore only Orders.Service (it will pull its ProjectReferences)
RUN dotnet restore "Publishing.Orders.Service/Publishing.Orders.Service.csproj"

# 1.3) Copy rest of source and publish
COPY src/Publishing.Orders.Service/. "Publishing.Orders.Service/"
WORKDIR "/src/Publishing.Orders.Service"
RUN dotnet publish "Publishing.Orders.Service.csproj" -c Release -o /app/publish

# ------------------------------------------------------------
# 2) Runtime stage with ASP.NET Core Runtime
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Publishing.Orders.Service.dll"]
