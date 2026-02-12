using IdentityService.Application.MediatR.Requests.Authentication.DeleteRefreshToken;
using IdentityService.Application.MediatR.Requests.Authentication.Login;
using IdentityService.Application.MediatR.Requests.Authentication.Register;
using Infrastructure.WebApi.Rest.Base;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Api.Rest.Controllers.Account;

public class AccountController : BaseController
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = await Mediator.Send(request, cancellationToken);
        return Ok(result);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = await Mediator.Send(request, cancellationToken);
        return Ok(result);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(
        CancellationToken cancellationToken
    )
    {
        var result = await Mediator.Send(new DeleteRefreshTokenRequest(), cancellationToken);
        return Ok(result);
    }
}