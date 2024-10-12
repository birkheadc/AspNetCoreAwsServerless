using AspNetCoreAwsServerless.Entities.Users;

namespace AspNetCoreAwsServerless.Dtos.Session;

public class SessionContext
{
  public required User User { get; init; }
  public required SessionTokens Tokens { get; init; }
}