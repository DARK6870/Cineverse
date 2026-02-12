using MediatR;

namespace IdentityService.Application.MediatR.Requests.Authentication.ConfirmEmail;

public record ConfirmEmailRequest(int VerificationCode) : IRequest<bool>;