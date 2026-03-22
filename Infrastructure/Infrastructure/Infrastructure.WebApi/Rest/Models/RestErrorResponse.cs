using System.Net;

namespace Infrastructure.WebApi.Rest.Models;

public record RestErrorResponse(
    HttpStatusCode StatusCode,
    string ErrorMessage,
    string[] ValidationErrors
);