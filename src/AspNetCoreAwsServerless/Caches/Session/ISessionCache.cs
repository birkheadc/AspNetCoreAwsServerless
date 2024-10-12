using AspNetCoreAwsServerless.Entities.Users;
using AspNetCoreAwsServerless.Utils.Id;
using AspNetCoreAwsServerless.Utils.Result;

namespace AspNetCoreAwsServerless.Caches.Session;

public interface ISessionCache
{
  Task<ApiResult<string>> GetAccessToken(Id<User> userId);

  Task<ApiResult> SetAccessToken(Id<User> userId, string accessToken);

  Task<ApiResult<Id<User>>> GetUserId(string accessToken);
}
