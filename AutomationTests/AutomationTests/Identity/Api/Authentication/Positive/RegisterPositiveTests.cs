using System.Text.RegularExpressions;
using AutomationTests.Core.Common.Helpers;
using AutomationTests.Core.Identity.Base;
using AutomationTests.Core.Identity.Fixture;
using AutomationTests.Models.Identity.Requests.Register;
using Xunit;

namespace AutomationTests.Identity.Api.Authentication.Positive;

public class RegisterPositiveTests(IdentityFixture fixture) : IdentityApiTestBase(fixture)
{
    [Fact]
    public async Task RegisterAccount_ShouldCreateAccount_SendEmailConfirmationMessage_AndReturnTokens()
    {
        // Arrange
        var registerRequest = new RegisterRequest(
            "tsymbalvlad.6870@gmail.com",
            "Vlad",
            "Timbal",
            "TestPass1",
            "TestPass1"
        );
        
        // Act
        var registerResponse = await UnauthorizedClient.Register(registerRequest);
        Assert.True(registerResponse.IsSuccess);
        
        await Task.Delay(5000, TestContext.Current.CancellationToken);
        var emailMessages = await ImapHelper.GetNewMessages();
        Assert.True(emailMessages.Count > 0);
        
        var emailConfirmationMessage = emailMessages.Single(x => x.Subject == "Email Confirmation Code");
        Assert.NotNull(emailConfirmationMessage);
        
        // Assert
        Assert.NotNull(registerResponse.Data);
        Assert.NotNull(registerResponse.Data.AccessToken);
        Assert.NotNull(registerResponse.Data.RefreshToken);

        Assert.NotNull(emailConfirmationMessage.HtmlBody);
        Assert.Contains("Email Confirmation Code", emailConfirmationMessage.HtmlBody);
        Assert.Contains("Hello, Vlad Timbal!", emailConfirmationMessage.HtmlBody);
        Assert.Contains("Please complete your account setup to explore our website without restrictions", emailConfirmationMessage.HtmlBody);
        Assert.Contains("Your verification code is", emailConfirmationMessage.HtmlBody);
        Assert.Contains("Please click", emailConfirmationMessage.HtmlBody);
        Assert.Contains("to confirm your email", emailConfirmationMessage.HtmlBody);
        
        var codeMatch = Regex.Match(emailConfirmationMessage.HtmlBody, @"Your verification code is (\d{5})");
        Assert.True(codeMatch.Success);
        
        var confirmationCode = codeMatch.Groups[1].Value;
        Assert.False(string.IsNullOrEmpty(confirmationCode));
    }
}