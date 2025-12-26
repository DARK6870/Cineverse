namespace NotificationService.Client.Models;

public class EmailNotification
{
    public required string EmailTo { get; set; }
 
    public required string Subject { get; set; }
    
    public required string Content { get; set; }
}