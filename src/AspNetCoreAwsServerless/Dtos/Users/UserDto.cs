namespace AspNetCoreAwsServerless.Dtos.Users;

public class UserDto
{
  public required string Id { get; init; }
  public required string EmailAddress { get; init; }
  public required UserProfileDto Profile { get; init; }
  public required UserRolesDto Roles { get; init; }
}
