using Cineverse.Identity.Services.RestorePassword;
using MediatR;

namespace Cineverse.Application.MediatR.Requests.Authentication.RestorePassword;

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