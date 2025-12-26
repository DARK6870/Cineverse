namespace NotificationService.Application.Services.Notification;

public interface INotificationService
{
    Task SendEmailNotificationAsync(string emailTo, string subject, string message);
}