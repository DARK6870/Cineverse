namespace AutomationTests.Core.Configuration.Options;

public class MongoOptions
{
    public required string ConnectionString { get; set; }
    public required string[] DatabaseNames { get; set; }
}