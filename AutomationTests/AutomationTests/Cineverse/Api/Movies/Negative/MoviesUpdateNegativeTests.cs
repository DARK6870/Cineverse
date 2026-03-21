using AutomationTests.Core.Cineverse.Base;
using AutomationTests.Core.Cineverse.DataGenerators;
using AutomationTests.Core.Cineverse.Fixture;
using AutomationTests.Core.Common.Constants.Shared;
using Xunit;

namespace AutomationTests.Cineverse.Api.Movies.Negative;

public class MoviesUpdateNegativeTests(CineverseFixture fixture) : CineverseApiTestBase(fixture)
{
    [Fact]
    public async Task UpdateMovie_WithoutAdminPermissions_ShouldReturnError()
    {
        // Act
        var updateMovieResponse = await UserClient.UpdateMovie(MovieDataGenerator.ValidUpdateMovieRequest());

        // Assert
        Assert.False(updateMovieResponse.IsSuccess);
        Assert.Equal(ApiConstants.ForbiddenErrorMessage, updateMovieResponse.ErrorMessage);
    }

    [Fact]
    public async Task UpdateMovie_WhenMovieIsNotFound_ShouldReturnError()
    {
        // Act
        var updateMovieResponse = await AdminClient.UpdateMovie(MovieDataGenerator.ValidUpdateMovieRequest());
        
        // Assert
        Assert.False(updateMovieResponse.IsSuccess);
        Assert.Equal("Movie was not found", updateMovieResponse.ErrorMessage);
    }
    
    [Fact]
    public async Task UpdateMovie_WithInvalidRequest_ShouldReturnValidationErrors()
    {
        // Arrange
        var updateMovieRequest = MovieDataGenerator.InvalidUpdateMovieRequest();
        
        // Act
        var updateMovieResponse = await AdminClient.UpdateMovie(updateMovieRequest);

        // Assert
        Assert.False(updateMovieResponse.IsSuccess);
        Assert.NotNull(updateMovieResponse.ValidationErrors);
        Assert.True(updateMovieResponse.ValidationErrors.Length is 7);
        Assert.Contains("Id cannot be empty", updateMovieResponse.ValidationErrors);
        Assert.Contains("Title cannot be empty", updateMovieResponse.ValidationErrors);
        Assert.Contains("Genre cannot be empty", updateMovieResponse.ValidationErrors);
        Assert.Contains("Description cannot be empty", updateMovieResponse.ValidationErrors);
        Assert.Contains("PosterUrl cannot be empty", updateMovieResponse.ValidationErrors);
        Assert.Contains("TrailerUrl cannot be empty", updateMovieResponse.ValidationErrors);
        Assert.Contains("Invalid movie duration", updateMovieResponse.ValidationErrors);
    }

    [Fact]
    public async Task UpdateMovie_WithInvalidObjectId_ShouldReturnValidationError()
    {
        // Arrange
        var updateMovieRequest = MovieDataGenerator.ValidUpdateMovieRequest("invalid_object_id");
        
        // Act
        var updateMovieResponse = await AdminClient.UpdateMovie(updateMovieRequest);
        
        // Assert
        Assert.False(updateMovieResponse.IsSuccess);
        Assert.NotNull(updateMovieResponse.ValidationErrors);
        Assert.Contains("Id must be a valid ObjectId", updateMovieResponse.ValidationErrors);
    }
}