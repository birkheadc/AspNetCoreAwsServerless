using AspNetCoreAwsServerless.Entities.Permissions;

namespace AspNetCoreAwsServerless.Entities.Roles;

/// <summary>
/// Defines the permissions for each role.
/// SuperAdmin dynamically includes all permissions.
/// </summary>
public static class RolePermissions
{
  private static readonly Dictionary<UserRole, UserPermission[]> _permissions =
    new()
    {
      { UserRole.SuperAdmin, Enum.GetValues<UserPermission>().ToArray() },
      { UserRole.Admin, new[] { UserPermission.ModifyBooks, UserPermission.ModifyUsers } },
    };

  public static UserPermission[] GetPermissionsForRoles(UserRole[] roles)
  {
    return roles.SelectMany(role => _permissions[role]).ToArray();
  }
}
