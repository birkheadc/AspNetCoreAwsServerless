using AspNetCoreAwsServerless.Dtos.Session;
using AspNetCoreAwsServerless.Services.Session;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreAwsServerless.Controllers.Session;

[ApiController]
[AllowAnonymous]
[Route("session")]
public class SessionController(ISessionService sessionService) : ControllerBase
{
  private readonly ISessionService _sessionService = sessionService;
  [HttpPost]
  public async Task<IActionResult> Login([FromBody] LoginDto dto)
  {
    var result = await _sessionService.Login(dto);

    if (result.IsFailure)
    {
      return result.GetActionResult();
    }

    HttpContext.Response.Cookies.Append("access_token", result.Value.AccessToken, new CookieOptions
    {
      HttpOnly = true,
      Secure = true,
      SameSite = SameSiteMode.Strict,
      Expires = result.Value.ExpiresInSeconds.HasValue ? DateTime.UtcNow.AddSeconds((double)result.Value.ExpiresInSeconds) : null,
    });

    HttpContext.Response.Cookies.Append("refresh_token", result.Value.RefreshToken, new CookieOptions
    {
      HttpOnly = true,
      Secure = true,
      SameSite = SameSiteMode.Strict,
      Expires = result.Value.ExpiresInSeconds.HasValue ? DateTime.UtcNow.AddSeconds((double)result.Value.ExpiresInSeconds) : null,
    });

    return Ok();
  }
}