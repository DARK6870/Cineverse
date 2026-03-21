namespace AutomationTests.Core.Cineverse.Constants;

public static class MovieConstants
{
    public const string GetMoviesQuery =
        """
        query getMovies($take: Int, $skip: Int) {
        movies(take: $take, skip: $skip, where: {filter}, order: {sorting}) {
          items {
            id
            title
            genre
            description
            posterUrl
            trailerUrl
            releaseDate
            duration
            isAvailable
            dateCreated
            dateModified
          }
          pageInfo {
            hasNextPage
            hasPreviousPage
          }
          totalCount
         }
        }
        """;

    public const string GetMovieByIdQuery =
        """
        query getMovieById($id: String!){
          movieById(id: $id){
              id
              title
              genre
              description
              posterUrl
              trailerUrl
              releaseDate
              duration
              isAvailable
              dateCreated
              dateModified
          }
        }
        """;

    public const string GetGenreDistinctFilterValuesQuery =
        """
        query getGenreDistinctFilterValues {
          genreDistinctFilterValues
        }
        """;

    public const string CreateMovieMutation =
        """
        mutation createMovieMutation($request: CreateMovieRequestInput!) {
          createMovie(request: $request)
        }
        """;

    public const string UpdateMovieMutation =
        """
        mutation updateMovieMutation($request: UpdateMovieRequestInput!) {
          updateMovie(request: $request)
        }
        """;

    public const string DeleteMovieMutation =
        """
        mutation deleteMovieMutation($id: String!) {
          deleteMovie(id: $id)
        }
        """;
}