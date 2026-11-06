# CrmApiCSharp Coding Standards & Best Practices

This document is the complete, self-contained specification of both the **Universal Coding Standards** and the **.NET 10 / C# 13, ASP.NET Core, Dapper, SqlKata, HotChocolate GraphQL, and Bruno API Testing Standards** for the **CrmApiCSharp** repository.

---

# Part I: Universal Coding Standards

```
┌────────────────────────────────────────────────────────────────────────┐
│                        Universal Standards Pillars                     │
├───────────────────┬───────────────────┬────────────────────────────────┤
│  Functional First │ Immutable Always  │ Explicit Side Effects          │
│  Maps over Loops  │ Records & Freezes │ Command/Query Segregation     │
├───────────────────┴──────┬────────────┴────────────────────────────────┤
│  Helper & Extension Fns  │ Zero Magic Numbers & Anti-Pattern Tolerance │
└──────────────────────────┴─────────────────────────────────────────────┘
```

## 1.Core Universal Principles

1. **Functional Logic Over Imperative Loops**:
   - Choose declarative transformations (`map`, `filter`, `reduce`, LINQ `.Select()`, `.Where()`, `.Aggregate()`, `.ToFrozenDictionary()`, `.ToImmutableArray()`) over state-mutating imperative loops (`for`, `foreach`, `while`).
   - Do not allocate an empty collection, loop over input, and imperatively append elements (`.Add()`).
   - **Acceptable Exceptions for Loops**: Streaming unbuffered database/network readers (`while (await reader.ReadAsync(ct))`), benchmark-proven hot paths where closure allocations cause measured GC pressure, or early-exit state machines. Every exception loop must include an inline comment explaining why a functional construct was rejected.
2. **Immutability by Default**:
   - All domain entities, DTOs, and configuration payloads must be deeply immutable once initialized (`record` with `init` accessors, `readonly struct`, `IReadOnlyList<T>`, `ImmutableArray<T>`, `IReadOnlyDictionary<TKey, TValue>`, `FrozenDictionary<TKey, TValue>`).
   - Pure functions must never mutate input parameters.
3. **Explicit Demarcation of Side Effects (Functional Core, Imperative Shell)**:
   - **Queries (Pure / Read-Only)**: Idempotent, side-effect free (`Get*`, `Find*`, `Parse*`, `Calculate*`, `To*`, `Apply*`).
   - **Commands (Side-Effecting)**: Explicitly mutate state or perform I/O (`Execute*`, `Create*`, `Update*`, `Delete*`, `Save*`, `Write*`).
   - Isolate database I/O and HTTP stream mutation to explicit outer boundaries.
4. **Zero Magic Numbers & String Literals**:
   - Ban raw numeric or string literals in code (`404`, `503`, `20`, `100`, `"asc"`, `"ms"`).
   - Replace with `StatusCodes.*`, `QueryOptions.DefaultPageSize`, `DatabaseProvider` enums, and `nameof(...)`.
5. **Eliminate Redundancy via Helper & Extension Functions**:
   - Consolidate repeated boilerplate into reusable pure helper functions, preferring `static` **extension methods** (`public static class ...Extensions`) where supported.

---

# Part II: C# 13 / .NET 10 & API Specific Standards

## 2. Functional-First Programming in C# 13

### 2.1 Maps and Projections Over Loops
Use LINQ projection pipelines instead of imperative `foreach` or `for` loops:

```csharp
// ❌ ANTI-PATTERN: Imperative loop with mutable accumulator
var filters = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
foreach (var queryParam in Request.Query)
{
    if (ReservedQueryParameterNames.Contains(queryParam.Key)) continue;
    filters[queryParam.Key] = queryParam.Value.ToString();
}

// ✅ BEST PRACTICE: Declarative functional pipeline in ControllerResultExtensions
var filters = queryCollection
    .Where(static kv => !ReservedQueryParameterNames.Contains(kv.Key))
    .ToFrozenDictionary(
        static kv => kv.Key,
        static kv => (string?)kv.Value.ToString(),
        StringComparer.OrdinalIgnoreCase);
```

### 2.2 Pattern Matching & Expressions Over Statements
Favor `switch` expressions, relational patterns, and positional patterns over nested `if / else` cascades:

```csharp
// ✅ BEST PRACTICE: Concise switch expression
public static DatabaseProvider ToDatabaseProvider(this string provider) =>
    provider?.ToLowerInvariant() switch
    {
        MsProviderCode => DatabaseProvider.Ms,
        PgProviderCode => DatabaseProvider.Pg,
        _ => throw new ArgumentException(
            $"Invalid provider '{provider}'. Must be '{MsProviderCode}' or '{PgProviderCode}'.",
            nameof(provider))
    };
```

---

## 3. Deep Immutability & Stateless Services

### 3.1 Domain Models and DTOs
All domain entities and Data Transfer Objects (DTOs) must be defined as `record` or `record struct` using `init` accessors. Mutable `set;` properties are forbidden on entity contracts.

### 3.2 Immutable and Read-Only Collections
- Expose `IReadOnlyList<T>` or `ImmutableArray<T>` for ordered sequences.
- Expose `IReadOnlyDictionary<TKey, TValue>` or `FrozenDictionary<TKey, TValue>` for key-value lookups.
- Never return mutable `List<T>` or `Dictionary<TKey, TValue>` from repositories or services.

### 3.3 Sealed Stateless Services
All concrete controllers, services, repositories, factories, and middleware classes must be stateless, thread-safe, and marked `sealed` (`public sealed class ...`) with `readonly` injected dependencies.

---

## 4. Explicit Demarcation of Side Effects

### 4.1 Command-Query Segregation (CQS)

| Category | Characteristics | Naming Convention | Return Type |
| :--- | :--- | :--- | :--- |
| **Query (Pure / Read-Only)** | Idempotent, safe, no state mutation, no DB writes | `Get*Async`, `Find*Async`, `Parse*`, `To*` | `Task<T?>`, `Task<IReadOnlyList<T>>`, `Task<PagedResult<T>>` |
| **Command (Side-Effecting)** | Alters DB state, opens connections, writes HTTP streams | `Execute*Async`, `Write*Async`, `Create*Async`, `Update*Async`, `Delete*Async` | `Task<CommandResult>`, `Task<int>` |

### 4.2 Isolating Database & HTTP I/O Boundaries
- In `BaseRepository`, query construction (`ApplyFilters`, `ApplySorting`, `ApplyPagination`) is 100% pure, while connection lifecycle I/O is isolated inside `ExecuteWithOpenConnectionAsync`.
- In `ExceptionHandlingMiddleware`, RFC 7807 payload creation (`CreateProblemDetails`) is a pure `static` function, while HTTP response writing is isolated inside `WriteProblemResponseAsync`.

---

## 5. Elimination of Magic Numbers & String Literals

### 5.1 Query Pagination Constants
All pagination and sorting defaults are centralized on `QueryOptions`:

```csharp
public record QueryOptions(
    int Page = QueryOptions.DefaultPage,
    int PageSize = QueryOptions.DefaultPageSize,
    string? SortBy = null,
    string SortDirection = QueryOptions.DefaultSortDirection,
    IReadOnlyDictionary<string, string?>? Filters = null
)
{
    public const int DefaultPage = 1;
    public const int DefaultPageSize = 20;
    public const int MaxPageSize = 100;
    public const string DefaultSortDirection = "asc";
    public const string DescendingSortDirection = "desc";
    public const string DefaultIdColumn = "Id";

    public static readonly QueryOptions Default = new();
}
```

### 5.2 HTTP Status Codes
Always use `StatusCodes.Status200OK`, `StatusCodes.Status400BadRequest`, `StatusCodes.Status404NotFound`, `StatusCodes.Status500InternalServerError`, and `StatusCodes.Status503ServiceUnavailable`.

---

## 6. Eliminate Redundancy via Extension & Helper Functions

Whenever logic is repeated across controllers, query builders, or tests, extract it into a pure `static` **extension method**:
- **`DatabaseProviderExtensions`**: `provider.ToDatabaseProvider()` and `provider.ToProviderCode()`.
- **`ControllerResultExtensions`**: `Request.Query.ToQueryOptions(...)`, `entityTask.ToActionResultAsync()`, and `resultTask.ToOkActionResultAsync()`.
- **`QueryFilteringExtensions`**: `query.ApplyFilters(filters).ApplySorting(sortBy, sortDirection, defaultSortColumn).ApplyPagination(page, pageSize)` and `rawValue.ParseFilterValue()`.
- **`ServiceCollectionTestExtensions`**: `services.ReplaceWithSingletonMock(mock)` in integration test fixtures.

---

## 7. Anti-Patterns & Common Mistakes Catalog for C# / .NET

1. **Shallow Immutability (`Dictionary<K,V>` in Records & `.ToList()`)**:
   - *Violation*: Using mutable `Dictionary<string, string?>` inside `QueryOptions`, returning `.ToList()` from repositories, or defining `public static QueryOptions Default => new();` (allocating on every access).
   - *Remedy*: Use `IReadOnlyDictionary<K, V>` / `FrozenDictionary<K, V>`, `.ToImmutableArray()`, and `public static readonly` fields.
2. **Culture-Sensitive Parsing (`CultureInfo.InvariantCulture` Omission)**:
   - *Violation*: Calling `decimal.TryParse(value, out var decVal)` or `int.TryParse` without `CultureInfo.InvariantCulture`.
   - *Remedy*: Always pass `NumberStyles` and `CultureInfo.InvariantCulture`.
3. **Unsealed Concrete Classes, Dead Code & Boolean Tautologies**:
   - *Violation*: Leaving leaf classes unsealed, keeping unused `_logger` fields, or leaving `if (app.Environment.IsDevelopment() || true)` in `Program.cs`.
   - *Remedy*: Mark concrete leaf classes `sealed`, remove dead fields, and use explicit configuration flags.
4. **Sync-Over-Async**:
   - *Violation*: Calling `.Result`, `.Wait()`, or `.GetAwaiter().GetResult()`.
   - *Remedy*: Always `await` and propagate `CancellationToken`.
5. **SQL Concatenation (Injection Vulnerability)**:
   - *Violation*: String interpolation in SQL queries.
   - *Remedy*: Use SqlKata query trees, `IsSafeSqlIdentifier()`, and Dapper parameterized `CommandDefinition`.
6. **N+1 Queries in GraphQL**:
   - *Violation*: Querying child relationships sequentially per parent row.
   - *Remedy*: Use HotChocolate `IDataLoader` batching.
7. **Swallowing Exceptions & 500 Information Disclosure**:
   - *Violation*: Empty `catch` blocks, or returning raw `exception.Message` on `500 Internal Server Error` in production.
   - *Remedy*: Centralize handling in `ExceptionHandlingMiddleware` and mask `500` details outside Development.

---

## 8. Automated API Testing (Bruno) & Pre-Commit Formatting

### 8.1 Bruno API Test Suite (`bruno/`)
- Partitioned by CRM domain (`00-Health`, `01-Salesforce`, `02-HubSpot`, `03-Dynamics365`, `04-GraphQL`, `05-Negative-Tests`).
- Uses environment variables (`{{baseUrl}}`, `{{provider}}`), never hardcoded URLs.

### 8.2 Pre-Commit Auto-Formatting & Unused Using Removal
- `.editorconfig` and `Directory.Build.props` enforce `EnforceCodeStyleInBuild`, `IDE0005` (remove unnecessary `using` directives), and `dotnet_sort_system_directives_first = true`.
- On every `git commit`, the pre-commit hook automatically formats staged `.cs` files, removes unused `using` statements, sorts remaining `using` directives, and re-stages the files.

---

## 9. Container Image & Runtime Configuration Contract

The same image runs unchanged on local Docker, AWS ECS Fargate, and Azure Container Apps (see [crm-infra/CODING_STANDARDS.md](file:///Users/cy/codes/localstack-reference-architecture/crm-infra/CODING_STANDARDS.md) and ADR 10–12 in [docs/architecture-decisions.md](file:///Users/cy/codes/localstack-reference-architecture/docs/architecture-decisions.md)).

- **Multi-Stage, Non-Root `Dockerfile`**: Build with `mcr.microsoft.com/dotnet/sdk`, run on `mcr.microsoft.com/dotnet/aspnet` as the non-root `$APP_UID` user. Never bake credentials or environment-specific settings into the image.
- **Environment-Only Configuration**: The container listens on `ASPNETCORE_HTTP_PORTS=8080` and reads `ConnectionStrings__CRMDatabaseMS` / `ConnectionStrings__CRMDatabasePG` (ADO.NET format). These names and shapes are shared with `crm-infra/terraform/modules/shared/crm-runtime-config` and must not change without updating it.
- **Documented Templates**: Every runtime variable is documented in [`.env.example`](file:///Users/cy/codes/localstack-reference-architecture/CrmApiCSharp/.env.example); the real `.env` is git-ignored and never committed.
- **Health Contract**: `/health` (and `/health/{ms|pg}`) must stay unauthenticated and cheap; load balancers and `crm-infra/scripts/smoke.sh` depend on it.
- **Build Context Hygiene**: Keep `.dockerignore` excluding `bin/`, `obj/`, `.env`, and test artifacts so the content-hash image tag changes only when shipped source changes.
