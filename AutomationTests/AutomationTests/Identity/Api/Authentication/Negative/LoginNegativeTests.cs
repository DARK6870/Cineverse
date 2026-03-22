using System.Net;
using AutomationTests.Core.Identity.Base;
using AutomationTests.Core.Identity.Fixture;
using AutomationTests.Models.Identity.Requests;
using Xunit;

namespace AutomationTests.Identity.Api.Authentication.Negative;

public class LoginNegativeTests(IdentityFixture fixture) : IdentityApiTestBase(fixture)
{
    [Fact]
    public async Task Login_WithInvalidRequest_ShouldReturnValidationErrors()
    {
        // Arrange
        var loginRequest = new LoginRequest("", "");

        // Act
        var loginResponse = await UnauthorizedClient.Login(loginRequest);

        // Assert
        Assert.False(loginResponse.IsSuccess);
        Assert.Equal(HttpStatusCode.BadRequest, loginResponse.StatusCode);
        Assert.True(loginResponse.ValidationErrors.Length is 2);
        Assert.Equal("Validation Failed", loginResponse.ErrorMessage);
        Assert.Contains("Email cannot be empty", loginResponse.ValidationErrors);
        Assert.Contains("Password cannot be empty", loginResponse.ValidationErrors);
    }

    [Fact]
    public async Task Login_WithInvalidEmail_ShouldReturnValidationError()
    {
        // Arrange
        var loginRequest = new LoginRequest("invalid_email", "valid_password");

        // Act
        var loginResponse = await UnauthorizedClient.Login(loginRequest);

        // Assert
        Assert.False(loginResponse.IsSuccess);
        Assert.Equal(HttpStatusCode.BadRequest, loginResponse.StatusCode);
        Assert.True(loginResponse.ValidationErrors.Length is 1);
        Assert.Equal("Validation Failed", loginResponse.ErrorMessage);
        Assert.Contains("Email is not valid", loginResponse.ValidationErrors);
    }

    [Fact]
    public async Task Login_WithInvalidCredentials_ShouldReturnError()
    {
        // Arrange
        var loginRequest = new LoginRequest("invalid@cineverse.com", "invalid_password");
        
        // Act
        var loginResponse = await UnauthorizedClient.Login(loginRequest);
        
        // Assert
        Assert.False(loginResponse.IsSuccess);
        Assert.NotNull(loginResponse.ErrorMessage);
        Assert.Equal(HttpStatusCode.BadRequest, loginResponse.StatusCode);
    }
}