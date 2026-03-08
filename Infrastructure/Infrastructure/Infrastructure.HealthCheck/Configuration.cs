using Confluent.Kafka;
using HealthChecks.Kafka;
using HealthChecks.UI.Client;
using Infrastructure.HealthCheck.Options;
using Infrastructure.Kafka.Common.Models.Settings;
using Infrastructure.Mongo.Models.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Driver;

namespace Infrastructure.HealthCheck;

public static class Configuration
{
    /// <summary>
    /// Configure health check endpoints
    /// </summary>
    public static void MapHealthCheck(this IApplicationBuilder app)
    {
        // main health endpoint - all checks
        var mainOptions = new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
            ResultStatusCodes =
            {
                [HealthStatus.Healthy] = StatusCodes.Status200OK,
                [HealthStatus.Degraded] = StatusCodes.Status200OK,
                [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
            }
        };
        
        app.UseHealthChecks("/health", mainOptions);
        
        // liveness probe - basic app health (for Kubernetes)
        app.UseHealthChecks("/health/live", new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("live"),
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
            ResultStatusCodes =
            {
                [HealthStatus.Healthy] = StatusCodes.Status200OK,
                [HealthStatus.Degraded] = StatusCodes.Status200OK,
                [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
            }
        });
        
        // readiness probe - dependencies health (for Kubernetes)
        app.UseHealthChecks("/health/ready", new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("ready"),
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
            ResultStatusCodes =
            {
                [HealthStatus.Healthy] = StatusCodes.Status200OK,
                [HealthStatus.Degraded] = StatusCodes.Status200OK,
                [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
            }
        });
    }
    
    /// <summary>
    /// Add infrastructure health checks
    /// </summary>
    public static IServiceCollection AddInfrastructureHealthChecks(
        this IServiceCollection services,
        Action<AddHealthCheckOptions> configureOptions
    )
    {
        var options = new AddHealthCheckOptions();
        configureOptions.Invoke(options);
        
        var builder = services.AddHealthChecks();
        
        // self check for liveness
        builder.AddCheck(
            "self",
            () => HealthCheckResult.Healthy("Application is running"),
            tags: ["live"]
        );
        
        if (options.IncludeMongoDb)
            builder.AddMongoDb();
        if (options.IncludeKafka)
            builder.AddKafka();
        
        return services;
    }

    /// <summary>
    /// Add MongoDB health check
    /// </summary>
    private static IHealthChecksBuilder AddMongoDb(
        this IHealthChecksBuilder builder
    )
    {
        return builder.AddMongoDb(
            clientFactory: sp =>
            {
                var mongoClient = sp.GetRequiredService<IMongoClient>();
                return mongoClient;
            },
            databaseNameFactory: sp =>
            {
                var mongoSettings = sp.GetRequiredService<MongoSettings>();
                return mongoSettings.DatabaseName;
            },
            name: "mongodb",
            failureStatus: HealthStatus.Unhealthy,
            tags: ["db", "mongodb", "ready"],
            timeout: TimeSpan.FromSeconds(5)
        );
    }

    /// <summary>
    /// Add a health check for Kafka cluster using settings resolved from DI.
    /// </summary>
    public static IHealthChecksBuilder AddKafka(
        this IHealthChecksBuilder builder
    )
    {
        builder.Services.AddSingleton(sp =>
        {
            var kafkaSettings = sp.GetRequiredService<KafkaSettings>();
            var config = new ProducerConfig { BootstrapServers = kafkaSettings.BootstrapServers };
            return new KafkaHealthCheck(new KafkaHealthCheckOptions 
            { 
                Configuration = config, 
                Topic = "health-check" 
            });
        });

        return builder.Add(new HealthCheckRegistration(
            factory: sp => sp.GetRequiredService<KafkaHealthCheck>(),
            name: "kafka",
            failureStatus: HealthStatus.Unhealthy,
            tags: ["broker", "kafka", "ready"],
            timeout: TimeSpan.FromSeconds(5)
        ));
    }
}