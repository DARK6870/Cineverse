using MediatR;

namespace IdentityService.Application.MediatR.Requests.Authentication.GenerateEmailVerificationCode;

public record GenerateVerificationCodeRequest : IRequest<bool>;