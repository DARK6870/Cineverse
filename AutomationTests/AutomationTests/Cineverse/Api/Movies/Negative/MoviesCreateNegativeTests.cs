using AutomationTests.Core.Fixtures;
using AutomationTests.Core.TestBases;
using Xunit;

namespace AutomationTests.Cineverse.Api.Movies.Negative;

public class MoviesCreateNegativeTests(CineverseFixture fixture) : CineverseApiTestBase(fixture)
{
    [Fact]
    public async Task CreateMovie_WithoutAdminPermissions_ShouldReturnForbidden()
    {
        // Arrange
        
        // Act
        
        // Assert
    }
}