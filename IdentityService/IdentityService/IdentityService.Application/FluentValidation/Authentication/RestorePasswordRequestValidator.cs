using FluentValidation;
using IdentityService.Application.MediatR.Requests.Authentication.RestorePassword;

namespace IdentityService.Application.FluentValidation.Authentication;

public class RestorePasswordRequestValidator : AbstractValidator<RestorePasswordRequest>
{
    public RestorePasswordRequestValidator()
    {
        RuleFor(x => x.Password)
            .Length(6, 20)
            .WithMessage("Password must be between 6 and 20 characters");
        
        RuleFor(x => x.ConfirmPassword)
            .Length(6, 20)
            .WithMessage("Password must be between 6 and 20 characters")
            .Equal(x => x.Password)
            .WithMessage("Passwords do not match");
        
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email cannot be empty")
            .EmailAddress()
            .WithMessage("Invalid email address");
    }
}