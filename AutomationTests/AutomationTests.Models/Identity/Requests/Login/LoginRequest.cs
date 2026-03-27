namespace AutomationTests.Models.Identity.Requests.Login;

public record LoginRequest(
    string Email,
    string Password
);