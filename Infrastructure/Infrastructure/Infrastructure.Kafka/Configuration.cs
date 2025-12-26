using Infrastructure.Kafka.Models.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Kafka;

public static class Configuration
{
    /// <summary>
    /// Register kafka settings
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection AddKafkaSettings(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var kafkaSettings = configuration.GetSection(nameof(KafkaOptions)).Get<KafkaOptions>()
                            ?? throw new ArgumentNullException(nameof(KafkaOptions));
        
        services.AddSingleton(kafkaSettings);
        
        return services;
    }
}