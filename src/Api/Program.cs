using Api.GraphQL;
using Api.GraphQL.Types;
using Api.Middleware;
using Application;
using Microsoft.OpenApi;

const string ApiVersion = "v1";
const string ApiTitle = "CrmApi - Multi-Provider Relational CRM Web API";
const string SwaggerEndpointPath = "/swagger/v1/swagger.json";
const string SwaggerRoutePrefix = "swagger";
const string EnableSwaggerConfigKey = "EnableSwagger";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(static c =>
{
    c.SwaggerDoc(ApiVersion, new OpenApiInfo
    {
        Title = ApiTitle,
        Version = ApiVersion,
        Description = "High-performance .NET 10 Web API serving Salesforce, HubSpot, and Dynamics 365 relational data from Microsoft SQL Server (ms) and PostgreSQL (pg) using Dapper and SqlKata."
    });
});

builder.Services.AddApplicationServices();

// HotChocolate GraphQL foundation
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddType<SalesforceAccountType>()
    .AddType<SalesforceContactType>()
    .AddType<HubSpotCompanyType>()
    .AddType<HubSpotContactType>()
    .AddType<Dynamics365AccountType>()
    .AddType<Dynamics365ContactType>()
    .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = builder.Environment.IsDevelopment());

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment() || app.Configuration.GetValue(EnableSwaggerConfigKey, true))
{
    app.UseSwagger();
    app.UseSwaggerUI(static c =>
    {
        c.SwaggerEndpoint(SwaggerEndpointPath, $"CrmApi {ApiVersion}");
        c.RoutePrefix = SwaggerRoutePrefix;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapGraphQL();

app.Run();

// Required for WebApplicationFactory in integration tests
public partial class Program { }
