using Auth.Authentication;
using Cineverse.Application;
using Cineverse.Mongo;
using Infrastructure.Context;
using Infrastructure.Logging;
using Infrastructure.MediatR;
using Infrastructure.Mongo;
using Infrastructure.Mongo.Migrations;
using Infrastructure.WebApi.GraphQl;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var assembly = typeof(Program).Assembly;

// ------ Configure logger ------ //
builder.AddInfrastructureLogging();

// ------ Configure services ------ //
builder.Services
    .AddHttpContextAccessor()
    .AddMongoDatabase(configuration)
    .AddMongoRepositories()
    .AddMongoMigrations()
    .AddAuth(configuration)
    .AddUserContext()
    .AddApplicationServices(configuration)
    .AddPipelineBehaviours()
    .AddGraphQLServer()
    .ConfigureGraphQl()
    .AddGraphQlQueriesFromAssembly(assembly)
    .AddGraphQlMutationsFromAssembly(assembly)
    ;

// ------ Configure WebApplication ------ //
var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGraphQlApi();

app.UseUserContextMiddleware();
app.UseGraphQlStatusCodeMiddleware();

// ------ Execute Migrations ------ //
await app.ExecuteMigrationsAsync();

app.Run();