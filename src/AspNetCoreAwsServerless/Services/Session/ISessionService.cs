using AspNetCoreAwsServerless.Dtos.Session;
using AspNetCoreAwsServerless.Utils.Result;

namespace AspNetCoreAwsServerless.Services.Session;

public interface ISessionService
{
  Task<ApiResult<SessionTokens>> Login(LoginDto code);
}
