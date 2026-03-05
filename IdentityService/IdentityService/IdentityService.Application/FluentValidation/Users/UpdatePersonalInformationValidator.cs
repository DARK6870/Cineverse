using FluentValidation;
using IdentityService.Application.MediatR.Requests.Users.UpdatePersonalInformation;

namespace IdentityService.Application.FluentValidation.Users;

public class UpdatePersonalInformationValidator : AbstractValidator<UpdatePersonalInformationRequest>
{
    public UpdatePersonalInformationValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name cannot be empty");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name cannot be empty");
    }
}