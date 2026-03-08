using Infrastructure.Kafka.Common.Exceptions;
using Infrastructure.Kafka.Common.Models.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Kafka.Producer.Extensions;

public static class ProducerDependencyInjectionExtensions
{
    /// <summary>
    /// Register kafka producer
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static IServiceCollection AddInfrastructureKafkaProducer<T>(
        this IServiceCollection services,
        IConfiguration configuration
    ) where T : class
    {
        var section = configuration.GetSection(typeof(T).Name);
        var producerSettings = section.Get<ProducerSettings>()
                             ?? throw new ProducerException($"{typeof(T).Name} config was not found");
        
        services.Configure<T>(section);
        
        services.RegisterInfrastructureProducer(producerSettings);
        return services;
    }

    private static void RegisterInfrastructureProducer(
        this IServiceCollection services,
        ProducerSettings producerSettings
    )
    {
        services.AddSingleton<IKafkaProducer, KafkaProducer>(provider =>
        {
            var logger = provider.GetRequiredService<ILogger<KafkaProducer>>();
            var kafkaSettings = provider.GetRequiredService<KafkaSettings>();

            producerSettings.ProducerConfig.BootstrapServers = kafkaSettings.BootstrapServers;
            producerSettings.ProducerConfig.SecurityProtocol = kafkaSettings.SecurityProtocol;
            producerSettings.ProducerConfig.SaslMechanism = kafkaSettings.SaslMechanism;
            producerSettings.ProducerConfig.SaslUsername = kafkaSettings.Username;
            producerSettings.ProducerConfig.SaslPassword = kafkaSettings.Password;

            return new KafkaProducer(logger, producerSettings);
        });
    }
}