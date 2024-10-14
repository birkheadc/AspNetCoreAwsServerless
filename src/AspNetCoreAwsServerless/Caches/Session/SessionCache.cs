using AspNetCoreAwsServerless.Entities.Users;
using AspNetCoreAwsServerless.Utils.Id;
using AspNetCoreAwsServerless.Utils.Result;

namespace AspNetCoreAwsServerless.Caches.Session;

public class SessionCache(ILogger<SessionCache> logger) : ISessionCache
{
  private readonly ILogger<SessionCache> _logger = logger;
  private readonly Dictionary<string, string> _userAccessTokens = new();

  public Task<ApiResult<string>> GetAccessToken(Id<User> userId)
  {
    _logger.LogInformation("Getting access token for user {UserId}", userId);
    string? accessToken = _userAccessTokens.GetValueOrDefault(userId.ToString());
    if (accessToken is null)
    {
      _logger.LogWarning("Access token not found for user {UserId}", userId);
      return Task.FromResult(ApiResult<string>.Failure(ApiResultErrors.NotFound));
    }

    _logger.LogInformation("Access token found for user {UserId}", userId);
    return Task.FromResult(ApiResult<string>.Success(accessToken));
  }

  public Task<ApiResult> SetAccessToken(Id<User> userId, string accessToken)
  {
    _logger.LogInformation("Storing access token for user {UserId}", userId);
    _userAccessTokens[userId.ToString()] = accessToken;
    return Task.FromResult(ApiResult.Success());
  }
}
