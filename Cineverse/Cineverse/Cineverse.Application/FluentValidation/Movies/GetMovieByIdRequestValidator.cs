using Cineverse.Application.Common.Extensions;
using Cineverse.Application.MediatR.Requests.Movies.GetMovieById;
using FluentValidation;

namespace Cineverse.Application.FluentValidation.Movies;

public class GetMovieByIdRequestValidator : AbstractValidator<GetMovieByIdRequest>
{
    public GetMovieByIdRequestValidator()
    {
        RuleFor(x => x.Id)
            .MustBeValidObjectId();
    }
}