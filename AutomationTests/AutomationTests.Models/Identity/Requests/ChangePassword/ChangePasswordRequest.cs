namespace AutomationTests.Models.Identity.Requests.ChangePassword;

public record ChangePasswordRequest(
    string Password,
    string NewPassword,
    string ConfirmNewPassword
);