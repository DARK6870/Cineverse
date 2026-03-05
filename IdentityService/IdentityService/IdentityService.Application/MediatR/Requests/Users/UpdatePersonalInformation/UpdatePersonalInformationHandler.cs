using System.Net;
using IdentityService.Mongo.Repositories.User;
using Infrastructure.Context.UserContext;
using Infrastructure.WebApi.Exceptions;
using MediatR;

namespace IdentityService.Application.MediatR.Requests.Users.UpdatePersonalInformation;

public class UpdatePersonalInformationHandler(
    IUserRepository userRepository,
    IUserContext userContext
) : IRequestHandler<UpdatePersonalInformationRequest, bool>
{
    public async Task<bool> Handle(UpdatePersonalInformationRequest request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FindByIdOrThrowAsync(userContext.UserId, cancellationToken);

        return await userRepository.UpdateUserPersonalInformationAsync(
            user.Id,
            request.FirstName,
            request.LastName
        );
    }
}