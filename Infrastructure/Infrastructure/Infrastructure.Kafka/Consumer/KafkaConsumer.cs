using Confluent.Kafka;
using Infrastructure.Kafka.Consumer.Extensions;
using Infrastructure.Kafka.Extensions;
using Infrastructure.Kafka.Models.Metadata;
using Infrastructure.Kafka.Models.Settings;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Kafka.Consumer;

public class KafkaConsumer: IKafkaConsumer
{
    private readonly ILogger<KafkaConsumer> _logger;
    private readonly ConsumerSettings _settings;
    
    public string Identifier { get; set; }
    public string[] TopicNames { get; set; }
    
    
    public KafkaConsumer(
        ILogger<KafkaConsumer> logger,
        ConsumerSettings settings
    )
    {
        _logger = logger;
        _settings = settings;

        Identifier = _settings.Identifier;
        TopicNames = _settings.TopicNames;

        if (_settings.IsEnabled)
            _logger.LogInformation("Kafka consumer enabled: {identifier}", Identifier);
        else
            _logger.LogWarning("Kafka consumer disabled: {identifier}", Identifier);
    }

    public Task StartConsumingMessagesAsync(
        Func<ConsumeResult<string?, string>, InfrastructureMessageMetadata, Task> handlerFunction,
        CancellationToken cancellationToken
    )
    {
        return _settings.IsEnabled
            ? PollForMessagesAsync(handlerFunction, cancellationToken)
            : Task.CompletedTask;
    }

    private async Task PollForMessagesAsync(
        Func<ConsumeResult<string?, string>, InfrastructureMessageMetadata, Task> handlerFunction,
        CancellationToken cancellationToken
    )
    {
        // auto-commit
        var autoCommitConfigured = _settings.ConsumerConfig.EnableAutoCommit is not null && _settings.ConsumerConfig.EnableAutoCommit.Value;

        // build consumer
        using var consumer = new ConsumerBuilder<string?, string>(_settings.ConsumerConfig)
            .SetErrorHandler((_, e) =>
                _logger.LogError("Consumer exception, reason: {reason} | ConsumerIdentifier: {identifier}", e.Reason, Identifier)
            )
            .Build();

        // subscribe to topics
        consumer.Subscribe(TopicNames);
        
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                // read message
                var consumeResult = consumer.Consume(cancellationToken);
                var metadata = consumeResult.GetInfrastructureMetadata();

                if (consumeResult.IsPartitionEOF)
                {
                    _logger.LogDebug(
                        "Reached end of {topic} topic, partition: {partition}, offset: {offset}. Identifier: {identifier}",
                        consumeResult.Topic,
                        consumeResult.Partition,
                        consumeResult.Offset,
                        Identifier
                    );
                    
                    continue;
                }

                _logger.LogDebug(
                    "Received message with offset {offset} on topic {topic}, partition {partition}. Identifier: {identifier}",
                    consumeResult.Offset,
                    consumeResult.Topic,
                    consumeResult.TopicPartition.Partition,
                    Identifier
                );

                    try
                    {
                        // call handler function to handle the incoming message
                        await handlerFunction(consumeResult, metadata);

                        _logger.LogDebug(
                            "Message with offset {offset} on topic {topic}, partition {partition} processed successfully. Identifier: {identifier}",
                            consumeResult.Offset,
                            consumeResult.Topic,
                            consumeResult.TopicPartition.Partition,
                            Identifier
                        );
                        
                        // commit the message
                        if (autoCommitConfigured)
                            consumer.Commit(consumeResult);
                    }
                    catch (Exception ex)
                    {
                        if (!autoCommitConfigured)
                            consumer.Seek(consumeResult.TopicPartitionOffset);
                        
                        _logger.LogError(
                            ex,
                            "Processing of message with offset {offset} on topic {topic}, partition {partition} has failed. Identifier: {identifier}",
                            consumeResult.Offset,
                            consumeResult.Topic,
                            consumeResult.TopicPartition.Partition,
                            Identifier
                        );
                    }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Kafka consumer error. Topic names={@topicNames},  Identifier: {Identifier}",
                    TopicNames,
                    Identifier
                );
            }
        }

        _logger.LogInformation("Stopping to poll messages for consumer, Identifier: {identifier}", Identifier);
    }
}