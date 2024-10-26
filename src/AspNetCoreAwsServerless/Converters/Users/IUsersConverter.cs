using AspNetCoreAwsServerless.Dtos.Users;
using AspNetCoreAwsServerless.Entities.Users;
using AspNetCoreAwsServerless.Utils.Result;

namespace AspNetCoreAwsServerless.Converters.Users;

public interface IUsersConverter
{
  public UserDto ToDto(User user);
  public ApiResult<User> FromEntityAndPatchDto(User user, UserPatchDto dto);
}
