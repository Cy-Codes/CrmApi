using System.Collections.Frozen;
using Domain.Common;
using Infrastructure.Compilers;
using Infrastructure.Repositories;
using SqlKata;

namespace Infrastructure.Tests;

public sealed class SqlKataCompilerTests
{
    private readonly ISqlKataCompilerFactory _factory = new SqlKataCompilerFactory();

    [Fact]
    public void SqlServerCompiler_GeneratesBracketedIdentifiers_And_OffsetFetch()
    {
        var compiler = _factory.GetCompiler(DatabaseProvider.Ms);
        var query = new Query("Salesforce.Account")
            .Where("Industry", "Technology")
            .OrderBy("Name")
            .Skip(20)
            .Take(10);

        var result = compiler.Compile(query);

        Assert.Contains("[Salesforce].[Account]", result.Sql);
        Assert.Contains("[Industry] =", result.Sql);
        Assert.Contains("ORDER BY [Name]", result.Sql);
        Assert.Contains("OFFSET", result.Sql);
        Assert.Contains("FETCH NEXT", result.Sql);
    }

    [Fact]
    public void PostgresCompiler_GeneratesDoubleQuotedIdentifiers_And_LimitOffset()
    {
        var compiler = _factory.GetCompiler(DatabaseProvider.Pg);
        var query = new Query("Salesforce.Account")
            .Where("Industry", "Technology")
            .OrderBy("Name")
            .Skip(20)
            .Take(10);

        var result = compiler.Compile(query);

        Assert.Contains("\"Salesforce\".\"Account\"", result.Sql);
        Assert.Contains("\"Industry\" =", result.Sql);
        Assert.Contains("ORDER BY \"Name\"", result.Sql);
        Assert.Contains("LIMIT", result.Sql);
        Assert.Contains("OFFSET", result.Sql);
    }

    [Fact]
    public void CountQuery_CompilesAccuratelyAcrossProviders()
    {
        var msCompiler = _factory.GetCompiler(DatabaseProvider.Ms);
        var pgCompiler = _factory.GetCompiler(DatabaseProvider.Pg);

        var countQuery = new Query("HubSpot.Company")
            .Where("domain", "acme.com")
            .AsCount();

        var msResult = msCompiler.Compile(countQuery);
        var pgResult = pgCompiler.Compile(countQuery);

        Assert.Contains("SELECT COUNT(*)", msResult.Sql.ToUpperInvariant());
        Assert.Contains("[HubSpot].[Company]", msResult.Sql);

        Assert.Contains("SELECT COUNT(*)", pgResult.Sql.ToUpperInvariant());
        Assert.Contains("\"HubSpot\".\"Company\"", pgResult.Sql);
    }

    [Fact]
    public void QueryFilteringExtensions_ApplyFilters_IgnoresUnsafeIdentifiers_And_ParsesInvariantTypes()
    {
        var filters = new Dictionary<string, string?>
        {
            ["Industry"] = "Technology",
            ["AnnualRevenue"] = "125000.50",
            ["IsDeleted"] = "false",
            ["Name; DROP TABLE Account;--"] = "Malicious"
        }.ToFrozenDictionary();

        var compiler = _factory.GetCompiler(DatabaseProvider.Ms);
        var query = new Query("Salesforce.Account")
            .ApplyFilters(filters)
            .ApplySorting("Name", QueryOptions.DescendingSortDirection)
            .ApplyPagination(2, 10);

        var compiled = compiler.Compile(query);

        Assert.Contains("[Industry] =", compiled.Sql);
        Assert.Contains("[AnnualRevenue] =", compiled.Sql);
        Assert.Contains("[IsDeleted] =", compiled.Sql);
        Assert.DoesNotContain("DROP TABLE", compiled.Sql);
        Assert.Contains("ORDER BY [Name] DESC", compiled.Sql);
        Assert.Contains(125000.50m, compiled.Bindings);
        Assert.Equal(false, "false".ParseFilterValue());
    }

    [Theory]
    [InlineData("ms", DatabaseProvider.Ms)]
    [InlineData("MS", DatabaseProvider.Ms)]
    [InlineData("pg", DatabaseProvider.Pg)]
    [InlineData("PG", DatabaseProvider.Pg)]
    public void DatabaseProviderExtensions_ToDatabaseProvider_ParsesValidProviders(
        string rawProvider,
        DatabaseProvider expected)
    {
        Assert.Equal(expected, rawProvider.ToDatabaseProvider());
        Assert.Equal(rawProvider.ToLowerInvariant(), expected.ToProviderCode());
    }

    [Fact]
    public void QueryFilteringExtensions_ApplyPagination_ProtectsAgainstIntegerOverflow()
    {
        var compiler = _factory.GetCompiler(DatabaseProvider.Ms);
        var query = new Query("Salesforce.Account")
            .OrderBy("Name")
            .ApplyPagination(int.MaxValue, 100);

        var compiled = compiler.Compile(query);

        // MaxPage is 100,000 so offset is (100,000 - 1) * 100 = 9,999,900
        Assert.Contains("OFFSET", compiled.Sql);
        Assert.Contains("FETCH NEXT", compiled.Sql);
        Assert.Contains(9999900L, compiled.Bindings);
        Assert.Contains(100, compiled.Bindings);
    }
}
