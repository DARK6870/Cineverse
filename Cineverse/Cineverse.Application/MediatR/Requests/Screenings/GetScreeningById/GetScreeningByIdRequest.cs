using Cineverse.Mongo.Schemas.Entities;
using MediatR;

namespace Cineverse.Application.MediatR.Requests.Screenings.GetScreeningById;

public record GetScreeningByIdRequest(string Id) : IRequest<ScreeningEntity?>;