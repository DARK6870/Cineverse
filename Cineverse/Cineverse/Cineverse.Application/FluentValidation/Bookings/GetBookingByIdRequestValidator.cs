using Cineverse.Application.Common.Extensions;
using Cineverse.Application.MediatR.Requests.Bookings.GetBookingById;
using FluentValidation;

namespace Cineverse.Application.FluentValidation.Bookings;

public class GetBookingByIdRequestValidator : AbstractValidator<GetBookingByIdRequest>
{
    public  GetBookingByIdRequestValidator()
    {
        RuleFor(x => x.Id)
            .MustBeValidObjectId();
    }
}