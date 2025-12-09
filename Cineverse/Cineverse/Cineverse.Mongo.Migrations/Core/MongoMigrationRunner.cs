using Cineverse.Mongo.Repositories.Migration;
using Cineverse.Mongo.Schemas.Entities;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Cineverse.Mongo.Migrations.Core;

public class MongoMigrationRunner(
    IMongoDatabase database,
    IMigrationRepository migrationRepository,
    ILogger<MongoMigrationRunner> logger
)
{
    public async Task ExecuteMigrationsAsync()
    {
        var migrations = GetMigrations();

        foreach (var migration in migrations)
        {
            if (Activator.CreateInstance(migration, database) is MongoMigration migrationInstance
                && !await migrationRepository.ExistsAsync(x => x.Description == migrationInstance.Description))
            {
                logger.LogInformation("Running migration '{migrationName}'", migration.Name);

                await migrationInstance.MigrateAsync();
                await migrationRepository.InsertOneAsync(new MigrationEntity(migrationInstance.Description));
                
                logger.LogInformation("Migration '{migrationName}' executed successfully", migration.Name);
            }
            else
            {
                logger.LogInformation("Migration {migrationName} has already been migrated.", migration.Name);
            }
        }
    }

    private static Type[] GetMigrations()
    {
        var assembly = typeof(MongoMigrationRunner).Assembly;
        
        return assembly.GetTypes()
            .Where(t => t.IsSubclassOf(typeof(MongoMigration)) && !t.IsAbstract)
            .ToArray();
    }
}