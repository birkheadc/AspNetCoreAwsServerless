using AspNetCoreAwsServerless.Entities.Roles;

namespace AspNetCoreAwsServerless.Entities.Users;

public record UserRoles
{
  public UserRole[] Roles { get; set; } = [];
}
