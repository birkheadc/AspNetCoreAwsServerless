using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreAwsServerless.Dtos.Users;
using AspNetCoreAwsServerless.Entities.Users;
using AspNetCoreAwsServerless.Services.Users;
using AspNetCoreAwsServerless.Utils.Id;
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
