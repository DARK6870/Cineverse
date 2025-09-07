using Cineverse.Notifications.Common.Builders;

namespace Cineverse.Notifications.Services.Notification.Extensions;

public static class VerificationCodeExtension
{
    public static async Task SendVerificationEmailAsync(
        this INotificationService notificationService,
        string emailTo,
        string fullName,
        int verificationCode,
        string actionUrl
    )
    {
        var notification = new MessageBuilder()
            .AppendTitle("Email Confirmation Code")
            .AppendGreeting(fullName)
            .AppendParagraph("Please complete your account setup to explore our website without restrictions")
            .AppendLineBreak()
            .AppendLineBreak()
            .AppendMessage("Your verification code is ")
            .AppendBold(verificationCode.ToString())
            .AppendLineBreak()
            .AppendSmall("The code will be valid for 2 minutes")
            .AppendLineBreak()
            .AppendLineBreak()
            .AppendAction(actionUrl, "to confirm your email");
        
        await notificationService.SendEmailNotification(emailTo, "Email Confirmation Code", notification);
    }
}