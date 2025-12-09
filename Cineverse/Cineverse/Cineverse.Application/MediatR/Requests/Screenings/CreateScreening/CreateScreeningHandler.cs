using Cineverse.Mongo.Repositories.Hall;
using Cineverse.Mongo.Repositories.Movie;
using Cineverse.Mongo.Repositories.Screening;
using Cineverse.Mongo.Schemas.Entities;
using MediatR;

namespace Cineverse.Application.MediatR.Requests.Screenings.CreateScreening;

public class CreateScreeningHandler(
    IScreeningRepository screeningRepository,
    IMovieRepository movieRepository,
    IHallRepository hallRepository
) : IRequestHandler<CreateScreeningRequest, bool>
{
    public async Task<bool> Handle(CreateScreeningRequest request, CancellationToken cancellationToken)
    {
        await movieRepository.ExistOrThrowAsync(
            x => x.Id == request.MovieId &&
                 x.IsAvailable,
            cancellationToken: cancellationToken
        );

        await hallRepository.ExistOrThrowAsync(
            x => x.Id == request.HallId,
            cancellationToken: cancellationToken
        );

        var screening = new ScreeningEntity
        {
            MovieId = request.MovieId,
            HallId = request.HallId,
            StartTime = request.StartTime,
            Date = request.Date,
            EndTime = request.EndTime,
            TicketPrice = request.TicketPrice
        };
        await screeningRepository.InsertOneAsync(screening, cancellationToken);
        
        return true;
    }
}