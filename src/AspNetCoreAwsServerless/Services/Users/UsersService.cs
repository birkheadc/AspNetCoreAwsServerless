using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreAwsServerless.Dtos.Users;
using AspNetCoreAwsServerless.Entities.Users;
using AspNetCoreAwsServerless.Utils.Id;
using AspNetCoreAwsServerless.Utils.Result;

namespace AspNetCoreAwsServerless.Services.Users;

public class UsersService : IUsersService
{
  public Task<ApiResult<User>> Get(Id<User> id)
  {
    throw new NotImplementedException();
  }

  public Task<ApiResult<User>> Patch(Id<User> id, UserPatchDto dto)
  {
    throw new NotImplementedException();
  }

  public Task<ApiResult<User>> Put(UserPutDto dto)
  {
    throw new NotImplementedException();
  }
}
