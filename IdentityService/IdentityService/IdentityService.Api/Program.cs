using Auth.Authentication;
using IdentityService.Api.Extensions;
using IdentityService.Application;
using IdentityService.Application.Common.Extensions;
using IdentityService.Mongo;
using Infrastructure.Context;
using Infrastructure.HealthCheck;
using Infrastructure.Logging;
using Infrastructure.MediatR;
using Infrastructure.Mongo;
using Infrastructure.Mongo.Migrations;
using Infrastructure.WebApi.GraphQl;
using Infrastructure.WebApi.Rest;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;
var assembly = typeof(Program).Assembly;

builder.AddInfrastructureLogging();

// configure services
services
    .AddAuth(configuration)
    .AddExternalProviders(services, configuration);

services
    .AddRestApi()
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

// configure web application
var app = builder.Build();
app.ConfigureWebApplication();

// execute migrations
await app.ExecuteMigrationsAsync();

app.Run();