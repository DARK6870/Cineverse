using NotificationService.Client.Builders;
using NotificationService.Client.Services;

namespace IdentityService.Application.Notifications.NotificationClientExtensions;

public static class RestorePasswordExtension
{
    public static async Task SendRestorePasswordEmailAsync(
        this INotificationServiceClient notificationServiceClient,
        string emailTo,
        string fullName,
        string actionUrl
    )
    {
        var notification = new MessageBuilder()
            .AppendTitle("Restore Password")
            .AppendGreeting(fullName)
            .AppendParagraphStart()
            .AppendText("To restore your password you need to follow the link below and set a new password.")
            .AppendLineBreak()
            .AppendText("If this wasn’t you, please contact support for future assistance.")
            .AppendLineBreak()
            .AppendLineBreak()
            .AppendSmall("The link will be valid for 10 minutes")
            .AppendLineBreak()
            .AppendParagraphEnd()
            .AppendAction(actionUrl, "to restore your password")
            ;
        
        await notificationServiceClient.SendEmailNotificationAsync(emailTo, "Restore Password", notification);
    }
}