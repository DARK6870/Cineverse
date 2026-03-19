namespace AutomationTests.Core.Cineverse.Constants;

public static class MovieConstants
{
    public const string GetMoviesQuery =
        """
        query getMovies {
        movies(take: 250) {
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
          }
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