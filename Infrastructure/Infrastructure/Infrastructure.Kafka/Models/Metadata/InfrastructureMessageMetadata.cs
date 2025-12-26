using System.Text.Json;

namespace Infrastructure.Kafka.Models.Metadata;

public class InfrastructureMessageMetadata
{
    public required ProducedByComponent ProducedBy { get; init; }
    
    public required string MessageType { get; init; }
    
    public Dictionary<string, string> SerializedTraceContext { get; init; } = new Dictionary<string, string>();

    public string JsonSerializedTraceContext => JsonSerializer.Serialize(this.SerializedTraceContext);
}