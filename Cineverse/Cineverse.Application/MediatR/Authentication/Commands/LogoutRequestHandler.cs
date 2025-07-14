using Cineverse.Infrastructure.Services.Interfaces;
using MediatR;

namespace Cineverse.Application.MediatR.Authentication.Commands;

public record LogoutRequest : IRequest<bool>;

public class LogoutRequestHandler(
    IAuthenticationService authenticationService
)
{
    public async Task<bool> Handle(LogoutRequest request, CancellationToken cancellationToken)
    {
        await authenticationService.LogoutUserAsync();
        return true;
    }
}