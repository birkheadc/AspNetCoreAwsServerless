using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreAwsServerless.Dtos.Example;

public record ExampleDto
{
  public required string DisplayName { get; init; }

  public required string Password { get; init; }

  public required string SecretCode { get; init; }
}
