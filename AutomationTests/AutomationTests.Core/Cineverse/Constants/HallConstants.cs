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
              seats {
                seatId
                row
                number
              }
              dateCreated
              dateModified
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
            seats {
              seatId
              row
              number
            }
            dateCreated
            dateModified
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