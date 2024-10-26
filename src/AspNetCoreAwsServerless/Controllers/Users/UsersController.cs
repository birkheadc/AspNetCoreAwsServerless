using AspNetCoreAwsServerless.Converters.Users;
using AspNetCoreAwsServerless.Dtos.Users;
using AspNetCoreAwsServerless.Entities.Users;
using AspNetCoreAwsServerless.Services.Users;
using AspNetCoreAwsServerless.Utils.Result;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreAwsServerless.Controllers.Users;

[ApiController]
[Route("api/[controller]")]
public class UsersController(
  IUsersService usersService,
  ILogger<UsersController> logger,
  IUsersConverter usersConverter
) : ControllerBase
{
  private readonly IUsersService _usersService = usersService;
  private readonly ILogger<UsersController> _logger = logger;
  private readonly IUsersConverter _usersConverter = usersConverter;

  [HttpPatch("{id}")]
  public async Task<ActionResult<UserDto>> UpdateUser(
    [FromRoute] string id,
    [FromBody] UserPatchDto dto
  )
  {
    ApiResult<User> result = await _usersService.Patch(id, dto);
    return result.GetActionResult(_usersConverter.ToDto);
  }
}
