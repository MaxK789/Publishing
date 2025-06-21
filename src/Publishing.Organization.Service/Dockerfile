# ------------------------------------------------------------
# 1) Build stage using the .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# 1.1) Copy the project file and restore dependencies
COPY ["src/Publishing.Organization.Service/Publishing.Organization.Service.csproj", "Publishing.Organization.Service/"]
RUN dotnet restore "Publishing.Organization.Service/Publishing.Organization.Service.csproj"

# 1.2) Copy the rest of the source and publish
COPY . .
WORKDIR "/src/Publishing.Organization.Service"
RUN dotnet publish "Publishing.Organization.Service.csproj" -c Release -o /app/publish

# ------------------------------------------------------------
# 2) Runtime stage with ASP.NET Core Runtime
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Publishing.Organization.Service.dll"]
