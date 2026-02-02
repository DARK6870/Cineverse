using Cineverse.Mongo.Schemas.Entities;
using MediatR;

namespace Cineverse.Application.MediatR.Requests.Hall.GetHallById;

public record GetHallByIdRequest(string Id) : IRequest<HallEntity>;