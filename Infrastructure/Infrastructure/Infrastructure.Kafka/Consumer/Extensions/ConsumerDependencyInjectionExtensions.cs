using Infrastructure.Kafka.Exceptions;
using Infrastructure.Kafka.Models.Options;
using Infrastructure.Kafka.Models.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Kafka.Consumer.Extensions;

public static class ConsumerDependencyInjectionExtensions
{
    /// <summary>
    /// Register kafka consumer
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static IServiceCollection AddInfrastructureKafkaConsumer<T>(
        this IServiceCollection services,
        IConfiguration configuration
    ) where T : class
    {
        var section = configuration.GetSection(typeof(T).Name);
        var consumerSettings = section.Get<ConsumerSettings>()
            ?? throw new ConsumerException($"{typeof(T).Name} config was not found");

        services.Configure<T>(section);
        
        services.RegisterInfrastructureConsumer(consumerSettings);
        
        return services;
    }

    private static void RegisterInfrastructureConsumer(this IServiceCollection services, ConsumerSettings consumerSettings)
    {
        services.AddSingleton<IKafkaConsumer, KafkaConsumer>(provider =>
        {
            var logger = provider.GetRequiredService<ILogger<KafkaConsumer>>();
            var kafkaOptions = provider.GetRequiredService<KafkaOptions>();
            
            consumerSettings.ConsumerConfig.GroupId = kafkaOptions.ConsumerGroup;
            consumerSettings.ConsumerConfig.BootstrapServers = kafkaOptions.BootstrapServers;
            consumerSettings.ConsumerConfig.SecurityProtocol = kafkaOptions.SecurityProtocol;
            consumerSettings.ConsumerConfig.SaslMechanism = kafkaOptions.SaslMechanism;
            consumerSettings.ConsumerConfig.SaslUsername = kafkaOptions.Username;
            consumerSettings.ConsumerConfig.SaslPassword = kafkaOptions.Password;
            
            return new KafkaConsumer(logger, consumerSettings);
        });
    }
}