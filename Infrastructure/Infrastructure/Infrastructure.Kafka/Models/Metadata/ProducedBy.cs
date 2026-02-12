namespace Infrastructure.Kafka.Models.Metadata;

public class ProducedBy
{
    public required string Name { get; init; }
    
    public required string Version { get; init; }
}