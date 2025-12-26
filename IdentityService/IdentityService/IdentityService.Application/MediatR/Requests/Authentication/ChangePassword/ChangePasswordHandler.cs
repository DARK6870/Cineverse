using System.Net;
using IdentityService.Mongo.Repositories.RefreshToken;
using IdentityService.Mongo.Repositories.User;
using Infrastructure.Context.UserContext;
using Infrastructure.WebApi.Exceptions;
using MediatR;
namespace IdentityService.Application.MediatR.Requests.Authentication.ChangePassword;

public class ChangePasswordHandler(
    IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository,
    //INotificationService notificationService,
    IUserContext userContext
    //IOptions<NotificationLinksOptions> notificationLinksOptions
) : IRequestHandler<ChangePasswordRequest, bool>
{
    public async Task<bool> Handle(ChangePasswordRequest request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserByCredentialsAsync(userContext.Email, request.Password)
                   ?? throw new ApiRequestException("Invalid password", HttpStatusCode.BadRequest);

        var result = await userRepository.UpdateUserPasswordAsync(user.Id, request.NewPassword);
        if (result)
        {
            // TODO: Add notifications
            /*var actionUrl = notificationLinksOptions.Value.BuildProfileUrl();
            await notificationService.SendPasswordChangedEmailAsync(
                userContext.Email,
                userContext.UserName,
                actionUrl
            );*/
            
            await refreshTokenRepository.DeleteManyAsync(x => x.UserId == user.Id, cancellationToken);
        }

        return result;
    }
}