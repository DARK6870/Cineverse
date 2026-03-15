using Cineverse.Application.Common.Extensions;
using Cineverse.Application.MediatR.Requests.Movies.UpdateMovie;
using Cineverse.Mongo.Repositories.Movie;
using FluentValidation;
using Infrastructure.Common.Exceptions;

namespace Cineverse.Application.FluentValidation.Movies;

public class UpdateMovieRequestValidator : AbstractValidator<UpdateMovieRequest>
{
    public UpdateMovieRequestValidator(IMovieRepository movieRepository)
    {
        RuleFor(x => x.Id)
            .MustBeValidObjectId();
        
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title cannot be empty")
            .DependentRules(() =>
            {
                RuleFor(x => x).CustomAsync(async (request, _, cancellationToken) =>
                {
                    if (await movieRepository.ExistsAsync(x => x.Title == request.Title && x.ReleaseDate == request.ReleaseDate, cancellationToken))
                        throw new ConflictException("A movie with the same title and release date already exists");
                });
            });;
        
        RuleFor(x => x.Genre)
            .NotEmpty()
            .WithMessage("Genre cannot be empty");
        
        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description cannot be empty");
        
        RuleFor(x => x.PosterUrl)
            .NotEmpty()
            .WithMessage("PosterUrl cannot be empty");
        
        RuleFor(x => x.TrailerUrl)
            .NotEmpty()
            .WithMessage("TrailerUrl cannot be empty");

        RuleFor(x => x.Duration)
            .GreaterThan(40)
            .WithMessage("Invalid movie duration");
    }
}