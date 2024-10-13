using System.Security.Claims;
using AspNetCoreAwsServerless.Entities.Users;
using AspNetCoreAwsServerless.Services.Users;
using AspNetCoreAwsServerless.Utils.Id;
using AspNetCoreAwsServerless.Utils.Result;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreAwsServerless.Controllers.ResolvedUser;

public abstract class ResolvedUserControllerBase<T>(ILogger<T> logger, IUsersService usersService) : ControllerBase
{
  protected readonly ILogger<T> _logger = logger;
  protected readonly IUsersService _usersService = usersService;
  protected async Task<User> GetCurrentUser()
  {
    _logger.LogInformation("GetCurrentUser");

    string? userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (userId is null)
    {
      _logger.LogError("No user ID found in claims");
      throw new UnauthorizedAccessException();
    }

    ApiResult<User> userResult = await _usersService.Get(new Id<User>(userId));
    if (userResult.IsFailure)
    {
      _logger.LogError("User not found for ID {UserId}", userId);
      throw new UnauthorizedAccessException();
    }

    return userResult.Value;
  }
}
