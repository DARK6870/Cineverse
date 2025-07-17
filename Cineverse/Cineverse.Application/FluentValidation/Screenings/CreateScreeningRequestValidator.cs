using Cineverse.Application.MediatR.Screenings.Commands;
using FluentValidation;

namespace Cineverse.Application.FluentValidation.Screenings;

public class CreateScreeningRequestValidator : AbstractValidator<CreateScreeningRequest>
{
    public CreateScreeningRequestValidator()
    {
        RuleFor(x => x.EndTime)
            .GreaterThan(x => x.StartTime)
            .WithMessage("End time must be greater than start time");
        
        RuleFor(x => x.TicketPrice)
            .GreaterThan(0)
            .WithMessage("Ticket price must be greater than 0");
    }
}