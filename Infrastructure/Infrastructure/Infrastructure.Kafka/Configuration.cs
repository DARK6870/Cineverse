using Infrastructure.Kafka.Models.Settings;
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
        var kafkaSettings = configuration.GetSection(nameof(KafkaSettings)).Get<KafkaSettings>()
                            ?? throw new ArgumentNullException(nameof(KafkaSettings));
        
        services.AddSingleton(kafkaSettings);
        
        return services;
    }
}