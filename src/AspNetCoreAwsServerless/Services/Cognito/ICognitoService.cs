using AspNetCoreAwsServerless.Dtos.Cognito;
using AspNetCoreAwsServerless.Dtos.Session;
using AspNetCoreAwsServerless.Utils.Result;

namespace AspNetCoreAwsServerless.Services.Cognito;

public interface ICognitoService
{
  public Task<ApiResult<CognitoTokens>> GetTokens(LoginDto loginDto);
}
