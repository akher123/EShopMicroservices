# EShopMicroservices

A sample e-commerce application built with **.NET microservices**, demonstrating distributed architecture patterns including API gateway routing, gRPC inter-service communication, event-driven messaging, centralized structured logging, and containerized deployment.

## Technology Summary

### Core Platform

| Technology | Version / Notes |
|---|---|
| **.NET** | 10.0 (`net10.0`) |
| **ASP.NET Core** | Web APIs, Razor Pages UI, gRPC services |
| **C#** | Nullable reference types enabled |

### Architecture & Patterns

| Pattern / Style | Where Used |
|---|---|
| **Microservices** | Catalog, Basket, Discount, Ordering |
| **Clean Architecture / CQRS** | Ordering service (Domain, Application, Infrastructure, Api) |
| **Vertical Slice Architecture** | Catalog and Basket APIs (feature folders + Carter modules) |
| **Event-Driven Architecture** | Basket checkout → RabbitMQ → Ordering integration |
| **Domain-Driven Design (DDD)** | Ordering domain events and aggregates |
| **API Gateway** | YARP reverse proxy with rate limiting |
| **Building Blocks** | Shared libraries for cross-cutting concerns, logging, and messaging |

### Services

| Service | Type | Key Technologies |
|---|---|---|
| **Catalog.Api** | REST API | Carter, MediatR, FluentValidation, Marten, PostgreSQL |
| **Basket.Api** | REST API | Carter, MediatR, Marten, PostgreSQL, Redis cache, gRPC client, MassTransit |
| **Discount.Grpc** | gRPC service | ASP.NET Core gRPC, EF Core, SQLite, Mapster |
| **Ordering.Api** | REST API | Carter, MediatR, EF Core, SQL Server, MassTransit, Feature Management |
| **ApiGateway** | Reverse proxy | YARP, ASP.NET Core rate limiting |
| **Shopping.Web** | Web UI | ASP.NET Core Razor Pages, Refit HTTP clients |

### Libraries & Frameworks

| Library | Purpose |
|---|---|
| **Carter** | Minimal-API-style endpoint modules |
| **MediatR** | CQRS command/query and domain event dispatch |
| **FluentValidation** | Request and command validation |
| **Mapster** | Object mapping |
| **Marten** | Document database access over PostgreSQL (Catalog, Basket) |
| **Entity Framework Core** | ORM for SQL Server (Ordering) and SQLite (Discount) |
| **MassTransit** | Message bus abstraction over RabbitMQ |
| **YARP** | Yet Another Reverse Proxy for API gateway |
| **Refit** | Typed REST client for the Shopping web app |
| **Grpc.AspNetCore** | gRPC server and client communication |
| **Scrutor** | Assembly scanning and DI registration (Basket) |
| **Microsoft.FeatureManagement** | Feature flags (Ordering) |
| **AspNetCore.HealthChecks** | Health check endpoints for PostgreSQL, Redis, SQL Server |
| **Serilog** | Structured logging across all host services |
| **Elastic.Serilog.Sinks** | ECS-formatted logs shipped to Elasticsearch 8.x data streams |
| **Elasticsearch / Kibana** | Centralized log storage and visualization |

### Observability & Logging

All host services use the shared **BuildingBlocks.Logging** library for consistent structured logging via **Serilog**, with logs shipped to **Elasticsearch** and viewed in **Kibana**.

| Component | Role |
|---|---|
| **BuildingBlocks.Logging** | Shared Serilog setup, enrichers, console + Elasticsearch sinks |
| **Serilog.AspNetCore** | Replaces default ASP.NET Core logging; HTTP request logging |
| **Elastic.Serilog.Sinks** | Official Elastic sink (ECS format, data streams) |
| **Elasticsearch 8.15** | Log storage (single-node, security disabled for local dev) |
| **Kibana 8.15** | Log search, filtering, and dashboards |

Each service writes to its own ECS data stream:

| Service | Data stream |
|---|---|
| Catalog.Api | `logs-catalog-api-default` |
| Basket.Api | `logs-basket-api-default` |
| Discount.Grpc | `logs-discount-grpc-default` |
| Ordering.Api | `logs-ordering-api-default` |
| ApiGateway | `logs-apigateway-default` |
| Shopping.Web | `logs-shopping-web-default` |

Logs are enriched with `service.name`, machine name, and environment name. Filter in Kibana by `service.name` (e.g. `catalog-api`, `basket-api`).

**Configuration** (in each service's `appsettings.json`):

```json
{
  "ElasticConfiguration": {
    "Uri": "http://localhost:9200"
  },
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "Microsoft.AspNetCore": "Warning",
        "System": "Warning"
      }
    }
  }
}
```

When running in Docker Compose, `ElasticConfiguration__Uri=http://elasticsearch:9200` is set via environment variables.

**Wiring in `Program.cs`** (via `BuildingBlocks.Logging.Serilog`):

```csharp
using BuildingBlocks.Logging.Serilog;

SerilogExtensions.ConfigureBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.AddCustomSerilog("Catalog.Api");

    // ... service registration ...

    var app = builder.Build();
    app.UseCustomSerilogRequestLogging();
    app.Run();
}
catch (Exception ex)
{
    SerilogExtensions.LogFatal(ex, "Application terminated unexpectedly");
}
finally
{
    SerilogExtensions.CloseAndFlush();
}
```

### Data Stores

| Store | Used By |
|---|---|
| **PostgreSQL 18** | Catalog and Basket services |
| **Microsoft SQL Server** | Ordering service |
| **SQLite** | Discount service |
| **Redis** | Distributed cache for Basket service |

### Messaging & Integration

| Component | Role |
|---|---|
| **RabbitMQ** | Async integration events (e.g. basket checkout → order creation) |
| **gRPC** | Synchronous calls from Basket to Discount service |
| **REST / HTTP** | Client-facing APIs via API gateway |

### DevOps & Tooling

| Tool | Purpose |
|---|---|
| **Docker** | Container images for all services |
| **Docker Compose** | Local orchestration of services and infrastructure |
| **Visual Studio Container Tools** | Docker integration for development |

## Solution Structure

```
src/
├── ApiGateways/
│   └── ApiGateway/              # YARP reverse proxy
├── BuildingBlocks/
│   ├── BuildingBlocks/          # Shared CQRS, validation, behaviors
│   ├── BuildingBlocks.Logging/  # Serilog + Elasticsearch logging setup
│   └── BuildingBlocks.Messaging/# MassTransit + RabbitMQ setup
├── Services/
│   ├── Basket/Basket.Api/
│   ├── Catalog/Catalog.Api/
│   ├── Discount/Discount.Grpc/
│   └── Ordering/                # Domain, Application, Infrastructure, Api
├── WebApps/
│   └── Shopping.Web/            # Razor Pages storefront
├── docker-compose.yml
└── eshop-microservices.slnx
```

## Local Development

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)

### Run with Docker Compose

From the `src` folder:

```bash
docker compose build --no-cache
docker compose up -d
docker compose ps

```

### Service Endpoints (default ports)

| Service | HTTP | HTTPS |
|---|---|---|
| Catalog API | 6000 | 6060 |
| Basket API | 6001 | 6061 |
| Discount gRPC | 6002 | 6062 |
| Ordering API | 6003 | 6063 |
| API Gateway | 6004 | 6064 |
| Shopping Web | 6005 | 6065 |
| RabbitMQ Management UI | — | 15672 |
| Kibana | 5601 | — |

### Infrastructure Ports

| Component | Port |
|---|---|
| PostgreSQL (Catalog) | 5432 |
| PostgreSQL (Basket) | 5433 |
| Redis | 6379 |
| SQL Server | 1433 |
| RabbitMQ | 5672 |
| Elasticsearch | 9200 |
| Kibana | 5601 |

### Viewing Logs in Kibana

1. Start the stack with Docker Compose (Elasticsearch and Kibana are included).
2. Open Kibana at [http://localhost:5601](http://localhost:5601).
3. Go to **Stack Management → Data Views → Create data view**.
4. Use index pattern `logs-*` and timestamp field `@timestamp`.
5. Open **Discover** and filter by `service.name` (e.g. `shopping-web`, `catalog-api`).
6. If data streams are not visible, enable **Include hidden data streams** in the data view advanced settings.

Generate log traffic by browsing Shopping.Web or calling APIs through the gateway.
