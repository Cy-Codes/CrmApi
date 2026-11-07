using System.Collections.Immutable;
using System.Data.Common;
using Domain.Common;
using Domain.Dynamics365;
using Infrastructure.Compilers;
using Infrastructure.Connections;
using Infrastructure.Identifiers;
using Infrastructure.Repositories;
using SqlKata;
using HubSpotCompanyWriteRequest = Domain.HubSpot.CompanyWriteRequest;
using SalesforceAccountWriteRequest = Domain.Salesforce.AccountWriteRequest;
using SalesforceOpportunityWriteRequest = Domain.Salesforce.OpportunityWriteRequest;

namespace Infrastructure.Tests;

public sealed class WriteCommandTests
{
    private static readonly DateTime FixedUtcNow = new(2026, 11, 7, 12, 0, 0, DateTimeKind.Utc);
    private static readonly Guid OwnerGuid = Guid.Parse("22222222-2222-2222-2222-222222222222");
    private static readonly Guid BusinessUnitGuid = Guid.Parse("33333333-3333-3333-3333-333333333333");

    private readonly ISqlKataCompilerFactory _factory = new SqlKataCompilerFactory();

    [Fact]
    public void SqlServerCompiler_CompilesParameterizedInsertUpdateDelete()
    {
        var compiler = _factory.GetCompiler(DatabaseProvider.Ms);
        var values = ImmutableDictionary<string, object?>.Empty.Add("name", "Acme").Add("ownerId", null);

        var insert = compiler.Compile(new Query("HubSpot.Company").AsInsert(values));
        var update = compiler.Compile(new Query("HubSpot.Company").Where("companyId", 7L).AsUpdate(values));
        var delete = compiler.Compile(new Query("HubSpot.Company").Where("companyId", 7L).AsDelete());

        Assert.StartsWith("INSERT INTO [HubSpot].[Company] (", insert.Sql);
        Assert.Contains("[name]", insert.Sql);
        Assert.Contains("[ownerId]", insert.Sql);
        Assert.DoesNotContain("Acme", insert.Sql);
        Assert.StartsWith("UPDATE [HubSpot].[Company] SET ", update.Sql);
        Assert.Contains("[name] =", update.Sql);
        Assert.Contains("WHERE [companyId] =", update.Sql);
        Assert.StartsWith("DELETE FROM [HubSpot].[Company] WHERE [companyId] =", delete.Sql);
    }

    [Fact]
    public void PostgresCompiler_CompilesParameterizedInsertUpdateDelete()
    {
        var compiler = _factory.GetCompiler(DatabaseProvider.Pg);
        var values = ImmutableDictionary<string, object?>.Empty.Add("name", "Acme").Add("ownerId", null);

        var insert = compiler.Compile(new Query("HubSpot.Company").AsInsert(values));
        var update = compiler.Compile(new Query("HubSpot.Company").Where("companyId", 7L).AsUpdate(values));
        var delete = compiler.Compile(new Query("HubSpot.Company").Where("companyId", 7L).AsDelete());

        Assert.StartsWith("INSERT INTO \"HubSpot\".\"Company\" (", insert.Sql);
        Assert.Contains("\"name\"", insert.Sql);
        Assert.Contains("\"ownerId\"", insert.Sql);
        Assert.DoesNotContain("Acme", insert.Sql);
        Assert.StartsWith("UPDATE \"HubSpot\".\"Company\" SET ", update.Sql);
        Assert.Contains("\"name\" =", update.Sql);
        Assert.StartsWith("DELETE FROM \"HubSpot\".\"Company\" WHERE \"companyId\" =", delete.Sql);
    }

    [Fact]
    public void SalesforceAccount_ToColumns_ExcludesServerOwnedColumns()
    {
        var columns = new SalesforceAccountWriteRequest { Name = "Acme", OwnerId = "005000000000001AAA" }.ToColumns();

        Assert.Equal("Acme", columns["Name"]);
        Assert.Equal("005000000000001AAA", columns["OwnerId"]);
        Assert.All(
            new[] { "Id", "CreatedDate", "CreatedById", "LastModifiedDate", "LastModifiedById", "IsDeleted" },
            column => Assert.False(columns.ContainsKey(column), column));
    }

    [Fact]
    public void SalesforceOpportunity_ToColumns_NeverWritesGeneratedExpectedRevenue_AndKeepsUnsetFlagsOutOfTheCommand()
    {
        var request = new SalesforceOpportunityWriteRequest
        {
            Name = "Deal",
            StageName = "Prospecting",
            CloseDate = new DateTime(2026, 12, 31, 13, 45, 0),
            OwnerId = "005000000000001AAA"
        };

        var columns = request.ToColumns();

        Assert.False(columns.ContainsKey("ExpectedRevenue"));
        Assert.False(columns.ContainsKey("IsClosed"));
        Assert.False(columns.ContainsKey("IsWon"));
        Assert.Equal(new DateTime(2026, 12, 31), columns["CloseDate"]);

        var closed = request with { IsClosed = true, IsWon = true };
        Assert.Equal(true, closed.ToColumns()["IsClosed"]);
    }

    [Fact]
    public void SalesforceCreateAudit_UsesOwnerAsCreatorAndModifier()
    {
        var columns = ImmutableDictionary<string, object?>.Empty
            .WithCreateAudit("001ABC", "005OWNER", FixedUtcNow);

        Assert.Equal("001ABC", columns["Id"]);
        Assert.Equal("005OWNER", columns["CreatedById"]);
        Assert.Equal("005OWNER", columns["LastModifiedById"]);
        Assert.Equal(FixedUtcNow, columns["CreatedDate"]);
        Assert.Equal(FixedUtcNow, columns["LastModifiedDate"]);
    }

    [Fact]
    public void HubSpotCompany_ToColumns_OmitsUnsetOptionalDefaults_AndWritesNullableAsNull()
    {
        var columns = new HubSpotCompanyWriteRequest { Name = "Acme" }.ToColumns();

        Assert.False(columns.ContainsKey("lifecyclestage"));
        Assert.False(columns.ContainsKey("archived"));
        Assert.True(columns.ContainsKey("ownerId"));
        Assert.Null(columns["ownerId"]);
    }

    [Fact]
    public void HubSpotAudit_StampsCreateAndUpdateTimestamps()
    {
        var created = ImmutableDictionary<string, object?>.Empty.WithCreateAudit("companyId", 42L, FixedUtcNow);
        var updated = ImmutableDictionary<string, object?>.Empty.WithUpdateAudit(FixedUtcNow);

        Assert.Equal(42L, created["companyId"]);
        Assert.Equal(FixedUtcNow, created["createdAt"]);
        Assert.Equal(FixedUtcNow, created["updatedAt"]);
        Assert.Single(updated);
        Assert.Equal(FixedUtcNow, updated["updatedAt"]);
    }

    [Fact]
    public void WriteColumnExtensions_AsUtc_NormalizesEveryDateTimeKind()
    {
        var unspecified = new DateTime(2026, 1, 1, 10, 0, 0, DateTimeKind.Unspecified);

        Assert.Equal(DateTimeKind.Utc, unspecified.AsUtc().Kind);
        Assert.Equal(unspecified.Ticks, unspecified.AsUtc().Ticks);
        Assert.Equal(DateTimeKind.Utc, new DateTime(2026, 1, 1, 10, 0, 0, DateTimeKind.Local).AsUtc().Kind);
        Assert.Null(((DateTime?)null).AsUtc());
    }

    [Fact]
    public void DynamicsContact_ToColumns_DerivesFullName()
    {
        var withBoth = new ContactWriteRequest
        {
            FirstName = "Ada",
            LastName = "Lovelace",
            OwnerId = OwnerGuid,
            Owningbusinessunit = BusinessUnitGuid
        };
        var lastNameOnly = withBoth with { FirstName = " " };

        Assert.Equal("Ada Lovelace", withBoth.ToColumns()["fullname"]);
        Assert.Equal("Lovelace", lastNameOnly.ToColumns()["fullname"]);
    }

    [Fact]
    public void DynamicsAudit_UsesOwnerAsCreatorAndModifier()
    {
        var columns = ImmutableDictionary<string, object?>.Empty
            .WithCreateAudit("accountid", Guid.Empty, OwnerGuid, FixedUtcNow);

        Assert.Equal(OwnerGuid, columns["createdby"]);
        Assert.Equal(OwnerGuid, columns["modifiedby"]);
        Assert.Equal(FixedUtcNow, columns["createdon"]);
        Assert.Equal(FixedUtcNow, columns["modifiedon"]);
    }

    [Theory]
    [InlineData("001")]
    [InlineData("003")]
    [InlineData("006")]
    public void IdGenerator_SalesforceId_Has18CharactersAndKeyPrefix(string prefix)
    {
        var generator = new CrmIdGenerator(TimeProvider.System);

        var id = generator.NewSalesforceId(prefix);

        Assert.Equal(CrmIdGenerator.SalesforceIdLength, id.Length);
        Assert.StartsWith(prefix, id);
        Assert.Matches("^[A-Z0-9]{18}$", id);
        Assert.NotEqual(id, generator.NewSalesforceId(prefix));
    }

    [Theory]
    [InlineData("")]
    [InlineData("0011")]
    public void IdGenerator_SalesforceId_RejectsInvalidPrefix(string prefix) =>
        Assert.Throws<ArgumentException>(() => new CrmIdGenerator(TimeProvider.System).NewSalesforceId(prefix));

    [Fact]
    public void IdGenerator_HubSpotId_IsPositiveAndTimeOrdered()
    {
        var earlier = new CrmIdGenerator(new FixedTimeProvider(FixedUtcNow)).NewHubSpotId();
        var later = new CrmIdGenerator(new FixedTimeProvider(FixedUtcNow.AddSeconds(1))).NewHubSpotId();

        Assert.True(earlier > 0);
        Assert.True(later > earlier);
    }

    [Theory]
    [InlineData("bad column")]
    [InlineData("name; DROP TABLE x")]
    [InlineData("na'me")]
    public async Task BaseRepository_RejectsUnsafeColumnNames_BeforeOpeningAConnection(string column)
    {
        var repository = new BaseRepository(new UnreachableConnectionFactory(), _factory);
        var values = ImmutableDictionary<string, object?>.Empty.Add(column, "x");

        await Assert.ThrowsAsync<ArgumentException>(() =>
            repository.ExecuteInsertAsync(DatabaseProvider.Pg, "HubSpot.Company", values));
        await Assert.ThrowsAsync<ArgumentException>(() =>
            repository.ExecuteUpdateAsync(DatabaseProvider.Pg, "HubSpot.Company", "companyId", 1L, values));
    }

    [Fact]
    public async Task BaseRepository_RejectsUnsafeTableAndIdColumnNames()
    {
        var repository = new BaseRepository(new UnreachableConnectionFactory(), _factory);
        var values = ImmutableDictionary<string, object?>.Empty.Add("name", "x");

        await Assert.ThrowsAsync<ArgumentException>(() =>
            repository.ExecuteInsertAsync(DatabaseProvider.Ms, "HubSpot.Company; --", values));
        await Assert.ThrowsAsync<ArgumentException>(() =>
            repository.ExecuteDeleteAsync(DatabaseProvider.Ms, "HubSpot.Company", "companyId OR 1=1", 1L));
    }

    private sealed class FixedTimeProvider(DateTime utcNow) : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => new(utcNow, TimeSpan.Zero);
    }

    private sealed class UnreachableConnectionFactory : IDbConnectionFactory
    {
        public DbConnection CreateConnection(DatabaseProvider provider) =>
            throw new InvalidOperationException("No database connection is available in unit tests.");

        public void ValidateConfiguration(bool allowFallbackDefaults = false)
        {
        }
    }
}
