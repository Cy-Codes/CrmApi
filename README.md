# CrmApi - Multi-Provider CRM Web API

A high-performance C# .NET 10 Web API serving relational data from two enterprise CRM database engines:
- **Microsoft SQL Server** (`CRMDatabaseMS`)
- **PostgreSQL** (`CRMDatabasePG`)

Covering **37 tables** across three CRM schemas:
- **Salesforce (15 entities)**: `Account`, `Contact`, `Opportunity`, `Lead`, `Campaign`, `Case`, `User`, `Event`, `Task`, `Product2`, `Pricebook2`, `PricebookEntry`, `OpportunityLineItem`, `AccountContactRelation`, `CampaignMember`.
- **HubSpot (11 entities)**: `Company`, `Contact`, `Deal`, `Ticket`, `Owner`, `Product`, `LineItem`, `Pipeline`, `DealStage`, `Engagement`, `ObjectAssociation`.
- **Dynamics 365 (11 entities)**: `Account`, `Contact`, `Opportunity`, `Lead`, `Incident`, `Product`, `PriceLevel`, `OpportunityProduct`, `BusinessUnit`, `SystemUser`, `ActivityPointer`.

---

## Architectural Highlights & Authoritative Sources

1. **Dapper Micro-ORM**:
   - High-throughput execution directly over ADO.NET with minimal heap allocations.
   - [Dapper GitHub Repository & Benchmarks](https://github.com/DapperLib/Dapper#benchmarks)
2. **SqlKata Multi-Dialect Query Builder**:
   - Compiles dynamic queries with parameterized values into SQL Server T-SQL (`SqlServerCompiler`) and PostgreSQL dialect (`PostgresCompiler`).
   - [SqlKata Documentation](https://sqlkata.com/docs) & [SqlKata GitHub Repository](https://github.com/sqlkata/querybuilder)
3. **Microsoft REST API Guidelines & Routing**:
   - Structured versioned resource paths `/api/{provider}/v1/{crm}/{resource}` allowing runtime provider selection (`ms` or `pg`).
   - [Microsoft REST API Guidelines](https://github.com/microsoft/api-guidelines) & [ASP.NET Core Web API Best Practices](https://learn.microsoft.com/en-us/aspnet/core/web-api/)
4. **Clean Architecture & Separation of Concerns**:
   - Decoupled `Domain`, `Infrastructure`, `Application`, and `Api` layers for maintainability and transport flexibility.
   - [Common Web Application Architectures (.NET Architecture Guides)](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures)
5. **HotChocolate GraphQL Ready**:
   - Schema and DataLoader foundation for zero N+1 batch querying.
   - [ChilliCream HotChocolate Documentation](https://chillicream.com/docs/hotchocolate)

---

## REST Route Conventions

All collection routes support pagination, sorting, and field-level filtering:
```
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

---

## Solution Layout

```
CrmApi/
├── CrmApi.slnx
├── README.md
├── docs/
│   └── architecture-decisions.md
├── bruno/                   # Bruno API test collection & environments
├── src/
│   ├── Domain/              # Pure entity models & common contracts (PagedResult, QueryOptions)
│   ├── Infrastructure/      # Connection factories, SqlKata compilers, CRM repositories
│   ├── Application/         # Query services, filtering & relationship resolvers
│   └── Api/                 # ASP.NET Core controllers, OpenAPI / Swagger, GraphQL
└── tests/
    ├── Infrastructure.Tests/# SqlKata compilation tests
    └── Api.Tests/           # WebApplicationFactory integration tests
```

---

## Getting Started

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/)
- Microsoft SQL Server (`CRMDatabaseMS`) and/or PostgreSQL (`CRMDatabasePG`) instances.
- [Bruno CLI or GUI](https://www.usebruno.com/) (optional, for running the API collection).

### Build and Run
```bash
cd CrmApi
dotnet build
dotnet run --project src/Api
```
Visit Swagger UI at `http://localhost:5172/swagger` or GraphQL Banana Cake Pop at `http://localhost:5172/graphql`.

### Testing Endpoints with Bruno
Run the automated Bruno test suite against your active database provider:
```bash
# Against SQL Server
bru run bruno -r --env Local-MsSql

# Against PostgreSQL
bru run bruno -r --env Local-Postgres
```
See [bruno/README.md](file:///Users/cy/codes/Work/CrmApi/bruno/README.md) for full collection documentation.

