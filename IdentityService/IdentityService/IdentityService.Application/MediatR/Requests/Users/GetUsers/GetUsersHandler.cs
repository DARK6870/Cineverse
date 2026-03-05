using IdentityService.Mongo.Repositories.User;
using IdentityService.Mongo.Schemas.Entities;
using MediatR;

namespace IdentityService.Application.MediatR.Requests.Users.GetUsers;

public class GetUsersHandler(
    IUserRepository userRepository
) : IRequestHandler<GetUsersRequest, IQueryable<UserEntity>>
{
    public Task<IQueryable<UserEntity>> Handle(GetUsersRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.SearchTerm))
            return Task.FromResult(userRepository.AsQueryable());

        return Task.FromResult(userRepository.SearchByText(
                request.SearchTerm,
                x => x.PasswordHash
            )
        );
    }
}