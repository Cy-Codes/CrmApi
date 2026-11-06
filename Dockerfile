# ============================================================================
# CrmApiCSharp - Cloud-Agnostic Container Image
# The same image runs unchanged on local Docker, AWS ECS (Fargate) and
# Azure Container Apps. All environment-specific settings are injected at
# runtime through environment variables (see .env.example).
# ============================================================================
ARG DOTNET_VERSION=10.0

# Stage 1: Restore & publish (project files first to maximize layer caching)
FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS build
WORKDIR /src

COPY .editorconfig Directory.Build.props CrmApiCSharp.slnx ./
COPY src/Api/Api.csproj src/Api/
COPY src/Application/Application.csproj src/Application/
COPY src/Domain/Domain.csproj src/Domain/
COPY src/Infrastructure/Infrastructure.csproj src/Infrastructure/
RUN dotnet restore src/Api/Api.csproj

COPY src/ src/
RUN dotnet publish src/Api/Api.csproj \
        --configuration Release \
        --no-restore \
        --output /app/publish \
        /p:UseAppHost=false

# Stage 2: Minimal ASP.NET Core runtime
FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION} AS runtime
WORKDIR /app

ENV ASPNETCORE_HTTP_PORTS=8080 \
    ASPNETCORE_ENVIRONMENT=Production \
    DOTNET_RUNNING_IN_CONTAINER=true

COPY --from=build /app/publish .

EXPOSE 8080
USER $APP_UID

ENTRYPOINT ["dotnet", "Api.dll"]
