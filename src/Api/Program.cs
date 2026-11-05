using Api.GraphQL;
using Api.Middleware;
using Application;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CrmApi - Multi-Provider Relational CRM Web API",
        Version = "v1",
        Description = "High-performance .NET 10 Web API serving Salesforce, HubSpot, and Dynamics 365 relational data from Microsoft SQL Server (ms) and PostgreSQL (pg) using Dapper and SqlKata."
    });
});

builder.Services.AddApplicationServices();

// HotChocolate GraphQL foundation
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddType<Api.GraphQL.Types.SalesforceAccountType>()
    .AddType<Api.GraphQL.Types.SalesforceContactType>()
    .AddType<Api.GraphQL.Types.HubSpotCompanyType>()
    .AddType<Api.GraphQL.Types.HubSpotContactType>()
    .AddType<Api.GraphQL.Types.Dynamics365AccountType>()
    .AddType<Api.GraphQL.Types.Dynamics365ContactType>()
    .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = builder.Environment.IsDevelopment());

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment() || true)
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CrmApi v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapGraphQL();

app.Run();

// Required for WebApplicationFactory in integration tests
public partial class Program { }
