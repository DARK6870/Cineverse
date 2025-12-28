using FluentValidation;
using IdentityService.Application.MediatR.Requests.Authentication.Register;

namespace IdentityService.Application.FluentValidation.Authentication;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email cannot be empty")
            .EmailAddress()
            .WithMessage("Email is not valid");
        
        RuleFor(x => x.Password)
            .Length(6, 20)
            .WithMessage("Password must be between 6 and 20 characters");
        
        RuleFor(x => x.ConfirmPassword)
            .Length(6, 20)
            .WithMessage("Password must be between 6 and 20 characters")
            .Equal(x => x.Password)
            .WithMessage("Passwords do not match");
        
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name cannot be empty");
        
        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name cannot be empty");
    }
}