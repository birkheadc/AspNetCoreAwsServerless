using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreAwsServerless.Utils.Id;
using FluentValidation;

namespace AspNetCoreAwsServerless.Extensions.IRuleBuilder;

public static class IRuleBuilderExtensions
{
  public static IRuleBuilderOptions<T, string> IsValidGuid<T>(
    this IRuleBuilder<T, string> ruleBuilder
  )
  {
    return ruleBuilder
      .Must(id => Guid.TryParse(id, out _))
      .WithMessage("Must be a valid GUID.")
      .WithErrorCode("ValidGuidValidator");
  }
}
