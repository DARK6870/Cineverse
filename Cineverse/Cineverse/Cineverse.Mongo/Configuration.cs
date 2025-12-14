using Cineverse.Mongo.Repositories.Booking;
using Cineverse.Mongo.Repositories.Hall;
using Cineverse.Mongo.Repositories.Movie;
using Cineverse.Mongo.Repositories.Screening;
using Infrastructure.Mongo.Repositories.Implementations;
using Infrastructure.Mongo.Repositories.Interfaces.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Cineverse.Mongo;

public static class Configuration
{
    public static IServiceCollection AddMongoRepositories(this IServiceCollection services)
    {
        services
            .AddSingleton(typeof(IGenericRepository<>), typeof(GenericRepository<>))
            .AddSingleton<IMovieRepository, MovieRepository>()
            .AddSingleton<IScreeningRepository, ScreeningRepository>()
            .AddSingleton<IBookingRepository, BookingRepository>()
            .AddSingleton<IHallRepository, HallRepository>()
            ;

        return services;
    }
}