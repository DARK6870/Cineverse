using Cineverse.API.GraphQl.Base;
using Cineverse.API.GraphQl.Mutations;
using Cineverse.Infrastructure.Authentication;
using HotChocolate.Types;

namespace Cineverse.API.GraphQl.Types;

public class BookingMutationType : ObjectTypeExtension<BookingMutation>
{
    protected override void Configure(IObjectTypeDescriptor<BookingMutation> descriptor)
    {
        if (!AuthenticationSetup.EnableSecurity)
            return;
        
        descriptor
            .Field(x => x.CreateBooking(null!, null!, CancellationToken.None))
            .Authorize(AuthenticationSetup.UserAccessPolicy);
        
        descriptor
            .Field(x => x.DeleteBooking(null!, null!, CancellationToken.None))
            .Authorize(AuthenticationSetup.ManagerAccessPolicy);
    }
}