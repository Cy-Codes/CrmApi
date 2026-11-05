using Domain.Common;
using SqlKata.Compilers;

namespace Infrastructure.Compilers;

/// <summary>
/// Singleton compiler factory providing thread-safe instances of SqlServerCompiler and PostgresCompiler.
/// </summary>
public class SqlKataCompilerFactory : ISqlKataCompilerFactory
{
    private static readonly SqlServerCompiler _sqlServerCompiler = new();
    private static readonly PostgresCompiler _postgresCompiler = new();

    public Compiler GetCompiler(DatabaseProvider provider)
    {
        return provider switch
        {
            DatabaseProvider.Ms => _sqlServerCompiler,
            DatabaseProvider.Pg => _postgresCompiler,
            _ => throw new ArgumentOutOfRangeException(nameof(provider), $"Unsupported database provider: {provider}")
        };
    }
}
