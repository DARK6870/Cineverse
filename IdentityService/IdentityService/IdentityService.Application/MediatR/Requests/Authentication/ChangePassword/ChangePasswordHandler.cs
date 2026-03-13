using IdentityService.Application.Notifications.NotificationClientExtensions;
using IdentityService.Mongo.Repositories.RefreshToken;
using IdentityService.Mongo.Repositories.User;
using Infrastructure.Common.Exceptions;
using Infrastructure.Context.UserContext;
using MediatR;
using Microsoft.Extensions.Options;
using NotificationService.Client.Models.Options;
using NotificationService.Client.Services;

namespace IdentityService.Application.MediatR.Requests.Authentication.ChangePassword;

public class ChangePasswordHandler(
    IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository,
    INotificationServiceClient notificationServiceClient,
    IUserContext userContext,
    IOptions<NotificationLinksOptions> notificationLinksOptions
) : IRequestHandler<ChangePasswordRequest, bool>
{
    public async Task<bool> Handle(ChangePasswordRequest request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserByCredentialsAsync(userContext.Email, request.Password)
                   ?? throw new ValidationException("Invalid password");

        var result = await userRepository.UpdateUserPasswordAsync(user.Id, request.NewPassword);
        if (result)
        {
            var actionUrl = notificationLinksOptions.Value.BuildProfileUrl();
            await notificationServiceClient.SendPasswordChangedEmailAsync(
                userContext.Email,
                userContext.UserName,
                actionUrl
            );
            
            await refreshTokenRepository.DeleteManyAsync(x => x.UserId == user.Id, cancellationToken);
        }

        return result;
    }
}