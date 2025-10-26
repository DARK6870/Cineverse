using System.Net;
using Cineverse.Domain.Common.Exceptions;
using Cineverse.Identity.Services.UserContext;
using Cineverse.Mongo.Repositories.RefreshToken;
using Cineverse.Mongo.Repositories.User;
using Cineverse.Notifications.Common.Options;
using Cineverse.Notifications.Services.Notification;
using Cineverse.Notifications.Services.Notification.Extensions;
using MediatR;
using Microsoft.Extensions.Options;

namespace Cineverse.Application.MediatR.Requests.Authentication.ChangePassword;

public class ChangePasswordHandler(
    IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository,
    INotificationService notificationService,
    IUserContext userContext,
    IOptions<NotificationLinksOptions> notificationLinksOptions
) : IRequestHandler<ChangePasswordRequest, bool>
{
    public async Task<bool> Handle(ChangePasswordRequest request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserByCredentialsAsync(userContext.Email, request.Password)
                   ?? throw new ApiRequestException("Invalid password", HttpStatusCode.BadRequest);

        var result = await userRepository.UpdateUserPasswordAsync(user.Id, request.NewPassword);
        if (result)
        {
            var actionUrl = notificationLinksOptions.Value.BuildProfileUrl();
            await notificationService.SendPasswordChangedEmailAsync(
                userContext.Email,
                userContext.UserName,
                actionUrl
            );
            
            await refreshTokenRepository.DeleteManyAsync(x => x.UserId == user.Id, cancellationToken);
        }

        return result;
    }
}