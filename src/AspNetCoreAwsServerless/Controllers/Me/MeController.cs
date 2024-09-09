using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
public class MeController(IUsersService usersService, IUsersConverter usersConverter, ILogger<MeController> logger) : ControllerBase
{
  private readonly IUsersService _usersService = usersService;
  private readonly ILogger<MeController> _logger = logger;
  private readonly IUsersConverter _usersConverter = usersConverter;
  [HttpGet]
  public async Task<ActionResult<UserDto>> Get()
  {
    _logger.LogInformation("Get");
    string? id = User.Claims.Where(c => c.Type == "username").FirstOrDefault()?.Value;
    if (id is null)
    {
      return Problem();
    }
    ApiResult<User> result = await _usersService.Get(id);
    return result.GetActionResult(_usersConverter.ToDto);
  }
}
