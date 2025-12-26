using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NotificationService.Application.Helpers;
using NotificationService.Application.Models.Options;

namespace NotificationService.Application.Services.Notification;

public class NotificationService(
    IOptions<SmtpOptions> smtpOptions,
    ILogger<NotificationService> logger
) : INotificationService
{
    private readonly SmtpOptions _smtpOptions = smtpOptions.Value;
    // TODO: add retry option
    public async Task SendEmailNotificationAsync(
        string emailTo,
        string subject,
        string message
    )
    {
        // build HTML message body
        var body = NotificationTemplateHelper.BuildTemplate(message);

        // create SMTP client
        using var smtpClient = new SmtpClient(_smtpOptions.SmtpServer)
        {
            Port = _smtpOptions.SmtpPort,
            Credentials = new NetworkCredential(_smtpOptions.SmtpEmail, _smtpOptions.SmtpPassword),
            EnableSsl = _smtpOptions.EnableSsl
        };

        // build mail message
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
            // send email
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