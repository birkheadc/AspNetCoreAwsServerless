using AspNetCoreAwsServerless.Converters.Users;
using AspNetCoreAwsServerless.Dtos.Users;
using AspNetCoreAwsServerless.Entities.Users;
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
    User user = new()
    {
      Id = id,
      EmailAddress = "email@address.com",
      DisplayName = "Display Name"
    };

    UserDto expectedDto = new()
    {
      Id = id.ToString(),
      EmailAddress = "email@address.com",
      DisplayName = "Display Name"
    };

    UserDto actualDto = _converter.ToDto(user);
    actualDto.Should().BeEquivalentTo(expectedDto);
  }
}
