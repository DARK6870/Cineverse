using Cineverse.Mongo.Repositories.Booking;
using Cineverse.Mongo.Repositories.Hall;
using Cineverse.Mongo.Repositories.Movie;
using Cineverse.Mongo.Repositories.Screening;
using Microsoft.Extensions.DependencyInjection;

namespace Cineverse.Mongo;

public static class Configuration
{
    public static IServiceCollection AddMongoRepositories(this IServiceCollection services)
    {
        services
            .AddScoped<IMovieRepository, MovieRepository>()
            .AddScoped<IScreeningRepository, ScreeningRepository>()
            .AddScoped<IBookingRepository, BookingRepository>()
            .AddScoped<IHallRepository, HallRepository>()
            ;

        return services;
    }
}