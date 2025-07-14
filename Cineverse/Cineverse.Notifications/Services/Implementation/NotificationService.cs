using System.Net;
using System.Net.Mail;
using Cineverse.Notifications.Common.Builders;
using Cineverse.Notifications.Common.Models.Enums;
using Cineverse.Notifications.Common.Options;
using Cineverse.Notifications.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Cineverse.Notifications.Services.Implementation;

public class NotificationService(
    IOptions<SmtpOptions> smtpOptions,
    ILogger<NotificationService> logger
) : INotificationService
{
    private readonly SmtpOptions _smtpOptions = smtpOptions.Value;
    private readonly string _templateFolderPath = GetTemplatesRootPath();

    public async Task SendEmailNotification(
        string emailTo,
        string subject,
        MessageBuilder message
    )
    {
        var body = await GetEmailTemplateAsync(NotificationType.Notification, message.FormattedMessage);

        await SendEmailAsync(emailTo, subject, body);
    }

    private async Task SendEmailAsync(
        string emailTo,
        string subject,
        string body
    )
    {
        using var smtpClient = new SmtpClient(_smtpOptions.SmtpServer)
        {
            Port = _smtpOptions.SmtpPort,
            Credentials = new NetworkCredential(_smtpOptions.SmtpEmail, _smtpOptions.SmtpPassword),
            EnableSsl = true
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_smtpOptions.SmtpEmail),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };
        mailMessage.To.Add(emailTo);

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


    private async Task<string> GetEmailTemplateAsync(
        NotificationType messageType,
        string content
    )
    {
        var filePath = Path.Combine(_templateFolderPath, messageType + ".html");
        
        var htmlTemplate = await File.ReadAllTextAsync(filePath);
        var finalHtml = htmlTemplate.Replace("{Content}", content);

        return finalHtml;
    }

    private static string GetTemplatesRootPath()
    {
        var rootPath = AppDomain.CurrentDomain.BaseDirectory;
        
        var directory = new DirectoryInfo(rootPath);
        while (directory != null && directory.Name != AppDomain.CurrentDomain.FriendlyName)
            directory = directory.Parent;
        
        return Path.Combine(directory!.FullName, "..", typeof(Configuration).Assembly.GetName().Name!, "Templates");
    }
}