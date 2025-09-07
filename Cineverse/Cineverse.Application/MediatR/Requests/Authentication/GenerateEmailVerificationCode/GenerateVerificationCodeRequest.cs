using MediatR;

namespace Cineverse.Application.MediatR.Requests.Authentication.GenerateEmailVerificationCode;

public record GenerateVerificationCodeRequest : IRequest<bool>;