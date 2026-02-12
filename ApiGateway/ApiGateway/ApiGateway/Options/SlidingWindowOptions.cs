namespace ApiGateway.Options;

public class SlidingWindowOptions
{
    public TimeSpan Window { get; set; } = TimeSpan.FromMinutes(1);
    public int PermitLimit { get; set; } = 100;
    public int SegmentsPerWindow { get; set; } = 6;
    public int QueueLimit { get; set; } = 10;
}