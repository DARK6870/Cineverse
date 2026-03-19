using AutomationTests.Core.Cineverse.Base;
using AutomationTests.Core.Cineverse.DataGenerators;
using AutomationTests.Core.Cineverse.Fixture;
using Xunit;

namespace AutomationTests.Cineverse.Api.Movies.Positive;

public class MoviesCreatePositiveTests(CineverseFixture fixture) : CineverseApiTestBase(fixture)
{
    [Fact]
    public async Task CreateMovie_ShouldSaveMovie()
    {
        // Arrange
        var movie = MovieDataGenerator.ValidCreateMovieRequest();
        
        // Act
        var createMovieResponse = await AdminClient.CreateMovie(movie);
        Assert.NotNull(createMovieResponse.Data);
        
        var getMovieByIdResponse = await UnauthorizedClient.GetMovie(createMovieResponse.Data);
        var movieById = getMovieByIdResponse.Data;

        // Assert
        Assert.NotNull(movieById);
        Assert.True(movieById.Id == createMovieResponse.Data);
        Assert.True(movieById.IsAvailable);
        Assert.NotEmpty(movieById.Title);
        Assert.NotEmpty(movieById.Description);
        Assert.NotEmpty(movieById.Genre);
        Assert.NotEmpty(movieById.PosterUrl);
        Assert.NotEmpty(movieById.TrailerUrl);
    }
}