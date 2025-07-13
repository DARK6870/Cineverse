using Cineverse.API;
using Cineverse.Application;
using Cineverse.Infrastructure;
using Cineverse.Infrastructure.Authentication;
using Cineverse.Infrastructure.Common.Configurations;
using Cineverse.Infrastructure.GraphQl;
using Cineverse.Mongo;
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
    .AddJwtAuthentication(builder.Configuration)
    .AddApplicationServices()
    .AddInfrastructureServices()
    .AddPipelineBehaviours()
    .AddGraphQLServer()
    .ConfigureGraphQl()
    .AddGraphQlQueries()
    .AddGraphQlMutations()
    ;
    

var app = builder.Build();

app.MapCineverseGraphQl();
app
    .UseAuthentication()
    .UseAuthorization()
    .UseMiddleware<GraphQlStatusCodeMiddleware>();

app.Run();