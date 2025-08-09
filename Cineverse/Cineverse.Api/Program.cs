using Cineverse.API;
using Cineverse.Application;
using Cineverse.Infrastructure;
using Cineverse.Infrastructure.Authentication;
using Cineverse.Infrastructure.Common.Configurations;
using Cineverse.Infrastructure.GraphQl;
using Cineverse.Mongo;
using Cineverse.Mongo.Migrations;
using Cineverse.Notifications;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure logger
Log.Logger = SerilogConfiguration
    .GetLoggerConfiguration()
    .CreateLogger();

builder.Host.UseSerilog();

// Configure services
builder.Services
    .AddMongoDb(builder.Configuration)
    .AddMongoRepositories()
    .AddMongoMigrations()
    .AddJwtAuthentication(builder.Configuration)
    .AddApplicationServices()
    .AddNotificationService(builder.Configuration)
    .AddInfrastructureServices()
    .AddPipelineBehaviours()
    .AddGraphQLServer()
    .ConfigureGraphQl()
    .AddGraphQlQueries()
    .AddGraphQlMutations()
    ;
    
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Configure WebApplication
var app = builder.Build();

app.UseCors("AllowAll");
app.MapCineverseGraphQl();
app.UseAuthentication()
    .UseAuthorization()
    .UseMiddleware<GraphQlStatusCodeMiddleware>();

// Execute Migrations
await app.ExecuteMigrations();

app.Run();