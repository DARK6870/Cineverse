using Auth.Models.Enums;
using Cineverse.Application.MediatR.Requests.Bookings.CreateBooking;
using Cineverse.Mongo.Repositories.Booking;
using Cineverse.Mongo.Repositories.Hall;
using Cineverse.Mongo.Repositories.Screening;
using FluentValidation;
using Infrastructure.Common.Exceptions;
using Infrastructure.Context.UserContext;

namespace Cineverse.Application.FluentValidation.Bookings;

public class CreateBookingRequestValidator : AbstractValidator<CreateBookingRequest>
{
    public CreateBookingRequestValidator(
        IBookingRepository bookingRepository,
        IHallRepository hallRepository,
        IScreeningRepository screeningRepository,
        IUserContext userContext
    )
    {
        RuleFor(x => x.SeatsIds)
            .NotEmpty()
            .WithMessage("SeatsIds can not be empty");

        RuleFor(x => x).CustomAsync(async (request, _, cancellationToken) =>
        {
            if (userContext.UserStatus == UserStatus.PendingEmailConfirmation)
                throw new ForbiddenException("Please confirm your email to create a booking");

            var screening = await screeningRepository.FindByIdOrThrowAsync(request.ScreeningId, cancellationToken);

            if (await bookingRepository.ExistsAsync(
                    x => x.ScreeningId == request.ScreeningId &&
                         x.SeatIds.Any(s => request.SeatsIds.Contains(s)),
                    cancellationToken
                ))
                throw new ConflictException("Some of the provided seats are already booked");

            if (!await hallRepository.ExistsAsync(x =>
                    x.Id == screening.HallId &&
                    request.SeatsIds.All(s => x.Seats.Select(seat => seat.SeatId).Contains(s)),
                cancellationToken
            )) throw new NotFoundException("Some of the provided seats were not found");
        });
    }
}
