using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreAwsServerless.Dtos.CognitoUser;

public class CognitoUser
{
  public required string Username { get; init; }
  public required string EmailAddress { get; init; }
}
