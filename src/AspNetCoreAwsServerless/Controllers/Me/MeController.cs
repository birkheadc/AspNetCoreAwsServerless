using AspNetCoreAwsServerless.Controllers.ResolvedUser;
using AspNetCoreAwsServerless.Converters.Users;
using AspNetCoreAwsServerless.Dtos.Users;
using AspNetCoreAwsServerless.Entities.Users;
using AspNetCoreAwsServerless.Services.Users;
using AspNetCoreAwsServerless.Utils.Id;
using AspNetCoreAwsServerless.Utils.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreAwsServerless.Controllers.Me;

/// <summary>
/// Provides endpoints for the current user to manage their own data.
/// </summary>
[ApiController]
[Route("me")]
[Authorize]
public class MeController(
  IUsersService usersService,
  IUsersConverter usersConverter,
  ILogger<MeController> logger
) : ResolvedUserControllerBase<MeController>(logger, usersService)
{
  private readonly IUsersConverter _usersConverter = usersConverter;

  [HttpGet]
  public async Task<ActionResult<UserDto>> Get()
  {
    Id<User> id = GetCurrentUserId();
    ApiResult<User> userResult = await _usersService.Get(id);
    return userResult.GetActionResult(_usersConverter.ToDto);
  }

  [HttpPatch("profile")]
  public async Task<ActionResult<UserDto>> UpdateProfile([FromBody] UserProfileDto dto)
  {
    Id<User> id = GetCurrentUserId();
    ApiResult<User> userResult = await _usersService.UpdateProfile(id, dto);
    return userResult.GetActionResult(_usersConverter.ToDto);
  }
}
