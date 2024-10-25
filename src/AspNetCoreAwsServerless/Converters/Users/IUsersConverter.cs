using AspNetCoreAwsServerless.Dtos.Users;
using AspNetCoreAwsServerless.Entities.Users;

namespace AspNetCoreAwsServerless.Converters.Users;

public interface IUsersConverter
{
  public UserDto ToDto(User user);
}
