using Confluent.Kafka;
using Infrastructure.Kafka.Consumer;
using Infrastructure.Kafka.Consumer.Extensions;
using Infrastructure.Kafka.Extensions;
using Infrastructure.Kafka.Models.Metadata;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NotificationService.Application.Kafka.Settings;
using NotificationService.Application.Services.Notification;
using NotificationService.Client.Models;

namespace NotificationService.Application.Kafka.Consumers;

public class EmailNotificationsConsumer(
    IOptions<EmailNotificationsConsumerSettings> consumerSettings,
    ILogger<EmailNotificationsConsumer> logger,
    IEnumerable<IKafkaConsumer> consumers,
    INotificationService notificationService
) : BackgroundService
{
    private readonly IKafkaConsumer _consumer = consumers.GetByIdentifier(consumerSettings.Value.Identifier);
     
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await Task.Run(async () =>
            {
                await _consumer.StartConsumingMessagesAsync(HandleMessageAsync, cancellationToken);
            }, cancellationToken
        );
    }

    private async Task HandleMessageAsync(
        ConsumeResult<string?, string> consumeResult,
        InfrastructureMessageMetadata metadata
    )
    {
        var notification = consumeResult.GetMessageValue<EmailNotification>();
        
        logger.LogDebug("Start processing message {@message}", notification);

        await notificationService.SendEmailNotificationAsync(
            notification.EmailTo,
            notification.Subject,
            notification.Content
        );
        
        logger.LogDebug("Message processed successfully");
    }
}