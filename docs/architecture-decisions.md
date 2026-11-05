# Architecture Decision Records (ADR) & Design Rationale

This document outlines the foundational technical decisions for the **CrmApi** project, serving relational replica data from **Microsoft SQL Server** (`CRMDatabaseMS`) and **PostgreSQL** (`CRMDatabasePG`) across Salesforce, HubSpot, and Microsoft Dynamics 365.

Every design choice is grounded in enterprise architectural patterns, authoritative industry specifications, and established literature from Martin Fowler, Microsoft Architecture Guides, and database/API consensus.

---

## Table of Contents
- [ADR 1: Data Access Layer via Dapper and SqlKata (Micro-ORM & Query Object over Heavy ORM)](#adr-1-data-access-layer-via-dapper-and-sqlkata-micro-orm--query-object-over-heavy-orm)
- [ADR 2: Dynamic Multi-Provider Routing (`/api/{provider}/v1/...`)](#adr-2-dynamic-multi-provider-routing-apiproviderv1)
- [ADR 3: Clean Architecture Layering & Inversion of Control](#adr-3-clean-architecture-layering--inversion-of-control)
- [ADR 4: HotChocolate GraphQL Foundation with DataLoader Batching](#adr-4-hotchocolate-graphql-foundation-with-dataloader-batching)
- [ADR 5: Repository and Abstract Factory Patterns for Multi-Engine Persistence](#adr-5-repository-and-abstract-factory-patterns-for-multi-engine-persistence)
- [ADR 6: Standardized Error Handling via RFC 7807 / RFC 9457 Problem Details](#adr-6-standardized-error-handling-via-rfc-7807--rfc-9457-problem-details)
- [ADR 7: Stateless Offset Pagination and Dynamic Filtering via Immutable Value Objects](#adr-7-stateless-offset-pagination-and-dynamic-filtering-via-immutable-value-objects)
- [ADR 8: Architectural Division of Responsibilities: API-First Testing vs. Database Invariants](#adr-8-architectural-division-of-responsibilities-api-first-testing-vs-database-invariants)

---

## ADR 1: Data Access Layer via Dapper and SqlKata (Micro-ORM & Query Object over Heavy ORM)

### Status
Accepted

### Context
The application queries 37 relational tables across 3 distinct enterprise CRM schemas (`Salesforce`, `HubSpot`, `Dynamics365`) hosted concurrently on both Microsoft SQL Server (`CRMDatabaseMS`) and PostgreSQL (`CRMDatabasePG`). Database schemas are maintained independently via declarative SSDT DACPAC projects and Flyway SQL migrations respectively.

We needed a data access strategy that:
1. Emits native, parameterized SQL for two different SQL dialects without maintaining duplicate SQL strings.
2. Achieves near-zero CPU and memory allocation overhead for high-concurrency read queries.
3. Completely decouples application code from database schema migrations and change-tracking state machines.

### Decision
Use **SqlKata** as an object-oriented query builder and **Dapper** as the execution and object-mapping micro-ORM, eschewing heavyweight full-featured ORMs like Entity Framework Core or NHibernate.

### Rationales & Authoritative Sources

1. **Object-Relational Impedance Mismatch & Micro-ORM Philosophy**:
   - Heavyweight ORMs introduce substantial runtime overhead (identity maps, change trackers, proxy generators, and state machines) that add no value for high-performance read-only projections.
   - *References*:
     - Martin Fowler: [OrmHate](https://martinfowler.com/bliki/OrmHate.html) — Explains the impedance mismatch and warns against treating relational databases merely as object dumps: *"The primary solution is to realize that ORMs are about mapping between objects and database... the key is to use them for what they are good at and not expect them to hide the database completely."*
     - Martin Fowler: [Data Mapper Pattern](https://martinfowler.com/eaaCatalog/dataMapper.html) (*Patterns of Enterprise Application Architecture* [P of EAA]) — Dapper functions as a streamlined Data Mapper, separating in-memory C# representations from database schemas without leaking persistence concerns into entities.

2. **Query Object Pattern for Multi-Dialect SQL Compilation**:
   - Rather than interpolating SQL strings or duplicating queries per engine, queries are composed as programmatic trees that compile to dialect-specific SQL (T-SQL brackets `[Salesforce].[Account]` with `OFFSET...FETCH NEXT` vs. ANSI quotes `"salesforce"."account"` with `LIMIT...OFFSET`).
   - *References*:
     - Martin Fowler: [Query Object Pattern](https://martinfowler.com/eaaCatalog/queryObject.html) (*P of EAA*) — *"A Query Object is an interpreter, that is, a structure of objects that can form itself into a SQL query. You can create this query by referring to classes and fields rather than tables and columns."*
     - SqlKata Official Documentation: [SqlKata Documentation & Architecture](https://sqlkata.com/docs) & [SqlKata GitHub Repository](https://github.com/sqlkata/querybuilder).

3. **Zero Change-Tracking Overhead & Maximum Execution Throughput**:
   - Dapper executes directly on native `DbCommand` and `DbDataReader` streams with runtime-emitted dynamic MSIL (`DynamicMethod`), yielding deserialization speeds within single-digit percentage points of handwritten ADO.NET.
   - *References*:
     - Dapper Micro-ORM: [Dapper Benchmarks & Documentation](https://github.com/DapperLib/Dapper#benchmarks).

4. **Decoupling Application from Database Schema Governance**:
   - Schemas are governed in dedicated database projects (`CRMDatabaseMS` via DACPAC and `CRMDatabasePG` via Flyway). Tying the API to EF Core migrations would create dual-state synchronization debt and violate single-source-of-truth principles.
   - *References*:
     - Pramod Sadalage & Martin Fowler: [Evolutionary Database Design](https://martinfowler.com/articles/evodb.html) — Outlines why database migrations must be independently versioned and automated alongside application deployments rather than generated implicitly.
     - Martin Fowler: [Database Migration Pattern](https://martinfowler.com/patterns/database-migration.html).

### Consequences
- **Positive**: Exceptional request throughput, negligible memory footprint, parameterized SQL injection prevention, single query definition running against both SQL Server and PostgreSQL.
- **Trade-off**: Developers write explicit Dapper mapping methods for deep multi-level object graphs instead of relying on ORM navigation property auto-population.

---

## ADR 2: Dynamic Multi-Provider Routing (`/api/{provider}/v1/...`)

### Status
Accepted

### Context
Clients must be able to interrogate either database engine (`ms` for Microsoft SQL Server, `pg` for PostgreSQL) dynamically across any CRM entity using standard RESTful conventions, enabling live benchmarking, verification, and migration without deploying separate APIs.

### Decision
Adopt route templates parameterized by the provider token constrained by regex:
```http
/api/{provider:regex(^(ms|pg)$)}/v1/{crm}/{resource}
```

### Rationales & Authoritative Sources

1. **Branch By Abstraction & Strangler Fig Architectural Patterns**:
   - Exposing both database engines under identical URL structures allows parallel running, A/B validation, and seamless progressive transitions from one database engine to another without breaking client contracts.
   - *References*:
     - Martin Fowler: [Branch By Abstraction](https://martinfowler.com/bliki/BranchByAbstraction.html) — *"Branch by Abstraction is a technique for making a large-scale change to a software system in gradual steps... allowing you to release your software with the new implementation running in parallel with the old one."*
     - Martin Fowler: [Strangler Fig Application](https://martinfowler.com/bliki/StranglerFigApplication.html) — Intercepting and routing calls to alternate backends behind a unified façade.

2. **Richardson Maturity Model & Microsoft REST API Guidelines**:
   - Routes model resources as predictable nouns, use HTTP verbs appropriately (`GET`), support explicit versioning (`v1`), and enforce provider constraints at the routing tier with standard HTTP 404 responses for invalid providers.
   - *References*:
     - Martin Fowler: [Richardson Maturity Model](https://martinfowler.com/articles/richardsonMaturityModel.html) — Steps to the glory of REST: Level 1 (Resources), Level 2 (HTTP Verbs), and uniform resource addressing.
     - Microsoft: [Microsoft REST API Guidelines](https://github.com/microsoft/api-guidelines) — Sections 5.1 (URL Path format), 7.2 (Versioning), and 9 (Collections).
     - Microsoft Learn: [Routing in ASP.NET Core & Route Constraints](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/routing).

### Consequences
- **Positive**: Single codebase supports both database backends simultaneously; easy integration testing with Bruno across environments by changing only the `provider` variable.
- **Trade-off**: Every controller route must declare `{provider}` and parse it into `DatabaseProvider` enum before repository calls.

---

## ADR 3: Clean Architecture Layering & Inversion of Control

### Status
Accepted

### Context
The solution must separate pure domain representations from database mechanics, query building, application orchestration, and external transport protocols (REST and GraphQL).

### Decision
Structure the solution into 4 strictly decoupled layers with inward-facing dependencies:
- **`Domain`**: Pure C# record definitions, pagination envelopes (`PagedResult<T>`), and query options (`QueryOptions`). Zero external package dependencies.
- **`Infrastructure`**: Database connection factories (`IDbConnectionFactory`), SqlKata compiler factories (`ISqlKataCompilerFactory`), and CRM repositories executing Dapper queries.
- **`Application`**: Query services (`ISalesforceQueryService`, `IHubSpotQueryService`, `IDynamics365QueryService`), filter orchestrators, and child collection loaders.
- **`Api`**: ASP.NET Core controllers, route dispatching, middleware, OpenAPI / Swagger specifications, and HotChocolate GraphQL endpoints.

### Rationales & Authoritative Sources

1. **Separation of Concerns & Presentation-Domain-Data Layering**:
   - Isolating domain contracts from infrastructure mechanics ensures business entities remain unaffected by database changes, driver upgrades, or transport migrations.
   - *References*:
     - Martin Fowler: [PresentationDomainDataLayering](https://martinfowler.com/bliki/PresentationDomainDataLayering.html) — *"One of the most common ways to modularize an information system is to divide it into three broad layers: presentation, domain, and data source."*
     - Martin Fowler: [Separated Interface](https://martinfowler.com/eaaCatalog/separatedInterface.html) (*P of EAA*) — Interfaces (`ISalesforceQueryService`, `ISalesforceRepository`) are owned by consumer layers, inverting runtime dependencies.
     - Robert C. Martin: [The Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html) — The Dependency Rule: Source code dependencies can only point inward.
     - Microsoft Learn: [Common Web Application Architectures](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures).

2. **Inversion of Control (IoC) & Transport Independence**:
   - Both REST controllers (`SalesforceController`) and HotChocolate GraphQL resolvers (`Query`) consume the exact same `Application` query services via dependency injection, eliminating duplicated query logic.
   - *References*:
     - Martin Fowler: [Inversion of Control Containers and the Dependency Injection pattern](https://martinfowler.com/articles/injection.html) — Principles of decoupling service configuration from resolution.
     - Microsoft Learn: [Dependency injection in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection).

### Consequences
- **Positive**: Testability is maximized (services can be unit tested with mock repositories); transport layers (REST vs. GraphQL) share 100% of domain and query logic.
- **Trade-off**: Requires explicit project references and dependency injection registration (`AddApplicationServices`, `AddInfrastructureServices`).

---

## ADR 4: HotChocolate GraphQL Foundation with DataLoader Batching

### Status
Accepted

### Context
In addition to REST endpoints, clients require a flexible query interface capable of selecting specific fields and navigating related entities (e.g. Accounts with Contacts, Opportunities with LineItems) without suffering from the $N+1$ query problem.

### Decision
Implement **HotChocolate GraphQL** utilizing the `IDataLoader` pattern for batching child entity queries over Dapper and SqlKata.

### Rationales & Authoritative Sources

1. **Mitigating the N+1 Query Problem in Relational Queries**:
   - In naïve GraphQL implementations, resolving child relationships for a list of $N$ parent records results in 1 initial query plus $N$ individual child queries ($N+1$).
   - HotChocolate DataLoaders collect foreign keys across resolver executions within a request turn and dispatch a single parameterized `WHERE id IN (@id1, @id2, ...)` batch query through SqlKata and Dapper.
   - *References*:
     - Martin Fowler: [Lazy Load / N+1 Problem](https://martinfowler.com/eaaCatalog/lazyLoad.html) (*P of EAA*) — Trade-offs of loading on demand versus eager batching.
     - Lee Byron: [GraphQL DataLoader Specification](https://github.com/graphql/dataloader) — Foundational pattern for batching and caching asynchronous requests in GraphQL execution engines.
     - ChilliCream: [HotChocolate DataLoaders Documentation](https://chillicream.com/docs/hotchocolate/v13/fetching-data/dataloader).

2. **Official GraphQL Specification Alignment**:
   - Supports declarative field selection, strongly typed schemas, and standard execution semantics.
   - *References*:
     - GraphQL Foundation: [Official GraphQL Specification](https://spec.graphql.org/).

### Consequences
- **Positive**: Clients can fetch tailored payloads in a single HTTP request while database query volume remains strictly bounded ($O(1)$ constant number of batch queries).
- **Trade-off**: GraphQL schemas require dedicated Type configurations (`SalesforceAccountType`, `SalesforceContactType`, etc.).

---

## ADR 5: Repository and Abstract Factory Patterns for Multi-Engine Persistence

### Status
Accepted

### Context
The application must instantiate either `Microsoft.Data.SqlClient.SqlConnection` or `Npgsql.NpgsqlConnection`, and pair it with either `SqlServerCompiler` or `PostgresCompiler`, transparently at runtime per request based on the `{provider}` token.

### Decision
Implement the **Abstract Factory Pattern** (`IDbConnectionFactory`, `ISqlKataCompilerFactory`) injected into a **Repository Pattern** (`BaseRepository`, `SalesforceRepository`, `HubSpotRepository`, `Dynamics365Repository`).

### Rationales & Authoritative Sources

1. **Repository Pattern for Decoupled Data Access**:
   - Repositories mediate between the domain and data mapping layers, acting like in-memory domain object collections and hiding SQL dialects from business logic.
   - *References*:
     - Martin Fowler: [Repository Pattern](https://martinfowler.com/eaaCatalog/repository.html) (*P of EAA*) — *"Mediates between the domain and data mapping layers using a collection-like interface for accessing domain objects."*
     - Martin Fowler: [Gateway Pattern](https://martinfowler.com/eaaCatalog/gateway.html) (*P of EAA*) — Encapsulating access to an external system or resource.

2. **Abstract Factory Pattern for Connection & Compiler Creation**:
   - Centralizes the creation of vendor-specific ADO.NET connection objects and query compilers without hardcoding connection constructors into repositories.
   - *References*:
     - Erich Gamma, Richard Helm, Ralph Johnson, John Vlissides: *Design Patterns: Elements of Reusable Object-Oriented Software* (GoF) — Abstract Factory Pattern.
     - Microsoft Learn: [Framework Design Guidelines: Factory Patterns](https://learn.microsoft.com/en-us/dotnet/standard/design-guidelines/).

3. **SQL Injection Defense-in-Depth**:
   - Dynamic sorting columns and filter keys are validated against strict alphanumeric regex (`^[a-zA-Z0-9_.]+$`), while all query values are passed as parameterized bindings (`@p0`, `@p1`) to Dapper.
   - *References*:
     - OWASP Foundation: [SQL Injection Prevention Cheat Sheet](https://cheatsheetseries.owasp.org/cheatsheets/SQL_Injection_Prevention_Cheat_Sheet.html).

### Consequences
- **Positive**: Clean encapsulation of connection lifecycle and compiler choices; strong protection against SQL injection; fully unit-testable.
- **Trade-off**: Additional abstraction layer compared to calling `new SqlConnection()` directly in endpoints.

---

## ADR 6: Standardized Error Handling via RFC 7807 / RFC 9457 Problem Details

### Status
Accepted

### Context
When exceptions occur (e.g. invalid database provider, bad filter parameter, missing entity, database timeout), the API must return standardized, machine-readable HTTP error payloads without leaking internal server stack traces or database connection details.

### Decision
Implement custom ASP.NET Core middleware (`ExceptionHandlingMiddleware`) returning standard **`ProblemDetails`** payloads with `Content-Type: application/problem+json` compliant with RFC 7807 and RFC 9457.

### Rationales & Authoritative Sources

1. **RFC 7807 / RFC 9457 Standardized Problem Details**:
   - Defines a standardized JSON schema for HTTP API errors (`status`, `title`, `detail`, `instance`), eliminating custom ad-hoc error formats across endpoints.
   - *References*:
     - IETF RFC 7807: [Problem Details for HTTP APIs](https://datatracker.ietf.org/doc/html/rfc7807).
     - IETF RFC 9457: [Problem Details for HTTP APIs (Updated RFC)](https://datatracker.ietf.org/doc/html/rfc9457).
     - Microsoft Learn: [Handle errors in ASP.NET Core web APIs](https://learn.microsoft.com/en-us/aspnet/core/web-api/handle-errors).

2. **Notification Pattern & Safe Fault Handling**:
   - Translates domain exceptions (`ArgumentException` $\rightarrow$ 400 Bad Request, `KeyNotFoundException` $\rightarrow$ 404 Not Found, unhandled $\rightarrow$ 500 Internal Server Error) while logging complete diagnostic context securely to `ILogger`.
   - *References*:
     - Martin Fowler: [Notification Pattern](https://martinfowler.com/articles/replaceThrowWithNotification.html) — Collecting errors and communicating status cleanly to consumers.

### Consequences
- **Positive**: Predictable error contracts for client libraries, automated Bruno test assertions, and secure sanitization of internal server details.
- **Trade-off**: Requires centralized exception mapping in middleware.

---

## ADR 7: Stateless Offset Pagination and Dynamic Filtering via Immutable Value Objects

### Status
Accepted

### Context
Querying 37 enterprise CRM entities requires uniform, stateless handling of pagination (`page`, `pageSize`), multi-column sorting (`sortBy`, `sortDirection`), and arbitrary field filtering (`{field}={value}`) across both SQL dialects.

### Decision
Model query parameters as an immutable `QueryOptions` record and return responses wrapped in an immutable `PagedResult<T>` envelope containing metadata (`Items`, `TotalCount`, `Page`, `PageSize`, `TotalPages`, `HasPreviousPage`, `HasNextPage`).

### Rationales & Authoritative Sources

1. **Value Object Pattern**:
   - Query options and paged responses are immutable value objects defined by their attributes rather than identity, guaranteeing thread safety and eliminating mutation bugs across layers.
   - *References*:
     - Martin Fowler: [Value Object](https://martinfowler.com/bliki/ValueObject.html) — Immutable objects compared by value rather than identity.
     - Martin Fowler: [Data Transfer Object (DTO)](https://martinfowler.com/eaaCatalog/dataTransferObject.html) (*P of EAA*) — Packaging data between processes to reduce round-trips.

2. **Microsoft REST API Guidelines on Collections & Pagination**:
   - Adheres to standard pagination patterns using `page` and `pageSize` (defaulting to 20, capped at 100), accompanied by total item counts and page calculation.
   - *References*:
     - Microsoft: [Microsoft REST API Guidelines Section 9 (Collections)](https://github.com/microsoft/api-guidelines#9-collections).

### Consequences
- **Positive**: Uniform client experience across all 37 endpoints; clean separation between HTTP query string parsing and database query construction.
- **Trade-off**: Offset pagination on extremely deep offsets (e.g. page 50,000) incurs SQL scan cost; acceptable for standard interactive CRM workloads.

---

## ADR 8: Architectural Division of Responsibilities: API-First Testing vs. Database Invariants

### Status
Accepted

### Context
An essential architectural decision across the reference architecture is determining where business logic, data validation, and automated testing should reside relative to the database tier (`CRMDatabaseMS` / `CRMDatabasePG`).

### Decision
Adopt an **API-First testing and validation strategy**:
- **API / Application Layer**: Domain models, business validation rules, pagination, sorting, query composition, and comprehensive test pyramids (unit tests, component tests, contract tests, and integration tests).
- **Database Layer**: Enforcing relational structural invariants (primary keys, foreign keys, unique constraints, nullability), ACID transaction guarantees, and physical indexing.

### Rationales & Authoritative Sources

1. **The Test Pyramid & Fast Feedback Cycles**:
   - Automated tests in the API layer execute in milliseconds in-memory, providing immediate CI/CD feedback, whereas heavy database stored procedure testing requires expensive instance provisioning and disk I/O.
   - *References*:
     - Martin Fowler: [The Practical Test Pyramid](https://martinfowler.com/articles/practical-test-pyramid.html) — Emphasizes lightweight, isolated tests at the base of the pyramid.
     - Martin Fowler: [Microservice Testing](https://martinfowler.com/articles/microservice-testing/) — Strategies for unit, component, contract, and end-to-end testing.

2. **Smart Endpoints and Dumb Pipes**:
   - Business intelligence and validation belongs in cohesive application services, while persistence and transport infrastructure remain decoupled backing services.
   - *References*:
     - Martin Fowler: [Smart Endpoints and Dumb Pipes](https://martinfowler.com/articles/microservices.html#SmartEndpointsAndDumbPipes) — Foundational microservice and distributed systems principle.
     - The Twelve-Factor App: [Stateless Processes & Backing Services](https://12factor.net/) — Stateless application processes scale horizontally at low cost, treating databases as attached resources.

3. **Cloud Infrastructure Economics & Vertical vs. Horizontal Scaling**:
   - API containers scale horizontally on commodity hardware or Kubernetes pods at negligible cost, whereas relational database CPU cycles, IOPS, and connection pools are among the most expensive and contention-prone resources in an enterprise architecture.
   - *References*:
     - AWS: [AWS Well-Architected Framework: Reliability & Performance Efficiency](https://aws.amazon.com/architecture/well-architected/).

### Consequences
- **Positive**: CI/CD pipelines run fast; application logic is completely portable across database engines (SQL Server vs PostgreSQL); database CPU is reserved for pure relational operations.
- **Trade-off**: Database cannot guarantee complex multi-entity business workflows without the API layer present.
