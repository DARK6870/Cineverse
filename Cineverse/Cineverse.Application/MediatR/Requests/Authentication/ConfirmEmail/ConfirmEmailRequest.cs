using MediatR;

namespace Cineverse.Application.MediatR.Requests.Authentication.ConfirmEmail;

public record ConfirmEmailRequest(int VerificationCode) : IRequest<bool>;