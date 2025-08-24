using Cineverse.Mongo.Common.Settings;
using Cineverse.Mongo.Repositories.Generic;
using Cineverse.Mongo.Repositories.Implementations;
using Cineverse.Mongo.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;

namespace Cineverse.Mongo;

public static class Configuration
{
    public static IServiceCollection AddMongoRepositories(this IServiceCollection services)
    {
        services
            .AddSingleton(typeof(IGenericRepository<>), typeof(GenericRepository<>))
            .AddSingleton<IUserRepository, UserRepository>()
            .AddSingleton<IMovieRepository, MovieRepository>()
            .AddSingleton<IRefreshTokenRepository, RefreshTokenRepository>()
            .AddSingleton<IScreeningRepository, ScreeningRepository>()
            .AddSingleton<IBookingRepository, BookingRepository>()
            .AddSingleton<IHallRepository, HallRepository>()
            .AddSingleton<IMigrationRepository, MigrationRepository>()
            ;

        return services;
    }

    public static IServiceCollection AddMongoDb(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // TODO: maybe change this to IOptions<model>
        var mongoSettings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>()
                            ?? throw new InvalidOperationException("MongoDb configuration is missing");
        
        services.AddSingleton(mongoSettings);

        services.AddSingleton<IMongoClient>(serviceProvider => 
        {
            var logger = serviceProvider.GetService<ILogger<MongoClient>>();
            var settings = MongoClientSettings.FromConnectionString(mongoSettings.ConnectionString);

            if (mongoSettings.LogQueries && logger != null)
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