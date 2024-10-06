using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreAwsServerless.Dtos.Cognito;
using AspNetCoreAwsServerless.Dtos.Session;
using AspNetCoreAwsServerless.Dtos.Users;
using AspNetCoreAwsServerless.Entities.Users;
using AspNetCoreAwsServerless.Utils.Result;

namespace AspNetCoreAwsServerless.Services.Cognito;

public interface ICognitoService
{
  public Task<ApiResult<CognitoUser>> GetUser(string accessToken);

  public Task<ApiResult<CognitoTokens>> GetTokens(LoginDto loginDto);
}
