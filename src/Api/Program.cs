using System.Text;
using Api.GraphQL;
using Api.GraphQL.Types;
using Api.Middleware;
using Application;
using Infrastructure.Connections;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

const string ApiVersion = "v1";
const string ApiTitle = "CrmApiCSharp - Multi-Provider Relational CRM Web API";
const string SwaggerEndpointPath = "/swagger/v1/swagger.json";
const string SwaggerRoutePrefix = "swagger";
const string EnableSwaggerConfigKey = "EnableSwagger";
const string AuthSectionName = "Authentication:Jwt";

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

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(_ => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference("Bearer"),
            new List<string>()
        }
    });
});

// Configure JWT & OAuth2 Authentication and Authorization
var jwtSection = builder.Configuration.GetSection(AuthSectionName);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var authority = jwtSection["Authority"] ?? "https://auth.localstack.local";
        var audience = jwtSection["Audience"] ?? "crm-api";
        var issuer = jwtSection["Issuer"] ?? authority;
        var signingKey = jwtSection["SigningKey"];

        options.Authority = authority;
        options.Audience = audience;
        options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = !string.IsNullOrEmpty(issuer),
            ValidIssuer = issuer,
            ValidateAudience = !string.IsNullOrEmpty(audience),
            ValidAudience = audience,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = !string.IsNullOrEmpty(signingKey),
            IssuerSigningKey = !string.IsNullOrEmpty(signingKey)
                ? new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey))
                : null
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, DynamicAuthorizationPolicyProvider>();

builder.Services.AddApplicationServices();

// HotChocolate GraphQL foundation with Authorization
builder.Services
    .AddGraphQLServer()
    .AddAuthorization()
    .AddQueryType<Query>()
    .AddType<SalesforceAccountType>()
    .AddType<SalesforceContactType>()
    .AddType<HubSpotCompanyType>()
    .AddType<HubSpotContactType>()
    .AddType<Dynamics365AccountType>()
    .AddType<Dynamics365ContactType>()
    .AddMaxExecutionDepthRule(10)
    .ModifyPagingOptions(opt => opt.MaxPageSize = 100)
    .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = builder.Environment.IsDevelopment());

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment() || app.Configuration.GetValue(EnableSwaggerConfigKey, true))
{
    app.UseSwagger();
    app.UseSwaggerUI(static c =>
    {
        c.SwaggerEndpoint(SwaggerEndpointPath, $"CrmApiCSharp {ApiVersion}");
        c.RoutePrefix = SwaggerRoutePrefix;
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapGraphQL();

// Fail fast on missing database configuration outside development mode
using (var scope = app.Services.CreateScope())
{
    var connectionFactory = scope.ServiceProvider.GetRequiredService<IDbConnectionFactory>();
    connectionFactory.ValidateConfiguration(allowFallbackDefaults: app.Environment.IsDevelopment());
}

app.Run();

// Required for WebApplicationFactory in integration tests
public partial class Program { }
