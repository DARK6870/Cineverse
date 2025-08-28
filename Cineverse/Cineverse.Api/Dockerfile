# ---------------- Base stage ----------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 7404

# ---------------- Build stage ----------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
ARG BUILD_CONFIGURATION=Release

COPY ["Cineverse.Api/Cineverse.Api.csproj", "Cineverse.Api/"]
COPY ["Cineverse.Application/Cineverse.Application.csproj", "Cineverse.Application/"]
COPY ["Cineverse.Infrastructure/Cineverse.Infrastructure.csproj", "Cineverse.Infrastructure/"]
COPY ["Cineverse.Mongo/Cineverse.Mongo.csproj", "Cineverse.Mongo/"]
COPY ["Cineverse.Mongo.Migrations/Cineverse.Mongo.Migrations.csproj", "Cineverse.Mongo.Migrations/"]
COPY ["Cineverse.Notifications/Cineverse.Notifications.csproj", "Cineverse.Notifications/"]

COPY Directory.Packages.props Directory.Build.props ./

RUN dotnet restore "Cineverse.Api/Cineverse.Api.csproj"

COPY . .

WORKDIR "/src/Cineverse.Api"

RUN dotnet publish "./Cineverse.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# ---------------- Final stage ----------------
FROM base AS final
WORKDIR /app

COPY --from=build /app/publish .
COPY NotificationTemplates /app/NotificationTemplates

RUN ls -la /app

ENTRYPOINT ["dotnet", "Cineverse.Api.dll"]
