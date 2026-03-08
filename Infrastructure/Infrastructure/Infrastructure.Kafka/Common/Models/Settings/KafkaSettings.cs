using Confluent.Kafka;

namespace Infrastructure.Kafka.Common.Models.Settings;

public class KafkaSettings
{
    public required string BootstrapServers { get; init; }
    public required string ConsumerGroup { get; init; }
    
    public SecurityProtocol SecurityProtocol { get; init; }
    public SaslMechanism? SaslMechanism { get; init; }

    public string? Username { get; init; }
    public string? Password { get; init; }
}