using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreAwsServerless.Dtos.Books;
using AspNetCoreAwsServerless.Extensions.IRuleBuilder;
using FluentValidation;

namespace AspNetCoreAwsServerless.Validators.Books;

public class BookPatchDtoValidator : AbstractValidator<BookPatchDto>
{
  public BookPatchDtoValidator()
  {
    RuleFor(v => v.Title).MinimumLength(1);
    RuleFor(v => v.Author).MinimumLength(1);
    RuleFor(v => v.Pages).GreaterThan(0);
  }
}
