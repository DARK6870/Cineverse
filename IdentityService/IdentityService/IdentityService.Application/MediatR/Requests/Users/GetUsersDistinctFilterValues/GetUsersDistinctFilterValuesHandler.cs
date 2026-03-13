using System.Linq.Expressions;
using IdentityService.Application.Common.Models.Filters;
using IdentityService.Mongo.Repositories.User;
using IdentityService.Mongo.Schemas.Entities;
using MediatR;

namespace IdentityService.Application.MediatR.Requests.Users.GetUsersDistinctFilterValues;

public class GetUsersDistinctFilterValuesHandler(
    IUserRepository userRepository
) : IRequestHandler<GetUsersDistinctFilterValuesRequest, IEnumerable<string>>
{
    public async Task<IEnumerable<string>> Handle(GetUsersDistinctFilterValuesRequest request, CancellationToken cancellationToken)
    {
        Expression<Func<UserEntity, object>> selector;

        switch (request.FilterField)
        {
            case UserFilterField.Status:
                selector = x => x.Status;
                break;

            case UserFilterField.Role:
                selector = x => x.Role;
                break;

            case UserFilterField.Provider:
                selector = x => x.Provider;
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(request.FilterField));
        }

        var values = await userRepository.GetDistinctFieldValuesAsync(selector, cancellationToken);
        return values.Select(x => x.ToString() ?? string.Empty);
    }
}