using Cineverse.Infrastructure.Services.Interfaces;
using MediatR;

namespace Cineverse.Application.MediatR.Authentication.Commands;

public record ConfirmEmailRequest(int VerificationCode) : IRequest<bool>;

public class ConfirmEmailRequestHandler(
    IAuthenticationService authenticationService
) : IRequestHandler<ConfirmEmailRequest, bool>
{
    public async Task<bool> Handle(ConfirmEmailRequest request, CancellationToken cancellationToken)
    {
        await authenticationService.ConfirmUserEmailAsync(request.VerificationCode);

        return true;
    }
}