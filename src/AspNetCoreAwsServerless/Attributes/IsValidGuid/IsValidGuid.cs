using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreAwsServerless.Attributes.IsValidGuid;

public class IsValidGuid : ValidationAttribute
{
  // public override bool IsValid(object? value)
  // {
  //   return Guid.TryParse(value?.ToString(), out _);
  // }

  protected override ValidationResult? IsValid(object? value, ValidationContext context)
  {
    return Guid.TryParse(value?.ToString(), out _)
      ? ValidationResult.Success
      : new ValidationResult("Must be a valid GUID.");
  }
}
