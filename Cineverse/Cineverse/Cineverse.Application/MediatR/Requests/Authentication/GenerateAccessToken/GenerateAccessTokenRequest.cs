using Cineverse.Infrastructure.Common.Models;
using MediatR;

namespace Cineverse.Application.MediatR.Requests.Authentication.GenerateAccessToken;

public record GenerateAccessTokenRequest(string RefreshToken) : IRequest<AuthenticationResponse>;