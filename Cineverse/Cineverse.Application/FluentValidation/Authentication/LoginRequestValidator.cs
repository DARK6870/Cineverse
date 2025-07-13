using Cineverse.Application.MediatR.Authentication.Commands;
using FluentValidation;

namespace Cineverse.Application.FluentValidation.Authentication;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email cannot be empty")
            .EmailAddress()
            .WithMessage("Email is not valid");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password cannot be empty");
    }
}