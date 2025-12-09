using Cineverse.Api.GraphQl.Queries;
using Cineverse.Identity;
using Cineverse.Identity.Authentication;
using HotChocolate.Types;

namespace Cineverse.Api.GraphQl.Types.QueryTypes;

public class BookingQueryType : ObjectTypeExtension<BookingQuery>
{
    protected override void Configure(IObjectTypeDescriptor<BookingQuery> descriptor)
    {
        if (!AuthenticationSetup.EnableSecurity)
            return;
        
        descriptor
            .Field(x => x.GetBookings(null!, CancellationToken.None))
            .Authorize(AuthenticationPolicies.ManagerAccessPolicy);
        
        descriptor.Field(x => x.GetBookingById(null!, null!, CancellationToken.None))
            .Authorize();
    }
}