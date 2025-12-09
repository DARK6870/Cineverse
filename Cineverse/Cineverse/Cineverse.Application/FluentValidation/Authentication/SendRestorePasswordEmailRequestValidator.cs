using Cineverse.Application.MediatR.Requests.Authentication.RestorePassword;
using FluentValidation;

namespace Cineverse.Application.FluentValidation.Authentication;

public class SendRestorePasswordEmailRequestValidator : AbstractValidator<SendRestorePasswordEmailRequest>
{
    public SendRestorePasswordEmailRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email cannot be empty")
            .EmailAddress()
            .WithMessage("Invalid email address");
    }
}