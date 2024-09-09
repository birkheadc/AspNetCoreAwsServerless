using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreAwsServerless.Dtos.Example;
using FluentValidation;

namespace AspNetCoreAwsServerless.Validators.Example;

public class ExampleDtoValidator : AbstractValidator<ExampleDto>
{
  public ExampleDtoValidator()
  {
    RuleFor(v => v.DisplayName).MinimumLength(4).MaximumLength(16);
    RuleFor(v => v.DisplayName).NotEqual("colby");
    RuleFor(v => v.Password).MinimumLength(8).MaximumLength(32);
    RuleFor(v => v.SecretCode).Equal("1234");
  }
}
