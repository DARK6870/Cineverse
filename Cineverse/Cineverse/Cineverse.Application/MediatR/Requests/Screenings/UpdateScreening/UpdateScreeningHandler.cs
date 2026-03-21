using Cineverse.Mongo.Repositories.Screening;
using Cineverse.Mongo.Schemas.Entities;
using MediatR;

namespace Cineverse.Application.MediatR.Requests.Screenings.UpdateScreening;

public class UpdateScreeningHandler(
    IScreeningRepository screeningRepository
) : IRequestHandler<UpdateScreeningRequest, bool>
{
    public async Task<bool> Handle(UpdateScreeningRequest request, CancellationToken cancellationToken)
    {
        var screening = await screeningRepository.FindByIdOrThrowAsync(
            request.Id,
            cancellationToken: cancellationToken
        );

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