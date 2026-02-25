# LoyaltyCard

A production-grade .NET microservice built using Clean Architecture, SOLID, and DDD principles.

## Architecture

The solution is structured into the following layers:

- **API** – ASP.NET Core Web API (HTTP entry point)
- **Worker** – Background service for async processing
- **Application** – Use cases and application logic
- **Domain** – Core business rules and entities
- **Infrastructure** – Database, caching, messaging, and logging implementations
- **Tests**-  Unit tests, Integration tests

##  Tech Stack

- **.NET 8 (LTS)**
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **PostgreSQL**
- **Redis (Distributed Cache)**
- **Apache Kafka**
- **Serilog (Structured Logging)**
- **Elasticsearch & Kibana**
- **Docker & Docker Compose**

---

##  Features (Planned)

- Clean Architecture implementation
- Background job processing
- Distributed caching
- Event-driven communication
- Observability & centralized logging
- Containerized deployment
- Scalable microservice design

---

##  Running the Project

```bash
dotnet restore
dotnet build
dotnet run --project LoyaltyCard.Api

## Status

	 Project is under active development.

## 📄 License

This project is intended for learning and architectural demonstration purposes.
