using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.HealthCheck.Options;

public class AddHealthCheckOptions
{
    public bool IncludeMongoDb { get; set; }
    
    public bool IncludeKafka { get; set; }
    
    public List<Func<IHealthChecksBuilder, IHealthChecksBuilder>> CustomChecks { get; } = [];
}
