using AutomationTests.Core.Cineverse.Base;
using AutomationTests.Core.Cineverse.Fixture;
using Xunit;

namespace AutomationTests.Cineverse.Api.Health;

public class CineverseHealthTests(CineverseFixture fixture) : CineverseApiTestBase(fixture)
{
    [Fact]
    public async Task CheckHealth_ShouldReturnOk()
    {
        // Assert
        Assert.True(await UnauthorizedClient.IsHealthy());
    }
}