# EShopMicroservices

A sample e-commerce application built with **.NET microservices**, demonstrating distributed architecture patterns including API gateway routing, gRPC inter-service communication, event-driven messaging, and containerized deployment.

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
| **Building Blocks** | Shared libraries for cross-cutting concerns and messaging |

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
docker compose up --build
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

### Infrastructure Ports

| Component | Port |
|---|---|
| PostgreSQL (Catalog) | 5432 |
| PostgreSQL (Basket) | 5433 |
| Redis | 6379 |
| SQL Server | 1433 |
| RabbitMQ | 5672 |
