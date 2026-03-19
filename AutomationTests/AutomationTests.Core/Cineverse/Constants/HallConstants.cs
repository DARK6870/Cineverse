namespace AutomationTests.Core.Cineverse.Constants;

public static class HallConstants
{
    public const string GetHallsQuery =
        """
        query getHallsPage {
          halls(take: 250) {
            items {
              id
              name
              dateCreated
              seats {
                seatId
                row
                number
              }
            }
            totalCount
          }
        }
        """;

    public const string GetHallByIdQuery =
        """
        query getHallById($id: String!) {
          hallById(id: $id) {
            id
            name
            dateCreated
            seats {
              seatId
              row
              number
            }
          }
        }
        """;

    public const string CreateHallMutation =
        """
        mutation createHallMutation($request: CreateHallRequestInput!) {
          createHall(request: $request)
        }
        """;

    public const string UpdateHallMutation =
        """
        mutation updateHallMutation($request: UpdateHallRequestInput!) {
          updateHall(request: $request)
        }
        """;

    public const string DeleteHallMutation =
        """
        mutation deleteHallMutation($id: String!) {
          deleteHall(id: $id)
        }
        """;
}