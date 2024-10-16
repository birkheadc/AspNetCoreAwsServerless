namespace AspNetCoreAwsServerless.Dtos.Cognito;

public class CognitoUser
{
  public required string Username { get; init; }
  public required string EmailAddress { get; init; }
}
