namespace Cineverse.Notifications.Common.Options;

public class EmailOptions
{
    public required string SupportDepartmentEmail { get; init; }
    
    public required string MarketingDepartmentEmail { get; init; }
    
    public required string CollaborationDepartmentEmail { get; init; }
    
    public required string ItSupportDepartmentEmail { get; init; }
}