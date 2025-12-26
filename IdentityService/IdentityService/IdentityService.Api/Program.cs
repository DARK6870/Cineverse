using Auth.Authentication;
using IdentityService.Application;
using IdentityService.Mongo;
using Infrastructure.Context;
using Infrastructure.Logging;
using Infrastructure.MediatR;
using Infrastructure.Mongo;
using Infrastructure.Mongo.Migrations;
using Infrastructure.WebApi.GraphQl;
using Infrastructure.WebApi.Rest;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.AddInfrastructureLogging();

// ------ Configure services ------ //
builder.Services
    .AddRestApi()
    .AddAuth(configuration)
    .AddUserContext()
    .AddMemoryCache()
    .AddMongoDatabase(configuration)
    .AddMongoRepositories()
    .AddMongoMigrations()
    .AddApplicationServices()
    .AddPipelineBehaviours()
    .AddGraphQLServer()
    .ConfigureGraphQl()
    .AddGraphQlQueries()
    .AddGraphQlMutations()
    ;

// ------ Configure WebApplication ------ //
var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.MapScalar();

app.UseUserContextMiddleware();
app.MapControllers();
app.MapGraphQlApi();
app.UseGraphQlStatusCodeMiddleware();

// ------ Execute Migrations ------ //
await app.ExecuteMigrationsAsync();

app.Run();

// TODO: add fluent validation