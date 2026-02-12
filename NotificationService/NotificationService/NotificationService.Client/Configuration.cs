using Infrastructure.Kafka.Producer.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Client.Kafka.Settings;
using NotificationService.Client.Models.Options;
using NotificationService.Client.Services;

namespace NotificationService.Client;

public static class Configuration
{
    public static IServiceCollection AddNotificationServiceClient(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.Configure<NotificationLinksOptions>(configuration.GetSection(nameof(NotificationLinksOptions)));
        services.AddInfrastructureKafkaProducer<EmailNotificationsProducerSettings>(configuration);
        services.AddSingleton<INotificationServiceClient, NotificationServiceClient>();
        
        return services;
    }
}