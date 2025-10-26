using Cineverse.Identity.Services.RestorePassword;
using MediatR;

namespace Cineverse.Application.MediatR.Requests.Authentication.RestorePassword;

public class RestorePasswordHandler(
    IRestorePasswordService restorePasswordService
) : IRequestHandler<RestorePasswordRequest, bool>
{
    public async Task<bool> Handle(RestorePasswordRequest request, CancellationToken cancellationToken)
    {
        return await restorePasswordService.RestorePasswordByCode(request.Email, request.Code, request.Password);
    }
}