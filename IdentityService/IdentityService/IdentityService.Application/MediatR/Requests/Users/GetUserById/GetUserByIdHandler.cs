using IdentityService.Mongo.Repositories.User;
using IdentityService.Mongo.Schemas.Entities;
using MediatR;

namespace IdentityService.Application.MediatR.Requests.Users.GetUserById;

public class GetUserByIdHandler(
    IUserRepository userRepository
) : IRequestHandler<GetUserByIdRequest, UserEntity>
{
    public async Task<UserEntity> Handle(GetUserByIdRequest request, CancellationToken cancellationToken)
    {
        return await userRepository.FindByIdOrThrowAsync(request.Id, cancellationToken);
    }
}