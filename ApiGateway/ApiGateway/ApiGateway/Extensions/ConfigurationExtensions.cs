namespace ApiGateway.Extensions;

public static class ConfigurationExtensions
{
    public static IConfigurationBuilder AddGatewayConfiguration(
        this IConfigurationBuilder builder,
        string environment)
    {
        var serviceDefinitionsPath = GetServiceDefinitionsRootPath();
        
        if (!Directory.Exists(serviceDefinitionsPath))
            throw new DirectoryNotFoundException($"Services definitions directory not found: {serviceDefinitionsPath}");

        var serviceDirs = Directory.GetDirectories(serviceDefinitionsPath);

        foreach (var serviceDir in serviceDirs)
        {
            var serviceName = Path.GetFileName(serviceDir);
            
            var routesFile = Path.Combine(serviceDir, "routes.json");
            if (File.Exists(routesFile))
                builder.AddJsonFile(routesFile, optional: false, reloadOnChange: true);
            
            var clusterFile = Path.Combine(serviceDir, "cluster.json");
            if (File.Exists(clusterFile))
                builder.AddJsonFile(clusterFile, optional: false, reloadOnChange: true);
            
            var clusterEnvFile = Path.Combine(serviceDir, $"cluster.{environment}.json");
            if (File.Exists(clusterEnvFile))
                builder.AddJsonFile(clusterEnvFile, optional: true, reloadOnChange: true);
        }

        return builder;
    }

    public static void ValidateGatewayConfiguration(this IConfiguration configuration)
    {
        var proxyConfig = configuration.GetSection("ReverseProxy");
        
        if (!proxyConfig.Exists())
            throw new InvalidOperationException("ReverseProxy configuration section not found");

        var routes = proxyConfig.GetSection("Routes").GetChildren();
        var clusters = proxyConfig.GetSection("Clusters").GetChildren();

        var clusterIds = clusters.Select(c => c.Key).ToHashSet();

        foreach (var route in routes)
        {
            var clusterId = route["ClusterId"];
            if (string.IsNullOrEmpty(clusterId))
                throw new InvalidOperationException($"Route '{route.Key}' is missing ClusterId");

            if (!clusterIds.Contains(clusterId))
                throw new InvalidOperationException($"Route '{route.Key}' references unknown cluster '{clusterId}'");
        }
    }

    public static void LogGatewayConfiguration(
        this IConfiguration configuration,
        ILogger logger)
    {
        var routes = configuration.GetSection("ReverseProxy:Routes").GetChildren().ToArray();
        var clusters = configuration.GetSection("ReverseProxy:Clusters").GetChildren();

        logger.LogInformation("=== Gateway Configuration ===");
        logger.LogInformation("Routes loaded: {Count}", routes.Count());
        logger.LogInformation("Clusters loaded: {Count}", clusters.Count());

        foreach (var route in routes)
        {
            var path = route["Match:Path"];
            var clusterId = route["ClusterId"];
            
            logger.LogInformation("Route: {RouteKey} -> {Path} => {ClusterId}", route.Key, path, clusterId);
        }

        logger.LogInformation("=============================");
    }
    
    private static string GetServiceDefinitionsRootPath()
    {
        const string targetFolderName = "ServiceDefinitions";
        
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory != null && !directory.Name.Equals(targetFolderName, StringComparison.CurrentCultureIgnoreCase))
            directory = directory.Parent;

        if (directory == null)
            throw new DirectoryNotFoundException("Could not find the target folder");

        return directory.FullName;
    }
}