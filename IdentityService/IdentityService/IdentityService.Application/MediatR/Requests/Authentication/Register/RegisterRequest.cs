using Auth.Models.Responses;
using MediatR;

namespace IdentityService.Application.MediatR.Requests.Authentication.Register;

public record RegisterRequest(
    string Email,
    string FirstName,
    string LastName,
    string Password,
    string ConfirmPassword
) : IRequest<AuthenticationResponse>;