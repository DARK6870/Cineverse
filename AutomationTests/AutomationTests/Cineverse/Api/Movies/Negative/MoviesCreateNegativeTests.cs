using AutomationTests.Core.Cineverse.Base;
using AutomationTests.Core.Cineverse.DataGenerators;
using AutomationTests.Core.Cineverse.Fixture;
using AutomationTests.Core.Common.Constants.Shared;
using Xunit;

namespace AutomationTests.Cineverse.Api.Movies.Negative;

public class MoviesCreateNegativeTests(CineverseFixture fixture) : CineverseApiTestBase(fixture)
{
    [Fact]
    public async Task CreateMovie_WithoutAdminPermissions_ShouldReturnError()
    {
        // Act
        var createMovieResponse = await UserClient.CreateMovie();

        // Assert
        Assert.False(createMovieResponse.IsSuccess);
        Assert.Equal(ApiConstants.ForbiddenErrorMessage, createMovieResponse.ErrorMessage);
    }

    [Fact]
    public async Task CreateMovie_WithInvalidRequest_ShouldReturnValidationErrors()
    {
        // Act
        var createMovieResponse = await AdminClient.CreateMovie(MovieDataGenerator.InvalidCreateMovieRequest());

        // Assert
        Assert.False(createMovieResponse.IsSuccess);
        Assert.NotNull(createMovieResponse.ValidationErrors);
        Assert.True(createMovieResponse.ValidationErrors.Length is 6);
        Assert.Contains("Title cannot be empty", createMovieResponse.ValidationErrors);
        Assert.Contains("Genre cannot be empty", createMovieResponse.ValidationErrors);
        Assert.Contains("Description cannot be empty", createMovieResponse.ValidationErrors);
        Assert.Contains("PosterUrl cannot be empty", createMovieResponse.ValidationErrors);
        Assert.Contains("TrailerUrl cannot be empty", createMovieResponse.ValidationErrors);
        Assert.Contains("Invalid movie duration", createMovieResponse.ValidationErrors);
    }
    
    [Fact]
    public async Task CreateDuplicatedMovie_ShouldReturnError()
    {
        // Arrange
        var createMovieRequest = MovieDataGenerator.ValidCreateMovieRequest();

        // Act
        var createMovieResponse1 = await AdminClient.CreateMovie(createMovieRequest);
        var createMovieResponse2 = await AdminClient.CreateMovie(createMovieRequest);

        // Assert
        Assert.True(createMovieResponse1.IsSuccess);
        Assert.False(createMovieResponse2.IsSuccess);
        Assert.NotNull(createMovieResponse2.ErrorMessage);
        Assert.Equal("A movie with the same title and release date already exists", createMovieResponse2.ErrorMessage);
    }
}