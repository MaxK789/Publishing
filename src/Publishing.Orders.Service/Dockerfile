# ------------------------------------------------------------
# 1) Build stage using the .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# 1.1) Copy the project file and restore dependencies
COPY ["src/Publishing.Orders.Service/Publishing.Orders.Service.csproj", "Publishing.Orders.Service/"]
RUN dotnet restore "Publishing.Orders.Service/Publishing.Orders.Service.csproj"

# 1.2) Copy the remaining source and publish
COPY . .
WORKDIR "/src/Publishing.Orders.Service"
RUN dotnet publish "Publishing.Orders.Service.csproj" -c Release -o /app/publish

# ------------------------------------------------------------
# 2) Runtime stage with ASP.NET Core Runtime
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Publishing.Orders.Service.dll"]
