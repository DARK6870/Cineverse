using System.Net;

namespace AutomationTests.Models.Generic;

public record RestErrorResponse(HttpStatusCode StatusCode, string ErrorMessage, string[] ValidationErrors);