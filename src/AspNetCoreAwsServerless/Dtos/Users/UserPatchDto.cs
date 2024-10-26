namespace AspNetCoreAwsServerless.Dtos.Users;

public class UserPatchDto
{
  public string? EmailAddress { get; init; }
  public string? DisplayName { get; init; }
  public string[]? Roles { get; init; }
}
