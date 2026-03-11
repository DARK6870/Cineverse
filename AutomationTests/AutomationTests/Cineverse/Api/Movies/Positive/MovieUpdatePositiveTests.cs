using AutomationTests.Core.Fixtures;
using AutomationTests.Core.TestBases;
using Xunit;

namespace AutomationTests.Cineverse.Api.Movies.Positive;

public class MovieUpdatePositiveTests(CineverseFixture fixture) : CineverseApiTestBase(fixture)
{
    [Fact]
    public async Task UpdateMovie_ShouldUpdateMovie()
    {
        // Arrange
        //var createMovieResponse = await Client.CreateMovie();
        
        // Act
        // Update movie
        // Assert
    }
}