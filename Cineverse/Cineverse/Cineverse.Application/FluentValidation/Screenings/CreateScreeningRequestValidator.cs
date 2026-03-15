using Cineverse.Application.MediatR.Requests.Screenings.CreateScreening;
using FluentValidation;

namespace Cineverse.Application.FluentValidation.Screenings;

public class CreateScreeningRequestValidator : AbstractValidator<CreateScreeningRequest>
{
    public CreateScreeningRequestValidator()
    {
        RuleFor(x => x.Date)
            .GreaterThan(DateOnly.FromDateTime(DateTime.Now))
            .WithMessage("Date must be greater than today's date");
        
        RuleFor(x => x.EndTime)
            .GreaterThan(x => x.StartTime)
            .WithMessage("End time must be greater than start time");
        
        RuleFor(x => x.TicketPrice)
            .GreaterThan(0)
            .WithMessage("Ticket price must be greater than 0");
    }
}