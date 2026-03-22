using FluentValidation;
using IdentityService.Application.MediatR.Requests.Authentication.Register;
using IdentityService.Mongo.Repositories.User;
using Infrastructure.Common.Exceptions;

namespace IdentityService.Application.FluentValidation.Authentication;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator(IUserRepository userRepository)
    {
        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Email cannot be empty")
            .EmailAddress()
            .WithMessage("Email is not valid");
        
        RuleFor(x => x.Password)
            .Length(6, 20)
            .WithMessage("Password must be between 6 and 20 characters");
        
        RuleFor(x => x.ConfirmPassword)
            .Cascade(CascadeMode.Stop)
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
        
        RuleFor(x => x).CustomAsync(async (request, _, cancellationToken) =>
        {
            if (await userRepository.ExistsAsync(x => x.Email == request.Email, cancellationToken))
                throw new ConflictException("This email already exists");
        });
    }
}