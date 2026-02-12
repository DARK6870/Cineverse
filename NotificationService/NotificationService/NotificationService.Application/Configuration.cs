using Hangfire;
using Hangfire.Redis.StackExchange;
using Infrastructure.Kafka;
using Infrastructure.Kafka.Consumer.Extensions;
using Infrastructure.Kafka.Producer.Extensions;
using Infrastructure.MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.Kafka.Consumers;
using NotificationService.Application.Kafka.Settings;
using NotificationService.Application.Models.Options;
using NotificationService.Application.Models.Settings;
using NotificationService.Application.Services.Notification;
using NotificationService.Client.Kafka.Settings;
using NotificationService.Client.Models.Options;
using StackExchange.Redis;

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
            .Configure<NotificationOptions>(configuration.GetSection(nameof(NotificationOptions)));
        
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

    public static IServiceCollection AddHangfireWithRedis(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var section = configuration.GetSection(nameof(RedisSettings));
        var redisSettings = section.Get<RedisSettings>()
                               ?? throw new ArgumentNullException(nameof(RedisSettings));

        services.Configure<RedisSettings>(section);

        
        services.AddHangfire(cfg => cfg
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseRedisStorage(redisSettings.ConnectionString, new RedisStorageOptions
            {
                Prefix = "hangfire:",
                ExpiryCheckInterval = TimeSpan.FromHours(1),
                InvisibilityTimeout = TimeSpan.FromMinutes(30),
                FetchTimeout = TimeSpan.FromMinutes(3)
            }));

        services.AddHangfireServer(options =>
        {
            options.WorkerCount = 5;
        });
        
        return services;
    }
}