using AutomationTests.Core.Common.Constants.Shared;
using AutomationTests.Core.DataGenerators.Cineverse;
using AutomationTests.Core.Fixtures;
using AutomationTests.Core.TestBases;
using Xunit;

namespace AutomationTests.Cineverse.Api.Movies.Negative;

public class MoviesUpdateNegativeTests(CineverseFixture fixture) : CineverseApiTestBase(fixture)
{
    [Fact]
    public async Task UpdateMovie_WithoutAdminPermissions_ShouldReturnForbidden()
    {
        // Act
        var updateMovieResponse = await UserClient.UpdateMovie(MovieDataGenerator.ValidUpdateMovieRequest("test"));

        // Assert
        Assert.False(updateMovieResponse.IsSuccess);
        Assert.Equal(ApiConstants.ForbiddenErrorMessage, updateMovieResponse.ErrorMessage);
    }

    [Fact]
    public async Task UpdateMovie_WhenMovieIsNotFound_ShouldReturnNotFound()
    {
        // Act
        var updateMovieResponse = await AdminClient.UpdateMovie(MovieDataGenerator.ValidUpdateMovieRequest());
        
        // Assert
        Assert.False(updateMovieResponse.IsSuccess);
        Assert.Equal("Movie was not found", updateMovieResponse.ErrorMessage);
    }
    
    [Fact]
    public async Task UpdateMovie_WithInvalidRequest_ShouldReturnBadRequest()
    {
        // Arrange
        var updateMovieRequest = MovieDataGenerator.InvalidUpdateMovieRequest();
        
        // Act
        var updateMovieResponse = await AdminClient.UpdateMovie(updateMovieRequest);

        // Assert
        Assert.False(updateMovieResponse.IsSuccess);
        Assert.NotNull(updateMovieResponse.ValidationErrors);
        Assert.True(updateMovieResponse.ValidationErrors.Length is 6);
        Assert.Contains("Id must be a valid ObjectId", updateMovieResponse.ValidationErrors);
        Assert.Contains("Title cannot be empty", updateMovieResponse.ValidationErrors);
        Assert.Contains("Genre cannot be empty", updateMovieResponse.ValidationErrors);
        Assert.Contains("Description cannot be empty", updateMovieResponse.ValidationErrors);
        Assert.Contains("PosterUrl cannot be empty", updateMovieResponse.ValidationErrors);
        Assert.Contains("Invalid movie duration", updateMovieResponse.ValidationErrors);
    }
}