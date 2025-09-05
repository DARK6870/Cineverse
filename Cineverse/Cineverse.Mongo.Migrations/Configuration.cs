using Cineverse.Mongo.Migrations.Core;
using Cineverse.Mongo.Repositories.Generic;
using Cineverse.Mongo.Repositories.Migration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Cineverse.Mongo.Migrations;

public static class Configuration
{
    public static IServiceCollection AddMongoMigrations(
        this IServiceCollection services
    )
    {
        services.AddTransient<MongoMigrationRunner>();
        
        return services;
    }
    
    public static Task ExecuteMigrations(
        this IApplicationBuilder app
    )
    {
        return app.ApplicationServices
            .GetRequiredService<MongoMigrationRunner>()
            .ExecuteMigrationsAsync();
    }
}