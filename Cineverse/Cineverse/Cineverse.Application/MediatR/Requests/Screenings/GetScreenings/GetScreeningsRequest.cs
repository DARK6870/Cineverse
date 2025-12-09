using Cineverse.Mongo.Schemas.Entities;
using MediatR;

namespace Cineverse.Application.MediatR.Requests.Screenings.GetScreenings;

public record GetScreeningsRequest : IRequest<IQueryable<ScreeningEntity>>;