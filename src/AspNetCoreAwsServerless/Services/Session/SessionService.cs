using AspNetCoreAwsServerless.Caches.Session;
using AspNetCoreAwsServerless.Converters.Session;
using AspNetCoreAwsServerless.Dtos.Cognito;
using AspNetCoreAwsServerless.Dtos.Session;
using AspNetCoreAwsServerless.Entities.Users;
using AspNetCoreAwsServerless.Services.Cognito;
using AspNetCoreAwsServerless.Services.Users;
using AspNetCoreAwsServerless.Utils.Result;

namespace AspNetCoreAwsServerless.Services.Session;

public class SessionService(ICognitoService cognitoService, IUsersService usersService, ISessionCache sessionCache, ISessionConverter sessionConverter, ILogger<SessionService> logger) : ISessionService
{
  private readonly ICognitoService _cognitoService = cognitoService;
  private readonly IUsersService _usersService = usersService;
  private readonly ILogger<SessionService> _logger = logger;
  private readonly ISessionCache _sessionCache = sessionCache;
  private readonly ISessionConverter _sessionConverter = sessionConverter;

  public async Task<ApiResult<SessionContext>> Login(LoginDto code)
  {
    _logger.LogInformation("Logging in with code: {code}", code.Code);
    ApiResult<CognitoTokens> cognitoTokensResult = await _cognitoService.GetTokens(code);

    if (cognitoTokensResult.IsFailure)
    {
      return cognitoTokensResult.Errors;
    }

    CognitoTokens cognitoTokens = cognitoTokensResult.Value;

    ApiResult<User> userResult = await _usersService.GetOrCreateNew(new IdToken { Value = cognitoTokens.IdToken });

    if (userResult.IsFailure)
    {
      return userResult.Errors;
    }

    User user = userResult.Value;
    await _sessionCache.SetAccessToken(user.Id, cognitoTokens.AccessToken);

    return ApiResult<SessionContext>.Success(new SessionContext
    {
      User = user,
      Tokens = _sessionConverter.ToSessionTokens(cognitoTokens)
    });
  }
}
