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

/// <summary>
/// Provides endpoints for signing in and out.
/// </summary>
[ApiController]
[AllowAnonymous]
[Route("session")]
public class SessionController(
  ISessionService sessionService,
  ILogger<SessionController> logger,
  IUsersConverter userConverter
) : ControllerBase
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

    await SigninUser(result.Value);

    return Ok(_userConverter.ToDto(result.Value.User));
  }

  [HttpDelete]
  public async Task<ActionResult> Logout()
  {
    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    DeleteRefreshTokenCookie();
    return Ok();
  }

  private async Task SigninUser(SessionContext context)
  {
    Claim[] roleClaims = context
      .User.Roles.Select(role => new Claim(ClaimTypes.Role, role.ToString()))
      .ToArray();
    Claim nameIdentifierClaim = new(ClaimTypes.NameIdentifier, context.User.Id.ToString());

    Claim[] claims = [.. roleClaims, nameIdentifierClaim];

    ClaimsIdentity identity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
    ClaimsPrincipal principal = new(identity);

    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
    SetRefreshTokenCookie(context.Tokens.RefreshToken, context.Tokens.ExpiresInSeconds);
  }

  /// <summary>
  /// Appends the user's refresh token to the response cookies as a secure http-only cookie.
  /// </summary>
  /// <param name="refreshToken">The refresh token to set.</param>
  /// <param name="expiresInSeconds">The number of seconds until the refresh token expires.</param>
  private void SetRefreshTokenCookie(string refreshToken, int? expiresInSeconds)
  {
    HttpContext.Response.Cookies.Append(
      "refresh_token",
      refreshToken,
      new CookieOptions
      {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.Strict,
        Expires = expiresInSeconds.HasValue
          ? DateTime.UtcNow.AddSeconds((double)expiresInSeconds)
          : null,
      }
    );
  }

  private void DeleteRefreshTokenCookie()
  {
    HttpContext.Response.Cookies.Delete("refresh_token");
  }
}
