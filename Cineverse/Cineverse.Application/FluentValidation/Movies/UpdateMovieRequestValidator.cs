using Cineverse.Application.MediatR.Movies.Commands;
using FluentValidation;

namespace Cineverse.Application.FluentValidation.Movies;

public class UpdateMovieRequestValidator : AbstractValidator<UpdateMovieRequest>
{
    public UpdateMovieRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title cannot be empty");
        
        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description cannot be empty");
        
        RuleFor(x => x.Images.Length)
            .GreaterThan(0)
            .WithMessage("Images can not be empty");
        
        RuleForEach(x => x.Images)
            .NotEmpty()
            .WithMessage("Image can not be empty");

        RuleFor(x => x.Duration)
            .GreaterThan(40)
            .WithMessage("Invalid movie duration");
    }
}