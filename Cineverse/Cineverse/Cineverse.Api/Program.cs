using Cineverse.Api.GraphQl;
using Cineverse.Api.GraphQl.Middlewares.StatusCodeMiddleware;
using Cineverse.Application;
using Cineverse.Identity;
using Cineverse.Identity.Middlewares;
using Cineverse.Infrastructure;
using Cineverse.Infrastructure.Cors;
using Cineverse.Infrastructure.Logging;
using Cineverse.Mongo;
using Cineverse.Mongo.Migrations;
using Cineverse.Notifications;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// ====== Configure logger ======
builder.AddSerilogLoggingWithOpenTelemetry(configuration);


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

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<UserContextMiddleware>();
app.UseMiddleware<GraphQlStatusCodeMiddleware>();

app.MapCineverseGraphQl();



// ====== Execute Migrations ======
await app.ExecuteMigrations();

app.Run();