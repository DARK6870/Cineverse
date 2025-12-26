using MediatR;

namespace IdentityService.Application.MediatR.Requests.Authentication.RestorePassword;

public record RestorePasswordRequest(
    string Email,
    string Code,
    string Password,
    string ConfirmPassword
) : IRequest<bool>;