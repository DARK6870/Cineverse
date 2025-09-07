using Cineverse.Mongo.Schemas.Entities;
using MediatR;

namespace Cineverse.Application.MediatR.Requests.Movies.GetMovieById;

public record GetMovieByIdRequest(string Id) : IRequest<MovieEntity?>;