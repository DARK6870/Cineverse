using Cineverse.Notifications.Common.Builders;

namespace Cineverse.Notifications.Services.Notification;

public interface INotificationService
{
    Task SendEmailNotificationAsync(string emailTo, string subject, MessageBuilder message);
}