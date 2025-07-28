using Cineverse.Application.MediatR.Bookings.Commands;
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