# Architecture Decision Records (ADR) & Design Rationale

This document outlines key technical decisions for the **CrmApi** project, serving relational replica data from **Microsoft SQL Server** (`CRMDatabaseMS`) and **PostgreSQL** (`CRMDatabasePG`) across Salesforce, HubSpot, and Microsoft Dynamics 365.

---

## ADR 1: Data Access Layer via Dapper and SqlKata

### Context
The application needs to query 37 relational tables across 3 distinct CRM schemas (`Salesforce`, `HubSpot`, `Dynamics365`) hosted concurrently on both Microsoft SQL Server and PostgreSQL. The schemas are maintained via declarative SSDT DACPAC projects and Flyway SQL migrations respectively.

### Decision
Use **SqlKata** as the query builder and **Dapper** as the execution and object mapper micro-ORM.

### Rationales & Authoritative Sources
1. **Multi-Dialect SQL Generation**:
   - SqlKata provides dedicated query compilers: `SqlServerCompiler` and `PostgresCompiler`.
   - The same business query definition compiled against `SqlServerCompiler` produces T-SQL with bracket delimiters (`[Salesforce].[Account]`) and `OFFSET...FETCH NEXT`, while `PostgresCompiler` produces standard ANSI/Postgres SQL (`"salesforce"."account"`, `LIMIT...OFFSET`).
   - *Reference*: [SqlKata Documentation & Architecture](https://sqlkata.com/docs) & [SqlKata GitHub Repository](https://github.com/sqlkata/querybuilder).
2. **Zero Change-Tracking Overhead & High Throughput**:
   - Dapper operates directly over `DbDataReader` streams with compiled IL emitting, achieving execution times and memory allocations within fractions of raw ADO.NET.
   - *Reference*: [Dapper Micro-ORM Benchmarks](https://github.com/DapperLib/Dapper#benchmarks).
3. **Decoupling from Entity Framework Migrations**:
   - Since database schemas are governed externally in database repositories (`CRMDatabaseMS` and `CRMDatabasePG`), using EF Core would introduce dual-state synchronization debt and duplicate schema definitions.

---

## ADR 2: Dynamic Multi-Provider Routing (`/api/{provider}/v1/...`)

### Context
Clients must be able to explicitly choose the target database engine (`ms` for SQL Server, `pg` for PostgreSQL) via standard REST endpoints.

### Decision
Adopt route templates parameterized by provider:
```
/api/{provider:regex(^(ms|pg)$)}/v1/{crm}/{resource}
```

### Rationales & Authoritative Sources
1. **Multi-Tenancy & Database Engine Transparency**:
   - Routing through `{provider}` allows transparent performance comparison, verification, and migration between SQL Server and PostgreSQL engines.
2. **Adherence to Microsoft REST Guidelines**:
   - RESTful resource paths are predictable, versioned (`v1`), and nouns represent collections and single resources.
   - *Reference*: [Microsoft REST API Guidelines](https://github.com/microsoft/api-guidelines) & [ASP.NET Core Web API Best Practices](https://learn.microsoft.com/en-us/aspnet/core/web-api/).

---

## ADR 3: Clean Architecture Layering

### Context
The solution must separate pure domain representations from database mechanics, application query orchestration, and external transport protocols (REST and GraphQL).

### Decision
Structure the solution into 4 decoupled layers:
- `Domain`: Pure C# record definitions, pagination envelopes (`PagedResult<T>`), and query options (`QueryOptions`). Zero external dependencies.
- `Infrastructure`: `IDbConnectionFactory`, `ISqlKataCompilerFactory`, and repository implementations executing Dapper queries.
- `Application`: Query services (`ISalesforceQueryService`, `IHubSpotQueryService`, `IDynamics365QueryService`), filter parsers, and relationship resolution.
- `Api`: ASP.NET Core controllers, route dispatching, middleware, OpenAPI / Swagger specifications, and HotChocolate GraphQL endpoints.

### Rationales & Authoritative Sources
1. **Transport Independence**:
   - Both REST controllers and HotChocolate GraphQL resolvers execute the same `Application` query services without code duplication.
2. **Inversion of Control**:
   - Domain is completely decoupled from ADO.NET, Dapper, or transport frameworks.
   - *Reference*: [Common Web Application Architectures (.NET Architecture Guides)](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures).

---

## ADR 4: HotChocolate GraphQL Foundation with DataLoader Batching

### Context
In addition to REST endpoints, clients require a flexible query interface capable of selecting specific fields and navigating related entities (e.g. Accounts with Contacts, Opportunities with LineItems).

### Decision
Implement **HotChocolate GraphQL** utilizing `IDataLoader` for batching relationship queries over Dapper/SqlKata.

### Rationales & Authoritative Sources
1. **Solving the N+1 Problem**:
   - HotChocolate DataLoaders collect foreign keys across resolver invocations and dispatch a single parameterized `WHERE id IN (...)` query through SqlKata and Dapper.
   - *Reference*: [ChilliCream HotChocolate DataLoaders](https://chillicream.com/docs/hotchocolate/v13/fetching-data/dataloader).
2. **Schema-First and Code-First Flexibility**:
   - Enables rich GraphQL schemas directly reusing strongly typed domain models.
