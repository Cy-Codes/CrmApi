using Domain.Common;
using Infrastructure.Compilers;
using SqlKata;
using Xunit;

namespace Infrastructure.Tests;

public class SqlKataCompilerTests
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
}
