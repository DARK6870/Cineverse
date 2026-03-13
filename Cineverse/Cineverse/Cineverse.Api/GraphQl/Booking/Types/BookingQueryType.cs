using Auth.Authentication;
using Cineverse.Api.GraphQl.Booking.Queries;
using HotChocolate.Types;

namespace Cineverse.Api.GraphQl.Booking.Types;

public class BookingQueryType : ObjectTypeExtension<BookingQuery>
{
    protected override void Configure(IObjectTypeDescriptor<BookingQuery> descriptor)
    {
        if (!AuthenticationSetup.EnableSecurity)
            return;
        
        descriptor
            .Field(x => x.GetBookings(null!, CancellationToken.None))
            .Authorize(AuthenticationPolicies.ManagerAccessPolicy);
        
        descriptor
            .Field(x => x.GetUserBookings(null!, CancellationToken.None))
            .Authorize();
        
        descriptor.Field(x => x.GetBookingById(null!, null!, CancellationToken.None))
            .Authorize();
    }
}