using System.Net;
using Cineverse.Domain.Common.Exceptions;
using Cineverse.Identity.Services.UserContext;
using Cineverse.Mongo.Repositories.Booking;
using Cineverse.Mongo.Repositories.Hall;
using Cineverse.Mongo.Repositories.Screening;
using Cineverse.Mongo.Schemas.Entities;
using Cineverse.Mongo.Schemas.Enums;
using Cineverse.Notifications.Common.Builders;
using Cineverse.Notifications.Common.Options;
using Cineverse.Notifications.Services.Notification;
using Cineverse.Notifications.Services.Notification.Extensions;
using MediatR;
using Microsoft.Extensions.Options;

namespace Cineverse.Application.MediatR.Requests.Bookings.CreateBooking;

public class CreateBookingHandler(
    IBookingRepository bookingRepository,
    IHallRepository hallRepository,
    IScreeningRepository screeningRepository,
    INotificationService notificationService,
    IUserContext userContext,
    IOptions<NotificationLinksOptions> notificationLinksOptions
) : IRequestHandler<CreateBookingRequest, bool>
{
    public async Task<bool> Handle(CreateBookingRequest request, CancellationToken cancellationToken)
    {
        // TODO: change validation
        if (userContext.UserStatus == UserStatus.PendingEmailConfirmation)
            throw new ApiRequestException("Please confirm your email to create a booking.", HttpStatusCode.Forbidden);

        var screening = await screeningRepository.FindByIdOrThrowAsync(request.ScreeningId, cancellationToken);
        
        if (await bookingRepository.ExistsAsync(
                x => x.ScreeningId == request.ScreeningId &&
                x.SeatIds.Any(s => request.SeatsIds.Contains(s)),
                cancellationToken
                )
        ) throw new ApiRequestException("Some of the provided seats are already booked.", HttpStatusCode.Conflict);


        if (!await hallRepository.ExistsAsync(x =>
                    x.Id == screening.HallId &&
                    Enumerable.All<string>(request.SeatsIds, s => x.Seats.Select(seat => seat.SeatId).Contains(s)),
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
        var actionUrl = notificationLinksOptions.Value.BaseUrl + notificationLinksOptions.Value.BookingDetailsPath.Replace("id", booking.Id);
        
        await notificationService.SendBookingEmailAsync(
            userContext.Email,
            userContext.UserName,
            booking.SeatIds.Length,
            booking.TotalPrice,
            screening.Date,
            screening.StartTime,
            actionUrl
        );

        return true;
    }
}