using Cineverse.Notifications.Common.Builders;

namespace Cineverse.Notifications.Services.Interfaces;

public interface INotificationService
{
    Task SendEmailNotification(string emailTo, string subject, MessageBuilder message);
}