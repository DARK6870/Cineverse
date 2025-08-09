using Cineverse.Notifications.Common.Builders;
using Cineverse.Notifications.Common.Options;
using Cineverse.Notifications.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Options;

namespace Cineverse.Application.MediatR.Support.Commands;

public record CreateSupportTicketRequest(
    string FirstName,
    string LastName,
    string Email,
    string Subject,
    string Description
) : IRequest<bool>;

public class CreateSupportTicketRequestHandler(
    INotificationService notificationService,
    IOptions<EmailOptions> emailOptions
) : IRequestHandler<CreateSupportTicketRequest, bool>
{
    public async Task<bool> Handle(CreateSupportTicketRequest request, CancellationToken cancellationToken)
    {
        var message = new MessageBuilder
        {
            FullName = "Support Team",
            Title = "New Support Ticket",
            Message = $"A new support ticket has been submitted via the Help Center<br><br>" +
                      $"<b>Submitted by:</b> {request.FirstName} {request.LastName}<br>" +
                      $"<b>Response email:</b> {request.Email}<br><br>" +
                      $"<b>Subject:</b> {request.Subject}<br>" +
                      $"<b>Description:</b><br>{request.Description}<br><br>" +
                      "<small>Please review this request and respond to the user as soon as possible<small>"
        };

        await notificationService.SendEmailNotification(emailOptions.Value.SupportEmail, "Support Ticket", message);
        return true;
    }
}