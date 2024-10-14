using AspNetCoreAwsServerless.Dtos.Cognito;
using AspNetCoreAwsServerless.Dtos.Session;

namespace AspNetCoreAwsServerless.Converters.Session;

public class SessionConverter : ISessionConverter
{
  public SessionTokens ToSessionTokens(CognitoTokens cognitoTokens)
  {
    return new SessionTokens
    {
      AccessToken = cognitoTokens.AccessToken,
      RefreshToken = cognitoTokens.RefreshToken,
      IdToken = cognitoTokens.IdToken,
      ExpiresInSeconds = cognitoTokens.ExpiresIn
    };
  }
}