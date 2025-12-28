using NotificationService.Client.Builders;
using NotificationService.Client.Services;

namespace Cineverse.Application.Notification.Extensions;

public static class ContactRequestExtension
{
    public static async Task SendContactRequestEmailAsync(
        this INotificationServiceClient notificationServiceClient,
        string emailTo,
        string department,
        string fullName,
        string responseEmail,
        string subject,
        string description
    )
    {
        var notification = new MessageBuilder()
            .AppendTitle("Contact Request")
            .AppendGreeting($"{department} Team")
            .AppendParagraphStart()
            .AppendText("A new message has been received through the contact form")
            .AppendLineBreak()
            .AppendLineBreak()
            .AppendBold("From: ").AppendText(fullName)
            .AppendLineBreak()
            .AppendBold("Email: ").AppendText(responseEmail)
            .AppendLineBreak()
            .AppendBold("Subject: ").AppendText(subject)
            .AppendLineBreak()
            .AppendBold("Message: ").AppendLineBreak().AppendText(description)
            .AppendLineBreak()
            .AppendLineBreak()
            .AppendLineBreak()
            .AppendSmall("Please review this request and respond to the user as soon as possible")
            .AppendLineBreak()
            .AppendParagraphEnd()
            ;
        
        await notificationServiceClient.SendEmailNotificationAsync(emailTo, "Contact Request", notification);
    }
}