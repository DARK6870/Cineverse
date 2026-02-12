using ApiGateway.Options;
using Microsoft.AspNetCore.RateLimiting;

namespace ApiGateway.Extensions;

public static class RateLimitDependencyInjectionExtensions
{
    public static IServiceCollection AddRateLimit(this IServiceCollection services, IConfiguration configuration)
    {
        var rateLimitOptions = configuration.GetSection(nameof(RateLimitOptions)).Get<RateLimitOptions>()
            ?? throw new ArgumentException(nameof(RateLimitOptions));

        services.AddRateLimiter(options =>
        {
            options.AddFixedWindowLimiter(nameof(RateLimitOptions.Fixed), opt =>
            {
                opt.Window = rateLimitOptions.Fixed.Window;
                opt.PermitLimit = rateLimitOptions.Fixed.PermitLimit;
                opt.QueueLimit = rateLimitOptions.Fixed.QueueLimit;
            });

            options.AddSlidingWindowLimiter(nameof(RateLimitOptions.ByIp), opt =>
            {
                opt.Window = rateLimitOptions.ByIp.Window;
                opt.PermitLimit = rateLimitOptions.ByIp.PermitLimit;
                opt.QueueLimit = rateLimitOptions.ByIp.QueueLimit;
                opt.SegmentsPerWindow = rateLimitOptions.ByIp.SegmentsPerWindow;
            });
        });
        
        return services;
    }
}