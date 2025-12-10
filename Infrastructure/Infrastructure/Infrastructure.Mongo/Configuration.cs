using Infrastructure.Mongo.Models.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;

namespace Infrastructure.Mongo;

public static class Configuration
{
    public static IServiceCollection AddMongoDatabase(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var mongoSettings = configuration.GetSection(nameof(MongoOptions)).Get<MongoOptions>()
                            ?? throw new ArgumentNullException(nameof(MongoOptions));
        
        services.AddSingleton(mongoSettings);

        services.AddSingleton<IMongoClient>(serviceProvider => 
        {
            var logger = serviceProvider.GetService<ILogger<MongoClient>>();
            var settings = MongoClientSettings.FromConnectionString(mongoSettings.ConnectionString);

            if (mongoSettings.LogDatabaseQueries && logger != null)
            {
                settings.ClusterConfigurator = cb =>
                {
                    cb.Subscribe<CommandStartedEvent>(e =>
                        logger.LogDebug("MongoDB Command: {CommandName}\n{CommandJson}",
                            e.CommandName,
                            e.Command.ToString()
                        )
                    );

                    cb.Subscribe<CommandFailedEvent>(e =>
                        logger.LogError("MongoDB Command FAILED: {CommandName}\n{Error}",
                            e.CommandName,
                            e.Failure
                        )
                    );
                };
            }
            
            return new MongoClient(settings);
        });

        services.AddSingleton<IMongoDatabase>(sp => 
        {
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase(mongoSettings.DatabaseName);
        });

        return services;
    }
}