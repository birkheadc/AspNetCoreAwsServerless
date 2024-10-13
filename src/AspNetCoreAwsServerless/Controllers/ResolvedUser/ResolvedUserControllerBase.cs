using AspNetCoreAwsServerless.Attributes.ResolveUser;
using AspNetCoreAwsServerless.Entities.Users;
using AspNetCoreAwsServerless.Utils.Id;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreAwsServerless.Controllers.ResolvedUser;

// [ResolveUser]
public abstract class ResolvedUserControllerBase : ControllerBase
{
  protected Id<User> CurrentUserId => new(User.FindFirst("sub")?.Value ?? throw new UnauthorizedAccessException());
}
