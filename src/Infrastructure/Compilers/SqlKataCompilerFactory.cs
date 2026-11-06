using Domain.Common;
using SqlKata.Compilers;

namespace Infrastructure.Compilers;

/// <summary>
/// Singleton compiler factory providing thread-safe instances of SqlServerCompiler and PostgresCompiler.
/// </summary>
public sealed class SqlKataCompilerFactory : ISqlKataCompilerFactory
{
    private static readonly SqlServerCompiler SqlServerCompilerInstance = new();
    private static readonly PostgresCompiler PostgresCompilerInstance = new();

    public Compiler GetCompiler(DatabaseProvider provider) =>
        provider switch
        {
            DatabaseProvider.Ms => SqlServerCompilerInstance,
            DatabaseProvider.Pg => PostgresCompilerInstance,
            _ => throw new ArgumentOutOfRangeException(
                nameof(provider),
                provider,
                $"Unsupported database provider: {provider}")
        };
}
