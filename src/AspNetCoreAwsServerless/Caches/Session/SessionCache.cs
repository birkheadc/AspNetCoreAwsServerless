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

  public Task<ApiResult<Id<User>>> GetUserId(string accessToken)
  {
    _logger.LogInformation("Getting user ID from cache via access token.");
    string? userId = _userAccessTokens.FirstOrDefault(u => u.Value == accessToken).Key;
    if (userId is null)
    {
      _logger.LogWarning("User ID not found for access token.");
      return Task.FromResult(ApiResult<Id<User>>.Failure(ApiResultErrors.NotFound));
    }

    _logger.LogInformation("User ID found for access token. User ID: {UserId}", userId);
    return Task.FromResult(ApiResult<Id<User>>.Success(new Id<User>(userId)));
  }
}
