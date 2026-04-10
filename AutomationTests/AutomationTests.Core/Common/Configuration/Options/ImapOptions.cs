namespace AutomationTests.Core.Common.Configuration.Options;

public class ImapOptions
{
    public required string HostName { get; init; }
    public required int Port { get; init; }
    public required bool Ssl { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
}