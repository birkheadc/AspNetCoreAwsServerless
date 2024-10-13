using AspNetCoreAwsServerless.Controllers.ResolvedUser;
using AspNetCoreAwsServerless.Converters.Users;
using AspNetCoreAwsServerless.Dtos.Users;
using AspNetCoreAwsServerless.Entities.Users;
using AspNetCoreAwsServerless.Services.Users;
using AspNetCoreAwsServerless.Utils.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreAwsServerless.Controllers.Me;

[ApiController]
[Route("me")]
[Authorize]
public class MeController(IUsersService usersService, IUsersConverter usersConverter, ILogger<MeController> logger) : ResolvedUserControllerBase<MeController>(logger, usersService)
{
  private readonly IUsersConverter _usersConverter = usersConverter;

  [HttpGet]
  public async Task<ActionResult<UserDto>> Get()
  {
    User user = await GetCurrentUser();
    return Ok(_usersConverter.ToDto(user));
  }
}
