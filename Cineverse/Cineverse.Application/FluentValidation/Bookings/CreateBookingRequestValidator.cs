using Cineverse.Application.MediatR.Requests.Bookings.CreateBooking;
using FluentValidation;

namespace Cineverse.Application.FluentValidation.Bookings;

public class CreateBookingRequestValidator : AbstractValidator<CreateBookingRequest>
{
    public CreateBookingRequestValidator()
    {
        RuleFor(x => x.SeatsIds)
            .NotEmpty()
            .WithMessage("SeatsIds can not be empty");
    }
}