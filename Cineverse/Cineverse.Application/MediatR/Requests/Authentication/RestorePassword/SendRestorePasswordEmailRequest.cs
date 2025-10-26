using MediatR;

namespace Cineverse.Application.MediatR.Requests.Authentication.RestorePassword;

public record SendRestorePasswordEmailRequest(
    string Email    
) : IRequest<bool>;