using Cineverse.Notifications.Common.Builders;

namespace Cineverse.Notifications.Services.Notification.Extensions;

public static class PasswordChangedExtension
{
    public static async Task SendPasswordChangedEmailAsync(
        this INotificationService notificationService,
        string emailTo,
        string fullName,
        string actionUrl
    )
    {
        var notification = new MessageBuilder()
            .AppendTitle("Your Password Has Been Changed")
            .AppendGreeting(fullName)
            .AppendParagraphStart()
            .AppendText("Your password has been changed. You have been logged out from all devices.")
            .AppendLineBreak()
            .AppendLineBreak()
            .AppendSmall("If this wasn’t you, please contact support for future assistance.")
            .AppendLineBreak()
            .AppendParagraphEnd()
            .AppendAction(actionUrl, "to open your profile");
            ;
        
        await notificationService.SendEmailNotificationAsync(emailTo, "Password Changed", notification);
    }
}