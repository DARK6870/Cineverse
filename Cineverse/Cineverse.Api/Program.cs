using Cineverse.Api.GraphQl;
using Cineverse.Api.GraphQl.Middlewares.StatusCodeMiddleware;
using Cineverse.Application;
using Cineverse.Domain.Common.Exceptions;
using Cineverse.Identity;
using Cineverse.Identity.Middlewares;
using Cineverse.Infrastructure;
using Cineverse.Infrastructure.Cors;
using Cineverse.Infrastructure.Logging;
using Cineverse.Mongo;
using Cineverse.Mongo.Migrations;
using Cineverse.Mongo.Schemas.Entities;
using Cineverse.Notifications;

// TODO: add telemetry
var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// ====== Configure logger ======
builder.AddSerilogLogging();


// ====== Configure services ======
builder.Services
    .AddCorsPolicy(configuration, builder.Environment)
    .AddMongoDb(configuration)
    .AddMongoRepositories()
    .AddMongoMigrations()
    .AddIdentityServices(configuration)
    .AddApplicationServices()
    .AddNotificationService(configuration)
    .AddInfrastructureServices()
    .AddPipelineBehaviours()
    .AddGraphQLServer()
    .ConfigureGraphQl()
    .AddGraphQlQueries()
    .AddGraphQlMutations()
    ;


// ====== Configure WebApplication ======
var app = builder.Build();

app.UseCors(CorsConfiguration.CorsPolicy);
app.MapCineverseGraphQl();
app.UseAuthentication()
    .UseAuthorization()
    .UseMiddleware<GraphQlStatusCodeMiddleware>()
    .UseMiddleware<UserContextMiddleware>()
    ;


// ====== Execute Migrations ======
await app.ExecuteMigrations();

app.Run();