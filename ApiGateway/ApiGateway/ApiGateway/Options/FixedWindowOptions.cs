namespace ApiGateway.Options;

public class FixedWindowOptions
{
    public TimeSpan Window { get; set; } = TimeSpan.FromMinutes(1);
    public int PermitLimit { get; set; } = 100;
    public int QueueLimit { get; set; } = 10;
}