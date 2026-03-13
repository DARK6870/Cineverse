using Cineverse.Application.Common.Extensions;
using Cineverse.Application.MediatR.Requests.Screenings.DeleteScreening;
using Cineverse.Mongo.Repositories.Booking;
using FluentValidation;
using Infrastructure.Common.Exceptions;

namespace Cineverse.Application.FluentValidation.Screenings;

public class DeleteScreeningRequestValidator : AbstractValidator<DeleteScreeningRequest>
{
    public  DeleteScreeningRequestValidator(IBookingRepository bookingRepository)
    {
        RuleFor(x => x.Id)
            .MustBeValidObjectId()
            .DependentRules(() =>
            {
                RuleFor(x => x).CustomAsync(async (request, _, cancellationToken) =>
                {
                    if (await bookingRepository.ExistsAsync(x => x.ScreeningId == request.Id, cancellationToken))
                        throw new ConflictException("Please remove all bookings assigned to this screening before deleting");
                });
            });
    }
}