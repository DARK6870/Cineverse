namespace Infrastructure.Kafka.Common.Models.Metadata;

public class InfrastructureMessageMetadata
{
    public required ProducedBy ProducedBy { get; init; }
    
    public required string MessageType { get; init; }
    
    public required string TraceId { get; init; }
}