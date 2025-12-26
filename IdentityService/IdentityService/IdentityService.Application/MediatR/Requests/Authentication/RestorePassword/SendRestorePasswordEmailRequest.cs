using MediatR;

namespace IdentityService.Application.MediatR.Requests.Authentication.RestorePassword;

public record SendRestorePasswordEmailRequest(
    string Email    
) : IRequest<bool>;