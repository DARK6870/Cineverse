using IdentityService.Application.MediatR.Requests.Authentication.GenerateAccessToken;
using Infrastructure.WebApi.Rest.Base;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Api.Rest.Controllers.Token;

public class TokenController : BaseController
{
    [HttpPost("generate")]
    public async Task<IActionResult> GenerateAccessToken(
        [FromBody] GenerateAccessTokenRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = await Mediator.Send(request, cancellationToken);
        return Ok(result);
    }
}