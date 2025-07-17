using System.Net;
using Cineverse.Infrastructure.Common.Exceptions;
using Cineverse.Mongo.Repositories.Interfaces;
using Cineverse.Mongo.Schemas.Entities;
using MediatR;

namespace Cineverse.Application.MediatR.Screenings.Commands;

public record UpdateScreeningRequest(
    string Id,
    string MovieId,
    string HallId,
    DateOnly Date,
    TimeOnly StartTime,
    TimeOnly EndTime,
    int TicketPrice
) : IRequest<bool>;

public class UpdateScreeningRequestHandler(
    IScreeningRepository screeningRepository,
    IMovieRepository movieRepository,
    IHallRepository hallRepository
) : IRequestHandler<UpdateScreeningRequest, bool>
{
    public async Task<bool> Handle(UpdateScreeningRequest request, CancellationToken cancellationToken)
    {
        var screening = await screeningRepository.FindByIdAsync(request.Id, cancellationToken)
            ?? throw new ApiRequestException("Screening does not exist", HttpStatusCode.BadRequest);
        
        if (await movieRepository.ExistsAsync(x => x.Id == request.MovieId && x.IsAvailable, cancellationToken))
            throw new ApiRequestException("Movie does not exist", HttpStatusCode.BadRequest);
        
        if (!await hallRepository.ExistsAsync(x => x.Id == request.HallId, cancellationToken))
            throw new ApiRequestException("Hall does not exist", HttpStatusCode.BadRequest);

        var updatedScreening = new ScreeningEntity
        {
            Id = request.Id,
            MovieId = request.MovieId,
            HallId = request.HallId,
            Date = request.Date,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            TicketPrice = request.TicketPrice,
            DateCreated = screening.DateCreated
        };
        await screeningRepository.ReplaceOneAsync(updatedScreening, cancellationToken);
        
        return true;
    }
}