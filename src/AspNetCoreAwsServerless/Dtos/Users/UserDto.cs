using AspNetCoreAwsServerless.Entities.Roles;

namespace AspNetCoreAwsServerless.Dtos.Users;

public class UserDto
{
  public required string Id { get; init; }
  public required string EmailAddress { get; init; }
  public string? DisplayName { get; init; }
  public required string[] Roles { get; init; }
}
