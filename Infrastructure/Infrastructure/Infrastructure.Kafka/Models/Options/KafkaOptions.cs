namespace Infrastructure.Kafka.Models.Options;

public class KafkaOptions
{
    public required string BootstrapServers { get; init; }
    public required string ConsumerGroup { get; init; }
    
    /*public SecurityProtocol SecurityProtocol { get; init; }
    public SaslMechanism? SaslMechanism { get; init; }*/

    public string? Username { get; init; }
    public string? Password { get; init; }
}