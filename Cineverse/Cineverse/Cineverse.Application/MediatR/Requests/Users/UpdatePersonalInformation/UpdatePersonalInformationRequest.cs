using MediatR;

namespace Cineverse.Application.MediatR.Requests.Users.UpdatePersonalInformation;

public record UpdatePersonalInformationRequest(
    string FirstName,
    string LastName
) : IRequest<bool>;