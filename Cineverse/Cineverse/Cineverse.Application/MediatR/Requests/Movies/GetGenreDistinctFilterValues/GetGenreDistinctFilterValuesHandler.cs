using Cineverse.Mongo.Repositories.Movie;
using MediatR;

namespace Cineverse.Application.MediatR.Requests.Movies.GetGenreDistinctFilterValues;

public class GetGenreDistinctFilterValuesHandler(
    IMovieRepository movieRepository
) : IRequestHandler<GetGenreDistinctFilterValuesRequest, IEnumerable<string>>
{
    public async Task<IEnumerable<string>> Handle(GetGenreDistinctFilterValuesRequest request, CancellationToken cancellationToken)
    {
        return await movieRepository.GetDistinctFieldValuesAsync(x => x.Genre, cancellationToken);
    }
}