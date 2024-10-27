using AspNetCoreAwsServerless.Dtos.Users;
using FluentValidation;

namespace AspNetCoreAwsServerless.Validators.Users;

public class UserProfileDtoValidator : AbstractValidator<UserProfileDto>
{
  public UserProfileDtoValidator()
  {
    RuleFor(v => v.DisplayName).MinimumLength(4).MaximumLength(32);
  }
}
