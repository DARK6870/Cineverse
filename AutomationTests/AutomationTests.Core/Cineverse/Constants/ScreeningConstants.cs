namespace AutomationTests.Core.Cineverse.Constants;

public static class ScreeningConstants
{
    public const string GetScreeningsQuery =
        """
        query getScreeningsPage {
          screenings(take: 250) {
            items {
              id
              movieId
              hallId
              date
              startTime
              endTime
              ticketPrice
              dateCreated
              dateModified
            }
            totalCount
          }
        }
        """;

    public const string GetScreeningByIdQuery =
        """
        query getScreeningById($id: String!) {
          screeningById(id: $id) {
            id
            movieId
            hallId
            date
            startTime
            endTime
            ticketPrice
            dateCreated
            dateModified
          }
        }
        """;

    public const string CreateScreeningMutation =
        """
        mutation createScreeningMutation($request: CreateScreeningRequestInput!) {
          createScreening(request: $request)
        }
        """;

    public const string UpdateScreeningMutation =
        """
        mutation updateScreeningMutation($request: UpdateScreeningRequestInput!) {
          updateScreening(request: $request)
        }
        """;

    public const string DeleteScreeningMutation =
        """
        mutation deleteScreeningMutation($id: String!) {
          deleteScreening(id: $id)
        }
        """;
}