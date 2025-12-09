namespace Cineverse.Notifications.Common.Options;

public class SmtpOptions
{
    public required string SmtpServer { get; init; }
    public required int SmtpPort { get; init; }
    public required string SmtpEmail { get; init; }
    public required string SmtpPassword { get; init; }
    public required bool EnableSsl { get; init; }
}