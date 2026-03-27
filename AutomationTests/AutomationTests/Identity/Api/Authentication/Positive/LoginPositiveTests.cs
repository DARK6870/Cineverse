using System.Net;
using AutomationTests.Core.Identity.Base;
using AutomationTests.Core.Identity.Fixture;
using AutomationTests.Models.Identity.Requests;
using AutomationTests.Models.Identity.Requests.Login;
using Xunit;

namespace AutomationTests.Identity.Api.Authentication.Positive;

public class LoginPositiveTests(IdentityFixture fixture) : IdentityApiTestBase(fixture)
{
    [Fact]
    public async Task Login_WithValidCredentials_ShouldReturnTokens()
    {
        // Arrange
        var loginRequest = new LoginRequest("admin@cineverse.com", "TestPass_1");
        
        // Act
        var loginResponse = await UnauthorizedClient.Login(loginRequest);
        
        // Assert
        Assert.True(loginResponse.IsSuccess);
        Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);
        Assert.NotNull(loginResponse.Data);
        Assert.NotEmpty(loginResponse.Data.AccessToken);
        Assert.NotEmpty(loginResponse.Data.RefreshToken);
    }
}