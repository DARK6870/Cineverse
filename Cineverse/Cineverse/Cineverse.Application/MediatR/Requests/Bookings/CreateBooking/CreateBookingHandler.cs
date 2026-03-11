using Cineverse.Application.Notification.Extensions;
using Cineverse.Mongo.Repositories.Booking;
using Cineverse.Mongo.Repositories.Screening;
using Cineverse.Mongo.Schemas.Entities;
using Infrastructure.Context.UserContext;
using MediatR;
using Microsoft.Extensions.Options;
using NotificationService.Client.Models.Options;
using NotificationService.Client.Services;

namespace Cineverse.Application.MediatR.Requests.Bookings.CreateBooking;

public class CreateBookingHandler(
    IBookingRepository bookingRepository,
    IScreeningRepository screeningRepository,
    INotificationServiceClient notificationServiceClient,
    IUserContext userContext,
    IOptions<NotificationLinksOptions> notificationLinksOptions
) : IRequestHandler<CreateBookingRequest, string>
{
    public async Task<string> Handle(CreateBookingRequest request, CancellationToken cancellationToken)
    {
        var screening = await screeningRepository.FindByIdOrThrowAsync(request.ScreeningId, cancellationToken);

        // create booking
        var booking = new BookingEntity
        {
            ScreeningId = request.ScreeningId,
            SeatIds = request.SeatsIds,
            UserId = userContext.UserId,
            TotalPrice = request.SeatsIds.Length * screening.TicketPrice
        };
        await bookingRepository.InsertOneAsync(booking, cancellationToken);

        // send email notification
        var actionUrl = notificationLinksOptions.Value.BuildBookingDetailsUrl(booking.Id);
        
        await notificationServiceClient.SendBookingEmailAsync(
            userContext.Email,
            userContext.UserName,
            booking.SeatIds.Length,
            booking.TotalPrice,
            screening.Date,
            screening.StartTime,
            actionUrl
        );

        return booking.Id;
    }
}
