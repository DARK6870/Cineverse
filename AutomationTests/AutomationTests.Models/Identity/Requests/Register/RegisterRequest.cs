namespace AutomationTests.Models.Identity.Requests.Register;

public record RegisterRequest(
    string Email,
    string FirstName,
    string LastName,
    string Password,
    string ConfirmPassword
);