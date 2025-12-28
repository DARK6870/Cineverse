using FluentValidation;
using IdentityService.Application.MediatR.Requests.Authentication.ChangePassword;

namespace IdentityService.Application.FluentValidation.Authentication;

public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password must not be empty")
            .NotEqual(x => x.NewPassword)
            .WithMessage("New password must be different from the current password");
        
        RuleFor(x => x.NewPassword)
            .Length(6, 20)
            .WithMessage("Password must be between 6 and 20 characters");
        
        RuleFor(x => x.ConfirmNewPassword)
            .Length(6, 20)
            .WithMessage("Password must be between 6 and 20 characters")
            .Equal(x => x.NewPassword)
            .WithMessage("Passwords do not match");
    }
}