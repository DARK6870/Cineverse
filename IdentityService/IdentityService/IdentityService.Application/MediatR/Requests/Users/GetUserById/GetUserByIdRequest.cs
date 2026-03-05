using IdentityService.Mongo.Schemas.Entities;
using MediatR;

namespace IdentityService.Application.MediatR.Requests.Users.GetUserById;

public record GetUserByIdRequest(string Id) : IRequest<UserEntity>;