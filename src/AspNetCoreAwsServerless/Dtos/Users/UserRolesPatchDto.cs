using AspNetCoreAwsServerless.Entities.Roles;

namespace AspNetCoreAwsServerless.Dtos.Users;

public class UserRolesPatchDto
{
  public required UserRole[] Roles { get; set; }
}
