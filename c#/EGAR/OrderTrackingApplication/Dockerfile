FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/OrderTrackingApplication.Domain/OrderTrackingApplication.Domain.csproj src/OrderTrackingApplication.Domain/
COPY src/OrderTrackingApplication.Application/OrderTrackingApplication.Application.csproj src/OrderTrackingApplication.Application/
COPY src/OrderTrackingApplication.Infrastructure/OrderTrackingApplication.Infrastructure.csproj src/OrderTrackingApplication.Infrastructure/
COPY src/OrderTrackingApplication.Api/OrderTrackingApplication.Api.csproj src/OrderTrackingApplication.Api/

RUN dotnet restore src/OrderTrackingApplication.Api/OrderTrackingApplication.Api.csproj

COPY src/OrderTrackingApplication.Domain/ src/OrderTrackingApplication.Domain/
COPY src/OrderTrackingApplication.Application/ src/OrderTrackingApplication.Application/
COPY src/OrderTrackingApplication.Infrastructure/ src/OrderTrackingApplication.Infrastructure/
COPY src/OrderTrackingApplication.Api/ src/OrderTrackingApplication.Api/

RUN dotnet publish src/OrderTrackingApplication.Api/OrderTrackingApplication.Api.csproj \
    -c Release \
    -o /app/publish \
    --no-restore \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

RUN apt-get update \
    && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/*

RUN mkdir -p /app/logs \
    && chown -R app:app /app

USER app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

HEALTHCHECK --interval=30s --timeout=5s --start-period=30s --retries=3 \
    CMD curl -f http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "OrderTrackingApplication.Api.dll"]
