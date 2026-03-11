namespace AutomationTests.Models.Identity.Requests;

public record LoginRequest(
    string Email,
    string Password
);