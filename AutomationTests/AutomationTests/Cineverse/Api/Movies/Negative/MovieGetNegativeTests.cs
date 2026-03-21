using AutomationTests.Core.Cineverse.Base;
using AutomationTests.Core.Cineverse.Fixture;
using Xunit;

namespace AutomationTests.Cineverse.Api.Movies.Negative;

public class MovieGetNegativeTests(CineverseFixture fixture) : CineverseApiTestBase(fixture)
{
    [Fact]
    public async Task GetAllMovies_WithInvalidPageSize_ShouldReturnError()
    {
        // Act
        var getMoviesResponse = await UnauthorizedClient.GetMovies(300);

        // Assert
        Assert.False(getMoviesResponse.IsSuccess);
        Assert.NotNull(getMoviesResponse.ErrorMessage);
        Assert.Equal("The maximum allowed items per page were exceeded.", getMoviesResponse.ErrorMessage);
    }
}