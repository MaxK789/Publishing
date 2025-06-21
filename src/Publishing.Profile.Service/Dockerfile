# ------------------------------------------------------------
# 1) Build stage using the .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# 1.1) Copy the project file and restore dependencies
COPY ["src/Publishing.Profile.Service/Publishing.Profile.Service.csproj", "Publishing.Profile.Service/"]
RUN dotnet restore "Publishing.Profile.Service/Publishing.Profile.Service.csproj"

# 1.2) Copy the rest of the source and publish
COPY . .
WORKDIR "/src/Publishing.Profile.Service"
RUN dotnet publish "Publishing.Profile.Service.csproj" -c Release -o /app/publish

# ------------------------------------------------------------
# 2) Runtime stage with ASP.NET Core Runtime
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Publishing.Profile.Service.dll"]
