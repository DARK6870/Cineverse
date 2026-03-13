using Auth.Models.Enums;
using IdentityService.Application.Services.EmailVerification;
using Infrastructure.Common.Exceptions;
using Infrastructure.Context.UserContext;
using MediatR;

namespace IdentityService.Application.MediatR.Requests.Authentication.GenerateEmailVerificationCode;

public class GenerateEmailVerificationCodeHandler(
    IEmailVerificationService emailVerificationService,
    IUserContext userContext
) : IRequestHandler<GenerateVerificationCodeRequest, bool>
{
    public async Task<bool> Handle(GenerateVerificationCodeRequest request, CancellationToken cancellationToken)
    {
        if (userContext.UserStatus is not UserStatus.PendingEmailConfirmation)
            throw new ConflictException("Email already confirmed");
        
        await emailVerificationService.GenerateAndSendVerificationCodeAsync(userContext.Email, userContext.UserName);
        
        return true;
    }
}