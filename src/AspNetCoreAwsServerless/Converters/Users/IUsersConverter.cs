using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreAwsServerless.Dtos.CognitoUser;
using AspNetCoreAwsServerless.Dtos.Users;
using AspNetCoreAwsServerless.Entities.Users;

namespace AspNetCoreAwsServerless.Converters.Users;

public interface IUsersConverter
{
  public UserDto ToDto(User user);

  public User FromCognitoUser(CognitoUser cognitoUser);
}
