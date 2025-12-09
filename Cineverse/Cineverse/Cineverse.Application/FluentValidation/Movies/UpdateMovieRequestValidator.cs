using Cineverse.Application.MediatR.Requests.Movies.UpdateMovie;
using FluentValidation;

namespace Cineverse.Application.FluentValidation.Movies;

public class UpdateMovieRequestValidator : AbstractValidator<UpdateMovieRequest>
{
    public UpdateMovieRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title cannot be empty");
        
        RuleFor(x => x.Genre)
            .NotEmpty()
            .WithMessage("Genre cannot be empty");
        
        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description cannot be empty");
        
        RuleFor(x => x.PosterUrl)
            .NotEmpty()
            .WithMessage("PosterUrl cannot be empty");
        
        RuleForEach(x => x.TrailerUrl)
            .NotEmpty()
            .WithMessage("TrailerUrl cannot be empty");

        RuleFor(x => x.Duration)
            .GreaterThan(40)
            .WithMessage("Invalid movie duration");
    }
}