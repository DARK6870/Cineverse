using Cineverse.Application.Common.Extensions;
using Cineverse.Application.MediatR.Requests.Hall.DeleteHall;
using Cineverse.Mongo.Repositories.Screening;
using FluentValidation;
using Infrastructure.Common.Exceptions;

namespace Cineverse.Application.FluentValidation.Halls;

public class DeleteHallRequestValidator : AbstractValidator<DeleteHallRequest>
{
    public DeleteHallRequestValidator(IScreeningRepository screeningRepository)
    {
        RuleFor(x => x.Id)
            .MustBeValidObjectId()
            .DependentRules(() =>
            {
                RuleFor(x => x).CustomAsync(async (request, _, cancellationToken) =>
                {
                    if (await screeningRepository.ExistsAsync(x => x.HallId == request.Id, cancellationToken))
                        throw new ConflictException("Please remove all screenings assigned to this hall before deleting");
                });
            });
    }
}