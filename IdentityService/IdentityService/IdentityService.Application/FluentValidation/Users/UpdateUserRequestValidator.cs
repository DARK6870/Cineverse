using FluentValidation;
using IdentityService.Application.Common.Extensions;
using IdentityService.Application.MediatR.Requests.Users.UpdateUser;

namespace IdentityService.Application.FluentValidation.Users;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.Id)
            .MustBeValidObjectId();
    }
}