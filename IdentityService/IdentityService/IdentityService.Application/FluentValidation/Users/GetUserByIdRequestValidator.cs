using FluentValidation;
using IdentityService.Application.Common.Extensions;
using IdentityService.Application.MediatR.Requests.Users.GetUserById;

namespace IdentityService.Application.FluentValidation.Users;

public class GetUserByIdRequestValidator : AbstractValidator<GetUserByIdRequest>
{
    public GetUserByIdRequestValidator()
    {
        RuleFor(x => x.Id)
            .MustBeValidObjectId();
    }
}