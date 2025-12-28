using Cineverse.Application.Common.Extensions;
using Cineverse.Application.Notification.Extensions;
using MediatR;
using NotificationService.Client.Services;

namespace Cineverse.Application.MediatR.Requests.Contact.CreateContact;

public class CreateContactHandler(
    INotificationServiceClient notificationServiceClient
) : IRequestHandler<CreateContactRequest, bool>
{
    public async Task<bool> Handle(CreateContactRequest request, CancellationToken cancellationToken)
    {
        /*var emailTo = request.Department switch
        {
            Department.Marketing => emailOptions.Value.MarketingDepartmentEmail,
            Department.Collaboration => emailOptions.Value.CollaborationDepartmentEmail,
            Department.CustomerService => emailOptions.Value.SupportDepartmentEmail,
            _ => emailOptions.Value.ItSupportDepartmentEmail
        };*/

        await notificationServiceClient.SendContactRequestEmailAsync(
            "emailTo",
            request.Department.GetDescription(),
            request.FirstName + " " + request.LastName,
            request.Email,
            request.Subject,
            request.Description
        );

        return await Task.FromResult(true);
    }
}