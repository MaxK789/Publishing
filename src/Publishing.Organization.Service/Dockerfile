# ------------------------------------------------------------
# 1) Build stage using the .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# 1.1) Copy dependent projects (code + csproj)
COPY src/Publishing.Core/.           "Publishing.Core/"
COPY src/Publishing.Infrastructure/. "Publishing.Infrastructure/"
COPY src/Publishing.Application/.    "Publishing.Application/"

# 1.2) Copy and restore Organization.Service itself
COPY src/Publishing.Organization.Service/. "Publishing.Organization.Service/"
RUN dotnet restore "Publishing.Organization.Service/Publishing.Organization.Service.csproj"

# 1.3) Publish
WORKDIR "/src/Publishing.Organization.Service"
RUN dotnet publish "Publishing.Organization.Service.csproj" -c Release -o /app/publish

# ------------------------------------------------------------
# 2) Runtime stage with ASP.NET Core Runtime
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Publishing.Organization.Service.dll"]
