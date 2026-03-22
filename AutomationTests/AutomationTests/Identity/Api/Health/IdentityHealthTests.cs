using AutomationTests.Core.Identity.Base;
using AutomationTests.Core.Identity.Fixture;
using Xunit;

namespace AutomationTests.Identity.Api.Health;

public class IdentityHealthTests(IdentityFixture fixture) : IdentityApiTestBase(fixture)
{
    [Fact]
    public async Task CheckHealth_ShouldReturnOk()
    {
        Assert.True(await UnauthorizedClient.IsHealthy());
    }
}