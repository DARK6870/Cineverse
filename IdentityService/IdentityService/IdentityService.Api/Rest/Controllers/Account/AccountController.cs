using IdentityService.Application.MediatR.Requests.Authentication.DeleteRefreshToken;
using IdentityService.Application.MediatR.Requests.Authentication.ExternalLogin;
using IdentityService.Application.MediatR.Requests.Authentication.Login;
using IdentityService.Application.MediatR.Requests.Authentication.Register;
using IdentityService.Application.Models.Options;
using IdentityService.Mongo.Schemas.Enums;
using Infrastructure.WebApi.Rest.Base;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace IdentityService.Api.Rest.Controllers.Account;

public class AccountController(
    IOptions<ExternalProvidersOptions> externalProvidersOptions
) : BaseController
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
    
    [HttpGet("login/google")]
    public IActionResult LoginGoogle()
    {
        var properties = new AuthenticationProperties
        {
            RedirectUri = Url.Action(nameof(GoogleCallback))
        };
        
        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }
    
    [HttpGet("login/github")]
    public IActionResult LoginGitHub()
    {
        var properties = new AuthenticationProperties
        {
            RedirectUri = Url.Action(nameof(GitHubCallback))
        };
        
        return Challenge(properties, nameof(AuthenticationProvider.Github));
    }
    
    [HttpGet("login/google/callback")]
    public async Task<IActionResult> GoogleCallback()
    { 
        await Mediator.Send(new ExternalLoginRequest(AuthenticationProvider.Google));
        return Redirect(externalProvidersOptions.Value.RedirectUrl);
    }
    
    [HttpGet("login/github/callback")]
    public async Task<IActionResult> GitHubCallback()
    {
        await Mediator.Send(new ExternalLoginRequest(AuthenticationProvider.Github));
        return Redirect(externalProvidersOptions.Value.RedirectUrl);
    }
}
