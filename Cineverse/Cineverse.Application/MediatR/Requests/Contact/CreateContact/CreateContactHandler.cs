using Cineverse.Application.Common.Extensions;
using Cineverse.Application.Common.Models.Enums;
using Cineverse.Notifications.Common.Options;
using Cineverse.Notifications.Services.Notification;
using Cineverse.Notifications.Services.Notification.Extensions;
using MediatR;
using Microsoft.Extensions.Options;

namespace Cineverse.Application.MediatR.Requests.Contact.CreateContact;

public class CreateContactHandler(
    INotificationService notificationService,
    IOptions<EmailOptions> emailOptions
) : IRequestHandler<CreateContactRequest, bool>
{
    public async Task<bool> Handle(CreateContactRequest request, CancellationToken cancellationToken)
    {
        var emailTo = request.Department switch
        {
            Department.Marketing => emailOptions.Value.MarketingDepartmentEmail,
            Department.Collaboration => emailOptions.Value.CollaborationDepartmentEmail,
            Department.CustomerService => emailOptions.Value.SupportDepartmentEmail,
            _ => emailOptions.Value.ItSupportDepartmentEmail
        };

        await notificationService.SendContactRequestEmailAsync(
            emailTo,
            request.Department.GetDescription(),
            request.FirstName + " " + request.LastName,
            request.Email,
            request.Subject,
            request.Description
        );

        return await Task.FromResult(true);
    }
}