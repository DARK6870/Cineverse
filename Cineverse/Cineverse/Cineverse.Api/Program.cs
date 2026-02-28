using Auth.Authentication;
using Cineverse.Api.Extensions;
using Cineverse.Application;
using Cineverse.Mongo;
using Infrastructure.Context;
using Infrastructure.HealthCheck;
using Infrastructure.Logging;
using Infrastructure.MediatR;
using Infrastructure.Mongo;
using Infrastructure.Mongo.Migrations;
using Infrastructure.WebApi.GraphQl;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;
var assembly = typeof(Program).Assembly;

builder.AddInfrastructureLogging();

// configure services
services.AddAuth(configuration);

services
    .AddInfrastructureHealthChecks(options =>
    {
        options.IncludeMongoDb = true;
        options.IncludeKafka = true;
    })
    .AddMongoDatabase(configuration)
    .AddMongoRepositories()
    .AddMongoMigrations()
    .AddUserContext()
    .AddApplicationServices(configuration)
    .AddPipelineBehaviours()
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

public partial class Program;