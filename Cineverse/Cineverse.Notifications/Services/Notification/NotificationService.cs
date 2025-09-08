using System.Net;
using System.Net.Mail;
using Cineverse.Notifications.Common.Builders;
using Cineverse.Notifications.Common.Helpers;
using Cineverse.Notifications.Common.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Cineverse.Notifications.Services.Notification;

public class NotificationService(
    IOptions<SmtpOptions> smtpOptions,
    ILogger<NotificationService> logger
) : INotificationService
{
    private readonly SmtpOptions _smtpOptions = smtpOptions.Value;

    public async Task SendEmailNotificationAsync(
        string emailTo,
        string subject,
        MessageBuilder message
    )
    {
        var body = NotificationTemplateHelper.BuildTemplate(message);

        using var smtpClient = new SmtpClient(_smtpOptions.SmtpServer)
        {
            Port = _smtpOptions.SmtpPort,
            Credentials = new NetworkCredential(_smtpOptions.SmtpEmail, _smtpOptions.SmtpPassword),
            EnableSsl = _smtpOptions.EnableSsl
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_smtpOptions.SmtpEmail),
            To = { emailTo },
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        try
        {
            await smtpClient.SendMailAsync(mailMessage);

            logger.LogInformation(
                "Email with subject '{subject}' was successfully sent to '{emailTo}'.",
                subject,
                emailTo
            );
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Failed to send email to '{emailTo}', subject '{subject}'",
                emailTo,
                subject
            );
            
            throw;
        }
    }
}