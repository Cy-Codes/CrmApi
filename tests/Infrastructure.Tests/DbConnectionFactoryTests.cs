using Domain.Common;
using Infrastructure.Connections;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Tests;

public class DbConnectionFactoryTests
{
    [Fact]
    public void CreateConnection_ResolvesDefaultFallbackConnectionStrings()
    {
        var config = new ConfigurationBuilder().Build();
        var factory = new DbConnectionFactory(config);

        var msConn = factory.CreateConnection(DatabaseProvider.Ms);
        var pgConn = factory.CreateConnection(DatabaseProvider.Pg);

        Assert.NotNull(msConn);
        Assert.NotNull(pgConn);
        Assert.Contains("CRMDatabaseMS", msConn.ConnectionString);
        Assert.Contains("CRMDatabase", pgConn.ConnectionString);
    }

    [Fact]
    public void ValidateConfiguration_ThrowsWhenMissingAndFallbackNotAllowed()
    {
        var config = new ConfigurationBuilder().Build();
        var factory = new DbConnectionFactory(config);

        var ex = Assert.Throws<InvalidOperationException>(() => factory.ValidateConfiguration(allowFallbackDefaults: false));
        Assert.Contains("Missing required database connection string configuration", ex.Message);
    }

    [Fact]
    public void ValidateConfiguration_SucceedsWhenConfiguredExplicitly()
    {
        var inMemory = new Dictionary<string, string?>
        {
            ["ConnectionStrings:CRMDatabaseMS"] = "Server=prod-sql;Database=CRMDatabaseMS;User Id=sa;Password=secret;",
            ["ConnectionStrings:CRMDatabasePG"] = "Host=prod-pg;Port=5432;Database=CRMDatabase;Username=postgres;Password=secret;"
        };

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemory)
            .Build();

        var factory = new DbConnectionFactory(config);
        factory.ValidateConfiguration(allowFallbackDefaults: false);

        var msConn = factory.CreateConnection(DatabaseProvider.Ms);
        var pgConn = factory.CreateConnection(DatabaseProvider.Pg);

        Assert.Contains("prod-sql", msConn.ConnectionString);
        Assert.Contains("prod-pg", pgConn.ConnectionString);
    }

    [Fact]
    public void ValidateConfiguration_SucceedsWhenFallbackAllowed()
    {
        var config = new ConfigurationBuilder().Build();
        var factory = new DbConnectionFactory(config);

        // Should not throw
        factory.ValidateConfiguration(allowFallbackDefaults: true);
    }
}
