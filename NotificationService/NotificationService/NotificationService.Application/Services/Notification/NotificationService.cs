using System.Net;
using System.Net.Mail;
using Hangfire;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NotificationService.Application.Helpers;
using NotificationService.Application.Models.Options;

namespace NotificationService.Application.Services.Notification;

public class NotificationService(
    IOptions<SmtpOptions> smtpOptions,
    IOptions<NotificationOptions> notificationOptions,
    ILogger<NotificationService> logger
) : INotificationService
{
    private readonly SmtpOptions _smtpOptions = smtpOptions.Value;
    private readonly NotificationOptions _notificationOptions = notificationOptions.Value;
    
    public async Task SendEmailNotificationAsync(
        string emailTo,
        string subject,
        string message
    )
    {
        // build HTML message body
        var body = NotificationTemplateHelper.BuildTemplate(message);

        // send email with retry
        await SendEmailWithRetryAsync(emailTo, subject, body);
    }

    [AutomaticRetry(Attempts = 0)]
    public async Task SendEmailWithRetryAsync(
        string emailTo,
        string subject,
        string body,
        int attempt = 1
    )
    {
        logger.LogInformation(
            "Sending email with subject '{subject}' to '{emailTo}'. Attempt: {attempt}",
            subject,
            emailTo,
            attempt
        );
        
        try
        {
            // send email
            await SendEmailInternalAsync(emailTo, subject, body);

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
                "Failed to send email to '{emailTo}', subject '{subject}'. Attempt: {attempt}",
                emailTo,
                subject,
                attempt
            );
            
            if (attempt >= _notificationOptions.RetryCount)
                throw;
            
            // schedule retry with exponential backoff
            var delay = CalculateRetryDelay(attempt);
            
            logger.LogInformation(
                "Scheduling retry {nextAttempt}/{maxAttempts} for email to '{emailTo}' in {delay} seconds",
                attempt + 1,
                _notificationOptions.RetryCount,
                emailTo,
                delay.TotalSeconds
            );

            BackgroundJob.Schedule(
                () => SendEmailWithRetryAsync(emailTo, subject, body, attempt + 1),
                delay
            );
        }
    }
    
    /// <summary>
    /// Internal method that actually sends the email
    /// </summary>
    private async Task SendEmailInternalAsync(string emailTo, string subject, string body)
    {
        using var mailMessage = new MailMessage();
        mailMessage.From = new MailAddress(_smtpOptions.SmtpEmail);
        mailMessage.Subject = subject;
        mailMessage.Body = body;
        mailMessage.IsBodyHtml = true;

        mailMessage.To.Add(emailTo);

        using var smtpClient = new SmtpClient(_smtpOptions.SmtpServer);
        smtpClient.Port = _smtpOptions.SmtpPort;
        smtpClient.Credentials = new NetworkCredential(_smtpOptions.SmtpEmail, _smtpOptions.SmtpPassword);
        smtpClient.EnableSsl = _smtpOptions.EnableSsl;
        smtpClient.Timeout = 30000;

        await smtpClient.SendMailAsync(mailMessage);
    }
    
    /// <summary>
    /// Calculate exponential backoff delay
    /// </summary>
    private TimeSpan CalculateRetryDelay(int attempt)
    {
        // Exponential backoff: 2^attempt * base delay
        // Attempt 1: 1x delay, Attempt 2: 2x delay, Attempt 3: 4x delay
        var multiplier = Math.Pow(2, attempt - 1);
        var delay = TimeSpan.FromSeconds(_notificationOptions.RetryDelayInSeconds * multiplier);
        
        // Cap at max delay (e.g., 5 minutes)
        var maxDelay = TimeSpan.FromMinutes(5);
        return delay > maxDelay ? maxDelay : delay;
    }
}