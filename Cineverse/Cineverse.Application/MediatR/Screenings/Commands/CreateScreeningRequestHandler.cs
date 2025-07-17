using System.Net;
using Cineverse.Infrastructure.Common.Exceptions;
using Cineverse.Mongo.Repositories.Interfaces;
using Cineverse.Mongo.Schemas.Entities;
using MediatR;

namespace Cineverse.Application.MediatR.Screenings.Commands;

public record CreateScreeningRequest(
    string MovieId,
    string HallId,
    DateOnly Date,
    TimeOnly StartTime,
    TimeOnly EndTime,
    int TicketPrice
) : IRequest<bool>;

public class CreateScreeningRequestHandler(
    IScreeningRepository screeningRepository,
    IMovieRepository movieRepository,
    IHallRepository hallRepository
) : IRequestHandler<CreateScreeningRequest, bool>
{
    public async Task<bool> Handle(CreateScreeningRequest request, CancellationToken cancellationToken)
    {
        if (await movieRepository.ExistsAsync(x => x.Id == request.MovieId && x.IsAvailable, cancellationToken))
            throw new ApiRequestException("Movie does not exist", HttpStatusCode.BadRequest);
        
        if (!await hallRepository.ExistsAsync(x => x.Id == request.HallId, cancellationToken))
            throw new ApiRequestException("Hall does not exist", HttpStatusCode.BadRequest);

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