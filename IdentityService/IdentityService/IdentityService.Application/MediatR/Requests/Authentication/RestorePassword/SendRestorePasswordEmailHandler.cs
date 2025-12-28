using IdentityService.Application.Services.Password;
using MediatR;

namespace IdentityService.Application.MediatR.Requests.Authentication.RestorePassword;

public class SendRestorePasswordEmailHandler(
    IRestorePasswordService restorePasswordService
) : IRequestHandler<SendRestorePasswordEmailRequest, bool>
{
    public async Task<bool> Handle(SendRestorePasswordEmailRequest request, CancellationToken cancellationToken)
    {
        await restorePasswordService.GenerateAndSendPasswordResetCode(request.Email);
        return true;
    }
}