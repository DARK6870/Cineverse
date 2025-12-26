using HotChocolate.Types;
using Infrastructure.WebApi.GraphQl.Base;

namespace IdentityService.Api.GraphQl.Queries;

[ExtendObjectType(nameof(BaseGraphQlQuery))]
public class TestQuery
{
    public string Test()
    {
        return "Test";
    }
}