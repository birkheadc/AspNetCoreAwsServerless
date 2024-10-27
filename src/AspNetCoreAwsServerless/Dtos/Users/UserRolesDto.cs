using AspNetCoreAwsServerless.Entities.Roles;

namespace AspNetCoreAwsServerless.Dtos.Users;

public class UserRolesDto
{
  public UserRole[] Roles { get; set; } = [];
}
