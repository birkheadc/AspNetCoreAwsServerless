using AspNetCoreAwsServerless.Dtos.Books;
using AspNetCoreAwsServerless.Extensions.IRuleBuilderExtensions;
using FluentValidation;

namespace AspNetCoreAwsServerless.Validators.Books;

public class BookPutDtoValidator : AbstractValidator<BookPutDto>
{
  public BookPutDtoValidator()
  {
    RuleFor(v => v.Id).NotNull().IsValidGuid();
    RuleFor(v => v.Title).NotNull().MinimumLength(1);
    RuleFor(v => v.Author).NotNull().MinimumLength(1);
    RuleFor(v => v.Pages).NotNull().GreaterThan(0);
  }
}
