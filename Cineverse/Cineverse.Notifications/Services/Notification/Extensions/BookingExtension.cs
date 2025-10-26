using Cineverse.Notifications.Common.Builders;

namespace Cineverse.Notifications.Services.Notification.Extensions;

public static class BookingExtension
{
    public static async Task SendBookingEmailAsync(
        this INotificationService notificationService,
        string emailTo,
        string fullName,
        int numberOfTickets,
        int totalTicketsPrice,
        DateOnly screeningDate,
        TimeOnly screeningTime,
        string actionUrl
    )
    {
        var notification = new MessageBuilder()
            .AppendTitle("Your booking has been created")
            .AppendGreeting(fullName)
            .AppendParagraphStart()
            .AppendText("Your booking has been created")
            .AppendLineBreak()
            .AppendText($"Number of tickets: {numberOfTickets}")
            .AppendLineBreak()
            .AppendBold($"Total tickets price: {totalTicketsPrice}")
            .AppendLineBreak()
            .AppendLineBreak()
            .AppendText("We are waiting for you on ").AppendBold($"{screeningDate} | {screeningTime}")
            .AppendAction(actionUrl, "to view booking details")
            ;
        
        await notificationService.SendEmailNotificationAsync(emailTo, "Ticket Booking", notification);
    }
}