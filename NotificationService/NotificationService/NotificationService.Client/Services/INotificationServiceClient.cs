using NotificationService.Client.Builders;

namespace NotificationService.Client.Services;

public interface INotificationServiceClient
{
    /// <summary>
    /// Send email notification via NotificationService
    /// </summary>
    /// <param name="emailTo">email to</param>
    /// <param name="subject">message subject</param>
    /// <param name="messageBuilder">message builder</param>
    /// <returns></returns>
    Task SendEmailNotificationAsync(string emailTo, string subject, MessageBuilder messageBuilder);
}