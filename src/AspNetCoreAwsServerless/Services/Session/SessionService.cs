using AspNetCoreAwsServerless.Dtos.Session;
using AspNetCoreAwsServerless.Services.Cognito;
using AspNetCoreAwsServerless.Utils.Result;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AspNetCoreAwsServerless.Services.Session;

public class SessionService(ICognitoService cognitoService) : ISessionService
{
  private readonly ICognitoService _cognitoService = cognitoService;

  public async Task<ApiResult<SessionTokens>> Login(LoginDto code)
  {
    var response = await _cognitoService.GetTokens(code);

    if (response.IsFailure)
    {
      return ApiResult<SessionTokens>.Failure(response.Errors);
    }

    return ApiResult<SessionTokens>.Success(new SessionTokens
    {
      AccessToken = response.Value.AccessToken,
      RefreshToken = response.Value.RefreshToken,
      IdToken = response.Value.IdToken,
      ExpiresInSeconds = response.Value.ExpiresIn
    });
  }
}
