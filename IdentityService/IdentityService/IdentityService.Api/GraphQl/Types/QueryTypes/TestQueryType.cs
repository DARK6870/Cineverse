using Auth.Authentication;
using HotChocolate.Types;
using IdentityService.Api.GraphQl.Queries;

namespace IdentityService.Api.GraphQl.Types.QueryTypes;

public class TestQueryType : ObjectTypeExtension<TestQuery>
{
    protected override void Configure(IObjectTypeDescriptor<TestQuery> descriptor)
    {
        if (!AuthenticationSetup.EnableSecurity)
            return;
    }
}