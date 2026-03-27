namespace AutomationTests.Models.Identity.Requests.RestorePassword;

public record RestorePasswordRequest(
    string Email,
    string Code,
    string Password,
    string ConfirmPassword
);