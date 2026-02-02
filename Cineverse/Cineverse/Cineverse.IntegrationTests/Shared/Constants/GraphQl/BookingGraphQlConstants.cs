namespace Cineverse.IntegrationTests.Shared.Constants.GraphQl;

public static class BookingGraphQlConstants
{
    public const string GetBookingByIdQuery =
        """
        query getBookingById($id: String!) {
          bookingById(id: $id) {
            id
            userId
            screeningId
            seatIds
            totalPrice
            dateCreated
          }
        }
        """;
}