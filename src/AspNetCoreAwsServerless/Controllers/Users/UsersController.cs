using AspNetCoreAwsServerless.Dtos.Users;
using AspNetCoreAwsServerless.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreAwsServerless.Controllers.Users;

[ApiController]
[Route("users")]
public class UsersController(IUsersService usersService) : ControllerBase
{
  private readonly IUsersService _usersService = usersService;

  [HttpGet]
  [Authorize]
  public async Task<ActionResult<UserDto>> Get()
  {
    throw new NotImplementedException();
  }
}
