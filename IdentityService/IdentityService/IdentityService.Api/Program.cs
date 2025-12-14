using IdentityService.Mongo;
using Infrastructure.Logging;
using Infrastructure.Mongo;
using Infrastructure.Mongo.Migrations;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.AddInfrastructureLogging();

// ------ Configure services ------ //

builder.Services
    .AddOpenApi()
    .AddControllers();

builder.Services
    .AddMongoDatabase(configuration)
    .AddMongoRepositories()
    .AddMongoMigrations()
    ;


// ------ Configure WebApplication ------ //
var app = builder.Build();

//app.UseAuthentication();
//app.UseAuthorization();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.Theme = ScalarTheme.Purple;
    });
}

app.MapControllers();

// ------ Execute Migrations ------ //
await app.ExecuteMigrationsAsync();

app.Run();