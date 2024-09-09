using AspNetCoreAwsServerless.Entities.Users;
using AspNetCoreAwsServerless.Utils.Id;
using AspNetCoreAwsServerless.Utils.Result;

namespace AspNetCoreAwsServerless.Repositories.Users;

public interface IUsersRepository
{
  Task<ApiResult<User>> Get(Id<User> id);
  Task<ApiResult<IEnumerable<User>>> GetAll();
  Task<ApiResult<User>> Put(User user);
  Task<ApiResult> Delete(Id<User> id);
}
