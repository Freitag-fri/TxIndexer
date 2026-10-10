# TxIndexer

A blockchain transaction indexer: stores transactions and serves them through a REST API, with a focus on idempotent ingestion and a clear transaction status lifecycle.

> Work in progress: API skeleton with an in-memory store. See [Roadmap](#roadmap).

## Tech stack

.NET 10, ASP.NET Core Web API, in-memory storage (SQL Server + EF Core planned).

## Getting started

```bash
git clone https://github.com/Freitag-fri/TxIndexer.git
cd TxIndexer
dotnet run --project TxIndexer.Api
```

The API listens on `http://localhost:5110`; sample transactions are seeded on startup.
Ready-to-run requests: [`TxIndexer.Api/TxIndexer.Api.http`](TxIndexer.Api/TxIndexer.Api.http).

## API

| Method | Route | Description |
| --- | --- | --- |
| `GET` | `/api/transactions?page=1&pageSize=30` | Paged list, newest first |
| `GET` | `/api/transactions/{hash}` | Single transaction, `404` if not found |

## Architecture

`Controllers → Services → Domain ← Data`. The repository interface lives in `Domain` and is implemented in `Data`, so the in-memory store can be replaced with EF Core by changing one DI registration. The API returns DTOs, never entities.

## Design decisions

- **Identity is the transaction hash.** It is the storage key; adding a known hash is a no-op, so repeated delivery is idempotent.
- **Status lifecycle:** `Pending → Confirmed | Failed`, changed only through entity methods that reject invalid transitions.
- **Amounts are strings in the API** (invariant culture) to avoid precision loss on JavaScript clients.
- **Async repository contract with `CancellationToken`** from day one, ready for EF Core.
- **Stable pagination:** sorted by timestamp, then hash; `pageSize` is limited to 100.

**Simplifications:** chain reorganizations are not modeled; data is lost on restart.

## Roadmap

- [x] API skeleton: domain model, repository, services, DTOs, controllers
- [ ] Validation, `ProblemDetails`, filtering
- [ ] Background ingestion (`BackgroundService`), Options pattern
- [ ] JWT authentication, `IHttpClientFactory`
- [ ] SQL Server + EF Core, optimistic concurrency
- [ ] Tests, Docker Compose, Azure deployment
