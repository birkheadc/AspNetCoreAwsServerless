using System.Security.Claims;
using AspNetCoreAwsServerless.Converters.Users;
using AspNetCoreAwsServerless.Dtos.Session;
using AspNetCoreAwsServerless.Dtos.Users;
using AspNetCoreAwsServerless.Services.Session;
using AspNetCoreAwsServerless.Utils.Result;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreAwsServerless.Controllers.Session;

[ApiController]
[AllowAnonymous]
[Route("session")]
public class SessionController(ISessionService sessionService, ILogger<SessionController> logger, IUsersConverter userConverter) : ControllerBase
{
  private readonly ISessionService _sessionService = sessionService;
  private readonly ILogger<SessionController> _logger = logger;

  private readonly IUsersConverter _userConverter = userConverter;
  [HttpPost]
  public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto dto)
  {
    _logger.LogInformation("SessionController.Login");
    ApiResult<SessionContext> result = await _sessionService.Login(dto);

    if (result.IsFailure)
    {
      return result.GetActionResult();
    }

    List<Claim> claims = [new(ClaimTypes.NameIdentifier, result.Value.User.Id.ToString())];
    ClaimsIdentity identity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

    HttpContext.Response.Cookies.Append("refresh_token", result.Value.Tokens.RefreshToken, new CookieOptions
    {
      HttpOnly = true,
      Secure = true,
      SameSite = SameSiteMode.Strict,
      Expires = result.Value.Tokens.ExpiresInSeconds.HasValue ? DateTime.UtcNow.AddSeconds((double)result.Value.Tokens.ExpiresInSeconds) : null,
    });

    return Ok(_userConverter.ToDto(result.Value.User));
  }
}