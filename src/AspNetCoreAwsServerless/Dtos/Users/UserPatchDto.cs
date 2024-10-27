using AspNetCoreAwsServerless.Entities.Roles;

namespace AspNetCoreAwsServerless.Dtos.Users;

public class UserPatchDto
{
  public string? EmailAddress { get; init; }
  public UserProfileDto? Profile { get; init; }
}
