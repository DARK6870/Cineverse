using AutomationTests.Core.Fixtures;
using AutomationTests.Core.TestBases;
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