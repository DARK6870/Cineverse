using AutomationTests.Core.Cineverse.Base;
using AutomationTests.Core.Cineverse.Fixture;
using AutomationTests.Core.Common.Constants.Shared;
using MongoDB.Bson;
using Xunit;

namespace AutomationTests.Cineverse.Api.Movies.Negative;

public class MoviesDeleteNegativeTests(CineverseFixture fixture) : CineverseApiTestBase(fixture)
{
    [Fact]
    public async Task DeleteMovie_WithoutAdminPermissions_ShouldReturnForbidden()
    {
        // Act
        var deleteMovieResponse = await UserClient.DeleteMovie(ObjectId.GenerateNewId().ToString());
        
        // Assert
        Assert.False(deleteMovieResponse.IsSuccess);
        Assert.Equal(ApiConstants.ForbiddenErrorMessage, deleteMovieResponse.ErrorMessage);
    }

    [Fact]
    public async Task DeleteMovie_WithInvalidObjectId_ShouldReturnBadRequest()
    {
        // Act
        var deleteMovieResponse = await AdminClient.DeleteMovie("test");
        
        // Assert
        Assert.False(deleteMovieResponse.IsSuccess);
        Assert.NotNull(deleteMovieResponse.ValidationErrors);
        Assert.True(deleteMovieResponse.ValidationErrors.Length is 1);
        Assert.Contains("Id must be a valid ObjectId", deleteMovieResponse.ValidationErrors);
    }

    [Fact]
    public async Task DeleteMovie_WithActiveScreening_ShouldReturnBadRequest()
    {
        // Arrange
        var createMovieResponse = await AdminClient.CreateMovie();
        Assert.True(createMovieResponse.IsSuccess);

        var createScreeningResponse = await AdminClient.CreateMovie();

        // Act

        // Assert
    }
}