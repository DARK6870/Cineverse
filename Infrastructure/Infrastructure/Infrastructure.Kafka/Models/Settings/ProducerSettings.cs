using Confluent.Kafka;

namespace Infrastructure.Kafka.Models.Settings;

public class ProducerSettings
{
    public bool IsEnabled { get; set; } = true;
    
    public required string Identifier { get; set; }
    
    public required string TopicName { get; set; }

    public ProducerConfig ProducerConfig { get; init; } = new ProducerConfig();
}