# Stage 1: Base image for running the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
USER $APP_UID
EXPOSE 8080
EXPOSE 8081

# Stage 2: Build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy only the project file and restore early for better caching
COPY SCIC_BE.csproj .
RUN dotnet restore SCIC_BE.csproj

# Copy everything else and build
COPY . .
RUN dotnet build SCIC_BE.csproj -c $BUILD_CONFIGURATION -o /app/build

# Stage 3: Publish the app
FROM build AS publish
RUN dotnet publish SCIC_BE.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage 4: Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set the entry point
ENTRYPOINT ["dotnet", "SCIC_BE.dll"]
