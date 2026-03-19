namespace AutomationTests.Core.Cineverse.Constants;

public static class BookingConstants
{
    public const string GetBookingsQuery =
        """
        query getBookings {
          bookings(take: 250) {
            items {
              id
              userId
              screeningId
              seatIds
              totalPrice
              dateCreated
            }
            totalCount
          }
        }
        """;
    
    public const string GetUserBookingsQuery =
        """
        query getUserBookings {
          userBookings {
              id
              userId
              screeningId
              seatIds
              totalPrice
              dateCreated
          }
        }
        """;
    
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

    public const string GetBookedSeatsQuery =
        """
        query getBookedSeats($screeningId: String!){
          bookedSeats (screeningId: $screeningId)
        }
        """;

    public const string CreateBookingMutation =
        """
        mutation createBooking($request: CreateBookingRequestInput!){
          createBooking(request: $request)
        }
        """;
}