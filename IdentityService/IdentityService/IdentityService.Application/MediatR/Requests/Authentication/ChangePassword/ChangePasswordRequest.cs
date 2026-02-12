using MediatR;

namespace IdentityService.Application.MediatR.Requests.Authentication.ChangePassword;

public record ChangePasswordRequest(
    string Password,
    string NewPassword,
    string ConfirmNewPassword
) : IRequest<bool>;