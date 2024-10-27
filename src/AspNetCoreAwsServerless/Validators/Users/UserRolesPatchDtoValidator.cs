using AspNetCoreAwsServerless.Dtos.Users;
using AspNetCoreAwsServerless.Entities.Roles;
using FluentValidation;

namespace AspNetCoreAwsServerless.Validators.Users;

public class UserRolesPatchDtoValidator : AbstractValidator<UserRolesDto>
{
  public UserRolesPatchDtoValidator()
  {
    RuleForEach(v => v.Roles)
      .IsInEnum()
      .WithMessage("That role does not exist.")
      .NotEqual(UserRole.SuperAdmin)
      .WithMessage("A user cannot be granted Super Admin role through the API.");
  }
}
