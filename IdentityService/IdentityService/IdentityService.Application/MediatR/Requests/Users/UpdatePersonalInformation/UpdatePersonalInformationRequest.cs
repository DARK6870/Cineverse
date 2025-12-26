using MediatR;

namespace IdentityService.Application.MediatR.Requests.Users.UpdatePersonalInformation;

public record UpdatePersonalInformationRequest(
    string FirstName,
    string LastName
) : IRequest<bool>;