using Auth.Authentication;
using HotChocolate.Types;
using IdentityService.Api.GraphQl.User.Query;
using IdentityService.Application.Common.Models.Filters;

namespace IdentityService.Api.GraphQl.User.Types;

public class UserQueryType : ObjectTypeExtension<UserQuery>
{
    protected override void Configure(IObjectTypeDescriptor<UserQuery> descriptor)
    {
        if (!AuthenticationSetup.EnableSecurity)
            return;

        descriptor
            .Field(x => x.GetUsers(null!, null!, CancellationToken.None))
            .Authorize(AuthenticationPolicies.ManagerAccessPolicy);
        
        descriptor
            .Field(x => x.GetUserById(null!, null!, CancellationToken.None))
            .Authorize(AuthenticationPolicies.ManagerAccessPolicy);
        
        descriptor
            .Field(x => x.GetUsersDistinctFilterValues(UserFilterField.Status, null!, CancellationToken.None))
            .Authorize(AuthenticationPolicies.ManagerAccessPolicy);
    }
}