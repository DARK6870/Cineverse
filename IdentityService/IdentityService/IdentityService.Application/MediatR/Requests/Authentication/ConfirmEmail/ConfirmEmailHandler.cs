/*using MediatR;
using Microsoft.AspNetCore.Authentication;

namespace IdentityService.Application.MediatR.Requests.Authentication.ConfirmEmail;

public class ConfirmEmailHandler(
    IAuthenticationService authenticationService
) : IRequestHandler<ConfirmEmailRequest, bool>
{
    public async Task<bool> Handle(ConfirmEmailRequest request, CancellationToken cancellationToken)
    {
        await authenticationService.ConfirmUserEmailAsync(request.VerificationCode);

        return true;
    }
}*/