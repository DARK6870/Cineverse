namespace AutomationTests.Models.Identity.Responses;

public record AuthenticationResponse(
    string RefreshToken,
    string AccessToken
);
