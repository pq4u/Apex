# Apex

Apex is an application to analyze Formula 1 drivers telemetry, session data and other data related to this sport. The data are fetched from OpenF1 API.

## Architecture

The application follows Clean Architecture principles with CQRS pattern, built on .NET 9 and Next.js 15.

### Projects

- **Apex.Domain** - Core business entities and domain logic
- **Apex.Application** - Business logic with CQRS (Commands/Queries)
- **Apex.Infrastructure** - Data access layer with Entity Framework Core and TimescaleDB
- **Apex.Api** - REST API built with ASP.NET Core
- **Apex.Worker** - Background service for data ingestion from OpenF1 API
- **Apex.Web** - Next.js frontend with React 19 and TailwindCSS
- **Apex.UnitTests** - Unit tests

### Technology Stack

**Backend:**
- .NET 9
- Entity Framework Core with PostgreSQL
- TimescaleDB (time-series extension for telemetry data)
- Dapper for raw SQL queries
- Serilog for logging
- Polly for resilience and retry policies

**Frontend:**
- Next.js 15 with Turbopack
- React 19
- TailwindCSS v4
- Chart.js for data visualization

**Infrastructure:**
- Docker & Docker Compose
- PostgreSQL with TimescaleDB extension

## Run Application

Execute command in terminal:

```bash
docker compose up -d
```

It will start Api, Worker, Web and PostgreSQL containers.

### Default Ports

- **apex-api** - http://localhost:5000
- **apex-web** - http://localhost:3001
- **postgresql** - localhost:5432

### API Documentation

Swagger UI is available in development mode at: http://localhost:5000/swagger

## Development

### Prerequisites

- .NET 9 SDK
- Node.js 20+
- Docker & Docker Compose

### Database Migrations

Migrations are automatically applied in development mode when the API or Worker starts.

To create a new migration:

```bash
dotnet ef migrations add MigrationName --project src/Apex.Infrastructure --startup-project src/Apex.Api
```

## Features

- Real-time F1 telemetry data ingestion
- Session and race data analysis
- Driver performance metrics
- Lap time comparisons
- Interactive data visualizations
