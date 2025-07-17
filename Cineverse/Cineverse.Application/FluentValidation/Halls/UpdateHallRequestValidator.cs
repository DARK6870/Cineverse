using Cineverse.Application.MediatR.Hall.Commands;
using FluentValidation;

namespace Cineverse.Application.FluentValidation.Halls;

public class UpdateHallRequestValidator : AbstractValidator<UpdateHallRequest>
{
    public UpdateHallRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name can not be empty");
        
        RuleFor(x => x.Seats)
            .NotEmpty()
            .WithMessage("Seats can not be empty");
    }
}