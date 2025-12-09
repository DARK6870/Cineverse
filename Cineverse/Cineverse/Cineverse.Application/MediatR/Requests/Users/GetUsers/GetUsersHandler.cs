using Cineverse.Mongo.Repositories.User;
using Cineverse.Mongo.Schemas.Entities;
using MediatR;

namespace Cineverse.Application.MediatR.Requests.Users.GetUsers;

public class GetUsersHandler(
    IUserRepository userRepository
) : IRequestHandler<GetUsersRequest, IQueryable<UserEntity>>
{
    public Task<IQueryable<UserEntity>> Handle(GetUsersRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(userRepository.AsQueryable());
    }
}