using Cineverse.Infrastructure.Common.Models;
using MediatR;

namespace Cineverse.Application.MediatR.Requests.Authentication.Register;

public record RegisterRequest(
    string Email,
    string FirstName,
    string LastName,
    string Password,
    string ConfirmPassword
) : IRequest<AuthenticationResponse>;