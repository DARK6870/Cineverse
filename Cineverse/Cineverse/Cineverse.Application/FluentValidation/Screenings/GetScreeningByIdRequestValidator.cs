using Cineverse.Application.Common.Extensions;
using Cineverse.Application.MediatR.Requests.Screenings.GetScreeningById;
using FluentValidation;

namespace Cineverse.Application.FluentValidation.Screenings;

public class GetScreeningByIdRequestValidator : AbstractValidator<GetScreeningByIdRequest>
{
    public GetScreeningByIdRequestValidator()
    {
        RuleFor(x => x.Id)
            .MustBeValidObjectId();
    }
}