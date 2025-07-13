namespace Cineverse.Mongo.Common.Settings;

public class MongoDbSettings
{
    public required string ConnectionString { get; set; }
    
    public required string DatabaseName { get; set; }
    
    public bool LogQueries { get; init; }
}