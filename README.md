# TaskFlow API

Modern .NET 10 ile geliştirilmiş, Clean Architecture prensipleriyle yazılmış bir proje ve görev yönetim sistemi REST API'si.

## 🏗️ Mimari

Clean Architecture — 4 katmanlı yapı:

- **TaskFlow.Domain** — Entity'ler, iş kuralları (saf C#, hiçbir bağımlılık yok)
- **TaskFlow.Application** — CQRS handler'ları, use case'ler, validator'lar
- **TaskFlow.Infrastructure** — EF Core, JWT, dış servisler
- **TaskFlow.API** — Controllers, middleware, REST endpoints

## 🛠️ Stack

- .NET 10, ASP.NET Core
- Entity Framework Core
- PostgreSQL
- MediatR (CQRS)
- FluentValidation
- AutoMapper
- Serilog
- xUnit + Moq (testler)
- Docker

## 🚀 Çalıştırmak

(İleride dolduracağız)

## 📚 Özellikler

- JWT tabanlı authentication
- Role-based authorization
- Project ve Task CRUD
- Task atama, yorum sistemi
- Resource-based authorization

## 📄 Lisans

MIT