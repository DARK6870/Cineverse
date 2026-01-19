using System.Net.Mail;

namespace NotificationService.Application.Services.Notification;

public interface INotificationService
{
    /// <summary>
    /// Send email notification
    /// </summary>
    /// <param name="emailTo"></param>
    /// <param name="subject"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    Task SendEmailNotificationAsync(string emailTo, string subject, string message);
}