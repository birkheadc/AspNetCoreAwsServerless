using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreAwsServerless.Utils.Id;

namespace AspNetCoreAwsServerless.Dtos.Users;

public class UserDto
{
  public required string Id { get; init; }
  public required string EmailAddress { get; init; }
  public string? DisplayName { get; init; }
}