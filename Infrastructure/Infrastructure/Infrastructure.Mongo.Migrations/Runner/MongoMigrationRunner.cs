using System.Reflection;
using Infrastructure.Mongo.Migrations.Core;
using Infrastructure.Mongo.Migrations.Entities;
using Infrastructure.Mongo.Migrations.Repositories.Migration;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Infrastructure.Mongo.Migrations.Runner;

public class MongoMigrationRunner(
    IMongoDatabase database,
    IMigrationRepository migrationRepository,
    ILogger<MongoMigrationRunner> logger
)
{
    public async Task ExecuteMigrationsAsync()
    {
        try
        {
            // step 1: get all migrations from assembly
            var migrations = GetMigrations();

            foreach (var migration in migrations)
            {
                // step 2: validate if migration was not executed before
                if (Activator.CreateInstance(migration, database) is MongoMigration migrationInstance
                    && !await migrationRepository.ExistsAsync(x => x.Description == migrationInstance.Description))
                {
                    logger.LogInformation("Running migration '{migrationName}'", migration.Name);

                    // step 3: execute migration
                    await migrationInstance.MigrateAsync();

                    // step 4: add a new record to _migrations collections
                    await migrationRepository.InsertOneAsync(new MigrationEntity(migrationInstance.Description));

                    logger.LogInformation("Migration '{migrationName}' executed successfully", migration.Name);
                }
                else
                {
                    logger.LogInformation("Migration {migrationName} has already been migrated.", migration.Name);
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occured during migration run");
        }
    }

    private static Type[] GetMigrations()
    {
        return DependencyContext.Default?.RuntimeLibraries
                   .SelectMany(lib => lib.GetDefaultAssemblyNames(DependencyContext.Default))
                   .Select(Assembly.Load)
                   .SelectMany(assembly => assembly.GetTypes())
                   .Where(t =>
                       !t.IsAbstract &&
                       t.IsSubclassOf(typeof(MongoMigration)))
                   .ToArray()
               ?? [];
    }
}