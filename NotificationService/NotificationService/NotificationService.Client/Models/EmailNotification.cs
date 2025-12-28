namespace NotificationService.Client.Models;

public class EmailNotification
{
    public required string EmailTo { get; init; }
 
    public required string Subject { get; init; }
    
    public required string Content { get; init; }
}