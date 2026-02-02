using Cineverse.IntegrationTests.Shared.Constants.Test;
using Cineverse.IntegrationTests.Shared.Extensions;
using Cineverse.Mongo.Repositories.Booking;
using Cineverse.Mongo.Repositories.Movie;
using Cineverse.Mongo.Schemas.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using MongoDB.Bson;

namespace Cineverse.IntegrationTests.Shared.Helpers;

public static class DataHelper
{
    public static async Task<BookingEntity> GenerateBookingAsync(
        this WebApplicationFactory<Program> factory,
        string? userId = null
    )
    {
        var bookingRepository = factory.GetRequiredScopedService<IBookingRepository>();

        var booking = new BookingEntity
        {
            UserId = userId ?? TestConstants.TestUserId,
            ScreeningId = new ObjectId().ToString(),
            SeatIds =
            [
                Guid.NewGuid().ToString()
            ],
            TotalPrice = 25
        };
        
        await bookingRepository.InsertOneAsync(booking);
        return booking;
    }
    
    public static async Task<MovieEntity> GenerateMovieAsync(
        this WebApplicationFactory<Program> factory
    )
    {
        var movieRepository = factory.GetRequiredScopedService<IMovieRepository>();

        var movie = new MovieEntity
        {
            Title = Guid.NewGuid().ToString(),
            Description = "Test Description",
            Genre = "Test Genre",
            PosterUrl = "https://test.com/poster.png",
            TrailerUrl =  "https://test.com/trailer.mp4",
            ReleaseDate = new DateOnly(2006, 08, 08),
            Duration = 120,
            IsAvailable =  true
        };
        
        await movieRepository.InsertOneAsync(movie);
        return movie;
    }
}