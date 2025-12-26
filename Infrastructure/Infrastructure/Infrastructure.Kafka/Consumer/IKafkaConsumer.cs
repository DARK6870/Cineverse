using Confluent.Kafka;
using Infrastructure.Kafka.Models.Metadata;

namespace Infrastructure.Kafka.Consumer;

public interface IKafkaConsumer
{
    /// <summary>
    /// Identifier
    /// </summary>
    string Identifier { get; set; }
    
    /// <summary>
    /// Topic names
    /// </summary>
    string[] TopicNames { get; set; }

    /// <summary>
    /// Start consuming messages
    /// </summary>
    /// <param name="handlerFunction"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task StartConsumingMessagesAsync(
        Func<ConsumeResult<string?, string>, InfrastructureMessageMetadata, Task> handlerFunction,
        CancellationToken cancellationToken
    );
}