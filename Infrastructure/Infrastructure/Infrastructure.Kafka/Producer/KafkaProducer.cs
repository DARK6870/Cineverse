using System.Text.Json;
using Confluent.Kafka;
using Infrastructure.Common.Json.Configuration;
using Infrastructure.Kafka.Exceptions;
using Infrastructure.Kafka.Extensions;
using Infrastructure.Kafka.Models.Settings;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Kafka.Producer;

public class KafkaProducer : IKafkaProducer
{
    // TODO: add retry options
    private readonly ILogger<KafkaProducer> _logger;
    private readonly IProducer<string?, string> _producer;
    
    public string Identifier { get; set; }
    public string TopicName { get; set; }

    public KafkaProducer(
        ILogger<KafkaProducer> logger,
        ProducerSettings settings
    )
    {
        var producerConfig = settings.ProducerConfig;
        
        _logger = logger;

        Identifier = settings.Identifier;
        TopicName = settings.TopicName;
        _producer = new ProducerBuilder<string?, string>(settings.ProducerConfig).Build();
        
        _logger.LogInformation("Initializing new kafka producer: {identifier}", Identifier);
    }

    public async Task ProduceAsync<T>(
        T message,
        CancellationToken cancellationToken = default
    )
    {
        var kafkaMessage = new Message<string?, string>
        {
            Value = JsonSerializer.Serialize(message, JsonSerializerConfiguration.JsonSerializerOptions)
        };
        kafkaMessage.AddInfrastructureMetadata<T>();
        
        await ProduceAsync(kafkaMessage, cancellationToken);
    }

    private async Task ProduceAsync(
        Message<string?, string> message,
        CancellationToken cancellationToken = default
    )
    {
        _logger.LogDebug(
            "Producing a new message to topic {topicName}. Identifier: {Identifier}",
            TopicName,
            Identifier
        );

        var result = await _producer.ProduceAsync(TopicName, message, cancellationToken);
        var isDelivered = result.Status == PersistenceStatus.Persisted;

        if (!isDelivered)
            throw new ProducerException($"Unable to produce message to ${TopicName}");

        _logger.LogDebug(
            "Message successfully produced to {topicName}, offset: {offset}. Identifier: {identifier}",
            TopicName,
            result.TopicPartitionOffset.Offset,
            Identifier
        );
    }
}