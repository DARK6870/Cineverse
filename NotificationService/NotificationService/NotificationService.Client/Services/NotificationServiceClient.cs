using Infrastructure.Kafka.Producer;
using Infrastructure.Kafka.Producer.Extensions;
using Microsoft.Extensions.Options;
using NotificationService.Client.Builders;
using NotificationService.Client.Kafka.Settings;
using NotificationService.Client.Models;

namespace NotificationService.Client.Services;

public class NotificationServiceClient(
    IEnumerable<IKafkaProducer> producers,
    IOptions<EmailNotificationsProducerSettings> producerSettings
) : INotificationServiceClient
{
    private readonly IKafkaProducer _producer = producers.GetByIdentifier(producerSettings.Value.Identifier);
    
    public async Task SendEmailNotificationAsync(string emailTo, string subject, MessageBuilder messageBuilder)
    {
        var emailNotification = new EmailNotification
        {
            EmailTo = emailTo,
            Subject = subject,
            Content = messageBuilder.Build()
        };
        
        await _producer.ProduceAsync(emailNotification);
    }
}