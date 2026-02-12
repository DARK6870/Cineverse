using IdentityService.Mongo.Repositories.RefreshToken;
using IdentityService.Mongo.Repositories.User;
using Infrastructure.Mongo.Repositories.Implementations;
using Infrastructure.Mongo.Repositories.Interfaces.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityService.Mongo;

public static class Configuration
{
    public static IServiceCollection AddMongoRepositories(this IServiceCollection services)
    {
        services
            .AddSingleton(typeof(IGenericRepository<>), typeof(GenericRepository<>))
            .AddSingleton<IUserRepository, UserRepository>()
            .AddSingleton<IRefreshTokenRepository, RefreshTokenRepository>()
            ;

        return services;
    }
}