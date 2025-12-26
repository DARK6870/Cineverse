using Infrastructure.Kafka.Exceptions;
using Infrastructure.Kafka.Models.Options;
using Infrastructure.Kafka.Models.Settings;
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
            var kafkaOptions = provider.GetRequiredService<KafkaOptions>();

            producerSettings.ProducerConfig.BootstrapServers = kafkaOptions.BootstrapServers;
            producerSettings.ProducerConfig.SecurityProtocol = kafkaOptions.SecurityProtocol;
            producerSettings.ProducerConfig.SaslMechanism = kafkaOptions.SaslMechanism;
            producerSettings.ProducerConfig.SaslUsername = kafkaOptions.Username;
            producerSettings.ProducerConfig.SaslPassword = kafkaOptions.Password;

            return new KafkaProducer(logger, producerSettings);
        });
    }
}