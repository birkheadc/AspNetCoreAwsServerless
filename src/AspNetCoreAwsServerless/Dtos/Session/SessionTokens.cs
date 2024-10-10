namespace AspNetCoreAwsServerless.Dtos.Session;

public class SessionTokens
{
  public required string AccessToken { get; init; }
  public required string RefreshToken { get; init; }

  public required string IdToken { get; init; }
  public int? ExpiresInSeconds { get; init; }
}
