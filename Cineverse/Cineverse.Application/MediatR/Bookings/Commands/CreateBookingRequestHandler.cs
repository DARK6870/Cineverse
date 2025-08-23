using System.Net;
using Cineverse.Infrastructure.Common.Exceptions;
using Cineverse.Infrastructure.Services.Interfaces;
using Cineverse.Mongo.Repositories.Interfaces;
using Cineverse.Mongo.Schemas.Entities;
using Cineverse.Mongo.Schemas.Enums;
using Cineverse.Notifications.Common.Builders;
using Cineverse.Notifications.Services.Interfaces;
using MediatR;

namespace Cineverse.Application.MediatR.Bookings.Commands;

public record CreateBookingRequest(
    string ScreeningId,
    string[] SeatsIds
) : IRequest<bool>;

public class CreateBookingRequestHandler(
    IBookingRepository bookingRepository,
    IHallRepository hallRepository,
    IScreeningRepository screeningRepository,
    INotificationService notificationService,
    IUserContext userContext
    ) : IRequestHandler<CreateBookingRequest, bool>
{
    public async Task<bool> Handle(CreateBookingRequest request, CancellationToken cancellationToken)
    {
        if (userContext.UserStatus == UserStatus.PendingEmailConfirmation)
            throw new ApiRequestException("Please confirm your email to create a booking.", HttpStatusCode.Forbidden);
        
        var screening = await screeningRepository.FindByIdAsync(request.ScreeningId, cancellationToken)
            ?? throw new ApiRequestException("Screening not found", HttpStatusCode.NotFound);
        
        if (await bookingRepository.ExistsAsync(
                x => x.SeatIds.Any(s => request.SeatsIds.Contains(s)),
                cancellationToken
                )
        ) throw new ApiRequestException("Some of the provided seats are already booked.", HttpStatusCode.Conflict);


        if (!await hallRepository.ExistsAsync(x =>
                    x.Id == screening.HallId &&
                    request.SeatsIds.All(s => x.Seats.Select(seat => seat.SeatId).Contains(s)),
                cancellationToken
           )
        ) throw new ApiRequestException("Some of the provided seat IDs do not exist in the selected hall", HttpStatusCode.BadRequest);

        // Create booking
        var booking = new BookingEntity
        {
            ScreeningId = request.ScreeningId,
            SeatIds = request.SeatsIds,
            UserId = userContext.UserId,
            TotalPrice = request.SeatsIds.Length * screening.TicketPrice
        };
        await bookingRepository.InsertOneAsync(booking, cancellationToken);

        // Send email notification
        var notification = new MessageBuilder
        {
            Title = "Your booking has been created",
            FullName = userContext.UserName,
            Message =
                $"Your booking has been created<br>" +
                $"Number of tickets: {request.SeatsIds.Length}<br>" +
                $"<b>Total tickets price: {booking.TotalPrice}$</b><br>"+
                $"<br>We are waiting for you on <b>{screening.Date} | {screening.StartTime}</b>",
            ActionUrl = $"https://www.bookings.com/bookings/{booking.Id}",
            ActionText = "to view booking details"
        };
        await notificationService.SendEmailNotification(
            userContext.Email,
            "Your booking has been created",
            notification
        );

        return true;
    }
}