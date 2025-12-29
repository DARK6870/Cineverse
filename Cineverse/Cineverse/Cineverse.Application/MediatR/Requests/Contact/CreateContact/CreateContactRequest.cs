using Cineverse.Application.Common.Models.Enums;
using MediatR;

namespace Cineverse.Application.MediatR.Requests.Contact.CreateContact;

public record CreateContactRequest(
    Department Department,
    string FirstName,
    string LastName,
    string Email,
    string Subject,
    string Description
) : IRequest<bool>;