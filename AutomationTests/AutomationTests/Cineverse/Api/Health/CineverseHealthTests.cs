using AutomationTests.Core.Cineverse.Api.Base;
using AutomationTests.Core.Cineverse.Api.Fixture;
using Xunit;

namespace AutomationTests.Cineverse.Api.Health;

public class CineverseHealthTests(CineverseFixture fixture) : CineverseApiTestBase(fixture)
{
    [Fact]
    public async Task CheckHealth_ShouldReturnOk()
    {
        Assert.True(await UnauthorizedClient.IsHealthy());
    }
}