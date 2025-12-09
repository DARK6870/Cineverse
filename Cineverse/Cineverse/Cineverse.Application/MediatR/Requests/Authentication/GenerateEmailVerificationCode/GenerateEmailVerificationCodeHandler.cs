using System.Net;
using Cineverse.Domain.Common.Exceptions;
using Cineverse.Identity.Services.EmailVerification;
using Cineverse.Identity.Services.UserContext;
using Cineverse.Mongo.Schemas.Enums;
using MediatR;

namespace Cineverse.Application.MediatR.Requests.Authentication.GenerateEmailVerificationCode;

public class GenerateEmailVerificationCodeHandler(
    IVerificationService verificationService,
    IUserContext userContext
) : IRequestHandler<GenerateVerificationCodeRequest, bool>
{
    public async Task<bool> Handle(GenerateVerificationCodeRequest request, CancellationToken cancellationToken)
    {
        if (userContext.UserStatus is not UserStatus.PendingEmailConfirmation)
            throw new ApiRequestException("Email already confirmed", HttpStatusCode.Conflict);
        
        await verificationService.GenerateAndSendVerificationCodeAsync(userContext.Email, userContext.UserName);
        
        return true;
    }
}