using Cineverse.Application.Common.Extensions;
using Cineverse.Application.MediatR.Requests.Movies.DeleteMovie;
using Cineverse.Mongo.Repositories.Screening;
using FluentValidation;
using Infrastructure.Common.Exceptions;

namespace Cineverse.Application.FluentValidation.Movies;

public class DeleteMovieRequestValidator : AbstractValidator<DeleteMovieRequest>
{
    public DeleteMovieRequestValidator(IScreeningRepository screeningRepository)
    {
        RuleFor(x => x.Id)
            .MustBeValidObjectId()
            .DependentRules(() =>
            {
                RuleFor(x => x).CustomAsync(async (request, _, cancellationToken) =>
                {
                    if (await screeningRepository.ExistsAsync(x => x.MovieId == request.Id, cancellationToken))
                        throw new ConflictException("Please remove all screenings assigned to this movie before deleting");
                });
            });
    }
}