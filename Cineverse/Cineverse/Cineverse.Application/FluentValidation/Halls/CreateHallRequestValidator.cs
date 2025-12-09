using Cineverse.Application.MediatR.Requests.Hall.CreateHall;
using FluentValidation;

namespace Cineverse.Application.FluentValidation.Halls;

public class CreateHallRequestValidator : AbstractValidator<CreateHallRequest>
{
    public CreateHallRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name can not be empty");
        
        RuleFor(x => x.Seats)
            .NotEmpty()
            .WithMessage("Seats can not be empty");
    }
}