using Confluent.Kafka;

namespace Infrastructure.Kafka.Producer;

public interface IKafkaProducer
{
    string Identifier { get; set; }
    
    string TopicName { get; set; }

    Task ProduceAsync<T>(
        T message,
        CancellationToken cancellationToken = default
    );
}