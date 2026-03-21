using AutomationTests.Models.Cineverse.Requests.Booking;

namespace AutomationTests.Core.Cineverse.DataGenerators;

public static class BookingDataGenerator
{
    public static CreateBookingRequest ValidCreateBookingRequest(string screeningId, string[]? seatsIds = null)
    {
        return new CreateBookingRequest(
            screeningId,
            seatsIds ?? ["1-1"]
        );
    }
    
    public static CreateBookingRequest InvalidCreateBookingRequest()
    {
        return new CreateBookingRequest(
            "",
            [""]
        );
    }
}