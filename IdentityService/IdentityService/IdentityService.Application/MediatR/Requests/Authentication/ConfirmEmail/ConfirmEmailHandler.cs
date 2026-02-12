using IdentityService.Application.Services.EmailVerification;
using MediatR;

namespace IdentityService.Application.MediatR.Requests.Authentication.ConfirmEmail;

public class ConfirmEmailHandler(
    IEmailVerificationService emailVerificationService
) : IRequestHandler<ConfirmEmailRequest, bool>
{
    public async Task<bool> Handle(ConfirmEmailRequest request, CancellationToken cancellationToken)
    {
        await emailVerificationService.ConfirmEmailAsync(request.VerificationCode);
        return true;
    }
}