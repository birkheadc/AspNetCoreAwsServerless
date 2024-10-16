using AspNetCoreAwsServerless.Dtos.Books;
using FluentValidation;

namespace AspNetCoreAwsServerless.Validators.Books;

public class BookCreateManyDtoValidator : AbstractValidator<BookCreateManyDto>
{
  public BookCreateManyDtoValidator()
  {
    RuleForEach(v => v.Books).SetValidator(new BookCreateDtoValidator());
  }
}
