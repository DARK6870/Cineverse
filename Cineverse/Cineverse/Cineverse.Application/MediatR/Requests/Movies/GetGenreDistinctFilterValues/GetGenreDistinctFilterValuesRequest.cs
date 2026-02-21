using MediatR;

namespace Cineverse.Application.MediatR.Requests.Movies.GetGenreDistinctFilterValues;

public record GetGenreDistinctFilterValuesRequest : IRequest<IEnumerable<string>>;