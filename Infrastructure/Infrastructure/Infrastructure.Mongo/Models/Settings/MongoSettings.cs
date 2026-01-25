namespace Infrastructure.Mongo.Models.Settings;

public class MongoSettings
{
    public required string ConnectionString { get; init; }
    
    public required string DatabaseName { get; init; }
    
    public bool LogDatabaseQueries { get; init; }
}