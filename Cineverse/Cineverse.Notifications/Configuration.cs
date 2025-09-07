using Cineverse.Notifications.Common.Options;
using Cineverse.Notifications.Services.Notification;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cineverse.Notifications;

// TODO: refactor all email notifications
public static class Configuration
{
    public static IServiceCollection AddNotificationService(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.Configure<SmtpOptions>(configuration.GetSection(nameof(SmtpOptions)));
        services.Configure<EmailOptions>(configuration.GetSection(nameof(EmailOptions)));
        services.Configure<NotificationLinksOptions>(configuration.GetSection(nameof(NotificationLinksOptions)));
        
        services.AddSingleton<INotificationService, NotificationService>();
        
        return services;
    }
}