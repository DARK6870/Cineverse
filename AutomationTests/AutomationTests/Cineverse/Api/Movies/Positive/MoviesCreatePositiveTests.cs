using AutomationTests.Core.DataGenerators.Cineverse;
using AutomationTests.Core.Fixtures;
using AutomationTests.Core.TestBases;
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
        var createMovieResponse = await Client.CreateMovie(movie, fixture.AdminToken);
        Assert.NotNull(createMovieResponse.Data);
        
        var getMovieByIdResponse = await Client.GetMovie(createMovieResponse.Data);
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