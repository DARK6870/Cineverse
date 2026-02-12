using MediatR;

namespace Cineverse.Application.MediatR.Requests.Movies.DeleteMovie;

public record DeleteMovieRequest(string Id) : IRequest<bool>;