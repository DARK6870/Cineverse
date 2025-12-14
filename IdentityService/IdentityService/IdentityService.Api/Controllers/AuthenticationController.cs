using IdentityService.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Api.Controllers;

public class AuthenticationController : BaseController
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        await Task.Delay(1);
        return Ok("test");
    }
}