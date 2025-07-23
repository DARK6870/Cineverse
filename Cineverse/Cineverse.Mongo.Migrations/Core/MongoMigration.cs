namespace Cineverse.Mongo.Migrations.Core;

public abstract class MongoMigration(string description)
{
    public string Description { get; } = description;
    
    public abstract Task MigrateAsync();
}