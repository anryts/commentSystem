# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS with-node
RUN apt-get update
RUN apt-get install -y curl sqlite3 libsqlite3-dev
RUN curl -sL https://deb.nodesource.com/setup_20.x | bash
RUN apt-get -y install nodejs
RUN npm install -g @angular/cli

FROM with-node AS build
ARG BUILD_CONFIGURATION=Debug
WORKDIR /src
COPY ["CommentarySystem.Server/CommentarySystem.Server.csproj", "CommentarySystem.Server/"]
COPY ["commentarysystem.client/commentarysystem.client.esproj", "commentarysystem.client/"]
RUN dotnet restore "./CommentarySystem.Server/CommentarySystem.Server.csproj"
COPY . .
WORKDIR "/src/CommentarySystem.Server"
RUN dotnet build "./CommentarySystem.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Debug
RUN dotnet publish "./CommentarySystem.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app

# Ensure the SQLite database file is copied and accessible
COPY --chown=app:app ["CommentarySystem.Server/sqlite.db", "/app/sqlite.db"]

COPY --from=publish /app/publish .

# Set appropriate permissions for the SQLite database
RUN chown app:app /app/sqlite.db

ENTRYPOINT ["dotnet", "CommentarySystem.Server.dll"]
