using Cineverse.Identity.Services.Authentication;
using MediatR;

namespace Cineverse.Application.MediatR.Authentication.Commands;

public record ConfirmEmailRequest(int VerificationCode) : IRequest<bool>;

// TODO: rename to ConfirmEmailHandler
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