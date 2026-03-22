using FluentValidation;
using IdentityService.Application.MediatR.Requests.Authentication.RestorePassword;

namespace IdentityService.Application.FluentValidation.Authentication;

public class SendRestorePasswordEmailRequestValidator : AbstractValidator<SendRestorePasswordEmailRequest>
{
    public SendRestorePasswordEmailRequestValidator()
    {
        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Email cannot be empty")
            .EmailAddress()
            .WithMessage("Invalid email address");
    }
}