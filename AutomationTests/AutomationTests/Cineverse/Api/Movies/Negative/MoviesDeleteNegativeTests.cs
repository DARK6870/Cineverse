using AutomationTests.Core.Common.Constants.Shared;
using AutomationTests.Core.Fixtures;
using AutomationTests.Core.TestBases;
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
}