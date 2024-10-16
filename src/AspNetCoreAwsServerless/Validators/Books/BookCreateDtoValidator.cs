using AspNetCoreAwsServerless.Dtos.Books;
using FluentValidation;

namespace AspNetCoreAwsServerless.Validators.Books;

public class BookCreateDtoValidator : AbstractValidator<BookCreateDto>
{
  public BookCreateDtoValidator()
  {
    RuleFor(v => v.Title).NotNull().MinimumLength(1);
    RuleFor(v => v.Author).NotNull().MinimumLength(1);
    RuleFor(v => v.Pages).NotNull().GreaterThan(0);
  }
}
