using Auth.Authentication;
using IdentityService.Application;
using IdentityService.Mongo;
using Infrastructure.Context;
using Infrastructure.HealthCheck;
using Infrastructure.Logging;
using Infrastructure.MediatR;
using Infrastructure.Mongo;
using Infrastructure.Mongo.Migrations;
using Infrastructure.WebApi.Cors;
using Infrastructure.WebApi.GraphQl;
using Infrastructure.WebApi.Rest;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var assembly = typeof(Program).Assembly;

builder.AddInfrastructureLogging();

// ------ Configure services ------ //
builder.Services
    .AddRestApi()
    .AddCorsPolicy(configuration)
    .AddAuth(configuration)
    .AddUserContext()
    .AddMemoryCache()
    .AddMongoDatabase(configuration)
    .AddMongoRepositories()
    .AddMongoMigrations()
    .AddApplicationServices(configuration)
    .AddPipelineBehaviours()
    .AddInfrastructureHealthChecks(options =>
    {
        options.IncludeMongoDb = true;
        options.IncludeKafka = true;
    })
    .AddGraphQLServer()
    .ConfigureGraphQl()
    .AddGraphQlQueriesFromAssembly(assembly)
    .AddGraphQlMutationsFromAssembly(assembly)
    ;

// ------ Configure WebApplication ------ //
var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.MapScalar();

app.UseCorsPolicy();
app.UseAuthentication();
app.UseAuthorization();

app.UseUserContextMiddleware();

app.MapControllers();
app.MapGraphQlApi();

app.UseGraphQlStatusCodeMiddleware();
app.UseRestApiExceptionHandlerMiddleware();

app.MapHealthCheck();

// ------ Execute Migrations ------ //
await app.ExecuteMigrationsAsync();

app.Run();