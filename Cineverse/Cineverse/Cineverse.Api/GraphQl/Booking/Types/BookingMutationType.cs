using Auth.Authentication;
using Cineverse.Api.GraphQl.Booking.Mutations;
using HotChocolate.Types;

namespace Cineverse.Api.GraphQl.Booking.Types;

public class BookingMutationType : ObjectTypeExtension<BookingMutation>
{
    protected override void Configure(IObjectTypeDescriptor<BookingMutation> descriptor)
    {
        if (!AuthenticationSetup.EnableSecurity)
            return;
        
        descriptor.Field(x => x.CreateBooking(null!, null!, CancellationToken.None))
            .Authorize();
        
        descriptor.Field(x => x.DeleteBooking(null!, null!, CancellationToken.None))
            .Authorize(AuthenticationPolicies.ManagerAccessPolicy);
    }
}