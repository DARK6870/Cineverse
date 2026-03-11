using AutomationTests.Core.Fixtures;
using AutomationTests.Core.TestBases;
using Xunit;

namespace AutomationTests.Cineverse.Api.Movies.Positive;

public class MoviesGetAllPositiveTests(CineverseFixture fixture) : CineverseApiTestBase(fixture)
{
    [Fact]
    public async Task GetAllMovies_ShouldReturnAllMovies()
    {
        // Arrange
        var createMovieResponse1 = await Client.CreateMovie(accessToken: fixture.AdminToken);
        var createMovieResponse2 = await Client.CreateMovie(accessToken: fixture.AdminToken);
        
        // Act
        var paginatedMovies = await Client.GetMovies();

        // Assert
        Assert.NotEmpty(paginatedMovies.Items);
        Assert.True(paginatedMovies.Items.Length > 0);
        Assert.NotNull(paginatedMovies.Items.FirstOrDefault(x => x.Id == createMovieResponse1.Data));
        Assert.NotNull(paginatedMovies.Items.FirstOrDefault(x => x.Id == createMovieResponse2.Data));
    }
}