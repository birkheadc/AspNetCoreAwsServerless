using AspNetCoreAwsServerless.Dtos.Users;
using AspNetCoreAwsServerless.Entities.Users;

namespace AspNetCoreAwsServerless.Converters.Users;

public class UsersConverter : IUsersConverter
{
  public UserDto ToDto(User user)
  {
    return new UserDto()
    {
      Id = user.Id.ToString(),
      EmailAddress = user.EmailAddress,
      Profile = ToDto(user.Profile),
      Roles = ToDto(user.Roles),
    };
  }

  public UserProfileDto ToDto(UserProfile profile)
  {
    return new() { DisplayName = profile.DisplayName };
  }

  public UserRolesDto ToDto(UserRoles roles)
  {
    return new() { Roles = roles.Roles };
  }

  public UserRoles ToEntity(UserRolesDto dto)
  {
    return new() { Roles = dto.Roles };
  }

  public UserProfile ToEntity(UserProfileDto dto)
  {
    return new() { DisplayName = dto.DisplayName };
  }
}
