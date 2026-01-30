using System.Text.Json;

namespace Infrastructure.Kafka.Models.Metadata;

public class InfrastructureMessageMetadata
{
    public required ProducedBy ProducedBy { get; init; }
    
    public required string MessageType { get; init; }
    
    // TODO: Add trace id with context
    //public required string TraceId { get; init; }
    
    public Dictionary<string, string> SerializedTraceContext { get; init; } = new();

    public string JsonSerializedTraceContext => JsonSerializer.Serialize(SerializedTraceContext);
}