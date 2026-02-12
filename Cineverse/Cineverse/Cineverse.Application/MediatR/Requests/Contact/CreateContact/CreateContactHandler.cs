using Cineverse.Application.Common.Extensions;
using Cineverse.Application.Common.Models.Enums;
using Cineverse.Application.Common.Models.Options;
using Cineverse.Application.Notification.Extensions;
using MediatR;
using Microsoft.Extensions.Options;
using NotificationService.Client.Services;

namespace Cineverse.Application.MediatR.Requests.Contact.CreateContact;

public class CreateContactHandler(
    INotificationServiceClient notificationServiceClient,
    IOptions<DepartmentEmailOptions> departmentEmailOptions
) : IRequestHandler<CreateContactRequest, bool>
{
    public async Task<bool> Handle(CreateContactRequest request, CancellationToken cancellationToken)
    {
        var emailTo = request.Department switch
        {
            Department.Marketing => departmentEmailOptions.Value.MarketingDepartmentEmail,
            Department.Collaboration => departmentEmailOptions.Value.CollaborationDepartmentEmail,
            Department.CustomerService => departmentEmailOptions.Value.SupportDepartmentEmail,
            _ => departmentEmailOptions.Value.ItSupportDepartmentEmail
        };

        await notificationServiceClient.SendContactRequestEmailAsync(
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