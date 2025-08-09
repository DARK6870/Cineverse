using Cineverse.Notifications.Common.Options;
using Cineverse.Notifications.Services.Implementation;
using Cineverse.Notifications.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cineverse.Notifications;

public static class Configuration
{
    public static IServiceCollection AddNotificationService(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.Configure<SmtpOptions>(configuration.GetSection(nameof(SmtpOptions)));
        services.Configure<EmailOptions>(configuration.GetSection(nameof(EmailOptions)));
        
        services.AddSingleton<INotificationService, NotificationService>();
        
        return services;
    }
}