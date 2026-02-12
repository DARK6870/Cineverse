using Infrastructure.Mongo.Migrations.Repositories.Migration;
using Infrastructure.Mongo.Migrations.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Mongo.Migrations;

public static class Configuration
{
    public static IServiceCollection AddMongoMigrations(
        this IServiceCollection services
    )
    {
        services.AddSingleton<IMigrationRepository, MigrationRepository>();
        services.AddTransient<MongoMigrationRunner>();
        
        return services;
    }
    
    public static Task ExecuteMigrationsAsync(
        this IApplicationBuilder app
    )
    {
        return app.ApplicationServices
            .GetRequiredService<MongoMigrationRunner>()
            .ExecuteMigrationsAsync();
    }
}