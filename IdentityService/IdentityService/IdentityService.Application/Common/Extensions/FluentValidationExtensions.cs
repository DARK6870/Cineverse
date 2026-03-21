using FluentValidation;
using MongoDB.Bson;

namespace IdentityService.Application.Common.Extensions;

public static class FluentValidationExtensions
{
    public static IRuleBuilderOptions<T, string> MustBeValidObjectId<T>(
        this IRuleBuilderInitial<T, string> ruleBuilder
    )
    {
        return ruleBuilder
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("{PropertyName} cannot be empty")
            .Must(id => ObjectId.TryParse(id, out _))
            .WithMessage("{PropertyName} must be a valid ObjectId");
    }
}