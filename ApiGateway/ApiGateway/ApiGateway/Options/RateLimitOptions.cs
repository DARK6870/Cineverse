namespace ApiGateway.Options;

public class RateLimitOptions
{
    public required FixedWindowOptions Fixed { get; set; }
    public required SlidingWindowOptions ByIp { get; set; }
}