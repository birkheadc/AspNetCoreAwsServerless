namespace AspNetCoreAwsServerless.Entities.Roles;

/// <summary>
/// Defines the roles for users.
/// There is no BasicUser role because all authenticated users are considered basic users.
/// </summary>
public enum UserRole
{
  SuperAdmin,
  Admin,
}