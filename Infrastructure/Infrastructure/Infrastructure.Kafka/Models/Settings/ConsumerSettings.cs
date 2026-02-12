using Confluent.Kafka;

namespace Infrastructure.Kafka.Models.Settings;

public class ConsumerSettings
{
    public bool IsEnabled { get; init; } = true;
    public required string Identifier { get; init; }
    
    public required string[] TopicNames { get; init; }

    public ConsumerConfig ConsumerConfig { get; init; } = new();
}