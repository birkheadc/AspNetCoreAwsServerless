using System.Security.Claims;
using AspNetCoreAwsServerless.Caches.Session;
using AspNetCoreAwsServerless.Dtos.Session;
using AspNetCoreAwsServerless.Entities.Users;
using AspNetCoreAwsServerless.Services.Cognito;
using AspNetCoreAwsServerless.Services.Jwt;
using AspNetCoreAwsServerless.Services.Users;
using AspNetCoreAwsServerless.Utils.Result;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AspNetCoreAwsServerless.Services.Session;

public class SessionService(ICognitoService cognitoService, IUsersService usersService, ISessionCache sessionCache, ILogger<SessionService> logger) : ISessionService
{
  private readonly ICognitoService _cognitoService = cognitoService;
  private readonly IUsersService _usersService = usersService;
  private readonly ILogger<SessionService> _logger = logger;
  private readonly ISessionCache _sessionCache = sessionCache;

  public async Task<ApiResult<SessionContext>> Login(LoginDto code)
  {
    var response = await _cognitoService.GetTokens(code);

    if (response.IsFailure)
    {
      return response.Errors;
    }

    ApiResult<User> userResult = await _usersService.GetOrCreateNew(new IdToken { Value = response.Value.IdToken });

    if (userResult.IsFailure)
    {
      return userResult.Errors;
    }

    User user = userResult.Value;
    // await _sessionCache.SetAccessToken(user.Id, response.Value.AccessToken);

    return ApiResult<SessionContext>.Success(new SessionContext
    {
      User = user,
      Tokens = new SessionTokens
      {
        AccessToken = response.Value.AccessToken,
        RefreshToken = response.Value.RefreshToken,
        IdToken = response.Value.IdToken,
        ExpiresInSeconds = response.Value.ExpiresIn
      }
    });
  }
}
