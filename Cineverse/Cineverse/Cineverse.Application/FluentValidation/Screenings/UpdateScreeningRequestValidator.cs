using Cineverse.Application.Common.Extensions;
using Cineverse.Application.MediatR.Requests.Screenings.UpdateScreening;
using FluentValidation;

namespace Cineverse.Application.FluentValidation.Screenings;

public class UpdateScreeningRequestValidator : AbstractValidator<UpdateScreeningRequest>
{
    public UpdateScreeningRequestValidator()
    {
        RuleFor(x => x.Id)
            .MustBeValidObjectId();
        
        RuleFor(x => x.EndTime)
            .GreaterThan(x => x.StartTime)
            .WithMessage("End time must be greater than start time");
        
        RuleFor(x => x.TicketPrice)
            .GreaterThan(0)
            .WithMessage("Ticket price must be greater than 0");
    }
}