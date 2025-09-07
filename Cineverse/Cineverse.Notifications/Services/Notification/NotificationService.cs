using System.Net;
using System.Net.Mail;
using Cineverse.Notifications.Common.Builders;
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
    private readonly string _templateFolderPath = GetTemplatesRootPath();

    public async Task SendEmailNotification(
        string emailTo,
        string subject,
        MessageBuilder message
    )
    {
        var body = await GetEmailTemplateAsync(message.Build());

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
            EnableSsl = _smtpOptions.EnableSsl
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

    private async Task<string> GetEmailTemplateAsync(string content)
    {
        var filePath = Path.Combine(_templateFolderPath  + ".html");
        
        var htmlTemplate = await File.ReadAllTextAsync(filePath);
        var finalHtml = htmlTemplate.Replace("{Content}", content);

        return finalHtml;
    }

    private static string GetTemplatesRootPath()
    {
        // TODO: change root path
        var rootPath = AppContext.BaseDirectory;
        var templatePath = Path.Combine(rootPath, "NotificationTemplates");

        if (!Directory.Exists(templatePath))
            return "";//throw new DirectoryNotFoundException($"Templates folder not found at path: {templatePath}");

        return templatePath;
    }
}