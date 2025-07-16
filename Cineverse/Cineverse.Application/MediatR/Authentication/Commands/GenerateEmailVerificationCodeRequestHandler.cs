using Cineverse.Infrastructure.Services.Interfaces;
using MediatR;

namespace Cineverse.Application.MediatR.Authentication.Commands;

public record GenerateVerificationCodeRequest() : IRequest<bool>;

public class GenerateEmailVerificationCodeRequestHandler(
    IAuthenticationService authenticationService,
    IUserContext userContext
) : IRequestHandler<GenerateVerificationCodeRequest, bool>
{
    public async Task<bool> Handle(GenerateVerificationCodeRequest request, CancellationToken cancellationToken)
    {
        await authenticationService.GenerateVerificationCodeAsync(userContext.Email);
        
        return true;
    }
}