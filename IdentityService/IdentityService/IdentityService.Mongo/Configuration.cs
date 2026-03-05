using IdentityService.Mongo.Repositories.RefreshToken;
using IdentityService.Mongo.Repositories.User;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityService.Mongo;

public static class Configuration
{
    public static IServiceCollection AddMongoRepositories(this IServiceCollection services)
    {
        services
            .AddSingleton<IUserRepository, UserRepository>()
            .AddSingleton<IRefreshTokenRepository, RefreshTokenRepository>()
            ;

        return services;
    }
}