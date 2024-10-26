using AspNetCoreAwsServerless.Converters.Users;
using AspNetCoreAwsServerless.Dtos.Users;
using AspNetCoreAwsServerless.Entities.Roles;
using AspNetCoreAwsServerless.Entities.Users;
using AspNetCoreAwsServerless.Utils.Result;
using FluentAssertions;
using Moq.AutoMock;

namespace AspNetCoreAwsServerless.Tests.Unit.Converters.Users;

public class UsersConverterTests
{
  private readonly AutoMocker _mocker;
  private readonly UsersConverter _converter;

  public UsersConverterTests()
  {
    _mocker = new AutoMocker();
    _converter = _mocker.CreateInstance<UsersConverter>();
  }

  [Fact]
  public void ToDto_ReturnsUserDto()
  {
    Guid id = Guid.NewGuid();
    User user =
      new()
      {
        Id = id,
        EmailAddress = "email@address.com",
        DisplayName = "Display Name",
        Roles = [UserRole.Admin]
      };

    UserDto expectedDto =
      new()
      {
        Id = id.ToString(),
        EmailAddress = "email@address.com",
        DisplayName = "Display Name",
        Roles = [UserRole.Admin]
      };

    UserDto actualDto = _converter.ToDto(user);
    actualDto.Should().BeEquivalentTo(expectedDto);
  }

  [Fact]
  public void FromEntityAndPatchDto_ReturnsUser_WithUpdatedData()
  {
    User oldUser =
      new()
      {
        Id = Guid.NewGuid(),
        EmailAddress = "email@address.com",
        DisplayName = "Display Name",
        Roles = [UserRole.Admin]
      };

    UserPatchDto dto =
      new() { EmailAddress = "new.email@address.com", DisplayName = "New Display Name", };

    User expected =
      new()
      {
        Id = oldUser.Id,
        EmailAddress = dto.EmailAddress,
        DisplayName = dto.DisplayName,
        Roles = oldUser.Roles
      };

    ApiResult<User> result = _converter.FromEntityAndPatchDto(oldUser, dto);
    result.Should().HaveValue(expected);
  }
}
