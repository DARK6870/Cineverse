/*using System.Net;
using Cineverse.Domain.Common.Exceptions;
using Cineverse.Identity.Services.UserContext;
using Cineverse.Mongo.Repositories.User;
using MediatR;

namespace Cineverse.Application.MediatR.Requests.Users.UpdatePersonalInformation;

public class UpdatePersonalInformationHandler(
    IUserRepository userRepository,
    IUserContext userContext
) : IRequestHandler<UpdatePersonalInformationRequest, bool>
{
    public async Task<bool> Handle(UpdatePersonalInformationRequest request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FindByIdAsync(userContext.UserId, cancellationToken)
            ?? throw new ApiRequestException("User not found", HttpStatusCode.NotFound);

        return await userRepository.UpdateUserPersonalInformationAsync(
            user.Id,
            request.FirstName,
            request.LastName
        );
    }
}*/