using Auth.Authentication;
using Cineverse.Application;
using Cineverse.Mongo;
using Cineverse.Notifications;
using Infrastructure.Common.MediatR;
using Infrastructure.Logging;
using Infrastructure.Mongo;
using Infrastructure.Mongo.Migrations;
using Infrastructure.WebApi.Cors;
using Infrastructure.WebApi.Cors.CorsPolicies;
using Infrastructure.WebApi.GraphQl;
using Infrastructure.WebApi.UserContext;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// ------ Configure logger ------ //
builder.AddInfrastructureLogging();


// ------ Configure services ------ //
builder.Services
    .AddHttpContextAccessor()
    .AddCorsPolicy(configuration)
    .AddMongoDatabase(configuration)
    .AddMongoRepositories()
    .AddMongoMigrations()
    .AddAuth(configuration)
    .AddUserContext()
    .AddApplicationServices()
    .AddNotificationService(configuration)
    .AddPipelineBehaviours()
    .AddGraphQLServer()
    .ConfigureGraphQl()
    .AddGraphQlQueries()
    .AddGraphQlMutations()
    ;


// ------ Configure WebApplication ------ //
var app = builder.Build();

app.UseCors(nameof(DefaultCorsPolicy));

app.UseAuthentication();
app.UseAuthorization();

app.UseUserContextMiddleware();
app.UseGraphQlStatusCodeMiddleware();

app.MapGraphQlApi();


// ------ Execute Migrations ------ //
await app.ExecuteMigrationsAsync();

app.Run();