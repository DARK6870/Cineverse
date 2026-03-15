namespace AutomationTests.Models.Cineverse.Requests.Booking;

public record CreateBookingRequest(
    string ScreeningId,
    string[] SeatsIds
);