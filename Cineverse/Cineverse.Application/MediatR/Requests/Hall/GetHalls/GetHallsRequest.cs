using Cineverse.Mongo.Schemas.Entities;
using MediatR;

namespace Cineverse.Application.MediatR.Requests.Hall.GetHalls;

public record GetHallsRequest : IRequest<IQueryable<HallEntity>>;