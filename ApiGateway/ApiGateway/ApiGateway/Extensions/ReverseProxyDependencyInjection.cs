using Yarp.ReverseProxy.Transforms;

namespace ApiGateway.Extensions;

public static class ReverseProxyDependencyInjection
{
    public static IServiceCollection AddApiGateway(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddReverseProxy()
            .LoadFromConfig(configuration.GetSection("ReverseProxy"))
            .AddTransforms(builderContext =>
            {
                builderContext.AddRequestTransform(async transformContext =>
                {
                    transformContext.ProxyRequest.Headers.Add(
                        "X-Correlation-ID",
                        Guid.CreateVersion7().ToString()
                    );
                });
            });
        
        return services;
    }
}