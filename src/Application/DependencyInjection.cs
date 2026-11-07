using Application.Interfaces;
using Application.Services;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddInfrastructureServices();
        services.AddScoped<ISalesforceQueryService, SalesforceQueryService>();
        services.AddScoped<IHubSpotQueryService, HubSpotQueryService>();
        services.AddScoped<IDynamics365QueryService, Dynamics365QueryService>();
        services.AddScoped<ISalesforceCommandService, SalesforceCommandService>();
        services.AddScoped<IHubSpotCommandService, HubSpotCommandService>();
        services.AddScoped<IDynamics365CommandService, Dynamics365CommandService>();

        return services;
    }
}
