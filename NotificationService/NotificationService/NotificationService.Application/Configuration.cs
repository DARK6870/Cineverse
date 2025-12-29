using Infrastructure.Kafka;
using Infrastructure.Kafka.Consumer.Extensions;
using Infrastructure.Kafka.Producer.Extensions;
using Infrastructure.MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.Kafka.Consumers;
using NotificationService.Application.Kafka.Settings;
using NotificationService.Application.Models.Options;
using NotificationService.Application.Services.Notification;
using NotificationService.Client.Kafka.Settings;
using NotificationService.Client.Models.Options;

namespace NotificationService.Application;

public static class Configuration
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // configuration
        services
            .Configure<SmtpOptions>(configuration.GetSection(nameof(SmtpOptions)))
            .Configure<NotificationLinksOptions>(configuration.GetSection(nameof(NotificationLinksOptions)));
        
        // services
        services.AddMediator(typeof(Configuration).Assembly);
        services.AddSingleton<INotificationService, Services.Notification.NotificationService>();
        
        return services;
    }

    public static IServiceCollection AddKafkaConsumers(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services
            .AddKafkaSettings(configuration)
            .AddInfrastructureKafkaProducer<EmailNotificationsProducerSettings>(configuration)
            .AddInfrastructureKafkaConsumer<EmailNotificationsConsumerSettings>(configuration);

        services.AddHostedService<EmailNotificationsConsumer>();
        
        return services;
    }
}