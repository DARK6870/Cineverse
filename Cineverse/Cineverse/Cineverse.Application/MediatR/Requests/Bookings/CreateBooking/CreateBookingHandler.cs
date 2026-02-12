using System.Net;
using Auth.Models.Enums;
using Cineverse.Application.Notification.Extensions;
using Cineverse.Mongo.Repositories.Booking;
using Cineverse.Mongo.Repositories.Hall;
using Cineverse.Mongo.Repositories.Screening;
using Cineverse.Mongo.Schemas.Entities;
using Infrastructure.Context.UserContext;
using Infrastructure.WebApi.Exceptions;
using MediatR;
using Microsoft.Extensions.Options;
using NotificationService.Client.Models.Options;
using NotificationService.Client.Services;

namespace Cineverse.Application.MediatR.Requests.Bookings.CreateBooking;

public class CreateBookingHandler(
    IBookingRepository bookingRepository,
    IHallRepository hallRepository,
    IScreeningRepository screeningRepository,
    INotificationServiceClient notificationServiceClient,
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
                    request.SeatsIds.All(s => x.Seats.Select(seat => seat.SeatId).Contains(s)),
                cancellationToken
           )
        ) throw new ApiRequestException("Some of the provided seat IDs do not exist in the selected hall", HttpStatusCode.BadRequest);

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

        return true;
    }
}