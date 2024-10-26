using System.Security.Claims;
using AspNetCoreAwsServerless.Entities.Permissions;
using AspNetCoreAwsServerless.Entities.Roles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AspNetCoreAwsServerless.Authorization;

public class RequiresPermissionAttribute(UserPermission[] requiredPermissions)
  : Attribute,
    IAsyncAuthorizationFilter
{
  private readonly UserPermission[] _requiredPermissions = requiredPermissions;

  public Task OnAuthorizationAsync(AuthorizationFilterContext context)
  {
    Claim[] roleClaims = context.HttpContext.User.FindAll(ClaimTypes.Role).ToArray();

    UserRole[] userRoles = roleClaims.Select(claim => Enum.Parse<UserRole>(claim.Value)).ToArray();
    UserPermission[] userPermissions = RolePermissions.GetPermissionsForRoles(userRoles);

    foreach (UserPermission permission in _requiredPermissions)
    {
      Console.WriteLine($"Checking permission {permission}");
      if (!userPermissions.Contains(permission))
      {
        context.Result = new ForbidResult();
        return Task.CompletedTask;
      }
    }

    return Task.CompletedTask;
  }
}
