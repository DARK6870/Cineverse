using AutomationTests.Core.Common.Constants.Shared;
using AutomationTests.Core.DataGenerators.Cineverse;
using AutomationTests.Core.Fixtures;
using AutomationTests.Core.TestBases;
using Xunit;

namespace AutomationTests.Cineverse.Api.Movies.Negative;

public class MoviesCreateNegativeTests(CineverseFixture fixture) : CineverseApiTestBase(fixture)
{
    [Fact]
    public async Task CreateMovie_WithoutAdminPermissions_ShouldReturnForbidden()
    {
        // Act
        var createMovieResponse = await UserClient.CreateMovie();

        // Assert
        Assert.False(createMovieResponse.IsSuccess);
        Assert.Equal(ApiConstants.ForbiddenErrorMessage, createMovieResponse.ErrorMessage);
    }

    [Fact]
    public async Task CreateMovie_WithInvalidRequest_ShouldReturnBadRequest()
    {
        // Act
        var createMovieResponse = await AdminClient.CreateMovie(MovieDataGenerator.InvalidCreateMovieRequest());

        // Assert
        Assert.False(createMovieResponse.IsSuccess);
        Assert.NotNull(createMovieResponse.ValidationErrors);
        Assert.True(createMovieResponse.ValidationErrors.Length is 5);
        Assert.Contains("Title cannot be empty", createMovieResponse.ValidationErrors);
        Assert.Contains("Genre cannot be empty", createMovieResponse.ValidationErrors);
        Assert.Contains("Description cannot be empty", createMovieResponse.ValidationErrors);
        Assert.Contains("PosterUrl cannot be empty", createMovieResponse.ValidationErrors);
        Assert.Contains("Invalid movie duration", createMovieResponse.ValidationErrors);
    }
}