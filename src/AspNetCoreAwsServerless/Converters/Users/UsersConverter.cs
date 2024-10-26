using AspNetCoreAwsServerless.Dtos.Users;
using AspNetCoreAwsServerless.Entities.Roles;
using AspNetCoreAwsServerless.Entities.Users;
using AspNetCoreAwsServerless.Utils.Result;

namespace AspNetCoreAwsServerless.Converters.Users;

public class UsersConverter : IUsersConverter
{
  public ApiResult<User> FromEntityAndPatchDto(User user, UserPatchDto dto)
  {
    try
    {
      User newUser =
        new()
        {
          Id = user.Id,
          EmailAddress = dto.EmailAddress ?? user.EmailAddress,
          DisplayName = dto.DisplayName ?? user.DisplayName,
          Roles = user.Roles
        };

      return newUser;
    }
    catch
    {
      return ApiResult<User>.InternalServerError;
    }
  }

  public UserDto ToDto(User user)
  {
    return new UserDto()
    {
      Id = user.Id.ToString(),
      EmailAddress = user.EmailAddress,
      DisplayName = user.DisplayName,
      Roles = user.Roles
    };
  }
}
