using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
      DisplayName = user.DisplayName,
    };
  }
}
