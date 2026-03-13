using Cineverse.Application.Common.Extensions;
using Cineverse.Application.MediatR.Requests.Hall.UpdateHall;
using FluentValidation;

namespace Cineverse.Application.FluentValidation.Halls;

public class UpdateHallRequestValidator : AbstractValidator<UpdateHallRequest>
{
    public UpdateHallRequestValidator()
    {
        RuleFor(x => x.Id)
            .MustBeValidObjectId();
        
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name can not be empty");
        
        RuleFor(x => x.Seats)
            .NotEmpty()
            .WithMessage("Seats can not be empty");
    }
}