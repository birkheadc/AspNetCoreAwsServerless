using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreAwsServerless.Attributes.ResolveUser;
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
  // [ResolveUser]
  public ActionResult<UserDto> Get()
  {
    _logger.LogInformation("Get");
    User? user = (User?)HttpContext.Items["user"];

    if (user is null)
    {
      _logger.LogWarning("User not found in HttpContext.Items");
      return Ok(_usersConverter.ToDto(new User() { Id = new Utils.Id.Id<User>(Guid.NewGuid()), EmailAddress = "email", DisplayName = "1" }));
      // return Unauthorized();
    }

    _logger.LogInformation("User found in HttpContext.Items: {username} | {emailAddress}", user.Id, user.EmailAddress);

    return Ok(_usersConverter.ToDto(user));
  }
}
