using IdentityService.Mongo.Schemas.Entities;
using MediatR;

namespace IdentityService.Application.MediatR.Requests.Users.GetUsers;

public record GetUsersRequest(string? SearchTerm) : IRequest<IQueryable<UserEntity>>;