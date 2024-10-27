using AspNetCoreAwsServerless.Dtos.Users;
using AspNetCoreAwsServerless.Entities.Users;

namespace AspNetCoreAwsServerless.Converters.Users;

public interface IUsersConverter
{
  public UserDto ToDto(User user);
  public UserProfileDto ToDto(UserProfile profile);
  public UserRolesDto ToDto(UserRoles roles);
  public UserRoles ToEntity(UserRolesDto dto);
}
