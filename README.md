# CrmApiCSharp - Multi-Provider CRM Web API (.NET 10 / C# 13)

A high-performance C# .NET 10 Web API serving relational data from two enterprise CRM database engines:
- **Microsoft SQL Server** (`CRMDatabaseMS`)
- **PostgreSQL** (`CRMDatabasePG`)

Covering **37 tables** across three CRM schemas:
- **Salesforce (15 entities)**: `Account`, `Contact`, `Opportunity`, `Lead`, `Campaign`, `Case`, `User`, `Event`, `Task`, `Product2`, `Pricebook2`, `PricebookEntry`, `OpportunityLineItem`, `AccountContactRelation`, `CampaignMember`.
- **HubSpot (11 entities)**: `Company`, `Contact`, `Deal`, `Ticket`, `Owner`, `Product`, `LineItem`, `Pipeline`, `DealStage`, `Engagement`, `ObjectAssociation`.
- **Dynamics 365 (11 entities)**: `Account`, `Contact`, `Opportunity`, `Lead`, `Incident`, `Product`, `PriceLevel`, `OpportunityProduct`, `BusinessUnit`, `SystemUser`, `ActivityPointer`.

Detailed Architecture Decision Records (ADRs) are documented in [docs/architecture-decisions.md](file:///Users/cy/codes/localstack-reference-architecture/docs/architecture-decisions.md). For engineering standards, refer to the [CrmApiCSharp Coding Standards](file:///Users/cy/codes/localstack-reference-architecture/CrmApiCSharp/CODING_STANDARDS.md) and repository-wide [Coding Standards](file:///Users/cy/codes/localstack-reference-architecture/CODING_STANDARDS.md).

---

## Architectural Highlights & Authoritative Sources

Every technical design choice in **CrmApiCSharp** is grounded in established enterprise patterns, official specifications, and literature from Martin Fowler and Microsoft Architecture:

### 1. High-Throughput Micro-ORM via Dapper
- **Design Choice**: Forego heavy ORMs (EF Core/NHibernate) and execute compiled MSIL directly over ADO.NET `DbDataReader` streams for near-zero allocation and raw query speed.
- **Authoritative References**:
  - Martin Fowler: [OrmHate](https://martinfowler.com/bliki/OrmHate.html) — Navigating the Object-Relational Impedance Mismatch and avoiding unnecessary ORM state tracking when reading relational data.
  - Martin Fowler: [Data Mapper Pattern](https://martinfowler.com/eaaCatalog/dataMapper.html) (*Patterns of Enterprise Application Architecture*) — Decoupling domain records from database schemas.
  - Dapper Micro-ORM: [Dapper Benchmarks & Documentation](https://github.com/DapperLib/Dapper#benchmarks).
  - Pramod Sadalage & Martin Fowler: [Evolutionary Database Design](https://martinfowler.com/articles/evodb.html) — Separating database schema lifecycles from application ORM generation.

### 2. Multi-Dialect Dynamic Query Building via SqlKata
- **Design Choice**: Programmatic query objects compiled into dialect-specific SQL (`SqlServerCompiler` for T-SQL bracketed identifiers and `OFFSET...FETCH` vs. `PostgresCompiler` for ANSI double quotes and `LIMIT...OFFSET`).
- **Authoritative References**:
  - Martin Fowler: [Query Object Pattern](https://martinfowler.com/eaaCatalog/queryObject.html) (*P of EAA*) — Representing database queries as structured object graphs independent of SQL syntax.
  - SqlKata Documentation: [SqlKata Architecture & Guide](https://sqlkata.com/docs) & [SqlKata GitHub](https://github.com/sqlkata/querybuilder).
  - OWASP Foundation: [SQL Injection Prevention Cheat Sheet](https://cheatsheetseries.owasp.org/cheatsheets/SQL_Injection_Prevention_Cheat_Sheet.html) — Identifier validation with regex and parameterized values.

### 3. Dynamic Multi-Provider Routing (`/api/{provider}/v1/...`)
- **Design Choice**: Single endpoint surface allowing clients to target Microsoft SQL Server (`ms`) or PostgreSQL (`pg`) at runtime via regex-constrained path parameters (`/api/{provider:regex(^(ms|pg)$)}/v1/...`).
- **Authoritative References**:
  - Martin Fowler: [Branch By Abstraction](https://martinfowler.com/bliki/BranchByAbstraction.html) — Running dual implementations concurrently to allow risk-free migration, testing, and side-by-side benchmarking.
  - Martin Fowler: [Strangler Fig Application](https://martinfowler.com/bliki/StranglerFigApplication.html) — Transparent routing behind a facade.
  - Martin Fowler: [Richardson Maturity Model](https://martinfowler.com/articles/richardsonMaturityModel.html) — Standard HTTP verbs and resource URIs.
  - Microsoft: [Microsoft REST API Guidelines](https://github.com/microsoft/api-guidelines) & [ASP.NET Core Web API Routing](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/routing).

### 4. Clean Architecture Layering & Inversion of Control
- **Design Choice**: Strict 4-layer separation (`Domain`, `Infrastructure`, `Application`, `Api`) with inward-pointing dependencies, enabling transport independence where REST controllers and GraphQL resolvers share identical application query services.
- **Authoritative References**:
  - Martin Fowler: [PresentationDomainDataLayering](https://martinfowler.com/bliki/PresentationDomainDataLayering.html) — Modularizing applications into presentation, domain, and data source tiers.
  - Martin Fowler: [Inversion of Control Containers and the Dependency Injection pattern](https://martinfowler.com/articles/injection.html) — Inverting runtime dependencies via DI.
  - Martin Fowler: [Separated Interface](https://martinfowler.com/eaaCatalog/separatedInterface.html) — Defining query service interfaces separately from implementations.
  - Microsoft Learn: [Common Web Application Architectures](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures).
  - Robert C. Martin: [The Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html).

### 5. HotChocolate GraphQL with DataLoader Batching
- **Design Choice**: Code-first GraphQL schema execution powered by `IDataLoader` to eliminate the $N+1$ query problem by batching child relationship lookups into single `WHERE id IN (...)` database queries.
- **Authoritative References**:
  - Martin Fowler: [Lazy Load / N+1 Problem](https://martinfowler.com/eaaCatalog/lazyLoad.html) (*P of EAA*) — Minimizing round-trips when traversing entity relationships.
  - Lee Byron: [GraphQL DataLoader Specification](https://github.com/graphql/dataloader) — Pattern for request batching and per-request memoization.
  - ChilliCream: [HotChocolate DataLoaders Documentation](https://chillicream.com/docs/hotchocolate/v13/fetching-data/dataloader).
  - GraphQL Foundation: [Official GraphQL Specification](https://spec.graphql.org/).

### 6. Standardized Error Handling via Problem Details
- **Design Choice**: Centralized `ExceptionHandlingMiddleware` returning RFC 7807 / RFC 9457 compliant `application/problem+json` envelopes for client predictability and internal stack-trace sanitization.
- **Authoritative References**:
  - IETF: [RFC 7807: Problem Details for HTTP APIs](https://datatracker.ietf.org/doc/html/rfc7807) & [RFC 9457 (Updated RFC)](https://datatracker.ietf.org/doc/html/rfc9457).
  - Microsoft Learn: [Handle errors in ASP.NET Core web APIs](https://learn.microsoft.com/en-us/aspnet/core/web-api/handle-errors).
  - Martin Fowler: [Notification Pattern](https://martinfowler.com/articles/replaceThrowWithNotification.html) — Encapsulating errors for clean consumer communication.

### 7. Stateless Value Objects for Pagination & Filtering
- **Design Choice**: Immutable `QueryOptions` and `PagedResult<T>` records representing request parameters and paginated payloads without maintaining server-side state.
- **Authoritative References**:
  - Martin Fowler: [Value Object](https://martinfowler.com/bliki/ValueObject.html) — Immutable domain records characterized by values rather than identity.
  - Martin Fowler: [Data Transfer Object (DTO)](https://martinfowler.com/eaaCatalog/dataTransferObject.html) — Transport envelopes minimizing chatty network calls.
  - Microsoft: [Microsoft REST API Guidelines Section 9 (Collections & Paging)](https://github.com/microsoft/api-guidelines#9-collections).

### 8. Application Database Encapsulation & Domain Ownership
- **Design Choice**: Business logic, multi-entity validation, and test automation reside exclusively in the API layer, treating the database as an encapsulated Application Database and reserving database CPU for indexed data access and structural constraints.
- **Authoritative References**:
  - Martin Fowler: [Application Database](https://martinfowler.com/bliki/ApplicationDatabase.html) & [Integration Database](https://martinfowler.com/bliki/IntegrationDatabase.html) — Encapsulating data access behind a single service boundary and eliminating shared database coupling.
  - Martin Fowler: [Domain Logic and SQL](https://martinfowler.com/articles/dblogic.html) — The engineering trade-offs of domain logic in application code vs. stored procedures.
  - Martin Fowler: [The Practical Test Pyramid](https://martinfowler.com/articles/practical-test-pyramid.html) & [Microservice Testing](https://martinfowler.com/articles/microservice-testing/).
  - Martin Fowler: [Smart Endpoints and Dumb Pipes](https://martinfowler.com/articles/microservices.html#SmartEndpointsAndDumbPipes).
  - The Twelve-Factor App: [Stateless Processes & Backing Services](https://12factor.net/).
  - AWS: [AWS Well-Architected Framework: Performance Efficiency & Reliability](https://aws.amazon.com/architecture/well-architected/).

---

## REST Route Conventions

All collection routes support pagination, sorting, and field-level filtering:
```http
GET /api/{provider}/v1/{crm}/{resource}?page=1&pageSize=20&sortBy={col}&sortDirection={asc|desc}&{filterKey}={filterVal}
```

Where:
- `{provider}`: `ms` (SQL Server) or `pg` (PostgreSQL)
- `{crm}`: `salesforce`, `hubspot`, `dynamics365`
- `{resource}`: `accounts`, `contacts`, `opportunities`, `companies`, `deals`, `leads`, etc.

### Example Queries
- `GET /api/ms/v1/salesforce/accounts?page=1&pageSize=10`
- `GET /api/pg/v1/salesforce/accounts/001000000000001AAA`
- `GET /api/ms/v1/salesforce/accounts/001000000000001AAA/contacts`
- `GET /api/pg/v1/hubspot/companies?domain=example.com`
- `GET /api/ms/v1/dynamics365/opportunities?statecode=0`

### Write Endpoints & Roles (first pass: core entities)
Create, update, and delete are available for the core entities of each CRM (the remaining tables are tracked as a follow-up):

| CRM | Resources | Identifier |
| :--- | :--- | :--- |
| `salesforce` | `accounts`, `contacts`, `opportunities` | 18-char string (`001…`, `003…`, `006…`) |
| `hubspot` | `companies`, `contacts`, `deals` | `long` |
| `dynamics365` | `accounts`, `contacts`, `opportunities` | `guid` |

| Verb | Route | Success | Required role |
| :--- | :--- | :--- | :--- |
| `POST` | `/api/{provider}/v1/{crm}/{resource}` | `201` + `Location` + created entity | `admin` or `user` |
| `PUT` | `/api/{provider}/v1/{crm}/{resource}/{id}` | `200` + updated entity (full replace of writable fields) | `admin` or `user` |
| `DELETE` | `/api/{provider}/v1/{crm}/{resource}/{id}` | `204` (hard delete) | `admin` |

Problem Details errors: `400` invalid body, `401` missing/invalid token, `403` token without the required role, `404` unknown id, `409` foreign-key/unique-key violation (e.g. deleting a row that is still referenced).

**Roles** come from the JWT `roles` claim (string or array). Anonymous callers are read-only: every `GET` (and `/health`, `/graphql`, Swagger) is public, while mutations are *always* protected — there is no switch that opens them up.

| Caller | Read | Create / Update | Delete |
| :--- | :---: | :---: | :---: |
| Anonymous (no token) | ✅ | ❌ `401` | ❌ `401` |
| `user` | ✅ | ✅ | ❌ `403` |
| `admin` | ✅ | ✅ | ✅ |

Server-owned columns are never accepted from the client: identifiers are generated (`ICrmIdGenerator`), audit timestamps use `TimeProvider`, and generated columns (`Salesforce.Contact.Name`, `Salesforce.Opportunity.ExpectedRevenue`, Dynamics `fullname`) are derived. Because JWT subjects do not map to CRM users, the record's `ownerId` doubles as creator/modifier on the audit columns.

```bash
# Mint a local-development HS256 token (same secret/issuer/audience as appsettings.json) and call the API
TOKEN=$(../crm-infra/scripts/dev-token.sh admin)
curl -X POST http://localhost:5172/api/pg/v1/hubspot/companies \
  -H "Authorization: Bearer ${TOKEN}" -H "Content-Type: application/json" \
  -d '{"name":"Acme"}'
```
Signing key, issuer, and audience are configured under `Authentication:Jwt` (`Authentication__Jwt__SigningKey` etc. as environment variables). Replace the dev signing key before any real deployment.

---

## Solution Layout

```
CrmApiCSharp/
├── CrmApiCSharp.slnx
├── README.md
├── CODING_STANDARDS.md
├── Dockerfile                     # Multi-stage, non-root, cloud-agnostic container image
├── .env.example                   # Container runtime contract (copy to git-ignored .env)
├── bruno/                         # Git-native API test collection & environments
├── src/
│   ├── Domain/                    # Pure entity records, write-request records & value objects (PagedResult, QueryOptions, CrmRoles)
│   ├── Infrastructure/            # DbConnectionFactory, SqlKata compilers, Dapper repositories
│   ├── Application/               # Query services, filtering & relationship resolvers
│   └── Api/                       # ASP.NET Core controllers, OpenAPI / Swagger, HotChocolate GraphQL
└── tests/
    ├── Infrastructure.Tests/      # SqlKata query compilation tests
    └── Api.Tests/                 # WebApplicationFactory integration tests
```

---

## Getting Started

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/)
- Microsoft SQL Server (`CRMDatabaseMS`) and/or PostgreSQL (`CRMDatabasePG`) instances.
- [Bruno CLI or GUI](https://www.usebruno.com/) (optional, for running the API collection).

### Build and Run
```bash
cd CrmApiCSharp
dotnet build CrmApiCSharp.slnx
dotnet run --project src/Api
```
Visit Swagger UI at `http://localhost:5172/swagger` or GraphQL Banana Cake Pop at `http://localhost:5172/graphql`.

### Run as a Container
The same image runs on local Docker, AWS ECS Fargate, and Azure Container Apps. All settings come from environment variables documented in [.env.example](file:///Users/cy/codes/localstack-reference-architecture/CrmApiCSharp/.env.example):
```bash
cp .env.example .env
docker build -t crm-api-csharp .
docker run --rm -p 8081:8080 --env-file .env crm-api-csharp
curl http://localhost:8081/health
```
To deploy every runtime × engine permutation locally or to LocalStack for AWS / Azure, see [crm-infra/README.md](file:///Users/cy/codes/localstack-reference-architecture/crm-infra/README.md).

### Testing Endpoints with Bruno
Run the automated Bruno test suite against your active database provider:
```bash
# Against SQL Server (mints dev admin/user tokens for the 06-Mutations suite)
./bruno/run_tests.sh Local-MsSql

# Against PostgreSQL
./bruno/run_tests.sh Local-Postgres
```
The `06-Mutations` suite reads `CRM_ADMIN_TOKEN` / `CRM_USER_TOKEN` from the environment; `run_tests.sh` fills them via `crm-infra/scripts/dev-token.sh` when unset.
See [bruno/README.md](file:///Users/cy/codes/localstack-reference-architecture/CrmApiCSharp/bruno/README.md) for full collection documentation.
