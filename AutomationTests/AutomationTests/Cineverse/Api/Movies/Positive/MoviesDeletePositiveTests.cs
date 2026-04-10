using AutomationTests.Core.Cineverse.Api.Base;
using AutomationTests.Core.Cineverse.Api.Fixture;
using Xunit;

namespace AutomationTests.Cineverse.Api.Movies.Positive;

public class MoviesDeletePositiveTests(CineverseFixture fixture) : CineverseApiTestBase(fixture)
{
    [Fact]
    public async Task DeleteMovie_ShouldDeleteMovie()
    {
        // Arrange
        var createMovieResponse = await AdminClient.CreateMovie();
        Assert.NotNull(createMovieResponse.Data);

        // Act
        var deleteMovieResponse = await AdminClient.DeleteMovie(createMovieResponse.Data);
        var getMovieByIdResponse = await UnauthorizedClient.GetMovie(createMovieResponse.Data);

        // Assert
        Assert.True(deleteMovieResponse.IsSuccess);
        
        Assert.False(getMovieByIdResponse.IsSuccess);
        Assert.Equal("Movie was not found", getMovieByIdResponse.ErrorMessage);
    }
}