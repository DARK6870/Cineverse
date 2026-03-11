namespace AutomationTests.Core.Common.Constants.Cineverse;

public static class MovieGraphQlConstants
{
    public const string GetMoviesQuery =
        """
        query getMovies {
        movies(take: 250)
        {
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
}