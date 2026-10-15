# TxIndexer

A blockchain transaction indexer: stores transactions and serves them through a REST API, with a focus on idempotent ingestion and a clear transaction status lifecycle.

> Work in progress: read-only API over an in-memory store. See [Roadmap](#roadmap).

## Tech stack

.NET 10, ASP.NET Core Web API (controllers), OpenAPI + Scalar, in-memory storage (SQL Server + EF Core planned).

## Getting started

```bash
git clone https://github.com/Freitag-fri/TxIndexer.git
cd TxIndexer
dotnet run --project TxIndexer.Api
```

The API listens on `http://localhost:5110`; sample transactions are seeded on startup.
In Development, the API reference is available at `/scalar` and the OpenAPI document at `/openapi/v1.json`.
Ready-to-run requests: [`TxIndexer.Api/TxIndexer.Api.http`](TxIndexer.Api/TxIndexer.Api.http).

## API

| Method | Route | Description |
| --- | --- | --- |
| `GET` | `/api/transactions` | Paged, filterable list, newest first |
| `GET` | `/api/transactions/{hash}` | Single transaction; `400` for a malformed hash, `404` if not found |

Query parameters for the list:

| Parameter | Description |
| --- | --- |
| `page`, `pageSize` | Pagination; `pageSize` is 1–100, default 30 |
| `token` | Token symbol, case-insensitive |
| `address` | Matches either sender or recipient, case-insensitive |
| `fromBlock`, `toBlock` | Block range, inclusive; `fromBlock` must not exceed `toBlock` |
| `status` | `Pending`, `Confirmed` or `Failed` |

Errors are returned as `ProblemDetails` (`application/problem+json`, RFC 9457) with a `traceId`; validation errors list every invalid field.

## Architecture

`Controllers → Services → Domain ← Data`. The repository interface lives in `Domain` and is implemented in `Data`, so the in-memory store can be replaced with EF Core by changing one DI registration. The API returns DTOs, never entities.

Cross-cutting concerns live in `Infrastructure`: a global `IExceptionHandler` and an MVC action filter that logs slow actions.

## Design decisions

- **Identity is the transaction hash.** It is the storage key; adding a known hash is a no-op, so repeated delivery is idempotent.
- **Status lifecycle:** `Pending → Confirmed | Failed`, changed only through entity methods that reject invalid transitions.
- **Amounts are strings in the API** (invariant culture), so clients that parse JSON numbers as floating point do not lose precision.
- **Async repository contract with `CancellationToken`** from day one, ready for EF Core.
- **Stable pagination:** sorted by timestamp, then hash; `pageSize` is limited to 100.
- **Layered validation:** request format is checked at the API boundary (`400`), invariants are enforced in the service and the entity.
- **No exception details leak to clients:** unexpected errors become a generic `500` `ProblemDetails`; the full exception is logged with structured fields.

**Simplifications:** chain reorganizations are not modeled; data is lost on restart.

## Roadmap

- [x] API skeleton: domain model, repository, services, DTOs, controllers
- [x] Request pipeline: validation, `ProblemDetails`, global exception handling, filtering, MVC action filter
- [ ] SQL Server + EF Core, migrations, optimistic concurrency
- [ ] Background ingestion (`BackgroundService`), Options pattern
- [ ] JWT authentication, `IHttpClientFactory`
- [ ] Tests, Docker Compose, Azure deployment
