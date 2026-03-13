using FluentValidation;
using MongoDB.Bson;

namespace Cineverse.Application.Common.Extensions;

public static class FluentValidationExtensions
{
    public static IRuleBuilderOptions<T, string> MustBeValidObjectId<T>(
        this IRuleBuilder<T, string> ruleBuilder
    )
    {
        return ruleBuilder
            .Must(id => ObjectId.TryParse(id, out _))
            .WithMessage("{PropertyName} must be a valid ObjectId");
    }
}