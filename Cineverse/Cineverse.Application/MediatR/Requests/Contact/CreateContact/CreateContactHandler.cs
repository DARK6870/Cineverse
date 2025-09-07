using Cineverse.Application.Common.Extensions;
using Cineverse.Application.Common.Models;
using Cineverse.Application.Common.Models.Enums;
using Cineverse.Notifications.Common.Builders;
using Cineverse.Notifications.Common.Options;
using Cineverse.Notifications.Services.Notification;
using MediatR;
using Microsoft.Extensions.Options;

namespace Cineverse.Application.MediatR.Requests.Contact.CreateContact;

public class CreateContactHandler(
    //INotificationService notificationService,
    //IOptions<EmailOptions> emailOptions
) : IRequestHandler<CreateContactRequest, bool>
{
    public async Task<bool> Handle(CreateContactRequest request, CancellationToken cancellationToken)
    {
        /*var message = new MessageBuilder
        {
            FullName = $"{request.Department.GetDescription()} Team",
            Title = "New Contact Request",
            Message = $"A new message has been received through the contact form<br><br>" +
                      $"<b>From:</b> {request.FirstName} {request.LastName}<br>" +
                      $"<b>Email:</b> {request.Email}<br><br>" +
                      $"<b>Subject:</b> {request.Subject}<br>" +
                      $"<b>Message:</b><br>{request.Description}<br><br>" +
                      "<small>Please review this request and respond to the user as soon as possible<small>"
        };

        var emailTo = request.Department switch
        {
            Department.Marketing => emailOptions.Value.MarketingDepartmentEmail,
            Department.Collaboration => emailOptions.Value.CollaborationDepartmentEmail,
            Department.CustomerService => emailOptions.Value.SupportDepartmentEmail,
            _ => emailOptions.Value.ItSupportDepartmentEmail
        };

        await notificationService.SendEmailNotification(emailTo, "Contact Request", message);*/
        return await Task.FromResult(true);
    }
}