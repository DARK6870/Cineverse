using Cineverse.Application.Common.Extensions;
using Cineverse.Application.MediatR.Requests.Bookings.DeleteBooking;
using FluentValidation;

namespace Cineverse.Application.FluentValidation.Bookings;

public class DeleteBookingRequestValidator : AbstractValidator<DeleteBookingRequest>
{
    public DeleteBookingRequestValidator()
    {
        RuleFor(x => x.Id)
            .MustBeValidObjectId();
    }
}