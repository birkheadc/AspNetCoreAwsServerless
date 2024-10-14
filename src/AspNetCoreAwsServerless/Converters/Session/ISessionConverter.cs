using AspNetCoreAwsServerless.Dtos.Cognito;
using AspNetCoreAwsServerless.Dtos.Session;

namespace AspNetCoreAwsServerless.Converters.Session;

public interface ISessionConverter
{
  SessionTokens ToSessionTokens(CognitoTokens cognitoTokens);
}
