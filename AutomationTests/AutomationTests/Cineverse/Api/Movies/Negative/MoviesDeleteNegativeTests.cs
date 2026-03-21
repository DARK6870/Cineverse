using AutomationTests.Core.Cineverse.Base;
using AutomationTests.Core.Cineverse.Fixture;
using AutomationTests.Core.Common.Constants.Shared;
using MongoDB.Bson;
using Xunit;

namespace AutomationTests.Cineverse.Api.Movies.Negative;

public class MoviesDeleteNegativeTests(CineverseFixture fixture) : CineverseApiTestBase(fixture)
{
    [Fact]
    public async Task DeleteMovie_WithoutAdminPermissions_ShouldReturnError()
    {
        // Act
        var deleteMovieResponse = await UserClient.DeleteMovie(ObjectId.GenerateNewId().ToString());
        
        // Assert
        Assert.False(deleteMovieResponse.IsSuccess);
        Assert.Equal(ApiConstants.ForbiddenErrorMessage, deleteMovieResponse.ErrorMessage);
    }
    
    [Fact]
    public async Task DeleteMovie_WithEmptyObjectId_ShouldReturnValidationError()
    {
        // Act
        var deleteMovieResponse = await AdminClient.DeleteMovie("");
        
        // Assert
        Assert.False(deleteMovieResponse.IsSuccess);
        Assert.NotNull(deleteMovieResponse.ValidationErrors);
        Assert.True(deleteMovieResponse.ValidationErrors.Length is 1);
        Assert.Contains("Id cannot be empty", deleteMovieResponse.ValidationErrors);
    }

    [Fact]
    public async Task DeleteMovie_WithInvalidObjectId_ShouldReturnValidationError()
    {
        // Act
        var deleteMovieResponse = await AdminClient.DeleteMovie("invalid_object_id");
        
        // Assert
        Assert.False(deleteMovieResponse.IsSuccess);
        Assert.NotNull(deleteMovieResponse.ValidationErrors);
        Assert.True(deleteMovieResponse.ValidationErrors.Length is 1);
        Assert.Contains("Id must be a valid ObjectId", deleteMovieResponse.ValidationErrors);
    }

    [Fact]
    public async Task DeleteMovie_WithActiveScreening_ShouldReturnError()
    {
        // Arrange
        var createMovieResponse = await AdminClient.CreateMovie();
        Assert.True(createMovieResponse.IsSuccess);
        Assert.NotNull(createMovieResponse.Data);

        var createScreeningResponse = await AdminClient.CreateScreening(createMovieResponse.Data, fixture.DefaultHall.Id);
        Assert.True(createScreeningResponse.IsSuccess);
        Assert.NotNull(createScreeningResponse.Data);

        // Act
        var deleteMovieResponse = await AdminClient.DeleteMovie(createMovieResponse.Data);

        // Assert
        Assert.False(deleteMovieResponse.IsSuccess);
        Assert.NotNull(deleteMovieResponse.ErrorMessage);
        Assert.Equal("Please remove all screenings assigned to this movie before deleting", deleteMovieResponse.ErrorMessage);
    }
}