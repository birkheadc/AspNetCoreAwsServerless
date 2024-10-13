using AspNetCoreAwsServerless.Attributes.ResolveUser;
using AspNetCoreAwsServerless.Controllers.ResolvedUser;
using AspNetCoreAwsServerless.Converters.Users;
using AspNetCoreAwsServerless.Dtos.Users;
using AspNetCoreAwsServerless.Entities.Users;
using AspNetCoreAwsServerless.Services.Users;
using AspNetCoreAwsServerless.Utils.Result;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreAwsServerless.Controllers.Me;

[ApiController]
[Route("me")]
// [ResolveUser]
// [Authorize]
public class MeController(IUsersService usersService, IUsersConverter usersConverter, ILogger<MeController> logger) : ResolvedUserControllerBase
{
  private readonly IUsersService _usersService = usersService;
  private readonly ILogger<MeController> _logger = logger;
  private readonly IUsersConverter _usersConverter = usersConverter;
  [HttpGet]
  public async Task<ActionResult<UserDto>> Get()
  {
    _logger.LogInformation("Get");

    ApiResult<User> userResult = await _usersService.Get(CurrentUserId);

    if (userResult.IsFailure)
    {
      _logger.LogWarning("User not found in HttpContext.Items");
      return Unauthorized();
    }

    _logger.LogInformation("User found in HttpContext.Items: {username} | {emailAddress}", userResult.Value.Id, userResult.Value.EmailAddress);

    return Ok(_usersConverter.ToDto(userResult.Value));
  }
}
