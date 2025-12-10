namespace Infrastructure.Mongo.Models.Options;

public class MongoOptions
{
    public required string ConnectionString { get; init; }
    
    public required string DatabaseName { get; init; }
    
    public bool LogDatabaseQueries { get; init; }
}