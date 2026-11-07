using Domain.Common;
using Infrastructure.Compilers;
using Infrastructure.Connections;
using Infrastructure.Identifiers;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
        services.AddSingleton<ISqlKataCompilerFactory, SqlKataCompilerFactory>();
        services.TryAddSingleton(TimeProvider.System);
        services.AddSingleton<ICrmIdGenerator, CrmIdGenerator>();
        services.AddScoped<IBaseRepository, BaseRepository>();
        services.AddScoped<ISalesforceRepository, SalesforceRepository>();
        services.AddScoped<IHubSpotRepository, HubSpotRepository>();
        services.AddScoped<IDynamics365Repository, Dynamics365Repository>();

        return services;
    }
}
