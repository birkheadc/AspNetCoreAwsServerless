using System.Security.Claims;
using AspNetCoreAwsServerless.Entities.Users;
using AspNetCoreAwsServerless.Services.Users;
using AspNetCoreAwsServerless.Utils.Id;
using AspNetCoreAwsServerless.Utils.Result;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreAwsServerless.Controllers.ResolvedUser;

/// <summary>
/// Base class for controllers that require the current user to be resolved.
/// </summary>
public abstract class ResolvedUserControllerBase<T>(ILogger<T> logger, IUsersService usersService) : ControllerBase
{
  protected readonly ILogger<T> _logger = logger;
  protected readonly IUsersService _usersService = usersService;
  protected async Task<User> GetCurrentUser()
  {
    _logger.LogInformation("GetCurrentUser");

    string? stringId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (stringId is null)
    {
      _logger.LogError("No user ID found in claims");
      throw new UnauthorizedAccessException();
    }

    if (!Id<User>.TryParse(stringId, out Id<User> id))
    {
      _logger.LogError("Invalid user ID {UserId}", stringId);
      throw new UnauthorizedAccessException();
    }

    ApiResult<User> userResult = await _usersService.Get(id);
    if (userResult.IsFailure)
    {
      _logger.LogError("User not found for ID {UserId}", stringId);
      throw new UnauthorizedAccessException();
    }

    return userResult.Value;
  }
}
