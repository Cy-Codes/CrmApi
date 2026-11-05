using Domain.Common;
using SqlKata.Compilers;

namespace Infrastructure.Compilers;

/// <summary>
/// Factory to provide the appropriate SqlKata Compiler for the given DatabaseProvider.
/// </summary>
public interface ISqlKataCompilerFactory
{
    /// <summary>
    /// Gets the SqlKata Compiler instance for the specified provider.
    /// </summary>
    /// <param name="provider">Target database engine.</param>
    /// <returns>Compiler instance (SqlServerCompiler or PostgresCompiler).</returns>
    Compiler GetCompiler(DatabaseProvider provider);
}
