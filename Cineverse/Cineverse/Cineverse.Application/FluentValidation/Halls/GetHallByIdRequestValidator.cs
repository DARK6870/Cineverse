using Cineverse.Application.Common.Extensions;
using Cineverse.Application.MediatR.Requests.Hall.GetHallById;
using FluentValidation;

namespace Cineverse.Application.FluentValidation.Halls;

public class GetHallByIdRequestValidator : AbstractValidator<GetHallByIdRequest>
{
    public  GetHallByIdRequestValidator()
    {
        RuleFor(x => x.Id)
            .MustBeValidObjectId();
    }
}