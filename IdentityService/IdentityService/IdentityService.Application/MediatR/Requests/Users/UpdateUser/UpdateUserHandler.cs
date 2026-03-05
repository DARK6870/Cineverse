using IdentityService.Mongo.Repositories.User;
using MediatR;

namespace IdentityService.Application.MediatR.Requests.Users.UpdateUser;

public class UpdateUserHandler(
    IUserRepository userRepository
): IRequestHandler<UpdateUserRequest, bool>
{
    public async Task<bool> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FindByIdOrThrowAsync(request.Id, cancellationToken);
        user = user with { Role = request.Role, Status = request.Status };
        
        await userRepository.ReplaceOneAsync(user, cancellationToken);
        return true;
    }
}