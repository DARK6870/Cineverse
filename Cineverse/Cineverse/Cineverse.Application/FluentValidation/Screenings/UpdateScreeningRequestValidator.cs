using Cineverse.Application.Common.Extensions;
using Cineverse.Application.MediatR.Requests.Screenings.UpdateScreening;
using Cineverse.Mongo.Repositories.Hall;
using Cineverse.Mongo.Repositories.Movie;
using FluentValidation;

namespace Cineverse.Application.FluentValidation.Screenings;

public class UpdateScreeningRequestValidator : AbstractValidator<UpdateScreeningRequest>
{
    public UpdateScreeningRequestValidator(
        IMovieRepository movieRepository,
        IHallRepository hallRepository
    )
    {
        RuleFor(x => x.Id)
            .MustBeValidObjectId();
        
        RuleFor(x => x.MovieId)
            .MustBeValidObjectId()
            .DependentRules(() =>
            {
                RuleFor(x => x).CustomAsync(async (request, _, cancellationToken) =>
                {
                    await movieRepository.ExistOrThrowAsync(x => x.Id == request.MovieId, cancellationToken);
                });
            });
        
        RuleFor(x => x.HallId)
            .MustBeValidObjectId()
            .DependentRules(() =>
            {
                RuleFor(x => x).CustomAsync(async (request, _, cancellationToken) =>
                {
                    await hallRepository.ExistOrThrowAsync(x => x.Id == request.HallId, cancellationToken);
                });
            });;
        
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