using Cineverse.Mongo.Repositories.Interfaces;
using Cineverse.Mongo.Schemas.Entities;
using MediatR;

namespace Cineverse.Application.MediatR.Users.Queries;

public class GetUsersRequest : IRequest<IQueryable<UserEntity>>;

public class GetUsersRequestHandler(
    IUserRepository userRepository
) : IRequestHandler<GetUsersRequest, IQueryable<UserEntity>>
{
    public Task<IQueryable<UserEntity>> Handle(GetUsersRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(userRepository.AsQueryable());
    }
}