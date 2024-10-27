using AspNetCoreAwsServerless.Authorization;
using AspNetCoreAwsServerless.Controllers.ResolvedUser;
using AspNetCoreAwsServerless.Entities.Permissions;
using AspNetCoreAwsServerless.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreAwsServerless.Controllers.Debug;

/// <summary>
/// Provides various convenient endpoints for debugging.
/// </summary>
[ApiController]
[Route("debug")]
[Authorize]
public class DebuggingController(ILogger<DebuggingController> logger, IUsersService usersService)
  : ResolvedUserControllerBase<DebuggingController>(logger, usersService)
{
  [HttpGet]
  [RequiresPermission([UserPermission.CanModifyBooks])]
  [Route("can-i/modify-books")]
  public ActionResult CanIModifyBooks()
  {
    return Ok("Yes you can modify books!");
  }

  [HttpGet]
  [RequiresPermission([UserPermission.CanModifyUserRoles])]
  [Route("can-i/modify-user-roles")]
  public ActionResult CanIModifyUserRoles()
  {
    return Ok("Yes you can modify user roles!");
  }
}
